namespace 教务管理系统
{
    partial class cou_table
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(cou_table));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.添加信息toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.删除信息toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.修改信息toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.course_number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.course_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.course_description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.course_credit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.teacher_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.course_department = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.toolStrip1.TabIndex = 4;
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
            this.course_number,
            this.course_name,
            this.course_description,
            this.course_credit,
            this.teacher_name,
            this.course_department});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 25);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(984, 536);
            this.dataGridView1.TabIndex = 6;
            this.dataGridView1.TabStop = false;
            // 
            // course_number
            // 
            this.course_number.DataPropertyName = "course_number";
            this.course_number.HeaderText = "课程编号";
            this.course_number.Name = "course_number";
            this.course_number.ReadOnly = true;
            // 
            // course_name
            // 
            this.course_name.DataPropertyName = "course_name";
            this.course_name.HeaderText = "课程名称";
            this.course_name.Name = "course_name";
            this.course_name.ReadOnly = true;
            // 
            // course_description
            // 
            this.course_description.DataPropertyName = "course_description";
            this.course_description.HeaderText = "课程描述";
            this.course_description.Name = "course_description";
            this.course_description.ReadOnly = true;
            // 
            // course_credit
            // 
            this.course_credit.DataPropertyName = "course_credit";
            this.course_credit.HeaderText = "学分";
            this.course_credit.Name = "course_credit";
            this.course_credit.ReadOnly = true;
            // 
            // teacher_name
            // 
            this.teacher_name.DataPropertyName = "teacher_name";
            this.teacher_name.HeaderText = "授课老师";
            this.teacher_name.Name = "teacher_name";
            this.teacher_name.ReadOnly = true;
            // 
            // course_department
            // 
            this.course_department.DataPropertyName = "course_department";
            this.course_department.HeaderText = "开课学院";
            this.course_department.Name = "course_department";
            this.course_department.ReadOnly = true;
            // 
            // cuo_table
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "cuo_table";
            this.Text = "课程信息";
            this.Load += new System.EventHandler(this.cuo_info_Load);
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
        private System.Windows.Forms.DataGridViewTextBoxColumn course_number;
        private System.Windows.Forms.DataGridViewTextBoxColumn course_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn course_description;
        private System.Windows.Forms.DataGridViewTextBoxColumn course_credit;
        private System.Windows.Forms.DataGridViewTextBoxColumn teacher_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn course_department;
    }
}