namespace ProcessMon
{
    partial class ProcessFilterSetting
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProcessFilterSetting));
            this.groupBox_AccessControl = new System.Windows.Forms.GroupBox();
            this.button_Save = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button_SelectPid = new System.Windows.Forms.Button();
            this.textBox_ProcessId = new System.Windows.Forms.TextBox();
            this.radioButton_Name = new System.Windows.Forms.RadioButton();
            this.radioButton_Pid = new System.Windows.Forms.RadioButton();
            this.button_ConfigFileAccessRights = new System.Windows.Forms.Button();
            this.button_ProcessInfo = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_ControlFlag = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.button_SelectControlFlag = new System.Windows.Forms.Button();
            this.textBox_ProcessName = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.textBox_ExcludeUserNames = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.textBox_ExcludeProcessNames = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox_AccessControl.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox_AccessControl
            // 
            this.groupBox_AccessControl.Controls.Add(this.button_Save);
            this.groupBox_AccessControl.Controls.Add(this.groupBox2);
            this.groupBox_AccessControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_AccessControl.Location = new System.Drawing.Point(0, 0);
            this.groupBox_AccessControl.Name = "groupBox_AccessControl";
            this.groupBox_AccessControl.Size = new System.Drawing.Size(537, 349);
            this.groupBox_AccessControl.TabIndex = 26;
            this.groupBox_AccessControl.TabStop = false;
            // 
            // button_Save
            // 
            this.button_Save.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.button_Save.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_Save.Location = new System.Drawing.Point(407, 314);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(116, 23);
            this.button_Save.TabIndex = 85;
            this.button_Save.Text = "Save Filter Rule";
            this.button_Save.UseVisualStyleBackColor = false;
            this.button_Save.Click += new System.EventHandler(this.button_Save_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label20);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.textBox_ExcludeUserNames);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.textBox_ExcludeProcessNames);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.button_SelectPid);
            this.groupBox2.Controls.Add(this.textBox_ProcessId);
            this.groupBox2.Controls.Add(this.radioButton_Name);
            this.groupBox2.Controls.Add(this.radioButton_Pid);
            this.groupBox2.Controls.Add(this.button_ConfigFileAccessRights);
            this.groupBox2.Controls.Add(this.button_ProcessInfo);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.textBox_ControlFlag);
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Controls.Add(this.button_SelectControlFlag);
            this.groupBox2.Controls.Add(this.textBox_ProcessName);
            this.groupBox2.Location = new System.Drawing.Point(6, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(517, 296);
            this.groupBox2.TabIndex = 76;
            this.groupBox2.TabStop = false;
            // 
            // button_SelectPid
            // 
            this.button_SelectPid.Location = new System.Drawing.Point(463, 19);
            this.button_SelectPid.Name = "button_SelectPid";
            this.button_SelectPid.Size = new System.Drawing.Size(41, 20);
            this.button_SelectPid.TabIndex = 102;
            this.button_SelectPid.Text = "...";
            this.button_SelectPid.UseVisualStyleBackColor = true;
            this.button_SelectPid.Click += new System.EventHandler(this.button_SelectPid_Click);
            // 
            // textBox_ProcessId
            // 
            this.textBox_ProcessId.Location = new System.Drawing.Point(202, 19);
            this.textBox_ProcessId.Name = "textBox_ProcessId";
            this.textBox_ProcessId.Size = new System.Drawing.Size(242, 20);
            this.textBox_ProcessId.TabIndex = 101;
            this.textBox_ProcessId.Text = "0";
            // 
            // radioButton_Name
            // 
            this.radioButton_Name.AutoSize = true;
            this.radioButton_Name.Location = new System.Drawing.Point(8, 51);
            this.radioButton_Name.Name = "radioButton_Name";
            this.radioButton_Name.Size = new System.Drawing.Size(134, 17);
            this.radioButton_Name.TabIndex = 100;
            this.radioButton_Name.Text = "Filter By Process Name";
            this.radioButton_Name.UseVisualStyleBackColor = true;
            this.radioButton_Name.Click += new System.EventHandler(this.radioButton_Name_Click);
            // 
            // radioButton_Pid
            // 
            this.radioButton_Pid.AutoSize = true;
            this.radioButton_Pid.Checked = true;
            this.radioButton_Pid.Location = new System.Drawing.Point(8, 19);
            this.radioButton_Pid.Name = "radioButton_Pid";
            this.radioButton_Pid.Size = new System.Drawing.Size(114, 17);
            this.radioButton_Pid.TabIndex = 99;
            this.radioButton_Pid.TabStop = true;
            this.radioButton_Pid.Text = "Filter by Process Id";
            this.radioButton_Pid.UseVisualStyleBackColor = true;
            this.radioButton_Pid.Click += new System.EventHandler(this.radioButton_Pid_Click);
            // 
            // button_ConfigFileAccessRights
            // 
            this.button_ConfigFileAccessRights.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.button_ConfigFileAccessRights.Location = new System.Drawing.Point(201, 245);
            this.button_ConfigFileAccessRights.Name = "button_ConfigFileAccessRights";
            this.button_ConfigFileAccessRights.Size = new System.Drawing.Size(241, 20);
            this.button_ConfigFileAccessRights.TabIndex = 98;
            this.button_ConfigFileAccessRights.Text = "Add File Access Control To Process";
            this.button_ConfigFileAccessRights.UseVisualStyleBackColor = false;
            this.button_ConfigFileAccessRights.Click += new System.EventHandler(this.button_ConfigFileAccessRights_Click);
            // 
            // button_ProcessInfo
            // 
            this.button_ProcessInfo.BackColor = System.Drawing.Color.AntiqueWhite;
            this.button_ProcessInfo.Image = global::ProcessMon.Properties.Resources.about;
            this.button_ProcessInfo.Location = new System.Drawing.Point(463, 245);
            this.button_ProcessInfo.Name = "button_ProcessInfo";
            this.button_ProcessInfo.Size = new System.Drawing.Size(41, 20);
            this.button_ProcessInfo.TabIndex = 97;
            this.button_ProcessInfo.UseVisualStyleBackColor = false;
            this.button_ProcessInfo.Click += new System.EventHandler(this.button_ProcessInfo_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(199, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(209, 13);
            this.label2.TabIndex = 90;
            this.label2.Text = "(  i.e. c:\\testfolder\\*.exe  or  \'notepad.exe\' )";
            // 
            // textBox_ControlFlag
            // 
            this.textBox_ControlFlag.Location = new System.Drawing.Point(201, 203);
            this.textBox_ControlFlag.Name = "textBox_ControlFlag";
            this.textBox_ControlFlag.ReadOnly = true;
            this.textBox_ControlFlag.Size = new System.Drawing.Size(242, 20);
            this.textBox_ControlFlag.TabIndex = 73;
            this.textBox_ControlFlag.Text = "0";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(6, 203);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(142, 13);
            this.label17.TabIndex = 72;
            this.label17.Text = "Process Access Control Flag";
            // 
            // button_SelectControlFlag
            // 
            this.button_SelectControlFlag.Location = new System.Drawing.Point(463, 203);
            this.button_SelectControlFlag.Name = "button_SelectControlFlag";
            this.button_SelectControlFlag.Size = new System.Drawing.Size(41, 20);
            this.button_SelectControlFlag.TabIndex = 74;
            this.button_SelectControlFlag.Text = "...";
            this.button_SelectControlFlag.UseVisualStyleBackColor = true;
            this.button_SelectControlFlag.Click += new System.EventHandler(this.button_SelectControlFlag_Click);
            // 
            // textBox_ProcessName
            // 
            this.textBox_ProcessName.Location = new System.Drawing.Point(201, 49);
            this.textBox_ProcessName.Name = "textBox_ProcessName";
            this.textBox_ProcessName.ReadOnly = true;
            this.textBox_ProcessName.Size = new System.Drawing.Size(242, 20);
            this.textBox_ProcessName.TabIndex = 77;
            this.textBox_ProcessName.Text = "notepad.exe";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(200, 119);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(168, 12);
            this.label20.TabIndex = 128;
            this.label20.Text = "( split with \';\' i.e \"*test1.exe;*test2.exe\")";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(200, 171);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(245, 12);
            this.label13.TabIndex = 127;
            this.label13.Text = "(split with \';\' for multiple items, format \"domain\\username\" )";
            // 
            // textBox_ExcludeUserNames
            // 
            this.textBox_ExcludeUserNames.Location = new System.Drawing.Point(202, 148);
            this.textBox_ExcludeUserNames.Name = "textBox_ExcludeUserNames";
            this.textBox_ExcludeUserNames.Size = new System.Drawing.Size(242, 20);
            this.textBox_ExcludeUserNames.TabIndex = 126;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 152);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(102, 13);
            this.label14.TabIndex = 125;
            this.label14.Text = "Exclude user names";
            // 
            // textBox_ExcludeProcessNames
            // 
            this.textBox_ExcludeProcessNames.Location = new System.Drawing.Point(201, 96);
            this.textBox_ExcludeProcessNames.Name = "textBox_ExcludeProcessNames";
            this.textBox_ExcludeProcessNames.Size = new System.Drawing.Size(242, 20);
            this.textBox_ExcludeProcessNames.TabIndex = 122;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(5, 99);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(119, 13);
            this.label11.TabIndex = 121;
            this.label11.Text = "Exclude process names";
            // 
            // ProcessFilterSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(537, 349);
            this.Controls.Add(this.groupBox_AccessControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ProcessFilterSetting";
            this.Text = "Process Filter Rule Setting";
            this.groupBox_AccessControl.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox_AccessControl;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_ControlFlag;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button button_SelectControlFlag;
        private System.Windows.Forms.TextBox textBox_ProcessName;
        private System.Windows.Forms.Button button_ProcessInfo;
        private System.Windows.Forms.Button button_SelectPid;
        private System.Windows.Forms.TextBox textBox_ProcessId;
        private System.Windows.Forms.RadioButton radioButton_Name;
        private System.Windows.Forms.RadioButton radioButton_Pid;
        private System.Windows.Forms.Button button_ConfigFileAccessRights;
        private System.Windows.Forms.Button button_Save;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textBox_ExcludeUserNames;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox textBox_ExcludeProcessNames;
        private System.Windows.Forms.Label label11;
    }
}