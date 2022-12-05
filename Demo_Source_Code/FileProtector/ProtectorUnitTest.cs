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

using EaseFilter.FilterControl;
using EaseFilter.CommonObjects;

namespace FileProtector
{
    public partial class FileProtectorUnitTest : Form
    {
        FilterControl filterControl = new FilterControl();
        bool isUnitTestStarted = false;

        /// <summary>
        ///  To manage your files, you need to create at least one filter rule, you can have multiple filter rules. 
        ///  A filter rule only can have one unique include file mask,
        ///  A filter rule can have multiple exclude file masks, multiple include process names and exclude process names, 
        ///  multiple include process Ids and exclude process Ids, multiple include user names and exclude user names. 
        /// </summary>
        private static string unitTestFolder = "c:\\EaseFilterUnitTest";
        private static string unitTestFile = "c:\\EaseFilterUnitTest\\unitTestFile.txt";

        /// <summary>
        /// Test monitor feature with registering the IO events, get notification after the file was closed for the registered IO.
        /// </summary>
        private static string unitTestMonitorTestFolder = "c:\\EaseFilterUnitTest\\monitorFolder";
        private static string unitTestMonitorTestFile = "c:\\EaseFilterUnitTest\\monitorFolder\\unitTestMonitorTestFile.txt";
        private static bool isTestMonitorFileCreated = false;
        private static bool isTestMonitorFileReanmed = false;
        private static bool isTestMonitorFileDeleted = false;
        private static bool isTestMonitorFileWritten = false;
        private static bool isTestMonitorFileRead = false;
        private static bool isTestMonitorFileSecurityChanged = false;
        private static bool isTestMonitorFileInfoChanged = false;

        public static bool isNewProcessCreated = false;


        /// <summary>
        /// Test Control IO feature with callback function, you can allow or block the file access in the callback function based on the 
        /// user and file information, here we demo how to block the new file creation, file rename, file delete IO.
        /// </summary>
        private static string unitTestCallbackFolder = "c:\\EaseFilterUnitTest\\callbackFolder";
        private static string unitTestCallbackFile = "c:\\EaseFilterUnitTest\\callbackFolder\\unitTestFile.txt";
        private static string unitTestCallbackBlockNewFile = unitTestCallbackFolder + "\\blockNewFileCreationFile.txt";
        private static string unitTestCallbackTestReparseFile = unitTestCallbackFolder + "\\reparseTestFile.txt";
        private static string unitTestCallbackReparseTargetFile = unitTestCallbackFolder + "\\reparseTargetFile.txt";
        private static string unitTestCallbackDeletionPreventionFile = "c:\\EaseFilterUnitTest\\callbackFolder\\deletionPreventionInCallbackUnitTestFile.txt";
        private static string unitTestCopyAfterDeleteCallbackFile = "c:\\EaseFilterUnitTest\\callbackFolder\\unitTestCopyAfterDeleteCallbackFile.txt";


        /// <summary>
        /// Set the exclude folders for a filter rule, exclude file mask must be the subset of the include file mask.
        /// All IO from the file names which match both include file mask and exclude file mask won't be intercepted by filter driver, 
        /// it meant it will be skipped.
        /// </summary>
        private static string filterRuleExcludeTestFolder = "c:\\EaseFilterUnitTest\\excludeFolder";
        private static string excludeFolderTestFile = "c:\\EaseFilterUnitTest\\excludeFolder\\excludeFile.txt";


        /// <summary>
        /// Set the exclude filter rule, it meant all IO from the excludeFilterRuleTestFolder won't be intercepted by filter driver.
        /// Exclude filter rule is a global setting, exclude filter rule has the highest priority.
        /// </summary>
        private static string globalExcludeFilterRuleTestFolder = "c:\\EaseFilterUnitTest\\excludeFilterRuleFolder";
        private static string globalExcludeFilterRuleTestFile = "c:\\EaseFilterUnitTest\\excludeFilterRuleFolder\\excludeFilterRuleTestFile.txt";

        //Purchase a license key with the link: http://www.easefilter.com/Order.htm
        //Email us to request a trial key: info@easefilter.com //free email is not accepted.
        public static string licenseKey = "******************************************";

        public FileProtectorUnitTest()
        {
            InitializeComponent();

        }          

