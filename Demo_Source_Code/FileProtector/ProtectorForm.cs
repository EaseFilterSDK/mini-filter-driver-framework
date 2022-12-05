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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

using EaseFilter.CommonObjects;
using EaseFilter.FilterControl;

namespace FileProtector
{
    public partial class ProtectorForm : Form
    {
        //Purchase a license key with the link: http://www.easefilter.com/Order.htm
        //Email us to request a trial key: info@easefilter.com //free email is not accepted.
        string licenseKey = "*****************************************************";

        MonitorEventHandler monitorEventHandler = null;
        ControlEventHandler controlEventHandler = null;
        EncryptEventHandler encryptEventHandler = new EncryptEventHandler();

        FilterControl filterControl = new FilterControl();

        public ProtectorForm()
        {
            GlobalConfig.filterType = FilterAPI.FilterType.MONITOR_FILTER | FilterAPI.FilterType.CONTROL_FILTER | FilterAPI.FilterType.ENCRYPTION_FILTER
                | FilterAPI.FilterType.PROCESS_FILTER ;

            InitializeComponent();
            monitorEventHandler = new MonitorEventHandler(listView_Info);
            controlEventHandler = new ControlEventHandler(listView_Info);

            StartPosition = FormStartPosition.CenterScreen;

            DisplayVersion();

            this.Load += new EventHandler(Form1_Load);

        }

        void Form1_Load(object sender, EventArgs e)
        {
            //to improve the listview performance
            SendMessage(listView_Info.Handle, LVM_SETTEXTBKCOLOR, IntPtr.Zero, unchecked((IntPtr)(int)0xFFFFFF));
        }

        ~ProtectorForm()
        {
            GlobalConfig.Stop();
        }

        private void DisplayVersion()
        {
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            try
            {
                string filterDllPath = Path.Combine(GlobalConfig.AssemblyPath, "FilterAPI.Dll");
                version = FileVersionInfo.GetVersionInfo(filterDllPath).ProductVersion;
            }
            catch (Exception ex)
            {
                EventManager.WriteMessage(43, "LoadFilterAPI Dll", EventLevel.Error, "FilterAPI.dll can't be found." + ex.Message);
            }

            this.Text += "    Version:  " + version;
        }

        void SendSettingsToFilter()
        {
            filterControl.ClearFilters();

            GlobalConfig.Load();

            if (GlobalConfig.FilterRules.Count == 0)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("You don't have any filter folder setup, please go to the settings to add a new filter rule, or the filter driver won't intercept any IOs.");
            }

