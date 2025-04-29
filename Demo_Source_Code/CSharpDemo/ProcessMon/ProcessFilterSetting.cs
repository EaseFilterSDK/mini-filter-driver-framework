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
        public ProcessFilter  selectedProcessFilter = null;
        bool isNewFilterRule = false;

        public ProcessFilterSetting(ProcessFilter _selectedFilterRule)
        {
            StartPosition = FormStartPosition.CenterParent;
            InitializeComponent();
            selectedProcessFilter = _selectedFilterRule;

            InitSelectedFilterRule();
        }

        private void InitSelectedFilterRule()
        {
            if (null == selectedProcessFilter)
            {
                selectedProcessFilter = new ProcessFilter("*");
                selectedProcessFilter.ControlFlag = 6922;

                isNewFilterRule = true;
            }

            if (selectedProcessFilter.ProcessId > 0 )
            {
                textBox_ProcessId.Text = selectedProcessFilter.ProcessId.ToString(); 
                radioButton_Pid_Click(null,null);
            }
            else
            {
                radioButton_Name_Click(null, null);
                textBox_ProcessName.Text = selectedProcessFilter.ProcessNameFilterMask;
                textBox_ControlFlag.Text = selectedProcessFilter.ControlFlag.ToString();
            }

            textBox_ExcludeProcessNames.Text = selectedProcessFilter.ExcludeProcessNameString;
            textBox_ExcludeUserNames.Text = selectedProcessFilter.ExcludeUserNameString;
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
                if (selectedProcessFilter.ProcessId != uint.Parse(textBox_ProcessId.Text))
                {
                    selectedProcessFilter = new ProcessFilter("");
                    isNewFilterRule = true;
                }

                selectedProcessFilter.ProcessId = uint.Parse(textBox_ProcessId.Text);
                
            }
            else if (textBox_ProcessName.Text.Trim().Length > 0)
            {
                if (selectedProcessFilter.ProcessNameFilterMask != textBox_ProcessName.Text)
                {
                    isNewFilterRule = true;
                    selectedProcessFilter = new ProcessFilter(textBox_ProcessName.Text);
                }

                selectedProcessFilter.ProcessNameFilterMask = textBox_ProcessName.Text;
               
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
                selectedProcessFilter.FileAccessRightString = "c:\\test\\*|" + FilterAPI.ALLOW_FILE_READ_ACCESS + ";";

            }

            ProcessFileAccessRights processFileAccessRights = new ProcessFileAccessRights(selectedProcessFilter);
            processFileAccessRights.ShowDialog();
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            if (textBox_ProcessId.Text.Trim().Length > 0 && textBox_ProcessId.Text != "0")
            {
                //please note that the process Id will be changed when the process launch every time.
                selectedProcessFilter.ProcessId = uint.Parse(textBox_ProcessId.Text);
                selectedProcessFilter.ProcessNameFilterMask = "";
            }
            else if (textBox_ProcessName.Text.Trim().Length > 0)
            {
                selectedProcessFilter.ProcessId = 0;
                selectedProcessFilter.ProcessNameFilterMask = textBox_ProcessName.Text;
            }
            else
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("The process name mask and Pid can't be null.", "Add Filter Rule", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            selectedProcessFilter.ExcludeProcessNameString = textBox_ExcludeProcessNames.Text;
            selectedProcessFilter.ExcludeUserNameString = textBox_ExcludeUserNames.Text;

            uint controlFlag = 0;
            uint.TryParse(textBox_ControlFlag.Text, out controlFlag);
            selectedProcessFilter.ControlFlag = controlFlag;

            GlobalConfig.AddProcessFilter(selectedProcessFilter);

        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
        }

      
    }
}
