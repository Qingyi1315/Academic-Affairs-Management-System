using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace 教务管理系统
{
    public partial class landing_form : Form
    {
        private DatabaseHelper dbHelper;
        private Color placeholderColor = Color.Gray;
        private Color inputColor = Color.Black;
        private Button togglePasswordButton;
        private bool isPasswordVisible = false;
        public bool IsLoginSuccessful { get; private set; }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.comboBox1.SelectedIndex = 0;

            // 初始化密码显示/隐藏按钮
            InitializeTogglePasswordButton();

            // 设置初始文本和颜色
            SetPlaceholderText(textBox1, "请输入用户名或账号");
            SetPlaceholderText(textBox2, "请输入密码");

            // 避免窗体加载时自动获取焦点
            var dummyPanel = new Panel();
            dummyPanel.Visible = false;
            this.Controls.Add(dummyPanel);
            this.ActiveControl = dummyPanel;

            this.textBox1.GotFocus += new EventHandler(textBox1_GotFocus);
            this.textBox2.GotFocus += new EventHandler(textBox2_GotFocus);
            this.textBox1.LostFocus += new EventHandler(textBox1_LostFocus);
            this.textBox2.LostFocus += new EventHandler(textBox2_LostFocus);


        }

        private void InitializeTogglePasswordButton()
        {
            // 创建按钮实例
            togglePasswordButton = new Button
            {
                Size = new Size(24, 24),
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat,
                BackgroundImageLayout = ImageLayout.Zoom,
                Visible = true
            };

            // 样式设置
            togglePasswordButton.FlatAppearance.BorderSize = 0;
            togglePasswordButton.BackColor = Color.White;
            togglePasswordButton.FlatAppearance.MouseOverBackColor = Color.White;
            togglePasswordButton.FlatAppearance.MouseDownBackColor = Color.White;

            // 初始图片设置
            UpdateToggleButtonImage();

            // 确保父容器存在
            if (textBox2.Parent == null)
            {
                var container = new Panel
                {
                    Size = textBox2.Size,
                    Location = textBox2.Location,
                    BorderStyle = BorderStyle.None
                };
                this.Controls.Add(container);
                container.Controls.Add(textBox2);
                textBox2.Location = new Point(0, 0);
            }

            // 添加按钮到父容器
            textBox2.Parent.Controls.Add(togglePasswordButton);
            UpdateButtonPosition();

            // 事件绑定
            togglePasswordButton.Click += TogglePasswordButton_Click;

            // 动态布局
            textBox2.Parent.SizeChanged += (s, e) => UpdateButtonPosition();
            textBox2.Parent.Paint += (s, e) => togglePasswordButton.BringToFront();
        }

        private void UpdateButtonPosition()
        {
            if (togglePasswordButton != null && textBox2.Parent != null)
            {
                togglePasswordButton.Left = textBox2.Right - togglePasswordButton.Width - 2;
                togglePasswordButton.Top = textBox2.Top + (textBox2.Height - togglePasswordButton.Height) / 2;
            }
        }

        private void UpdateToggleButtonImage()
        {
            var resourceName = isPasswordVisible ? "smile" : "eye_closed";
            togglePasswordButton.BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject(resourceName);
        }

        private bool IsPasswordPlaceholderActive()
        {
            return textBox2.Text == "请输入密码" &&
                   textBox2.ForeColor == placeholderColor &&
                   textBox2.PasswordChar == '\0';
        }

        private void TogglePasswordButton_Click(object sender, EventArgs e)
        {
            if (IsPasswordPlaceholderActive())
            {
                return;
            }

            isPasswordVisible = !isPasswordVisible;
            textBox2.PasswordChar = isPasswordVisible ? '\0' : '●';
            UpdateToggleButtonImage();
        }

        public landing_form()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["MyDatabase"].ConnectionString;
            dbHelper = new DatabaseHelper(connectionString);
        }

        private void SetPlaceholderText(TextBox textBox, string placeholderText)
        {
            if (textBox == textBox2)
            {
                isPasswordVisible = false;
                textBox.PasswordChar = '\0';
                UpdateToggleButtonImage();
            }

            textBox.Text = placeholderText;
            textBox.ForeColor = placeholderColor;
        }
        private void textBox1_GotFocus(object sender, EventArgs e)
        {
            if (textBox1.Text == "请输入用户名或账号")
            {
                textBox1.Text = null;
                textBox1.ForeColor = inputColor;
            }
        }

        private void textBox2_GotFocus(object sender, EventArgs e)
        {
            if (textBox2.Text == "请输入密码")
            {
                textBox2.Text = "";
                textBox2.ForeColor = inputColor;
                textBox2.PasswordChar = '●'; // 始终使用密码字符
            }
        }

        private void textBox1_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                SetPlaceholderText(textBox1, "请输入用户名或账号");
            }
        }

        private void textBox2_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                // 重置时强制更新按钮状态
                isPasswordVisible = false;
                textBox2.PasswordChar = '\0';
                UpdateToggleButtonImage();
                SetPlaceholderText(textBox2, "请输入密码");
            }
        }

        private void empty_bt_Click(object sender, EventArgs e)
        {
            SetPlaceholderText(textBox1, "请输入用户名或账号");
            SetPlaceholderText(textBox2, "请输入密码");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //string username = textBox1.Text.Trim();
            //string password = textBox2.Text.Trim();
            string username = "G001";
            string password = "password1";

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("用户名或密码不能为空！");
                return;
            }

            DataTable studentResult = null;
            DataTable teacherResult = null;
            DataTable adminResult = null;

            // 查询学生、教师和管理员表
            ValidateLogin(username, password, out studentResult, out teacherResult, out adminResult);

            // 判断账号密码是否正确
            bool isStudentValid = studentResult != null && studentResult.Rows.Count > 0;
            bool isTeacherValid = teacherResult != null && teacherResult.Rows.Count > 0;
            bool isAdminValid = adminResult != null && adminResult.Rows.Count > 0;

            if (!isStudentValid && !isTeacherValid && !isAdminValid)
            {
                MessageBox.Show("账号或密码错误！");
            }
            else
            {
                // 判断角色是否正确
                if (isStudentValid && comboBox1.SelectedIndex == 0) // 学生角色正确
                {
                    this.pictureBox1.Image = Properties.Resources._2;
                    GlobalVariables.CurrentUser = username;
                    GlobalVariables.CurrentRole = "学生";
                    IsLoginSuccessful = true;
                    this.DialogResult = DialogResult.OK;
                    this.Hide();
                }
                else if (isTeacherValid && comboBox1.SelectedIndex == 1) // 教师角色正确
                {
                    this.pictureBox1.Image = Properties.Resources._2;
                    GlobalVariables.CurrentUser = username;
                    GlobalVariables.CurrentRole = "教师";
                    IsLoginSuccessful = true;
                    this.DialogResult = DialogResult.OK;
                    this.Hide();
                }
                else if (isAdminValid && comboBox1.SelectedIndex == 2) // 管理员角色正确
                {
                    this.pictureBox1.Image = Properties.Resources._2;
                    GlobalVariables.CurrentUser = username;
                    GlobalVariables.CurrentRole = "管理员";
                    IsLoginSuccessful = true;
                    this.DialogResult = DialogResult.OK;
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("角色错误！");
                }
            }
        }

        private void ValidateLogin(string username, string password, out DataTable studentResult, out DataTable teacherResult, out DataTable adminResult)
        {
            studentResult = null;
            teacherResult = null;
            adminResult = null;

            // 查询学生表
            string studentSql = "SELECT * FROM student_information WHERE student_number = @username AND AES_DECRYPT(student_password, @encryption_key) = @password ";
            // 查询教师表
            string teacherSql = "SELECT * FROM teacher_information WHERE teacher_number = @username AND AES_DECRYPT(teacher_password, @encryption_key) = @password";
            // 查询管理员表
            string adminSql = "SELECT * FROM admin_information WHERE admin_number = @username AND AES_DECRYPT(admin_password, @encryption_key) = @password";

            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@username", username),
                new MySqlParameter("@password", password)
            };

            studentResult = dbHelper.ExecuteQuery(studentSql, parameters);
            teacherResult = dbHelper.ExecuteQuery(teacherSql, parameters);
            adminResult = dbHelper.ExecuteQuery(adminSql, parameters);
        }
    }
}