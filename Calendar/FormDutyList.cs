using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClosedXML.Excel;  // ClosedXML使用

namespace Calendar
{
    public partial class FormDutyList : Form
    {
		public int Year { get; set; }
		public int Month { get; set; }
		public StandbyList.StandbyList StandbyList { get; set; }
        public FormDutyList()
        {
            InitializeComponent();
			Year = DateTime.MinValue.Year;
			Month = DateTime.MinValue.Month;
			StandbyList = null;
        }

		private void FormDutyList_Load(object sender, EventArgs e)
		{
            // イベント
            dgView.MouseUp += DgView_MouseUp;
            const int rowslide = 0;
            const int colslide = 3;	// columnの最初は固定枠（0:日付, 1:曜日, 2:病棟）のため、最初のcolumn開始位置をスライドさせる。
			if (StandbyList == null ||
				(Year == DateTime.MinValue.Year && Month == DateTime.MinValue.Month))
			{
				Close(); // フォームの初期化が不十分なため終了。
				return;
			}
			Text = string.Format("{0}年{1}月 当直表", Year, Month);
			dgView.Columns.Clear();
			dgView.Rows.Clear();
            var duties = StandbyList.GetDuties().ToList();
            dgView.Columns.Add("", "");
            dgView.Columns.Add("曜日", "");
            dgView.Columns.Add("病棟", "病棟");
            dgView.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgView.Columns[0].Width = 20;
            dgView.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgView.Columns[1].Width = 20;
            dgView.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgView.Columns[1].Frozen = true;
            dgView.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgView.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgView.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgView.Columns[2].Width = 60;
            // DutyをdgViewのColumnに追加
            for(int i = 0; i < duties.Count(); i++)
            {
                dgView.Columns.Add(duties[i], duties[i]);
                dgView.Columns[i + colslide].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgView.Columns[i + colslide].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
			// カレンダーを作成
            for(int i = 1; i <= DateTime.DaysInMonth(Year, Month); i++)
            {
                DateTime dt = new DateTime(Year, Month, i);
                string[] weekday = { "日", "月", "火", "水", "木", "金", "土" };
                var row = new DataGridViewRow();
                row.CreateCells(dgView);
                row.Cells[0].Value = i.ToString();
                row.Cells[1].Value = weekday[(int)dt.DayOfWeek];
                row.Cells[0].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                row.Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                if (dt.DayOfWeek == DayOfWeek.Saturday)
                    row.DefaultCellStyle.BackColor = Color.LightBlue;
                else if (dt.DayOfWeek == DayOfWeek.Sunday)
                    row.DefaultCellStyle.BackColor = Color.LightPink;
                else if (CalendarControl.GenCalendar.HolidayChecker.IsHoliday(dt))
                    row.DefaultCellStyle.BackColor = Color.LightGreen;
                dgView.Rows.Add(row);
            }
            // 病棟当直を埋める
            for(int i = 0; i < DateTime.DaysInMonth(Year, Month); i++)
            {
                if (StandbyList.Standby[i + 1] != null)
                {
                    dgView[2, i].Value = StandbyList.Standby[i + 1].Name;
                    dgView[2, i].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
            // dutyに一致するPersonと日にち,勤務形態を取得する
            for(int i = 0; i < duties.Count; i++)
            { 
                var tpl = StandbyList.GetDayAndAccessory(duties[i]);
                tpl.ToList().ForEach(u =>
                {
                    var days = u.Item2.GetDaysWithoutAffair(Year, Month);
                    days.ForEach(v =>
                    {
                        var cell = dgView[i + colslide, v - (rowslide + 1)];
                        if (u.Item2.RuleClass.SelfIdentify == global::StandbyList.RepeatRule.Identify.MonthClass)
                        {
                            global::StandbyList.Accessory accessory = global::StandbyList.Accessory.None;
                            if (u.Item2.RuleClass.DayOfWeek == global::StandbyList.RepeatRule.DayOfWeek.土日)
                            {
                                var date = new DateTime(Year, Month, v);
                                if (date.DayOfWeek == DayOfWeek.Sunday)
                                    accessory = u.Item2.Accessory2;
                                else
                                    accessory = u.Item2.Accessory1;
                            }
                            else
                                accessory = u.Item2.Accessory1;

                            switch (accessory)
                            {
                                // Tuple<string, string, string> string1:日直, string2:当直, string3:透析
                                case global::StandbyList.Accessory.Day:
                                    cell.Tag = new Tuple<string, string, string>(u.Item1, cell.Tag != null ? (cell.Tag as Tuple<string, string, string>).Item2 : "", "");
                                    break;
                                case global::StandbyList.Accessory.Night:
                                    cell.Tag = new Tuple<string, string, string>(cell.Tag != null ? (cell.Tag as Tuple<string, string, string>).Item1 : "", u.Item1, "");
                                    break;
                                case global::StandbyList.Accessory.DayAndNight:
                                    cell.Tag = new Tuple<string, string, string>(u.Item1, u.Item1, "");
                                    break;
                                case global::StandbyList.Accessory.HD:
                                    cell.Tag = new Tuple<string, string, string>("", "", u.Item1);
                                    break;
                            }
                            if (accessory != global::StandbyList.Accessory.HD)
                                cell.Value = cell.Tag != null ? (cell.Tag as Tuple<string, string, string>).Item1 + "/" + (cell.Tag as Tuple<string, string, string>).Item2 : "";
                            else
                                cell.Value = cell.Tag != null ? (cell.Tag as Tuple<string, string, string>).Item3 : "";
                            cell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        }
                    });
                });

            }

        }

        private void DgView_MouseUp(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                ContextMenu menu = new ContextMenu();
                MenuItem item = new MenuItem();
                item.Text = "この列をコピー(&C)";
                item.Click += menuVCopy_Click;
                menu.MenuItems.Add(item);
                item = new MenuItem();
                item.Text = "すべてコピー(&A)";
                item.Click += menuACopy_Click;
                menu.MenuItems.Add(item);

                var ht = dgView.HitTest(e.X, e.Y);
                dgView.ClearSelection();
                dgView[ht.ColumnIndex, ht.RowIndex].Selected = true;

                menu.Show(dgView, e.Location);
            }
        }

        private void menuACopy_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < dgView.ColumnCount; i++)
            {
                if (i != dgView.ColumnCount - 1)
                {
                    sb.Append(dgView.Columns[i].HeaderText);
                    sb.Append("\t");
                }
                else
                    sb.AppendLine(dgView.Columns[i].HeaderText);
            }
            for(int i = 0; i < dgView.RowCount; i++)
            {
                for(int j = 0; j < dgView.ColumnCount; j++)
                {
                    if (j != dgView.ColumnCount - 1)
                    {
                        sb.Append(dgView[j, i].Value != null ? dgView[j, i].Value.ToString() : "");
                        sb.Append("\t");
                    }
                    else
                    {
                        sb.Append(dgView[j, i].Value != null ? dgView[j, i].Value.ToString() : "");
                        sb.Append(Environment.NewLine);
                    }
                    
                }
            }
            var text = sb.ToString();
            Clipboard.SetText(text);
        }

