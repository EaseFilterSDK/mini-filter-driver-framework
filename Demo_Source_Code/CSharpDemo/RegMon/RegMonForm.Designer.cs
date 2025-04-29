namespace RegMon
{
    partial class RegMonForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RegMonForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_StartFilter = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_Stop = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_ClearMessage = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_UnitTest = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_ApplyTrialKey = new System.Windows.Forms.ToolStripButton();
            this.listView_Info = new System.Windows.Forms.ListView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_StartFilter,
            this.toolStripSeparator1,
            this.toolStripButton_Stop,
            this.toolStripSeparator2,
            this.toolStripButton_ClearMessage,
            this.toolStripSeparator3,
            this.toolStripButton_UnitTest,
            this.toolStripButton_ApplyTrialKey});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(909, 25);
            this.toolStrip1.TabIndex = 2;
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
            // toolStripButton_UnitTest
            // 
            this.toolStripButton_UnitTest.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_UnitTest.Image")));
            this.toolStripButton_UnitTest.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_UnitTest.Name = "toolStripButton_UnitTest";
            this.toolStripButton_UnitTest.Size = new System.Drawing.Size(118, 22);
            this.toolStripButton_UnitTest.Text = "Registry Unit Test";
            this.toolStripButton_UnitTest.Click += new System.EventHandler(this.toolStripButton_UnitTest_Click);
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
            // listView_Info
            // 
            this.listView_Info.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_Info.FullRowSelect = true;
            this.listView_Info.HoverSelection = true;
            this.listView_Info.LabelEdit = true;
            this.listView_Info.Location = new System.Drawing.Point(0, 49);
            this.listView_Info.Name = "listView_Info";
            this.listView_Info.ShowItemToolTips = true;
            this.listView_Info.Size = new System.Drawing.Size(909, 434);
            this.listView_Info.TabIndex = 3;
            this.listView_Info.UseCompatibleStateImageBehavior = false;
            this.listView_Info.View = System.Windows.Forms.View.Details;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(909, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // RegMonForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(909, 483);
            this.Controls.Add(this.listView_Info);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "RegMonForm";
            this.Text = "Registry Monitoring and Protection";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RegMonForm_FormClosing);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton_StartFilter;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton_Stop;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButton_ClearMessage;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripButton_UnitTest;
        private System.Windows.Forms.ListView listView_Info;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton_ApplyTrialKey;
    }
}

