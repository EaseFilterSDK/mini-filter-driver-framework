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
using System.Windows.Forms;

using EaseFilter.CommonObjects;

namespace  SecureShare
{
    public partial class ShareFileSettingForm : Form
    {

        public ShareFileSettingForm()
        {
            StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();

            this.textBox_AutoEncryptFolder.Text = GlobalConfig.AutoEncryptFolder;
            this.textBox_AuthorizedUserNames.Text = GlobalConfig.AuthorizedUserNames;
            this.textBox_UnauthorizedUserNames.Text = GlobalConfig.UnAuthorizedUserNames;
            this.textBox_ProtectFolderWhiteList.Text = GlobalConfig.AuthorizedProcessNames;
            this.textBox_ProtectFolderBlackList.Text = GlobalConfig.UnAuthorizedProcessNames;

            DateTime expireDateTime = DateTime.FromFileTimeUtc(GlobalConfig.ShareFileExpireTime);
            if(expireDateTime < DateTime.Now)
            {
                expireDateTime = DateTime.Now.AddDays(2); 
            }

            dateTimePicker_ExpireTime.Value = dateTimePicker_ExpireDate.Value = expireDateTime;

            this.textBox_SharedFileDropFolder.Text = GlobalConfig.DropFolder;
            this.textBox_DRMFolder.Text = GlobalConfig.DRMFolder;
            this.radioButton_DRMInLocal.Checked = GlobalConfig.IsDRMDataInLocal;
        }

        private void button_BrowseFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox_SharedFileDropFolder.Text = fbd.SelectedPath;
            }
        }

        private void button_DRMFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox_DRMFolder.Text = fbd.SelectedPath;
            }
        }

        private void button_ApplySettings_Click(object sender, EventArgs e)
        {
            if (textBox_SharedFileDropFolder.Text.Length == 0)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("The share file folder name can't be empty.", "share file folder", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return ;
            }

            GlobalConfig.AutoEncryptFolder = textBox_AutoEncryptFolder.Text;
            GlobalConfig.AuthorizedProcessNames = textBox_ProtectFolderWhiteList.Text;
            GlobalConfig.UnAuthorizedProcessNames = textBox_ProtectFolderBlackList.Text;
            DateTime expireDateTime = dateTimePicker_ExpireDate.Value.Date + dateTimePicker_ExpireTime.Value.TimeOfDay;
            GlobalConfig.ShareFileExpireTime = expireDateTime.ToFileTimeUtc();

            GlobalConfig.DropFolder = textBox_SharedFileDropFolder.Text;
            GlobalConfig.DRMFolder = textBox_DRMFolder.Text;
            GlobalConfig.IsDRMDataInLocal = radioButton_DRMInLocal.Checked;

            if (!Directory.Exists(GlobalConfig.DropFolder))
            {
                Directory.CreateDirectory(GlobalConfig.DropFolder);
            }

            if (!Directory.Exists(GlobalConfig.AutoEncryptFolder))
            {
                Directory.CreateDirectory(GlobalConfig.AutoEncryptFolder);
            }

            if (!Directory.Exists(GlobalConfig.DRMFolder))
            {
                Directory.CreateDirectory(GlobalConfig.DRMFolder);
            }

            GlobalConfig.SaveConfigSetting();

            this.Close();
        }

        private void button_help_Click(object sender, EventArgs e)
        {
            MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
            string helpInfo = "1.The Auto Encrypt Folder is a real-time encryption and decryption folder. When you copy a file into this folder, it is automatically encrypted using a newly generated encryption key and a unique random IV (initialization vector).\r\n";
                helpInfo += "If you want to copy the encrypted file out of this folder, you must add the process name to the unauthorized process list.For example, if you're using Windows Explorer to copy the file, add explorer.exe to the unauthorizedProcessNames list.\r\n\r\n";

            helpInfo += "2.Encrypted Shared File Drop Folder:\r\nYou can copy encrypted shared files into this folder.It will not encrypt newly created files, but it will automatically decrypt encrypted files for authorized processes.\r\n\r\n";
            helpInfo += "3.The DRM data can be stored either on the server or locally. If the DRM data is stored on the server, you can grant or revoke access rights anytime and from anywhere.\r\n";
            helpInfo +=  " It is more convenient for POC testing if the DRM data is stored locally.\r\n";

            MessageBox.Show(helpInfo, "How to set the configuration?", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

      
    }
}
