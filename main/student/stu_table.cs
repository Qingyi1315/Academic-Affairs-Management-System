using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace 教务管理系统
{
    public partial class stu_table : Form
    {
        private DatabaseHelper dbHelper;

        public stu_table()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["MyDatabase"].ConnectionString;
            dbHelper = new DatabaseHelper(connectionString);
        }

        private void stu_table_Load(object sender, EventArgs e)
        {
            LoadData();
            dataGridView1.ClearSelection();
            dataGridView1.MouseClick += new MouseEventHandler(dataGridView1_MouseClick);
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
                    *
                FROM
                    student_information";

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
            dataGridView1.Columns["id"].Visible = false;
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // 获取密码列的索引
            int passwordColumnIndex = -1;
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                if (column.Name == "student_password")
                {
                    passwordColumnIndex = column.Index;
                    break;
                }
            }

            // 检查是否是密码列
            if (e.ColumnIndex == passwordColumnIndex && e.RowIndex >= 0)
            {
                // 检查单元格的值是否为空
                if (e.Value != null)
                {
                    // 将密码显示为星号
                    e.Value = new string('*', e.Value.ToString().Length);
                    e.FormattingApplied = true;
                }
            }
        }

        private void Stu_Info_Add_DataInserted(object sender, EventArgs e)
        {
            // 重新加载数据以更新 DataGridView
            LoadData();
        }

        private void Stu_Info_Alt_DataSaved(object sender, EventArgs e)
        {
            // 重新加载数据以更新 DataGridView
            LoadData();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            stu_info_add stu_Info_Add = new stu_info_add();
            stu_Info_Add.DataInserted += Stu_Info_Add_DataInserted;
            stu_Info_Add.Show();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                // 检查是否选中了行
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                    string studentNum = selectedRow.Cells["student_number"].Value.ToString();

                    // 构建 SQL 删除语句
                    string sql = "DELETE FROM student_information WHERE student_number = @studentNum";

                    // 创建参数
                    MySqlParameter[] parameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@studentNum", MySqlDbType.VarChar) { Value = studentNum }
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
                    string studentNumber = selectedRow.Cells["student_number"].Value.ToString();
                    string studentName = selectedRow.Cells["student_name"].Value.ToString();
                    string studentPassword = selectedRow.Cells["student_password"].Value.ToString();
                    string studentGender = selectedRow.Cells["student_gender"].Value.ToString();
                    string studentClass = selectedRow.Cells["student_class"].Value.ToString();
                    string studentMajor = selectedRow.Cells["student_major"].Value.ToString();
                    string studentDepartment = selectedRow.Cells["student_department"].Value.ToString();
                    string studentBirthday = selectedRow.Cells["student_birthday"].Value.ToString();
                    string studentOrigin = selectedRow.Cells["student_origin"].Value.ToString();
                    string studentAddress = selectedRow.Cells["student_address"].Value.ToString();
                    string studentPhone = selectedRow.Cells["student_phone"].Value.ToString();
                    string studentEmail = selectedRow.Cells["student_email"].Value.ToString();
                    string studentEnrollmentDate = selectedRow.Cells["student_enrollment_date"].Value.ToString();
                    string studentGraduationDate = selectedRow.Cells["student_graduation_date"].Value.ToString();

                    // 打开修改窗体并传递数据
                    stu_info_alt stu_Info_Alt = new stu_info_alt();
                    stu_Info_Alt.StudentNumber = studentNumber;
                    stu_Info_Alt.StudentName = studentName;
                    stu_Info_Alt.StudentPassword = studentPassword;
                    stu_Info_Alt.StudentGender = studentGender;
                    stu_Info_Alt.StudentClass = studentClass;
                    stu_Info_Alt.StudentMajor = studentMajor;
                    stu_Info_Alt.StudentDepartment = studentDepartment;
                    stu_Info_Alt.StudentBirthday = studentBirthday;
                    stu_Info_Alt.StudentOrigin = studentOrigin;
                    stu_Info_Alt.StudentAddress = studentAddress;
                    stu_Info_Alt.StudentPhone = studentPhone;
                    stu_Info_Alt.StudentEmail = studentEmail;
                    stu_Info_Alt.StudentEnrollmentDate = studentEnrollmentDate;
                    stu_Info_Alt.StudentGraduationDate = studentGraduationDate;
                    stu_Info_Alt.DataSaved += Stu_Info_Alt_DataSaved;
                    stu_Info_Alt.Show();
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
                    string studentNumber = selectedRow.Cells["student_number"].Value.ToString();

                    // 构建 SQL 删除语句
                    string sql = "DELETE FROM student_information WHERE student_number = @studentNumber";

                    // 创建参数
                    MySqlParameter[] parameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@studentNumber", MySqlDbType.VarChar) { Value = studentNumber }
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
                    string studentPassword = selectedRow.Cells["student_password"].Value.ToString();
                    string studentGender = selectedRow.Cells["student_gender"].Value.ToString();
                    string studentClass = selectedRow.Cells["student_class"].Value.ToString();
                    string studentMajor = selectedRow.Cells["student_major"].Value.ToString();
                    string studentDepartment = selectedRow.Cells["student_department"].Value.ToString();
                    string studentBirthday = selectedRow.Cells["student_birthday"].Value.ToString();
                    string studentOrigin = selectedRow.Cells["student_origin"].Value.ToString();
                    string studentAddress = selectedRow.Cells["student_address"].Value.ToString();
                    string studentPhone = selectedRow.Cells["student_phone"].Value.ToString();
                    string studentEmail = selectedRow.Cells["student_email"].Value.ToString();
                    string studentEnrollmentDate = selectedRow.Cells["student_enrollment_date"].Value.ToString();
                    string studentGraduationDate = selectedRow.Cells["student_graduation_date"].Value.ToString();

                    // 打开修改窗体并传递数据
                    stu_info_alt stu_Info_Alt = new stu_info_alt();
                    stu_Info_Alt.StudentNumber = studentNumber;
                    stu_Info_Alt.StudentName = studentName;
                    stu_Info_Alt.StudentPassword = studentPassword;
                    stu_Info_Alt.StudentGender = studentGender;
                    stu_Info_Alt.StudentClass = studentClass;
                    stu_Info_Alt.StudentMajor = studentMajor;
                    stu_Info_Alt.StudentDepartment = studentDepartment;
                    stu_Info_Alt.StudentBirthday = studentBirthday;
                    stu_Info_Alt.StudentOrigin = studentOrigin;
                    stu_Info_Alt.StudentAddress = studentAddress;
                    stu_Info_Alt.StudentPhone = studentPhone;
                    stu_Info_Alt.StudentEmail = studentEmail;
                    stu_Info_Alt.StudentEnrollmentDate = studentEnrollmentDate;
                    stu_Info_Alt.StudentGraduationDate = studentGraduationDate;
                    stu_Info_Alt.DataSaved += Stu_Info_Alt_DataSaved;
                    stu_Info_Alt.Show();
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
