namespace 教务管理系统
{
    partial class ServerSocket
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.txt_IP = new System.Windows.Forms.TextBox();
            this.txt_Point = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cb_IP = new System.Windows.Forms.ComboBox();
            this.bt_start = new System.Windows.Forms.Button();
            this.rtb_log = new System.Windows.Forms.RichTextBox();
            this.rtb_sendmsg = new System.Windows.Forms.RichTextBox();
            this.bt_send = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_num = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.聊天管理 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txt_IP
            // 
            this.txt_IP.Location = new System.Drawing.Point(102, 77);
            this.txt_IP.Name = "txt_IP";
            this.txt_IP.Size = new System.Drawing.Size(85, 21);
            this.txt_IP.TabIndex = 0;
            this.txt_IP.Text = "127.0.0.1";
            // 
            // txt_Point
            // 
            this.txt_Point.Location = new System.Drawing.Point(102, 113);
            this.txt_Point.Name = "txt_Point";
            this.txt_Point.Size = new System.Drawing.Size(85, 21);
            this.txt_Point.TabIndex = 1;
            this.txt_Point.Text = "50000";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "IP地址：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(43, 116);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "端口号：";
            // 
            // cb_IP
            // 
            this.cb_IP.FormattingEnabled = true;
            this.cb_IP.Location = new System.Drawing.Point(76, 360);
            this.cb_IP.Name = "cb_IP";
            this.cb_IP.Size = new System.Drawing.Size(121, 20);
            this.cb_IP.TabIndex = 4;
            // 
            // bt_start
            // 
            this.bt_start.Location = new System.Drawing.Point(102, 193);
            this.bt_start.Name = "bt_start";
            this.bt_start.Size = new System.Drawing.Size(75, 23);
            this.bt_start.TabIndex = 5;
            this.bt_start.Text = "启动服务器";
            this.bt_start.UseVisualStyleBackColor = true;
            this.bt_start.Click += new System.EventHandler(this.bt_start_Click);
            // 
            // rtb_log
            // 
            this.rtb_log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb_log.Location = new System.Drawing.Point(3, 17);
            this.rtb_log.Name = "rtb_log";
            this.rtb_log.ReadOnly = true;
            this.rtb_log.Size = new System.Drawing.Size(336, 263);
            this.rtb_log.TabIndex = 6;
            this.rtb_log.Text = "";
            // 
            // rtb_sendmsg
            // 
            this.rtb_sendmsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb_sendmsg.Location = new System.Drawing.Point(3, 17);
            this.rtb_sendmsg.Name = "rtb_sendmsg";
            this.rtb_sendmsg.Size = new System.Drawing.Size(333, 102);
            this.rtb_sendmsg.TabIndex = 7;
            this.rtb_sendmsg.Text = "";
            this.rtb_sendmsg.KeyDown += new System.Windows.Forms.KeyEventHandler(this.rtb_sendmsg_KeyDown);
            // 
            // bt_send
            // 
            this.bt_send.Location = new System.Drawing.Point(470, 458);
            this.bt_send.Name = "bt_send";
            this.bt_send.Size = new System.Drawing.Size(75, 23);
            this.bt_send.TabIndex = 11;
            this.bt_send.Text = "发送信息";
            this.bt_send.UseVisualStyleBackColor = true;
            this.bt_send.Click += new System.EventHandler(this.bt_send_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 158);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 15;
            this.label3.Text = "连接个数：";
            // 
            // tb_num
            // 
            this.tb_num.Location = new System.Drawing.Point(102, 155);
            this.tb_num.Name = "tb_num";
            this.tb_num.Size = new System.Drawing.Size(85, 21);
            this.tb_num.TabIndex = 14;
            this.tb_num.Text = "10";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rtb_log);
            this.groupBox1.Location = new System.Drawing.Point(203, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(342, 283);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "接收消息";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rtb_sendmsg);
            this.groupBox2.Location = new System.Drawing.Point(206, 330);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(339, 122);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "发送消息";
            // 
            // 聊天管理
            // 
            this.聊天管理.AutoSize = true;
            this.聊天管理.Location = new System.Drawing.Point(5, 363);
            this.聊天管理.Name = "聊天管理";
            this.聊天管理.Size = new System.Drawing.Size(65, 12);
            this.聊天管理.TabIndex = 18;
            this.聊天管理.Text = "发送对象：";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(449, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 19;
            this.button1.Text = "打开客户端";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ServerSocket
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 491);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.聊天管理);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tb_num);
            this.Controls.Add(this.bt_send);
            this.Controls.Add(this.bt_start);
            this.Controls.Add(this.cb_IP);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_Point);
            this.Controls.Add(this.txt_IP);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ServerSocket";
            this.Text = "服务端";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_IP;
        private System.Windows.Forms.TextBox txt_Point;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cb_IP;
        private System.Windows.Forms.Button bt_start;
        private System.Windows.Forms.RichTextBox rtb_log;
        private System.Windows.Forms.RichTextBox rtb_sendmsg;
        private System.Windows.Forms.Button bt_send;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb_num;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label 聊天管理;
        private System.Windows.Forms.Button button1;
    }
}

