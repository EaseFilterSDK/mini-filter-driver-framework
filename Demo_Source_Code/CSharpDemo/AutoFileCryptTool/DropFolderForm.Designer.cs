namespace AutoFileCryptTool
{
    partial class DropFolderForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BlackListForm));
            this.label_InputPrompt = new System.Windows.Forms.Label();
            this.textBox_DropFolder = new System.Windows.Forms.TextBox();
            this.button_Apply = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label_InputPrompt
            // 
            this.label_InputPrompt.AutoSize = true;
            this.label_InputPrompt.Location = new System.Drawing.Point(20, 8);
            this.label_InputPrompt.Name = "label_InputPrompt";
            this.label_InputPrompt.Size = new System.Drawing.Size(154, 13);
            this.label_InputPrompt.TabIndex = 0;
            this.label_InputPrompt.Text = "Copy the encrypted file to here ";
            // 
            // textBox_DropFolder
            // 
            this.textBox_DropFolder.Location = new System.Drawing.Point(23, 33);
            this.textBox_DropFolder.Name = "textBox_DropFolder";
            this.textBox_DropFolder.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.textBox_DropFolder.Size = new System.Drawing.Size(446, 20);
            this.textBox_DropFolder.TabIndex = 1;
            this.textBox_DropFolder.Text = "c:\\EaseFilter\\dropFolder";
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label1.Location = new System.Drawing.Point(20, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(302, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "The encrypted files in this folder will be decrypted automatically";
            // 
            // BlackListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 113);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_Apply);
            this.Controls.Add(this.textBox_DropFolder);
            this.Controls.Add(this.label_InputPrompt);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BlackListForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Setup the drop folder ";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_InputPrompt;
        private System.Windows.Forms.TextBox textBox_DropFolder;
        private System.Windows.Forms.Button button_Apply;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label1;
    }
}