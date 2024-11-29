using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using EaseFilter.FilterControl;
using EaseFilter.CommonObjects;

namespace SecureSandbox
{
    public partial class ProcessFileAccessRights : Form
    {
        public struct FileAccessRight
        {
            public string FileNameMask;
            public uint AccessFlag;
        }

        ProcessFilterRule currentFilterRule = null;
        Dictionary<string, FileAccessRight> processFileAccessRightsList = new Dictionary<string, FileAccessRight>();
        FileAccessRight currentFileAccessRight = new FileAccessRight();

        public ProcessFileAccessRights(ProcessFilterRule filterRule)
        {
            InitializeComponent();           

            StartPosition = FormStartPosition.CenterParent;

            currentFilterRule = filterRule;
            string processFileAccessRights = filterRule.FileAccessRights; 

            string[] accessRightList = processFileAccessRights.ToLower().Split(new char[] { ';' });
            if (accessRightList.Length > 0)
            {
                foreach (string processFileAccessRightStr in accessRightList)
                {
                    if (processFileAccessRightStr.Trim().Length > 0)
                    {
                        string fileMask = processFileAccessRightStr.Substring(0, processFileAccessRightStr.IndexOf('!'));
                        uint accessFlag = uint.Parse(processFileAccessRightStr.Substring(processFileAccessRightStr.IndexOf('!') + 1));

                        if (!processFileAccessRightsList.ContainsKey(fileMask))
                        {
                            FileAccessRight fileAccessRight = new FileAccessRight();
                            fileAccessRight.FileNameMask = fileMask;
                            fileAccessRight.AccessFlag = accessFlag;

                            processFileAccessRightsList.Add(fileMask, fileAccessRight);
                            currentFileAccessRight = fileAccessRight;
                        }

                        textBox_FileMask.Text = fileMask;
                        textBox_AccessFlag.Text = accessFlag.ToString();
                    }
                }

            }

            groupBox_ProcessRights.Text = "The file access rights for processes which match " + filterRule.ProcessNameFilterMask;

            InitListView();
            SetCheckBoxValue();
        }

        public void InitListView()
        {
            //init ListView control
            listView_ProcessFileAccessRights.Clear();		//clear control
            //create column header for ListView
            listView_ProcessFileAccessRights.Columns.Add("#", 20, System.Windows.Forms.HorizontalAlignment.Left);
            listView_ProcessFileAccessRights.Columns.Add("File Name Mask", 350, System.Windows.Forms.HorizontalAlignment.Left);
            listView_ProcessFileAccessRights.Columns.Add("Access Flag", 150, System.Windows.Forms.HorizontalAlignment.Left);

            foreach (FileAccessRight fileAccessRight in processFileAccessRightsList.Values)
            {
                string[] itemStr = new string[listView_ProcessFileAccessRights.Columns.Count];
                itemStr[0] = listView_ProcessFileAccessRights.Items.Count.ToString();
                itemStr[1] = fileAccessRight.FileNameMask;
                itemStr[2] = fileAccessRight.AccessFlag.ToString("X");

                ListViewItem item = new ListViewItem(itemStr, 0);
                item.Tag = fileAccessRight;
                listView_ProcessFileAccessRights.Items.Add(item);
            }

        }

        private void SetCheckBoxValue()
        {

            if (textBox_AccessFlag.Text.Trim().Length == 0)
            {
                return;
            }

            uint accessFlags = uint.Parse(textBox_AccessFlag.Text);

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

            if ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_QUERY_INFORMATION_ACCESS) > 0)
            {
                checkBox_QueryInfo.Checked = true;
            }
            else
            {
                checkBox_QueryInfo.Checked = false;
            }

