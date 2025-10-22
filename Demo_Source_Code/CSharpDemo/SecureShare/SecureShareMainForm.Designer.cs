namespace  SecureShare
{
    partial class SecureShareMainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SecureShareMainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_StartFilter = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_Stop = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_ShareFileManager = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_GetSharedFileAccessLog = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_ClearMessage = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.listView_Message = new System.Windows.Forms.ListView();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.exitToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(974, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Settings";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(38, 20);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_StartFilter,
            this.toolStripSeparator1,
            this.toolStripButton_Stop,
            this.toolStripSeparator2,
            this.toolStripButton_ShareFileManager,
            this.toolStripSeparator6,
            this.toolStripButton_GetSharedFileAccessLog,
            this.toolStripSeparator4,
            this.toolStripButton_ClearMessage,
            this.toolStripSeparator3});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(974, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton_StartFilter
            // 
            this.toolStripButton_StartFilter.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_StartFilter.Image")));
            this.toolStripButton_StartFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_StartFilter.Name = "toolStripButton_StartFilter";
            this.toolStripButton_StartFilter.Size = new System.Drawing.Size(91, 22);
            this.toolStripButton_StartFilter.Text = "Start Service";
            this.toolStripButton_StartFilter.Click += new System.EventHandler(this.toolStripButton_StartFilter_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_Stop
            // 
            this.toolStripButton_Stop.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Stop.Image")));
            this.toolStripButton_Stop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Stop.Name = "toolStripButton_Stop";
            this.toolStripButton_Stop.Size = new System.Drawing.Size(91, 22);
            this.toolStripButton_Stop.Text = "Stop Service";
            this.toolStripButton_Stop.Click += new System.EventHandler(this.toolStripButton_Stop_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_ShareFileManager
            // 
            this.toolStripButton_ShareFileManager.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_ShareFileManager.Image")));
            this.toolStripButton_ShareFileManager.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_ShareFileManager.Name = "toolStripButton_ShareFileManager";
            this.toolStripButton_ShareFileManager.Size = new System.Drawing.Size(132, 22);
            this.toolStripButton_ShareFileManager.Text = "Shared file manager";
            this.toolStripButton_ShareFileManager.Click += new System.EventHandler(this.toolStripButton_ShareFileManager_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_GetSharedFileAccessLog
            // 
            this.toolStripButton_GetSharedFileAccessLog.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_GetSharedFileAccessLog.Image")));
            this.toolStripButton_GetSharedFileAccessLog.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_GetSharedFileAccessLog.Name = "toolStripButton_GetSharedFileAccessLog";
            this.toolStripButton_GetSharedFileAccessLog.Size = new System.Drawing.Size(83, 22);
            this.toolStripButton_GetSharedFileAccessLog.Text = "Access log";
            this.toolStripButton_GetSharedFileAccessLog.Click += new System.EventHandler(this.toolStripButton_GetSharedFileAccessLog_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_ClearMessage
            // 
            this.toolStripButton_ClearMessage.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_ClearMessage.Image")));
            this.toolStripButton_ClearMessage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_ClearMessage.Name = "toolStripButton_ClearMessage";
            this.toolStripButton_ClearMessage.Size = new System.Drawing.Size(108, 22);
            this.toolStripButton_ClearMessage.Text = "Clear messages";
            this.toolStripButton_ClearMessage.Click += new System.EventHandler(this.toolStripButton_ClearMessage_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // listView_Message
            // 
            this.listView_Message.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.listView_Message.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_Message.FullRowSelect = true;
            this.listView_Message.GridLines = true;
            this.listView_Message.HideSelection = false;
            this.listView_Message.HotTracking = true;
            this.listView_Message.HoverSelection = true;
            this.listView_Message.LabelEdit = true;
            this.listView_Message.Location = new System.Drawing.Point(0, 49);
            this.listView_Message.MultiSelect = false;
            this.listView_Message.Name = "listView_Message";
            this.listView_Message.ShowItemToolTips = true;
            this.listView_Message.Size = new System.Drawing.Size(974, 457);
            this.listView_Message.TabIndex = 2;
            this.listView_Message.UseCompatibleStateImageBehavior = false;
            this.listView_Message.View = System.Windows.Forms.View.Details;
            // 
            // SecureShareMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(974, 506);
            this.Controls.Add(this.listView_Message);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "SecureShareMainForm";
            this.Text = "EaseFilter Secure File Sharing Demo";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ConsoleForm_FormClosed);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton_StartFilter;
        private System.Windows.Forms.ToolStripButton toolStripButton_Stop;
        private System.Windows.Forms.ToolStripButton toolStripButton_ShareFileManager;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton_GetSharedFileAccessLog;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ListView listView_Message;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton toolStripButton_ClearMessage;
    }
}

