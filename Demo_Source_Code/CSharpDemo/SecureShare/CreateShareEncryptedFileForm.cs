///////////////////////////////////////////////////////////////////////////////
//
//    (C) Copyright 2012 EaseFilter Technologies Inc.
//    All Rights Reserved
//
//    This software is part of a licensed software product and may
//    only be used or copied in accordance with the terms of that license.
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

using EaseFilter.CommonObjects;

namespace FileProtector
{
    public partial class ShareEncryptedFileCreationForm : Form
    {
        public ShareEncryptedFileCreationForm()
        {
            InitializeComponent();
            textBox_FileAccessFlags.Text = FilterAPI.ALLOW_MAX_RIGHT_ACCESS.ToString();
            dateTimePicker_ExpireTime.Value = DateTime.Now + TimeSpan.FromDays(1);
        }

        private void button_FileAccessFlags_Click(object sender, EventArgs e)
        {
            OptionForm optionForm = new OptionForm(OptionForm.OptionType.Access_Flag, textBox_FileAccessFlags.Text);

            if (optionForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox_FileAccessFlags.Text = optionForm.AccessFlags.ToString();
            }
        }


        private void button_CreateShareEncryptedFile_Click(object sender, EventArgs e)
        {
            if (textBox_FileName.Text.Trim().Length == 0)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("The file name can't be empty.", "Create share encrypted file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (textBox_PassPhrase.Text.Trim().Length == 0)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("The pass phrase can't be empty.", "Create share encrypted file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            AESAccessPolicy policy = new AESAccessPolicy();

            if (textBox_IncludeProcessNames.Text.Trim().Length > 0 || textBox_ExcludeProcessNames.Text.Trim().Length > 0)
            {
                policy.AESFlags |=  (uint)AESFlags.Flags_Enabled_Check_ProcessName;
            }

            if (textBox_IncludeUserNames.Text.Trim().Length > 0 || textBox_ExcludeUserNames.Text.Trim().Length > 0)
            {
                policy.AESFlags |= (uint)AESFlags.Flags_Enabled_Check_UserName;
            }

            policy.AESFlags |= (uint)AESFlags.Flags_Enabled_Check_AccessFlags | (uint)AESFlags.Flags_Enabled_Expire_Time;

            string encryptionPassPhrase =  textBox_PassPhrase.Text;
            uint accessFlags = uint.Parse(textBox_FileAccessFlags.Text);
            
            policy.AccessFlags = accessFlags;
            policy.IncludeProcessNames = textBox_IncludeProcessNames.Text.Trim();
            policy.LengthOfIncludeProcessNames = (uint)textBox_IncludeProcessNames.Text.Length * 2;
            policy.ExcludeProcessNames = textBox_ExcludeProcessNames.Text.Trim();
            policy.LengthOfExcludeProcessNames = (uint)textBox_ExcludeProcessNames.Text.Length * 2;
            policy.IncludeUserNames = textBox_IncludeUserNames.Text.Trim();
            policy.LengthOfIncludeUserNames = (uint)textBox_IncludeUserNames.Text.Length * 2;
            policy.ExcludeUserNames = textBox_ExcludeUserNames.Text.Trim();
            policy.LengthOfExcludeUserNames = (uint)textBox_ExcludeUserNames.Text.Length * 2;
            policy.ExpireTime = dateTimePicker_ExpireTime.Value.ToUniversalTime().ToFileTimeUtc();

            string lastError = string.Empty;

            if (!EncryptionHandler.EncryptFileWithEmbeddedPolicy(textBox_FileName.Text, textBox_PassPhrase.Text, policy, out lastError))
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("Create share encrypted file failed with error:" + lastError, "Process share encrypted file", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }
            else
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("Create share encrypted file succeeded.", "Process share encrypted file", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }


        private void checkBox_DisplayPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_DisplayPassword.Checked)
            {
                textBox_PassPhrase.UseSystemPasswordChar = false;
            }
            else
            {
                textBox_PassPhrase.UseSystemPasswordChar = true;
            }
        }     

        private void button_OpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox_FileName.Text = openFileDialog.FileName;
            }
        }

 


    }
}
