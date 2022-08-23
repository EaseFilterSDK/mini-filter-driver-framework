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

        DRPolicy selectedDRPolicy = null;

        public ShareFileForm(DRPolicy drPolicy)
        {
            InitializeComponent();

            textBox_FileName.Text = drPolicy.FileName;

            selectedDRPolicy = drPolicy;
            textBox_FileName.Enabled = false;
            textBox_TargetName.Enabled = false;
            button_OpenFile.Enabled = false;

            dateTimePicker_ExpireDate.Value = DateTime.FromFileTime(drPolicy.ExpireTime);
            dateTimePicker_ExpireTime.Value = DateTime.FromFileTime(drPolicy.ExpireTime);
            textBox_authorizedProcessNames.Text = drPolicy.AuthorizedProcessNames;
            textBox_UnauthorizedProcessNames.Text = drPolicy.UnauthorizedProcessNames;
            textBox_AuthorizedUserNames.Text = drPolicy.AuthorizedUserNames;
            textBox_UnauthorizedUserNames.Text = drPolicy.UnauthorizedUserNames;

            button_CreateFile.Text = "Apply change";
        }

        public ShareFileForm()
        {
            InitializeComponent();
            dateTimePicker_ExpireDate.Value = DateTime.Now.AddDays(1);
            dateTimePicker_ExpireTime.Value = DateTime.Now;

            button_CreateFile.Text = "Create Share File";
        }

        private DRPolicy GetDRSetting()
        {
            DRPolicy drPolicy = new DRPolicy();

            try
            {
                drPolicy.AuthorizedProcessNames = textBox_authorizedProcessNames.Text.Trim().ToLower();
                drPolicy.UnauthorizedProcessNames = textBox_UnauthorizedProcessNames.Text.Trim().ToLower();
                drPolicy.AuthorizedUserNames = textBox_AuthorizedUserNames.Text.Trim().ToLower();
                drPolicy.UnauthorizedUserNames = textBox_UnauthorizedUserNames.Text.Trim().ToLower();
                DateTime expireDate = dateTimePicker_ExpireDate.Value.Date + dateTimePicker_ExpireTime.Value.TimeOfDay;
                drPolicy.FileName = Path.GetFileName(textBox_TargetName.Text);
                drPolicy.ExpireTime = expireDate.ToFileTime();

                if (null != selectedDRPolicy)
                {
                    drPolicy.EncryptionIV = selectedDRPolicy.EncryptionIV;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Apply digital right failed with error:" + ex.Message);
            }

            return drPolicy;
        }


        private bool CreateOrModifyShareEncryptFile()
        {
            string lastError = string.Empty;            

            string authorizedProcessNames = textBox_authorizedProcessNames.Text.Trim();
            string unauthorizedProcessNames = textBox_UnauthorizedProcessNames.Text.Trim();
            string authorizedUserNames = textBox_AuthorizedUserNames.Text.Trim();
            string unauthorizedUserNames = textBox_UnauthorizedUserNames.Text.Trim();
            string fileName = textBox_FileName.Text.Trim();
            string targetFileName = textBox_TargetName.Text;

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


                //here we generate the random unique IV and key, you can use your own key and iv
                byte[] encryptionIV = Utils.GetRandomIV();
                byte[] encryptionKey = Utils.GetRandomKey();

                string keyStr = string.Empty;
                string ivStr = string.Empty;
                string tagStr = Utils.ByteArrayToHexStr(encryptionIV);

                byte[] tagData = UnicodeEncoding.Unicode.GetBytes(tagStr);

                bool retVal = false;

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
                    //set this flag to the encrypted file, require to get permission from user mode when the file open 
                    if (!FilterAPI.SetHeaderFlags(targetFileName, (uint)AESFlags.Flags_Request_IV_And_Key_From_User,FilterAPI.ALLOW_MAX_RIGHT_ACCESS))
                    {
                        MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                        MessageBox.Show("SetHeaderFlags for file " + targetFileName + " failed with error:" + FilterAPI.GetLastErrorMessage(), "SetHeaderFlags", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        return false;
                    }

                    if (!GlobalConfig.StoreSharedFileMetaDataInServer)
                    {
                        //add the permission meta data to a file and store it in the server, it will be used when the file open.
                        if (!DRServer.AddDRInfoToFile(targetFileName, authorizedProcessNames, unauthorizedProcessNames, authorizedUserNames, unauthorizedUserNames
                            , expireDateTime, encryptionIV, encryptionKey, FilterAPI.ALLOW_MAX_RIGHT_ACCESS.ToString()))
                        {
                            return false;
                        }

                    }
                 
                    MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                    string message = "Create encrypted file " + targetFileName + " succeeded, you can distribute this encrypted file to your client.\r\n\r\nDownload this file to the share file drop folder in the client,";
                    message += " then start the filter service there, now you can open the encrypted file if the process in client has the permission.";
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
                if (CreateOrModifyShareEncryptFile())
                {
                    this.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("Add digital right information to file " + textBox_FileName + " failed with error " + ex.Message, "Create", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    

    }
}
