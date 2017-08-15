using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace CalendarControl
{
	public class ScheduleLabel
	{
		public Calendar Parent { get; set; }
		public Point Location { get; private set; }
		public Rectangle ClientRectangle { get; set; }
		public Schedule Schedule { get; set; }
		public bool IsSelected { get; private set; }

        public event EventHandler<MouseEventArgs> MouseUp;
		public event EventHandler<MouseEventArgs> MouseDoubleClick;
		public event EventHandler<MouseEventArgs> MouseMove;
		public event EventHandler<KeyEventArgs> KeyDown;
		public event EventHandler SelectionChanged;
		public event EventHandler Leave;

		public ScheduleLabel(Calendar parent, Schedule schedule)
		{
			Parent = parent;
			ClientRectangle = Rectangle.Empty;
			Schedule = schedule;
			schedule.ParentLabel = this;
			Schedule.ItemChanged += Schedule_ItemChanged;
			IsSelected = false;
			Parent.MouseUp -= Parent_MouseUp;
			Parent.MouseUp += Parent_MouseUp;
			//Parent.MouseDoubleClick -= Parent_MouseDoubleClick;
			Parent.MouseDoubleClick += Parent_MouseDoubleClick;
			Parent.KeyDown += Parent_KeyDown;

		}

		void Parent_KeyDown(object sender, KeyEventArgs e)
		{
			if (this.IsSelected)
				OnKeyDown(e);
		}

		void Schedule_ItemChanged(object sender, EventArgs e)
		{
			Parent.Invalidate(Rectangle.Round(this.ClientRectangle));
		}

		void Parent_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if(ClientRectangle != Rectangle.Empty &&
				e.X > ClientRectangle.X && e.Y > ClientRectangle.Y &&
				e.X < ClientRectangle.X + ClientRectangle.Width &&
				e.Y < ClientRectangle.Y + ClientRectangle.Height)
			{
				Point point = new Point(e.X - this.Location.X, e.Y - this.Location.Y);
				OnMouseDoubleClick(new MouseEventArgs(e.Button, e.Clicks, point.X, point.Y, e.Delta));
			}
		}

		private void OnMouseDoubleClick(MouseEventArgs e)
		{
			if (MouseDoubleClick != null)
				MouseDoubleClick(this, e);
		}

		protected virtual void OnSelectionChanged(EventArgs e)
		{
			if (SelectionChanged != null)
				SelectionChanged(this, e);
		}

		protected virtual void OnLeave(EventArgs e)
		{
			if (Leave != null)
				Leave(this, e);
		}

		protected virtual void OnKeyDown(KeyEventArgs e)
		{
			if (KeyDown != null)
				KeyDown(this, e);
		}
		void Parent_MouseUp(object sender, MouseEventArgs e)
		{
				if (ClientRectangle != Rectangle.Empty &&
					e.X > ClientRectangle.X && e.Y > ClientRectangle.Y &&
					e.X < ClientRectangle.X + ClientRectangle.Width &&
					e.Y < ClientRectangle.Y + ClientRectangle.Height)
				{
					Point point = new Point(e.X - this.Location.X, e.Y - this.Location.Y);

					OnMouseUp(new MouseEventArgs(e.Button, e.Clicks, point.X, point.Y, e.Delta));
				}
			
		}

		public void SetSelected(bool b)
		{
			IsSelected = b;
			Parent.Invalidate(Rectangle.Round(this.ClientRectangle));
			OnSelectionChanged(EventArgs.Empty);
		}

		public virtual void OnMouseUp(MouseEventArgs e)
		{
			SetSelected(true);
			Parent.Invalidate(Rectangle.Round(this.ClientRectangle));
			if (MouseUp != null)
				MouseUp(this, e);
		}

		public void Invalidate()
		{
			Parent.Invalidate(Rectangle.Round(this.ClientRectangle));
		}
		public void Paint(Graphics g, bool isprint = false)
		{
			StringFormat format = new StringFormat();
			format.LineAlignment = Schedule.LineAlignment;
			format.Alignment = Schedule.Alignment;
			format.Trimming = StringTrimming.EllipsisCharacter;
			format.FormatFlags = StringFormatFlags.NoWrap;

			if (IsSelected && !isprint)
			{
				g.FillRectangle(SystemBrushes.ActiveCaption, ClientRectangle);
			}
			else
			{
				g.FillRectangle(new SolidBrush(Schedule.BackColor), ClientRectangle);
			}
			string text = "";
			if (Schedule.Item != null)
				text = Schedule.Item.ToString();
			g.DrawString(text, Parent.Font, new SolidBrush(Schedule.ForeColor), ClientRectangle, format);

			g.DrawRectangle(new Pen(Schedule.BorderColor), Rectangle.Round(ClientRectangle));
		}

		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			ScheduleLabel other = obj as ScheduleLabel;
			if (other == null) return false;
			return this.Schedule == other.Schedule && this.ClientRectangle == other.ClientRectangle;
		}

		public override int GetHashCode()
		{
			return this.Schedule.GetHashCode() ^ this.ClientRectangle.GetHashCode();
		}

		public static bool operator == (ScheduleLabel a, ScheduleLabel b)
		{
			if (ReferenceEquals(a, b)) return true;
			if(((object)a == null) || ((object)b == null)) return false;
			return a.Equals(b);
		}

		public static bool operator != (ScheduleLabel a, ScheduleLabel b)
		{
			if (ReferenceEquals(a, b)) return false;
			return !a.Equals(b);
		}

		public void Print(Graphics g)
		{
			StringFormat format = new StringFormat();
			format.LineAlignment = Schedule.LineAlignment;
			format.Alignment = Schedule.Alignment;
			format.Trimming = StringTrimming.EllipsisCharacter;
			format.FormatFlags = StringFormatFlags.NoWrap;

			g.FillRectangle(new SolidBrush(Schedule.BackColor), ClientRectangle);

			string text = "";
			if (Schedule.Item != null)
				text = Schedule.Item.ToString();
			g.DrawString(text, Parent.Font, new SolidBrush(Schedule.ForeColor), ClientRectangle, format);

			g.DrawRectangle(Pens.White, Rectangle.Round(ClientRectangle));
		}
	}
}
