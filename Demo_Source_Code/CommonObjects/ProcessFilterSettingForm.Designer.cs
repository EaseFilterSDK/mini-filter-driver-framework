namespace EaseFilter.CommonObjects
{
    partial class ProcessFilterSettingForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProcessFilterSettingForm));
            this.groupBox_AccessControl = new System.Windows.Forms.GroupBox();
            this.button_Apply = new System.Windows.Forms.Button();
            this.button_DeleteFilter = new System.Windows.Forms.Button();
            this.button_AddFilter = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listView_FilterRules = new System.Windows.Forms.ListView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button_ProcessInfo = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_ControlFlag = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.button_SelectControlFlag = new System.Windows.Forms.Button();
            this.label22 = new System.Windows.Forms.Label();
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
            this.groupBox_AccessControl.Size = new System.Drawing.Size(537, 331);
            this.groupBox_AccessControl.TabIndex = 26;
            this.groupBox_AccessControl.TabStop = false;
            // 
            // button_Apply
            // 
            this.button_Apply.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_Apply.Location = new System.Drawing.Point(382, 263);
            this.button_Apply.Name = "button_Apply";
            this.button_Apply.Size = new System.Drawing.Size(138, 23);
            this.button_Apply.TabIndex = 84;
            this.button_Apply.Text = "Apply Filter Rule Settings";
            this.button_Apply.UseVisualStyleBackColor = true;
            this.button_Apply.Click += new System.EventHandler(this.button_Apply_Click_1);
            // 
            // button_DeleteFilter
            // 
            this.button_DeleteFilter.Location = new System.Drawing.Point(197, 263);
            this.button_DeleteFilter.Name = "button_DeleteFilter";
            this.button_DeleteFilter.Size = new System.Drawing.Size(103, 23);
            this.button_DeleteFilter.TabIndex = 83;
            this.button_DeleteFilter.Text = "Delete Filter Rule";
            this.button_DeleteFilter.UseVisualStyleBackColor = true;
            this.button_DeleteFilter.Click += new System.EventHandler(this.button_DeleteFilter_Click);
            // 
            // button_AddFilter
            // 
            this.button_AddFilter.Location = new System.Drawing.Point(6, 263);
            this.button_AddFilter.Name = "button_AddFilter";
            this.button_AddFilter.Size = new System.Drawing.Size(116, 23);
            this.button_AddFilter.TabIndex = 82;
            this.button_AddFilter.Text = "Add New Filter Rule";
            this.button_AddFilter.UseVisualStyleBackColor = true;
            this.button_AddFilter.Click += new System.EventHandler(this.button_AddFilter_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listView_FilterRules);
            this.groupBox1.Location = new System.Drawing.Point(6, 158);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(517, 93);
            this.groupBox1.TabIndex = 81;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Process Filter Rule Collection";
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
            this.groupBox2.Controls.Add(this.button_ProcessInfo);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.textBox_ControlFlag);
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Controls.Add(this.button_SelectControlFlag);
            this.groupBox2.Controls.Add(this.label22);
            this.groupBox2.Controls.Add(this.textBox_ProcessName);
            this.groupBox2.Location = new System.Drawing.Point(6, 19);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(517, 124);
            this.groupBox2.TabIndex = 76;
            this.groupBox2.TabStop = false;
            // 
            // button_ProcessInfo
            // 
            this.button_ProcessInfo.BackColor = System.Drawing.Color.AntiqueWhite;
            this.button_ProcessInfo.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
            this.button_ProcessInfo.Location = new System.Drawing.Point(465, 27);
            this.button_ProcessInfo.Name = "button_ProcessInfo";
            this.button_ProcessInfo.Size = new System.Drawing.Size(33, 20);
            this.button_ProcessInfo.TabIndex = 97;
            this.button_ProcessInfo.UseVisualStyleBackColor = false;
            this.button_ProcessInfo.Click += new System.EventHandler(this.button_ProcessInfo_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(201, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(209, 13);
            this.label2.TabIndex = 90;
            this.label2.Text = "(  i.e. c:\\testfolder\\*.exe  or  \'notepad.exe\' )";
            // 
            // textBox_ControlFlag
            // 
            this.textBox_ControlFlag.Location = new System.Drawing.Point(203, 78);
            this.textBox_ControlFlag.Name = "textBox_ControlFlag";
            this.textBox_ControlFlag.ReadOnly = true;
            this.textBox_ControlFlag.Size = new System.Drawing.Size(242, 20);
            this.textBox_ControlFlag.TabIndex = 73;
            this.textBox_ControlFlag.Text = "0";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(6, 83);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(142, 13);
            this.label17.TabIndex = 72;
            this.label17.Text = "Process Access Control Flag";
            // 
            // button_SelectControlFlag
            // 
            this.button_SelectControlFlag.Location = new System.Drawing.Point(465, 78);
            this.button_SelectControlFlag.Name = "button_SelectControlFlag";
            this.button_SelectControlFlag.Size = new System.Drawing.Size(41, 20);
            this.button_SelectControlFlag.TabIndex = 74;
            this.button_SelectControlFlag.Text = "...";
            this.button_SelectControlFlag.UseVisualStyleBackColor = true;
            this.button_SelectControlFlag.Click += new System.EventHandler(this.button_SelectControlFlag_Click);
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(7, 27);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(116, 13);
            this.label22.TabIndex = 78;
            this.label22.Text = "Processes Name Mask";
            // 
            // textBox_ProcessName
            // 
            this.textBox_ProcessName.Location = new System.Drawing.Point(203, 27);
            this.textBox_ProcessName.Name = "textBox_ProcessName";
            this.textBox_ProcessName.Size = new System.Drawing.Size(242, 20);
            this.textBox_ProcessName.TabIndex = 77;
            this.textBox_ProcessName.Text = "*";
            // 
            // ProcessFilterSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(537, 331);
            this.Controls.Add(this.groupBox_AccessControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ProcessFilterSettingForm";
            this.Text = "Process Filter Rule Settings";
            this.groupBox_AccessControl.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
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
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox textBox_ProcessName;
        private System.Windows.Forms.Button button_Apply;
        private System.Windows.Forms.Button button_DeleteFilter;
        private System.Windows.Forms.Button button_AddFilter;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView listView_FilterRules;
        private System.Windows.Forms.Button button_ProcessInfo;
    }
}