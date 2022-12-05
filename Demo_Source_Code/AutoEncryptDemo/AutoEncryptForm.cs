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
        
            string encryptFolder = textBox_EncrptFolder.Text.Trim();
            string authorizedProcessesForEncryptFolder = textBox_AuthorizedProcessesForEncryptFolder.Text;
            string authorizedUsersForEncryptFolder = textBox_AuthorizedUsersForEncryptFolder.Text;

            string decryptFolder = textBox_DecryptFolder.Text.Trim();
            string authorizedProcessesForDecryptFolder = textBox_AuthorizedProcessesForDecryptFolder.Text;

            string passPhrase = textBox_PassPhrase.Text.Trim();

            //Purchase a license key with the link: http://www.easefilter.com/Order.htm
            //Email us to request a trial key: info@easefilter.com //free email is not accepted.
            string licenseKey = "******************************************";

            GlobalConfig.filterType = FilterAPI.FilterType.CONTROL_FILTER | FilterAPI.FilterType.ENCRYPTION_FILTER| FilterAPI.FilterType.PROCESS_FILTER;

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

                filterControl.ClearFilters();

                //setup a file filter rule for folder encryptFolder
                FileFilter fileFilter = new FileFilter(encryptFolder + "\\*");
                
                //enable the encryption for the filter rule.
                fileFilter.EnableEncryption = true;

                //if we enable the encryption key from service, you can authorize the encryption or decryption for every file
                //in the callback function OnFilterRequestEncryptKey.
                fileFilter.EnableEncryptionKeyFromService = checkBox_EnableUniqueEncryptionKey.Checked;
                fileFilter.OnFilterRequestEncryptKey += OnFilterRequestEncryptKey;
                
                //get the 256bits encryption key with the passphrase
                fileFilter.EncryptionKey = Utils.GetKeyByPassPhrase(passPhrase, 32);

                //disable the decyrption right, read the raw encrypted data for all except the authorized processes or users.
                fileFilter.EnableReadEncryptedData = false;

                if (textBox_AuthorizedProcessesForEncryptFolder.Text.Trim().Length == 0)
                {
                    authorizedProcessesForEncryptFolder = "*";
                }

                string[] processNames = authorizedProcessesForEncryptFolder.Split(new char[] { ';' });
                if (processNames.Length > 0)
                {
                    foreach (string processName in processNames)
                    {
                        if (processName.Trim().Length > 0)
                        {
                            //authorized the process with the read encrytped data right.
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

                //add the encryption file filter rule to the filter control
                filterControl.AddFilter(fileFilter);

                //setup a file filter rule for folder decryptFolder
                FileFilter decryptFileFilter = new FileFilter(decryptFolder + "\\*");

                //enable the encryption for the filter rule.
                decryptFileFilter.EnableEncryption = true;

                //if we enable the encryption key from service, you can authorize the decryption for every file
                //in the callback function OnFilterRequestEncryptKey.
                decryptFileFilter.EnableEncryptionKeyFromService = checkBox_EnableUniqueEncryptionKey.Checked;
                decryptFileFilter.OnFilterRequestEncryptKey += OnFilterRequestEncryptKey;

                //get the 256bits encryption key with the passphrase
                decryptFileFilter.EncryptionKey = Utils.GetKeyByPassPhrase(passPhrase, 32);

                //don't encrypt the new created file in the folder.
                decryptFileFilter.EnableEncryptNewFile = false;

                //disable the decyrption right, read the raw encrypted data for all except the authorized processes or users.
                decryptFileFilter.EnableReadEncryptedData = false;

                processNames = authorizedProcessesForDecryptFolder.Split(new char[] { ';' });
                if (processNames.Length > 0)
                {
                    foreach (string processName in processNames)
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
                    return;
                }

                EventManager.WriteMessage(102, "StartFilter", EventLevel.Information, "Start filter service succeeded.");

                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("Start filter service succeeded.", "StartFilter", MessageBoxButtons.OK, MessageBoxIcon.Information);

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

        /// <summary>
        /// Fires this event when the encrypted file requests the encryption key.
        /// </summary>
        public void OnFilterRequestEncryptKey(object sender, EncryptEventArgs e)
        {
            e.ReturnStatus = NtStatus.Status.Success;

            if (e.IsNewCreatedFile)
            {
                //if you want to block the new file creation, you can return accessdenied status.
                //e.ReturnStatus = NtStatus.Status.AccessDenied;

                //if you want to the file being created without encryption, return below status.
                //e.ReturnStatus = NtStatus.Status.FileIsNoEncrypted;

                //for the new created file, you can add your custom tag data to the header of the encyrpted file.
                //here we just add the file name as the tag data.
                e.EncryptionTag = UnicodeEncoding.Unicode.GetBytes(e.FileName);
            }
            else
            {
                //this is the encrytped file open request, request the encryption key and iv.

                //if you want to block encrypted file being opened, you can return accessdenied status.
                //e.ReturnStatus = NtStatus.Status.AccessDenied;

                //if you want to return the raws encrypted data for this encrypted file, return below status.
                //e.ReturnStatus = NtStatus.Status.FileIsEncrypted;

                //here is the tag data if you set custom tag data when the new created file requested the key.
                byte[] tagData = e.EncryptionTag;
            }

            //here is the encryption key for the encrypted file, you can set it with your own key.
            e.EncryptionKey = Utils.GetKeyByPassPhrase(GlobalConfig.MasterPassword, 32);
           
           //if you want to use your own iv for the encrypted file, set the value here, 
           //or don't set the iv here, then the unique auto generated iv will be assigned to the file.
           //e.IV = Utils.GetIVByPassPhrase(GlobalConfig.MasterPassword);

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

            return ;
        }

        private void AutoEncryptForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            GlobalConfig.Stop();
            filterControl.StopFilter();
        }

        private void checkBox_EnableUniqueEncryptionKey_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_EnableUniqueEncryptionKey.Checked)
            {
                //if get encryption key from service, then only the callback function will authorize the encryption or decryption.
                textBox_PassPhrase.Enabled = false;
                textBox_AuthorizedProcessesForEncryptFolder.Enabled = false;
                textBox_AuthorizedUsersForEncryptFolder.Enabled = false;
            }
            else
            {
                textBox_PassPhrase.Enabled = true;
                textBox_AuthorizedProcessesForEncryptFolder.Enabled = true;
                textBox_AuthorizedUsersForEncryptFolder.Enabled = true;
            }
        }

        private void button_EncryptFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox_EncrptFolder.Text = folderDialog.SelectedPath;
            }
        }

       
    }
}
