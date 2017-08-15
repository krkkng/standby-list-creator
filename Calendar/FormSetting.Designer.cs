namespace Calendar
{
	partial class FormSetting
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
            this.button1 = new System.Windows.Forms.Button();
            this.lblCalendar = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.num1st = new System.Windows.Forms.NumericUpDown();
            this.numHoliday = new System.Windows.Forms.NumericUpDown();
            this.numInterval = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbAttr = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.calendar1 = new CalendarControl.Calendar();
            this.btnAddRule = new System.Windows.Forms.Button();
            this.dgvRule = new System.Windows.Forms.DataGridView();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.numHolidayInterval = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.num1st)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHoliday)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRule)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHolidayInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(517, 591);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblCalendar
            // 
            this.lblCalendar.AutoSize = true;
            this.lblCalendar.Location = new System.Drawing.Point(11, 106);
            this.lblCalendar.Name = "lblCalendar";
            this.lblCalendar.Size = new System.Drawing.Size(47, 12);
            this.lblCalendar.TabIndex = 2;
            this.lblCalendar.Text = "不可日：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(112, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "休日可能日数";
            // 
            // num1st
            // 
            this.num1st.Location = new System.Drawing.Point(14, 67);
            this.num1st.Name = "num1st";
            this.num1st.Size = new System.Drawing.Size(62, 19);
            this.num1st.TabIndex = 11;
            // 
            // numHoliday
            // 
            this.numHoliday.Location = new System.Drawing.Point(113, 67);
            this.numHoliday.Name = "numHoliday";
            this.numHoliday.Size = new System.Drawing.Size(62, 19);
            this.numHoliday.TabIndex = 13;
            // 
            // numInterval
            // 
            this.numInterval.Location = new System.Drawing.Point(218, 67);
            this.numInterval.Name = "numInterval";
            this.numInterval.Size = new System.Drawing.Size(62, 19);
            this.numInterval.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(217, 52);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 12);
            this.label5.TabIndex = 15;
            this.label5.Text = "最小の間隔";
            // 
            // cmbAttr
            // 
            this.cmbAttr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAttr.FormattingEnabled = true;
            this.cmbAttr.Location = new System.Drawing.Point(238, 12);
            this.cmbAttr.Name = "cmbAttr";
            this.cmbAttr.Size = new System.Drawing.Size(104, 20);
            this.cmbAttr.TabIndex = 16;
            this.cmbAttr.SelectedIndexChanged += new System.EventHandler(this.cmbAttr_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(200, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 12);
            this.label6.TabIndex = 17;
            this.label6.Text = "属性：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 52);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(87, 12);
            this.label7.TabIndex = 18;
            this.label7.Text = "平日の可能日数";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(53, 12);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(110, 19);
            this.txtName.TabIndex = 27;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 15);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(35, 12);
            this.label9.TabIndex = 28;
            this.label9.Text = "名前：";
            // 
            // calendar1
            // 
            this.calendar1.AllowChangeMonth = false;
            this.calendar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.calendar1.BackColor = System.Drawing.SystemColors.Window;
            this.calendar1.BackgroundColor = System.Drawing.Color.Empty;
            this.calendar1.DayFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.calendar1.DayOfWeekFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.calendar1.EnabledColor = System.Drawing.SystemColors.Control;
            this.calendar1.FooterMessage = null;
            this.calendar1.Header = 18;
            this.calendar1.HilightColor = System.Drawing.Color.Lavender;
            this.calendar1.HolidayColor = System.Drawing.Color.Red;
            this.calendar1.LabelHeight = 0;
            this.calendar1.Location = new System.Drawing.Point(9, 290);
            this.calendar1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.calendar1.Name = "calendar1";
            this.calendar1.PrintFooterHeight = 50;
            this.calendar1.PrintHeaderHeight = 50;
            this.calendar1.SaturdayColor = System.Drawing.Color.Blue;
            this.calendar1.ScheduleLabelSpace = 2;
            this.calendar1.Size = new System.Drawing.Size(584, 296);
            this.calendar1.StartWeekday = System.DayOfWeek.Monday;
            this.calendar1.TabIndex = 0;
            this.calendar1.WeekDayColor = System.Drawing.Color.Black;
            this.calendar1.ChangeDate += new System.EventHandler<CalendarControl.Calendar.ChangeDateEventArgs>(this.calendar1_ChangeDate);
            this.calendar1.ChangeDrawDate += new System.EventHandler<CalendarControl.Calendar.ChangeDateEventArgs>(this.calendar1_ChangeDrawDate);
            this.calendar1.ScheduleLabelMouseUp += new System.EventHandler<CalendarControl.Calendar.ScheduleLabelMouseEventArgs>(this.calendar1_ScheduleLabelMouseUp);
            this.calendar1.DoubleClick += new System.EventHandler(this.calendar1_DoubleClick);
            // 
            // btnAddRule
            // 
            this.btnAddRule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddRule.Location = new System.Drawing.Point(532, 260);
            this.btnAddRule.Name = "btnAddRule";
            this.btnAddRule.Size = new System.Drawing.Size(61, 23);
            this.btnAddRule.TabIndex = 31;
            this.btnAddRule.Text = "条件追加";
            this.btnAddRule.UseVisualStyleBackColor = true;
            this.btnAddRule.Click += new System.EventHandler(this.btnAddRule_Click);
            // 
            // dgvRule
            // 
            this.dgvRule.AllowUserToAddRows = false;
            this.dgvRule.AllowUserToDeleteRows = false;
            this.dgvRule.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvRule.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRule.Location = new System.Drawing.Point(9, 121);
            this.dgvRule.MultiSelect = false;
            this.dgvRule.Name = "dgvRule";
            this.dgvRule.ReadOnly = true;
            this.dgvRule.RowHeadersVisible = false;
            this.dgvRule.RowTemplate.Height = 21;
            this.dgvRule.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRule.Size = new System.Drawing.Size(517, 162);
            this.dgvRule.TabIndex = 32;
            this.dgvRule.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRule_CellDoubleClick);
            this.dgvRule.SelectionChanged += new System.EventHandler(this.dgvRule_SelectionChanged);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Location = new System.Drawing.Point(532, 231);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(61, 23);
            this.btnDelete.TabIndex = 33;
            this.btnDelete.Text = "削除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEdit.Location = new System.Drawing.Point(532, 202);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(61, 23);
            this.btnEdit.TabIndex = 34;
            this.btnEdit.Text = "編集";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // numHolidayInterval
            // 
            this.numHolidayInterval.Location = new System.Drawing.Point(319, 67);
            this.numHolidayInterval.Name = "numHolidayInterval";
            this.numHolidayInterval.Size = new System.Drawing.Size(62, 19);
            this.numHolidayInterval.TabIndex = 35;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(317, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 12);
            this.label1.TabIndex = 36;
            this.label1.Text = "休日当直の最小間隔";
            // 
            // FormSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(605, 624);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numHolidayInterval);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.dgvRule);
            this.Controls.Add(this.btnAddRule);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cmbAttr);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.numInterval);
            this.Controls.Add(this.numHoliday);
            this.Controls.Add(this.num1st);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblCalendar);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.calendar1);
            this.Name = "FormSetting";
            this.Text = "FormSetting";
            this.Load += new System.EventHandler(this.FormSetting_Load);
            ((System.ComponentModel.ISupportInitialize)(this.num1st)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHoliday)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRule)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHolidayInterval)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private CalendarControl.Calendar calendar1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label lblCalendar;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.NumericUpDown num1st;
		private System.Windows.Forms.NumericUpDown numHoliday;
		private System.Windows.Forms.NumericUpDown numInterval;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox cmbAttr;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnAddRule;
        private System.Windows.Forms.DataGridView dgvRule;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.NumericUpDown numHolidayInterval;
        private System.Windows.Forms.Label label1;
    }
}