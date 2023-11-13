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
using System.Reflection;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using EaseFilter.FilterControl;
using EaseFilter.CommonObjects;

namespace SecureSandbox
{
    public partial class SecureSandbox : Form
    {
        /// <summary>
        /// Filter rule is the access control setting for the specific files. 
        /// 
        /// for example: set file filter mask c:\test\*, you can set the access rights to the files in c:\test.
        /// you also can add the specific process or user access rights to the files in c:\test
        /// </summary>
        FileFilterRule selectedFilterRule = null;

        /// <summary>
        /// Process filter rule is the access control of the process, set the file access rights to this process.
        /// 
        /// for example: set the process filter mask: c:\test\*.exe, you can control the processes which were launched from c:\test, set the file access rights
        /// to this process, not allow the process to change the files in c:\Windows or other sensitive files.
        /// </summary>
        ProcessFilterRule selectedProcessFilterRule = null;

        /// <summary>
        /// Registry filter rule is the access control of the registry, allow the process to access or change the registry.
        /// </summary>
        RegistryFilterRule selectedRegistryFilterRule = null;
       

        EncryptEventHandler encryptEventHandler = new EncryptEventHandler();
        FilterControl filterControl = new FilterControl();    

        public SecureSandbox()
        {
            GlobalConfig.filterType = FilterAPI.FilterType.CONTROL_FILTER | FilterAPI.FilterType.ENCRYPTION_FILTER
                | FilterAPI.FilterType.PROCESS_FILTER | FilterAPI.FilterType.REGISTRY_FILTER;

            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        
            InitListView();

            DisplayVersion();
        }

        ~SecureSandbox()
        {
            GlobalConfig.Stop();
        }

