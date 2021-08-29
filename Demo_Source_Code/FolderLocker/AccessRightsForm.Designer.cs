namespace EaseFilter.FolderLocker
{
    partial class Form_AccessRights
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_AccessRights));
            this.button_Apply = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox_AllowReadEncryptedFiles = new System.Windows.Forms.CheckBox();
            this.checkBox_AllowRemoteAccess = new System.Windows.Forms.CheckBox();
            this.checkBox_AllowCopyout = new System.Windows.Forms.CheckBox();
            this.checkBox_AllowSaveAs = new System.Windows.Forms.CheckBox();
            this.checkBox_Execution = new System.Windows.Forms.CheckBox();
            this.checkBox_SetSecurity = new System.Windows.Forms.CheckBox();
            this.checkBox_QueryInfo = new System.Windows.Forms.CheckBox();
            this.checkBox_Read = new System.Windows.Forms.CheckBox();
            this.checkBox_QuerySecurity = new System.Windows.Forms.CheckBox();
            this.checkBox_SetInfo = new System.Windows.Forms.CheckBox();
            this.checkBox_Write = new System.Windows.Forms.CheckBox();
            this.checkBox_AllowDelete = new System.Windows.Forms.CheckBox();
            this.checkBox_AllowRename = new System.Windows.Forms.CheckBox();
            this.checkBox_Creation = new System.Windows.Forms.CheckBox();
            this.label_Name = new System.Windows.Forms.Label();
            this.textBox_AccessName = new System.Windows.Forms.TextBox();
            this.groupBox_UserName = new System.Windows.Forms.GroupBox();
            this.groupBox2.SuspendLayout();
            this.groupBox_UserName.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_Apply
            // 
            this.button_Apply.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_Apply.Location = new System.Drawing.Point(508, 331);
            this.button_Apply.Name = "button_Apply";
            this.button_Apply.TabIndex = 25;
            this.button_Apply.Text = "Apply";
            this.button_Apply.UseVisualStyleBackColor = true;
            this.button_Apply.Click += new System.EventHandler(this.button_Apply_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBox_AllowReadEncryptedFiles);
            this.groupBox2.Controls.Add(this.checkBox_AllowRemoteAccess);
            this.groupBox2.Controls.Add(this.checkBox_AllowCopyout);
            this.groupBox2.Controls.Add(this.checkBox_AllowSaveAs);
            this.groupBox2.Controls.Add(this.checkBox_Execution);
            this.groupBox2.Controls.Add(this.checkBox_SetSecurity);
            this.groupBox2.Controls.Add(this.checkBox_QueryInfo);
            this.groupBox2.Controls.Add(this.checkBox_Read);
            this.groupBox2.Controls.Add(this.checkBox_QuerySecurity);
            this.groupBox2.Controls.Add(this.checkBox_SetInfo);
            this.groupBox2.Controls.Add(this.checkBox_Write);
            this.groupBox2.Controls.Add(this.checkBox_AllowDelete);
            this.groupBox2.Controls.Add(this.checkBox_AllowRename);
            this.groupBox2.Controls.Add(this.checkBox_Creation);
            this.groupBox2.Location = new System.Drawing.Point(28, 157);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(555, 149);
            this.groupBox2.TabIndex = 76;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Acess Rights";
            // 
            // checkBox_AllowReadEncryptedFiles
            // 
            this.checkBox_AllowReadEncryptedFiles.AutoSize = true;
            this.checkBox_AllowReadEncryptedFiles.Checked = true;
            this.checkBox_AllowReadEncryptedFiles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_AllowReadEncryptedFiles.Location = new System.Drawing.Point(358, 119);
            this.checkBox_AllowReadEncryptedFiles.Name = "checkBox_AllowReadEncryptedFiles";
            this.checkBox_AllowReadEncryptedFiles.Size = new System.Drawing.Size(186, 15);
            this.checkBox_AllowReadEncryptedFiles.TabIndex = 37;
            this.checkBox_AllowReadEncryptedFiles.Text = "Allow encrpted files being read";
            this.checkBox_AllowReadEncryptedFiles.UseVisualStyleBackColor = true;
            this.checkBox_AllowReadEncryptedFiles.CheckedChanged += new System.EventHandler(this.checkBox_AllowReadEncryptedFiles_CheckedChanged);
            // 
            // checkBox_AllowRemoteAccess
            // 
            this.checkBox_AllowRemoteAccess.AutoSize = true;
            this.checkBox_AllowRemoteAccess.Checked = true;
            this.checkBox_AllowRemoteAccess.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_AllowRemoteAccess.Location = new System.Drawing.Point(9, 119);
            this.checkBox_AllowRemoteAccess.Name = "checkBox_AllowRemoteAccess";
            this.checkBox_AllowRemoteAccess.Size = new System.Drawing.Size(201, 15);
            this.checkBox_AllowRemoteAccess.TabIndex = 36;
            this.checkBox_AllowRemoteAccess.Text = "Allow file accessing from network";
            this.checkBox_AllowRemoteAccess.UseVisualStyleBackColor = true;
            this.checkBox_AllowRemoteAccess.CheckedChanged += new System.EventHandler(this.checkBox_AllowRemoteAccess_CheckedChanged);
            // 
            // checkBox_AllowCopyout
            // 
            this.checkBox_AllowCopyout.AutoSize = true;
            this.checkBox_AllowCopyout.Checked = true;
            this.checkBox_AllowCopyout.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_AllowCopyout.Location = new System.Drawing.Point(358, 96);
            this.checkBox_AllowCopyout.Name = "checkBox_AllowCopyout";
            this.checkBox_AllowCopyout.Size = new System.Drawing.Size(170, 15);
            this.checkBox_AllowCopyout.TabIndex = 35;
            this.checkBox_AllowCopyout.Text = "Allow files being copied out";
            this.checkBox_AllowCopyout.UseVisualStyleBackColor = true;
            this.checkBox_AllowCopyout.CheckedChanged += new System.EventHandler(this.checkBox_AllowCopyout_CheckedChanged);
            // 
            // checkBox_AllowSaveAs
            // 
            this.checkBox_AllowSaveAs.AutoSize = true;
            this.checkBox_AllowSaveAs.Checked = true;
            this.checkBox_AllowSaveAs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_AllowSaveAs.Location = new System.Drawing.Point(358, 74);
            this.checkBox_AllowSaveAs.Name = "checkBox_AllowSaveAs";
            this.checkBox_AllowSaveAs.Size = new System.Drawing.Size(157, 15);
            this.checkBox_AllowSaveAs.TabIndex = 34;
            this.checkBox_AllowSaveAs.Text = "Allow files being saved as";
            this.checkBox_AllowSaveAs.UseVisualStyleBackColor = true;
            this.checkBox_AllowSaveAs.CheckedChanged += new System.EventHandler(this.checkBox_AllowSaveAs_CheckedChanged);
            // 
            // checkBox_Execution
            // 
            this.checkBox_Execution.AutoSize = true;
            this.checkBox_Execution.Checked = true;
            this.checkBox_Execution.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_Execution.Location = new System.Drawing.Point(9, 97);
            this.checkBox_Execution.Name = "checkBox_Execution";
            this.checkBox_Execution.Size = new System.Drawing.Size(126, 15);
            this.checkBox_Execution.TabIndex = 30;
            this.checkBox_Execution.Text = "Allow file execution";
            this.checkBox_Execution.UseVisualStyleBackColor = true;
            this.checkBox_Execution.CheckedChanged += new System.EventHandler(this.checkBox_Execution_CheckedChanged);
            // 
            // checkBox_SetSecurity
            // 
            this.checkBox_SetSecurity.AutoSize = true;
            this.checkBox_SetSecurity.Checked = true;
            this.checkBox_SetSecurity.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_SetSecurity.Location = new System.Drawing.Point(358, 52);
            this.checkBox_SetSecurity.Name = "checkBox_SetSecurity";
            this.checkBox_SetSecurity.Size = new System.Drawing.Size(169, 15);
            this.checkBox_SetSecurity.TabIndex = 29;
            this.checkBox_SetSecurity.Text = "Allow file security changing";
            this.checkBox_SetSecurity.UseVisualStyleBackColor = true;
            this.checkBox_SetSecurity.CheckedChanged += new System.EventHandler(this.checkBox_SetSecurity_CheckedChanged);
            // 
            // checkBox_QueryInfo
            // 
            this.checkBox_QueryInfo.AutoSize = true;
            this.checkBox_QueryInfo.Checked = true;
            this.checkBox_QueryInfo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_QueryInfo.Location = new System.Drawing.Point(155, 30);
            this.checkBox_QueryInfo.Name = "checkBox_QueryInfo";
            this.checkBox_QueryInfo.Size = new System.Drawing.Size(188, 15);
            this.checkBox_QueryInfo.TabIndex = 24;
            this.checkBox_QueryInfo.Text = "Allow file information querying";
            this.checkBox_QueryInfo.UseVisualStyleBackColor = true;
            this.checkBox_QueryInfo.CheckedChanged += new System.EventHandler(this.checkBox_QueryInfo_CheckedChanged);
            // 
            // checkBox_Read
            // 
            this.checkBox_Read.AutoSize = true;
            this.checkBox_Read.Checked = true;
            this.checkBox_Read.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_Read.Location = new System.Drawing.Point(9, 30);
            this.checkBox_Read.Name = "checkBox_Read";
            this.checkBox_Read.Size = new System.Drawing.Size(115, 15);
            this.checkBox_Read.TabIndex = 26;
            this.checkBox_Read.Text = "Allow file reading";
            this.checkBox_Read.UseVisualStyleBackColor = true;
            this.checkBox_Read.CheckedChanged += new System.EventHandler(this.checkBox_Read_CheckedChanged);
            // 
            // checkBox_QuerySecurity
            // 
            this.checkBox_QuerySecurity.AutoSize = true;
            this.checkBox_QuerySecurity.Checked = true;
            this.checkBox_QuerySecurity.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_QuerySecurity.Location = new System.Drawing.Point(358, 30);
            this.checkBox_QuerySecurity.Name = "checkBox_QuerySecurity";
            this.checkBox_QuerySecurity.Size = new System.Drawing.Size(166, 15);
            this.checkBox_QuerySecurity.TabIndex = 25;
            this.checkBox_QuerySecurity.Text = "Allow file security querying";
            this.checkBox_QuerySecurity.UseVisualStyleBackColor = true;
            this.checkBox_QuerySecurity.CheckedChanged += new System.EventHandler(this.checkBox_QuerySecurity_CheckedChanged);
            // 
            // checkBox_SetInfo
            // 
            this.checkBox_SetInfo.AutoSize = true;
            this.checkBox_SetInfo.Checked = true;
            this.checkBox_SetInfo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_SetInfo.Location = new System.Drawing.Point(155, 52);
            this.checkBox_SetInfo.Name = "checkBox_SetInfo";
            this.checkBox_SetInfo.Size = new System.Drawing.Size(191, 15);
            this.checkBox_SetInfo.TabIndex = 28;
            this.checkBox_SetInfo.Text = "Allow file information changing";
            this.checkBox_SetInfo.UseVisualStyleBackColor = true;
            this.checkBox_SetInfo.CheckedChanged += new System.EventHandler(this.checkBox_SetInfo_CheckedChanged);
            // 
            // checkBox_Write
            // 
            this.checkBox_Write.AutoSize = true;
            this.checkBox_Write.Checked = true;
            this.checkBox_Write.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_Write.Location = new System.Drawing.Point(9, 53);
            this.checkBox_Write.Name = "checkBox_Write";
            this.checkBox_Write.Size = new System.Drawing.Size(112, 15);
            this.checkBox_Write.TabIndex = 15;
            this.checkBox_Write.Text = "Allow file writing";
            this.checkBox_Write.UseVisualStyleBackColor = true;
            this.checkBox_Write.CheckedChanged += new System.EventHandler(this.checkBox_Write_CheckedChanged);
            // 
            // checkBox_AllowDelete
            // 
            this.checkBox_AllowDelete.AutoSize = true;
            this.checkBox_AllowDelete.Checked = true;
            this.checkBox_AllowDelete.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_AllowDelete.Location = new System.Drawing.Point(155, 97);
            this.checkBox_AllowDelete.Name = "checkBox_AllowDelete";
            this.checkBox_AllowDelete.Size = new System.Drawing.Size(118, 15);
            this.checkBox_AllowDelete.TabIndex = 17;
            this.checkBox_AllowDelete.Text = "Allow file deletion";
            this.checkBox_AllowDelete.UseVisualStyleBackColor = true;
            this.checkBox_AllowDelete.CheckedChanged += new System.EventHandler(this.checkBox_AllowDelete_CheckedChanged);
            // 
            // checkBox_AllowRename
            // 
            this.checkBox_AllowRename.AutoSize = true;
            this.checkBox_AllowRename.Checked = true;
            this.checkBox_AllowRename.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_AllowRename.Location = new System.Drawing.Point(155, 75);
            this.checkBox_AllowRename.Name = "checkBox_AllowRename";
            this.checkBox_AllowRename.Size = new System.Drawing.Size(126, 15);
            this.checkBox_AllowRename.TabIndex = 16;
            this.checkBox_AllowRename.Text = "Allow file renaming";
            this.checkBox_AllowRename.UseVisualStyleBackColor = true;
            this.checkBox_AllowRename.CheckedChanged += new System.EventHandler(this.checkBox_AllowRename_CheckedChanged);
            // 
            // checkBox_Creation
            // 
            this.checkBox_Creation.AutoSize = true;
            this.checkBox_Creation.Checked = true;
            this.checkBox_Creation.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_Creation.Location = new System.Drawing.Point(9, 75);
            this.checkBox_Creation.Name = "checkBox_Creation";
            this.checkBox_Creation.Size = new System.Drawing.Size(143, 15);
            this.checkBox_Creation.TabIndex = 22;
            this.checkBox_Creation.Text = "Allow new file creation";
            this.checkBox_Creation.UseVisualStyleBackColor = true;
            this.checkBox_Creation.CheckedChanged += new System.EventHandler(this.checkBox_Creation_CheckedChanged);
            // 
            // label_Name
            // 
            this.label_Name.AutoSize = true;
            this.label_Name.Location = new System.Drawing.Point(9, 31);
            this.label_Name.Name = "label_Name";
            this.label_Name.Size = new System.Drawing.Size(75, 19);
            this.label_Name.TabIndex = 28;
            this.label_Name.Text = "User Name";
            // 
            // textBox_AccessName
            // 
            // 
            // 
            // 
            
            this.textBox_AccessName.Lines = new string[0];
            this.textBox_AccessName.Location = new System.Drawing.Point(149, 28);
            this.textBox_AccessName.MaxLength = 32767;
            this.textBox_AccessName.Name = "textBox_AccessName";
            this.textBox_AccessName.PasswordChar = '\0';
            this.textBox_AccessName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBox_AccessName.SelectedText = "";
            this.textBox_AccessName.SelectionLength = 0;
            this.textBox_AccessName.SelectionStart = 0;
            this.textBox_AccessName.ShortcutsEnabled = true;
            this.textBox_AccessName.Size = new System.Drawing.Size(332, 20);
            this.textBox_AccessName.TabIndex = 27;
           // 
            // groupBox_UserName
            // 
            this.groupBox_UserName.Controls.Add(this.textBox_AccessName);
            this.groupBox_UserName.Controls.Add(this.label_Name);
            this.groupBox_UserName.Location = new System.Drawing.Point(28, 74);
            this.groupBox_UserName.Name = "groupBox_UserName";
            this.groupBox_UserName.Size = new System.Drawing.Size(555, 71);
            this.groupBox_UserName.TabIndex = 30;
            this.groupBox_UserName.TabStop = false;
            // 
            // Form_AccessRights
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(617, 411);
            this.Controls.Add(this.groupBox_UserName);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.button_Apply);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_AccessRights";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configure Access Rights";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox_UserName.ResumeLayout(false);
            this.groupBox_UserName.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_Apply;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBox_Write;
        private System.Windows.Forms.CheckBox checkBox_AllowDelete;
        private System.Windows.Forms.CheckBox checkBox_AllowRename;
        private System.Windows.Forms.CheckBox checkBox_Creation;
        private System.Windows.Forms.CheckBox checkBox_SetSecurity;
        private System.Windows.Forms.CheckBox checkBox_QueryInfo;
        private System.Windows.Forms.CheckBox checkBox_Read;
        private System.Windows.Forms.CheckBox checkBox_QuerySecurity;
        private System.Windows.Forms.CheckBox checkBox_SetInfo;
        private System.Windows.Forms.CheckBox checkBox_Execution;
        private System.Windows.Forms.CheckBox checkBox_AllowCopyout;
        private System.Windows.Forms.CheckBox checkBox_AllowSaveAs;
        private System.Windows.Forms.CheckBox checkBox_AllowRemoteAccess;
        private System.Windows.Forms.CheckBox checkBox_AllowReadEncryptedFiles;
        private   System.Windows.Forms.Label label_Name;
        private System.Windows.Forms.TextBox textBox_AccessName;
        private System.Windows.Forms.GroupBox groupBox_UserName;
    }
}