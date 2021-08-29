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
            button_Stop.Enabled = true;
            button_Start.Enabled = false;

            string encryptFolder = textBox_EncrptFolder.Text.Trim();
            string authorizedProcessesForEncryptFolder = textBox_AuthorizedProcessesForEncryptFolder.Text;
            string authorizedUsersForEncryptFolder = textBox_AuthorizedUsersForEncryptFolder.Text;

            string decryptFolder = textBox_DecryptFolder.Text.Trim();
            string authorizedProcessesForDecryptFolder = textBox_AuthorizedProcessesForDecryptFolder.Text;

            string passPhrase = textBox_PassPhrase.Text.Trim();

            string licenseKey = "7ECF8-58DEE-755D3-C186E-C32CF-6C696";
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
                
                //get the 256bits encryption key with the passphrase
                fileFilter.EncryptionKey = Utils.GetKeyByPassPhrase(passPhrase, 32);

                //disable the decyrption right, read the raw encrypted data for all except the authorized processes or users.
                fileFilter.EnableReadEncryptedData = false;

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
            }
            catch (Exception ex)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("Start filter service failed with error " + ex.Message, "StartFilter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                EventManager.WriteMessage(104, "StartFilter", EventLevel.Error, "Start filter service failed with error " + ex.Message);
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

            return ;
        }

        private void AutoEncryptForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            GlobalConfig.Stop();
            filterControl.StopFilter();
        }
    }
}
