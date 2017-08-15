using StandbyList;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Calendar
{
    public static class RectangleExtension
    {
        public static Rectangle ToRectangle(this string str)
        {
            var rect = new Rectangle();
            Regex rg = new Regex(@"X=(?<x>-?\d+),Y=(?<y>-?\d+),Width=(?<w>-?\d+),Height=(?<h>-?\d+)");
            var matches = rg.Match(str);
            try
            {
                rect.X = int.Parse(matches.Groups["x"].Value);
                rect.Y = int.Parse(matches.Groups["y"].Value);
                rect.Width = int.Parse(matches.Groups["w"].Value);
                rect.Height = int.Parse(matches.Groups["h"].Value);
            }
            catch
            {
                rect = Rectangle.Empty;
            }
            return rect;
        }
    }
	public static class Serializer
	{
		const string SETTINGS = "Settings";
		const string COLORDUTY = "ColorDuty";
		const string TRYCOUNT = "Try_Count";
		const string FOOTERMESSAGE = "Footer_m";
		const string DATAS = "Datas";
		const string YEAR_MONTH = "Year_Month";
		const string DATE_YEAR = "Year";
		const string DATE_MONTH = "Month";
		const string PERSONS = "Persons";
		const string PERSON = "Person";
		const string ID = "id";
		const string NAME = "Name";
		const string ATTRIBUTE = "Attribute";
		const string DUTY_COUNT = "duty_count";
		const string HOLIDAYCOUNT = "holidaycount";
		const string DUTY_INTERVAL = "duty_interval";
        const string HOLIDAY_INTERVAL = "holiday_interval";
		const string POSSIBLES = "Possibles";
		const string DUTYLISTS = "DutyLists";
        const string RULES = "Rules";
        const string RULE = "Rule";
        const string START = "Start";
        const string END = "End";
        const string REASON = "Reason";
        const string ACCESSORY1 = "Accessory1";
        const string ACCESSORY2 = "Accessory2";
        const string RULECLASS = "RuleClass";
        const string IDENTIFY = "Identify";
        const string INTERVAL = "Interval";
        const string BDOW = "BDoW"; // IsByDayOfWeek
        const string WEEKNO = "WeekNo";
        const string DOW = "DoW"; // DayofWeek
        const string AFFAIR_BA = "Affair_BA";
        const string EXHOLIDAY = "ExHoliday";

		public static void SaveDatas(string filename)
		{
			var xml = new XDocument(new XDeclaration("1.0", "utf-8", "true"), new XComment("当直表作成くん ver1.0"), new XElement("Main"));

			var SettingElem = new XElement(SETTINGS,
				new XElement(COLORDUTY, Setting.DutyFieldColor.Name),
				new XElement(TRYCOUNT, Setting.TryCount),
				new XElement(FOOTERMESSAGE, Setting.FooterMessage));
			xml.Root.Add(SettingElem);

			var DataElem = new XElement(DATAS);

			Core.StandbyLists.ForEach(t =>
			{
				var DateElem = new XElement(YEAR_MONTH);
				DataElem.Add(DateElem);
				var Date_YearElem = new XElement(DATE_YEAR, t.Year);
				var Date_MonthElem = new XElement(DATE_MONTH, t.Month);
				DateElem.Add(Date_YearElem);
				DateElem.Add(Date_MonthElem);

				var PersonsElem = new XElement(PERSONS);
				for (int i = 0; i < t.Persons.Count; i++)
				{
					var PersonElem = new XElement(PERSON);
					PersonElem.Add(new XElement(ID, t.Persons[i].ID));
					PersonElem.Add(new XElement(NAME, t.Persons[i].Name));
					PersonElem.Add(new XElement(ATTRIBUTE, (int)t.Persons[i].Attre));
					PersonElem.Add(new XElement(DUTY_COUNT, t.Persons[i].Requirement.PossibleTimes));
					PersonElem.Add(new XElement(HOLIDAYCOUNT, t.Persons[i].Requirement.HolidayPossibleTimes));
					PersonElem.Add(new XElement(DUTY_INTERVAL, t.Persons[i].Requirement.Interval));
                    PersonElem.Add(new XElement(HOLIDAY_INTERVAL, t.Persons[i].Requirement.HolidayInterval));
                    var RulesElem = new XElement(RULES);
                    t.Persons[i].Requirement.RepeatRule.ForEach(u =>
                    {
                        var RuleElem = new XElement(RULE);
                        RuleElem.Add(new XElement(START, u.Start.ToString()));
                        RuleElem.Add(new XElement(END, u.End != DateTime.MaxValue ? u.End.ToString() : "-"));
                        RuleElem.Add(new XElement(REASON, u.Reason));
                        RuleElem.Add(new XElement(ACCESSORY1, (int)u.Accessory1));
                        RuleElem.Add(new XElement(ACCESSORY2, (int)u.Accessory2));
                        RuleElem.Add(new XElement(EXHOLIDAY, u.IsExcludeHoliday));
                        var ClassElem = new XElement(RULECLASS);
                        if (u.RuleClass.SelfIdentify == RepeatRule.Identify.MonthClass)
                        {
                            ClassElem.Add(new XElement(IDENTIFY, (int)RepeatRule.Identify.MonthClass));
                            ClassElem.Add(new XElement(INTERVAL, u.RuleClass.Interval));
                            ClassElem.Add(new XElement(BDOW, u.RuleClass.IsByDayOfWeek));
                            ClassElem.Add(new XElement(WEEKNO, (int)u.RuleClass.WeekNumber));
                            ClassElem.Add(new XElement(DOW, (int)u.RuleClass.DayOfWeek));
                            ClassElem.Add(new XElement(AFFAIR_BA, (int)u.RuleClass.AffairBA));
                        }
                        RuleElem.Add(ClassElem);
                        RulesElem.Add(RuleElem);
                    });
                    PersonElem.Add(RulesElem);
					var PossibleElem = new XElement(POSSIBLES);
					for (int j = 0; j < DateTime.DaysInMonth(t.Year, t.Month); j++)
						PossibleElem.Add(new XElement("_" + (j + 1).ToString(), (int)t.Persons[i][new DateTime(t.Year, t.Month, j + 1)]));
					PersonElem.Add(PossibleElem);
					PersonsElem.Add(PersonElem);
				}
				DateElem.Add(PersonsElem);
				var StandbyListElem = new XElement(DUTYLISTS);
                for (int i = 0; i < DateTime.DaysInMonth(t.Year, t.Month); i++)
                    StandbyListElem.Add(new XElement("_" + (i + 1).ToString(),
                        new XElement("duty", (t.Standby[i+1] != null && t.Standby[i+1].ID != -1) ? t.Standby[i+1].ID.ToString() : "")));
				DateElem.Add(StandbyListElem);
			});
			xml.Root.Add(DataElem);


			try
			{
				using (StreamWriter sw = new StreamWriter(new FileStream(filename, FileMode.Create), Encoding.Unicode))
				{
					sw.Write(xml);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "エラー");
			}
		}
		public static void LoadDatas(string filename)
		{
			if (string.IsNullOrEmpty(filename)) throw new FileNotFoundException();
			XmlDocument xmldoc = new XmlDocument();
			try
			{
				xmldoc.Load(filename);
				XmlElement xmlroot = xmldoc.DocumentElement;
				
				foreach(XmlElement ms in xmlroot.ChildNodes)
				{
					if (ms.Name == SETTINGS)
					{
						LoadSettingElement(ms);
					}
					else if(ms.Name == DATAS)
					{
						LoadDatasElement(ms);
					}
				}
			}
			catch(Exception e)
			{
				MessageBox.Show(e.Message, "ファイルの読み込みに失敗しました。");
			}
			finally {  }
		}

		private static void LoadSettingElement(XmlElement ms)
		{
			foreach (XmlElement s in ms.ChildNodes)
			{
				//XmlNodeList nodeslist;
				switch (s.Name)
				{
					case COLORDUTY:
						Setting.DutyFieldColor = Color.FromName(s.InnerText);
						break;
					case TRYCOUNT:
						Setting.TryCount = int.Parse(s.InnerText);
						break;
					case FOOTERMESSAGE:
						Setting.FooterMessage = s.InnerText;
						break;
				}
			}
		}

		private static void LoadDatasElement(XmlElement ms)
		{
			foreach (XmlElement s in ms.ChildNodes)
			{
				switch (s.Name)
				{
					case YEAR_MONTH:
						StandbyList.StandbyList st = null;
						int year = 1999;
						int month = 1;
						foreach (XmlElement ss in s.ChildNodes)
						{
							switch (ss.Name)
							{
								case DATE_YEAR:
									year = int.Parse(ss.InnerText);
									break;
								case DATE_MONTH:
									month = int.Parse(ss.InnerText);
									st = new StandbyList.StandbyList(year, month);
									break;
								case PERSONS:
									st.Persons.AddRange(LoadPersonsElement(ss, year, month));
									break;
								case DUTYLISTS:
									DutyPersonsCollection standby = new DutyPersonsCollection(year, month);
									foreach(XmlElement dl in ss.ChildNodes)
									{
										int day = int.Parse(dl.Name.TrimStart('_'));
										Person person = null;
										try
										{
											person = st.Persons.FirstOrDefault(t => t.ID == long.Parse(dl.FirstChild.InnerText));
										}
										catch { person = null; }
										switch (dl.FirstChild.Name)
										{
											case "duty":
												standby.SetDuty(day, person ?? new Person(-1) { Name = dl.FirstChild.InnerText });
												break;
										}
										
									}
									st.Standby = standby;
									break;
							}
						}
						Core.StandbyLists.Add(st);
						break;
				}
			}
		}

		private static List<Person> LoadPersonsElement(XmlElement ss, int year, int month)
		{
			List<Person> persons = new List<Person>();
			foreach (XmlElement sss in ss.ChildNodes)
			{
				if (sss.Name == PERSON)
				{
					persons.Add(LoadPersonElement(sss, year, month));
				}
			}
			return persons;
		}

		private static Person LoadPersonElement(XmlElement sss, int year, int month)
		{
			Person person = null;
			foreach (XmlElement p in sss.ChildNodes)
			{
				switch (p.Name)
				{
					case ID:
						person = new Person(long.Parse(p.InnerText));
						break;
					case NAME:
						person.Name = p.InnerText;
						break;
					case ATTRIBUTE:
						person.Attre = (Person.Attributes)int.Parse(p.InnerText);
						break;
					case DUTY_COUNT:
						person.Requirement.PossibleTimes = int.Parse(p.InnerText);
						break;
					case HOLIDAYCOUNT:
						person.Requirement.HolidayPossibleTimes = int.Parse(p.InnerText);
						break;
					case DUTY_INTERVAL:
						person.Requirement.Interval = int.Parse(p.InnerText);
						break;
                    case HOLIDAY_INTERVAL:
                        person.Requirement.HolidayInterval = int.Parse(p.InnerText);
                        break;
					case RULES:
						LoadRulesElement(year, month, person, p);
						break;
					case POSSIBLES:
						foreach(XmlElement pos in p.ChildNodes)
						{
							int day = int.Parse(pos.Name.TrimStart('_'));
							person[new DateTime(year, month, day)] = (PossibleDays.Status)int.Parse(pos.FirstChild.InnerText);
						}
						break;
				}
			}
			return person;
		}

		private static void LoadRulesElement(int year, int month, Person person, XmlElement p)
		{
			RepeatRule rule = null;
			foreach (XmlElement rs in p.ChildNodes)
			{
				foreach (XmlElement r in rs.ChildNodes)
				{
					switch (r.Name)
					{
						case START:
							rule = new RepeatRule(year, month, null);
							rule.Start = DateTime.Parse(r.InnerText);
							break;
						case END:
							if (r.InnerText == "-")
								rule.End = DateTime.MaxValue;
							else
								rule.End = DateTime.Parse(r.InnerText);
							break;
						case REASON:
							rule.Reason = r.InnerText;
							break;
                        case ACCESSORY1:
                            rule.Accessory1 = (StandbyList.Accessory)int.Parse(r.InnerText);
                            break;
                        case ACCESSORY2:
                            rule.Accessory2 = (StandbyList.Accessory)int.Parse(r.InnerText);
                            break;
						case RULECLASS:
							LoadRuleElement(rule, r);
							break;
                        case EXHOLIDAY:
                            rule.IsExcludeHoliday = bool.Parse(r.InnerText);
                            break;
					}
				}
				if(rule != null)
					person.Requirement.RepeatRule.Add(rule);
			}
		}

		private static void LoadRuleElement(RepeatRule rule, XmlElement r)
		{
			foreach (XmlElement cls in r.ChildNodes)
			{
				switch (cls.Name)
				{
					case IDENTIFY:
						switch ((RepeatRule.Identify)int.Parse(cls.InnerText))
						{
							case RepeatRule.Identify.MonthClass:
								rule.RuleClass = new RepeatRule.MonthRule();
								break;
							case RepeatRule.Identify.DayClass:
							case RepeatRule.Identify.WeekClass:
							case RepeatRule.Identify.YearClass:
							default:
								break;
						}
						break;
					case INTERVAL:
						rule.RuleClass.Interval = int.Parse(cls.InnerText);
						break;
					case BDOW:
						rule.RuleClass.IsByDayOfWeek = bool.Parse(cls.InnerText);
						break;
					case WEEKNO:
						rule.RuleClass.WeekNumber = (RepeatRule.WeekNo)int.Parse(cls.InnerText);
						break;
					case DOW:
						rule.RuleClass.DayOfWeek = (RepeatRule.DayOfWeek)int.Parse(cls.InnerText);
						break;
                    case AFFAIR_BA:
                        rule.RuleClass.AffairBA = (RepeatRule.AffairBA)int.Parse(cls.InnerText);
                        break;
				}

			}
		}

        const string sSETTING = "Settings";
        const string sDUTYCOLOR = "DutyColor";
        const string sHILIGHTCOLOR = "HilightColor";
        const string sTRYCOUNT = "TryCount";
        const string sFORMMAINR = "FormMainRect";
        const string sFORMMAINS = "FormMainState";
        const string sAGFORMR = "AgFormRect";
        const string sAGFORMS = "AgFormState";
        const string sALLSCHEDULER = "AllScheduleFormRect";
        const string sALLSCHEDULES = "AllScheduleFormState";
        const string sGLOBALSETTINGR = "GlobalSettingFormRect";
        const string sGLOBALSETTINGS = "GlobalSettingFormState";
        const string sRULERECTR = "RuleFormRect";
        const string sRULERECTS = "RuleFormState";
        const string sFORMSETTINGR = "SettingFormRect";
        const string sFORMSETTINGS = "SettingFormState";
        const string sSUBFORMR = "SubFormRect";
        const string sSUBFORMS = "SubFormState";

        const string sDATEINTERVAL = "DateInterval";
        /// <summary>
        /// 設定保存用
        /// </summary>
        /// <param name="v"></param>
        public static void SaveSettings(string filename)
        {
            var xml = new XDocument(new XDeclaration("1.0", "uft-8", "true"), new XComment("当直表作成くん設定保存ファイル"),
                new XElement(sSETTING,
                    new XElement(sDUTYCOLOR, Setting.DutyFieldColor.Name),
                    new XElement(sHILIGHTCOLOR, Setting.DutyHilightColor.Name),
                    new XElement(TRYCOUNT, Setting.TryCount),
                    new XElement(sFORMMAINR, Setting.MainFormRect.ToString()),
                    new XElement(sFORMMAINS, (int)Setting.MainFormState),
                    new XElement(sAGFORMR, Setting.AggregationFormRect.ToString()),
                    new XElement(sAGFORMS, (int)Setting.AggregationFormState),
                    new XElement(sALLSCHEDULER, Setting.FormAllSchedulesRect.ToString()),
                    new XElement(sALLSCHEDULES, (int)Setting.FormAllSchedulesState),
                    new XElement(sGLOBALSETTINGR, Setting.FormGlobalSettingRect.ToString()),
                    new XElement(sGLOBALSETTINGS, (int)Setting.FormGlobalSettingState),
                    new XElement(sRULERECTR, Setting.FormRuleRect.ToString()),
                    new XElement(sRULERECTS, (int)Setting.FormRuleState),
                    new XElement(sSUBFORMR, Setting.FormSubFormRect.ToString()),
                    new XElement(sSUBFORMS, (int)Setting.FormSubFormState),
                    new XElement(sDATEINTERVAL, Setting.DateInterval.Start.ToString())
                    ));

            try
            {
                using (StreamWriter sw = new StreamWriter(new FileStream(filename, FileMode.Create), Encoding.Unicode))
                {
                    sw.Write(xml);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "エラー");
            }

        }

        public static void LoadSettings(string filename)
        {
            if (string.IsNullOrEmpty(filename)) throw new FileNotFoundException();
            XmlDocument xmldoc = new XmlDocument();
            try
            {
                xmldoc.Load(filename);
                XmlElement xmlroot = xmldoc.DocumentElement;
                foreach (XmlElement s in xmlroot)
                {
                    switch (s.Name)
                    {
                        case sDUTYCOLOR:
                            Setting.DutyFieldColor = Color.FromName(s.InnerText);
                            break;
                        case sHILIGHTCOLOR:
                            Setting.DutyHilightColor = Color.FromName(s.InnerText);
                            break;
                        case TRYCOUNT:
                            Setting.TryCount = int.Parse(s.InnerText);
                            break;
                        case sFORMMAINR:
                            Setting.MainFormRect = s.InnerText.ToRectangle();
                            break;
                        case sFORMMAINS:
                            Setting.MainFormState = (FormWindowState)(int.Parse(s.InnerText));
                            break;
                        case sAGFORMR:
                            Setting.AggregationFormRect = s.InnerText.ToRectangle();
                            break;
                        case sAGFORMS:
                            Setting.AggregationFormState = (FormWindowState)(int.Parse(s.InnerText));
                            break;
                        case sALLSCHEDULER:
                            Setting.FormAllSchedulesRect = s.InnerText.ToRectangle();
                            break;
                        case sALLSCHEDULES:
                            Setting.FormAllSchedulesState = (FormWindowState)(int.Parse(s.InnerText));
                            break;
                        case sGLOBALSETTINGR:
                            Setting.FormGlobalSettingRect = s.InnerText.ToRectangle();
                            break;
                        case sGLOBALSETTINGS:
                            Setting.FormGlobalSettingState = (FormWindowState)(int.Parse(s.InnerText));
                            break;
                        case sRULERECTR:
                            Setting.FormRuleRect = s.InnerText.ToRectangle();
                            break;
                        case sRULERECTS:
                            Setting.FormRuleState = (FormWindowState)(int.Parse(s.InnerText));
                            break;
                        case sSUBFORMR:
                            Setting.FormSubFormRect = s.InnerText.ToRectangle();
                            break;
                        case sSUBFORMS:
                            Setting.FormSubFormState = (FormWindowState)(int.Parse(s.InnerText));
                            break;
                        case sDATEINTERVAL:
                            Setting.DateInterval = new AggregationForm.DateInterval(DateTime.Parse(s.InnerText));
                            break;
                        default:
                            break;
                    }
                }
            }
            catch
            { }
        }
    }
}
