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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using EaseFilter.FilterControl;
using EaseFilter.CommonObjects;

namespace AutoEncryptDemo
{
     
    public partial class DRMForm : Form
    {
      
        public DRMForm()
        {
            InitializeComponent();

            dateTimePicker_ExpireDate.Value = DateTime.Now.AddDays(1);

            if (null != DRMServer.dRMInfo)
            {
                dateTimePicker_ExpireDate.Value = DateTime.FromFileTime(DRMServer.dRMInfo.ExpireTime);
                dateTimePicker_ExpireTime.Value = DateTime.FromFileTime(DRMServer.dRMInfo.ExpireTime);
                textBox_authorizedProcessNames.Text = DRMServer.dRMInfo.AuthorizedProcessNames;
                textBox_UnauthorizedProcessNames.Text = DRMServer.dRMInfo.UnauthorizedProcessNames;
                textBox_AuthorizedUserNames.Text = DRMServer.dRMInfo.AuthorizedUserNames;
                textBox_UnauthorizedUserNames.Text = DRMServer.dRMInfo.UnauthorizedUserNames;
                textBox_ComputerId.Text = DRMServer.dRMInfo.AuthorizedComputerIds;

                radioButton_EmbedDRM.Checked = DRMServer.embedDRMToFile;
                radioButton_StoreDRMToServer.Checked = !DRMServer.embedDRMToFile;
            }
          
        }
      
        private void button_ApplySettings_Click(object sender, EventArgs e)
        {
            try
            {
                DRMInfo dRMInfo = new DRMInfo();
                DRMServer.embedDRMToFile = radioButton_EmbedDRM.Checked;
                dRMInfo.AuthorizedProcessNames = textBox_authorizedProcessNames.Text.Trim().ToLower();
                dRMInfo.UnauthorizedProcessNames = textBox_UnauthorizedProcessNames.Text.Trim().ToLower();
                dRMInfo.AuthorizedUserNames = textBox_AuthorizedUserNames.Text.Trim().ToLower();
                dRMInfo.UnauthorizedUserNames = textBox_UnauthorizedUserNames.Text.Trim().ToLower();
                dRMInfo.AuthorizedComputerIds = textBox_ComputerId.Text;
                DateTime expireDate = dateTimePicker_ExpireDate.Value.Date + dateTimePicker_ExpireTime.Value.TimeOfDay;
                dRMInfo.ExpireTime = expireDate.ToFileTime();
                dRMInfo.AuthorizedComputerIds = textBox_ComputerId.Text;

                DRMServer.SetDRMInfo(dRMInfo);
            }
            catch (Exception ex)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("Apply setting got exception:" + ex.Message, "apply settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        }

        private void button_GetComputerId_Click(object sender, EventArgs e)
        {
            textBox_ComputerId.Text = DRMServer.GetComputerId();
        }
    }
}
