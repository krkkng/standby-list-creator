using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using CalendarControl.GenCalendar;
using System.Globalization;

namespace StandbyList
{
    public class StandbyList : ICloneable
	{
		public int Year { get; private set; }
		public int Month { get; private set; }
		public List<Person> Persons { get; set; }
		public DutyPersonsCollection Standby { get; set; }
		public StandbyList(int year, int month)
		{
			Persons = new List<Person>();
			Year = year;
			Month = month;
			Standby = new DutyPersonsCollection(year, month);
		}
        public void Clear()
        {
            Standby.Clear();
        }
        public int HolidayCount(Person person)
        {
            int result = 0;
            for(int i = 0; i < Standby.Count; i++)
            {
                if (Standby[i+1] != null && Standby[i+1].ID == person.ID)
                    if (HolidayChecker.IsHoliday(new DateTime(Year, Month, i + 1)))
                        result++;
            }
            return result;
        }

        public static int HolidayCount(int _year, int _month, DutyPersonsCollection dp, Person person)
        {
            int result = 0;
            for (int i = 0; i < dp.Count; i++)
            {
                if (dp[i+1] != null && dp[i+1].ID == person.ID)
                    if (HolidayChecker.IsHoliday(new DateTime(_year, _month, i + 1)))
                        result++;
            }
            return result;
        }

        public int OrdinaryDutyCount(Person person)
        {
            int result = 0;
            for(int i = 0; i < Standby.Count; i++)
            {
                if (Standby[i+1] != null && Standby[i+1].ID == person.ID)
                    if (!HolidayChecker.IsHoliday(new DateTime(Year, Month, i + 1)))
                        result++;
            }
            return result;
        }

        public static int OrdinaryDutyCount(int _year, int _month, DutyPersonsCollection dp, Person person)
        {
            int result = 0;
            for (int i = 0; i < dp.Count; i++)
            {
                if (dp[i+1] != null && dp[i+1].ID == person.ID)
                    if (!HolidayChecker.IsHoliday(new DateTime(_year, _month, i + 1)))
                        result++;
            }
            return result;
        }
		/// <summary>
		/// 指定したpersonの当直の総数を数えます。休日当直は平日2回分に相当。
		/// </summary>
		/// <param name="person"></param>
		/// <returns></returns>
		public int DutyCount(Person person)
		{
			int result = 0;
			for (int i = 0; i < Standby.Count; i++ )
			{ 
				if(Standby[i+1] != null)
				{
					if(Standby[i+1].ID == person.ID)
					{
						if(HolidayChecker.IsHoliday(new DateTime(Year, Month, i + 1)))
							result += 2; // 休日当直は平日2回分として換算。
						else
							result++;
					}

				}
			}
			return result;
		}

        /// <summary>
        /// DutyListを取得します
        /// </summary>
        /// <returns>勤務先のリスト</returns>
        public IEnumerable<string> GetDuties()
        {
            var duties = new List<string>();
            Persons.ForEach(t =>
            {
                t.Requirement.RepeatRule.ForEach(u =>
                {
                    if (u.Accessory1 != global::StandbyList.Accessory.None)
                    {
                        duties.Add(u.Reason);
                    }
                });
            });
            // dutyのリストの重複を削除する
            return duties.Distinct();
        }

        /// <summary>
        /// 勤務先に一致したPersonの名前、RepeatRuleを取得します(ただし、勤務形態でNoneが選択してある場合を除く）
        /// </summary>
        /// <param name="Duty">勤務先の名称</param>
        /// <returns></returns>
        public IEnumerable<Tuple<string, RepeatRule>> GetDayAndAccessory(string Duty)
        {
            var tpl = new List<Tuple<string, RepeatRule>>();
            Persons.ForEach(t =>
            {
                t.Requirement.RepeatRule.ForEach(u =>
                {
                    if (u.Reason == Duty && (u.Accessory1 != Accessory.None || u.Accessory2 != Accessory.None))
                    {
                        tpl.Add(new Tuple<string, RepeatRule>(t.Name, u));
                    }
                });
            });
            return tpl;
        }
        public static int DutyCount(int _year, int _month, Person person, DutyPersonsCollection dp)
        {
            int result = 0;
            for(int i = 0; i < dp.Count; i++)
            {
                if(dp[i+1] != null)
                {
                    if(dp[i+1].ID == person.ID)
                    {
                        if (HolidayChecker.IsHoliday(new DateTime(_year, _month, i + 1)))
                            result += 2;
                        else
                            result++;
                    }
                }
            }
            return result;
        }
		public void Create(List<StandbyList> standbylists)
		{
			
			Standby = create2(standbylists);
		}

