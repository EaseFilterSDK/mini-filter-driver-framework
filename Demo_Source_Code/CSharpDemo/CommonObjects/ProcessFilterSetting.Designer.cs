namespace EaseFilter.CommonObjects
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
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_ControlFlag = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.button_SelectControlFlag = new System.Windows.Forms.Button();
            this.textBox_ProcessName = new System.Windows.Forms.TextBox();
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
            this.groupBox_AccessControl.Size = new System.Drawing.Size(537, 246);
            this.groupBox_AccessControl.TabIndex = 26;
            this.groupBox_AccessControl.TabStop = false;
            // 
            // button_Save
            // 
            this.button_Save.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.button_Save.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_Save.Location = new System.Drawing.Point(376, 195);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(147, 23);
            this.button_Save.TabIndex = 85;
            this.button_Save.Text = "Save Process Filter Rule";
            this.button_Save.UseVisualStyleBackColor = false;
            this.button_Save.Click += new System.EventHandler(this.button_Save_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button_SelectPid);
            this.groupBox2.Controls.Add(this.textBox_ProcessId);
            this.groupBox2.Controls.Add(this.radioButton_Name);
            this.groupBox2.Controls.Add(this.radioButton_Pid);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.textBox_ControlFlag);
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Controls.Add(this.button_SelectControlFlag);
            this.groupBox2.Controls.Add(this.textBox_ProcessName);
            this.groupBox2.Location = new System.Drawing.Point(6, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(517, 161);
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(199, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(209, 13);
            this.label2.TabIndex = 90;
            this.label2.Text = "(  i.e. c:\\testfolder\\*.exe  or  \'notepad.exe\' )";
            // 
            // textBox_ControlFlag
            // 
            this.textBox_ControlFlag.Location = new System.Drawing.Point(201, 95);
            this.textBox_ControlFlag.Name = "textBox_ControlFlag";
            this.textBox_ControlFlag.ReadOnly = true;
            this.textBox_ControlFlag.Size = new System.Drawing.Size(242, 20);
            this.textBox_ControlFlag.TabIndex = 73;
            this.textBox_ControlFlag.Text = "0";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(6, 95);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(142, 13);
            this.label17.TabIndex = 72;
            this.label17.Text = "Process Access Control Flag";
            // 
            // button_SelectControlFlag
            // 
            this.button_SelectControlFlag.Location = new System.Drawing.Point(463, 95);
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
            // ProcessFilterSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(537, 246);
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
        private System.Windows.Forms.Button button_SelectPid;
        private System.Windows.Forms.TextBox textBox_ProcessId;
        private System.Windows.Forms.RadioButton radioButton_Name;
        private System.Windows.Forms.RadioButton radioButton_Pid;
        private System.Windows.Forms.Button button_Save;
    }
}