            foreach (FileFilterRule filterRule in GlobalConfig.FilterRules.Values)
            {
                FileFilter fileFilter = filterRule.ToFileFilter();

                //add the monitor event handler for the file filter.
                fileFilter.OnFileOpen += monitorEventHandler.OnFileOpen;
                fileFilter.OnNewFileCreate += monitorEventHandler.OnNewFileCreate;
                fileFilter.OnDeleteFile += monitorEventHandler.OnDeleteFile;
                fileFilter.OnFileRead += monitorEventHandler.OnFileRead;
                fileFilter.OnFileWrite += monitorEventHandler.OnFileWrite;
                fileFilter.OnQueryFileBasicInfo += monitorEventHandler.OnQueryFileBasicInfo;
                fileFilter.OnQueryFileId += monitorEventHandler.OnQueryFileId;
                fileFilter.OnQueryFileNetworkInfo += monitorEventHandler.OnQueryFileNetworkInfo;
                fileFilter.OnQueryFileSize += monitorEventHandler.OnQueryFileSize;
                fileFilter.OnQueryFileStandardInfo += monitorEventHandler.OnQueryFileStandardInfo;
                fileFilter.OnQueryFileInfo += monitorEventHandler.OnQueryFileInfo;
                fileFilter.OnSetFileBasicInfo += monitorEventHandler.OnSetFileBasicInfo;
                fileFilter.OnSetFileNetworkInfo += monitorEventHandler.OnSetFileNetworkInfo;
                fileFilter.OnSetFileSize += monitorEventHandler.OnSetFileSize;
                fileFilter.OnSetFileStandardInfo += monitorEventHandler.OnSetFileStandardInfo;
                fileFilter.OnMoveOrRenameFile += monitorEventHandler.OnMoveOrRenameFile;
                fileFilter.OnSetFileInfo += monitorEventHandler.OnSetFileInfo;
                fileFilter.OnQueryFileSecurity += monitorEventHandler.OnQueryFileSecurity;
                fileFilter.OnQueryDirectoryFile += monitorEventHandler.OnQueryDirectoryFile;
                fileFilter.OnFileHandleClose += monitorEventHandler.OnFileHandleClose;
                fileFilter.OnFileClose += monitorEventHandler.OnFileClose;

                fileFilter.NotifyFileWasChanged += monitorEventHandler.NotifyFileWasChanged;

                //add the control pre-event handler for the control file filter.
                fileFilter.OnPreCreateFile += controlEventHandler.OnPreCreateFile;
                fileFilter.OnPreDeleteFile += controlEventHandler.OnPreDeleteFile;
                fileFilter.OnPreFileRead += controlEventHandler.OnPreFileRead;
                fileFilter.OnPreFileWrite += controlEventHandler.OnPreFileWrite;
                fileFilter.OnPreQueryFileBasicInfo += controlEventHandler.OnPreQueryFileBasicInfo;
                fileFilter.OnPreQueryFileId += controlEventHandler.OnPreQueryFileId;
                fileFilter.OnPreQueryFileNetworkInfo += controlEventHandler.OnPreQueryFileNetworkInfo;
                fileFilter.OnPreQueryFileSize += controlEventHandler.OnPreQueryFileSize;
                fileFilter.OnPreQueryFileStandardInfo += controlEventHandler.OnPreQueryFileStandardInfo;
                fileFilter.OnPreQueryFileInfo += controlEventHandler.OnPreQueryFileInfo;
                fileFilter.OnPreSetFileBasicInfo += controlEventHandler.OnPreSetFileBasicInfo;
                fileFilter.OnPreSetFileNetworkInfo += controlEventHandler.OnPreSetFileNetworkInfo;
                fileFilter.OnPreSetFileSize += controlEventHandler.OnPreSetFileSize;
                fileFilter.OnPreSetFileStandardInfo += controlEventHandler.OnPreSetFileStandardInfo;
                fileFilter.OnPreMoveOrRenameFile += controlEventHandler.OnPreMoveOrRenameFile;
                fileFilter.OnPreSetFileInfo += controlEventHandler.OnPreSetFileInfo;
                fileFilter.OnPreQueryFileSecurity += controlEventHandler.OnPreQueryFileSecurity;
                fileFilter.OnPreQueryDirectoryFile += controlEventHandler.OnPreQueryDirectoryFile;
                fileFilter.OnPreFileHandleClose += controlEventHandler.OnPreFileHandleClose;
                fileFilter.OnPreFileClose += controlEventHandler.OnPreFileClose;
                //add the control post-event handler for the control file filter.
                fileFilter.OnPostCreateFile += controlEventHandler.OnPostCreateFile;
                fileFilter.OnPostDeleteFile += controlEventHandler.OnPostDeleteFile;
                fileFilter.OnPostFileRead += controlEventHandler.OnPostFileRead;
                fileFilter.OnPostFileWrite += controlEventHandler.OnPostFileWrite;
                fileFilter.OnPostQueryFileBasicInfo += controlEventHandler.OnPostQueryFileBasicInfo;
                fileFilter.OnPostQueryFileId += controlEventHandler.OnPostQueryFileId;
                fileFilter.OnPostQueryFileNetworkInfo += controlEventHandler.OnPostQueryFileNetworkInfo;
                fileFilter.OnPostQueryFileSize += controlEventHandler.OnPostQueryFileSize;
                fileFilter.OnPostQueryFileStandardInfo += controlEventHandler.OnPostQueryFileStandardInfo;
                fileFilter.OnPostQueryFileInfo += controlEventHandler.OnPostQueryFileInfo;
                fileFilter.OnPostSetFileBasicInfo += controlEventHandler.OnPostSetFileBasicInfo;
                fileFilter.OnPostSetFileNetworkInfo += controlEventHandler.OnPostSetFileNetworkInfo;
                fileFilter.OnPostSetFileSize += controlEventHandler.OnPostSetFileSize;
                fileFilter.OnPostSetFileStandardInfo += controlEventHandler.OnPostSetFileStandardInfo;
                fileFilter.OnPostMoveOrRenameFile += controlEventHandler.OnPostMoveOrRenameFile;
                fileFilter.OnPostSetFileInfo += controlEventHandler.OnPostSetFileInfo;
                fileFilter.OnPostQueryFileSecurity += controlEventHandler.OnPostQueryFileSecurity;
                fileFilter.OnPostQueryDirectoryFile += controlEventHandler.OnPostQueryDirectoryFile;
                fileFilter.OnPostFileHandleClose += controlEventHandler.OnPostFileHandleClose;
                fileFilter.OnPostFileClose += controlEventHandler.OnPostFileClose;

                //add encrypt event handler if needed.
                fileFilter.OnFilterRequestEncryptKey += encryptEventHandler.OnFilterRequestEncryptKey;

                filterControl.AddFilter(fileFilter);
            }

            filterControl.ProtectedProcessIdList = GlobalConfig.ProtectPidList;
            filterControl.IncludeProcessIdList = GlobalConfig.IncludePidList;
            filterControl.ExcludeProcessIdList = GlobalConfig.ExcludePidList;
            filterControl.BooleanConfig = GlobalConfig.BooleanConfig;

            filterControl.NotifiyFileIOWasBlocked -= controlEventHandler.NotifiyFileIOWasBlocked;
            filterControl.NotifiyFileIOWasBlocked += controlEventHandler.NotifiyFileIOWasBlocked;

            filterControl.NotifiyProcessTerminatedWasBlocked -= controlEventHandler.NotifiyProcessTerminatedWasBlocked;
            filterControl.NotifiyProcessTerminatedWasBlocked += controlEventHandler.NotifiyProcessTerminatedWasBlocked;