		public void CreateEmpty()
		{
			/*Persons.ForEach(p =>
			{
				p.HolidayCount = 0;
				p.OrdinaryDutyCount = 0;
			});*/
            Standby.Clear();
		}
        /// <summary>
        /// 指定した日にちに、Personが当直可能かどうかを調べます
        /// </summary>
        /// <param name="dt">チェックする対象の日にち</param>
        /// <param name="person">チェックする対象のPerson</param>
        /// <param name="sp">チェックする対象月のDutyPersonsCollection</param>
        /// <param name="StandbyLists">全ての月のStandbyListが格納されたList</param>
        /// <returns>当直可能かどうかを示すLimitedPersonオブジェクト</returns>
        public LimitedPerson PossiblePerson(DateTime dt, Person person, DutyPersonsCollection sp, List<StandbyList> StandbyLists)
        {
            int day = dt.Day;
            if (person == null) return null;
            DateTime date = new DateTime(Year, Month, 1);
            DateTime postdate = date.AddMonths(-person.Requirement.HolidayInterval);
            StandbyLists.Sort((x, y) =>
            {
                var datex = new DateTime(x.Year, x.Month, 1);
                var datey = new DateTime(y.Year, y.Month, 1);
                return datex.CompareTo(datey);
            });
            int takecnt = person.Requirement.HolidayInterval;
            var one = StandbyLists.SkipWhile(t => postdate > new DateTime(t.Year, t.Month, 1)).ToList();
            var two = one.Take(takecnt).ToList();
            bool isholidayinterval = two.Any(t =>
            {
                var p = t.Persons.FirstOrDefault(pp => pp.ID == person.ID);
                if (p != null)
                    return p.HolidayCount > 0;
                else
                    return false;
            });

            //bool isholidayinterval = StandbyLists.SkipWhile(t => t.Year == postdate.Year && t.Month == postdate.Month).Take(takecnt).Any(t => t.Persons.FirstOrDefault(pp => pp.ID == person.ID) != null ? t.Persons.FirstOrDefault(pp => pp.ID == person.ID).HolidayCount > 0 : false);
            if (sp[day] == null || person.ID != sp[day].ID)
            {
                LimitedPerson lp = new LimitedPerson(person);
                if (person[dt] == PossibleDays.Status.Affair)
                {
                    lp.LimitStatus = LimitedPerson.Limit.Cannot;
                    lp.Messages.Add("不都合日です");
                }
                else
                {
                    if (person[dt] == PossibleDays.Status.Limited)
                    {
                        lp.LimitStatus = LimitedPerson.Limit.Limited;
                        lp.Messages.Add("当直可能時間に制限があります");
                    }
                    if (HolidayChecker.IsHoliday(dt) && person.HolidayCount >= person.Requirement.HolidayPossibleTimes)
                    {
                        lp.LimitStatus = LimitedPerson.Limit.Limited;
                        lp.Messages.Add("当直の休日可能数を越えています");
                    }
                    if (!HolidayChecker.IsHoliday(dt) && person.OrdinaryDutyCount >= person.Requirement.PossibleTimes)
                    {
                        lp.LimitStatus = LimitedPerson.Limit.Limited;
                        lp.Messages.Add("平日当直の制限数を越えています");
                    }
                    if (!HolidayChecker.IsHoliday(dt) && person.HolidayCount > 0)
                    {
                        lp.LimitStatus = LimitedPerson.Limit.Limited;
                        lp.Messages.Add("休日当直が1回以上選択されています");
                    }
                    if (HolidayChecker.IsHoliday(dt) && person.OrdinaryDutyCount > 0)
                    {
                        lp.LimitStatus = LimitedPerson.Limit.Limited;
                        lp.Messages.Add("平日当直が1回以上選択されています");
                    }
                    //if (HolidayChecker.IsHoliday(dt) && (stp != null && stp.Persons.FirstOrDefault(pp => pp.ID == p.ID) != null ? stp.Persons.First(pp => pp.ID == p.ID).HolidayCount > 0 : false))
                    if (HolidayChecker.IsHoliday(dt) && isholidayinterval)
                    {
                        lp.LimitStatus = LimitedPerson.Limit.Limited;
                        lp.Messages.Add("過去" + person.Requirement.HolidayInterval + "ヶ月間に1回以上休日当直を行っています");
                    }
                    if (CheckInterval(person, dt, sp, StandbyLists))
                    {
                        lp.LimitStatus = LimitedPerson.Limit.Limited;
                        lp.Messages.Add("当直の間隔が制限を越えています");
                    }
                }
                return lp;
            }
            return null;

        }
        /// <summary>
        /// 指定した日にちの当直可能なPersonのリストを作成します
        /// </summary>
        /// <param name="dt">チェックする対象の日にち</param>
        /// <param name="person">チェックする対象のPerson</param>
        /// <param name="sp">チェックする対象月のDutyPersonsCollection</param>
        /// <param name="StandbyLists">全ての月のStandbyListが格納されたList</param>
        /// <returns>当直可能かどうかを示すLimitedPersonオブジェクト</returns>
        public List<LimitedPerson> PossibleList(DateTime dt, DutyPersonsCollection sp, List<StandbyList> StandbyLists)
		{
            List<LimitedPerson> tmp = new List<LimitedPerson>();
            int day = dt.Day;
            //DateTime prevm = dt.AddMonths(-1);
            StandbyList st = StandbyLists.FirstOrDefault(t => t.Year == dt.Year && t.Month == dt.Month);
            //StandbyList stp = StandbyLists.FirstOrDefault(t => t.Year == prevm.Year && t.Month == prevm.Month);
            
            if (st != null)
            {
                st.Persons.ForEach(p =>
                {
                    /*DateTime date = new DateTime(Year, Month, 1);
                    DateTime postdate = date.AddMonths(-p.Requirement.HolidayInterval);
                    int takecnt = p.Requirement.HolidayInterval;
                    bool isholidayinterval = StandbyLists.SkipWhile(t => t.Year == postdate.Year && t.Month == postdate.Month).Take(takecnt).Any(t => t.Persons.FirstOrDefault(pp => pp.ID == p.ID) != null ? t.Persons.FirstOrDefault(pp => pp.ID == p.ID).HolidayCount > 0 : false);
                    if (sp[day] == null || p.ID != sp[day].ID)
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
                            if (HolidayChecker.IsHoliday(dt) && p.HolidayCount >= p.Requirement.HolidayPossibleTimes)
                            {
                                lp.LimitStatus = LimitedPerson.Limit.Limited;
                                lp.Messages.Add("当直の休日可能数を越えています");
                            }
                            if (!HolidayChecker.IsHoliday(dt) && p.OrdinaryDutyCount >= p.Requirement.PossibleTimes)
                            {
                                lp.LimitStatus = LimitedPerson.Limit.Limited;
                                lp.Messages.Add("平日当直の制限数を越えています");
                            }
                            if(!HolidayChecker.IsHoliday(dt) && p.HolidayCount > 0)
                            {
                                lp.LimitStatus = LimitedPerson.Limit.Limited;
                                lp.Messages.Add("休日当直が1回以上選択されています");
                            }
                            if(HolidayChecker.IsHoliday(dt) && p.OrdinaryDutyCount > 0)
                            {
                                lp.LimitStatus = LimitedPerson.Limit.Limited;
                                lp.Messages.Add("平日当直が1回以上選択されています");
                            }
                            //if (HolidayChecker.IsHoliday(dt) && (stp != null && stp.Persons.FirstOrDefault(pp => pp.ID == p.ID) != null ? stp.Persons.First(pp => pp.ID == p.ID).HolidayCount > 0 : false))
                            if(HolidayChecker.IsHoliday(dt) && isholidayinterval)
                            {
                                lp.LimitStatus = LimitedPerson.Limit.Limited;
                                lp.Messages.Add("過去" + p.Requirement.HolidayInterval + "ヶ月間に1回以上休日当直を行っています");
                            }
                            if (CheckInterval(p, dt, sp, StandbyLists))
                            {
                                lp.LimitStatus = LimitedPerson.Limit.Limited;
                                lp.Messages.Add("当直の間隔が制限を越えています");
                            }
                        }
                        */
                        LimitedPerson lp = PossiblePerson(dt, p, sp, StandbyLists);
                        if(lp != null)
                            tmp.Add(lp);
                });
            }
            return tmp;

            //List<LimitedPerson> tmp = new List<LimitedPerson>();
            //int date = dt.Day - 1;


            //   Persons.ForEach(p =>
            //    {
            //	    if (p != sp[date].Duty)
            //	    {
            //		    LimitedPerson limitp = new LimitedPerson(p);
            //		    if (p[dt] == PossibleDays.Status.Affair)
            //		    {
            //			    limitp.LimitStatus = LimitedPerson.Limit.Cannot;
            //			    limitp.Messages.Add("不都合日です");
            //		    }
            //		    else
            //		    {
            //			    if(p[dt] == PossibleDays.Status.Limited)
            //			    {
            //				    limitp.LimitStatus = LimitedPerson.Limit.Limited;
            //				    limitp.Messages.Add("当直可能時間に制限があります");
            //			    }
            //			    if (HolidayChecker.IsHoliday(dt) &&
            //				    StandbyList.HolidayCount(dt.Year, dt.Month, sp, p) >= p.Requirement.HolidayPossibleTimes)
            //			    {
            //				    limitp.LimitStatus = LimitedPerson.Limit.Limited;
            //				    limitp.Messages.Add("当直の休日可能数を越えています");
            //			    }
            //			    if (!HolidayChecker.IsHoliday(dt) && StandbyList.OrdinaryDutyCount(dt.Year, dt.Month, sp, p) >= p.Requirement.PossibleTimes)
            //			    {
            //				    limitp.LimitStatus = LimitedPerson.Limit.Limited;
            //				    limitp.Messages.Add("当直の制限数を超えています");
            //			    }
            //			    if (checkInterval(p, dt, sp))
            //			    {
            //				    limitp.LimitStatus = LimitedPerson.Limit.Limited;
            //				    limitp.Messages.Add("当直の間隔が制限を越えています");
            //			    }

            //		    }
            //		    tmp.Add(limitp);
            //	    }
            //    });

            //return tmp;
        }
        
