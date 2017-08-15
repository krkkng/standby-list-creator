using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using CalendarControl;
using StandbyList;
using CalendarControl.GenCalendar;
using System.Runtime.InteropServices;

namespace Calendar
{
	public partial class Form1 : Form
	{
		private ContextMenuStrip menu;
		private AggregationForm AgForm;
		public Form1()
		{
			InitializeComponent();
			Core.StandbyLists = new List<StandbyList.StandbyList>();
			Text = "当直表作成くん";

			LoadData();
            LoadSetting();
		}

		private void CreateStandby()
		{
			calendar1.RemoveMonth(calendar1.DrawYear, calendar1.DrawMonth);
			StandbyList.StandbyList st = Core.StandbyLists.FirstOrDefault(t => t.Year == calendar1.Year && t.Month == calendar1.Month);
			if (st == null) return;
			st.CreateEmpty(); // 現在のデータを初期化する。
				
			List<StandbyList.StandbyList> tmpsp = new List<StandbyList.StandbyList>();
            List<double> listbunsan = new List<double>();
            List<double> listbunsanh = new List<double>();
			List<double> listbunsant = new List<double>();
			int count = 0;
            int index = 0;
			StandbyList.StandbyList most = null;

            while (true)
			{
				count++;
				st.Create(Core.StandbyLists);
                
				tmpsp.Add(st.Clone() as StandbyList.StandbyList);
				st.Clear();
                // *分散を求めて、最も分散が小さいものを選択。(平日の当直回数のバラツキが最も少ないものを選択）
                double dutycount = 0;
                double holidaycount = 0;
				double totalcount = 0;
                st.Persons.ForEach(t =>
                    {
                        dutycount += st.OrdinaryDutyCount(t);
                        holidaycount += st.HolidayCount(t);
						totalcount += st.DutyCount(t);
                    });
                double dutyaverage = dutycount / st.Persons.Count;
                double holidayaverage = holidaycount / st.Persons.Count;
				double totalaverage = totalcount / st.Persons.Count;

                double dutybunsan = 0;
                double holidaybunsan = 0; // 今の所、こちらは使わない。
				double totalbunsan = 0;

                st.Persons.ForEach(t =>
                {
                    dutybunsan += Math.Pow((dutyaverage - st.OrdinaryDutyCount(t)), 2);
                    holidaybunsan += Math.Pow((holidayaverage - st.HolidayCount(t)), 2);
					totalbunsan += Math.Pow((totalaverage - st.DutyCount(t)), 2);
                });

                listbunsan.Add(dutybunsan);
                listbunsanh.Add(holidaybunsan);
				listbunsant.Add(totalbunsan);
                // *ここまで
				if (count >= 1000/*Setting.TryCount*/)
				{
					int nullcount = DateTime.DaysInMonth(calendar1.DrawYear, calendar1.DrawMonth);
					tmpsp.ForEach(t =>
						{
							if (most == null)
							{
								most = t;
							}
							else
							{
								int c = t.Standby.NullCount();
								if (nullcount >= c)
								{
									most = t;
									nullcount = c;
								}
							}

						});
			

					break;
				}
			}
            while (tmpsp.Count > 0)
            {
                double min = listbunsant.Min(); // 求めた分散の中で最も小さいものを選別。
                index = listbunsant.IndexOf(min);
                // 分散で選んだ予定表候補に抜けがないかをチェック。抜けがある場合はリストから削除し、再度最小の分散を選択する。
                if (tmpsp[index].Standby.NullCheck())
                {
                    Core.StandbyLists.Remove(st);
                    Core.StandbyLists.Add(tmpsp[index]);
                    break;
                }
                else
                {
                    tmpsp.RemoveAt(index);
                    listbunsant.RemoveAt(index);
                }
            }
            // 抜けのない予定表候補が見つからない場合はあきらめる。
            if(tmpsp.Count == 0)
            {
                MessageBox.Show(count.ToString() + "通りの組合せを検討した結果、条件を満たす組合せが見つかりませんでした。" +
                                "「設定」から個人の必要条件を変更の上、再度お試し下さい。");
                Core.StandbyLists.Remove(st);
                Core.StandbyLists.Add(most);
				//st = most;
            }
            // カレンダーへの表示処理
            StandbyList.StandbyList resultlist = Core.StandbyLists.FirstOrDefault(t => t.Year == calendar1.Year && t.Month == calendar1.Month);
            for (int i = 0; i < resultlist.Standby.Count; i++)
			{
				CalendarControl.Schedule item = new CalendarControl.Schedule();
				item.Start = new DateTime(calendar1.DrawYear, calendar1.DrawMonth, i + 1);
				item.Item = resultlist.Standby[i+1];
				if (resultlist.Standby[i+1] != null)
				{
                    // これは削除可能？
					var status = resultlist.Standby[i+1].Possible.First(t => t.Year == calendar1.DrawYear && t.Month == calendar1.DrawMonth).PossibleDay[i];
				}
				item.Description = "duty";
				item.BackColor = Setting.DutyFieldColor;
				item.Alignment = StringAlignment.Center;
				calendar1.AddSchedule(item);

			}
            // 集計
			ShowAgrregationForm();

		}
		private void CreateStandbyEmpty()
		{
			calendar1.RemoveMonth(calendar1.DrawYear, calendar1.DrawMonth);
			StandbyList.StandbyList st = Core.StandbyLists.FirstOrDefault(t => t.Year == calendar1.DrawYear && t.Month == calendar1.DrawMonth);
			if (st == null) return;
				
			StandbyList.DutyPersonsCollection sp;
			st.CreateEmpty();
			sp = st.Standby;
			var stold = Core.StandbyLists.FirstOrDefault(t => t.Year == st.Year && t.Month == st.Month);
			if(stold != null)
			{
				Core.StandbyLists.Remove(stold);
			}
			Core.StandbyLists.Add(st);
			Core.Sort();
			for (int i = 0; i < sp.Count; i++)
			{
				CalendarControl.Schedule item = new Schedule();
				item.Start = new DateTime(st.Year, st.Month, i + 1);
				item.Item = null;
				item.Description = "duty";
				item.BackColor = Setting.DutyFieldColor;
				item.Alignment = StringAlignment.Center;
				calendar1.AddSchedule(item);
			}
			ShowAgrregationForm();
			
		}
		private void btnCreate_Click(object sender, EventArgs e)
		{
			CreateStandby();
		}
		private void ShowAgrregationForm()
		{
            if (AgForm == null)
            {
                AgForm = new AggregationForm();

                AgForm.dgView.Columns.Clear();
                AgForm.dgView.Columns.Add("name", "名前");
                AgForm.dgView.Columns.Add("total", "総数");
                AgForm.dgView.Columns.Add("当直回数", "平日当直");
                AgForm.dgView.Columns.Add("休日当直", "休日当直");
                AgForm.dgView.Columns.Add("総負荷", "総負荷");
                AgForm.dgView.Columns[0].Width = 70;
                AgForm.dgView.Columns[1].Width = 65;
                AgForm.dgView.Columns[2].Width = 65;
                AgForm.dgView.Columns[3].Width = 65;
                AgForm.dgView.Columns[4].Width = 70;
			    AgForm.SelectionChanged += AgForm_SelectionChanged;
                AgForm.FormClosed += AgForm_FormClosed;
                AgForm.AggregationIntervalChanged += AgForm_AggregationIntervalChanged;
            }
            AgForm.Year = calendar1.DrawYear;
            AgForm.Month = calendar1.DrawMonth;
            PersonsAggregation(new AggregationForm.DateInterval(AgForm.CriteriaDate));
            AgForm.Reload();
            // 現在選択中のRowを仮保存
            DataGridViewRow selected = null;
            if(AgForm.dgView.SelectedRows.Count != 0)
                selected = AgForm.dgView.SelectedRows[0];
			AgForm.dgView.Rows.Clear();
			StandbyList.StandbyList st = Core.StandbyLists.FirstOrDefault(t => t.Year == calendar1.DrawYear && t.Month == calendar1.DrawMonth);
			if (st != null)
			{
				st.Persons.ForEach(p =>
				{
					DataGridViewRow row = new DataGridViewRow();
					row.CreateCells(AgForm.dgView);
                    if (p.Attre == Person.Attributes.助教)
                        row.DefaultCellStyle.BackColor = Color.AliceBlue;
					row.Tag = p;
					row.Cells[0].Value = p;
					row.Cells[1].Value = p.OrdinaryDutyCount + p.HolidayCount;
					row.Cells[2].Value = p.OrdinaryDutyCount + "/" + p.Requirement.PossibleTimes;
					row.Cells[3].Value = p.HolidayCount + "/" + p.Requirement.HolidayPossibleTimes;
					row.Cells[4].Value = p.TotalBurden;
					AgForm.dgView.Rows.Add(row);
				});
                if (selected != null)
                {
                    var trow = AgForm.dgView.Rows.Cast<DataGridViewRow>().FirstOrDefault(r => (r.Cells[0].Value as Person).ID == (selected.Cells[0].Value as Person).ID);
                    if (trow != null)
                        AgForm.dgView.Rows[trow.Index].Selected = true;
                }
			}
			if (!AgForm.Visible)
				AgForm.Show(this);
		}

