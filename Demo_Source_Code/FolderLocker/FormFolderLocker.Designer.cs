namespace EaseFilter.FolderLocker
{
    partial class Form_FolderLocker
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_FolderLocker));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tabPage_Client = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_BrowseFolder = new System.Windows.Forms.Button();
            this.textBox_SharedFileDropFolder = new System.Windows.Forms.TextBox();
            this.button_SaveDropFolder = new System.Windows.Forms.Button();
            this.tabPage_Help = new System.Windows.Forms.TabPage();
            this.linkLabel_SDK = new System.Windows.Forms.LinkLabel();
            this.linkLabel_Report = new System.Windows.Forms.LinkLabel();
            this.label_VersionInfo = new System.Windows.Forms.Label();
            this.tabPage_ShareFile = new System.Windows.Forms.TabPage();
            this.listView_SharedFiles = new System.Windows.Forms.ListView();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_RefreshList = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_CreateShareFile = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_RemoveShareFile = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_ModifyShareFile = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_AccessLog = new System.Windows.Forms.ToolStripButton();
            this.tabPage_Folder = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.listView_LockFolders = new System.Windows.Forms.ListView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_AddFolder = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_RemoveFolder = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_ModifyFolder = new System.Windows.Forms.ToolStripButton();
            this.listView_AccessRights = new System.Windows.Forms.ListView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.tabPage_Client.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage_Help.SuspendLayout();
            this.tabPage_ShareFile.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.tabPage_Folder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "folder_ok.png");
            // 
            // tabPage_Client
            // 
            this.tabPage_Client.Controls.Add(this.groupBox1);
            this.tabPage_Client.Controls.Add(this.button_SaveDropFolder);
            this.tabPage_Client.Location = new System.Drawing.Point(4, 32);
            this.tabPage_Client.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage_Client.Name = "tabPage_Client";
            this.tabPage_Client.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage_Client.Size = new System.Drawing.Size(609, 311);
            this.tabPage_Client.TabIndex = 5;
            this.tabPage_Client.Text = "Configuration Of Client";
            this.tabPage_Client.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_BrowseFolder);
            this.groupBox1.Controls.Add(this.textBox_SharedFileDropFolder);
            this.groupBox1.Location = new System.Drawing.Point(22, 42);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Size = new System.Drawing.Size(518, 76);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Shared File Drop Folder in Client";
            // 
            // button_BrowseFolder
            // 
            this.button_BrowseFolder.Location = new System.Drawing.Point(428, 34);
            this.button_BrowseFolder.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_BrowseFolder.Name = "button_BrowseFolder";
            this.button_BrowseFolder.Size = new System.Drawing.Size(62, 18);
            this.button_BrowseFolder.TabIndex = 2;
            this.button_BrowseFolder.Text = "Browse";
            this.button_BrowseFolder.UseVisualStyleBackColor = true;
            // 
            // textBox_SharedFileDropFolder
            // 
            this.textBox_SharedFileDropFolder.Location = new System.Drawing.Point(25, 34);
            this.textBox_SharedFileDropFolder.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBox_SharedFileDropFolder.Name = "textBox_SharedFileDropFolder";
            this.textBox_SharedFileDropFolder.Size = new System.Drawing.Size(379, 20);
            this.textBox_SharedFileDropFolder.TabIndex = 1;
            // 
            // button_SaveDropFolder
            // 
            this.button_SaveDropFolder.Location = new System.Drawing.Point(449, 145);
            this.button_SaveDropFolder.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_SaveDropFolder.Name = "button_SaveDropFolder";
            this.button_SaveDropFolder.Size = new System.Drawing.Size(56, 18);
            this.button_SaveDropFolder.TabIndex = 3;
            this.button_SaveDropFolder.Text = "Save";
            this.button_SaveDropFolder.UseVisualStyleBackColor = true;
            this.button_SaveDropFolder.Click += new System.EventHandler(this.button_SaveDropFolder_Click);
            // 
            // tabPage_Help
            // 
            this.tabPage_Help.Controls.Add(this.linkLabel_SDK);
            this.tabPage_Help.Controls.Add(this.linkLabel_Report);
            this.tabPage_Help.Controls.Add(this.label_VersionInfo);
            this.tabPage_Help.Location = new System.Drawing.Point(4, 32);
            this.tabPage_Help.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage_Help.Name = "tabPage_Help";
            this.tabPage_Help.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage_Help.Size = new System.Drawing.Size(609, 311);
            this.tabPage_Help.TabIndex = 4;
            this.tabPage_Help.Text = "Help";
            this.tabPage_Help.UseVisualStyleBackColor = true;
            this.tabPage_Help.Visible = false;
            // 
            // linkLabel_SDK
            // 
            this.linkLabel_SDK.AutoSize = true;
            this.linkLabel_SDK.Location = new System.Drawing.Point(18, 63);
            this.linkLabel_SDK.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.linkLabel_SDK.Name = "linkLabel_SDK";
            this.linkLabel_SDK.Size = new System.Drawing.Size(192, 13);
            this.linkLabel_SDK.TabIndex = 10;
            this.linkLabel_SDK.TabStop = true;
            this.linkLabel_SDK.Text = "Online help for EaseFilter Folder Locker";
            this.linkLabel_SDK.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_SDK_LinkClicked);
            // 
            // linkLabel_Report
            // 
            this.linkLabel_Report.AutoSize = true;
            this.linkLabel_Report.Location = new System.Drawing.Point(18, 33);
            this.linkLabel_Report.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.linkLabel_Report.Name = "linkLabel_Report";
            this.linkLabel_Report.Size = new System.Drawing.Size(173, 13);
            this.linkLabel_Report.TabIndex = 9;
            this.linkLabel_Report.TabStop = true;
            this.linkLabel_Report.Text = "Report a bug or make a suggestion";
            this.linkLabel_Report.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_Report_LinkClicked);
            // 
            // label_VersionInfo
            // 
            this.label_VersionInfo.AutoSize = true;
            this.label_VersionInfo.Location = new System.Drawing.Point(18, 20);
            this.label_VersionInfo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_VersionInfo.Name = "label_VersionInfo";
            this.label_VersionInfo.Size = new System.Drawing.Size(0, 13);
            this.label_VersionInfo.TabIndex = 9;
            // 
            // tabPage_ShareFile
            // 
            this.tabPage_ShareFile.Controls.Add(this.listView_SharedFiles);
            this.tabPage_ShareFile.Controls.Add(this.toolStrip2);
            this.tabPage_ShareFile.Location = new System.Drawing.Point(4, 32);
            this.tabPage_ShareFile.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage_ShareFile.Name = "tabPage_ShareFile";
            this.tabPage_ShareFile.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage_ShareFile.Size = new System.Drawing.Size(609, 311);
            this.tabPage_ShareFile.TabIndex = 1;
            this.tabPage_ShareFile.Text = "Secure File Sharing";
            this.tabPage_ShareFile.UseVisualStyleBackColor = true;
            this.tabPage_ShareFile.Visible = false;
            // 
            // listView_SharedFiles
            // 
            this.listView_SharedFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_SharedFiles.FullRowSelect = true;
            this.listView_SharedFiles.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView_SharedFiles.HideSelection = false;
            this.listView_SharedFiles.HoverSelection = true;
            this.listView_SharedFiles.Location = new System.Drawing.Point(2, 27);
            this.listView_SharedFiles.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.listView_SharedFiles.MultiSelect = false;
            this.listView_SharedFiles.Name = "listView_SharedFiles";
            this.listView_SharedFiles.ShowItemToolTips = true;
            this.listView_SharedFiles.Size = new System.Drawing.Size(605, 282);
            this.listView_SharedFiles.TabIndex = 5;
            this.listView_SharedFiles.UseCompatibleStateImageBehavior = false;
            this.listView_SharedFiles.View = System.Windows.Forms.View.Details;
            // 
            // toolStrip2
            // 
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator6,
            this.toolStripButton_RefreshList,
            this.toolStripSeparator2,
            this.toolStripButton_CreateShareFile,
            this.toolStripSeparator7,
            this.toolStripButton_RemoveShareFile,
            this.toolStripSeparator8,
            this.toolStripButton_ModifyShareFile,
            this.toolStripButton_AccessLog});
            this.toolStrip2.Location = new System.Drawing.Point(2, 2);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(605, 25);
            this.toolStrip2.TabIndex = 4;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_RefreshList
            // 
            this.toolStripButton_RefreshList.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_RefreshList.Image")));
            this.toolStripButton_RefreshList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_RefreshList.Name = "toolStripButton_RefreshList";
            this.toolStripButton_RefreshList.Size = new System.Drawing.Size(87, 22);
            this.toolStripButton_RefreshList.Text = "Refresh List";
            this.toolStripButton_RefreshList.Click += new System.EventHandler(this.toolStripButton_RefreshList_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_CreateShareFile
            // 
            this.toolStripButton_CreateShareFile.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_CreateShareFile.Image")));
            this.toolStripButton_CreateShareFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_CreateShareFile.Name = "toolStripButton_CreateShareFile";
            this.toolStripButton_CreateShareFile.Size = new System.Drawing.Size(114, 22);
            this.toolStripButton_CreateShareFile.Text = "Create Share File";
            this.toolStripButton_CreateShareFile.Click += new System.EventHandler(this.toolStripButton_CreateShareFile_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_RemoveShareFile
            // 
            this.toolStripButton_RemoveShareFile.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_RemoveShareFile.Image")));
            this.toolStripButton_RemoveShareFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_RemoveShareFile.Name = "toolStripButton_RemoveShareFile";
            this.toolStripButton_RemoveShareFile.Size = new System.Drawing.Size(125, 22);
            this.toolStripButton_RemoveShareFile.Text = "Delete Shared Files";
            this.toolStripButton_RemoveShareFile.Click += new System.EventHandler(this.toolStripButton_RemoveShareFile_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_ModifyShareFile
            // 
            this.toolStripButton_ModifyShareFile.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_ModifyShareFile.Image")));
            this.toolStripButton_ModifyShareFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_ModifyShareFile.Name = "toolStripButton_ModifyShareFile";
            this.toolStripButton_ModifyShareFile.Size = new System.Drawing.Size(170, 22);
            this.toolStripButton_ModifyShareFile.Text = "Modify Shared File Settings";
            this.toolStripButton_ModifyShareFile.Click += new System.EventHandler(this.toolStripButton_ModifyShareFile_Click);
            // 
            // toolStripButton_AccessLog
            // 
            this.toolStripButton_AccessLog.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_AccessLog.Image")));
            this.toolStripButton_AccessLog.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_AccessLog.Name = "toolStripButton_AccessLog";
            this.toolStripButton_AccessLog.Size = new System.Drawing.Size(86, 22);
            this.toolStripButton_AccessLog.Text = "Access Log";
            this.toolStripButton_AccessLog.Click += new System.EventHandler(this.toolStripButton_AccessLog_Click);
            // 
            // tabPage_Folder
            // 
            this.tabPage_Folder.Controls.Add(this.splitContainer1);
            this.tabPage_Folder.Location = new System.Drawing.Point(4, 32);
            this.tabPage_Folder.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage_Folder.Name = "tabPage_Folder";
            this.tabPage_Folder.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage_Folder.Size = new System.Drawing.Size(609, 311);
            this.tabPage_Folder.TabIndex = 0;
            this.tabPage_Folder.Text = "Locked Folders";
            this.tabPage_Folder.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(2, 2);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.statusStrip1);
            this.splitContainer1.Panel1.Controls.Add(this.listView_LockFolders);
            this.splitContainer1.Panel1.Controls.Add(this.toolStrip1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listView_AccessRights);
            this.splitContainer1.Size = new System.Drawing.Size(605, 307);
            this.splitContainer1.SplitterDistance = 198;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 176);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 10, 0);
            this.statusStrip1.Size = new System.Drawing.Size(605, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
    
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(417, 17);
            this.toolStripStatusLabel1.Text = "Below is the access rights to the specific users and processes in selected folder" +
    "";
            // 
            // listView_LockFolders
            // 
            this.listView_LockFolders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_LockFolders.FullRowSelect = true;
            this.listView_LockFolders.HideSelection = false;
            this.listView_LockFolders.HoverSelection = true;
            this.listView_LockFolders.Location = new System.Drawing.Point(0, 25);
            this.listView_LockFolders.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.listView_LockFolders.MultiSelect = false;
            this.listView_LockFolders.Name = "listView_LockFolders";
            this.listView_LockFolders.ShowItemToolTips = true;
            this.listView_LockFolders.Size = new System.Drawing.Size(605, 173);
            this.listView_LockFolders.TabIndex = 3;
            this.listView_LockFolders.UseCompatibleStateImageBehavior = false;
            this.listView_LockFolders.View = System.Windows.Forms.View.Details;
            this.listView_LockFolders.SelectedIndexChanged += new System.EventHandler(this.listView_LockFolders_SelectedIndexChanged);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.toolStripButton_AddFolder,
            this.toolStripSeparator5,
            this.toolStripButton_RemoveFolder,
            this.toolStripSeparator4,
            this.toolStripButton_ModifyFolder});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(605, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_AddFolder
            // 
            this.toolStripButton_AddFolder.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_AddFolder.Image")));
            this.toolStripButton_AddFolder.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_AddFolder.Name = "toolStripButton_AddFolder";
            this.toolStripButton_AddFolder.Size = new System.Drawing.Size(139, 22);
            this.toolStripButton_AddFolder.Text = "Add Folder To Locker";
            this.toolStripButton_AddFolder.Click += new System.EventHandler(this.toolStripButton_AddFolder_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_RemoveFolder
            // 
            this.toolStripButton_RemoveFolder.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_RemoveFolder.Image")));
            this.toolStripButton_RemoveFolder.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_RemoveFolder.Name = "toolStripButton_RemoveFolder";
            this.toolStripButton_RemoveFolder.Size = new System.Drawing.Size(175, 22);
            this.toolStripButton_RemoveFolder.Text = "Remove Folder From Locker";
            this.toolStripButton_RemoveFolder.Click += new System.EventHandler(this.toolStripButton_RemoveFolder_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_ModifyFolder
            // 
            this.toolStripButton_ModifyFolder.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_ModifyFolder.Image")));
            this.toolStripButton_ModifyFolder.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_ModifyFolder.Name = "toolStripButton_ModifyFolder";
            this.toolStripButton_ModifyFolder.Size = new System.Drawing.Size(146, 22);
            this.toolStripButton_ModifyFolder.Text = "Modify Folder Settings";
            this.toolStripButton_ModifyFolder.Click += new System.EventHandler(this.toolStripButton_ModifyFolder_Click);
            // 
            // listView_AccessRights
            // 
            this.listView_AccessRights.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_AccessRights.FullRowSelect = true;
            this.listView_AccessRights.HideSelection = false;
            this.listView_AccessRights.HoverSelection = true;
            this.listView_AccessRights.Location = new System.Drawing.Point(0, 0);
            this.listView_AccessRights.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.listView_AccessRights.MultiSelect = false;
            this.listView_AccessRights.Name = "listView_AccessRights";
            this.listView_AccessRights.ShowItemToolTips = true;
            this.listView_AccessRights.Size = new System.Drawing.Size(605, 106);
            this.listView_AccessRights.TabIndex = 3;
            this.listView_AccessRights.UseCompatibleStateImageBehavior = false;
            this.listView_AccessRights.View = System.Windows.Forms.View.Details;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage_Folder);
            this.tabControl1.Controls.Add(this.tabPage_ShareFile);
            this.tabControl1.Controls.Add(this.tabPage_Client);
            this.tabControl1.Controls.Add(this.tabPage_Help);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new System.Drawing.Point(6, 8);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(617, 347);
            this.tabControl1.TabIndex = 0;
            // 
            // Form_FolderLocker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(617, 347);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form_FolderLocker";
            this.Text = " EaseFilter Folder Locker";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form_FolderLocker_FormClosed);
            this.tabPage_Client.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage_Help.ResumeLayout(false);
            this.tabPage_Help.PerformLayout();
            this.tabPage_ShareFile.ResumeLayout(false);
            this.tabPage_ShareFile.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.tabPage_Folder.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TabPage tabPage_Client;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_BrowseFolder;
        private System.Windows.Forms.TextBox textBox_SharedFileDropFolder;
        private System.Windows.Forms.Button button_SaveDropFolder;
        private System.Windows.Forms.TabPage tabPage_Help;
        private System.Windows.Forms.LinkLabel linkLabel_SDK;
        private System.Windows.Forms.LinkLabel linkLabel_Report;
        private System.Windows.Forms.Label label_VersionInfo;
        private System.Windows.Forms.TabPage tabPage_ShareFile;
        private System.Windows.Forms.ListView listView_SharedFiles;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton toolStripButton_RefreshList;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButton_CreateShareFile;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripButton toolStripButton_RemoveShareFile;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripButton toolStripButton_ModifyShareFile;
        private System.Windows.Forms.ToolStripButton toolStripButton_AccessLog;
        private System.Windows.Forms.TabPage tabPage_Folder;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ListView listView_LockFolders;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton_AddFolder;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton toolStripButton_RemoveFolder;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton toolStripButton_ModifyFolder;
        private System.Windows.Forms.ListView listView_AccessRights;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}

