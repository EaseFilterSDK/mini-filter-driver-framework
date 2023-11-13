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
        MessageHandler messageHandler = null;
        MonitorEventHandler monitorEventHandler = null;
        ControlEventHandler controlEventHandler = null;
        ProcessEventHandler processEventHandler = null;
        EncryptEventHandler encryptEventHandler = new EncryptEventHandler();

        FilterControl filterControl = new FilterControl();

        public ProtectorForm()
        {
            GlobalConfig.filterType = FilterAPI.FilterType.MONITOR_FILTER | FilterAPI.FilterType.CONTROL_FILTER | FilterAPI.FilterType.ENCRYPTION_FILTER
               | FilterAPI.FilterType.PROCESS_FILTER | FilterAPI.FilterType.REGISTRY_FILTER;

            InitializeComponent();

            messageHandler = new MessageHandler(listView_Info);
            monitorEventHandler = new MonitorEventHandler(messageHandler);
            controlEventHandler = new ControlEventHandler(messageHandler);
            processEventHandler = new ProcessEventHandler(messageHandler);

            StartPosition = FormStartPosition.CenterScreen;

            this.Text += GlobalConfig.GetVersionInfo();

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

            foreach (ProcessFilterRule filterRule in GlobalConfig.ProcessFilterRules.Values)
            {
                ProcessFilter processFilter = filterRule.ToProcessFilter();

                processFilter.OnProcessCreation += processEventHandler.OnProcessCreation;
                processFilter.OnProcessPreTermination += processEventHandler.OnProcessPreTermination;
                processFilter.NotifyProcessWasBlocked += processEventHandler.NotifyProcessWasBlocked;
                processFilter.NotifyProcessTerminated += processEventHandler.NotifyProcessTerminated;
                processFilter.NotifyThreadCreation += processEventHandler.NotifyThreadCreation;
                processFilter.NotifyThreadTerminated += processEventHandler.NotifyThreadTerminated;
                processFilter.NotifyProcessHandleInfo += processEventHandler.NotifyProcessHandleInfo;
                processFilter.NotifyThreadHandleInfo += processEventHandler.NotifyThreadHandleInfo;

                filterControl.AddFilter(processFilter);
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
                //Purchase a license key with the link: http://www.easefilter.com/Order.htm
                //Email us to request a trial key: info@easefilter.com //free email is not accepted.        
                string licenseKey = GlobalConfig.LicenseKey;

                string lastError = string.Empty;

                bool ret = filterControl.StartFilter(GlobalConfig.filterType, GlobalConfig.FilterConnectionThreads, GlobalConfig.ConnectionTimeOut, licenseKey, ref lastError);
                if (!ret)
                {
                    MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                    MessageBox.Show("Start filter failed." + lastError, "StartFilter", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            messageHandler.InitListView();
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


        private void getEncryptedFileTagdataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputForm inputForm = new InputForm("Input file name", "Plase input file name", "");

            if (inputForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fileName = inputForm.InputText;

                byte[] tagData = new Byte[FilterAPI.MAX_AES_TAG_SIZE];
                uint tagDataLength = (uint)tagData.Length;
                bool retVal = FilterAPI.GetAESTagData(fileName, ref tagDataLength, tagData);

                if (!retVal)
                {
                    MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                    MessageBox.Show("GetAESTagData failed with error:" + FilterAPI.GetLastErrorMessage(), "GetAESTagData", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("Get encrypted file " + fileName + " tag data succeeded. return tag data length:" + tagDataLength, "tagData", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            messageHandler.LoadMessageFromLogToConsole();
        }

        private void toolStripButton_UnitTest_Click(object sender, EventArgs e)
        {
            toolStripButton_Stop_Click(null, null);
            FileProtectorUnitTest fileProtectorUnitTest = new FileProtectorUnitTest();
            FileProtectorUnitTest.licenseKey = GlobalConfig.LicenseKey;

            fileProtectorUnitTest.ShowDialog();
        }

     
        private void toolStripButton_Help_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://blog.easefilter.com/file-protector-demo-step-by-step/");
        }

        private void toolStripButton_ApplyTrialKey_Click(object sender, EventArgs e)
        {
            return;
        }
       
    }
}