        private void InitializeUnitTest()
        {
            if (!Directory.Exists(unitTestFolder))
            {
                Directory.CreateDirectory(unitTestFolder);
            }

            if (!File.Exists(unitTestFile))
            {
                File.AppendAllText(unitTestFile, "This is unit test file.");
            }

            if (!Directory.Exists(unitTestMonitorTestFolder))
            {
                Directory.CreateDirectory(unitTestMonitorTestFolder);
            }


            if (!Directory.Exists(globalExcludeFilterRuleTestFolder))
            {
                Directory.CreateDirectory(globalExcludeFilterRuleTestFolder);
            }

            if (!File.Exists(globalExcludeFilterRuleTestFile))
            {
                File.AppendAllText(globalExcludeFilterRuleTestFile, "This is unit test excludeFilterRuleTestFolder file.");
            }


            if (!Directory.Exists(filterRuleExcludeTestFolder))
            {
                Directory.CreateDirectory(filterRuleExcludeTestFolder);
            }

            if (!File.Exists(excludeFolderTestFile))
            {
                File.AppendAllText(excludeFolderTestFile, "This is excludeFolderTestFile test file.");
            }

            if (!Directory.Exists(unitTestCallbackFolder))
            {
                Directory.CreateDirectory(unitTestCallbackFolder);
            }

            if (!File.Exists(unitTestCallbackFile))
            {
                File.AppendAllText(unitTestCallbackFile, "This is unitTestCallbackFile test file.");
            }

            if (!File.Exists(unitTestCallbackDeletionPreventionFile))
            {
                File.AppendAllText(unitTestCallbackDeletionPreventionFile, "This is unitTestCopyBeforeDeleteCallbackFile test file.");
            }
        }

        private void CleanupTestFolder(string folder)
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
     
        private void ShowMonitorFileEventsUnitTestResult()
        {

            /// print out the monitor IO events test result.
            string message = "File IO Events Unit Test.";
            AppendText(message, Color.Black);
            message = " Register the monitor file IO events: create,rename,delete,written,file info changed, read, security change in the test folder." + Environment.NewLine;
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
                | FilterAPI.FileChangedEvents.NotifyFileWasRenamed | FilterAPI.FileChangedEvents.NotifyFileWasWritten | FilterAPI.FileChangedEvents.NotifyFileSecurityWasChanged | FilterAPI.FileChangedEvents.NotifyFileWasRead);


            //register the events
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

