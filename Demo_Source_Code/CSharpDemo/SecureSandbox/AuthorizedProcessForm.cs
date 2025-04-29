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

using EaseFilter.FilterControl;
using EaseFilter.CommonObjects;

namespace SecureSandbox
{

    public partial class Form_ProcessRights : Form
    {
        public struct ProcessRight
        {
            public string ProcessName;
            public uint ProcessId;
            public uint AccessFlag;
        }

        public FileFilter currentFileFilter = null;
        ProcessRight selectedProcessRight;
        Dictionary<string, ProcessRight> processRights = new Dictionary<string, ProcessRight>();

        public Form_ProcessRights(ref FileFilter fileFilter)
        {
            InitializeComponent();
            currentFileFilter = fileFilter;

            StartPosition = FormStartPosition.CenterParent;

            foreach (KeyValuePair<string, ProcessRightInfo> entry in fileFilter.TrustedProcessAccessRightList)
            {
                string processName = entry.Value.processNameFilterMask;
                uint accessFlag = entry.Value.accessFlags;

                if (!processRights.ContainsKey(processName))
                {
                    ProcessRight processRight = new ProcessRight();
                    processRight.ProcessName = processName;
                    processRight.ProcessId = 0;
                    processRight.AccessFlag = accessFlag;

                    processRights.Add(processName, processRight);
                }

                textBox_ProcessName.Text = processName;
                textBox_AccessFlag.Text = accessFlag.ToString();
            }

            foreach (KeyValuePair<uint, uint> entry in fileFilter.ProcessIdAccessRightList)
            {
                string processId = entry.Key.ToString();
                uint accessFlag = entry.Value;

                if (!processRights.ContainsKey(processId))
                {
                    ProcessRight processRight = new ProcessRight();
                    processRight.ProcessName = "";
                    processRight.ProcessId = uint.Parse(processId);
                    processRight.AccessFlag = accessFlag;

                    processRights.Add(processId, processRight);
                }

                textBox_ProcessId.Text = processId;
                textBox_AccessFlag.Text = accessFlag.ToString();
            }

            InitListView();
            SetCheckBoxValue();
        }

        public void InitListView()
        {
            //init ListView control
            listView_ProcessRights.Clear();		//clear control
            //create column header for ListView
            listView_ProcessRights.Columns.Add("#", 20, System.Windows.Forms.HorizontalAlignment.Left);
            listView_ProcessRights.Columns.Add("Process Name", 350, System.Windows.Forms.HorizontalAlignment.Left);
            listView_ProcessRights.Columns.Add("Process Id", 100, System.Windows.Forms.HorizontalAlignment.Left);
            listView_ProcessRights.Columns.Add("Access Rights", 150, System.Windows.Forms.HorizontalAlignment.Left);

            foreach (ProcessRight processRight in processRights.Values)
            {
                string[] itemStr = new string[listView_ProcessRights.Columns.Count];
                itemStr[0] = listView_ProcessRights.Items.Count.ToString();
                itemStr[1] = processRight.ProcessName;
                itemStr[2] = processRight.ProcessId.ToString();
                itemStr[3] = processRight.AccessFlag.ToString();

                ListViewItem item = new ListViewItem(itemStr, 0);
                item.Tag = processRight;
                listView_ProcessRights.Items.Add(item);

                selectedProcessRight = processRight;
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

            if ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_WRITE_ACCESS) > 0 )
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

            if ( (accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_SET_INFORMATION) > 0 )
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

