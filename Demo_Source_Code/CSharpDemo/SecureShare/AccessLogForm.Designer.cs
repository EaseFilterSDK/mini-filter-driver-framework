
namespace SecureShare
{
    partial class AccessLogForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AccessLogForm));
            this.listView_AccessLog = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // listView_AccessLog
            // 
            this.listView_AccessLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_AccessLog.FullRowSelect = true;
            this.listView_AccessLog.GridLines = true;
            this.listView_AccessLog.HideSelection = false;
            this.listView_AccessLog.LabelEdit = true;
            this.listView_AccessLog.Location = new System.Drawing.Point(0, 0);
            this.listView_AccessLog.Name = "listView_AccessLog";
            this.listView_AccessLog.ShowItemToolTips = true;
            this.listView_AccessLog.Size = new System.Drawing.Size(800, 450);
            this.listView_AccessLog.TabIndex = 0;
            this.listView_AccessLog.UseCompatibleStateImageBehavior = false;
            this.listView_AccessLog.View = System.Windows.Forms.View.Details;
            // 
            // AccessLogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.listView_AccessLog);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AccessLogForm";
            this.Text = "AccessLogForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView_AccessLog;
    }
}