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
using System.Windows.Forms;

using EaseFilter.FilterControl;

namespace EaseFilter.CommonObjects
{
    public partial class RegistryAccessControlForm : Form
    {
        RegistryFilter selectedFilterRule = null;

        public RegistryAccessControlForm()
        {
            InitializeComponent();

            InitListView();
           
        }

        public void InitListView()
        {
            //init ListView control
            listView_FilterRules.Clear();		//clear control
            //create column header for ListView
            listView_FilterRules.Columns.Add("#", 20, System.Windows.Forms.HorizontalAlignment.Left);
            listView_FilterRules.Columns.Add("ProcessId", 50, System.Windows.Forms.HorizontalAlignment.Left);
            listView_FilterRules.Columns.Add("ProcessName", 100, System.Windows.Forms.HorizontalAlignment.Left);
            listView_FilterRules.Columns.Add("KeyNameMask", 100, System.Windows.Forms.HorizontalAlignment.Left);
            listView_FilterRules.Columns.Add("AccessFlags", 100, System.Windows.Forms.HorizontalAlignment.Left);
            listView_FilterRules.Columns.Add("CallbackClass", 100, System.Windows.Forms.HorizontalAlignment.Left);

            foreach (RegistryFilter registryFilter in GlobalConfig.RegistryFilters.Values)
            {
                AddItem(registryFilter);
                selectedFilterRule = registryFilter;
            }

            InitSelectedFilterRule();
        }

        void InitSelectedFilterRule()
        {
            if (null == selectedFilterRule)
            {
                selectedFilterRule = new RegistryFilter();

                selectedFilterRule.ControlFlag = FilterAPI.MAX_REGITRY_ACCESS_FLAG;
                selectedFilterRule.RegCallbackClass = 93092006832128; //by default only register post callback class
                selectedFilterRule.ProcessNameFilterMask = "*";
                selectedFilterRule.RegistryKeyNameFilterMask = "*";
            }

            if (selectedFilterRule.ProcessId> 0 )
            {
                textBox_ProcessId.Text = selectedFilterRule.ProcessId.ToString();
                radioButton_Pid_Click(null, null);
            }
            else
            {
                radioButton_Name_Click(null, null);
                textBox_ProcessName.Text = selectedFilterRule.ProcessNameFilterMask;
            }

            textBox_RegistryKeyNameFilterMask.Text = selectedFilterRule.RegistryKeyNameFilterMask;

            textBox_ExcludeProcessNames.Text = selectedFilterRule.ExcludeProcessNameString;
            textBox_ExcludeUserNames.Text = selectedFilterRule.ExcludeUserNameString;
            textBox_ExcludeKeyNames.Text = selectedFilterRule.ExcludeKeyNameString;

            textBox_AccessFlags.Text = selectedFilterRule.ControlFlag.ToString();
            textBox_RegistryCallbackClass.Text = selectedFilterRule.RegCallbackClass.ToString();
            checkBox_isExcludeFilter.Checked = selectedFilterRule.IsExcludeFilter;
        }

        private void AddItem(RegistryFilter newRule)
        {
            string[] itemStr = new string[listView_FilterRules.Columns.Count];
            itemStr[0] = listView_FilterRules.Items.Count.ToString();
            itemStr[1] = newRule.ProcessId.ToString();
            itemStr[2] = newRule.ProcessNameFilterMask;
            itemStr[3] = newRule.RegistryKeyNameFilterMask;
            itemStr[4] = newRule.ControlFlag.ToString();
            itemStr[5] = newRule.RegCallbackClass.ToString();
            ListViewItem item = new ListViewItem(itemStr, 0);
            item.Tag = newRule;
            listView_FilterRules.Items.Add(item);
        }


