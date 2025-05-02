using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace 教务管理系统
{
    public partial class fileED_logform : Form
    {
        private TextBox logTextBox;
        private Button copyButton;
        private Queue<string> pendingLogs = new Queue<string>();
        private bool isHandleCreated = false;
        // 添加无操作检测相关成员
        private System.Windows.Forms.Timer idleTimer;
        private DateTime lastActivityTime;
        private const int IdleTimeoutMinutes = 30; // 30分钟无操作触发
        private object syncLock = new object();
        private const int MaxLogLines = 5000;

        public fileED_logform()
        {
            // 必须首先调用保证控件初始化
            InitializeComponent();

            // 初始化无操作检测定时器
            idleTimer = new System.Windows.Forms.Timer
            {
                Interval = 60000 // 每分钟检查一次
            };
            idleTimer.Tick += CheckIdleTime;
            ResetActivity();
            idleTimer.Start();

            // 手动订阅句柄创建事件
            this.HandleCreated += (s, e) =>
            {
                isHandleCreated = true;
                ProcessPendingLogs();
            };
        }

        // 重置活动时间（在用户操作时调用）
        public void ResetActivity()
        {
            lock (syncLock)
            {
                lastActivityTime = DateTime.Now;
            }
        }

        // 检测无操作状态
        private void CheckIdleTime(object sender, EventArgs e)
        {
            lock (syncLock)
            {
                var idleTime = DateTime.Now - lastActivityTime;
                if (idleTime.TotalMinutes >= IdleTimeoutMinutes)
                {
                    AddAutoSeparator();
                    // 重置以避免重复触发
                    lastActivityTime = DateTime.Now;
                }
            }
        }

        // 自动分割线（仅在无操作时触发）
        private void AddAutoSeparator()
        {
            this.BeginInvoke((Action)(() =>
            {
                AppendLog($"══════════ 系统自动分割线 [{DateTime.Now:HH:mm:ss}] ══════════");
            }));
        }

        // 手动添加分割线方法
        public void AddManualSeparator()
        {
            AppendLog("──────────────────────────────────────────────");
        }

        private void InitializeComponent()
        {
            // 基础控件初始化
            logTextBox = new TextBox();
            copyButton = new Button();

            // 日志文本框配置
            logTextBox.Multiline = true;
            logTextBox.ReadOnly = true;
            logTextBox.Dock = DockStyle.Fill;
            logTextBox.ScrollBars = ScrollBars.Vertical;
            logTextBox.BorderStyle = BorderStyle.None;

            // 复制按钮配置
            copyButton.Text = "复制日志";
            copyButton.Dock = DockStyle.Bottom;
            copyButton.Height = 30;
            copyButton.Click += new EventHandler(CopyButton_Click);

            // 窗体配置
            this.TopLevel = false;  // 关键设置：允许嵌入容器
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;

            // 添加控件
            this.Controls.Add(logTextBox);
            this.Controls.Add(copyButton);
        }

        private void CopyButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (logTextBox.Text.Length > 0)
                {
                    Clipboard.SetText(logTextBox.Text);
                    MessageBox.Show("日志已复制到剪贴板", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("复制失败: " + ex.Message);
            }
        }

        // 修改后的日志追加方法
        public void AppendLog(string message)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<string>(AppendLog), message);
                return;
            }

            // 记录活动时间（写日志视为操作）
            ResetActivity();

            // 原有日志处理逻辑...
            WriteLog(message);
            ProcessPendingLogs();
        }
        private void ProcessPendingLogs()
        {
            lock (pendingLogs)
            {
                while (pendingLogs.Count > 0)
                {
                    WriteLog(pendingLogs.Dequeue());
                }
            }
        }

        private void WriteLog(string message)
        {
            // 清理旧日志
            if (logTextBox.Lines.Length > MaxLogLines)
            {
                var lines = logTextBox.Lines;
                var newLines = lines.Skip(lines.Length - MaxLogLines / 2).ToArray();
                logTextBox.Lines = newLines;
            }

            // 处理日志文本框被释放的情况
            if (logTextBox.IsDisposed) return;

            logTextBox.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");

            // 保持最后一行可见
            logTextBox.SelectionStart = logTextBox.Text.Length;
            logTextBox.ScrollToCaret();
        }

        /*        protected override void OnLoad(EventArgs e)
                {
                    base.OnLoad(e);

                    // 强制创建句柄
                    var handle = this.Handle;
                    var textBoxHandle = logTextBox.Handle;
                }
        */
    }
}