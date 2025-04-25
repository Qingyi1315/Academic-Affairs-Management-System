using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace 教务管理系统
{
    public partial class stu_gra : Form
    {
        private DatabaseHelper dbHelper;

        public stu_gra()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["MyDatabase"].ConnectionString;
            dbHelper = new DatabaseHelper(connectionString);
            // 订阅 DataBindingComplete 事件
            dataGridView1.DataBindingComplete += DataGridView1_DataBindingComplete;
        }

        private void DataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.DefaultCellStyle.BackColor = Color.White; // 设置默认颜色
            }
            dataGridView1.ClearSelection(); // 清除选择
        }

        private void stu_gra_Load(object sender, EventArgs e)
        {
            LoadData();
            if (GlobalVariables.CurrentRole == "学生")
            {
                toolStrip1.Visible = false;
            }
            else
            {
                dataGridView1.MouseClick += new MouseEventHandler(dataGridView1_TeaMouseClick);
            }
        }

        private void dataGridView1_TeaMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // 获取点击位置
                Point clickPosition = e.Location;

                // 检查点击位置是否在 DataGridView 内部
                DataGridView.HitTestInfo hitTestInfo = dataGridView1.HitTest(clickPosition.X, clickPosition.Y);
                if (hitTestInfo.Type == DataGridViewHitTestType.Cell)
                {
                    // 获取点击的行索引
                    int rowIndex = hitTestInfo.RowIndex;

                    // 选中点击的行
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[rowIndex].Selected = true;

                    // 显示上下文菜单
                    ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
                    ToolStripMenuItem deleteMenuItem = new ToolStripMenuItem("删除信息");
                    ToolStripMenuItem editMenuItem = new ToolStripMenuItem("修改信息");

                    // 添加菜单项到上下文菜单
                    contextMenuStrip.Items.Add(deleteMenuItem);
                    contextMenuStrip.Items.Add(editMenuItem);

                    // 为菜单项添加点击事件
                    deleteMenuItem.Click += DeleteMenuItem_Click;
                    editMenuItem.Click += EditMenuItem_Click;

                    // 显示上下文菜单
                    contextMenuStrip.Show(dataGridView1, clickPosition);
                }
            }
        }

        private void DeleteMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // 检查是否选中了行
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                    string studentNumber = selectedRow.Cells["student_number"].Value.ToString();
                    string courseName = selectedRow.Cells["course_name"].Value.ToString();
                    string semester = selectedRow.Cells["semester"].Value.ToString();
                    string examType = selectedRow.Cells["exam_type"].Value.ToString();

                    // 构建 SQL 删除语句
                    string sql = @"
                        DELETE FROM student_grades WHERE student_number = @studentNumber
                        AND course_number = (SELECT course_number FROM course_information WHERE course_name = @courseName)
                        AND semester = @semester
                        AND exam_type = @examType";

                    // 创建参数
                    MySqlParameter[] parameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@studentNumber", MySqlDbType.VarChar) { Value = studentNumber },
                        new MySqlParameter("@courseName", MySqlDbType.VarChar) { Value = courseName },
                        new MySqlParameter("@semester", MySqlDbType.VarChar) { Value = semester },
                        new MySqlParameter("@examType", MySqlDbType.VarChar) { Value = examType },
                    };

                    // 执行删除操作
                    int rowsAffected = dbHelper.ExecuteNonQuery(sql, parameters);

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("数据删除成功！");
                        // 重新加载数据以更新 DataGridView
                        LoadData();
                        dataGridView1.ClearSelection();
                    }
                    else
                    {
                        MessageBox.Show("数据删除失败！");
                    }
                }
                else
                {
                    MessageBox.Show("请先选中一行数据。");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误: " + ex.Message);
            }
        }

        private void EditMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // 检查是否选中了行
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                    string studentNumber = selectedRow.Cells["student_number"].Value.ToString();
                    string studentName = selectedRow.Cells["student_name"].Value.ToString();
                    string courseName = selectedRow.Cells["course_name"].Value.ToString();
                    string semester = selectedRow.Cells["semester"].Value.ToString();
                    string examType = selectedRow.Cells["exam_type"].Value.ToString();
                    decimal score = decimal.Parse(selectedRow.Cells["score"].Value.ToString());
                    decimal gradePoint = decimal.Parse(selectedRow.Cells["grade_point"].Value.ToString());
                    decimal credit = decimal.Parse(selectedRow.Cells["credit"].Value.ToString());
                    string teacherName = selectedRow.Cells["teacher_name"].Value.ToString();

                    // 打开修改窗体并传递数据
                    stu_gra_alt stu_Gra_Alt = new stu_gra_alt();
                    stu_Gra_Alt.StudentNumber = studentNumber;
                    stu_Gra_Alt.StudentName = studentName;
                    stu_Gra_Alt.CourseName = courseName;
                    stu_Gra_Alt.Semester = semester;
                    stu_Gra_Alt.ExamType = examType;
                    stu_Gra_Alt.Score = score;
                    stu_Gra_Alt.GradePoint = gradePoint;
                    stu_Gra_Alt.Credit = credit;
                    stu_Gra_Alt.TeacherName = teacherName;
                    stu_Gra_Alt.DataSaved += Stu_Gra_Alt_DataSaved;

                    stu_Gra_Alt.Show();
                }
                else
                {
                    MessageBox.Show("请先选中一行数据。");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误: " + ex.Message);
            }
        }

        private void LoadData()
        {
            try
            {
                string sql;
                MySqlParameter[] parameters;

                if (GlobalVariables.CurrentRole == "学生")
                {
                    // 学生角色：查询当前学生的成绩
                    string userNumber = GlobalVariables.CurrentUser;

                    sql = @"
                        SELECT
                          sg.student_number,
                          (SELECT si.student_name FROM student_information si WHERE si.student_number = sg.student_number) AS student_name,
                          (SELECT ci.course_name FROM course_information ci WHERE ci.course_number = sg.course_number) AS course_name,
                          sg.semester,
                          sg.exam_type,
                          sg.score,
                          sg.grade_point,
                          sg.credit,
                          (SELECT ti.teacher_name FROM teacher_information ti WHERE ti.teacher_number = sg.teacher_number) AS teacher_name
                        FROM
                          student_grades sg
                        WHERE
                          sg.student_number = @userNumber";

                    parameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@userNumber", MySqlDbType.VarChar) { Value = userNumber }
                    };
                }
                else if (GlobalVariables.CurrentRole == "教师" || GlobalVariables.CurrentRole == "管理员")
                {
                    // 教师和管理员角色：查询所有学生的成绩
                    sql = @"
                        SELECT
                          sg.student_number,
                          (SELECT si.student_name FROM student_information si WHERE si.student_number = sg.student_number) AS student_name,
                          (SELECT ci.course_name FROM course_information ci WHERE ci.course_number = sg.course_number) AS course_name,
                          sg.semester,
                          sg.exam_type,
                          sg.score,
                          sg.grade_point,
                          sg.credit,
                          (SELECT ti.teacher_name FROM teacher_information ti WHERE ti.teacher_number = sg.teacher_number) AS teacher_name
                        FROM
                          student_grades sg";

                    parameters = new MySqlParameter[0]; // 教师和管理员查询不需要参数
                }
                else
                {
                    MessageBox.Show("未知角色，无法加载数据！");
                    return;
                }

                // 执行查询
                DataTable dataTable = dbHelper.ExecuteQuery(sql, parameters);

                if (dataTable != null)
                {
                    dataGridView1.AutoGenerateColumns = true; // 确保自动生成列
                    dataGridView1.DataSource = dataTable;
                }
                else
                {
                    MessageBox.Show("无法加载数据，请检查数据库连接。");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void Stu_Gra_Add_DataInserted(object sender, EventArgs e)
        {
            // 重新加载数据以更新 DataGridView
            LoadData();
        }

        private void Stu_Gra_Alt_DataSaved(object sender, EventArgs e)
        {
            // 重新加载数据以更新 DataGridView
            LoadData();
        }

        private void 添加信息toolStripButton1_Click(object sender, EventArgs e)
        {
            stu_gra_add stu_Gra_Add = new stu_gra_add();
            stu_Gra_Add.DataInserted += Stu_Gra_Add_DataInserted;
            stu_Gra_Add.Show();
        }

        private void 删除信息toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                // 检查是否选中了行
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                    string studentNumber = selectedRow.Cells["student_number"].Value.ToString();
                    string courseName = selectedRow.Cells["course_name"].Value.ToString();
                    string semester = selectedRow.Cells["semester"].Value.ToString();
                    string examType = selectedRow.Cells["exam_type"].Value.ToString();

                    // 构建 SQL 删除语句
                    string sql = @"
                        DELETE FROM student_grades WHERE student_number = @studentNumber
                        AND course_number = (SELECT course_number FROM course_information WHERE course_name = @courseName)
                        AND semester = @semester
                        AND exam_type = @examType";

                    // 创建参数
                    MySqlParameter[] parameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@studentNumber", MySqlDbType.VarChar) { Value = studentNumber },
                        new MySqlParameter("@courseName", MySqlDbType.VarChar) { Value = courseName },
                        new MySqlParameter("@semester", MySqlDbType.VarChar) { Value = semester },
                        new MySqlParameter("@examType", MySqlDbType.VarChar) { Value = examType },
                    };

                    // 执行删除操作
                    int rowsAffected = dbHelper.ExecuteNonQuery(sql, parameters);

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("数据删除成功！");
                        // 重新加载数据以更新 DataGridView
                        LoadData();
                        dataGridView1.ClearSelection();
                    }
                    else
                    {
                        MessageBox.Show("数据删除失败！");
                    }
                }
                else
                {
                    MessageBox.Show("请先选中一行数据。");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误: " + ex.Message);
            }

        }

        private void 修改信息toolStripButton3_Click(object sender, EventArgs e)
        {
            try
            {
                // 检查是否选中了行
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                    string studentNumber = selectedRow.Cells["student_number"].Value.ToString();
                    string studentName = selectedRow.Cells["student_name"].Value.ToString();
                    string courseName = selectedRow.Cells["course_name"].Value.ToString();
                    string semester = selectedRow.Cells["semester"].Value.ToString();
                    string examType = selectedRow.Cells["exam_type"].Value.ToString();
                    decimal score = decimal.Parse(selectedRow.Cells["score"].Value.ToString());
                    decimal gradePoint = decimal.Parse(selectedRow.Cells["grade_point"].Value.ToString());
                    decimal credit = decimal.Parse(selectedRow.Cells["credit"].Value.ToString());
                    string teacherName = selectedRow.Cells["teacher_name"].Value.ToString();

                    // 打开修改窗体并传递数据
                    stu_gra_alt stu_Gra_Alt = new stu_gra_alt();
                    stu_Gra_Alt.StudentNumber = studentNumber;
                    stu_Gra_Alt.StudentName = studentName;
                    stu_Gra_Alt.CourseName = courseName;
                    stu_Gra_Alt.Semester = semester;
                    stu_Gra_Alt.ExamType = examType;
                    stu_Gra_Alt.Score = score;
                    stu_Gra_Alt.GradePoint = gradePoint;
                    stu_Gra_Alt.Credit = credit;
                    stu_Gra_Alt.TeacherName = teacherName;
                    stu_Gra_Alt.DataSaved += Stu_Gra_Alt_DataSaved;

                    stu_Gra_Alt.Show();
                }
                else
                {
                    MessageBox.Show("请先选中一行数据。");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误: " + ex.Message);
            }

        }
    }
}
