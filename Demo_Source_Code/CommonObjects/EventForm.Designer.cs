namespace EaseFilter.CommonObjects
{
    partial class EventForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EventForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.messageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.errorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.warningToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.informationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verboseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.traceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RefreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listView_EventView = new System.Windows.Forms.ListView();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.messageToolStripMenuItem,
            this.RefreshToolStripMenuItem,
            this.clearToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1012, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // messageToolStripMenuItem
            // 
            this.messageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.errorToolStripMenuItem,
            this.warningToolStripMenuItem,
            this.informationToolStripMenuItem,
            this.verboseToolStripMenuItem,
            this.traceToolStripMenuItem});
            this.messageToolStripMenuItem.Name = "messageToolStripMenuItem";
            this.messageToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.messageToolStripMenuItem.Text = "Message";
            // 
            // errorToolStripMenuItem
            // 
            this.errorToolStripMenuItem.Checked = true;
            this.errorToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.errorToolStripMenuItem.Name = "errorToolStripMenuItem";
            this.errorToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.errorToolStripMenuItem.Text = "Error";
            this.errorToolStripMenuItem.Click += new System.EventHandler(this.errorToolStripMenuItem_Click);
            // 
            // warningToolStripMenuItem
            // 
            this.warningToolStripMenuItem.Checked = true;
            this.warningToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.warningToolStripMenuItem.Name = "warningToolStripMenuItem";
            this.warningToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.warningToolStripMenuItem.Text = "Warning";
            this.warningToolStripMenuItem.Click += new System.EventHandler(this.warningToolStripMenuItem_Click);
            // 
            // informationToolStripMenuItem
            // 
            this.informationToolStripMenuItem.Checked = true;
            this.informationToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.informationToolStripMenuItem.Name = "informationToolStripMenuItem";
            this.informationToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.informationToolStripMenuItem.Text = "Information";
            this.informationToolStripMenuItem.Click += new System.EventHandler(this.informationToolStripMenuItem_Click);
            // 
            // verboseToolStripMenuItem
            // 
            this.verboseToolStripMenuItem.Name = "verboseToolStripMenuItem";
            this.verboseToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.verboseToolStripMenuItem.Text = "Verbose";
            this.verboseToolStripMenuItem.Click += new System.EventHandler(this.verboseToolStripMenuItem_Click);
            // 
            // traceToolStripMenuItem
            // 
            this.traceToolStripMenuItem.Name = "traceToolStripMenuItem";
            this.traceToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.traceToolStripMenuItem.Text = "Trace";
            this.traceToolStripMenuItem.Click += new System.EventHandler(this.traceToolStripMenuItem_Click);
            // 
            // RefreshToolStripMenuItem
            // 
            this.RefreshToolStripMenuItem.Name = "RefreshToolStripMenuItem";
            this.RefreshToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.RefreshToolStripMenuItem.Text = "Refresh";
            this.RefreshToolStripMenuItem.Click += new System.EventHandler(this.clearTasksToolStripMenuItem_Click);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.clearToolStripMenuItem.Text = "Clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // listView_EventView
            // 
            this.listView_EventView.BackColor = System.Drawing.SystemColors.Window;
            this.listView_EventView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_EventView.FullRowSelect = true;
            this.listView_EventView.Location = new System.Drawing.Point(0, 24);
            this.listView_EventView.Name = "listView_EventView";
            this.listView_EventView.ShowItemToolTips = true;
            this.listView_EventView.Size = new System.Drawing.Size(1012, 694);
            this.listView_EventView.TabIndex = 2;
            this.listView_EventView.UseCompatibleStateImageBehavior = false;
            this.listView_EventView.View = System.Windows.Forms.View.Details;
            this.listView_EventView.DoubleClick += new System.EventHandler(this.listView_EventView_DoubleClick);
            // 
            // EventForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1012, 718);
            this.Controls.Add(this.listView_EventView);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EventForm";
            this.Text = "Event Message Log";
            this.Load += new System.EventHandler(this.EventForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem messageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem verboseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem informationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem warningToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem errorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem RefreshToolStripMenuItem;
        private System.Windows.Forms.ListView listView_EventView;
        private System.Windows.Forms.ToolStripMenuItem traceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
    }
}