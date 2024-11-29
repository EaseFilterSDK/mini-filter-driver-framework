namespace SecureSandbox
{
    partial class Form_ProcessRights
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_ProcessRights));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button_info = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_ProcessId = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_ProcessName = new System.Windows.Forms.TextBox();
            this.button_AddProcessName = new System.Windows.Forms.Button();
            this.checkBox_AllowReadEncryptedFiles = new System.Windows.Forms.CheckBox();
            this.checkBox_AllowCopyout = new System.Windows.Forms.CheckBox();
            this.checkBox_AllowSaveAs = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_AccessFlag = new System.Windows.Forms.TextBox();
            this.button_AccessFlag = new System.Windows.Forms.Button();
            this.checkBox_SetSecurity = new System.Windows.Forms.CheckBox();
            this.checkBox_QueryInfo = new System.Windows.Forms.CheckBox();
            this.checkBox_Read = new System.Windows.Forms.CheckBox();
            this.checkBox_QuerySecurity = new System.Windows.Forms.CheckBox();
            this.checkBox_SetInfo = new System.Windows.Forms.CheckBox();
            this.checkBox_Write = new System.Windows.Forms.CheckBox();
            this.checkBox_AllowDelete = new System.Windows.Forms.CheckBox();
            this.checkBox_AllowRename = new System.Windows.Forms.CheckBox();
            this.checkBox_Creation = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listView_ProcessRights = new System.Windows.Forms.ListView();
            this.button_Add = new System.Windows.Forms.Button();
            this.button_ApplyAll = new System.Windows.Forms.Button();
            this.button_Delete = new System.Windows.Forms.Button();
            this.button_SaveProcessRight = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button_info);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.textBox_ProcessId);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.textBox_ProcessName);
            this.groupBox2.Controls.Add(this.button_AddProcessName);
            this.groupBox2.Controls.Add(this.checkBox_AllowReadEncryptedFiles);
            this.groupBox2.Controls.Add(this.checkBox_AllowCopyout);
            this.groupBox2.Controls.Add(this.checkBox_AllowSaveAs);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.textBox_AccessFlag);
            this.groupBox2.Controls.Add(this.button_AccessFlag);
            this.groupBox2.Controls.Add(this.checkBox_SetSecurity);
            this.groupBox2.Controls.Add(this.checkBox_QueryInfo);
            this.groupBox2.Controls.Add(this.checkBox_Read);
            this.groupBox2.Controls.Add(this.checkBox_QuerySecurity);
            this.groupBox2.Controls.Add(this.checkBox_SetInfo);
            this.groupBox2.Controls.Add(this.checkBox_Write);
            this.groupBox2.Controls.Add(this.checkBox_AllowDelete);
            this.groupBox2.Controls.Add(this.checkBox_AllowRename);
            this.groupBox2.Controls.Add(this.checkBox_Creation);
            this.groupBox2.Location = new System.Drawing.Point(21, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(517, 232);
            this.groupBox2.TabIndex = 76;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Process Rights";
            // 
            // button_info
            // 
            this.button_info.Image = ((System.Drawing.Image)(resources.GetObject("button_info.Image")));
            this.button_info.Location = new System.Drawing.Point(422, 25);
            this.button_info.Name = "button_info";
            this.button_info.Size = new System.Drawing.Size(60, 33);
            this.button_info.TabIndex = 97;
            this.button_info.UseVisualStyleBackColor = true;
            this.button_info.Click += new System.EventHandler(this.button_info_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 15);
            this.label3.TabIndex = 42;
            this.label3.Text = "Process Id";
            // 
            // textBox_ProcessId
            // 
            this.textBox_ProcessId.Location = new System.Drawing.Point(152, 65);
            this.textBox_ProcessId.Name = "textBox_ProcessId";
            this.textBox_ProcessId.ReadOnly = true;
            this.textBox_ProcessId.Size = new System.Drawing.Size(242, 20);
            this.textBox_ProcessId.TabIndex = 41;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 15);
            this.label1.TabIndex = 39;
            this.label1.Text = "Process Name";
            // 
            // textBox_ProcessName
            // 
            this.textBox_ProcessName.Location = new System.Drawing.Point(152, 32);
            this.textBox_ProcessName.Name = "textBox_ProcessName";
            this.textBox_ProcessName.Size = new System.Drawing.Size(242, 20);
            this.textBox_ProcessName.TabIndex = 38;
            // 
            // button_AddProcessName
            // 
            this.button_AddProcessName.Location = new System.Drawing.Point(422, 65);
            this.button_AddProcessName.Name = "button_AddProcessName";
            this.button_AddProcessName.Size = new System.Drawing.Size(60, 20);
            this.button_AddProcessName.TabIndex = 40;
            this.button_AddProcessName.Text = "...";
            this.button_AddProcessName.UseVisualStyleBackColor = true;
            this.button_AddProcessName.Click += new System.EventHandler(this.button_AddProcessName_Click);
            // 
            // checkBox_AllowReadEncryptedFiles
            // 
            this.checkBox_AllowReadEncryptedFiles.AutoSize = true;
            this.checkBox_AllowReadEncryptedFiles.Checked = true;
            this.checkBox_AllowReadEncryptedFiles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_AllowReadEncryptedFiles.Location = new System.Drawing.Point(151, 205);
            this.checkBox_AllowReadEncryptedFiles.Name = "checkBox_AllowReadEncryptedFiles";
            this.checkBox_AllowReadEncryptedFiles.Size = new System.Drawing.Size(195, 19);
            this.checkBox_AllowReadEncryptedFiles.TabIndex = 37;
            this.checkBox_AllowReadEncryptedFiles.Text = "Allow encrypted file being read";
            this.checkBox_AllowReadEncryptedFiles.UseVisualStyleBackColor = true;
            this.checkBox_AllowReadEncryptedFiles.CheckedChanged += new System.EventHandler(this.checkBox_AllowReadEncryptedFiles_CheckedChanged);
            // 
            // checkBox_AllowCopyout
            // 
            this.checkBox_AllowCopyout.AutoSize = true;
            this.checkBox_AllowCopyout.Checked = true;
            this.checkBox_AllowCopyout.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_AllowCopyout.Location = new System.Drawing.Point(336, 205);
            this.checkBox_AllowCopyout.Name = "checkBox_AllowCopyout";
            this.checkBox_AllowCopyout.Size = new System.Drawing.Size(177, 19);
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
            this.checkBox_AllowSaveAs.Location = new System.Drawing.Point(336, 182);
            this.checkBox_AllowSaveAs.Name = "checkBox_AllowSaveAs";
            this.checkBox_AllowSaveAs.Size = new System.Drawing.Size(162, 19);
            this.checkBox_AllowSaveAs.TabIndex = 34;
            this.checkBox_AllowSaveAs.Text = "Allow file being saved as";
            this.checkBox_AllowSaveAs.UseVisualStyleBackColor = true;
            this.checkBox_AllowSaveAs.CheckedChanged += new System.EventHandler(this.checkBox_AllowSaveAs_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 15);
            this.label2.TabIndex = 32;
            this.label2.Text = "Access Control Flag";
            // 
            // textBox_AccessFlag
            // 
            this.textBox_AccessFlag.Location = new System.Drawing.Point(152, 100);
            this.textBox_AccessFlag.Name = "textBox_AccessFlag";
            this.textBox_AccessFlag.ReadOnly = true;
            this.textBox_AccessFlag.Size = new System.Drawing.Size(242, 20);
            this.textBox_AccessFlag.TabIndex = 31;
            this.textBox_AccessFlag.Text = "0";
            // 
            // button_AccessFlag
            // 
            this.button_AccessFlag.Location = new System.Drawing.Point(422, 102);
            this.button_AccessFlag.Name = "button_AccessFlag";
            this.button_AccessFlag.Size = new System.Drawing.Size(60, 20);
            this.button_AccessFlag.TabIndex = 33;
            this.button_AccessFlag.Text = "...";
            this.button_AccessFlag.UseVisualStyleBackColor = true;
            this.button_AccessFlag.Click += new System.EventHandler(this.button_AccessFlag_Click);
            // 
            // checkBox_SetSecurity
            // 
            this.checkBox_SetSecurity.AutoSize = true;
            this.checkBox_SetSecurity.Checked = true;
            this.checkBox_SetSecurity.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_SetSecurity.Location = new System.Drawing.Point(336, 159);
            this.checkBox_SetSecurity.Name = "checkBox_SetSecurity";
            this.checkBox_SetSecurity.Size = new System.Drawing.Size(156, 19);
            this.checkBox_SetSecurity.TabIndex = 29;
            this.checkBox_SetSecurity.Text = "Allow changing security";
            this.checkBox_SetSecurity.UseVisualStyleBackColor = true;
            this.checkBox_SetSecurity.CheckedChanged += new System.EventHandler(this.checkBox_SetSecurity_CheckedChanged);
            // 
            // checkBox_QueryInfo
            // 
            this.checkBox_QueryInfo.AutoSize = true;
            this.checkBox_QueryInfo.Checked = true;
            this.checkBox_QueryInfo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_QueryInfo.Location = new System.Drawing.Point(152, 137);
            this.checkBox_QueryInfo.Name = "checkBox_QueryInfo";
            this.checkBox_QueryInfo.Size = new System.Drawing.Size(173, 19);
            this.checkBox_QueryInfo.TabIndex = 24;
            this.checkBox_QueryInfo.Text = "Allow querying information";
            this.checkBox_QueryInfo.UseVisualStyleBackColor = true;
            this.checkBox_QueryInfo.CheckedChanged += new System.EventHandler(this.checkBox_QueryInfo_CheckedChanged);
            // 
            // checkBox_Read
            // 
            this.checkBox_Read.AutoSize = true;
            this.checkBox_Read.Checked = true;
            this.checkBox_Read.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_Read.Location = new System.Drawing.Point(12, 137);
            this.checkBox_Read.Name = "checkBox_Read";
            this.checkBox_Read.Size = new System.Drawing.Size(122, 19);
            this.checkBox_Read.TabIndex = 26;
            this.checkBox_Read.Text = "Allow reading file";
            this.checkBox_Read.UseVisualStyleBackColor = true;
            this.checkBox_Read.CheckedChanged += new System.EventHandler(this.checkBox_Read_CheckedChanged);
            // 
            // checkBox_QuerySecurity
            // 
            this.checkBox_QuerySecurity.AutoSize = true;
            this.checkBox_QuerySecurity.Checked = true;
            this.checkBox_QuerySecurity.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_QuerySecurity.Location = new System.Drawing.Point(336, 137);
            this.checkBox_QuerySecurity.Name = "checkBox_QuerySecurity";
            this.checkBox_QuerySecurity.Size = new System.Drawing.Size(152, 19);
            this.checkBox_QuerySecurity.TabIndex = 25;
            this.checkBox_QuerySecurity.Text = "Allow querying security";
            this.checkBox_QuerySecurity.UseVisualStyleBackColor = true;
            this.checkBox_QuerySecurity.CheckedChanged += new System.EventHandler(this.checkBox_QuerySecurity_CheckedChanged);
            // 
            // checkBox_SetInfo
            // 
            this.checkBox_SetInfo.AutoSize = true;
            this.checkBox_SetInfo.Checked = true;
            this.checkBox_SetInfo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_SetInfo.Location = new System.Drawing.Point(152, 159);
            this.checkBox_SetInfo.Name = "checkBox_SetInfo";
            this.checkBox_SetInfo.Size = new System.Drawing.Size(177, 19);
            this.checkBox_SetInfo.TabIndex = 28;
            this.checkBox_SetInfo.Text = "Allow changing information";
            this.checkBox_SetInfo.UseVisualStyleBackColor = true;
            this.checkBox_SetInfo.CheckedChanged += new System.EventHandler(this.checkBox_SetInfo_CheckedChanged);
            // 
            // checkBox_Write
            // 
            this.checkBox_Write.AutoSize = true;
            this.checkBox_Write.Checked = true;
            this.checkBox_Write.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_Write.Location = new System.Drawing.Point(12, 160);
            this.checkBox_Write.Name = "checkBox_Write";
            this.checkBox_Write.Size = new System.Drawing.Size(116, 19);
            this.checkBox_Write.TabIndex = 15;
            this.checkBox_Write.Text = "Allow writing file";
            this.checkBox_Write.UseVisualStyleBackColor = true;
            this.checkBox_Write.CheckedChanged += new System.EventHandler(this.checkBox_Write_CheckedChanged);
            // 
            // checkBox_AllowDelete
            // 
            this.checkBox_AllowDelete.AutoSize = true;
            this.checkBox_AllowDelete.Checked = true;
            this.checkBox_AllowDelete.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_AllowDelete.Location = new System.Drawing.Point(12, 205);
            this.checkBox_AllowDelete.Name = "checkBox_AllowDelete";
            this.checkBox_AllowDelete.Size = new System.Drawing.Size(124, 19);
            this.checkBox_AllowDelete.TabIndex = 17;
            this.checkBox_AllowDelete.Text = "Allow deleting file";
            this.checkBox_AllowDelete.UseVisualStyleBackColor = true;
            this.checkBox_AllowDelete.CheckedChanged += new System.EventHandler(this.checkBox_AllowDelete_CheckedChanged);
            // 
            // checkBox_AllowRename
            // 
            this.checkBox_AllowRename.AutoSize = true;
            this.checkBox_AllowRename.Checked = true;
            this.checkBox_AllowRename.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_AllowRename.Location = new System.Drawing.Point(152, 182);
            this.checkBox_AllowRename.Name = "checkBox_AllowRename";
            this.checkBox_AllowRename.Size = new System.Drawing.Size(133, 19);
            this.checkBox_AllowRename.TabIndex = 16;
            this.checkBox_AllowRename.Text = "Allow renaming file";
            this.checkBox_AllowRename.UseVisualStyleBackColor = true;
            this.checkBox_AllowRename.CheckedChanged += new System.EventHandler(this.checkBox_AllowRename_CheckedChanged);
            // 
            // checkBox_Creation
            // 
            this.checkBox_Creation.AutoSize = true;
            this.checkBox_Creation.Checked = true;
            this.checkBox_Creation.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_Creation.Location = new System.Drawing.Point(12, 182);
            this.checkBox_Creation.Name = "checkBox_Creation";
            this.checkBox_Creation.Size = new System.Drawing.Size(150, 19);
            this.checkBox_Creation.TabIndex = 22;
            this.checkBox_Creation.Text = "Allow creating new file";
            this.checkBox_Creation.UseVisualStyleBackColor = true;
            this.checkBox_Creation.CheckedChanged += new System.EventHandler(this.checkBox_Creation_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listView_ProcessRights);
            this.groupBox1.Location = new System.Drawing.Point(21, 279);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(523, 105);
            this.groupBox1.TabIndex = 77;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Process rights collection";
            // 
            // listView_ProcessRights
            // 
            this.listView_ProcessRights.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_ProcessRights.FullRowSelect = true;
            this.listView_ProcessRights.Location = new System.Drawing.Point(3, 16);
            this.listView_ProcessRights.Name = "listView_ProcessRights";
            this.listView_ProcessRights.Size = new System.Drawing.Size(517, 86);
            this.listView_ProcessRights.TabIndex = 1;
            this.listView_ProcessRights.UseCompatibleStateImageBehavior = false;
            this.listView_ProcessRights.View = System.Windows.Forms.View.Details;
            this.listView_ProcessRights.SelectedIndexChanged += new System.EventHandler(this.listView_ProcessRights_SelectedIndexChanged);
            // 
            // button_Add
            // 
            this.button_Add.Location = new System.Drawing.Point(21, 406);
            this.button_Add.Name = "button_Add";
            this.button_Add.Size = new System.Drawing.Size(122, 23);
            this.button_Add.TabIndex = 79;
            this.button_Add.Text = "Add Process Rights";
            this.button_Add.UseVisualStyleBackColor = true;
            this.button_Add.Click += new System.EventHandler(this.button_AddProcessRights_Click);
            // 
            // button_ApplyAll
            // 
            this.button_ApplyAll.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_ApplyAll.Location = new System.Drawing.Point(376, 406);
            this.button_ApplyAll.Name = "button_ApplyAll";
            this.button_ApplyAll.Size = new System.Drawing.Size(162, 23);
            this.button_ApplyAll.TabIndex = 80;
            this.button_ApplyAll.Text = "Apply Settings To Sandbox";
            this.button_ApplyAll.UseVisualStyleBackColor = true;
            this.button_ApplyAll.Click += new System.EventHandler(this.button_ApplyAll_Click);
            // 
            // button_Delete
            // 
            this.button_Delete.Location = new System.Drawing.Point(197, 406);
            this.button_Delete.Name = "button_Delete";
            this.button_Delete.Size = new System.Drawing.Size(153, 23);
            this.button_Delete.TabIndex = 78;
            this.button_Delete.Text = "Delete Process Rights";
            this.button_Delete.UseVisualStyleBackColor = true;
            this.button_Delete.Click += new System.EventHandler(this.button_Delete_Click);
            // 
            // button_SaveProcessRight
            // 
            this.button_SaveProcessRight.BackColor = System.Drawing.Color.Gold;
            this.button_SaveProcessRight.Location = new System.Drawing.Point(381, 250);
            this.button_SaveProcessRight.Name = "button_SaveProcessRight";
            this.button_SaveProcessRight.Size = new System.Drawing.Size(153, 23);
            this.button_SaveProcessRight.TabIndex = 81;
            this.button_SaveProcessRight.Text = "Save Process Right";
            this.button_SaveProcessRight.UseVisualStyleBackColor = false;
            this.button_SaveProcessRight.Click += new System.EventHandler(this.button_SaveProcessRight_Click);
            // 
            // Form_ProcessRights
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 465);
            this.Controls.Add(this.button_SaveProcessRight);
            this.Controls.Add(this.button_ApplyAll);
            this.Controls.Add(this.button_Add);
            this.Controls.Add(this.button_Delete);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "Form_ProcessRights";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set the access rights to the specific process";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_AccessFlag;
        private System.Windows.Forms.Button button_AccessFlag;
        private System.Windows.Forms.CheckBox checkBox_AllowCopyout;
        private System.Windows.Forms.CheckBox checkBox_AllowSaveAs;
        private System.Windows.Forms.CheckBox checkBox_AllowReadEncryptedFiles;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_ProcessName;
        private System.Windows.Forms.Button button_AddProcessName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView listView_ProcessRights;
        private System.Windows.Forms.Button button_Add;
        private System.Windows.Forms.Button button_ApplyAll;
        private System.Windows.Forms.Button button_Delete;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_ProcessId;
        private System.Windows.Forms.Button button_info;
        private System.Windows.Forms.Button button_SaveProcessRight;
    }
}