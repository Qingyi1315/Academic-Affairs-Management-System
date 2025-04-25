using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace 教务管理系统
{
    public partial class stu_info_add : Form
    {
        public delegate void DataInsertedEventHandler(object sender, EventArgs e);
        public event DataInsertedEventHandler DataInserted;

        private DatabaseHelper dbHelper;

        public stu_info_add()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["MyDatabase"].ConnectionString;
            dbHelper = new DatabaseHelper(connectionString);
        }

        private void stu_info_Load(object sender, EventArgs e)
        {
            this.comboBox1.SelectedIndex = 0;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取用户输入的数据
                string studentNumber = textBox1.Text;
                string studentName = textBox2.Text;
                string studentPassword = textBox3.Text;
                string studentGender = comboBox1.Text;
                string studentClass = textBox4.Text;
                string studentMajor = textBox5.Text;
                string studentDepartment = textBox6.Text;
                DateTime studentBirthday = dateTimePicker1.Value;
                string studentOrigin = textBox7.Text;
                string studentAddress = textBox8.Text;
                string studentPhone = textBox9.Text;
                string studentEmail = textBox10.Text;
                DateTime studentEnrollmentDate = dateTimePicker2.Value;
                DateTime studentGraduationDate = dateTimePicker3.Value;

                // 构建 SQL 插入语句
                string sql = @"
                    INSERT INTO student_information (
                        student_number, student_name, student_password, student_gender, student_class, student_major, student_department, 
                        student_birthday, student_origin, student_address, student_phone, student_email, student_enrollment_date, student_graduation_date
                    ) VALUES (
                        @studentNumber, @studentName, AES_ENCRYPT(@studentPassword, @encryption_key), @studentGender, @studentClass, @studentMajor, @studentDepartment, 
                        @studentBirthday, @studentOrigin, @studentAddress, @studentPhone, @studentEmail, @studentEnrollmentDate, @studentGraduationDate
                    )";

                // 创建参数
                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@studentNumber", MySqlDbType.VarChar) { Value = studentNumber },
                    new MySqlParameter("@studentName", MySqlDbType.VarChar) { Value = studentName },
                    new MySqlParameter("@studentPassword", MySqlDbType.VarChar) { Value = studentPassword },
                    new MySqlParameter("@studentGender", MySqlDbType.Enum) { Value = studentGender },
                    new MySqlParameter("@studentClass", MySqlDbType.VarChar) { Value = studentClass },
                    new MySqlParameter("@studentMajor", MySqlDbType.VarChar) { Value = studentMajor },
                    new MySqlParameter("@studentDepartment", MySqlDbType.VarChar) { Value = studentDepartment },
                    new MySqlParameter("@studentBirthday", MySqlDbType.Date) { Value = studentBirthday.ToString("yyyy-MM-dd") },
                    new MySqlParameter("@studentOrigin", MySqlDbType.VarChar) { Value = studentOrigin },
                    new MySqlParameter("@studentAddress", MySqlDbType.VarChar) { Value = studentAddress },
                    new MySqlParameter("@studentPhone", MySqlDbType.VarChar) { Value = studentPhone },
                    new MySqlParameter("@studentEmail", MySqlDbType.VarChar) { Value = studentEmail },
                    new MySqlParameter("@studentEnrollmentDate", MySqlDbType.Date) { Value = studentEnrollmentDate.ToString("yyyy-MM-dd") },
                    new MySqlParameter("@studentGraduationDate", MySqlDbType.Date) { Value = studentGraduationDate.ToString("yyyy-MM-dd") }
                };

                // 执行插入操作
                int rowsAffected = dbHelper.ExecuteNonQuery(sql, parameters);

                if (rowsAffected > 0)
                {
                    MessageBox.Show("数据插入成功！");
                    // 清空输入框
                    ClearInputFields();
                    // 触发事件通知数据已插入
                    DataInserted?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    MessageBox.Show("数据插入失败！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误: " + ex.Message);
            }
        }

        private void ClearInputFields()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            comboBox1.SelectedIndex = 0;
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            dateTimePicker1.Value = DateTime.Now;
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();
            textBox10.Clear();
            dateTimePicker2.Value = DateTime.Now;
            dateTimePicker3.Value = DateTime.Now;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
