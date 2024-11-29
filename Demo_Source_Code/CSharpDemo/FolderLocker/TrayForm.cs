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

namespace EaseFilter.FolderLocker
{
    public partial class TrayForm : Form
    {
        Form_FolderLocker folderLockerForm = null;

        public TrayForm()
        {
              
            InitializeComponent();

            Utils.CopyOSPlatformDependentFiles();

          
            this.Hide();

            folderLockerForm = new Form_FolderLocker();

        }

        private void TrayForm_Load(object sender, EventArgs e)
        {
            this.Hide();
            this.notifyIcon.Visible = true;
            folderLockerForm.ShowDialog();
        }


        private void openConsoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!folderLockerForm.Visible)
            {
                folderLockerForm.StartPosition = FormStartPosition.CenterScreen;
                folderLockerForm.ShowDialog();
            }
        }
      
   
        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            GlobalConfig.Stop();
            folderLockerForm.Close();

            Application.Exit();
        }

        private void startLockertoolStripMenuItem_Click(object sender, EventArgs e)
        {
            string lastError = string.Empty;
            if (!FilterWorker.StartService(ref lastError))
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("Start service failed with error:" + lastError + ",folder locker service will stop.", "Folder locker Service", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void stopLockertoolStripMenuItem_Click(object sender, EventArgs e)
        {
            GlobalConfig.Stop();
            FilterAPI.StopFilter();
        }



      
    
    }
}
