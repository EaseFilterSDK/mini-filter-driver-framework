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

using EaseFilter.FilterControl;
using EaseFilter.CommonObjects;

namespace AutoFileCryptTool
{
    public partial class Form_FileCrypt : Form
    {

        FilterControl filterControl = new FilterControl();

        public enum FilterRuleType : int
        {
            AutoEncryption = 0,
            EncryptionOnRead,
        }

        public Form_FileCrypt()
        {
            GlobalConfig.filterType = FilterAPI.FilterType.ENCRYPTION_FILTER|FilterAPI.FilterType.CONTROL_FILTER|FilterAPI.FilterType.PROCESS_FILTER;

            InitializeComponent();

            StartPosition = FormStartPosition.CenterScreen;

            InitializeFileCrypt();

        }


        private void InitializeFileCrypt()
        {
            int numberOfAutoEncryptFolders = 0;
            int numberOfEncryptOnReadFolders = 0;

            foreach (FileFilterRule filterRule in GlobalConfig.FilterRules.Values)
            {
                if (filterRule.Type == (int)FilterRuleType.AutoEncryption)
                {
                    numberOfAutoEncryptFolders++;
                }
                else if (filterRule.Type == (int)FilterRuleType.EncryptionOnRead)
                {
                    numberOfEncryptOnReadFolders++;
                }
            }

            listView_AutoEncryptFolders.Items.Clear();
            listView_EncryptOnReadFolders.Items.Clear();

            if (numberOfAutoEncryptFolders == 0)
            {
                AddDefaultItemsToAutoFolderList();

            }

            if (numberOfEncryptOnReadFolders == 0)
            {
                AddDefaultItemsToEncryptOnReadList();
            }

            foreach (FileFilterRule filterRule in GlobalConfig.FilterRules.Values)
            {
                if (filterRule.Type == (int)FilterRuleType.AutoEncryption)
                {

                    string folderName = filterRule.IncludeFileFilterMask;
                    if (folderName.EndsWith("\\*"))
                    {
                        folderName = folderName.Substring(0, folderName.Length - 2);
                    }

                    ListViewItem item = new ListViewItem(folderName);
                    item.ImageIndex = 0;
                    listView_AutoEncryptFolders.Items.Add(item);
                }
                else if (filterRule.Type == (int)FilterRuleType.EncryptionOnRead)
                {

                    string folderName = filterRule.IncludeFileFilterMask;
                    if (folderName.EndsWith("\\*"))
                    {
                        folderName = folderName.Substring(0, folderName.Length - 2);
                    }

                    ListViewItem item = new ListViewItem(folderName);
                    item.ImageIndex = 0;
                    listView_EncryptOnReadFolders.Items.Add(item);
                }
            }

            GlobalConfig.SaveConfigSetting();
        }

