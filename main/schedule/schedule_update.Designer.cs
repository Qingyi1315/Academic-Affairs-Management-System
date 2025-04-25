namespace 教务管理系统.main
{
    partial class schedule_update
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.添加信息toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.删除信息toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.修改信息toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.本周课表toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.下周课表toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.course_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.course_type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.teacher_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.course_location = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.course_start_time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.course_end_time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.toolStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加信息toolStripButton1,
            this.删除信息toolStripButton2,
            this.修改信息toolStripButton3,
            this.本周课表toolStripButton1,
            this.下周课表toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(984, 25);
            this.toolStrip1.TabIndex = 10;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // 添加信息toolStripButton1
            // 
            this.添加信息toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.添加信息toolStripButton1.Image = global::教务管理系统.Properties.Resources.add;
            this.添加信息toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.添加信息toolStripButton1.Name = "添加信息toolStripButton1";
            this.添加信息toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.添加信息toolStripButton1.Text = "添加信息";
            this.添加信息toolStripButton1.Click += new System.EventHandler(this.添加信息toolStripButton1_Click);
            // 
            // 删除信息toolStripButton2
            // 
            this.删除信息toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.删除信息toolStripButton2.Image = global::教务管理系统.Properties.Resources.del;
            this.删除信息toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.删除信息toolStripButton2.Name = "删除信息toolStripButton2";
            this.删除信息toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.删除信息toolStripButton2.Text = "删除信息";
            this.删除信息toolStripButton2.Click += new System.EventHandler(this.删除信息toolStripButton2_Click);
            // 
            // 修改信息toolStripButton3
            // 
            this.修改信息toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.修改信息toolStripButton3.Image = global::教务管理系统.Properties.Resources.alter;
            this.修改信息toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.修改信息toolStripButton3.Name = "修改信息toolStripButton3";
            this.修改信息toolStripButton3.Size = new System.Drawing.Size(23, 22);
            this.修改信息toolStripButton3.Text = "修改信息";
            this.修改信息toolStripButton3.Click += new System.EventHandler(this.修改信息toolStripButton3_Click);
            // 
            // 本周课表toolStripButton1
            // 
            this.本周课表toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.本周课表toolStripButton1.Image = global::教务管理系统.Properties.Resources.week;
            this.本周课表toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.本周课表toolStripButton1.Name = "本周课表toolStripButton1";
            this.本周课表toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.本周课表toolStripButton1.Text = "本周课表";
            this.本周课表toolStripButton1.Click += new System.EventHandler(this.本周课表toolStripButton1_Click);
            // 
            // 下周课表toolStripButton1
            // 
            this.下周课表toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.下周课表toolStripButton1.Image = global::教务管理系统.Properties.Resources.last_week;
            this.下周课表toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.下周课表toolStripButton1.Name = "下周课表toolStripButton1";
            this.下周课表toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.下周课表toolStripButton1.Text = "下周课表";
            this.下周课表toolStripButton1.Click += new System.EventHandler(this.下周课表toolStripButton1_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.dataGridView1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 25);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(984, 536);
            this.tableLayoutPanel1.TabIndex = 11;
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
            this.course_name,
            this.course_type,
            this.teacher_name,
            this.course_location,
            this.course_start_time,
            this.course_end_time});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(984, 422);
            this.dataGridView1.TabIndex = 11;
            // 
            // course_name
            // 
            this.course_name.DataPropertyName = "course_name";
            this.course_name.HeaderText = "课程名称";
            this.course_name.Name = "course_name";
            this.course_name.ReadOnly = true;
            // 
            // course_type
            // 
            this.course_type.DataPropertyName = "course_type";
            this.course_type.HeaderText = "课程类型";
            this.course_type.Name = "course_type";
            this.course_type.ReadOnly = true;
            // 
            // teacher_name
            // 
            this.teacher_name.DataPropertyName = "teacher_name";
            this.teacher_name.HeaderText = "授课老师";
            this.teacher_name.Name = "teacher_name";
            this.teacher_name.ReadOnly = true;
            // 
            // course_location
            // 
            this.course_location.DataPropertyName = "course_location";
            this.course_location.HeaderText = "上课地点";
            this.course_location.Name = "course_location";
            this.course_location.ReadOnly = true;
            // 
            // course_start_time
            // 
            this.course_start_time.DataPropertyName = "course_start_time";
            this.course_start_time.HeaderText = "开始时间";
            this.course_start_time.Name = "course_start_time";
            this.course_start_time.ReadOnly = true;
            // 
            // course_end_time
            // 
            this.course_end_time.DataPropertyName = "course_end_time";
            this.course_end_time.HeaderText = "结束时间";
            this.course_end_time.Name = "course_end_time";
            this.course_end_time.ReadOnly = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("方正标致简体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(3, 431);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(984, 102);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "菜单";
            // 
            // button1
            // 
            this.button1.AutoSize = true;
            this.button1.Location = new System.Drawing.Point(19, 33);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 50);
            this.button1.TabIndex = 0;
            this.button1.Text = "查询教师课表";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // schedule_update
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "schedule_update";
            this.Text = "更改课表";
            this.Load += new System.EventHandler(this.schedule_update_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton 下周课表toolStripButton1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn course_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn course_type;
        private System.Windows.Forms.DataGridViewTextBoxColumn teacher_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn course_location;
        private System.Windows.Forms.DataGridViewTextBoxColumn course_start_time;
        private System.Windows.Forms.DataGridViewTextBoxColumn course_end_time;
        private System.Windows.Forms.ToolStripButton 添加信息toolStripButton1;
        private System.Windows.Forms.ToolStripButton 删除信息toolStripButton2;
        private System.Windows.Forms.ToolStripButton 修改信息toolStripButton3;
        private System.Windows.Forms.ToolStripButton 本周课表toolStripButton1;
    }
}