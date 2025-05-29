using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;

namespace 教务管理系统
{
    public partial class cou_info_alt : Form
    {
        public event EventHandler DataSaved;
        private DatabaseHelper dbHelper;

        // 公有属性用于传递数据
        public string CourseNumber { get; set; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public decimal CourseCredit { get; set; }
        public string TeacherName { get; set; }
        public string CourseDepartment { get; set; }


        public cou_info_alt()
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


        private void cuo_info_alt_Load(object sender, EventArgs e)
        {
            // 显示传递过来的数据
            if (!string.IsNullOrEmpty(CourseNumber))
            {
                textBox1.Text = CourseNumber;
                textBox2.Text = CourseName;
                richTextBox1.Text = CourseDescription;
                textBox3.Text = CourseCredit.ToString();
                textBox4.Text = TeacherName;
                textBox5.Text = CourseDepartment;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取用户输入的数据
                string courseNumber = textBox1.Text;
                string courseDescription = richTextBox1.Text;
                decimal courseCredit = decimal.Parse(textBox3.Text);
                string teacherName = textBox4.Text;
                string courseDepartment = textBox5.Text;

                // 构建 SQL 更新语句
                string sql = @"
                    UPDATE course_information 
                    SET course_description = @courseDescription, course_credit = @courseCredit, 
                        course_teacher_number = (SELECT teacher_number FROM teacher_information WHERE teacher_name = @teacherName LIMIT 1),
                        course_department = @courseDepartment
                    WHERE course_number = @courseNumber";

                // 创建参数
                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@courseNumber", MySqlDbType.VarChar) { Value = courseNumber },
                    new MySqlParameter("@courseDescription", MySqlDbType.VarChar) { Value = courseDescription },
                    new MySqlParameter("@courseCredit", MySqlDbType.Decimal) { Value = courseCredit },
                    new MySqlParameter("@teacherName", MySqlDbType.VarChar) { Value = teacherName },
                    new MySqlParameter("@courseDepartment", MySqlDbType.VarChar) { Value = courseDepartment }
                };

                // 执行更新操作
                int rowsAffected = dbHelper.ExecuteNonQuery(sql, parameters);

                if (rowsAffected > 0)
                {
                    MessageBox.Show("数据更新成功！");
                    // 通知主窗体数据已更新
                    DataSaved?.Invoke(this, EventArgs.Empty);
                    this.Close();
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

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            richTextBox1.SelectionFont = new Font("方正标致简体", 12, FontStyle.Regular);

        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            richTextBox1.SelectionFont = new Font("方正标致简体", 12, FontStyle.Regular);

        }
    }
}
