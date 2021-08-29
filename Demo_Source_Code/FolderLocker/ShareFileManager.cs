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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using EaseFilter.FilterControl;
using EaseFilter.CommonObjects;

namespace EaseFilter.FolderLocker
{
    public partial class Form_FolderLocker :Form
    {
        DRPolicy selectedDRPolicy = new DRPolicy();
        public Dictionary<string, DRPolicy> sharedFileList = new Dictionary<string,DRPolicy>();
        

        public void InitShareFileListView()
        {
            textBox_SharedFileDropFolder.Text = GlobalConfig.ShareFolder;

            listView_SharedFiles.Clear();
            //create column header for ListView
            listView_SharedFiles.Columns.Add("FileName", 150, System.Windows.Forms.HorizontalAlignment.Left);
            listView_SharedFiles.Columns.Add("CreationTime", 100, System.Windows.Forms.HorizontalAlignment.Left);
            listView_SharedFiles.Columns.Add("ExpireTime", 100, System.Windows.Forms.HorizontalAlignment.Left);
            listView_SharedFiles.Columns.Add("AuthorizedProcessNames", 100, System.Windows.Forms.HorizontalAlignment.Left);
            listView_SharedFiles.Columns.Add("UnauthorizedProcessNames", 100, System.Windows.Forms.HorizontalAlignment.Left);
            listView_SharedFiles.Columns.Add("AuthorizedUserNames", 100, System.Windows.Forms.HorizontalAlignment.Left);
            listView_SharedFiles.Columns.Add("UnauthorizedUserNames", 100, System.Windows.Forms.HorizontalAlignment.Left);

            try
            {
                foreach (DRPolicy sharedFile in sharedFileList.Values)
                {
                    int index = 0;
                    ListViewItem lvItem = new ListViewItem();
                    string[] listEntry = new string[listView_SharedFiles.Columns.Count];

                    string fileName = sharedFile.FileName;

                    if (fileName.EndsWith(GlobalConfig.ShareFileExt))
                    {
                        fileName = fileName.Replace(GlobalConfig.ShareFileExt, "");
                    }

                    listEntry[index++] = fileName;
                    long dateTimeN = sharedFile.CreationTime;
                    DateTime dateTimeD = DateTime.FromFileTime(dateTimeN);
                    listEntry[index++] = dateTimeD.ToShortDateString();// String.Format("{0:F}", dateTimeD);
                    dateTimeN = sharedFile.ExpireTime;
                    dateTimeD = DateTime.FromFileTime(dateTimeN);
                    listEntry[index++] = dateTimeD.ToShortDateString();//String.Format("{0:F}", dateTimeD);
                    listEntry[index++] = sharedFile.AuthorizedProcessNames;
                    listEntry[index++] = sharedFile.UnauthorizedProcessNames;
                    listEntry[index++] = sharedFile.AuthorizedUserNames;
                    listEntry[index++] = sharedFile.UnauthorizedUserNames;

                    lvItem = new ListViewItem(listEntry, 0);
                    lvItem.Tag = sharedFile;

                    int insertIndex = 0;
                    if (listView_SharedFiles.Items.Count > 0)
                    {
                        for (insertIndex = 0; insertIndex < listView_SharedFiles.Items.Count; insertIndex++)
                        {
                            DRPolicy sharedFileInList = (DRPolicy)(listView_SharedFiles.Items[insertIndex]).Tag;

                            if (sharedFile.CreationTime < sharedFileInList.CreationTime)
                            {
                                break;
                            }
                        }

                    }

                    listView_SharedFiles.Items.Insert(insertIndex, lvItem);

                }
            }
            catch
            {
            }
        }

        public bool GetSharedFileList()
        {
            string lastError = string.Empty;
            string encryptFileList = string.Empty;

            sharedFileList.Clear();

            bool retVal = WebAPIServices.GetFileList(ref encryptFileList, ref lastError);
            if (!retVal)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show(lastError, "GetFileList", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }
            else
            {
                if (encryptFileList.Length > 0)
                {
                    List<DRPolicy> decrypFileList = Utils.DecryptStrToObject<List<DRPolicy>>(encryptFileList);
                    sharedFileList.Clear();

                    foreach (DRPolicy drPolicy in decrypFileList)
                    {
                        sharedFileList.Add(drPolicy.EncryptionIV, drPolicy);
                    }
                }

                InitShareFileListView();

            }

            return true;
        }

   
        private void toolStripButton_CreateShareFile_Click(object sender, EventArgs e)
        {
            ShareFileForm shareFileForm = new ShareFileForm();

            if (shareFileForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                GetSharedFileList();
            }

        }


        private void toolStripButton_RemoveShareFile_Click(object sender, EventArgs e)
        {
            if (listView_SharedFiles.SelectedItems.Count != 1)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("Please select a file.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DRPolicy drPolicy = (DRPolicy)listView_SharedFiles.SelectedItems[0].Tag;
            string lastError = string.Empty;

            if (!WebAPIServices.DeleteShareFile(drPolicy.EncryptionIV, ref lastError))
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("Delete shared file " + selectedDRPolicy.FileName + " failed with error:" + lastError, "DeleteSharedFile", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            GetSharedFileList();
        }

        private void toolStripButton_ModifyShareFile_Click(object sender, EventArgs e)
        {
            if (listView_SharedFiles.SelectedItems.Count != 1)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("Please select a file.", "Edit", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DRPolicy drPolicy = (DRPolicy)listView_SharedFiles.SelectedItems[0].Tag;

            ShareFileForm shareFileForm = new ShareFileForm(drPolicy);
            shareFileForm.Text = "EaseFilter Shared File Modification";

            if (shareFileForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                GetSharedFileList();
                InitShareFileListView();
            }
        }

        private void toolStripButton_AccessLog_Click(object sender, EventArgs e)
        {
            SharedFileAccessLogForm accessLog = new SharedFileAccessLogForm();
            accessLog.ShowDialog();

        }


        public bool RefreshSharedFilesInClient()
        {
            string lastError = string.Empty;
            string encryptFileList = string.Empty;

            sharedFileList.Clear();

            bool retVal = WebAPIServices.GetFileList(ref encryptFileList, ref lastError);
            if (!retVal)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show(lastError, "GetFileList", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }
            else
            {
                if (encryptFileList.Length > 0)
                {
                    List<DRPolicy> decrypFileList = Utils.DecryptStrToObject<List<DRPolicy>>(encryptFileList);
                    sharedFileList.Clear();

                    foreach (DRPolicy drPolicy in decrypFileList)
                    {
                        sharedFileList.Add(drPolicy.EncryptionIV, drPolicy);
                    }
                }

                InitShareFileListView();

            }

            return true;
        }

        private void button_SaveDropFolder_Click(object sender, EventArgs e)
        {
            if (textBox_SharedFileDropFolder.Text.Length > 0)
            {
                GlobalConfig.ShareFolder = textBox_SharedFileDropFolder.Text;
            }
        }


        private void toolStripButton_RefreshList_Click(object sender, EventArgs e)
        {
            RefreshSharedFilesInClient();
        }

     

    }
}
