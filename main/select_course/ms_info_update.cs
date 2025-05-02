using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace 教务管理系统
{
    public partial class ms_info_update : Form
    {
        public delegate void DataUpdateEventHandler(object sender, EventArgs e);
        public event DataUpdateEventHandler DataUpdate;

        private DatabaseHelper dbHelper;

        public ms_info_update()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["MyDatabase"].ConnectionString;
            dbHelper = new DatabaseHelper(connectionString);
        }

        private void ms_info_add_Load(object sender, EventArgs e)
        {
            LoadData();
            dataGridView1.ClearSelection();
            // 避免窗体加载时自动获取焦点
            var dummyPanel = new Panel();
            dummyPanel.Visible = false;
            this.Controls.Add(dummyPanel);
            this.ActiveControl = dummyPanel;
        }

        private void LoadData()
        {
            try
            {
                // 使用全局变量获取当前登录的用户名
                string userNumber = GlobalVariables.CurrentUserNumber;

                string courseSql = @"
                    SELECT 
                        ci.course_number,
                        ci.course_name,
                        ci.course_description,
                        ci.course_credit,
                        ci.course_department,
                        (SELECT ti.teacher_name FROM teacher_information ti WHERE ti.teacher_number = ci.course_teacher_number) AS teacher_name,
                        (SELECT ti.teacher_number FROM teacher_information ti WHERE ti.teacher_number = ci.course_teacher_number) AS teacher_number
                    FROM
                        course_information ci";

                DataTable courseTable = dbHelper.ExecuteQuery(courseSql);

                string selectionSql = @"
                    SELECT 
                        course_number
                    FROM 
                        course_selection
                    WHERE 
                        student_number = @userNumber AND course_status = ""已选""";

                MySqlParameter[] selectionParams = new MySqlParameter[]
                {
                    new MySqlParameter("@userNumber", MySqlDbType.VarChar) { Value = userNumber }
                };

                DataTable selectionTable = dbHelper.ExecuteQuery(selectionSql, selectionParams);

                // 获取已选课程的Number列表
                List<string> selectedCourseNumbers = new List<string>();
                if (selectionTable != null)
                {
                    foreach (DataRow row in selectionTable.Rows)
                    {
                        if (row["course_number"] != DBNull.Value)
                        {
                            selectedCourseNumbers.Add(row["course_number"].ToString());
                        }
                    }
                }

                if (courseTable != null)
                {
                    dataGridView1.DataSource = courseTable;

                    // 设置复选框状态
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.Cells["course_number"].Value != null && !row.Cells["course_number"].Value.Equals(DBNull.Value))
                        {
                            string courseNumber = row.Cells["course_number"].Value.ToString();
                            row.Cells["Selected"].Value = selectedCourseNumbers.Contains(courseNumber);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("无法加载课程信息，请检查数据库连接。");
                }
                if (dataGridView1.Columns["teacher_number"] != null)
                {
                    dataGridView1.Columns["teacher_number"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 弹出确认对话框
            DialogResult result = MessageBox.Show(
                "您确定要提交选课更改吗？",
                "确认操作",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                // 用户点击“确定”，执行选课操作
                try
                {
                    string userNumber = GlobalVariables.CurrentUserNumber;

                    // 获取当前所有选课记录
                    string existingSelectionSql = @"
                        SELECT 
                            course_number
                        FROM 
                            course_selection
                        WHERE 
                            student_number = @userNumber AND course_status = ""已选""";

                    MySqlParameter[] existingParams = new MySqlParameter[]
                    {
                        new MySqlParameter("@userNumber", MySqlDbType.VarChar) { Value = userNumber }
                    };

                    DataTable existingSelectionTable = dbHelper.ExecuteQuery(existingSelectionSql, existingParams);

                    List<string> currentSelectedCourseNumbers = new List<string>();
                    List<string> currentSelectedTeacherNumber = new List<string>();

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.Cells["Selected"].Value != null && (bool)row.Cells["Selected"].Value)
                        {
                            if (row.Cells["course_number"].Value != null && !row.Cells["course_number"].Value.Equals(DBNull.Value))
                            {
                                string courseNumber = row.Cells["course_number"].Value.ToString();
                                string teacherNumber = row.Cells["teacher_number"].Value.ToString();

                                currentSelectedCourseNumbers.Add(courseNumber);
                                currentSelectedTeacherNumber.Add(teacherNumber);
                            }
                        }
                    }

                    // 插入新的选课记录
                    for (int i = 0; i < currentSelectedCourseNumbers.Count; i++)
                    {
                        string courseNumber = currentSelectedCourseNumbers[i];
                        string teacherNumber = currentSelectedTeacherNumber[i];

                        // 检查是否已经存在该选课记录
                        string checkSql = @"SELECT COUNT(*) FROM course_selection WHERE student_number = @userNumber AND course_number = @courseNumber AND (course_status = ""已选"" OR course_status = ""退选"")";
                        MySqlParameter[] checkParams = new MySqlParameter[]
                        {
                            new MySqlParameter("@userNumber", MySqlDbType.VarChar) { Value = userNumber },
                            new MySqlParameter("@courseNumber", MySqlDbType.VarChar) { Value = courseNumber }
                        };

                        object resultCheck = dbHelper.ExecuteScalar(checkSql, checkParams);
                        int count = Convert.ToInt32(resultCheck ?? 0);

                        if (count == 0)
                        {
                            // 插入选课记录
                            string insertSql = @"
                                INSERT INTO course_selection (
                                    student_number, course_number, teacher_number, course_status
                                ) VALUES (
                                    @userNumber, @courseNumber, @teacherNumber, ""已选""
                                )";

                            MySqlParameter[] insertParams = new MySqlParameter[]
                            {
                                new MySqlParameter("@userNumber", MySqlDbType.VarChar) { Value = userNumber },
                                new MySqlParameter("@courseNumber", MySqlDbType.VarChar) { Value = courseNumber },
                                new MySqlParameter("@teacherNumber", MySqlDbType.VarChar) { Value = teacherNumber }
                            };

                            dbHelper.ExecuteNonQuery(insertSql, insertParams);
                            DataUpdate?.Invoke(this, EventArgs.Empty);
                        }
                        else
                        {
                            string updateSql = @"
                                UPDATE course_selection
                                SET course_status = ""已选""
                                WHERE
                                  student_number = @userNumber
                                  AND course_number = @courseNumber
                                  AND course_status = ""退选""";

                            MySqlParameter[] updateParams = new MySqlParameter[]
                            {
                                new MySqlParameter("@userNumber", MySqlDbType.VarChar) { Value = userNumber },
                                new MySqlParameter("@courseNumber", MySqlDbType.VarChar) { Value = courseNumber }
                            };

                            dbHelper.ExecuteNonQuery(updateSql, updateParams);
                            DataUpdate?.Invoke(this, EventArgs.Empty);
                        }
                    }

                    // 更新课程
                    if (existingSelectionTable != null)
                    {
                        foreach (DataRow row in existingSelectionTable.Rows)
                        {
                            if (row["course_number"] != DBNull.Value)
                            {
                                string courseNumber = Convert.ToString(row["course_number"]);
                                if (!currentSelectedCourseNumbers.Contains(courseNumber))
                                {
                                    string deleteSql = @"
                                        UPDATE course_selection
                                        SET course_status = ""退选""
                                        WHERE
                                          student_number = @userNumber
                                          AND course_number = @courseNumber";

                                    MySqlParameter[] deleteParams = new MySqlParameter[]
                                    {
                                        new MySqlParameter("@userNumber", MySqlDbType.VarChar) { Value = userNumber },
                                        new MySqlParameter("@courseNumber", MySqlDbType.VarChar) { Value = courseNumber }
                                    };

                                    dbHelper.ExecuteNonQuery(deleteSql, deleteParams);
                                    DataUpdate?.Invoke(this, EventArgs.Empty);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            else
            {
                // 用户点击“取消”，还原初始状态
                LoadData(); // 重新加载数据，恢复到初始状态
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
