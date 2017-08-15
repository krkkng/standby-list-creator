namespace Calendar
{
	partial class AggregationForm
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
            this.dgView1 = new System.Windows.Forms.DataGridView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.基準日ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsCriteriaDateTime = new System.Windows.Forms.ToolStripComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgView1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgView1
            // 
            this.dgView1.AllowUserToAddRows = false;
            this.dgView1.AllowUserToDeleteRows = false;
            this.dgView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgView1.Location = new System.Drawing.Point(0, 24);
            this.dgView1.MultiSelect = false;
            this.dgView1.Name = "dgView1";
            this.dgView1.ReadOnly = true;
            this.dgView1.RowTemplate.Height = 21;
            this.dgView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgView1.Size = new System.Drawing.Size(456, 364);
            this.dgView1.TabIndex = 0;
            this.dgView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgView1_CellContentClick);
            this.dgView1.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgView1_CellContentDoubleClick);
            this.dgView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgView1_CellContentDoubleClick);
            this.dgView1.SelectionChanged += new System.EventHandler(this.dgView1_SelectionChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.基準日ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 1, 0, 1);
            this.menuStrip1.Size = new System.Drawing.Size(456, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 基準日ToolStripMenuItem
            // 
            this.基準日ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsCriteriaDateTime});
            this.基準日ToolStripMenuItem.Name = "基準日ToolStripMenuItem";
            this.基準日ToolStripMenuItem.Size = new System.Drawing.Size(77, 22);
            this.基準日ToolStripMenuItem.Text = "集計の期間";
            // 
            // tsCriteriaDateTime
            // 
            this.tsCriteriaDateTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tsCriteriaDateTime.Name = "tsCriteriaDateTime";
            this.tsCriteriaDateTime.Size = new System.Drawing.Size(182, 23);
            // 
            // AggregationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(456, 388);
            this.Controls.Add(this.dgView1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "AggregationForm";
            this.Text = "集計";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AggregationForm_FormClosing);
            this.Load += new System.EventHandler(this.AggregationForm_Load);
            this.ResizeEnd += new System.EventHandler(this.AggregationForm_ResizeEnd);
            ((System.ComponentModel.ISupportInitialize)(this.dgView1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DataGridView dgView1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 基準日ToolStripMenuItem;
        private System.Windows.Forms.ToolStripComboBox tsCriteriaDateTime;
    }
}