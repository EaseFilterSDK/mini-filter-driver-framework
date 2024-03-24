namespace EaseFilter.CommonObjects
{
    partial class ControlFilterRuleSettigs
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox_AccessControl = new System.Windows.Forms.GroupBox();
            this.button_InfoControlFlag = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button_InfoControlEvents = new System.Windows.Forms.Button();
            this.label17 = new System.Windows.Forms.Label();
            this.textBox_ControlIO = new System.Windows.Forms.TextBox();
            this.button_RegisterControlIO = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button_InfoSignedProcessRights = new System.Windows.Forms.Button();
            this.button_AddSignedProcessAccessRights = new System.Windows.Forms.Button();
            this.textBox_SignedProcessAccessRights = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button_InfoTrustedProcessRights = new System.Windows.Forms.Button();
            this.button_AddTrustedProcessRights = new System.Windows.Forms.Button();
            this.textBox_Sha256ProcessAccessRights = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button_InfoProcessNameRights = new System.Windows.Forms.Button();
            this.button_InfoProcessIdRights = new System.Windows.Forms.Button();
            this.button_InfoUserRights = new System.Windows.Forms.Button();
            this.textBox_UserRights = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_AddProcessRights = new System.Windows.Forms.Button();
            this.textBox_ProcessIdRights = new System.Windows.Forms.TextBox();
            this.textBox_ProcessRights = new System.Windows.Forms.TextBox();
            this.button_AddProcessIdRights = new System.Windows.Forms.Button();
            this.label22 = new System.Windows.Forms.Label();
            this.button_AddUserRights = new System.Windows.Forms.Button();
            this.label23 = new System.Windows.Forms.Label();
            this.button_SaveControlSettings = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox_EnableSendDeniedEvent = new System.Windows.Forms.CheckBox();
            this.button_InfoEncryptKeyLenght = new System.Windows.Forms.Button();
            this.button_InfoEncryptNewFile = new System.Windows.Forms.Button();
            this.button_InfoCopyout = new System.Windows.Forms.Button();
            this.button_InfoDecryption = new System.Windows.Forms.Button();
            this.button_InfoEncryptOnRead = new System.Windows.Forms.Button();
            this.button_InfoPassPhrase = new System.Windows.Forms.Button();
            this.button_HideFileFilterMask = new System.Windows.Forms.Button();
            this.button_InfoReparseFile = new System.Windows.Forms.Button();
            this.radioButton_EncryptFileWithTagData = new System.Windows.Forms.RadioButton();
            this.radioButton_EncryptFileWithSameKey = new System.Windows.Forms.RadioButton();
            this.checkBox_EnableReparseFile = new System.Windows.Forms.CheckBox();
            this.checkBox_EnableHidenFile = new System.Windows.Forms.CheckBox();
            this.checkBox_AllowReadEncryptedFiles = new System.Windows.Forms.CheckBox();
            this.checkBox_AllowEncryptNewFile = new System.Windows.Forms.CheckBox();
            this.checkBox_AllowCopyOut = new System.Windows.Forms.CheckBox();
            this.checkBox_AllowSetSecurity = new System.Windows.Forms.CheckBox();
            this.checkBox_AllowFileWriting = new System.Windows.Forms.CheckBox();
            this.checkBox_EnableProtectionInBootTime = new System.Windows.Forms.CheckBox();
            this.checkBox_AllowChange = new System.Windows.Forms.CheckBox();
            this.checkBox_AllowDelete = new System.Windows.Forms.CheckBox();
            this.checkBox_AllowRename = new System.Windows.Forms.CheckBox();
            this.checkBox_AllowRemoteAccess = new System.Windows.Forms.CheckBox();
            this.textBox_ReparseFileFilterMask = new System.Windows.Forms.TextBox();
            this.checkBox_AllowNewFileCreation = new System.Windows.Forms.CheckBox();
            this.checkBox_Encryption = new System.Windows.Forms.CheckBox();
            this.textBox_PassPhrase = new System.Windows.Forms.TextBox();
            this.textBox_HiddenFilterMask = new System.Windows.Forms.TextBox();
            this.label_AccessFlags = new System.Windows.Forms.Label();
            this.textBox_FileAccessFlags = new System.Windows.Forms.TextBox();
            this.button_FileAccessFlags = new System.Windows.Forms.Button();
            this.textBox_EncryptWriteBufferSize = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button_EncryptWriteBufferSize = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox_AccessControl.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox_AccessControl);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(573, 660);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            // 
            // groupBox_AccessControl
            // 
            this.groupBox_AccessControl.Controls.Add(this.button_InfoControlFlag);
            this.groupBox_AccessControl.Controls.Add(this.groupBox4);
            this.groupBox_AccessControl.Controls.Add(this.groupBox3);
            this.groupBox_AccessControl.Controls.Add(this.button_SaveControlSettings);
            this.groupBox_AccessControl.Controls.Add(this.groupBox2);
            this.groupBox_AccessControl.Controls.Add(this.label_AccessFlags);
            this.groupBox_AccessControl.Controls.Add(this.textBox_FileAccessFlags);
            this.groupBox_AccessControl.Controls.Add(this.button_FileAccessFlags);
            this.groupBox_AccessControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_AccessControl.Location = new System.Drawing.Point(3, 16);
            this.groupBox_AccessControl.Name = "groupBox_AccessControl";
            this.groupBox_AccessControl.Size = new System.Drawing.Size(567, 641);
            this.groupBox_AccessControl.TabIndex = 24;
            this.groupBox_AccessControl.TabStop = false;
            // 
            // button_InfoControlFlag
            // 
            this.button_InfoControlFlag.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
            this.button_InfoControlFlag.Location = new System.Drawing.Point(530, 8);
            this.button_InfoControlFlag.Name = "button_InfoControlFlag";
            this.button_InfoControlFlag.Size = new System.Drawing.Size(28, 20);
            this.button_InfoControlFlag.TabIndex = 118;
            this.button_InfoControlFlag.UseVisualStyleBackColor = true;
            this.button_InfoControlFlag.Click += new System.EventHandler(this.button_InfoControlFlag_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.button_InfoControlEvents);
            this.groupBox4.Controls.Add(this.label17);
            this.groupBox4.Controls.Add(this.textBox_ControlIO);
            this.groupBox4.Controls.Add(this.button_RegisterControlIO);
            this.groupBox4.Location = new System.Drawing.Point(6, 512);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(561, 71);
            this.groupBox4.TabIndex = 91;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Register File I/O Callback Notification Settings";
            // 
            // button_InfoControlEvents
            // 
            this.button_InfoControlEvents.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
            this.button_InfoControlEvents.Location = new System.Drawing.Point(524, 31);
            this.button_InfoControlEvents.Name = "button_InfoControlEvents";
            this.button_InfoControlEvents.Size = new System.Drawing.Size(28, 20);
            this.button_InfoControlEvents.TabIndex = 127;
            this.button_InfoControlEvents.UseVisualStyleBackColor = true;
            this.button_InfoControlEvents.Click += new System.EventHandler(this.button_InfoControlEvents_Click);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(12, 34);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(130, 13);
            this.label17.TabIndex = 72;
            this.label17.Text = "Register control IO events";
            // 
            // textBox_ControlIO
            // 
            this.textBox_ControlIO.Location = new System.Drawing.Point(215, 32);
            this.textBox_ControlIO.Name = "textBox_ControlIO";
            this.textBox_ControlIO.ReadOnly = true;
            this.textBox_ControlIO.Size = new System.Drawing.Size(242, 20);
            this.textBox_ControlIO.TabIndex = 73;
            this.textBox_ControlIO.Text = "0";
            // 
            // button_RegisterControlIO
            // 
            this.button_RegisterControlIO.Location = new System.Drawing.Point(473, 31);
            this.button_RegisterControlIO.Name = "button_RegisterControlIO";
            this.button_RegisterControlIO.Size = new System.Drawing.Size(41, 20);
            this.button_RegisterControlIO.TabIndex = 74;
            this.button_RegisterControlIO.Text = "...";
            this.button_RegisterControlIO.UseVisualStyleBackColor = true;
            this.button_RegisterControlIO.Click += new System.EventHandler(this.button_RegisterControlIO_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button_InfoSignedProcessRights);
            this.groupBox3.Controls.Add(this.button_AddSignedProcessAccessRights);
            this.groupBox3.Controls.Add(this.textBox_SignedProcessAccessRights);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.button_InfoTrustedProcessRights);
            this.groupBox3.Controls.Add(this.button_AddTrustedProcessRights);
            this.groupBox3.Controls.Add(this.textBox_Sha256ProcessAccessRights);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.button_InfoProcessNameRights);
            this.groupBox3.Controls.Add(this.button_InfoProcessIdRights);
            this.groupBox3.Controls.Add(this.button_InfoUserRights);
            this.groupBox3.Controls.Add(this.textBox_UserRights);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.button_AddProcessRights);
            this.groupBox3.Controls.Add(this.textBox_ProcessIdRights);
            this.groupBox3.Controls.Add(this.textBox_ProcessRights);
            this.groupBox3.Controls.Add(this.button_AddProcessIdRights);
            this.groupBox3.Controls.Add(this.label22);
            this.groupBox3.Controls.Add(this.button_AddUserRights);
            this.groupBox3.Controls.Add(this.label23);
            this.groupBox3.Location = new System.Drawing.Point(9, 326);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(555, 180);
            this.groupBox3.TabIndex = 90;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Add or Remove Access Rights to Processes or Users (create whiltelist or blacklist" +
    ")";
            // 
            // button_InfoSignedProcessRights
            // 
            this.button_InfoSignedProcessRights.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
            this.button_InfoSignedProcessRights.Location = new System.Drawing.Point(518, 88);
            this.button_InfoSignedProcessRights.Name = "button_InfoSignedProcessRights";
            this.button_InfoSignedProcessRights.Size = new System.Drawing.Size(28, 20);
            this.button_InfoSignedProcessRights.TabIndex = 134;
            this.button_InfoSignedProcessRights.UseVisualStyleBackColor = true;
            this.button_InfoSignedProcessRights.Click += new System.EventHandler(this.button_InfoSignedProcessRights_Click);
            // 
            // button_AddSignedProcessAccessRights
            // 
            this.button_AddSignedProcessAccessRights.Location = new System.Drawing.Point(470, 87);
            this.button_AddSignedProcessAccessRights.Name = "button_AddSignedProcessAccessRights";
            this.button_AddSignedProcessAccessRights.Size = new System.Drawing.Size(41, 20);
            this.button_AddSignedProcessAccessRights.TabIndex = 133;
            this.button_AddSignedProcessAccessRights.Text = "Add";
            this.button_AddSignedProcessAccessRights.UseVisualStyleBackColor = true;
            this.button_AddSignedProcessAccessRights.Click += new System.EventHandler(this.button_AddSignedProcessAccessRights_Click);
            // 
            // textBox_SignedProcessAccessRights
            // 
            this.textBox_SignedProcessAccessRights.Location = new System.Drawing.Point(212, 88);
            this.textBox_SignedProcessAccessRights.Name = "textBox_SignedProcessAccessRights";
            this.textBox_SignedProcessAccessRights.Size = new System.Drawing.Size(242, 20);
            this.textBox_SignedProcessAccessRights.TabIndex = 131;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 13);
            this.label4.TabIndex = 132;
            this.label4.Text = "Signed process rights";
            // 
            // button_InfoTrustedProcessRights
            // 
            this.button_InfoTrustedProcessRights.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
            this.button_InfoTrustedProcessRights.Location = new System.Drawing.Point(521, 55);
            this.button_InfoTrustedProcessRights.Name = "button_InfoTrustedProcessRights";
            this.button_InfoTrustedProcessRights.Size = new System.Drawing.Size(28, 20);
            this.button_InfoTrustedProcessRights.TabIndex = 130;
            this.button_InfoTrustedProcessRights.UseVisualStyleBackColor = true;
            this.button_InfoTrustedProcessRights.Click += new System.EventHandler(this.button_InfoSha256ProcessRights_Click);
            // 
            // button_AddTrustedProcessRights
            // 
            this.button_AddTrustedProcessRights.Location = new System.Drawing.Point(473, 54);
            this.button_AddTrustedProcessRights.Name = "button_AddTrustedProcessRights";
            this.button_AddTrustedProcessRights.Size = new System.Drawing.Size(41, 20);
            this.button_AddTrustedProcessRights.TabIndex = 129;
            this.button_AddTrustedProcessRights.Text = "Add";
            this.button_AddTrustedProcessRights.UseVisualStyleBackColor = true;
            this.button_AddTrustedProcessRights.Click += new System.EventHandler(this.button_AddSha256ProcessAccessRights_Click);
            // 
            // textBox_Sha256ProcessAccessRights
            // 
            this.textBox_Sha256ProcessAccessRights.Location = new System.Drawing.Point(212, 55);
            this.textBox_Sha256ProcessAccessRights.Name = "textBox_Sha256ProcessAccessRights";
            this.textBox_Sha256ProcessAccessRights.Size = new System.Drawing.Size(242, 20);
            this.textBox_Sha256ProcessAccessRights.TabIndex = 127;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 13);
            this.label3.TabIndex = 128;
            this.label3.Text = "Sha256 process rights";
            // 
            // button_InfoProcessNameRights
            // 
            this.button_InfoProcessNameRights.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
            this.button_InfoProcessNameRights.Location = new System.Drawing.Point(521, 22);
            this.button_InfoProcessNameRights.Name = "button_InfoProcessNameRights";
            this.button_InfoProcessNameRights.Size = new System.Drawing.Size(28, 20);
            this.button_InfoProcessNameRights.TabIndex = 124;
            this.button_InfoProcessNameRights.UseVisualStyleBackColor = true;
            this.button_InfoProcessNameRights.Click += new System.EventHandler(this.button_InfoProcessNameRights_Click);
            // 
            // button_InfoProcessIdRights
            // 
            this.button_InfoProcessIdRights.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
            this.button_InfoProcessIdRights.Location = new System.Drawing.Point(521, 121);
            this.button_InfoProcessIdRights.Name = "button_InfoProcessIdRights";
            this.button_InfoProcessIdRights.Size = new System.Drawing.Size(28, 20);
            this.button_InfoProcessIdRights.TabIndex = 125;
            this.button_InfoProcessIdRights.UseVisualStyleBackColor = true;
            this.button_InfoProcessIdRights.Click += new System.EventHandler(this.button_InfoProcessIdRights_Click);
            // 
            // button_InfoUserRights
            // 
            this.button_InfoUserRights.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
            this.button_InfoUserRights.Location = new System.Drawing.Point(521, 153);
            this.button_InfoUserRights.Name = "button_InfoUserRights";
            this.button_InfoUserRights.Size = new System.Drawing.Size(28, 20);
            this.button_InfoUserRights.TabIndex = 126;
            this.button_InfoUserRights.UseVisualStyleBackColor = true;
            this.button_InfoUserRights.Click += new System.EventHandler(this.button_InfoUserRights_Click);
            // 
            // textBox_UserRights
            // 
            this.textBox_UserRights.Location = new System.Drawing.Point(212, 153);
            this.textBox_UserRights.Name = "textBox_UserRights";
            this.textBox_UserRights.Size = new System.Drawing.Size(242, 20);
            this.textBox_UserRights.TabIndex = 80;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 123);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 88;
            this.label1.Text = "Processes Id rights";
            // 
            // button_AddProcessRights
            // 
            this.button_AddProcessRights.Location = new System.Drawing.Point(473, 21);
            this.button_AddProcessRights.Name = "button_AddProcessRights";
            this.button_AddProcessRights.Size = new System.Drawing.Size(41, 20);
            this.button_AddProcessRights.TabIndex = 79;
            this.button_AddProcessRights.Text = "Add";
            this.button_AddProcessRights.UseVisualStyleBackColor = true;
            this.button_AddProcessRights.Click += new System.EventHandler(this.button_AddProcessRights_Click);
            // 
            // textBox_ProcessIdRights
            // 
            this.textBox_ProcessIdRights.Location = new System.Drawing.Point(212, 124);
            this.textBox_ProcessIdRights.Name = "textBox_ProcessIdRights";
            this.textBox_ProcessIdRights.Size = new System.Drawing.Size(242, 20);
            this.textBox_ProcessIdRights.TabIndex = 87;
            // 
            // textBox_ProcessRights
            // 
            this.textBox_ProcessRights.Location = new System.Drawing.Point(212, 22);
            this.textBox_ProcessRights.Name = "textBox_ProcessRights";
            this.textBox_ProcessRights.Size = new System.Drawing.Size(242, 20);
            this.textBox_ProcessRights.TabIndex = 77;
            // 
            // button_AddProcessIdRights
            // 
            this.button_AddProcessIdRights.Location = new System.Drawing.Point(473, 123);
            this.button_AddProcessIdRights.Name = "button_AddProcessIdRights";
            this.button_AddProcessIdRights.Size = new System.Drawing.Size(41, 20);
            this.button_AddProcessIdRights.TabIndex = 89;
            this.button_AddProcessIdRights.Text = "Add";
            this.button_AddProcessIdRights.UseVisualStyleBackColor = true;
            this.button_AddProcessIdRights.Click += new System.EventHandler(this.button_AddProcessIdRights_Click);
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(12, 21);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(113, 13);
            this.label22.TabIndex = 78;
            this.label22.Text = "Processes name rights";
            // 
            // button_AddUserRights
            // 
            this.button_AddUserRights.Location = new System.Drawing.Point(473, 153);
            this.button_AddUserRights.Name = "button_AddUserRights";
            this.button_AddUserRights.Size = new System.Drawing.Size(41, 20);
            this.button_AddUserRights.TabIndex = 82;
            this.button_AddUserRights.Text = "Add";
            this.button_AddUserRights.UseVisualStyleBackColor = true;
            this.button_AddUserRights.Click += new System.EventHandler(this.button_AddUserRights_Click);
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(12, 157);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(62, 13);
            this.label23.TabIndex = 81;
            this.label23.Text = "Users rights";
            // 
            // button_SaveControlSettings
            // 
            this.button_SaveControlSettings.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_SaveControlSettings.Location = new System.Drawing.Point(390, 588);
            this.button_SaveControlSettings.Margin = new System.Windows.Forms.Padding(2);
            this.button_SaveControlSettings.Name = "button_SaveControlSettings";
            this.button_SaveControlSettings.Size = new System.Drawing.Size(130, 22);
            this.button_SaveControlSettings.TabIndex = 85;
            this.button_SaveControlSettings.Text = "Save Control Settings";
            this.button_SaveControlSettings.UseVisualStyleBackColor = true;
            this.button_SaveControlSettings.Click += new System.EventHandler(this.button_SaveControlSettings_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button_EncryptWriteBufferSize);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.textBox_EncryptWriteBufferSize);
            this.groupBox2.Controls.Add(this.checkBox_EnableSendDeniedEvent);
            this.groupBox2.Controls.Add(this.button_InfoEncryptKeyLenght);
            this.groupBox2.Controls.Add(this.button_InfoEncryptNewFile);
            this.groupBox2.Controls.Add(this.button_InfoCopyout);
            this.groupBox2.Controls.Add(this.button_InfoDecryption);
            this.groupBox2.Controls.Add(this.button_InfoEncryptOnRead);
            this.groupBox2.Controls.Add(this.button_InfoPassPhrase);
            this.groupBox2.Controls.Add(this.button_HideFileFilterMask);
            this.groupBox2.Controls.Add(this.button_InfoReparseFile);
            this.groupBox2.Controls.Add(this.radioButton_EncryptFileWithTagData);
            this.groupBox2.Controls.Add(this.radioButton_EncryptFileWithSameKey);
            this.groupBox2.Controls.Add(this.checkBox_EnableReparseFile);
            this.groupBox2.Controls.Add(this.checkBox_EnableHidenFile);
            this.groupBox2.Controls.Add(this.checkBox_AllowReadEncryptedFiles);
            this.groupBox2.Controls.Add(this.checkBox_AllowEncryptNewFile);
            this.groupBox2.Controls.Add(this.checkBox_AllowCopyOut);
            this.groupBox2.Controls.Add(this.checkBox_AllowSetSecurity);
            this.groupBox2.Controls.Add(this.checkBox_AllowFileWriting);
            this.groupBox2.Controls.Add(this.checkBox_EnableProtectionInBootTime);
            this.groupBox2.Controls.Add(this.checkBox_AllowChange);
            this.groupBox2.Controls.Add(this.checkBox_AllowDelete);
            this.groupBox2.Controls.Add(this.checkBox_AllowRename);
            this.groupBox2.Controls.Add(this.checkBox_AllowRemoteAccess);
            this.groupBox2.Controls.Add(this.textBox_ReparseFileFilterMask);
            this.groupBox2.Controls.Add(this.checkBox_AllowNewFileCreation);
            this.groupBox2.Controls.Add(this.checkBox_Encryption);
            this.groupBox2.Controls.Add(this.textBox_PassPhrase);
            this.groupBox2.Controls.Add(this.textBox_HiddenFilterMask);
            this.groupBox2.Location = new System.Drawing.Point(9, 38);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(555, 282);
            this.groupBox2.TabIndex = 76;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Quick Access Control Flags Settings";
            // 
            // checkBox_EnableSendDeniedEvent
            // 
            this.checkBox_EnableSendDeniedEvent.AutoSize = true;
            this.checkBox_EnableSendDeniedEvent.Location = new System.Drawing.Point(11, 256);
            this.checkBox_EnableSendDeniedEvent.Name = "checkBox_EnableSendDeniedEvent";
            this.checkBox_EnableSendDeniedEvent.Size = new System.Drawing.Size(414, 17);
            this.checkBox_EnableSendDeniedEvent.TabIndex = 126;
            this.checkBox_EnableSendDeniedEvent.Text = "Enable send denied file IO event which was blocked by the file access control fla" +
    "g";
            this.checkBox_EnableSendDeniedEvent.UseVisualStyleBackColor = true;
            // 
            // button_InfoEncryptKeyLenght
            // 
            this.button_InfoEncryptKeyLenght.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
            this.button_InfoEncryptKeyLenght.Location = new System.Drawing.Point(521, 139);
            this.button_InfoEncryptKeyLenght.Name = "button_InfoEncryptKeyLenght";
            this.button_InfoEncryptKeyLenght.Size = new System.Drawing.Size(28, 20);
            this.button_InfoEncryptKeyLenght.TabIndex = 119;
            this.button_InfoEncryptKeyLenght.UseVisualStyleBackColor = true;
            this.button_InfoEncryptKeyLenght.Click += new System.EventHandler(this.button_EnableEncryptionKeyFromService_Click);
            // 
            // button_InfoEncryptNewFile
            // 
            this.button_InfoEncryptNewFile.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
            this.button_InfoEncryptNewFile.Location = new System.Drawing.Point(521, 40);
            this.button_InfoEncryptNewFile.Name = "button_InfoEncryptNewFile";
            this.button_InfoEncryptNewFile.Size = new System.Drawing.Size(28, 20);
            this.button_InfoEncryptNewFile.TabIndex = 125;
            this.button_InfoEncryptNewFile.UseVisualStyleBackColor = true;
            this.button_InfoEncryptNewFile.Click += new System.EventHandler(this.button_InfoEncryptNewFile_Click);
            // 
            // button_InfoCopyout
            // 
            this.button_InfoCopyout.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
            this.button_InfoCopyout.Location = new System.Drawing.Point(521, 19);
            this.button_InfoCopyout.Name = "button_InfoCopyout";
            this.button_InfoCopyout.Size = new System.Drawing.Size(28, 20);
            this.button_InfoCopyout.TabIndex = 124;
            this.button_InfoCopyout.UseVisualStyleBackColor = true;
            this.button_InfoCopyout.Click += new System.EventHandler(this.button_InfoCopyout_Click);
            // 
            // button_InfoDecryption
            // 
            this.button_InfoDecryption.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
            this.button_InfoDecryption.Location = new System.Drawing.Point(521, 66);
            this.button_InfoDecryption.Name = "button_InfoDecryption";
            this.button_InfoDecryption.Size = new System.Drawing.Size(28, 20);
            this.button_InfoDecryption.TabIndex = 119;
            this.button_InfoDecryption.UseVisualStyleBackColor = true;
            this.button_InfoDecryption.Click += new System.EventHandler(this.button_InfoDecryption_Click);
            // 
            // button_InfoEncryptOnRead
            // 
            this.button_InfoEncryptOnRead.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
            this.button_InfoEncryptOnRead.Location = new System.Drawing.Point(521, 92);
            this.button_InfoEncryptOnRead.Name = "button_InfoEncryptOnRead";
            this.button_InfoEncryptOnRead.Size = new System.Drawing.Size(28, 20);
            this.button_InfoEncryptOnRead.TabIndex = 120;
            this.button_InfoEncryptOnRead.UseVisualStyleBackColor = true;
            this.button_InfoEncryptOnRead.Click += new System.EventHandler(this.button_InfoEncryptOnRead_Click);
            // 
            // button_InfoPassPhrase
            // 
            this.button_InfoPassPhrase.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
            this.button_InfoPassPhrase.Location = new System.Drawing.Point(521, 118);
            this.button_InfoPassPhrase.Name = "button_InfoPassPhrase";
            this.button_InfoPassPhrase.Size = new System.Drawing.Size(28, 20);
            this.button_InfoPassPhrase.TabIndex = 121;
            this.button_InfoPassPhrase.UseVisualStyleBackColor = true;
            this.button_InfoPassPhrase.Click += new System.EventHandler(this.button_InfoPassPhrase_Click);
            // 
            // button_HideFileFilterMask
            // 
            this.button_HideFileFilterMask.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
            this.button_HideFileFilterMask.Location = new System.Drawing.Point(521, 196);
            this.button_HideFileFilterMask.Name = "button_HideFileFilterMask";
            this.button_HideFileFilterMask.Size = new System.Drawing.Size(28, 20);
            this.button_HideFileFilterMask.TabIndex = 122;
            this.button_HideFileFilterMask.UseVisualStyleBackColor = true;
            this.button_HideFileFilterMask.Click += new System.EventHandler(this.button_HideFileFilterMask_Click);
            // 
            // button_InfoReparseFile
            // 
            this.button_InfoReparseFile.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
            this.button_InfoReparseFile.Location = new System.Drawing.Point(521, 225);
            this.button_InfoReparseFile.Name = "button_InfoReparseFile";
            this.button_InfoReparseFile.Size = new System.Drawing.Size(28, 20);
            this.button_InfoReparseFile.TabIndex = 123;
            this.button_InfoReparseFile.UseVisualStyleBackColor = true;
            this.button_InfoReparseFile.Click += new System.EventHandler(this.button_InfoReparseFile_Click);
            // 
            // radioButton_EncryptFileWithTagData
            // 
            this.radioButton_EncryptFileWithTagData.AutoSize = true;
            this.radioButton_EncryptFileWithTagData.Location = new System.Drawing.Point(158, 138);
            this.radioButton_EncryptFileWithTagData.Name = "radioButton_EncryptFileWithTagData";
            this.radioButton_EncryptFileWithTagData.Size = new System.Drawing.Size(343, 17);
            this.radioButton_EncryptFileWithTagData.TabIndex = 93;
            this.radioButton_EncryptFileWithTagData.Text = "Encrypt file with unique encryption key, iv and tag data from service";
            this.radioButton_EncryptFileWithTagData.UseVisualStyleBackColor = true;
            // 
            // radioButton_EncryptFileWithSameKey
            // 
            this.radioButton_EncryptFileWithSameKey.AutoSize = true;
            this.radioButton_EncryptFileWithSameKey.Checked = true;
            this.radioButton_EncryptFileWithSameKey.Location = new System.Drawing.Point(158, 117);
            this.radioButton_EncryptFileWithSameKey.Name = "radioButton_EncryptFileWithSameKey";
            this.radioButton_EncryptFileWithSameKey.Size = new System.Drawing.Size(248, 17);
            this.radioButton_EncryptFileWithSameKey.TabIndex = 91;
            this.radioButton_EncryptFileWithSameKey.TabStop = true;
            this.radioButton_EncryptFileWithSameKey.Text = "Encrypt file with same root key from passphrase";
            this.radioButton_EncryptFileWithSameKey.UseVisualStyleBackColor = true;
            // 
            // checkBox_EnableReparseFile
            // 
            this.checkBox_EnableReparseFile.AutoSize = true;
            this.checkBox_EnableReparseFile.Location = new System.Drawing.Point(12, 225);
            this.checkBox_EnableReparseFile.Name = "checkBox_EnableReparseFile";
            this.checkBox_EnableReparseFile.Size = new System.Drawing.Size(190, 17);
            this.checkBox_EnableReparseFile.TabIndex = 31;
            this.checkBox_EnableReparseFile.Text = "Enable reparse files with filter mask";
            this.checkBox_EnableReparseFile.UseVisualStyleBackColor = true;
            this.checkBox_EnableReparseFile.CheckedChanged += new System.EventHandler(this.checkBox_EnableReparseFile_CheckedChanged);
            // 
            // checkBox_EnableHidenFile
            // 
            this.checkBox_EnableHidenFile.AutoSize = true;
            this.checkBox_EnableHidenFile.Location = new System.Drawing.Point(12, 190);
            this.checkBox_EnableHidenFile.Name = "checkBox_EnableHidenFile";
            this.checkBox_EnableHidenFile.Size = new System.Drawing.Size(175, 17);
            this.checkBox_EnableHidenFile.TabIndex = 30;
            this.checkBox_EnableHidenFile.Text = "Enable hide files with filter mask";
            this.checkBox_EnableHidenFile.UseVisualStyleBackColor = true;
            this.checkBox_EnableHidenFile.CheckedChanged += new System.EventHandler(this.checkBox_EnableHidenFile_CheckedChanged);
            // 
            // checkBox_AllowReadEncryptedFiles
            // 
            this.checkBox_AllowReadEncryptedFiles.AutoSize = true;
            this.checkBox_AllowReadEncryptedFiles.Location = new System.Drawing.Point(349, 69);
            this.checkBox_AllowReadEncryptedFiles.Name = "checkBox_AllowReadEncryptedFiles";
            this.checkBox_AllowReadEncryptedFiles.Size = new System.Drawing.Size(141, 17);
            this.checkBox_AllowReadEncryptedFiles.TabIndex = 28;
            this.checkBox_AllowReadEncryptedFiles.Text = "Allow read encrypted file";
            this.checkBox_AllowReadEncryptedFiles.UseVisualStyleBackColor = true;
            this.checkBox_AllowReadEncryptedFiles.CheckedChanged += new System.EventHandler(this.checkBox_AllowReadEncryptedFiles_CheckedChanged);
            // 
            // checkBox_AllowEncryptNewFile
            // 
            this.checkBox_AllowEncryptNewFile.AutoSize = true;
            this.checkBox_AllowEncryptNewFile.Location = new System.Drawing.Point(349, 46);
            this.checkBox_AllowEncryptNewFile.Name = "checkBox_AllowEncryptNewFile";
            this.checkBox_AllowEncryptNewFile.Size = new System.Drawing.Size(142, 17);
            this.checkBox_AllowEncryptNewFile.TabIndex = 26;
            this.checkBox_AllowEncryptNewFile.Text = "Allow new file encryption";
            this.checkBox_AllowEncryptNewFile.UseVisualStyleBackColor = true;
            this.checkBox_AllowEncryptNewFile.CheckedChanged += new System.EventHandler(this.checkBox_AllowEncryptNewFile_CheckedChanged);
            // 
            // checkBox_AllowCopyOut
            // 
            this.checkBox_AllowCopyOut.AutoSize = true;
            this.checkBox_AllowCopyOut.Location = new System.Drawing.Point(349, 23);
            this.checkBox_AllowCopyOut.Name = "checkBox_AllowCopyOut";
            this.checkBox_AllowCopyOut.Size = new System.Drawing.Size(154, 17);
            this.checkBox_AllowCopyOut.TabIndex = 27;
            this.checkBox_AllowCopyOut.Text = "Allow files being copied out";
            this.checkBox_AllowCopyOut.UseVisualStyleBackColor = true;
            this.checkBox_AllowCopyOut.CheckedChanged += new System.EventHandler(this.checkBox_AllowCopyOut_CheckedChanged);
            // 
            // checkBox_AllowSetSecurity
            // 
            this.checkBox_AllowSetSecurity.AutoSize = true;
            this.checkBox_AllowSetSecurity.Location = new System.Drawing.Point(158, 46);
            this.checkBox_AllowSetSecurity.Name = "checkBox_AllowSetSecurity";
            this.checkBox_AllowSetSecurity.Size = new System.Drawing.Size(137, 17);
            this.checkBox_AllowSetSecurity.TabIndex = 25;
            this.checkBox_AllowSetSecurity.Text = "Allow security changing";
            this.checkBox_AllowSetSecurity.UseVisualStyleBackColor = true;
            this.checkBox_AllowSetSecurity.CheckedChanged += new System.EventHandler(this.checkBox_AllowSetSecurity_CheckedChanged);
            // 
            // checkBox_AllowFileWriting
            // 
            this.checkBox_AllowFileWriting.AutoSize = true;
            this.checkBox_AllowFileWriting.Location = new System.Drawing.Point(12, 46);
            this.checkBox_AllowFileWriting.Name = "checkBox_AllowFileWriting";
            this.checkBox_AllowFileWriting.Size = new System.Drawing.Size(100, 17);
            this.checkBox_AllowFileWriting.TabIndex = 24;
            this.checkBox_AllowFileWriting.Text = "Allow file writing";
            this.checkBox_AllowFileWriting.UseVisualStyleBackColor = true;
            this.checkBox_AllowFileWriting.CheckedChanged += new System.EventHandler(this.checkBox_AllowFileWriting_CheckedChanged);
            // 
            // checkBox_EnableProtectionInBootTime
            // 
            this.checkBox_EnableProtectionInBootTime.AutoSize = true;
            this.checkBox_EnableProtectionInBootTime.Location = new System.Drawing.Point(158, 92);
            this.checkBox_EnableProtectionInBootTime.Name = "checkBox_EnableProtectionInBootTime";
            this.checkBox_EnableProtectionInBootTime.Size = new System.Drawing.Size(166, 17);
            this.checkBox_EnableProtectionInBootTime.TabIndex = 23;
            this.checkBox_EnableProtectionInBootTime.Text = "Enable protection in boot time";
            this.checkBox_EnableProtectionInBootTime.UseVisualStyleBackColor = true;
            // 
            // checkBox_AllowChange
            // 
            this.checkBox_AllowChange.AutoSize = true;
            this.checkBox_AllowChange.Location = new System.Drawing.Point(12, 92);
            this.checkBox_AllowChange.Name = "checkBox_AllowChange";
            this.checkBox_AllowChange.Size = new System.Drawing.Size(134, 17);
            this.checkBox_AllowChange.TabIndex = 15;
            this.checkBox_AllowChange.Text = "Allow file info changing";
            this.checkBox_AllowChange.UseVisualStyleBackColor = true;
            this.checkBox_AllowChange.CheckedChanged += new System.EventHandler(this.checkBox_AllowChange_CheckedChanged);
            // 
            // checkBox_AllowDelete
            // 
            this.checkBox_AllowDelete.AutoSize = true;
            this.checkBox_AllowDelete.Location = new System.Drawing.Point(12, 23);
            this.checkBox_AllowDelete.Name = "checkBox_AllowDelete";
            this.checkBox_AllowDelete.Size = new System.Drawing.Size(107, 17);
            this.checkBox_AllowDelete.TabIndex = 17;
            this.checkBox_AllowDelete.Text = "Allow file deletion";
            this.checkBox_AllowDelete.UseVisualStyleBackColor = true;
            this.checkBox_AllowDelete.CheckedChanged += new System.EventHandler(this.checkBox_AllowDelete_CheckedChanged);
            // 
            // checkBox_AllowRename
            // 
            this.checkBox_AllowRename.AutoSize = true;
            this.checkBox_AllowRename.Location = new System.Drawing.Point(12, 69);
            this.checkBox_AllowRename.Name = "checkBox_AllowRename";
            this.checkBox_AllowRename.Size = new System.Drawing.Size(113, 17);
            this.checkBox_AllowRename.TabIndex = 16;
            this.checkBox_AllowRename.Text = "Allow file renaming";
            this.checkBox_AllowRename.UseVisualStyleBackColor = true;
            this.checkBox_AllowRename.CheckedChanged += new System.EventHandler(this.checkBox_AllowRename_CheckedChanged);
            // 
            // checkBox_AllowRemoteAccess
            // 
            this.checkBox_AllowRemoteAccess.AutoSize = true;
            this.checkBox_AllowRemoteAccess.Location = new System.Drawing.Point(158, 69);
            this.checkBox_AllowRemoteAccess.Name = "checkBox_AllowRemoteAccess";
            this.checkBox_AllowRemoteAccess.Size = new System.Drawing.Size(176, 17);
            this.checkBox_AllowRemoteAccess.TabIndex = 21;
            this.checkBox_AllowRemoteAccess.Text = "Allow file accessing via network";
            this.checkBox_AllowRemoteAccess.UseVisualStyleBackColor = true;
            this.checkBox_AllowRemoteAccess.CheckedChanged += new System.EventHandler(this.checkBox_AllowRemoteAccess_CheckedChanged);
            // 
            // textBox_ReparseFileFilterMask
            // 
            this.textBox_ReparseFileFilterMask.Location = new System.Drawing.Point(212, 225);
            this.textBox_ReparseFileFilterMask.Name = "textBox_ReparseFileFilterMask";
            this.textBox_ReparseFileFilterMask.ReadOnly = true;
            this.textBox_ReparseFileFilterMask.Size = new System.Drawing.Size(302, 20);
            this.textBox_ReparseFileFilterMask.TabIndex = 83;
            // 
            // checkBox_AllowNewFileCreation
            // 
            this.checkBox_AllowNewFileCreation.AutoSize = true;
            this.checkBox_AllowNewFileCreation.Location = new System.Drawing.Point(158, 23);
            this.checkBox_AllowNewFileCreation.Name = "checkBox_AllowNewFileCreation";
            this.checkBox_AllowNewFileCreation.Size = new System.Drawing.Size(131, 17);
            this.checkBox_AllowNewFileCreation.TabIndex = 22;
            this.checkBox_AllowNewFileCreation.Text = "Allow new file creation";
            this.checkBox_AllowNewFileCreation.UseVisualStyleBackColor = true;
            this.checkBox_AllowNewFileCreation.CheckedChanged += new System.EventHandler(this.checkBox_AllowNewFileCreation_CheckedChanged);
            // 
            // checkBox_Encryption
            // 
            this.checkBox_Encryption.AutoSize = true;
            this.checkBox_Encryption.Location = new System.Drawing.Point(12, 118);
            this.checkBox_Encryption.Name = "checkBox_Encryption";
            this.checkBox_Encryption.Size = new System.Drawing.Size(111, 17);
            this.checkBox_Encryption.TabIndex = 18;
            this.checkBox_Encryption.Text = "Enable encryption";
            this.checkBox_Encryption.UseVisualStyleBackColor = true;
            this.checkBox_Encryption.CheckedChanged += new System.EventHandler(this.checkBox_Encryption_CheckedChanged);
            // 
            // textBox_PassPhrase
            // 
            this.textBox_PassPhrase.Location = new System.Drawing.Point(412, 114);
            this.textBox_PassPhrase.Name = "textBox_PassPhrase";
            this.textBox_PassPhrase.ReadOnly = true;
            this.textBox_PassPhrase.Size = new System.Drawing.Size(91, 20);
            this.textBox_PassPhrase.TabIndex = 19;
            this.textBox_PassPhrase.UseSystemPasswordChar = true;
            // 
            // textBox_HiddenFilterMask
            // 
            this.textBox_HiddenFilterMask.Location = new System.Drawing.Point(212, 195);
            this.textBox_HiddenFilterMask.Name = "textBox_HiddenFilterMask";
            this.textBox_HiddenFilterMask.ReadOnly = true;
            this.textBox_HiddenFilterMask.Size = new System.Drawing.Size(302, 20);
            this.textBox_HiddenFilterMask.TabIndex = 24;
            // 
            // label_AccessFlags
            // 
            this.label_AccessFlags.AutoSize = true;
            this.label_AccessFlags.Location = new System.Drawing.Point(17, 12);
            this.label_AccessFlags.Name = "label_AccessFlags";
            this.label_AccessFlags.Size = new System.Drawing.Size(139, 13);
            this.label_AccessFlags.TabIndex = 12;
            this.label_AccessFlags.Text = "The file access control flags";
            // 
            // textBox_FileAccessFlags
            // 
            this.textBox_FileAccessFlags.Location = new System.Drawing.Point(221, 9);
            this.textBox_FileAccessFlags.Name = "textBox_FileAccessFlags";
            this.textBox_FileAccessFlags.Size = new System.Drawing.Size(242, 20);
            this.textBox_FileAccessFlags.TabIndex = 11;
            this.textBox_FileAccessFlags.Text = "0";
            // 
            // button_FileAccessFlags
            // 
            this.button_FileAccessFlags.Location = new System.Drawing.Point(482, 9);
            this.button_FileAccessFlags.Name = "button_FileAccessFlags";
            this.button_FileAccessFlags.Size = new System.Drawing.Size(41, 20);
            this.button_FileAccessFlags.TabIndex = 14;
            this.button_FileAccessFlags.Text = "...";
            this.button_FileAccessFlags.UseVisualStyleBackColor = true;
            this.button_FileAccessFlags.Click += new System.EventHandler(this.button_FileAccessFlags_Click);
            // 
            // textBox_EncryptWriteBufferSize
            // 
            this.textBox_EncryptWriteBufferSize.Location = new System.Drawing.Point(212, 164);
            this.textBox_EncryptWriteBufferSize.Name = "textBox_EncryptWriteBufferSize";
            this.textBox_EncryptWriteBufferSize.Size = new System.Drawing.Size(302, 20);
            this.textBox_EncryptWriteBufferSize.TabIndex = 127;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 164);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(166, 13);
            this.label2.TabIndex = 135;
            this.label2.Text = "Encryption write buffer cache size";
            // 
            // button_EncryptWriteBufferSize
            // 
            this.button_EncryptWriteBufferSize.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
            this.button_EncryptWriteBufferSize.Location = new System.Drawing.Point(521, 164);
            this.button_EncryptWriteBufferSize.Name = "button_EncryptWriteBufferSize";
            this.button_EncryptWriteBufferSize.Size = new System.Drawing.Size(28, 20);
            this.button_EncryptWriteBufferSize.TabIndex = 136;
            this.button_EncryptWriteBufferSize.UseVisualStyleBackColor = true;
            this.button_EncryptWriteBufferSize.Click += new System.EventHandler(this.button_EncryptWriteBufferSize_Click);
            // 
            // ControlFilterRuleSettigs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(573, 660);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ControlFilterRuleSettigs";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Access Control Settings of The Filter Rule";
            this.groupBox1.ResumeLayout(false);
            this.groupBox_AccessControl.ResumeLayout(false);
            this.groupBox_AccessControl.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox_AccessControl;
        private System.Windows.Forms.TextBox textBox_ReparseFileFilterMask;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox textBox_UserRights;
        private System.Windows.Forms.Button button_AddUserRights;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox textBox_ProcessRights;
        private System.Windows.Forms.Button button_AddProcessRights;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBox_AllowSetSecurity;
        private System.Windows.Forms.CheckBox checkBox_AllowFileWriting;
        private System.Windows.Forms.CheckBox checkBox_EnableProtectionInBootTime;
        private System.Windows.Forms.CheckBox checkBox_AllowChange;
        private System.Windows.Forms.CheckBox checkBox_AllowDelete;
        private System.Windows.Forms.CheckBox checkBox_AllowRename;
        private System.Windows.Forms.CheckBox checkBox_AllowRemoteAccess;
        private System.Windows.Forms.CheckBox checkBox_AllowNewFileCreation;
        private System.Windows.Forms.Button button_RegisterControlIO;
        private System.Windows.Forms.TextBox textBox_ControlIO;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox textBox_HiddenFilterMask;
        private System.Windows.Forms.Label label_AccessFlags;
        private System.Windows.Forms.TextBox textBox_FileAccessFlags;
        private System.Windows.Forms.Button button_FileAccessFlags;
        private System.Windows.Forms.TextBox textBox_PassPhrase;
        private System.Windows.Forms.CheckBox checkBox_Encryption;
        private System.Windows.Forms.Button button_SaveControlSettings;
        private System.Windows.Forms.CheckBox checkBox_AllowEncryptNewFile;
        private System.Windows.Forms.CheckBox checkBox_AllowCopyOut;
        private System.Windows.Forms.CheckBox checkBox_AllowReadEncryptedFiles;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_ProcessIdRights;
        private System.Windows.Forms.Button button_AddProcessIdRights;
        private System.Windows.Forms.CheckBox checkBox_EnableReparseFile;
        private System.Windows.Forms.CheckBox checkBox_EnableHidenFile;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton radioButton_EncryptFileWithTagData;
        private System.Windows.Forms.RadioButton radioButton_EncryptFileWithSameKey;
        private System.Windows.Forms.Button button_InfoControlFlag;
        private System.Windows.Forms.Button button_InfoControlEvents;
        private System.Windows.Forms.Button button_InfoProcessNameRights;
        private System.Windows.Forms.Button button_InfoProcessIdRights;
        private System.Windows.Forms.Button button_InfoUserRights;
        private System.Windows.Forms.Button button_InfoEncryptKeyLenght;
        private System.Windows.Forms.Button button_InfoEncryptNewFile;
        private System.Windows.Forms.Button button_InfoCopyout;
        private System.Windows.Forms.Button button_InfoDecryption;
        private System.Windows.Forms.Button button_InfoEncryptOnRead;
        private System.Windows.Forms.Button button_InfoPassPhrase;
        private System.Windows.Forms.Button button_HideFileFilterMask;
        private System.Windows.Forms.Button button_InfoReparseFile;
        private System.Windows.Forms.CheckBox checkBox_EnableSendDeniedEvent;
        private System.Windows.Forms.Button button_InfoTrustedProcessRights;
        private System.Windows.Forms.Button button_AddTrustedProcessRights;
        private System.Windows.Forms.TextBox textBox_Sha256ProcessAccessRights;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button_InfoSignedProcessRights;
        private System.Windows.Forms.Button button_AddSignedProcessAccessRights;
        private System.Windows.Forms.TextBox textBox_SignedProcessAccessRights;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button_EncryptWriteBufferSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_EncryptWriteBufferSize;
    }
}