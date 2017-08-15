namespace Calendar
{
    partial class FormCondition
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
            this.cmbRepeat = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // cmbRepeat
            // 
            this.cmbRepeat.FormattingEnabled = true;
            this.cmbRepeat.Items.AddRange(new object[] {
            "毎週",
            "毎月"});
            this.cmbRepeat.Location = new System.Drawing.Point(61, 12);
            this.cmbRepeat.Name = "cmbRepeat";
            this.cmbRepeat.Size = new System.Drawing.Size(121, 20);
            this.cmbRepeat.TabIndex = 0;
            // 
            // FormCondition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(323, 310);
            this.Controls.Add(this.cmbRepeat);
            this.Name = "FormCondition";
            this.Text = "FormCondition";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbRepeat;
    }
}