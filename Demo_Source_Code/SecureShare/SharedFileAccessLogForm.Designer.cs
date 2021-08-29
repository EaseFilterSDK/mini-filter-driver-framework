namespace SecureShare
{
    partial class SharedFileAccessLogForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SharedFileAccessLogForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.listView_AccessLog = new System.Windows.Forms.ListView() ;
            this.toolStripButton_ClearMessage = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripSeparator1,
            this.toolStripButton_ClearMessage});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(961, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(123, 22);
            this.toolStripButton1.Text = "Refresh access log";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton_GetAccessLog_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // listView_AccessLog
            // 
            this.listView_AccessLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_AccessLog.FullRowSelect = true;
            this.listView_AccessLog.HoverSelection = true;
            this.listView_AccessLog.LabelEdit = true;
            this.listView_AccessLog.Location = new System.Drawing.Point(0, 25);
            this.listView_AccessLog.Name = "listView_AccessLog";
            this.listView_AccessLog.ShowItemToolTips = true;
            this.listView_AccessLog.Size = new System.Drawing.Size(961, 521);
            this.listView_AccessLog.TabIndex = 4;
            this.listView_AccessLog.UseCompatibleStateImageBehavior = false;
            this.listView_AccessLog.View = System.Windows.Forms.View.Details;
            // 
            // toolStripButton_ClearMessage
            // 
            this.toolStripButton_ClearMessage.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_ClearMessage.Image")));
            this.toolStripButton_ClearMessage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_ClearMessage.Name = "toolStripButton_ClearMessage";
            this.toolStripButton_ClearMessage.Size = new System.Drawing.Size(111, 22);
            this.toolStripButton_ClearMessage.Text = "Clear access log";
            this.toolStripButton_ClearMessage.Click += new System.EventHandler(this.toolStripButton_ClearMessage_Click);
            // 
            // SharedFileAccessLogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(961, 546);
            this.Controls.Add(this.listView_AccessLog);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SharedFileAccessLogForm";
            this.Text = "Shared Files Access Log";
            this.Shown += new System.EventHandler(this.AccessLogForm_Shown);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ListView listView_AccessLog;
        private System.Windows.Forms.ToolStripButton toolStripButton_ClearMessage;
    }
}