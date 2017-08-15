using StandbyList;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Calendar
{
	public partial class FormComment : Form
	{
		public Person Person { get; set; }
		public FormComment()
		{
			InitializeComponent();
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			Person = new Person(-1) { Name = txtComment.Text };
			DialogResult = System.Windows.Forms.DialogResult.OK;
			Close();
		}
	}
}