        private void FolderProtectionUnitTest()
        {
            //
            //Test case2: Folder Protection Unit Test
            //All files in the test folder "c:\\EaseFilterUnitTest" will be readonly, except the current process, you can add more authorized processes here.   
            //All files in the test folder "c:\\EaseFilterUnitTest" can't be accessed by process "explorer.exe" and "cmd.exe", you can add more processes.
            //There are exception for the sub folder "excludeFolder", no any restriction to this folder.

            //pros: it can cover all files in the protected folder.

            //cons: the blocked processes don't have permission to access the files in the protected folder.
            //      it only can block the listed processes.

            string lastError = string.Empty;
            string message = "Folder Protection Unit Test";
            AppendText(message, Color.Black);

            message = "1. The protected folder is read only to all processes except for the authorized processes." + Environment.NewLine;
            message += "2. Block the explorer to copy&paste the protected files." + Environment.NewLine;
            message += "3. Block the protected files file access from remote network." + Environment.NewLine;
            message += "4. Set up an exclude sub foler of the protected folder which excludes the security restrition." + Environment.NewLine;

            AppendText(message, Color.Gray);

            //Prevent copy&paste operation from explorer and dos prompt.
            FileFilter blockAccessFilter = new FileFilter(unitTestFolder + "\\*");
            //exclude the folder from the filter
            blockAccessFilter.ExcludeFileFilterMaskList.Add(filterRuleExcludeTestFolder + "\\*");
            //blocked application list.
            blockAccessFilter.IncludeProcessNameList.Add("explorer.exe");
            blockAccessFilter.IncludeProcessNameList.Add("cmd.exe");
            blockAccessFilter.EnableReadFileData = false;
         

            //set the accessFlag to readonly for all the files in the test folder except the current process.
            FileFilter readonlyFilter = new FileFilter(unitTestFolder + "*");
            //exclude the folder from the filter
            readonlyFilter.ExcludeFileFilterMaskList.Add(filterRuleExcludeTestFolder + "\\*");
            readonlyFilter.EnableDeleteFile = false;
            readonlyFilter.EnableFileCreation = false;
            readonlyFilter.EnableWriteToFile = false;
            readonlyFilter.EnableChangeFileSecurity = false;
            readonlyFilter.EnableChangeFileInfo = false;
            readonlyFilter.EnableFileBeingAccessedViaNetwork = false;

            try
            {
                filterControl.ClearFilters();
                filterControl.AddFilter(blockAccessFilter);
                filterControl.AddFilter(readonlyFilter);
                filterControl.SendConfigSettingsToFilter(ref lastError);

                // The current process should have the read permission.
                try
                {
                    string textData = File.ReadAllText(unitTestFile);
                }
                catch (Exception ex)
                {
                    AppendText("Read file " + unitTestFile + " failed." + ex.Message, Color.Red);
                    return;
                }

                try
                {
                    string cmdExcludeFileName = filterRuleExcludeTestFolder + "\\cmd_copy.txt";

                    //for files in the exclude folder, anyone can read and write here.
                    File.Copy(unitTestFile, cmdExcludeFileName);

                    if (File.Exists(cmdExcludeFileName))
                    {
                        AppendText("Test with exclude folder passed.", Color.Green);
                        AppendText("Copy a protected test file to the exclude sub folder succeeded.", Color.Gray);
                    }
                    else
                    {
                        AppendText("Test with excludeFolder failed.", Color.Red);
                    }

                }
                catch (Exception ex)
                {
                    AppendText("Test with exclude folder failed." + ex.Message, Color.Red);
                    return;
                }

                try
                {
                    //The current process shouldn't have the write permission.
                    File.AppendAllText(unitTestFile, ",new test data was added.");

                    AppendText("Test with blocking write access failed.", Color.Red);
                    return;

                }
                catch
                {
                    AppendText("Test with blocking write access passed.", Color.Green);
                    AppendText("Write data to the protected file was blocked.", Color.Gray);
                }


                filterControl.RemoveFilter(readonlyFilter);

                //here you can put your exclude process name which has the full permission to the test folder.
                readonlyFilter.ExcludeProcessIdList.Add(FilterAPI.GetCurrentProcessId());
                filterControl.AddFilter(readonlyFilter);
                filterControl.SendConfigSettingsToFilter(ref lastError);

                try
                {
                    //The current process should have the write permission.
                    File.AppendAllText(unitTestFile, ",new test data was added.");

                    AppendText("Test with exclude process passed.", Color.Green);
                    AppendText("Write data to the protected file was passed with authorized process.", Color.Gray);
                }
                catch
                {
                    AppendText("Test with exclude process failed.", Color.Red);
                    return;
                }

                try
                {
                    //the cmd.exe should be blocked to access here
                    string cmdBlockFileName = unitTestFolder + "\\cmd_copy.txt";
                    Process.Start("cmd", "/C copy " + unitTestFile + "  " + cmdBlockFileName);

                    if (File.Exists(cmdBlockFileName))
                    {
                        AppendText("Test with include process cmd failed.", Color.Red);
                        return;
                    }
                    else
                    {
                        AppendText("Test with include process cmd passed.", Color.Green);
                        AppendText("Copy protected file from cmd.exe in dos prompt was blocked.", Color.Gray);
                    }
                }
                catch (Exception ex)
                {
                    AppendText("Test with include process cmd failed." + ex.Message, Color.Red);
                }


                //blockAccessFilterRule inlcude file mask is the subset of the include file mask of the readonlyFilterRule
                //the filter driver will check with the filter rule blockAccessFilterRule, if it doesn't match, then check with readonlyFilterRule.

                AppendText("Test with sub folder filter rule passed.", Color.Green);
                AppendText("Create two filter rules, the files inside the sub folder matched with sub folder filter rule.", Color.Gray);

            }
            catch (Exception ex)
            {
                AppendText("Test readonly folder failed." + ex.Message, Color.Red);
            }
            finally
            {
                filterControl.ClearFilters();
                filterControl.SendConfigSettingsToFilter(ref lastError);
            }

        }

