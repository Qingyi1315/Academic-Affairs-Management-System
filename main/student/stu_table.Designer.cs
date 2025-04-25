namespace 教务管理系统
{
    partial class stu_table
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(stu_table));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.添加信息toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.删除信息toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.修改信息toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.student_number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.student_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.student_password = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.student_gender = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.student_class = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.student_major = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.student_department = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.student_birthday = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.student_origin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.student_address = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.student_phone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.student_email = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.student_enrollment_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.student_graduation_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.toolStrip1.TabIndex = 2;
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
            this.添加信息toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // 删除信息toolStripButton2
            // 
            this.删除信息toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.删除信息toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("删除信息toolStripButton2.Image")));
            this.删除信息toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.删除信息toolStripButton2.Name = "删除信息toolStripButton2";
            this.删除信息toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.删除信息toolStripButton2.Text = "删除信息";
            this.删除信息toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // 修改信息toolStripButton3
            // 
            this.修改信息toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.修改信息toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("修改信息toolStripButton3.Image")));
            this.修改信息toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.修改信息toolStripButton3.Name = "修改信息toolStripButton3";
            this.修改信息toolStripButton3.Size = new System.Drawing.Size(23, 22);
            this.修改信息toolStripButton3.Text = "修改信息";
            this.修改信息toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
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
            this.student_number,
            this.student_name,
            this.student_password,
            this.student_gender,
            this.student_class,
            this.student_major,
            this.student_department,
            this.student_birthday,
            this.student_origin,
            this.student_address,
            this.student_phone,
            this.student_email,
            this.student_enrollment_date,
            this.student_graduation_date});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 25);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(984, 536);
            this.dataGridView1.TabIndex = 3;
            this.dataGridView1.TabStop = false;
            this.dataGridView1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView1_CellFormatting);
            // 
            // student_number
            // 
            this.student_number.DataPropertyName = "student_number";
            this.student_number.HeaderText = "学号";
            this.student_number.Name = "student_number";
            this.student_number.ReadOnly = true;
            // 
            // student_name
            // 
            this.student_name.DataPropertyName = "student_name";
            this.student_name.HeaderText = "姓名";
            this.student_name.Name = "student_name";
            this.student_name.ReadOnly = true;
            // 
            // student_password
            // 
            this.student_password.DataPropertyName = "student_password";
            this.student_password.HeaderText = "密码";
            this.student_password.Name = "student_password";
            this.student_password.ReadOnly = true;
            // 
            // student_gender
            // 
            this.student_gender.DataPropertyName = "student_gender";
            this.student_gender.HeaderText = "性别";
            this.student_gender.Name = "student_gender";
            this.student_gender.ReadOnly = true;
            // 
            // student_class
            // 
            this.student_class.DataPropertyName = "student_class";
            this.student_class.HeaderText = "班级";
            this.student_class.Name = "student_class";
            this.student_class.ReadOnly = true;
            // 
            // student_major
            // 
            this.student_major.DataPropertyName = "student_major";
            this.student_major.HeaderText = "专业";
            this.student_major.Name = "student_major";
            this.student_major.ReadOnly = true;
            // 
            // student_department
            // 
            this.student_department.DataPropertyName = "student_department";
            this.student_department.HeaderText = "学院";
            this.student_department.Name = "student_department";
            this.student_department.ReadOnly = true;
            // 
            // student_birthday
            // 
            this.student_birthday.DataPropertyName = "student_birthday";
            this.student_birthday.HeaderText = "出生日期";
            this.student_birthday.Name = "student_birthday";
            this.student_birthday.ReadOnly = true;
            // 
            // student_origin
            // 
            this.student_origin.DataPropertyName = "student_origin";
            this.student_origin.HeaderText = "籍贯";
            this.student_origin.Name = "student_origin";
            this.student_origin.ReadOnly = true;
            // 
            // student_address
            // 
            this.student_address.DataPropertyName = "student_address";
            this.student_address.HeaderText = "地址";
            this.student_address.Name = "student_address";
            this.student_address.ReadOnly = true;
            // 
            // student_phone
            // 
            this.student_phone.DataPropertyName = "student_phone";
            this.student_phone.HeaderText = "电话";
            this.student_phone.Name = "student_phone";
            this.student_phone.ReadOnly = true;
            // 
            // student_email
            // 
            this.student_email.DataPropertyName = "student_email";
            this.student_email.HeaderText = "邮箱";
            this.student_email.Name = "student_email";
            this.student_email.ReadOnly = true;
            // 
            // student_enrollment_date
            // 
            this.student_enrollment_date.DataPropertyName = "student_enrollment_date";
            this.student_enrollment_date.HeaderText = "入学日期";
            this.student_enrollment_date.Name = "student_enrollment_date";
            this.student_enrollment_date.ReadOnly = true;
            // 
            // student_graduation_date
            // 
            this.student_graduation_date.DataPropertyName = "student_graduation_date";
            this.student_graduation_date.HeaderText = "毕业日期";
            this.student_graduation_date.Name = "student_graduation_date";
            this.student_graduation_date.ReadOnly = true;
            // 
            // stu_table
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "stu_table";
            this.Text = "学生信息";
            this.Load += new System.EventHandler(this.stu_table_Load);
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
        private System.Windows.Forms.DataGridViewTextBoxColumn student_number;
        private System.Windows.Forms.DataGridViewTextBoxColumn student_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn student_password;
        private System.Windows.Forms.DataGridViewTextBoxColumn student_gender;
        private System.Windows.Forms.DataGridViewTextBoxColumn student_class;
        private System.Windows.Forms.DataGridViewTextBoxColumn student_major;
        private System.Windows.Forms.DataGridViewTextBoxColumn student_department;
        private System.Windows.Forms.DataGridViewTextBoxColumn student_birthday;
        private System.Windows.Forms.DataGridViewTextBoxColumn student_origin;
        private System.Windows.Forms.DataGridViewTextBoxColumn student_address;
        private System.Windows.Forms.DataGridViewTextBoxColumn student_phone;
        private System.Windows.Forms.DataGridViewTextBoxColumn student_email;
        private System.Windows.Forms.DataGridViewTextBoxColumn student_enrollment_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn student_graduation_date;
    }
}