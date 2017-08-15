namespace Calendar
{
    partial class FormRule
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
            this.cmbWeekNo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbDayOfWeek = new System.Windows.Forms.ComboBox();
            this.txtReason = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.cmbInterval = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.chkUseEnd = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radio4 = new System.Windows.Forms.RadioButton();
            this.radio3 = new System.Windows.Forms.RadioButton();
            this.radio2 = new System.Windows.Forms.RadioButton();
            this.radio1 = new System.Windows.Forms.RadioButton();
            this.chkHoliday = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioDayAndNight1 = new System.Windows.Forms.RadioButton();
            this.radioNight1 = new System.Windows.Forms.RadioButton();
            this.radioDay1 = new System.Windows.Forms.RadioButton();
            this.radioNone1 = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radioDayAndNight2 = new System.Windows.Forms.RadioButton();
            this.radioNight2 = new System.Windows.Forms.RadioButton();
            this.radioDay2 = new System.Windows.Forms.RadioButton();
            this.radioNone2 = new System.Windows.Forms.RadioButton();
            this.radioHD1 = new System.Windows.Forms.RadioButton();
            this.radioHD2 = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbWeekNo
            // 
            this.cmbWeekNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWeekNo.FormattingEnabled = true;
            this.cmbWeekNo.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "毎週"});
            this.cmbWeekNo.Location = new System.Drawing.Point(64, 12);
            this.cmbWeekNo.Name = "cmbWeekNo";
            this.cmbWeekNo.Size = new System.Drawing.Size(47, 20);
            this.cmbWeekNo.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "第";
            // 
            // cmbDayOfWeek
            // 
            this.cmbDayOfWeek.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbDayOfWeek.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDayOfWeek.FormattingEnabled = true;
            this.cmbDayOfWeek.Items.AddRange(new object[] {
            "日曜日",
            "月曜日",
            "火曜日",
            "水曜日",
            "木曜日",
            "金曜日",
            "土曜日",
            "土・日曜日"});
            this.cmbDayOfWeek.Location = new System.Drawing.Point(117, 12);
            this.cmbDayOfWeek.Name = "cmbDayOfWeek";
            this.cmbDayOfWeek.Size = new System.Drawing.Size(94, 20);
            this.cmbDayOfWeek.TabIndex = 2;
            this.cmbDayOfWeek.SelectedIndexChanged += new System.EventHandler(this.cmbDayOfWeek_SelectedIndexChanged);
            // 
            // txtReason
            // 
            this.txtReason.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtReason.Location = new System.Drawing.Point(64, 114);
            this.txtReason.Name = "txtReason";
            this.txtReason.Size = new System.Drawing.Size(147, 19);
            this.txtReason.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 117);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "勤務先";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(256, 259);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // cmbInterval
            // 
            this.cmbInterval.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbInterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbInterval.FormattingEnabled = true;
            this.cmbInterval.Items.AddRange(new object[] {
            "毎月",
            "1ヶ月",
            "2ヶ月",
            "3ヶ月",
            "4ヶ月",
            "5ヶ月",
            "6ヶ月",
            "7ヶ月",
            "8ヶ月",
            "9ヶ月",
            "10ヶ月",
            "11ヶ月",
            "12ヶ月"});
            this.cmbInterval.Location = new System.Drawing.Point(64, 35);
            this.cmbInterval.Name = "cmbInterval";
            this.cmbInterval.Size = new System.Drawing.Size(147, 20);
            this.cmbInterval.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(29, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "間隔";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(29, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "開始";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(64, 64);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(147, 19);
            this.dateTimePicker1.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(29, 91);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "終了";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePicker2.Location = new System.Drawing.Point(64, 89);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(147, 19);
            this.dateTimePicker2.TabIndex = 11;
            this.dateTimePicker2.ValueChanged += new System.EventHandler(this.dateTimePicker2_ValueChanged);
            this.dateTimePicker2.Validating += new System.ComponentModel.CancelEventHandler(this.dateTimePicker2_Validating);
            // 
            // chkUseEnd
            // 
            this.chkUseEnd.AutoSize = true;
            this.chkUseEnd.Location = new System.Drawing.Point(12, 91);
            this.chkUseEnd.Name = "chkUseEnd";
            this.chkUseEnd.Size = new System.Drawing.Size(15, 14);
            this.chkUseEnd.TabIndex = 12;
            this.chkUseEnd.UseVisualStyleBackColor = true;
            this.chkUseEnd.CheckedChanged += new System.EventHandler(this.chkUseEnd_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.radio4);
            this.groupBox1.Controls.Add(this.radio3);
            this.groupBox1.Controls.Add(this.radio2);
            this.groupBox1.Controls.Add(this.radio1);
            this.groupBox1.Location = new System.Drawing.Point(229, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(103, 111);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "追加の不都合日";
            // 
            // radio4
            // 
            this.radio4.AutoSize = true;
            this.radio4.Location = new System.Drawing.Point(6, 85);
            this.radio4.Name = "radio4";
            this.radio4.Size = new System.Drawing.Size(47, 16);
            this.radio4.TabIndex = 19;
            this.radio4.TabStop = true;
            this.radio4.Text = "両方";
            this.radio4.UseVisualStyleBackColor = true;
            // 
            // radio3
            // 
            this.radio3.AutoSize = true;
            this.radio3.Location = new System.Drawing.Point(6, 63);
            this.radio3.Name = "radio3";
            this.radio3.Size = new System.Drawing.Size(47, 16);
            this.radio3.TabIndex = 18;
            this.radio3.TabStop = true;
            this.radio3.Text = "翌日";
            this.radio3.UseVisualStyleBackColor = true;
            // 
            // radio2
            // 
            this.radio2.AutoSize = true;
            this.radio2.Location = new System.Drawing.Point(6, 41);
            this.radio2.Name = "radio2";
            this.radio2.Size = new System.Drawing.Size(47, 16);
            this.radio2.TabIndex = 17;
            this.radio2.TabStop = true;
            this.radio2.Text = "前日";
            this.radio2.UseVisualStyleBackColor = true;
            // 
            // radio1
            // 
            this.radio1.AutoSize = true;
            this.radio1.Location = new System.Drawing.Point(6, 19);
            this.radio1.Name = "radio1";
            this.radio1.Size = new System.Drawing.Size(42, 16);
            this.radio1.TabIndex = 16;
            this.radio1.TabStop = true;
            this.radio1.Text = "なし";
            this.radio1.UseVisualStyleBackColor = true;
            // 
            // chkHoliday
            // 
            this.chkHoliday.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkHoliday.AutoSize = true;
            this.chkHoliday.Location = new System.Drawing.Point(235, 129);
            this.chkHoliday.Name = "chkHoliday";
            this.chkHoliday.Size = new System.Drawing.Size(76, 16);
            this.chkHoliday.TabIndex = 17;
            this.chkHoliday.Text = "休日は除く";
            this.chkHoliday.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioHD1);
            this.groupBox2.Controls.Add(this.radioDayAndNight1);
            this.groupBox2.Controls.Add(this.radioNight1);
            this.groupBox2.Controls.Add(this.radioDay1);
            this.groupBox2.Controls.Add(this.radioNone1);
            this.groupBox2.Location = new System.Drawing.Point(12, 149);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(319, 48);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "勤務(1日目)";
            // 
            // radioDayAndNight1
            // 
            this.radioDayAndNight1.AutoSize = true;
            this.radioDayAndNight1.Location = new System.Drawing.Point(173, 18);
            this.radioDayAndNight1.Name = "radioDayAndNight1";
            this.radioDayAndNight1.Size = new System.Drawing.Size(59, 16);
            this.radioDayAndNight1.TabIndex = 3;
            this.radioDayAndNight1.TabStop = true;
            this.radioDayAndNight1.Text = "日当直";
            this.radioDayAndNight1.UseVisualStyleBackColor = true;
            // 
            // radioNight1
            // 
            this.radioNight1.AutoSize = true;
            this.radioNight1.Location = new System.Drawing.Point(120, 18);
            this.radioNight1.Name = "radioNight1";
            this.radioNight1.Size = new System.Drawing.Size(47, 16);
            this.radioNight1.TabIndex = 2;
            this.radioNight1.TabStop = true;
            this.radioNight1.Text = "当直";
            this.radioNight1.UseVisualStyleBackColor = true;
            // 
            // radioDay1
            // 
            this.radioDay1.AutoSize = true;
            this.radioDay1.Location = new System.Drawing.Point(67, 18);
            this.radioDay1.Name = "radioDay1";
            this.radioDay1.Size = new System.Drawing.Size(47, 16);
            this.radioDay1.TabIndex = 1;
            this.radioDay1.TabStop = true;
            this.radioDay1.Text = "日直";
            this.radioDay1.UseVisualStyleBackColor = true;
            // 
            // radioNone1
            // 
            this.radioNone1.AutoSize = true;
            this.radioNone1.Location = new System.Drawing.Point(19, 18);
            this.radioNone1.Name = "radioNone1";
            this.radioNone1.Size = new System.Drawing.Size(42, 16);
            this.radioNone1.TabIndex = 0;
            this.radioNone1.TabStop = true;
            this.radioNone1.Text = "なし";
            this.radioNone1.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.radioHD2);
            this.groupBox3.Controls.Add(this.radioDayAndNight2);
            this.groupBox3.Controls.Add(this.radioNight2);
            this.groupBox3.Controls.Add(this.radioDay2);
            this.groupBox3.Controls.Add(this.radioNone2);
            this.groupBox3.Location = new System.Drawing.Point(12, 203);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(320, 48);
            this.groupBox3.TabIndex = 19;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "勤務(2日目)";
            // 
            // radioDayAndNight2
            // 
            this.radioDayAndNight2.AutoSize = true;
            this.radioDayAndNight2.Location = new System.Drawing.Point(173, 18);
            this.radioDayAndNight2.Name = "radioDayAndNight2";
            this.radioDayAndNight2.Size = new System.Drawing.Size(59, 16);
            this.radioDayAndNight2.TabIndex = 3;
            this.radioDayAndNight2.TabStop = true;
            this.radioDayAndNight2.Text = "日当直";
            this.radioDayAndNight2.UseVisualStyleBackColor = true;
            // 
            // radioNight2
            // 
            this.radioNight2.AutoSize = true;
            this.radioNight2.Location = new System.Drawing.Point(120, 18);
            this.radioNight2.Name = "radioNight2";
            this.radioNight2.Size = new System.Drawing.Size(47, 16);
            this.radioNight2.TabIndex = 2;
            this.radioNight2.TabStop = true;
            this.radioNight2.Text = "当直";
            this.radioNight2.UseVisualStyleBackColor = true;
            // 
            // radioDay2
            // 
            this.radioDay2.AutoSize = true;
            this.radioDay2.Location = new System.Drawing.Point(67, 18);
            this.radioDay2.Name = "radioDay2";
            this.radioDay2.Size = new System.Drawing.Size(47, 16);
            this.radioDay2.TabIndex = 1;
            this.radioDay2.TabStop = true;
            this.radioDay2.Text = "日直";
            this.radioDay2.UseVisualStyleBackColor = true;
            // 
            // radioNone2
            // 
            this.radioNone2.AutoSize = true;
            this.radioNone2.Location = new System.Drawing.Point(19, 18);
            this.radioNone2.Name = "radioNone2";
            this.radioNone2.Size = new System.Drawing.Size(42, 16);
            this.radioNone2.TabIndex = 0;
            this.radioNone2.TabStop = true;
            this.radioNone2.Text = "なし";
            this.radioNone2.UseVisualStyleBackColor = true;
            // 
            // radioHD1
            // 
            this.radioHD1.AutoSize = true;
            this.radioHD1.Location = new System.Drawing.Point(238, 18);
            this.radioHD1.Name = "radioHD1";
            this.radioHD1.Size = new System.Drawing.Size(47, 16);
            this.radioHD1.TabIndex = 4;
            this.radioHD1.TabStop = true;
            this.radioHD1.Text = "透析";
            this.radioHD1.UseVisualStyleBackColor = true;
            // 
            // radioHD2
            // 
            this.radioHD2.AutoSize = true;
            this.radioHD2.Location = new System.Drawing.Point(238, 18);
            this.radioHD2.Name = "radioHD2";
            this.radioHD2.Size = new System.Drawing.Size(47, 16);
            this.radioHD2.TabIndex = 4;
            this.radioHD2.TabStop = true;
            this.radioHD2.Text = "透析";
            this.radioHD2.UseVisualStyleBackColor = true;
            // 
            // FormRule
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(343, 290);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.chkHoliday);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chkUseEnd);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbInterval);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtReason);
            this.Controls.Add(this.cmbDayOfWeek);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbWeekNo);
            this.Name = "FormRule";
            this.Text = "不都合日の条件";
            this.Load += new System.EventHandler(this.FormRule_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbWeekNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbDayOfWeek;
        private System.Windows.Forms.TextBox txtReason;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ComboBox cmbInterval;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.CheckBox chkUseEnd;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radio4;
        private System.Windows.Forms.RadioButton radio3;
        private System.Windows.Forms.RadioButton radio2;
        private System.Windows.Forms.RadioButton radio1;
        private System.Windows.Forms.CheckBox chkHoliday;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioDayAndNight1;
        private System.Windows.Forms.RadioButton radioNight1;
        private System.Windows.Forms.RadioButton radioDay1;
        private System.Windows.Forms.RadioButton radioNone1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton radioDayAndNight2;
        private System.Windows.Forms.RadioButton radioNight2;
        private System.Windows.Forms.RadioButton radioDay2;
        private System.Windows.Forms.RadioButton radioNone2;
        private System.Windows.Forms.RadioButton radioHD1;
        private System.Windows.Forms.RadioButton radioHD2;
    }
}