                /// <summary>
        /// 指定した前後のインターバルに担当になっている日がないかをチェックします。
        /// </summary>
        /// <param name="p"></param>
        /// <param name="dt"></param>
        /// <param name="sp"></param>
        /// <returns>担当がある true, 担当がない false</returns>
        public bool CheckInterval(Person p, DateTime dt, DutyPersonsCollection sp, List<StandbyList> StandbyLists)
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
                    StandbyList st_p = StandbyLists.FirstOrDefault(t => t.Year == checkdatep.Year && t.Month == checkdatep.Month);
                    if (st_p != null)
                        dp_p = st_p.Standby;
                }
                else
                    dp_p = sp;
                if (checkdatef.Year != dt.Year || checkdatef.Month != dt.Month)
                {
                    StandbyList st_f = StandbyLists.FirstOrDefault(t => t.Year == checkdatef.Year && t.Month == checkdatef.Month);
                    if (st_f != null)
                        dp_f = st_f.Standby;
                }
                else
                    dp_f = sp;

                int checkdaypindex = checkdatep.Day;
                int checkdayfindex = checkdatef.Day;
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
    

    public DutyPersonsCollection create2(List<StandbyList> standbylists)
        {
            DutyPersonsCollection result = new DutyPersonsCollection(Year, Month);
            List<Person> Persons = new List<Person>();
            StandbyList st = standbylists.FirstOrDefault(t => t.Year == Year && t.Month == Month);
            Random rand1 = new Random(Environment.TickCount);
            int maxday = DateTime.DaysInMonth(Year, Month);
            // 不都合な医師が多い日から優先的に決めていく）
            DayCounter[] day = new DayCounter[maxday];
            for (int i = 0; i < maxday; i++)
            {
                DateTime date = new DateTime(Year, Month, i + 1);
                day[i] = new DayCounter() { Date = date, Counter = 0 };
                st.Persons.ForEach(p =>
                {
                    if (p[date] != PossibleDays.Status.Affair || p[date] != PossibleDays.Status.Limited)
                        day[i].Counter++;
                });
            }
            day = day.OrderBy(d => d.Counter).ToArray();
            //Array.Sort(day, (x, y) => x.Counter - y.Counter); //優先的に担当を決める日付順に並べる。

            // アルゴリズム
            for (int i = 0; i < maxday; i++)
            {
                List<Person> tmp = new List<Person>();
                DateTime date = day[i].Date;
                Person select = null;

                if (result[date.Day] == null)
                {
                    // 当直が可能な人のリストを作る
                    tmp = PossibleList(date, result, standbylists).Where(p => p.LimitStatus == LimitedPerson.Limit.None).Select(t => t.Person).ToList();
                    // tmpをシャッフル (cf.)http://dobon.net/vb/dotnet/programing/arrayshuffle.html
                    tmp = tmp.OrderBy(u => Guid.NewGuid()).ToList();
                    if (tmp.Count == 0)
                    { }
                    else if (tmp.Count == 1)
                    {
                        select = tmp[0];
                        result.SetDuty(date.Day, select);
                    }
                    else
                    {
                        List<Person> duplicatelist = new List<Person>();
                        // まずは助教のみで決める
                        if (tmp.Any(p => p.Attre == Person.Attributes.助教))
                        {
                            for (int j = 0; j < tmp.Count(); j++)
                            {
                                int dutycount = tmp[j].OrdinaryDutyCount;  // 今月のdutyが何回入っているかを数える。
                                if (tmp[j].Attre == Person.Attributes.助教)
                                {
                                    for (int k = 0; k < tmp[j].Requirement.PossibleTimes - dutycount; k++)
                                    {
                                        duplicatelist.Add(tmp[j]);
                                    }
                                }
                            }
                            if (duplicatelist.Count != 0)
                            {
                                int con = rand1.Next(duplicatelist.Count);
                                select = duplicatelist[con];
                                result.SetDuty(date.Day, select);
                            }
                        }
                        // 助教のみで決まらなければ、医員で決める。
                        if (result[date.Day] == null)
                        {
                            // total burdenが低い人から選択
                            while (true)
                            {
                                var minburden = tmp.Min(t => t.HolidayCount * 2 + t.OrdinaryDutyCount);
                                var minlist = tmp.Where(t => t.HolidayCount * 2 + t.OrdinaryDutyCount == minburden).ToList();
                                //minlist.Sort((x, y) => StandbyList.DutyCount(date.Year, date.Month, x, result) - StandbyList.DutyCount(date.Year, date.Month, y, result));
                                if (minlist.Count() != 0)
                                {
                                    //int con = rand1.Next(minlist.Count());
                                    select = minlist[0];
                                }
                                if (select != null)
                                    break;
                            }
                            result.SetDuty(date.Day, select);


                        }
                        /*if (result[date.Day - 1].Duty == null)
                        { 
                            if (HolidayChecker.IsHoliday(date))
                            {
                                for (int j = 0; j < tmp.Count(); j++)
                                {
                                    int holidaycount = StandbyList.HolidayCount(Year, Month, result, tmp[j]);
                                    for (int k = 0; k < tmp[j].Requirement.HolidayPossibleTimes - holidaycount; k++)
                                    {
                                        duplicatelist.Add(tmp[j]);
                                    }
                                }

                            }
                            else
                            {
                                for (int j = 0; j < tmp.Count(); j++)
                                {
                                    int dutycount = StandbyList.OrdinaryDutyCount(Year, Month, result, tmp[j]);
                                    for (int k = 0; k < tmp[j].Requirement.PossibleTimes - dutycount; k++)
                                    {
                                        duplicatelist.Add(tmp[j]);
                                    }
                                }
                            }
                            // duplicatelistの中からランダムで選択
                            if (duplicatelist.Count != 0)
                            {
                                int con = rand1.Next(duplicatelist.Count);
                                select = duplicatelist[con];

                                result[date.Day - 1].Duty = select;
                            }
                        }
                        */

                    }
                }


            }
            return result;
        }


