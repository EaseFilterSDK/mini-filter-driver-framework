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
        /// File filter is the access control setting for the specific files. 
        /// 
        /// for example: set file filter mask c:\test\*, you can set the access rights to the files in c:\test.
        /// you also can add the specific process or user access rights to the files in c:\test
        /// </summary>
        FileFilter selectedFileFilter = null;

        /// <summary>
        /// Process filter is the access control of the process, set the file access rights to this process.
        /// 
        /// for example: set the process filter mask: c:\test\*.exe, you can control the processes which were launched from c:\test, set the file access rights
        /// to this process, not allow the process to change the files in c:\Windows or other sensitive files.
        /// </summary>
        ProcessFilter selectedProcessFilter = null;

        /// <summary>
        /// Registry filter is the access control of the registry, allow the process to access or change the registry.
        /// </summary>
        RegistryFilter selectedRegistryFilter = null;
       

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
                foreach (FileFilter fileFilter in GlobalConfig.FileFilters.Values)
                {
                    fileFilter.OnFilterRequestEncryptKey += encryptEventHandler.OnFilterRequestEncryptKey;
                    filterControl.AddFilter(fileFilter);
                }

                foreach (ProcessFilter processFilter in GlobalConfig.ProcessFilters.Values)
                {
                    filterControl.AddFilter(processFilter);
                }

                foreach (RegistryFilter registryFilter in GlobalConfig.RegistryFilters.Values)
                {
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

        void SetSelectedFilterRule(FileFilter fileFilter)
        {
            selectedFileFilter = fileFilter;
            selectedProcessFilter = GlobalConfig.GetProcessFilter("", selectedFileFilter.IncludeFileFilterMask);

            if (null == selectedProcessFilter)
            {
                selectedProcessFilter = new ProcessFilter("");
            }

            selectedRegistryFilter = GlobalConfig.GetRegistryFilter("", selectedFileFilter.IncludeFileFilterMask);
            if (null == selectedRegistryFilter)
            {
                selectedRegistryFilter = new  RegistryFilter();
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

                foreach (FileFilter fileFilter in GlobalConfig.FileFilters.Values)
                {
                    string[] itemStr = new string[listView_Sandbox.Columns.Count];
                    itemStr[0] = listView_Sandbox.Items.Count.ToString();
                    itemStr[1] = fileFilter.IncludeFileFilterMask;
                    itemStr[2] = fileFilter.AccessFlags.ToString();

                    ListViewItem item = new ListViewItem(itemStr, 0);
                    item.Tag = fileFilter;
                    listView_Sandbox.Items.Add(item);

                    SetSelectedFilterRule(fileFilter);
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
            selectedFileFilter = new FileFilter("*");
            selectedProcessFilter = new ProcessFilter("*");
            selectedRegistryFilter = new RegistryFilter();

            textBox_SandboxFolder.Text = "c:\\newSandboxFolder";

            selectedFileFilter.IncludeFileFilterMask = textBox_SandboxFolder.Text.Trim() + "\\*";
            selectedProcessFilter.ProcessNameFilterMask = textBox_SandboxFolder.Text.Trim() + "\\*";
            selectedRegistryFilter.ProcessNameFilterMask = textBox_SandboxFolder.Text.Trim() + "\\*";

            //by default allow the binaries inside the sandbox to read/write the registry                
            selectedRegistryFilter.ControlFlag = FilterAPI.MAX_REGITRY_ACCESS_FLAG;

            selectedProcessFilter.ProcessId = selectedRegistryFilter.ProcessId = 0;          
            //by default not allow the executable 
            selectedProcessFilter.ControlFlag = (uint)FilterAPI.ProcessControlFlag.DENY_NEW_PROCESS_CREATION;
            //set the maximum access rights to the sandbox for all binaries inside the sandbox 
            selectedProcessFilter.FileAccessRightString = textBox_SandboxFolder.Text + "|" + FilterAPI.ALLOW_MAX_RIGHT_ACCESS.ToString() + ";";
            //allow the windows dll or exe to be read by the process, or it can't be loaded.
            selectedProcessFilter.FileAccessRightString += "c:\\windows\\*|" + FilterAPI.ALLOW_FILE_READ_ACCESS + ";";
            //No access rights to all other folders by default.
            selectedProcessFilter.FileAccessRightString += "*|" + ((uint)FilterAPI.AccessFlag.LEAST_ACCESS_FLAG).ToString() + ";";
            
            //by default the sandbox folder doesn't allow being read/write by processes, if the processes want to access the sandbox, it needs to add process rights.
            selectedFileFilter.AccessFlags = (FilterAPI.AccessFlag)(FilterAPI.ALLOW_MAX_RIGHT_ACCESS|(uint)FilterAPI.AccessFlag.ENABLE_FILE_ENCRYPTION_RULE);
            //Not allow the explorer.exe to read the encrypted files, when you copy the encrypted files from exploer, the file can stay encrypted.
            selectedFileFilter.ProcessNameAccessRightString = "explorer.exe|" + ((uint)FilterAPI.ALLOW_MAX_RIGHT_ACCESS &~(uint)(FilterAPI.AccessFlag.ALLOW_READ_ENCRYPTED_FILES)).ToString() + "||;";
        }

        private void InitSandbox()
        {
            try
            {
                if (selectedFileFilter == null)
                {
                    InitNewFilterRule();
                }
                else
                {
                    textBox_SandboxFolder.Text = selectedFileFilter.IncludeFileFilterMask.Replace("\\*","");
                }

                uint fileAccessFlag = (uint)selectedFileFilter.AccessFlags;
                uint processControlFlag = selectedProcessFilter.ControlFlag;
                uint processRegistryControlFlag = selectedRegistryFilter.ControlFlag;               

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
                FilterAPI.AccessFlag fileAccessFlag = selectedFileFilter.AccessFlags;
                uint processRegistryControlFlag = FilterAPI.MAX_REGITRY_ACCESS_FLAG;

                if (textBox_SandboxFolder.Text.Trim().Length == 0)
                {
                    MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                    MessageBox.Show("The sandbox folder can't be empty.", "Add Filter Rule", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if(selectedFileFilter.IncludeFileFilterMask != textBox_SandboxFolder.Text.Trim() + "\\*")
                {
                    selectedFileFilter = new FileFilter(textBox_SandboxFolder.Text.Trim() + "\\*");
                    selectedProcessFilter = new ProcessFilter(textBox_SandboxFolder.Text.Trim() + "\\*");
                    selectedRegistryFilter = new RegistryFilter();
                }

                selectedFileFilter.IncludeFileFilterMask = textBox_SandboxFolder.Text.Trim() + "\\*";
                selectedProcessFilter.ProcessNameFilterMask = textBox_SandboxFolder.Text.Trim() + "\\*";
                selectedRegistryFilter.ProcessNameFilterMask = textBox_SandboxFolder.Text.Trim() + "\\*";
            
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

                selectedRegistryFilter.ControlFlag = processRegistryControlFlag;

                if (checkBox_EnableEncryption.Checked)
                {
                    fileAccessFlag |= FilterAPI.AccessFlag.ENABLE_FILE_ENCRYPTION_RULE;

                    //use the default encryption password to generate encryption key, you can change with your own password, 
                    selectedFileFilter.EncryptionPassPhrase = GlobalConfig.MasterPassword;

                }

                selectedFileFilter.AccessFlags = fileAccessFlag;

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
            if (selectedProcessFilter.ProcessNameFilterMask != textBox_SandboxFolder.Text)
            {
                InitNewFilterRule();
            }

            OptionForm optionForm = new OptionForm(OptionForm.OptionType.Access_Flag, selectedFileFilter.AccessFlags.ToString());

            if (optionForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (optionForm.AccessFlags > 0)
                {
                    selectedFileFilter.AccessFlags = (FilterAPI.AccessFlag)optionForm.AccessFlags;
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

            if (selectedProcessFilter.ProcessNameFilterMask != textBox_SandboxFolder.Text)
            {
                InitNewFilterRule();
            }

            selectedProcessFilter.ProcessNameFilterMask = textBox_SandboxFolder.Text;          

            ProcessFileAccessRights processFileAccessRights = new ProcessFileAccessRights(selectedProcessFilter);
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

            if (selectedFileFilter.IncludeFileFilterMask != textBox_SandboxFolder.Text)
            {
                GetSandboxSetting();    
            }
            
            Form_ProcessRights processRights = new Form_ProcessRights(ref selectedFileFilter);
            if (processRights.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
               selectedFileFilter = processRights.currentFileFilter;
               GlobalConfig.AddFileFilter(selectedFileFilter);
               InitListView();
            }
            
        }

        private void listView_Sandbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView_Sandbox.SelectedItems.Count == 0)
            {
                return;
            }

            SetSelectedFilterRule((FileFilter)listView_Sandbox.SelectedItems[0].Tag);
          
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
                FileFilter filterRule = (FileFilter)item.Tag;

                GlobalConfig.RemoveFileFilter(filterRule.IncludeFileFilterMask);

                ProcessFilter processFilter = GlobalConfig.GetProcessFilter("", filterRule.IncludeFileFilterMask);
                if (null != processFilter)
                {
                    GlobalConfig.RemoveProcessFilter(processFilter);
                }

                RegistryFilter registryFilter = GlobalConfig.GetRegistryFilter("", filterRule.IncludeFileFilterMask);
                if (null != registryFilter)
                {
                    GlobalConfig.RemoveRegistryFilter(registryFilter);
                }
                
                GlobalConfig.SaveConfigSetting();

                SendSettingsToFilter();
            }

            if (GlobalConfig.FileFilters.Count > 0)
            {
                SetSelectedFilterRule(GlobalConfig.FileFilters.Values.ElementAt(0));
            }
            else
            {
                selectedFileFilter = null;
            }          

            InitListView();
        }

     

        private void checkBox_AllowSandboxRead_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_AllowSandboxRead.Checked)
            {
                selectedFileFilter.AccessFlags |= (FilterAPI.AccessFlag)FilterAPI.ALLOW_FILE_READ_ACCESS;
            }
            else
            {
                selectedFileFilter.AccessFlags &= (FilterAPI.AccessFlag)(~FilterAPI.ALLOW_FILE_READ_ACCESS);
            }
        }

        private void checkBox_AllowSandboxChange_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_AllowSandboxChange.Checked)
            {
                selectedFileFilter.AccessFlags |= (FilterAPI.AccessFlag)FilterAPI.ALLOW_FILE_CHANGE_ACCESS;
            }
            else
            {
                selectedFileFilter.AccessFlags &= (FilterAPI.AccessFlag)~FilterAPI.ALLOW_FILE_CHANGE_ACCESS;
            }
        }

        private void checkBox_AllowFolderBrowsing_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_AllowFolderBrowsing.Checked)
            {
                selectedFileFilter.AccessFlags |= FilterAPI.AccessFlag.ALLOW_DIRECTORY_LIST_ACCESS;
            }
            else
            {
                selectedFileFilter.AccessFlags &= ~FilterAPI.AccessFlag.ALLOW_DIRECTORY_LIST_ACCESS;
            }
        }


        private void checkBox_AllowReadEncryptedFiles_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_AllowReadEncryptedFiles.Checked)
            {
                selectedFileFilter.AccessFlags |= FilterAPI.AccessFlag.ALLOW_READ_ENCRYPTED_FILES;
            }
            else
            {
                selectedFileFilter.AccessFlags &= ~FilterAPI.AccessFlag.ALLOW_READ_ENCRYPTED_FILES;
            }
        }

        private void checkBox_EnableEncryption_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_EnableEncryption.Checked)
            {
                selectedFileFilter.AccessFlags |= FilterAPI.AccessFlag.ENABLE_FILE_ENCRYPTION_RULE;
            }
            else
            {
                selectedFileFilter.AccessFlags &= ~FilterAPI.AccessFlag.ENABLE_FILE_ENCRYPTION_RULE;
            }
        }

        private void checkBox_AllowExecute_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_AllowExecute.Checked)
            {
                selectedProcessFilter.ControlFlag &= ~(uint)FilterAPI.ProcessControlFlag.DENY_NEW_PROCESS_CREATION;
            }
            else
            {
                selectedProcessFilter.ControlFlag |= (uint)FilterAPI.ProcessControlFlag.DENY_NEW_PROCESS_CREATION;
            }
        }

     
        private void button_SaveSandBox_Click(object sender, EventArgs e)
        {
            if (GetSandboxSetting())
            {
                GlobalConfig.AddFileFilter(selectedFileFilter);
                GlobalConfig.AddProcessFilter(selectedProcessFilter);
                GlobalConfig.AddRegistryFilter(selectedRegistryFilter);
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
