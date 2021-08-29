namespace SecureSandbox
{
    partial class SecureSandbox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SecureSandbox));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_SaveSandBox = new System.Windows.Forms.Button();
            this.button_help = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_StartFilter = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_Stop = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_EventViewer = new System.Windows.Forms.ToolStripButton();
            this.button_AddSandbox = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.checkBox_AllowFolderBrowsing = new System.Windows.Forms.CheckBox();
            this.button_ConfigProcessRights = new System.Windows.Forms.Button();
            this.checkBox_EnableEncryption = new System.Windows.Forms.CheckBox();
            this.checkBox_AllowSandboxRead = new System.Windows.Forms.CheckBox();
            this.checkBox_AllowReadEncryptedFiles = new System.Windows.Forms.CheckBox();
            this.checkBox_AllowSandboxChange = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button_ProcessFileAccessRights = new System.Windows.Forms.Button();
            this.checkBox_AllowExecute = new System.Windows.Forms.CheckBox();
            this.checkBox_AllowChangeRegistry = new System.Windows.Forms.CheckBox();
            this.checkBox_AllowReadRegistry = new System.Windows.Forms.CheckBox();
            this.textBox_SandboxFolder = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button_SelectFolder = new System.Windows.Forms.Button();
            this.button_DeleteSandbox = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listView_Sandbox = new System.Windows.Forms.ListView();
            this.groupBox1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_help);
            this.groupBox1.Controls.Add(this.toolStrip1);
            this.groupBox1.Controls.Add(this.button_AddSandbox);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.button_DeleteSandbox);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(544, 520);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            // 
            // button_SaveSandBox
            // 
            this.button_SaveSandBox.BackColor = System.Drawing.Color.Gold;
            this.button_SaveSandBox.Location = new System.Drawing.Point(363, 251);
            this.button_SaveSandBox.Name = "button_SaveSandBox";
            this.button_SaveSandBox.Size = new System.Drawing.Size(131, 26);
            this.button_SaveSandBox.TabIndex = 97;
            this.button_SaveSandBox.Text = "Save Sandbox";
            this.button_SaveSandBox.UseVisualStyleBackColor = false;
            this.button_SaveSandBox.Click += new System.EventHandler(this.button_SaveSandBox_Click);
            // 
            // button_help
            // 
            this.button_help.Image = ((System.Drawing.Image)(resources.GetObject("button_help.Image")));
            this.button_help.Location = new System.Drawing.Point(485, 471);
            this.button_help.Name = "button_help";
            this.button_help.Size = new System.Drawing.Size(36, 33);
            this.button_help.TabIndex = 96;
            this.button_help.UseVisualStyleBackColor = true;
            this.button_help.Click += new System.EventHandler(this.button_help_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_StartFilter,
            this.toolStripSeparator1,
            this.toolStripButton_Stop,
            this.toolStripSeparator2,
            this.toolStripButton_EventViewer});
            this.toolStrip1.Location = new System.Drawing.Point(3, 16);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(538, 27);
            this.toolStrip1.TabIndex = 95;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton_StartFilter
            // 
            this.toolStripButton_StartFilter.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_StartFilter.Image")));
            this.toolStripButton_StartFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_StartFilter.Name = "toolStripButton_StartFilter";
            this.toolStripButton_StartFilter.Size = new System.Drawing.Size(111, 24);
            this.toolStripButton_StartFilter.Text = "Start Service";
            this.toolStripButton_StartFilter.Click += new System.EventHandler(this.toolStripButton_StartFilter_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripButton_Stop
            // 
            this.toolStripButton_Stop.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Stop.Image")));
            this.toolStripButton_Stop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Stop.Name = "toolStripButton_Stop";
            this.toolStripButton_Stop.Size = new System.Drawing.Size(111, 24);
            this.toolStripButton_Stop.Text = "Stop Service";
            this.toolStripButton_Stop.Click += new System.EventHandler(this.toolStripButton_Stop_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripButton_EventViewer
            // 
            this.toolStripButton_EventViewer.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_EventViewer.Image")));
            this.toolStripButton_EventViewer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_EventViewer.Name = "toolStripButton_EventViewer";
            this.toolStripButton_EventViewer.Size = new System.Drawing.Size(112, 24);
            this.toolStripButton_EventViewer.Text = "Event viewer";
            this.toolStripButton_EventViewer.Click += new System.EventHandler(this.toolStripButton_EventViewer_Click);
            // 
            // button_AddSandbox
            // 
            this.button_AddSandbox.Location = new System.Drawing.Point(20, 476);
            this.button_AddSandbox.Name = "button_AddSandbox";
            this.button_AddSandbox.Size = new System.Drawing.Size(94, 23);
            this.button_AddSandbox.TabIndex = 94;
            this.button_AddSandbox.Text = "Add Sandbox";
            this.button_AddSandbox.UseVisualStyleBackColor = true;
            this.button_AddSandbox.Click += new System.EventHandler(this.button_AddSandbox_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button_SaveSandBox);
            this.groupBox3.Controls.Add(this.groupBox5);
            this.groupBox3.Controls.Add(this.groupBox4);
            this.groupBox3.Controls.Add(this.textBox_SandboxFolder);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.button_SelectFolder);
            this.groupBox3.Location = new System.Drawing.Point(17, 53);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(507, 277);
            this.groupBox3.TabIndex = 57;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Sandbox Settings";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.checkBox_AllowFolderBrowsing);
            this.groupBox5.Controls.Add(this.button_ConfigProcessRights);
            this.groupBox5.Controls.Add(this.checkBox_EnableEncryption);
            this.groupBox5.Controls.Add(this.checkBox_AllowSandboxRead);
            this.groupBox5.Controls.Add(this.checkBox_AllowReadEncryptedFiles);
            this.groupBox5.Controls.Add(this.checkBox_AllowSandboxChange);
            this.groupBox5.Location = new System.Drawing.Point(14, 144);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(492, 101);
            this.groupBox5.TabIndex = 78;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "The default file access rights of the sandbox to all processes";
            // 
            // checkBox_AllowFolderBrowsing
            // 
            this.checkBox_AllowFolderBrowsing.AutoSize = true;
            this.checkBox_AllowFolderBrowsing.Checked = true;
            this.checkBox_AllowFolderBrowsing.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_AllowFolderBrowsing.Location = new System.Drawing.Point(7, 76);
            this.checkBox_AllowFolderBrowsing.Name = "checkBox_AllowFolderBrowsing";
            this.checkBox_AllowFolderBrowsing.Size = new System.Drawing.Size(145, 19);
            this.checkBox_AllowFolderBrowsing.TabIndex = 80;
            this.checkBox_AllowFolderBrowsing.Text = "Allow browsing folder";
            this.checkBox_AllowFolderBrowsing.UseVisualStyleBackColor = true;
            this.checkBox_AllowFolderBrowsing.CheckedChanged += new System.EventHandler(this.checkBox_AllowFolderBrowsing_CheckedChanged);
            // 
            // button_ConfigProcessRights
            // 
            this.button_ConfigProcessRights.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.button_ConfigProcessRights.Location = new System.Drawing.Point(372, 34);
            this.button_ConfigProcessRights.Name = "button_ConfigProcessRights";
            this.button_ConfigProcessRights.Size = new System.Drawing.Size(108, 23);
            this.button_ConfigProcessRights.TabIndex = 58;
            this.button_ConfigProcessRights.Text = "Advance Settings";
            this.button_ConfigProcessRights.UseVisualStyleBackColor = false;
            this.button_ConfigProcessRights.Click += new System.EventHandler(this.button_ConfigProcessRights_Click);
            // 
            // checkBox_EnableEncryption
            // 
            this.checkBox_EnableEncryption.AutoSize = true;
            this.checkBox_EnableEncryption.Checked = true;
            this.checkBox_EnableEncryption.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_EnableEncryption.Location = new System.Drawing.Point(181, 34);
            this.checkBox_EnableEncryption.Name = "checkBox_EnableEncryption";
            this.checkBox_EnableEncryption.Size = new System.Drawing.Size(127, 19);
            this.checkBox_EnableEncryption.TabIndex = 30;
            this.checkBox_EnableEncryption.Text = "Enable encryption";
            this.checkBox_EnableEncryption.UseVisualStyleBackColor = true;
            this.checkBox_EnableEncryption.CheckedChanged += new System.EventHandler(this.checkBox_EnableEncryption_CheckedChanged);
            // 
            // checkBox_AllowSandboxRead
            // 
            this.checkBox_AllowSandboxRead.AutoSize = true;
            this.checkBox_AllowSandboxRead.Checked = true;
            this.checkBox_AllowSandboxRead.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_AllowSandboxRead.Location = new System.Drawing.Point(7, 34);
            this.checkBox_AllowSandboxRead.Name = "checkBox_AllowSandboxRead";
            this.checkBox_AllowSandboxRead.Size = new System.Drawing.Size(139, 19);
            this.checkBox_AllowSandboxRead.TabIndex = 29;
            this.checkBox_AllowSandboxRead.Text = "Allow file being read";
            this.checkBox_AllowSandboxRead.UseVisualStyleBackColor = true;
            this.checkBox_AllowSandboxRead.CheckedChanged += new System.EventHandler(this.checkBox_AllowSandboxRead_CheckedChanged);
            // 
            // checkBox_AllowReadEncryptedFiles
            // 
            this.checkBox_AllowReadEncryptedFiles.AutoSize = true;
            this.checkBox_AllowReadEncryptedFiles.Checked = true;
            this.checkBox_AllowReadEncryptedFiles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_AllowReadEncryptedFiles.Location = new System.Drawing.Point(181, 59);
            this.checkBox_AllowReadEncryptedFiles.Name = "checkBox_AllowReadEncryptedFiles";
            this.checkBox_AllowReadEncryptedFiles.Size = new System.Drawing.Size(195, 19);
            this.checkBox_AllowReadEncryptedFiles.TabIndex = 28;
            this.checkBox_AllowReadEncryptedFiles.Text = "Allow encrypted file being read";
            this.checkBox_AllowReadEncryptedFiles.UseVisualStyleBackColor = true;
            this.checkBox_AllowReadEncryptedFiles.CheckedChanged += new System.EventHandler(this.checkBox_AllowReadEncryptedFiles_CheckedChanged);
            // 
            // checkBox_AllowSandboxChange
            // 
            this.checkBox_AllowSandboxChange.AutoSize = true;
            this.checkBox_AllowSandboxChange.Checked = true;
            this.checkBox_AllowSandboxChange.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_AllowSandboxChange.Location = new System.Drawing.Point(7, 54);
            this.checkBox_AllowSandboxChange.Name = "checkBox_AllowSandboxChange";
            this.checkBox_AllowSandboxChange.Size = new System.Drawing.Size(162, 19);
            this.checkBox_AllowSandboxChange.TabIndex = 15;
            this.checkBox_AllowSandboxChange.Text = "Allow file being changed";
            this.checkBox_AllowSandboxChange.UseVisualStyleBackColor = true;
            this.checkBox_AllowSandboxChange.CheckedChanged += new System.EventHandler(this.checkBox_AllowSandboxChange_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.button_ProcessFileAccessRights);
            this.groupBox4.Controls.Add(this.checkBox_AllowExecute);
            this.groupBox4.Controls.Add(this.checkBox_AllowChangeRegistry);
            this.groupBox4.Controls.Add(this.checkBox_AllowReadRegistry);
            this.groupBox4.Location = new System.Drawing.Point(14, 60);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(492, 85);
            this.groupBox4.TabIndex = 77;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "The policy of the executable binaries inside the sandbox";
            // 
            // button_ProcessFileAccessRights
            // 
            this.button_ProcessFileAccessRights.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.button_ProcessFileAccessRights.Location = new System.Drawing.Point(177, 51);
            this.button_ProcessFileAccessRights.Name = "button_ProcessFileAccessRights";
            this.button_ProcessFileAccessRights.Size = new System.Drawing.Size(303, 23);
            this.button_ProcessFileAccessRights.TabIndex = 96;
            this.button_ProcessFileAccessRights.Text = "Configure executable binaries file access rights";
            this.button_ProcessFileAccessRights.UseVisualStyleBackColor = false;
            this.button_ProcessFileAccessRights.Click += new System.EventHandler(this.button_ProcessFileAccessRights_Click);
            // 
            // checkBox_AllowExecute
            // 
            this.checkBox_AllowExecute.AutoSize = true;
            this.checkBox_AllowExecute.Location = new System.Drawing.Point(177, 28);
            this.checkBox_AllowExecute.Name = "checkBox_AllowExecute";
            this.checkBox_AllowExecute.Size = new System.Drawing.Size(281, 19);
            this.checkBox_AllowExecute.TabIndex = 17;
            this.checkBox_AllowExecute.Text = "Allow binaries inside sandbox  being executed";
            this.checkBox_AllowExecute.UseVisualStyleBackColor = true;
            this.checkBox_AllowExecute.CheckedChanged += new System.EventHandler(this.checkBox_AllowExecute_CheckedChanged);
            // 
            // checkBox_AllowChangeRegistry
            // 
            this.checkBox_AllowChangeRegistry.AutoSize = true;
            this.checkBox_AllowChangeRegistry.Location = new System.Drawing.Point(23, 51);
            this.checkBox_AllowChangeRegistry.Name = "checkBox_AllowChangeRegistry";
            this.checkBox_AllowChangeRegistry.Size = new System.Drawing.Size(154, 19);
            this.checkBox_AllowChangeRegistry.TabIndex = 16;
            this.checkBox_AllowChangeRegistry.Text = "Allow registry changing";
            this.checkBox_AllowChangeRegistry.UseVisualStyleBackColor = true;
            // 
            // checkBox_AllowReadRegistry
            // 
            this.checkBox_AllowReadRegistry.AutoSize = true;
            this.checkBox_AllowReadRegistry.Checked = true;
            this.checkBox_AllowReadRegistry.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_AllowReadRegistry.Location = new System.Drawing.Point(23, 28);
            this.checkBox_AllowReadRegistry.Name = "checkBox_AllowReadRegistry";
            this.checkBox_AllowReadRegistry.Size = new System.Drawing.Size(145, 19);
            this.checkBox_AllowReadRegistry.TabIndex = 21;
            this.checkBox_AllowReadRegistry.Text = "Allow registry reading";
            this.checkBox_AllowReadRegistry.UseVisualStyleBackColor = true;
            // 
            // textBox_SandboxFolder
            // 
            this.textBox_SandboxFolder.Location = new System.Drawing.Point(98, 21);
            this.textBox_SandboxFolder.Name = "textBox_SandboxFolder";
            this.textBox_SandboxFolder.Size = new System.Drawing.Size(321, 20);
            this.textBox_SandboxFolder.TabIndex = 37;
            this.textBox_SandboxFolder.Text = "c:\\MySandbox";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 15);
            this.label5.TabIndex = 36;
            this.label5.Text = "Sandbox Folder";
            // 
            // button_SelectFolder
            // 
            this.button_SelectFolder.Location = new System.Drawing.Point(434, 21);
            this.button_SelectFolder.Name = "button_SelectFolder";
            this.button_SelectFolder.Size = new System.Drawing.Size(60, 20);
            this.button_SelectFolder.TabIndex = 38;
            this.button_SelectFolder.Text = "...";
            this.button_SelectFolder.UseVisualStyleBackColor = true;
            this.button_SelectFolder.Click += new System.EventHandler(this.button_SelectFolder_Click);
            // 
            // button_DeleteSandbox
            // 
            this.button_DeleteSandbox.Location = new System.Drawing.Point(321, 476);
            this.button_DeleteSandbox.Name = "button_DeleteSandbox";
            this.button_DeleteSandbox.Size = new System.Drawing.Size(131, 23);
            this.button_DeleteSandbox.TabIndex = 52;
            this.button_DeleteSandbox.Text = "Delete Sandbox";
            this.button_DeleteSandbox.UseVisualStyleBackColor = true;
            this.button_DeleteSandbox.Click += new System.EventHandler(this.button_DeleteSandbox_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.listView_Sandbox);
            this.groupBox2.Location = new System.Drawing.Point(17, 336);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(507, 116);
            this.groupBox2.TabIndex = 50;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Sandbox Collection";
            // 
            // listView_Sandbox
            // 
            this.listView_Sandbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_Sandbox.FullRowSelect = true;
            this.listView_Sandbox.Location = new System.Drawing.Point(3, 16);
            this.listView_Sandbox.Name = "listView_Sandbox";
            this.listView_Sandbox.Size = new System.Drawing.Size(501, 97);
            this.listView_Sandbox.TabIndex = 1;
            this.listView_Sandbox.UseCompatibleStateImageBehavior = false;
            this.listView_Sandbox.View = System.Windows.Forms.View.Details;
            this.listView_Sandbox.SelectedIndexChanged += new System.EventHandler(this.listView_Sandbox_SelectedIndexChanged);
            // 
            // SecureSandbox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 520);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "SecureSandbox";
            this.Text = "Easefilter Secure Sandbox";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SecureSandbox_FormClosed);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_DeleteSandbox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView listView_Sandbox;
        private System.Windows.Forms.Button button_SelectFolder;
        private System.Windows.Forms.TextBox textBox_SandboxFolder;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox checkBox_AllowExecute;
        private System.Windows.Forms.CheckBox checkBox_AllowChangeRegistry;
        private System.Windows.Forms.CheckBox checkBox_AllowReadRegistry;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox checkBox_AllowSandboxRead;
        private System.Windows.Forms.CheckBox checkBox_AllowReadEncryptedFiles;
        private System.Windows.Forms.CheckBox checkBox_AllowSandboxChange;
        private System.Windows.Forms.CheckBox checkBox_EnableEncryption;
        private System.Windows.Forms.Button button_ConfigProcessRights;
        private System.Windows.Forms.Button button_AddSandbox;
        private System.Windows.Forms.CheckBox checkBox_AllowFolderBrowsing;
        private System.Windows.Forms.Button button_ProcessFileAccessRights;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton_StartFilter;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton_Stop;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButton_EventViewer;
        private System.Windows.Forms.Button button_help;
        private System.Windows.Forms.Button button_SaveSandBox;
    }
}