        private void AddProcessRightsUnitTest()
        {
            //
            //Test case3: Add Process Rights Unit Test
            //Test folder "c:\\EaseFilterUnitTest" .   

            //pros: You can set the specific access rights to different processes in the protected folder, all other processes will have the same access rights of the filter rule.
            string lastError = string.Empty;
            string message = "Add Process Rights Unit Test";
            AppendText(message, Color.Black);

            message = "1. Remove the access rights for process 'cmd.exe' to files in test folder." + Environment.NewLine;
            AppendText(message, Color.Gray);

            //Prevent copy&paste operation from explorer and dos prompt.
            FileFilter blockAccessFilter = new FileFilter(unitTestFolder + "\\*");

            //Remove all access rights for the process "cmd".
            blockAccessFilter.ProcessNameAccessRightList.Add("cmd.exe",((uint)FilterAPI.AccessFlag.LEAST_ACCESS_FLAG));

            try
            {
                filterControl.ClearFilters();
                filterControl.AddFilter(blockAccessFilter);
                filterControl.SendConfigSettingsToFilter(ref lastError);

                // The current process should have all permission.
                File.AppendAllText(unitTestFile, ",new test data was added.");
                string textData = File.ReadAllText(unitTestFile);

                //the cmd.exe should be blocked to access here
                string cmdBlockFileName = unitTestFolder + "\\cmd_copy.txt";
                //delete the test file first
                File.Delete(cmdBlockFileName);
                Process.Start("cmd", "/C copy " + unitTestFile + "  " + cmdBlockFileName);

                if (File.Exists(cmdBlockFileName))
                {
                    AppendText("Test add process rights failed.", Color.Red);
                    return;
                }
                else
                {
                    AppendText("Test add process rights passed.", Color.Green);
                    AppendText("Remove process cmd.exe access rights succeeded.", Color.Gray);
                }
            }
            catch (Exception ex)
            {
                AppendText("Test add process rights failed." + ex.Message, Color.Red);
            }
            finally
            {
                filterControl.ClearFilters();
                filterControl.SendConfigSettingsToFilter(ref lastError);
            }

        }

        private void AddUserRightsUnitTest()
        {
            //
            //Test case4: Add User Rights Unit Test
            //Set current user with readonly right access to folder "c:\\EaseFilterUnitTest".   

            //pros: You can set the specific access rights to different users in the protected folder, all other users will have the same access rights of the filter rule.
            string lastError = string.Empty;
            string message = "Add User Rights Unit Test";
            AppendText(message, Color.Black);

            string userName = Environment.UserDomainName + "\\" + Environment.UserName;

            message = "1. The protected folder can't be acccessed by the current user:" + userName + Environment.NewLine;
            AppendText(message, Color.Gray);

            //Prevent copy&paste operation from explorer and dos prompt.
            FileFilter blockAccessFilter = new FileFilter(unitTestFolder + "\\*");

            //Remove all access rights to the current user.
            blockAccessFilter.userAccessRightList.Add(userName,((uint)FilterAPI.AccessFlag.LEAST_ACCESS_FLAG));

            try
            {
                filterControl.ClearFilters();
                filterControl.AddFilter(blockAccessFilter);
                filterControl.SendConfigSettingsToFilter(ref lastError);

                try
                {
                    // The current process doesnt have any permission.
                    File.AppendAllText(unitTestFile, ",new test data was added.");
                    AppendText("Test add user rights failed.", Color.Red);
                }
                catch
                {
                    AppendText("Test add user rights passed.", Color.Green);
                    AppendText("Remove current user access rights succeeded.", Color.Gray);
                    return;
                }

            }
            catch (Exception ex)
            {
                AppendText("Test add user rights failed." + ex.Message, Color.Red);
            }
            finally
            {
                filterControl.ClearFilters();
                filterControl.SendConfigSettingsToFilter(ref lastError);
            }

        }

        private void NoRenameAndCopyUnitTest()
        {


            //
            //Test case5: prevent specific files in protected folder from being copied out 
            //All files with protected extension ".prt" only can store in protected folder "c:\\EaseFilterUnitTest".   
            //
            //pros: the simplest way to prevent the protected files from being copied out.
            //cons: it only can block the specific files.     

            GlobalConfig.FilterRules.Clear();
            string lastError = string.Empty;
            string message = "Protect Files With Extension '.prt'";
            AppendText(message, Color.Black);

            message = "1. Prevent the protected files from renaming and deleting." + Environment.NewLine;
            message += "2. Prevent the protected files from copying out of the protected folder." + Environment.NewLine;
            message += "3. Set up an exclude sub foler of the protected folder which excludes the security restrition." + Environment.NewLine;

            AppendText(message, Color.Gray);

            //not allow file rename or delete and copy out of the folder
            FileFilter noRenameFilter = new FileFilter(unitTestFolder + "*.prt");
            noRenameFilter.EnableRenameOrMoveFile = false;
            noRenameFilter.EnableDeleteFile = false;
            noRenameFilter.EnableFileBeingCopied = false;

            FileFilter globalExcludeFilter = new FileFilter(filterRuleExcludeTestFolder + "\\*");

            try
            {

                filterControl.ClearFilters();
                filterControl.AddFilter(noRenameFilter);
                filterControl.AddFilter(globalExcludeFilter);
                filterControl.SendConfigSettingsToFilter(ref lastError);

                string protectedFileName = unitTestFolder + "\\protectFile.prt";
                File.AppendAllText(protectedFileName, "This is protected file with .prt extension.");

                //file rename is blocked 
                try
                {
                    File.Move(protectedFileName, unitTestFolder + "\\test.txt");
                    AppendText("Test with file rename prevention failed.", Color.Red);
                    return;
                }
                catch
                {
                    AppendText("Test with file rename prevention passed.", Color.Green);
                    AppendText("Block the protected file being renamed.", Color.Gray);
                }

                //file delete is blocked 
                try
                {
                    File.Delete(protectedFileName);
                    AppendText("Test with file delete prevention failed.", Color.Red);
                    return;
                }
                catch
                {
                    AppendText("Test with file delete prevention passed.", Color.Green);
                    AppendText("Block the protected file being deleted.", Color.Gray);
                }

                //exclude folder file deletion is allowed
                try
                {
                    File.Delete(globalExcludeFilterRuleTestFile);
                    AppendText("Test global exclude filter rule passed.", Color.Green);
                    AppendText("Delete the file from the exclude folder succeeded.", Color.Gray);
                }
                catch (Exception ex)
                {
                    AppendText("Test global exclude filter rule failed." + ex.Message, Color.Red);
                    return;
                }

                //protected file copy out to the protect folder is blocked 
                try
                {
                    File.Copy(protectedFileName, "c:\\test.txt");
                    AppendText("Test with file copying out prevention failed.", Color.Red);
                    return;
                }
                catch
                {
                    AppendText("Test with file copying out prevention passed.", Color.Green);
                    AppendText("Copy the protected file outside of the protected folder is blocked.", Color.Gray);
                }
            }
            catch (Exception ex)
            {
                AppendText("NoRenameAndCopyUnitTest failed." + ex.Message, Color.Red);
                return;
            }
            finally
            {
                filterControl.ClearFilters();
                filterControl.SendConfigSettingsToFilter(ref lastError);
            }
        }