		public struct DayCounter
		{
			public int Counter;
			public DateTime Date;
		}


        /*
        /// <summary>
        /// 指定した日付(date)にpersonを設定する場合、条件に合致するかどうかを調べます。
        /// </summary>
        /// <param name="date">調べる日付</param>
        /// <param name="person">調べる対象のPerson</param>
        /// <returns>条件に合致する場合はパラメータに設定したpersonを、合致しない場合はnullを返します</returns>
        public Person CheckPerson(DateTime date, Person person)
        {
            bool isHoliday = HolidayChecker.IsHoliday(date);
            Person result = null;
            if (isHoliday)
            {
                // 前の月に休日当直が入っていないことと、休日の当直制限回数を超えていないかどうかを確認
                if (!person.IsPrevMonthDutyInHoliday &&
                    (person.HolidayCounter < person.Requirement.HolidayPossibleTimes) &&
                    (person.DutyCounter < person.Requirement.PossibleTimes))
                {
                    if (person.DutyCounter == 0)
                    {
                        result = person;
                        person.HolidayCounter++;
                    }
                    else
                        result = null;
                }
                else
                {
                    result = null;
                }
            }
            else
            {
                if (person.DutyCounter < person.Requirement.PossibleTimes)
                {
                    if (person.HolidayCounter == 0)
                    {
                        result = person;
                        result.DutyCounter++;
                    }
                    else
                        result = null;
                }
                else
                    result = null;
            }
            return result;
        }
        */
		public void SetDate(int year, int month)
		{
			if (Year < DateTime.MinValue.Year && Year > DateTime.MaxValue.Year &&
				Month <= 0 && Month > 12)
				throw new ArgumentOutOfRangeException();
			Year = year;
			Month = month;
			Persons.ForEach(p =>
				{
					PossibleDays pd = new PossibleDays(year, month);
					p.Possible.Add(pd);
				});
		}
		public Tuple<double, double> get1stVariance(Person person)
		{
			List<double> dayinterval = new List<double>();
			int counter = 0;
			if (Standby != null)
			{
				for (int i = 0; i < Standby.Count; i++)
				{
					counter++;
					if (Standby[i+1] != null &&  Standby[i+1].ID == person.ID)
					{
						dayinterval.Add(counter);
						counter = 0;
					}
				}
			}
			if (dayinterval.Count == 0) return Tuple.Create(0d, 0d);
			dayinterval.Remove(dayinterval[0]);
			if (dayinterval.Count == 0) return Tuple.Create(0d, 0d);
			return Tuple.Create(dayinterval.Min(), MathNet.Numerics.Statistics.Statistics.Variance(dayinterval));
		}

		public int Compare(StandbyList st)
		{
			int resultthis = 0;
			int resultother = 0;
			st.Persons.ForEach(p =>
			{
				double nthis = get1stVariance(p).Item1;
				double pother = st.get1stVariance(p).Item1;
				if (nthis > pother)
					resultthis++;
				else if (nthis < pother)
					resultother++;
			});
			return resultthis - resultother;
		}

