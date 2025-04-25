using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace 教务管理系统
{
    public partial class stu_gra_alt : Form
    {
        private DatabaseHelper dbHelper;
        public event EventHandler DataSaved;

        // 公有属性用于传递数据
        public string StudentNumber { get; set; }
        public string StudentName { get; set; }
        public string CourseName { get; set; }
        public string Semester { get; set; }
        public string ExamType { get; set; }
        public decimal Score { get; set; }
        public decimal GradePoint { get; set; }
        public decimal Credit { get; set; }
        public string TeacherName { get; set; }

        public stu_gra_alt()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["MyDatabase"].ConnectionString;
            dbHelper = new DatabaseHelper(connectionString);
        }

        private void stu_gra_alt_Load(object sender, EventArgs e)
        {
            // 显示传递过来的数据
            if (!string.IsNullOrEmpty(StudentNumber))
            {
                textBox1.Text = StudentNumber;
                textBox2.Text = StudentName;
                textBox3.Text = CourseName;
                comboBox1.Text = Semester;
                comboBox2.Text = ExamType;
                textBox4.Text = Score.ToString();
                textBox5.Text = GradePoint.ToString();
                textBox6.Text = Credit.ToString();
                textBox7.Text = TeacherName;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取用户输入的数据
                string studentNumber = textBox1.Text;
                string studentName = textBox2.Text;
                string courseName = textBox3.Text;
                string semester = comboBox1.Text;
                string examType = comboBox2.Text;
                decimal score = Convert.ToDecimal(textBox4.Text);
                decimal gradePoint = Convert.ToDecimal(textBox5.Text);
                decimal credit = Convert.ToDecimal(textBox6.Text);
                string teacherName = textBox7.Text;


                // 构建 SQL 语句
                string sql = @"
                    UPDATE student_grades
                    SET 
                    score = @score,
                    grade_point = @gradePoint,
                    credit = @credit
                    WHERE
                      student_number = @studentNumber
                      AND course_number = (SELECT course_number FROM course_information WHERE course_name = @courseName)
                      AND semester = @semester
                      AND exam_type = @examType";

                // 创建参数
                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@studentNumber", MySqlDbType.VarChar) { Value = studentNumber },
                    new MySqlParameter("@courseName", MySqlDbType.VarChar) { Value = courseName },
                    new MySqlParameter("@semester", MySqlDbType.Enum) { Value = semester },
                    new MySqlParameter("@examType", MySqlDbType.Enum) { Value = examType },
                    new MySqlParameter("@score", MySqlDbType.Decimal) { Value = score },
                    new MySqlParameter("@gradePoint", MySqlDbType.Decimal) { Value = gradePoint },
                    new MySqlParameter("@credit", MySqlDbType.Decimal) { Value = credit },
                };

                // 执行操作
                int rowsAffected = dbHelper.ExecuteNonQuery(sql, parameters);

                if (rowsAffected > 0)
                {
                    MessageBox.Show("数据更新成功！");
                    // 触发事件通知数据已插入
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

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
