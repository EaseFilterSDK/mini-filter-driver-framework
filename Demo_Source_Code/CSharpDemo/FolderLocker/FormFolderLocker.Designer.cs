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
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.tabPage_Help = new System.Windows.Forms.TabPage();
            this.linkLabel_SDK = new System.Windows.Forms.LinkLabel();
            this.linkLabel_Report = new System.Windows.Forms.LinkLabel();
            this.label_VersionInfo = new System.Windows.Forms.Label();
            this.tabPage_Folder = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.listView_LockFolders = new System.Windows.Forms.ListView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_StartFilter = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Stop = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_AddFolder = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_RemoveFolder = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_ModifyFolder = new System.Windows.Forms.ToolStripButton();
            this.listView_AccessRights = new System.Windows.Forms.ListView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.toolStripButton_ApplyTrialKey = new System.Windows.Forms.ToolStripButton();
            this.tabPage_Help.SuspendLayout();
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
            // tabPage_Help
            // 
            this.tabPage_Help.Controls.Add(this.linkLabel_SDK);
            this.tabPage_Help.Controls.Add(this.linkLabel_Report);
            this.tabPage_Help.Controls.Add(this.label_VersionInfo);
            this.tabPage_Help.Location = new System.Drawing.Point(4, 32);
            this.tabPage_Help.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage_Help.Name = "tabPage_Help";
            this.tabPage_Help.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage_Help.Size = new System.Drawing.Size(699, 311);
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
            // tabPage_Folder
            // 
            this.tabPage_Folder.Controls.Add(this.splitContainer1);
            this.tabPage_Folder.Location = new System.Drawing.Point(4, 32);
            this.tabPage_Folder.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage_Folder.Name = "tabPage_Folder";
            this.tabPage_Folder.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage_Folder.Size = new System.Drawing.Size(699, 311);
            this.tabPage_Folder.TabIndex = 0;
            this.tabPage_Folder.Text = "Locked Folders";
            this.tabPage_Folder.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(2, 2);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(2);
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
            this.splitContainer1.Size = new System.Drawing.Size(695, 307);
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
            this.statusStrip1.Size = new System.Drawing.Size(695, 22);
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
            this.listView_LockFolders.Margin = new System.Windows.Forms.Padding(2);
            this.listView_LockFolders.MultiSelect = false;
            this.listView_LockFolders.Name = "listView_LockFolders";
            this.listView_LockFolders.ShowItemToolTips = true;
            this.listView_LockFolders.Size = new System.Drawing.Size(695, 173);
            this.listView_LockFolders.TabIndex = 3;
            this.listView_LockFolders.UseCompatibleStateImageBehavior = false;
            this.listView_LockFolders.View = System.Windows.Forms.View.Details;
            this.listView_LockFolders.SelectedIndexChanged += new System.EventHandler(this.listView_LockFolders_SelectedIndexChanged);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.toolStripButton_StartFilter,
            this.toolStripButton_Stop,
            this.toolStripButton_AddFolder,
            this.toolStripSeparator5,
            this.toolStripButton_RemoveFolder,
            this.toolStripSeparator4,
            this.toolStripButton_ModifyFolder,
            this.toolStripButton_ApplyTrialKey});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(695, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_StartFilter
            // 
            this.toolStripButton_StartFilter.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_StartFilter.Image")));
            this.toolStripButton_StartFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_StartFilter.Name = "toolStripButton_StartFilter";
            this.toolStripButton_StartFilter.Size = new System.Drawing.Size(103, 22);
            this.toolStripButton_StartFilter.Text = "Start protector";
            this.toolStripButton_StartFilter.Click += new System.EventHandler(this.toolStripButton_StartFilter_Click);
            // 
            // toolStripButton_Stop
            // 
            this.toolStripButton_Stop.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Stop.Image")));
            this.toolStripButton_Stop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Stop.Name = "toolStripButton_Stop";
            this.toolStripButton_Stop.Size = new System.Drawing.Size(103, 22);
            this.toolStripButton_Stop.Text = "Stop protector";
            this.toolStripButton_Stop.Click += new System.EventHandler(this.toolStripButton_Stop_Click);
            // 
            // toolStripButton_AddFolder
            // 
            this.toolStripButton_AddFolder.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_AddFolder.Image")));
            this.toolStripButton_AddFolder.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_AddFolder.Name = "toolStripButton_AddFolder";
            this.toolStripButton_AddFolder.Size = new System.Drawing.Size(85, 22);
            this.toolStripButton_AddFolder.Text = "Add Folder";
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
            this.toolStripButton_RemoveFolder.Size = new System.Drawing.Size(106, 22);
            this.toolStripButton_RemoveFolder.Text = "Remove Folder";
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
            this.toolStripButton_ModifyFolder.Size = new System.Drawing.Size(110, 22);
            this.toolStripButton_ModifyFolder.Text = "Modify Settings";
            this.toolStripButton_ModifyFolder.Click += new System.EventHandler(this.toolStripButton_ModifyFolder_Click);
            // 
            // listView_AccessRights
            // 
            this.listView_AccessRights.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_AccessRights.FullRowSelect = true;
            this.listView_AccessRights.HideSelection = false;
            this.listView_AccessRights.HoverSelection = true;
            this.listView_AccessRights.Location = new System.Drawing.Point(0, 0);
            this.listView_AccessRights.Margin = new System.Windows.Forms.Padding(2);
            this.listView_AccessRights.MultiSelect = false;
            this.listView_AccessRights.Name = "listView_AccessRights";
            this.listView_AccessRights.ShowItemToolTips = true;
            this.listView_AccessRights.Size = new System.Drawing.Size(695, 106);
            this.listView_AccessRights.TabIndex = 3;
            this.listView_AccessRights.UseCompatibleStateImageBehavior = false;
            this.listView_AccessRights.View = System.Windows.Forms.View.Details;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage_Folder);
            this.tabControl1.Controls.Add(this.tabPage_Help);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new System.Drawing.Point(6, 8);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(707, 347);
            this.tabControl1.TabIndex = 0;
            // 
            // toolStripButton_ApplyTrialKey
            // 
            this.toolStripButton_ApplyTrialKey.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_ApplyTrialKey.Image")));
            this.toolStripButton_ApplyTrialKey.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_ApplyTrialKey.Name = "toolStripButton_ApplyTrialKey";
            this.toolStripButton_ApplyTrialKey.Size = new System.Drawing.Size(102, 22);
            this.toolStripButton_ApplyTrialKey.Text = "Apply trial key";
            this.toolStripButton_ApplyTrialKey.Click += new System.EventHandler(this.toolStripButton_ApplyTrialKey_Click);
            // 
            // Form_FolderLocker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(707, 347);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form_FolderLocker";
            this.Text = " EaseFilter Folder Locker";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form_FolderLocker_FormClosed);
            this.tabPage_Help.ResumeLayout(false);
            this.tabPage_Help.PerformLayout();
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
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TabPage tabPage_Help;
        private System.Windows.Forms.LinkLabel linkLabel_SDK;
        private System.Windows.Forms.LinkLabel linkLabel_Report;
        private System.Windows.Forms.Label label_VersionInfo;
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
        private System.Windows.Forms.ToolStripButton toolStripButton_StartFilter;
        private System.Windows.Forms.ToolStripButton toolStripButton_Stop;
        private System.Windows.Forms.ToolStripButton toolStripButton_ApplyTrialKey;
    }
}

