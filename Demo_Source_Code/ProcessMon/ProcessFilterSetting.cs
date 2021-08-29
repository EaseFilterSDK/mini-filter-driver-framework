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
    public partial class ProcessFilterSetting : Form
    {
        public ProcessFilterRule  selectedFilterRule = null;
        bool isNewFilterRule = false;

        public ProcessFilterSetting(ProcessFilterRule _selectedFilterRule)
        {
            StartPosition = FormStartPosition.CenterParent;
            InitializeComponent();
            selectedFilterRule = _selectedFilterRule;

            InitSelectedFilterRule();
        }

        private void InitSelectedFilterRule()
        {
            if (null == selectedFilterRule)
            {
                selectedFilterRule = new ProcessFilterRule();
                selectedFilterRule.ProcessNameFilterMask = "*";
                selectedFilterRule.ControlFlag = 16130;

                isNewFilterRule = true;
            }

            if (selectedFilterRule.ProcessId.Length > 0 && selectedFilterRule.ProcessId != "0" )
            {
                textBox_ProcessId.Text = selectedFilterRule.ProcessId; 
                radioButton_Pid_Click(null,null);
            }
            else
            {
                radioButton_Name_Click(null, null);
                textBox_ProcessName.Text = selectedFilterRule.ProcessNameFilterMask;
                textBox_ControlFlag.Text = selectedFilterRule.ControlFlag.ToString();
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

        private void button_Apply_Click_1(object sender, EventArgs e)
        {
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

  
        private void radioButton_Pid_Click(object sender, EventArgs e)
        {
            radioButton_Pid.Checked = true;
            textBox_ProcessName.ReadOnly = true;
            textBox_ProcessId.ReadOnly = false;
            button_SelectPid.Enabled = true;
        }

        private void radioButton_Name_Click(object sender, EventArgs e)
        {
            radioButton_Name.Checked = true;
            textBox_ProcessName.ReadOnly = false;
            textBox_ProcessId.ReadOnly = true;
            textBox_ProcessId.Text = "0";
            button_SelectPid.Enabled = false;
        }

        private void button_SelectPid_Click(object sender, EventArgs e)
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

        private void button_ConfigFileAccessRights_Click(object sender, EventArgs e)
        {

            if (textBox_ProcessId.Text.Trim().Length > 0 && textBox_ProcessId.Text != "0" )
            {
                if (selectedFilterRule.ProcessId != textBox_ProcessId.Text)
                {
                    selectedFilterRule = new ProcessFilterRule();
                    isNewFilterRule = true;
                }

                selectedFilterRule.ProcessId = textBox_ProcessId.Text;
                
            }
            else if (textBox_ProcessName.Text.Trim().Length > 0)
            {
                if (selectedFilterRule.ProcessNameFilterMask != textBox_ProcessName.Text)
                {
                    isNewFilterRule = true;
                    selectedFilterRule = new ProcessFilterRule();
                }

                selectedFilterRule.ProcessNameFilterMask = textBox_ProcessName.Text;
               
            }
            else
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("The process name mask and Pid can't be null.", "Add Filter Rule", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            uint controlFlag = 0;

            if (uint.TryParse(textBox_ControlFlag.Text, out controlFlag) && ((controlFlag & (uint)FilterAPI.ProcessControlFlag.DENY_NEW_PROCESS_CREATION )> 0))
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("The process was blocked to executed, no need to set the file access rights for the process.", "Add Filter Rule", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (isNewFilterRule)
            {
                //default file access rights setting for new process filter rule

                //allow the windows dll or exe to be read by the process, or it can't be loaded.
                selectedFilterRule.FileAccessRights = "c:\\test\\*!" + FilterAPI.ALLOW_FILE_READ_ACCESS + ";";

            }

            ProcessFileAccessRights processFileAccessRigths = new ProcessFileAccessRights(selectedFilterRule);
            processFileAccessRigths.ShowDialog();
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            if (textBox_ProcessId.Text.Trim().Length > 0 && textBox_ProcessId.Text != "0")
            {
                //please note that the process Id will be changed when the process launch every time.
                selectedFilterRule.ProcessId = textBox_ProcessId.Text;
                selectedFilterRule.ProcessNameFilterMask = "";
            }
            else if (textBox_ProcessName.Text.Trim().Length > 0)
            {
                selectedFilterRule.ProcessId = "";
                selectedFilterRule.ProcessNameFilterMask = textBox_ProcessName.Text;
            }
            else
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("The process name mask and Pid can't be null.", "Add Filter Rule", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            uint controlFlag = 0;
            uint.TryParse(textBox_ControlFlag.Text, out controlFlag);
            selectedFilterRule.ControlFlag = controlFlag;

            GlobalConfig.AddProcessFilterRule(selectedFilterRule);

        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
        }

      
    }
}
