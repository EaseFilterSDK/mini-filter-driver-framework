using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using EaseFilter.CommonObjects;
using EaseFilter.FilterControl;

namespace ZeroTrustDemo
{
    public partial class ZeroTrustForm : Form
    {
        FilterControl filterControl = new FilterControl();
        FileFilter fileFilter = new FileFilter("c:\\test\\*");

        public ZeroTrustForm()
        {
            this.StartPosition = FormStartPosition.CenterScreen;

            InitializeComponent();

            fileFilter.AccessFlags = FilterAPI.AccessFlag.LEAST_ACCESS_FLAG;

            foreach (FileFilter savedFileFilter in GlobalConfig.FileFilters.Values)
            {
                fileFilter = savedFileFilter;
                break;
            }

            InitAccessRightsListView(fileFilter);
        }

        ~ZeroTrustForm()
        {
            GlobalConfig.Stop();
        }

        private void InitAccessRightsListView(FileFilter fileFilter)
        {
            textBox_AccessRights.Text = ((uint)fileFilter.AccessFlags).ToString();

            if(( fileFilter.AccessFlags & FilterAPI.AccessFlag.ENABLE_FILE_ENCRYPTION_RULE) > 0 )
            {
                checkBox_Encryption.Checked = true;
                textBox_PassPhrase.Text = fileFilter.EncryptionPassPhrase;
            }
            else
            {
                checkBox_Encryption.Checked = false;
                textBox_PassPhrase.Text = "";
            }

            //init ListView control
            listView_ProcessRights.Clear();		//clear control
            //create column header for ListView
            listView_ProcessRights.Columns.Add("ProcessName", 160, System.Windows.Forms.HorizontalAlignment.Left);
            listView_ProcessRights.Columns.Add("Readable", 70, System.Windows.Forms.HorizontalAlignment.Left);
            listView_ProcessRights.Columns.Add("Writable", 70, System.Windows.Forms.HorizontalAlignment.Left);
            listView_ProcessRights.Columns.Add("Deletable", 70, System.Windows.Forms.HorizontalAlignment.Left);
            listView_ProcessRights.Columns.Add("Renamable", 70, System.Windows.Forms.HorizontalAlignment.Left);
            listView_ProcessRights.Columns.Add("CertName", 70, System.Windows.Forms.HorizontalAlignment.Left);
            listView_ProcessRights.Columns.Add("Sha256", 70, System.Windows.Forms.HorizontalAlignment.Left);


            foreach (KeyValuePair<string, ProcessRightInfo> entry in fileFilter.TrustedProcessAccessRightList)
            {
                uint accessFlags = entry.Value.accessFlags;
                string processName = entry.Value.processNameFilterMask;
                string certName = entry.Value.certificateName;
                string sha256Hash = entry.Value.imageSha256Hash;

                string[] listEntry = new string[listView_ProcessRights.Columns.Count];
                int index = 0;
                listEntry[index++] = processName;
                listEntry[index++] = ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_READ_ACCESS) > 0).ToString();
                listEntry[index++] = ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_WRITE_ACCESS) > 0).ToString();
                listEntry[index++] = ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_FILE_DELETE) > 0).ToString();
                listEntry[index++] = ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_FILE_RENAME) > 0).ToString();
                listEntry[index++] = certName;
                listEntry[index++] = sha256Hash;

                ListViewItem listItem = new ListViewItem(listEntry, 0);
                listItem.Tag = entry.Value;
                listView_ProcessRights.Items.Add(listItem);

            }

            //init ListView control
            listView_UserRights.Clear();		//clear control
            //create column header for ListView
            listView_UserRights.Columns.Add("UserName", 160, System.Windows.Forms.HorizontalAlignment.Left);
            listView_UserRights.Columns.Add("Readable", 70, System.Windows.Forms.HorizontalAlignment.Left);
            listView_UserRights.Columns.Add("Writable", 70, System.Windows.Forms.HorizontalAlignment.Left);
            listView_UserRights.Columns.Add("Deletable", 70, System.Windows.Forms.HorizontalAlignment.Left);
            listView_UserRights.Columns.Add("Renamable", 70, System.Windows.Forms.HorizontalAlignment.Left);

            foreach (KeyValuePair<string, uint> userRight in fileFilter.UserAccessRightList)
            {
                uint accessFlags = userRight.Value;
                string processName = userRight.Key;

                string[] listEntry = new string[listView_UserRights.Columns.Count];
                int index = 0;
                listEntry[index++] = processName;
                listEntry[index++] = ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_READ_ACCESS) > 0).ToString();
                listEntry[index++] = ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_WRITE_ACCESS) > 0).ToString();
                listEntry[index++] = ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_FILE_DELETE) > 0).ToString();
                listEntry[index++] = ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_FILE_RENAME) > 0).ToString();

                ListViewItem listItem = new ListViewItem(listEntry, 0);
                listItem.Tag = userRight;
                listView_UserRights.Items.Add(listItem);
            }

        }

        private void button_ProtectedFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog browseFolder = new FolderBrowserDialog();

            if (browseFolder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox_ProtectedFolder.Text = browseFolder.SelectedPath + "\\*";
            }
        }

        private void button_AccessRights_Click(object sender, EventArgs e)
        {
            OptionForm optionForm = new OptionForm(OptionForm.OptionType.Access_Flag, textBox_AccessRights.Text);

            if (optionForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (optionForm.AccessFlags > 0)
                {
                    textBox_AccessRights.Text = optionForm.AccessFlags.ToString();
                    fileFilter.AccessFlags = (FilterAPI.AccessFlag)optionForm.AccessFlags;
                }
                else
                {
                    //if the accessFlag is 0, it is exclude filter rule,this is not what we want, so we need to include this flag.
                    textBox_AccessRights.Text = ((uint)FilterAPI.AccessFlag.LEAST_ACCESS_FLAG).ToString();
                }

            }
        }

        private void textBox_ProtectedFolder_TextChanged(object sender, EventArgs e)
        {
            fileFilter = new FileFilter(textBox_ProtectedFolder.Text);
            fileFilter.AccessFlags = FilterAPI.AccessFlag.LEAST_ACCESS_FLAG;

            InitAccessRightsListView(fileFilter);
        }      

        private void toolStripButton_AddProcessRights_Click(object sender, EventArgs e)
        {
            ProcessRightInfo processRightInfo = new ProcessRightInfo(FilterAPI.ALLOW_MAX_RIGHT_ACCESS, "explorer.exe", "", "");
            AccessRightForm rightForm = new AccessRightForm(processRightInfo);
            if(rightForm.ShowDialog() == DialogResult.OK)
            {
                processRightInfo = rightForm.processRightInfo;
                fileFilter.TrustedProcessAccessRightList.Remove(processRightInfo.processNameFilterMask);
                fileFilter.TrustedProcessAccessRightList.Add(processRightInfo.processNameFilterMask, processRightInfo);
                InitAccessRightsListView(fileFilter);
            }
        }

        private void toolStripButton_ModifyProcessRights_Click(object sender, EventArgs e)
        {
            if (listView_ProcessRights.SelectedItems.Count != 1)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("Please select one item to modify.", "modification", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            System.Windows.Forms.ListViewItem item = listView_ProcessRights.SelectedItems[0];
            ProcessRightInfo processRightInfo = (ProcessRightInfo)item.Tag;
            AccessRightForm rightForm = new AccessRightForm(processRightInfo);

            if (rightForm.ShowDialog() == DialogResult.OK)
            {
                processRightInfo = rightForm.processRightInfo;
                fileFilter.TrustedProcessAccessRightList.Remove(processRightInfo.processNameFilterMask);
                fileFilter.TrustedProcessAccessRightList.Add(processRightInfo.processNameFilterMask, processRightInfo);
                InitAccessRightsListView(fileFilter);
            }
        }

        private void toolStripButton_RemoveProcess_Click(object sender, EventArgs e)
        {
            if (listView_ProcessRights.SelectedItems.Count != 1)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("Please select one item to modify.", "modification", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            System.Windows.Forms.ListViewItem item = listView_ProcessRights.SelectedItems[0];
            ProcessRightInfo processRightInfo = (ProcessRightInfo)item.Tag;

            fileFilter.TrustedProcessAccessRightList.Remove(processRightInfo.processNameFilterMask);

            InitAccessRightsListView(fileFilter);
        }

        private void toolStripButton_AddUserRights_Click(object sender, EventArgs e)
        {
            AccessRightForm rightForm = new AccessRightForm("domain\\user1",FilterAPI.ALLOW_MAX_RIGHT_ACCESS);
            if (rightForm.ShowDialog() == DialogResult.OK)
            {
                string userName = rightForm.userName;
                uint accessRight = rightForm.accessRight;
                fileFilter.UserAccessRightList.Remove(userName);
                fileFilter.UserAccessRightList.Add(userName, accessRight);
                InitAccessRightsListView(fileFilter);
            }
        }

        private void toolStripButton_ModifyUserRights_Click(object sender, EventArgs e)
        {
            if (listView_UserRights.SelectedItems.Count != 1)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("Please select one item to modify.", "modification", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            System.Windows.Forms.ListViewItem item = listView_UserRights.SelectedItems[0];
            KeyValuePair<string,uint> userRightInfo = (KeyValuePair<string, uint>)item.Tag;
            AccessRightForm rightForm = new AccessRightForm(userRightInfo.Key, userRightInfo.Value);

            if (rightForm.ShowDialog() == DialogResult.OK)
            {
                string userName = rightForm.userName;
                uint accessRight = rightForm.accessRight;
                fileFilter.UserAccessRightList.Remove(userName);
                fileFilter.UserAccessRightList.Add(userName, accessRight);
                InitAccessRightsListView(fileFilter);
            }
        }

        private void toolStripButton_RemoveUser_Click(object sender, EventArgs e)
        {
            if (listView_UserRights.SelectedItems.Count != 1)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("Please select one item to modify.", "modification", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            System.Windows.Forms.ListViewItem item = listView_UserRights.SelectedItems[0];
            KeyValuePair<string, uint> userRightInfo = (KeyValuePair<string, uint>)item.Tag;
            fileFilter.UserAccessRightList.Remove(userRightInfo.Key);
            InitAccessRightsListView(fileFilter);

        }

        private void checkBox_Encryption_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Encryption.Checked)
            {
                textBox_PassPhrase.Enabled = true;
                fileFilter.AccessFlags = (FilterAPI.AccessFlag)FilterAPI.ALLOW_MAX_RIGHT_ACCESS|FilterAPI.AccessFlag.ENABLE_FILE_ENCRYPTION_RULE;
                //for encryption, by default we don't allow the encrypted file being decrypted for all processes.
                fileFilter.AccessFlags &= ~FilterAPI.AccessFlag.ALLOW_READ_ENCRYPTED_FILES; 
            }
            else
            {
                textBox_PassPhrase.Enabled = false;
                fileFilter.AccessFlags = FilterAPI.AccessFlag.LEAST_ACCESS_FLAG;
            }

            InitAccessRightsListView(fileFilter);
        }

        private void button_Info_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The Zero Trust tool applies a default - deny access control policy to the protected folder.Only processes or users explicitly defined in the allow list are permitted access.\n\n " +
           "With encryption enabled, decryption is performed transparently only for authorized entities. Unauthorized entities can access only the encrypted file stream and will receive ciphertext data.\n", "ZeroTrust Demo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        

        private void button_Start_Click(object sender, EventArgs e)
        {
            //To request a trial or production license key, please contact info@easefilter.com
            //Requests from free email domains are not accepted        
            string licenseKey = GlobalConfig.LicenseKey;

            GlobalConfig.filterType = FilterAPI.FilterType.CONTROL_FILTER | FilterAPI.FilterType.ENCRYPTION_FILTER | FilterAPI.FilterType.PROCESS_FILTER;
            bool ret = false;
            string lastError = string.Empty;

            if (!textBox_ProtectedFolder.Text.EndsWith("*"))
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("The file filter mask for a protected folder must end with '*'", "StartFilter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(checkBox_Encryption.Checked)
            {
                if (textBox_PassPhrase.Text.Trim().Length == 0)
                {
                    MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                    MessageBox.Show("Please enter the passpharse for encryption policy.", "StartFilter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                fileFilter.EnableEncryption = true;
                fileFilter.EncryptionPassPhrase = textBox_PassPhrase.Text;
            }

            GlobalConfig.FileFilters.Clear();
            GlobalConfig.AddFileFilter(fileFilter);
            GlobalConfig.SaveConfigSetting();

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

                filterControl.AddFilter(fileFilter);

                if (!filterControl.SendConfigSettingsToFilter(ref lastError))
                {
                    MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                    MessageBox.Show("SendConfigSettingsToFilter failed." + lastError, "SendConfigSettingsToFilter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


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

        private void ZeroTrustForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            FilterAPI.ResetConfigData();
            GlobalConfig.Stop();
            filterControl.StopFilter();
            Application.Exit();
        }
    }
}