        private void AgForm_AggregationIntervalChanged(object sender, AggregationForm.AggregationIntervalChangedEventArgs e)
        {
            //PersonsAggregation(e.DateInterval);
            ShowAgrregationForm();

        }

        private void AgForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            calendar1.GetSchedules().ForEach(s =>
            {
                if (s.BackColor != Setting.DutyFieldColor)
                    s.BackColor = Setting.DutyFieldColor;
            });
        }

        void AgForm_SelectionChanged(object sender, EventArgs e)
		{
			DataGridView dgview = sender as DataGridView;
			if (dgview.SelectedRows.Count > 0)
			{
				//MessageBox.Show((dgview.SelectedRows[0].Tag as Person).Name);
				calendar1.GetSchedules().ForEach(s =>{
					if (s.BackColor != Setting.DutyFieldColor)
						s.BackColor = Setting.DutyFieldColor;
                    if (s.Item != null)
                    {
                        if ((s.Item as Person).ID == (dgview.SelectedRows[0].Tag as Person).ID)
                            s.BackColor = Setting.DutyHilightColor;
                    }
				});
			}
		}
		private void PersonsAggregation(AggregationForm.DateInterval di)
		{
			StandbyList.StandbyList st = Core.StandbyLists.FirstOrDefault(t => t.Year == calendar1.DrawYear && t.Month == calendar1.DrawMonth);
			if (st == null) return;
			// 再カウント
			Core.TotalBurden(st.Persons, new DateTime(calendar1.Year, calendar1.Month, 1), di.Start , true);
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			Application.ApplicationExit += Application_ApplicationExit;
			menu = new ContextMenuStrip();
			dateTimePicker1.Value = new DateTime(DateTime.Today.AddMonths(1).Year, DateTime.Today.AddMonths(1).Month, 1);
            ChangeLabelHeight();

            if (Setting.MainFormRect != Rectangle.Empty)
            {
                this.Bounds = Setting.MainFormRect;
                this.WindowState = Setting.MainFormState;
            }
            SetHotKeys();
		}
        const int MOD_ALT = 0x0001;
        const int MOD_CONTROL = 0x0002;
        const int MOD_SHIFT = 0x0004;
        const int WM_HOTKEY = 0x0312;

