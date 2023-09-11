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
using System.ServiceProcess;
using System.Text;
using System.IO;
using System.Reflection;
using System.Configuration.Install;

using EaseFilter.FilterControl;
using EaseFilter.CommonObjects;

namespace EaseFltCSConsoleDemo
{
    static class program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {

            try
            {
                string lastError = string.Empty;
                GlobalConfig.filterType = FilterAPI.FilterType.REGISTRY_FILTER | FilterAPI.FilterType.PROCESS_FILTER | FilterAPI.FilterType.CONTROL_FILTER | FilterAPI.FilterType.MONITOR_FILTER;

                if (Environment.UserInteractive)
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
                                    FilterWorker.StopService();
                                  
                                    bool ret = FilterAPI.UnInstallDriver();

                                    if (!ret)
                                    {
                                        Console.WriteLine("UnInstall driver failed:" + FilterAPI.GetLastErrorMessage());
                                    }

                                    break;
                                }

                            case "-installservice":
                                {
                                    try
                                    {
                                        ManagedInstallerClass.InstallHelper(new string[] { Assembly.GetExecutingAssembly().Location });
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("Install service failed:" + ex.Message);
                                    }

                                    break;
                                }

                            case "-uninstallservice":
                                {
                                    try
                                    {
                                        ManagedInstallerClass.InstallHelper(new string[] { "/u", Assembly.GetExecutingAssembly().Location });
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("UnInstall service failed:" + ex.Message);
                                    }

                                    break;
                                }

                            case "-console":
                                {
                                    try
                                    {
                                        Console.WriteLine("Starting EaseFilter console application, the filter rule will be loaded from the config file EaseFltCSConsoleDemo.exe.config.");
                                     
                                        if(!FilterWorker.StartService(out lastError))
                                        {
                                            Console.WriteLine("\n\nStart service failed." + lastError);
                                            return;
                                        }

                                        Console.WriteLine("\n\nPress any key to stop program");
                                        Console.Read();
                                        FilterWorker.StopService();
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("Start EaseFilter service failed:" + ex.Message);
                                    }

                                    break;
                                }

                            default: Console.WriteLine("The command " + command + " doesn't exist"); PrintUsage(); break;

                        }

                    }
                    else
                    {
                        PrintUsage();
                    }

                }
                else
                {
                    Console.WriteLine("Starting EaseFilter windows service...");
                    EaseFilterService service = new EaseFilterService();
                    ServiceBase.Run(service);

                }
            }
            catch (Exception ex)
            {
                EventManager.WriteMessage(132, "EaseFilterService", EventLevel.Error, "EaseFilter failed with error " + ex.Message);
            }
            finally
            {
                Console.WriteLine("Exiting EaseFilter service.");
                GlobalConfig.Stop();
            }


        }

        static void PrintUsage()
        {
            Console.WriteLine("Usage: EaseFltCSConsoleDemo command");
            Console.WriteLine("Commands:");
            Console.WriteLine("-InstallDriver       --Install EaseFilter filter driver.");
            Console.WriteLine("-UninstallDriver     --Uninstall EaseFilter filter driver.");
            Console.WriteLine("-InstallService      --Install EaseFilter Windows service.");
            Console.WriteLine("-UnInstallService    ---Uninstall EaseFilter Windows service.");
            Console.WriteLine("-Console             ----start the console application.");
        }

    }
}