        private void button_AddFilter_Click(object sender, EventArgs e)
        {
            try
            {
                selectedFilterRule = new RegistryFilter();

                if (textBox_ProcessId.Text.Trim().Length > 0 && textBox_ProcessId.Text != "0")
                {
                    //please note that the process Id will be changed when the process launch every time.
                    selectedFilterRule.ProcessId = uint.Parse(textBox_ProcessId.Text);
                    selectedFilterRule.ProcessNameFilterMask = "";
                }
                else if (textBox_ProcessName.Text.Trim().Length > 0)
                {
                    selectedFilterRule.ProcessId = 0;
                    selectedFilterRule.ProcessNameFilterMask = textBox_ProcessName.Text;
                }
                else
                {
                    MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                    MessageBox.Show("The process name mask and Pid can't be null.", "Add Filter Rule", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                selectedFilterRule.RegistryKeyNameFilterMask = textBox_RegistryKeyNameFilterMask.Text;

                selectedFilterRule.ExcludeProcessNameString = textBox_ExcludeProcessNames.Text;
                selectedFilterRule.ExcludeUserNameString = textBox_ExcludeUserNames.Text;
                selectedFilterRule.ExcludeKeyNameString = textBox_ExcludeKeyNames.Text;

                //this is the key of the filter rule for registry filter rule
                selectedFilterRule.IsExcludeFilter = checkBox_isExcludeFilter.Checked;
                selectedFilterRule.ControlFlag = uint.Parse(textBox_AccessFlags.Text);
                selectedFilterRule.RegCallbackClass = ulong.Parse(textBox_RegistryCallbackClass.Text);

                GlobalConfig.AddRegistryFilter(selectedFilterRule);

                InitListView();

            }
            catch (Exception ex)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("Add registry filter rule failed." + ex.Message, "Add Filter Rule", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

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
                RegistryFilter registryFilter = (RegistryFilter)item.Tag;
                GlobalConfig.RemoveRegistryFilter(registryFilter);
            }

            InitListView();
        }

        private void button_SelectProcessId_Click(object sender, EventArgs e)
        {
            OptionForm optionForm = new OptionForm(OptionForm.OptionType.ProccessId, textBox_ProcessId.Text);

            if (optionForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox_ProcessId.Text = optionForm.ProcessId.ToString();
            }
        }

        private void button_SelectRegistryAccessFlags_Click(object sender, EventArgs e)
        {
            OptionForm optionForm = new OptionForm(OptionForm.OptionType.RegistryAccessFlag, textBox_AccessFlags.Text);

            if (optionForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox_AccessFlags.Text = optionForm.RegAccessFlags.ToString();
            }
        }

        private void button_SelectRegistryCallbackClass_Click(object sender, EventArgs e)
        {
            OptionForm optionForm = new OptionForm(OptionForm.OptionType.RegistryCallbackClass, textBox_RegistryCallbackClass.Text);

            if (optionForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox_RegistryCallbackClass.Text = optionForm.RegCallbackClass.ToString();
            }
        }

        private void button_Apply_Click(object sender, EventArgs e)
        {
            GlobalConfig.SaveConfigSetting();
        }

        private void button_Info_Click(object sender, EventArgs e)
        {
            string information = "Filter by process Id: Filter the registry access by process Id, skip the process Id check if it is 0.\r\n\r\n";
            information += "Filter by process name: Filter the registry access by process name filter mask, use '*' to include all processes, use ';' to seperate multiple process names.\r\n\r\n";
            information += "Filter by user name: Filter the registry access by user name with format 'domain name\\user name', use '*' to include all users.\r\n\r\n";
            information += "Filter by registry key name: Filter the registry access by the registry key name filter mask, use '*' to include all keys.\r\n\r\n";
            information += "Registry access control: Set the registry access control flags to allow or deny the registry access.\r\n\r\n";
            information += "Registry callback setting: Set the registry access notification callback class, monitor the registry access by registering post-notification class, control the registry access by registering pre-notification class in user mode service.\r\n\r\n";
            information += "Exclude the registry filter rule: Skip the registry access for this filter rule if it is enabled.\r\n\r\n";

            MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
            MessageBox.Show(information, "Registry Filter Setting", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void listView_FilterRules_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView_FilterRules.SelectedItems.Count > 0)
            {
                selectedFilterRule = (RegistryFilter)listView_FilterRules.SelectedItems[0].Tag;
                InitSelectedFilterRule();
            }
        }

       
        private void radioButton_Pid_Click(object sender, EventArgs e)
        {
            radioButton_Pid.Checked = true;
            textBox_ProcessName.ReadOnly = true;
            textBox_ProcessId.ReadOnly = false;
            button_SelectProcessId.Enabled = true;
        }

        private void radioButton_Name_Click(object sender, EventArgs e)
        {
            radioButton_Name.Checked = true;
            textBox_ProcessName.ReadOnly = false;
            textBox_ProcessId.ReadOnly = true;
            textBox_ProcessId.Text = "0";
            button_SelectProcessId.Enabled = false;
        }

     
    }
}
