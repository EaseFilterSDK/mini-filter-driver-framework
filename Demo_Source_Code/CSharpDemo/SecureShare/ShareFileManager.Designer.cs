namespace  SecureShare
{
    partial class ShareFileManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShareFileManager));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_CreateShareFile = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_ModifySharedFile = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_DeleteDRM = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_GetSharedFiles = new System.Windows.Forms.ToolStripButton();
            this.listView_ShareFileList = new System.Windows.Forms.ListView();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_CreateShareFile,
            this.toolStripSeparator5,
            this.toolStripButton_ModifySharedFile,
            this.toolStripSeparator7,
            this.toolStripButton_DeleteDRM,
            this.toolStripSeparator4,
            this.toolStripButton_GetSharedFiles});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(974, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton_CreateShareFile
            // 
            this.toolStripButton_CreateShareFile.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_CreateShareFile.Image")));
            this.toolStripButton_CreateShareFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_CreateShareFile.Name = "toolStripButton_CreateShareFile";
            this.toolStripButton_CreateShareFile.Size = new System.Drawing.Size(111, 22);
            this.toolStripButton_CreateShareFile.Text = "Create share file";
            this.toolStripButton_CreateShareFile.Click += new System.EventHandler(this.toolStripButton_CreateShareFile_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_ModifySharedFile
            // 
            this.toolStripButton_ModifySharedFile.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_ModifySharedFile.Image")));
            this.toolStripButton_ModifySharedFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_ModifySharedFile.Name = "toolStripButton_ModifySharedFile";
            this.toolStripButton_ModifySharedFile.Size = new System.Drawing.Size(122, 22);
            this.toolStripButton_ModifySharedFile.Text = "Modify shared file";
            this.toolStripButton_ModifySharedFile.Click += new System.EventHandler(this.toolStripButton_ModifySharedFile_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_DeleteDRM
            // 
            this.toolStripButton_DeleteDRM.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_DeleteDRM.Image")));
            this.toolStripButton_DeleteDRM.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_DeleteDRM.Name = "toolStripButton_DeleteDRM";
            this.toolStripButton_DeleteDRM.Size = new System.Drawing.Size(117, 22);
            this.toolStripButton_DeleteDRM.Text = "Delete shared file";
            this.toolStripButton_DeleteDRM.Click += new System.EventHandler(this.toolStripButton_DeleteDRM_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_GetSharedFiles
            // 
            this.toolStripButton_GetSharedFiles.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_GetSharedFiles.Image")));
            this.toolStripButton_GetSharedFiles.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_GetSharedFiles.Name = "toolStripButton_GetSharedFiles";
            this.toolStripButton_GetSharedFiles.Size = new System.Drawing.Size(124, 22);
            this.toolStripButton_GetSharedFiles.Text = "Get Shared file List";
            this.toolStripButton_GetSharedFiles.Click += new System.EventHandler(this.toolStripButton_GetSharedFiles_Click);
            // 
            // listView_ShareFileList
            // 
            this.listView_ShareFileList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_ShareFileList.FullRowSelect = true;
            this.listView_ShareFileList.GridLines = true;
            this.listView_ShareFileList.HideSelection = false;
            this.listView_ShareFileList.LabelEdit = true;
            this.listView_ShareFileList.Location = new System.Drawing.Point(0, 25);
            this.listView_ShareFileList.MultiSelect = false;
            this.listView_ShareFileList.Name = "listView_ShareFileList";
            this.listView_ShareFileList.Size = new System.Drawing.Size(974, 481);
            this.listView_ShareFileList.TabIndex = 2;
            this.listView_ShareFileList.UseCompatibleStateImageBehavior = false;
            this.listView_ShareFileList.View = System.Windows.Forms.View.Details;
            // 
            // ShareFileManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(974, 506);
            this.Controls.Add(this.listView_ShareFileList);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ShareFileManager";
            this.Text = "Share File Manager";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton_GetSharedFiles;
        private System.Windows.Forms.ListView listView_ShareFileList;
        private System.Windows.Forms.ToolStripButton toolStripButton_CreateShareFile;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton toolStripButton_ModifySharedFile;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripButton toolStripButton_DeleteDRM;
    }
}

