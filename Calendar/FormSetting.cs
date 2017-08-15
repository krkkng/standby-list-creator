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
	public partial class FormSetting : Form
	{
		public Person Person { get; set; }
        public List<StandbyList.StandbyList> StandbyLists { get; set; }
		public int Year { get; set; }
		public int Month { get; set; }
        internal ContextMenuStrip menu { get; set; }

        private List<RepeatRule> AddedRule { get; set; }
        private List<RepeatRule> DeletedRule { get; set; }
		public FormSetting()
		{
			InitializeComponent();
		}

		private void calendar1_ChangeDate(object sender, EventArgs e)
		{

		}

		private void calendar1_DoubleClick(object sender, EventArgs e)
		{
			var schedule = calendar1.GetSchedule(calendar1.SelectedDate);
			CalendarControl.Schedule item = new CalendarControl.Schedule();
			item.Start = calendar1.SelectedDate;
			if (schedule.Count() == 0)
			{
				item.Item = "×";
				calendar1.RemoveSchedule(calendar1.SelectedDate);
				calendar1.AddSchedule(item);
			}
			else if (schedule[0].Item.ToString() == "×")
			{
				item.Item = "△";
				calendar1.RemoveSchedule(calendar1.SelectedDate);
				calendar1.AddSchedule(item);
			}
			else
			{
				calendar1.RemoveSchedule(calendar1.SelectedDate);
			}

			
		}

		private void label2_Click(object sender, EventArgs e)
		{

		}
        /// <summary>
        /// StandbyListsをソートします
        /// </summary>
        private void SortStandbyLists()
        {
            StandbyLists.Sort((x, y) =>
            {
                var datex = new DateTime(x.Year, x.Month, 1);
                var datey = new DateTime(y.Year, y.Month, 1);
                return datex.CompareTo(datey);
            });
        }
		private void FormSetting_Load(object sender, EventArgs e)
		{
            menu = new ContextMenuStrip();
            this.Text = "個人設定";
            SortStandbyLists();
            txtName.Text = Person.Name;
            calendar1.ScheduleLabelSpace = 2;
            calendar1.LabelHeight = 30;

			if (Person != null)
			{
				num1st.Value = Person.Requirement.PossibleTimes;
				numHoliday.Value = Person.Requirement.HolidayPossibleTimes;
				numInterval.Value = Person.Requirement.Interval;
                numHolidayInterval.Value = Person.Requirement.HolidayInterval;
				calendar1.SetDrawDate(Year, Month);
				
				cmbAttr.Items.Add("医員");
				cmbAttr.Items.Add("助教");
				switch (Person.Attre)
				{
					case StandbyList.Person.Attributes.医員:
						cmbAttr.Text = "医員";
						break;
					case StandbyList.Person.Attributes.助教:
						cmbAttr.Text = "助教";
						break;
				}
				PossibleDays.Status status;
				for(int i = 0; i < DateTime.DaysInMonth(Year, Month); i++)
				{
					status = Person[new DateTime(Year, Month, i + 1)];
					CalendarControl.Schedule item;
					DateTime date = new DateTime(Year, Month, i + 1);
					switch (status)
					{
						case PossibleDays.Status.Affair:
							item = new CalendarControl.Schedule();
							item.Start = date;
                            item.BackColor = Color.LightPink;
                            item.LineAlignment = StringAlignment.Center;
                            item.Alignment = StringAlignment.Center;
							item.Item = "×";
							calendar1.AddSchedule(item);
							break;
						case PossibleDays.Status.Limited:
							item = new CalendarControl.Schedule();
							item.Start = date;
                            item.BackColor = Color.LightPink;
                            item.LineAlignment = StringAlignment.Center;
                            item.Alignment = StringAlignment.Center;
							item.Item = "△";
							calendar1.AddSchedule(item);
							break;
						default:
							break;
					}
				}

                // dgvRuleの初期化
                dgvRule.Columns.Add("期限", "期限");
                dgvRule.Columns[(int)RuleIndex.Limit].Width = 130;
                dgvRule.Columns.Add("ルール", "ルール");
                dgvRule.Columns[(int)RuleIndex.Rule].Width = 130;
                dgvRule.Columns.Add("理由", "理由");
                dgvRule.Columns.Add("追加の不都合日", "追加の不都合日");
                dgvRule.Columns[(int)RuleIndex.AddedAffair].Width = 100;
                Person.Requirement.RepeatRule.ForEach(t => setDgv(t));

                // 新しく追加されたルール、削除されたルールを一時的に格納する変数を初期化
                AddedRule = new List<RepeatRule>();
                DeletedRule = new List<RepeatRule>();
                
			}
		}
        public enum RuleIndex
        {
            Limit = 0,
            Rule = 1,
            Reason = 2,
            AddedAffair = 3
        }
        private void setDgv(RepeatRule rule)
        {
            var row = new DataGridViewRow();
            row.CreateCells(dgvRule);
            row.Tag = rule;
            if (rule.RuleClass.Interval != 0)
            {
                if (rule.RuleClass.WeekNumber != RepeatRule.WeekNo.All)
                    row.Cells[(int)RuleIndex.Rule].Value = string.Format("{0}ヶ月おき/第{1}週/{2}", rule.RuleClass.Interval, (int)rule.RuleClass.WeekNumber, rule.RuleClass.DayOfWeek);
                else
                    row.Cells[(int)RuleIndex.Rule].Value = string.Format("{0}ヶ月おき/毎週/{1}", rule.RuleClass.Interval, rule.RuleClass.DayOfWeek);
            }
            else
            {
                if (rule.RuleClass.WeekNumber != RepeatRule.WeekNo.All)
                    row.Cells[(int)RuleIndex.Rule].Value = string.Format("毎月/第{0}週/{1}", (int)rule.RuleClass.WeekNumber, rule.RuleClass.DayOfWeek);
                else
                    row.Cells[(int)RuleIndex.Rule].Value = string.Format("毎月/毎週/{0}", rule.RuleClass.DayOfWeek);
            }
            row.Cells[(int)RuleIndex.Reason].Value = rule.Reason;
            if (rule.End != DateTime.MaxValue)
            {
                row.Cells[(int)RuleIndex.Limit].Value = string.Format("{0}年{1}月～{2}年{3}月", rule.Start.Year, rule.Start.Month, rule.End.Year, rule.End.Month);
            }
            else
                row.Cells[(int)RuleIndex.Limit].Value = string.Format("{0}年{1}月～", rule.Start.Year, rule.Start.Month);
            row.Cells[(int)RuleIndex.AddedAffair].Value = rule.RuleClass.AffairBA.ToString();
            dgvRule.Rows.Add(row);
            dgvRule.Rows[dgvRule.Rows.Count - 1].Selected = true;
        }
        private void setRule(RepeatRule rule)
        {
            // ルールに則ってカレンダーに不都合日を設定する
            var days = rule.GetDays(Year, Month);
            days.AddRange(getBAMonthsAffairDays(Year, Month, rule));
            days.Where(day => day > 0 && day <= DateTime.DaysInMonth(Year, Month)).ToList().ForEach(day =>
            {
                DateTime target = new DateTime(Year, Month, day);
                var schedule = calendar1.GetSchedule(target);
                CalendarControl.Schedule item = new CalendarControl.Schedule();

                item.Start = target;
                if (schedule.Count() == 0)
                {
                    item.Item = "×";
                    item.Alignment = StringAlignment.Center;
                    calendar1.RemoveSchedule(item.Start);
                    calendar1.AddSchedule(item);
                }

            });
            AddedRule.Add(rule);
        }

        private void btnOK_Click(object sender, EventArgs e)
		{
			if (txtName.Text == "")
			{
				MessageBox.Show("名前を入力して下さい");
				return;
			}
			Person.Name = txtName.Text;
			switch(cmbAttr.Text)
			{
				case "医員":
					Person.Attre = StandbyList.Person.Attributes.医員;
					break;
				case "助教":
					Person.Attre = StandbyList.Person.Attributes.助教;
					break;
			}
			Person.Requirement.PossibleTimes = (int)num1st.Value;
			Person.Requirement.HolidayPossibleTimes = (int)numHoliday.Value;
			Person.Requirement.Interval = (int)numInterval.Value;
            Person.Requirement.HolidayInterval = (int)numHolidayInterval.Value;
			if (!Person.Possible.Any(t => t.Year == Year && t.Month == Month))
				Person.Possible.Add(new PossibleDays(Year, Month));
			for(int i = 0; i < DateTime.DaysInMonth(Year, Month); i++)
			{
				DateTime dt = new DateTime(Year, Month, i + 1);
				CalendarControl.Schedule[] schedule = calendar1.GetSchedule(dt);
				if (schedule.Count() != 0 && schedule[0].Item.ToString() == "×")
				{
					Person[new DateTime(Year, Month, i + 1)] = StandbyList.PossibleDays.Status.Affair;
				}
				else if (schedule.Count() != 0 && schedule[0].Item.ToString() == "△")
				{
					Person[new DateTime(Year, Month, i + 1)] = PossibleDays.Status.Limited;
				}
				else
					Person[new DateTime(Year, Month, i + 1)] = StandbyList.PossibleDays.Status.None;
			}
            // RepeatRuleによる他の月における不都合日の設定
            AddedRule.ForEach(t => SetOtherMonthsAffair(t));
            Person.Requirement.RepeatRule.AddRange(AddedRule);
            DeletedRule.ForEach(t => DeleteOtherMonthsRules(t));
			DialogResult = System.Windows.Forms.DialogResult.OK;
			this.Close();
		}

        private void SetOtherMonthsAffair(RepeatRule rule)
        {
            var list = StandbyLists.Where(p => rule.Start <= new DateTime(p.Year, p.Month, 1) && rule.End > new DateTime(p.Year, p.Month, DateTime.DaysInMonth(p.Year, p.Month))).Select(q => new { date = new DateTime(q.Year, q.Month, 1), person = q.Persons.FirstOrDefault(r => r.ID == Person.ID) });

            list.ToList().ForEach(u =>
            {
                // 現在の月はすでに不都合日設定してあるため、飛ばす。
                if (u.date.Year == Year && u.date.Month == Month) return;
                if (u.person == null) return;
                u.person.Requirement.RepeatRule.Add(rule);
                var days = rule.GetDays(u.date.Year, u.date.Month);
                days.AddRange(getBAMonthsAffairDays(u.date.Year, u.date.Month, rule));
                days.Where(day => day > 0 && day <= DateTime.DaysInMonth(u.date.Year, u.date.Month)).ToList().ForEach(day =>
                {
                    u.person[new DateTime(u.date.Year, u.date.Month, day)] = PossibleDays.Status.Affair;
                });
            });

        }
        /// <summary>
        /// 前後の月の不都合日を検索して、今月の不都合日に設定すべき日を取得します
        /// </summary>
        /// <param name="Year">不都合日を取得する対象の年</param>
        /// <param name="Month">不都合日を取得する対象の月</param>
        /// <param name="rule">不都合日を設定するルールを格納したRepeatRuleオブジェクト</param>
        /// <returns></returns>
        private List<int> getBAMonthsAffairDays(int Year, int Month, RepeatRule rule)
        {
            var days = new List<int>();
            var nextmonth = new DateTime(Year, Month, DateTime.DaysInMonth(Year, Month)).AddDays(1);
            var prevmonth = new DateTime(Year, Month, 1).AddDays(-1);
            var eprevdays = rule.GetDays(prevmonth.Year, prevmonth.Month).Where(t =>
            {
                return (t > DateTime.DaysInMonth(prevmonth.Year, prevmonth.Month));
            }).Select(t =>
            {
                return new DateTime(prevmonth.Year, prevmonth.Month, 1).AddDays(t - 1).Day;
            });

            var enextdays = rule.GetDays(nextmonth.Year, nextmonth.Month).Where(t => t < 1).Select(t => nextmonth.AddDays(t - 1).Day);
            days.AddRange(eprevdays);
            days.AddRange(enextdays);
            return days;
        }

        private void calendar1_ChangeDrawDate(object sender, EventArgs e)
		{
			lblCalendar.Text = "不可日：" + calendar1.DrawYear + "年" + calendar1.DrawMonth + "月";
		}

		private void cmbAttr_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

        private void btnAddRule_Click(object sender, EventArgs e)
        {
            var form = new FormRule();
           
            form.Year = Year;
            form.Month = Month;

            if(form.ShowDialog() == DialogResult.OK)
            {
                /*if (checkDuplicateRule(form.Rule))
                {
                    MessageBox.Show("重複した条件は登録できません。", "条件の重複");
                    return;
                }
                */
                //Person.Requirement.RepeatRule.Add(form.Rule);
                //AddedRule.Add(form.Rule);
                setDgv(form.Rule);
                setRule(form.Rule);
            }
        }

        private void DeleteFromCalendar(RepeatRule target)
        {
            // dgvRuleで選択中のRowとそれに対応するRuleを削除する
            dgvRule.Rows.Remove(dgvRule.SelectedRows[0]);
            // 現在表示中のカレンダーの不都合日表示のみ削除する。（実際に削除するのはダイアログがOKされた時）
            var pdays = target.GetDays(Year, Month);
            pdays.AddRange(getBAMonthsAffairDays(Year, Month, target));
            pdays.Where(pday => pday > 0 && pday <= DateTime.DaysInMonth(Year, Month)).ToList().ForEach(pday =>
            {
                var targetdate = new DateTime(Year, Month, pday);
                if (!CheckTargetDate(Year, Month, pday))
                {
                    // 指定した日にちを対象とするルールが存在しなければカレンダーから削除
                    var schedule = calendar1.GetSchedule(targetdate);
                    CalendarControl.Schedule item = new CalendarControl.Schedule();
                    item.Start = targetdate;
                    calendar1.RemoveSchedule(item.Start);
                }
            });
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvRule.SelectedRows.Count == 0) return;
            if (MessageBox.Show("当月以降のルールも全て削除されますがよろしいですか？", "確認", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                var target = dgvRule.SelectedRows[0].Tag as RepeatRule;
                DeleteFromCalendar(target);
                DeletedRule.Add(target);    // 削除対象のルールを一時リストに保存する
            }
        }
        /// <summary>
        /// パラメータ(day)で指定した日にちを対象とするルールが存在しているかどうかをチェックします
        /// </summary>
        /// <param name="day">チェックする日にち</param>
        /// <returns>存在する true、存在しない false</returns>
        private bool CheckTargetDate(int year, int month, int day)
        {
            // 現在の年・月のRuleを対象とする場合
            if (year == this.Year && month == this.Month)
            {
                return dgvRule.Rows.Cast<DataGridViewRow>().Any(t =>
                {
                    var rule = t.Tag as RepeatRule;
                    var tdays = rule.GetDays(year, month);
                    return tdays.Where(tday => tday > 0 && tday <= DateTime.DaysInMonth(year, month)).ToList().Any(tday => tday == day);
                });
            }
            else
            {
                var standby = StandbyLists.FirstOrDefault(t => t.Year == year && t.Month == month);
                if(standby != null)
                {
                    var person = standby.Persons.FirstOrDefault(p => p.ID == Person.ID);
                    return person.Requirement.RepeatRule.Any(rule =>
                    {
                        var tdays = rule.GetDays(year, month);
                        return tdays.Where(tday => tday > 0 && tday <= DateTime.DaysInMonth(year, month)).ToList().Any(tday => tday == day);
                    });
                }
            }
            return false;
        }
        /// <summary>
        /// ルールの削除を実行します
        /// </summary>
        /// <param name="target">削除する対象のルール</param>
        private void DeleteOtherMonthsRules(RepeatRule target)
        {
            // 今月よりも先のルールを匿名型(person, date)の形式でリストする。
            var plist = StandbyLists.Where(sl => new DateTime(sl.Year, sl.Month, DateTime.DaysInMonth(sl.Year, sl.Month)) > new DateTime(Year, Month, DateTime.DaysInMonth(Year, Month))).Select(st => new { person = st.Persons.FirstOrDefault(p => p.ID == Person.ID), date = new DateTime(st.Year, st.Month, 1) });
            // 今月よりも前のルールを匿名型(person, date)の形式でリストする。
            var blist = StandbyLists.Where(li => new DateTime(li.Year, li.Month, 1) < new DateTime(Year, Month, 1)).Select(st => new { person = st.Persons.FirstOrDefault(p => p.ID == Person.ID), date = new DateTime(st.Year, st.Month, 1) });
            var tdays = target.GetDays(Year, Month);
            tdays.AddRange(getBAMonthsAffairDays(Year, Month, target));
            tdays.Where(tday => tday > 0 && DateTime.DaysInMonth(Year, Month) >= tday).ToList().ForEach(tday =>
            {
                // 現在対象の個人のRepeatRuleを書き換え
                var tdate = new DateTime(Year, Month, tday);
                if (!CheckTargetDate(Year, Month, tday))
                    Person[tdate] = PossibleDays.Status.None;
            });
            // 今月より前のルールの終了日(End)を書き換える
            blist.ToList().ForEach(b =>
            {
                if (b.person == null) return;
                var findtarget = b.person.Requirement.RepeatRule.Find(f => f == target);
                if (findtarget != null)
                    findtarget.End = new DateTime(Year, Month, 1).AddDays(-1);
            });
            // 今月より先のルールを削除し、不都合日を削除する
            plist.ToList().ForEach(p =>
            {
                if (p.person == null) return;
                p.person.Requirement.RepeatRule.Remove(target);
                var days = target.GetDays(p.date.Year, p.date.Month);
                days.Where(day => day > 0 && DateTime.DaysInMonth(p.date.Year, p.date.Month) >= day).ToList().ForEach(day =>
                {
                    var tdate = new DateTime(p.date.Year, p.date.Month, day);
                    if (!CheckTargetDate(tdate.Year, tdate.Month, day))
                        p.person[tdate] = PossibleDays.Status.None;
                });
            });
            // 個人のRepeatRuleリストからルールを削除します
            Person.Requirement.RepeatRule.Remove(target);
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvRule.SelectedRows.Count == 0) return;
            var form = new FormRule();
            form.Year = Year;
            form.Month = Month;
            form.Rule = dgvRule.SelectedRows[0].Tag as RepeatRule;
            if(form.ShowDialog() == DialogResult.OK)
            {
                var target = dgvRule.SelectedRows[0].Tag as RepeatRule;
                DeleteFromCalendar(target);
                DeletedRule.Add(target);

                setDgv(form.Rule);
                setRule(form.Rule);

            }
        }

        private void calendar1_ScheduleLabelMouseUp(object sender, CalendarControl.Calendar.ScheduleLabelMouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                menu.Items.Clear();
                menu.Items.Add("新しいルールを作成", null, MenuSetRuleClick);
                this.ContextMenuStrip = menu;
				this.ContextMenuStrip.Show(Cursor.Position);//PointToScreen(new Point(e.X, e.Y)));
            }
        }

        private void MenuSetRuleClick(object sender, EventArgs e)
        {
			var form = new FormRule();
			form.Year = Year;
			form.Month = Month;
			form.Rule = new RepeatRule(Year, Month, new RepeatRule.MonthRule());
			form.Rule.RuleClass.WeekNumber = RepeatRule.GetWeekNo(calendar1.SelectedDate);
			form.Rule.RuleClass.DayOfWeek = (RepeatRule.DayOfWeek)calendar1.SelectedDate.DayOfWeek;
			form.Rule.RuleClass.Interval = 0;
			if(form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				//Person.Requirement.RepeatRule.Add(form.Rule);
                //AddedRule.Add(form.Rule);
				setDgv(form.Rule);
				setRule(form.Rule);
			}
        }

        private void dgvRule_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView dgview = sender as DataGridView;
            if (dgview.SelectedRows.Count > 0)
            {
                RepeatRule rule = dgview.SelectedRows[0].Tag as RepeatRule;
                var days = rule.GetDays(Year, Month);
                days.AddRange(getBAMonthsAffairDays(Year, Month, rule));

                calendar1.GetSchedules().ForEach(s => {
                    if (s.BackColor != Setting.DutyFieldColor)
                        s.BackColor = Setting.DutyFieldColor;
                    if (s.Item != null)
                    {
                        if (days.Any(t => t == s.Start.Day))
                            s.BackColor = Setting.DutyHilightColor;
                    }
                });
            }

        }

        private void dgvRule_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnEdit.PerformClick();
        }
    }
}
