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

using EaseFilter.CommonObjects;

namespace AutoFileCryptTool
{
    public partial class VerifyPasswordForm : Form
    {
        public bool isPasswordMatched = false;

        public VerifyPasswordForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void button_VerifyPassword_Click(object sender, EventArgs e)
        {
            if (string.Compare(textBox_Password1.Text, GlobalConfig.MasterPassword, false) != 0)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("The password is not correct.", "Password", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isPasswordMatched = false;
            }
            else
            {
                isPasswordMatched = true;
                this.Close();
            }
        }
    }
}
