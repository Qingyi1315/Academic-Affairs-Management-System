using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace 教务管理系统
{
    public partial class ServerSocket : Form
    {
        public ServerSocket()
        {
            InitializeComponent();
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


        //启动服务器
        private void bt_start_Click(object sender, EventArgs e)
        {
            try
            {
                //第一步创建一个开始监听的Socket
                Socket socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //第二步创建Ip地址和端口号对象
                IPAddress ip = IPAddress.Any;
                IPEndPoint point = new IPEndPoint(ip, Convert.ToInt32(this.txt_Point.Text));
                //第三步让监听的Socket绑定Ip地址跟端口号
                socketWatch.Bind(point);
                //设置监听队列
                socketWatch.Listen(int.Parse(this.tb_num.Text));

                Thread t = new Thread(Listen);
                t.IsBackground = true;
                t.Start(socketWatch);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Listen(object o)
        {
            Socket socketWatch = o as Socket;
            while (true)
            {
                Socket clientSocket = socketWatch.Accept();

                // 接收第一个消息（昵称）
                byte[] buffer = new byte[1024];
                int r = clientSocket.Receive(buffer);
                string nickname = Encoding.UTF8.GetString(SocketHelper.Intanter.RemoveHeader(buffer), 0, r - 1);

                lock (SocketHelper.lockObj)
                {
                    SocketHelper.Intanter.dicUsers.Add(nickname, clientSocket);
                    SocketHelper.Intanter.UserList.Add(nickname);
                }
                string remoteEndPoint = clientSocket.RemoteEndPoint.ToString();
                lock (SocketHelper.lockObj)
                {
                    SocketHelper.Intanter.dicScoket.Add(remoteEndPoint, clientSocket);
                    SocketHelper.Intanter.IPItem.Add(remoteEndPoint);
                }
                AddCbItem(remoteEndPoint);
                AddContent(remoteEndPoint + "连接成功");
                BroadcastUserList();
                Thread t = new Thread(Recive);
                t.IsBackground = true;
                t.Start(clientSocket);
            }
        }

        // 新增广播用户列表方法
        private void BroadcastUserList()
        {
            string userList;
            List<Socket> validSockets = new List<Socket>();

            lock (SocketHelper.lockObj)
            {
                userList = string.Join(",", SocketHelper.Intanter.UserList);
                validSockets = SocketHelper.Intanter.dicUsers.Values
                    .Where(s => SocketHelper.Intanter.ValidateSocket(s))
                    .ToList();
            }

            byte[] buffer = SocketHelper.Intanter.SendMessageToClient(userList,
                SocketHelper.MessageType.userlist);

            foreach (var socket in validSockets)
            {
                try
                {
                    socket.Send(buffer);
                }
                catch (SocketException ex) when
                    (ex.SocketErrorCode == SocketError.ConnectionReset ||
                     ex.SocketErrorCode == SocketError.ConnectionAborted)
                {
                    HandleDisconnectedSocket(socket);
                }
            }
        }

        private void HandleDisconnectedSocket(Socket badSocket)
        {
            lock (SocketHelper.lockObj)
            {
                // 通过Socket查找IP
                var ipEntry = SocketHelper.Intanter.dicScoket
                    .FirstOrDefault(x => x.Value == badSocket);

                if (ipEntry.Key != null)
                {
                    SocketHelper.Intanter.dicScoket.Remove(ipEntry.Key);
                    SocketHelper.Intanter.IPItem.Remove(ipEntry.Key);
                }

                // 通过Socket查找用户
                var userEntry = SocketHelper.Intanter.dicUsers
                    .FirstOrDefault(x => x.Value == badSocket);

                if (userEntry.Key != null)
                {
                    SocketHelper.Intanter.dicUsers.Remove(userEntry.Key);
                    SocketHelper.Intanter.UserList.Remove(userEntry.Key);
                }
            }
        }

        //循环接收发送过来的数据
        void Recive(object o)
        {
            Socket currentSocket = o as Socket; // 正确的局部变量
            string remoteEndPoint = string.Empty;
            string nickname = string.Empty;

            try
            {
                // 记录远程端点（必须在Socket未关闭时获取）
                remoteEndPoint = currentSocket.RemoteEndPoint.ToString();

                // 通过Socket反查昵称（需在锁定环境下操作）
                lock (SocketHelper.lockObj)
                {
                    var userEntry = SocketHelper.Intanter.dicUsers
                        .FirstOrDefault(x => x.Value == currentSocket);
                    nickname = userEntry.Key ?? "未知用户";
                }

                // 主接收循环
                while (true)
                {
                    byte[] buffer = new byte[1024 * 1024 * 2];
                    int receivedBytes;

                    try
                    {
                        receivedBytes = currentSocket.Receive(buffer);
                        if (receivedBytes == 0) break;
                    }
                    catch (SocketException ex) when
                        (ex.SocketErrorCode == SocketError.ConnectionReset ||
                         ex.SocketErrorCode == SocketError.ConnectionAborted)
                    {
                        AddContent($"{nickname}({remoteEndPoint}) 异常断开: {ex.Message}");
                        break;
                    }
                    catch (ObjectDisposedException)
                    {
                        // Socket已被释放，直接退出循环
                        break;
                    }

                    // 处理接收到的数据
                    try
                    {
                        switch (buffer[0])
                        {
                            case 0:
                                HandleBroadcastMessage(buffer, receivedBytes, nickname, remoteEndPoint, currentSocket);
                                break;
                            case 3:
                                HandlePrivateMessage(buffer, receivedBytes, nickname, remoteEndPoint, currentSocket); // 传入当前Socket
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        AddContent($"处理消息时发生错误: {ex.Message}");
                    }
                }
            }
            finally
            {
                // 清理资源（无论正常/异常断开都会执行）
                lock (SocketHelper.lockObj)
                {
                    // 移除Socket记录
                    if (SocketHelper.Intanter.dicScoket.ContainsKey(remoteEndPoint))
                    {
                        SocketHelper.Intanter.dicScoket.Remove(remoteEndPoint);
                        SocketHelper.Intanter.IPItem.Remove(remoteEndPoint);
                    }

                    // 移除用户记录
                    if (!string.IsNullOrEmpty(nickname) &&
                        SocketHelper.Intanter.dicUsers.ContainsKey(nickname))
                    {
                        SocketHelper.Intanter.dicUsers.Remove(nickname);
                        SocketHelper.Intanter.UserList.Remove(nickname);
                    }
                }

                try
                {
                    if (currentSocket.Connected)
                    {
                        currentSocket.Shutdown(SocketShutdown.Both);
                    }
                    currentSocket.Close();
                    currentSocket.Dispose();
                }
                catch (ObjectDisposedException) { /* 忽略已释放的Socket */ }
                catch (SocketException ex)
                {
                    AddContent($"关闭Socket时发生错误: {ex.Message}");
                }

                // 广播更新用户列表
                BroadcastUserList();
            }
        }

        private void HandlePrivateMessage(byte[] buffer, int receivedBytes, string senderName,
            string remoteEndPoint, Socket senderSocket) // 添加senderSocket参数
        {
            try
            {
                string privateData = Encoding.UTF8.GetString(
                    SocketHelper.Intanter.RemoveHeader(buffer),
                    0,
                    receivedBytes - 1
                );

                int splitIndex = privateData.IndexOf(':');
                if (splitIndex == -1) return;

                string targetUser = privateData.Substring(0, splitIndex);
                string message = privateData.Substring(splitIndex + 1);

                Socket targetSocket = null;
                lock (SocketHelper.lockObj)
                {
                    if (!SocketHelper.Intanter.dicUsers.TryGetValue(targetUser, out targetSocket))
                    {
                        string errorMsg = $"错误：用户 {targetUser} 不存在或已离线";
                        byte[] errorBuffer = SocketHelper.Intanter.SendMessageToClient(errorMsg);
                        senderSocket.Send(errorBuffer); // 使用传入的senderSocket
                        return;
                    }
                }

                string formattedMsg = $"[私信] {senderName} -> {targetUser}:\r\n   {message}";
                byte[] targetBuffer = SocketHelper.Intanter.SendMessageToClient(formattedMsg);
                targetSocket.Send(targetBuffer);

                string receiptMsg = $"已发送给 {targetUser}:\r\n   {message}";
                byte[] receiptBuffer = SocketHelper.Intanter.SendMessageToClient(receiptMsg);
                senderSocket.Send(receiptBuffer); // 使用传入的senderSocket

                AddContent(formattedMsg);
            }
            catch (Exception ex)
            {
                AddContent($"处理私信时出错: {ex.Message}");
            }
        }

        // 原广播消息处理封装
        private void HandleBroadcastMessage(byte[] buffer, int receivedBytes,
            string nickname, string remoteEndPoint, Socket senderSocket)
        {
            try
            {
                // 1. 解析消息内容
                string message = Encoding.UTF8.GetString(
                    SocketHelper.Intanter.RemoveHeader(buffer),
                    0,
                    receivedBytes - 1
                );

                // 2. 格式化显示消息
                string formattedMsg = $"{nickname}({remoteEndPoint}):\r\n   {message}";

                // 3. 构造广播数据包（使用消息类型0）
                byte[] newBuffer = SocketHelper.Intanter.SendMessageToClient(formattedMsg);

                // 4. 通过SocketHelper广播给所有有效客户端
                SocketHelper.Intanter.SendMessage(newBuffer);

                // 5. 在服务器日志中记录广播消息
                AddContent(formattedMsg);
            }
            catch (Exception ex)
            {
                AddContent($"处理广播消息时发生错误: {ex.Message}");
            }
        }

        //将连接的IP添加到下拉框
        public void AddCbItem(string ItemName)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                cb_IP.Items.Add(ItemName);
                SocketHelper.Intanter.IPItem.Add(ItemName);
            }));
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

        private void SendMessageToClient()
        {
            if (cb_IP.SelectedItem == null)
            {
                MessageBox.Show("请先选择要发送的客户端");
                return;
            }

            string ip = cb_IP.SelectedItem.ToString();
            string msg = rtb_sendmsg.Text.Trim();

            if (string.IsNullOrWhiteSpace(msg))
            {
                MessageBox.Show("消息内容不能为空");
                return;
            }

            lock (SocketHelper.lockObj)
            {
                if (SocketHelper.Intanter.dicScoket.TryGetValue(ip, out Socket targetSocket))
                {
                    try
                    {
                        // 添加发送状态提示
                        AddContent($"正在向 {ip} 发送消息...");

                        byte[] buffer = SocketHelper.Intanter.SendMessageToClient(msg, SocketHelper.MessageType.news);
                        targetSocket.Send(buffer);

                        // 清空输入框并添加发送成功提示
                        rtb_sendmsg.Clear();
                        AddContent($"成功发送到 {ip}: {msg}");
                    }
                    catch (SocketException ex) when (ex.SocketErrorCode == SocketError.ConnectionReset)
                    {
                        AddContent($"发送失败：目标客户端 {ip} 已断开连接");
                        SocketHelper.Intanter.dicScoket.Remove(ip);
                        cb_IP.Items.Remove(ip);
                    }
                    catch (Exception ex)
                    {
                        AddContent($"发送失败: {ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show("目标客户端已断开");
                    cb_IP.Items.Remove(ip);
                }
            }
        }

        //选择连接的客户端发送数据
        private void bt_send_Click(object sender, EventArgs e)
        {
            SendMessageToClient();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new ClientSocket().Show();
        }

        private void rtb_sendmsg_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (e.Control)
                {
                    // Ctrl+Enter 换行处理
                    int selectionStart = rtb_sendmsg.SelectionStart;
                    rtb_sendmsg.Text = rtb_sendmsg.Text.Insert(selectionStart, Environment.NewLine);
                    rtb_sendmsg.SelectionStart = selectionStart + Environment.NewLine.Length;
                }
                else
                {
                    // 普通回车发送
                    SendMessageToClient();
                    e.SuppressKeyPress = true; // 阻止系统提示音
                }
            }
        }
    }
}
