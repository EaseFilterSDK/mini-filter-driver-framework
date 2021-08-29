namespace FileProtector
{
    partial class TutorialForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TutorialForm));
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage_FilterRule = new System.Windows.Forms.TabPage();
            this.tabPage_GlobalSettings = new System.Windows.Forms.TabPage();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.tabPage_AccessControl = new System.Windows.Forms.TabPage();
            this.richTextBox3 = new System.Windows.Forms.RichTextBox();
            this.tabPage_Encryption = new System.Windows.Forms.TabPage();
            this.richTextBox4 = new System.Windows.Forms.RichTextBox();
            this.tabPage_Example = new System.Windows.Forms.TabPage();
            this.richTextBox5 = new System.Windows.Forms.RichTextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage_FilterRule.SuspendLayout();
            this.tabPage_GlobalSettings.SuspendLayout();
            this.tabPage_AccessControl.SuspendLayout();
            this.tabPage_Encryption.SuspendLayout();
            this.tabPage_Example.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Font = new System.Drawing.Font("Arial", 9F);
            this.richTextBox1.Location = new System.Drawing.Point(3, 3);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(596, 575);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage_FilterRule);
            this.tabControl1.Controls.Add(this.tabPage_GlobalSettings);
            this.tabControl1.Controls.Add(this.tabPage_AccessControl);
            this.tabControl1.Controls.Add(this.tabPage_Encryption);
            this.tabControl1.Controls.Add(this.tabPage_Example);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(610, 607);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage_FilterRule
            // 
            this.tabPage_FilterRule.Controls.Add(this.richTextBox1);
            this.tabPage_FilterRule.Location = new System.Drawing.Point(4, 22);
            this.tabPage_FilterRule.Name = "tabPage_FilterRule";
            this.tabPage_FilterRule.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_FilterRule.Size = new System.Drawing.Size(602, 581);
            this.tabPage_FilterRule.TabIndex = 0;
            this.tabPage_FilterRule.Text = "Filter rule settings";
            this.tabPage_FilterRule.UseVisualStyleBackColor = true;
            // 
            // tabPage_GlobalSettings
            // 
            this.tabPage_GlobalSettings.Controls.Add(this.richTextBox2);
            this.tabPage_GlobalSettings.Location = new System.Drawing.Point(4, 22);
            this.tabPage_GlobalSettings.Name = "tabPage_GlobalSettings";
            this.tabPage_GlobalSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_GlobalSettings.Size = new System.Drawing.Size(602, 581);
            this.tabPage_GlobalSettings.TabIndex = 1;
            this.tabPage_GlobalSettings.Text = "Global settings";
            this.tabPage_GlobalSettings.UseVisualStyleBackColor = true;
            // 
            // richTextBox2
            // 
            this.richTextBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox2.Location = new System.Drawing.Point(3, 3);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(596, 575);
            this.richTextBox2.TabIndex = 1;
            this.richTextBox2.Text = resources.GetString("richTextBox2.Text");
            // 
            // tabPage_AccessControl
            // 
            this.tabPage_AccessControl.Controls.Add(this.richTextBox3);
            this.tabPage_AccessControl.Location = new System.Drawing.Point(4, 22);
            this.tabPage_AccessControl.Name = "tabPage_AccessControl";
            this.tabPage_AccessControl.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_AccessControl.Size = new System.Drawing.Size(602, 581);
            this.tabPage_AccessControl.TabIndex = 2;
            this.tabPage_AccessControl.Text = "Access control";
            this.tabPage_AccessControl.UseVisualStyleBackColor = true;
            // 
            // richTextBox3
            // 
            this.richTextBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox3.Location = new System.Drawing.Point(3, 3);
            this.richTextBox3.Name = "richTextBox3";
            this.richTextBox3.Size = new System.Drawing.Size(596, 575);
            this.richTextBox3.TabIndex = 2;
            this.richTextBox3.Text = resources.GetString("richTextBox3.Text");
            // 
            // tabPage_Encryption
            // 
            this.tabPage_Encryption.Controls.Add(this.richTextBox4);
            this.tabPage_Encryption.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Encryption.Name = "tabPage_Encryption";
            this.tabPage_Encryption.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Encryption.Size = new System.Drawing.Size(602, 581);
            this.tabPage_Encryption.TabIndex = 3;
            this.tabPage_Encryption.Text = "File encryption";
            this.tabPage_Encryption.UseVisualStyleBackColor = true;
            // 
            // richTextBox4
            // 
            this.richTextBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox4.Location = new System.Drawing.Point(3, 3);
            this.richTextBox4.Name = "richTextBox4";
            this.richTextBox4.Size = new System.Drawing.Size(596, 575);
            this.richTextBox4.TabIndex = 2;
            this.richTextBox4.Text = resources.GetString("richTextBox4.Text");
            // 
            // tabPage_Example
            // 
            this.tabPage_Example.Controls.Add(this.richTextBox5);
            this.tabPage_Example.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Example.Name = "tabPage_Example";
            this.tabPage_Example.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Example.Size = new System.Drawing.Size(602, 581);
            this.tabPage_Example.TabIndex = 4;
            this.tabPage_Example.Text = "Data protection examples";
            this.tabPage_Example.UseVisualStyleBackColor = true;
            // 
            // richTextBox5
            // 
            this.richTextBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox5.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox5.Location = new System.Drawing.Point(3, 3);
            this.richTextBox5.Name = "richTextBox5";
            this.richTextBox5.Size = new System.Drawing.Size(596, 575);
            this.richTextBox5.TabIndex = 2;
            this.richTextBox5.Text = resources.GetString("richTextBox5.Text");
            // 
            // TutorialForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(610, 607);
            this.Controls.Add(this.tabControl1);
            this.Name = "TutorialForm";
            this.Text = "File Protector Tutorial";
            this.tabControl1.ResumeLayout(false);
            this.tabPage_FilterRule.ResumeLayout(false);
            this.tabPage_GlobalSettings.ResumeLayout(false);
            this.tabPage_AccessControl.ResumeLayout(false);
            this.tabPage_Encryption.ResumeLayout(false);
            this.tabPage_Example.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage_FilterRule;
        private System.Windows.Forms.TabPage tabPage_GlobalSettings;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.TabPage tabPage_AccessControl;
        private System.Windows.Forms.RichTextBox richTextBox3;
        private System.Windows.Forms.TabPage tabPage_Encryption;
        private System.Windows.Forms.RichTextBox richTextBox4;
        private System.Windows.Forms.TabPage tabPage_Example;
        private System.Windows.Forms.RichTextBox richTextBox5;
    }
}