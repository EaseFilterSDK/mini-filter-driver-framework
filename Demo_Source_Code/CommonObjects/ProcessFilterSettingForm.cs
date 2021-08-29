using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EaseFilter.CommonObjects
{
    public partial class ProcessFilterSettingForm : Form
    {
        List<FilterRule> filterRuleList = new List<FilterRule>();

        public ProcessFilterSettingForm()
        {
            InitializeComponent();
            filterRuleList.AddRange(GlobalConfig.FilterRules.Values);
            InitListView();
        }

        public void InitListView()
        {
            //init ListView control
            listView_FilterRules.Clear();		//clear control
            //create column header for ListView
            listView_FilterRules.Columns.Add("#", 20, System.Windows.Forms.HorizontalAlignment.Left);
            listView_FilterRules.Columns.Add("ProcessNameMask", 200, System.Windows.Forms.HorizontalAlignment.Left);
            listView_FilterRules.Columns.Add("ControlFlag", 100, System.Windows.Forms.HorizontalAlignment.Left);

            foreach (FilterRule rule in filterRuleList)
            {
                AddItem(rule);
            }

        }

        private void AddItem(FilterRule newRule)
        {
            if (newRule.Type == (uint)FilterAPI.FilterType.FILE_SYSTEM_PROCESS && newRule.ProcessControlFlag > 0 )
            {
                string[] itemStr = new string[listView_FilterRules.Columns.Count];
                itemStr[0] = listView_FilterRules.Items.Count.ToString();
                itemStr[1] = newRule.IncludeFileFilterMask;
                itemStr[2] = newRule.ProcessControlFlag.ToString();
                ListViewItem item = new ListViewItem(itemStr, 0);
                item.Tag = newRule;
                listView_FilterRules.Items.Add(item);
            }
        }


        private void button_SelectControlFlag_Click(object sender, EventArgs e)
        {
            OptionForm optionForm = new OptionForm(OptionForm.OptionType.ProcessControlFlag, textBox_ControlFlag.Text);

            if (optionForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox_ControlFlag.Text = optionForm.ProcessControlFlag.ToString();
            }
        }


        private void button_AddFilter_Click(object sender, EventArgs e)
        {
            try
            {
                FilterRule filterRule = new FilterRule();

                filterRule.Type = (int)FilterAPI.FilterType.FILE_SYSTEM_PROCESS;

                if (textBox_ProcessName.Text.Trim().Length > 0)
                {
                    filterRule.IncludeFileFilterMask = textBox_ProcessName.Text;
                }
                else
                {
                    MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                    MessageBox.Show("The process name mask can't be null.", "Add Filter Rule", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    filterRule.ProcessControlFlag = uint.Parse(textBox_ControlFlag.Text);
                }
                catch
                {
                }

                if (filterRule.ProcessControlFlag == 0)
                {
                    MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                    MessageBox.Show("The process control flag is empty, the entry won't be added.", "Add Filter Rule", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                filterRuleList.Add(filterRule);
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
                filterRuleList.Remove(filterRule);

                GlobalConfig.RemoveFilterRule(filterRule.IncludeFileFilterMask);
            }

            InitListView();
        }

        private void button_Apply_Click_1(object sender, EventArgs e)
        {
            foreach (FilterRule filterRule in filterRuleList)
            {
                GlobalConfig.AddFilterRule(filterRule);
            }

            GlobalConfig.SaveConfigSetting();
        }

        private void button_ProcessInfo_Click(object sender, EventArgs e)
        {
            string information = "Process Control: allow/deny the binaries executing, prevent the suspicious binaries(malware) from executing.\r\n\r\n";
            information += "Get notification for process/thread creation and termination.\r\n\r\n";

            MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
            MessageBox.Show(information, "Process Filter Setting", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void listView_FilterRules_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView_FilterRules.SelectedItems.Count > 0)
            {
                FilterRule filterRule = (FilterRule)listView_FilterRules.SelectedItems[0].Tag;
                textBox_ProcessName.Text = filterRule.IncludeFileFilterMask;
                textBox_ControlFlag.Text = filterRule.ProcessControlFlag.ToString();

            }
        }

      
    }
}
