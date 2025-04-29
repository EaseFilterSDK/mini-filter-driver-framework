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
using System.Windows.Forms;

using EaseFilter.FilterControl;
using EaseFilter.CommonObjects;

namespace EaseFilter.FolderLocker
{   

    public partial class FolderLockerSettigs : Form
    {
        public FileFilter fileFilter = new FileFilter("");

        FilterAPI.AccessFlag accessFlags = (FilterAPI.AccessFlag)FilterAPI.ALLOW_MAX_RIGHT_ACCESS;
        Dictionary<string, uint> userRightList = new Dictionary<string, uint>();
        Dictionary<string, ProcessRightInfo> processNameAccessRightList = new Dictionary<string, ProcessRightInfo>();
        bool isFormInitialized = false;

        public FolderLockerSettigs(FileFilter _fileFilter)
        {
            InitializeComponent();

            if (null != _fileFilter)
            {
                fileFilter = _fileFilter;
                accessFlags = _fileFilter.AccessFlags;

                textBox_FolderName.Enabled = false;
                button_BrowseFolder.Enabled = false;

                textBox_FolderName.Text = fileFilter.IncludeFileFilterMask.Replace("\\*","");
                textBox_PassPhrase.Text = fileFilter.EncryptionPassPhrase;

            }
            else
            {
                textBox_FolderName.Enabled = true;
                button_BrowseFolder.Enabled = true;
            }

            this.processNameAccessRightList = fileFilter.TrustedProcessAccessRightList;
            this.userRightList = fileFilter.UserAccessRightList;

            SetCheckBoxValue();

            InitAccessRightsListView();

            isFormInitialized = true;
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

            foreach (KeyValuePair<string, uint> entry in userRightList)
            {
                string userName = entry.Key;
                uint accessFlags = entry.Value;

                string[] listEntry = new string[listView_AccessRights.Columns.Count];
                int index = 0;
                listEntry[index++] = "user";
                listEntry[index++] = userName;
                listEntry[index++] = ((accessFlags&(uint)FilterAPI.AccessFlag.ALLOW_READ_ACCESS) > 0).ToString();
                listEntry[index++] = ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_WRITE_ACCESS) > 0).ToString();
                listEntry[index++] = ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_FILE_DELETE) > 0).ToString();
                listEntry[index++] = ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_FILE_RENAME) > 0).ToString();

                ListViewItem item = new ListViewItem(listEntry, 0);
                item.Tag = entry;
                listView_AccessRights.Items.Add(item);

            }


            foreach (KeyValuePair<string, ProcessRightInfo> entry in processNameAccessRightList)
            {
                ProcessRightInfo processRight = entry.Value;
                string processName = processRight.processNameFilterMask;
                uint accessFlags = processRight.accessFlags;
                string certname = processRight.certificateName;
                string sha256 = processRight.imageSha256Hash;

                string[] listEntry = new string[listView_AccessRights.Columns.Count];
                int index = 0;
                listEntry[index++] = "process";
                listEntry[index++] = processName;
                listEntry[index++] = ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_READ_ACCESS) > 0).ToString();
                listEntry[index++] = ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_WRITE_ACCESS) > 0).ToString();
                listEntry[index++] = ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_FILE_DELETE) > 0).ToString();
                listEntry[index++] = ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_FILE_RENAME) > 0).ToString();
                listEntry[index++] = certname;
                listEntry[index++] = sha256;

                ListViewItem item = new ListViewItem(listEntry, 0);
                item.Tag = processRight;
                listView_AccessRights.Items.Add(item);

            }

        }

        private void SetCheckBoxValue()
        {

            if ((accessFlags & FilterAPI.AccessFlag.ENABLE_FILE_ENCRYPTION_RULE) > 0 )
            {
                checkBox_Encryption.Checked = true;
                textBox_PassPhrase.ReadOnly = false;
            }
            else
            {
                checkBox_Encryption.Checked = false;
                textBox_PassPhrase.ReadOnly = true;
            }

            if ((accessFlags & FilterAPI.AccessFlag.ALLOW_FILE_ACCESS_FROM_NETWORK) > 0)
            {
                checkBox_AllowRemoteAccess.Checked = true;
            }
            else
            {
                checkBox_AllowRemoteAccess.Checked = false;
            }

            if ((accessFlags & FilterAPI.AccessFlag.ALLOW_FILE_DELETE) > 0)
            {
                checkBox_AllowDelete.Checked = true;
            }
            else
            {
                checkBox_AllowDelete.Checked = false;
            }

            if ((accessFlags & FilterAPI.AccessFlag.ALLOW_SET_SECURITY_ACCESS) > 0)
            {
                checkBox_AllowSetSecurity.Checked = true;
            }
            else
            {
                checkBox_AllowSetSecurity.Checked = false;
            }

            if ((accessFlags & FilterAPI.AccessFlag.ALLOW_FILE_RENAME) > 0)
            {
                checkBox_AllowRename.Checked = true;
            }
            else
            {
                checkBox_AllowRename.Checked = false;
            }

            if ( (accessFlags & FilterAPI.AccessFlag.ALLOW_WRITE_ACCESS) > 0 )
            {
                checkBox_AllowWrite.Checked = true;
            }
            else
            {
                checkBox_AllowWrite.Checked = false;
            }

            if ((accessFlags & FilterAPI.AccessFlag.ALLOW_OPEN_WITH_CREATE_OR_OVERWRITE_ACCESS) > 0)
            {
                checkBox_AllowNewFileCreation.Checked = true;
            }
            else
            {
                checkBox_AllowNewFileCreation.Checked = false;
            }

            if ((accessFlags & FilterAPI.AccessFlag.ENABLE_HIDE_FILES_IN_DIRECTORY_BROWSING) > 0)
            {
                checkBox_HideFiles.Checked = true;
            }
            else
            {
                checkBox_HideFiles.Checked = false;
            }

            if ((accessFlags & FilterAPI.AccessFlag.ALLOW_READ_ACCESS) > 0)
            {
                checkBox_AllowRead.Checked = true;
            }
            else
            {
                checkBox_AllowRead.Checked = false;
            }


            if ((accessFlags & FilterAPI.AccessFlag.ALLOW_COPY_PROTECTED_FILES_OUT) > 0)
            {
                checkBox_AllowCopyout.Checked = true;
            }
            else
            {
                checkBox_AllowCopyout.Checked = false;
            }
        }

        private void button_SaveControlSettings_Click(object sender, EventArgs e)
        {
            fileFilter.IncludeFileFilterMask = textBox_FolderName.Text + "\\*";
            fileFilter.EncryptionPassPhrase = textBox_PassPhrase.Text;
            fileFilter.AccessFlags = accessFlags;

            if ((accessFlags & FilterAPI.AccessFlag.ENABLE_FILE_ENCRYPTION_RULE) > 0 )
            {
                if (textBox_PassPhrase.Text.Trim().Length == 0)
                {
                    MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                    MessageBox.Show("Please enter the passphrase when encryption is enabled.", "Passphrase", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }

            if ((accessFlags & FilterAPI.AccessFlag.ENABLE_HIDE_FILES_IN_DIRECTORY_BROWSING) > 0)
            {
                fileFilter.HiddenFileFilterMaskList.Add("*");
            }

            fileFilter.UserAccessRightList = userRightList;
            fileFilter.TrustedProcessAccessRightList = processNameAccessRightList;

        }
        
        private void checkBox_AllowDelete_CheckedChanged(object sender, EventArgs e)
        {

            if (!checkBox_AllowDelete.Checked)
            {
                accessFlags = accessFlags & (~FilterAPI.AccessFlag.ALLOW_FILE_DELETE);
            }
            else
            {
                accessFlags = accessFlags | (FilterAPI.AccessFlag.ALLOW_FILE_DELETE);
            }

        }

        private void checkBox_AllowWrite_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox_AllowWrite.Checked)
            {
                accessFlags = accessFlags & (~FilterAPI.AccessFlag.ALLOW_WRITE_ACCESS);
            }
            else
            {
                accessFlags = accessFlags | (FilterAPI.AccessFlag.ALLOW_WRITE_ACCESS);
            }

        }

        private void checkBox_AllowRename_CheckedChanged(object sender, EventArgs e)
        {

            if (!checkBox_AllowRename.Checked)
            {
                accessFlags = accessFlags & (~FilterAPI.AccessFlag.ALLOW_FILE_RENAME);
            }
            else
            {
                accessFlags = accessFlags | (FilterAPI.AccessFlag.ALLOW_FILE_RENAME);
            }

        }

        private void checkBox_AllowRemoteAccess_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox_AllowRemoteAccess.Checked)
            {
                accessFlags = accessFlags & (~FilterAPI.AccessFlag.ALLOW_FILE_ACCESS_FROM_NETWORK);
            }
            else
            {
                accessFlags = accessFlags | (FilterAPI.AccessFlag.ALLOW_FILE_ACCESS_FROM_NETWORK);
            }

        }

        private void checkBox_AllowNewFileCreation_CheckedChanged(object sender, EventArgs e)
        {

            if (!checkBox_AllowNewFileCreation.Checked)
            {
                accessFlags = accessFlags & (~FilterAPI.AccessFlag.ALLOW_OPEN_WITH_CREATE_OR_OVERWRITE_ACCESS);
            }
            else
            {
                accessFlags = accessFlags | (FilterAPI.AccessFlag.ALLOW_OPEN_WITH_CREATE_OR_OVERWRITE_ACCESS);
            }

        }

        private void checkBox_AllowSetSecurity_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox_AllowSetSecurity.Checked)
            {
                accessFlags = accessFlags & (~FilterAPI.AccessFlag.ALLOW_SET_SECURITY_ACCESS);
            }
            else
            {
                accessFlags = accessFlags | (FilterAPI.AccessFlag.ALLOW_SET_SECURITY_ACCESS);
            }

        }
     

        private void checkBox_HideFiles_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox_HideFiles.Checked)
            {
                accessFlags = accessFlags & (~FilterAPI.AccessFlag.ENABLE_HIDE_FILES_IN_DIRECTORY_BROWSING);
            }
            else
            {
                accessFlags = accessFlags | (FilterAPI.AccessFlag.ENABLE_HIDE_FILES_IN_DIRECTORY_BROWSING);
            }

        }

        private void checkBox_AllowCopyout_CheckedChanged_1(object sender, EventArgs e)
        {
            if (!checkBox_AllowCopyout.Checked)
            {
                accessFlags = accessFlags & (~FilterAPI.AccessFlag.ALLOW_COPY_PROTECTED_FILES_OUT);
            }
            else
            {
                accessFlags = accessFlags | (FilterAPI.AccessFlag.ALLOW_COPY_PROTECTED_FILES_OUT);
            }
        }

        private void checkBox_AllowRead_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox_AllowRead.Checked)
            {
                accessFlags = accessFlags & (~FilterAPI.AccessFlag.ALLOW_READ_ACCESS);
            }
            else
            {
                accessFlags = accessFlags | (FilterAPI.AccessFlag.ALLOW_READ_ACCESS);
            }

        }

        private void checkBox_EnableEncryption_CheckedChanged(object sender, EventArgs e)
        {
            if (!isFormInitialized)
            {
                return;
            }

            if (checkBox_Encryption.Checked)
            {
                textBox_PassPhrase.ReadOnly = false;
                accessFlags = accessFlags | (FilterAPI.AccessFlag.ENABLE_FILE_ENCRYPTION_RULE);

                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("Please note that the existing files won't be encrypted.", "encryption", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                textBox_PassPhrase.ReadOnly = true;
                accessFlags = accessFlags & (~FilterAPI.AccessFlag.ENABLE_FILE_ENCRYPTION_RULE);

                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("Please note that the existing encrypted files won't be decrypted.", "encryption", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void button_AddUserRights_Click_1(object sender, EventArgs e)
        {
            string defaultUserName = Environment.UserDomainName + "\\" + Environment.UserName;
            Form_AccessRights accessRightsForm = new Form_AccessRights(false, FilterAPI.ALLOW_MAX_RIGHT_ACCESS, defaultUserName);

            if (accessRightsForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string userName = accessRightsForm.accessName;
                uint accessFlags = accessRightsForm.accessFlags;

                if( userRightList.ContainsKey(userName))
                {
                    userRightList.Remove(userName);
                }

                userRightList.Add(userName, accessFlags);
                InitAccessRightsListView();
            }
        }

        private void button_AddProcessRights_Click(object sender, EventArgs e)
        {
            Form_AccessRights accessRightsForm = new Form_AccessRights(true, FilterAPI.ALLOW_MAX_RIGHT_ACCESS, "explorer.exe");

            if (accessRightsForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string processName = accessRightsForm.accessName;
                uint accessFlags = accessRightsForm.accessFlags;
                string certName = accessRightsForm.certName;
                string sha256Hash = accessRightsForm.imageSha256Name;

                if (processNameAccessRightList.ContainsKey(processName))
                {
                    processNameAccessRightList.Remove(processName);
                }

                ProcessRightInfo processRight = new ProcessRightInfo(accessFlags, processName, certName, sha256Hash);

                processNameAccessRightList.Add(processName, processRight);
                InitAccessRightsListView();
            }
        }

        private void button_RemoveRights_Click(object sender, EventArgs e)
        {
            if (listView_AccessRights.SelectedItems.Count != 1)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("Please select one item to delete.", "Delete Access Right", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            System.Windows.Forms.ListViewItem item = listView_AccessRights.SelectedItems[0];
            string type = item.SubItems[0].Text;
            string name = item.SubItems[1].Text;

            if (string.Compare(type, "user") == 0)
            {
                userRightList.Remove(name);
            }
            else
            {
                processNameAccessRightList.Remove(name);
            }

            InitAccessRightsListView();

        }

        private void button_BrowseFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog browseFolder = new FolderBrowserDialog();

            if (browseFolder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox_FolderName.Text = browseFolder.SelectedPath;
            }
        }

     
    }
}
