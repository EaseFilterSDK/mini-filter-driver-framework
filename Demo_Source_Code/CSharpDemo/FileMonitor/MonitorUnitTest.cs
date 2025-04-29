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
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32.SafeHandles;
using System.Security.AccessControl;
using System.Security.Principal;

using EaseFilter.CommonObjects;
using EaseFilter.FilterControl;

namespace FileMonitor
{
    public partial class MonitorUnitTest : Form
    {
        FilterControl filterControl = new FilterControl();
        bool isUnitTestStarted = false;
        static string binaryPath = GlobalConfig.AssemblyPath;

        /// <summary>
        /// Test monitor feature with registering the IO events, get notification after the file was closed.
        /// </summary>
        private  string unitTestMonitorTestFolder = Path.Combine(binaryPath, "EaseFilterUnitTest\\monitorFolder");
        private  string unitTestMonitorTestFile = Path.Combine(binaryPath, "EaseFilterUnitTest\\monitorFolder") + "\\unitTestMonitorTestFile.txt";
        private  bool isTestMonitorFileCreated = false;
        private  bool isTestMonitorFileReanmed = false;
        private  bool isTestMonitorFileDeleted = false;
        private  bool isTestMonitorFileWritten = false;
        private  bool isTestMonitorFileRead = false;
        private  bool isTestMonitorFileSecurityChanged = false;
        private  bool isTestMonitorFileInfoChanged = false;

        /// <summary>
        /// Test monitor IO registration,get notification for the registered IO.
        /// </summary>
        private  bool isOnFileOpenEventWasCalled = false;
        private  bool isOnSetFileSizeEventWasCalled = false;

        /// <summary>
        /// Unit test folder
        /// </summary>
        private  string unitTestFolder = Path.Combine(binaryPath, "EaseFilterUnitTest");

        /// <summary>
        /// Test Control IO feature with callback function
        /// </summary>
        private  string unitTestCallbackFolder = Path.Combine(binaryPath, "EaseFilterUnitTest") + "\\callbackFolder";
        private  string unitTestCallbackFile = Path.Combine(binaryPath, "EaseFilterUnitTest") + "\\callbackFolder\\unitTestFile.txt";

        //Purchase a license key with the link: http://www.easefilter.com/Order.htm
        //Email us to request a trial key: info@easefilter.com //free email is not accepted.
        public string licenseKey = "******************************************";


        public MonitorUnitTest()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;
        }


        private void InitializeUnitTest()
        {

            if (!Directory.Exists(unitTestMonitorTestFolder))
            {
                Directory.CreateDirectory(unitTestMonitorTestFolder);
            }

            if (!Directory.Exists(unitTestCallbackFolder))
            {
                Directory.CreateDirectory(unitTestCallbackFolder);
            }

            if (!File.Exists(unitTestCallbackFile))
            {
                File.AppendAllText(unitTestCallbackFile, "This is unitTestCallbackFile test file.");
            }

        }

        private void CleanupTest(string folder)
        {
            try
            {
                if (Directory.Exists(folder))
                {
                    Directory.Delete(folder, true);
                }

            }
            catch (Exception ex)
            {
                AppendText("Clean up test folder failed:" + ex.Message, Color.Red);
            }
        }

        private void AppendText(string text, Color color)
        {
            if (color == Color.Black)
            {
                richTextBox_TestResult.AppendText(Environment.NewLine);
                richTextBox_TestResult.SelectionFont = new Font("Arial", 12, FontStyle.Bold);
            }

            richTextBox_TestResult.SelectionStart = richTextBox_TestResult.TextLength;
            richTextBox_TestResult.SelectionLength = 0;

            richTextBox_TestResult.SelectionColor = color;
            richTextBox_TestResult.AppendText(text + Environment.NewLine);
            richTextBox_TestResult.SelectionColor = richTextBox_TestResult.ForeColor;

            if (color == Color.Black)
            {
                richTextBox_TestResult.AppendText(Environment.NewLine);
            }


        }


        /// <summary>
        /// Fires this event when a file's information was changed after the file handle closed
        /// </summary>
        public void NotifyFileWasChanged(object sender, FileChangedEventArgs e)
        {
            if (string.Compare(unitTestMonitorTestFile, e.FileName, true) == 0)
            {
                if ((e.eventType & FilterAPI.FileChangedEvents.NotifyFileWasCreated) > 0)
                {
                    isTestMonitorFileCreated = true;
                }

                if ((e.eventType & FilterAPI.FileChangedEvents.NotifyFileWasWritten) > 0)
                {
                    isTestMonitorFileWritten = true;
                }

                if ((e.eventType & FilterAPI.FileChangedEvents.NotifyFileWasRenamed) > 0)
                {
                    isTestMonitorFileReanmed = true;
                }

                if ((e.eventType & FilterAPI.FileChangedEvents.NotifyFileWasDeleted) > 0)
                {
                    isTestMonitorFileDeleted = true;
                }

                if ((e.eventType & FilterAPI.FileChangedEvents.NotifyFileInfoWasChanged) > 0)
                {
                    isTestMonitorFileInfoChanged = true;
                }

                if ((e.eventType & FilterAPI.FileChangedEvents.NotifyFileSecurityWasChanged) > 0)
                {
                    isTestMonitorFileSecurityChanged = true;
                }

                if ((e.eventType & FilterAPI.FileChangedEvents.NotifyFileWasRead) > 0)
                {
                    isTestMonitorFileRead = true;
                }

            }
        }
     

