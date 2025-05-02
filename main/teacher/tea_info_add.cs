using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Windows.Forms;

namespace 教务管理系统
{
    public partial class tea_info_add : Form
    {
        public delegate void DataInsertedEventHandler(object sender, EventArgs e);
        public event DataInsertedEventHandler DataInserted;

        private DatabaseHelper dbHelper;

        public tea_info_add()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["MyDatabase"].ConnectionString;
            dbHelper = new DatabaseHelper(connectionString);
        }

        private void tea_info_Load(object sender, EventArgs e)
        {
            this.comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取用户输入的数据
                string teacherNumber = textBox1.Text;
                string teacherName = textBox2.Text;
                string teacherPassword = textBox3.Text;
                string teacherGender = comboBox1.Text;
                string teacherTitle = textBox4.Text;
                string teacherDepartment = textBox5.Text;
                DateTime teacherBirthday = dateTimePicker1.Value;
                string teacherPhone = textBox6.Text;
                string teacherEmail = textBox7.Text;
                string teacherProfessionalField = textBox8.Text;
                string teacherEducationLevel = textBox9.Text;
                DateTime teacherWorkStartDate = dateTimePicker2.Value;

                // 构建 SQL 插入语句
                string sql = @"
            INSERT INTO teacher_information (
                teacher_number, teacher_name, teacher_password, teacher_gender, teacher_title, teacher_department, 
                teacher_birthday, teacher_phone, teacher_email, teacher_professional_field, teacher_education_level, teacher_work_start_date
            ) VALUES (
                @teacherNumber, @teacherName, @teacherPassword, @teacherGender, @teacherTitle, @teacherDepartment, 
                @teacherBirthday, @teacherPhone, @teacherEmail, @teacherProfessionalField, @teacherEducationLevel, @teacherWorkStartDate
            )";

                // 创建参数
                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@teacherNumber", MySqlDbType.VarChar) { Value = teacherNumber },
                    new MySqlParameter("@teacherName", MySqlDbType.VarChar) { Value = teacherName },
                    new MySqlParameter("@teacherPassword", MySqlDbType.VarChar) { Value = teacherPassword },
                    new MySqlParameter("@teacherGender", MySqlDbType.VarChar) { Value = teacherGender },
                    new MySqlParameter("@teacherTitle", MySqlDbType.VarChar) { Value = teacherTitle },
                    new MySqlParameter("@teacherDepartment", MySqlDbType.VarChar) { Value = teacherDepartment },
                    new MySqlParameter("@teacherBirthday", MySqlDbType.Date) { Value = teacherBirthday.ToString("yyyy-MM-dd") },
                    new MySqlParameter("@teacherPhone", MySqlDbType.VarChar) { Value = teacherPhone },
                    new MySqlParameter("@teacherEmail", MySqlDbType.VarChar) { Value = teacherEmail },
                    new MySqlParameter("@teacherProfessionalField", MySqlDbType.VarChar) { Value = teacherProfessionalField },
                    new MySqlParameter("@teacherEducationLevel", MySqlDbType.VarChar) { Value = teacherEducationLevel },
                    new MySqlParameter("@teacherWorkStartDate", MySqlDbType.Date) { Value = teacherWorkStartDate.ToString("yyyy-MM-dd") }
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
            dateTimePicker1.Value = DateTime.Now;
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();
            dateTimePicker2.Value = DateTime.Now;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
