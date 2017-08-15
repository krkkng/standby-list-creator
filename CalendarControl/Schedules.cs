using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace CalendarControl
{
	public class Schedule
	{
		public struct Repeater
		{
			public enum RepeatFlag
			{
				None,
				Day,
				Week,
				Month,
				Year
			}
			public int Times { get; set; }
			public RepeatFlag Flag { get; set; }
		}
		private object item;
		public event EventHandler ItemChanged;
		public DateTime Start { get; set; }
		public DateTime End { get; set; }
		public object Item
		{
			get { return item; }
			set { 
				item = value;
				OnItemChanged(EventArgs.Empty);
			}
		}
		public ScheduleLabel ParentLabel { get; set; }
		protected virtual void OnItemChanged(EventArgs e)
		{
			if(ItemChanged != null)
			{
				ItemChanged(this, e);
			}
		}
		public string Description { get; set; }
		public Repeater RepeatSetting { get; set; }
        private Color forecolor;
        public Color ForeColor
        {
            set
            {
                forecolor = value;
                if(this.ParentLabel != null)
                    this.ParentLabel.Invalidate();
            }
            get
            {
                return forecolor;
            }
        }
        private Font font;
        public Font Font
        {
            set
            {
                font = value;
                if(this.ParentLabel != null)
                    this.ParentLabel.Invalidate();
            }
            get
            {
                return font;
            }
        }
        private Color backcolor;
        public Color BackColor
        {
            set
            {
                backcolor = value;
                if(this.ParentLabel != null)
                    this.ParentLabel.Invalidate();
            }
            get
            {
                return backcolor;
            }
        }
        private Color bordercolor;
        public Color BorderColor
        {
            set
            {
                bordercolor = value;
                if(this.ParentLabel != null)
                    this.ParentLabel.Invalidate();
            }
            get
            {
                return bordercolor;
            }
        }
        private StringAlignment alignment;
        public StringAlignment Alignment
        {
            set
            {
                alignment = value;
                if (this.ParentLabel != null)
                    this.ParentLabel.Invalidate();
            }
            get
            {
                return alignment;
            }
        }
        private StringAlignment linealignment;
        public StringAlignment LineAlignment
        {
            set
            {
                linealignment = value;
                if (this.ParentLabel != null)
                    this.ParentLabel.Invalidate();
            }
            get
            {
                return linealignment;
            }
        }

        public Schedule()
		{
			Start = DateTime.MinValue;
			End = DateTime.MinValue;
			ParentLabel = null;
            ForeColor = Color.Black;
            BackColor = Color.LightPink;
            BorderColor = Color.White;
            Alignment = StringAlignment.Near;
            LineAlignment = StringAlignment.Center;
            Font = new Font("MS UI Gothic", 9);
        }

        public override bool Equals(object obj)
		{
			if (obj == null) return false;
			Schedule other = obj as Schedule;
			if (other == null) return false;
			return this.Item == other.Item &&
				this.Start == other.Start &&
				this.End == other.End &&
				this.Description == other.Description;
		}

		public override int GetHashCode()
		{
			return Item.GetHashCode() ^ Start.GetHashCode() ^ End.GetHashCode() ^ Description.GetHashCode();
		}

		public static bool operator == (Schedule a, Schedule b)
		{
			if (ReferenceEquals(a, b)) return true;
			if ((object)a == null || (object)b == null) return false;
			return a.Equals(b);
		}

		public static bool operator != (Schedule a, Schedule b)
		{
			if (ReferenceEquals(a, b)) return false;
			return !a.Equals(b);
		}
	}
}
