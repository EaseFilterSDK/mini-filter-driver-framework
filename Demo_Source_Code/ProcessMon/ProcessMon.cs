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

using EaseFilter.FilterControl;
using EaseFilter.CommonObjects;

namespace ProcessMon
{
    public partial class ProcessMon : Form
    {        

        ProcessHandler processHandler = null;
        FilterControl filterControl = new FilterControl();        

        public ProcessMon()
        {
            GlobalConfig.filterType = FilterAPI.FilterType.PROCESS_FILTER|FilterAPI.FilterType.CONTROL_FILTER|FilterAPI.FilterType.MONITOR_FILTER;
            InitializeComponent();

            StartPosition = FormStartPosition.CenterScreen;
            DisplayVersion();

            processHandler = new ProcessHandler(listView_Info);

        }

        ~ProcessMon()
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

            if (GlobalConfig.ProcessFilterRules.Count == 0)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("You don't have any process filter setup, please go to the settings to add a new filter rule, or the filter driver won't intercept any process or IO.");
            }

            foreach (ProcessFilterRule filterRule in GlobalConfig.ProcessFilterRules.Values)
            {
                ProcessFilter processFilter = filterRule.ToProcessFilter();
                
                processFilter.OnProcessCreation += processHandler.OnProcessCreation;
                processFilter.OnProcessPreTermination += processHandler.OnProcessPreTermination;
                processFilter.NotifyProcessWasBlocked += processHandler.NotifyProcessWasBlocked;
                processFilter.NotifyProcessTerminated += processHandler.NotifyProcessTerminated;
                processFilter.NotifyThreadCreation += processHandler.NotifyThreadCreation;
                processFilter.NotifyThreadTerminated += processHandler.NotifyThreadTerminated;
                processFilter.NotifyProcessHandleInfo += processHandler.NotifyProcessHandleInfo;
                processFilter.NotifyThreadHandleInfo += processHandler.NotifyThreadHandleInfo;

                filterControl.AddFilter(processFilter);
            }


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

        private void ProcessMon_FormClosing(object sender, FormClosingEventArgs e)
        {
            FilterAPI.ResetConfigData();
            GlobalConfig.Stop();
            filterControl.StopFilter();
            Application.Exit();
        }

        private void toolStripButton_Stop_Click(object sender, EventArgs e)
        {
            FilterAPI.ResetConfigData();
            filterControl.StopFilter();

            toolStripButton_StartFilter.Enabled = true;
            toolStripButton_Stop.Enabled = false;
        }

        private void toolStripButton_ClearMessage_Click(object sender, EventArgs e)
        {
            processHandler.InitListView();
        }

        private void exitToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            FilterAPI.ResetConfigData();
            GlobalConfig.Stop();
            filterControl.StopFilter();
            Close();
        }

        private void uninstallDriverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GlobalConfig.Stop();
            filterControl.StopFilter();
            FilterAPI.UnInstallDriver();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
            ProcessFilterSettingCollection settingForm = new ProcessFilterSettingCollection();
            if (settingForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SendSettingsToFilter();
            }
        }

        private void toolStripButton_UnitTest_Click(object sender, EventArgs e)
        {
            toolStripButton_Stop_Click(null, null);
            ProcessUnitTestForm regUnitTest = new ProcessUnitTestForm(GlobalConfig.LicenseKey);
            regUnitTest.ShowDialog();
        }
    }
}
