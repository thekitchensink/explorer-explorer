using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace explorer_explorer.mainwindow
{
    public static class NativeMethods
    {
        [DllImport("native.dll")]
        public static extern void BlueScreenMeDaddy();
    }
}
