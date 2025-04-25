using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace 教务管理系统.main
{
    public partial class schedule_update : Form
    {
        private DatabaseHelper dbHelper;
        private bool isNextWeek = false; // 默认显示本周课表

        public schedule_update()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["MyDatabase"].ConnectionString;
            dbHelper = new DatabaseHelper(connectionString);
        }

        private void Schedule_Update_Add_DataInserted(object sender, EventArgs e)
        {
            // 根据当前状态刷新对应的数据
            if (isNextWeek)
            {
                LoadNextWeekData(GlobalVariables.AdminStatusTeacherNumber, false); // 刷新下周课表，不显示提示
            }
            else
            {
                LoadWeekData(GlobalVariables.AdminStatusTeacherNumber); // 刷新本周课表
            }
        }

        private void Schedule_Update_Alt_DataSaved(object sender, EventArgs e)
        {
            // 根据当前状态刷新对应的数据
            if (isNextWeek)
            {
                LoadNextWeekData(GlobalVariables.AdminStatusTeacherNumber, false); // 刷新下周课表，不显示提示
            }
            else
            {
                LoadWeekData(GlobalVariables.AdminStatusTeacherNumber); // 刷新本周课表
            }
        }

        private void 下周课表toolStripButton1_Click(object sender, EventArgs e)
        {
            if (!IsTeacherNumberSet()) return; // 检查是否已输入教师编号

            if (!isNextWeek) // 如果当前不是下周课表
            {
                isNextWeek = true; // 设置为下周课表

                // 更新按钮颜色
                下周课表toolStripButton1.BackColor = Color.LightBlue; // 选中颜色
                本周课表toolStripButton1.BackColor = SystemColors.Control; // 默认颜色

                // 加载下周课表
                LoadNextWeekData(GlobalVariables.AdminStatusTeacherNumber, true);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            while (true) // 使用循环保留弹出窗体
            {
                using (Form inputForm = new Form())
                {
                    inputForm.Text = "输入教师编号";
                    inputForm.StartPosition = FormStartPosition.CenterParent;
                    inputForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                    inputForm.Width = 300;
                    inputForm.Height = 150;

                    Label label = new Label() { Width = 200, Left = 10, Top = 20, Text = "请输入教师编号：" };
                    TextBox textBox = new TextBox() { Left = 10, Top = 50, Width = 260 };
                    Button confirmButton = new Button() { Text = "确定", Left = 200, Width = 70, Top = 80, DialogResult = DialogResult.OK };

                    inputForm.Controls.Add(label);
                    inputForm.Controls.Add(textBox);
                    inputForm.Controls.Add(confirmButton);
                    inputForm.AcceptButton = confirmButton;

                    if (inputForm.ShowDialog() == DialogResult.OK)
                    {
                        string input = textBox.Text;

                        // 检查用户是否输入了值  
                        if (!string.IsNullOrWhiteSpace(input))
                        {
                            // 验证输入的教师编号是否存在
                            if (IsValidTeacherNumber(input))
                            {
                                // 将输入的教师编号赋值给 GlobalVariables.AdminStatusTeacherNumber  
                                GlobalVariables.AdminStatusTeacherNumber = input;

                                // 获取教师本周课程表
                                LoadWeekData(input);
                                break; // 输入有效，退出循环
                            }
                            else
                            {
                                // 提示用户输入的教师编号无效
                                MessageBox.Show("输入的教师编号无效，请重新输入。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            // 提示用户未输入任何值  
                            MessageBox.Show("教师编号未设置，请重新输入。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        // 用户取消输入，退出循环
                        break;
                    }
                }
            }
        }

        private bool IsValidTeacherNumber(string teacherNumber)
        {
            try
            {
                // SQL 查询语句，用于验证教师编号是否存在
                string sql = "SELECT COUNT(*) FROM teacher_information WHERE teacher_number = @teacherNumber";

                // 参数化查询
                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@teacherNumber", MySqlDbType.VarChar) { Value = teacherNumber }
                };

                // 执行查询
                object result = dbHelper.ExecuteScalar(sql, parameters);

                // 如果查询结果大于 0，表示教师编号有效
                return Convert.ToInt32(result) > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"验证教师编号时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void LoadWeekData(string teacherNumber)
        {
            try
            {
                string sql = @"  
                    SELECT DISTINCT
                        ci.course_name,
                        cwv.course_type,
                        ti.teacher_name,
                        cwv.course_location,
                        cwv.course_start_time,
                        cwv.course_end_time,
                        cwv.course_day_of_week,
                        cwv.course_week_pattern,
                        cwv.course_specific_weeks,
                        cwv.course_start_date,
                        cwv.course_end_date
                    FROM 
                        current_week_view cwv
                    JOIN 
                        course_information ci ON cwv.course_number = ci.course_number
                    JOIN 
                        teacher_information ti ON cwv.course_teacher_number = ti.teacher_number
                    JOIN 
                        course_selection cs ON cwv.course_number = cs.course_number 
                    WHERE  
                        cs.course_status = '已选' AND cs.teacher_number = @userNumber";

                MySqlParameter[] parameters = new MySqlParameter[]
                {
                   new MySqlParameter("@userNumber", MySqlDbType.VarChar) { Value = teacherNumber }
                };

                DataTable dataTable = dbHelper.ExecuteQuery(sql, parameters);

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    dataGridView1.AutoGenerateColumns = true; // 确保自动生成列  
                    dataGridView1.DataSource = dataTable; // 绑定数据  
                    dataGridView1.ClearSelection();

                    // 隐藏指定列
                    if (dataGridView1.Columns["course_day_of_week"] != null)
                        dataGridView1.Columns["course_day_of_week"].Visible = false;

                    if (dataGridView1.Columns["course_week_pattern"] != null)
                        dataGridView1.Columns["course_week_pattern"].Visible = false;

                    if (dataGridView1.Columns["course_specific_weeks"] != null)
                        dataGridView1.Columns["course_specific_weeks"].Visible = false;

                    if (dataGridView1.Columns["course_start_date"] != null)
                        dataGridView1.Columns["course_start_date"].Visible = false;

                    if (dataGridView1.Columns["course_end_date"] != null)
                        dataGridView1.Columns["course_end_date"].Visible = false;
                }
                else
                {
                    MessageBox.Show("该教师本周休息。☕");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void schedule_update_Load(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDatabase"].ConnectionString;
            dbHelper = new DatabaseHelper(connectionString);

            // 避免窗体加载时自动获取焦点
            var dummyPanel = new Panel();
            dummyPanel.Visible = false;
            this.Controls.Add(dummyPanel);
            this.ActiveControl = dummyPanel;

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

        private void EditMenuItem_Click(object sender, EventArgs e)
        {
            if (!IsTeacherNumberSet()) return; // 检查是否已输入教师编号
            try
            {
                // 检查是否选中了行
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                    // 获取选中行的数据
                    string courseName = selectedRow.Cells["course_name"].Value.ToString();
                    string courseType = selectedRow.Cells["course_type"].Value.ToString();
                    string teacherName = selectedRow.Cells["teacher_name"].Value.ToString();
                    string courseLocation = selectedRow.Cells["course_location"].Value.ToString();
                    string courseStartTime = selectedRow.Cells["course_start_time"].Value.ToString();
                    string courseEndTime = selectedRow.Cells["course_end_time"].Value.ToString();
                    string courseDayOfWeek = selectedRow.Cells["course_day_of_week"].Value.ToString();
                    string courseWeekPattern = selectedRow.Cells["course_week_pattern"].Value.ToString();
                    string courseSpecificWeeks = selectedRow.Cells["course_specific_weeks"].Value.ToString();
                    string courseStartDate = selectedRow.Cells["course_start_date"].Value.ToString();
                    string courseEndDate = selectedRow.Cells["course_end_date"].Value.ToString();

                    // 创建并打开 schedule_update_alt 窗体
                    schedule_update_alt scheduleUpdateAlt = new schedule_update_alt
                    {
                        CourseName = courseName,
                        CourseType = courseType,
                        TeacherName = teacherName,
                        CourseLocation = courseLocation,
                        CourseStartTime = courseStartTime,
                        CourseEndTime = courseEndTime,
                        CourseDayOfWeek = courseDayOfWeek,
                        CourseWeekPattern = courseWeekPattern,
                        CourseSpecificWeeks = courseSpecificWeeks,
                        CourseStartDate = courseStartDate,
                        CourseEndDate = courseEndDate
                    };

                    // 订阅数据保存事件
                    scheduleUpdateAlt.DataSaved += Schedule_Update_Alt_DataSaved;

                    scheduleUpdateAlt.Show();
                }
                else
                {
                    MessageBox.Show("请先选中一行数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteMenuItem_Click(object sender, EventArgs e)
        {
            if (!IsTeacherNumberSet()) return; // 检查是否已输入教师编号
            try
            {
                // 检查是否选中了行
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                    string courseName = selectedRow.Cells["course_name"].Value.ToString();
                    string teacherName = selectedRow.Cells["teacher_name"].Value.ToString();

                    // 构建 SQL 删除语句
                    string sql = "DELETE FROM course_tables WHERE course_teacher_number = (SELECT teacher_number FROM teacher_information WHERE teacher_name = @teacherName) AND course_number = (SELECT course_number FROM course_information WHERE course_name = @courseName)";

                    // 创建参数
                    MySqlParameter[] parameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@courseName", MySqlDbType.VarChar) { Value = courseName },
                        new MySqlParameter("@teacherName", MySqlDbType.VarChar) { Value = teacherName }
                    };

                    // 执行删除操作
                    int rowsAffected = dbHelper.ExecuteNonQuery(sql, parameters);

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("数据删除成功！");
                        // 重新加载数据以更新 DataGridView
                        LoadWeekData(GlobalVariables.AdminStatusTeacherNumber);
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

        private void 添加信息toolStripButton1_Click(object sender, EventArgs e)
        {
            if (!IsTeacherNumberSet()) return; // 检查是否已输入教师编号
            schedule_update_add schedule_Update_Add = new schedule_update_add();
            schedule_Update_Add.DataInserted += new schedule_update_add.DataInsertedEventHandler(Schedule_Update_Add_DataInserted);
            schedule_Update_Add.Show();
        }

        private void 删除信息toolStripButton2_Click(object sender, EventArgs e)
        {
            if (!IsTeacherNumberSet()) return; // 检查是否已输入教师编号
            try
            {
                // 检查是否选中了行
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                    string courseName = selectedRow.Cells["course_name"].Value.ToString();
                    string teacherName = selectedRow.Cells["teacher_name"].Value.ToString();

                    // 构建 SQL 删除语句
                    string sql = "DELETE FROM course_tables WHERE course_teacher_number = (SELECT teacher_number FROM teacher_information WHERE teacher_name = @teacherName) AND course_number = (SELECT course_number FROM course_information WHERE course_name = @courseName)";

                    // 创建参数
                    MySqlParameter[] parameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@courseName", MySqlDbType.VarChar) { Value = courseName },
                        new MySqlParameter("@teacherName", MySqlDbType.VarChar) { Value = teacherName }
                    };

                    // 执行删除操作
                    int rowsAffected = dbHelper.ExecuteNonQuery(sql, parameters);

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("数据删除成功！");
                        // 重新加载数据以更新 DataGridView
                        LoadWeekData(GlobalVariables.AdminStatusTeacherNumber);
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
            if (!IsTeacherNumberSet()) return; // 检查是否已输入教师编号
            try
            {
                // 检查是否选中了行
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                    // 获取选中行的数据
                    string courseName = selectedRow.Cells["course_name"].Value.ToString();
                    string courseType = selectedRow.Cells["course_type"].Value.ToString();
                    string teacherName = selectedRow.Cells["teacher_name"].Value.ToString();
                    string courseLocation = selectedRow.Cells["course_location"].Value.ToString();
                    string courseStartTime = selectedRow.Cells["course_start_time"].Value.ToString();
                    string courseEndTime = selectedRow.Cells["course_end_time"].Value.ToString();
                    string courseDayOfWeek = selectedRow.Cells["course_day_of_week"].Value.ToString();
                    string courseWeekPattern = selectedRow.Cells["course_week_pattern"].Value.ToString();
                    string courseSpecificWeeks = selectedRow.Cells["course_specific_weeks"].Value.ToString();
                    string courseStartDate = selectedRow.Cells["course_start_date"].Value.ToString();
                    string courseEndDate = selectedRow.Cells["course_end_date"].Value.ToString();

                    // 创建并打开 schedule_update_alt 窗体
                    schedule_update_alt scheduleUpdateAlt = new schedule_update_alt
                    {
                        CourseName = courseName,
                        CourseType = courseType,
                        TeacherName = teacherName,
                        CourseLocation = courseLocation,
                        CourseStartTime = courseStartTime,
                        CourseEndTime = courseEndTime,
                        CourseDayOfWeek = courseDayOfWeek,
                        CourseWeekPattern = courseWeekPattern,
                        CourseSpecificWeeks = courseSpecificWeeks,
                        CourseStartDate = courseStartDate,
                        CourseEndDate = courseEndDate
                    };

                    // 订阅数据保存事件
                    scheduleUpdateAlt.DataSaved += Schedule_Update_Alt_DataSaved;

                    scheduleUpdateAlt.Show();
                }
                else
                {
                    MessageBox.Show("请先选中一行数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsTeacherNumberSet()
        {
            if (string.IsNullOrWhiteSpace(GlobalVariables.AdminStatusTeacherNumber))
            {
                MessageBox.Show("请先输入教师编号！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void 本周课表toolStripButton1_Click(object sender, EventArgs e)
        {
            if (!IsTeacherNumberSet()) return; // 检查是否已输入教师编号

            if (isNextWeek) // 如果当前是下周课表
            {
                isNextWeek = false; // 设置为本周课表

                // 更新按钮颜色
                本周课表toolStripButton1.BackColor = Color.LightBlue; // 选中颜色
                下周课表toolStripButton1.BackColor = SystemColors.Control; // 默认颜色

                // 加载本周课表
                LoadWeekData(GlobalVariables.AdminStatusTeacherNumber);
            }
        }
        private void LoadNextWeekData(string teacherNumber, bool showMessage = true)
        {
            try
            {
                string sql = @"  
                    SELECT DISTINCT
                        ci.course_name,
                        nwv.course_type,
                        ti.teacher_name,
                        nwv.course_location,
                        nwv.course_start_time,
                        nwv.course_end_time,
                        nwv.course_day_of_week,
                        nwv.course_week_pattern,
                        nwv.course_specific_weeks,
                        nwv.course_start_date,
                        nwv.course_end_date
                    FROM 
                        next_week_view nwv
                    JOIN 
                        course_information ci ON nwv.course_number = ci.course_number
                    JOIN 
                        teacher_information ti ON nwv.course_teacher_number = ti.teacher_number
                    JOIN 
                        course_selection cs ON nwv.course_number = cs.course_number 
                    WHERE  
                        cs.course_status = '已选' AND cs.teacher_number = @userNumber";

                MySqlParameter[] parameters = new MySqlParameter[]
                {
                   new MySqlParameter("@userNumber", MySqlDbType.VarChar) { Value = teacherNumber }
                };

                DataTable dataTable = dbHelper.ExecuteQuery(sql, parameters);

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    dataGridView1.AutoGenerateColumns = true; // 确保自动生成列  
                    dataGridView1.DataSource = dataTable; // 绑定数据  
                    dataGridView1.ClearSelection();

                    // 隐藏指定列
                    if (dataGridView1.Columns["course_day_of_week"] != null)
                        dataGridView1.Columns["course_day_of_week"].Visible = false;

                    if (dataGridView1.Columns["course_week_pattern"] != null)
                        dataGridView1.Columns["course_week_pattern"].Visible = false;

                    if (dataGridView1.Columns["course_specific_weeks"] != null)
                        dataGridView1.Columns["course_specific_weeks"].Visible = false;

                    if (dataGridView1.Columns["course_start_date"] != null)
                        dataGridView1.Columns["course_start_date"].Visible = false;

                    if (dataGridView1.Columns["course_end_date"] != null)
                        dataGridView1.Columns["course_end_date"].Visible = false;
                }
                else
                {
                    // 仅在需要时显示提示
                    if (showMessage)
                    {
                        MessageBox.Show("该教师下周休息。☕");
                    }
                    dataGridView1.DataSource = GetEmptyDataTable(); // 设置空数据模板
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private DataTable GetEmptyDataTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("course_name", typeof(string));
            table.Columns.Add("course_type", typeof(string));
            table.Columns.Add("teacher_name", typeof(string));
            table.Columns.Add("course_location", typeof(string));
            table.Columns.Add("course_start_time", typeof(string));
            table.Columns.Add("course_end_time", typeof(string));
            return table;
        }
    }
}
