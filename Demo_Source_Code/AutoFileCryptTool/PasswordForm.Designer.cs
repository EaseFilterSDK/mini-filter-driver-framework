namespace AutoFileCryptTool
{
    partial class PasswordForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PasswordForm));
            this.label_InputPrompt = new System.Windows.Forms.Label();
            this.textBox_PassPhrase = new System.Windows.Forms.TextBox();
            this.button_Apply = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // label_InputPrompt
            // 
            this.label_InputPrompt.AutoSize = true;
            this.label_InputPrompt.Location = new System.Drawing.Point(20, 8);
            this.label_InputPrompt.Name = "label_InputPrompt";
            this.label_InputPrompt.Size = new System.Drawing.Size(214, 13);
            this.label_InputPrompt.TabIndex = 0;
            this.label_InputPrompt.Text = "Set up the passphrase for the file encryption";
            // 
            // textBox_PassPhrase
            // 
            this.textBox_PassPhrase.Location = new System.Drawing.Point(23, 33);
            this.textBox_PassPhrase.Name = "textBox_PassPhrase";
            this.textBox_PassPhrase.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.textBox_PassPhrase.Size = new System.Drawing.Size(446, 20);
            this.textBox_PassPhrase.TabIndex = 1;
            this.textBox_PassPhrase.Text = "password";
            // 
            // button_Apply
            // 
            this.button_Apply.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_Apply.Location = new System.Drawing.Point(394, 81);
            this.button_Apply.Name = "button_Apply";
            this.button_Apply.Size = new System.Drawing.Size(75, 20);
            this.button_Apply.TabIndex = 2;
            this.button_Apply.Text = "Apply";
            this.button_Apply.UseVisualStyleBackColor = true;
            this.button_Apply.Click += new System.EventHandler(this.button_Ok_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // PasswordForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 113);
            this.Controls.Add(this.button_Apply);
            this.Controls.Add(this.textBox_PassPhrase);
            this.Controls.Add(this.label_InputPrompt);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PasswordForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Password Setup ";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_InputPrompt;
        private System.Windows.Forms.TextBox textBox_PassPhrase;
        private System.Windows.Forms.Button button_Apply;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}