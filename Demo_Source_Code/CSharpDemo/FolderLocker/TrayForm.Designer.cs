namespace EaseFilter.FolderLocker
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
            this.openConsoleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.startLockertoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopLockertoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openConsoleToolStripMenuItem,
            this.toolStripMenuItem5,
            this.startLockertoolStripMenuItem,
            this.stopLockertoolStripMenuItem,
            this.toolStripMenuItem3,
            this.exitToolStripMenuItem1});
            this.contextMenuStrip.Name = "contextMenuStrip_fileAgent";
            this.contextMenuStrip.Size = new System.Drawing.Size(150, 104);
            this.contextMenuStrip.Text = "-";
            // 
            // openConsoleToolStripMenuItem
            // 
            this.openConsoleToolStripMenuItem.Name = "openConsoleToolStripMenuItem";
            this.openConsoleToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.openConsoleToolStripMenuItem.Text = "Open Console";
            this.openConsoleToolStripMenuItem.Click += new System.EventHandler(this.openConsoleToolStripMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(146, 6);
            // 
            // startLockertoolStripMenuItem
            // 
            this.startLockertoolStripMenuItem.Name = "startLockertoolStripMenuItem";
            this.startLockertoolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.startLockertoolStripMenuItem.Text = "Start Locker";
            this.startLockertoolStripMenuItem.Click += new System.EventHandler(this.startLockertoolStripMenuItem_Click);
            // 
            // stopLockertoolStripMenuItem
            // 
            this.stopLockertoolStripMenuItem.Name = "stopLockertoolStripMenuItem";
            this.stopLockertoolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.stopLockertoolStripMenuItem.Text = "Stop Locker";
            this.stopLockertoolStripMenuItem.Click += new System.EventHandler(this.stopLockertoolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(146, 6);
            // 
            // exitToolStripMenuItem1
            // 
            this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            this.exitToolStripMenuItem1.Size = new System.Drawing.Size(149, 22);
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
        private System.Windows.Forms.ToolStripMenuItem openConsoleToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem1;
        public System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ToolStripMenuItem startLockertoolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopLockertoolStripMenuItem;
    }
}