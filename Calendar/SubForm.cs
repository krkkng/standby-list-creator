using CalendarControl;
using CalendarControl.GenCalendar;
using StandbyList;
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
	public partial class SubForm : Form
	{
		public Person Person { get; set; }
        public DateTime Date { get; set; }
        public SubFormType Type { get; internal set; }
        public event EventHandler SelectionChanged;

		public SubForm()
		{
			InitializeComponent();
            dgView.MouseDoubleClick += dgView_MouseDoubleClick;
            dgViewSwap.MouseDoubleClick += dgViewSwap_MouseDoubleClick;
            dgViewSwap.SelectionChanged += DgViewSwap_SelectionChanged;
		}

        public enum SubFormType
        {
            Change,
            Swap
        }
        private void DgViewSwap_SelectionChanged(object sender, EventArgs e)
        {
            OnSelectionChanged(e);
        }

        public virtual void OnSelectionChanged(EventArgs e)
        {
            if (SelectionChanged != null)
                SelectionChanged(dgViewSwap, e);
        }

		public void AddRange(List<LimitedPerson> p)
		{
			dgView.Columns.Clear();
			dgView.Columns.Add("医師名", "医師名");
			dgView.Columns.Add("変更", "変更");
			dgView.Columns.Add("メッセージ", "メッセージ");
			dgView.Columns[0].Width = 100;
			dgView.Columns[1].Width = 60;
			dgView.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dgView.Columns[2].Width = 200;
			p.Sort((x, y) => x.LimitStatus - y.LimitStatus);
			DataGridViewRow[] row = new DataGridViewRow[p.Count];
			for(int i = 0; i < p.Count; i++)
			{
				row[i] = new DataGridViewRow();
				row[i].CreateCells(dgView);
				row[i].Cells[0].Value = p[i].Person;
				switch (p[i].LimitStatus)
				{
					case LimitedPerson.Limit.None:
						row[i].Cells[1].Value = "可";
						break;
					case LimitedPerson.Limit.Limited:
						row[i].Cells[1].Value = "制限あり";
						break;
					case LimitedPerson.Limit.Cannot:
						row[i].Cells[1].Value = "不可";
						break;
				}
				row[i].Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
				row[i].Cells[2].Value = "";
				p[i].Messages.ForEach(t => row[i].Cells[2].Value = row[i].Cells[2].Value + "/" + t);
				row[i].Cells[2].Value = row[i].Cells[2].Value.ToString().TrimStart('/');
				row[i].Cells[2].Value = row[i].Cells[2].Value.ToString().TrimEnd('/');
				if (p[i].LimitStatus == LimitedPerson.Limit.Limited)
					row[i].DefaultCellStyle.ForeColor = Color.Gray;
				else if (p[i].LimitStatus == LimitedPerson.Limit.Cannot)
					row[i].DefaultCellStyle.ForeColor = Color.LightGray;
			}
			dgView.Rows.AddRange(row);
		}

        public void AddRange(List<SwapableList> s)
        {
            dgViewSwap.Columns.Clear();
            dgViewSwap.Columns.Add("医師名", "医師名");
            dgViewSwap.Columns.Add("日付", "日付");
            dgViewSwap.Columns.Add("コメント", "コメント");
            dgViewSwap.Columns[2].Width = 200;
            var row = new DataGridViewRow[s.Count];
            for(int i = 0; i < s.Count; i++)
            {
                row[i] = new DataGridViewRow();
                row[i].CreateCells(dgViewSwap);
                row[i].Cells[0].Value = s[i].Person;
                row[i].Cells[1].Value = s[i].Day + "日";
                row[i].Cells[1].Tag = s[i].Day;
                if (HolidayChecker.IsHoliday(Date) && HolidayChecker.IsHoliday(new DateTime(Date.Year, Date.Month, s[i].Day)))
                {
                    row[i].DefaultCellStyle.ForeColor = Color.Red;
                    row[i].Cells[2].Value = "休日同士の交代です";
                }
                else if(!HolidayChecker.IsHoliday(Date) && !HolidayChecker.IsHoliday(new DateTime(Date.Year, Date.Month, s[i].Day)))
                {
                    row[i].DefaultCellStyle.ForeColor = Color.Blue;
                    row[i].Cells[2].Value = "平日同士の交代です";
                }
                else
                {
                    row[i].DefaultCellStyle.ForeColor = Color.Gray;
                    row[i].Cells[2].Value = "休日と平日の交代です";
                }
            }
            dgViewSwap.Rows.AddRange(row);
        }
		private void SubForm_Load(object sender, EventArgs e)
		{
            if (Setting.FormSubFormRect != Rectangle.Empty)
                this.Bounds = Setting.FormSubFormRect;
		}

		private void dgView_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (dgView.SelectedRows[0].Index != -1)
				Person = dgView.SelectedRows[0].Cells[0].Value as Person;
			else
				return;
			if (dgView.SelectedRows[0].Cells[1].Value.ToString() == "不可")
			{
				MessageBox.Show(Person.ToString() + "さんとは交代できません");
				return;
			}
            Type = SubFormType.Change;
			this.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.Close();
		}

        private void dgViewSwap_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (dgViewSwap.SelectedRows[0].Index != -1)
            {
                Person = dgViewSwap.SelectedRows[0].Cells[0].Value as Person;
                Date = new DateTime(Date.Year, Date.Month, (int)dgViewSwap.SelectedRows[0].Cells[1].Tag);
            }
            else
                return;
            Type = SubFormType.Swap;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
        private void dgView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void SubForm_ResizeEnd(object sender, EventArgs e)
        {
            Setting.FormSubFormRect = this.Bounds;
        }
    }
}