        void SendSettingsToFilter()
        {
            try
            {
                filterControl.ClearFilters();
                foreach (FileFilterRule filterRule in GlobalConfig.FilterRules.Values)
                {
                    FileFilter fileFilter = filterRule.ToFileFilter();
                    fileFilter.OnFilterRequestEncryptKey += encryptEventHandler.OnFilterRequestEncryptKey;
                    filterControl.AddFilter(fileFilter);
                }

                foreach (ProcessFilterRule filterRule in GlobalConfig.ProcessFilterRules.Values)
                {
                    ProcessFilter processFilter = filterRule.ToProcessFilter();
                    filterControl.AddFilter(processFilter);
                }

                foreach (RegistryFilterRule filterRule in GlobalConfig.RegistryFilterRules.Values)
                {
                    RegistryFilter registryFilter = filterRule.ToRegistryFilter();
                    filterControl.AddFilter(registryFilter);
                }

                string lastError = string.Empty;
                if (!filterControl.SendConfigSettingsToFilter(ref lastError))
                {
                    MessageBox.Show(lastError, "SendConfigSettingsToFilter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("SendConfigSettingsToFilter exception:" + ex.Message, "SendConfigSettingsToFilter", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void SecureSandbox_FormClosed(object sender, FormClosedEventArgs e)
        {
            MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
            if (MessageBox.Show("Do you want to minimize to system tray?", "Close", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {

            }
            else
            {
                FilterAPI.ResetConfigData();
                GlobalConfig.Stop();
                filterControl.StopFilter();
                Application.Exit();
            }
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

        void SetSelectedFilterRule(FileFilterRule filterRule)
        {
            selectedFilterRule = filterRule;
            selectedProcessFilterRule = GlobalConfig.GetProcessFilterRule("", selectedFilterRule.IncludeFileFilterMask);

            if (null == selectedProcessFilterRule)
            {
                selectedProcessFilterRule = new ProcessFilterRule();
            }

            selectedRegistryFilterRule = GlobalConfig.GetRegistryFilterRule("", selectedFilterRule.IncludeFileFilterMask);
            if (null == selectedRegistryFilterRule)
            {
                selectedRegistryFilterRule = new  RegistryFilterRule();
            }

        }

        public void InitListView()
        {
            try
            {
                //init ListView control
                listView_Sandbox.Clear();		//clear control
                //create column header for ListView
                listView_Sandbox.Columns.Add("#", 20, System.Windows.Forms.HorizontalAlignment.Left);
                listView_Sandbox.Columns.Add("Sandbox Folder", 330, System.Windows.Forms.HorizontalAlignment.Left);
                listView_Sandbox.Columns.Add("Access Flag", 100, System.Windows.Forms.HorizontalAlignment.Left);

                foreach (FileFilterRule rule in GlobalConfig.FilterRules.Values)
                {
                    string[] itemStr = new string[listView_Sandbox.Columns.Count];
                    itemStr[0] = listView_Sandbox.Items.Count.ToString();
                    itemStr[1] = rule.IncludeFileFilterMask;
                    itemStr[2] = rule.AccessFlag.ToString();

                    ListViewItem item = new ListViewItem(itemStr, 0);
                    item.Tag = rule;
                    listView_Sandbox.Items.Add(item);

                    SetSelectedFilterRule(rule);
                }

                InitSandbox();
            }
            catch (Exception ex)
            {
                EventManager.WriteMessage(127, "InitListView", EventLevel.Error, "InitListView failed with error:" + ex.Message);
            }
        }

        void InitNewFilterRule()
        {
            selectedFilterRule = new FileFilterRule();
            selectedProcessFilterRule = new ProcessFilterRule();
            selectedRegistryFilterRule = new RegistryFilterRule();

            textBox_SandboxFolder.Text = "c:\\newSandboxFolder";

            selectedFilterRule.IncludeFileFilterMask = textBox_SandboxFolder.Text.Trim() + "\\*";
            selectedProcessFilterRule.ProcessNameFilterMask = textBox_SandboxFolder.Text.Trim() + "\\*";
            selectedRegistryFilterRule.ProcessNameFilterMask = textBox_SandboxFolder.Text.Trim() + "\\*";

            //by default allow the binaries inside the sandbox to read/write the registry                
            selectedRegistryFilterRule.AccessFlag = FilterAPI.MAX_REGITRY_ACCESS_FLAG;

            selectedProcessFilterRule.ProcessId = selectedRegistryFilterRule.ProcessId = "";          
            //by default not allow the executable 
            selectedProcessFilterRule.ControlFlag = (uint)FilterAPI.ProcessControlFlag.DENY_NEW_PROCESS_CREATION;
            //set the maximum access rights to the sandbox for all binaries inside the sandbox 
            selectedProcessFilterRule.FileAccessRights = textBox_SandboxFolder.Text + "!" + FilterAPI.ALLOW_MAX_RIGHT_ACCESS.ToString() + ";";
            //allow the windows dll or exe to be read by the process, or it can't be loaded.
            selectedProcessFilterRule.FileAccessRights += "c:\\windows\\*!" + FilterAPI.ALLOW_FILE_READ_ACCESS + ";";
            //No access rights to all other folders by default.
            selectedProcessFilterRule.FileAccessRights += "*!" + ((uint)FilterAPI.AccessFlag.LEAST_ACCESS_FLAG).ToString() + ";";
            
            //by default the sandbox folder doesn't allow being read/write by processes, if the processes want to access the sandbox, it needs to add process rights.
            selectedFilterRule.AccessFlag = (uint)(FilterAPI.ALLOW_MAX_RIGHT_ACCESS|(uint)FilterAPI.AccessFlag.ENABLE_FILE_ENCRYPTION_RULE);
            //Not allow the explorer.exe to read the encrytped files, when you copy the encrypted files from exploer, the file can stay encrypted.
            selectedFilterRule.ProcessNameRights = "explorer.exe!" + ((uint)FilterAPI.ALLOW_MAX_RIGHT_ACCESS &~(uint)(FilterAPI.AccessFlag.ALLOW_READ_ENCRYPTED_FILES)).ToString() + ";";
        }

        private void InitSandbox()
        {
            try
            {
                if (selectedFilterRule == null)
                {
                    InitNewFilterRule();
                }
                else
                {
                    textBox_SandboxFolder.Text = selectedFilterRule.IncludeFileFilterMask.Replace("\\*","");
                }

                uint fileAccessFlag = selectedFilterRule.AccessFlag;
                uint processControlFlag = selectedProcessFilterRule.ControlFlag;
                uint processRegistryControlFlag = selectedRegistryFilterRule.AccessFlag;               

                if ((processRegistryControlFlag & (uint)FilterAPI.ALLOW_READ_REGITRY_ACCESS_FLAG) != 0)
                {
                    checkBox_AllowReadRegistry.Checked = true;
                }
                else
                {
                    checkBox_AllowReadRegistry.Checked = false;
                }

                if ((processRegistryControlFlag & (uint)FilterAPI.ALLOW_CHANGE_REGITRY_ACCESS_FLAG) != 0)
                {
                    checkBox_AllowChangeRegistry.Checked = true;
                }
                else
                {
                    checkBox_AllowChangeRegistry.Checked = false;
                }

                if ((processControlFlag & (uint)FilterAPI.ProcessControlFlag.DENY_NEW_PROCESS_CREATION) != 0)
                {
                    checkBox_AllowExecute.Checked = false;
                }
                else
                {
                    checkBox_AllowExecute.Checked = true;
                }

                if ((fileAccessFlag & (uint)FilterAPI.ALLOW_FILE_READ_ACCESS) != 0)
                {
                    checkBox_AllowSandboxRead.Checked = true;
                }
                else
                {
                    checkBox_AllowSandboxRead.Checked = false;
                }

                if ((fileAccessFlag & (uint)FilterAPI.ALLOW_FILE_CHANGE_ACCESS) != 0)
                {
                    checkBox_AllowSandboxChange.Checked = true;
                }
                else
                {
                    checkBox_AllowSandboxChange.Checked = false;
                }

                if ((fileAccessFlag & (uint)FilterAPI.AccessFlag.ALLOW_DIRECTORY_LIST_ACCESS) != 0)
                {
                    checkBox_AllowFolderBrowsing.Checked = true;
                }
                else
                {
                    checkBox_AllowFolderBrowsing.Checked = false;
                }


                if ((fileAccessFlag & (uint)FilterAPI.AccessFlag.ALLOW_READ_ENCRYPTED_FILES) != 0)
                {
                    checkBox_AllowReadEncryptedFiles.Checked = true;
                }
                else
                {
                    checkBox_AllowReadEncryptedFiles.Checked = false;
                }

                if ((fileAccessFlag & (uint)FilterAPI.AccessFlag.ENABLE_FILE_ENCRYPTION_RULE) != 0)
                {
                    checkBox_EnableEncryption.Checked = true;
                }
                else
                {
                    checkBox_EnableEncryption.Checked = false;
                }

                SendSettingsToFilter();

            }
            catch (Exception ex)
            {
                EventManager.WriteMessage(254, "InitSandbox", EventLevel.Error, "InitSandbox failed with error:" + ex.Message);
            }

        }

        private bool GetSandboxSetting()
        {
            try
            {
                uint fileAccessFlag = selectedFilterRule.AccessFlag;
                uint processRegistryControlFlag = FilterAPI.MAX_REGITRY_ACCESS_FLAG;

                if (textBox_SandboxFolder.Text.Trim().Length == 0)
                {
                    MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                    MessageBox.Show("The sandbox folder can't be empty.", "Add Filter Rule", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                //copy the current filter instance.
                selectedFilterRule = selectedFilterRule.Copy();

                selectedFilterRule.IncludeFileFilterMask = textBox_SandboxFolder.Text.Trim() + "\\*";
                selectedProcessFilterRule.ProcessNameFilterMask = textBox_SandboxFolder.Text.Trim() + "\\*";
                selectedRegistryFilterRule.ProcessNameFilterMask = textBox_SandboxFolder.Text.Trim() + "\\*";
            
                if (checkBox_AllowReadRegistry.Checked)
                {
                    processRegistryControlFlag |= (uint)FilterAPI.ALLOW_READ_REGITRY_ACCESS_FLAG;
                }
                else
                {
                    processRegistryControlFlag &= ~(uint)FilterAPI.ALLOW_READ_REGITRY_ACCESS_FLAG;
                }

                if (checkBox_AllowChangeRegistry.Checked)
                {
                    processRegistryControlFlag |= (uint)FilterAPI.ALLOW_CHANGE_REGITRY_ACCESS_FLAG;
                }
                else
                {
                    processRegistryControlFlag &= ~(uint)FilterAPI.ALLOW_CHANGE_REGITRY_ACCESS_FLAG;
                }

                selectedRegistryFilterRule.AccessFlag = processRegistryControlFlag;

                if (checkBox_EnableEncryption.Checked)
                {
                    fileAccessFlag |= (uint)FilterAPI.AccessFlag.ENABLE_FILE_ENCRYPTION_RULE;

                    selectedFilterRule.EncryptMethod = (int)FilterAPI.EncryptionMethod.ENCRYPT_FILE_WITH_SAME_KEY_AND_UNIQUE_IV;
                    //use the default encryption password to generate encrytpion key, you can change with your own password, 
                    selectedFilterRule.EncryptionPassPhrase = GlobalConfig.MasterPassword;

                }

                selectedFilterRule.AccessFlag = fileAccessFlag;

                return true;
            }
            catch (Exception ex)
            {
                EventManager.WriteMessage(326, "GetSandboxSetting", EventLevel.Error, "GetSandboxSetting failed with error:" + ex.Message);
                return false;
            }

        }

        private void button_AccessFlag_Click(object sender, EventArgs e)
        {
            if (selectedProcessFilterRule.ProcessNameFilterMask != textBox_SandboxFolder.Text)
            {
                InitNewFilterRule();
            }

            OptionForm optionForm = new OptionForm(OptionForm.OptionType.Access_Flag, selectedFilterRule.AccessFlag.ToString());

            if (optionForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (optionForm.AccessFlags > 0)
                {
                    selectedFilterRule.AccessFlag = optionForm.AccessFlags;
                }
            }

            InitSandbox();
        }
     

        private void button_SelectFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox_SandboxFolder.Text = folderBrowserDialog.SelectedPath ;
            }
        }

        private void button_ProcessFileAccessRights_Click(object sender, EventArgs e)
        {
            if (textBox_SandboxFolder.Text.Trim().Length == 0)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("Sandbox folder can't be empty.", "config file access", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!checkBox_AllowExecute.Checked)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("Allow the binaries being executed checkbox is not check, there are no access rights to the binaries.", "config file access", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (selectedProcessFilterRule.ProcessNameFilterMask != textBox_SandboxFolder.Text)
            {
                InitNewFilterRule();
            }

            selectedProcessFilterRule.ProcessNameFilterMask = textBox_SandboxFolder.Text;          

            ProcessFileAccessRights processFileAccessRights = new ProcessFileAccessRights(selectedProcessFilterRule);
            processFileAccessRights.ShowDialog();
        }

        private void button_ConfigProcessRights_Click(object sender, EventArgs e)
        {
            if (textBox_SandboxFolder.Text.Trim().Length == 0)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("Sandbox folder can't be empty.", "config sendbox", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (selectedFilterRule.IncludeFileFilterMask != textBox_SandboxFolder.Text)
            {
                GetSandboxSetting();    
            }
            
            Form_ProcessRights processRights = new Form_ProcessRights(ref selectedFilterRule);
            if (processRights.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
               selectedFilterRule = processRights.currentFilterRule;
               GlobalConfig.AddFileFilterRule(selectedFilterRule);
               InitListView();
            }
            
        }

        private void listView_Sandbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView_Sandbox.SelectedItems.Count == 0)
            {
                return;
            }

            SetSelectedFilterRule(((FileFilterRule)listView_Sandbox.SelectedItems[0].Tag).Copy());
          
            InitSandbox();
        }


        private void button_AddSandbox_Click(object sender, EventArgs e)
        {
            InitNewFilterRule();
            InitSandbox();           
        }

    
        private void button_DeleteSandbox_Click(object sender, EventArgs e)
        {
            if (listView_Sandbox.SelectedItems.Count == 0)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("There are no sandbox selected.", "Delete sendbox", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (System.Windows.Forms.ListViewItem item in listView_Sandbox.SelectedItems)
            {
                FileFilterRule filterRule = (FileFilterRule)item.Tag;

                GlobalConfig.RemoveFilterRule(filterRule.IncludeFileFilterMask);

                ProcessFilterRule processFilterRule = GlobalConfig.GetProcessFilterRule("", filterRule.IncludeFileFilterMask);
                if (null != processFilterRule)
                {
                    GlobalConfig.RemoveProcessFilterRule(processFilterRule);
                }

                RegistryFilterRule registryFilterRule = GlobalConfig.GetRegistryFilterRule("", filterRule.IncludeFileFilterMask);
                if (null != registryFilterRule)
                {
                    GlobalConfig.RemoveRegistryFilterRule(registryFilterRule);
                }
                
                GlobalConfig.SaveConfigSetting();

                SendSettingsToFilter();
            }

            if (GlobalConfig.FilterRules.Count > 0)
            {
                SetSelectedFilterRule(GlobalConfig.FilterRules.Values.ElementAt(0).Copy());
            }
            else
            {
                selectedFilterRule = null;
            }          

            InitListView();
        }

     

        private void checkBox_AllowSandboxRead_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_AllowSandboxRead.Checked)
            {
                selectedFilterRule.AccessFlag |= FilterAPI.ALLOW_FILE_READ_ACCESS;
            }
            else
            {
                selectedFilterRule.AccessFlag &= ~FilterAPI.ALLOW_FILE_READ_ACCESS;
            }
        }

        private void checkBox_AllowSandboxChange_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_AllowSandboxChange.Checked)
            {
                selectedFilterRule.AccessFlag |= FilterAPI.ALLOW_FILE_CHANGE_ACCESS;
            }
            else
            {
                selectedFilterRule.AccessFlag &= ~FilterAPI.ALLOW_FILE_CHANGE_ACCESS;
            }
        }

        private void checkBox_AllowFolderBrowsing_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_AllowFolderBrowsing.Checked)
            {
                selectedFilterRule.AccessFlag |= (uint)FilterAPI.AccessFlag.ALLOW_DIRECTORY_LIST_ACCESS;
            }
            else
            {
                selectedFilterRule.AccessFlag &= ~(uint)FilterAPI.AccessFlag.ALLOW_DIRECTORY_LIST_ACCESS;
            }
        }


        private void checkBox_AllowReadEncryptedFiles_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_AllowReadEncryptedFiles.Checked)
            {
                selectedFilterRule.AccessFlag |= (uint)FilterAPI.AccessFlag.ALLOW_READ_ENCRYPTED_FILES;
            }
            else
            {
                selectedFilterRule.AccessFlag &= ~(uint)FilterAPI.AccessFlag.ALLOW_READ_ENCRYPTED_FILES;
            }
        }

