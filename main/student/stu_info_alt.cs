using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Windows.Forms;

namespace 教务管理系统
{
    public partial class stu_info_alt : Form
    {
        private DatabaseHelper dbHelper;
        public event EventHandler DataSaved;

        // 公有属性用于传递数据
        public string StudentNumber { get; set; }
        public string StudentName { get; set; }
        public string StudentPassword { get; set; }
        public string StudentGender { get; set; }
        public string StudentClass { get; set; }
        public string StudentMajor { get; set; }
        public string StudentDepartment { get; set; }
        public string StudentBirthday { get; set; }
        public string StudentOrigin { get; set; }
        public string StudentAddress { get; set; }
        public string StudentPhone { get; set; }
        public string StudentEmail { get; set; }
        public string StudentEnrollmentDate { get; set; }
        public string StudentGraduationDate { get; set; }

        public stu_info_alt()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["MyDatabase"].ConnectionString;
            dbHelper = new DatabaseHelper(connectionString);
        }

        private void stu_info_Load(object sender, EventArgs e)
        {

            // 显示传递过来的数据
            if (!string.IsNullOrEmpty(StudentNumber))
            {
                textBox1.Text = StudentNumber;
                textBox2.Text = StudentName;
                textBox3.Text = StudentPassword;
                comboBox1.Text = StudentGender;
                textBox4.Text = StudentClass;
                textBox5.Text = StudentMajor;
                textBox6.Text = StudentDepartment;
                // 解析日期字符串
                DateTime birthDate;
                if (DateTime.TryParse(StudentBirthday, out birthDate))
                {
                    dateTimePicker1.Value = birthDate;
                }
                else
                {
                    dateTimePicker1.Value = DateTime.Now;
                    MessageBox.Show("日期格式出现错误！当前显示为默认日期。");
                }
                textBox7.Text = StudentOrigin;
                textBox8.Text = StudentAddress;
                textBox9.Text = StudentPhone;
                textBox10.Text = StudentEmail;
                DateTime enrollmentDate;
                if (DateTime.TryParse(StudentEnrollmentDate, out enrollmentDate))
                {
                    dateTimePicker2.Value = enrollmentDate;
                }
                else
                {
                    dateTimePicker2.Value = DateTime.Now;
                    MessageBox.Show("日期格式出现错误！当前显示为默认日期。");
                }

                DateTime graduationDate;
                if (DateTime.TryParse(StudentGraduationDate, out graduationDate))
                {
                    dateTimePicker3.Value = graduationDate;
                }
                else
                {
                    dateTimePicker3.Value = DateTime.Now;
                    MessageBox.Show("日期格式出现错误！当前显示为默认日期。");
                }
            }
            if (GlobalVariables.CurrentRole == "学生")
            {
                textBox4.Enabled = false;
                textBox5.Enabled = false;
                textBox6.Enabled = false;
                textBox7.Enabled = false;
                textBox8.Enabled = false;
                dateTimePicker3.Enabled = false;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取用户输入的数据
                string studentNumber = textBox1.Text;
                string studentPassword = textBox3.Text;
                string studentClass = textBox4.Text;
                string studentMajor = textBox5.Text;
                string studentDepartment = textBox6.Text;
                string studentOrigin = textBox7.Text;
                string studentAddress = textBox8.Text;
                string studentPhone = textBox9.Text;
                string studentEmail = textBox10.Text;
                DateTime studentGraduationDate = dateTimePicker3.Value;

                // 构建 SQL 更新语句
                string sql = @"
                    UPDATE student_information 
                    SET student_password = @studentPassword, student_class = @studentClass, 
                        student_major = @studentMajor, student_department = @studentDepartment, 
                        student_origin = @studentOrigin, student_address = @studentAddress, student_phone = @studentPhone, student_email = @studentEmail, 
                        student_graduation_date = @studentGraduationDate
                    WHERE student_number = @studentNumber";

                // 创建参数
                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@studentNumber", MySqlDbType.VarChar) { Value = studentNumber },
                    new MySqlParameter("@studentPassword", MySqlDbType.VarChar) { Value = studentPassword },
                    new MySqlParameter("@studentClass", MySqlDbType.VarChar) { Value = studentClass },
                    new MySqlParameter("@studentMajor", MySqlDbType.VarChar) { Value = studentMajor },
                    new MySqlParameter("@studentDepartment", MySqlDbType.VarChar) { Value = studentDepartment },
                    new MySqlParameter("@studentOrigin", MySqlDbType.VarChar) { Value = studentOrigin },
                    new MySqlParameter("@studentAddress", MySqlDbType.VarChar) { Value = studentAddress },
                    new MySqlParameter("@studentPhone", MySqlDbType.VarChar) { Value = studentPhone },
                    new MySqlParameter("@studentEmail", MySqlDbType.VarChar) { Value = studentEmail },
                    new MySqlParameter("@studentGraduationDate", MySqlDbType.Date) { Value = studentGraduationDate.ToString("yyyy-MM-dd") }
                };

                // 执行更新操作
                int rowsAffected = dbHelper.ExecuteNonQuery(sql, parameters);

                if (rowsAffected > 0)
                {
                    MessageBox.Show("数据更新成功！");
                    // 通知主窗体数据已更新
                    DataSaved?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    MessageBox.Show("数据更新失败！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误: " + ex.Message);
            }
        }
    }
}
