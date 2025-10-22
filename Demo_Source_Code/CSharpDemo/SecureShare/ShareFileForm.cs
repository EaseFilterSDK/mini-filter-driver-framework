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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using EaseFilter.FilterControl;
using EaseFilter.CommonObjects;

namespace  SecureShare
{
    public partial class ShareFileForm : Form
    {
        /// <summary>
        /// This is the existing DRMData to be modified.
        /// </summary>
        DRMData selectDRMData = null;

        public ShareFileForm(DRMData drmData)
        {
            InitializeComponent();

            textBox_FileName.Text = drmData.FileName;

            selectDRMData = drmData;
            textBox_FileName.Enabled = false;
            textBox_TargetName.Enabled = false;
            button_OpenFile.Enabled = false;

            dateTimePicker_ExpireDate.Value = DateTime.FromFileTime(drmData.ExpireTime);
            dateTimePicker_ExpireTime.Value = DateTime.FromFileTime(drmData.ExpireTime);
            textBox_authorizedProcessNames.Text = drmData.AuthorizedProcessNames;
            textBox_UnauthorizedProcessNames.Text = drmData.UnauthorizedProcessNames;
            textBox_AuthorizedUserNames.Text = drmData.AuthorizedUserNames;
            textBox_UnauthorizedUserNames.Text = drmData.UnauthorizedUserNames;

            button_CreateFile.Text = "Apply change";
        }

        public ShareFileForm()
        {
            InitializeComponent();
            dateTimePicker_ExpireDate.Value = DateTime.Now.AddDays(2);
            dateTimePicker_ExpireTime.Value = DateTime.Now;

            button_CreateFile.Text = "Create Share File";
        }

        private void GetDRMData(ref DRMData drmData)
        {
            try
            {
                drmData.AuthorizedProcessNames = textBox_authorizedProcessNames.Text.Trim().ToLower();
                drmData.UnauthorizedProcessNames = textBox_UnauthorizedProcessNames.Text.Trim().ToLower();
                drmData.AuthorizedUserNames = textBox_AuthorizedUserNames.Text.Trim().ToLower();
                drmData.UnauthorizedUserNames = textBox_UnauthorizedUserNames.Text.Trim().ToLower();
                DateTime expireDate = dateTimePicker_ExpireDate.Value.Date + dateTimePicker_ExpireTime.Value.TimeOfDay;
                drmData.FileName = Path.GetFileName(textBox_FileName.Text);
                drmData.ExpireTime = expireDate.ToFileTime();

            }
            catch (Exception ex)
            {
                throw new Exception("GetDRMData failed with error:" + ex.Message);
            }

            return ;
        }


