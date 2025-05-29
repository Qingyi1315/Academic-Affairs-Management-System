using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace 教务管理系统
{
    public partial class ClientSocket : Form
    {
        public ClientSocket()
        {
            InitializeComponent();
            listBox1.MouseDoubleClick += ListBox1_MouseDoubleClick;
            if (GlobalVariables.CurrentRole == "学生")
            {
                tb_name.Text = GlobalVariables.CurrentUserName + GlobalVariables.CurrentUserNumber;
                tb_name.Enabled = false;
            }
            else if (GlobalVariables.CurrentRole == "教师")
            {
                tb_name.Text = GlobalVariables.CurrentUserName + GlobalVariables.CurrentUserNumber;
                tb_name.Enabled = false;
            }
            else if (GlobalVariables.CurrentRole == "管理员")
            {
                tb_name.Text = GlobalVariables.CurrentUserName + GlobalVariables.CurrentUserNumber + "(管理员)";
                tb_name.Enabled = false;

            }
        }
        Socket socketSend;

        protected override void WndProc(ref Message m)
        {
            const int WM_NCLBUTTONDBLCLK = 0x00A3;

            if (m.Msg == WM_NCLBUTTONDBLCLK)
            {
                return; // 阻止双击标题栏行为
            }

            base.WndProc(ref m);
        }


        private void ListBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBox1.SelectedItem == null) return;

            string selectedUser = listBox1.SelectedItem.ToString();
            string myName = tb_name.Text.Trim();

            if (selectedUser == myName)
            {
                MessageBox.Show("不能选择自己作为消息接收方");
                return;
            }

            label4.Text = selectedUser;
            label4.ForeColor = Color.Green;
        }


        //将接受到的内容显示出来
        private void AddContent(string content)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                rtb_log.AppendText(content + " ");
                rtb_log.AppendText("\r\n");
                rtb_log.Focus();//先获取焦点
                rtb_log.Select(rtb_log.TextLength, 0);//选中数据末尾0个字符
                rtb_log.ScrollToCaret();//将滚动条移动到当前位置
                                        //记录收到的字符个数
                                        //toolStripStatusLabel1.Text += (int.Parse(toolStripStatusLabel1.Text) + content.Length).ToString();
            }));
        }
        // 修改连接方法，发送昵称
        private void bt_start_Click(object sender, EventArgs e)
        {
            try
            {
                socketSend = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint point = new IPEndPoint(IPAddress.Parse(tb_IP.Text), Convert.ToInt32(tb_Point.Text));
                socketSend.Connect(point);

                // 发送昵称作为第一个消息
                string nickname = tb_name.Text.Trim();
                byte[] nameBuffer = Encoding.UTF8.GetBytes(nickname);
                socketSend.Send(SocketHelper.Intanter.SendMessageToClient(nickname, SocketHelper.MessageType.nickname));

                Thread t = new Thread(Recive);
                t.IsBackground = true;
                t.Start();
            }
            catch { /* 异常处理 */ }
        }

        // 修改消息接收处理
        void Recive()
        {
            while (true)
            {
                try
                {
                    byte[] buffer = new byte[1024 * 1024 * 2];
                    int r = socketSend.Receive(buffer);
                    if (r == 0) break;

                    switch (buffer[0])
                    {
                        case 0: // 文本消息
                            string str = Encoding.UTF8.GetString(SocketHelper.Intanter.RemoveHeader(buffer), 0, r - 1);
                            AddContent(str);
                            break;
                        case 1: // 用户列表（修正消息类型）
                            string userList = Encoding.UTF8.GetString(SocketHelper.Intanter.RemoveHeader(buffer), 0, r - 1);
                            UpdateUserList(userList);
                            break;
                        case 4: // 新增成功回执类型
                            string receipt = Encoding.UTF8.GetString(
                                SocketHelper.Intanter.RemoveHeader(buffer),
                                0,
                                r - 1
                            );
                            AddContent(receipt);
                            break;
                    }
                }
                catch { /* 异常处理 */ }
            }
        }

        private void UpdateUserList(string userList)
        {
            this.BeginInvoke(new MethodInvoker(() =>
            {
                listBox1.BeginUpdate();
                listBox1.Items.Clear();

                foreach (string user in userList.Split(','))
                {
                    if (!string.IsNullOrEmpty(user))
                        listBox1.Items.Add(user);
                }

                listBox1.EndUpdate();
            }));
        }
        private void SendMessage()
        {
            string receiver = label4.Text;
            string myName = tb_name.Text.Trim();
            string msg = rtb_sendmsg.Text.Trim();

            // 验证逻辑
            if (string.IsNullOrWhiteSpace(receiver))
            {
                MessageBox.Show("请先双击选择接收用户");
                return;
            }

            if (receiver == myName)
            {
                MessageBox.Show("不能给自己发送私信");
                return;
            }

            if (string.IsNullOrWhiteSpace(msg))
            {
                MessageBox.Show("消息内容不能为空");
                return;
            }

            try
            {
                // 构造私信格式：接收者:消息内容
                string fullMsg = $"{receiver}:{msg}";
                byte[] buffer = SocketHelper.Intanter.SendMessageToClient(
                    fullMsg,
                    SocketHelper.MessageType.privateMsg
                );

                socketSend.Send(buffer);
                rtb_sendmsg.Clear();

                // 在本地显示发送记录
                AddContent($"我 -> {receiver}: {msg} (发送中...)");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发送失败: {ex.Message}");
            }
        }


        private void bt_send_Click(object sender, EventArgs e)
        {
            SendMessage();
        }

        private void rtb_sendmsg_KeyDown(object sender, KeyEventArgs e)
        {
            // 处理回车发送（支持Ctrl+Enter换行）
            if (e.KeyCode == Keys.Enter)
            {
                if (e.Control)
                {
                    // Ctrl+Enter 换行处理
                    int selectionStart = rtb_sendmsg.SelectionStart;
                    rtb_sendmsg.Text = rtb_sendmsg.Text.Insert(rtb_sendmsg.SelectionStart, Environment.NewLine);
                    rtb_sendmsg.SelectionStart = selectionStart + Environment.NewLine.Length;
                }
                else
                {
                    // 普通回车发送
                    SendMessage();
                    e.SuppressKeyPress = true; // 阻止系统提示音
                }
            }
        }
    }
}
