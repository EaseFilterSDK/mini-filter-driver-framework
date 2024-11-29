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

namespace ProcessMon
{
    public partial class ProcessFilterSettingCollection : Form
    {
        public ProcessFilterSettingCollection()
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
            listView_FilterRules.Columns.Add("ProcessId", 100, System.Windows.Forms.HorizontalAlignment.Left);
            listView_FilterRules.Columns.Add("ProcessNameMask", 200, System.Windows.Forms.HorizontalAlignment.Left);
            listView_FilterRules.Columns.Add("ControlFlag", 100, System.Windows.Forms.HorizontalAlignment.Left);
            listView_FilterRules.Columns.Add("FileAccessRights", 300, System.Windows.Forms.HorizontalAlignment.Left);

            foreach (ProcessFilterRule rule in GlobalConfig.ProcessFilterRules.Values)
            {
                AddItem(rule);
            }

        }

        private void AddItem(ProcessFilterRule newRule)
        {
            string[] itemStr = new string[listView_FilterRules.Columns.Count];
            itemStr[0] = listView_FilterRules.Items.Count.ToString();
            itemStr[1] = newRule.ProcessId;
            itemStr[2] = newRule.ProcessNameFilterMask;
            itemStr[3] = newRule.ControlFlag.ToString();
            itemStr[4] = newRule.FileAccessRights;
            ListViewItem item = new ListViewItem(itemStr, 0);
            item.Tag = newRule;

            foreach (ListViewItem lvItem in listView_FilterRules.Items)
            {
                if (string.Compare(((ProcessFilterRule)(lvItem.Tag)).ProcessNameFilterMask, newRule.ProcessNameFilterMask, true) == 0)
                {
                    listView_FilterRules.Items.Remove(lvItem);
                    break;
                }
            }

            listView_FilterRules.Items.Add(item);
        }


        private void button_AddFilter_Click(object sender, EventArgs e)
        {
            try
            {
                ProcessFilterSetting processFilterSetting = new ProcessFilterSetting(null);
                if (processFilterSetting.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    ProcessFilterRule newRule = processFilterSetting.selectedFilterRule;
                    AddItem(newRule);
                }
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

            listView_FilterRules.Items.Remove(listView_FilterRules.SelectedItems[0]);
        }
      

        private void button_ModifyFilter_Click(object sender, EventArgs e)
        {
            if (listView_FilterRules.SelectedItems.Count == 0)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("There are no filter rule selected.", "Delete Filter Rule", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ProcessFilterRule processFilter = (ProcessFilterRule)listView_FilterRules.SelectedItems[0].Tag;
            ProcessFilterSetting processFilterSetting = new ProcessFilterSetting(processFilter);

            if (processFilterSetting.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                listView_FilterRules.Items.Remove(listView_FilterRules.SelectedItems[0]);

                ProcessFilterRule modifiedProcessFilter = processFilterSetting.selectedFilterRule;
                AddItem(modifiedProcessFilter);
            }

        }

        private void button_Apply_Click_1(object sender, EventArgs e)
        {
            GlobalConfig.ClearProcessFilterRule();

            foreach(ListViewItem item in listView_FilterRules.Items)
            {
                GlobalConfig.AddProcessFilterRule((ProcessFilterRule)item.Tag);                
            }

            GlobalConfig.SaveConfigSetting();
        }

        private void button_ProcessInfo_Click(object sender, EventArgs e)
        {
            string information = "Process Control: allow/deny the binaries executing, prevent the suspicious binaries(malware) from executing.\r\n\r\n";
            information += "Get notification for process/thread creation and termination.\r\n\r\n";
            information += "Set the file access rights to the process.\r\n\r\n";

            MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
            MessageBox.Show(information, "Process Filter Setting", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

      
    }
}
