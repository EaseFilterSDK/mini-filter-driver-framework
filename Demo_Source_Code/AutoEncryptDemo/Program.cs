using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AutoEncryptDemo
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
                MessageBox.Show("FilterAPI was loaded by another process, start application failed.", "Start", MessageBoxButtons.OK, MessageBoxIcon.Error);
                mutex.Close();
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new AutoEncryptForm());

            mutex.Close();
        }
    }
}
