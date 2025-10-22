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
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Diagnostics;


using EaseFilter.FilterControl;
using EaseFilter.CommonObjects;

namespace  SecureShare
{
    public partial class ShareFileManager : Form
    {

        public ShareFileManager()
        {
            InitializeComponent();
            InitListView();
            StartPosition = FormStartPosition.CenterScreen;

            string lastError = string.Empty;
            DRMServer.GetSharedFileList(GlobalConfig.AccountName, listView_ShareFileList, out lastError);
        }

       
        public void InitListView()
        {
            //init ListView control
            listView_ShareFileList.Clear();		//clear control
            //create column header for ListView
            listView_ShareFileList.Columns.Add("Id#", 40, System.Windows.Forms.HorizontalAlignment.Left);            
            listView_ShareFileList.Columns.Add("FileName", 200, System.Windows.Forms.HorizontalAlignment.Left);
            listView_ShareFileList.Columns.Add("CreationTime", 120, System.Windows.Forms.HorizontalAlignment.Left);
            listView_ShareFileList.Columns.Add("ExpireTime", 100, System.Windows.Forms.HorizontalAlignment.Left);
            listView_ShareFileList.Columns.Add("AuthorizedProcessNames", 200, System.Windows.Forms.HorizontalAlignment.Left);
            listView_ShareFileList.Columns.Add("UnauthorizedProcessNames", 200, System.Windows.Forms.HorizontalAlignment.Left);
            listView_ShareFileList.Columns.Add("AuthorizedUserNames", 200, System.Windows.Forms.HorizontalAlignment.Left);
            listView_ShareFileList.Columns.Add("UnauthorizedUserNames", 200, System.Windows.Forms.HorizontalAlignment.Left);

        }

        private void toolStripButton_CreateShareFile_Click(object sender, EventArgs e)
        {
            ShareFileForm shareFileForm = new ShareFileForm();
            shareFileForm.ShowDialog();
            
            string lastError = string.Empty;
            DRMServer.GetSharedFileList(GlobalConfig.AccountName, listView_ShareFileList, out lastError);
        }

        private void toolStripButton_GetSharedFiles_Click(object sender, EventArgs e)
        {
            string lastError = string.Empty;

            //get the shared file list from server.
            if (!DRMServer.GetSharedFileList(GlobalConfig.AccountName, listView_ShareFileList, out lastError))
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("Get shared file list failed." + lastError, "GetSharedFileList", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void toolStripButton_ModifySharedFile_Click(object sender, EventArgs e)
        {
            if (listView_ShareFileList.SelectedItems.Count != 1)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("Please select one shared file to edit.", "Edit shared file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            System.Windows.Forms.ListViewItem item = listView_ShareFileList.SelectedItems[0];
            DRMData dRMData = (DRMData)item.Tag;

            ShareFileForm shareFileForm = new ShareFileForm(dRMData);
            shareFileForm.ShowDialog();

            string lastError = string.Empty;
            DRMServer.GetSharedFileList(GlobalConfig.AccountName, listView_ShareFileList, out lastError);

        }

        private void toolStripButton_DeleteDRM_Click(object sender, EventArgs e)
        {
            if (listView_ShareFileList.SelectedItems.Count != 1)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("Please select one shared file to delete.", "Delete shared file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            System.Windows.Forms.ListViewItem item = listView_ShareFileList.SelectedItems[0];
            DRMData dRMData = (DRMData)item.Tag;

            string lastError = string.Empty;
            if(!DRMServer.DeleteDRMDataFromServer(dRMData.EncryptionIV, out lastError))
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("Delete shared file's DRM failed:" + lastError, "Delete shared file", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            DRMServer.GetSharedFileList(GlobalConfig.AccountName, listView_ShareFileList, out lastError);
        }     
       
    }
}
