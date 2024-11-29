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
using System.IO;

using EaseFilter.CommonObjects;

namespace SecureSandbox
{
    public partial class TrayForm: Form
    {
        SecureSandbox secureSandbox = null;

        public TrayForm()
        {
              
            InitializeComponent();

            this.Hide();

            secureSandbox = new SecureSandbox();
            
        }

        private void TrayForm_Load(object sender, EventArgs e)
        {
            this.Hide();
            this.notifyIcon.Visible = true;
            secureSandbox.ShowDialog();
        }


        private void consoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!secureSandbox.Visible)
            {
                secureSandbox.StartPosition = FormStartPosition.CenterScreen;
                secureSandbox.ShowDialog();
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsForm settingForm = new SettingsForm();
            settingForm.StartPosition = FormStartPosition.CenterScreen;
            settingForm.ShowDialog();
        }



        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            GlobalConfig.Stop();
            secureSandbox.Close();

            Application.Exit();
        }

    
    }
}