            if ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_SET_INFORMATION) > 0)
            {
                checkBox_SetInfo.Checked = true;
            }
            else
            {
                checkBox_SetInfo.Checked = false;
            }

            if ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_OPEN_WITH_CREATE_OR_OVERWRITE_ACCESS) > 0)
            {
                checkBox_Creation.Checked = true;
            }
            else
            {
                checkBox_Creation.Checked = false;
            }

            if ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_QUERY_SECURITY_ACCESS) > 0)
            {
                checkBox_QuerySecurity.Checked = true;
            }
            else
            {
                checkBox_QuerySecurity.Checked = false;
            }

            if ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_SET_SECURITY_ACCESS) > 0)
            {
                checkBox_SetSecurity.Checked = true;
            }
            else
            {
                checkBox_SetSecurity.Checked = false;
            }

        
        }

        private void button_AccessFlag_Click(object sender, EventArgs e)
        {
            OptionForm optionForm = new OptionForm(OptionForm.OptionType.Access_Flag, textBox_AccessFlag.Text);

            if (optionForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (optionForm.AccessFlags > 0)
                {
                    textBox_AccessFlag.Text = optionForm.AccessFlags.ToString();
                }
                else
                {
                    //if the accessFlag is 0, it is exclude filter rule,this is not what we want, so we need to include this flag.
                    textBox_AccessFlag.Text = ((uint)FilterAPI.AccessFlag.LEAST_ACCESS_FLAG).ToString();
                }

                SetCheckBoxValue();
            }
        }

        private void checkBox_Read_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_AccessFlag.Text.Trim());
            if (checkBox_Read.Checked)
            {
                accessFlags |= (uint)FilterAPI.ALLOW_FILE_READ_ACCESS;
            }
            else
            {
                accessFlags &= ~(uint)FilterAPI.ALLOW_FILE_READ_ACCESS;
            }

            textBox_AccessFlag.Text = accessFlags.ToString();
        }

        private void checkBox_Write_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_AccessFlag.Text.Trim());
            if (checkBox_Write.Checked)
            {
                accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_WRITE_ACCESS | (uint)FilterAPI.AccessFlag.ALLOW_OPEN_WITH_WRITE_ACCESS;
            }
            else
            {
                accessFlags &= ~((uint)FilterAPI.AccessFlag.ALLOW_WRITE_ACCESS | (uint)FilterAPI.AccessFlag.ALLOW_OPEN_WITH_WRITE_ACCESS);
            }

            textBox_AccessFlag.Text = accessFlags.ToString();
        }

        private void checkBox_Creation_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_AccessFlag.Text.Trim());
            if (checkBox_Creation.Checked)
            {
                accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_OPEN_WITH_CREATE_OR_OVERWRITE_ACCESS;
            }
            else
            {
                accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_OPEN_WITH_CREATE_OR_OVERWRITE_ACCESS;
            }

            textBox_AccessFlag.Text = accessFlags.ToString();
        }


        private void checkBox_QueryInfo_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_AccessFlag.Text.Trim());
            if (checkBox_QueryInfo.Checked)
            {
                accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_QUERY_INFORMATION_ACCESS;
            }
            else
            {
                accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_QUERY_INFORMATION_ACCESS;
            }

            textBox_AccessFlag.Text = accessFlags.ToString();
        }

        private void checkBox_SetInfo_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_AccessFlag.Text.Trim());
            if (checkBox_SetInfo.Checked)
            {
                accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_SET_INFORMATION;
            }
            else
            {
                accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_SET_INFORMATION;
            }

            textBox_AccessFlag.Text = accessFlags.ToString();
        }

        private void checkBox_AllowRename_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_AccessFlag.Text.Trim());
            if (checkBox_AllowRename.Checked)
            {
                accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_FILE_RENAME;
            }
            else
            {
                accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_FILE_RENAME;
            }

            textBox_AccessFlag.Text = accessFlags.ToString();
        }

        private void checkBox_AllowDelete_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_AccessFlag.Text.Trim());
            if (checkBox_AllowDelete.Checked)
            {
                accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_FILE_DELETE;
            }
            else
            {
                accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_FILE_DELETE;
            }

            textBox_AccessFlag.Text = accessFlags.ToString();
        }

        private void checkBox_QuerySecurity_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_AccessFlag.Text.Trim());
            if (checkBox_QuerySecurity.Checked)
            {
                accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_QUERY_SECURITY_ACCESS;
            }
            else
            {
                accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_QUERY_SECURITY_ACCESS;
            }

            textBox_AccessFlag.Text = accessFlags.ToString();
        }

        private void checkBox_SetSecurity_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_AccessFlag.Text.Trim());
            if (checkBox_SetSecurity.Checked)
            {
                accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_SET_SECURITY_ACCESS;
            }
            else
            {
                accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_SET_SECURITY_ACCESS;
            }

            textBox_AccessFlag.Text = accessFlags.ToString();
        }


        private void button_Add_Click(object sender, EventArgs e)
        {
            if (textBox_FileMask.Text.Trim().Length == 0)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("The file name mask can't be empty.", "Add file entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
         

            FileAccessRight fileAccessRight = new FileAccessRight();
            fileAccessRight.FileNameMask = textBox_FileMask.Text;
            fileAccessRight.AccessFlag = uint.Parse(textBox_AccessFlag.Text);

            processFileAccessRightsList.Remove(fileAccessRight.FileNameMask);

            processFileAccessRightsList.Add(fileAccessRight.FileNameMask, fileAccessRight);

            InitListView();
        }

        private void button_Delete_Click(object sender, EventArgs e)
        {
            if (listView_ProcessFileAccessRights.SelectedItems.Count == 0)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("There are no item selected.", "Delete entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            FileAccessRight processFileAccessRight = (FileAccessRight)listView_ProcessFileAccessRights.SelectedItems[0].Tag;

            processFileAccessRightsList.Remove(processFileAccessRight.FileNameMask);

            InitListView();
        }

        private void button_ApplyAll_Click(object sender, EventArgs e)
        {
            currentFilterRule.FileAccessRights = string.Empty;

            foreach (FileAccessRight fileAccessRight in processFileAccessRightsList.Values)
            {
                currentFilterRule.FileAccessRights += fileAccessRight.FileNameMask + "!" + fileAccessRight.AccessFlag + ";";
            }
        }

        private void button_SelectFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDiag = new FolderBrowserDialog();

            if (folderDiag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox_FileMask.Text = folderDiag.SelectedPath + "\\*";
            }
        }

        private void button_Info_Click(object sender, EventArgs e)
        {
            string information = "Set up the file access rights with the file filter mask to the processes which were launched from the sandbox.\r\n\r\n";
            information += "you can restrict the process's file access rights to the specific folders, prevent the files being read or changed by the processes inside the sandbox.\r\n\r\n";

            MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
            MessageBox.Show(information, "File access rights", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void listView_ProcessFileAccessRights_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView_ProcessFileAccessRights.SelectedItems.Count > 0)
            {

                currentFileAccessRight = (FileAccessRight)listView_ProcessFileAccessRights.SelectedItems[0].Tag;

                textBox_FileMask.Text = currentFileAccessRight.FileNameMask;
                textBox_AccessFlag.Text = currentFileAccessRight.AccessFlag.ToString();
               
                SetCheckBoxValue();
            }
        }
    }
}
