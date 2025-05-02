using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;

namespace 教务管理系统
{
    public partial class cou_info_add : Form
    {
        public delegate void DataInsertedEventHandler(object sender, EventArgs e);
        public event DataInsertedEventHandler DataInserted;

        private DatabaseHelper dbHelper;

        public cou_info_add()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["MyDatabase"].ConnectionString;
            dbHelper = new DatabaseHelper(connectionString);
        }

        private void cuo_info_add_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取用户输入的数据
                string courseNumber = textBox1.Text;
                string courseName = textBox2.Text;
                string courseDescription = richTextBox1.Text;
                decimal courseCredit = decimal.Parse(textBox3.Text);
                string teacherName = textBox4.Text;
                string courseDepartment = textBox5.Text;

                // 构建 SQL 插入语句
                string sql = @"
            INSERT INTO course_information (
                course_number, course_name, course_description, course_credit, course_teacher_number, course_department
            ) VALUES (
                @courseNumber, @courseName, @courseDescription, @courseCredit,
                (SELECT teacher_number FROM teacher_information WHERE teacher_name = @teacherName LIMIT 1),
                @courseDepartment
            )";

                // 创建参数
                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@courseNumber", MySqlDbType.VarChar) { Value = courseNumber },
                    new MySqlParameter("@courseName", MySqlDbType.VarChar) { Value = courseName },
                    new MySqlParameter("@courseDescription", MySqlDbType.VarChar) { Value = courseDescription },
                    new MySqlParameter("@courseCredit", MySqlDbType.Decimal) { Value = courseCredit },
                    new MySqlParameter("@teacherName", MySqlDbType.VarChar) { Value = teacherName },
                    new MySqlParameter("@courseDepartment", MySqlDbType.VarChar) { Value = courseDepartment }
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
            textBox4.Clear();
            textBox5.Clear();
            richTextBox1.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            richTextBox1.SelectionFont = new Font("方正标致简体", 12, FontStyle.Bold);
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            richTextBox1.SelectionFont = new Font("方正标致简体", 12, FontStyle.Bold);
        }
    }
}