		#region ICloneable メンバー

		public object Clone()
		{
			StandbyList csbl = new StandbyList(Year, Month);
			csbl.Persons.Clear();
			csbl.Month = this.Month;
			csbl.Year = this.Year;
			this.Persons.ForEach(p => csbl.Persons.Add(p.Clone() as Person));
			csbl.Persons.ForEach(p =>
			{
				p.OrdinaryDutyCount = 0;
				p.HolidayCount = 0;
			});
            //csbl.Standby = this.Standby.Clone() as DutyPersonsCollection;
			for(int i = 0; i < csbl.Standby.Count; i++)
			{
				Person addperson = csbl.Persons.FirstOrDefault(p => p.ID == (this.Standby[i+1] != null ? this.Standby[i+1].ID : -1));
				csbl.Standby.SetDuty(i + 1,  addperson);
			}

			return csbl;
		}

		#endregion
	}

    public class DutyPersonsCollection : ICloneable
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public DutyPersonsCollection(int year, int month)
        {
            Year = year;
            Month = month;
            _dutyPersons = new DutyPersons[DateTime.DaysInMonth(Year, Month)];
            //_dutyPersons.ToList().ForEach(t => t = new DutyPersons());  // 初期化
			for (int i = 0; i < _dutyPersons.Count(); i++)
				_dutyPersons[i] = new DutyPersons();
        }
		private DutyPersons[] _dutyPersons;

        public void SetDuty(int day, Person person)
        {
            if (_dutyPersons[day - 1].Duty != null)
            {
                if (HolidayChecker.IsHoliday(new DateTime(Year, Month, day)))
                    _dutyPersons[day - 1].Duty.HolidayCount--;
                else
                    _dutyPersons[day - 1].Duty.OrdinaryDutyCount--;
            }
            
            _dutyPersons[day - 1].Duty = person;
			if (person != null)
			{
				if (HolidayChecker.IsHoliday(new DateTime(Year, Month, day)))
					person.HolidayCount++;
				else
					person.OrdinaryDutyCount++;
			}
        }
        public void RemoveDuty(int day)
        {
            if (_dutyPersons[day - 1].Duty != null)
            {
                if (HolidayChecker.IsHoliday(new DateTime(Year, Month, day)))
                    _dutyPersons[day - 1].Duty.HolidayCount--;
                else
                    _dutyPersons[day - 1].Duty.OrdinaryDutyCount--;
            }
            _dutyPersons[day - 1].Duty = null;
        }

        public Person this[int day]
        {
            get
            {
                return _dutyPersons[day - 1].Duty;
            }
            //set
            //{
            //    SetDuty(day, value);
            //}
        }

        public int Count
        {
            get { return _dutyPersons.Count(); }
        }

        public void Clear()
        {
            for (int i = 0; i < _dutyPersons.Count(); i++)
            {
                if (_dutyPersons[i].Duty != null)
                {
                    if (HolidayChecker.IsHoliday(new DateTime(Year, Month, i + 1)))
                        _dutyPersons[i].Duty.HolidayCount--;
                    else
                        _dutyPersons[i].Duty.OrdinaryDutyCount--;
                }
                _dutyPersons[i].Duty = null;
            }
        }
        /// <summary>
        /// 要素内にnullオブジェクトが含まれていないかどうかを確認します
        /// </summary>
        /// <returns></returns>
        public bool NullCheck()
        {
            return _dutyPersons.ToList().All(p => p.Duty != null);
        }
		public int NullCount()
		{
			return _dutyPersons.ToList().Count(t => t.Duty == null);
		}
        public object Clone()
        {
            var other = new DutyPersonsCollection(this.Year, this.Month);
            for(int i = 0; i < this.Count; i++)
            {
                other.SetDuty(i + 1, this[i+1] != null ? this[i+1].Clone() as Person : null);
            }
            return other;
        }
    }
	public class DutyPersons : ICloneable
	{
		public Person Duty { get; set; }

		public bool Judge()
		{
			if (Duty != null)
				return true;
			else
				return false;
		}
		public int NullCount()
		{
			int counter = 0;
			if (Duty == null)
				counter++;
			return counter;
		}

		#region ICloneable メンバー

		public object Clone()
		{
			DutyPersons csbp = new DutyPersons();
			csbp.Duty = this.Duty != null ? this.Duty.Clone() as Person : null;
			return csbp;
		}

		#endregion
	}

	public class PossibleDays : ICloneable
	{
		public int Year { get; private set; }
		public int Month { get; private set; }
		public Status[] PossibleDay { get; set; }
        public string[] Detail { get; set; }
		public void SetDate(int year, int month)
		{
			if (Year < DateTime.MinValue.Year && Year > DateTime.MaxValue.Year &&
				Month <= 0 && Month > 12)
				throw new ArgumentOutOfRangeException();
			Year = year;
			Month = month;
			PossibleDay = new PossibleDays.Status[DateTime.DaysInMonth(year, month)];
            Detail = new string[DateTime.DaysInMonth(year, month)];
            for (int i = 0; i < PossibleDay.Count(); i++)
            {
                PossibleDay[i] = Status.None;
                Detail[i] = string.Empty;
            }
            
		}
		public PossibleDays(int year, int month)
		{
			SetDate(year, month);
		}
		public enum Status
		{
			None,
			Affair,
			Limited
		}

		#region ICloneable メンバー

		public object Clone()
		{
			PossibleDays cpd = new PossibleDays(Year, Month);
			for (int i = 0; i < PossibleDay.Count(); i++)
				cpd.PossibleDay[i] = PossibleDay[i];
			return cpd;
		}

		#endregion
	}
    
	public class Person : ICloneable
	{
		public string Name { get; set; }
		public long ID { get; private set; }
		public List<PossibleDays> Possible { get; set; }
        /// <summary>
        /// 役職
        /// </summary>
		public Attributes Attre { get; set; }
        /// <summary>
        /// 条件
        /// </summary>
		public Requirement Requirement { get; set; }
		/// <summary>
		/// 以前のデータから算出した総負担（休日当直は平日当直2回分に相当）
		/// </summary>
		public int TotalBurden { get; set; }
        public int HolidayCount { get; internal set; }
        public int OrdinaryDutyCount { get; internal set; }
		public enum Attributes
		{
            助教,
            医員
		}

		public Person(long id)
		{
			Requirement = new Requirement();
			Possible = new List<PossibleDays>();
			ID = id;
		}
		public override string ToString()
		{
			return this.Name;
		}

		public PossibleDays.Status this[DateTime dt]
		{
			get
			{
				if (!Possible.Any(t => t.Year == dt.Year && t.Month == dt.Month))
				{
					PossibleDays pd = new PossibleDays(dt.Year, dt.Month);
					Possible.Add(pd);
					return pd.PossibleDay[dt.Day - 1];
				}

				return Possible.First(t => t.Year == dt.Year && t.Month == dt.Month).PossibleDay[dt.Day - 1];
			}
			set
			{
				if (!Possible.Any(t => t.Year == dt.Year && t.Month == dt.Month))
				{
					PossibleDays pd = new PossibleDays(dt.Year, dt.Month);
					pd.PossibleDay[dt.Day - 1] = value;
					Possible.Add(pd);
				}
				else
					Possible.First(t => t.Year == dt.Year && t.Month == dt.Month).PossibleDay[dt.Day - 1] = value;
			}
		}



		#region ICloneable メンバー

		public object Clone()
		{
			Person cperson = new Person(ID);
			cperson.Name = Name;
			Possible.ForEach(t => cperson.Possible.Add(t.Clone() as PossibleDays));
			cperson.Attre = Attre;
			cperson.Requirement = Requirement.Clone() as Requirement;
			cperson.OrdinaryDutyCount = OrdinaryDutyCount;
			cperson.HolidayCount = HolidayCount;
			return cperson;
		}

		#endregion
	}
    public class RepeatRule : ICloneable
    {
        public dynamic RuleClass { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool IsExcludeHoliday { get; set; }
        public string Reason { get; set; }
        public Accessory Accessory1 { get; set; }   // 1日目の勤務態勢
        public Accessory Accessory2 { get; set; }   // 2日目の勤務態勢
        /// <summary>
        /// ルールに合致する日にちを全て取得します。（ただし、Affairに依存する日にちは除く）
        /// </summary>
        /// <param name="Year"></param>
        /// <param name="Month"></param>
        /// <returns></returns>
        public List<int> GetDaysWithoutAffair(int Year, int Month)
        {
            var days = new List<int>();
            var lastdate = new DateTime(Year, Month, DateTime.DaysInMonth(Year, Month));
            var firstdate = new DateTime(Year, Month, 1);
            if (Start > lastdate || End < firstdate) return days;
            if (RepeatRule.checkInterval(Year, Month, this)) return days;
            if (RuleClass.SelfIdentify == Identify.MonthClass)
            {
                var date = new DateTime(Year, Month, 1);
                if (RuleClass.DayOfWeek == RepeatRule.DayOfWeek.土日 &&
                    RepeatRule.GetWeekNo(date.AddDays(-1)) == RuleClass.WeekNumber &&
                    Start <= date.AddDays(-1) &&
                    date.DayOfWeek == System.DayOfWeek.Sunday &&
                    !RepeatRule.checkInterval(date.AddDays(-1).Year, date.AddDays(-1).Month, this) &&
                    !(IsExcludeHoliday && HolidayChecker.IsHoliday(date)))
                {
                    days.Add(1);
                    days.Add(0);
                }
                for (int i = 0; i < DateTime.DaysInMonth(Year, Month); i++)
                {
                    date = new DateTime(Year, Month, i + 1);
                    if (!(IsExcludeHoliday && HolidayChecker.IsHoliday(date)))
                    {
                        var tmp = RepeatRule.GetWeekNo(date);
                        if (RepeatRule.GetWeekNo(date) == RuleClass.WeekNumber || RuleClass.WeekNumber == RepeatRule.WeekNo.All)
                        {
                            if (RuleClass.DayOfWeek != RepeatRule.DayOfWeek.土日)
                            {
                                if (RuleClass.IsByDayOfWeek && (RepeatRule.DayOfWeek)date.DayOfWeek == RuleClass.DayOfWeek)
                                {
                                    days.Add(i + 1);

                                }
                            }
                            else
                            {
                                if (date.AddDays(-1).DayOfWeek == System.DayOfWeek.Saturday && RepeatRule.GetWeekNo(date.AddDays(-1)) != RuleClass.WeekNumber) continue;
                                if (RuleClass.IsByDayOfWeek && ((RepeatRule.DayOfWeek)date.DayOfWeek == RepeatRule.DayOfWeek.土/* || ((RepeatRule.DayOfWeek)date.DayOfWeek == RepeatRule.DayOfWeek.日))*/))
                                    if (!(date.Day == 1 && date.DayOfWeek == System.DayOfWeek.Sunday))
                                    {
                                        days.Add(i + 1);
                                        days.Add(i + 2);

                                    }
                            }
                        }
                    }
                }
            }
            return days;
        }
        /// <summary>
        /// ルールに合致する日にちを全て取得します。
        /// </summary>
        /// <param name="Year">対象の年</param>
        /// <param name="Month">対象の月</param>
        /// <returns>ルールに合致する日にち</returns>
        public List<int> GetDays(int Year, int Month)
        {
            List<int> days = new List<int>();
            var lastdate = new DateTime(Year, Month, DateTime.DaysInMonth(Year, Month));
            var firstdate = new DateTime(Year, Month, 1);
            if (Start > lastdate || End < firstdate) return days;
            if (RepeatRule.checkInterval(Year, Month, this)) return days;
            if(RuleClass.SelfIdentify == Identify.MonthClass)
            {
                var date = new DateTime(Year, Month, 1);
                if (RuleClass.DayOfWeek == RepeatRule.DayOfWeek.土日 &&
                    RepeatRule.GetWeekNo(date.AddDays(-1)) == RuleClass.WeekNumber &&
                    Start <= date.AddDays(-1) &&
                    date.DayOfWeek == System.DayOfWeek.Sunday &&
                    !RepeatRule.checkInterval(date.AddDays(-1).Year, date.AddDays(-1).Month, this) &&
                    !(IsExcludeHoliday && HolidayChecker.IsHoliday(date)))
                {
                    days.Add(1);
                    days.Add(0);
                    if(RuleClass.AffairBA == AffairBA.After || RuleClass.AffairBA == AffairBA.Both)
                        days.Add(2);
                    if (RuleClass.AffairBA == AffairBA.Before || RuleClass.AffairBA == AffairBA.Both)
                        days.Add(-1);
                }
                for(int i = 0; i < DateTime.DaysInMonth(Year, Month); i++)
                {
                    date = new DateTime(Year, Month, i + 1);
                    if (!(IsExcludeHoliday && HolidayChecker.IsHoliday(date)))
                    {
						var tmp = RepeatRule.GetWeekNo(date);
                        if (RepeatRule.GetWeekNo(date) == RuleClass.WeekNumber || RuleClass.WeekNumber == RepeatRule.WeekNo.All)
                        {
                            if (RuleClass.DayOfWeek != RepeatRule.DayOfWeek.土日)
                            {
                                if (RuleClass.IsByDayOfWeek && (RepeatRule.DayOfWeek)date.DayOfWeek == RuleClass.DayOfWeek)
                                {
                                    days.Add(i + 1);
                                    if (RuleClass.AffairBA == AffairBA.After || RuleClass.AffairBA == AffairBA.Both)
                                        days.Add(i + 2);
                                    if (RuleClass.AffairBA == AffairBA.Before || RuleClass.AffairBA == AffairBA.Both)
                                        days.Add(i);
                                }
                            }
                            else
                            {
                                if (date.AddDays(-1).DayOfWeek == System.DayOfWeek.Saturday && RepeatRule.GetWeekNo(date.AddDays(-1)) != RuleClass.WeekNumber) continue;
                                if (RuleClass.IsByDayOfWeek && ((RepeatRule.DayOfWeek)date.DayOfWeek == RepeatRule.DayOfWeek.土/* || ((RepeatRule.DayOfWeek)date.DayOfWeek == RepeatRule.DayOfWeek.日))*/))
                                    if (!(date.Day == 1 && date.DayOfWeek == System.DayOfWeek.Sunday))
                                    {
                                        days.Add(i + 1);
                                        days.Add(i + 2);
                                        if (RuleClass.AffairBA == AffairBA.After || RuleClass.AffairBA == AffairBA.Both)
                                            days.Add(i + 3);
                                        if (RuleClass.AffairBA == AffairBA.Before || RuleClass.AffairBA == AffairBA.Both)
                                            days.Add(i);
                                    }
                            }
                        }
                    }
                }
            }
            return days;

        }
        private static bool checkInterval(int year, int month, RepeatRule rule)
        {
            bool b = (((((year - rule.Start.Year) * 12) + month) - rule.Start.Month) % (rule.RuleClass.Interval + 1) != 0);
            return b;
        }
        public static RepeatRule.WeekNo GetWeekNo(DateTime date)
        {
            // (cf.) http://point56.blogspot.jp/2009/02/c.html
            Calendar cal = CultureInfo.CurrentCulture.Calendar;
            CalendarWeekRule cwr = CultureInfo.CurrentCulture.DateTimeFormat.CalendarWeekRule;
            System.DayOfWeek firstDay = new DateTime(date.Year, date.Month, 1).DayOfWeek;

            return (RepeatRule.WeekNo)cal.GetWeekOfYear(date, cwr, firstDay) - (date.Month != 1 ? cal.GetWeekOfYear(new DateTime(date.Year, date.Month, 1).AddDays(-1), cwr, firstDay) : 0);
        }

        public object Clone()
        {
            var clone = new RepeatRule(Start.Year, Start.Month, RuleClass.Clone());
            clone.Start = this.Start;
            clone.End = this.End;
            clone.Reason = this.Reason;
            clone.IsExcludeHoliday = this.IsExcludeHoliday;
            clone.Accessory1 = this.Accessory1;
            clone.Accessory2 = this.Accessory2;
            return clone;
        }

        public RepeatRule(int year, int month, dynamic _class)
        {
            Start = new DateTime(year, month, 1);
            End = DateTime.MaxValue;
            RuleClass = _class;
            Reason = string.Empty;
            IsExcludeHoliday = false;
            Accessory1 = Accessory.None;
            Accessory2 = Accessory.None;
        }

        #region Equalsのoverride
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            var p = obj as RepeatRule;
            if ((Object)p == null)
                return false;
            return Start == p.Start &&
                End == p.End &&
                Reason == p.Reason &&
                Accessory1 == p.Accessory1 &&
                Accessory2 == p.Accessory2 &&
                RuleClass == p.RuleClass;

        }

        public bool Equals(RepeatRule p)
        {
            if ((object)p == null)
                return false;
            return Start == p.Start &&
                End == p.End &&
                Reason == p.Reason &&
                Accessory1 == p.Accessory1 &&
                Accessory2 == p.Accessory2 &&
                RuleClass == p.RuleClass;
        }

        public override int GetHashCode()
        {
            return Start.GetHashCode() ^ End.GetHashCode() ^ Reason.GetHashCode() ^ RuleClass.GetHashCode() ^ Accessory1.GetHashCode() ^ Accessory2.GetHashCode();
        }

        public static bool operator ==(RepeatRule a, RepeatRule b)
        {
            if (object.ReferenceEquals(a, b))
                return true;
            if (((object)a == null) || ((object)b == null))
                return false;
            return a.Start == b.Start &&
                a.End == b.End &&
                a.Reason == b.Reason &&
                a.Accessory1 == b.Accessory1 &&
                a.Accessory2 == b.Accessory2 &&
                a.RuleClass == b.RuleClass;
        }
        public static bool operator !=(RepeatRule a, RepeatRule b)
        {
            return !(a == b);
        }
        #endregion  Equalsのoverride

        public class DayRule
        {
            public int Interval { get; set; }
			public Identify SelfIdentify { get; private set; }

            public DayRule()
            {
                SelfIdentify = Identify.DayClass;
                Interval = 1;
            }
        }
        public class WeekRule
        {
            public int Interval { get; set; }
            public List<DayOfWeek> DayOfWeeks { get; set; }
			public Identify SelfIdentify { get; private set; }

            public WeekRule()
            {
                SelfIdentify = Identify.WeekClass;
                Interval = 1;
                DayOfWeeks = new List<DayOfWeek>();
            }
        }
        public class MonthRule : ICloneable
        {
            public int Interval { get; set; }
            public bool IsByDayOfWeek { get; set; }
            public WeekNo WeekNumber { get; set; }
            public RepeatRule.DayOfWeek DayOfWeek { get; set; }
            public AffairBA AffairBA { get; set; }
			public Identify SelfIdentify { get; private set; }
            public MonthRule()
            {
                SelfIdentify = Identify.MonthClass;
                AffairBA = AffairBA.None;
                Interval = 1;
                WeekNumber = WeekNo.First;
                IsByDayOfWeek = true;
            }

            public object Clone()
            {
                MonthRule clone = new MonthRule();
                clone.Interval = this.Interval;
                clone.IsByDayOfWeek = this.IsByDayOfWeek;
                clone.WeekNumber = this.WeekNumber;
                clone.DayOfWeek = this.DayOfWeek;
                clone.AffairBA = this.AffairBA;
                return clone;
            }

            #region Equalsのoverride
            public override bool Equals(object obj)
            {
                if (obj == null)
                    return false;
                var p = obj as MonthRule;
                if ((object)p == null)
                    return false;
                return Interval == p.Interval &&
                    IsByDayOfWeek == p.IsByDayOfWeek &&
                    WeekNumber == p.WeekNumber &&
                    DayOfWeek == p.DayOfWeek;
            }

            public bool Equals(MonthRule p)
            {
                if ((object)p == null)
                    return false;
                return Interval == p.Interval &&
                    IsByDayOfWeek == p.IsByDayOfWeek &&
                    WeekNumber == p.WeekNumber &&
                    DayOfWeek == p.DayOfWeek;
            }

            public override int GetHashCode()
            {
                return Interval ^ IsByDayOfWeek.GetHashCode() ^ WeekNumber.GetHashCode() ^ DayOfWeek.GetHashCode();
            }

            public static bool operator ==(MonthRule a, MonthRule b)
            {
                if (object.ReferenceEquals(a, b))
                    return true;
                if (((object)a == null) || ((object)b == null))
                    return false;
                return a.Interval == b.Interval &&
                    a.IsByDayOfWeek == b.IsByDayOfWeek &&
                    a.WeekNumber == b.WeekNumber &&
                    a.DayOfWeek == b.DayOfWeek;
            }

            public static bool operator !=(MonthRule a, MonthRule b)
            {
                return !(a == b);
            }
            #endregion Equalsのoverride
        }
        public class YearRule
        {
            public int Interval { get; set; }
			public Identify SelfIdentify { get; private set; }
            public YearRule()
            {
                SelfIdentify = Identify.YearClass;
                Interval = 1;
            }
        }
        public enum Identify
        {
            DayClass,
            WeekClass,
            MonthClass,
            YearClass
        }
        public enum WeekNo
        {
            None = -1,
            First = 1,
            Second = 2,
            Third = 3,
            Forth = 4,
            Fifth = 5,
            All = 6,
        }
        public enum DayOfWeek
        {
            日 = 0,
            月 = 1,
            火 = 2,
            水 = 3,
            木 = 4,
            金 = 5,
            土 = 6,
            土日 = 7
        }
        public enum AffairBA
        {
            None,
            Before,
            After,
            Both
        }
    }
    public class Requirement : ICloneable
	{
		public int PossibleTimes { get; set; }
		public int HolidayPossibleTimes { get; set; }
		public int Interval { get; set; }
        public int HolidayInterval { get; set; }
        public List<RepeatRule> RepeatRule { get; set; }
		public Requirement()
		{

			Interval = 1;
			PossibleTimes = 5;
			HolidayPossibleTimes = 2;
            HolidayInterval = 1;
            RepeatRule = new List<RepeatRule>();
		}


		#region ICloneable メンバー

		public object Clone()
		{
			Requirement rcopy = new Requirement();
			rcopy.PossibleTimes = PossibleTimes;
			rcopy.HolidayPossibleTimes = HolidayPossibleTimes;
			rcopy.Interval = Interval;
            rcopy.HolidayInterval = HolidayInterval;
            //this.RepeatRule.CopyTo(rcopy.RepeatRule.ToArray(), 0);
            //rcopy.RepeatRule.CopyTo(this.RepeatRule.ToArray(), 0);
            this.RepeatRule.ForEach(t => rcopy.RepeatRule.Add(t.Clone() as RepeatRule));
			return rcopy;
		}

		#endregion
	}

	public class LimitedPerson
	{
		public enum Limit
		{
			None,
			Limited,
			Cannot,
		}
		public Person Person { get; set; }
		public Limit LimitStatus { get; set; }
		public List<string> Messages { get; set; }
		public LimitedPerson(Person p)
		{
			Person = p;
			LimitStatus = Limit.None;
			Messages = new List<string>();
		}
	}
    public class SwapableList
    {
        public int Day { get; set; }
        public Person Person { get; set; }
    }

}