        private void FolderLockerUnitTest()
        {

            //
            //Test case6: Folder Locker
            //1. Transparently encrypt/decrpt files files in the test folder "c:\\EaseFilterUnitTest".   
            //2. All files are hidden in the test folder "c:\\EaseFilterUnitTest".
            //3. Add/Remove the exception application which will be excluded from the restriction.
            string lastError = string.Empty;
            string message = "Folder Locker Unit Test";
            AppendText(message, Color.Black);

            message = " 1. Encrypt files transparently in the locked folder." + Environment.NewLine;
            message += " 2. Hide protected files to unauthorized processes." + Environment.NewLine;
            message += " 3. Allow authorized processes to read encrypted files." + Environment.NewLine;

            AppendText(message, Color.Gray);

            //the authorized processes in the include process list.
            FileFilter authorizedProcessFilter = new FileFilter(unitTestFolder + "\\*");
            authorizedProcessFilter.IncludeProcessIdList.Add(FilterAPI.GetCurrentProcessId());
            authorizedProcessFilter.EnableEncryption = true;
            authorizedProcessFilter.EncryptionPassPhrase = "password";

            //all other processes can't read the files, can't see the files.
            FileFilter folderLockerFilter = new FileFilter(unitTestFolder + "*");
            folderLockerFilter.EnableEncryption = true;
            folderLockerFilter.EncryptionPassPhrase = "password";
            folderLockerFilter.EnableReadEncryptedData = false;
            folderLockerFilter.EnableHiddenFile = true;
            folderLockerFilter.HiddenFileFilterMaskList.Add("*");

            try
            {
                filterControl.ClearFilters();                
                filterControl.AddFilter(folderLockerFilter);
                filterControl.SendConfigSettingsToFilter(ref lastError);

                string folderLockerTestFileName = unitTestFolder + "\\folderLockerTestFile.txt";
                string testContent = "This is folderLockerTestFileName FileName test file.";

                File.AppendAllText(folderLockerTestFileName, testContent);

                string[] txtFileList = Directory.GetFiles(unitTestFolder, "*.txt");
                if (txtFileList.Length == 0)
                {
                    //All .txt file name should be hidden, the file list should be empty.
                    AppendText("Test hidden file list passed.", Color.Green);
                    AppendText("Hide the protected files from directory browsing.", Color.Gray);
                }
                else
                {
                    AppendText("Test hidden file list failed.", Color.Red);
                    return;
                }

                try
                {
                    //read the encrypted file was blocked.
                    string readContent = File.ReadAllText(folderLockerTestFileName);

                    if (string.Compare(testContent, readContent, false) != 0)
                    {
                        AppendText("Test read raw encrypted data passed.", Color.Green);
                        AppendText("Protected file's data can't be read.", Color.Gray);
                    }
                    else
                    {
                        AppendText("Test read raw encrypted data failed.", Color.Red);
                        return;
                    }

                    return;

                }
                catch
                {
                    AppendText("Test block read access passed.", Color.Green);
                    AppendText("Block the protected file reading from unauthorized processes.", Color.Gray);
                }


                filterControl.AddFilter(authorizedProcessFilter);
                filterControl.SendConfigSettingsToFilter(ref lastError);

                //the current process was added with exception filter rule.
                txtFileList = Directory.GetFiles(unitTestFolder, "*.txt");
                if (txtFileList.Length > 0)
                {
                    //All .txt file name should be listed now for the current process.
                    AppendText("Test authorized processes filter rule passed.", Color.Green);

                    AppendText("Add authorized processes which can get the file list.", Color.Gray);
                }
                else
                {
                    AppendText("Test  authorized processes  filter rule failed.", Color.Red);
                    return;
                }

                try
                {
                    IntPtr fileHandle = IntPtr.Zero;

                    if (Utils.OpenRawEnCyptedFile(folderLockerTestFileName, out fileHandle, out lastError))
                    {
                        SafeFileHandle shFile = new SafeFileHandle(fileHandle, true);
                        FileStream fs = new FileStream(shFile, FileAccess.Read);
                        byte[] buffer = new byte[1024];
                        int len = fs.Read(buffer, 0, 1024);

                        string encryptedText = Encoding.Unicode.GetString(buffer);
                        encryptedText = encryptedText.TrimEnd((char)0);

                        fs.Close();

                        if (string.Compare(testContent, encryptedText, false) != 0)
                        {
                            AppendText("Test folder locker transparent file encryption passed.", Color.Green);
                            AppendText("Protected file's data was encrypted.", Color.Gray);
                        }
                        else
                        {
                            AppendText("Test folder locker transparent file encryption failed.", Color.Red);
                            return;
                        }

                    }
                    else
                    {
                        AppendText("Test folder locker transparent file encryption failed.OpenRawEnCyptedFile failed:" + lastError, Color.Red);
                        return;
                    }

                    string readContent = File.ReadAllText(folderLockerTestFileName);
                    if (string.Compare(testContent, readContent, false) == 0)
                    {
                        AppendText("Test folder locker transparent file decryption passed.", Color.Green);
                        AppendText("Protected file's data was decrypted.", Color.Gray);
                    }
                    else
                    {
                        AppendText("Test folder locker transparent file decryption failed.", Color.Red);
                        return;
                    }
                }
                catch(Exception ex)
                {
                    AppendText("Test  folder locker transparent file decryption failed." + ex.Message, Color.Red);
                    return;
                }

            }
            catch( Exception ex)
            {
                AppendText("Test folder locker failed." + ex.Message, Color.Red);
            }
            finally
            {
                filterControl.ClearFilters();
                filterControl.SendConfigSettingsToFilter(ref lastError);
            }

        }

