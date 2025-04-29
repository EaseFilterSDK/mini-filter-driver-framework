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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.InteropServices;

using EaseFilter.FilterControl;
using EaseFilter.CommonObjects;

namespace EaseFilter.FolderLocker
{
    public partial class Form_FolderLocker:  Form
    {
      
        FilterControl.FilterControl filterControl = new FilterControl.FilterControl();

        void SendSettingsToFilter()
        {
            try
            {
                filterControl.ClearFilters();

                foreach (FileFilter fileFilter in GlobalConfig.FileFilters.Values)
                {
                    filterControl.AddFilter(fileFilter);
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

        public Form_FolderLocker()
        {
            GlobalConfig.filterType = FilterAPI.FilterType.MONITOR_FILTER | FilterAPI.FilterType.CONTROL_FILTER | FilterAPI.FilterType.ENCRYPTION_FILTER
                | FilterAPI.FilterType.PROCESS_FILTER | FilterAPI.FilterType.REGISTRY_FILTER;

            InitializeComponent();

            StartPosition = FormStartPosition.CenterScreen;            

            InitFolderLockerListView();            
            InitAccessRightsListView();

            DisplayVersion();
          
        }

        private void DisplayVersion()
        {
            string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            try
            {
                string filterDllPath = Path.Combine(GlobalConfig.AssemblyPath, "FilterAPI.Dll");
                FileVersionInfo fileVersion = FileVersionInfo.GetVersionInfo(filterDllPath);
                version = fileVersion.ProductVersion;
            }
            catch (Exception ex)
            {
                EventManager.WriteMessage(43, "LoadFilterAPI Dll", EventLevel.Error, "FilterAPI.dll can't be found." + ex.Message);
            }

            this.Text += "(V" + version + ")";
        }

        private void InitFolderLockerListView()
        {
            //init ListView control
            listView_LockFolders.Clear();		//clear control
            //create column header for ListView
            listView_LockFolders.Columns.Add("Folder Name", 160, System.Windows.Forms.HorizontalAlignment.Left);
            listView_LockFolders.Columns.Add("Readable", 70, System.Windows.Forms.HorizontalAlignment.Left);
            listView_LockFolders.Columns.Add("Writable", 70, System.Windows.Forms.HorizontalAlignment.Left);
            listView_LockFolders.Columns.Add("Deletable", 70, System.Windows.Forms.HorizontalAlignment.Left);
            listView_LockFolders.Columns.Add("Renamable", 70, System.Windows.Forms.HorizontalAlignment.Left);
            listView_LockFolders.Columns.Add("Encrypted", 70, System.Windows.Forms.HorizontalAlignment.Left);
            listView_LockFolders.Columns.Add("Copyable", 70, System.Windows.Forms.HorizontalAlignment.Left);
            listView_LockFolders.Columns.Add("Visible", 70, System.Windows.Forms.HorizontalAlignment.Left);

            foreach (FileFilter fileFilter in GlobalConfig.FileFilters.Values)
            {
                string folderName = fileFilter.IncludeFileFilterMask.Replace("\\*","");
                uint accessFlags = (uint)fileFilter.AccessFlags;

                if (!string.IsNullOrEmpty(GlobalConfig.ShareFolder) && folderName.StartsWith(GlobalConfig.ShareFolder))
                {
                    continue;
                }

                string[] listEntry = new string[listView_LockFolders.Columns.Count];
                int index = 0;
                listEntry[index++] = folderName;
                listEntry[index++] = ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_READ_ACCESS) > 0).ToString();
                listEntry[index++] = ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_WRITE_ACCESS) > 0).ToString();
                listEntry[index++] = ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_FILE_DELETE) > 0).ToString();
                listEntry[index++] = ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_FILE_RENAME) > 0).ToString();
                listEntry[index++] = ((accessFlags & (uint)FilterAPI.AccessFlag.ENABLE_FILE_ENCRYPTION_RULE) >0 ).ToString();
                listEntry[index++] = ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_COPY_PROTECTED_FILES_OUT) > 0).ToString();
                listEntry[index++] = ((accessFlags & (uint)FilterAPI.AccessFlag.ENABLE_HIDE_FILES_IN_DIRECTORY_BROWSING) == 0).ToString();

                ListViewItem item = new ListViewItem(listEntry, 0);
                item.Tag = fileFilter;
                listView_LockFolders.Items.Add(item);
            }

            if (listView_LockFolders.Items.Count > 0)
            {
                listView_LockFolders.Items[0].Selected = true;
                listView_LockFolders.Select();
            }

        }

        private void InitAccessRightsListView()
        {
            //init ListView control
            listView_AccessRights.Clear();		//clear control
            //create column header for ListView
            listView_AccessRights.Columns.Add("Type", 50, System.Windows.Forms.HorizontalAlignment.Left);
            listView_AccessRights.Columns.Add("Name", 160, System.Windows.Forms.HorizontalAlignment.Left);
            listView_AccessRights.Columns.Add("Readable", 70, System.Windows.Forms.HorizontalAlignment.Left);
            listView_AccessRights.Columns.Add("Writable", 70, System.Windows.Forms.HorizontalAlignment.Left);
            listView_AccessRights.Columns.Add("Deletable", 70, System.Windows.Forms.HorizontalAlignment.Left);
            listView_AccessRights.Columns.Add("Renamable", 70, System.Windows.Forms.HorizontalAlignment.Left);
            listView_AccessRights.Columns.Add("CertName", 70, System.Windows.Forms.HorizontalAlignment.Left);
            listView_AccessRights.Columns.Add("Sha256", 70, System.Windows.Forms.HorizontalAlignment.Left);

            if (listView_LockFolders.SelectedItems.Count == 1)
            {
                System.Windows.Forms.ListViewItem item = listView_LockFolders.SelectedItems[0];
                FileFilter fileFilter = (FileFilter)item.Tag;

                foreach (KeyValuePair<string, ProcessRightInfo> entry in fileFilter.TrustedProcessAccessRightList)
                {
                    uint accessFlags = entry.Value.accessFlags;
                    string processName = entry.Value.processNameFilterMask;
                    string certName = entry.Value.certificateName;
                    string sha256Hash = entry.Value.imageSha256Hash;

                    string[] listEntry = new string[listView_AccessRights.Columns.Count];
                    int index = 0;
                    listEntry[index++] = "process";
                    listEntry[index++] = processName;
                    listEntry[index++] = ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_READ_ACCESS) > 0).ToString();
                    listEntry[index++] = ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_WRITE_ACCESS) > 0).ToString();
                    listEntry[index++] = ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_FILE_DELETE) > 0).ToString();
                    listEntry[index++] = ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_FILE_RENAME) > 0).ToString();
                    listEntry[index++] = certName;
                    listEntry[index++] = sha256Hash;

                    ListViewItem listItem = new ListViewItem(listEntry, 0);
                    item.Tag = fileFilter;
                    listView_AccessRights.Items.Add(listItem);

                }

                foreach (KeyValuePair<string, uint> userRight in fileFilter.UserAccessRightList)
                {
                    string userName = userRight.Key;
                    uint accessFlags = userRight.Value;

                    string[] listEntry = new string[listView_AccessRights.Columns.Count];
                    int index = 0;
                    listEntry[index++] = "user";
                    listEntry[index++] = userName;
                    listEntry[index++] = ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_READ_ACCESS) > 0).ToString();
                    listEntry[index++] = ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_WRITE_ACCESS) > 0).ToString();
                    listEntry[index++] = ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_FILE_DELETE) > 0).ToString();
                    listEntry[index++] = ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_FILE_RENAME) > 0).ToString();

                    ListViewItem listItem = new ListViewItem(listEntry, 0);
                    item.Tag = fileFilter;
                    listView_AccessRights.Items.Add(listItem);
                }

            }

        }
   
        private void linkLabel_Report_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://easefilter.com/Company.htm");
        }

        private void linkLabel_SDK_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://easefilter.com/Forums_Files/FolderLocker.htm");
        }

        private void Form_FolderLocker_FormClosed(object sender, FormClosedEventArgs e)
        {
             GlobalConfig.Stop();
             filterControl.StopFilter();
             Application.Exit();
        }

    
        private void toolStripButton_AddFolder_Click(object sender, EventArgs e)
        {
            FolderLockerSettigs folderLocker = new FolderLockerSettigs(null);

            if (folderLocker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FileFilter fileFilter = folderLocker.fileFilter;

                GlobalConfig.AddFileFilter(fileFilter);

                GlobalConfig.SaveConfigSetting();

                SendSettingsToFilter();

                InitFolderLockerListView();
            }
        }

        private void toolStripButton_RemoveFolder_Click(object sender, EventArgs e)
        {
            if (listView_LockFolders.SelectedItems.Count != 1)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("Please select one item to delete.", "Delete Folder", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {

                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);

                if (MessageBox.Show("Are you sure you want to remove a folder from locker?", "Delete Folder", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    System.Windows.Forms.ListViewItem item = listView_LockFolders.SelectedItems[0];
                    GlobalConfig.RemoveFileFilter(((FileFilter)item.Tag).IncludeFileFilterMask);

                    GlobalConfig.SaveConfigSetting();

                    SendSettingsToFilter();

                    InitFolderLockerListView();
                }
            }
        }

        private void toolStripButton_ModifyFolder_Click(object sender, EventArgs e)
        {
            if (listView_LockFolders.SelectedItems.Count != 1)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("Please select one item to delete.", "Delete Folder", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            System.Windows.Forms.ListViewItem item = listView_LockFolders.SelectedItems[0];
            FolderLockerSettigs folderLocker = new FolderLockerSettigs((FileFilter)item.Tag);

            if (folderLocker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FileFilter fileFilter = folderLocker.fileFilter;

                GlobalConfig.RemoveFileFilter(((FileFilter)item.Tag).IncludeFileFilterMask);
                GlobalConfig.AddFileFilter(fileFilter);
                GlobalConfig.SaveConfigSetting();

                SendSettingsToFilter();

                InitFolderLockerListView();
            }
            

            InitFolderLockerListView();
        }

        private void listView_LockFolders_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitAccessRightsListView();
        }

        private void toolStripButton_StartFilter_Click(object sender, EventArgs e)
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
        }

        private void toolStripButton_Stop_Click(object sender, EventArgs e)
        {
            GlobalConfig.Stop();
            filterControl.StopFilter();

            toolStripButton_StartFilter.Enabled = true;
            toolStripButton_Stop.Enabled = false;
        }

        private void toolStripButton_ApplyTrialKey_Click(object sender, EventArgs e)
        {
        }

       
    }
}
