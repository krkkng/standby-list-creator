using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CalendarControl
{
    public partial class PanelWindow : Form
    {
        const int CS_DROPSHADOW = 0x00020000;

        const int WN_NCHITTEST = 0x84;
        const int HTTOPLEFT = 13;
        const int HTBOTTOMLEFT = 16;
        const int HTLEFT = 10;
        const int HTTOPRIGHT = 14;
        const int HTBOTTOMRIGHT = 17;
        const int HTRIGHT = 11;
        const int HTTOP = 12;
        const int HTBOTTOM = 15;
        public PanelWindow()
        {
            InitializeComponent();
        }

        // cf)https://www.ipentec.com/document/document.aspx?page=csharp-winform-form-drop-shadow
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }
        // cf)http://www.atmarkit.co.jp/bbs/phpBB/viewtopic.php?topic=38887&forum=7&start=8
        protected override void WndProc(ref Message m)
        {
            switch(m.Msg)
            {
                case WN_NCHITTEST:
                    Point p = PointToClient(new Point(m.LParam.ToInt32() % 65536, (int)m.LParam.ToInt32() / 65536));
                    if (p.X > ClientRectangle.Right) break;
                    if (p.X < ClientRectangle.Left) break;
                    if (p.Y < ClientRectangle.Top) break;
                    if (p.Y > ClientRectangle.Bottom) break;
                    if(p.X < ClientRectangle.Left + 5)
                    {
                        if(p.Y < ClientRectangle.Top + 5)
                        {
                            m.Result = (IntPtr)HTTOPLEFT;
                            this.Invalidate();
                            return;
                        }
                        if(p.Y > ClientRectangle.Bottom - 5)
                        {
                            m.Result = (IntPtr)HTBOTTOMLEFT;
                            this.Invalidate();
                            return;
                        }
                        m.Result = (IntPtr)HTLEFT;
                        this.Invalidate();
                        return;
                    }
                    if(p.X > ClientRectangle.Right - 5)
                    {
                        if(p.Y < ClientRectangle.Top + 5)
                        {
                            m.Result = (IntPtr)HTTOPRIGHT;
                            this.Invalidate();
                            return;
                        }
                        if(p.Y > ClientRectangle.Bottom - 5)
                        {
                            m.Result = (IntPtr)HTBOTTOMRIGHT;
                            this.Invalidate();
                            return;
                        }
                        m.Result = (IntPtr)HTRIGHT;
                        Invalidate();
                        return;
                    }
                    /*if(p.Y < ClientRectangle.Top + 5)
                    {
                        m.Result = (IntPtr)HTTOP;
                        Invalidate();
                        return;
                    }
                    if(p.Y > ClientRectangle.Bottom - 5)
                    {
                        m.Result = (IntPtr)HTBOTTOM;
                        Invalidate();
                        return;
                    }
                    */
                    break;
            }
            base.WndProc(ref m);
        }

        private void PanelWindow_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(Pens.Black, new Rectangle(0, 0, this.Width-1, this.Height-1));
            DrawSchedule(e.Graphics);
        }

        private void PanelWindow_Resize(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void PanelWindow_Deactivate(object sender, EventArgs e)
        {
            Close();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

        }
        public int ScheduleLabelSpace { get; set; }
        public int DayHeaderHeight { get; set; }
        public int PanelHeaderYUpperSpace { get; set; }
        public int PanelHeaderXSpace { get; set; }
        public int LabelHeight { get; set; }
        public ScheduleLabel[] ScheduleLabels { get; set; }
        private void DrawSchedule(Graphics g)
        {
            if (ScheduleLabels == null) return;
            int space = ScheduleLabelSpace;
            int stack = DayHeaderHeight;
            for (int i = 0; i < ScheduleLabels.Count(); i++)
            {
                ScheduleLabel scj = ScheduleLabels[i];
                if (scj == null) return;
                int Xp = ClientRectangle.X + PanelHeaderXSpace;
                int Yp;
                Rectangle screct;
                Yp = ClientRectangle.Y + ((LabelHeight + space) * i) + DayHeaderHeight + 1;
                screct = new Rectangle(Xp, Yp, ClientRectangle.Width - (PanelHeaderXSpace * 2) - 1, LabelHeight);
                stack += (int)screct.Height + space;
                scj.ClientRectangle = screct;
                scj.Paint(g, false);
            }
            this.Height = stack + space;
            
        }

        private void PanelWindow_SizeChanged(object sender, EventArgs e)
        {
            Invalidate();
        }
    }
}
