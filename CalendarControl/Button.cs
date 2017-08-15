using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarControl
{
	internal class Button
	{
		public Calendar Parent { get; set; }
		public Rectangle ClientRectangle { get; set; }
		public Font Font { get; set; }
		public string Text { get; set; }
		public Color ForeColor { get; set; }
		public Color BackColor { get; set; }
        public int Day { get; set; }
		private bool ispush;
		private bool ismousedown;
		private string constext;
		private Font consfont;
		public event EventHandler Click;

		public Button(Calendar parent)
		{
			Parent = parent;
			consfont = new Font("Wingdings 3", Parent.Font.Size, Parent.Font.Style);
			Font = Parent.Font;
			constext = "";
			ForeColor = Parent.ForeColor;
			ClientRectangle = Rectangle.Empty;
			Parent.MouseDown += Parent_MouseDown;
			Parent.MouseUp += Parent_MouseUp;
			ispush = false;
		}


		public void Draw(Graphics g)
		{
			StringFormat format = new StringFormat();
			format.LineAlignment = StringAlignment.Center;
			format.Alignment = StringAlignment.Far;

			if (ispush)
			{
				g.DrawString(constext, consfont, new SolidBrush(Color.Red), ClientRectangle, format);
				g.DrawString(Text, Font, new SolidBrush(Color.Red), new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - (int)g.MeasureString(constext, consfont).Width, ClientRectangle.Height), format);
			}
			else
			{
				g.DrawString(constext, consfont, new SolidBrush(ForeColor), ClientRectangle, format);
				g.DrawString(Text, Font, new SolidBrush(ForeColor), new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - (int)g.MeasureString(constext, consfont).Width, ClientRectangle.Height), format);
			}
		}

		void Parent_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(e.Button == System.Windows.Forms.MouseButtons.Left &&
				ClientRectangle.Contains(e.Location))
			{
				ispush = true;
				Parent.Invalidate(ClientRectangle);
			}
		}
		void Parent_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(e.Button == System.Windows.Forms.MouseButtons.Left &&
				ClientRectangle.Contains(e.Location))
			{
				if (ispush)
				{
					OnClick(EventArgs.Empty);
					ispush = false;
				}
				Parent.Invalidate(ClientRectangle);

			}
		}
		public virtual void OnClick(EventArgs e)
		{
			if (Click != null)
				Click(this, e);
		}


	}
}
