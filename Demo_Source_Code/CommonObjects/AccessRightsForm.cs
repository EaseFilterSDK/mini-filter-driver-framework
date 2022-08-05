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

using EaseFilter.FilterControl;

namespace EaseFilter.CommonObjects
{

    public partial class Form_AccessRights : Form
    {
        public enum AccessRightType
        {
            ProcessNameRight = 0,
            ProcessSha256,
            ProccessIdRight,
            UserNameRight,
        }

        AccessRightType type = AccessRightType.ProccessIdRight;

        public string accessRightText = string.Empty;

        public Form_AccessRights(AccessRightType _type)
        {
            InitializeComponent();

            type = _type;
            groupBox_AccessRights.Location = groupBox_ProcessSha256.Location;
            groupBox_ProcessId.Location = groupBox_ProcessName.Location;
            groupBox_ProcessSha256.Location = groupBox_ProcessName.Location;
            groupBox_UserName.Location = groupBox_ProcessName.Location;

            textBox_FileAccessFlags.Text = FilterAPI.ALLOW_MAX_RIGHT_ACCESS.ToString();
            SetCheckBoxValue();

            switch (type)
            {
                case AccessRightType.ProcessNameRight: groupBox_ProcessName.Visible = true; break;
                case AccessRightType.ProcessSha256: groupBox_ProcessSha256.Visible = true; break;
                case AccessRightType.ProccessIdRight: groupBox_ProcessId.Visible = true; break;
                case AccessRightType.UserNameRight: groupBox_UserName.Visible = true; break;
            }
            
        }

        private void button_Add_Click(object sender, EventArgs e)
        {

            switch (type)
            {
                case AccessRightType.ProcessNameRight:
                    {
                        if (textBox_ProcessName.Text.Trim().Length > 0)
                        {
                            string[] processNames = textBox_ProcessName.Text.Trim().Split(new char[] { ';' });
                            if (processNames.Length > 0)
                            {
                                foreach (string processName in processNames)
                                {
                                    if (processName.Trim().Length > 0)
                                    {
                                        if (accessRightText.Length > 0)
                                        {
                                            accessRightText += ";";
                                        }

                                        accessRightText += processName.Trim() + "!" + textBox_FileAccessFlags.Text;
                                    }
                                }
                            }
                        }
                        break;
                    }

                case AccessRightType.ProcessSha256:
                    {
                        if (textBox_ProcessSha256.Text.Trim().Length > 0)
                        {
                            string[] processShaList = textBox_ProcessSha256.Text.Trim().Split(new char[] { ';' });
                            if (processShaList.Length > 0)
                            {
                                foreach (string processSha in processShaList)
                                {
                                    if (processSha.Trim().Length > 0)
                                    {
                                        if (accessRightText.Length > 0)
                                        {
                                            accessRightText += ";";
                                        }

                                        accessRightText += processSha.Trim() + "!" + textBox_FileAccessFlags.Text;
                                    }
                                }
                            }
                        }
                        break;
                    }

                case AccessRightType.UserNameRight:
                    {
                        if (textBox_UserName.Text.Trim().Length > 0)
                        {
                            string[] userNames = textBox_UserName.Text.Trim().Split(new char[] { ';' });
                            if (userNames.Length > 0)
                            {
                                foreach (string userName in userNames)
                                {
                                    if (userName.Trim().Length > 0)
                                    {
                                        if (accessRightText.Length > 0)
                                        {
                                            accessRightText += ";";
                                        }

                                        accessRightText += userName.Trim() + "!" + textBox_FileAccessFlags.Text;
                                    }
                                }
                            }
                        }

                        break;
                    }

                case AccessRightType.ProccessIdRight:
                    {
                        if (textBox_ProcessId.Text.Trim().Length > 0)
                        {
                            string[] processIds = textBox_ProcessId.Text.Trim().Split(new char[] { ';' });
                            if (processIds.Length > 0)
                            {
                                foreach (string processId in processIds)
                                {
                                    if (processId.Trim().Length > 0)
                                    {
                                        if (accessRightText.Length > 0)
                                        {
                                            accessRightText += ";";
                                        }

                                        accessRightText += processId.Trim() + "!" + textBox_FileAccessFlags.Text;
                                    }
                                }
                            }
                        }

                        break;
                    }
            }

        }


        private void SetCheckBoxValue()
        {

            uint accessFlags = uint.Parse(textBox_FileAccessFlags.Text);
    
            if ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_FILE_DELETE) > 0)
            {
                checkBox_AllowDelete.Checked = true;
            }
            else
            {
                checkBox_AllowDelete.Checked = false;
            }

