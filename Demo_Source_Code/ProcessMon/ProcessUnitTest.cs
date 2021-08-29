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
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

using EaseFilter.FilterControl;
using EaseFilter.CommonObjects;

namespace ProcessMon
{
    static public class ProcessUnitTest
    {
        static FilterControl filterControl = new FilterControl();
        public static RichTextBox unitTestResult = new RichTextBox();
        public static bool newProcessCreationNotification = false;
        public static bool monitorIONotification = false;
        public static bool controlIONotification = false;

        static string lastError = string.Empty;

        static private void AppendUnitTestResult(string text, Color color)
        {
            if (color == Color.Black)
            {
                unitTestResult.AppendText(Environment.NewLine);
                unitTestResult.SelectionFont = new Font("Arial", 12, FontStyle.Bold);
            }

            unitTestResult.SelectionStart = unitTestResult.TextLength;
            unitTestResult.SelectionLength = 0;

            unitTestResult.SelectionColor = color;
            unitTestResult.AppendText(text + Environment.NewLine);
            unitTestResult.SelectionColor = unitTestResult.ForeColor;

            if (color == Color.Black)
            {
                unitTestResult.AppendText(Environment.NewLine);
            }


        }

        private static void DenyNewProcessTest()
        {
            try
            {

                //
                //Process control flag test,deny new process creation.
                //
                uint controlFlag = (uint)FilterAPI.ProcessControlFlag.DENY_NEW_PROCESS_CREATION;

                //start new process should be fine
                string processName = "cmd.exe";
                var proc = System.Diagnostics.Process.Start(processName);
                proc.Kill();

                ProcessFilter processFilter = new ProcessFilter(processName);
                processFilter.ControlFlag = controlFlag;

                filterControl.ClearFilters();
                filterControl.AddFilter(processFilter);
                filterControl.SendConfigSettingsToFilter(ref lastError);

                try
                {
                    proc = System.Diagnostics.Process.Start(processName);
                    proc.Kill();
                    AppendUnitTestResult("DENY_NEW_PROCESS_CREATION control flag test failed, denied process launch was failed.", Color.Red);
                }
                catch (Exception ex)
                {
                    AppendUnitTestResult("DENY_NEW_PROCESS_CREATION control flag test passed.", Color.Green);
                }

            }
            catch (Exception ex)
            {
                AppendUnitTestResult("DENY_NEW_PROCESS_CREATION failed, return error:" + ex.Message, Color.Red);
            }
            finally
            {
                filterControl.ClearFilters();
                filterControl.SendConfigSettingsToFilter(ref lastError);
            }
           
        }

        /// <summary>
        /// Fires this event when the new process was being created.
        /// </summary>
        public static void OnProcessCreation(object sender, ProcessEventArgs e)
        {
            if (e.ImageFileName.IndexOf("cmd.exe") >= 0)
            {
                //this is our unit test file name.
                newProcessCreationNotification = true;
            }
        }

        /// <summary>
        /// Fires this event after the event was triggered.
        /// </summary>
        public static void OnProcessInfoNotification(object sender, ProcessEventArgs e)
        {
            //do your job here.

        }

        private static void NewProcessCallbackTest()
        {
            try
            {

                //
                //Process control flag test, new process creation notification.
                //
                string processName = "cmd.exe";
                ProcessFilter processFilter = new ProcessFilter(processName);
                processFilter.ControlFlag = (uint)FilterAPI.ProcessControlFlag.PROCESS_CREATION_NOTIFICATION;
                processFilter.OnProcessCreation += OnProcessCreation;

                filterControl.ClearFilters();
                filterControl.AddFilter(processFilter);
                filterControl.SendConfigSettingsToFilter(ref lastError);

                try
                {
                    //start new process should be fine
                    var proc = System.Diagnostics.Process.Start(processName);
                    proc.Kill();

                }
                catch (Exception ex)
                {
                    AppendUnitTestResult("PROCESS_CREATION_NOTIFICATION failed," + ex.Message, Color.Red);
                }

            }
            catch (Exception ex)
            {
                AppendUnitTestResult("PROCESS_CREATION_NOTIFICATION failed," + ex.Message, Color.Red);
            }
            finally
            {
                filterControl.SendConfigSettingsToFilter(ref lastError);
            }
           
        }




