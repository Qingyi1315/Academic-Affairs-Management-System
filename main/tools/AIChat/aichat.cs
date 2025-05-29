using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using 教务管理系统.AiServices;

namespace 教务管理系统
{
    public partial class aichat : Form
    {
        // 控件声明
        private ComboBox cb_service = new ComboBox();
        private StatusStrip statusStrip;
        private ToolStripStatusLabel statusLabel;

        private IAiClient _currentClient;
        private bool _isProcessing = false;

        public aichat()
        {
            InitializeComponent();
            InitializeCustomComponents();
            InitializeEventHandlers();
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

        #region 初始化方法
        private void InitializeCustomComponents()
        {
            // 服务选择器
            cb_service.DropDownStyle = ComboBoxStyle.DropDownList;
            cb_service.Items.AddRange(new[] { "DeepSeek", "Kimi" });
            cb_service.SelectedIndex = 0;
            cb_service.Location = new Point(10, 10);

            // 状态栏
            statusStrip = new StatusStrip();
            statusLabel = new ToolStripStatusLabel();
            statusStrip.Items.Add(statusLabel);
            statusStrip.Location = new Point(0, 379);
            statusStrip.Size = new Size(497, 20);

            // 添加控件
            Controls.AddRange(new Control[] {
                cb_service,
                statusStrip
            });
        }

        private void InitializeEventHandlers()
        {
            cb_service.SelectedIndexChanged += (s, e) => LoadApiKey();
            bt_send.Click += async (s, e) => await ProcessRequestAsync();
            bt_validate.Click += async (s, e) => await ValidateApiKeyAsync();
        }
        #endregion

        #region 核心业务逻辑
        private async Task ProcessRequestAsync()
        {
            if (_isProcessing || !ValidateInput()) return;

            try
            {
                StartProcessing();
                using (_currentClient = CreateClient())
                {
                    AppendMessage("您", rtb_sendmsg.Text);
                    var response = await _currentClient.GetResponseAsync(
                        rtb_sendmsg.Text,
                        new CancellationTokenSource(30000).Token
                    );
                    AppendMessage("AI助手", response);
                    SaveChatHistory();
                }
            }
            catch (Exception ex)
            {
                ShowError($"请求失败: {ex.Message}");
            }
            finally
            {
                EndProcessing();
                rtb_sendmsg.Clear();
            }
        }

        private async Task ValidateApiKeyAsync()
        {
            if (_isProcessing || !ValidateInput()) return;

            try
            {
                StartProcessing();
                using (var client = CreateClient())
                {
                    var isValid = await client.ValidateApiKey(
                        new CancellationTokenSource(15000).Token
                    );
                    HandleValidationResult(isValid);
                }
            }
            catch (Exception ex)
            {
                ShowError($"验证失败: {ex.Message}");
            }
            finally
            {
                EndProcessing();
            }
        }
        #endregion

        #region 辅助方法
        private IAiClient CreateClient()
        {
            switch (cb_service.SelectedItem.ToString())
            {
                case "DeepSeek":
                    return new DeepSeek(tb_api.Text.Trim());
                case "Kimi":
                    return new Kimi(tb_api.Text.Trim());
                default:
                    throw new NotSupportedException("不支持的AI服务");
            }
        }

        private void LoadApiKey()
        {
            var service = cb_service.SelectedItem.ToString().ToLower();
            var keyFile = $"{service}_api.key";

            try
            {
                if (File.Exists(keyFile))
                {
                    tb_api.Text = File.ReadAllText(keyFile, Encoding.UTF8);
                    tb_api.PasswordChar = '*';
                }
            }
            catch { /* 忽略加载错误 */ }
        }

        private void SaveApiKey()
        {
            var service = cb_service.SelectedItem.ToString().ToLower();
            File.WriteAllText($"{service}_api.key", tb_api.Text);
        }

        private void HandleValidationResult(bool isValid)
        {
            if (isValid)
            {
                SaveApiKey();
                MessageBox.Show("验证成功", "系统提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("密钥无效", "验证失败",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void StartProcessing()
        {
            _isProcessing = true;
            ToggleUIState(false);
            statusLabel.Text = "处理中...";
            Cursor = Cursors.WaitCursor;
        }

        private void EndProcessing()
        {
            _isProcessing = false;
            ToggleUIState(true);
            statusLabel.Text = "就绪";
            Cursor = Cursors.Default;
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(tb_api.Text))
            {
                MessageBox.Show("请输入API密钥", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void ToggleUIState(bool enable)
        {
            bt_send.Enabled = enable;
            bt_validate.Enabled = enable;
            rtb_sendmsg.Enabled = enable;
        }

        private void AppendMessage(string sender, string message)
        {
            if (rtb_chat.InvokeRequired)
            {
                rtb_chat.Invoke(new Action(() => AppendMessage(sender, message)));
                return;
            }

            var timestamp = DateTime.Now.ToString("HH:mm:ss");
            rtb_chat.AppendText($"[{timestamp}] {sender}：\n{message}\n\n");
            rtb_chat.ScrollToCaret();
        }

        private void SaveChatHistory()
        {
            try
            {
                File.AppendAllText($"chatlog_{DateTime.Today:yyyyMMdd}.txt",
                    $"[{DateTime.Now}] 用户：{rtb_sendmsg.Text}\n",
                    Encoding.UTF8);
            }
            catch { /* 忽略保存错误 */ }
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message, "错误",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        #endregion
    }
}