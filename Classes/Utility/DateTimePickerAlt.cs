using System;
using System.Linq;
using System.Windows.Forms;

namespace if2ktool
{
    // DateTimePicker, but with a CheckedChanged event. We need to do this because the default DateTimePicker doesn't always fire ValueChanged when the Checked value changes, notably when the control doesn't have focus
    public partial class DateTimePickerAlt : DateTimePicker
    {
        private const int WM_REFLECT = 0x2000;
        private const int WM_NOTIFY = 0x004E;
        private const uint DTN_DATETIMECHANGE = 0xFFFFFD09;

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        private struct NMHDR
        {
            public IntPtr hwndFrom;
            public IntPtr idFrom;
            public uint code;
        }

        public bool CheckedLast { get; set; }

        public event EventHandler ValueChangedSpecial;
        public new event MouseEventHandler MouseDoubleClick;

        private System.Diagnostics.Stopwatch doubleClickTimer;

        public DateTimePickerAlt() : base()
        {
            this.MouseDown += DateTimePickerAlt_MouseDown;
            this.MouseLeave += DateTimePickerAlt_MouseLeave;
            this.doubleClickTimer = new System.Diagnostics.Stopwatch();
        }

        // Implement DateTimePicker MouseDoubleClick manually (because somehow Microsoft is incapable of putting a working double click event on this one control)
        private void DateTimePickerAlt_MouseDown(object sender, MouseEventArgs e)
        {
            if (doubleClickTimer.IsRunning && doubleClickTimer.ElapsedMilliseconds <= SystemInformation.DoubleClickTime)
            {
                var pos = this.PointToClient(Cursor.Position);
                var args = new MouseEventArgs(Control.MouseButtons, 2, pos.X, pos.Y, 0);
                this.MouseDoubleClick?.Invoke(this, args);
                doubleClickTimer.Stop();
            }
            else
            {
                doubleClickTimer.Restart();
            }
        }

        private void DateTimePickerAlt_MouseLeave(object sender, EventArgs e)
        {
            if (doubleClickTimer.IsRunning)
                doubleClickTimer.Reset();
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (!this.DesignMode)
            {
                switch (m.Msg)
                {
                    case WM_REFLECT + WM_NOTIFY:
                        {
                            NMHDR nm = (NMHDR)m.GetLParam(typeof(NMHDR));
                            if (nm.code == DTN_DATETIMECHANGE)
                                OnValueChangedSpecial();
                            break;
                        }
                }
            }
        }

        protected virtual void OnValueChangedSpecial()
        {
            ValueChangedSpecial?.Invoke(this, EventArgs.Empty);

            // Set CheckedLast to value of Checked
            CheckedLast = Checked;
        }

        public new bool Checked
        {
            get
            {
                return base.Checked;
            }
            set
            {
                if (value != base.Checked)
                {
                    base.Checked = value;
                    CheckedLast = value;
                }
            }
        }
    }
}
