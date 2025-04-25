using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace 教务管理系统
{
    public partial class tea_info_alt : Form
    {
        private DatabaseHelper dbHelper;
        public event EventHandler DataSaved;

        // 公有属性用于传递数据
        public string TeacherNumber { get; set; }
        public string TeacherName { get; set; }
        public string TeacherPassword { get; set; }
        public string TeacherGender { get; set; }
        public string TeacherTitle { get; set; }
        public string TeacherDepartment { get; set; }
        public string TeacherBirthday { get; set; }
        public string TeacherPhone { get; set; }
        public string TeacherEmail { get; set; }
        public string TeacherProfessionalField { get; set; }
        public string TeacherEducationLevel { get; set; }
        public string TeacherWorkStartDate { get; set; }

        public tea_info_alt()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["MyDatabase"].ConnectionString;
            dbHelper = new DatabaseHelper(connectionString);
        }

        private void tea_info_Load(object sender, EventArgs e)
        {
            // 显示传递过来的数据
            if (!string.IsNullOrEmpty(TeacherNumber))
            {
                textBox1.Text = TeacherNumber;
                textBox2.Text = TeacherName;
                textBox3.Text = TeacherPassword;
                comboBox1.Text = TeacherGender;
                textBox4.Text = TeacherTitle;
                textBox5.Text = TeacherDepartment;
                // 解析日期字符串
                DateTime birthday;
                if (DateTime.TryParse(TeacherBirthday, out birthday))
                {
                    dateTimePicker1.Value = birthday;
                }
                else
                {
                    dateTimePicker1.Value = DateTime.Now;
                    MessageBox.Show("日期格式出现错误！当前显示为默认日期。");
                }
                textBox6.Text = TeacherPhone;
                textBox7.Text = TeacherEmail;
                textBox8.Text = TeacherProfessionalField;
                textBox9.Text = TeacherEducationLevel;
                DateTime workStartDate;
                if (DateTime.TryParse(TeacherWorkStartDate, out workStartDate))
                {
                    dateTimePicker2.Value = workStartDate;
                }
                else
                {
                    dateTimePicker2.Value = DateTime.Now;
                    MessageBox.Show("日期格式出现错误！当前显示为默认日期。");
                }
            }
            if (GlobalVariables.CurrentRole == "教师")
            {
                textBox4.Enabled = false;
                textBox5.Enabled = false;
                textBox8.Enabled = false;
                textBox9.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取用户输入的数据
                string teacherNumber = textBox1.Text;
                string teacherPassword = textBox3.Text;
                string teacherTitle = textBox4.Text;
                string teacherDepartment = textBox5.Text;
                string teacherPhone = textBox6.Text;
                string teacherEmail = textBox7.Text;
                string teacherProfessionalField = textBox8.Text;
                string teacherEducationLevel = textBox9.Text;

                // 构建 SQL 更新语句
                string sql = @"
                    UPDATE teacher_information 
                    SET teacher_password = AES_ENCRYPT(@teacherPassword, @encryption_key),  
                        teacher_title = @teacherTitle, teacher_department = @teacherDepartment, 
                        teacher_phone = @teacherPhone, teacher_email = @teacherEmail, teacher_professional_field = @teacherProfessionalField, 
                        teacher_education_level = @teacherEducationLevel
                    WHERE teacher_number = @teacherNumber";

                // 创建参数
                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@teacherNumber", MySqlDbType.VarChar) { Value = teacherNumber },
                    new MySqlParameter("@teacherPassword", MySqlDbType.VarChar) { Value = teacherPassword },
                    new MySqlParameter("@teacherTitle", MySqlDbType.VarChar) { Value = teacherTitle },
                    new MySqlParameter("@teacherDepartment", MySqlDbType.VarChar) { Value = teacherDepartment },
                    new MySqlParameter("@teacherPhone", MySqlDbType.VarChar) { Value = teacherPhone },
                    new MySqlParameter("@teacherEmail", MySqlDbType.VarChar) { Value = teacherEmail },
                    new MySqlParameter("@teacherProfessionalField", MySqlDbType.VarChar) { Value = teacherProfessionalField },
                    new MySqlParameter("@teacherEducationLevel", MySqlDbType.VarChar) { Value = teacherEducationLevel },
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
