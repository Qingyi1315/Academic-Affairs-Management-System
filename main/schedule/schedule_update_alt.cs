using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace 教务管理系统.main
{
    public partial class schedule_update_alt : Form
    {
        private DatabaseHelper dbHelper;
        public event EventHandler DataSaved;

        // 公有属性用于传递数据
        public string CourseName { get; set; }
        public string CourseType { get; set; }
        public string TeacherName { get; set; }
        public string CourseLocation { get; set; }
        public string CourseStartTime { get; set; }
        public string CourseEndTime { get; set; }
        public string CourseDayOfWeek { get; set; }
        public string CourseWeekPattern { get; set; }
        public string CourseSpecificWeeks { get; set; }
        public string CourseStartDate { get; set; }
        public string CourseEndDate { get; set; }

        public schedule_update_alt()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["MyDatabase"].ConnectionString;
            dbHelper = new DatabaseHelper(connectionString);
        }

        private void schedule_update_alt_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(CourseName))
            {
                textBox1.Text = CourseName;
                comboBox1.Text = CourseType;
                textBox2.Text = TeacherName;
                textBox3.Text = CourseLocation;

                string[] timeFormats = { "HH:mm:ss", "HH:mm" };

                if (!string.IsNullOrWhiteSpace(CourseStartTime))
                {
                    if (DateTime.TryParseExact(CourseStartTime.Trim(), timeFormats, null, System.Globalization.DateTimeStyles.None, out DateTime startTime))
                    {
                        dateTimePicker1.Value = startTime;
                    }
                    else
                    {
                        dateTimePicker1.Value = DateTime.Now;
                        MessageBox.Show("开始时间格式出现错误！当前显示为默认时间。");
                    }
                }

                if (!string.IsNullOrWhiteSpace(CourseEndTime))
                {
                    if (DateTime.TryParseExact(CourseEndTime.Trim(), timeFormats, null, System.Globalization.DateTimeStyles.None, out DateTime endTime))
                    {
                        dateTimePicker2.Value = endTime;
                    }
                    else
                    {
                        dateTimePicker2.Value = DateTime.Now;
                        MessageBox.Show("结束时间格式出现错误！当前显示为默认时间。");
                    }
                }


                comboBox2.Text = CourseDayOfWeek;
                comboBox3.Text = CourseWeekPattern;
                textBox4.Text = CourseSpecificWeeks;

                if (!string.IsNullOrWhiteSpace(CourseStartDate))
                {
                    if (DateTime.TryParse(CourseStartDate.Trim(), out DateTime startDate))
                    {
                        dateTimePicker3.Value = startDate;
                    }
                    else
                    {
                        dateTimePicker3.Value = DateTime.Now;
                        MessageBox.Show("开始日期格式出现错误！当前显示为默认日期。");
                    }
                }
                else
                {
                    dateTimePicker3.Value = DateTime.Now;
                    MessageBox.Show("开始日期为空！当前显示为默认日期。");
                }

                if (!string.IsNullOrWhiteSpace(CourseEndDate))
                {
                    if (DateTime.TryParse(CourseEndDate.Trim(), out DateTime endDate))
                    {
                        dateTimePicker4.Value = endDate;
                    }
                    else
                    {
                        dateTimePicker4.Value = DateTime.Now;
                        MessageBox.Show("结束日期格式出现错误！当前显示为默认日期。");
                    }
                }
                else
                {
                    dateTimePicker4.Value = DateTime.Now;
                    MessageBox.Show("结束日期为空！当前显示为默认日期。");
                }
            }

            // 避免窗体加载时自动获取焦点
            var dummyPanel = new Panel();
            dummyPanel.Visible = false;
            this.Controls.Add(dummyPanel);
            this.ActiveControl = dummyPanel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取用户输入的数据
                string courseName = textBox1.Text;
                string courseType = comboBox1.Text;
                string teacherName = textBox2.Text;
                string courseLocation = textBox3.Text;
                TimeSpan courseStartTime = dateTimePicker1.Value.TimeOfDay;
                TimeSpan courseEndTime = dateTimePicker2.Value.TimeOfDay;
                string courseDayOfWeek = comboBox2.Text;
                string courseWeekPattern = comboBox3.Text;
                string courseSpecificWeeks = textBox4.Text;
                DateTime courseStartDate = dateTimePicker3.Value.Date;
                DateTime courseEndDate = dateTimePicker4.Value.Date;

                // 构建 SQL 更新语句
                string sql = @"
                    UPDATE course_tables 
                    SET 
                        course_type = @courseType, 
                        course_teacher_number = (SELECT teacher_number FROM teacher_information WHERE teacher_name = @teacherName), 
                        course_location = @courseLocation, 
                        course_start_time = @courseStartTime, 
                        course_end_time = @courseEndTime, 
                        course_day_of_week = @courseDayOfWeek, 
                        course_week_pattern = @courseWeekPattern, 
                        course_specific_weeks = @courseSpecificWeeks, 
                        course_start_date = @courseStartDate, 
                        course_end_date = @courseEndDate
                    WHERE 
                        course_number = (SELECT course_number FROM course_information WHERE course_name = @courseName)";

                // 创建参数
                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@courseName", MySqlDbType.VarChar) { Value = courseName },
                    new MySqlParameter("@courseType", MySqlDbType.VarChar) { Value = courseType },
                    new MySqlParameter("@teacherName", MySqlDbType.VarChar) { Value = teacherName },
                    new MySqlParameter("@courseLocation", MySqlDbType.VarChar) { Value = courseLocation },
                    new MySqlParameter("@courseStartTime", MySqlDbType.Time) { Value = courseStartTime },
                    new MySqlParameter("@courseEndTime", MySqlDbType.Time) { Value = courseEndTime },
                    new MySqlParameter("@courseDayOfWeek", MySqlDbType.VarChar) { Value = courseDayOfWeek },
                    new MySqlParameter("@courseWeekPattern", MySqlDbType.VarChar) { Value = courseWeekPattern },
                    new MySqlParameter("@courseSpecificWeeks", MySqlDbType.VarChar) { Value = courseSpecificWeeks },
                    new MySqlParameter("@courseStartDate", MySqlDbType.Date) { Value = courseStartDate },
                    new MySqlParameter("@courseEndDate", MySqlDbType.Date) { Value = courseEndDate }
                };

                // 执行更新操作
                int rowsAffected = dbHelper.ExecuteNonQuery(sql, parameters);

                if (rowsAffected > 0)
                {
                    MessageBox.Show("数据更新成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 通知主窗体数据已更新
                    DataSaved?.Invoke(this, EventArgs.Empty);

                    // 关闭当前窗体
                    this.Close();
                }
                else
                {
                    MessageBox.Show("数据更新失败，请重试！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
