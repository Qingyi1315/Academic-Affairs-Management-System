using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace 教务管理系统
{
    public partial class stu_gra_add : Form
    {
        public delegate void DataInsertedEventHandler(object sender, EventArgs e);
        public event DataInsertedEventHandler DataInserted;

        private DatabaseHelper dbHelper;

        public stu_gra_add()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["MyDatabase"].ConnectionString;
            dbHelper = new DatabaseHelper(connectionString);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取用户输入的数据
                string studentNumber = textBox1.Text;
                string courseName = textBox3.Text;
                string semester = comboBox1.Text;
                string examType = comboBox2.Text;
                decimal score = Convert.ToDecimal(textBox4.Text);
                decimal gradePoint = Convert.ToDecimal(textBox5.Text);
                decimal credit = Convert.ToDecimal(textBox6.Text);
                string teacherName = textBox7.Text;


                // 构建 SQL 插入语句
                string sql = @"
                    INSERT INTO student_grades (student_number, course_number, semester, exam_type, score, grade_point, credit, teacher_number)
                    VALUES
                    (
                      @studentNumber,
                      (SELECT course_number FROM course_information WHERE course_name = @courseName),
                      @semester,
                      @examType,
                      @score,
                      @gradePoint,
                      @credit,
                      (SELECT teacher_number FROM teacher_information WHERE teacher_name = @teacherName)
                    )";

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
                    new MySqlParameter("@teacherName", MySqlDbType.VarChar) { Value = teacherName }
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
            this.comboBox1.SelectedIndex = 0;
            this.comboBox2.SelectedIndex = 0;
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void stu_gra_add_Load(object sender, EventArgs e)
        {
            this.comboBox1.SelectedIndex = 0;
            this.comboBox2.SelectedIndex = 0;
        }

    }
}
