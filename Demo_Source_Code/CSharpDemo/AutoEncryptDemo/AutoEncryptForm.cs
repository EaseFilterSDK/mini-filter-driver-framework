using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using EaseFilter.CommonObjects;
using EaseFilter.FilterControl;

namespace AutoEncryptDemo
{
    public partial class AutoEncryptForm : Form
    {

        static FilterControl filterControl = new FilterControl();

        public AutoEncryptForm()
        {
            InitializeComponent();
        }

        ~AutoEncryptForm()
        {
            GlobalConfig.Stop();
            filterControl.StopFilter();
        }

        private void button_Start_Click(object sender, EventArgs e)
        {           

            //Purchase a license key with the link: http://www.easefilter.com/Order.htm
            //Email us to request a trial key: info@easefilter.com //free email is not accepted.        
            string licenseKey = GlobalConfig.LicenseKey;

            GlobalConfig.filterType = FilterAPI.FilterType.CONTROL_FILTER | FilterAPI.FilterType.ENCRYPTION_FILTER | FilterAPI.FilterType.PROCESS_FILTER;

            bool ret = false;

            string lastError = string.Empty;

            try
            {
                ret = filterControl.StartFilter(GlobalConfig.filterType, GlobalConfig.FilterConnectionThreads, GlobalConfig.ConnectionTimeOut, licenseKey, ref lastError);
                if (!ret)
                {
                    MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                    MessageBox.Show("StartFilter failed with error:" + lastError, "StartFilter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }               

                EventManager.WriteMessage(102, "StartFilter", EventLevel.Information, "Start filter service succeeded.");

                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("Start filter service succeeded.", "StartFilter", MessageBoxButtons.OK, MessageBoxIcon.Information);

                SendConfigToFilter();

                button_Stop.Enabled = true;
                button_Start.Enabled = false;

            }
            catch (Exception ex)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("Start filter service failed with error " + ex.Message, "StartFilter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                EventManager.WriteMessage(104, "StartFilter", EventLevel.Error, "Start filter service failed with error " + ex.Message);
            }
        }

        private bool SendConfigToFilter()
        {
            string lastError = string.Empty;

            string encryptFolder = textBox_EncrptFolder.Text.Trim();
            string authorizedProcessesForEncryptFolder = textBox_AuthorizedProcessesForEncryptFolder.Text;
            string authorizedUsersForEncryptFolder = textBox_AuthorizedUsersForEncryptFolder.Text;

            string decryptFolder = textBox_DecryptFolder.Text.Trim();
            string authorizedProcessesForDecryptFolder = textBox_AuthorizedProcessesForDecryptFolder.Text;

            string passPhrase = textBox_PassPhrase.Text.Trim();

            filterControl.ClearFilters();

            //setup a file filter rule for folder encryptFolder
            if (encryptFolder.IndexOf("*") < 0)
            {
                //set the file filter mask
                encryptFolder += "\\*";
            }

            FileFilter fileFilter = new FileFilter(encryptFolder);

            //enable the encryption for the filter rule.
            fileFilter.EnableEncryption = true;

            //if we enable the encryption key from service, you can authorize the encryption or decryption for every file
            //in the callback function OnFilterRequestEncryptKey.
            fileFilter.EnableEncryptionKeyFromService = checkBox_EnableEncryptionWithDRM.Checked;
            fileFilter.OnFilterRequestEncryptKey += OnFilterRequestEncryptKey;

            //get the 256bits encryption key with the passphrase
            fileFilter.EncryptionKey = Utils.GetKeyByPassPhrase(passPhrase, 32);

            //disable the decryption right, read the raw encrypted data for all except the authorized processes or users.
            fileFilter.EnableReadEncryptedData = false;

            if (textBox_AuthorizedProcessesForEncryptFolder.Text.Trim().Length == 0 || textBox_AuthorizedProcessesForEncryptFolder.Text.Trim().Equals("*"))
            {
                authorizedProcessesForEncryptFolder = "*";

                //allow everyone to read the encrypted data by default, except you remove it from the process access right.
                fileFilter.EnableReadEncryptedData = true;
            }
            else
            {
                string[] processNames = authorizedProcessesForEncryptFolder.Split(new char[] { ';' });

                if (processNames.Length > 0)
                {
                    foreach (string processName in processNames)
                    {
                        if (processName.Trim().Length > 0)
                        {
                            //authorized the process with the read encrypted data right.
                            fileFilter.ProcessNameAccessRightList.Add(processName, FilterAPI.ALLOW_MAX_RIGHT_ACCESS);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(authorizedUsersForEncryptFolder) && !authorizedUsersForEncryptFolder.Equals("*"))
                {
                    string[] userNames = authorizedUsersForEncryptFolder.Split(new char[] { ';' });
                    if (userNames.Length > 0)
                    {
                        foreach (string userName in userNames)
                        {
                            if (userName.Trim().Length > 0)
                            {
                                //authorized the user with the read encrypted data right.
                                fileFilter.userAccessRightList.Add(userName, FilterAPI.ALLOW_MAX_RIGHT_ACCESS);
                            }
                        }
                    }

                    if (fileFilter.userAccessRightList.Count > 0)
                    {
                        //set black list for all other users except the white list users.
                        uint accessFlag = FilterAPI.ALLOW_MAX_RIGHT_ACCESS & ~(uint)FilterAPI.AccessFlag.ALLOW_READ_ENCRYPTED_FILES;
                        //disable the decryption right, read the raw encrypted data for all except the authorized users.
                        fileFilter.userAccessRightList.Add("*", accessFlag);
                    }
                }
            }

            //add the encryption file filter rule to the filter control
            filterControl.AddFilter(fileFilter);

            //setup a file filter rule for folder decryptFolder
            if (decryptFolder.IndexOf("*") < 0)
            {
                //set the file filter mask
                decryptFolder += "\\*";
            }

            FileFilter decryptFileFilter = new FileFilter(decryptFolder);

            //enable the encryption for the filter rule.
            decryptFileFilter.EnableEncryption = true;

            //if we enable the encryption key from service, you can authorize the decryption for every file
            //in the callback function OnFilterRequestEncryptKey.
            decryptFileFilter.EnableEncryptionKeyFromService = checkBox_EnableEncryptionWithDRM.Checked;
            decryptFileFilter.OnFilterRequestEncryptKey += OnFilterRequestEncryptKey;

            //get the 256bits encryption key with the passphrase
            decryptFileFilter.EncryptionKey = Utils.GetKeyByPassPhrase(passPhrase, 32);

            //encrypt the new created file or modification in the folder.
            decryptFileFilter.EnableEncryptNewFile = true;

            //this is important if your process will copy the encrypted file to decryption folder, you need to exclude them.
            //Exclude Windows explorer.exe process, copy the encrypted file decryption folder with explorer will be excluded,
            //so the encrypted file won't be encrypted again.
            decryptFileFilter.ExcludeProcessNameList.Add("explorer.exe");

            //disable the decyrption right, read the raw encrypted data for all except the authorized processes or users.
            decryptFileFilter.EnableReadEncryptedData = false;

            string[] processNamesToDecrypt = authorizedProcessesForDecryptFolder.Split(new char[] { ';' });
            if (processNamesToDecrypt.Length > 0)
            {
                foreach (string processName in processNamesToDecrypt)
                {
                    if (processName.Trim().Length > 0)
                    {
                        //authorized the process with the read encrypted data right.
                        decryptFileFilter.ProcessNameAccessRightList.Add(processName, FilterAPI.ALLOW_MAX_RIGHT_ACCESS);
                    }
                }
            }

            filterControl.AddFilter(decryptFileFilter);

            if (!filterControl.SendConfigSettingsToFilter(ref lastError))
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("SendConfigSettingsToFilter failed." + lastError, "SendConfigSettingsToFilter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Fires this event when the encrypted file requests the encryption key.
        /// </summary>
        public void OnFilterRequestEncryptKey(object sender, EncryptEventArgs e)
        {
            e.ReturnStatus = NtStatus.Status.Success;

            try
            {
                if (e.IsNewCreatedFile)
                {                   
                    byte[] iv = Guid.NewGuid().ToByteArray();
                    //for the new created file, you can add your custom tag data to the header of the encyrpted file here.
                    //here we add the iv to the tag data.
                    e.EncryptionTag = iv;

                    //if you don't set the iv data, the filter driver will generate the new GUID as iv 
                    e.IV = iv;

                    //here is the encryption key for the new encrypted file, you can set it with your own custom key.
                    e.EncryptionKey = Utils.GetKeyByPassPhrase(GlobalConfig.MasterPassword, 32);

                    //if you want to block the new file creation, you can return access denied status.
                    //e.ReturnStatus = NtStatus.Status.AccessDenied;

                    //if you want to the file being created without encryption, return below status.
                    //e.ReturnStatus = NtStatus.Status.FileIsNoEncrypted;  

                   EventManager.WriteMessage(200, "EncryptNewFile", EventLevel.Information, "embedDRMToFile = " 
                        + DRMServer.embedDRMToFile.ToString() + " for file:" + e.FileName + ",return status:" + e.ReturnStatus.ToString());
                }
                else
                {
                    //this is the encrypted file open request, request the encryption key and iv.
                    //here is the tag data if you set custom tag data when the new created file requested the key.
                    byte[] tagData = e.EncryptionTag;
                    
                    if (!DRMServer.GetEncryptedFileAccessPermission(e))
                    {
                        //here didn't get the permission for the encrypted file open, it will return the raw encrypted data.
                        e.ReturnStatus = NtStatus.Status.FileIsEncrypted;
                    }

                    //The encryption key must be the same one which you created the new encrypted file.
                    e.EncryptionKey = Utils.GetKeyByPassPhrase(GlobalConfig.MasterPassword, 32);

                    //here is the iv key we saved in tag data.
                    e.IV = tagData;

                    //if you want to block encrypted file being opened, you can return accessdenied status.
                    //e.ReturnStatus = NtStatus.Status.AccessDenied;

                    //if you want to return the raw encrypted data for this encrypted file, return below status.
                    //e.ReturnStatus = NtStatus.Status.FileIsEncrypted;

                    EventManager.WriteMessage(250, "OpenEncryptedFile", EventLevel.Information, 
                        "OpenEncryptedFile:" + e.FileName + ",userName:" + e.UserName + ",processName:" + e.ProcessName + ",return status:" + e.ReturnStatus.ToString());

                }              
              
            }
            catch( Exception ex)
            {
                EventManager.WriteMessage(500, "OnFilterRequestEncryptKey", EventLevel.Error, "OnFilterRequestEncryptKey:" + e.FileName + ",got exeception:" + ex.Message);
                e.ReturnStatus = NtStatus.Status.AccessDenied;
            }

        }

        private void button_Stop_Click(object sender, EventArgs e)
        {
            button_Stop.Enabled = false;
            button_Start.Enabled = true;

            GlobalConfig.Stop();
            filterControl.StopFilter();

            EventManager.WriteMessage(102, "StopFilter", EventLevel.Information, "Stopped filter service succeeded.");

            MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
            MessageBox.Show("Stop filter service succeeded.", "StopFilter", MessageBoxButtons.OK, MessageBoxIcon.Information);

            return;
        }

        private void AutoEncryptForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            GlobalConfig.Stop();
            filterControl.StopFilter();
        }

        private void checkBox_EnableEncryptionWithDRM_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_EnableEncryptionWithDRM.Checked)
            {
                //To enable DRM encryption, the filter driver will always go to service callback function to get the encryption key,
                //so you can setup your own unique encryption key for every new encrypted file.

                textBox_PassPhrase.Enabled = false;
                textBox_AuthorizedProcessesForEncryptFolder.Enabled = false;
                textBox_AuthorizedUsersForEncryptFolder.Enabled = false;
                textBox_AuthorizedProcessesForDecryptFolder.Enabled = false;
                button_DRMSetting.Enabled = true;
            }
            else
            {
                button_DRMSetting.Enabled = false;
                textBox_PassPhrase.Enabled = true;
                textBox_AuthorizedProcessesForEncryptFolder.Enabled = true;
                textBox_AuthorizedUsersForEncryptFolder.Enabled = true;
                textBox_AuthorizedProcessesForDecryptFolder.Enabled = true;
            }
        }

        private void button_EncryptFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox_EncrptFolder.Text = folderDialog.SelectedPath + "\\*";
            }
        }

        private void button_DRMSetting_Click(object sender, EventArgs e)
        {
            DRMForm dRMForm = new DRMForm();
            if (dRMForm.ShowDialog() == System.Windows.Forms.DialogResult.OK && filterControl.IsFilterStarted )
            {
                SendConfigToFilter();
            }
        }
      

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.easefilter.com/kb/auto-file-drm-encryption-tool.htm");
        }

        private void applyTrialKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WebFormServices webForm = new WebFormServices();
            webForm.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;

            System.Threading.Tasks.Task.Factory.StartNew(() => { webForm.ShowDialog(); });
        }

        private void getTagDataOfEncryptedFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputForm inputForm = new InputForm("Input file name", "Plase input file name", "");

            if (inputForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fileName = inputForm.InputText;

                byte[] tagData = new Byte[FilterAPI.MAX_AES_TAG_SIZE];
                uint tagDataLength = (uint)tagData.Length;
                bool retVal = FilterAPI.GetAESTagData(fileName, ref tagDataLength, tagData);

                if (!retVal)
                {
                    MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                    MessageBox.Show("GetAESTagData failed with error:" + FilterAPI.GetLastErrorMessage(), "GetAESTagData", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("Get encrypted file " + fileName + " tag data succeeded. return tag data length:" + tagDataLength, "tagData", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
