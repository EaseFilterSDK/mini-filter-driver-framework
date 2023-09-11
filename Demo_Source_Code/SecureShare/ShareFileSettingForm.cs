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

            this.textBox_DRFolder.Text = GlobalConfig.DRInfoFolder;
            this.textBox_ShareFolder.Text = GlobalConfig.ShareFolder;
            this.textBox_ShareFolderWhiteList.Text = GlobalConfig.ShareFolderWhiteList;
            this.textBox_ShareFolderBlackList.Text = GlobalConfig.ShareFolderBlackList;
            this.textBox_ProtectFolder.Text = GlobalConfig.ProtectFolder;
            this.textBox_ProtectFolderWhiteList.Text = GlobalConfig.ProtectFolderWhiteList;
            this.textBox_ProtectFolderBlackList.Text = GlobalConfig.ProtectFolderBlackList;

        }

        private void button_BrowseFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox_ShareFolder.Text = fbd.SelectedPath;
            }
        }

        private void button_ApplySettings_Click(object sender, EventArgs e)
        {
            if (textBox_ShareFolder.Text.Length == 0)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("The share file folder name can't be empty.", "share file folder", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return ;
            }

            if (textBox_DRFolder.Text.Length == 0)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("The DR folder name can't be empty.", "share file folder", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            GlobalConfig.ProtectFolder = textBox_ProtectFolder.Text;
            GlobalConfig.ProtectFolderWhiteList = textBox_ProtectFolderWhiteList.Text;
            GlobalConfig.ProtectFolderBlackList = textBox_ProtectFolderBlackList.Text;

            GlobalConfig.ShareFolder = textBox_ShareFolder.Text;
            GlobalConfig.ShareFolderWhiteList = textBox_ShareFolderWhiteList.Text;
            GlobalConfig.ShareFolderBlackList = textBox_ShareFolderBlackList.Text;

            GlobalConfig.StoreSharedFileMetaDataInServer = false;
            GlobalConfig.DRInfoFolder = textBox_DRFolder.Text;

            if (!Directory.Exists(GlobalConfig.ShareFolder))
            {
                Directory.CreateDirectory(GlobalConfig.ShareFolder);
            }

            if (!Directory.Exists(GlobalConfig.ProtectFolder))
            {
                Directory.CreateDirectory(GlobalConfig.ProtectFolder);
            }

            if (!Directory.Exists(GlobalConfig.DRInfoFolder))
            {
                Directory.CreateDirectory(GlobalConfig.DRInfoFolder);
            }

            GlobalConfig.SaveConfigSetting();

            this.Close();
        }

        private void button_help_Click(object sender, EventArgs e)
        {
            MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
            string helpInfo = "1.Protected folder is a real time encryption and decryption folder, if you copy a file to this folder, it will be encrypted automatically with default 256 bits encryption key and a random unique iv, so don't copy the encrypted files this folder, or it will be encrypted again. if you want to copy the encrypted file out of this folder, you need to add the process name to unauhtorized process list, for example if you copy it with Windows explorer, then add explorer.exe to the black list.\r\n\r\n";
            helpInfo += "2.Shared file drop folder, you can copy the encrypted share files to this folder, this folder won't encrypt the new created file, but it can decrypt the encrypted file automatically for the authorized processes.\r\n\r\n";
            helpInfo += "3.When you create the encrypted file from share file manager, the meta data of the encrypted file will be stored in local or in server, if you store the meta data in server, you can grant or revoke access rights anytime anywhere.\r\n";

            MessageBox.Show(helpInfo, "How to set the configuration?", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