        /// <summary>
        /// Fires this event after the new file was created, the handle is not closed.
        /// </summary>
        public void OnPreCreateFile(object sender, FileCreateEventArgs e)
        {
            if (string.Compare(unitTestCallbackBlockNewFile, e.FileName, true) == 0)
            {
                //test block the file open here.
                e.ReturnStatus = NtStatus.Status.AccessDenied;
            }            
            else if (string.Compare(unitTestCallbackTestReparseFile, e.FileName, true) == 0)
            {
                //test reparse file open to other file.
                e.reparseFileName = unitTestCallbackReparseTargetFile;
                e.ReturnStatus = NtStatus.Status.Reparse;
                e.IsDataModified = true;
            }

        }

        /// <summary>
        /// Fires the event before the file is going to be renamed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnPreMoveOrRenameFile(object sender, FileMoveOrRenameEventArgs e)
        {
            if (string.Compare(unitTestCallbackFile, e.FileName, true) == 0)
            {
                e.ReturnStatus = NtStatus.Status.AccessDenied;
            }
        }

        private static bool CopyFileBeforeDeltion(string fileName)
        {
            try
            {
                IntPtr fileHandle = IntPtr.Zero;
                bool retVal = FilterAPI.GetFileHandleInFilter(fileName, (uint)FileAccess.Read, ref fileHandle);
                if (retVal)
                {
                    SafeFileHandle sHandle = new SafeFileHandle(fileHandle, false);
                    FileStream fileStream = new FileStream(sHandle, FileAccess.Read);

                    FileStream copyFileStream = new FileStream(unitTestCopyAfterDeleteCallbackFile, FileMode.Create, FileAccess.Write, FileShare.None);

                    byte[] buffer = new byte[4096];
                    int readLen = 0;

                    do
                    {
                        readLen = fileStream.Read(buffer, 0, buffer.Length);

                        if (readLen > 0)
                        {
                            copyFileStream.Write(buffer, 0, readLen);
                        }
                    }
                    while (readLen > 0);
                    
                    FilterAPI.CloseFileHandleInFilter(fileHandle);
                    copyFileStream.Close();


                }

            }
            catch (Exception ex)
            {
                EventManager.WriteMessage(156, "CopyFileBeforeDeltion", EventLevel.Error, "CopyFileBeforeDeltion failed." + ex.Message);
            }

            return true;
        }