            filterControl.NotifyUSBReadWasBlocked -= controlEventHandler.NotifyUSBReadWasBlocked;
            filterControl.NotifyUSBReadWasBlocked += controlEventHandler.NotifyUSBReadWasBlocked;

            filterControl.NotifyUSBWriteWasBlocked -= controlEventHandler.NotifyUSBWriteWasBlocked;
            filterControl.NotifyUSBWriteWasBlocked += controlEventHandler.NotifyUSBWriteWasBlocked;

            filterControl.VolumeControlFlag = (FilterAPI.VolumeControlFlag)GlobalConfig.VolumeControlFlag;
            filterControl.NotifyFilterAttachToVolume -= controlEventHandler.NotifyFilterAttachToVolume;
            filterControl.NotifyFilterAttachToVolume += controlEventHandler.NotifyFilterAttachToVolume;
            filterControl.NotifyFilterDetachFromVolume -= controlEventHandler.NotifyFilterDetachFromVolume;
            filterControl.NotifyFilterDetachFromVolume += controlEventHandler.NotifyFilterDetachFromVolume;

            string lastError = string.Empty;
            if (!filterControl.SendConfigSettingsToFilter(ref lastError))
            {
                MessageBox.Show(lastError, "StartFilter", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }


        private void toolStripButton_StartFilter_Click(object sender, EventArgs e)
        {
            try
            {
                string lastError = string.Empty;

                bool ret = filterControl.StartFilter(GlobalConfig.filterType, GlobalConfig.FilterConnectionThreads, GlobalConfig.ConnectionTimeOut, licenseKey, ref lastError);
                if (!ret)
                {
                    MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                    MessageBox.Show("Start filter failed." + lastError);
                    return;
                }

                SendSettingsToFilter();

                toolStripButton_StartFilter.Enabled = false;
                toolStripButton_Stop.Enabled = true;

                EventManager.WriteMessage(102, "StartFilter", EventLevel.Information, "Start filter service succeeded.");
            }
            catch (Exception ex)
            {
                EventManager.WriteMessage(104, "StartFilter", EventLevel.Error, "Start filter service failed with error " + ex.Message);
            }

        }


        private void toolStripButton_Stop_Click(object sender, EventArgs e)
        {
            filterControl.StopFilter();

            toolStripButton_StartFilter.Enabled = true;
            toolStripButton_Stop.Enabled = false;
        }

        private void toolStripButton_ClearMessage_Click(object sender, EventArgs e)
        {
            controlEventHandler.InitListView();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsForm settingForm = new SettingsForm();
            settingForm.StartPosition = FormStartPosition.CenterParent;
            if (settingForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SendSettingsToFilter();
            }
        }


        private void toolStripDisplayEvent_Click(object sender, EventArgs e)
        {
            EventForm.DisplayEventForm();
        }

        private void encryptFileWithToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EncryptedFileForm encryptForm = new EncryptedFileForm("Encrypt file", Utils.EncryptType.Encryption);
            encryptForm.ShowDialog();
        }

        private void decryptFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EncryptedFileForm encryptForm = new EncryptedFileForm("Decrypt file", Utils.EncryptType.Decryption);
            encryptForm.ShowDialog();
        }

        private void decryptFileWithOffsetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DecryptedFileForm decryptForm = new DecryptedFileForm();
            decryptForm.ShowDialog();
        }


        private void getEncryptedFileIVTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputForm inputForm = new InputForm("Input file name", "Plase input file name", "");

            if (inputForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fileName = inputForm.InputText;

                //by default we set the custom tag data with iv data

                byte[] iv = new Byte[16];
                uint ivLength = (uint)iv.Length;
                bool retVal = FilterAPI.GetAESTagData(fileName, ref ivLength, iv);

                if (!retVal)
                {
                    MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                    MessageBox.Show("GetAESTagData failed with error " + FilterAPI.GetLastErrorMessage(), "GetAESTagData", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("Get encrypted file " + fileName + " tag data succeeded.", "IV Tag", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FilterAPI.ResetConfigData();
            GlobalConfig.Stop();
            filterControl.StopFilter();
            Close();
        }

  
        private void ProtectorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            FilterAPI.ResetConfigData();
            GlobalConfig.Stop();            
            filterControl.StopFilter();
            Application.Exit();
        }

        private void unInstallFilterDriverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            filterControl.StopFilter();
            FilterAPI.UnInstallDriver();
        }     

        private void protectorTutorialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TutorialForm tutorialForm = new TutorialForm();
            tutorialForm.ShowDialog();
        }

        private void toolStripButton_LoadMessage_Click(object sender, EventArgs e)
        {
            controlEventHandler.LoadMessageFromLogToConsole();
        }

        private void toolStripButton_UnitTest_Click(object sender, EventArgs e)
        {
            toolStripButton_Stop_Click(null, null);
            FileProtectorUnitTest fileProtectorUnitTest = new FileProtectorUnitTest();
            FileProtectorUnitTest.licenseKey = licenseKey;

            fileProtectorUnitTest.ShowDialog();
        }

     
        private void toolStripButton_Help_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.easefilter.com/programming.htm");
        }
       
    }
}
