
namespace EaseFilter.FolderLocker
{
    partial class FolderLockerSettigs
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FolderLockerSettigs));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox_AccessControl = new System.Windows.Forms.GroupBox();
            this.button_RemoveRights = new System.Windows.Forms.Button();
            this.button_AddProcessRights = new System.Windows.Forms.Button();
            this.button_AddUserRights = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.listView_AccessRights = new System.Windows.Forms.ListView();
            this.button_SaveSettings = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox_AllowCopyout = new System.Windows.Forms.CheckBox();
            this.checkBox_Encryption = new System.Windows.Forms.CheckBox();
            this.checkBox_HideFiles = new System.Windows.Forms.CheckBox();
            this.checkBox_AllowRead = new System.Windows.Forms.CheckBox();
            this.checkBox_AllowSetSecurity = new System.Windows.Forms.CheckBox();
            this.checkBox_AllowWrite = new System.Windows.Forms.CheckBox();
            this.checkBox_AllowDelete = new System.Windows.Forms.CheckBox();
            this.checkBox_AllowRename = new System.Windows.Forms.CheckBox();
            this.checkBox_AllowRemoteAccess = new System.Windows.Forms.CheckBox();
            this.checkBox_AllowNewFileCreation = new System.Windows.Forms.CheckBox();
            this.textBox_PassPhrase = new System.Windows.Forms.TextBox();
            this.label_PassPhrase = new System.Windows.Forms.Label();
            this.label_FolderName = new System.Windows.Forms.Label();
            this.textBox_FolderName = new System.Windows.Forms.TextBox();
            this.button_BrowseFolder = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox_AccessControl.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox_AccessControl);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(554, 499);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            // 
            // groupBox_AccessControl
            // 
            this.groupBox_AccessControl.Controls.Add(this.button_RemoveRights);
            this.groupBox_AccessControl.Controls.Add(this.button_AddProcessRights);
            this.groupBox_AccessControl.Controls.Add(this.button_AddUserRights);
            this.groupBox_AccessControl.Controls.Add(this.groupBox3);
            this.groupBox_AccessControl.Controls.Add(this.button_SaveSettings);
            this.groupBox_AccessControl.Controls.Add(this.groupBox2);
            this.groupBox_AccessControl.Controls.Add(this.label_FolderName);
            this.groupBox_AccessControl.Controls.Add(this.textBox_FolderName);
            this.groupBox_AccessControl.Controls.Add(this.button_BrowseFolder);
            this.groupBox_AccessControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_AccessControl.Location = new System.Drawing.Point(3, 16);
            this.groupBox_AccessControl.Name = "groupBox_AccessControl";
            this.groupBox_AccessControl.Size = new System.Drawing.Size(548, 480);
            this.groupBox_AccessControl.TabIndex = 24;
            this.groupBox_AccessControl.TabStop = false;
            // 
            // button_RemoveRights
            // 
            this.button_RemoveRights.Location = new System.Drawing.Point(248, 422);
            this.button_RemoveRights.Margin = new System.Windows.Forms.Padding(2);
            this.button_RemoveRights.Name = "button_RemoveRights";
            this.button_RemoveRights.Size = new System.Drawing.Size(131, 22);
            this.button_RemoveRights.TabIndex = 89;
            this.button_RemoveRights.Text = "Remove Access Right";
            this.button_RemoveRights.UseVisualStyleBackColor = true;
            this.button_RemoveRights.Click += new System.EventHandler(this.button_RemoveRights_Click);
            // 
            // button_AddProcessRights
            // 
            this.button_AddProcessRights.Location = new System.Drawing.Point(121, 422);
            this.button_AddProcessRights.Margin = new System.Windows.Forms.Padding(2);
            this.button_AddProcessRights.Name = "button_AddProcessRights";
            this.button_AddProcessRights.Size = new System.Drawing.Size(110, 22);
            this.button_AddProcessRights.TabIndex = 88;
            this.button_AddProcessRights.Text = "Add Process Rights";
            this.button_AddProcessRights.UseVisualStyleBackColor = true;
            this.button_AddProcessRights.Click += new System.EventHandler(this.button_AddProcessRights_Click);
            // 
            // button_AddUserRights
            // 
            this.button_AddUserRights.Location = new System.Drawing.Point(9, 422);
            this.button_AddUserRights.Margin = new System.Windows.Forms.Padding(2);
            this.button_AddUserRights.Name = "button_AddUserRights";
            this.button_AddUserRights.Size = new System.Drawing.Size(96, 22);
            this.button_AddUserRights.TabIndex = 87;
            this.button_AddUserRights.Text = "Add User Rights";
            this.button_AddUserRights.UseVisualStyleBackColor = true;
            this.button_AddUserRights.Click += new System.EventHandler(this.button_AddUserRights_Click_1);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.listView_AccessRights);
            this.groupBox3.Location = new System.Drawing.Point(6, 213);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(517, 196);
            this.groupBox3.TabIndex = 86;
            this.groupBox3.TabStop = false;
            // 
            // listView_AccessRights
            // 
            this.listView_AccessRights.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_AccessRights.FullRowSelect = true;
            this.listView_AccessRights.HideSelection = false;
            this.listView_AccessRights.HoverSelection = true;
            this.listView_AccessRights.Location = new System.Drawing.Point(3, 16);
            this.listView_AccessRights.MultiSelect = false;
            this.listView_AccessRights.Name = "listView_AccessRights";
            this.listView_AccessRights.ShowItemToolTips = true;
            this.listView_AccessRights.Size = new System.Drawing.Size(511, 177);
            this.listView_AccessRights.TabIndex = 4;
            this.listView_AccessRights.UseCompatibleStateImageBehavior = false;
            this.listView_AccessRights.View = System.Windows.Forms.View.Details;
            // 
            // button_SaveSettings
            // 
            this.button_SaveSettings.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_SaveSettings.Location = new System.Drawing.Point(402, 422);
            this.button_SaveSettings.Margin = new System.Windows.Forms.Padding(2);
            this.button_SaveSettings.Name = "button_SaveSettings";
            this.button_SaveSettings.Size = new System.Drawing.Size(118, 22);
            this.button_SaveSettings.TabIndex = 85;
            this.button_SaveSettings.Text = "Save Settings";
            this.button_SaveSettings.UseVisualStyleBackColor = true;
            this.button_SaveSettings.Click += new System.EventHandler(this.button_SaveControlSettings_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBox_AllowCopyout);
            this.groupBox2.Controls.Add(this.checkBox_Encryption);
            this.groupBox2.Controls.Add(this.checkBox_HideFiles);
            this.groupBox2.Controls.Add(this.checkBox_AllowRead);
            this.groupBox2.Controls.Add(this.checkBox_AllowSetSecurity);
            this.groupBox2.Controls.Add(this.checkBox_AllowWrite);
            this.groupBox2.Controls.Add(this.checkBox_AllowDelete);
            this.groupBox2.Controls.Add(this.checkBox_AllowRename);
            this.groupBox2.Controls.Add(this.checkBox_AllowRemoteAccess);
            this.groupBox2.Controls.Add(this.checkBox_AllowNewFileCreation);
            this.groupBox2.Controls.Add(this.textBox_PassPhrase);
            this.groupBox2.Controls.Add(this.label_PassPhrase);
            this.groupBox2.Location = new System.Drawing.Point(6, 51);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(517, 156);
            this.groupBox2.TabIndex = 76;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Folder Access Control Settings";
            // 
            // checkBox_AllowCopyout
            // 
            this.checkBox_AllowCopyout.AutoSize = true;
            this.checkBox_AllowCopyout.Location = new System.Drawing.Point(7, 68);
            this.checkBox_AllowCopyout.Name = "checkBox_AllowCopyout";
            this.checkBox_AllowCopyout.Size = new System.Drawing.Size(154, 17);
            this.checkBox_AllowCopyout.TabIndex = 36;
            this.checkBox_AllowCopyout.Text = "Allow files being copied out";
            this.checkBox_AllowCopyout.UseVisualStyleBackColor = true;
            this.checkBox_AllowCopyout.CheckedChanged += new System.EventHandler(this.checkBox_AllowCopyout_CheckedChanged_1);
            // 
            // checkBox_Encryption
            // 
            this.checkBox_Encryption.AutoSize = true;
            this.checkBox_Encryption.Location = new System.Drawing.Point(346, 55);
            this.checkBox_Encryption.Name = "checkBox_Encryption";
            this.checkBox_Encryption.Size = new System.Drawing.Size(127, 17);
            this.checkBox_Encryption.TabIndex = 28;
            this.checkBox_Encryption.Text = "Enable file encryption";
            this.checkBox_Encryption.UseVisualStyleBackColor = true;
            this.checkBox_Encryption.CheckedChanged += new System.EventHandler(this.checkBox_EnableEncryption_CheckedChanged);
            // 
            // checkBox_HideFiles
            // 
            this.checkBox_HideFiles.AutoSize = true;
            this.checkBox_HideFiles.Location = new System.Drawing.Point(346, 32);
            this.checkBox_HideFiles.Name = "checkBox_HideFiles";
            this.checkBox_HideFiles.Size = new System.Drawing.Size(109, 17);
            this.checkBox_HideFiles.TabIndex = 26;
            this.checkBox_HideFiles.Text = "Hide files in folder";
            this.checkBox_HideFiles.UseVisualStyleBackColor = true;
            this.checkBox_HideFiles.CheckedChanged += new System.EventHandler(this.checkBox_HideFiles_CheckedChanged);
            // 
            // checkBox_AllowRead
            // 
            this.checkBox_AllowRead.AutoSize = true;
            this.checkBox_AllowRead.Location = new System.Drawing.Point(191, 88);
            this.checkBox_AllowRead.Name = "checkBox_AllowRead";
            this.checkBox_AllowRead.Size = new System.Drawing.Size(110, 17);
            this.checkBox_AllowRead.TabIndex = 27;
            this.checkBox_AllowRead.Text = "Allow files reading";
            this.checkBox_AllowRead.UseVisualStyleBackColor = true;
            this.checkBox_AllowRead.CheckedChanged += new System.EventHandler(this.checkBox_AllowRead_CheckedChanged);
            // 
            // checkBox_AllowSetSecurity
            // 
            this.checkBox_AllowSetSecurity.AutoSize = true;
            this.checkBox_AllowSetSecurity.Location = new System.Drawing.Point(191, 68);
            this.checkBox_AllowSetSecurity.Name = "checkBox_AllowSetSecurity";
            this.checkBox_AllowSetSecurity.Size = new System.Drawing.Size(129, 17);
            this.checkBox_AllowSetSecurity.TabIndex = 25;
            this.checkBox_AllowSetSecurity.Text = "Allow security change";
            this.checkBox_AllowSetSecurity.UseVisualStyleBackColor = true;
            this.checkBox_AllowSetSecurity.CheckedChanged += new System.EventHandler(this.checkBox_AllowSetSecurity_CheckedChanged);
            // 
            // checkBox_AllowWrite
            // 
            this.checkBox_AllowWrite.AutoSize = true;
            this.checkBox_AllowWrite.Location = new System.Drawing.Point(191, 32);
            this.checkBox_AllowWrite.Name = "checkBox_AllowWrite";
            this.checkBox_AllowWrite.Size = new System.Drawing.Size(100, 17);
            this.checkBox_AllowWrite.TabIndex = 15;
            this.checkBox_AllowWrite.Text = "Allow file writing";
            this.checkBox_AllowWrite.UseVisualStyleBackColor = true;
            this.checkBox_AllowWrite.CheckedChanged += new System.EventHandler(this.checkBox_AllowWrite_CheckedChanged);
            // 
            // checkBox_AllowDelete
            // 
            this.checkBox_AllowDelete.AutoSize = true;
            this.checkBox_AllowDelete.Location = new System.Drawing.Point(7, 32);
            this.checkBox_AllowDelete.Name = "checkBox_AllowDelete";
            this.checkBox_AllowDelete.Size = new System.Drawing.Size(107, 17);
            this.checkBox_AllowDelete.TabIndex = 17;
            this.checkBox_AllowDelete.Text = "Allow file deletion";
            this.checkBox_AllowDelete.UseVisualStyleBackColor = true;
            this.checkBox_AllowDelete.CheckedChanged += new System.EventHandler(this.checkBox_AllowDelete_CheckedChanged);
            // 
            // checkBox_AllowRename
            // 
            this.checkBox_AllowRename.AutoSize = true;
            this.checkBox_AllowRename.Location = new System.Drawing.Point(7, 49);
            this.checkBox_AllowRename.Name = "checkBox_AllowRename";
            this.checkBox_AllowRename.Size = new System.Drawing.Size(113, 17);
            this.checkBox_AllowRename.TabIndex = 16;
            this.checkBox_AllowRename.Text = "Allow file renaming";
            this.checkBox_AllowRename.UseVisualStyleBackColor = true;
            this.checkBox_AllowRename.CheckedChanged += new System.EventHandler(this.checkBox_AllowRename_CheckedChanged);
            // 
            // checkBox_AllowRemoteAccess
            // 
            this.checkBox_AllowRemoteAccess.AutoSize = true;
            this.checkBox_AllowRemoteAccess.Location = new System.Drawing.Point(6, 88);
            this.checkBox_AllowRemoteAccess.Name = "checkBox_AllowRemoteAccess";
            this.checkBox_AllowRemoteAccess.Size = new System.Drawing.Size(182, 17);
            this.checkBox_AllowRemoteAccess.TabIndex = 21;
            this.checkBox_AllowRemoteAccess.Text = "Allow file accessing from network";
            this.checkBox_AllowRemoteAccess.UseVisualStyleBackColor = true;
            this.checkBox_AllowRemoteAccess.CheckedChanged += new System.EventHandler(this.checkBox_AllowRemoteAccess_CheckedChanged);
            // 
            // checkBox_AllowNewFileCreation
            // 
            this.checkBox_AllowNewFileCreation.AutoSize = true;
            this.checkBox_AllowNewFileCreation.Location = new System.Drawing.Point(191, 49);
            this.checkBox_AllowNewFileCreation.Name = "checkBox_AllowNewFileCreation";
            this.checkBox_AllowNewFileCreation.Size = new System.Drawing.Size(131, 17);
            this.checkBox_AllowNewFileCreation.TabIndex = 22;
            this.checkBox_AllowNewFileCreation.Text = "Allow new file creation";
            this.checkBox_AllowNewFileCreation.UseVisualStyleBackColor = true;
            this.checkBox_AllowNewFileCreation.CheckedChanged += new System.EventHandler(this.checkBox_AllowNewFileCreation_CheckedChanged);
            // 
            // textBox_PassPhrase
            // 
            this.textBox_PassPhrase.Location = new System.Drawing.Point(346, 100);
            this.textBox_PassPhrase.Name = "textBox_PassPhrase";
            this.textBox_PassPhrase.PasswordChar = '●';
            this.textBox_PassPhrase.ReadOnly = true;
            this.textBox_PassPhrase.Size = new System.Drawing.Size(100, 20);
            this.textBox_PassPhrase.TabIndex = 19;
            this.textBox_PassPhrase.UseSystemPasswordChar = true;
            // 
            // label_PassPhrase
            // 
            this.label_PassPhrase.AutoSize = true;
            this.label_PassPhrase.Location = new System.Drawing.Point(344, 76);
            this.label_PassPhrase.Name = "label_PassPhrase";
            this.label_PassPhrase.Size = new System.Drawing.Size(88, 13);
            this.label_PassPhrase.TabIndex = 20;
            this.label_PassPhrase.Text = "Password phrase";
            // 
            // label_FolderName
            // 
            this.label_FolderName.AutoSize = true;
            this.label_FolderName.Location = new System.Drawing.Point(10, 15);
            this.label_FolderName.Name = "label_FolderName";
            this.label_FolderName.Size = new System.Drawing.Size(67, 13);
            this.label_FolderName.TabIndex = 12;
            this.label_FolderName.Text = "Folder Name";
            // 
            // textBox_FolderName
            // 
            this.textBox_FolderName.Location = new System.Drawing.Point(196, 20);
            this.textBox_FolderName.Name = "textBox_FolderName";
            this.textBox_FolderName.Size = new System.Drawing.Size(265, 20);
            this.textBox_FolderName.TabIndex = 11;
            this.textBox_FolderName.Text = "c:\\FolderLocker";
            // 
            // button_BrowseFolder
            // 
            this.button_BrowseFolder.Location = new System.Drawing.Point(482, 20);
            this.button_BrowseFolder.Name = "button_BrowseFolder";
            this.button_BrowseFolder.Size = new System.Drawing.Size(41, 20);
            this.button_BrowseFolder.TabIndex = 14;
            this.button_BrowseFolder.Text = "...";
            this.button_BrowseFolder.UseVisualStyleBackColor = true;
            this.button_BrowseFolder.Click += new System.EventHandler(this.button_BrowseFolder_Click);
            // 
            // FolderLockerSettigs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 499);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FolderLockerSettigs";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Folder Locker Settings";
            this.Load += new System.EventHandler(this.FolderLockerSettigs_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox_AccessControl.ResumeLayout(false);
            this.groupBox_AccessControl.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
         
      
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox_AccessControl;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBox_AllowSetSecurity;
        private System.Windows.Forms.CheckBox checkBox_AllowWrite;
        private System.Windows.Forms.CheckBox checkBox_AllowDelete;
        private System.Windows.Forms.CheckBox checkBox_AllowRename;
        private System.Windows.Forms.CheckBox checkBox_AllowRemoteAccess;
        private System.Windows.Forms.CheckBox checkBox_AllowNewFileCreation;
        private System.Windows.Forms.Label label_PassPhrase;
        private System.Windows.Forms.Label label_FolderName;
        private System.Windows.Forms.TextBox textBox_FolderName;
        private System.Windows.Forms.Button button_BrowseFolder;
        private System.Windows.Forms.TextBox textBox_PassPhrase;
        private System.Windows.Forms.Button button_SaveSettings;
        private System.Windows.Forms.CheckBox checkBox_HideFiles;
        private System.Windows.Forms.CheckBox checkBox_AllowRead;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox checkBox_Encryption;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button_RemoveRights;
        private System.Windows.Forms.Button button_AddProcessRights;
        private System.Windows.Forms.Button button_AddUserRights;
        private System.Windows.Forms.ListView listView_AccessRights;
        private System.Windows.Forms.CheckBox checkBox_AllowCopyout;
    }
}