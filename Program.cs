using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FI.PlateReader.Gen4.JETI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //SetProcessDPIAware();       // use to make fonts look normal on HDPI monitors
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        //[System.Runtime.InteropServices.DllImport("user32.dll")]
       // private static extern bool SetProcessDPIAware();
    }
}