        /// <summary>
        /// Fires the event before the file is going to be deleted.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnPreDeleteFile(object sender, FileIOEventArgs e)
        {
            e.ReturnStatus = NtStatus.Status.Success;

            if (string.Compare(unitTestCallbackFile, e.FileName, true) == 0)
            {
                e.ReturnStatus = NtStatus.Status.AccessDenied;
            }

            if (string.Compare(unitTestCallbackDeletionPreventionFile, e.FileName, true) == 0)
            {
                e.ReturnStatus = NtStatus.Status.AccessDenied;                
            }

        }

        private void FileAccessControlUnitTest()
        {

            //
            //Test case7: Control the file IO with callback function.
            // 1. You will get the notification before the file was opened,renamed,deleted. 
            // 2. In callback funtion, you can decide if proceed the IO operation based on the information
            //    (user name,process name,file information, file content). 
            // 3. For the test file, it will be blocked to create new file,rename file, delete file.
            string lastError = string.Empty;
            string message = "File Access Control With Callback Function Unit Test";
            AppendText(message, Color.Black);

            message = " 1. Register the PRE_CREATE and PRE_SET_INFORMATION." + Environment.NewLine;
            message += " 2. The callback function will get the notification before the protected file was opened or file information was changed." + Environment.NewLine;
            message += " 3. Block the new file creation, file rename and file deletion in callback function." + Environment.NewLine;

            AppendText(message, Color.Gray);

            FileFilter callbackControlFilter = new FileFilter(unitTestCallbackFolder + "\\*");
            callbackControlFilter.ControlFileIOEventFilter = (ulong)(ControlFileIOEvents.OnPreFileCreate | ControlFileIOEvents.OnPreMoveOrRenameFile| ControlFileIOEvents.OnPreDeleteFile);
            callbackControlFilter.OnPreCreateFile += OnPreCreateFile;
            callbackControlFilter.OnPreMoveOrRenameFile += OnPreMoveOrRenameFile;
            callbackControlFilter.OnPreDeleteFile += OnPreDeleteFile;


            try
            {
                filterControl.ClearFilters();
                filterControl.AddFilter(callbackControlFilter);
                filterControl.SendConfigSettingsToFilter(ref lastError);

                try
                {
                    if (File.Exists(unitTestCallbackFile))
                    {
                        File.Delete(unitTestCallbackFile);
                        AppendText("Test block file deletion in callback failed. file " + unitTestCallbackFile + " was deleted.", Color.Red);
                        return;
                    }
                    else
                    {
                        AppendText("Test block file deletion in callback failed. file " + unitTestCallbackFile + " doesn't exist.", Color.Red);
                        return;
                    }

                }
                catch
                {
                    AppendText("Test block file deletion in callback  passed.", Color.Green);
                    AppendText("Delete protected file was blocked by callback function.", Color.Gray);
                }

                try
                {
                    string blockRenameFileName = unitTestFolder + "\\blockRenameFile.txt";
                    File.Move(unitTestCallbackFile, blockRenameFileName);
                    AppendText("Test block file rename in callback failed.", Color.Red);
                    return;
                }
                catch
                {
                    AppendText("Test block file rename in callback  passed.", Color.Green);
                    AppendText("Rename protected file was blocked by callback function.", Color.Gray);
                }

                try
                {
                    FileStream fs = new FileStream(unitTestCallbackBlockNewFile, FileMode.CreateNew);
                    fs.Close();

                    AppendText("Test block new file creation in callback failed.", Color.Red);
                    return;
                }
                catch
                {
                    AppendText("Test block the new file creation in callback passed.", Color.Green);
                    AppendText("Create new file was blocked by callback function.", Color.Gray);
                }

            }
            catch (Exception ex)
            {
                AppendText("Test File Access Control failed." + ex.Message, Color.Red);
            }
            finally
            {
                filterControl.ClearFilters();
                filterControl.SendConfigSettingsToFilter(ref lastError);
            }
        }

