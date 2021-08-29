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
using System.IO;
using System.Windows.Forms;

namespace EaseFilter.CommonObjects
{
    public partial class RegistryAccessControlForm : Form
    {
        List<FilterRule> regFilterRuleList = new List<FilterRule>();

        public RegistryAccessControlForm()
        {
            InitializeComponent();

            regFilterRuleList.AddRange(GlobalConfig.FilterRules.Values);

            InitListView();

            textBox_AccessFlags.Text = FilterAPI.MAX_REGITRY_ACCESS_FLAG.ToString();
            textBox_RegistryCallbackClass.Text = "93092006832128"; //by default only register post callback class
            textBox_ProcessName.Text = Path.GetFileName(GlobalConfig.AssemblyName);


        }

        public void InitListView()
        {
            //init ListView control
            listView_FilterRules.Clear();		//clear control
            //create column header for ListView
            listView_FilterRules.Columns.Add("#", 20, System.Windows.Forms.HorizontalAlignment.Left);
            listView_FilterRules.Columns.Add("ProcessId", 50, System.Windows.Forms.HorizontalAlignment.Left);
            listView_FilterRules.Columns.Add("ProcessName", 100, System.Windows.Forms.HorizontalAlignment.Left);
            listView_FilterRules.Columns.Add("AccessFlags", 100, System.Windows.Forms.HorizontalAlignment.Left);
            listView_FilterRules.Columns.Add("CallbackClass", 100, System.Windows.Forms.HorizontalAlignment.Left);

            foreach (FilterRule rule in regFilterRuleList)
            {
                AddItem(rule);
            }

        }

        private void AddItem(FilterRule newRule)
        {
            string[] itemStr = new string[listView_FilterRules.Columns.Count];
            itemStr[0] = listView_FilterRules.Items.Count.ToString();
            itemStr[1] = newRule.IncludeProcessIds;
            itemStr[2] = newRule.IncludeProcessNames;
            itemStr[3] = newRule.RegistryControlFlag.ToString();
            itemStr[4] = newRule.RegistryCallbackClass.ToString();
            ListViewItem item = new ListViewItem(itemStr, 0);
            item.Tag = newRule;
            listView_FilterRules.Items.Add(item);
        }


        private void button_AddFilter_Click(object sender, EventArgs e)
        {
            try
            {
                FilterRule regFilterRule = new FilterRule();
                if (textBox_ProcessId.Text.Trim().Length > 0 )
                {
                    //please note that the process Id will be changed when the process launch every time.
                    regFilterRule.IncludeProcessIds = textBox_ProcessId.Text;
                }
                else
                {
                    regFilterRule.IncludeProcessIds = "";
                }

                if (textBox_ProcessName.Text.Trim().Length > 0)
                {
                    regFilterRule.IncludeProcessNames = textBox_ProcessName.Text;
                }
                else
                {
                    regFilterRule.IncludeProcessNames = "";
                }

                //this is the key of the filter rule for registry filter rule
                regFilterRule.IncludeFileFilterMask = regFilterRule.IncludeProcessIds + regFilterRule.IncludeProcessNames;
                regFilterRule.Type = (int)FilterAPI.FilterType.FILE_SYSTEM_REGISTRY;
                regFilterRule.IsExcludeFilter = checkBox_isExcludeFilter.Checked;
                regFilterRule.RegistryControlFlag = uint.Parse(textBox_AccessFlags.Text);
                regFilterRule.RegistryCallbackClass = ulong.Parse(textBox_RegistryCallbackClass.Text);


                regFilterRuleList.Add(regFilterRule);
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
                FilterRule filterRule = (FilterRule)item.Tag;
                regFilterRuleList.Remove(filterRule);
                GlobalConfig.RemoveFilterRule(filterRule.IncludeFileFilterMask);
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
            foreach (FilterRule regFilterRule in regFilterRuleList)
            {
                GlobalConfig.AddFilterRule(regFilterRule);
            }

            GlobalConfig.SaveConfigSetting();
        }

        private void button_Info_Click(object sender, EventArgs e)
        {
            string information = "Filter by process Id: Filter the registry access by process Id, skip the process Id check if it is 0.\r\n\r\n";
            information += "Filter by process name: Filter the registry access by process name filter mask, use '*' to include all processes, use ';' to seperate multiple process names.\r\n\r\n";
            information += "Filter by user name: Filter the registry access by user name with format 'domain name\\user name', use '*' to include all users.\r\n\r\n";
            information += "Registry access control: Set the registry access control flags to allow or deny the registry access.\r\n\r\n";
            information += "Registry callback setting: Set the registry access notification callback class, monitor the registry access by registering post-notification class, control the registry access by registering pre-notification class in user mode service.\r\n\r\n";
            information += "Exclude the registry filter rule: Skip the registry access for this filter rule if it is enabled.\r\n\r\n";

            MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
            MessageBox.Show(information, "Registry Filter Setting", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
