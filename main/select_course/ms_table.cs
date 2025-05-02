using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace 教务管理系统
{
    public partial class ms_table : Form
    {
        private DatabaseHelper dbHelper;
        public ms_table()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["MyDatabase"].ConnectionString;
            dbHelper = new DatabaseHelper(connectionString);

            // 订阅 DataBindingComplete 事件
            dataGridView1.DataBindingComplete += DataGridView1_DataBindingComplete;
        }

        private void DataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // 遍历 DataGridView 的行，根据课程状态设置行颜色
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["course_status"].Value != null)
                {
                    string status = row.Cells["course_status"].Value.ToString();
                    if (status == "已选")
                    {
                        row.DefaultCellStyle.BackColor = Color.LightGreen; // 设置为绿色
                        row.DefaultCellStyle.ForeColor = Color.Black; // 确保前景色可见
                    }
                    else if (status == "退选")
                    {
                        row.DefaultCellStyle.BackColor = Color.LightGray; // 设置为灰色
                        row.DefaultCellStyle.ForeColor = Color.Black; // 确保前景色可见
                    }
                }
            }
            dataGridView1.ClearSelection();
        }


        public void ms_table_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void Ms_Info_Update_ReLoad(object sender, EventArgs e)
        {
            // 重新加载数据以更新 DataGridView
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                // 使用全局变量获取当前登录的用户名
                string userNumber = GlobalVariables.CurrentUserNumber;

                string sql = @"
            SELECT
                cs.course_number,
                (SELECT ci.course_name FROM course_information ci WHERE ci.course_number = cs.course_number) AS course_name,
                (SELECT ci.course_credit FROM course_information ci WHERE ci.course_number = cs.course_number) AS course_credit,
                (SELECT ti.teacher_name FROM teacher_information ti WHERE ti.teacher_number = (SELECT ci.course_teacher_number FROM course_information ci WHERE ci.course_number = cs.course_number)) AS teacher_name,
                cs.course_status
            FROM 
                course_selection cs
            WHERE 
                cs.student_number = @userNumber
                AND (cs.course_status = '已选' OR cs.course_status = '退选');";

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
                    MessageBox.Show("没有数据加载到 DataGridView。");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void 更改选课toolStripButton1_Click(object sender, EventArgs e)
        {
            ms_info_update ms_Info_Update = new ms_info_update();
            ms_Info_Update.DataUpdate += Ms_Info_Update_ReLoad;
            ms_Info_Update.Show();

        }
    }
}
