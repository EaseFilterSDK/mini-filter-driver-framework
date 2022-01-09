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

using EaseFilter.FilterControl;
using EaseFilter.CommonObjects;

namespace FileProtector
{
    public partial class DecryptedFileForm : Form
    {

        public DecryptedFileForm()
        {
            InitializeComponent();
        }

        private void button_Start_Click(object sender, EventArgs e)
        {

            string passPhrase = textBox_PassPhrase.Text.Trim();
            string fileName = textBox_FileName.Text.Trim();
            long offset = long.Parse(textBox_Offset.Text.Trim());
            int decryptionLength = int.Parse(textBox_DecryptionLength.Text.Trim());
            string lastError = string.Empty;
            bool retVal = false;

            if (passPhrase.Length == 0)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("Password phrase can't be empty.", "Encryption", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (fileName.Length == 0)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("File name can't be empty.", "Encryption", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            byte[] key = Utils.GetKeyByPassPhrase(passPhrase,32);
            byte[] decryptedBuffer = new byte[decryptionLength];
            int bytesDecrypted = 0;

            retVal = FilterAPI.AESDecryptBytes(fileName, (uint)key.Length, key, 0, null, offset, decryptionLength, decryptedBuffer, ref bytesDecrypted);
            if (!retVal)
            {
                string errorMessage = FilterAPI.GetLastErrorMessage();

                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("Decrypt file " + fileName + " at offset " + offset.ToString() + " failed with error:" + errorMessage, "decryption offset", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }
            else
            {
                Array.Resize(ref decryptedBuffer, bytesDecrypted);

                string decryptedText = Encoding.ASCII.GetString(decryptedBuffer);
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show(decryptedText, "Decrypted data", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
