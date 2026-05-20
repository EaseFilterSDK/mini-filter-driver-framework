
namespace ZeroTrustDemo
{
    partial class AccessRightForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AccessRightForm));
            this.groupBox_SignedProcess = new System.Windows.Forms.GroupBox();
            this.button_GetCertificateName = new System.Windows.Forms.Button();
            this.textBox_ProcessCertificateName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox_ProcessSha256 = new System.Windows.Forms.GroupBox();
            this.button_GetProcessSha256 = new System.Windows.Forms.Button();
            this.textBox_ProcessSha256Hash = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox_UserName = new System.Windows.Forms.GroupBox();
            this.button_InfoUserName = new System.Windows.Forms.Button();
            this.textBox_UserName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox_AccessRights = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_ApplySettings = new System.Windows.Forms.Button();
            this.checkBox_AllowReadEncryptedFiles = new System.Windows.Forms.CheckBox();
            this.checkBox_Read = new System.Windows.Forms.CheckBox();
            this.checkBox_Write = new System.Windows.Forms.CheckBox();
            this.checkBox_AllowDelete = new System.Windows.Forms.CheckBox();
            this.checkBox_AllowRename = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_FileAccessFlags = new System.Windows.Forms.TextBox();
            this.button_FileAccessFlags = new System.Windows.Forms.Button();
            this.groupBox_ProcessName = new System.Windows.Forms.GroupBox();
            this.button_InfoProcessName = new System.Windows.Forms.Button();
            this.textBox_ProcessName = new System.Windows.Forms.TextBox();
            this.label_AccessFlags = new System.Windows.Forms.Label();
            this.groupBox_SignedProcess.SuspendLayout();
            this.groupBox_ProcessSha256.SuspendLayout();
            this.groupBox_UserName.SuspendLayout();
            this.groupBox_AccessRights.SuspendLayout();
            this.groupBox_ProcessName.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox_SignedProcess
            // 
            this.groupBox_SignedProcess.Controls.Add(this.button_GetCertificateName);
            this.groupBox_SignedProcess.Controls.Add(this.textBox_ProcessCertificateName);
            this.groupBox_SignedProcess.Controls.Add(this.label5);
            this.groupBox_SignedProcess.Location = new System.Drawing.Point(18, 63);
            this.groupBox_SignedProcess.Name = "groupBox_SignedProcess";
            this.groupBox_SignedProcess.Size = new System.Drawing.Size(535, 48);
            this.groupBox_SignedProcess.TabIndex = 131;
            this.groupBox_SignedProcess.TabStop = false;
            this.groupBox_SignedProcess.Visible = false;
            // 
            // button_GetCertificateName
            // 
            this.button_GetCertificateName.Location = new System.Drawing.Point(461, 12);
            this.button_GetCertificateName.Name = "button_GetCertificateName";
            this.button_GetCertificateName.Size = new System.Drawing.Size(41, 20);
            this.button_GetCertificateName.TabIndex = 40;
            this.button_GetCertificateName.Text = "...";
            this.button_GetCertificateName.UseVisualStyleBackColor = true;
            this.button_GetCertificateName.Click += new System.EventHandler(this.button_GetCertificateName_Click);
            // 
            // textBox_ProcessCertificateName
            // 
            this.textBox_ProcessCertificateName.Location = new System.Drawing.Point(149, 12);
            this.textBox_ProcessCertificateName.Name = "textBox_ProcessCertificateName";
            this.textBox_ProcessCertificateName.Size = new System.Drawing.Size(298, 20);
            this.textBox_ProcessCertificateName.TabIndex = 39;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(130, 13);
            this.label5.TabIndex = 29;
            this.label5.Text = "Certificate\'s subject name ";
            // 
            // groupBox_ProcessSha256
            // 
            this.groupBox_ProcessSha256.Controls.Add(this.button_GetProcessSha256);
            this.groupBox_ProcessSha256.Controls.Add(this.textBox_ProcessSha256Hash);
            this.groupBox_ProcessSha256.Controls.Add(this.label4);
            this.groupBox_ProcessSha256.Location = new System.Drawing.Point(18, 100);
            this.groupBox_ProcessSha256.Name = "groupBox_ProcessSha256";
            this.groupBox_ProcessSha256.Size = new System.Drawing.Size(535, 48);
            this.groupBox_ProcessSha256.TabIndex = 129;
            this.groupBox_ProcessSha256.TabStop = false;
            this.groupBox_ProcessSha256.Visible = false;
            // 
            // button_GetProcessSha256
            // 
            this.button_GetProcessSha256.Location = new System.Drawing.Point(461, 15);
            this.button_GetProcessSha256.Name = "button_GetProcessSha256";
            this.button_GetProcessSha256.Size = new System.Drawing.Size(41, 20);
            this.button_GetProcessSha256.TabIndex = 38;
            this.button_GetProcessSha256.Text = "...";
            this.button_GetProcessSha256.UseVisualStyleBackColor = true;
            this.button_GetProcessSha256.Click += new System.EventHandler(this.button_GetProcessSha256_Click);
            // 
            // textBox_ProcessSha256Hash
            // 
            this.textBox_ProcessSha256Hash.Location = new System.Drawing.Point(149, 16);
            this.textBox_ProcessSha256Hash.Name = "textBox_ProcessSha256Hash";
            this.textBox_ProcessSha256Hash.Size = new System.Drawing.Size(298, 20);
            this.textBox_ProcessSha256Hash.TabIndex = 27;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(133, 13);
            this.label4.TabIndex = 28;
            this.label4.Text = "The process  sha256 hash";
            // 
            // groupBox_UserName
            // 
            this.groupBox_UserName.Controls.Add(this.button_InfoUserName);
            this.groupBox_UserName.Controls.Add(this.textBox_UserName);
            this.groupBox_UserName.Controls.Add(this.label1);
            this.groupBox_UserName.Location = new System.Drawing.Point(18, 149);
            this.groupBox_UserName.Name = "groupBox_UserName";
            this.groupBox_UserName.Size = new System.Drawing.Size(535, 46);
            this.groupBox_UserName.TabIndex = 128;
            this.groupBox_UserName.TabStop = false;
            this.groupBox_UserName.Visible = false;
            // 
            // button_InfoUserName
            // 
            this.button_InfoUserName.Image = global::ZeroTrustDemo.Properties.Resources.about;
            this.button_InfoUserName.Location = new System.Drawing.Point(461, 16);
            this.button_InfoUserName.Name = "button_InfoUserName";
            this.button_InfoUserName.Size = new System.Drawing.Size(41, 20);
            this.button_InfoUserName.TabIndex = 115;
            this.button_InfoUserName.UseVisualStyleBackColor = true;
            this.button_InfoUserName.Click += new System.EventHandler(this.button_InfoUserName_Click);
            // 
            // textBox_UserName
            // 
            this.textBox_UserName.Location = new System.Drawing.Point(149, 16);
            this.textBox_UserName.Name = "textBox_UserName";
            this.textBox_UserName.Size = new System.Drawing.Size(298, 20);
            this.textBox_UserName.TabIndex = 27;
            this.textBox_UserName.Text = "domain1\\user1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 28;
            this.label1.Text = "User name ";
            // 
            // groupBox_AccessRights
            // 
            this.groupBox_AccessRights.Controls.Add(this.groupBox1);
            this.groupBox_AccessRights.Controls.Add(this.button_ApplySettings);
            this.groupBox_AccessRights.Controls.Add(this.checkBox_AllowReadEncryptedFiles);
            this.groupBox_AccessRights.Controls.Add(this.checkBox_Read);
            this.groupBox_AccessRights.Controls.Add(this.checkBox_Write);
            this.groupBox_AccessRights.Controls.Add(this.checkBox_AllowDelete);
            this.groupBox_AccessRights.Controls.Add(this.checkBox_AllowRename);
            this.groupBox_AccessRights.Controls.Add(this.label2);
            this.groupBox_AccessRights.Controls.Add(this.textBox_FileAccessFlags);
            this.groupBox_AccessRights.Controls.Add(this.button_FileAccessFlags);
            this.groupBox_AccessRights.Location = new System.Drawing.Point(18, 196);
            this.groupBox_AccessRights.Name = "groupBox_AccessRights";
            this.groupBox_AccessRights.Size = new System.Drawing.Size(535, 201);
            this.groupBox_AccessRights.TabIndex = 130;
            this.groupBox_AccessRights.TabStop = false;
            this.groupBox_AccessRights.Text = "File Acess Rights";
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(9, 135);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(516, 10);
            this.groupBox1.TabIndex = 124;
            this.groupBox1.TabStop = false;
            // 
            // button_ApplySettings
            // 
            this.button_ApplySettings.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_ApplySettings.Location = new System.Drawing.Point(372, 154);
            this.button_ApplySettings.Name = "button_ApplySettings";
            this.button_ApplySettings.Size = new System.Drawing.Size(75, 23);
            this.button_ApplySettings.TabIndex = 25;
            this.button_ApplySettings.Text = "Apply";
            this.button_ApplySettings.UseVisualStyleBackColor = true;
            this.button_ApplySettings.Click += new System.EventHandler(this.button_ApplySettings_Click);
            // 
            // checkBox_AllowReadEncryptedFiles
            // 
            this.checkBox_AllowReadEncryptedFiles.AutoSize = true;
            this.checkBox_AllowReadEncryptedFiles.Checked = true;
            this.checkBox_AllowReadEncryptedFiles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_AllowReadEncryptedFiles.Location = new System.Drawing.Point(149, 101);
            this.checkBox_AllowReadEncryptedFiles.Name = "checkBox_AllowReadEncryptedFiles";
            this.checkBox_AllowReadEncryptedFiles.Size = new System.Drawing.Size(173, 17);
            this.checkBox_AllowReadEncryptedFiles.TabIndex = 49;
            this.checkBox_AllowReadEncryptedFiles.Text = "Allow reading the encrypted file";
            this.checkBox_AllowReadEncryptedFiles.UseVisualStyleBackColor = true;
            this.checkBox_AllowReadEncryptedFiles.CheckedChanged += new System.EventHandler(this.checkBox_AllowReadEncryptedFiles_CheckedChanged);
            // 
            // checkBox_Read
            // 
            this.checkBox_Read.AutoSize = true;
            this.checkBox_Read.Checked = true;
            this.checkBox_Read.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_Read.Location = new System.Drawing.Point(9, 78);
            this.checkBox_Read.Name = "checkBox_Read";
            this.checkBox_Read.Size = new System.Drawing.Size(105, 17);
            this.checkBox_Read.TabIndex = 44;
            this.checkBox_Read.Text = "Allow file reading";
            this.checkBox_Read.UseVisualStyleBackColor = true;
            this.checkBox_Read.CheckedChanged += new System.EventHandler(this.checkBox_Read_CheckedChanged);
            // 
            // checkBox_Write
            // 
            this.checkBox_Write.AutoSize = true;
            this.checkBox_Write.Checked = true;
            this.checkBox_Write.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_Write.Location = new System.Drawing.Point(149, 77);
            this.checkBox_Write.Name = "checkBox_Write";
            this.checkBox_Write.Size = new System.Drawing.Size(100, 17);
            this.checkBox_Write.TabIndex = 38;
            this.checkBox_Write.Text = "Allow file writing";
            this.checkBox_Write.UseVisualStyleBackColor = true;
            this.checkBox_Write.CheckedChanged += new System.EventHandler(this.checkBox_Write_CheckedChanged);
            // 
            // checkBox_AllowDelete
            // 
            this.checkBox_AllowDelete.AutoSize = true;
            this.checkBox_AllowDelete.Checked = true;
            this.checkBox_AllowDelete.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_AllowDelete.Location = new System.Drawing.Point(340, 77);
            this.checkBox_AllowDelete.Name = "checkBox_AllowDelete";
            this.checkBox_AllowDelete.Size = new System.Drawing.Size(107, 17);
            this.checkBox_AllowDelete.TabIndex = 40;
            this.checkBox_AllowDelete.Text = "Allow file deletion";
            this.checkBox_AllowDelete.UseVisualStyleBackColor = true;
            this.checkBox_AllowDelete.CheckedChanged += new System.EventHandler(this.checkBox_AllowDelete_CheckedChanged);
            // 
            // checkBox_AllowRename
            // 
            this.checkBox_AllowRename.AutoSize = true;
            this.checkBox_AllowRename.Checked = true;
            this.checkBox_AllowRename.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_AllowRename.Location = new System.Drawing.Point(9, 101);
            this.checkBox_AllowRename.Name = "checkBox_AllowRename";
            this.checkBox_AllowRename.Size = new System.Drawing.Size(113, 17);
            this.checkBox_AllowRename.TabIndex = 39;
            this.checkBox_AllowRename.Text = "Allow file renaming";
            this.checkBox_AllowRename.UseVisualStyleBackColor = true;
            this.checkBox_AllowRename.CheckedChanged += new System.EventHandler(this.checkBox_AllowRename_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 13);
            this.label2.TabIndex = 32;
            this.label2.Text = "Access Control Flags";
            // 
            // textBox_FileAccessFlags
            // 
            this.textBox_FileAccessFlags.Location = new System.Drawing.Point(149, 35);
            this.textBox_FileAccessFlags.Name = "textBox_FileAccessFlags";
            this.textBox_FileAccessFlags.Size = new System.Drawing.Size(298, 20);
            this.textBox_FileAccessFlags.TabIndex = 31;
            this.textBox_FileAccessFlags.Text = "0";
            // 
            // button_FileAccessFlags
            // 
            this.button_FileAccessFlags.Location = new System.Drawing.Point(461, 35);
            this.button_FileAccessFlags.Name = "button_FileAccessFlags";
            this.button_FileAccessFlags.Size = new System.Drawing.Size(50, 20);
            this.button_FileAccessFlags.TabIndex = 33;
            this.button_FileAccessFlags.Text = "...";
            this.button_FileAccessFlags.UseVisualStyleBackColor = true;
            this.button_FileAccessFlags.Click += new System.EventHandler(this.button_FileAccessFlags_Click);
            // 
            // groupBox_ProcessName
            // 
            this.groupBox_ProcessName.Controls.Add(this.button_InfoProcessName);
            this.groupBox_ProcessName.Controls.Add(this.textBox_ProcessName);
            this.groupBox_ProcessName.Controls.Add(this.label_AccessFlags);
            this.groupBox_ProcessName.Location = new System.Drawing.Point(18, 21);
            this.groupBox_ProcessName.Name = "groupBox_ProcessName";
            this.groupBox_ProcessName.Size = new System.Drawing.Size(535, 48);
            this.groupBox_ProcessName.TabIndex = 126;
            this.groupBox_ProcessName.TabStop = false;
            this.groupBox_ProcessName.Visible = false;
            // 
            // button_InfoProcessName
            // 
            this.button_InfoProcessName.Image = global::ZeroTrustDemo.Properties.Resources.about;
            this.button_InfoProcessName.Location = new System.Drawing.Point(461, 16);
            this.button_InfoProcessName.Name = "button_InfoProcessName";
            this.button_InfoProcessName.Size = new System.Drawing.Size(41, 20);
            this.button_InfoProcessName.TabIndex = 114;
            this.button_InfoProcessName.UseVisualStyleBackColor = true;
            this.button_InfoProcessName.Click += new System.EventHandler(this.button_InfoProcessName_Click);
            // 
            // textBox_ProcessName
            // 
            this.textBox_ProcessName.Location = new System.Drawing.Point(149, 16);
            this.textBox_ProcessName.Name = "textBox_ProcessName";
            this.textBox_ProcessName.Size = new System.Drawing.Size(298, 20);
            this.textBox_ProcessName.TabIndex = 27;
            this.textBox_ProcessName.Text = "notepad.exe";
            // 
            // label_AccessFlags
            // 
            this.label_AccessFlags.AutoSize = true;
            this.label_AccessFlags.Location = new System.Drawing.Point(9, 19);
            this.label_AccessFlags.Name = "label_AccessFlags";
            this.label_AccessFlags.Size = new System.Drawing.Size(124, 13);
            this.label_AccessFlags.TabIndex = 28;
            this.label_AccessFlags.Text = "Process name filter mask";
            // 
            // AccessRightForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(579, 417);
            this.Controls.Add(this.groupBox_SignedProcess);
            this.Controls.Add(this.groupBox_ProcessSha256);
            this.Controls.Add(this.groupBox_UserName);
            this.Controls.Add(this.groupBox_AccessRights);
            this.Controls.Add(this.groupBox_ProcessName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AccessRightForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Access right settings";
            this.groupBox_SignedProcess.ResumeLayout(false);
            this.groupBox_SignedProcess.PerformLayout();
            this.groupBox_ProcessSha256.ResumeLayout(false);
            this.groupBox_ProcessSha256.PerformLayout();
            this.groupBox_UserName.ResumeLayout(false);
            this.groupBox_UserName.PerformLayout();
            this.groupBox_AccessRights.ResumeLayout(false);
            this.groupBox_AccessRights.PerformLayout();
            this.groupBox_ProcessName.ResumeLayout(false);
            this.groupBox_ProcessName.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox_SignedProcess;
        private System.Windows.Forms.Button button_GetCertificateName;
        private System.Windows.Forms.TextBox textBox_ProcessCertificateName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox_ProcessSha256;
        private System.Windows.Forms.Button button_GetProcessSha256;
        private System.Windows.Forms.TextBox textBox_ProcessSha256Hash;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox_UserName;
        private System.Windows.Forms.TextBox textBox_UserName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox_AccessRights;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_ApplySettings;
        private System.Windows.Forms.CheckBox checkBox_AllowReadEncryptedFiles;
        private System.Windows.Forms.CheckBox checkBox_Read;
        private System.Windows.Forms.CheckBox checkBox_Write;
        private System.Windows.Forms.CheckBox checkBox_AllowDelete;
        private System.Windows.Forms.CheckBox checkBox_AllowRename;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_FileAccessFlags;
        private System.Windows.Forms.Button button_FileAccessFlags;
        private System.Windows.Forms.GroupBox groupBox_ProcessName;
        private System.Windows.Forms.Button button_InfoProcessName;
        private System.Windows.Forms.TextBox textBox_ProcessName;
        private System.Windows.Forms.Label label_AccessFlags;
        private System.Windows.Forms.Button button_InfoUserName;
    }
}