        private void menuVCopy_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            sb.AppendLine(dgView.Columns[dgView.SelectedCells[0].ColumnIndex].HeaderText);
            for(int i = 0; i < dgView.Rows.Count; i++)
            {
                dgView[dgView.SelectedCells[0].ColumnIndex, i].Selected = true;
                sb.AppendLine(dgView[dgView.SelectedCells[0].ColumnIndex, i].Value != null ? 
                    dgView[dgView.SelectedCells[0].ColumnIndex, i].Value.ToString() : "");
            }
            var text = sb.ToString();
            try
            {
                Clipboard.SetText(text);

            }
            catch
            {
                MessageBox.Show("エラー", "コピー中に不明なエラーが発生しました。");
            }
        }

        private void mnuSave_Click(object sender, EventArgs e)
		{

            // ファイルを保存ダイアログを作成
            var sfd = new SaveFileDialog();
            sfd.FileName = string.Format("{0}年{1}月 当直表.xlsx", Year, Month);
            sfd.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            sfd.Filter = "Excelファイル(*.xlsx)|*.xlsx|全てのファイル(*.*)|*.*";
            sfd.FilterIndex = 1;
            sfd.Title = "保存先を選択して下さい";
            sfd.RestoreDirectory = true;
            sfd.OverwritePrompt = true;
            sfd.CheckPathExists = true;

            // ダイアログを表示
            if(sfd.ShowDialog() == DialogResult.OK)
            {
                // Excelファイルを作成
                var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add(string.Format("{0}年{1}月 当直表", Year, Month));

                for (int i = 0; i < dgView.Columns.Count; i++)
                {
                    worksheet.Column(i + 1).Width = dgView.Columns[i].Width / 6;
                    worksheet.Cell(1, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center; 
                    worksheet.Cell(1, i + 1).Value = dgView.Columns[i].HeaderCell.Value;
                }

                for (int i = 0; i < dgView.Columns.Count; i++)
                {
                    for(int j = 0; j < dgView.Rows.Count; j++)
                    {
                        DateTime dt = new DateTime(Year, Month, j + 1);

                        worksheet.Cell(j + 2, i + 1).Value = dgView[i, j].Value;
                        worksheet.Cell(j + 2, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        if (dt.DayOfWeek == DayOfWeek.Saturday)
                            worksheet.Cell(j + 2, i + 1).Style.Fill.SetBackgroundColor(XLColor.LightBlue);
                        else if (dt.DayOfWeek == DayOfWeek.Sunday)
                            worksheet.Cell(j + 2, i + 1).Style.Fill.SetBackgroundColor(XLColor.LightPink);
                        else if (CalendarControl.GenCalendar.HolidayChecker.IsHoliday(dt))
                            worksheet.Cell(j + 2, i + 1).Style.Fill.SetBackgroundColor(XLColor.LightPink);

                    }
                }
                // 罫線を引く
                worksheet.Range(1, 1, dgView.Rows.Count + 1, dgView.Columns.Count).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Range(1, 1, dgView.Rows.Count + 1, dgView.Columns.Count).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                try
                {
                    workbook.SaveAs(sfd.FileName);
                }
                catch(Exception)
                {
                    MessageBox.Show("エラー");
                }
            }


		}

    }
}