        const int HOTKEY_RANDOMSELECT = 0x0001;  // 0x0000～0xbfff 内の適当な値でよい

        [DllImport("user32.dll")]
        extern static int RegisterHotKey(IntPtr HWnd, int ID, int MOD_KEY, int KEY);
        [DllImport("user32.dll")]
        extern static int UnregisterHotKey(IntPtr HWnd, int ID);

        protected override void WndProc(ref Message m)
        {
            const int WM_SIZE = 0x5;
            const int SIZE_RESTORED = 0x0;
            const int SIZE_MINIMIZED = 0x1;
            const int SIZE_MAXIMIZED = 0x2;

            if (m.Msg == WM_SIZE)
            {
                switch (m.WParam.ToInt32())
                {
                    case SIZE_RESTORED:
                        Setting.MainFormRect = this.Bounds;
                        break;

                    case SIZE_MINIMIZED:
                        break;

                    case SIZE_MAXIMIZED:
                        break;
                }
            }
            if(m.Msg == WM_HOTKEY)
            {
                if((int)m.WParam == HOTKEY_RANDOMSELECT)
                {
                    RandomSelect();
                }
            }
            base.WndProc(ref m);
        }

        void Application_ApplicationExit(object sender, EventArgs e)
		{
            Setting.MainFormState = this.WindowState;

			Serializer.SaveDatas(Application.StartupPath + @"\datas.dbx");
            Serializer.SaveSettings(Application.StartupPath + @"\setting.xml");

		}

