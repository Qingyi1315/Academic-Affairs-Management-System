using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace 教务管理系统
{
    public partial class tea_table : Form
    {
        private DatabaseHelper dbHelper;

        public tea_table()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["MyDatabase"].ConnectionString;
            dbHelper = new DatabaseHelper(connectionString);
        }

        private void tea_table_Load(object sender, EventArgs e)
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
                  teacher_information";

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
                if (column.Name == "teacher_password")
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

        private void Tea_Info_Add_DataInserted(object sender, EventArgs e)
        {
            // 重新加载数据以更新 DataGridView
            LoadData();
        }

        private void Tea_Info_Alt_DataSaved(object sender, EventArgs e)
        {
            // 重新加载数据以更新 DataGridView
            LoadData();
        }

        private void 添加信息toolStripButton1_Click(object sender, EventArgs e)
        {
            tea_info_add tea_Info_Add = new tea_info_add();
            tea_Info_Add.DataInserted += Tea_Info_Add_DataInserted;
            tea_Info_Add.Show();
        }

        private void 删除信息toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                // 检查是否选中了行
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                    string teacherNumber = selectedRow.Cells["teacher_number"].Value.ToString();

                    // 构建 SQL 删除语句
                    string sql = "DELETE FROM teacher_information WHERE teacher_number = @teacherNumber";

                    // 创建参数
                    MySqlParameter[] parameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@teacherNumber", MySqlDbType.VarChar) { Value = teacherNumber }
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
                    string teacherNumber = selectedRow.Cells["teacher_number"].Value.ToString();
                    string teacherName = selectedRow.Cells["teacher_name"].Value.ToString();
                    string teacherPassword = selectedRow.Cells["teacher_password"].Value.ToString();
                    string teacherGender = selectedRow.Cells["teacher_gender"].Value.ToString();
                    string teacherTitle = selectedRow.Cells["teacher_title"].Value.ToString();
                    string teacherDepartment = selectedRow.Cells["teacher_department"].Value.ToString();
                    string teacherBirthday = selectedRow.Cells["teacher_birthday"].Value.ToString();
                    string teacherPhone = selectedRow.Cells["teacher_phone"].Value.ToString();
                    string teacherEmail = selectedRow.Cells["teacher_email"].Value.ToString();
                    string teacherProfessionalField = selectedRow.Cells["teacher_professional_field"].Value.ToString();
                    string teacherEducationLevel = selectedRow.Cells["teacher_education_level"].Value.ToString();
                    string teacherWorkStartDate = selectedRow.Cells["teacher_work_start_date"].Value.ToString();

                    // 打开修改窗体并传递数据
                    tea_info_alt tea_Info_Alt = new tea_info_alt();
                    tea_Info_Alt.TeacherNumber = teacherNumber;
                    tea_Info_Alt.TeacherName = teacherName;
                    tea_Info_Alt.TeacherPassword = teacherPassword;
                    tea_Info_Alt.TeacherGender = teacherGender;
                    tea_Info_Alt.TeacherTitle = teacherTitle;
                    tea_Info_Alt.TeacherDepartment = teacherDepartment;
                    tea_Info_Alt.TeacherBirthday = teacherBirthday;
                    tea_Info_Alt.TeacherPhone = teacherPhone;
                    tea_Info_Alt.TeacherEmail = teacherEmail;
                    tea_Info_Alt.TeacherProfessionalField = teacherProfessionalField;
                    tea_Info_Alt.TeacherEducationLevel = teacherEducationLevel;
                    tea_Info_Alt.TeacherWorkStartDate = teacherWorkStartDate;
                    tea_Info_Alt.DataSaved += Tea_Info_Alt_DataSaved;
                    tea_Info_Alt.Show();
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
                    string teacherNumber = selectedRow.Cells["teacher_number"].Value.ToString();

                    // 构建 SQL 删除语句
                    string sql = "DELETE FROM teacher_information WHERE teacher_number = @teacherNumber";

                    // 创建参数
                    MySqlParameter[] parameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@teacherNumber", MySqlDbType.VarChar) { Value = teacherNumber }
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
                    string teacherNumber = selectedRow.Cells["teacher_number"].Value.ToString();
                    string teacherName = selectedRow.Cells["teacher_name"].Value.ToString();
                    string teacherPassword = selectedRow.Cells["teacher_password"].Value.ToString();
                    string teacherGender = selectedRow.Cells["teacher_gender"].Value.ToString();
                    string teacherTitle = selectedRow.Cells["teacher_title"].Value.ToString();
                    string teacherDepartment = selectedRow.Cells["teacher_department"].Value.ToString();
                    string teacherBirthday = selectedRow.Cells["teacher_birthday"].Value.ToString();
                    string teacherPhone = selectedRow.Cells["teacher_phone"].Value.ToString();
                    string teacherEmail = selectedRow.Cells["teacher_email"].Value.ToString();
                    string teacherProfessionalField = selectedRow.Cells["teacher_professional_field"].Value.ToString();
                    string teacherEducationLevel = selectedRow.Cells["teacher_education_level"].Value.ToString();
                    string teacherWorkStartDate = selectedRow.Cells["teacher_work_start_date"].Value.ToString();

                    // 打开修改窗体并传递数据
                    tea_info_alt tea_Info_Alt = new tea_info_alt();
                    tea_Info_Alt.TeacherNumber = teacherNumber;
                    tea_Info_Alt.TeacherName = teacherName;
                    tea_Info_Alt.TeacherPassword = teacherPassword;
                    tea_Info_Alt.TeacherGender = teacherGender;
                    tea_Info_Alt.TeacherTitle = teacherTitle;
                    tea_Info_Alt.TeacherDepartment = teacherDepartment;
                    tea_Info_Alt.TeacherBirthday = teacherBirthday;
                    tea_Info_Alt.TeacherPhone = teacherPhone;
                    tea_Info_Alt.TeacherEmail = teacherEmail;
                    tea_Info_Alt.TeacherProfessionalField = teacherProfessionalField;
                    tea_Info_Alt.TeacherEducationLevel = teacherEducationLevel;
                    tea_Info_Alt.TeacherWorkStartDate = teacherWorkStartDate;
                    tea_Info_Alt.DataSaved += Tea_Info_Alt_DataSaved;
                    tea_Info_Alt.Show();
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
