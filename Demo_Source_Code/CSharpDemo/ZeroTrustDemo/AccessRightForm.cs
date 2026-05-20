using System;
using System.Windows.Forms;

using EaseFilter.FilterControl;
using EaseFilter.CommonObjects;

namespace ZeroTrustDemo
{
    public partial class AccessRightForm : Form
    {
        public string userName = string.Empty;
        public uint accessRight = FilterAPI.ALLOW_MAX_RIGHT_ACCESS;
        public ProcessRightInfo processRightInfo = new ProcessRightInfo(FilterAPI.ALLOW_MAX_RIGHT_ACCESS, "*", "","");

        public AccessRightForm(ProcessRightInfo _processRightInfo)
        {
            processRightInfo = _processRightInfo;
            InitializeComponent();

            groupBox_AccessRights.Location = groupBox_UserName.Location;
            groupBox_ProcessName.Visible = true;
            groupBox_ProcessSha256.Visible = true;
            groupBox_SignedProcess.Visible = true;

            textBox_FileAccessFlags.Text = processRightInfo.accessFlags.ToString();
            textBox_ProcessName.Text = processRightInfo.processNameFilterMask;
            textBox_ProcessCertificateName.Text = processRightInfo.certificateName;
            textBox_ProcessSha256Hash.Text = processRightInfo.imageSha256Hash;

            SetCheckBoxValue();
        }

        public AccessRightForm(string userName, uint _userAccessRight)
        {
            accessRight = _userAccessRight;
            InitializeComponent();

            textBox_UserName.Text = userName;
            textBox_FileAccessFlags.Text = _userAccessRight.ToString();

            groupBox_AccessRights.Location = groupBox_ProcessSha256.Location;
            groupBox_UserName.Location = groupBox_ProcessName.Location;
            groupBox_UserName.Visible = true;

            SetCheckBoxValue();
        }

        private void SetCheckBoxValue()
        {

            uint accessFlags = uint.Parse(textBox_FileAccessFlags.Text);

            if ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_FILE_DELETE) > 0)
            {
                checkBox_AllowDelete.Checked = true;
            }
            else
            {
                checkBox_AllowDelete.Checked = false;
            }

            if ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_FILE_RENAME) > 0)
            {
                checkBox_AllowRename.Checked = true;
            }
            else
            {
                checkBox_AllowRename.Checked = false;
            }

            if ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_WRITE_ACCESS) > 0)
            {
                checkBox_Write.Checked = true;
            }
            else
            {
                checkBox_Write.Checked = false;
            }

            if ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_READ_ACCESS) > 0)
            {
                checkBox_Read.Checked = true;
            }
            else
            {
                checkBox_Read.Checked = false;
            }

            if ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_READ_ENCRYPTED_FILES) > 0)
            {
                checkBox_AllowReadEncryptedFiles.Checked = true;
            }
            else
            {
                checkBox_AllowReadEncryptedFiles.Checked = false;
            }

        }

        private void button_FileAccessFlags_Click(object sender, EventArgs e)
        {
            OptionForm optionForm = new OptionForm(OptionForm.OptionType.Access_Flag, textBox_FileAccessFlags.Text);

            if (optionForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (optionForm.AccessFlags > 0)
                {
                    textBox_FileAccessFlags.Text = optionForm.AccessFlags.ToString();
                }
                else
                {
                    //if the accessFlag is 0, it is exclude filter rule,this is not what we want, so we need to include this flag.
                    textBox_FileAccessFlags.Text = ((uint)FilterAPI.AccessFlag.LEAST_ACCESS_FLAG).ToString();
                }

                SetCheckBoxValue();
            }
        }

        private void checkBox_Read_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_FileAccessFlags.Text.Trim());
            if (checkBox_Read.Checked)
            {
                accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_READ_ACCESS;
            }
            else
            {
                accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_READ_ACCESS;
            }

            textBox_FileAccessFlags.Text = accessFlags.ToString();
        }

        private void checkBox_Write_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_FileAccessFlags.Text.Trim());
            if (checkBox_Write.Checked)
            {
                accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_WRITE_ACCESS;
            }
            else
            {
                accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_WRITE_ACCESS;
            }

            textBox_FileAccessFlags.Text = accessFlags.ToString();
        }
   
     
        private void checkBox_AllowRename_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_FileAccessFlags.Text.Trim());
            if (checkBox_AllowRename.Checked)
            {
                accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_FILE_RENAME;
            }
            else
            {
                accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_FILE_RENAME;
            }

            textBox_FileAccessFlags.Text = accessFlags.ToString();
        }

        private void checkBox_AllowDelete_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_FileAccessFlags.Text.Trim());
            if (checkBox_AllowDelete.Checked)
            {
                accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_FILE_DELETE;
            }
            else
            {
                accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_FILE_DELETE;
            }

            textBox_FileAccessFlags.Text = accessFlags.ToString();
        }

        private void checkBox_AllowReadEncryptedFiles_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_FileAccessFlags.Text.Trim());
            if (checkBox_AllowReadEncryptedFiles.Checked)
            {
                accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_READ_ENCRYPTED_FILES;
            }
            else
            {
                accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_READ_ENCRYPTED_FILES;
            }

            textBox_FileAccessFlags.Text = accessFlags.ToString();
        }

        private void button_GetProcessSha256_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fileName = fileDialog.FileName;
                byte[] hashBytes = new byte[32];
                uint hashBytesLength = 32;

                if (FilterAPI.Sha256HashFile(fileName, hashBytes, ref hashBytesLength))
                {
                    textBox_ProcessSha256Hash.Text += Utils.ByteArrayToHex(hashBytes);
                }
                else
                {
                    string lastError = "Get file sha256 hash failed with error:" + FilterAPI.GetLastErrorMessage();
                    MessageBox.Show(lastError, "Get sha256 hash", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void button_GetCertificateName_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fileName = fileDialog.FileName;
                uint len = 1024;
                long signedTime = 0;
                string subjectName = new string((char)0, (int)len);

                if (FilterAPI.GetSignerInfo(fileName, subjectName, ref len, ref signedTime))
                {
                    subjectName = subjectName.Substring(0, (int)len / 2);
                    textBox_ProcessCertificateName.Text = subjectName;
                }
                else
                {
                    string lastError = "Get process's certificate name failed with error:" + FilterAPI.GetLastErrorMessage();
                    MessageBox.Show(lastError, "Get process's certificate name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void button_ApplySettings_Click(object sender, EventArgs e)
        {
            userName = textBox_UserName.Text;
            accessRight = uint.Parse(textBox_FileAccessFlags.Text);

            processRightInfo.accessFlags = accessRight;
            processRightInfo.processNameFilterMask = textBox_ProcessName.Text;
            processRightInfo.certificateName = textBox_ProcessCertificateName.Text;
            processRightInfo.imageSha256Hash = textBox_ProcessSha256Hash.Text;

        }

        private void button_InfoProcessName_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The process name can be the binary path filter mask.\nThe certificate name of the signed process is optional, " +
              "if it is not empty, then only the process signed with the certificate will have the access rights.\n " +
               "The imageSha256 is optional, if it is not empty, then only the process with same sha256 hash will have the access rights.\n");
        }

        private void button_InfoUserName_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Set the specific access rights to the user name.");
        }
     
    }
}
