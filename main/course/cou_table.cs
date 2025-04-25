using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace 教务管理系统
{
    public partial class cou_table : Form
    {
        private DatabaseHelper dbHelper;

        public cou_table()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["MyDatabase"].ConnectionString;
            dbHelper = new DatabaseHelper(connectionString);
        }

        private void cuo_info_Load(object sender, EventArgs e)
        {
            LoadData();
            dataGridView1.ClearSelection();
            if (GlobalVariables.CurrentRole == "教师")
            {
                toolStrip1.Visible = false;
            }
            else
            {
                dataGridView1.MouseClick += new MouseEventHandler(dataGridView1_MouseClick);
            }
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
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

        private void LoadData()
        {
            try
            {
                string sql = @"
            SELECT
                  ci.course_number,
                  ci.course_name,
                  ci.course_description,
                  ci.course_credit,
                  ti.teacher_name,
                  ci.course_department
            FROM
                  course_information ci
                  LEFT JOIN teacher_information ti ON ci.course_teacher_number = ti.teacher_number";

                DataTable dataTable = dbHelper.ExecuteQuery(sql);

                if (dataTable != null)
                {
                    dataGridView1.DataSource = dataTable;
                    dataGridView1.ClearSelection();
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

        private void Cuo_Info_Add_DataInserted(object sender, EventArgs e)
        {
            // 重新加载数据以更新 DataGridView
            LoadData();
        }

        private void Cuo_Info_Alt_DataSaved(object sender, EventArgs e)
        {
            // 重新加载数据以更新 DataGridView
            LoadData();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            cou_info_add cuo_Info_Add = new cou_info_add();
            cuo_Info_Add.DataInserted += Cuo_Info_Add_DataInserted;
            cuo_Info_Add.Show();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                // 检查是否选中了行
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                    string courseNumber = selectedRow.Cells["course_number"].Value.ToString();

                    // 构建 SQL 删除语句
                    string sql = "DELETE FROM course_information WHERE course_number = @courseNumber";

                    // 创建参数
                    MySqlParameter[] parameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@courseNumber", MySqlDbType.VarChar) { Value = courseNumber }
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

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            try
            {
                // 检查是否选中了行
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                    string courseNumber = selectedRow.Cells["course_number"].Value.ToString();
                    string courseName = selectedRow.Cells["course_name"].Value.ToString();
                    string courseDescription = selectedRow.Cells["course_description"].Value.ToString();
                    decimal courseCredit = decimal.Parse(selectedRow.Cells["course_credit"].Value.ToString());
                    string teacherName = selectedRow.Cells["teacher_name"].Value.ToString();
                    string courseDepartment = selectedRow.Cells["course_department"].Value.ToString();

                    // 打开修改窗体并传递数据
                    cou_info_alt cuo_Info_Alt = new cou_info_alt();
                    cuo_Info_Alt.CourseNumber = courseNumber;
                    cuo_Info_Alt.CourseName = courseName;
                    cuo_Info_Alt.CourseDescription = courseDescription;
                    cuo_Info_Alt.CourseCredit = courseCredit;
                    cuo_Info_Alt.TeacherName = teacherName;
                    cuo_Info_Alt.CourseDepartment = courseDepartment;
                    cuo_Info_Alt.DataSaved += Cuo_Info_Alt_DataSaved;
                    cuo_Info_Alt.Show();
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

        private void 添加信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cou_info_add cuo_Info_Add = new cou_info_add();
            cuo_Info_Add.DataInserted += Cuo_Info_Add_DataInserted;
            cuo_Info_Add.Show();
        }

        private void 删除信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // 检查是否选中了行
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                    string courseNumber = selectedRow.Cells["course_number"].Value.ToString();

                    // 构建 SQL 删除语句
                    string sql = "DELETE FROM course_information WHERE course_number = @courseNumber";

                    // 创建参数
                    MySqlParameter[] parameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@courseNumber", MySqlDbType.VarChar) { Value = courseNumber }
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

        private void 修改信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // 检查是否选中了行
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                    string courseNumber = selectedRow.Cells["course_number"].Value.ToString();
                    string courseName = selectedRow.Cells["course_name"].Value.ToString();
                    string courseDescription = selectedRow.Cells["course_description"].Value.ToString();
                    decimal courseCredit = decimal.Parse(selectedRow.Cells["course_credit"].Value.ToString());
                    string teacherName = selectedRow.Cells["teacher_name"].Value.ToString();
                    string courseDepartment = selectedRow.Cells["course_department"].Value.ToString();

                    // 打开修改窗体并传递数据
                    cou_info_alt cuo_Info_Alt = new cou_info_alt();
                    cuo_Info_Alt.CourseNumber = courseNumber;
                    cuo_Info_Alt.CourseName = courseName;
                    cuo_Info_Alt.CourseDescription = courseDescription;
                    cuo_Info_Alt.CourseCredit = courseCredit;
                    cuo_Info_Alt.TeacherName = teacherName;
                    cuo_Info_Alt.CourseDepartment = courseDepartment;
                    cuo_Info_Alt.DataSaved += Cuo_Info_Alt_DataSaved;
                    cuo_Info_Alt.Show();
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

        private void DeleteMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // 检查是否选中了行
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                    string courseNumber = selectedRow.Cells["course_number"].Value.ToString();

                    // 构建 SQL 删除语句
                    string sql = "DELETE FROM course_information WHERE course_number = @courseNumber";

                    // 创建参数
                    MySqlParameter[] parameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@courseNumber", MySqlDbType.VarChar) { Value = courseNumber }
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
                    string courseNumber = selectedRow.Cells["course_number"].Value.ToString();
                    string courseName = selectedRow.Cells["course_name"].Value.ToString();
                    string courseDescription = selectedRow.Cells["course_description"].Value.ToString();
                    decimal courseCredit = decimal.Parse(selectedRow.Cells["course_credit"].Value.ToString());
                    string teacherName = selectedRow.Cells["teacher_name"].Value.ToString();
                    string courseDepartment = selectedRow.Cells["course_department"].Value.ToString();

                    // 打开修改窗体并传递数据
                    cou_info_alt cuo_Info_Alt = new cou_info_alt();
                    cuo_Info_Alt.CourseNumber = courseNumber;
                    cuo_Info_Alt.CourseName = courseName;
                    cuo_Info_Alt.CourseDescription = courseDescription;
                    cuo_Info_Alt.CourseCredit = courseCredit;
                    cuo_Info_Alt.TeacherName = teacherName;
                    cuo_Info_Alt.CourseDepartment = courseDepartment;
                    cuo_Info_Alt.DataSaved += Cuo_Info_Alt_DataSaved;
                    cuo_Info_Alt.Show();
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

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            about about = new about();
            about.Show();
        }

        private void 学生成绩ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stu_gra stu_Gra = new stu_gra();
            stu_Gra.Show();
        }

        private void 修改个人信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string sql = @"
    SELECT
        *
    FROM
        teacher_information
    WHERE
        teacher_number = @userNumber";

            MySqlParameter[] parameters = new MySqlParameter[]
            {
    new MySqlParameter("@userNumber", MySqlDbType.VarChar) { Value = GlobalVariables.CurrentUser }
            };

            DataTable dataTable = dbHelper.ExecuteQuery(sql, parameters);

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];

                // 创建 tea_info_alt 窗体并传递数据
                tea_info_alt tea_Info_Alt = new tea_info_alt
                {
                    TeacherNumber = row["teacher_number"].ToString(),
                    TeacherName = row["teacher_name"].ToString(),
                    TeacherPassword = row["teacher_password"].ToString(),
                    TeacherGender = row["teacher_gender"].ToString(),
                    TeacherTitle = row["teacher_title"].ToString(),
                    TeacherDepartment = row["teacher_department"].ToString(),
                    TeacherBirthday = row["teacher_birthday"].ToString(),
                    TeacherPhone = row["teacher_phone"].ToString(),
                    TeacherEmail = row["teacher_email"].ToString(),
                    TeacherProfessionalField = row["teacher_professional_field"].ToString(),
                    TeacherEducationLevel = row["teacher_education_level"].ToString(),
                    TeacherWorkStartDate = row["teacher_work_start_date"].ToString()

                };

                tea_Info_Alt.Show();
            }
            else
            {
                MessageBox.Show("未找到教师信息！");
            }

        }

        private void 发送反馈ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // 定义收件人和标题
                string recipient = "1162801098@qq.com";
                string subject = "反馈意见";

                // 使用 mailto 协议构建 URL
                string mailto = $"mailto:{recipient}?subject={Uri.EscapeDataString(subject)}";

                // 打开默认邮件客户端
                System.Diagnostics.Process.Start(mailto);
            }
            catch (Exception ex)
            {
                // 如果发生错误，显示提示信息
                MessageBox.Show($"无法打开邮件客户端: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