        private void checkBox_EnableEncryption_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_EnableEncryption.Checked)
            {
                selectedFilterRule.AccessFlag |= (uint)FilterAPI.AccessFlag.ENABLE_FILE_ENCRYPTION_RULE;
            }
            else
            {
                selectedFilterRule.AccessFlag &= ~(uint)FilterAPI.AccessFlag.ENABLE_FILE_ENCRYPTION_RULE;
            }
        }

        private void checkBox_AllowExecute_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_AllowExecute.Checked)
            {
                selectedProcessFilterRule.ControlFlag &= ~(uint)FilterAPI.ProcessControlFlag.DENY_NEW_PROCESS_CREATION;
            }
            else
            {
                selectedProcessFilterRule.ControlFlag |= (uint)FilterAPI.ProcessControlFlag.DENY_NEW_PROCESS_CREATION;
            }
        }

     
        private void button_SaveSandBox_Click(object sender, EventArgs e)
        {
            if (GetSandboxSetting())
            {
                GlobalConfig.AddFileFilterRule(selectedFilterRule);
                GlobalConfig.AddProcessFilterRule(selectedProcessFilterRule);
                GlobalConfig.AddRegistryFilterRule(selectedRegistryFilterRule);
                GlobalConfig.SaveConfigSetting();

                SendSettingsToFilter();
            }

            InitListView();
        }


        private void toolStripButton_EventViewer_Click(object sender, EventArgs e)
        {
            EventForm.DisplayEventForm();
        }

      
        private void button_help_Click(object sender, EventArgs e)
        {
            string information = "The sandbox can be used in below two purposes:\r\n\r\n"; 
            information += "1. Restrict the binary access rights inside the sandbox, prevent the registries";
            information += "from being accessed or changed, prevent your important files from being accessed, changed or deleted by the untrusted processes inside the sandbox.\r\n\r\n";
            information += "i.e. you can create a sandbox for your document setting folder,prevent the untrusted downloaded binaries from being launched there,";
            information += "or you can create a sandbox for your test software, only can read/write the files in the sandbox or other specific folder.\r\n\r\n";
            information += "2. Protect sensitive files inside the sandbox, encrypt the sensitive files, add the white list processes who can access the encrypted files, black list processes who can't access the encrypted files.\r\n";
            information += "set the specific access rights to the specific process, allow the process to read, write, rename, delete or change security of the files inside the sandbox.\r\n\r\n";
            information += "i.e. you create a sandbox for your confidential files,";
            information += "only allow authorized user/process to read or change the files there.\r\n\r\n";

            MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
            MessageBox.Show(information, "How to use sandbox?", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void toolStripButton_ApplyTrialKey_Click(object sender, EventArgs e)
        {
            
        }
            
    }
}
