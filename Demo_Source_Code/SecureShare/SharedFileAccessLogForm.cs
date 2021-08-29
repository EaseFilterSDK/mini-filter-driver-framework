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

namespace SecureShare
{
    public partial class SharedFileAccessLogForm : Form
    {
        public string accessLogStr = string.Empty;

        public SharedFileAccessLogForm()
        {
            this.StartPosition = FormStartPosition.CenterParent;
            InitializeComponent();

            toolStripButton_GetAccessLog_Click(null,null);

            InitListView();
        }

        public void InitListView()
        {
        
            listView_AccessLog.Clear();
            //create column header for ListView            
            listView_AccessLog.Columns.Add("Status", 80, System.Windows.Forms.HorizontalAlignment.Left);
            listView_AccessLog.Columns.Add("AccessTime",160, System.Windows.Forms.HorizontalAlignment.Left);            
            listView_AccessLog.Columns.Add("UserName",100, System.Windows.Forms.HorizontalAlignment.Left);
            listView_AccessLog.Columns.Add("ProcessName",100, System.Windows.Forms.HorizontalAlignment.Left);
            listView_AccessLog.Columns.Add("FileName",150, System.Windows.Forms.HorizontalAlignment.Left);
            listView_AccessLog.Columns.Add("Location", 200, System.Windows.Forms.HorizontalAlignment.Left);
            listView_AccessLog.Columns.Add("Description", 350, System.Windows.Forms.HorizontalAlignment.Left);

            try
            {
                if( accessLogStr.Length == 0 )
                {
                    return;
                }

                string accessLog = Utils.AESEncryptDecryptStr(accessLogStr, Utils.EncryptType.Decryption);
                StringReader sr = new StringReader(accessLog);

                while (true)
                {
                    string accessInfo = sr.ReadLine().Trim();
                    if (string.IsNullOrEmpty(accessInfo))
                    {
                        break;
                    }

                    try
                    {

                        string[] infos = accessInfo.Split(new char[] { '|' });

                        if (infos.Length < 7)
                        {
                            continue;
                        }

                        ListViewItem lvItem = new ListViewItem();
                        string[] listData = new string[listView_AccessLog.Columns.Count];

                        if (infos[0].Trim() == "1")
                        {
                            listData[0] = "Authorized";
                        }
                        else
                        {
                            listData[0] = "Denied";
                            listData[6] = infos[6].Trim();
                        }

                        long accessTimeL = 0;

                        if (long.TryParse(infos[1].Trim(), out accessTimeL))
                        {
                            DateTime accessTime = DateTime.FromFileTime(accessTimeL);
                            listData[1] = accessTime.ToString("F");
                        }

                        listData[2] = infos[2].Trim();
                        listData[3] = infos[3].Trim();
                        listData[4] = infos[4].Trim();
                        listData[5] = infos[5].Trim();

                        lvItem = new ListViewItem(listData, 0);

                        if (infos[0].Trim() == "0")
                        {
                            lvItem.BackColor = Color.LightGray;
                            lvItem.ForeColor = Color.Red;
                        }
                      

                        listView_AccessLog.Items.Add(lvItem);
                    }
                    catch
                    {
                    }
                }

            }
            catch
            {
            }
        }

        private void AccessLogForm_Shown(object sender, EventArgs e)
        {
            InitListView();
        }

 
        private void toolStripButton_GetAccessLog_Click(object sender, EventArgs e)
        {
            string logStr = string.Empty;
            string lastError = string.Empty;

            bool retVal = WebAPIServices.GetAccessLog( ref logStr, ref lastError);

            if (retVal)
            {
                accessLogStr = logStr;
                InitListView();
            }
          
        }

        private void toolStripButton_ClearMessage_Click(object sender, EventArgs e)
        {
            string lastError = string.Empty;

            bool retVal = WebAPIServices.ClearAccessLog(ref lastError);

            if (retVal)
            {
                accessLogStr = string.Empty;
                InitListView();
            }
        }

     
    }
}
