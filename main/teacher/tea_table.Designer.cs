namespace 教务管理系统
{
    partial class tea_table
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(tea_table));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.添加信息toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.删除信息toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.修改信息toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.teacher_number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.teacher_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.teacher_password = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.teacher_gender = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.teacher_title = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.teacher_department = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.teacher_birthday = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.teacher_phone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.teacher_email = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.teacher_professional_field = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.teacher_education_level = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.teacher_work_start_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加信息toolStripButton1,
            this.删除信息toolStripButton2,
            this.修改信息toolStripButton3});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(984, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // 添加信息toolStripButton1
            // 
            this.添加信息toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.添加信息toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("添加信息toolStripButton1.Image")));
            this.添加信息toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.添加信息toolStripButton1.Name = "添加信息toolStripButton1";
            this.添加信息toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.添加信息toolStripButton1.Text = "添加信息";
            this.添加信息toolStripButton1.Click += new System.EventHandler(this.添加信息toolStripButton1_Click);
            // 
            // 删除信息toolStripButton2
            // 
            this.删除信息toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.删除信息toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("删除信息toolStripButton2.Image")));
            this.删除信息toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.删除信息toolStripButton2.Name = "删除信息toolStripButton2";
            this.删除信息toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.删除信息toolStripButton2.Text = "删除信息";
            this.删除信息toolStripButton2.Click += new System.EventHandler(this.删除信息toolStripButton2_Click);
            // 
            // 修改信息toolStripButton3
            // 
            this.修改信息toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.修改信息toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("修改信息toolStripButton3.Image")));
            this.修改信息toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.修改信息toolStripButton3.Name = "修改信息toolStripButton3";
            this.修改信息toolStripButton3.Size = new System.Drawing.Size(23, 22);
            this.修改信息toolStripButton3.Text = "修改信息";
            this.修改信息toolStripButton3.Click += new System.EventHandler(this.修改信息toolStripButton3_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("方正标致简体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.teacher_number,
            this.teacher_name,
            this.teacher_password,
            this.teacher_gender,
            this.teacher_title,
            this.teacher_department,
            this.teacher_birthday,
            this.teacher_phone,
            this.teacher_email,
            this.teacher_professional_field,
            this.teacher_education_level,
            this.teacher_work_start_date});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 25);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(984, 536);
            this.dataGridView1.TabIndex = 7;
            this.dataGridView1.TabStop = false;
            this.dataGridView1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView1_CellFormatting);
            // 
            // teacher_number
            // 
            this.teacher_number.DataPropertyName = "teacher_number";
            this.teacher_number.FillWeight = 47.89808F;
            this.teacher_number.HeaderText = "工号";
            this.teacher_number.Name = "teacher_number";
            this.teacher_number.ReadOnly = true;
            // 
            // teacher_name
            // 
            this.teacher_name.DataPropertyName = "teacher_name";
            this.teacher_name.FillWeight = 47.89808F;
            this.teacher_name.HeaderText = "姓名";
            this.teacher_name.Name = "teacher_name";
            this.teacher_name.ReadOnly = true;
            // 
            // teacher_password
            // 
            this.teacher_password.DataPropertyName = "teacher_password";
            this.teacher_password.FillWeight = 47.89808F;
            this.teacher_password.HeaderText = "密码";
            this.teacher_password.Name = "teacher_password";
            this.teacher_password.ReadOnly = true;
            // 
            // teacher_gender
            // 
            this.teacher_gender.DataPropertyName = "teacher_gender";
            this.teacher_gender.FillWeight = 126.5195F;
            this.teacher_gender.HeaderText = "性别";
            this.teacher_gender.Name = "teacher_gender";
            this.teacher_gender.ReadOnly = true;
            // 
            // teacher_title
            // 
            this.teacher_title.DataPropertyName = "teacher_title";
            this.teacher_title.FillWeight = 47.89808F;
            this.teacher_title.HeaderText = "职称";
            this.teacher_title.Name = "teacher_title";
            this.teacher_title.ReadOnly = true;
            // 
            // teacher_department
            // 
            this.teacher_department.DataPropertyName = "teacher_department";
            this.teacher_department.FillWeight = 83.38074F;
            this.teacher_department.HeaderText = "学院";
            this.teacher_department.Name = "teacher_department";
            this.teacher_department.ReadOnly = true;
            // 
            // teacher_birthday
            // 
            this.teacher_birthday.DataPropertyName = "teacher_birthday";
            this.teacher_birthday.FillWeight = 47.89808F;
            this.teacher_birthday.HeaderText = "出生日期";
            this.teacher_birthday.Name = "teacher_birthday";
            this.teacher_birthday.ReadOnly = true;
            // 
            // teacher_phone
            // 
            this.teacher_phone.DataPropertyName = "teacher_phone";
            this.teacher_phone.FillWeight = 47.89808F;
            this.teacher_phone.HeaderText = "电话";
            this.teacher_phone.Name = "teacher_phone";
            this.teacher_phone.ReadOnly = true;
            // 
            // teacher_email
            // 
            this.teacher_email.DataPropertyName = "teacher_email";
            this.teacher_email.FillWeight = 83.38074F;
            this.teacher_email.HeaderText = "邮箱";
            this.teacher_email.Name = "teacher_email";
            this.teacher_email.ReadOnly = true;
            // 
            // teacher_professional_field
            // 
            this.teacher_professional_field.DataPropertyName = "teacher_professional_field";
            this.teacher_professional_field.FillWeight = 83.38074F;
            this.teacher_professional_field.HeaderText = "专业领域";
            this.teacher_professional_field.Name = "teacher_professional_field";
            this.teacher_professional_field.ReadOnly = true;
            // 
            // teacher_education_level
            // 
            this.teacher_education_level.DataPropertyName = "teacher_education_level";
            this.teacher_education_level.FillWeight = 83.38074F;
            this.teacher_education_level.HeaderText = "学历";
            this.teacher_education_level.Name = "teacher_education_level";
            this.teacher_education_level.ReadOnly = true;
            // 
            // teacher_work_start_date
            // 
            this.teacher_work_start_date.DataPropertyName = "teacher_work_start_date";
            this.teacher_work_start_date.FillWeight = 83.38074F;
            this.teacher_work_start_date.HeaderText = "工作开始日期";
            this.teacher_work_start_date.Name = "teacher_work_start_date";
            this.teacher_work_start_date.ReadOnly = true;
            // 
            // tea_table
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "tea_table";
            this.Text = "教师信息";
            this.Load += new System.EventHandler(this.tea_table_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton 添加信息toolStripButton1;
        private System.Windows.Forms.ToolStripButton 删除信息toolStripButton2;
        private System.Windows.Forms.ToolStripButton 修改信息toolStripButton3;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn teacher_number;
        private System.Windows.Forms.DataGridViewTextBoxColumn teacher_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn teacher_password;
        private System.Windows.Forms.DataGridViewTextBoxColumn teacher_gender;
        private System.Windows.Forms.DataGridViewTextBoxColumn teacher_title;
        private System.Windows.Forms.DataGridViewTextBoxColumn teacher_department;
        private System.Windows.Forms.DataGridViewTextBoxColumn teacher_birthday;
        private System.Windows.Forms.DataGridViewTextBoxColumn teacher_phone;
        private System.Windows.Forms.DataGridViewTextBoxColumn teacher_email;
        private System.Windows.Forms.DataGridViewTextBoxColumn teacher_professional_field;
        private System.Windows.Forms.DataGridViewTextBoxColumn teacher_education_level;
        private System.Windows.Forms.DataGridViewTextBoxColumn teacher_work_start_date;
    }
}