            if ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_FILE_RENAME) > 0)
            {
                checkBox_AllowRename.Checked = true;
            }
            else
            {
                checkBox_AllowRename.Checked = false;
            }

            if ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_WRITE_ACCESS) > 0 )
            {
                checkBox_Write.Checked = true;
            }
            else
            {
                checkBox_Write.Checked = false;
            }

            if ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_READ_ACCESS) > 0)
            {
                checkBox_Read.Checked = true;
            }
            else
            {
                checkBox_Read.Checked = false;
            }

            if ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_QUERY_INFORMATION_ACCESS) > 0)
            {
                checkBox_QueryInfo.Checked = true;
            }
            else
            {
                checkBox_QueryInfo.Checked = false;
            }

            if ( (accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_SET_INFORMATION) > 0 )
            {
                checkBox_SetInfo.Checked = true;
            }
            else
            {
                checkBox_SetInfo.Checked = false;
            }

            if ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_OPEN_WITH_CREATE_OR_OVERWRITE_ACCESS) > 0)
            {
                checkBox_Creation.Checked = true;
            }
            else
            {
                checkBox_Creation.Checked = false;
            }

            if ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_QUERY_SECURITY_ACCESS) > 0)
            {
                checkBox_EnableEncryptionOnRead.Checked = true;
            }
            else
            {
                checkBox_EnableEncryptionOnRead.Checked = false;
            }

            if ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_SET_SECURITY_ACCESS) > 0)
            {
                checkBox_SetSecurity.Checked = true;
            }
            else
            {
                checkBox_SetSecurity.Checked = false;
            }

            if ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_ALL_SAVE_AS) > 0)
            {
                checkBox_AllowEncryptNewFile.Checked = true;
            }
            else
            {
                checkBox_AllowEncryptNewFile.Checked = false;
            }

            if ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_COPY_PROTECTED_FILES_OUT) > 0)
            {
                checkBox_AllowCopyout.Checked = true;
            }
            else
            {
                checkBox_AllowCopyout.Checked = false;
            }

            if ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_READ_ENCRYPTED_FILES) > 0)
            {
                checkBox_AllowReadEncryptedFiles.Checked = true;
            }
            else
            {
                checkBox_AllowReadEncryptedFiles.Checked = false;
            }


            if ((accessFlags & (uint)FilterAPI.AccessFlag.DISABLE_ENCRYPT_DATA_ON_READ) > 0)
            {
                checkBox_EnableEncryptionOnRead.Checked = false;
            }
            else
            {
                checkBox_EnableEncryptionOnRead.Checked = true;
            }
        }

        private void button_FileAccessFlags_Click(object sender, EventArgs e)
        {
            OptionForm optionForm = new OptionForm(OptionForm.OptionType.Access_Flag, textBox_FileAccessFlags.Text);

            if (optionForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (optionForm.AccessFlags > 0)
                {
                    textBox_FileAccessFlags.Text = optionForm.AccessFlags.ToString();
                }
                else
                {
                    //if the accessFlag is 0, it is exclude filter rule,this is not what we want, so we need to include this flag.
                    textBox_FileAccessFlags.Text = ((uint)FilterAPI.AccessFlag.LEAST_ACCESS_FLAG).ToString();
                }

                SetCheckBoxValue();
            }
        }

        private void checkBox_Read_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_FileAccessFlags.Text.Trim());
            if (checkBox_Read.Checked)
            {
                accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_READ_ACCESS;
            }
            else
            {
                accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_READ_ACCESS;
            }

            textBox_FileAccessFlags.Text = accessFlags.ToString();
        }

        private void checkBox_Write_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_FileAccessFlags.Text.Trim());
            if (checkBox_Write.Checked)
            {
                accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_WRITE_ACCESS;
            }
            else
            {
                accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_WRITE_ACCESS;
            }

            textBox_FileAccessFlags.Text = accessFlags.ToString();
        }

        private void checkBox_Creation_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_FileAccessFlags.Text.Trim());
            if (checkBox_Creation.Checked)
            {
                accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_OPEN_WITH_CREATE_OR_OVERWRITE_ACCESS;
            }
            else
            {
                accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_OPEN_WITH_CREATE_OR_OVERWRITE_ACCESS;
            }

            textBox_FileAccessFlags.Text = accessFlags.ToString();
        }



        private void checkBox_QueryInfo_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_FileAccessFlags.Text.Trim());
            if (checkBox_QueryInfo.Checked)
            {
                accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_QUERY_INFORMATION_ACCESS;
            }
            else
            {
                accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_QUERY_INFORMATION_ACCESS;
            }

            textBox_FileAccessFlags.Text = accessFlags.ToString();
        }

        private void checkBox_SetInfo_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_FileAccessFlags.Text.Trim());
            if (checkBox_SetInfo.Checked)
            {
                accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_SET_INFORMATION;
            }
            else
            {
                accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_SET_INFORMATION;
            }

            textBox_FileAccessFlags.Text = accessFlags.ToString();
        }

        private void checkBox_AllowRename_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_FileAccessFlags.Text.Trim());
            if (checkBox_AllowRename.Checked)
            {
                accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_FILE_RENAME;
            }
            else
            {
                accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_FILE_RENAME;
            }

            textBox_FileAccessFlags.Text = accessFlags.ToString();
        }

        private void checkBox_AllowDelete_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_FileAccessFlags.Text.Trim());
            if (checkBox_AllowDelete.Checked)
            {
                accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_FILE_DELETE;
            }
            else
            {
                accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_FILE_DELETE;
            }

            textBox_FileAccessFlags.Text = accessFlags.ToString();
        }

        private void checkBox_EncryptionOnRead_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_FileAccessFlags.Text.Trim());
            if (checkBox_EnableEncryptionOnRead.Checked)
            {
                accessFlags &= ~(uint)FilterAPI.AccessFlag.DISABLE_ENCRYPT_DATA_ON_READ;
            }
            else
            {
                accessFlags |= (uint)FilterAPI.AccessFlag.DISABLE_ENCRYPT_DATA_ON_READ;
            }

            textBox_FileAccessFlags.Text = accessFlags.ToString();
        }

        private void checkBox_SetSecurity_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_FileAccessFlags.Text.Trim());
            if (checkBox_SetSecurity.Checked)
            {
                accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_SET_SECURITY_ACCESS;
            }
            else
            {
                accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_SET_SECURITY_ACCESS;
            }

            textBox_FileAccessFlags.Text = accessFlags.ToString();
        }

        private void checkBox_AllowEncryptNewFile_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_FileAccessFlags.Text.Trim());
            if (checkBox_AllowEncryptNewFile.Checked)
            {
                accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_ENCRYPT_NEW_FILE;
            }
            else
            {
                accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_ENCRYPT_NEW_FILE;
            }

            textBox_FileAccessFlags.Text = accessFlags.ToString();
        }

        private void checkBox_AllowCopyout_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_FileAccessFlags.Text.Trim());
            if (checkBox_AllowCopyout.Checked)
            {
                accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_COPY_PROTECTED_FILES_OUT;
            }
            else
            {
                accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_COPY_PROTECTED_FILES_OUT;
            }

            textBox_FileAccessFlags.Text = accessFlags.ToString();
        }

        private void checkBox_AllowReadEncryptedFiles_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_FileAccessFlags.Text.Trim());
            if (checkBox_AllowReadEncryptedFiles.Checked)
            {
                accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_READ_ENCRYPTED_FILES;
            }
            else
            {
                accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_READ_ENCRYPTED_FILES;
            }

            textBox_FileAccessFlags.Text = accessFlags.ToString();
        }

        private void button_ProcessId_Click(object sender, EventArgs e)
        {
            OptionForm optionForm = new OptionForm(OptionForm.OptionType.ProccessId, textBox_ProcessId.Text);

            if (optionForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (optionForm.ProcessId.Length > 0)
                {
                    textBox_ProcessId.Text = optionForm.ProcessId;
                }
            }
        }

        private void button_GetProcessSha256_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fileName = fileDialog.FileName;
                byte[] hashBytes = new byte[32];
                uint hashBytesLength = 32;

                if (FilterAPI.Sha256HashFile(fileName, hashBytes, ref hashBytesLength))
                {
                    textBox_ProcessSha256.Text += Utils.ByteArrayToHex(hashBytes);
                }
                else
                {
                    string lastError = "Get file sha256 hash failed with error:" + FilterAPI.GetLastErrorMessage();
                    MessageBox.Show(lastError, "Get sha256 hash", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void button_InfoProcessName_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Set the specific access rights to the process name list, seperate the process name with ';'.");
        }

        private void button_InfoUserName_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Set the specific access rights to the user name list, seperate the user name with ';'.");
        }

        private void button_InfoCopyout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Prevent the files from being copied out of the folder when it was disabled.");
        }

        private void button_InfoEncryptNewFile_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Automatically encrypt the new created file when it was enabled, or it won't encrypt the new created file, a use case: copy the encrypted file to the folder, it won't encrypt the file again. To enable this feature it requires the encryption was enabled in the filter rule.");  
        }

        private void button_InfoDecryptFile_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Automatically decrypt the created file when it was enabled, or the process will read the raw data of the encrypted file, a use case: the backup software. To enable this feature it requires the encryption was enabled in the filter rule.");
        }

        private void button_InfoEncryptOnRead_Click(object sender, EventArgs e)
        {
            MessageBox.Show("If you want to encrypt the file only when it was read by the process, you can enable the encryption feature, disable the new file encryption, enable the encryption on the go. To enable this feature it requires the encryption was enabled in the filter rule.");
        }       

 
    }
}
