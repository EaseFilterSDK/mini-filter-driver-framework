namespace AutoEncryptDemo
{
    partial class AutoEncryptForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoEncryptForm));
            this.button_Start = new System.Windows.Forms.Button();
            this.textBox_EncrptFolder = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_AuthorizedProcessesForEncryptFolder = new System.Windows.Forms.TextBox();
            this.button_Stop = new System.Windows.Forms.Button();
            this.textBox_AuthorizedProcessesForDecryptFolder = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_DecryptFolder = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_PassPhrase = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_AuthorizedUsersForEncryptFolder = new System.Windows.Forms.TextBox();
            this.checkBox_EnableEncryptionWithDRM = new System.Windows.Forms.CheckBox();
            this.button_EncryptFolder = new System.Windows.Forms.Button();
            this.button_DRMSetting = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.getTagDataOfEncryptedFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.applyTrialKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button_DRMInfo = new System.Windows.Forms.Button();
            this.button_DecryptInfo = new System.Windows.Forms.Button();
            this.button_AuthorizedProcessInfo = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_Start
            // 
            this.button_Start.Location = new System.Drawing.Point(31, 372);
            this.button_Start.Name = "button_Start";
            this.button_Start.Size = new System.Drawing.Size(86, 23);
            this.button_Start.TabIndex = 0;
            this.button_Start.Text = "Start Service";
            this.button_Start.UseVisualStyleBackColor = true;
            this.button_Start.Click += new System.EventHandler(this.button_Start_Click);
            // 
            // textBox_EncrptFolder
            // 
            this.textBox_EncrptFolder.Location = new System.Drawing.Point(34, 51);
            this.textBox_EncrptFolder.Name = "textBox_EncrptFolder";
            this.textBox_EncrptFolder.Size = new System.Drawing.Size(320, 20);
            this.textBox_EncrptFolder.TabIndex = 1;
            this.textBox_EncrptFolder.Text = "c:\\AutoEncryptFolder\\*";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(184, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Auto Encryption Folder In Computer A";
            // 
            // textBox_AuthorizedProcessesForEncryptFolder
            // 
            this.textBox_AuthorizedProcessesForEncryptFolder.Location = new System.Drawing.Point(34, 97);
            this.textBox_AuthorizedProcessesForEncryptFolder.Name = "textBox_AuthorizedProcessesForEncryptFolder";
            this.textBox_AuthorizedProcessesForEncryptFolder.Size = new System.Drawing.Size(320, 20);
            this.textBox_AuthorizedProcessesForEncryptFolder.TabIndex = 3;
            this.textBox_AuthorizedProcessesForEncryptFolder.Text = "notepad.exe;wordpad.exe";
            // 
            // button_Stop
            // 
            this.button_Stop.Location = new System.Drawing.Point(267, 372);
            this.button_Stop.Name = "button_Stop";
            this.button_Stop.Size = new System.Drawing.Size(87, 23);
            this.button_Stop.TabIndex = 5;
            this.button_Stop.Text = "Stop Service";
            this.button_Stop.UseVisualStyleBackColor = true;
            this.button_Stop.Click += new System.EventHandler(this.button_Stop_Click);
            // 
            // textBox_AuthorizedProcessesForDecryptFolder
            // 
            this.textBox_AuthorizedProcessesForDecryptFolder.Location = new System.Drawing.Point(34, 336);
            this.textBox_AuthorizedProcessesForDecryptFolder.Name = "textBox_AuthorizedProcessesForDecryptFolder";
            this.textBox_AuthorizedProcessesForDecryptFolder.Size = new System.Drawing.Size(320, 20);
            this.textBox_AuthorizedProcessesForDecryptFolder.TabIndex = 6;
            this.textBox_AuthorizedProcessesForDecryptFolder.Text = "notepad.exe;wordpad.exe";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(34, 81);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(324, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Authorized Processes To Decrypt Files (* or empty for all processes)";
            // 
            // textBox_DecryptFolder
            // 
            this.textBox_DecryptFolder.Location = new System.Drawing.Point(34, 285);
            this.textBox_DecryptFolder.Name = "textBox_DecryptFolder";
            this.textBox_DecryptFolder.Size = new System.Drawing.Size(320, 20);
            this.textBox_DecryptFolder.TabIndex = 8;
            this.textBox_DecryptFolder.Text = "c:\\EncryptedFolder\\*";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 269);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(335, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Encryption Folder In Computer B ( Use Explorer to copy encrypted file)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 320);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(189, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Authorized Processes To Decrypt Files";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(34, 205);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(258, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "PassPhrase (must be the same for computer A and B)";
            // 
            // textBox_PassPhrase
            // 
            this.textBox_PassPhrase.Location = new System.Drawing.Point(34, 221);
            this.textBox_PassPhrase.Name = "textBox_PassPhrase";
            this.textBox_PassPhrase.Size = new System.Drawing.Size(320, 20);
            this.textBox_PassPhrase.TabIndex = 12;
            this.textBox_PassPhrase.Text = "your password";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(34, 125);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(267, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Authorized Users To Decrypt Files  (domain\\usename1)";
            // 
            // textBox_AuthorizedUsersForEncryptFolder
            // 
            this.textBox_AuthorizedUsersForEncryptFolder.Location = new System.Drawing.Point(34, 141);
            this.textBox_AuthorizedUsersForEncryptFolder.Name = "textBox_AuthorizedUsersForEncryptFolder";
            this.textBox_AuthorizedUsersForEncryptFolder.Size = new System.Drawing.Size(320, 20);
            this.textBox_AuthorizedUsersForEncryptFolder.TabIndex = 14;
            // 
            // checkBox_EnableEncryptionWithDRM
            // 
            this.checkBox_EnableEncryptionWithDRM.AutoSize = true;
            this.checkBox_EnableEncryptionWithDRM.Location = new System.Drawing.Point(34, 176);
            this.checkBox_EnableEncryptionWithDRM.Name = "checkBox_EnableEncryptionWithDRM";
            this.checkBox_EnableEncryptionWithDRM.Size = new System.Drawing.Size(15, 14);
            this.checkBox_EnableEncryptionWithDRM.TabIndex = 16;
            this.checkBox_EnableEncryptionWithDRM.UseVisualStyleBackColor = true;
            this.checkBox_EnableEncryptionWithDRM.CheckedChanged += new System.EventHandler(this.checkBox_EnableEncryptionWithDRM_CheckedChanged);
            // 
            // button_EncryptFolder
            // 
            this.button_EncryptFolder.Location = new System.Drawing.Point(364, 49);
            this.button_EncryptFolder.Name = "button_EncryptFolder";
            this.button_EncryptFolder.Size = new System.Drawing.Size(26, 23);
            this.button_EncryptFolder.TabIndex = 17;
            this.button_EncryptFolder.Text = "...";
            this.button_EncryptFolder.UseVisualStyleBackColor = true;
            this.button_EncryptFolder.Click += new System.EventHandler(this.button_EncryptFolder_Click);
            // 
            // button_DRMSetting
            // 
            this.button_DRMSetting.Enabled = false;
            this.button_DRMSetting.Location = new System.Drawing.Point(55, 171);
            this.button_DRMSetting.Name = "button_DRMSetting";
            this.button_DRMSetting.Size = new System.Drawing.Size(299, 23);
            this.button_DRMSetting.TabIndex = 18;
            this.button_DRMSetting.Text = "Enable DRM For Encrypted File Settings";
            this.button_DRMSetting.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.button_DRMSetting.UseVisualStyleBackColor = true;
            this.button_DRMSetting.Click += new System.EventHandler(this.button_DRMSetting_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.getTagDataOfEncryptedFileToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.applyTrialKeyToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(402, 24);
            this.menuStrip1.TabIndex = 21;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // getTagDataOfEncryptedFileToolStripMenuItem
            // 
            this.getTagDataOfEncryptedFileToolStripMenuItem.Name = "getTagDataOfEncryptedFileToolStripMenuItem";
            this.getTagDataOfEncryptedFileToolStripMenuItem.Size = new System.Drawing.Size(92, 20);
            this.getTagDataOfEncryptedFileToolStripMenuItem.Text = "Get DRM data";
            this.getTagDataOfEncryptedFileToolStripMenuItem.Click += new System.EventHandler(this.getTagDataOfEncryptedFileToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // applyTrialKeyToolStripMenuItem
            // 
            this.applyTrialKeyToolStripMenuItem.Name = "applyTrialKeyToolStripMenuItem";
            this.applyTrialKeyToolStripMenuItem.Size = new System.Drawing.Size(94, 20);
            this.applyTrialKeyToolStripMenuItem.Text = "Apply trial key";
            this.applyTrialKeyToolStripMenuItem.Click += new System.EventHandler(this.applyTrialKeyToolStripMenuItem_Click);
            // 
            // button_DRMInfo
            // 
            this.button_DRMInfo.Image = global::AutoEncryptDemo.Properties.Resources.about;
            this.button_DRMInfo.Location = new System.Drawing.Point(364, 171);
            this.button_DRMInfo.Name = "button_DRMInfo";
            this.button_DRMInfo.Size = new System.Drawing.Size(26, 23);
            this.button_DRMInfo.TabIndex = 22;
            this.button_DRMInfo.UseVisualStyleBackColor = true;
            this.button_DRMInfo.Click += new System.EventHandler(this.button_DRMInfo_Click);
            // 
            // button_DecryptInfo
            // 
            this.button_DecryptInfo.Image = global::AutoEncryptDemo.Properties.Resources.about;
            this.button_DecryptInfo.Location = new System.Drawing.Point(364, 282);
            this.button_DecryptInfo.Name = "button_DecryptInfo";
            this.button_DecryptInfo.Size = new System.Drawing.Size(26, 23);
            this.button_DecryptInfo.TabIndex = 23;
            this.button_DecryptInfo.UseVisualStyleBackColor = true;
            this.button_DecryptInfo.Click += new System.EventHandler(this.button_DecryptInfo_Click);
            // 
            // button_AuthorizedProcessInfo
            // 
            this.button_AuthorizedProcessInfo.Image = global::AutoEncryptDemo.Properties.Resources.about;
            this.button_AuthorizedProcessInfo.Location = new System.Drawing.Point(364, 97);
            this.button_AuthorizedProcessInfo.Name = "button_AuthorizedProcessInfo";
            this.button_AuthorizedProcessInfo.Size = new System.Drawing.Size(26, 23);
            this.button_AuthorizedProcessInfo.TabIndex = 24;
            this.button_AuthorizedProcessInfo.UseVisualStyleBackColor = true;
            this.button_AuthorizedProcessInfo.Click += new System.EventHandler(this.button_AuthorizedProcessInfo_Click);
            // 
            // AutoEncryptForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 410);
            this.Controls.Add(this.button_AuthorizedProcessInfo);
            this.Controls.Add(this.button_DecryptInfo);
            this.Controls.Add(this.button_DRMInfo);
            this.Controls.Add(this.button_DRMSetting);
            this.Controls.Add(this.button_EncryptFolder);
            this.Controls.Add(this.checkBox_EnableEncryptionWithDRM);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBox_AuthorizedUsersForEncryptFolder);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox_PassPhrase);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox_DecryptFolder);
            this.Controls.Add(this.textBox_AuthorizedProcessesForDecryptFolder);
            this.Controls.Add(this.button_Stop);
            this.Controls.Add(this.textBox_AuthorizedProcessesForEncryptFolder);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_EncrptFolder);
            this.Controls.Add(this.button_Start);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "AutoEncryptForm";
            this.Text = "Auto File DRM Encryption Tool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AutoEncryptForm_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_Start;
        private System.Windows.Forms.TextBox textBox_EncrptFolder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_AuthorizedProcessesForEncryptFolder;
        private System.Windows.Forms.Button button_Stop;
        private System.Windows.Forms.TextBox textBox_AuthorizedProcessesForDecryptFolder;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_DecryptFolder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_PassPhrase;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox_AuthorizedUsersForEncryptFolder;
        private System.Windows.Forms.CheckBox checkBox_EnableEncryptionWithDRM;
        private System.Windows.Forms.Button button_EncryptFolder;
        private System.Windows.Forms.Button button_DRMSetting;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem applyTrialKeyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getTagDataOfEncryptedFileToolStripMenuItem;
        private System.Windows.Forms.Button button_DRMInfo;
        private System.Windows.Forms.Button button_DecryptInfo;
        private System.Windows.Forms.Button button_AuthorizedProcessInfo;
    }
}

