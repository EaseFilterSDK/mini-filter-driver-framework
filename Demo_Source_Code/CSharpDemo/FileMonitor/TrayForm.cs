﻿///////////////////////////////////////////////////////////////////////////////
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
using EaseFilter.FilterControl;

namespace FileMonitor
{
    public partial class TrayForm : Form
    {
        MonitorForm monitorForm = new MonitorForm();

        public TrayForm()
        {
            InitializeComponent();

        }

        private void TrayForm_Load(object sender, EventArgs e)
        {
            this.Hide();
            this.notifyIcon.Visible = true;
            monitorForm.ShowDialog();
        }

        private void consoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!monitorForm.Visible)
            {
                monitorForm.StartPosition = FormStartPosition.CenterScreen;
                monitorForm.ShowDialog();
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsForm settingForm = new SettingsForm();
            settingForm.StartPosition = FormStartPosition.CenterScreen;
            settingForm.ShowDialog();
        }

        private void helpTopicsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.easefilter.com/Forums_Files/FileMonitor.htm");
        }

        private void reportAProblemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.easefilter.com/ReportIssue.htm");
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            GlobalConfig.Stop();
            monitorForm.Close();

            Application.Exit();
        }

        private void sdkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.easefilter.com/info/easefilter_manual.pdf");
        }

        private void openSourceCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetEntryAssembly();
            string AssemblyPath = Path.Combine(Path.GetDirectoryName(assembly.Location), "Demo");
            System.Diagnostics.Process.Start("explorer.exe", AssemblyPath);
        }

        private void uninstallDriverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FilterAPI.UnInstallDriver();
        }


    }
}
