using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace 教务管理系统
{
    public class SocketHelper
    {
        //将远程连接过来的客服端的IP地址和Socket存入集合
        public static SocketHelper Intanter = new SocketHelper();
        public Dictionary<string, Socket> dicScoket = new Dictionary<string, Socket>();
        public List<string> IPItem = new List<string>();
        public static object lockObj = new object();
        public Dictionary<string, Socket> dicUsers = new Dictionary<string, Socket>();
        public List<string> UserList = new List<string>();
        public Dictionary<string, Socket> Connections = new Dictionary<string, Socket>();
        public ConcurrentDictionary<string, Socket> ActiveConnections = new ConcurrentDictionary<string, Socket>();
        private static readonly object _lock = new object();

        //消息类型枚举
        public enum MessageType
        {
            news,       // 0-广播消息
            userlist,   // 1-用户列表
            nickname,   // 2-昵称注册
            privateMsg  // 3-私信消息（新增）
        }

        public void SendMessage(byte[] buffer)
        {
            List<string> validIPs = new List<string>();

            lock (lockObj)
            {
                validIPs = IPItem.Where(ip =>
                    dicScoket.ContainsKey(ip) &&
                    ValidateSocket(dicScoket[ip])
                ).ToList();
            }

            Parallel.ForEach(validIPs, ip =>
            {
                try
                {
                    lock (lockObj)
                    {
                        if (dicScoket.TryGetValue(ip, out Socket socket))
                        {
                            socket.Send(buffer);
                        }
                    }
                }
                catch (SocketException ex) when
                    (ex.SocketErrorCode == SocketError.ConnectionReset ||
                     ex.SocketErrorCode == SocketError.ConnectionAborted)
                {
                    RemoveDisconnectedSocket(ip);
                }
                catch (ObjectDisposedException)
                {
                    RemoveDisconnectedSocket(ip);
                }
            });
        }

        private void RemoveDisconnectedSocket(string ip)
        {
            lock (lockObj)
            {
                if (dicScoket.ContainsKey(ip))
                {
                    dicScoket.Remove(ip);
                    IPItem.Remove(ip);
                }
            }
        }

        public byte[] SendMessageToClient(string message, MessageType ms = MessageType.news)
        {
            List<byte> newbuffer = new List<byte>();
            byte[] buffer = Encoding.UTF8.GetBytes(message);

            switch (ms)
            {
                case MessageType.news:
                    newbuffer.Add(0);
                    break;
                case MessageType.userlist:
                    newbuffer.Add(1);
                    break;
                case MessageType.nickname:
                    newbuffer.Add(2);
                    break;
                case MessageType.privateMsg:  // 新增case
                    newbuffer.Add(3);
                    break;
            }

            newbuffer.AddRange(buffer);
            return newbuffer.ToArray();
        }

        public byte[] RemoveHeader(byte[] buffer)
        {
            List<byte> newbuffer = buffer.ToList();
            newbuffer.RemoveAt(0);
            return newbuffer.ToArray();
        }

        public bool IsConnectionActive(string nickname)
        {
            lock (lockObj)
            {
                if (!dicUsers.ContainsKey(nickname)) return false;
                return ValidateSocket(dicUsers[nickname]);
            }
        }

        public bool ValidateSocket(Socket socket)
        {
            try
            {
                return !socket.Poll(1000, SelectMode.SelectRead) || socket.Available != 0;
            }
            catch (ObjectDisposedException)
            {
                return false;
            }
        }
    }
}
