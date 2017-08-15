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
	public partial class FormGlobalSetting : Form
	{
		public StandbyList.StandbyList StandbyList { get; set; }
		private List<StandbyList.Person> tmppersons;
		public DateTime Date { get; set; }
        public List<StandbyList.StandbyList> tmpstandbylist { get; private set; }
		public FormGlobalSetting()
		{
			InitializeComponent();
            this.ResizeEnd += FormGlobalSetting_ResizeEnd;
		}

        private void FormGlobalSetting_ResizeEnd(object sender, EventArgs e)
        {
            Setting.FormGlobalSettingRect = this.Bounds;
        }

        private void FormGlobalSetting_Load(object sender, EventArgs e)
		{
            tmpstandbylist = new List<StandbyList.StandbyList>();
            // CoreのStandbyListを全てコピー
            Core.StandbyLists.ForEach(t => tmpstandbylist.Add(t.Clone() as StandbyList.StandbyList));

            StandbyList = tmpstandbylist.FirstOrDefault(t => t.Year == Date.Year && t.Month == Date.Month);

            tmppersons = StandbyList.Persons;//(StandbyList.Clone() as StandbyList.StandbyList).Persons;
			RefreshDGV();

			cmbColor1st.SelectedItem = Setting.DutyFieldColor;
            cmbColorHilight.SelectedItem = Setting.DutyHilightColor;
			txtFooterMessage.Text = Setting.FooterMessage;

            if (Setting.FormGlobalSettingRect != Rectangle.Empty)
                this.Bounds = Setting.FormGlobalSettingRect;
		}

		private void RefreshDGV()
		{
			dgView.Columns.Clear();
			dgView.Rows.Clear();
			dgView.Columns.Add("名前", "名前");
			dgView.Columns.Add("役職", "役職");
			dgView.Columns.Add("当直可能数", "当直可能数");
			dgView.Columns.Add("休日可能数", "当直の休日可能数");

			tmppersons.ForEach(p =>
			{
				DataGridViewRow row = new DataGridViewRow();
				row.CreateCells(dgView);
				row.Cells[0].Value = p;
				var attr = "";
				switch(p.Attre)
				{
					case global::StandbyList.Person.Attributes.助教:
						attr = "助教";
						break;
                    case global::StandbyList.Person.Attributes.医員:
                        attr = "医員";
						break;
				}
				row.Cells[1].Value = attr;
				row.Cells[2].Value = p.Requirement.PossibleTimes;
				row.Cells[3].Value = p.Requirement.HolidayPossibleTimes;
				dgView.Rows.Add(row);
			});
		}
		private void btnEditMember_Click(object sender, EventArgs e)
		{
			ShowEditForm();
		}

		private void ShowEditForm()
		{
			if (dgView.SelectedRows.Count == 0) return;
            var targetperson = dgView.SelectedRows[0].Cells[0].Value as StandbyList.Person;
            FormSetting form = new FormSetting { Year = Date.Year, Month = Date.Month, Person = targetperson, StandbyLists = tmpstandbylist };
			if(form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				int i = dgView.SelectedRows[0].Index;
				RefreshDGV();
				dgView.Rows[i].Selected = true;
			}
		}
		private void btnAddMember_Click(object sender, EventArgs e)
		{
			FormSetting form = new FormSetting { Year = Date.Year, Month = Date.Month, Person = new StandbyList.Person(DateTime.Now.Ticks), StandbyLists = tmpstandbylist };
			if(form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				tmppersons.Add(form.Person);
				RefreshDGV();
			}
		}

		private void btnDeleteMember_Click(object sender, EventArgs e)
		{
			if (dgView.SelectedRows.Count == 0) return;
			StandbyList.Person person = dgView.SelectedRows[0].Cells[0].Value as StandbyList.Person;
			if (person != null)
				tmppersons.Remove(person);
			RefreshDGV();
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			StandbyList.Persons = tmppersons;
            
			DialogResult = System.Windows.Forms.DialogResult.OK;

			Setting.DutyFieldColor = (Color)cmbColor1st.SelectedItem;
            Setting.DutyHilightColor = (Color)cmbColorHilight.SelectedItem;
			Setting.FooterMessage = this.txtFooterMessage.Text;
			Setting.DepartmentString = this.txtDepartment.Text;
            Core.StandbyLists = tmpstandbylist;
			DialogResult = System.Windows.Forms.DialogResult.OK;
			this.Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.Close();
		}

		private void label2_Click(object sender, EventArgs e)
		{

		}

		private void dgView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			ShowEditForm();
		}

		private void btnAllSchedules_Click(object sender, EventArgs e)
		{
			FormAllSchedules form = new FormAllSchedules() { Year = Date.Year, Month = Date.Month, StandbyList = this.StandbyList.Clone() as StandbyList.StandbyList };
			if(form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				tmppersons = form.StandbyList.Persons;
				RefreshDGV();
			}
		}
    }
}
