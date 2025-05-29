using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Windows.Forms;

namespace 教务管理系统.main
{
    public partial class schedule_update_add : Form
    {
        public delegate void DataInsertedEventHandler(object sender, EventArgs e);
        public event DataInsertedEventHandler DataInserted;

        private DatabaseHelper dbHelper;

        public schedule_update_add()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["MyDatabase"].ConnectionString;
            dbHelper = new DatabaseHelper(connectionString);

        }

        protected override void WndProc(ref Message m)
        {
            const int WM_NCLBUTTONDBLCLK = 0x00A3;

            if (m.Msg == WM_NCLBUTTONDBLCLK)
            {
                return; // 阻止双击标题栏行为
            }

            base.WndProc(ref m);
        }

        private void schedule_update_add_Load(object sender, EventArgs e)
        {
            this.comboBox1.SelectedIndex = 0;
            this.comboBox2.SelectedIndex = 0;
            this.comboBox3.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取窗体数据
                string courseName = textBox1.Text;
                string courseType = comboBox1.SelectedItem.ToString();
                string teacherName = textBox2.Text;
                string courseLocation = textBox3.Text;

                // 将时间转换为 TimeSpan 类型以匹配数据库的 TIME 类型
                TimeSpan courseStartTime = dateTimePicker1.Value.TimeOfDay;
                TimeSpan courseEndTime = dateTimePicker2.Value.TimeOfDay;

                string courseDayOfWeek = comboBox2.SelectedItem.ToString();
                string courseWeekPattern = comboBox3.SelectedItem.ToString();
                string courseSpecificWeeks = textBox4.Text;

                // 日期直接使用 DateTime 类型以匹配数据库的 DATE 类型
                DateTime courseStartDate = dateTimePicker3.Value.Date;
                DateTime courseEndDate = dateTimePicker4.Value.Date;

                // SQL 插入语句
                string sql = @"
        INSERT INTO course_tables (
            course_number, 
            course_type, 
            course_teacher_number, 
            course_location, 
            course_start_time, 
            course_end_time, 
            course_day_of_week, 
            course_week_pattern, 
            course_specific_weeks, 
            course_start_date, 
            course_end_date
        ) VALUES (
            (SELECT course_number FROM course_information WHERE course_name = @courseName), 
            @courseType, 
            (SELECT teacher_number FROM teacher_information WHERE teacher_name = @teacherName), 
            @courseLocation, 
            @courseStartTime, 
            @courseEndTime, 
            @courseDayOfWeek, 
            @courseWeekPattern, 
            @courseSpecificWeeks, 
            @courseStartDate, 
            @courseEndDate
        )";

                // 参数化查询
                MySqlParameter[] parameters = new MySqlParameter[]
                {
        new MySqlParameter("@courseName", MySqlDbType.VarChar) { Value = courseName },
        new MySqlParameter("@courseType", MySqlDbType.Enum) { Value = courseType },
        new MySqlParameter("@teacherName", MySqlDbType.VarChar) { Value = teacherName },
        new MySqlParameter("@courseLocation", MySqlDbType.VarChar) { Value = courseLocation },
        new MySqlParameter("@courseStartTime", MySqlDbType.Time) { Value = courseStartTime },
        new MySqlParameter("@courseEndTime", MySqlDbType.Time) { Value = courseEndTime },
        new MySqlParameter("@courseDayOfWeek", MySqlDbType.Enum) { Value = courseDayOfWeek },
        new MySqlParameter("@courseWeekPattern", MySqlDbType.Enum) { Value = courseWeekPattern },
        new MySqlParameter("@courseSpecificWeeks", MySqlDbType.VarChar) { Value = courseSpecificWeeks },
        new MySqlParameter("@courseStartDate", MySqlDbType.Date) { Value = courseStartDate },
        new MySqlParameter("@courseEndDate", MySqlDbType.Date) { Value = courseEndDate }
                };

                // 执行插入操作
                int rowsAffected = dbHelper.ExecuteNonQuery(sql, parameters);

                if (rowsAffected > 0)
                {
                    MessageBox.Show("课表新增成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 触发数据插入事件
                    DataInserted?.Invoke(this, EventArgs.Empty);

                    // 清空表单
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("课程添加失败，请重试！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearForm()
        {
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
            dateTimePicker3.Value = DateTime.Now;
            dateTimePicker4.Value = DateTime.Now;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