        private void MonitorFileEventsUnitTest()
        {
            string lastError = string.Empty;

            //
            //Test case1: Monitor File IO ( creation,rename,delete,write,security change,file info change, file read)
            //Register file events to get the notification when the registered IO was triggered.
            //This test will notify the UnitTestCallbackHandler with the IO events which we did below.
            //


            FileFilter monitorFilterRule = new FileFilter(unitTestMonitorTestFolder + "\\*");
            monitorFilterRule.AccessFlags = (FilterAPI.AccessFlag)FilterAPI.ALLOW_MAX_RIGHT_ACCESS;
            monitorFilterRule.FileChangeEventFilter = (FilterAPI.FileChangedEvents.NotifyFileWasCreated | FilterAPI.FileChangedEvents.NotifyFileWasDeleted | FilterAPI.FileChangedEvents.NotifyFileInfoWasChanged
                | FilterAPI.FileChangedEvents.NotifyFileWasRenamed | FilterAPI.FileChangedEvents.NotifyFileWasWritten |FilterAPI.FileChangedEvents.NotifyFileSecurityWasChanged|FilterAPI.FileChangedEvents.NotifyFileWasRead);


            //register the file changed event handler
            monitorFilterRule.NotifyFileWasChanged += NotifyFileWasChanged;

            try
            {
                filterControl.ClearFilters();
                filterControl.AddFilter(monitorFilterRule);

                if (!filterControl.SendConfigSettingsToFilter(ref lastError))
                {
                    MessageBox.Show(lastError, "SendConfigToFilter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //create and write events
                File.AppendAllText(unitTestMonitorTestFile, "This is IO events monitor test file.");

                //read event
                File.ReadAllText(unitTestMonitorTestFile);

                //file info change event
                File.SetAttributes(unitTestMonitorTestFile, FileAttributes.Normal);

                // Get a FileSecurity object that represents the current security settings.
                DirectoryInfo dInfo = new DirectoryInfo(unitTestMonitorTestFile);
                DirectorySecurity dSecurity = dInfo.GetAccessControl();
                dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
                //test security change event
                dInfo.SetAccessControl(dSecurity);

                //test rename event.
                string newMonitorFileName = unitTestMonitorTestFolder + "\\newMonitorFileName.txt";
                File.Move(unitTestMonitorTestFile, newMonitorFileName);

                //create test file for deletion.
                File.AppendAllText(unitTestMonitorTestFile, "This is IO events monitor test file.");

                //test delete event.
                File.Delete(unitTestMonitorTestFile);
                File.Delete(newMonitorFileName);

                //wait for the test result.
                System.Threading.Thread.Sleep(3000);
            }
            catch (Exception ex)
            {
                AppendText("FileEventMonitorTest failed." + ex.Message, Color.Red);
            }
            finally
            {
                filterControl.RemoveFilter(monitorFilterRule);
                filterControl.SendConfigSettingsToFilter(ref lastError);
            }

        }


        public void OnFileOpen(object sender, FileCreateEventArgs e)
        {
            if (string.Compare(unitTestCallbackFile, e.FileName, true) == 0)
            {
                isOnFileOpenEventWasCalled = true;
            }
        }

        public void OnSetFileSize(object sender, FileSizeEventArgs e)
        {
            if (string.Compare(unitTestCallbackFile, e.FileName, true) == 0)
            {
                isOnSetFileSizeEventWasCalled = true;
            }
        }

        private void MonitorIOEventsUnitTest()
        {
            string lastError = string.Empty;
            string message = "Monitor IO Registration Unit Test";
            AppendText(message, Color.Black);

            message = " 1. Register the POST_CREATE and POST_SET_INFORMATION." + Environment.NewLine;
            message += " 2. The callback function will get the notification after the file was opened or file information was changed." + Environment.NewLine;

            AppendText(message, Color.Gray);

            FileFilter fileMonitorFilter = new FileFilter(unitTestCallbackFolder + "\\*");
            fileMonitorFilter.MonitorFileIOEventFilter = (MonitorFileIOEvents.OnFileOpen | MonitorFileIOEvents.OnSetFileSize);

            fileMonitorFilter.OnFileOpen += OnFileOpen;
            fileMonitorFilter.OnSetFileSize += OnSetFileSize;

            try
            {
                filterControl.ClearFilters();
                filterControl.AddFilter(fileMonitorFilter);
                filterControl.SendConfigSettingsToFilter(ref lastError);

                FileStream fs = new FileStream(unitTestCallbackFile, FileMode.Open, FileAccess.ReadWrite);
                fs.SetLength(1024);

                fs.Close();

                //wait for the test result.
                System.Threading.Thread.Sleep(3000);

            }
            catch (Exception ex)
            {
                AppendText("Monitor IO Registration Unit Test failed." + ex.Message, Color.Red);
            }
            finally
            {
                filterControl.ClearFilters();
                filterControl.SendConfigSettingsToFilter(ref lastError);
            }       

            if (isOnFileOpenEventWasCalled)
            {
                AppendText("Test file open event passed.", Color.Green);
                AppendText("The OnFileOpen event was invoked.", Color.Gray);
            }
            else
            {
                AppendText("Test file open event failed.", Color.Red);
            }


            if (isOnSetFileSizeEventWasCalled)
            {
                AppendText("Test file size change event passed.", Color.Green);
                AppendText("The OnFileSetFileSize event was invoked.", Color.Gray);
            }
            else
            {
                AppendText("Test OnFileSetFileSize event failed.", Color.Red);
            }
        }

        private void ShowMonitorFileEventsUnitTestResult()
        {

            /// print out the monitor IO events test result.
            string message = "Monitor File IO Events Unit Test.";
            AppendText(message, Color.Black);
            message = "Register the monitor file IO events: create,rename,delete,written,file info changed, read, security change in the test folder." + Environment.NewLine;
            AppendText(message, Color.Gray);

            if (isTestMonitorFileCreated)
            {
                AppendText("Test new file creation event passed.", Color.Green);
                AppendText("A test file was created.", Color.Gray);
            }
            else
            {
                AppendText("Test new file creation event failed.", Color.Red);
            }

            if (isTestMonitorFileInfoChanged)
            {
                AppendText("Test file info change event passed.", Color.Green);
                AppendText("The test file's attributes were changed.", Color.Gray);
            }
            else
            {
                AppendText("Test file info change event failed.", Color.Red);
            }

            if (isTestMonitorFileRead)
            {
                AppendText("Test file read event passed.", Color.Green);
                AppendText("The test file's data was read.", Color.Gray);
            }
            else
            {
                AppendText("Test file read event  failed.", Color.Red);
            }
            if (isTestMonitorFileReanmed)
            {
                AppendText("Test file rename event passed.", Color.Green);
                AppendText("The test file was renamed.", Color.Gray);
            }
            else
            {
                AppendText("Test file rename event failed.", Color.Red);
            }
            if (isTestMonitorFileSecurityChanged)
            {
                AppendText("Test file security change event passed.", Color.Green);
                AppendText("The test file's security was changed.", Color.Gray);
            }
            else
            {
                AppendText("Test file security change event failed.", Color.Red);
            }

            if (isTestMonitorFileWritten)
            {
                AppendText("Test file written event passed.", Color.Green);
                AppendText("The test file was written.", Color.Gray);
            }
            else
            {
                AppendText("Test file written event failed.", Color.Red);
            }


            if (isTestMonitorFileDeleted)
            {
                AppendText("Test file delete event passed.", Color.Green);
                AppendText("The test file was deleted.", Color.Gray);
            }
            else
            {
                AppendText("Test file delete event failed.", Color.Red);
            }
        }


        public void StartFilterUnitTest()
        {
            try
            {
                CleanupTest(unitTestFolder);

                InitializeUnitTest();

                MonitorFileEventsUnitTest();

                MonitorIOEventsUnitTest();
          
                ShowMonitorFileEventsUnitTestResult();


            }
            catch (Exception ex)
            {
                richTextBox_TestResult.Text = "Filter test exception:" + ex.Message;
            }
        }

        private void MonitorDemo_Activated(object sender, EventArgs e)
        {
            if (!isUnitTestStarted)
            {
                isUnitTestStarted = true;

                string lastError = string.Empty;
                if (!filterControl.StartFilter(FilterAPI.FilterType.MONITOR_FILTER, GlobalConfig.FilterConnectionThreads, GlobalConfig.ConnectionTimeOut,licenseKey, ref lastError))
                {
                    MessageBox.Show(lastError, "StartFilter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                System.Threading.Thread.Sleep(3000);

                StartFilterUnitTest();

                filterControl.StopFilter();
            }
        }
    }
}
