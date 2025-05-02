using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace 教务管理系统
{
    public partial class dmForm : Form
    {
        // 存储学生名单的列表
        private List<string> studentList = new List<string>();
        private int flipCount = 0; // 记录反转次数
        private string originalText; // 原始文本
        private Timer flipTimer; // 用于反转动画的计时器
        private int currentIndex = 0; // 当前索引
        private bool isAscending = true; // 是否向上滚动
        private Panel overlayPanel; // 遮罩面板

        public dmForm()
        {
            InitializeComponent();
            InitializeStudentList();
            InitializeFlipTimer(); // 初始化反转计时器
            InitializeDragDrop(); // 初始化拖放功能
            InitializeOverlayPanel(); // 初始化遮罩面板
        }

        // 初始化反转计时器
        private void InitializeFlipTimer()
        {
            flipTimer = new Timer();
            flipTimer.Interval = 50; // 设置间隔为50毫秒
            flipTimer.Tick += FlipTimer_Tick;
        }

        // 初始化拖放功能
        private void InitializeDragDrop()
        {
            listBox1.AllowDrop = true;
            listBox1.DragEnter += ListBox1_DragEnter;
            listBox1.DragDrop += ListBox1_DragDrop;
        }

        // 初始化遮罩面板
        private void InitializeOverlayPanel()
        {
            overlayPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(128, Color.White),
                Visible = true,
            };

            Label overlayLabel = new Label
            {
                Text = "拖入txt文件到这里(每一行一个名字)",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent,
                ForeColor = Color.Gray
            };

            overlayPanel.Controls.Add(overlayLabel);
            // 将遮罩面板添加到 listBox1 上
            listBox1.Controls.Add(overlayPanel);
        }

        // 处理拖放进入事件
        private void ListBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        // 处理拖放完成事件
        private void ListBox1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];

            if (files != null && files.Length > 0)
            {
                string filePath = files[0];

                if (File.Exists(filePath) && Path.GetExtension(filePath).Equals(".txt", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        // 清空现有列表
                        studentList.Clear();
                        listBox1.Items.Clear();

                        // 读取文件每行
                        string[] lines = File.ReadAllLines(filePath);

                        foreach (string line in lines)
                        {
                            string studentName = line.Trim();

                            if (!string.IsNullOrEmpty(studentName))
                            {
                                studentList.Add(studentName);
                                listBox1.Items.Add(studentName);
                            }
                        }

                        overlayPanel.Visible = false; // 导入成功后隐藏遮罩面板
                        MessageBox.Show("导入成功！");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"导入失败：{ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show("请选择有效的文本文件 (.txt)！");
                }
            }
        }

        // 反转计时器的Tick事件
        private void FlipTimer_Tick(object sender, EventArgs e)
        {
            if (studentList.Count == 0)
                return;

            // 更新索引
            currentIndex += isAscending ? 1 : -1;

            // 边界检查
            if (currentIndex >= studentList.Count - 1)
                isAscending = false;
            if (currentIndex <= 0)
                isAscending = true;

            // 更新文本
            label1.Text = studentList[currentIndex];

            // 计数
            flipCount++;

            // 反转10次后停止
            if (flipCount >= 20) // 调整反转次数以控制动画持续时间
            {
                flipTimer.Stop();
                flipCount = 0;
                currentIndex = studentList.IndexOf(originalText); // 回到原始文本的索引
            }
        }

        // 初始化学生名单
        private void InitializeStudentList()
        {
            // 将学生名单添加到 ListBox 中
            foreach (string student in studentList)
            {
                listBox1.Items.Add(student);
            }
        }

        // 导入按钮点击事件
        private void button2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Text files (*.txt)|*.txt";
                openFileDialog.Title = "选择学生名单文件";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;

                    try
                    {
                        // 清空现有列表
                        studentList.Clear();
                        listBox1.Items.Clear();

                        // 读取文件每行
                        string[] lines = File.ReadAllLines(filePath);

                        foreach (string line in lines)
                        {
                            string studentName = line.Trim();

                            if (!string.IsNullOrEmpty(studentName))
                            {
                                studentList.Add(studentName);
                                listBox1.Items.Add(studentName);
                            }
                        }

                        overlayPanel.Visible = false; // 导入成功后隐藏遮罩面板
                        MessageBox.Show("导入成功！");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"导入失败：{ex.Message}");
                    }
                }
            }
        }

        // 点名按钮点击事件
        private void button1_Click(object sender, EventArgs e)
        {
            if (studentList.Count == 0)
            {
                MessageBox.Show("学生名单为空！");
                return;
            }

            // 随机生成一个索引
            Random random = new Random();
            int randomIndex = random.Next(studentList.Count);
            originalText = studentList[randomIndex]; // 保存原始文本
            currentIndex = randomIndex;

            // 开始反转动画
            flipTimer.Start();
        }
    }
}