namespace EaseFilter.CommonObjects
{
    partial class RegistryAccessControlForm
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
            this.groupBox_AccessControl = new System.Windows.Forms.GroupBox();
            this.button_Apply = new System.Windows.Forms.Button();
            this.button_DeleteFilter = new System.Windows.Forms.Button();
            this.button_AddFilter = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listView_FilterRules = new System.Windows.Forms.ListView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_RegistryKeyNameFilterMask = new System.Windows.Forms.TextBox();
            this.radioButton_Name = new System.Windows.Forms.RadioButton();
            this.radioButton_Pid = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.button_Info = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox_isExcludeFilter = new System.Windows.Forms.CheckBox();
            this.button_SelectRegistryCallbackClass = new System.Windows.Forms.Button();
            this.textBox_AccessFlags = new System.Windows.Forms.TextBox();
            this.textBox_RegistryCallbackClass = new System.Windows.Forms.TextBox();
            this.button_SelectProcessId = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_ProcessId = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.button_SelectRegistryAccessFlags = new System.Windows.Forms.Button();
            this.textBox_ProcessName = new System.Windows.Forms.TextBox();
            this.groupBox_AccessControl.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox_AccessControl
            // 
            this.groupBox_AccessControl.Controls.Add(this.button_Apply);
            this.groupBox_AccessControl.Controls.Add(this.button_DeleteFilter);
            this.groupBox_AccessControl.Controls.Add(this.button_AddFilter);
            this.groupBox_AccessControl.Controls.Add(this.groupBox1);
            this.groupBox_AccessControl.Controls.Add(this.groupBox2);
            this.groupBox_AccessControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_AccessControl.Location = new System.Drawing.Point(0, 0);
            this.groupBox_AccessControl.Name = "groupBox_AccessControl";
            this.groupBox_AccessControl.Size = new System.Drawing.Size(546, 459);
            this.groupBox_AccessControl.TabIndex = 25;
            this.groupBox_AccessControl.TabStop = false;
            // 
            // button_Apply
            // 
            this.button_Apply.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_Apply.Location = new System.Drawing.Point(375, 411);
            this.button_Apply.Name = "button_Apply";
            this.button_Apply.Size = new System.Drawing.Size(138, 23);
            this.button_Apply.TabIndex = 80;
            this.button_Apply.Text = "Apply Filter Rule Settings";
            this.button_Apply.UseVisualStyleBackColor = true;
            this.button_Apply.Click += new System.EventHandler(this.button_Apply_Click);
            // 
            // button_DeleteFilter
            // 
            this.button_DeleteFilter.Location = new System.Drawing.Point(190, 411);
            this.button_DeleteFilter.Name = "button_DeleteFilter";
            this.button_DeleteFilter.Size = new System.Drawing.Size(103, 23);
            this.button_DeleteFilter.TabIndex = 79;
            this.button_DeleteFilter.Text = "Delete Filter Rule";
            this.button_DeleteFilter.UseVisualStyleBackColor = true;
            this.button_DeleteFilter.Click += new System.EventHandler(this.button_DeleteFilter_Click);
            // 
            // button_AddFilter
            // 
            this.button_AddFilter.Location = new System.Drawing.Point(6, 411);
            this.button_AddFilter.Name = "button_AddFilter";
            this.button_AddFilter.Size = new System.Drawing.Size(116, 23);
            this.button_AddFilter.TabIndex = 78;
            this.button_AddFilter.Text = "Add New Filter Rule";
            this.button_AddFilter.UseVisualStyleBackColor = true;
            this.button_AddFilter.Click += new System.EventHandler(this.button_AddFilter_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listView_FilterRules);
            this.groupBox1.Location = new System.Drawing.Point(6, 306);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(517, 93);
            this.groupBox1.TabIndex = 77;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Registry Filter Rule Collection";
            // 
            // listView_FilterRules
            // 
            this.listView_FilterRules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_FilterRules.FullRowSelect = true;
            this.listView_FilterRules.Location = new System.Drawing.Point(3, 16);
            this.listView_FilterRules.Name = "listView_FilterRules";
            this.listView_FilterRules.Size = new System.Drawing.Size(511, 74);
            this.listView_FilterRules.TabIndex = 1;
            this.listView_FilterRules.UseCompatibleStateImageBehavior = false;
            this.listView_FilterRules.View = System.Windows.Forms.View.Details;
            this.listView_FilterRules.SelectedIndexChanged += new System.EventHandler(this.listView_FilterRules_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.textBox_RegistryKeyNameFilterMask);
            this.groupBox2.Controls.Add(this.radioButton_Name);
            this.groupBox2.Controls.Add(this.radioButton_Pid);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.button_Info);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.checkBox_isExcludeFilter);
            this.groupBox2.Controls.Add(this.button_SelectRegistryCallbackClass);
            this.groupBox2.Controls.Add(this.textBox_AccessFlags);
            this.groupBox2.Controls.Add(this.textBox_RegistryCallbackClass);
            this.groupBox2.Controls.Add(this.button_SelectProcessId);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.textBox_ProcessId);
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Controls.Add(this.button_SelectRegistryAccessFlags);
            this.groupBox2.Controls.Add(this.textBox_ProcessName);
            this.groupBox2.Location = new System.Drawing.Point(6, 19);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(517, 281);
            this.groupBox2.TabIndex = 76;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Add or Edit The Registry Filter Rule Entry";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 111);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(174, 15);
            this.label3.TabIndex = 105;
            this.label3.Text = "Registry Key Name Filter Mask";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(201, 132);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(208, 15);
            this.label5.TabIndex = 104;
            this.label5.Text = "(key name filter mask, i.e. *Windows*";
            // 
            // textBox_RegistryKeyNameFilterMask
            // 
            this.textBox_RegistryKeyNameFilterMask.Location = new System.Drawing.Point(203, 111);
            this.textBox_RegistryKeyNameFilterMask.Name = "textBox_RegistryKeyNameFilterMask";
            this.textBox_RegistryKeyNameFilterMask.Size = new System.Drawing.Size(242, 20);
            this.textBox_RegistryKeyNameFilterMask.TabIndex = 103;
            this.textBox_RegistryKeyNameFilterMask.Text = "*";
            // 
            // radioButton_Name
            // 
            this.radioButton_Name.AutoSize = true;
            this.radioButton_Name.Location = new System.Drawing.Point(9, 52);
            this.radioButton_Name.Name = "radioButton_Name";
            this.radioButton_Name.Size = new System.Drawing.Size(155, 19);
            this.radioButton_Name.TabIndex = 102;
            this.radioButton_Name.Text = "Filter By Process Name";
            this.radioButton_Name.UseVisualStyleBackColor = true;
            this.radioButton_Name.Click += new System.EventHandler(this.radioButton_Name_Click);
            // 
            // radioButton_Pid
            // 
            this.radioButton_Pid.AutoSize = true;
            this.radioButton_Pid.Checked = true;
            this.radioButton_Pid.Location = new System.Drawing.Point(9, 20);
            this.radioButton_Pid.Name = "radioButton_Pid";
            this.radioButton_Pid.Size = new System.Drawing.Size(130, 19);
            this.radioButton_Pid.TabIndex = 101;
            this.radioButton_Pid.TabStop = true;
            this.radioButton_Pid.Text = "Filter by Process Id";
            this.radioButton_Pid.UseVisualStyleBackColor = true;
            this.radioButton_Pid.Click += new System.EventHandler(this.radioButton_Pid_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(201, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(171, 15);
            this.label4.TabIndex = 93;
            this.label4.Text = "(skip processId check if it is 0 )";
            // 
            // button_Info
            // 
            this.button_Info.Location = new System.Drawing.Point(465, 224);
            this.button_Info.Name = "button_Info";
            this.button_Info.Size = new System.Drawing.Size(41, 20);
            this.button_Info.TabIndex = 92;
            this.button_Info.UseVisualStyleBackColor = true;
            this.button_Info.Click += new System.EventHandler(this.button_Info_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(201, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(177, 15);
            this.label2.TabIndex = 90;
            this.label2.Text = "(use * to include all processes )";
            // 
            // checkBox_isExcludeFilter
            // 
            this.checkBox_isExcludeFilter.AutoSize = true;
            this.checkBox_isExcludeFilter.Location = new System.Drawing.Point(203, 224);
            this.checkBox_isExcludeFilter.Margin = new System.Windows.Forms.Padding(1);
            this.checkBox_isExcludeFilter.Name = "checkBox_isExcludeFilter";
            this.checkBox_isExcludeFilter.Size = new System.Drawing.Size(198, 19);
            this.checkBox_isExcludeFilter.TabIndex = 89;
            this.checkBox_isExcludeFilter.Text = "Is Excluded Registry Filter Rule";
            this.checkBox_isExcludeFilter.UseVisualStyleBackColor = true;
            // 
            // button_SelectRegistryCallbackClass
            // 
            this.button_SelectRegistryCallbackClass.Location = new System.Drawing.Point(465, 194);
            this.button_SelectRegistryCallbackClass.Name = "button_SelectRegistryCallbackClass";
            this.button_SelectRegistryCallbackClass.Size = new System.Drawing.Size(41, 20);
            this.button_SelectRegistryCallbackClass.TabIndex = 88;
            this.button_SelectRegistryCallbackClass.Text = "...";
            this.button_SelectRegistryCallbackClass.UseVisualStyleBackColor = true;
            this.button_SelectRegistryCallbackClass.Click += new System.EventHandler(this.button_SelectRegistryCallbackClass_Click);
            // 
            // textBox_AccessFlags
            // 
            this.textBox_AccessFlags.Location = new System.Drawing.Point(203, 158);
            this.textBox_AccessFlags.Name = "textBox_AccessFlags";
            this.textBox_AccessFlags.ReadOnly = true;
            this.textBox_AccessFlags.Size = new System.Drawing.Size(242, 20);
            this.textBox_AccessFlags.TabIndex = 73;
            this.textBox_AccessFlags.Text = "0";
            // 
            // textBox_RegistryCallbackClass
            // 
            this.textBox_RegistryCallbackClass.Location = new System.Drawing.Point(203, 194);
            this.textBox_RegistryCallbackClass.Name = "textBox_RegistryCallbackClass";
            this.textBox_RegistryCallbackClass.ReadOnly = true;
            this.textBox_RegistryCallbackClass.Size = new System.Drawing.Size(242, 20);
            this.textBox_RegistryCallbackClass.TabIndex = 87;
            this.textBox_RegistryCallbackClass.Text = "0";
            // 
            // button_SelectProcessId
            // 
            this.button_SelectProcessId.Location = new System.Drawing.Point(466, 17);
            this.button_SelectProcessId.Name = "button_SelectProcessId";
            this.button_SelectProcessId.Size = new System.Drawing.Size(41, 20);
            this.button_SelectProcessId.TabIndex = 14;
            this.button_SelectProcessId.Text = "...";
            this.button_SelectProcessId.UseVisualStyleBackColor = true;
            this.button_SelectProcessId.Click += new System.EventHandler(this.button_SelectProcessId_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 199);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(183, 15);
            this.label1.TabIndex = 86;
            this.label1.Text = "Register Registry Callback Class";
            // 
            // textBox_ProcessId
            // 
            this.textBox_ProcessId.Location = new System.Drawing.Point(203, 17);
            this.textBox_ProcessId.Name = "textBox_ProcessId";
            this.textBox_ProcessId.Size = new System.Drawing.Size(242, 20);
            this.textBox_ProcessId.TabIndex = 11;
            this.textBox_ProcessId.Text = "0";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(6, 163);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(167, 15);
            this.label17.TabIndex = 72;
            this.label17.Text = "Registry Access Control Flags";
            // 
            // button_SelectRegistryAccessFlags
            // 
            this.button_SelectRegistryAccessFlags.Location = new System.Drawing.Point(465, 158);
            this.button_SelectRegistryAccessFlags.Name = "button_SelectRegistryAccessFlags";
            this.button_SelectRegistryAccessFlags.Size = new System.Drawing.Size(41, 20);
            this.button_SelectRegistryAccessFlags.TabIndex = 74;
            this.button_SelectRegistryAccessFlags.Text = "...";
            this.button_SelectRegistryAccessFlags.UseVisualStyleBackColor = true;
            this.button_SelectRegistryAccessFlags.Click += new System.EventHandler(this.button_SelectRegistryAccessFlags_Click);
            // 
            // textBox_ProcessName
            // 
            this.textBox_ProcessName.Location = new System.Drawing.Point(203, 59);
            this.textBox_ProcessName.Name = "textBox_ProcessName";
            this.textBox_ProcessName.Size = new System.Drawing.Size(242, 20);
            this.textBox_ProcessName.TabIndex = 77;
            this.textBox_ProcessName.Text = "*";
            // 
            // RegistryAccessControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(546, 459);
            this.Controls.Add(this.groupBox_AccessControl);
            this.Name = "RegistryAccessControlForm";
            this.Text = "Registry Access Control Settings";
            this.groupBox_AccessControl.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox_AccessControl;
        private System.Windows.Forms.TextBox textBox_ProcessName;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button_SelectRegistryAccessFlags;
        private System.Windows.Forms.TextBox textBox_AccessFlags;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox textBox_ProcessId;
        private System.Windows.Forms.Button button_SelectProcessId;
        private System.Windows.Forms.Button button_SelectRegistryCallbackClass;
        private System.Windows.Forms.TextBox textBox_RegistryCallbackClass;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_DeleteFilter;
        private System.Windows.Forms.Button button_AddFilter;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView listView_FilterRules;
        private System.Windows.Forms.CheckBox checkBox_isExcludeFilter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_Apply;
        private System.Windows.Forms.Button button_Info;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton radioButton_Name;
        private System.Windows.Forms.RadioButton radioButton_Pid;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_RegistryKeyNameFilterMask;

    }
}