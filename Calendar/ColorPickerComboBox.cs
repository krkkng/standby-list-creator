using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Calendar
{
	class ColorPickerComboBox : ComboBox
	{
		public ColorPickerComboBox()
		{
			foreach (KnownColor kc in Enum.GetValues(typeof(KnownColor)))
			{
				Color c = Color.FromKnownColor(kc);
				if (!c.IsSystemColor)
				{
					this.Items.Add(c);
				}
			}
			this.SelectedIndex = 0;

		}
		protected override void OnCreateControl()
		{
			if (this.SelectedIndex == -1)
				this.SelectedIndex = 0;
		}

		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			using (SolidBrush sb = new SolidBrush(e.BackColor))
			{
				e.Graphics.FillRectangle(sb, e.Bounds);
			}

			Color c = (Color)this.Items[e.Index];
			using (SolidBrush sbBack = new SolidBrush(c))
			using (SolidBrush sbFore = new SolidBrush(e.ForeColor))
			{
				Rectangle rcColor = new Rectangle(e.Bounds.Left + 2, e.Bounds.Top + 1, e.Bounds.Height - 3, e.Bounds.Height - 3);
				Rectangle rcText = new Rectangle(rcColor.Right + 2, e.Bounds.Top, e.Bounds.Width - rcColor.Width, e.Bounds.Height);

				e.Graphics.FillRectangle(sbBack, rcColor);
				e.Graphics.DrawRectangle(Pens.Black, rcColor);

				StringFormat sf = new StringFormat();
				sf.Alignment = StringAlignment.Near;
				sf.LineAlignment = StringAlignment.Center;
				sf.Trimming = StringTrimming.EllipsisCharacter;
				sf.FormatFlags = StringFormatFlags.NoWrap;

				string text = "";// string.Format("#{0:x2}{1:x2}{2:x2}", c.R, c.G, c.B);
				if (c.IsNamedColor)
				{
					text += " " + c.Name;
				}
				e.Graphics.DrawString(text, e.Font, sbFore, rcText, sf);
			}
		}

	}
}