        public void SendConfigSettingsToFilter()
        {
            filterControl.ClearFilters();
            foreach (FileFilterRule filterRule in GlobalConfig.FilterRules.Values)
            {
                FileFilter fileFilter = filterRule.ToFileFilter();
                filterControl.AddFilter(fileFilter);
            }

            FileFilter dropFolderFileFilter = new FileFilter(GlobalConfig.DropFolder + "\\*");
            //enable the encryption, it is for decryption, disable encrypt new file since the file was encrypted in this folder.
            dropFolderFileFilter.AccessFlags = (FilterAPI.AccessFlag)((FilterAPI.ALLOW_MAX_RIGHT_ACCESS | (uint)FilterAPI.AccessFlag.ENABLE_FILE_ENCRYPTION_RULE) & (~(uint)FilterAPI.AccessFlag.ALLOW_ENCRYPT_NEW_FILE));
            dropFolderFileFilter.EncryptionPassPhrase = GlobalConfig.MasterPassword;
            dropFolderFileFilter.EncryptionIV = Utils.GetIVByPassPhrase(GlobalConfig.MasterPassword);
            
            filterControl.AddFilter(dropFolderFileFilter);

            string lastError = string.Empty;
            if (!filterControl.SendConfigSettingsToFilter(ref lastError))
            {
                MessageBox.Show(lastError, "StartFilter", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            return;
        }

        /// <summary>
        /// The new file will be automatically encrypted when it was added to the auto encrypt folders.
        /// The while list process will automatically get the decrypted data when it reads the encrypted file.
        /// The black list process will get the raw data ( encrypted ) data when it reads the encrypted file.
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        private bool AddAutoEncyrptFolder(string folderName)
        {
           
            string includeFilterMask = folderName + "\\*";
            string blackProcessList = string.Empty;
            string passPhrase = string.Empty;

            BlackListForm blackListForm = new BlackListForm();

            if (blackListForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                blackProcessList = blackListForm.BlackList;
            }

            //for blacklist process for autoencryption, it has maximum acess rights.
            string blackListProcessRights = "";
            string[] blacklist = blackProcessList.Split(new char[] { ';' });
            if (blacklist.Length > 0)
            {
                foreach (string unAuthorizedUser in blacklist)
                {
                    if (unAuthorizedUser.Trim().Length > 0)
                    {
                        //can't read the encrypted files
                        uint accessFlag = FilterAPI.ALLOW_MAX_RIGHT_ACCESS & (uint)(~FilterAPI.AccessFlag.ALLOW_READ_ENCRYPTED_FILES);
                        blackListProcessRights += ";" + unAuthorizedUser + "!" + accessFlag.ToString();
                    }
                }
            }

            PasswordForm passwordForm = new PasswordForm();

            if (passwordForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                passPhrase = passwordForm.PassPhrase;
            }

            if (passPhrase.Trim().Length == 0)
            {
                MessageBox.Show("The passphrase was empty,it will use the default passPhrase 'test'", "PassPhrase", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                passPhrase = "test";
            }

            FileFilterRule autoEncrytFilterRule = new FileFilterRule();
            autoEncrytFilterRule.Type = (int)FilterRuleType.AutoEncryption;
            autoEncrytFilterRule.IncludeFileFilterMask = folderName + "\\*";
            autoEncrytFilterRule.EncryptionPassPhrase = passPhrase;
            autoEncrytFilterRule.AccessFlag = (uint)FilterAPI.ALLOW_MAX_RIGHT_ACCESS | (uint)FilterAPI.AccessFlag.ENABLE_FILE_ENCRYPTION_RULE;
            autoEncrytFilterRule.EncryptMethod = (int)FilterAPI.EncryptionMethod.ENCRYPT_FILE_WITH_SAME_KEY_AND_UNIQUE_IV;
            autoEncrytFilterRule.ProcessNameRights = blackListProcessRights;

            GlobalConfig.AddFileFilterRule(autoEncrytFilterRule);
           
            InitializeFileCrypt();

            return true;
        }

        private void button_AddFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fdDiaglog = new FolderBrowserDialog();
            if (fdDiaglog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string folderName = fdDiaglog.SelectedPath;
                AddAutoEncyrptFolder(folderName);
            }

            string lastError = string.Empty;

            SendConfigSettingsToFilter();
        }


        private void button_RemoveFolder_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView_AutoEncryptFolders.SelectedItems)
            {
                string folderName = item.Text + "\\*";
                GlobalConfig.RemoveFilterRule(folderName);
            }

            InitializeFileCrypt();

            string lastError = string.Empty;

            SendConfigSettingsToFilter();
        }

        private void button_StartToEncrypt_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = textBox_EncryptSourceName.Text;
                string targetFileName = textBox_EncryptTargetName.Text;

                string passPhrase = textBox_PassPhraseEncrypt.Text.Trim();
                byte[] key = Utils.GetKeyByPassPhrase(passPhrase, 32);
                byte[] iv = Utils.GetIVByPassPhrase(passPhrase);                

                //test if the file was encrypted already, then we don't need to encrypt it again.
                byte[] ivBuffer = new byte[16];
                uint ivSize = 16;
                if (FilterAPI.GetAESIV(fileName, ref ivSize, ivBuffer))
                {
                    string lastError = "The file " + fileName + " was encrypted already, you can't encrypt it again.";
                    ShowMessage(lastError, MessageBoxIcon.Warning);

                    return;
                }

                bool retVal = false;

                if (fileName.Equals(targetFileName, StringComparison.CurrentCulture))
                {
                    retVal = FilterAPI.AESEncryptFileWithTag(fileName, (uint)key.Length, key, (uint)iv.Length, iv, (uint)iv.Length, iv);
                }
                else
                {
                    retVal = FilterAPI.AESEncryptFileToFileWithTag(fileName, targetFileName, (uint)key.Length, key, (uint)iv.Length, iv, (uint)iv.Length, iv);
                }