        private static void ProcessFileControlTest()
        {

            try
            {
                
                //
                //File access control test for process, to test block the new file creation for current process
                //

                ProcessFilter processFilter = new ProcessFilter("");
                processFilter.ControlFlag = 0;
                processFilter.ProcessId = FilterAPI.GetCurrentProcessId();
                uint accessFlag = FilterAPI.ALLOW_MAX_RIGHT_ACCESS & (uint)~FilterAPI.AccessFlag.ALLOW_OPEN_WITH_CREATE_OR_OVERWRITE_ACCESS;
                processFilter.FileAccessRights.Add("*", accessFlag);

                filterControl.ClearFilters();
                filterControl.AddFilter(processFilter);
                filterControl.SendConfigSettingsToFilter(ref lastError);

                string fileName = "test.txt";

                try
                {
                    File.AppendAllText(fileName, "This is test file content");
                    AppendUnitTestResult("File access control test for current process failed, can't block the new creation.", Color.Red);
                }
                catch (Exception ex)
                {
                    AppendUnitTestResult("File access control flag ALLOW_OPEN_WITH_CREATE_OR_OVERWRITE_ACCESS test for current process passed", Color.Green);
                }

            }
            catch (Exception ex)
            {
                AppendUnitTestResult("File access control test for current process failed," + ex.Message, Color.Red);
            }

            
        }

        /// <summary>
        /// Fires this event after the new file was created, the handle is not closed.
        /// </summary>
        public static void OnPreCreateFile(object sender, FileCreateEventArgs e)
        {
            if (e.FileName.IndexOf("test.txt") > 0)
            {
                controlIONotification = true;
            }

        }

        /// <summary>
        /// Fires this event after the new file was created, the handle is not closed.
        /// </summary>
        public static void OnFileCreate(object sender, FileCreateEventArgs e)
        {
            if (e.FileName.IndexOf("test.txt") > 0)
            {
                monitorIONotification = true;
            }
        }


        private static void ProcessFileIOCallbackTest()
        {
            try
            {
                ProcessFilter processFilter = new ProcessFilter("");
                processFilter.ControlFlag = 0;
                processFilter.ProcessNameFilterMask = GlobalConfig.AssemblyName;
                uint accessFlag = FilterAPI.ALLOW_MAX_RIGHT_ACCESS;
                processFilter.FileAccessRights.Add("*", accessFlag);
                processFilter.MonitorFileIOEventFilter = (ulong)(MonitorFileIOEvents.OnFileCreate);
                processFilter.ControlFileIOEventFilter = (ulong)(ControlFileIOEvents.OnPreFileCreate);

                processFilter.OnPreCreateFile += OnPreCreateFile;
                processFilter.OnNewFileCreate += OnFileCreate;

                filterControl.ClearFilters();
                filterControl.AddFilter(processFilter);

                if (!filterControl.SendConfigSettingsToFilter(ref lastError))
                {
                    AppendUnitTestResult("ProcessFileIOCallbackTest SendConfigSettingsToFilter failed:" + lastError, Color.Red);
                    return;
                }

                //
                //monitor and control IO callback notification test for current process
                //
                string fileName = GlobalConfig.AssemblyPath + "\\test.txt";

                try
                {
                    File.Delete(fileName);
                    File.AppendAllText(fileName, "This is test file content");                   

                }
                catch (Exception ex)
                {
                    AppendUnitTestResult("Register monitor/control IO callback for current process failed." + ex.Message, Color.Red);
                }

            }
            catch (Exception ex)
            {
                AppendUnitTestResult("File access control test for current process failed," + ex.Message, Color.Red);
           
            }
            finally
            {
                filterControl.SendConfigSettingsToFilter(ref lastError);
            }
            
        }

        public static void GetUnitTestResult()
        {

            Thread.Sleep(3000);

            if (newProcessCreationNotification)
            {
                AppendUnitTestResult("PROCESS_CREATION_NOTIFICATION control flag test passed.", Color.Green);
            }
            else
            {
                AppendUnitTestResult("PROCESS_CREATION_NOTIFICATION test failed, didn't receive the new process creation notification.", Color.Red);
            }

            if (controlIONotification)
            {
                AppendUnitTestResult("Register control IO callback for current process test passed.", Color.Green);
            }
            else
            {
                AppendUnitTestResult("File control IO callback test failed, no monitor callback.", Color.Red);
            }

            if (monitorIONotification)
            {
                AppendUnitTestResult("Register monitor IO callback for current process test passed.", Color.Green);
            }
            else
            {
                AppendUnitTestResult("File monitor IO callback test failed, no monitor callback.", Color.Red);
            }

            
        }

        public static void ProcessFilterUnitTest(RichTextBox richTextBox_TestResult)
        {

            string lastError = string.Empty;
            string userName = Environment.UserDomainName + "\\" + Environment.UserName;

            unitTestResult = richTextBox_TestResult;


            string message = "Process Filter Driver Unit Test.";
            AppendUnitTestResult(message, Color.Black);

            if (!filterControl.StartFilter(GlobalConfig.filterType, GlobalConfig.FilterConnectionThreads, GlobalConfig.ConnectionTimeOut, GlobalConfig.licenseKey, ref lastError))
            {
                MessageBox.Show(lastError, "StartFilter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DenyNewProcessTest();

            NewProcessCallbackTest();

            ProcessFileControlTest();

            ProcessFileIOCallbackTest();

            GetUnitTestResult();

            filterControl.StopFilter();
        }
    }
}
