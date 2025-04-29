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

namespace EaseFilter.CommonObjects
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            InitOptionForm();
        }

        private void InitOptionForm()
        {
            try
            {
                if (GlobalConfig.filterType == FilterAPI.FilterType.MONITOR_FILTER)
                {
                    //disable some non monitor settings
                    label_protector1.Visible = false;
                    textBox_ProtectedPID.Visible = false;
                    button_InfoProtectPid.Visible = false;
                    button_SelectProtectPID.Visible = false;

                    checkBox_BlockFormatting.Visible = false;
                    checkBox_BlockUSBRead.Visible = false;
                    checkBox_BlockUSBWrite.Visible = false;
                    button_InfoBlockVolumeFormatting.Visible = false;
                    button_InfoBlockUSBWrite.Visible = false;
                    button_InfoBlockUSBRead.Visible = false;
                }

                textBox_MaximumFilterMessage.Text = GlobalConfig.MaximumFilterMessages.ToString();
                textBox_TransactionLog.Text = GlobalConfig.FilterMessageLogName;
                checkBox_TransactionLog.Checked = GlobalConfig.EnableLogTransaction;
                checkBox_OutputMessageToConsole.Checked = GlobalConfig.OutputMessageToConsole;
                checkBox_DisableDir.Checked =  GlobalConfig.DisableDirIO;

                textBox_ConnectionThreads.Text = GlobalConfig.FilterConnectionThreads.ToString();
                textBox_ConnectionTimeout.Text = GlobalConfig.ConnectionTimeOut.ToString();

                if ((GlobalConfig.BooleanConfig & FilterAPI.BooleanConfig.ENABLE_SEND_DATA_BUFFER) > 0)
                {
                    checkBox_SendBuffer.Checked = true;
                }
                else
                {
                    checkBox_SendBuffer.Checked = false;
                }

                if ((GlobalConfig.VolumeControlFlag & (uint)FilterAPI.VolumeControlFlag.GET_ATTACHED_VOLUME_INFO) > 0)
                {
                    checkBox_GetVolumeInfo.Checked = true;
                }
                else
                {
                    checkBox_GetVolumeInfo.Checked = false;
                }

                if ((GlobalConfig.VolumeControlFlag & (uint)FilterAPI.VolumeControlFlag.VOLUME_ATTACHED_NOTIFICATION) > 0)
                {
                    checkBox_CallbackVolumeAttached.Checked = true;
                }
                else
                {
                    checkBox_CallbackVolumeAttached.Checked = false;
                }

                if ((GlobalConfig.VolumeControlFlag & (uint)FilterAPI.VolumeControlFlag.VOLUME_DETACHED_NOTIFICATION) > 0)
                {
                    checkBox_CallbackVolumeDetached.Checked = true;
                }
                else
                {
                    checkBox_CallbackVolumeDetached.Checked = false;
                }

                if ((GlobalConfig.VolumeControlFlag & (uint)FilterAPI.VolumeControlFlag.BLOCK_VOLUME_DISMOUNT) > 0)
                {
                    checkBox_BlockFormatting.Checked = true;
                }
                else
                {
                    checkBox_BlockFormatting.Checked = false;
                }

                if ((GlobalConfig.VolumeControlFlag & (uint)FilterAPI.VolumeControlFlag.BLOCK_USB_READ) > 0)
                {
                    checkBox_BlockUSBRead.Checked = true;
                }
                else
                {
                    checkBox_BlockUSBRead.Checked = false;
                }

                if ((GlobalConfig.VolumeControlFlag & (uint)FilterAPI.VolumeControlFlag.BLOCK_USB_WRITE) > 0)
                {
                    checkBox_BlockUSBWrite.Checked = true;
                }
                else
                {
                    checkBox_BlockUSBWrite.Checked = false;
                }


                foreach (uint pid in GlobalConfig.IncludePidList)
                {
                    if (textBox_ConnectionThreads.Text.Length > 0)
                    {
                        textBox_ConnectionThreads.Text += ";";
                    }

                    textBox_ConnectionThreads.Text += pid.ToString();
                }

                foreach (uint pid in GlobalConfig.ExcludePidList)
                {
                    if (textBox_ConnectionTimeout.Text.Length > 0)
                    {
                        textBox_ConnectionTimeout.Text += ";";
                    }

                    textBox_ConnectionTimeout.Text += pid.ToString();
                }

                foreach (uint pid in GlobalConfig.ProtectPidList)
                {
                    if (textBox_ProtectedPID.Text.Length > 0)
                    {
                        textBox_ProtectedPID.Text += ";";
                    }

                    textBox_ProtectedPID.Text += pid.ToString();
                }

                InitListView();

            }
            catch (Exception ex)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("Initialize the option form failed with error " + ex.Message, "Init options.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void InitListView()
        {
            //init ListView control
            listView_FilterRules.Clear();		//clear control
            //create column header for ListView
            listView_FilterRules.Columns.Add("#", 20, System.Windows.Forms.HorizontalAlignment.Left);
            listView_FilterRules.Columns.Add("InlcudeFilterMask", 150, System.Windows.Forms.HorizontalAlignment.Left);
            listView_FilterRules.Columns.Add("ExcludeFilterMask", 200, System.Windows.Forms.HorizontalAlignment.Left);
            listView_FilterRules.Columns.Add("AccessFlags", 100, System.Windows.Forms.HorizontalAlignment.Left);

            foreach (FileFilter fileFilter in GlobalConfig.FileFilters.Values)
            {
                AddItem(fileFilter);
            }

        }

        private void AddItem(FileFilter fileFilter)
        {
            string[] itemStr = new string[listView_FilterRules.Columns.Count];
            itemStr[0] = listView_FilterRules.Items.Count.ToString();
            itemStr[1] = fileFilter.IncludeFileFilterMask;
            itemStr[2] = fileFilter.ExcludeFileFilterMaskString;
            itemStr[3] = fileFilter.AccessFlags.ToString();
            ListViewItem item = new ListViewItem(itemStr, 0);
            item.Tag = fileFilter;
            listView_FilterRules.Items.Add(item);
        }

        private void button_AddFilter_Click(object sender, EventArgs e)
        {
            string defaultAccessFlags = ((uint)FilterAPI.ALLOW_MAX_RIGHT_ACCESS ).ToString();
           
            FilterRuleForm filterRuleForm = new FilterRuleForm();
            filterRuleForm.StartPosition = FormStartPosition.CenterParent;
            filterRuleForm.ShowDialog();

            InitListView();
        }

        private void button_EditFilterRule_Click(object sender, EventArgs e)
        {
            if (listView_FilterRules.SelectedItems.Count != 1)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("Please select one filter rule to edit.", "Edit Filter Rule", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            System.Windows.Forms.ListViewItem item = listView_FilterRules.SelectedItems[0];
            FileFilter fileFilter = (FileFilter)item.Tag;

            FilterRuleForm filterRuleForm = new FilterRuleForm(fileFilter);
            filterRuleForm.StartPosition = FormStartPosition.CenterParent;
            filterRuleForm.ShowDialog();

            InitListView();
        }

        private void button_DeleteFilter_Click(object sender, EventArgs e)
        {
            if (listView_FilterRules.SelectedItems.Count == 0)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("There are no filter rule selected.", "Delete Filter Rule", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (System.Windows.Forms.ListViewItem item in listView_FilterRules.SelectedItems)
            {
                FileFilter fileFilter = (FileFilter)item.Tag;
                GlobalConfig.RemoveFileFilter(fileFilter.IncludeFileFilterMask);
            }

            InitListView();
        }


        private void button_ApplyOptions_Click(object sender, EventArgs e)
        {
            try
            {
               
                GlobalConfig.MaximumFilterMessages = int.Parse(textBox_MaximumFilterMessage.Text);
                GlobalConfig.EnableLogTransaction = checkBox_TransactionLog.Checked;
                GlobalConfig.OutputMessageToConsole = checkBox_OutputMessageToConsole.Checked;
                GlobalConfig.FilterMessageLogName = textBox_TransactionLog.Text;
                GlobalConfig.DisableDirIO = checkBox_DisableDir.Checked;

                if (checkBox_BlockFormatting.Checked)
                {
                    GlobalConfig.VolumeControlFlag |= (uint)FilterAPI.VolumeControlFlag.BLOCK_VOLUME_DISMOUNT;
                }
                else
                {
                    GlobalConfig.VolumeControlFlag &= (uint)(~FilterAPI.VolumeControlFlag.BLOCK_VOLUME_DISMOUNT);
                }

                if (checkBox_CallbackVolumeAttached.Checked)
                {
                    GlobalConfig.VolumeControlFlag |= (uint)FilterAPI.VolumeControlFlag.VOLUME_ATTACHED_NOTIFICATION;
                }
                else
                {
                    GlobalConfig.VolumeControlFlag &= (uint)(~FilterAPI.VolumeControlFlag.VOLUME_ATTACHED_NOTIFICATION);
                }

                if (checkBox_CallbackVolumeDetached.Checked)
                {
                    GlobalConfig.VolumeControlFlag |= (uint)FilterAPI.VolumeControlFlag.VOLUME_DETACHED_NOTIFICATION;
                }
                else
                {
                    GlobalConfig.VolumeControlFlag &= (uint)(~FilterAPI.VolumeControlFlag.VOLUME_DETACHED_NOTIFICATION);
                }

                if (checkBox_GetVolumeInfo.Checked)
                {
                    GlobalConfig.VolumeControlFlag |= (uint)FilterAPI.VolumeControlFlag.GET_ATTACHED_VOLUME_INFO;
                }
                else
                {
                    GlobalConfig.VolumeControlFlag &= (uint)(~FilterAPI.VolumeControlFlag.GET_ATTACHED_VOLUME_INFO);
                }

                if (checkBox_BlockUSBRead.Checked)
                {
                    GlobalConfig.VolumeControlFlag |= (uint)FilterAPI.VolumeControlFlag.BLOCK_USB_READ;
                }
                else
                {
                    GlobalConfig.VolumeControlFlag &= (uint)(~FilterAPI.VolumeControlFlag.BLOCK_USB_READ);
                }

                if (checkBox_BlockUSBWrite.Checked)
                {
                    GlobalConfig.VolumeControlFlag |= (uint)FilterAPI.VolumeControlFlag.BLOCK_USB_WRITE;
                }
                else
                {
                    GlobalConfig.VolumeControlFlag &= (uint)(~FilterAPI.VolumeControlFlag.BLOCK_USB_WRITE);
                }


                if (checkBox_SendBuffer.Checked)
                {
                    GlobalConfig.BooleanConfig |= FilterAPI.BooleanConfig.ENABLE_SEND_DATA_BUFFER;
                }
                else
                {
                    GlobalConfig.BooleanConfig &= (~FilterAPI.BooleanConfig.ENABLE_SEND_DATA_BUFFER);
                }

                GlobalConfig.FilterConnectionThreads = int.Parse(textBox_ConnectionThreads.Text);
                GlobalConfig.ConnectionTimeOut = int.Parse(textBox_ConnectionTimeout.Text);

                List<uint> protectPids = new List<uint>();
                if (textBox_ProtectedPID.Text.Trim().Length > 0)
                {
                    if (textBox_ProtectedPID.Text.EndsWith(";"))
                    {
                        textBox_ProtectedPID.Text = textBox_ProtectedPID.Text.Remove(textBox_ProtectedPID.Text.Length - 1);
                    }

                    string[] pids = textBox_ProtectedPID.Text.Split(new char[] { ';' });
                    for (int i = 0; i < pids.Length; i++)
                    {
                        protectPids.Add(uint.Parse(pids[i].Trim()));
                    }
                }

                GlobalConfig.ProtectPidList = protectPids;

                if (GlobalConfig.FileFilters.Count == 0)
                {
                    MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                    MessageBox.Show("There are no one filter rule added, the filter driver won't monitor any file.", "Filter Rule", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                GlobalConfig.SaveConfigSetting();

                this.Close();

            }
            catch (Exception ex)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("Save options failed with error " + ex.Message, "Save options.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button_SelectIncludePID_Click(object sender, EventArgs e)
        {

            OptionForm optionForm = new OptionForm(OptionForm.OptionType.ProccessId, textBox_ConnectionThreads.Text);

            if (optionForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox_ConnectionThreads.Text = optionForm.ProcessId;
            }
        }

        private void button_SelectExcludePID_Click(object sender, EventArgs e)
        {

            OptionForm optionForm = new OptionForm(OptionForm.OptionType.ProccessId, textBox_ConnectionTimeout.Text);

            if (optionForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox_ConnectionTimeout.Text = optionForm.ProcessId;
            }
        }


        private void button_SelectProtectPID_Click(object sender, EventArgs e)
        {
            OptionForm optionForm = new OptionForm(OptionForm.OptionType.ProccessId, textBox_ProtectedPID.Text);

            if (optionForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox_ProtectedPID.Text = optionForm.ProcessId;
            }
        }

        private void button_InfoProtectPid_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Prevent the processes from being terminated when the process Id list is not empty, multiple process Ids are supported.");
        }

        private void button_InfoConnectionThreads_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The number of the threads to communicate with the filter driver to get the events.");
        }

        private void button_InfoConnectionTimeout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The maximum timeout the filter driver wait for the IO event return in seconds.");
        }

        private void button_InfoMessageOutput_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Enable or disable the message displaying in console when there are notfication events were fired.");
        }

        private void button_InfoLogMessage_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Enable or disable logging the message to a file when there are notfication events were fired.");
        }

        private void button_InfoHideDirIO_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Enable or disable the directory IO message displaying in console when there are notfication events were fired.");
        }

        private void button_InfoGetVolumeInfo_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Get all the attached volume information from the filter driver when the service is started if it was enabled.");
        }

        private void button_InfoNewVolumeInfo_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Get the notification when the filter driver attached to a new volume if it was enabled.");
        }

        private void button_InfoSendBuffer_Click(object sender, EventArgs e)
        {
            MessageBox.Show("In order to reduce the stress of the events, it will send the read/write data when the read/write events were fired only when it was enabled.");
        }

        private void button_InfoBlockVolumeFormatting_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Prevent all the attached volumes from being formatting when it was enabled.");
        }

        private void button_InfoVolumeDetach_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Get the notification when a attached volume was detached if it was enabled.");
        }

        private void button_InfoBlockUSBWrite_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Block the write data to the USB disk.");
        }

        private void button_InfoBlockUSBRead_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Block the read data from the USB disk.");
        }

             
     
    }
}
