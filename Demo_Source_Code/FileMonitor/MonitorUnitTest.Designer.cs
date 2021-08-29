namespace FileMonitor
{
    partial class MonitorUnitTest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MonitorUnitTest));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.richTextBox_TestResult = new System.Windows.Forms.RichTextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.richTextBox_TestResult);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(636, 408);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // richTextBox_TestResult
            // 
            this.richTextBox_TestResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox_TestResult.Location = new System.Drawing.Point(3, 16);
            this.richTextBox_TestResult.Name = "richTextBox_TestResult";
            this.richTextBox_TestResult.Size = new System.Drawing.Size(630, 389);
            this.richTextBox_TestResult.TabIndex = 0;
            this.richTextBox_TestResult.Text = "";
            // 
            // MonitorDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(636, 408);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MonitorDemo";
            this.Text = "Monitor Feature Demo ";
            this.Activated += new System.EventHandler(this.MonitorDemo_Activated);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox richTextBox_TestResult;
    }
}