        /// <summary>
        /// HotKeyを設定
        /// </summary>
        private void SetHotKeys()
        {
            RegisterHotKey(this.Handle, HOTKEY_RANDOMSELECT, MOD_CONTROL, (int)Keys.R);
        }
        /// <summary>
        /// HotKeyを解除
        /// </summary>
        private void RemoveHotKey()
        {
            UnregisterHotKey(this.Handle, HOTKEY_RANDOMSELECT);
        }
        private void RandomSelect()
        {
            ScheduleLabel label = calendar1.SelectedScheduleLabel;
            if (label == null) return;
            StandbyList.StandbyList st = Core.StandbyLists.FirstOrDefault(t => t.Year == calendar1.DrawYear & t.Month == calendar1.DrawMonth);
            if(st != null)
            {
                var plist = st.PossibleList(calendar1.SelectedDate, st.Standby, Core.StandbyLists);
                if (plist.Count == 0)
                {
                    MessageBox.Show("この日に設定できる候補者が存在しません");
                    return;
                }
                plist = plist.OrderBy(u => Guid.NewGuid()).ToList();
                st.Standby.SetDuty(label.Schedule.Start.Day, plist[0].Person);
                label.Schedule.Item = plist[0].Person;
                ShowAgrregationForm();
            }
        }
        private void MenuRandomSelect(object sender, EventArgs e)
        {
            RandomSelect();
        }

        private void DeleteSchedule()
		{
			ScheduleLabel label = calendar1.SelectedScheduleLabel;
			StandbyList.StandbyList st = Core.StandbyLists.FirstOrDefault(t => t.Year == calendar1.Year && t.Month == calendar1.Month);
			if (st == null) return;

			int day = label.Schedule.Start.Day;
			if (label != null)
			{
				Person p = label.Schedule.Item as Person;
				if (p != null)
				{
					Person newp = new Person(-1) { Name = "-" };
					if (label.Schedule.Description == "duty")
					{
						st.Standby.SetDuty(day, newp);
						label.Schedule.Item = newp;
					}
				}
				ShowAgrregationForm();
			}
		}
		private void MenuDeleteClick(object sender, EventArgs e)
		{
			DeleteSchedule();
		}
		private void MenuOtherClick(object sender, EventArgs e)
		{
			ShowSubForm(calendar1.SelectedScheduleLabel);
		}
		private void MenuFreeCommentClick(object sender, EventArgs e)
		{
			ShowfreeCommentForm(calendar1.SelectedScheduleLabel);
		}

