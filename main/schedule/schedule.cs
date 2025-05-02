using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace 教务管理系统
{
    public partial class schedule : Form
    {
        private DatabaseHelper dbHelper;

        public schedule()
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
                // 检查 "course_start_time" 列是否有值
                if (row.Cells["course_start_time"].Value != null)
                {
                    // 尝试解析时间
                    if (TimeSpan.TryParse(row.Cells["course_start_time"].Value.ToString(), out TimeSpan startTime))
                    {
                        // 根据时间段设置行颜色
                        if (startTime >= TimeSpan.FromHours(0) && startTime < TimeSpan.FromHours(12))
                        {
                            row.DefaultCellStyle.BackColor = Color.LightBlue; // 上午
                        }
                        else if (startTime >= TimeSpan.FromHours(12) && startTime < TimeSpan.FromHours(18))
                        {
                            row.DefaultCellStyle.BackColor = Color.LightGreen; // 下午
                        }
                        else if (startTime >= TimeSpan.FromHours(18) && startTime < TimeSpan.FromHours(24))
                        {
                            row.DefaultCellStyle.BackColor = Color.LightCoral; // 晚上
                        }
                    }
                }
            }
            dataGridView1.ClearSelection(); // 清除选择        
        }

        private void schedule_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                // 使用全局变量获取当前登录的用户名  
                string userNumber = GlobalVariables.CurrentUserNumber;

                string sql = string.Empty; // 初始化 sql 变量  

                if (GlobalVariables.CurrentRole == "学生")
                {
                    sql = @"  
                    SELECT DISTINCT
                        ci.course_name,
                        cwv.course_type,
                        ti.teacher_name,
                        cwv.course_location,
                        cwv.course_start_time,
                        cwv.course_end_time
                    FROM 
                        current_week_view cwv
                    JOIN 
                        course_information ci ON cwv.course_number = ci.course_number
                    JOIN 
                        teacher_information ti ON cwv.course_teacher_number = ti.teacher_number
                    JOIN 
                        course_selection cs ON cwv.course_number = cs.course_number
                    WHERE 
                        cs.course_status = '已选'AND cs.student_number = @userNumber;";
                }
                else if (GlobalVariables.CurrentRole == "教师")
                {
                    sql = @"  
                    SELECT DISTINCT
                        ci.course_name,
                        cwv.course_type,
                        ti.teacher_name,
                        cwv.course_location,
                        cwv.course_start_time,
                        cwv.course_end_time
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
                }
                else
                {
                    MessageBox.Show("未知角色，请检查登录信息。");
                    return;
                }

                MySqlParameter[] parameters = new MySqlParameter[]
                {
                   new MySqlParameter("@userNumber", MySqlDbType.VarChar) { Value = userNumber }
                };

                DataTable dataTable = dbHelper.ExecuteQuery(sql, parameters);

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    dataGridView1.AutoGenerateColumns = true; // 确保自动生成列  
                    dataGridView1.DataSource = dataTable; // 绑定数据  
                }
                else
                {
                    MessageBox.Show("本周休息。☕");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void 更改选课toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void 下周课表toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                // 使用全局变量获取当前登录的用户名  
                string userNumber = GlobalVariables.CurrentUserNumber;

                string sql = string.Empty; // 初始化 sql 变量  

                if (GlobalVariables.CurrentRole == "学生")
                {
                    sql = @"  
                    SELECT DISTINCT
                        ci.course_name,
                        nwv.course_type,
                        ti.teacher_name,
                        nwv.course_location,
                        nwv.course_start_time,
                        nwv.course_end_time
                    FROM 
                        next_week_view nwv
                    JOIN 
                        course_information ci ON nwv.course_number = ci.course_number
                    JOIN 
                        teacher_information ti ON nwv.course_teacher_number = ti.teacher_number
                    JOIN 
                        course_selection cs ON nwv.course_number = cs.course_number
                    WHERE 
                        cs.course_status = '已选'AND cs.student_number = @userNumber;";
                }
                else if (GlobalVariables.CurrentRole == "教师")
                {
                    sql = @"  
                    SELECT DISTINCT
                        ci.course_name,
                        nwv.course_type,
                        ti.teacher_name,
                        nwv.course_location,
                        nwv.course_start_time,
                        nwv.course_end_time
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
                }
                else
                {
                    MessageBox.Show("未知角色，请检查登录信息。");
                    return;
                }

                MySqlParameter[] parameters = new MySqlParameter[]
                {
                   new MySqlParameter("@userNumber", MySqlDbType.VarChar) { Value = userNumber }
                };

                DataTable dataTable = dbHelper.ExecuteQuery(sql, parameters);

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    dataGridView1.AutoGenerateColumns = true; // 确保自动生成列  
                    dataGridView1.DataSource = dataTable; // 绑定数据  
                }
                else
                {
                    MessageBox.Show("下周休息。☕");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

        }
    }
}
