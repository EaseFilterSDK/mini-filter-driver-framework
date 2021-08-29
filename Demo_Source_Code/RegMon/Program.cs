using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace RegMon
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool mutexCreated = false;
            System.Threading.Mutex mutex = new System.Threading.Mutex(true, "FilterControl", out mutexCreated);
            if (!mutexCreated)
            {
                mutex.Close();
                //only one FilterControl can be loaded to communicate with the filter driver.
                MessageBox.Show("A FilterControl was loaded by another process, start application failed.", "Start", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }    

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new RegMonForm());

            mutex.Close();

        }
    }
}
