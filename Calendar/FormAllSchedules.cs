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
	public partial class FormAllSchedules : Form
	{
		public int Year { get; set; }
		public int Month { get; set; }
		public StandbyList.StandbyList StandbyList { get; set; }

		public FormAllSchedules()
		{
			InitializeComponent();
			Year = DateTime.MinValue.Year;
			Month = DateTime.MinValue.Month;
			StandbyList = null;
		}

		private void FormAllSchedules_Load(object sender, EventArgs e)
		{
			if (StandbyList == null ||
				(Year == DateTime.MinValue.Year && Month == DateTime.MinValue.Month))
			{
				Close(); // フォームの初期化が不十分なため終了。
				return;
			}
			Text = string.Format("{0}年{1}月の不都合日設定", Year, Month);
			dgView.Columns.Clear();
			dgView.Rows.Clear();
			dgView.Columns.Add(" ", " ");
			dgView.Columns[0].Width = 70;
			for (int i = 1; i < DateTime.DaysInMonth(Year, Month) + 1; i++)
			{
				DateTime dt = new DateTime(Year, Month, i);
				dgView.Columns.Add((i).ToString(), (i).ToString());
				if (dt.DayOfWeek == DayOfWeek.Saturday)
					dgView.Columns[i].HeaderCell.Style.ForeColor = Color.Blue;
				else if (dt.DayOfWeek == DayOfWeek.Sunday)
					dgView.Columns[i].HeaderCell.Style.ForeColor = Color.Red;
				else if (CalendarControl.GenCalendar.HolidayChecker.IsHoliday(dt))
					dgView.Columns[i].HeaderCell.Style.ForeColor = Color.Green;
				dgView.Columns[i].Width = 30;
				dgView.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
			}
			StandbyList.Persons.ForEach(p =>
			{
				DataGridViewRow row = new DataGridViewRow();
				dgView.EnableHeadersVisualStyles = false;
				row.CreateCells(dgView);
				row.Cells[0].Value = p;
				row.Cells[0].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
				for (int i = 1; i < DateTime.DaysInMonth(Year, Month) + 1; i++ )
				{
					DateTime dt = new DateTime(Year, Month, i);
					if (dt.DayOfWeek == DayOfWeek.Saturday)
						row.Cells[i].Style.BackColor = Color.LightBlue;
					else if (dt.DayOfWeek == DayOfWeek.Sunday)
						row.Cells[i].Style.BackColor = Color.LightPink;
					else if (CalendarControl.GenCalendar.HolidayChecker.IsHoliday(dt))
						row.Cells[i].Style.BackColor = Color.LightGreen;
					switch( p[new DateTime(Year, Month, i)])
					{
						case global::StandbyList.PossibleDays.Status.Affair:
							row.Cells[i].Value = "×";
							break;
						case global::StandbyList.PossibleDays.Status.Limited:
							row.Cells[i].Value = "△";
							break;
						default:
							row.Cells[i].Value = "";
							break;
					}
					row.Cells[i].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
				}
				
				dgView.Rows.Add(row);
			});

		}

		private void dgView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex <= 0 || e.RowIndex < 0) return;
			string value = dgView[e.ColumnIndex, e.RowIndex].Value as string;
			switch (value)
			{
				case "×":
					dgView[e.ColumnIndex, e.RowIndex].Value = "△";
					dgView[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Black;
					(dgView.Rows[e.RowIndex].Cells[0].Value as StandbyList.Person)[new DateTime(Year, Month, e.ColumnIndex)] = global::StandbyList.PossibleDays.Status.Limited;
					break;
				case "":
					dgView[e.ColumnIndex, e.RowIndex].Value = "×";
					dgView[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Black;
					(dgView.Rows[e.RowIndex].Cells[0].Value as StandbyList.Person)[new DateTime(Year, Month, e.ColumnIndex)] = global::StandbyList.PossibleDays.Status.Affair;
					break;
				case "△":
					dgView[e.ColumnIndex, e.RowIndex].Value = "";
					dgView[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Black;
					(dgView.Rows[e.RowIndex].Cells[0].Value as StandbyList.Person)[new DateTime(Year, Month, e.ColumnIndex)] = global::StandbyList.PossibleDays.Status.None;
					break;
			}

		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			DialogResult = System.Windows.Forms.DialogResult.OK;
			Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = System.Windows.Forms.DialogResult.Cancel;
			Close();
		}
	}
}
