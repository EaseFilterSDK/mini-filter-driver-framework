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
using EaseFilter.CommonObjects;

namespace EaseFilter.FolderLocker
{
   
    public partial class Form_AccessRights : Form
    {
        bool isProcessRights = true;

        public string accessName = string.Empty;
        public uint accessFlags = FilterAPI.ALLOW_MAX_RIGHT_ACCESS;
        public string certName = string.Empty;
        public string imageSha256Name = string.Empty;

        public Form_AccessRights(bool _isProcessRights, uint accessRights, string name)
        {
            InitializeComponent();

            isProcessRights = _isProcessRights;
            accessFlags = accessRights;
            textBox_AccessName.Text = name;

            if (isProcessRights)
            {
                label_Name.Text = "Process Name";
                
            }
            else
            {
                label_Name.Text = "User Name";
                groupBox_TrustedProcess.Visible = false;
            }

            SetCheckBoxValue();
        }

        private void button_Apply_Click(object sender, EventArgs e)
        {
            accessName = textBox_AccessName.Text;
            certName = textBox_CertificatName.Text;
            imageSha256Name = textBox_ImageSha256.Text;
        }


        private void SetCheckBoxValue()
        {       

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

            if ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_FILE_MEMORY_MAPPED) > 0)
            {
                checkBox_Execution.Checked = true;
            }
            else
            {
                checkBox_Execution.Checked = false;
            }

            if ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_QUERY_SECURITY_ACCESS) > 0)
            {
                checkBox_QuerySecurity.Checked = true;
            }
            else
            {
                checkBox_QuerySecurity.Checked = false;
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
                checkBox_AllowSaveAs.Checked = true;
            }
            else
            {
                checkBox_AllowSaveAs.Checked = false;
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
        }

        private void checkBox_Read_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Read.Checked)
            {
                accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_READ_ACCESS;
            }
            else
            {
                accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_READ_ACCESS;
            }

        }

        private void checkBox_Write_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Write.Checked)
            {
                accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_WRITE_ACCESS;
            }
            else
            {
                accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_WRITE_ACCESS;
            }

        }

        private void checkBox_Creation_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Creation.Checked)
            {
                accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_OPEN_WITH_CREATE_OR_OVERWRITE_ACCESS;
            }
            else
            {
                accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_OPEN_WITH_CREATE_OR_OVERWRITE_ACCESS;
            }

        }

        private void checkBox_Execution_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Execution.Checked)
            {
                accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_FILE_MEMORY_MAPPED;
            }
            else
            {
                accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_FILE_MEMORY_MAPPED;
            }

        }

        private void checkBox_AllowRemoteAccess_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_AllowRemoteAccess.Checked)
            {
                accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_FILE_ACCESS_FROM_NETWORK;
            }
            else
            {
                accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_FILE_ACCESS_FROM_NETWORK;
            }

        }

        private void checkBox_QueryInfo_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_QueryInfo.Checked)
            {
                accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_QUERY_INFORMATION_ACCESS;
            }
            else
            {
                accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_QUERY_INFORMATION_ACCESS;
            }

        }

        private void checkBox_SetInfo_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_SetInfo.Checked)
            {
                accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_SET_INFORMATION;
            }
            else
            {
                accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_SET_INFORMATION;
            }

        }

        private void checkBox_AllowRename_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_AllowRename.Checked)
            {
                accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_FILE_RENAME;
            }
            else
            {
                accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_FILE_RENAME;
            }
        }

        private void checkBox_AllowDelete_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_AllowDelete.Checked)
            {
                accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_FILE_DELETE;
            }
            else
            {
                accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_FILE_DELETE;
            }

        }

        private void checkBox_QuerySecurity_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_QuerySecurity.Checked)
            {
                accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_QUERY_SECURITY_ACCESS;
            }
            else
            {
                accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_QUERY_SECURITY_ACCESS;
            }

        }

        private void checkBox_SetSecurity_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_SetSecurity.Checked)
            {
                accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_SET_SECURITY_ACCESS;
            }
            else
            {
                accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_SET_SECURITY_ACCESS;
            }

        }

        private void checkBox_AllowSaveAs_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_AllowSaveAs.Checked)
            {
                accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_ALL_SAVE_AS;
            }
            else
            {
                accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_ALL_SAVE_AS;
            }

        }

        private void checkBox_AllowCopyout_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_AllowCopyout.Checked)
            {
                accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_COPY_PROTECTED_FILES_OUT;
            }
            else
            {
                accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_COPY_PROTECTED_FILES_OUT;
            }

        }

        private void checkBox_AllowReadEncryptedFiles_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_AllowReadEncryptedFiles.Checked)
            {
                accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_READ_ENCRYPTED_FILES;
            }
            else
            {
                accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_READ_ENCRYPTED_FILES;
            }

        }

        private void button_Info_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The certificate name of the signed process is optional, " +
               "if it is not empty, then only the process signed with the certificate will have the access rights.\n " +
                "The process sha256 hash is optional, if it is not empty, then only the process with same sha256 hash will have the access rights.\n");
        }

        private void button_GetCertificateName_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fileName = fileDialog.FileName;
                uint len = 1024;
                long signedTime = 0;
                string subjectName = new string((char)0, (int)len);

                if (FilterAPI.GetSignerInfo(fileName, subjectName, ref len, ref signedTime))
                {
                    subjectName = subjectName.Substring(0, (int)len / 2);
                    textBox_CertificatName.Text = subjectName;
                }
                else
                {
                    string lastError = "Get process's certificate name failed with error:" + FilterAPI.GetLastErrorMessage();
                    MessageBox.Show(lastError, "Get process's certificate name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button_GetSha256_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fileName = fileDialog.FileName;
                byte[] hashBytes = new byte[32];
                uint hashBytesLength = 32;

                if (FilterAPI.Sha256HashFile(fileName, hashBytes, ref hashBytesLength))
                {
                    textBox_ImageSha256.Text = Utils.ByteArrayToHex(hashBytes);
                }
                else
                {
                    string lastError = "Get file sha256 hash failed with error:" + FilterAPI.GetLastErrorMessage();
                    MessageBox.Show(lastError, "Get sha256 hash", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
