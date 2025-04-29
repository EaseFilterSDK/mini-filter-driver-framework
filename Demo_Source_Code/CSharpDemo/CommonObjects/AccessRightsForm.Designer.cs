namespace EaseFilter.CommonObjects
{
    partial class Form_AccessRights
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
            this.button_Add = new System.Windows.Forms.Button();
            this.groupBox_ProcessName = new System.Windows.Forms.GroupBox();
            this.button_InfoProcessName = new System.Windows.Forms.Button();
            this.textBox_ProcessName = new System.Windows.Forms.TextBox();
            this.label_AccessFlags = new System.Windows.Forms.Label();
            this.groupBox_AccessRights = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_InfoEncryptOnRead = new System.Windows.Forms.Button();
            this.button_InfoDecryptFile = new System.Windows.Forms.Button();
            this.button_InfoEncryptNewFile = new System.Windows.Forms.Button();
            this.button_InfoCopyout = new System.Windows.Forms.Button();
            this.checkBox_AllowReadEncryptedFiles = new System.Windows.Forms.CheckBox();
            this.checkBox_AllowCopyout = new System.Windows.Forms.CheckBox();
            this.checkBox_AllowEncryptNewFile = new System.Windows.Forms.CheckBox();
            this.checkBox_SetSecurity = new System.Windows.Forms.CheckBox();
            this.checkBox_QueryInfo = new System.Windows.Forms.CheckBox();
            this.checkBox_Read = new System.Windows.Forms.CheckBox();
            this.checkBox_EnableEncryptionOnRead = new System.Windows.Forms.CheckBox();
            this.checkBox_SetInfo = new System.Windows.Forms.CheckBox();
            this.checkBox_Write = new System.Windows.Forms.CheckBox();
            this.checkBox_AllowDelete = new System.Windows.Forms.CheckBox();
            this.checkBox_AllowRename = new System.Windows.Forms.CheckBox();
            this.checkBox_Creation = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_FileAccessFlags = new System.Windows.Forms.TextBox();
            this.button_FileAccessFlags = new System.Windows.Forms.Button();
            this.groupBox_UserName = new System.Windows.Forms.GroupBox();
            this.button_InfoUserName = new System.Windows.Forms.Button();
            this.textBox_UserName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox_ProcessId = new System.Windows.Forms.GroupBox();
            this.button_ProcessId = new System.Windows.Forms.Button();
            this.textBox_ProcessId = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox_ProcessSha256 = new System.Windows.Forms.GroupBox();
            this.button_GetProcessSha256 = new System.Windows.Forms.Button();
            this.textBox_ProcessSha256Hash = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox_SignedProcess = new System.Windows.Forms.GroupBox();
            this.button_GetCertificateName = new System.Windows.Forms.Button();
            this.textBox_ProcessCertificateName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox_ProcessName.SuspendLayout();
            this.groupBox_AccessRights.SuspendLayout();
            this.groupBox_UserName.SuspendLayout();
            this.groupBox_ProcessId.SuspendLayout();
            this.groupBox_ProcessSha256.SuspendLayout();
            this.groupBox_SignedProcess.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_Add
            // 
            this.button_Add.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_Add.Location = new System.Drawing.Point(436, 201);
            this.button_Add.Name = "button_Add";
            this.button_Add.Size = new System.Drawing.Size(75, 23);
            this.button_Add.TabIndex = 25;
            this.button_Add.Text = "Apply";
            this.button_Add.UseVisualStyleBackColor = true;
            this.button_Add.Click += new System.EventHandler(this.button_Add_Click);
            // 
            // groupBox_ProcessName
            // 
            this.groupBox_ProcessName.Controls.Add(this.button_InfoProcessName);
            this.groupBox_ProcessName.Controls.Add(this.textBox_ProcessName);
            this.groupBox_ProcessName.Controls.Add(this.label_AccessFlags);
            this.groupBox_ProcessName.Location = new System.Drawing.Point(25, 19);
            this.groupBox_ProcessName.Name = "groupBox_ProcessName";
            this.groupBox_ProcessName.Size = new System.Drawing.Size(535, 48);
            this.groupBox_ProcessName.TabIndex = 26;
            this.groupBox_ProcessName.TabStop = false;
            this.groupBox_ProcessName.Visible = false;
            // 
            // button_InfoProcessName
            // 
            this.button_InfoProcessName.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
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
            // groupBox_AccessRights
            // 
            this.groupBox_AccessRights.Controls.Add(this.groupBox1);
            this.groupBox_AccessRights.Controls.Add(this.button_InfoEncryptOnRead);
            this.groupBox_AccessRights.Controls.Add(this.button_InfoDecryptFile);
            this.groupBox_AccessRights.Controls.Add(this.button_InfoEncryptNewFile);
            this.groupBox_AccessRights.Controls.Add(this.button_InfoCopyout);
            this.groupBox_AccessRights.Controls.Add(this.button_Add);
            this.groupBox_AccessRights.Controls.Add(this.checkBox_AllowReadEncryptedFiles);
            this.groupBox_AccessRights.Controls.Add(this.checkBox_AllowCopyout);
            this.groupBox_AccessRights.Controls.Add(this.checkBox_AllowEncryptNewFile);
            this.groupBox_AccessRights.Controls.Add(this.checkBox_SetSecurity);
            this.groupBox_AccessRights.Controls.Add(this.checkBox_QueryInfo);
            this.groupBox_AccessRights.Controls.Add(this.checkBox_Read);
            this.groupBox_AccessRights.Controls.Add(this.checkBox_EnableEncryptionOnRead);
            this.groupBox_AccessRights.Controls.Add(this.checkBox_SetInfo);
            this.groupBox_AccessRights.Controls.Add(this.checkBox_Write);
            this.groupBox_AccessRights.Controls.Add(this.checkBox_AllowDelete);
            this.groupBox_AccessRights.Controls.Add(this.checkBox_AllowRename);
            this.groupBox_AccessRights.Controls.Add(this.checkBox_Creation);
            this.groupBox_AccessRights.Controls.Add(this.label2);
            this.groupBox_AccessRights.Controls.Add(this.textBox_FileAccessFlags);
            this.groupBox_AccessRights.Controls.Add(this.button_FileAccessFlags);
            this.groupBox_AccessRights.Location = new System.Drawing.Point(25, 231);
            this.groupBox_AccessRights.Name = "groupBox_AccessRights";
            this.groupBox_AccessRights.Size = new System.Drawing.Size(535, 234);
            this.groupBox_AccessRights.TabIndex = 76;
            this.groupBox_AccessRights.TabStop = false;
            this.groupBox_AccessRights.Text = "File Acess Rights";
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(9, 170);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(516, 10);
            this.groupBox1.TabIndex = 124;
            this.groupBox1.TabStop = false;
            // 
            // button_InfoEncryptOnRead
            // 
            this.button_InfoEncryptOnRead.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
            this.button_InfoEncryptOnRead.Location = new System.Drawing.Point(501, 147);
            this.button_InfoEncryptOnRead.Name = "button_InfoEncryptOnRead";
            this.button_InfoEncryptOnRead.Size = new System.Drawing.Size(28, 20);
            this.button_InfoEncryptOnRead.TabIndex = 120;
            this.button_InfoEncryptOnRead.UseVisualStyleBackColor = true;
            this.button_InfoEncryptOnRead.Click += new System.EventHandler(this.button_InfoEncryptOnRead_Click);
            // 
            // button_InfoDecryptFile
            // 
            this.button_InfoDecryptFile.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
            this.button_InfoDecryptFile.Location = new System.Drawing.Point(501, 124);
            this.button_InfoDecryptFile.Name = "button_InfoDecryptFile";
            this.button_InfoDecryptFile.Size = new System.Drawing.Size(28, 20);
            this.button_InfoDecryptFile.TabIndex = 121;
            this.button_InfoDecryptFile.UseVisualStyleBackColor = true;
            this.button_InfoDecryptFile.Click += new System.EventHandler(this.button_InfoDecryptFile_Click);
            // 
            // button_InfoEncryptNewFile
            // 
            this.button_InfoEncryptNewFile.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
            this.button_InfoEncryptNewFile.Location = new System.Drawing.Point(501, 98);
            this.button_InfoEncryptNewFile.Name = "button_InfoEncryptNewFile";
            this.button_InfoEncryptNewFile.Size = new System.Drawing.Size(28, 20);
            this.button_InfoEncryptNewFile.TabIndex = 122;
            this.button_InfoEncryptNewFile.UseVisualStyleBackColor = true;
            this.button_InfoEncryptNewFile.Click += new System.EventHandler(this.button_InfoEncryptNewFile_Click);
            // 
            // button_InfoCopyout
            // 
            this.button_InfoCopyout.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
            this.button_InfoCopyout.Location = new System.Drawing.Point(501, 75);
            this.button_InfoCopyout.Name = "button_InfoCopyout";
            this.button_InfoCopyout.Size = new System.Drawing.Size(28, 20);
            this.button_InfoCopyout.TabIndex = 123;
            this.button_InfoCopyout.UseVisualStyleBackColor = true;
            this.button_InfoCopyout.Click += new System.EventHandler(this.button_InfoCopyout_Click);
            // 
            // checkBox_AllowReadEncryptedFiles
            // 
            this.checkBox_AllowReadEncryptedFiles.AutoSize = true;
            this.checkBox_AllowReadEncryptedFiles.Checked = true;
            this.checkBox_AllowReadEncryptedFiles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_AllowReadEncryptedFiles.Location = new System.Drawing.Point(320, 124);
            this.checkBox_AllowReadEncryptedFiles.Name = "checkBox_AllowReadEncryptedFiles";
            this.checkBox_AllowReadEncryptedFiles.Size = new System.Drawing.Size(177, 17);
            this.checkBox_AllowReadEncryptedFiles.TabIndex = 49;
            this.checkBox_AllowReadEncryptedFiles.Text = "Enable encrypted file decryption";
            this.checkBox_AllowReadEncryptedFiles.UseVisualStyleBackColor = true;
            this.checkBox_AllowReadEncryptedFiles.CheckedChanged += new System.EventHandler(this.checkBox_AllowReadEncryptedFiles_CheckedChanged);
            // 
            // checkBox_AllowCopyout
            // 
            this.checkBox_AllowCopyout.AutoSize = true;
            this.checkBox_AllowCopyout.Checked = true;
            this.checkBox_AllowCopyout.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_AllowCopyout.Location = new System.Drawing.Point(320, 78);
            this.checkBox_AllowCopyout.Name = "checkBox_AllowCopyout";
            this.checkBox_AllowCopyout.Size = new System.Drawing.Size(154, 17);
            this.checkBox_AllowCopyout.TabIndex = 48;
            this.checkBox_AllowCopyout.Text = "Allow files being copied out";
            this.checkBox_AllowCopyout.UseVisualStyleBackColor = true;
            this.checkBox_AllowCopyout.CheckedChanged += new System.EventHandler(this.checkBox_AllowCopyout_CheckedChanged);
            // 
            // checkBox_AllowEncryptNewFile
            // 
            this.checkBox_AllowEncryptNewFile.AutoSize = true;
            this.checkBox_AllowEncryptNewFile.Checked = true;
            this.checkBox_AllowEncryptNewFile.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_AllowEncryptNewFile.Location = new System.Drawing.Point(320, 101);
            this.checkBox_AllowEncryptNewFile.Name = "checkBox_AllowEncryptNewFile";
            this.checkBox_AllowEncryptNewFile.Size = new System.Drawing.Size(150, 17);
            this.checkBox_AllowEncryptNewFile.TabIndex = 47;
            this.checkBox_AllowEncryptNewFile.Text = "Enable new file encryption";
            this.checkBox_AllowEncryptNewFile.UseVisualStyleBackColor = true;
            this.checkBox_AllowEncryptNewFile.CheckedChanged += new System.EventHandler(this.checkBox_AllowEncryptNewFile_CheckedChanged);
            // 
            // checkBox_SetSecurity
            // 
            this.checkBox_SetSecurity.AutoSize = true;
            this.checkBox_SetSecurity.Checked = true;
            this.checkBox_SetSecurity.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_SetSecurity.Location = new System.Drawing.Point(149, 146);
            this.checkBox_SetSecurity.Name = "checkBox_SetSecurity";
            this.checkBox_SetSecurity.Size = new System.Drawing.Size(137, 17);
            this.checkBox_SetSecurity.TabIndex = 46;
            this.checkBox_SetSecurity.Text = "Allow security changing";
            this.checkBox_SetSecurity.UseVisualStyleBackColor = true;
            this.checkBox_SetSecurity.CheckedChanged += new System.EventHandler(this.checkBox_SetSecurity_CheckedChanged);
            // 
            // checkBox_QueryInfo
            // 
            this.checkBox_QueryInfo.AutoSize = true;
            this.checkBox_QueryInfo.Checked = true;
            this.checkBox_QueryInfo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_QueryInfo.Location = new System.Drawing.Point(149, 100);
            this.checkBox_QueryInfo.Name = "checkBox_QueryInfo";
            this.checkBox_QueryInfo.Size = new System.Drawing.Size(130, 17);
            this.checkBox_QueryInfo.TabIndex = 42;
            this.checkBox_QueryInfo.Text = "Allow file info querying";
            this.checkBox_QueryInfo.UseVisualStyleBackColor = true;
            this.checkBox_QueryInfo.CheckedChanged += new System.EventHandler(this.checkBox_QueryInfo_CheckedChanged);
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
            // checkBox_EnableEncryptionOnRead
            // 
            this.checkBox_EnableEncryptionOnRead.AutoSize = true;
            this.checkBox_EnableEncryptionOnRead.Checked = true;
            this.checkBox_EnableEncryptionOnRead.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_EnableEncryptionOnRead.Location = new System.Drawing.Point(320, 147);
            this.checkBox_EnableEncryptionOnRead.Name = "checkBox_EnableEncryptionOnRead";
            this.checkBox_EnableEncryptionOnRead.Size = new System.Drawing.Size(159, 17);
            this.checkBox_EnableEncryptionOnRead.TabIndex = 43;
            this.checkBox_EnableEncryptionOnRead.Text = "Enable encryption on the go";
            this.checkBox_EnableEncryptionOnRead.UseVisualStyleBackColor = true;
            this.checkBox_EnableEncryptionOnRead.CheckedChanged += new System.EventHandler(this.checkBox_EncryptionOnRead_CheckedChanged);
            // 
            // checkBox_SetInfo
            // 
            this.checkBox_SetInfo.AutoSize = true;
            this.checkBox_SetInfo.Checked = true;
            this.checkBox_SetInfo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_SetInfo.Location = new System.Drawing.Point(149, 123);
            this.checkBox_SetInfo.Name = "checkBox_SetInfo";
            this.checkBox_SetInfo.Size = new System.Drawing.Size(134, 17);
            this.checkBox_SetInfo.TabIndex = 45;
            this.checkBox_SetInfo.Text = "Allow file info changing";
            this.checkBox_SetInfo.UseVisualStyleBackColor = true;
            this.checkBox_SetInfo.CheckedChanged += new System.EventHandler(this.checkBox_SetInfo_CheckedChanged);
            // 
            // checkBox_Write
            // 
            this.checkBox_Write.AutoSize = true;
            this.checkBox_Write.Checked = true;
            this.checkBox_Write.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_Write.Location = new System.Drawing.Point(9, 101);
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
            this.checkBox_AllowDelete.Location = new System.Drawing.Point(9, 123);
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
            this.checkBox_AllowRename.Location = new System.Drawing.Point(149, 77);
            this.checkBox_AllowRename.Name = "checkBox_AllowRename";
            this.checkBox_AllowRename.Size = new System.Drawing.Size(113, 17);
            this.checkBox_AllowRename.TabIndex = 39;
            this.checkBox_AllowRename.Text = "Allow file renaming";
            this.checkBox_AllowRename.UseVisualStyleBackColor = true;
            this.checkBox_AllowRename.CheckedChanged += new System.EventHandler(this.checkBox_AllowRename_CheckedChanged);
            // 
            // checkBox_Creation
            // 
            this.checkBox_Creation.AutoSize = true;
            this.checkBox_Creation.Checked = true;
            this.checkBox_Creation.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_Creation.Location = new System.Drawing.Point(9, 146);
            this.checkBox_Creation.Name = "checkBox_Creation";
            this.checkBox_Creation.Size = new System.Drawing.Size(131, 17);
            this.checkBox_Creation.TabIndex = 41;
            this.checkBox_Creation.Text = "Allow new file creation";
            this.checkBox_Creation.UseVisualStyleBackColor = true;
            this.checkBox_Creation.CheckedChanged += new System.EventHandler(this.checkBox_Creation_CheckedChanged);
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
            // groupBox_UserName
            // 
            this.groupBox_UserName.Controls.Add(this.button_InfoUserName);
            this.groupBox_UserName.Controls.Add(this.textBox_UserName);
            this.groupBox_UserName.Controls.Add(this.label1);
            this.groupBox_UserName.Location = new System.Drawing.Point(25, 171);
            this.groupBox_UserName.Name = "groupBox_UserName";
            this.groupBox_UserName.Size = new System.Drawing.Size(535, 46);
            this.groupBox_UserName.TabIndex = 30;
            this.groupBox_UserName.TabStop = false;
            this.groupBox_UserName.Visible = false;
            // 
            // button_InfoUserName
            // 
            this.button_InfoUserName.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
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
            // groupBox_ProcessId
            // 
            this.groupBox_ProcessId.Controls.Add(this.button_ProcessId);
            this.groupBox_ProcessId.Controls.Add(this.textBox_ProcessId);
            this.groupBox_ProcessId.Controls.Add(this.label3);
            this.groupBox_ProcessId.Location = new System.Drawing.Point(25, 136);
            this.groupBox_ProcessId.Name = "groupBox_ProcessId";
            this.groupBox_ProcessId.Size = new System.Drawing.Size(535, 48);
            this.groupBox_ProcessId.TabIndex = 29;
            this.groupBox_ProcessId.TabStop = false;
            this.groupBox_ProcessId.Visible = false;
            // 
            // button_ProcessId
            // 
            this.button_ProcessId.Location = new System.Drawing.Point(461, 15);
            this.button_ProcessId.Name = "button_ProcessId";
            this.button_ProcessId.Size = new System.Drawing.Size(50, 20);
            this.button_ProcessId.TabIndex = 38;
            this.button_ProcessId.Text = "...";
            this.button_ProcessId.UseVisualStyleBackColor = true;
            this.button_ProcessId.Click += new System.EventHandler(this.button_ProcessId_Click);
            // 
            // textBox_ProcessId
            // 
            this.textBox_ProcessId.Location = new System.Drawing.Point(149, 16);
            this.textBox_ProcessId.Name = "textBox_ProcessId";
            this.textBox_ProcessId.Size = new System.Drawing.Size(298, 20);
            this.textBox_ProcessId.TabIndex = 27;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 28;
            this.label3.Text = "Process Id";
            // 
            // groupBox_ProcessSha256
            // 
            this.groupBox_ProcessSha256.Controls.Add(this.button_GetProcessSha256);
            this.groupBox_ProcessSha256.Controls.Add(this.textBox_ProcessSha256Hash);
            this.groupBox_ProcessSha256.Controls.Add(this.label4);
            this.groupBox_ProcessSha256.Location = new System.Drawing.Point(25, 98);
            this.groupBox_ProcessSha256.Name = "groupBox_ProcessSha256";
            this.groupBox_ProcessSha256.Size = new System.Drawing.Size(535, 48);
            this.groupBox_ProcessSha256.TabIndex = 39;
            this.groupBox_ProcessSha256.TabStop = false;
            this.groupBox_ProcessSha256.Visible = false;
            // 
            // button_GetProcessSha256
            // 
            this.button_GetProcessSha256.Location = new System.Drawing.Point(461, 15);
            this.button_GetProcessSha256.Name = "button_GetProcessSha256";
            this.button_GetProcessSha256.Size = new System.Drawing.Size(50, 20);
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
            // groupBox_SignedProcess
            // 
            this.groupBox_SignedProcess.Controls.Add(this.button_GetCertificateName);
            this.groupBox_SignedProcess.Controls.Add(this.textBox_ProcessCertificateName);
            this.groupBox_SignedProcess.Controls.Add(this.label5);
            this.groupBox_SignedProcess.Location = new System.Drawing.Point(25, 61);
            this.groupBox_SignedProcess.Name = "groupBox_SignedProcess";
            this.groupBox_SignedProcess.Size = new System.Drawing.Size(535, 48);
            this.groupBox_SignedProcess.TabIndex = 125;
            this.groupBox_SignedProcess.TabStop = false;
            this.groupBox_SignedProcess.Visible = false;
            // 
            // button_GetCertificateName
            // 
            this.button_GetCertificateName.Location = new System.Drawing.Point(461, 12);
            this.button_GetCertificateName.Name = "button_GetCertificateName";
            this.button_GetCertificateName.Size = new System.Drawing.Size(50, 20);
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
            // Form_AccessRights
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 467);
            this.Controls.Add(this.groupBox_SignedProcess);
            this.Controls.Add(this.groupBox_ProcessSha256);
            this.Controls.Add(this.groupBox_ProcessId);
            this.Controls.Add(this.groupBox_UserName);
            this.Controls.Add(this.groupBox_AccessRights);
            this.Controls.Add(this.groupBox_ProcessName);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form_AccessRights";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Access Rights";
            this.groupBox_ProcessName.ResumeLayout(false);
            this.groupBox_ProcessName.PerformLayout();
            this.groupBox_AccessRights.ResumeLayout(false);
            this.groupBox_AccessRights.PerformLayout();
            this.groupBox_UserName.ResumeLayout(false);
            this.groupBox_UserName.PerformLayout();
            this.groupBox_ProcessId.ResumeLayout(false);
            this.groupBox_ProcessId.PerformLayout();
            this.groupBox_ProcessSha256.ResumeLayout(false);
            this.groupBox_ProcessSha256.PerformLayout();
            this.groupBox_SignedProcess.ResumeLayout(false);
            this.groupBox_SignedProcess.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_Add;
        private System.Windows.Forms.GroupBox groupBox_ProcessName;
        private System.Windows.Forms.TextBox textBox_ProcessName;
        private System.Windows.Forms.Label label_AccessFlags;
        private System.Windows.Forms.GroupBox groupBox_AccessRights;
        private System.Windows.Forms.GroupBox groupBox_UserName;
        private System.Windows.Forms.TextBox textBox_UserName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_FileAccessFlags;
        private System.Windows.Forms.Button button_FileAccessFlags;
        private System.Windows.Forms.GroupBox groupBox_ProcessId;
        private System.Windows.Forms.Button button_ProcessId;
        private System.Windows.Forms.TextBox textBox_ProcessId;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBox_AllowReadEncryptedFiles;
        private System.Windows.Forms.CheckBox checkBox_AllowCopyout;
        private System.Windows.Forms.CheckBox checkBox_AllowEncryptNewFile;
        private System.Windows.Forms.CheckBox checkBox_SetSecurity;
        private System.Windows.Forms.CheckBox checkBox_QueryInfo;
        private System.Windows.Forms.CheckBox checkBox_Read;
        private System.Windows.Forms.CheckBox checkBox_EnableEncryptionOnRead;
        private System.Windows.Forms.CheckBox checkBox_SetInfo;
        private System.Windows.Forms.CheckBox checkBox_Write;
        private System.Windows.Forms.CheckBox checkBox_AllowDelete;
        private System.Windows.Forms.CheckBox checkBox_AllowRename;
        private System.Windows.Forms.CheckBox checkBox_Creation;
        private System.Windows.Forms.Button button_InfoEncryptOnRead;
        private System.Windows.Forms.Button button_InfoDecryptFile;
        private System.Windows.Forms.Button button_InfoEncryptNewFile;
        private System.Windows.Forms.Button button_InfoCopyout;
        private System.Windows.Forms.Button button_InfoProcessName;
        private System.Windows.Forms.Button button_InfoUserName;
        private System.Windows.Forms.GroupBox groupBox_ProcessSha256;
        private System.Windows.Forms.Button button_GetProcessSha256;
        private System.Windows.Forms.TextBox textBox_ProcessSha256Hash;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox_SignedProcess;
        private System.Windows.Forms.Button button_GetCertificateName;
        private System.Windows.Forms.TextBox textBox_ProcessCertificateName;
        private System.Windows.Forms.Label label5;
    }
}