		private void ShowfreeCommentForm(ScheduleLabel schedulelabel)
		{
			FormComment form = new FormComment();
			Person person = null;
			if(schedulelabel.Schedule.Item != null)
				person = schedulelabel.Schedule.Item as Person;
			StandbyList.StandbyList st = Core.StandbyLists.FirstOrDefault(t => t.Year == calendar1.DrawYear && t.Month == calendar1.DrawMonth);
			if (st == null) return;
			int day = schedulelabel.Schedule.Start.Day;
			if(form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				if(form.Person != null)
				{
					st.Standby.SetDuty(day, form.Person);
					schedulelabel.Schedule.Item = form.Person;
				}
				ShowAgrregationForm();
			}
		}
		private void ShowSubForm(ScheduleLabel schedulelabel)
		{
			SubForm form = new SubForm();
			List<LimitedPerson> list;
			StandbyList.StandbyList st = Core.StandbyLists.FirstOrDefault(t => t.Year == calendar1.DrawYear && t.Month == calendar1.DrawMonth);
			if (st == null) return;
			list = st.PossibleList(schedulelabel.Schedule.Start, st.Standby, Core.StandbyLists);
            form.Date = schedulelabel.Schedule.Start;
			form.AddRange(list);
            form.AddRange(GetSwapableList(schedulelabel.Schedule.Start.Day));
            form.SelectionChanged += SubForm_SwapViewSelectionChanged;
            form.FormClosed += SubForm_FormClosed;
			int day = schedulelabel.Schedule.Start.Day;
			if (schedulelabel != null)
				if (schedulelabel.Schedule.Description == "duty")
					form.Person = st.Standby[day];
			if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{

                if (form.Person != null)
                {
                    switch (form.Type)
                    {
                        case SubForm.SubFormType.Change:
                            if (schedulelabel.Schedule.Description == "duty")
                            {
                                st.Standby.SetDuty(day, form.Person);
                                schedulelabel.Schedule.Item = form.Person;
                            }
                            break;
                        case SubForm.SubFormType.Swap:
                            st.Standby.SetDuty(form.Date.Day, schedulelabel.Schedule.Item as Person);
                            st.Standby.SetDuty(schedulelabel.Schedule.Start.Day, form.Person);
                            var swapschedule = calendar1.GetSchedule(form.Date);
                            swapschedule[0].Item = schedulelabel.Schedule.Item;
                            schedulelabel.Schedule.Item = form.Person;
                            form.SelectionChanged -= SubForm_SwapViewSelectionChanged;

                            break;
                    }
                }
				ShowAgrregationForm();
			}

		}

