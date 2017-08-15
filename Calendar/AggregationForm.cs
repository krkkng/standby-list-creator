using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calendar
{
	public partial class AggregationForm : Form
	{
		public DataGridView dgView { get { return dgView1; } }
		public int Year { get; set; }
		public int Month { get; set; }
        public DateTime CriteriaDate { get; internal set; }
		public event EventHandler SelectionChanged;
        public event EventHandler<AggregationIntervalChangedEventArgs> AggregationIntervalChanged;
        private bool IsReloading = false;

		public AggregationForm()
		{
			InitializeComponent();

			Year = DateTime.Now.Year;
			Month = DateTime.Now.Month;
            if (Setting.DateInterval == null)
                CriteriaDate = DateTime.Now;
            else
                CriteriaDate = Setting.DateInterval.Start;
		}

        public void Reload()
        {
            if (IsReloading) return;
            IsReloading = true;

            if(tsCriteriaDateTime.SelectedIndex != -1)
                Setting.DateInterval = tsCriteriaDateTime.Items[tsCriteriaDateTime.SelectedIndex] as DateInterval;
            
            tsCriteriaDateTime.Items.Clear();
            tsCriteriaDateTime.Items.AddRange(Core.StandbyLists.Where(t => new DateTime(t.Year, t.Month, 1) <= new DateTime(Year, Month, 1)).Select(t => new DateInterval(new DateTime(t.Year, t.Month, 1))).ToArray());
            if (tsCriteriaDateTime.Items.Count == 0)
            {
                tsCriteriaDateTime.Items.Add(Setting.DateInterval);
            }

            if (Setting.DateInterval != null)
            {

                tsCriteriaDateTime.SelectedItem = Setting.DateInterval;
            }
            else
            {
                if (tsCriteriaDateTime.Items.Count != 0)
                {
                    tsCriteriaDateTime.SelectedIndex = 0;
                }
            }

            IsReloading = false;
        }
		private void dgView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex == -1) return;
			FormSetting form = new FormSetting { Year = this.Year, Month = this.Month, Person = dgView.SelectedRows[0].Cells[0].Value as StandbyList.Person, StandbyLists = Core.StandbyLists };
			form.ShowDialog();
		}

		private void dgView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{

		}
		
		public virtual void OnSelectionChanged(EventArgs e)
		{
			if (SelectionChanged != null && !IsReloading)
				SelectionChanged(dgView, e);
		}

		private void dgView1_SelectionChanged(object sender, EventArgs e)
		{
			OnSelectionChanged(e);
		}

        private void AggregationForm_Load(object sender, EventArgs e)
        {
            tsCriteriaDateTime.SelectedIndexChanged += TsCriteriaDateTime_SelectedIndexChanged;
            if (Setting.AggregationFormRect != Rectangle.Empty)
                this.Bounds = Setting.AggregationFormRect;
        }

        private void TsCriteriaDateTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            var agg = new AggregationIntervalChangedEventArgs() { DateInterval = tsCriteriaDateTime.Items[tsCriteriaDateTime.SelectedIndex] as DateInterval };
            CriteriaDate = agg.DateInterval.Start;
            OnAggregationIntervalChanged(agg);
        }

        private void AggregationForm_ResizeEnd(object sender, EventArgs e)
        {
            Setting.AggregationFormRect = this.Bounds;
        }

        protected void OnAggregationIntervalChanged(AggregationIntervalChangedEventArgs e)
        {
            if(AggregationIntervalChanged != null)
            {
                e.DateInterval = tsCriteriaDateTime.Items[tsCriteriaDateTime.SelectedIndex] as DateInterval;
                AggregationIntervalChanged(this, e);
            }
        }

        public class AggregationIntervalChangedEventArgs : EventArgs
        {
            public DateInterval DateInterval { get; internal set; }
        }

        public class DateInterval
        {
            public DateTime Start { get; set; }

            public DateInterval(DateTime start)
            {
                Start = start;
            }
            public override string ToString()
            {
                return string.Format("{0}年{1}月～", Start.Year, Start.Month);
            }
            public override bool Equals(object obj)
            {
                if (obj == null)
                    return false;
                var p = obj as DateInterval;
                if ((Object)p == null)
                    return false;
                return Start == p.Start;
            }

            public bool Equals(DateInterval p)
            {
                if ((object)p == null)
                    return false;
                return Start == p.Start;
            }

            public override int GetHashCode()
            {
                return Start.GetHashCode();
            }

            public static bool operator ==(DateInterval a, DateInterval b)
            {
                if (object.ReferenceEquals(a, b))
                    return true;
                if (((object)a == null) || ((object)b == null))
                    return false;
                return a.Start == b.Start;
            }
            public static bool operator !=(DateInterval a, DateInterval b)
            {
                return !(a == b);
            }
        }

        private void AggregationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.FormOwnerClosing)
                this.Close();
            else
            {
                this.Hide();
                e.Cancel = true;
            }
        }
    }
}
