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
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

using EaseFilter.CommonObjects;
using EaseFilter.FilterControl;

namespace FileMonitor
{

    public partial class MonitorForm : Form
    {
                        
        MonitorEventHandler monitorEventHandler = null;
        FilterControl filterControl = new FilterControl();


        public MonitorForm()
        {

            try
            {
                GlobalConfig.filterType = FilterAPI.FilterType.MONITOR_FILTER;

                InitializeComponent();
                monitorEventHandler = new MonitorEventHandler(listView_Info);

                StartPosition = FormStartPosition.CenterScreen;

                this.Text += GlobalConfig.GetVersionInfo();

                this.Load += new EventHandler(Form1_Load);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Start FileMonitor failed," + ex.Message, "Start failed.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }

        }

        void Form1_Load(object sender, EventArgs e)
        {
            //to improve the listview performance
            SendMessage(listView_Info.Handle, LVM_SETTEXTBKCOLOR, IntPtr.Zero, unchecked((IntPtr)(int)0xFFFFFF));
        }
      

        ~MonitorForm()
        {
            GlobalConfig.Stop();
        }

        private void MonitorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            GlobalConfig.Stop();
        }

        void SendSettingsToFilter()
        {
            filterControl.ClearFilters();

            GlobalConfig.Load();

            if (GlobalConfig.FilterRules.Count == 0)
            {
               // MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("You don't have any monitor folder setup, please set up a filter rule in the settings, or there are no IOs will be filtered.", "FilterRule", MessageBoxButtons.OK, MessageBoxIcon.Warning);
              //  MessageBox.Show("You don't have any monitor folder setup, please set up a filter rule in the settings, or there are no IOs will be filtered.");
            }

            foreach (FileFilterRule filterRule in GlobalConfig.FilterRules.Values)
            {
                FileFilter fileFilter = filterRule.ToFileFilter();

                //add the event handler for the file filter.
                fileFilter.OnFileOpen += monitorEventHandler.OnFileOpen;
                fileFilter.OnNewFileCreate += monitorEventHandler.OnFileCreate;
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

                filterControl.AddFilter(fileFilter);
            }

            filterControl.VolumeControlFlag = (FilterAPI.VolumeControlFlag)GlobalConfig.VolumeControlFlag;
            filterControl.NotifyFilterAttachToVolume -= monitorEventHandler.NotifyFilterAttachToVolume;
            filterControl.NotifyFilterAttachToVolume += monitorEventHandler.NotifyFilterAttachToVolume;
            filterControl.NotifyFilterDetachFromVolume -= monitorEventHandler.NotifyFilterDetachFromVolume;
            filterControl.NotifyFilterDetachFromVolume += monitorEventHandler.NotifyFilterDetachFromVolume;

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

                bool ret = filterControl.StartFilter(FilterAPI.FilterType.MONITOR_FILTER, GlobalConfig.FilterConnectionThreads, GlobalConfig.ConnectionTimeOut, licenseKey, ref lastError);
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
            monitorEventHandler.InitListView();
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


        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            EventForm.DisplayEventForm();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }


        private void MonitorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            FilterAPI.ResetConfigData();
            GlobalConfig.Stop();
            filterControl.StopFilter();
            Application.Exit();
        }

        private void unInstallFilterDriverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string lastError = string.Empty;
            filterControl.StopFilter();
            filterControl.UnInstallDriver(ref lastError);
        }

        private void toolStripButton_LoadMessage_Click(object sender, EventArgs e)
        {
            monitorEventHandler.LoadMessageFromLogToConsole();
        }

        private void toolStripButton_UnitTest_Click(object sender, EventArgs e)
        {
            toolStripButton_Stop_Click(null, null);
            MonitorUnitTest monitorDemo = new MonitorUnitTest();
            monitorDemo.licenseKey = GlobalConfig.LicenseKey;

            monitorDemo.ShowDialog();
        }

        private void toolStripButton_Help_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://blog.easefilter.com/file-monitor-demo-step-by-step/");
        }

        private void toolStripButton_ApplyTrialKey_Click(object sender, EventArgs e)
        {
         
        }

    }
}