        private void SubForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            calendar1.GetSchedules().ForEach(s =>
            {
                if (s.BackColor != Setting.DutyFieldColor)
                    s.BackColor = Setting.DutyFieldColor;
            });
        }

        private void SubForm_SwapViewSelectionChanged(object sender, EventArgs e)
        {
            var dgView = sender as DataGridView;
            if(dgView.SelectedRows.Count > 0)
            {
                calendar1.GetSchedules().ForEach(s =>
                {
                    if (s.BackColor != Setting.DutyFieldColor)
                        s.BackColor = Setting.DutyFieldColor;
                    if (s.Item != null)
                    {
                        if ((s.Item as Person).ID == (dgView.SelectedRows[0].Cells[0].Value as Person).ID &&
                        s.Start.Day == (int)dgView.SelectedRows[0].Cells[1].Tag)
                            s.BackColor = Setting.DutyHilightColor;
                    }
                });
            }
        }

        /// <summary>
        /// 当直を交代可能な人のリストを作成します
        /// </summary>
        /// <param name="day">交代する対象の日にち（交代してほしい日にち）</param>
        /// <returns></returns>
        private List<SwapableList> GetSwapableList(int day)
        {
            var list = new List<SwapableList>();
            StandbyList.StandbyList st = Core.StandbyLists.FirstOrDefault(t => t.Year == calendar1.DrawYear && t.Month == calendar1.DrawMonth);
            // 対象の日にちに設定してあるPersonを一時的に削除する
            Person tmptp = st.Standby[day];
            st.Standby.SetDuty(day, null);
            for (int i = 0; i < DateTime.DaysInMonth(calendar1.DrawYear, calendar1.DrawMonth); i++)
            {
                // 1日から順番に走査して、交代が可能であればリストに追加していく
                // 交代相手の日にちのPersonも一時的に削除して、双方のPersonが双方の日にちで当直が可能かどうかを検討する
                Person tmpp = st.Standby[i + 1];
                st.Standby.SetDuty(i + 1, null);
                LimitedPerson lp = st.PossiblePerson(new DateTime(calendar1.DrawYear, calendar1.DrawMonth, day), tmpp, st.Standby, Core.StandbyLists);
                LimitedPerson ltp = st.PossiblePerson(new DateTime(calendar1.DrawYear, calendar1.DrawMonth, i + 1), tmptp, st.Standby, Core.StandbyLists);
                if (lp != null && lp.LimitStatus == LimitedPerson.Limit.None && ltp != null && ltp.LimitStatus == LimitedPerson.Limit.None)
                {
                    if (tmpp.ID != tmptp.ID)
                    {
                        var swap = new SwapableList() { Day = i + 1, Person = tmpp };
                        list.Add(swap);
                    }
                }
                st.Standby.SetDuty(i + 1, tmpp); //削除していたPersonを元に戻す
            }
            st.Standby.SetDuty(day, tmptp);

            return list;
        }

		private void dgView_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{

		}

		bool calendar_flag = false;
		bool datepicker_flag = false;
		private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
		{
			if (!calendar_flag)
			{
				datepicker_flag = true;
				calendar1.SetDrawDate(dateTimePicker1.Value);
				calendar1.SetDate(dateTimePicker1.Value);
			}
			datepicker_flag = false;
		}

		private void changeEnable()
		{
			if (Core.StandbyLists.FirstOrDefault(t => t.Year == calendar1.DrawYear && t.Month == calendar1.DrawMonth) == null)
			{
				btnCreate.Enabled = false;
				calendar1.Enabled = false;
				menuGlobalSetting.Enabled = false;
				mnuCreate.Enabled = false;
				mnuPrint.Enabled = false;
			}
			else
			{
				btnCreate.Enabled = true;
				calendar1.Enabled = true;
				menuGlobalSetting.Enabled = true;
				mnuCreate.Enabled = true;
				mnuPrint.Enabled = true;
			}
		}


		private void mnuCreate_Click(object sender, EventArgs e)
		{
			CreateStandby();
		}

		private void Print()
		{
			System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();
			pd.PrintPage += pd_PrintPage;
			PrintDialog pdialog = new PrintDialog();
			pdialog.Document = pd;
			pdialog.UseEXDialog = true;
			if(pdialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				pd.Print();
			}
		}
		private void PrintInitialCalendar()
		{
			System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();
			pd.PrintPage += pd_PrintCalendarPage;
			PrintDialog pdialog = new PrintDialog();
			pdialog.Document = pd;
			pdialog.UseEXDialog = true;
			if (pdialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				pd.Print();
		}
		void pd_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
		{
			calendar1.PrintCalendar(e.Graphics, e.MarginBounds, Setting.FooterMessage, Setting.DepartmentString);
		}

		private int pagenumber = 0;
		void pd_PrintCalendarPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
		{
			var standbylist = Core.StandbyLists.FirstOrDefault(t => t.Year == calendar1.Year && t.Month == calendar1.Month);
			Person p = standbylist.Persons[pagenumber];

			string[] str = new string[DateTime.DaysInMonth(calendar1.Year, calendar1.Month)];
			for (int i = 0; i < DateTime.DaysInMonth(calendar1.Year, calendar1.Month); i++)
			{
				DateTime dt = new DateTime(calendar1.Year, calendar1.Month, i + 1);
				switch (p[dt])
				{
					default:
						str[i] = "";
						break;
				}
			}
			string strfooter = "不都合日には”×”を付けて下さい。1日中は無理でも、待機可能な時間帯がある場合は”△”を付けて、可能な時間帯を併せて記入して下さい（特に休日）。";
			calendar1.PrintInitialCalendar(e.Graphics, e.MarginBounds, "不都合日確認表 " + calendar1.DrawYear.ToString() + "年" + calendar1.DrawMonth + "月 - " + p.Name + "先生", strfooter, str);
			pagenumber++;
			if (pagenumber > standbylist.Persons.Count() - 1)
			{
				e.HasMorePages = false;
				pagenumber = 0;
			}
			else
				e.HasMorePages = true;
		}
        #region Calendar1関連イベント処理
        private void calendar1_ChangeDate(object sender, CalendarControl.Calendar.ChangeDateEventArgs e)
        {
            if (!datepicker_flag)
            {
                calendar_flag = true;
                dateTimePicker1.Value = new DateTime(calendar1.Year, calendar1.Month, calendar1.Day);
            }
            calendar_flag = false;
            if (!(e.OldDate.Year == calendar1.Year && e.OldDate.Month == calendar1.Month))
                ShowAgrregationForm();
        }

        private void calendar1_ChangeDrawDate(object sender, CalendarControl.Calendar.ChangeDateEventArgs e)
        {
            changeEnable();
            ChangeLabelHeight();

        }

        private void calendar1_ScheduleLabelKeyDown(object sender, CalendarControl.Calendar.ScheduleLabelKeyEventArgs e)
		{
			if (e.KeyData == Keys.Delete)
				DeleteSchedule();

		}

        private void calendar1_ScheduleLabelMouseDoubleClick(object sender, CalendarControl.Calendar.ScheduleLabelMouseEventArgs e)
        {
            ShowSubForm(e.Label);
        }

        private void calendar1_ScheduleLabelMouseUp(object sender, CalendarControl.Calendar.ScheduleLabelMouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                var menuitems = new ToolStripMenuItem[4];

                menuitems[0] = new ToolStripMenuItem("削除", null, MenuDeleteClick);
                menuitems[1] = new ToolStripMenuItem("その他の候補者", null, MenuOtherClick);
                menuitems[2] = new ToolStripMenuItem("ランダム選択", null, MenuRandomSelect);
                menuitems[3] = new ToolStripMenuItem("フリー入力", null, MenuFreeCommentClick);
                menuitems[0].ShortcutKeyDisplayString = "Delete";
                menuitems[2].ShortcutKeyDisplayString = "Ctrl+R";

                menu.Items.Clear();
                menuitems.ToList().ForEach(t => menu.Items.Add(t));

                this.ContextMenuStrip = menu;
                this.ContextMenuStrip.Show(PointToScreen(new Point(e.X, e.Y)));
            }

        }

        private void calendar1_DoubleClick(object sender, EventArgs e)
        {
            CalendarControl.Schedule item = new CalendarControl.Schedule();
            item.Start = calendar1.SelectedDate;
            item.Item = calendar1.SelectedDate.ToString() + "追加";
            calendar1.AddSchedule(item);
        }

        private void calendar1_SizeChanged(object sender, EventArgs e)
        {
            ChangeLabelHeight();
        }
        #endregion Calendar1関連イベント処理

        private void btnPrevMonth_Click(object sender, EventArgs e)
		{
			calendar1.SetDrawDate(new DateTime(calendar1.DrawYear, calendar1.DrawMonth, 1).AddMonths(-1));
			calendar1.SetDate(new DateTime(calendar1.DrawYear, calendar1.DrawMonth, 1));
        }

        private void btnNextMonth_Click(object sender, EventArgs e)
		{
			calendar1.SetDrawDate(new DateTime(calendar1.DrawYear, calendar1.DrawMonth, 1).AddMonths(1));
			calendar1.SetDate(new DateTime(calendar1.DrawYear, calendar1.DrawMonth, 1));
        }

        private void menuGlobalSetting_Click(object sender, EventArgs e)
		{
			ShowGlobalSettingForm();
		}

		private void ShowGlobalSettingForm()
		{
			FormGlobalSetting form = new FormGlobalSetting();
			//form.StandbyList = Core.StandbyLists.FirstOrDefault(t => t.Year == calendar1.Year && t.Month == calendar1.Month);
			form.Date = this.dateTimePicker1.Value;
			if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				for (int i = 0; i < DateTime.DaysInMonth(calendar1.Year, calendar1.Month); i++)
				{
					calendar1.GetSchedules().ForEach(t =>
						{
							switch (t.Description)
							{
								case "duty":
									t.BackColor = Setting.DutyFieldColor;
									break;
							}
						});
				}
				calendar1.Invalidate();
			}
		}

		private void menuNew_Click(object sender, EventArgs e)
		{
			StandbyList.StandbyList st;

			var stold = Core.StandbyLists.FirstOrDefault(t => t.Year == calendar1.DrawYear && t.Month == calendar1.DrawMonth);
            if (stold != null)
            {
                if (MessageBox.Show("データを初期化してもよろしいですか？", "確認", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    if(MessageBox.Show("設定も初期化してよろしいですか？", "確認", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        Core.StandbyLists.Remove(stold);
                    else
                    {
                        stold.Standby = new DutyPersonsCollection(calendar1.DrawYear, calendar1.DrawMonth);
                        CreateStandbyEmpty();
                        return;
                    }
                }

            }
            
            st = new StandbyList.StandbyList(calendar1.DrawYear, calendar1.DrawMonth);
            Core.Sort();
            var prevst = Core.StandbyLists.LastOrDefault(t => t.Year < st.Year || (t.Year <= st.Year && t.Month < st.Month));
            if (prevst != null)
            {
                if (MessageBox.Show(string.Format("以前の設定({0}年{1}月)を引き続き使用しますか？", prevst.Year, prevst.Month), "確認", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    prevst.Persons.ForEach(p =>
                    {
                        var targetperson = p.Clone() as Person;
                        if (targetperson == null) return;
                        p.Requirement.RepeatRule.ForEach(rule =>
                        {
                            // 有効期限が切れているRuleは削除
                            if (rule.End < new DateTime(calendar1.DrawYear, calendar1.DrawMonth, DateTime.DaysInMonth(calendar1.DrawYear, calendar1.DrawMonth)))
                            {
                                targetperson.Requirement.RepeatRule.Remove(rule);
                            }
                            else
                            {
                                // 新規月の不都合日に当たる日を取得し、不都合日を設定する。
                                var days = rule.GetDays(calendar1.DrawYear, calendar1.DrawMonth);
                                days.Where(day => day > 0 && day <= DateTime.DaysInMonth(calendar1.DrawYear, calendar1.DrawMonth)).ToList().ForEach(day =>
                                {
                                    var date = new DateTime(calendar1.DrawYear, calendar1.DrawMonth, day);
                                    targetperson[date] = PossibleDays.Status.Affair;
                                });
                            }
                        });
                        st.Persons.Add(targetperson);

                    });
                }
                
                Core.StandbyLists.Add(st);
                Core.Sort();
                CreateStandbyEmpty();
                //Core.StandbyLists.Add(st);
            }
			calendar1.Enabled = true;
			btnCreate.Enabled = true;
			mnuCreate.Enabled = true;
			mnuPrint.Enabled = true;
			menuGlobalSetting.Enabled = true;
		}

		private void LoadData()
		{
			calendar1.RemoveAll();
			Serializer.LoadDatas(Application.StartupPath + @"\datas.dbx");
			Core.StandbyLists.ForEach(t =>
				{
					int Year = t.Year;
					int Month = t.Month;
					for (int i = 0; i < DateTime.DaysInMonth(Year, Month); i++)
					{
						CalendarControl.Schedule item = new Schedule();
						item.Start = new DateTime(Year, Month, i + 1);
						item.Item = t.Standby[i+1];
						item.Description = "duty";
                        item.BackColor = Setting.DutyFieldColor;
						item.Alignment = StringAlignment.Center;
						calendar1.AddSchedule(item);
					}
				});
			changeEnable();
		}
        private void LoadSetting()
        {
            Serializer.LoadSettings(Application.StartupPath + @"\setting.xml");
        }

		private void MenuFutugoubiTablePrint_Click(object sender, EventArgs e)
		{
			PrintInitialCalendar();
		}

		private void MenuDutyTablePrint_Click(object sender, EventArgs e)
		{
			Print();
		}

        private void mnuShowAgForm_Click(object sender, EventArgs e)
        {
            ShowAgrregationForm();
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            if(this.WindowState == FormWindowState.Normal)
                Setting.MainFormRect = this.Bounds;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            RemoveHotKey();
        }


        private void mnuOutput_Click(object sender, EventArgs e)
        {
            var st = Core.StandbyLists.FirstOrDefault(t => t.Year == calendar1.Year && t.Month == calendar1.Month);
            if (st == null) return;
            var result = new StringBuilder();
            for (int i = 1; i <= st.Standby.Count; i++)
                result.Append(st.Standby[i] != null ? st.Standby[i].Name : "").AppendLine();
            Clipboard.SetDataObject(result.ToString(), true);

            MessageBox.Show(string.Format("{0}年{1}月の当直データをクリップボードに転送しました。", calendar1.Year, calendar1.Month));
        }


        private void ChangeLabelHeight()
        {
            calendar1.LabelHeight = calendar1.GetPanelHeight(new DateTime(calendar1.Year, calendar1.Month, calendar1.Day)) - calendar1.GetPanelHeaderHeight() - 3;
        }

        private void btnShowDutyList_Click(object sender, EventArgs e)
        {
        }

        private void mnu当直表表示_Click(object sender, EventArgs e)
        {
            var form = new FormDutyList();
            form.Year = calendar1.Year;
            form.Month = calendar1.Month;
            form.StandbyList = Core.StandbyLists.FirstOrDefault(t => t.Year == calendar1.Year && t.Month == calendar1.Month);
            form.Show();

        }
    }
}
