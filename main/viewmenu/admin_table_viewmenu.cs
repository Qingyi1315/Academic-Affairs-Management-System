using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using 教务管理系统.main;

namespace 教务管理系统
{
    public partial class admin_table_viewmenu : Form
    {

        public admin_table_viewmenu()
        {
            InitializeComponent();
            tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl1.DrawItem += TabControl1_DrawItem;
            tabControl1.MouseDown += TabControl1_MouseDown;
            tabControl1.Padding = new Point(20, 3); // 增加标签页内边距
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
            //点击关闭按钮
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

        private void button1_Click(object sender, EventArgs e)
        {
            // 检查是否已经存在名为 "学生管理" 的选项卡
            foreach (TabPage tabPage in tabControl1.TabPages)
            {
                if (tabPage.Text == "学生管理")
                {
                    tabControl1.SelectedTab = tabPage; // 如果已存在，直接切换到该选项卡
                    return;
                }
            }

            // 创建新的选项卡
            TabPage stuTableTabPage = new TabPage("学生管理");

            // 创建 stu_table 窗体并嵌入到选项卡中
            stu_table stu_Table = new stu_table
            {
                TopLevel = false, // 设置为非顶级窗体
                FormBorderStyle = FormBorderStyle.None, // 去掉边框
                Dock = DockStyle.Fill // 填充整个选项卡
            };
            stuTableTabPage.Controls.Add(stu_Table); // 将窗体添加到选项卡
            tabControl1.TabPages.Add(stuTableTabPage); // 将选项卡添加到 tabControl1
            tabControl1.SelectedTab = stuTableTabPage; // 切换到新选项卡
            stu_Table.Show(); // 显示嵌入的窗体
        }

        private void admin_table_Load(object sender, EventArgs e)
        {
            // 避免窗体加载时自动获取焦点
            var dummyPanel = new Panel();
            dummyPanel.Visible = false;
            this.Controls.Add(dummyPanel);
            this.ActiveControl = dummyPanel;
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

        private void button2_Click(object sender, EventArgs e)
        {
            // 检查是否已经存在名为 "教师管理" 的选项卡
            foreach (TabPage tabPage in tabControl1.TabPages)
            {
                if (tabPage.Text == "教师管理")
                {
                    tabControl1.SelectedTab = tabPage; // 如果已存在，直接切换到该选项卡
                    return;
                }
            }

            // 创建新的选项卡
            TabPage teaTableTabPage = new TabPage("教师管理");

            // 创建 tea_table 窗体并嵌入到选项卡中
            tea_table tea_Table = new tea_table
            {
                TopLevel = false, // 设置为非顶级窗体
                FormBorderStyle = FormBorderStyle.None, // 去掉边框
                Dock = DockStyle.Fill // 填充整个选项卡
            };
            teaTableTabPage.Controls.Add(tea_Table); // 将窗体添加到选项卡
            tabControl1.TabPages.Add(teaTableTabPage); // 将选项卡添加到 tabControl1
            tabControl1.SelectedTab = teaTableTabPage; // 切换到新选项卡
            tea_Table.Show(); // 显示嵌入的窗体
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // 检查是否已经存在名为 "学生成绩" 的选项卡
            foreach (TabPage tabPage in tabControl1.TabPages)
            {
                if (tabPage.Text == "学生成绩")
                {
                    tabControl1.SelectedTab = tabPage; // 如果已存在，直接切换到该选项卡
                    return;
                }
            }

            // 创建新的选项卡
            TabPage stuGraTabPage = new TabPage("学生成绩");

            // 创建 stu_gra 窗体并嵌入到选项卡中
            stu_gra stu_Gra = new stu_gra
            {
                TopLevel = false, // 设置为非顶级窗体
                FormBorderStyle = FormBorderStyle.None, // 去掉边框
                Dock = DockStyle.Fill // 填充整个选项卡
            };
            stuGraTabPage.Controls.Add(stu_Gra); // 将窗体添加到选项卡
            tabControl1.TabPages.Add(stuGraTabPage); // 将选项卡添加到 tabControl1
            tabControl1.SelectedTab = stuGraTabPage; // 切换到新选项卡
            stu_Gra.Show(); // 显示嵌入的窗体
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // 检查是否已经存在名为 "课程信息" 的选项卡
            foreach (TabPage tabPage in tabControl1.TabPages)
            {
                if (tabPage.Text == "课程信息")
                {
                    tabControl1.SelectedTab = tabPage; // 如果已存在，直接切换到该选项卡
                    return;
                }
            }

            // 创建新的选项卡
            TabPage cuoTableTabPage = new TabPage("课程信息");

            // 创建 cuo_table 窗体并嵌入到选项卡中
            cou_table cuo_Table = new cou_table
            {
                TopLevel = false, // 设置为非顶级窗体
                FormBorderStyle = FormBorderStyle.None, // 去掉边框
                Dock = DockStyle.Fill // 填充整个选项卡
            };
            cuoTableTabPage.Controls.Add(cuo_Table); // 将窗体添加到选项卡
            tabControl1.TabPages.Add(cuoTableTabPage); // 将选项卡添加到 tabControl1
            tabControl1.SelectedTab = cuoTableTabPage; // 切换到新选项卡
            cuo_Table.Show(); // 显示嵌入的窗体
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // 检查是否已经存在名为 "课程安排" 的选项卡
            foreach (TabPage tabPage in tabControl1.TabPages)
            {
                if (tabPage.Text == "课程安排")
                {
                    tabControl1.SelectedTab = tabPage; // 如果已存在，直接切换到该选项卡
                    return;
                }
            }

            // 创建新的选项卡
            TabPage scheduleTabPage = new TabPage("课程安排");

            // 创建 schedule_update 窗体并嵌入到选项卡中
            schedule_update scheduleUpdate = new schedule_update
            {
                TopLevel = false, // 设置为非顶级窗体
                FormBorderStyle = FormBorderStyle.None, // 去掉边框
                Dock = DockStyle.Fill // 填充整个选项卡
            };
            scheduleTabPage.Controls.Add(scheduleUpdate); // 将窗体添加到选项卡
            tabControl1.TabPages.Add(scheduleTabPage); // 将选项卡添加到 tabControl1
            tabControl1.SelectedTab = scheduleTabPage; // 切换到新选项卡
            scheduleUpdate.Show(); // 显示嵌入的窗体
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

    }
}
