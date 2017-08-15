using StandbyList;
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
    public partial class FormRule : Form
    {
        public RepeatRule Rule { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public FormRule()
        {
            InitializeComponent();
            Rule = null;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var monthrule = new RepeatRule.MonthRule();
            monthrule.IsByDayOfWeek = true;
            switch(cmbWeekNo.SelectedIndex)
            {
                case 0:
                    monthrule.WeekNumber = RepeatRule.WeekNo.First;
                    break;
                case 1:
                    monthrule.WeekNumber = RepeatRule.WeekNo.Second;
                    break;
                case 2:
                    monthrule.WeekNumber = RepeatRule.WeekNo.Third;
                    break;
                case 3:
                    monthrule.WeekNumber = RepeatRule.WeekNo.Forth;
                    break;
                case 4:
                    monthrule.WeekNumber = RepeatRule.WeekNo.Fifth;
                    break;
                case 5:
                    monthrule.WeekNumber = RepeatRule.WeekNo.All;
                    break;
                default:
                    monthrule.WeekNumber = RepeatRule.WeekNo.None;
                    break;
            }
            switch(cmbDayOfWeek.SelectedIndex)
            {
                case 0:
                    monthrule.DayOfWeek = RepeatRule.DayOfWeek.日;
                    break;
                case 1:
                    monthrule.DayOfWeek = RepeatRule.DayOfWeek.月;
                    break;
                case 2:
                    monthrule.DayOfWeek = RepeatRule.DayOfWeek.火;
                    break;
                case 3:
                    monthrule.DayOfWeek = RepeatRule.DayOfWeek.水;
                    break;
                case 4:
                    monthrule.DayOfWeek = RepeatRule.DayOfWeek.木;
                    break;
                case 5:
                    monthrule.DayOfWeek = RepeatRule.DayOfWeek.金;
                    break;
                case 6:
                    monthrule.DayOfWeek = RepeatRule.DayOfWeek.土;
                    break;
                case 7:
                    monthrule.DayOfWeek = RepeatRule.DayOfWeek.土日;
                    break;
                default:
                    break;

            }
            if (radio1.Checked) monthrule.AffairBA = RepeatRule.AffairBA.None;
            else if (radio2.Checked) monthrule.AffairBA = RepeatRule.AffairBA.Before;
            else if (radio3.Checked) monthrule.AffairBA = RepeatRule.AffairBA.After;
            else if (radio4.Checked) monthrule.AffairBA = RepeatRule.AffairBA.Both;

            monthrule.Interval = cmbInterval.SelectedIndex;
            Rule = new RepeatRule(Year, Month, monthrule);
            Rule.Start = dateTimePicker1.Value;
            if (chkUseEnd.Checked)
                Rule.End = dateTimePicker2.Value;
            else
                Rule.End = DateTime.MaxValue;
            Rule.Reason = txtReason.Text;
            Rule.IsExcludeHoliday = chkHoliday.Checked;
            if (radioNone1.Checked) Rule.Accessory1 = Accessory.None;
            else if (radioDay1.Checked) Rule.Accessory1 = Accessory.Day;
            else if (radioNight1.Checked) Rule.Accessory1 = Accessory.Night;
            else if (radioDayAndNight1.Checked) Rule.Accessory1 = Accessory.DayAndNight;
            else if (radioHD1.Checked) Rule.Accessory1 = Accessory.HD;

            if (radioNone2.Checked) Rule.Accessory2 = Accessory.None;
            else if (radioDay2.Checked) Rule.Accessory2 = Accessory.Day;
            else if (radioNight2.Checked) Rule.Accessory2 = Accessory.Night;
            else if (radioDayAndNight2.Checked) Rule.Accessory2 = Accessory.DayAndNight;
            else if (radioHD2.Checked) Rule.Accessory2 = Accessory.HD;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void FormRule_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy年MM月";
            dateTimePicker2.CustomFormat = "yyyy年MM月";
            if (Rule == null)
            {
                cmbInterval.SelectedIndex = 0;
                cmbDayOfWeek.SelectedIndex = 0;
                cmbWeekNo.SelectedIndex = 0;
                dateTimePicker1.Text = string.Format("{0}年{1}月", this.Year, this.Month);
                dateTimePicker2.Text = string.Format("{0}年{1}月", Year, Month);
                dateTimePicker2.Enabled = chkUseEnd.Checked;
                radio1.Checked = true;
            }
            else
            {
                cmbInterval.SelectedIndex = Rule.RuleClass.Interval;
                cmbDayOfWeek.SelectedIndex = (int)Rule.RuleClass.DayOfWeek;
                cmbWeekNo.SelectedIndex = (int)Rule.RuleClass.WeekNumber - 1;
                chkHoliday.Checked = Rule.IsExcludeHoliday;
                dateTimePicker1.Text = string.Format("{0}年{1}月", Rule.Start.Year, Rule.Start.Month);
                if (Rule.End != DateTime.MaxValue)
                {
                    chkUseEnd.Checked = true;
                    dateTimePicker2.Text = string.Format("{0}年{1}月", Rule.End.Year, Rule.End.Month);
                }
                else
                {
                    chkUseEnd.Checked = false;
                    dateTimePicker2.Enabled = false;
                }
                switch((RepeatRule.AffairBA)Rule.RuleClass.AffairBA)
                {
                    case RepeatRule.AffairBA.None:
                        radio1.Checked = true;
                        break;
                    case RepeatRule.AffairBA.Before:
                        radio2.Checked = true;
                        break;
                    case RepeatRule.AffairBA.After:
                        radio3.Checked = true;
                        break;
                    case RepeatRule.AffairBA.Both:
                        radio4.Checked = true;
                        break;
                }
                switch(Rule.Accessory1)
                {
                    case Accessory.None:
                        radioNone1.Checked = true;
                        break;
                    case Accessory.Day:
                        radioDay1.Checked = true;
                        break;
                    case Accessory.Night:
                        radioNight1.Checked = true;
                        break;
                    case Accessory.DayAndNight:
                        radioDayAndNight1.Checked = true;
                        break;
                    case Accessory.HD:
                        radioHD1.Checked = true;
                        break;
                }
                switch(Rule.Accessory2)
                {
                    case Accessory.None:
                        radioNone2.Checked = true;
                        break;
                    case Accessory.Day:
                        radioDay2.Checked = true;
                        break;
                    case Accessory.Night:
                        radioNight2.Checked = true;
                        break;
                    case Accessory.DayAndNight:
                        radioDayAndNight2.Checked = true;
                        break;
                    case Accessory.HD:
                        radioHD2.Checked = true;
                        break;
                }
                txtReason.Text = Rule.Reason;
            }
        }

        private void chkUseEnd_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePicker2.Enabled = chkUseEnd.Checked;

        
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void dateTimePicker2_Validating(object sender, CancelEventArgs e)
        {
            if (dateTimePicker2.Value < dateTimePicker1.Value)
            {
                MessageBox.Show("開始月と終了月の関係が不正です。");
                e.Cancel = true;
            }
        }

        private void cmbDayOfWeek_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDayOfWeek.SelectedIndex == 7)
            {
                groupBox3.Enabled = true;
            }
            else
                groupBox3.Enabled = false;
        }
    }
}
