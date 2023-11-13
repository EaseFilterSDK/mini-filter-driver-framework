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

namespace  SecureShare
{
    public partial class SecureShareManager : Form
    {
       
        EncryptEventHandler encryptEventHandler = null;
        FilterControl filterControl = new FilterControl();

        public SecureShareManager()
        {

            GlobalConfig.filterType = FilterAPI.FilterType.MONITOR_FILTER | FilterAPI.FilterType.CONTROL_FILTER | FilterAPI.FilterType.ENCRYPTION_FILTER
                | FilterAPI.FilterType.PROCESS_FILTER | FilterAPI.FilterType.REGISTRY_FILTER;

            InitializeComponent();

            StartPosition = FormStartPosition.CenterScreen;

            encryptEventHandler = new EncryptEventHandler(listView_Info);

            DisplayVersion();

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

        ~SecureShareManager()
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

            string[] whiteList = null;
            string[] blacklist = null;

            FileFilterRule filterRuleShareFolder = new FileFilterRule();
            filterRuleShareFolder.IncludeFileFilterMask = GlobalConfig.ShareFolder + "\\*";
            filterRuleShareFolder.AccessFlag |= (uint)FilterAPI.AccessFlag.ENABLE_FILE_ENCRYPTION_RULE | FilterAPI.ALLOW_MAX_RIGHT_ACCESS;
            filterRuleShareFolder.AccessFlag &= (uint)(~FilterAPI.AccessFlag.ALLOW_ENCRYPT_NEW_FILE);// this folder won't encrypt the new file.
            filterRuleShareFolder.AccessFlag &= (uint)(~FilterAPI.AccessFlag.ALLOW_READ_ENCRYPTED_FILES);// all process can't read the encyrpted file except the authorized processes.
            filterRuleShareFolder.EncryptMethod = (int)FilterAPI.EncryptionMethod.ENCRYPT_FILE_WITH_SAME_KEY_AND_UNIQUE_IV;
            filterRuleShareFolder.EncryptionPassPhrase = GlobalConfig.MasterPassword;

            //for whitelist process, it has maximum acess rights.
            if (GlobalConfig.ProtectFolderWhiteList == "*")
            {
                //allow all processes to read the encrypted file except the black list processes.
                filterRuleShareFolder.AccessFlag |= (uint)(FilterAPI.AccessFlag.ALLOW_READ_ENCRYPTED_FILES);
            }
            else
            {
                //for whitelist process, it has maximum acess rights.
                whiteList = GlobalConfig.ShareFolderWhiteList.Split(new char[] { ';' });
                if (whiteList.Length > 0)
                {
                    foreach (string authorizedUser in whiteList)
                    {
                        if (authorizedUser.Trim().Length > 0)
                        {
                            //not allow to encrypt the new file
                            uint accessFlag = filterRuleShareFolder.AccessFlag | (uint)(FilterAPI.AccessFlag.ALLOW_READ_ENCRYPTED_FILES);
                            filterRuleShareFolder.ProcessNameRights += ";" + authorizedUser + "!" + accessFlag.ToString();
                        }
                    }
                }

            }

            //for blacklist process, it has maximum acess rights.
            blacklist = GlobalConfig.ShareFolderBlackList.Split(new char[] { ';' });
            if (blacklist.Length > 0)
            {
                foreach (string unAuthorizedUser in blacklist)
                {
                    if (unAuthorizedUser.Trim().Length > 0)
                    {
                        //can't read the encrypted files, not allow to encrypt the new file
                        uint accessFlag = filterRuleShareFolder.AccessFlag;
                        filterRuleShareFolder.ProcessNameRights += ";" + unAuthorizedUser + "!" + accessFlag.ToString();
                    }
                }
            }

            FileFilter shareFolderFileFilter = filterRuleShareFolder.ToFileFilter();
            shareFolderFileFilter.OnFilterRequestEncryptKey += encryptEventHandler.OnFilterRequestEncryptKey;
            filterControl.AddFilter(shareFolderFileFilter);

            FileFilterRule filterRuleProtectFolder = new FileFilterRule();
            filterRuleProtectFolder.IncludeFileFilterMask = GlobalConfig.ProtectFolder + "\\*";
            filterRuleProtectFolder.AccessFlag |= (uint)FilterAPI.AccessFlag.ENABLE_FILE_ENCRYPTION_RULE | FilterAPI.ALLOW_MAX_RIGHT_ACCESS;
            filterRuleProtectFolder.AccessFlag &= (uint)(~FilterAPI.AccessFlag.ALLOW_READ_ENCRYPTED_FILES);// all process can't read the encyrpted file except the authorized processes.
            filterRuleProtectFolder.EncryptMethod = (int)FilterAPI.EncryptionMethod.ENCRYPT_FILE_WITH_SAME_KEY_AND_UNIQUE_IV;
            filterRuleProtectFolder.EncryptionPassPhrase = GlobalConfig.MasterPassword;

            //for whitelist process, it has maximum acess rights.
            if (GlobalConfig.ProtectFolderWhiteList == "*")
            {
                //allow all processes to read the encrypted file except the black list processes.
                filterRuleProtectFolder.AccessFlag |= (uint)(FilterAPI.AccessFlag.ALLOW_READ_ENCRYPTED_FILES);
            }
            else
            {
                //for whitelist process, it has maximum acess rights.
                whiteList = GlobalConfig.ProtectFolderWhiteList.Split(new char[] { ';' });
                if (whiteList.Length > 0)
                {
                    foreach (string authorizedUser in whiteList)
                    {
                        if (authorizedUser.Trim().Length > 0)
                        {
                            filterRuleProtectFolder.ProcessNameRights += ";" + authorizedUser + "!" + FilterAPI.ALLOW_MAX_RIGHT_ACCESS.ToString();
                        }
                    }
                }
            }

            //for blacklist process, it has maximum acess rights.
            blacklist = GlobalConfig.ProtectFolderBlackList.Split(new char[] { ';' });
            if (blacklist.Length > 0)
            {
                foreach (string unAuthorizedUser in blacklist)
                {
                    if (unAuthorizedUser.Trim().Length > 0)
                    {
                        //can't read the encrypted files
                        uint accessFlag = FilterAPI.ALLOW_MAX_RIGHT_ACCESS & (uint)(~FilterAPI.AccessFlag.ALLOW_READ_ENCRYPTED_FILES);
                        filterRuleProtectFolder.ProcessNameRights += ";" + unAuthorizedUser + "!" + accessFlag.ToString();
                    }
                }
            }

            FileFilter protectFolderFileFilter = filterRuleProtectFolder.ToFileFilter();
            protectFolderFileFilter.OnFilterRequestEncryptKey += encryptEventHandler.OnFilterRequestEncryptKey;
            filterControl.AddFilter(protectFolderFileFilter);

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
            encryptEventHandler.InitListView();
        }

       
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShareFileSettingForm settingForm = new ShareFileSettingForm();
            settingForm.StartPosition = FormStartPosition.CenterParent;
            settingForm.ShowDialog();
        }


