namespace FileProtector
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
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.helpTopicsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpTopicsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.reportAProblemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uninstallDriverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openProtectorSourceCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sdkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.consoleToolStripMenuItem,
            this.toolStripMenuItem2,
            this.settingsToolStripMenuItem,
            this.toolStripMenuItem5,
            this.helpTopicsToolStripMenuItem,
            this.openProtectorSourceCodeToolStripMenuItem,
            this.sdkToolStripMenuItem,
            this.toolStripMenuItem3,
            this.exitToolStripMenuItem1});
            this.contextMenuStrip.Name = "contextMenuStrip_fileAgent";
            this.contextMenuStrip.Size = new System.Drawing.Size(200, 154);
            this.contextMenuStrip.Text = "-";
            // 
            // consoleToolStripMenuItem
            // 
            this.consoleToolStripMenuItem.Name = "consoleToolStripMenuItem";
            this.consoleToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.consoleToolStripMenuItem.Text = "Show protector console";
            this.consoleToolStripMenuItem.Click += new System.EventHandler(this.consoleToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(196, 6);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.settingsToolStripMenuItem.Text = "Settings...";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(196, 6);
            // 
            // helpTopicsToolStripMenuItem
            // 
            this.helpTopicsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpTopicsToolStripMenuItem1,
            this.reportAProblemToolStripMenuItem,
            this.uninstallDriverToolStripMenuItem});
            this.helpTopicsToolStripMenuItem.Name = "helpTopicsToolStripMenuItem";
            this.helpTopicsToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.helpTopicsToolStripMenuItem.Text = "Help";
            // 
            // helpTopicsToolStripMenuItem1
            // 
            this.helpTopicsToolStripMenuItem1.Name = "helpTopicsToolStripMenuItem1";
            this.helpTopicsToolStripMenuItem1.Size = new System.Drawing.Size(241, 22);
            this.helpTopicsToolStripMenuItem1.Text = "Help topics";
            this.helpTopicsToolStripMenuItem1.Click += new System.EventHandler(this.helpTopicsToolStripMenuItem1_Click);
            // 
            // reportAProblemToolStripMenuItem
            // 
            this.reportAProblemToolStripMenuItem.Name = "reportAProblemToolStripMenuItem";
            this.reportAProblemToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.reportAProblemToolStripMenuItem.Text = "Report a problem or suggestion";
            this.reportAProblemToolStripMenuItem.Click += new System.EventHandler(this.reportAProblemToolStripMenuItem_Click);
            // 
            // uninstallDriverToolStripMenuItem
            // 
            this.uninstallDriverToolStripMenuItem.Name = "uninstallDriverToolStripMenuItem";
            this.uninstallDriverToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.uninstallDriverToolStripMenuItem.Text = "Uninstall driver";
            this.uninstallDriverToolStripMenuItem.Click += new System.EventHandler(this.uninstallDriverToolStripMenuItem_Click);
            // 
            // openProtectorSourceCodeToolStripMenuItem
            // 
            this.openProtectorSourceCodeToolStripMenuItem.Name = "openProtectorSourceCodeToolStripMenuItem";
            this.openProtectorSourceCodeToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.openProtectorSourceCodeToolStripMenuItem.Text = "Open SDK folder";
            this.openProtectorSourceCodeToolStripMenuItem.Click += new System.EventHandler(this.openProtectorSourceCodeToolStripMenuItem_Click);
            // 
            // sdkToolStripMenuItem
            // 
            this.sdkToolStripMenuItem.Name = "sdkToolStripMenuItem";
            this.sdkToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.sdkToolStripMenuItem.Text = "SDK manual";
            this.sdkToolStripMenuItem.Click += new System.EventHandler(this.sdkToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(196, 6);
            // 
            // exitToolStripMenuItem1
            // 
            this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            this.exitToolStripMenuItem1.Size = new System.Drawing.Size(199, 22);
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
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem helpTopicsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpTopicsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem reportAProblemToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem1;
        public System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ToolStripMenuItem openProtectorSourceCodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uninstallDriverToolStripMenuItem;
    }
}