        private bool CreateShareEncryptFile()
        {
            string lastError = string.Empty;            

            string authorizedProcessNames = textBox_authorizedProcessNames.Text.Trim();
            string unauthorizedProcessNames = textBox_UnauthorizedProcessNames.Text.Trim();
            string authorizedUserNames = textBox_AuthorizedUserNames.Text.Trim();
            string unauthorizedUserNames = textBox_UnauthorizedUserNames.Text.Trim();
            string authorizedComputerIds = string.Empty;
            string authorizedIps = string.Empty;
            string fileName = textBox_FileName.Text.Trim();
            string targetFileName = textBox_TargetName.Text;
            uint accessFlags = FilterAPI.ALLOW_MAX_RIGHT_ACCESS;

            try
            {
                if (fileName.Length == 0)
                {
                    MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                    MessageBox.Show("The file name can't be empty.", "Create share encrypted file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                DateTime expireDateTime = dateTimePicker_ExpireDate.Value.Date + dateTimePicker_ExpireTime.Value.TimeOfDay;
                if (expireDateTime <= DateTime.Now)
                {
                    MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                    MessageBox.Show("The expire time can't be less than current time.", "Create share encrypted file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if(!File.Exists(fileName))
                {
                    MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                    MessageBox.Show("The file " + fileName + " doesn't exist.", "FileExist", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                //here we generate the random unique IV and key, you can use your own key and iv
                byte[] encryptionIV = Utils.GetRandomIV();
                byte[] encryptionKey = Utils.GetRandomKey();
                string encryptionIVStr = Utils.ByteArrayToHexStr(encryptionIV);
                string encryptionKeyStr = Utils.ByteArrayToHexStr(encryptionKey);

                bool retVal = DRMServer.AddDRMDataToServer(fileName, authorizedProcessNames, unauthorizedProcessNames, authorizedUserNames, unauthorizedUserNames, authorizedComputerIds
                    ,authorizedIps, expireDateTime.ToFileTimeUtc(), encryptionIVStr, encryptionKeyStr, accessFlags, out lastError);

                if(!retVal)
                {
                    MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                    MessageBox.Show("Create share file failed, AddDRMDataToServer failed with error:" + lastError, "AddDRMDataToServer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                string tagDataStr = GlobalConfig.AccountName + ";" + Environment.MachineName + ";" + encryptionIVStr;
                byte[] tagData = ASCIIEncoding.ASCII.GetBytes(tagDataStr);

                if (fileName.Equals(targetFileName, StringComparison.CurrentCulture))
                {
                    retVal = FilterAPI.AESEncryptFileWithTag(fileName, (uint)encryptionKey.Length, encryptionKey, (uint)encryptionIV.Length, encryptionIV, (uint)tagData.Length, tagData);
                }
                else
                {
                    retVal = FilterAPI.AESEncryptFileToFileWithTag(fileName, targetFileName, (uint)encryptionKey.Length, encryptionKey, (uint)encryptionIV.Length, encryptionIV, (uint)tagData.Length, tagData);
                }

                if (!retVal)
                {
                    MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                    MessageBox.Show("Create encrypted file " + targetFileName + " failed with error:" + FilterAPI.GetLastErrorMessage(), "Create share encrypted file", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    if (!fileName.Equals(targetFileName, StringComparison.CurrentCulture))
                    {
                        File.Delete(targetFileName);
                    }

                    return false;
                }
                else
                {
                    //set this flag to the encrypted file, require to get permission from user mode when the share encrypted file was opened 
                    if (!FilterAPI.SetHeaderFlags(targetFileName, (uint)AESFlags.Flags_Request_IV_And_Key_From_User,FilterAPI.ALLOW_MAX_RIGHT_ACCESS))
                    {
                        MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                        MessageBox.Show("SetHeaderFlags for file " + targetFileName + " failed with error:" + FilterAPI.GetLastErrorMessage(), "SetHeaderFlags", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        return false;
                    }

                    MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                    string message = "Create encrypted file " + targetFileName + " succeeded, you can share this encrypted file to your clients.\r\n\r\nPlease put this encrypted file to the share file drop folder in the client,";
                    message += " you can read this encrypted file if you have the permission to read when the driver service is started.";
                    MessageBox.Show(message, "Share encrypted file", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                return true;

            }
            catch (Exception ex)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("Create share file failed with error " + ex.Message, "Create share encrypted file", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }
        }

        private bool ModifyShareEncryptFile()
        {
            string lastError = string.Empty;

            string authorizedProcessNames = textBox_authorizedProcessNames.Text.Trim();
            string unauthorizedProcessNames = textBox_UnauthorizedProcessNames.Text.Trim();
            string authorizedUserNames = textBox_AuthorizedUserNames.Text.Trim();
            string unauthorizedUserNames = textBox_UnauthorizedUserNames.Text.Trim();
            string authorizedComputerIds = string.Empty;
            string authorizedIps = string.Empty;
            string fileName = textBox_FileName.Text.Trim();
            string targetFileName = textBox_TargetName.Text;
            uint accessFlags = FilterAPI.ALLOW_MAX_RIGHT_ACCESS;
            string encryptionIVStr = selectDRMData.EncryptionIV;
            string encryptionKeyStr = selectDRMData.EncryptionKey;            

            try
            {
                if (fileName.Length == 0)
                {
                    MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                    MessageBox.Show("The file name can't be empty.", "Create share encrypted file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                DateTime expireDateTime = dateTimePicker_ExpireDate.Value.Date + dateTimePicker_ExpireTime.Value.TimeOfDay;
                if (expireDateTime <= DateTime.Now)
                {
                    MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                    MessageBox.Show("The expire time can't be less than current time.", "Create share encrypted file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                bool retVal = DRMServer.ModifyDRMDataFromServer(fileName, authorizedProcessNames, unauthorizedProcessNames, authorizedUserNames, unauthorizedUserNames, authorizedComputerIds
                    , authorizedIps, expireDateTime.ToFileTimeUtc(), encryptionIVStr, encryptionKeyStr, accessFlags, out lastError);

                if (!retVal)
                {
                    MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                    MessageBox.Show("ModifyDRMDataFromServer failed with error:" + lastError, "ModifyDRMDataFromServer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                GetDRMData(ref selectDRMData);

                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                string message = "ModifyDRMDataFromServer " + targetFileName + " succeeded.";
                MessageBox.Show(message, "ModifyDRMDataFromServer", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return true;

            }
            catch (Exception ex)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("Create share file failed with error " + ex.Message, "Create share encrypted file", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }
        }


        private void button_OpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox_FileName.Text = openFileDialog.FileName;
                textBox_TargetName.Text = openFileDialog.FileName;
            }
        }

      
        private void button_CreateFile_Click(object sender, EventArgs e)
        {
            try
            {
                string lastError = string.Empty;

                if(null == selectDRMData)
                {
                    CreateShareEncryptFile();
                }
                else 
                {
                    ModifyShareEncryptFile();
                }

                this.Close();

            }
            catch (Exception ex)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("Add digital right information to file " + textBox_FileName + " failed with error " + ex.Message, "Create", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    

    }
}
