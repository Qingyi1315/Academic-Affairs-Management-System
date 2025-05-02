namespace 教务管理系统
{
    partial class ClientSocket
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.bt_start = new System.Windows.Forms.Button();
            this.tb_Point = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_IP = new System.Windows.Forms.TextBox();
            this.rtb_sendmsg = new System.Windows.Forms.RichTextBox();
            this.bt_send = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_name = new System.Windows.Forms.TextBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.rtb_log = new System.Windows.Forms.RichTextBox();
            this.tp_news = new System.Windows.Forms.TabControl();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tabPage1.SuspendLayout();
            this.tp_news.SuspendLayout();
            this.SuspendLayout();
            // 
            // bt_start
            // 
            this.bt_start.Location = new System.Drawing.Point(570, 23);
            this.bt_start.Name = "bt_start";
            this.bt_start.Size = new System.Drawing.Size(75, 23);
            this.bt_start.TabIndex = 11;
            this.bt_start.Text = "连接";
            this.bt_start.UseVisualStyleBackColor = true;
            this.bt_start.Click += new System.EventHandler(this.bt_start_Click);
            // 
            // tb_Point
            // 
            this.tb_Point.Location = new System.Drawing.Point(298, 25);
            this.tb_Point.Name = "tb_Point";
            this.tb_Point.Size = new System.Drawing.Size(66, 21);
            this.tb_Point.TabIndex = 7;
            this.tb_Point.Text = "50000";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "IP地址：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(239, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "端口号：";
            // 
            // tb_IP
            // 
            this.tb_IP.Location = new System.Drawing.Point(76, 25);
            this.tb_IP.Name = "tb_IP";
            this.tb_IP.Size = new System.Drawing.Size(151, 21);
            this.tb_IP.TabIndex = 6;
            this.tb_IP.Text = "127.0.0.1";
            // 
            // rtb_sendmsg
            // 
            this.rtb_sendmsg.Location = new System.Drawing.Point(20, 326);
            this.rtb_sendmsg.Name = "rtb_sendmsg";
            this.rtb_sendmsg.Size = new System.Drawing.Size(385, 63);
            this.rtb_sendmsg.TabIndex = 13;
            this.rtb_sendmsg.Text = "";
            this.rtb_sendmsg.KeyDown += new System.Windows.Forms.KeyEventHandler(this.rtb_sendmsg_KeyDown);
            // 
            // bt_send
            // 
            this.bt_send.Location = new System.Drawing.Point(411, 352);
            this.bt_send.Name = "bt_send";
            this.bt_send.Size = new System.Drawing.Size(75, 37);
            this.bt_send.TabIndex = 14;
            this.bt_send.Text = "发送消息";
            this.bt_send.UseVisualStyleBackColor = true;
            this.bt_send.Click += new System.EventHandler(this.bt_send_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(376, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 16;
            this.label3.Text = "设置昵称：";
            // 
            // tb_name
            // 
            this.tb_name.Location = new System.Drawing.Point(447, 25);
            this.tb_name.Name = "tb_name";
            this.tb_name.Size = new System.Drawing.Size(117, 21);
            this.tb_name.TabIndex = 15;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.rtb_log);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(380, 242);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "消息";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // rtb_log
            // 
            this.rtb_log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb_log.Location = new System.Drawing.Point(3, 3);
            this.rtb_log.Name = "rtb_log";
            this.rtb_log.ReadOnly = true;
            this.rtb_log.Size = new System.Drawing.Size(374, 236);
            this.rtb_log.TabIndex = 12;
            this.rtb_log.Text = "";
            // 
            // tp_news
            // 
            this.tp_news.Controls.Add(this.tabPage1);
            this.tp_news.Location = new System.Drawing.Point(20, 52);
            this.tp_news.Name = "tp_news";
            this.tp_news.SelectedIndex = 0;
            this.tp_news.Size = new System.Drawing.Size(388, 268);
            this.tp_news.TabIndex = 17;
            // 
            // listBox1
            // 
            this.listBox1.Font = new System.Drawing.Font("楷体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 21;
            this.listBox1.Location = new System.Drawing.Point(503, 77);
            this.listBox1.Name = "listBox1";
            this.listBox1.ScrollAlwaysVisible = true;
            this.listBox1.Size = new System.Drawing.Size(145, 214);
            this.listBox1.TabIndex = 18;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(503, 326);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(145, 23);
            this.label4.TabIndex = 19;
            this.label4.Text = "当前聊天对象";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ClientSocket
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(666, 401);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.tp_news);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tb_name);
            this.Controls.Add(this.bt_send);
            this.Controls.Add(this.rtb_sendmsg);
            this.Controls.Add(this.bt_start);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_Point);
            this.Controls.Add(this.tb_IP);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ClientSocket";
            this.Text = "客户端";
            this.tabPage1.ResumeLayout(false);
            this.tp_news.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bt_start;
        private System.Windows.Forms.TextBox tb_Point;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_IP;
        private System.Windows.Forms.RichTextBox rtb_sendmsg;
        private System.Windows.Forms.Button bt_send;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb_name;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.RichTextBox rtb_log;
        private System.Windows.Forms.TabControl tp_news;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label4;
    }
}