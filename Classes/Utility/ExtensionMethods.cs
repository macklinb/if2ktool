using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace if2ktool
{
    public static class ExtensionMethods
    {
        public static void DoubleBuffered(this DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }

        public static void DoubleBuffered(this TableLayoutPanel tlp, bool setting)
        {
            Type tlpType = tlp.GetType();
            PropertyInfo pi = tlpType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(tlp, setting, null);
        }

        public static void DoubleBuffered(this Control control, bool setting)
        {
            Type tlpType = control.GetType();
            PropertyInfo pi = tlpType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(control, setting, null);
        }

        // Invoke in the context of the control's thread without needing to cast to delegate
        public static void Invoke(this Control control, Action action)
        {
            control.Invoke((Delegate)action);
        }

        [DllImport("user32.dll")]
        public static extern bool FlashWindow(IntPtr hwnd, bool bInvert);

        public static void Flash(this Form form, bool flash)
        {
            FlashWindow(form.Handle, flash);
        }
    }
}