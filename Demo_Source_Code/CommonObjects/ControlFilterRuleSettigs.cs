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
    public partial class ControlFilterRuleSettigs : Form
    {
        public FileFilterRule filterRule = new FileFilterRule();

        public ControlFilterRuleSettigs()
        {
        }

        public ControlFilterRuleSettigs(FileFilterRule _filterRule)
        {
            InitializeComponent();
            filterRule = _filterRule;

            textBox_FileAccessFlags.Text = filterRule.AccessFlag.ToString();        
            textBox_ControlIO.Text = filterRule.RegisterControlFileIOEvents.ToString();
            checkBox_EnableProtectionInBootTime.Checked = filterRule.IsResident;
            textBox_ProcessRights.Text = filterRule.ProcessNameRights;
            textBox_ProcessIdRights.Text = filterRule.ProcessIdRights;
            textBox_SignedProcessAccessRights.Text = filterRule.SignedProcessAccessRights;
            textBox_UserRights.Text = filterRule.UserRights;

            textBox_PassPhrase.Text = filterRule.EncryptionPassPhrase;
            textBox_HiddenFilterMask.Text = filterRule.HiddenFileFilterMasks;
            textBox_ReparseFileFilterMask.Text = filterRule.ReparseFileFilterMask;
            textBox_EncryptWriteBufferSize.Text = filterRule.EncryptWriteBufferSize.ToString();

            SetCheckBoxValue();

            if (filterRule.EncryptMethod ==  (int)FilterAPI.EncryptionMethod.ENCRYPT_FILE_WITH_SAME_KEY_AND_UNIQUE_IV )
            {
                radioButton_EncryptFileWithSameKey.Checked = true;
                radioButton_EncryptFileWithTagData.Checked = false;
            }
            else
            {
                radioButton_EncryptFileWithSameKey.Checked = false;
                radioButton_EncryptFileWithTagData.Checked = true;
            }
        }

        private void SetCheckBoxValue()
        {

            uint accessFlags = uint.Parse(textBox_FileAccessFlags.Text);

            if ((accessFlags & (uint)FilterAPI.AccessFlag.ENABLE_FILE_ENCRYPTION_RULE) > 0 )
            {
                checkBox_Encryption.Checked = true;
                textBox_PassPhrase.ReadOnly = false;
            }
            else
            {
                checkBox_Encryption.Checked = false;
                textBox_PassPhrase.ReadOnly = true;
            }

            if ((accessFlags & (uint)FilterAPI.AccessFlag.ENABLE_HIDE_FILES_IN_DIRECTORY_BROWSING) > 0)
            {
                checkBox_EnableHidenFile.Checked = true;
                textBox_HiddenFilterMask.ReadOnly = false;
            }
            else
            {
                checkBox_EnableHidenFile.Checked = false;
                textBox_HiddenFilterMask.ReadOnly = true;
            }

            if ((accessFlags & (uint)FilterAPI.AccessFlag.ENABLE_REPARSE_FILE_OPEN) > 0)
            {
                checkBox_EnableReparseFile.Checked = true;
                textBox_ReparseFileFilterMask.ReadOnly = false;
            }
            else
            {
                checkBox_EnableReparseFile.Checked = false;
                textBox_ReparseFileFilterMask.ReadOnly = true;
            }

            if ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_FILE_ACCESS_FROM_NETWORK) > 0)
            {
                checkBox_AllowRemoteAccess.Checked = true;
            }
            else
            {
                checkBox_AllowRemoteAccess.Checked = false;
            }

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

            if ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_WRITE_ACCESS) > 0)
            {
                checkBox_AllowFileWriting.Checked = true;
            }
            else
            {
                checkBox_AllowFileWriting.Checked = false;
            }

            if ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_SET_INFORMATION) > 0)
            {
                checkBox_AllowChange.Checked = true;
            }
            else
            {
                checkBox_AllowChange.Checked = false;
            }

            if ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_OPEN_WITH_CREATE_OR_OVERWRITE_ACCESS) > 0)
            {
                checkBox_AllowNewFileCreation.Checked = true;
            }
            else
            {
                checkBox_AllowNewFileCreation.Checked = false;
            }

         
            if ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_SET_SECURITY_ACCESS) > 0)
            {
                checkBox_AllowSetSecurity.Checked = true;
            }
            else
            {
                checkBox_AllowSetSecurity.Checked = false;
            }


            if ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_ENCRYPT_NEW_FILE) > 0)
            {
                checkBox_AllowEncryptNewFile.Checked = true;
            }
            else
            {
                checkBox_AllowEncryptNewFile.Checked = false;
            }

            if ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_COPY_PROTECTED_FILES_OUT) > 0)
            {
                checkBox_AllowCopyOut.Checked = true;
            }
            else
            {
                checkBox_AllowCopyOut.Checked = false;
            }

            if ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_READ_ENCRYPTED_FILES) > 0)
            {
                checkBox_AllowReadEncryptedFiles.Checked = true;
            }
            else
            {
                checkBox_AllowReadEncryptedFiles.Checked = false;
            }

            if (GlobalConfig.EnableSendDeniedEvent)
            {
                checkBox_EnableSendDeniedEvent.Checked = true;
            }
            else
            {
                checkBox_EnableSendDeniedEvent.Checked = false;
            }
        }

        private void button_SaveControlSettings_Click(object sender, EventArgs e)
        {
            string encryptionPassPhrase = string.Empty;
            uint accessFlags = uint.Parse(textBox_FileAccessFlags.Text);

            if (checkBox_Encryption.Checked)
            {               
                encryptionPassPhrase = textBox_PassPhrase.Text;

                //enable encryption for this filter rule.
                accessFlags = accessFlags | (uint)FilterAPI.AccessFlag.ENABLE_FILE_ENCRYPTION_RULE;

                if (radioButton_EncryptFileWithSameKey.Checked)
                {
                    filterRule.EncryptMethod = (int)FilterAPI.EncryptionMethod.ENCRYPT_FILE_WITH_SAME_KEY_AND_UNIQUE_IV;
                }
                else
                {
                    filterRule.EncryptMethod = (int)FilterAPI.EncryptionMethod.ENCRYPT_FILE_WITH_KEY_IV_AND_TAGDATA_FROM_SERVICE;
                }

                filterRule.EncryptWriteBufferSize = uint.Parse(textBox_EncryptWriteBufferSize.Text);
            }

            if (textBox_HiddenFilterMask.Text.Trim().Length > 0)
            {
                //enable hidden filter mask for this filter rule.
                accessFlags = accessFlags | (uint)FilterAPI.AccessFlag.ENABLE_HIDE_FILES_IN_DIRECTORY_BROWSING;
            }

            if (checkBox_EnableSendDeniedEvent.Checked)
            {
                GlobalConfig.EnableSendDeniedEvent = true;
            }
            else
            {
                GlobalConfig.EnableSendDeniedEvent = false;
            }           
           
            filterRule.EncryptionPassPhrase = encryptionPassPhrase;
            filterRule.HiddenFileFilterMasks = textBox_HiddenFilterMask.Text;
            filterRule.ReparseFileFilterMask = textBox_ReparseFileFilterMask.Text;
            filterRule.AccessFlag = accessFlags;
            filterRule.RegisterControlFileIOEvents = ulong.Parse(textBox_ControlIO.Text);
            filterRule.IsResident = checkBox_EnableProtectionInBootTime.Checked;
            filterRule.UserRights = textBox_UserRights.Text;
            filterRule.ProcessNameRights = textBox_ProcessRights.Text;
            filterRule.Sha256ProcessAccessRights = textBox_Sha256ProcessAccessRights.Text;
            filterRule.SignedProcessAccessRights = textBox_SignedProcessAccessRights.Text;
            filterRule.ProcessIdRights = textBox_ProcessIdRights.Text;

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


        private void button_RegisterControlIO_Click(object sender, EventArgs e)
        {
            OptionForm optionForm = new OptionForm(OptionForm.OptionType.ControlFileIOEvents, textBox_ControlIO.Text);

            if (optionForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox_ControlIO.Text = optionForm.ControlIOEvents.ToString();
            }
        }

        private void button_AddProcessRights_Click(object sender, EventArgs e)
        {
            Form_AccessRights accessRightsForm = new Form_AccessRights(Form_AccessRights.AccessRightType.ProcessNameRight);

            if (accessRightsForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (textBox_ProcessRights.Text.Trim().Length > 0)
                {
                    textBox_ProcessRights.Text += ";" + accessRightsForm.accessRightText;
                }
                else
                {
                    textBox_ProcessRights.Text = accessRightsForm.accessRightText;
                }
            }
        }

        private void button_AddSha256ProcessAccessRights_Click(object sender, EventArgs e)
        {
            Form_AccessRights accessRightsForm = new Form_AccessRights(Form_AccessRights.AccessRightType.Sha256Process);

            if (accessRightsForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (textBox_ProcessRights.Text.Trim().Length > 0)
                {
                    textBox_Sha256ProcessAccessRights.Text += ";" + accessRightsForm.accessRightText;
                }
                else
                {
                    textBox_Sha256ProcessAccessRights.Text = accessRightsForm.accessRightText;
                }
            }
        }

        private void button_AddSignedProcessAccessRights_Click(object sender, EventArgs e)
        {
            Form_AccessRights accessRightsForm = new Form_AccessRights(Form_AccessRights.AccessRightType.SignedProcess);

            if (accessRightsForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (textBox_ProcessRights.Text.Trim().Length > 0)
                {
                    textBox_SignedProcessAccessRights.Text += ";" + accessRightsForm.accessRightText;
                }
                else
                {
                    textBox_SignedProcessAccessRights.Text = accessRightsForm.accessRightText;
                }
            }
        }     


        private void button_AddProcessIdRights_Click(object sender, EventArgs e)
        {
            Form_AccessRights accessRightsForm = new Form_AccessRights(Form_AccessRights.AccessRightType.ProccessIdRight);

            if (accessRightsForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (textBox_ProcessIdRights.Text.Trim().Length > 0)
                {
                    textBox_ProcessIdRights.Text += ";" + accessRightsForm.accessRightText;
                }
                else
                {
                    textBox_ProcessIdRights.Text = accessRightsForm.accessRightText;
                }
            }

        }   


        private void button_AddUserRights_Click(object sender, EventArgs e)
        {
            Form_AccessRights accessRightsForm = new Form_AccessRights(Form_AccessRights.AccessRightType.UserNameRight);

            if (accessRightsForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (textBox_UserRights.Text.Trim().Length > 0)
                {
                    textBox_UserRights.Text += ";" + accessRightsForm.accessRightText;
                }
                else
                {
                    textBox_UserRights.Text = accessRightsForm.accessRightText;
                }
            }
        }


        private void checkBox_Encryption_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_FileAccessFlags.Text);

            if (checkBox_Encryption.Checked)
            {
                textBox_PassPhrase.ReadOnly = false;
                accessFlags = accessFlags | ((uint)FilterAPI.AccessFlag.ENABLE_FILE_ENCRYPTION_RULE);
            }
            else
            {
                textBox_PassPhrase.ReadOnly = true;
                accessFlags = accessFlags & ((uint)~FilterAPI.AccessFlag.ENABLE_FILE_ENCRYPTION_RULE);
            }

            textBox_FileAccessFlags.Text = accessFlags.ToString();
        }

        private void checkBox_AllowDelete_CheckedChanged(object sender, EventArgs e)
        {

            uint accessFlags = uint.Parse(textBox_FileAccessFlags.Text);

            if (!checkBox_AllowDelete.Checked)
            {
                accessFlags = accessFlags & ((uint)~FilterAPI.AccessFlag.ALLOW_FILE_DELETE);
            }
            else
            {
                accessFlags = accessFlags | ((uint)FilterAPI.AccessFlag.ALLOW_FILE_DELETE);
            }

            textBox_FileAccessFlags.Text = accessFlags.ToString();
        }

        private void checkBox_AllowChange_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_FileAccessFlags.Text);
            if (!checkBox_AllowChange.Checked)
            {
                accessFlags = accessFlags & ((uint)~FilterAPI.AccessFlag.ALLOW_SET_INFORMATION);
            }
            else
            {
                accessFlags = accessFlags | ((uint)FilterAPI.AccessFlag.ALLOW_SET_INFORMATION);
            }

            textBox_FileAccessFlags.Text = accessFlags.ToString();
        }

        private void checkBox_AllowRename_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_FileAccessFlags.Text);

            if (!checkBox_AllowRename.Checked)
            {
                accessFlags = accessFlags & ((uint)~FilterAPI.AccessFlag.ALLOW_FILE_RENAME);
            }
            else
            {
                accessFlags = accessFlags | ((uint)FilterAPI.AccessFlag.ALLOW_FILE_RENAME);
            }

            textBox_FileAccessFlags.Text = accessFlags.ToString();
        }

        private void checkBox_AllowRemoteAccess_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_FileAccessFlags.Text);

            if (!checkBox_AllowRemoteAccess.Checked)
            {
                accessFlags = accessFlags & ((uint)~FilterAPI.AccessFlag.ALLOW_FILE_ACCESS_FROM_NETWORK);
            }
            else
            {
                accessFlags = accessFlags | ((uint)FilterAPI.AccessFlag.ALLOW_FILE_ACCESS_FROM_NETWORK);
            }

            textBox_FileAccessFlags.Text = accessFlags.ToString();
        }

        private void checkBox_AllowNewFileCreation_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_FileAccessFlags.Text);

            if (!checkBox_AllowNewFileCreation.Checked)
            {
                accessFlags = accessFlags & ((uint)~FilterAPI.AccessFlag.ALLOW_OPEN_WITH_CREATE_OR_OVERWRITE_ACCESS);
            }
            else
            {
                accessFlags = accessFlags | ((uint)FilterAPI.AccessFlag.ALLOW_OPEN_WITH_CREATE_OR_OVERWRITE_ACCESS);
            }

            textBox_FileAccessFlags.Text = accessFlags.ToString();
        }

        private void checkBox_AllowFileWriting_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_FileAccessFlags.Text);

            if (!checkBox_AllowFileWriting.Checked)
            {
                accessFlags = accessFlags & ((uint)~FilterAPI.AccessFlag.ALLOW_WRITE_ACCESS);
            }
            else
            {
                accessFlags = accessFlags | ((uint)FilterAPI.AccessFlag.ALLOW_WRITE_ACCESS);
            }

            textBox_FileAccessFlags.Text = accessFlags.ToString();
        }

        private void checkBox_AllowSetSecurity_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_FileAccessFlags.Text);

            if (!checkBox_AllowSetSecurity.Checked)
            {
                accessFlags = accessFlags & ((uint)~FilterAPI.AccessFlag.ALLOW_SET_SECURITY_ACCESS);
            }
            else
            {
                accessFlags = accessFlags | ((uint)FilterAPI.AccessFlag.ALLOW_SET_SECURITY_ACCESS);
            }

            textBox_FileAccessFlags.Text = accessFlags.ToString();
        }


        private void checkBox_AllowEncryptNewFile_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_FileAccessFlags.Text);

            if (!checkBox_AllowEncryptNewFile.Checked)
            {
                accessFlags = accessFlags & ((uint)~FilterAPI.AccessFlag.ALLOW_ENCRYPT_NEW_FILE);
            }
            else
            {
                accessFlags = accessFlags | ((uint)FilterAPI.AccessFlag.ALLOW_ENCRYPT_NEW_FILE);
            }

            textBox_FileAccessFlags.Text = accessFlags.ToString();
        }

        private void checkBox_AllowCopyOut_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_FileAccessFlags.Text);

            if (!checkBox_AllowCopyOut.Checked)
            {
                accessFlags = accessFlags & ((uint)~FilterAPI.AccessFlag.ALLOW_COPY_PROTECTED_FILES_OUT);
            }
            else
            {
                accessFlags = accessFlags | ((uint)FilterAPI.AccessFlag.ALLOW_COPY_PROTECTED_FILES_OUT);
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


        private void checkBox_EnableHidenFile_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_FileAccessFlags.Text);

            if (checkBox_EnableHidenFile.Checked)
            {
                textBox_HiddenFilterMask.ReadOnly = false;
                accessFlags = accessFlags | ((uint)FilterAPI.AccessFlag.ENABLE_HIDE_FILES_IN_DIRECTORY_BROWSING);
            }
            else
            {
                textBox_HiddenFilterMask.ReadOnly = true;
                accessFlags = accessFlags & ((uint)~FilterAPI.AccessFlag.ENABLE_HIDE_FILES_IN_DIRECTORY_BROWSING);
            }

            textBox_FileAccessFlags.Text = accessFlags.ToString();
        }

        private void checkBox_EnableReparseFile_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_FileAccessFlags.Text);

            if (checkBox_EnableReparseFile.Checked)
            {
                textBox_ReparseFileFilterMask.ReadOnly = false;
                accessFlags = accessFlags | ((uint)FilterAPI.AccessFlag.ENABLE_REPARSE_FILE_OPEN);
            }
            else
            {
                textBox_ReparseFileFilterMask.ReadOnly = true;
                accessFlags = accessFlags & ((uint)~FilterAPI.AccessFlag.ENABLE_REPARSE_FILE_OPEN);
            }

            textBox_FileAccessFlags.Text = accessFlags.ToString();
        }


        private void button_InfoControlFlag_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This is the file access control flags of the filter rule, enable or disable the specific access right by checking or unchecking the selected box.");
        }

        private void button_InfoCopyout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Prevent the files from being copied out of the folder when it was disabled.");
        }

        private void button_InfoEncryptNewFile_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Automatically encrypt the new created file when it was enabled, or it won't encrypt the new created file, a use case: copy the encrypted file to the folder, it won't encrypt the file again.");
        }

        private void button_InfoDecryption_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Automatically decrypt the created file when it was enabled, or the process will read the raw data of the encrypted file, a use case: the backup software.");
        }

        private void button_InfoEncryptOnRead_Click(object sender, EventArgs e)
        {
            MessageBox.Show("If you want to encrypt the file only when it was read by the process, you can enable the encryption feature, disable the new file encryption, enable the encryption on the go.");
        }

        private void button_InfoPassPhrase_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Enable the encryption feature and set the encryption key phrase for generating the encryption key.");
        }

        private void button_EnableEncryptionKeyFromService_Click(object sender, EventArgs e)
        {
            MessageBox.Show("If this is enabled, all encryption/decryption will get the encryption key from the callback service, you also can embed the custom tag data to the new created encrypted file.");
        }

        private void button_HideFileFilterMask_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Enable the hiden file feature, hide the file when the file name matches the filter mask, seperate the multiple filter mask with ';' character. i.e. *.txt;*.exe, this will hide .txt and .exe files");
        }

        private void button_InfoReparseFile_Click(object sender, EventArgs e)
        {
            string info = "Enable the reparse file feature,reparse the file open to the new file which the file name will be replaced with the reparse filter mask.\r\n\r\n";
            info += "i.e. FilterMask = c:\\test\\*txt, ReparseFilterMask = d:\\reparse\\*doc, If you open file c:\\test\\MyTest.txt, it will reparse to the file d:\\reparse\\MyTest.doc.";

            MessageBox.Show(info);
        }

        private void button_InfoProcessNameRights_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Add or remove the file access right of the process via the process name.");
        }

        private void button_InfoSha256ProcessRights_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Add the trusted process with the sha256 hash of the executable binary to filter rule, only the process has the same sha256 hash can access the files. ");
        }

        private void button_InfoProcessIdRights_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Add or remove the file access right of the process via the process Id.");
        }

        private void button_InfoUserRights_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Add or remove the file access right of the user via the user name.");
        }

        private void button_InfoControlEvents_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Register the control events, you can allow, modify or deny the file I/O.");
        }

        private void button_InfoSignedProcessRights_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Add the trusted process which was signed with the certificate to filter rule, only the authenticated process can access the files. ");
        }

        private void button_EncryptWriteBufferSize_Click(object sender, EventArgs e)
        {
            MessageBox.Show("If the encrypt write buffer size is greater than 0, then the small buffer encryption write will be combined together to a bigger buffer, and write it to the disk.");
        }

     
             
    }
}
