namespace SecureShare
{
    partial class ShareFileManagerInServer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShareFileManagerInServer));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
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
            this.listView_SharedFiles = new System.Windows.Forms.ListView();
            this.toolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "folder_ok.png");
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
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(890, 27);
            this.toolStrip2.TabIndex = 3;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripButton_RefreshList
            // 
            this.toolStripButton_RefreshList.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_RefreshList.Image")));
            this.toolStripButton_RefreshList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_RefreshList.Name = "toolStripButton_RefreshList";
            this.toolStripButton_RefreshList.Size = new System.Drawing.Size(104, 24);
            this.toolStripButton_RefreshList.Text = "Refresh List";
            this.toolStripButton_RefreshList.Click += new System.EventHandler(this.toolStripButton_RefreshList_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripButton_CreateShareFile
            // 
            this.toolStripButton_CreateShareFile.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_CreateShareFile.Image")));
            this.toolStripButton_CreateShareFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_CreateShareFile.Name = "toolStripButton_CreateShareFile";
            this.toolStripButton_CreateShareFile.Size = new System.Drawing.Size(140, 24);
            this.toolStripButton_CreateShareFile.Text = "Create Share File";
            this.toolStripButton_CreateShareFile.Click += new System.EventHandler(this.toolStripButton_CreateShareFile_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripButton_RemoveShareFile
            // 
            this.toolStripButton_RemoveShareFile.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_RemoveShareFile.Image")));
            this.toolStripButton_RemoveShareFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_RemoveShareFile.Name = "toolStripButton_RemoveShareFile";
            this.toolStripButton_RemoveShareFile.Size = new System.Drawing.Size(156, 24);
            this.toolStripButton_RemoveShareFile.Text = "Delete Shared Files";
            this.toolStripButton_RemoveShareFile.Click += new System.EventHandler(this.toolStripButton_RemoveShareFile_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripButton_ModifyShareFile
            // 
            this.toolStripButton_ModifyShareFile.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_ModifyShareFile.Image")));
            this.toolStripButton_ModifyShareFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_ModifyShareFile.Name = "toolStripButton_ModifyShareFile";
            this.toolStripButton_ModifyShareFile.Size = new System.Drawing.Size(210, 24);
            this.toolStripButton_ModifyShareFile.Text = "Modify Shared File Settings";
            this.toolStripButton_ModifyShareFile.Click += new System.EventHandler(this.toolStripButton_ModifyShareFile_Click);
            // 
            // toolStripButton_AccessLog
            // 
            this.toolStripButton_AccessLog.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_AccessLog.Image")));
            this.toolStripButton_AccessLog.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_AccessLog.Name = "toolStripButton_AccessLog";
            this.toolStripButton_AccessLog.Size = new System.Drawing.Size(102, 24);
            this.toolStripButton_AccessLog.Text = "Access Log";
            this.toolStripButton_AccessLog.Click += new System.EventHandler(this.toolStripButton_AccessLog_Click);
            // 
            // listView_SharedFiles
            // 
            this.listView_SharedFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_SharedFiles.FullRowSelect = true;
            this.listView_SharedFiles.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView_SharedFiles.HideSelection = false;
            this.listView_SharedFiles.HoverSelection = true;
            this.listView_SharedFiles.Location = new System.Drawing.Point(0, 27);
            this.listView_SharedFiles.MultiSelect = false;
            this.listView_SharedFiles.Name = "listView_SharedFiles";
            this.listView_SharedFiles.ShowItemToolTips = true;
            this.listView_SharedFiles.Size = new System.Drawing.Size(890, 292);
            this.listView_SharedFiles.TabIndex = 4;
            this.listView_SharedFiles.UseCompatibleStateImageBehavior = false;
            this.listView_SharedFiles.View = System.Windows.Forms.View.Details;
            // 
            // ShareFileManagerInServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(890, 319);
            this.Controls.Add(this.listView_SharedFiles);
            this.Controls.Add(this.toolStrip2);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ShareFileManagerInServer";
            this.Text = "Secure File Sharing Manager In Server";
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
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
        private System.Windows.Forms.ListView listView_SharedFiles;
    }
}

