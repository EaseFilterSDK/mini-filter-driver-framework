///////////////////////////////////////////////////////////////////////////////
//
//    (C) Copyright 2011 EaseFilter Technologies
//    All Rights Reserved
//
//    This software is part of a licensed software product and may
//    only be used or copied in accordance with the terms of that license.
//
//    NOTE:  THIS MODULE IS UNSUPPORTED SAMPLE CODE
//
//    This module contains sample code provided for convenience and
//    demonstration purposes only,this software is provided on an 
//    "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, 
//     either express or implied.  
//
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using EaseFilter.FilterControl;

namespace FileMonitor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
       static void Main(string[] args)
        {

            if (args.Length > 0)
            {
                string command = args[0];
                switch (command.ToLower())
                {
                    case "-installdriver":
                        {
                            bool ret = FilterAPI.InstallDriver();
                            if (!ret)
                            {
                                Console.WriteLine("Install driver failed:" + FilterAPI.GetLastErrorMessage());
                            }

                            break;
                        }

                    case "-uninstalldriver":
                        {
                            bool ret = FilterAPI.UnInstallDriver();
                            if (!ret)
                            {
                                Console.WriteLine("UnInstall driver failed:" + FilterAPI.GetLastErrorMessage());
                            }

                            break;
                        }
                }
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MonitorForm());
            }

        }
    }
}