        private void DeletionCallbackUnitTest()
        {

            //
            //Test case8: copy file before it was deleted 
            //Make a copy of the file before it was deleted.   
            string lastError = string.Empty;
            string message = "Copy Protected File Before File Deletion Unit Test";
            AppendText(message, Color.Black);

            message = " Make a copy of the file before it was deleted." + Environment.NewLine;
            AppendText(message, Color.Gray);

            FileFilter deletionCallbackFilter = new FileFilter(unitTestCallbackFolder + "\\*");
            deletionCallbackFilter.ControlFileIOEventFilter = (ulong)(ControlFileIOEvents.OnPreFileCreate | ControlFileIOEvents.OnPreDeleteFile);
            deletionCallbackFilter.OnPreDeleteFile += OnPreDeleteFile;


            try
            {
                filterControl.ClearFilters();
                filterControl.AddFilter(deletionCallbackFilter);
                filterControl.SendConfigSettingsToFilter(ref lastError);

                try
                {
                    File.Delete(unitTestCallbackDeletionPreventionFile);

                    //the deletion should be blocked in callback function.
                    AppendText("Test deletion prevention in callback failed.", Color.Red);
                    return;

                }
                catch
                {
                }

                filterControl.ClearFilters();
                filterControl.SendConfigSettingsToFilter(ref lastError);

                try
                {
                    File.Delete(unitTestCallbackDeletionPreventionFile);

                    //Now the file can be deleted.
                    AppendText("Test deletion prevention in callback passed.", Color.Green);
                    return;

                }
                catch
                {
                }


            }
            catch (Exception ex)
            {
                AppendText("Test deletion prevention in callback exception:" + ex.Message, Color.Red);
            }
            finally
            {
                filterControl.ClearFilters();
                filterControl.SendConfigSettingsToFilter(ref lastError);
            }
        }

        private void ReparseFileOpenUnitTest()
        {

            //
            //Test case9: Reparse file open to the new file path.
            string lastError = string.Empty;
            string message = "Reparse file open to the new file path unit test";
            AppendText(message, Color.Black);

            message = "Reparse file " + unitTestCallbackTestReparseFile + " open to " + unitTestCallbackReparseTargetFile + Environment.NewLine;
            AppendText(message, Color.Gray);

            FileFilter reparseFileFilter = new FileFilter(unitTestCallbackFolder + "\\*");
            reparseFileFilter.ControlFileIOEventFilter = (ulong)(ControlFileIOEvents.OnPreFileCreate);
            reparseFileFilter.OnPreCreateFile += OnPreCreateFile;

            try
            {
                filterControl.ClearFilters();
                filterControl.AddFilter(reparseFileFilter);
                filterControl.SendConfigSettingsToFilter(ref lastError);

                string targetFileContent = "unitTestCallbackReparseTargetFile content";
                File.AppendAllText(unitTestCallbackReparseTargetFile, targetFileContent);

                string readContent = File.ReadAllText(unitTestCallbackTestReparseFile);
                if (string.Compare(readContent, targetFileContent, true) == 0)
                {
                    AppendText("Test reparse file open passed.", Color.Green);
                    AppendText("reparse file open to another file.", Color.Gray);
                }
                else
                {
                    AppendText("Test reparse file open failed.", Color.Red);
                    return;
                }


            }
            catch (Exception ex)
            {
                AppendText("Test reparse file open exception:" + ex.Message, Color.Red);
            }
            finally
            {
                filterControl.ClearFilters();
                filterControl.SendConfigSettingsToFilter(ref lastError);
            }
        }

        public void StartFilterUnitTest()
        {
            try
            {

                CleanupTestFolder(unitTestFolder);

                InitializeUnitTest();

                MonitorFileEventsUnitTest();

                FolderProtectionUnitTest();

                AddProcessRightsUnitTest();

                AddUserRightsUnitTest();

                NoRenameAndCopyUnitTest();

                FolderLockerUnitTest();

                FileAccessControlUnitTest();

                DeletionCallbackUnitTest();

                ReparseFileOpenUnitTest();

                ShowMonitorFileEventsUnitTestResult();



            }
            catch (Exception ex)
            {
                richTextBox_TestResult.Text = "Filter test exception:" + ex.Message;
            }
        }

        private void FileProtectorUnitTest_Activated(object sender, EventArgs e)
        {
            if (!isUnitTestStarted)
            {
                isUnitTestStarted = true;

                string lastError = string.Empty;
                if (!filterControl.StartFilter(GlobalConfig.filterType, GlobalConfig.FilterConnectionThreads, GlobalConfig.ConnectionTimeOut, licenseKey, ref lastError))
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
