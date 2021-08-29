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
    public partial class SetupPasswordForm : Form
    {
        public bool isPasswordMatched = false;

        public SetupPasswordForm()
        {           
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void button_NewPassword_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox_Password1.Text))
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("The password can't be empty.", "Password", MessageBoxButtons.OK, MessageBoxIcon.Error);

                isPasswordMatched = false;
            }
            else if (string.Compare(textBox_Password1.Text, textBox_Password2.Text, false) != 0)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("The password doesn't match.", "Password", MessageBoxButtons.OK, MessageBoxIcon.Error);

                isPasswordMatched = false;
            }
            else
            {
                isPasswordMatched = true;
                GlobalConfig.MasterPassword = textBox_Password1.Text;
                this.Close();
            }
        }

      
    }
}
