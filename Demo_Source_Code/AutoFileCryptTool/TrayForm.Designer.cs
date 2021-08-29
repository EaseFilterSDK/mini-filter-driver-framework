namespace AutoFileCryptTool
{
    partial class TrayForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TrayForm));
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.consoleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.openProtectorSourceCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sdkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.reportBugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.toolStripMenuItemEncryptInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.consoleToolStripMenuItem,
            this.toolStripMenuItem5,
            this.openProtectorSourceCodeToolStripMenuItem,
            this.toolStripMenuItemEncryptInfo,
            this.sdkToolStripMenuItem,
            this.toolStripMenuItem1,
            this.reportBugToolStripMenuItem,
            this.toolStripMenuItem3,
            this.exitToolStripMenuItem1});
            this.contextMenuStrip.Name = "contextMenuStrip_fileAgent";
            this.contextMenuStrip.Size = new System.Drawing.Size(225, 176);
            this.contextMenuStrip.Text = "-";
            // 
            // consoleToolStripMenuItem
            // 
            this.consoleToolStripMenuItem.Name = "consoleToolStripMenuItem";
            this.consoleToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.consoleToolStripMenuItem.Text = "Open FileCrypt Tool";
            this.consoleToolStripMenuItem.Click += new System.EventHandler(this.consoleToolStripMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(221, 6);
            // 
            // openProtectorSourceCodeToolStripMenuItem
            // 
            this.openProtectorSourceCodeToolStripMenuItem.Name = "openProtectorSourceCodeToolStripMenuItem";
            this.openProtectorSourceCodeToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.openProtectorSourceCodeToolStripMenuItem.Text = "Open SDK folder";
            this.openProtectorSourceCodeToolStripMenuItem.Click += new System.EventHandler(this.openProtectorSourceCodeToolStripMenuItem_Click);
            // 
            // sdkToolStripMenuItem
            // 
            this.sdkToolStripMenuItem.Name = "sdkToolStripMenuItem";
            this.sdkToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.sdkToolStripMenuItem.Text = "SDK manual";
            this.sdkToolStripMenuItem.Click += new System.EventHandler(this.sdkToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(221, 6);
            // 
            // reportBugToolStripMenuItem
            // 
            this.reportBugToolStripMenuItem.Name = "reportBugToolStripMenuItem";
            this.reportBugToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.reportBugToolStripMenuItem.Text = "Report bug or make suggestion";
            this.reportBugToolStripMenuItem.Click += new System.EventHandler(this.reportBugToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(221, 6);
            // 
            // exitToolStripMenuItem1
            // 
            this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            this.exitToolStripMenuItem1.Size = new System.Drawing.Size(224, 22);
            this.exitToolStripMenuItem1.Text = "Exit";
            this.exitToolStripMenuItem1.Click += new System.EventHandler(this.exitToolStripMenuItem1_Click);
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "EaseFilter File Protector";
            this.notifyIcon.Visible = true;
            // 
            // toolStripMenuItemEncryptInfo
            // 
            this.toolStripMenuItemEncryptInfo.Name = "toolStripMenuItemEncryptInfo";
            this.toolStripMenuItemEncryptInfo.Size = new System.Drawing.Size(224, 22);
            this.toolStripMenuItemEncryptInfo.Text = "Transparent file encryption";
            this.toolStripMenuItemEncryptInfo.Click += new System.EventHandler(this.toolStripMenuItemEncryptInfo_Click);
            // 
            // TrayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(263, 102);
            this.Name = "TrayForm";
            this.Text = "TrayForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.TrayForm_Load);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem consoleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sdkToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem reportBugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem1;
        public System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ToolStripMenuItem openProtectorSourceCodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemEncryptInfo;
    }
}