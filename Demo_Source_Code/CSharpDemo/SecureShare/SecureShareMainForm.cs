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
    public partial class SecureShareMainForm : Form
    {
       
        FilterControl filterControl = new FilterControl();

        public SecureShareMainForm()
        {

            GlobalConfig.filterType = FilterAPI.FilterType.CONTROL_FILTER | FilterAPI.FilterType.ENCRYPTION_FILTER | FilterAPI.FilterType.PROCESS_FILTER;

            InitializeComponent();

            InitListView();

            StartPosition = FormStartPosition.CenterScreen;

            DisplayVersion();           

        }

        private void DisplayVersion()
        {
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            try
            {
                string filterDllPath = Path.Combine(GlobalConfig.AssemblyPath, "FilterAPI.Dll");
                version = FileVersionInfo.GetVersionInfo(filterDllPath).ProductVersion;
            }
            catch (Exception ex)
            {
                EventManager.WriteMessage(43, "LoadFilterAPI Dll", EventLevel.Error, "FilterAPI.dll can't be found." + ex.Message);
            }

            this.Text += "    Version:  " + version;
        }     

        ~SecureShareMainForm()
        {
            GlobalConfig.Stop();
        }

        private void MonitorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            GlobalConfig.Stop();
        }

        /// <summary>
        /// Fires this event after the new file was created, the handle is not closed.
        /// </summary>
        void OnFilterRequestEncryptKey(object sender, EncryptEventArgs e)
        {
            lock (filterControl)
            {
                string[] items = new string[listView_Message.Columns.Count];
                int i = 0;
                items[i++] = listView_Message.Items.Count.ToString();
                items[i++] = DateTime.Now.ToLongTimeString();
                items[i++] = e.UserName;
                items[i++] = e.ProcessName + "(" + e.ProcessId + ")";
                if (e.IsNewCreatedFile)
                {
                    items[i++] = "Encrypt new file";
                }
                else
                {
                    items[i++] = "Decrypt file";
                }

                items[i++] = e.FileName;

                if (e.IsNewCreatedFile)
                {
                    string authorizedProcessNames = GlobalConfig.AuthorizedProcessNames;
                    string unauthorizedProcessNames = GlobalConfig.UnAuthorizedProcessNames;
                    string authorizedUserNames = GlobalConfig.AuthorizedUserNames;
                    string unauthorizedUserNames = GlobalConfig.UnAuthorizedUserNames;
                    string authorizedComputerIds = string.Empty;
                    string authorizedIps = string.Empty;
                    string fileName = e.FileName;
                    uint accessFlags = FilterAPI.ALLOW_MAX_RIGHT_ACCESS;

                    long expireDateTime = GlobalConfig.ShareFileExpireTime;

                    //here we generate the random unique IV and key, you can use your own key and iv
                    byte[] encryptionIV = Utils.GetRandomIV();
                    byte[] encryptionKey = Utils.GetRandomKey();
                    string encryptionIVStr = Utils.ByteArrayToHexStr(encryptionIV);
                    string encryptionKeyStr = Utils.ByteArrayToHexStr(encryptionKey);

                    string lastError = string.Empty;

                    bool retVal = DRMServer.AddDRMDataToServer(fileName, authorizedProcessNames, unauthorizedProcessNames, authorizedUserNames, unauthorizedUserNames, authorizedComputerIds
                       , authorizedIps, expireDateTime, encryptionIVStr, encryptionKeyStr, accessFlags, out lastError);

                    if (retVal)
                    {
                        string tagDataStr = GlobalConfig.AccountName + ";" + RegisterForm.GetUniqueComputerId().ToString() + ";" + encryptionIVStr;
                        byte[] tagData = ASCIIEncoding.ASCII.GetBytes(tagDataStr);
                        e.EncryptionTag = tagData;

                        e.IV = encryptionIV;
                        //here is the encryption key for the new encrypted file.
                        e.EncryptionKey = encryptionKey;

                        e.ReturnStatus = NtStatus.Status.Success;

                        EventManager.WriteMessage(110, "CreateNewEncryptedFile", EventLevel.Information
                            , "New created file: " + e.FileName + " was encrypted.The process name: " + e.ProcessName + ", user name: " + e.UserName + ",tagDataStr:" + tagDataStr + ",encryptionKeyStr:" + encryptionKeyStr);
                    }
                    else
                    {
                        e.ReturnStatus = NtStatus.Status.AccessDenied;

                        EventManager.WriteMessage(120, "CreateNewEncryptedFile-AddDRMDataToServer", EventLevel.Error, "AddDRMDataToServer: " + e.FileName + " failed with error " + lastError);
                    }

                    items[i++] = e.ReturnStatus.ToString();
                    items[i++] = lastError;
                }
                else
                {
                    string lastError = string.Empty;

                    if (!DRMServer.GetFilePermission(e, ref lastError))
                    {
                        //here didn't get the permission for the encrypted file open, it will return the raw encrypted data.
                        e.ReturnStatus = NtStatus.Status.FileIsEncrypted;

                        EventManager.WriteMessage(130, "GetFilePermission", EventLevel.Error, "GetFilePermission: " + e.FileName + " failed with AccessDenied");
                    }

                    items[i++] = e.ReturnStatus.ToString();
                    items[i++] = lastError;

                }

                ListViewItem lvItem = new ListViewItem(items, 0);

                if ((uint)e.ReturnStatus >= (uint)NtStatus.Status.Error)
                {
                    lvItem.BackColor = System.Drawing.Color.LightGray;
                    lvItem.ForeColor = System.Drawing.Color.Red;
                }
                else if ((uint)e.IoStatus > (uint)NtStatus.Status.Warning)
                {
                    lvItem.BackColor = System.Drawing.Color.LightGray;
                    lvItem.ForeColor = System.Drawing.Color.Yellow;
                }
              
                listView_Message.Items.Add(lvItem);
            }

        }

        void SendSettingsToFilter()
        {
            filterControl.ClearFilters();

            string[] whiteList = null;
            string[] blacklist = null;

            //Set up the auto encryption filter rule, the new created files in this folder will be encrypted automatically.
            //Only the authorized processes or users can read the encrypted files,  
            FileFilter autoEncryptFilter = new FileFilter(GlobalConfig.AutoEncryptFolder + "\\*");
            //enable the auto encryption for this filter rule.
            autoEncryptFilter.EnableEncryption = true;
            // all process can't read the encyrpted file except the authorized processes.
          
            //autoEncryptFilter.EnableReadEncryptedData = false;
            //get encryption key and iv from the server.
            autoEncryptFilter.EnableEncryptionKeyFromService = true;

            //if we enable the encryption key from service, you can authorize the encryption or decryption for every file
            //in the callback function OnFilterRequestEncryptKey.
            autoEncryptFilter.OnFilterRequestEncryptKey += OnFilterRequestEncryptKey;

            //get the 256bits encryption key with the passphrase
            autoEncryptFilter.EncryptionKey = Utils.GetKeyByPassPhrase("passPhrase", 32);

            //disable the decryption right, read the raw encrypted data for all except the authorized processes or users.
            autoEncryptFilter.EnableReadEncryptedData = false;

            ////for whitelist process, it has maximum acess rights.
            whiteList = GlobalConfig.AuthorizedProcessNames.Split(new char[] { ';' });
            if (whiteList.Length > 0)
            {
                foreach (string authorizedProcess in whiteList)
                {
                    if (authorizedProcess.Trim().Length > 0)
                    {
                        autoEncryptFilter.AddTrustedProcessRight(FilterAPI.ALLOW_MAX_RIGHT_ACCESS, authorizedProcess, "", "");
                    }
                }
            }

            //for blacklist process, it has maximum acess rights.
            blacklist = GlobalConfig.UnAuthorizedProcessNames.Split(new char[] { ';' });
            if (blacklist.Length > 0)
            {
                foreach (string unAuthorizedProcess in blacklist)
                {
                    if (unAuthorizedProcess.Trim().Length > 0)
                    {
                        //can't read the encrypted files
                        uint accessFlag = FilterAPI.ALLOW_MAX_RIGHT_ACCESS & (uint)(~FilterAPI.AccessFlag.ALLOW_READ_ENCRYPTED_FILES);
                        autoEncryptFilter.AddTrustedProcessRight(accessFlag, unAuthorizedProcess, "", "");

                    }
                }
            }

            //
            //here is the decryption folder setting for the recipient-side's client machine.
            //
            FileFilter decryptionFilter = new FileFilter(GlobalConfig.DropFolder + "\\*");
            //enable the auto encryption for this filter rule.
            decryptionFilter.EnableEncryption = true;
            // all process can't read the encyrpted file except the authorized processes.
            decryptionFilter.EnableReadEncryptedData = false;
            // the new created file will be encrypted automatically.
            decryptionFilter.EnableEncryptNewFile = true;
            //exclude the explorer to encrypt or decrypt the files.
            decryptionFilter.ExcludeProcessNameList.Add("explorer.exe");
            //get encryption key and iv from the server.
            decryptionFilter.EnableEncryptionKeyFromService = true;
         
            decryptionFilter.OnFilterRequestEncryptKey += OnFilterRequestEncryptKey;


            //add the filter rules to the filter driver.
            filterControl.AddFilter(autoEncryptFilter);
            filterControl.AddFilter(decryptionFilter);

         
            string lastError = string.Empty;
            if (!filterControl.SendConfigSettingsToFilter(ref lastError))
            {
                MessageBox.Show(lastError, "StartFilter", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        private void toolStripButton_StartFilter_Click(object sender, EventArgs e)
        {
            try
            {
                //Purchase a license key with the link: http://www.easefilter.com/Order.htm
                //Email us to request a trial key: info@easefilter.com //free email is not accepted.        
                string licenseKey = GlobalConfig.LicenseKey;

                string lastError = string.Empty;

                bool ret = filterControl.StartFilter(GlobalConfig.filterType, GlobalConfig.FilterConnectionThreads, GlobalConfig.ConnectionTimeOut, licenseKey, ref lastError);
                if (!ret)
                {
                    MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                    MessageBox.Show("Start filter failed." + lastError, "StartFilter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                SendSettingsToFilter();

                toolStripButton_StartFilter.Enabled = false;
                toolStripButton_Stop.Enabled = true;

                EventManager.WriteMessage(102, "StartFilter", EventLevel.Information, "Start filter service succeeded.");
            }
            catch (Exception ex)
            {
                EventManager.WriteMessage(104, "StartFilter", EventLevel.Error, "Start filter service failed with error " + ex.Message);
            }

        }

        private void toolStripButton_Stop_Click(object sender, EventArgs e)
        {
            filterControl.StopFilter();

            toolStripButton_StartFilter.Enabled = true;
            toolStripButton_Stop.Enabled = false;
        }
    
       
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShareFileSettingForm settingForm = new ShareFileSettingForm();
            settingForm.StartPosition = FormStartPosition.CenterParent;
            settingForm.ShowDialog();
        }


        private void ConsoleForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            FilterAPI.ResetConfigData();
            GlobalConfig.Stop();
            filterControl.StopFilter();
            Application.Exit();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FilterAPI.ResetConfigData();
            GlobalConfig.Stop();
            filterControl.StopFilter();
            Close();
        }


        public void InitListView()
        {
            //init ListView control
            listView_Message.Clear();		//clear control
            //create column header for ListView
            listView_Message.Columns.Add("Id#", 40, System.Windows.Forms.HorizontalAlignment.Left);
            listView_Message.Columns.Add("Time", 80, System.Windows.Forms.HorizontalAlignment.Left);
            listView_Message.Columns.Add("UserName", 130, System.Windows.Forms.HorizontalAlignment.Left);
            listView_Message.Columns.Add("ProcessName(PID)", 200, System.Windows.Forms.HorizontalAlignment.Left);
            listView_Message.Columns.Add("EventName", 100, System.Windows.Forms.HorizontalAlignment.Left);
            listView_Message.Columns.Add("FileName", 300, System.Windows.Forms.HorizontalAlignment.Left);
            listView_Message.Columns.Add("IO Status", 100, System.Windows.Forms.HorizontalAlignment.Left);
            listView_Message.Columns.Add("Description", 600, System.Windows.Forms.HorizontalAlignment.Left);
        }

        private void toolStripButton_ShareFileManager_Click(object sender, EventArgs e)
        {
            ShareFileManager shareFileManager = new ShareFileManager();
            shareFileManager.ShowDialog();
        }

        private void toolStripButton_GetSharedFileAccessLog_Click(object sender, EventArgs e)
        {
            string licenseKey = GlobalConfig.LicenseKey;

            AccessLogForm accessLogForm = new AccessLogForm();
            if (accessLogForm.GetAccessLog())
            {
                accessLogForm.ShowDialog();
            }
            
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
            string helpInfo = "1. To create the encrypted share file, you have two options: a. Create the encrypted file manually. b. Create the encrypted file in auto encryption folder by setting.\n\n";
            helpInfo += "2. Settings: you can setup the auto encryption folder with the DRM data. The shared file drop folder is the folder in recipient-client's computer to receive the shared files.\n\n";
            helpInfo += "3. Start Service: enable the file encryption or decryption automatically.\n\n";
            helpInfo += "4. Create share file: create the secure share file manually.\n\n";
            helpInfo += "5. Modify shared file DRM: modify the shared file's DRM data after the encrypted shared file was created.\n\n";
            helpInfo += "6. Delete shared file DRM: delete the shared file's DRM data after the encrypted shared file was created.\n\n";
            helpInfo += "7. Access log: get the shared file access log to know who and when the shared files were accessed.\r\n";
            helpInfo += "8. Share files: you can get the shared file list if the shared files DRM data was stored in the server.\n\n";

            MessageBox.Show(helpInfo, "How to use this application?", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void toolStripButton_ApplyTrialKey_Click(object sender, EventArgs e)
        {
            RegisterForm webForm = new RegisterForm();
            webForm.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;

            System.Threading.Tasks.Task.Factory.StartNew(() => { webForm.ShowDialog(); });
        }

        private void toolStripButton_ClearMessage_Click(object sender, EventArgs e)
        {
            listView_Message.Items.Clear();
        }
    }
}
