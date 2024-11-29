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

using EaseFilter.FilterControl;
using EaseFilter.CommonObjects;

namespace  SecureShare
{
    public partial class ShareFileManagerInLocal : Form
    {
       

        public ShareFileManagerInLocal()
        {
            InitializeComponent();
            InitListView();
            StartPosition = FormStartPosition.CenterParent;
        }

        public void InitListView()
        {
            listView_SharedFiles.Clear();
            //create column header for ListView
            listView_SharedFiles.Columns.Add("fileName", 250, System.Windows.Forms.HorizontalAlignment.Left);
            listView_SharedFiles.Columns.Add("creationTime",200, System.Windows.Forms.HorizontalAlignment.Left);
            listView_SharedFiles.Columns.Add("expireTime", 200, System.Windows.Forms.HorizontalAlignment.Left);
            listView_SharedFiles.Columns.Add("authorizedProcessNames", 200, System.Windows.Forms.HorizontalAlignment.Left);
            listView_SharedFiles.Columns.Add("unauthorizedProcessNames", 200, System.Windows.Forms.HorizontalAlignment.Left);
            listView_SharedFiles.Columns.Add("authorizedUserNames", 200, System.Windows.Forms.HorizontalAlignment.Left);
            listView_SharedFiles.Columns.Add("unauthorizedUserNames", 200, System.Windows.Forms.HorizontalAlignment.Left);
            listView_SharedFiles.Columns.Add("accessFlags", 200, System.Windows.Forms.HorizontalAlignment.Left);

            try
            {
                string[] sharedFiles = Directory.GetFiles(GlobalConfig.DRInfoFolder);

                foreach (string sharedFile in sharedFiles)
                {
                    string fileName = string.Empty;
                    string expireTime = string.Empty;
                    string creationTime = string.Empty;
                    string authorizedProcessNames = string.Empty;
                    string unauthorizedProcessNames = string.Empty;
                    string authorizedUserNames = string.Empty;
                    string unauthorizedUserNames = string.Empty;
                    string accessFlags = string.Empty;
                    DateTime currentTime = DateTime.Now;

                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    Utils.LoadAppSetting(sharedFile, ref keyValues);

                    keyValues.TryGetValue("fileName", out fileName);
                    keyValues.TryGetValue("creationTime", out creationTime);
                    keyValues.TryGetValue("expireTime", out expireTime);
                    keyValues.TryGetValue("authorizedProcessNames", out authorizedProcessNames);
                    keyValues.TryGetValue("unauthorizedProcessNames", out unauthorizedProcessNames);
                    keyValues.TryGetValue("authorizedUserNames", out authorizedUserNames);
                    keyValues.TryGetValue("unauthorizedUserNames", out unauthorizedUserNames);
                    keyValues.TryGetValue("accessFlags", out accessFlags);


                    ListViewItem lvItem = new ListViewItem();
                    string[] listData = new string[listView_SharedFiles.Columns.Count];
                    listData[0] = fileName;
                    long dateTimeN = long.Parse(creationTime);
                    DateTime dateTimeD = DateTime.FromFileTime(dateTimeN).ToLocalTime();
                    listData[1] = String.Format("{0:F}", dateTimeD);
                    dateTimeN = long.Parse(expireTime);
                    dateTimeD = DateTime.FromFileTime(dateTimeN).ToLocalTime();
                    listData[2] = String.Format("{0:F}", dateTimeD);
                    listData[3] = authorizedProcessNames;
                    listData[4] = unauthorizedProcessNames;
                    listData[5] = authorizedUserNames;
                    listData[6] = unauthorizedUserNames;
                    listData[7] = accessFlags;

                    lvItem = new ListViewItem(listData, 0);
                    lvItem.Tag = sharedFile;

                    listView_SharedFiles.Items.Add(lvItem);

                }
            }
            catch
            {
            }
        }

        private void button_CreateShareEncryptedFile_Click(object sender, EventArgs e)
        {
            ShareFileForm shareFileForm = new ShareFileForm();
            shareFileForm.ShowDialog();
            InitListView();
        }

        private void button_Delete_Click(object sender, EventArgs e)
        {
            button_Delete.Enabled = false;

            try
            {
                if (listView_SharedFiles.SelectedItems.Count != 1)
                {
                    MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                    MessageBox.Show("Please select a file.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string drFileName = (string)listView_SharedFiles.SelectedItems[0].Tag;

                File.Delete(drFileName);

                InitListView();

            }
            catch (Exception ex)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("Delete shared file failed with error " + ex.Message, "DeleteSharedFile", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                button_Delete.Enabled = true;
            }
        }


    }
}
