namespace 教务管理系统
{
    partial class aichat
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
            this.rtb_chat = new System.Windows.Forms.RichTextBox();
            this.rtb_sendmsg = new System.Windows.Forms.RichTextBox();
            this.bt_send = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_api = new System.Windows.Forms.TextBox();
            this.bt_validate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rtb_chat
            // 
            this.rtb_chat.Location = new System.Drawing.Point(46, 103);
            this.rtb_chat.Name = "rtb_chat";
            this.rtb_chat.ReadOnly = true;
            this.rtb_chat.Size = new System.Drawing.Size(374, 236);
            this.rtb_chat.TabIndex = 15;
            this.rtb_chat.Text = "";
            // 
            // rtb_sendmsg
            // 
            this.rtb_sendmsg.Location = new System.Drawing.Point(46, 373);
            this.rtb_sendmsg.Name = "rtb_sendmsg";
            this.rtb_sendmsg.Size = new System.Drawing.Size(374, 78);
            this.rtb_sendmsg.TabIndex = 16;
            this.rtb_sendmsg.Text = "";
            // 
            // bt_send
            // 
            this.bt_send.Location = new System.Drawing.Point(449, 413);
            this.bt_send.Name = "bt_send";
            this.bt_send.Size = new System.Drawing.Size(78, 38);
            this.bt_send.TabIndex = 17;
            this.bt_send.Text = "发送";
            this.bt_send.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(16, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 18;
            this.label1.Text = "API";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tb_api
            // 
            this.tb_api.Location = new System.Drawing.Point(87, 57);
            this.tb_api.Name = "tb_api";
            this.tb_api.Size = new System.Drawing.Size(333, 21);
            this.tb_api.TabIndex = 19;
            // 
            // bt_validate
            // 
            this.bt_validate.Location = new System.Drawing.Point(449, 57);
            this.bt_validate.Name = "bt_validate";
            this.bt_validate.Size = new System.Drawing.Size(75, 23);
            this.bt_validate.TabIndex = 20;
            this.bt_validate.Text = "验证密钥";
            this.bt_validate.UseVisualStyleBackColor = true;
            // 
            // aichat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 496);
            this.Controls.Add(this.bt_validate);
            this.Controls.Add(this.tb_api);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bt_send);
            this.Controls.Add(this.rtb_sendmsg);
            this.Controls.Add(this.rtb_chat);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "aichat";
            this.Text = "AI聊天";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RichTextBox rtb_chat;
        private System.Windows.Forms.RichTextBox rtb_sendmsg;
        private System.Windows.Forms.Button bt_send;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_api;
        private System.Windows.Forms.Button bt_validate;
    }
}