        private void ConsoleForm_FormClosed(object sender, FormClosedEventArgs e)
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

        private void shareManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShareFileManagerInLocal shareManager = new ShareFileManagerInLocal();
            shareManager.ShowDialog();
        }

        private void toolStripButton_Help_Click(object sender, EventArgs e)
        {
            MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
            string helpInfo = "1.Setup the configuration by clicking the settings to test in local or in EaseFilter Server.\r\n";
            helpInfo += "2.Create encrypted files in the share file manager.\r\n";
            helpInfo += "3.Distribute the shared file to the clients.\r\n";
            helpInfo += "4. Copy the shared file to the share file drop folder in the client.\r\n";
            helpInfo += "5. if you want to copy the shared file to real time protected folder in the client, you have to stop the filter service first, or the file will be encrypted again.\r\n";
            helpInfo += "6. Start the filter service in the client.\r\n";            
            helpInfo += "7. Open the shared file in the client.\r\n";
            helpInfo += "8. You can check who accessed your files from the access log in the share file manager when you test with the EaseFilter Server.\r\n";

            MessageBox.Show(helpInfo, "How to use this application?", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FilterAPI.ResetConfigData();
            GlobalConfig.Stop();
            filterControl.StopFilter();
            Close();
        }

      
    
    }
}
