namespace  SecureShare
{
    partial class ShareFileManagerInLocal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShareFileManagerInLocal));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listView_SharedFiles = new System.Windows.Forms.ListView();
            this.button_Delete = new System.Windows.Forms.Button();
            this.button_CreateShareEncryptedFile = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.listView_SharedFiles);
            this.groupBox2.Location = new System.Drawing.Point(23, 24);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(538, 235);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Shared Files  With Revoke Access Control";
            // 
            // listView_SharedFiles
            // 
            this.listView_SharedFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_SharedFiles.FullRowSelect = true;
            this.listView_SharedFiles.GridLines = true;
            this.listView_SharedFiles.Location = new System.Drawing.Point(3, 16);
            this.listView_SharedFiles.MultiSelect = false;
            this.listView_SharedFiles.Name = "listView_SharedFiles";
            this.listView_SharedFiles.Size = new System.Drawing.Size(532, 216);
            this.listView_SharedFiles.TabIndex = 0;
            this.listView_SharedFiles.UseCompatibleStateImageBehavior = false;
            this.listView_SharedFiles.View = System.Windows.Forms.View.Details;
            // 
            // button_Delete
            // 
            this.button_Delete.Location = new System.Drawing.Point(435, 281);
            this.button_Delete.Name = "button_Delete";
            this.button_Delete.Size = new System.Drawing.Size(123, 23);
            this.button_Delete.TabIndex = 19;
            this.button_Delete.Text = "Delete Shared File";
            this.button_Delete.UseVisualStyleBackColor = true;
            this.button_Delete.Click += new System.EventHandler(this.button_Delete_Click);
            // 
            // button_CreateShareEncryptedFile
            // 
            this.button_CreateShareEncryptedFile.Location = new System.Drawing.Point(26, 281);
            this.button_CreateShareEncryptedFile.Name = "button_CreateShareEncryptedFile";
            this.button_CreateShareEncryptedFile.Size = new System.Drawing.Size(127, 23);
            this.button_CreateShareEncryptedFile.TabIndex = 18;
            this.button_CreateShareEncryptedFile.Text = "Create New Share File";
            this.button_CreateShareEncryptedFile.UseVisualStyleBackColor = true;
            this.button_CreateShareEncryptedFile.Click += new System.EventHandler(this.button_CreateShareEncryptedFile_Click);
            // 
            // ShareFilesManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 350);
            this.Controls.Add(this.button_Delete);
            this.Controls.Add(this.button_CreateShareEncryptedFile);
            this.Controls.Add(this.groupBox2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ShareFilesManagerForm";
            this.Text = "Share File Manager";
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView listView_SharedFiles;
        private System.Windows.Forms.Button button_Delete;
        private System.Windows.Forms.Button button_CreateShareEncryptedFile;
    }
}