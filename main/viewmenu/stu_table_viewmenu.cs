using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace 教务管理系统.viewmenu
{
    public partial class stu_table_viewmenu : Form
    {
        private DatabaseHelper dbHelper;

        public stu_table_viewmenu()
        {
            InitializeComponent();
            tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl1.DrawItem += TabControl1_DrawItem;
            tabControl1.MouseDown += TabControl1_MouseDown;
            tabControl1.Padding = new Point(20, 3); // 增加标签页内边距
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


        private void stu_table_viewmenu_Load(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDatabase"].ConnectionString;
            dbHelper = new DatabaseHelper(connectionString);

            // 避免窗体加载时自动获取焦点
            var dummyPanel = new Panel();
            dummyPanel.Visible = false;
            this.Controls.Add(dummyPanel);
            this.ActiveControl = dummyPanel;
        }

        private void TabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabPage tabPage = tabControl1.TabPages[e.Index];
            Rectangle tabRect = tabControl1.GetTabRect(e.Index);

            // 统一参数定义
            const int closeButtonSize = 15;
            const int padding = 5;

            // 绘制背景
            using (Brush backBrush = new SolidBrush(e.Index == tabControl1.SelectedIndex
                ? SystemColors.ControlLight
                : SystemColors.Control))
            {
                e.Graphics.FillRectangle(backBrush, e.Bounds);
            }

            // 计算关闭按钮位置（右侧）
            Rectangle closeButton = new Rectangle(
                tabRect.Right - closeButtonSize - padding,
                tabRect.Top + (tabRect.Height - closeButtonSize) / 2,
                closeButtonSize,
                closeButtonSize
            );

            // 计算标题区域（避开关闭按钮）
            Rectangle titleRect = new Rectangle(
                tabRect.Left + padding,
                tabRect.Top + 2, // 微调垂直位置
                tabRect.Width - (closeButtonSize + padding * 3),
                tabRect.Height
            );

            // 绘制标题
            TextRenderer.DrawText(
                e.Graphics,
                tabPage.Text,
                tabPage.Font,
                titleRect,
                tabPage.ForeColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter
            );

            // 绘制关闭按钮
            ControlPaint.DrawCaptionButton(
                e.Graphics,
                closeButton,
                CaptionButton.Close,
                e.State == DrawItemState.Selected ? ButtonState.Pushed : ButtonState.Normal
            );
        }

        private void TabControl1_MouseDown(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < tabControl1.TabPages.Count; i++)
            {
                Rectangle tabRect = tabControl1.GetTabRect(i);
                const int closeButtonSize = 15;
                const int padding = 5;

                // 使用与绘制时相同的坐标计算
                Rectangle closeButton = new Rectangle(
                    tabRect.Right - closeButtonSize - padding,
                    tabRect.Top + (tabRect.Height - closeButtonSize) / 2,
                    closeButtonSize,
                    closeButtonSize
                );

                if (closeButton.Contains(e.Location))
                {
                    tabControl1.TabPages.RemoveAt(i);
                    tabControl1.Invalidate(); // 触发重绘
                    return; // 直接退出，避免索引越界
                }
            }
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            about about = new about();
            about.Show();
        }

        private void 发送反馈ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // 定义收件人和标题
                string recipient = "1162801098@qq.com";
                string subject = "反馈意见";

                // 使用 mailto 协议构建 URL
                string mailto = $"mailto:{recipient}?subject={Uri.EscapeDataString(subject)}";

                // 打开默认邮件客户端
                System.Diagnostics.Process.Start(mailto);
            }
            catch (Exception ex)
            {
                // 如果发生错误，显示提示信息
                MessageBox.Show($"无法打开邮件客户端: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // 检查是否已经存在名为 "成绩信息" 的选项卡
                foreach (TabPage tabPage in tabControl1.TabPages)
                {
                    if (tabPage.Text == "成绩信息")
                    {
                        tabControl1.SelectedTab = tabPage; // 如果已存在，直接切换到该选项卡
                        return;
                    }
                }

                // 创建新的选项卡
                TabPage gradesTabPage = new TabPage("成绩信息");

                // 创建 stu_gra_view 窗体并嵌入到选项卡中
                stu_gra stu_Gra = new stu_gra
                {
                    TopLevel = false, // 设置为非顶级窗体
                    FormBorderStyle = FormBorderStyle.None, // 去掉边框
                    Dock = DockStyle.Fill // 填充整个选项卡
                };

                // 将 stu_gra_view 添加到选项卡
                gradesTabPage.Controls.Add(stu_Gra);
                tabControl1.TabPages.Add(gradesTabPage);
                tabControl1.SelectedTab = gradesTabPage; // 切换到新选项卡
                stu_Gra.Show(); // 显示嵌入的窗体
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取当前用户
                string currentUser = GlobalVariables.CurrentUserNumber;

                // 查询数据库获取当前用户的学生信息
                string sql = @"
                    SELECT
                        student_number,
                        student_name,
                        student_password,
                        student_gender,
                        student_class,
                        student_major,
                        student_department,
                        student_birthday,
                        student_origin,
                        student_address,
                        student_phone,
                        student_email,
                        student_enrollment_date,
                        student_graduation_date
                    FROM
                        student_information
                    WHERE
                        student_number = @studentNumber";

                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@studentNumber", MySqlDbType.VarChar) { Value = currentUser }
                };

                // 执行查询
                DataTable dataTable = dbHelper.ExecuteQuery(sql, parameters);

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    // 获取查询结果
                    DataRow row = dataTable.Rows[0];

                    // 创建 stu_info_alt 窗体并传递数据
                    stu_info_alt stu_Info_Alt = new stu_info_alt
                    {
                        StudentNumber = row["student_number"].ToString(),
                        StudentName = row["student_name"].ToString(),
                        StudentPassword = row["student_password"].ToString(),
                        StudentGender = row["student_gender"].ToString(),
                        StudentClass = row["student_class"].ToString(),
                        StudentMajor = row["student_major"].ToString(),
                        StudentDepartment = row["student_department"].ToString(),
                        StudentBirthday = row["student_birthday"].ToString(),
                        StudentOrigin = row["student_origin"].ToString(),
                        StudentAddress = row["student_address"].ToString(),
                        StudentPhone = row["student_phone"].ToString(),
                        StudentEmail = row["student_email"].ToString(),
                        StudentEnrollmentDate = row["student_enrollment_date"].ToString(),
                        StudentGraduationDate = row["student_graduation_date"].ToString()
                    };

                    // 检查是否已经存在名为 "学生信息" 的选项卡
                    foreach (TabPage tabPage in tabControl1.TabPages)
                    {
                        if (tabPage.Text == "学生信息")
                        {
                            tabControl1.SelectedTab = tabPage; // 如果已存在，直接切换到该选项卡
                            return;
                        }
                    }

                    // 创建新的选项卡
                    TabPage stuInfoTabPage = new TabPage("学生信息");

                    // 嵌入 stu_info_alt 窗体到选项卡中
                    stu_Info_Alt.TopLevel = false; // 设置为非顶级窗体
                    stu_Info_Alt.FormBorderStyle = FormBorderStyle.None; // 去掉边框
                    stu_Info_Alt.Dock = DockStyle.Fill; // 填充整个选项卡
                    stuInfoTabPage.Controls.Add(stu_Info_Alt); // 将窗体添加到选项卡
                    tabControl1.TabPages.Add(stuInfoTabPage); // 将选项卡添加到 tabControl1
                    tabControl1.SelectedTab = stuInfoTabPage; // 切换到新选项卡
                    stu_Info_Alt.Show(); // 显示嵌入的窗体
                }
                else
                {
                    MessageBox.Show("未找到学生信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                // 检查是否已经存在名为 "选课管理" 的选项卡
                foreach (TabPage tabPage in tabControl1.TabPages)
                {
                    if (tabPage.Text == "选课管理")
                    {
                        tabControl1.SelectedTab = tabPage; // 如果已存在，直接切换到该选项卡
                        return;
                    }
                }

                // 创建新的选项卡
                TabPage msTableTabPage = new TabPage("选课管理");

                // 创建 ms_table 窗体并嵌入到选项卡中
                ms_table ms_Table = new ms_table
                {
                    TopLevel = false, // 设置为非顶级窗体
                    FormBorderStyle = FormBorderStyle.None, // 去掉边框
                    Dock = DockStyle.Fill // 填充整个选项卡
                };

                // 将 ms_table 添加到选项卡
                msTableTabPage.Controls.Add(ms_Table);
                tabControl1.TabPages.Add(msTableTabPage);
                tabControl1.SelectedTab = msTableTabPage; // 切换到新选项卡
                ms_Table.Show(); // 显示嵌入的窗体
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // 检查是否已经存在名为 "我的课表" 的选项卡
                foreach (TabPage tabPage in tabControl1.TabPages)
                {
                    if (tabPage.Text == "我的课表")
                    {
                        tabControl1.SelectedTab = tabPage; // 如果已存在，直接切换到该选项卡
                        return;
                    }
                }

                // 创建新的选项卡
                TabPage scheduleTabPage = new TabPage("我的课表");

                // 创建 schedule 窗体并嵌入到选项卡中
                schedule scheduleForm = new schedule
                {
                    TopLevel = false, // 设置为非顶级窗体
                    FormBorderStyle = FormBorderStyle.None, // 去掉边框
                    Dock = DockStyle.Fill // 填充整个选项卡
                };

                // 将 schedule 窗体添加到选项卡
                scheduleTabPage.Controls.Add(scheduleForm);
                tabControl1.TabPages.Add(scheduleTabPage);
                tabControl1.SelectedTab = scheduleTabPage; // 切换到新选项卡
                scheduleForm.Show(); // 显示嵌入的窗体
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void 退出账户ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 弹出确认提示框
            var result = MessageBox.Show("确定要退出账户吗？", "退出确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // 清空全局变量
                GlobalVariables.Clear();

                // 关闭当前窗体
                this.Close();
            }
        }

        private void 文件加解密ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fileED fileED = new fileED();
            fileED.Show();
        }

        private void 简易聊天ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ClientSocket().Show();
        }

        private void 课堂随机点名ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new dmForm().Show();
        }

        private void aI聊天ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new aichat().Show();
        }
    }
}
