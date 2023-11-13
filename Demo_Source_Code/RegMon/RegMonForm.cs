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

using EaseFilter.FilterControl;
using EaseFilter.CommonObjects;

namespace RegMon
{
    public partial class RegMonForm : Form
    {        
        RegistryHandler registryHandler = null;
        FilterControl filterControl = new FilterControl();        

        public RegMonForm()
        {
            GlobalConfig.filterType = FilterAPI.FilterType.REGISTRY_FILTER;

            InitializeComponent();

            StartPosition = FormStartPosition.CenterScreen;
            DisplayVersion();

            registryHandler = new RegistryHandler(listView_Info);

        }

        ~RegMonForm()
        {
            GlobalConfig.Stop();
            filterControl.StopFilter();
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

        private void RegMonForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            FilterAPI.ResetConfigData();
            GlobalConfig.Stop();
            filterControl.StopFilter();
            Application.Exit();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
            RegistryAccessControlForm regitryAccessControlForm = new RegistryAccessControlForm();
            if (regitryAccessControlForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SendSettingsToFilter();
            }
        }

        void SendSettingsToFilter()
        {
            filterControl.ClearFilters();

            GlobalConfig.Load();

            if (GlobalConfig.RegistryFilterRules.Count == 0)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("You don't have any registry filter setup, please go to the settings to add a new filter rule, or the filter driver won't intercept any process or IO.");
            }

            foreach (RegistryFilterRule filterRule in GlobalConfig.RegistryFilterRules.Values)
            {
                RegistryFilter registryFilter = filterRule.ToRegistryFilter();

                registryFilter.NotifyRegWasBlocked += registryHandler.NotifyRegWasBlocked;

                registryFilter.OnPreDeleteKey += registryHandler.OnPreDeleteKey;
                registryFilter.OnPreSetValueKey += registryHandler.OnPreSetValueKey;
                registryFilter.OnPreDeleteValueKey += registryHandler.OnPreDeleteValueKey;
                registryFilter.OnPreSetInformationKey += registryHandler.OnPreSetInformationKey;
                registryFilter.OnPreRenameKey += registryHandler.OnPreRenameKey;
                registryFilter.OnPreEnumerateKey += registryHandler.OnPreEnumerateKey;
                registryFilter.OnPreEnumerateValueKey += registryHandler.OnPreEnumerateValueKey;
                registryFilter.OnPreQueryKey += registryHandler.OnPreQueryKey;
                registryFilter.OnPreQueryValueKey += registryHandler.OnPreQueryValueKey;
                registryFilter.OnPreQueryMultipleValueKey += registryHandler.OnPreQueryMultipleValueKey;
                registryFilter.OnPreCreateKey += registryHandler.OnPreCreateKey;
                registryFilter.OnPreOpenKey += registryHandler.OnPreOpenKey;
                registryFilter.OnPreKeyHandleClose += registryHandler.OnPreKeyHandleClose;
                registryFilter.OnPreCreateKeyEx += registryHandler.OnPreCreateKeyEx;
                registryFilter.OnPreOpenKeyEx += registryHandler.OnPreOpenKeyEx;
                registryFilter.OnPreFlushKey += registryHandler.OnPreFlushKey;
                registryFilter.OnPreLoadKey += registryHandler.OnPreLoadKey;
                registryFilter.OnPreUnLoadKey += registryHandler.OnPreUnLoadKey;
                registryFilter.OnPreQueryKeySecurity += registryHandler.OnPreQueryKeySecurity;
                registryFilter.OnPreSetKeySecurity += registryHandler.OnPreSetKeySecurity;
                registryFilter.OnPreRestoreKey += registryHandler.OnPreRestoreKey;
                registryFilter.OnPreSaveKey += registryHandler.OnPreSaveKey;
                registryFilter.OnPreReplaceKey += registryHandler.OnPreReplaceKey;
                registryFilter.OnPreQueryKeyName += registryHandler.OnPreQueryKeyName;

                registryFilter.NotifyDeleteKey += registryHandler.NotifyDeleteKey;
                registryFilter.NotifySetValueKey += registryHandler.NotifySetValueKey;
                registryFilter.NotifyDeleteValueKey += registryHandler.NotifyDeleteValueKey;
                registryFilter.NotifySetInformationKey += registryHandler.NotifySetInformationKey;
                registryFilter.NotifyRenameKey += registryHandler.NotifyRenameKey;
                registryFilter.NotifyEnumerateKey += registryHandler.NotifyEnumerateKey;
                registryFilter.NotifyEnumerateValueKey += registryHandler.NotifyEnumerateValueKey;
                registryFilter.NotifyQueryKey += registryHandler.NotifyQueryKey;
                registryFilter.NotifyQueryValueKey += registryHandler.NotifyQueryValueKey;
                registryFilter.NotifyQueryMultipleValueKey += registryHandler.NotifyQueryMultipleValueKey;
                registryFilter.NotifyCreateKey += registryHandler.NotifyCreateKey;
                registryFilter.NotifyOpenKey += registryHandler.NotifyOpenKey;
                registryFilter.NotifyKeyHandleClose += registryHandler.NotifyKeyHandleClose;
                registryFilter.NotifyCreateKeyEx += registryHandler.NotifyCreateKeyEx;
                registryFilter.NotifyOpenKeyEx += registryHandler.NotifyOpenKeyEx;
                registryFilter.NotifyFlushKey += registryHandler.NotifyFlushKey;
                registryFilter.NotifyLoadKey += registryHandler.NotifyLoadKey;
                registryFilter.NotifyUnLoadKey += registryHandler.NotifyUnLoadKey;
                registryFilter.NotifyQueryKeySecurity += registryHandler.NotifyQueryKeySecurity;
                registryFilter.NotifySetKeySecurity += registryHandler.NotifySetKeySecurity;
                registryFilter.NotifyRestoreKey += registryHandler.NotifyRestoreKey;
                registryFilter.NotifySaveKey += registryHandler.NotifySaveKey;
                registryFilter.NotifyReplaceKey += registryHandler.NotifyReplaceKey;
                registryFilter.NotifyQueryKeyName += registryHandler.NotifyQueryKeyName;

                filterControl.AddFilter(registryFilter);
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
            FilterAPI.ResetConfigData();
            filterControl.StopFilter();

            toolStripButton_StartFilter.Enabled = true;
            toolStripButton_Stop.Enabled = false;
        }

        private void toolStripButton_ClearMessage_Click(object sender, EventArgs e)
        {
            registryHandler.InitListView();
        }

        private void toolStripButton_UnitTest_Click(object sender, EventArgs e)
        {
            toolStripButton_Stop_Click(null, null);
            RegUnitTest regUnitTest = new RegUnitTest(GlobalConfig.LicenseKey );
            regUnitTest.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
      

        private void toolStripButton_ApplyTrialKey_Click(object sender, EventArgs e)
        {
           
        }

    
    }
}