            if ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_ALL_SAVE_AS) > 0)
            {
                checkBox_AllowSaveAs.Checked = true;
            }
            else
            {
                checkBox_AllowSaveAs.Checked = false;
            }

            if ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_COPY_PROTECTED_FILES_OUT) > 0)
            {
                checkBox_AllowCopyout.Checked = true;
            }
            else
            {
                checkBox_AllowCopyout.Checked = false;
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
                accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_READ_ACCESS;
            }
            else
            {
                accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_READ_ACCESS;
            }

            textBox_AccessFlag.Text = accessFlags.ToString();
        }

        private void checkBox_Write_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_AccessFlag.Text.Trim());
            if (checkBox_Write.Checked)
            {
                accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_WRITE_ACCESS;
            }
            else
            {
                accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_WRITE_ACCESS;
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

        private void checkBox_AllowSaveAs_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_AccessFlag.Text.Trim());
            if (checkBox_AllowSaveAs.Checked)
            {
                accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_ALL_SAVE_AS;
            }
            else
            {
                accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_ALL_SAVE_AS;
            }

            textBox_AccessFlag.Text = accessFlags.ToString();
        }

        private void checkBox_AllowCopyout_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_AccessFlag.Text.Trim());
            if (checkBox_AllowCopyout.Checked)
            {
                accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_COPY_PROTECTED_FILES_OUT;
            }
            else
            {
                accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_COPY_PROTECTED_FILES_OUT;
            }

            textBox_AccessFlag.Text = accessFlags.ToString();
        }

        private void checkBox_AllowReadEncryptedFiles_CheckedChanged(object sender, EventArgs e)
        {
            uint accessFlags = uint.Parse(textBox_AccessFlag.Text.Trim());
            if (checkBox_AllowReadEncryptedFiles.Checked)
            {
                accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_READ_ENCRYPTED_FILES;
            }
            else
            {
                accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_READ_ENCRYPTED_FILES;
            }

            textBox_AccessFlag.Text = accessFlags.ToString();
        }

        private void button_AddProcessName_Click(object sender, EventArgs e)
        {

            OptionForm optionForm = new OptionForm(OptionForm.OptionType.ProccessId, textBox_ProcessId.Text);

            if (optionForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (optionForm.ProcessId.Length > 0)
                {
                    textBox_ProcessId.Text = optionForm.ProcessId;

                    //we choose to use process Id instead of the process name
                    textBox_ProcessName.Text = "";
                }
            }
        }

        private void button_Delete_Click(object sender, EventArgs e)
        {
            if (listView_ProcessRights.SelectedItems.Count == 0)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("There are no process selected.", "Delete process", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ProcessRight processRight = (ProcessRight)listView_ProcessRights.SelectedItems[0].Tag;

            if (processRight.ProcessId > 0)
            {
                processRights.Remove(processRight.ProcessId.ToString());
            }
            else
            {
                processRights.Remove(processRight.ProcessName.ToString());
            }

            InitListView();
        }

        private void listView_ProcessRights_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (listView_ProcessRights.SelectedItems.Count > 0)
            {
                ProcessRight processRight = (ProcessRight)listView_ProcessRights.SelectedItems[0].Tag;

                textBox_AccessFlag.Text = processRight.AccessFlag.ToString();
                textBox_ProcessName.Text = processRight.ProcessName;
                textBox_ProcessId.Text = processRight.ProcessId.ToString();

                SetCheckBoxValue();
            }

        }

        private void button_AddProcessRights_Click(object sender, EventArgs e)
        {
            textBox_ProcessName.Text = "newprocessname.exe";
            textBox_AccessFlag.Text = FilterAPI.ALLOW_MAX_RIGHT_ACCESS.ToString();
            SetCheckBoxValue();          
        }

        private void button_ApplyAll_Click(object sender, EventArgs e)
        {
            currentFileFilter.TrustedProcessAccessRightList.Clear();
            currentFileFilter.ProcessIdAccessRightList.Clear();

            foreach (ProcessRight processRight in processRights.Values )
            {
                if (processRight.ProcessId > 0)
                {
                    currentFileFilter.ProcessIdAccessRightList.Add(processRight.ProcessId,processRight.AccessFlag);
                }
                else
                {
                    currentFileFilter.AddTrustedProcessRight(processRight.AccessFlag, processRight.ProcessName, "", "");
                }
            }
        }

        private void button_info_Click(object sender, EventArgs e)
        {
            string   information = "1.With the access right control to the process, you can control which process can access or change your files.\r\n\r\n"; 
            information += "2. The process name filter mask format can be the image path filter mask or the process name, e.g. 'c:\\test\\*.exe'. or only process name, e.g. 'notepad.exe'.\r\n\r\n";
            information += "3. To authorize all access rights to the process, set the accessFlag to the maximum access rights.";
            information += "To remove some specific access rights to the process, set the accessFlag to the maximum access rights first, then remove thoese specific rights.\r\n\r\n";
          

            MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
            MessageBox.Show(information, "How to set the access rights to specific process?", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button_SaveProcessRight_Click(object sender, EventArgs e)
        {
            
            if (textBox_ProcessName.Text.Trim().Length == 0 && (textBox_ProcessId.Text.Length == 0 || textBox_ProcessId.Text == "0"))
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("The process name and process Id can't be empty.", "Save settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string[] processList = textBox_ProcessName.Text.ToLower().Split(new char[] { ';' });
            uint accessFlag = uint.Parse(textBox_AccessFlag.Text);

            if (processList.Length > 0)
            {
                foreach (string processName in processList)
                {
                    if (processName.Trim().Length > 0)
                    {
                        processRights.Remove(processName);
                        ProcessRight processRight = new ProcessRight();
                        processRight.ProcessName = processName;
                        processRight.ProcessId = 0;
                        processRight.AccessFlag = accessFlag;

                        processRights.Add(processName, processRight);
                    }
                }

            }

            string[] processIdList = textBox_ProcessId.Text.Split(new char[] { ';' });
            if (processIdList.Length > 0)
            {
                foreach (string processId in processIdList)
                {
                    uint pid = 0;

                    if (uint.TryParse(processId, out pid) && pid > 0)
                    {
                        processRights.Remove(processId);

                        ProcessRight processRight = new ProcessRight();
                        processRight.ProcessName = "";
                        processRight.ProcessId = uint.Parse(processId);
                        processRight.AccessFlag = accessFlag;

                        processRights.Add(processId, processRight);

                    }
                }

            }

            InitListView();
        }

       
    }
}
