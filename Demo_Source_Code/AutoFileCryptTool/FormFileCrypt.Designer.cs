namespace AutoFileCryptTool
{
    partial class Form_FileCrypt
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

        private void AddDefaultItemsToAutoFolderList()
        {
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("");
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
            "Add auto encryption folder here"}, -1, System.Drawing.Color.RoyalBlue, System.Drawing.Color.Empty, new System.Drawing.Font("Arial Rounded MT Bold", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))));
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem(new string[] {
            "The new created files in the folder will be encrypted automatically"}, -1, System.Drawing.Color.Orange, System.Drawing.Color.Empty, new System.Drawing.Font("Arial Rounded MT Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))));
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem(new string[] {
            "The files will be encrypted with 256 bit key and unique IV"}, -1, System.Drawing.Color.Orange, System.Drawing.Color.Empty, new System.Drawing.Font("Arial Rounded MT Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))));
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem(new string[] {
            "If the process wants to get the raw data of the encrypted file"}, -1, System.Drawing.Color.Orange, System.Drawing.Color.Empty, new System.Drawing.Font("Arial Rounded MT Bold", 9F));
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem(new string[] {
            "Then add the process to the black list"}, -1, System.Drawing.Color.Orange, System.Drawing.Color.Empty, new System.Drawing.Font("Arial Rounded MT Bold", 9F));

            this.listView_AutoEncryptFolders.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4,
            listViewItem5,
            listViewItem6});
        }

        private void AddDefaultItemsToEncryptOnReadList()
        {
            System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem("");
            System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem(new string[] {
            "Add output protected folders here"}, -1, System.Drawing.Color.DodgerBlue, System.Drawing.Color.Empty, new System.Drawing.Font("Arial Rounded MT Bold", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))));
            System.Windows.Forms.ListViewItem listViewItem9 = new System.Windows.Forms.ListViewItem(new string[] {
            "The file is not encrypted in the local disk"}, -1, System.Drawing.Color.Orange, System.Drawing.Color.Empty, new System.Drawing.Font("Arial Rounded MT Bold", 9F));
            System.Windows.Forms.ListViewItem listViewItem10 = new System.Windows.Forms.ListViewItem(new string[] {
            "The file will be encrypted automatically when it was read"}, -1, System.Drawing.Color.Orange, System.Drawing.Color.Empty, new System.Drawing.Font("Arial Rounded MT Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))));
            System.Windows.Forms.ListViewItem listViewItem11 = new System.Windows.Forms.ListViewItem(new string[] {
            "by the processes from blacklist"}, -1, System.Drawing.Color.Orange, System.Drawing.Color.Empty, new System.Drawing.Font("Arial Rounded MT Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))));
            System.Windows.Forms.ListViewItem listViewItem12 = new System.Windows.Forms.ListViewItem(new string[] {
            "To decrypt file, copy the encrypted file to the drop folder"}, -1, System.Drawing.Color.Orange, System.Drawing.Color.Empty, new System.Drawing.Font("Arial Rounded MT Bold", 9F));
            this.listView_EncryptOnReadFolders.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem7,
            listViewItem8,
            listViewItem9,
            listViewItem10,
            listViewItem11,
            listViewItem12});
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_FileCrypt));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tabPage_EncryptFile = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.textBox_PassPhraseEncrypt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_EncryptTargetName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_EncryptSourceName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.button_BrowseEncryptFile = new System.Windows.Forms.Button();
            this.button_StartToEncrypt = new System.Windows.Forms.Button();
            this.tabPage_Folder = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_Help = new System.Windows.Forms.Button();
            this.button_Stop = new System.Windows.Forms.Button();
            this.button_Start = new System.Windows.Forms.Button();
            this.button_RemoveFolder = new System.Windows.Forms.Button();
            this.button_AddFolder = new System.Windows.Forms.Button();
            this.listView_AutoEncryptFolders = new System.Windows.Forms.ListView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage_EncryptOnRead = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button_StopService = new System.Windows.Forms.Button();
            this.button_StartService = new System.Windows.Forms.Button();
            this.button_RemoveProtectFolder = new System.Windows.Forms.Button();
            this.button_AddProtectFolder = new System.Windows.Forms.Button();
            this.listView_EncryptOnReadFolders = new System.Windows.Forms.ListView();
            this.tabPage_DecryptFile = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label17 = new System.Windows.Forms.Label();
            this.textBox_PassPhraseDecrypt = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.textBox_DecryptTargetName = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.textBox_DecryptSourceName = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.button_BrowseFileToDecrypt = new System.Windows.Forms.Button();
            this.button_StartToDecrypt = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button_SetupDropFolder = new System.Windows.Forms.Button();
            this.tabPage_EncryptFile.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage_Folder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage_EncryptOnRead.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabPage_DecryptFile.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "folder_ok.png");
            // 
            // tabPage_EncryptFile
            // 
            this.tabPage_EncryptFile.Controls.Add(this.groupBox2);
            this.tabPage_EncryptFile.Controls.Add(this.button_StartToEncrypt);
            this.tabPage_EncryptFile.Location = new System.Drawing.Point(4, 22);
            this.tabPage_EncryptFile.Name = "tabPage_EncryptFile";
            this.tabPage_EncryptFile.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_EncryptFile.Size = new System.Drawing.Size(548, 255);
            this.tabPage_EncryptFile.TabIndex = 1;
            this.tabPage_EncryptFile.Text = "Encrypt File Manually";
            this.tabPage_EncryptFile.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.textBox_PassPhraseEncrypt);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.textBox_EncryptTargetName);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.textBox_EncryptSourceName);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.button_BrowseEncryptFile);
            this.groupBox2.Location = new System.Drawing.Point(12, 21);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(506, 184);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Encrypt file manually ";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(16, 143);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(66, 13);
            this.label12.TabIndex = 16;
            this.label12.Text = "Pass Phrase";
            // 
            // textBox_PassPhraseEncrypt
            // 
            this.textBox_PassPhraseEncrypt.Location = new System.Drawing.Point(138, 143);
            this.textBox_PassPhraseEncrypt.Name = "textBox_PassPhraseEncrypt";
            this.textBox_PassPhraseEncrypt.Size = new System.Drawing.Size(260, 20);
            this.textBox_PassPhraseEncrypt.TabIndex = 15;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(137, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(257, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "target file name can be the same as the source name";
            // 
            // textBox_EncryptTargetName
            // 
            this.textBox_EncryptTargetName.Location = new System.Drawing.Point(140, 95);
            this.textBox_EncryptTargetName.Name = "textBox_EncryptTargetName";
            this.textBox_EncryptTargetName.Size = new System.Drawing.Size(258, 20);
            this.textBox_EncryptTargetName.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 95);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Target File name";
            // 
            // textBox_EncryptSourceName
            // 
            this.textBox_EncryptSourceName.Location = new System.Drawing.Point(140, 38);
            this.textBox_EncryptSourceName.Name = "textBox_EncryptSourceName";
            this.textBox_EncryptSourceName.Size = new System.Drawing.Size(258, 20);
            this.textBox_EncryptSourceName.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 46);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Source File name";
            // 
            // button_BrowseEncryptFile
            // 
            this.button_BrowseEncryptFile.Location = new System.Drawing.Point(404, 36);
            this.button_BrowseEncryptFile.Name = "button_BrowseEncryptFile";
            this.button_BrowseEncryptFile.Size = new System.Drawing.Size(44, 23);
            this.button_BrowseEncryptFile.TabIndex = 6;
            this.button_BrowseEncryptFile.Text = "...";
            this.button_BrowseEncryptFile.UseVisualStyleBackColor = true;
            this.button_BrowseEncryptFile.Click += new System.EventHandler(this.button_BrowseEncryptFile_Click);
            // 
            // button_StartToEncrypt
            // 
            this.button_StartToEncrypt.Location = new System.Drawing.Point(443, 211);
            this.button_StartToEncrypt.Name = "button_StartToEncrypt";
            this.button_StartToEncrypt.Size = new System.Drawing.Size(75, 23);
            this.button_StartToEncrypt.TabIndex = 4;
            this.button_StartToEncrypt.Text = "Start";
            this.button_StartToEncrypt.UseVisualStyleBackColor = true;
            this.button_StartToEncrypt.Click += new System.EventHandler(this.button_StartToEncrypt_Click);
            // 
            // tabPage_Folder
            // 
            this.tabPage_Folder.Controls.Add(this.splitContainer1);
            this.tabPage_Folder.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Folder.Name = "tabPage_Folder";
            this.tabPage_Folder.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Folder.Size = new System.Drawing.Size(548, 255);
            this.tabPage_Folder.TabIndex = 0;
            this.tabPage_Folder.Text = "Auto Folder Encrption";
            this.tabPage_Folder.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listView_AutoEncryptFolders);
            this.splitContainer1.Size = new System.Drawing.Size(542, 249);
            this.splitContainer1.SplitterDistance = 114;
            this.splitContainer1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_Help);
            this.groupBox1.Controls.Add(this.button_Stop);
            this.groupBox1.Controls.Add(this.button_Start);
            this.groupBox1.Controls.Add(this.button_RemoveFolder);
            this.groupBox1.Controls.Add(this.button_AddFolder);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(114, 249);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // button_Help
            // 
            this.button_Help.Location = new System.Drawing.Point(6, 206);
            this.button_Help.Name = "button_Help";
            this.button_Help.Size = new System.Drawing.Size(94, 23);
            this.button_Help.TabIndex = 4;
            this.button_Help.Text = "Help Info";
            this.button_Help.UseVisualStyleBackColor = true;
            this.button_Help.Click += new System.EventHandler(this.button_Help_Click);
            // 
            // button_Stop
            // 
            this.button_Stop.Location = new System.Drawing.Point(6, 153);
            this.button_Stop.Name = "button_Stop";
            this.button_Stop.Size = new System.Drawing.Size(94, 23);
            this.button_Stop.TabIndex = 3;
            this.button_Stop.Text = "Stop Service";
            this.button_Stop.UseVisualStyleBackColor = true;
            this.button_Stop.Click += new System.EventHandler(this.button_Stop_Click);
            // 
            // button_Start
            // 
            this.button_Start.Location = new System.Drawing.Point(6, 124);
            this.button_Start.Name = "button_Start";
            this.button_Start.Size = new System.Drawing.Size(94, 23);
            this.button_Start.TabIndex = 2;
            this.button_Start.Text = "Start Service";
            this.button_Start.UseVisualStyleBackColor = true;
            this.button_Start.Click += new System.EventHandler(this.button_Start_Click);
            // 
            // button_RemoveFolder
            // 
            this.button_RemoveFolder.Location = new System.Drawing.Point(6, 72);
            this.button_RemoveFolder.Name = "button_RemoveFolder";
            this.button_RemoveFolder.Size = new System.Drawing.Size(94, 23);
            this.button_RemoveFolder.TabIndex = 1;
            this.button_RemoveFolder.Text = "Remove folder";
            this.button_RemoveFolder.UseVisualStyleBackColor = true;
            this.button_RemoveFolder.Click += new System.EventHandler(this.button_RemoveFolder_Click);
            // 
            // button_AddFolder
            // 
            this.button_AddFolder.Location = new System.Drawing.Point(6, 34);
            this.button_AddFolder.Name = "button_AddFolder";
            this.button_AddFolder.Size = new System.Drawing.Size(94, 23);
            this.button_AddFolder.TabIndex = 0;
            this.button_AddFolder.Text = "Add folder";
            this.button_AddFolder.UseVisualStyleBackColor = true;
            this.button_AddFolder.Click += new System.EventHandler(this.button_AddFolder_Click);
            // 
            // listView_AutoEncryptFolders
            // 
            this.listView_AutoEncryptFolders.AllowDrop = true;
            this.listView_AutoEncryptFolders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_AutoEncryptFolders.FullRowSelect = true;
            this.listView_AutoEncryptFolders.Location = new System.Drawing.Point(0, 0);
            this.listView_AutoEncryptFolders.Name = "listView_AutoEncryptFolders";
            this.listView_AutoEncryptFolders.Size = new System.Drawing.Size(424, 249);
            this.listView_AutoEncryptFolders.SmallImageList = this.imageList1;
            this.listView_AutoEncryptFolders.TabIndex = 0;
            this.listView_AutoEncryptFolders.UseCompatibleStateImageBehavior = false;
            this.listView_AutoEncryptFolders.View = System.Windows.Forms.View.List;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage_Folder);
            this.tabControl1.Controls.Add(this.tabPage_EncryptOnRead);
            this.tabControl1.Controls.Add(this.tabPage_EncryptFile);
            this.tabControl1.Controls.Add(this.tabPage_DecryptFile);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(556, 281);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage_EncryptOnRead
            // 
            this.tabPage_EncryptOnRead.Controls.Add(this.splitContainer2);
            this.tabPage_EncryptOnRead.Location = new System.Drawing.Point(4, 22);
            this.tabPage_EncryptOnRead.Name = "tabPage_EncryptOnRead";
            this.tabPage_EncryptOnRead.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_EncryptOnRead.Size = new System.Drawing.Size(548, 255);
            this.tabPage_EncryptOnRead.TabIndex = 4;
            this.tabPage_EncryptOnRead.Text = "Encrypt File On The Go";
            this.tabPage_EncryptOnRead.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.groupBox4);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.listView_EncryptOnReadFolders);
            this.splitContainer2.Size = new System.Drawing.Size(542, 249);
            this.splitContainer2.SplitterDistance = 114;
            this.splitContainer2.TabIndex = 1;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.button_SetupDropFolder);
            this.groupBox4.Controls.Add(this.button_StopService);
            this.groupBox4.Controls.Add(this.button_StartService);
            this.groupBox4.Controls.Add(this.button_RemoveProtectFolder);
            this.groupBox4.Controls.Add(this.button_AddProtectFolder);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(114, 249);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            // 
            // button_StopService
            // 
            this.button_StopService.Location = new System.Drawing.Point(5, 192);
            this.button_StopService.Name = "button_StopService";
            this.button_StopService.Size = new System.Drawing.Size(103, 23);
            this.button_StopService.TabIndex = 3;
            this.button_StopService.Text = "Stop Service";
            this.button_StopService.UseVisualStyleBackColor = true;
            this.button_StopService.Click += new System.EventHandler(this.button_StopService_Click);
            // 
            // button_StartService
            // 
            this.button_StartService.Location = new System.Drawing.Point(6, 163);
            this.button_StartService.Name = "button_StartService";
            this.button_StartService.Size = new System.Drawing.Size(102, 23);
            this.button_StartService.TabIndex = 2;
            this.button_StartService.Text = "Start Service";
            this.button_StartService.UseVisualStyleBackColor = true;
            this.button_StartService.Click += new System.EventHandler(this.button_StartService_Click);
            // 
            // button_RemoveProtectFolder
            // 
            this.button_RemoveProtectFolder.Location = new System.Drawing.Point(5, 48);
            this.button_RemoveProtectFolder.Name = "button_RemoveProtectFolder";
            this.button_RemoveProtectFolder.Size = new System.Drawing.Size(103, 23);
            this.button_RemoveProtectFolder.TabIndex = 1;
            this.button_RemoveProtectFolder.Text = "Remove folder";
            this.button_RemoveProtectFolder.UseVisualStyleBackColor = true;
            this.button_RemoveProtectFolder.Click += new System.EventHandler(this.button_RemoveEncryptOnReadFolder_Click);
            // 
            // button_AddProtectFolder
            // 
            this.button_AddProtectFolder.Location = new System.Drawing.Point(6, 19);
            this.button_AddProtectFolder.Name = "button_AddProtectFolder";
            this.button_AddProtectFolder.Size = new System.Drawing.Size(102, 23);
            this.button_AddProtectFolder.TabIndex = 0;
            this.button_AddProtectFolder.Text = "Add folder";
            this.button_AddProtectFolder.UseVisualStyleBackColor = true;
            this.button_AddProtectFolder.Click += new System.EventHandler(this.button_AddEncryptOnReadFolder_Click);
            // 
            // listView_EncryptOnReadFolders
            // 
            this.listView_EncryptOnReadFolders.AllowDrop = true;
            this.listView_EncryptOnReadFolders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_EncryptOnReadFolders.FullRowSelect = true;
            this.listView_EncryptOnReadFolders.Location = new System.Drawing.Point(0, 0);
            this.listView_EncryptOnReadFolders.Name = "listView_EncryptOnReadFolders";
            this.listView_EncryptOnReadFolders.Size = new System.Drawing.Size(424, 249);
            this.listView_EncryptOnReadFolders.SmallImageList = this.imageList1;
            this.listView_EncryptOnReadFolders.TabIndex = 0;
            this.listView_EncryptOnReadFolders.UseCompatibleStateImageBehavior = false;
            this.listView_EncryptOnReadFolders.View = System.Windows.Forms.View.List;
            // 
            // tabPage_DecryptFile
            // 
            this.tabPage_DecryptFile.Controls.Add(this.groupBox3);
            this.tabPage_DecryptFile.Controls.Add(this.button_StartToDecrypt);
            this.tabPage_DecryptFile.Location = new System.Drawing.Point(4, 22);
            this.tabPage_DecryptFile.Name = "tabPage_DecryptFile";
            this.tabPage_DecryptFile.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_DecryptFile.Size = new System.Drawing.Size(548, 255);
            this.tabPage_DecryptFile.TabIndex = 2;
            this.tabPage_DecryptFile.Text = "Decrypt File Manually";
            this.tabPage_DecryptFile.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label17);
            this.groupBox3.Controls.Add(this.textBox_PassPhraseDecrypt);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.textBox_DecryptTargetName);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.textBox_DecryptSourceName);
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Controls.Add(this.button_BrowseFileToDecrypt);
            this.groupBox3.Location = new System.Drawing.Point(17, 21);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(498, 188);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Decrypt file manually";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(16, 134);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(66, 13);
            this.label17.TabIndex = 19;
            this.label17.Text = "Pass Phrase";
            // 
            // textBox_PassPhraseDecrypt
            // 
            this.textBox_PassPhraseDecrypt.Location = new System.Drawing.Point(140, 134);
            this.textBox_PassPhraseDecrypt.Name = "textBox_PassPhraseDecrypt";
            this.textBox_PassPhraseDecrypt.Size = new System.Drawing.Size(258, 20);
            this.textBox_PassPhraseDecrypt.TabIndex = 18;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(137, 64);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(257, 13);
            this.label14.TabIndex = 14;
            this.label14.Text = "target file name can be the same as the source name";
            // 
            // textBox_DecryptTargetName
            // 
            this.textBox_DecryptTargetName.Location = new System.Drawing.Point(140, 89);
            this.textBox_DecryptTargetName.Name = "textBox_DecryptTargetName";
            this.textBox_DecryptTargetName.Size = new System.Drawing.Size(258, 20);
            this.textBox_DecryptTargetName.TabIndex = 13;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(16, 89);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(86, 13);
            this.label15.TabIndex = 12;
            this.label15.Text = "Target File name";
            // 
            // textBox_DecryptSourceName
            // 
            this.textBox_DecryptSourceName.Location = new System.Drawing.Point(140, 32);
            this.textBox_DecryptSourceName.Name = "textBox_DecryptSourceName";
            this.textBox_DecryptSourceName.Size = new System.Drawing.Size(258, 20);
            this.textBox_DecryptSourceName.TabIndex = 11;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(16, 40);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(89, 13);
            this.label16.TabIndex = 10;
            this.label16.Text = "Source File name";
            // 
            // button_BrowseFileToDecrypt
            // 
            this.button_BrowseFileToDecrypt.Location = new System.Drawing.Point(404, 30);
            this.button_BrowseFileToDecrypt.Name = "button_BrowseFileToDecrypt";
            this.button_BrowseFileToDecrypt.Size = new System.Drawing.Size(44, 23);
            this.button_BrowseFileToDecrypt.TabIndex = 6;
            this.button_BrowseFileToDecrypt.Text = "...";
            this.button_BrowseFileToDecrypt.UseVisualStyleBackColor = true;
            this.button_BrowseFileToDecrypt.Click += new System.EventHandler(this.button_BrowseFileToDecrypt_Click);
            // 
            // button_StartToDecrypt
            // 
            this.button_StartToDecrypt.Location = new System.Drawing.Point(440, 215);
            this.button_StartToDecrypt.Name = "button_StartToDecrypt";
            this.button_StartToDecrypt.Size = new System.Drawing.Size(75, 23);
            this.button_StartToDecrypt.TabIndex = 6;
            this.button_StartToDecrypt.Text = "Start";
            this.button_StartToDecrypt.UseVisualStyleBackColor = true;
            this.button_StartToDecrypt.Click += new System.EventHandler(this.button_StartToDecrypt_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(148, 48);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(190, 13);
            this.label13.TabIndex = 52;
            this.label13.Text = "(split with \';\' for multiple process names)";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(151, 80);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(287, 20);
            this.textBox1.TabIndex = 51;
            this.textBox1.Text = "explorer.exe";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 83);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(105, 13);
            this.label7.TabIndex = 50;
            this.label7.Text = "Black List Processes";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(151, 25);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(287, 20);
            this.textBox2.TabIndex = 49;
            this.textBox2.Text = "*";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 25);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(106, 13);
            this.label8.TabIndex = 48;
            this.label8.Text = "White List Processes";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(148, 103);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(190, 13);
            this.label11.TabIndex = 47;
            this.label11.Text = "(split with \';\' for multiple process names)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 101);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(144, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(257, 13);
            this.label4.TabIndex = 9;
            // 
            // button_SetupDropFolder
            // 
            this.button_SetupDropFolder.Location = new System.Drawing.Point(3, 102);
            this.button_SetupDropFolder.Name = "button_SetupDropFolder";
            this.button_SetupDropFolder.Size = new System.Drawing.Size(105, 23);
            this.button_SetupDropFolder.TabIndex = 4;
            this.button_SetupDropFolder.Text = "Setup drop folder";
            this.button_SetupDropFolder.UseVisualStyleBackColor = true;
            this.button_SetupDropFolder.Click += new System.EventHandler(this.button_SetupDropFolder_Click);
            // 
            // Form_FileCrypt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(556, 281);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_FileCrypt";
            this.Text = "Auto FileCrypt Tool";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form_FileCrypt_FormClosed);
            this.tabPage_EncryptFile.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage_Folder.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage_EncryptOnRead.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.tabPage_DecryptFile.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TabPage tabPage_EncryptFile;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button_BrowseEncryptFile;
        private System.Windows.Forms.Button button_StartToEncrypt;
        private System.Windows.Forms.TabPage tabPage_Folder;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_RemoveFolder;
        private System.Windows.Forms.Button button_AddFolder;
        private System.Windows.Forms.ListView listView_AutoEncryptFolders;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_EncryptTargetName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_EncryptSourceName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button_Stop;
        private System.Windows.Forms.Button button_Start;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TabPage tabPage_DecryptFile;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox textBox_DecryptTargetName;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox textBox_DecryptSourceName;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button button_BrowseFileToDecrypt;
        private System.Windows.Forms.Button button_StartToDecrypt;
        private System.Windows.Forms.TabPage tabPage_EncryptOnRead;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button button_StopService;
        private System.Windows.Forms.Button button_StartService;
        private System.Windows.Forms.Button button_RemoveProtectFolder;
        private System.Windows.Forms.Button button_AddProtectFolder;
        private System.Windows.Forms.ListView listView_EncryptOnReadFolders;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBox_PassPhraseEncrypt;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox textBox_PassPhraseDecrypt;
        private System.Windows.Forms.Button button_Help;
        private System.Windows.Forms.Button button_SetupDropFolder;
    }
}