                if (!retVal)
                {
                    string lastError = "Encrypt file " + targetFileName + " failed with error:" + FilterAPI.GetLastErrorMessage();
                    ShowMessage(lastError, MessageBoxIcon.Error);
                }
                else
                {
                    string lastError = "Encrypt file " + fileName + " to " + targetFileName + " succeeded.";
                    ShowMessage(lastError, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                string lastError = "Encrypt file failed with error:" + ex.Message;
                ShowMessage(lastError, MessageBoxIcon.Error);
            }
        }

        private void button_BrowseEncryptFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDiag  = new OpenFileDialog();
            if (fileDiag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox_EncryptSourceName.Text = fileDiag.FileName;
                textBox_EncryptTargetName.Text = fileDiag.FileName;
            }
        }

        private void button_BrowseFileToDecrypt_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDiag = new OpenFileDialog();
            if (fileDiag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox_DecryptSourceName.Text = fileDiag.FileName; 
                textBox_DecryptTargetName.Text = fileDiag.FileName;
               
            }
        }

        private void button_StartToDecrypt_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = textBox_DecryptSourceName.Text;
                string targetFileName = textBox_DecryptTargetName.Text;

                string passPhrase = textBox_PassPhraseDecrypt.Text.Trim();
                byte[] key = Utils.GetKeyByPassPhrase(passPhrase, 32);
                byte[] iv = new byte[16];
                uint ivSize = (uint)iv.Length;

                if(checkBox_DecryptFileOnTheGo.Checked)
                {
                    iv = Utils.GetIVByPassPhrase(GlobalConfig.MasterPassword);
                    key = Utils.GetKeyByPassPhrase(GlobalConfig.MasterPassword, 32);
                }
                else if (!FilterAPI.GetAESIV(fileName, ref ivSize, iv))
                {
                    string lastError = "GetAESIV from encrypted file " + fileName + " failed with error:" + FilterAPI.GetLastErrorMessage();
                    ShowMessage(lastError, MessageBoxIcon.Error);
                    return;
                }

                bool retVal = false;
                if (fileName.Equals(targetFileName, StringComparison.CurrentCulture))
                {
                    retVal = FilterAPI.AESDecryptFile(fileName, (uint)key.Length, key, (uint)iv.Length, iv);
                }
                else
                {
                    retVal = FilterAPI.AESDecryptFileToFile(fileName, targetFileName, (uint)key.Length, key, (uint)iv.Length, iv);
                }

                if (!retVal)
                {
                    string lastError = "Decrypt file " + fileName + " failed with error:" + FilterAPI.GetLastErrorMessage();
                    ShowMessage(lastError, MessageBoxIcon.Error);
                }
                else
                {
                    string lastError = "Decrypt file " + fileName + " to " + targetFileName + " succeeded.";
                    ShowMessage(lastError, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                string lastError = "Decrypt file failed with error:" + ex.Message;
                ShowMessage(lastError, MessageBoxIcon.Error);
            }
        }


        private void ShowMessage(string message,MessageBoxIcon messageIcon)
        {
            MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
            MessageBox.Show(message, "Message", MessageBoxButtons.OK, messageIcon);
        }


        private void Form_FileCrypt_FormClosed(object sender, FormClosedEventArgs e)
        {
            FilterAPI.ResetConfigData();
            GlobalConfig.Stop();
            filterControl.StopFilter();

            Application.Exit();
        }


        void StartService()
        {

            //Purchase a license key with the link: http://www.easefilter.com/Order.htm
            //Email us to request a trial key: info@easefilter.com //free email is not accepted.        
            string licenseKey = GlobalConfig.LicenseKey;

            GlobalConfig.filterType = FilterAPI.FilterType.CONTROL_FILTER | FilterAPI.FilterType.ENCRYPTION_FILTER | FilterAPI.FilterType.PROCESS_FILTER;

            bool ret = false;
            string lastError = string.Empty;

            try
            {
                ret = filterControl.StartFilter(GlobalConfig.filterType, GlobalConfig.FilterConnectionThreads, GlobalConfig.ConnectionTimeOut, licenseKey, ref lastError);
                if (!ret)
                {
                    MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                    MessageBox.Show("Start encryption service failed with error:" + lastError + ", auto folder encryption service will stop.", "Auto FileCrypt Service", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return ;
                }

                SendConfigSettingsToFilter();

            }
            catch (Exception ex)
            {
                EventManager.WriteMessage(104, "StartFilter", EventLevel.Error, "Start filter service failed with error " + ex.Message);
                ret = false;
            }

            button_Start.Enabled = false;
            button_Stop.Enabled = true;
            button_StartService.Enabled = false;
            button_StopService.Enabled = true;

        }

        void StopService()
        {
            button_Start.Enabled = true;
            button_Stop.Enabled = false;
            button_StartService.Enabled = true;
            button_StopService.Enabled = false;


            filterControl.StopFilter();
        }
  
        private void button_Start_Click(object sender, EventArgs e)
        {
            StartService();
        }

        private void button_Stop_Click(object sender, EventArgs e)
        {
            StopService();
        }

        /// <summary>
        /// The files in protected folder will be automatically encrypted when it was uploaded or copied out
        /// by the blacklist processes, the files were not encrypted in the local disk.
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        private bool AddEncryptOnReadFolder(string folderName)
        {

            string includeFilterMask = folderName + "\\*";

            FileFilterRule EncryptOnReadFilterRule = new FileFilterRule();
            EncryptOnReadFilterRule.Type = (int)FilterRuleType.EncryptionOnRead;
            EncryptOnReadFilterRule.IncludeFileFilterMask = folderName + "\\*";
            EncryptOnReadFilterRule.EncryptionPassPhrase = GlobalConfig.MasterPassword;
            EncryptOnReadFilterRule.AccessFlag = (uint)FilterAPI.ALLOW_MAX_RIGHT_ACCESS | (uint)FilterAPI.AccessFlag.ENABLE_FILE_ENCRYPTION_RULE;
            EncryptOnReadFilterRule.AccessFlag &= (uint)(~FilterAPI.AccessFlag.ALLOW_ENCRYPT_NEW_FILE); //disable new created file encryption
            EncryptOnReadFilterRule.EncryptMethod = (int)FilterAPI.EncryptionMethod.ENCRYPT_FILE_WITH_SAME_KEY_AND_IV;

            GlobalConfig.AddFileFilterRule(EncryptOnReadFilterRule);

            GlobalConfig.SaveConfigSetting();

            return true;
        }


        private void button_AddEncryptOnReadFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fdDiaglog = new FolderBrowserDialog();
            if (fdDiaglog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string folderName = fdDiaglog.SelectedPath;
                AddEncryptOnReadFolder(folderName);
            }

            InitializeFileCrypt();

            string lastError = string.Empty;

            SendConfigSettingsToFilter();
        }


        private void button_RemoveEncryptOnReadFolder_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView_EncryptOnReadFolders.SelectedItems)
            {
                string folderName = item.Text + "\\*";
                GlobalConfig.RemoveFilterRule(folderName);
            }

            InitializeFileCrypt();

            string lastError = string.Empty;

            SendConfigSettingsToFilter();
        }

        private void button_SetupDropFolder_Click(object sender, EventArgs e)
        {
            DropFolderForm dropFolderForm = new DropFolderForm();
            if (dropFolderForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                GlobalConfig.DropFolder = dropFolderForm.dropFolder;
                GlobalConfig.SaveConfigSetting();

                SendConfigSettingsToFilter();
            }
        }

    
        private void button_StartService_Click(object sender, EventArgs e)
        {
            StartService();
        }

        private void button_StopService_Click(object sender, EventArgs e)
        {
            StopService();
        }

        private void button_Help_Click(object sender, EventArgs e)
        {
            MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
            string helpInfo = "1.Setup auto encryption folder.\r\n";
            helpInfo += "The new created file will be encrypted automatically,the process which is not in the black list will get the decrypted data automatically when it reads the file.";
            helpInfo += "Add the process to the black list if you want the process to get the raw data of the encrypted file, i.e. backup software.\r\n\r\n";
            helpInfo += "2.Setup auto encrypt file on the go folder.\r\n";
            helpInfo += "The file will be encrypted automatically when the process which is in the black list reads the file.";
            helpInfo += "To decrypt the encrypted file via encrypt file on the go, you either decrypt the file manually, or copy it to the auto encryption folder without the service running,";
            helpInfo += "i.e. web browser upload the files, outlook emails the files via attachment, the files will be encrypted automatically, but the files in the local are not encrypted.\r\n\r\n";
            helpInfo += "3.Encrypt/Decrypt file manually.\r\n";
            helpInfo += "You can encrypt the file manually, or decrypt the encrypted file manually, make sure to use the same passphrase for both encryption and decryption.\r\n";

            MessageBox.Show(helpInfo, "How to use the tool?", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

     
       }
}
