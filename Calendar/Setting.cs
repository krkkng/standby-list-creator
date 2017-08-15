using CalendarControl.GenCalendar;
using StandbyList;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calendar
{
	public static class Setting
	{
		public static Color DutyFieldColor = Color.LightPink;

		public static int TryCount = 1;
		public static string FooterMessage = "";
		public static string DepartmentString = "第一内科";
        public static Color DutyHilightColor = Color.GreenYellow;

        public static Rectangle MainFormRect { get; set; }
        public static FormWindowState MainFormState { get; set; }
        public static Rectangle AggregationFormRect { get; set; }
        public static FormWindowState AggregationFormState { get; set; }
        public static Rectangle FormAllSchedulesRect { get; set; }
        public static FormWindowState FormAllSchedulesState { get; set; }
        public static Rectangle FormGlobalSettingRect { get; set; }
        public static FormWindowState FormGlobalSettingState { get; set; }
        public static Rectangle FormRuleRect { get; set; }
        public static FormWindowState FormRuleState { get; set; }
        public static Rectangle FormSettingRect { get; set; }
        public static FormWindowState FormSettingState { get; set; }
        public static Rectangle FormSubFormRect { get; set; }
        public static FormWindowState FormSubFormState { get; set; }
        // 集計の期間(AggregationFormのtoolstrip)
        public static AggregationForm.DateInterval DateInterval { get; set; }

    }

	public static class Core
	{
		public static List<StandbyList.StandbyList> StandbyLists { get; set; }
		public static void Sort()
		{
			StandbyLists.Sort((x, y) =>
				{
					var datex = new DateTime(x.Year, x.Month, 1);
					var datey = new DateTime(y.Year, y.Month, 1);
					return datex.CompareTo(datey);
				});
		}
        /// <summary>
        /// 総負荷を計算します。（平日当直1、休日当直2の割合で計算）
        /// </summary>
        /// <param name="persons">計算する月のPersonリスト</param>
        /// <param name="date">計算する月</param>
        /// <param name="backnum">何ヶ月前までさかのぼって計算するか</param>
        /// <param name="containpresentmonth">計算する月も計算に含めるかどうか。含める場合はtrue、含めない場合はfalse</param>
		public static void TotalBurden(List<Person> persons, DateTime date, int backnum, bool containpresentmonth)
		{
			persons.ForEach(p => p.TotalBurden = 0);
			// 総負荷を計算
			int start = containpresentmonth ? 0 : 1;
			for (int i = start; i <= backnum; i++)
			{
				DateTime tdate = date.AddMonths(-1 * i);
				StandbyList.StandbyList st2 = Core.StandbyLists.FirstOrDefault(t => t.Year == tdate.Year && t.Month == tdate.Month);
				if (st2 != null)
				{
					persons.ForEach(t =>
						{
							t.TotalBurden += st2.DutyCount(t);
						});
				}
			}

		}
        /// <summary>
        /// 総負荷を計算します。（平日当直1, 休日当直2の割合で計算)
        /// </summary>
        /// <param name="persons">計算する月のPersonリスト</param>
        /// <param name="targetdate">計算する月</param>
        /// <param name="criteriadate">基準となる月</param>
        /// <param name="containpresentmonth">計算する月も計算に含めるかどうか。</param>
        public static void TotalBurden(List<Person> persons, DateTime targetdate, DateTime criteriadate, bool containpresentmonth)
        {
            int backnum = MonthDiff(targetdate, criteriadate);
            TotalBurden(persons, targetdate, backnum, containpresentmonth);
        }

        /// <summary>
        /// 2つの日付の月差を求める(http://oresi.hatenablog.com/entry/2014/03/14/120211)
        /// </summary>
        ///対象日付1
        ///対象日付2
        /// <returns></returns>
        public static int MonthDiff(DateTime dTime1, DateTime dTime2)
        {
            int iRet = 0;
            DateTime dtFrom = DateTime.MinValue;
            DateTime dtTo = DateTime.MaxValue;

            if (dTime1 < dTime2)
            {
                dtFrom = dTime1;
                dtTo = dTime2;
            }
            else
            {
                dtFrom = dTime2;
                dtTo = dTime1;
            }

            // 月差計算（年差考慮(差分1年 → 12(ヶ月)加算)）
            iRet = (dtTo.Month + (dtTo.Year - dtFrom.Year) * 12) - dtFrom.Month;

            return iRet;
        }

        /*public static List<LimitedPerson> PossibleList(DateTime dt, DutyPersonsCollection sp)
        {
            List<LimitedPerson> tmp = new List<LimitedPerson>();
            int day = dt.Day - 1;
            DateTime prevm = dt.AddMonths(-1);
            StandbyList.StandbyList st = Core.StandbyLists.FirstOrDefault(t => t.Year == dt.Year && t.Month == dt.Month);
            StandbyList.StandbyList stp = Core.StandbyLists.FirstOrDefault(t => t.Year == prevm.Year && t.Month == prevm.Month);
            if(st != null)
            {
                st.Persons.ForEach(p =>
                {
                    if (p.ID != sp[day].ID)
                    {
                        LimitedPerson lp = new LimitedPerson(p);
                        if (p[dt] == PossibleDays.Status.Affair)
                        {
                            lp.LimitStatus = LimitedPerson.Limit.Cannot;
                            lp.Messages.Add("不都合日です");
                        }
                        else
                        {
                            if (p[dt] == PossibleDays.Status.Limited)
                            {
                                lp.LimitStatus = LimitedPerson.Limit.Limited;
                                lp.Messages.Add("当直可能時間に制限があります");
                            }
                            if (HolidayChecker.IsHoliday(dt) && st.HolidayCount(p) >= p.Requirement.HolidayPossibleTimes)
                            {
                                lp.LimitStatus = LimitedPerson.Limit.Limited;
                                lp.Messages.Add("当直の休日可能数を越えています");
                            }
                            if (!HolidayChecker.IsHoliday(dt) && st.OrdinaryDutyCount(p) >= p.Requirement.PossibleTimes)
                            {
                                lp.LimitStatus = LimitedPerson.Limit.Limited;
                                lp.Messages.Add("平日当直の制限数を越えています");
                            }
                            if (HolidayChecker.IsHoliday(dt) && (stp.HolidayCount(p) > 0))
                            {
                                lp.LimitStatus = LimitedPerson.Limit.Limited;
                                lp.Messages.Add("前月に休日当直を行っています");
                            }
                            if(CheckInterval(p, dt, st.Standby))
                            {
                                lp.LimitStatus = LimitedPerson.Limit.Limited;
                                lp.Messages.Add("当直の間隔が制限を越えています");
                            }
                        }
                        tmp.Add(lp);
                    }
                });
            }
            return tmp;
        }
        /// <summary>
        /// 指定した前後のインターバルに担当になっている日がないかをチェックします。
        /// </summary>
        /// <param name="p"></param>
        /// <param name="dt"></param>
        /// <param name="sp"></param>
        /// <returns>担当がある true, 担当がない false</returns>
        public static bool CheckInterval(Person p, DateTime dt, DutyPersonsCollection sp)
        {
            int interval = p.Requirement.Interval;
            for (int i = 0; i < interval; i++)
            {
                DateTime checkdatep = dt.AddDays(-1 * (i + 1));
                DateTime checkdatef = dt.AddDays(i + 1);
                DutyPersonsCollection dp_p = null;
                DutyPersonsCollection dp_f = null;
                if (checkdatep.Year != dt.Year || checkdatep.Month != dt.Month)
                {
                    StandbyList.StandbyList st_p = StandbyLists.FirstOrDefault(t => t.Year == checkdatep.Year && t.Month == checkdatep.Month);
                    if (st_p != null)
                        dp_p = st_p.Standby;
                }
                else
                    dp_p = sp;
                if (checkdatef.Year != dt.Year || checkdatef.Month != dt.Month)
                {
                    StandbyList.StandbyList st_f = StandbyLists.FirstOrDefault(t => t.Year == checkdatef.Year && t.Month == checkdatef.Month);
                    if (st_f != null)
                        dp_f = st_f.Standby;
                }
                else
                    dp_f = sp;

                int checkdaypindex = checkdatep.Day - 1;
                int checkdayfindex = checkdatef.Day - 1;
                if (dp_p != null)
                {
                    if (dp_p[checkdaypindex] != null)
                    {
                        if (dp_p[checkdaypindex] != null && dp_p[checkdaypindex].ID == p.ID)
                            return true;
                    }
                }
                if (dp_f != null)
                {
                    if (dp_f[checkdayfindex] != null)
                    {
                        if (dp_f[checkdayfindex] != null && dp_f[checkdayfindex].ID == p.ID)
                            return true;
                    }
                }
            }
            return false;
        }
        */
	}
}
