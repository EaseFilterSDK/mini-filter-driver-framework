using EaseFilter.CommonObjects;

namespace ProcessMon
{
    partial class ProcessFileAccessRights
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProcessFileAccessRights));
            this.button_ApplyAll = new System.Windows.Forms.Button();
            this.button_Add = new System.Windows.Forms.Button();
            this.button_Delete = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listView_ProcessFileAccessRights = new System.Windows.Forms.ListView();
            this.groupBox_ProcessRights = new System.Windows.Forms.GroupBox();
            this.button_SelectFolder = new System.Windows.Forms.Button();
            this.button_Info = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_FileMask = new System.Windows.Forms.TextBox();
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
            this.groupBox1.SuspendLayout();
            this.groupBox_ProcessRights.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_ApplyAll
            // 
            this.button_ApplyAll.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_ApplyAll.Location = new System.Drawing.Point(406, 381);
            this.button_ApplyAll.Name = "button_ApplyAll";
            this.button_ApplyAll.Size = new System.Drawing.Size(144, 23);
            this.button_ApplyAll.TabIndex = 85;
            this.button_ApplyAll.Text = "Apply settings to sandbox";
            this.button_ApplyAll.UseVisualStyleBackColor = true;
            this.button_ApplyAll.Click += new System.EventHandler(this.button_ApplyAll_Click);
            // 
            // button_Add
            // 
            this.button_Add.Location = new System.Drawing.Point(15, 381);
            this.button_Add.Name = "button_Add";
            this.button_Add.Size = new System.Drawing.Size(122, 23);
            this.button_Add.TabIndex = 84;
            this.button_Add.Text = "Add file entry";
            this.button_Add.UseVisualStyleBackColor = true;
            this.button_Add.Click += new System.EventHandler(this.button_Add_Click);
            // 
            // button_Delete
            // 
            this.button_Delete.Location = new System.Drawing.Point(197, 381);
            this.button_Delete.Name = "button_Delete";
            this.button_Delete.Size = new System.Drawing.Size(128, 23);
            this.button_Delete.TabIndex = 83;
            this.button_Delete.Text = "Delete file entry";
            this.button_Delete.UseVisualStyleBackColor = true;
            this.button_Delete.Click += new System.EventHandler(this.button_Delete_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listView_ProcessFileAccessRights);
            this.groupBox1.Location = new System.Drawing.Point(15, 225);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(535, 134);
            this.groupBox1.TabIndex = 82;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "File Access rights collection";
            // 
            // listView_ProcessFileAccessRights
            // 
            this.listView_ProcessFileAccessRights.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_ProcessFileAccessRights.FullRowSelect = true;
            this.listView_ProcessFileAccessRights.Location = new System.Drawing.Point(3, 16);
            this.listView_ProcessFileAccessRights.Name = "listView_ProcessFileAccessRights";
            this.listView_ProcessFileAccessRights.Size = new System.Drawing.Size(529, 115);
            this.listView_ProcessFileAccessRights.TabIndex = 1;
            this.listView_ProcessFileAccessRights.UseCompatibleStateImageBehavior = false;
            this.listView_ProcessFileAccessRights.View = System.Windows.Forms.View.Details;
            this.listView_ProcessFileAccessRights.SelectedIndexChanged += new System.EventHandler(this.listView_ProcessFileAccessRights_SelectedIndexChanged);
            // 
            // groupBox_ProcessRights
            // 
            this.groupBox_ProcessRights.Controls.Add(this.button_SelectFolder);
            this.groupBox_ProcessRights.Controls.Add(this.button_Info);
            this.groupBox_ProcessRights.Controls.Add(this.label1);
            this.groupBox_ProcessRights.Controls.Add(this.textBox_FileMask);
            this.groupBox_ProcessRights.Controls.Add(this.label2);
            this.groupBox_ProcessRights.Controls.Add(this.textBox_AccessFlag);
            this.groupBox_ProcessRights.Controls.Add(this.button_AccessFlag);
            this.groupBox_ProcessRights.Controls.Add(this.checkBox_SetSecurity);
            this.groupBox_ProcessRights.Controls.Add(this.checkBox_QueryInfo);
            this.groupBox_ProcessRights.Controls.Add(this.checkBox_Read);
            this.groupBox_ProcessRights.Controls.Add(this.checkBox_QuerySecurity);
            this.groupBox_ProcessRights.Controls.Add(this.checkBox_SetInfo);
            this.groupBox_ProcessRights.Controls.Add(this.checkBox_Write);
            this.groupBox_ProcessRights.Controls.Add(this.checkBox_AllowDelete);
            this.groupBox_ProcessRights.Controls.Add(this.checkBox_AllowRename);
            this.groupBox_ProcessRights.Controls.Add(this.checkBox_Creation);
            this.groupBox_ProcessRights.Location = new System.Drawing.Point(21, 12);
            this.groupBox_ProcessRights.Name = "groupBox_ProcessRights";
            this.groupBox_ProcessRights.Size = new System.Drawing.Size(529, 206);
            this.groupBox_ProcessRights.TabIndex = 81;
            this.groupBox_ProcessRights.TabStop = false;
            this.groupBox_ProcessRights.Text = "File Access Rights the Process ";
            // 
            // button_SelectFolder
            // 
            this.button_SelectFolder.Location = new System.Drawing.Point(422, 32);
            this.button_SelectFolder.Name = "button_SelectFolder";
            this.button_SelectFolder.Size = new System.Drawing.Size(60, 20);
            this.button_SelectFolder.TabIndex = 92;
            this.button_SelectFolder.Text = "...";
            this.button_SelectFolder.UseVisualStyleBackColor = true;
            this.button_SelectFolder.Click += new System.EventHandler(this.button_SelectFolder_Click);
            // 
            // button_Info
            // 
            this.button_Info.BackColor = System.Drawing.Color.AntiqueWhite;
            this.button_Info.Image = global::ProcessMon.Properties.Resources.about;
            this.button_Info.Location = new System.Drawing.Point(484, 32);
            this.button_Info.Name = "button_Info";
            this.button_Info.Size = new System.Drawing.Size(33, 20);
            this.button_Info.TabIndex = 91;
            this.button_Info.UseVisualStyleBackColor = false;
            this.button_Info.Click += new System.EventHandler(this.button_Info_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 39;
            this.label1.Text = "File name mask";
            // 
            // textBox_FileMask
            // 
            this.textBox_FileMask.Location = new System.Drawing.Point(152, 32);
            this.textBox_FileMask.Name = "textBox_FileMask";
            this.textBox_FileMask.Size = new System.Drawing.Size(242, 20);
            this.textBox_FileMask.TabIndex = 38;
            this.textBox_FileMask.Text = "c:\\test\\*";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 13);
            this.label2.TabIndex = 32;
            this.label2.Text = "Access control flag";
            // 
            // textBox_AccessFlag
            // 
            this.textBox_AccessFlag.Location = new System.Drawing.Point(152, 67);
            this.textBox_AccessFlag.Name = "textBox_AccessFlag";
            this.textBox_AccessFlag.ReadOnly = true;
            this.textBox_AccessFlag.Size = new System.Drawing.Size(242, 20);
            this.textBox_AccessFlag.TabIndex = 31;
            this.textBox_AccessFlag.Text = "0";
            // 
            // button_AccessFlag
            // 
            this.button_AccessFlag.Location = new System.Drawing.Point(422, 69);
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
            this.checkBox_SetSecurity.Location = new System.Drawing.Point(152, 152);
            this.checkBox_SetSecurity.Name = "checkBox_SetSecurity";
            this.checkBox_SetSecurity.Size = new System.Drawing.Size(137, 17);
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
            this.checkBox_QueryInfo.Location = new System.Drawing.Point(334, 129);
            this.checkBox_QueryInfo.Name = "checkBox_QueryInfo";
            this.checkBox_QueryInfo.Size = new System.Drawing.Size(148, 17);
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
            this.checkBox_Read.Location = new System.Drawing.Point(334, 106);
            this.checkBox_Read.Name = "checkBox_Read";
            this.checkBox_Read.Size = new System.Drawing.Size(105, 17);
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
            this.checkBox_QuerySecurity.Location = new System.Drawing.Point(334, 152);
            this.checkBox_QuerySecurity.Name = "checkBox_QuerySecurity";
            this.checkBox_QuerySecurity.Size = new System.Drawing.Size(133, 17);
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
            this.checkBox_SetInfo.Location = new System.Drawing.Point(152, 128);
            this.checkBox_SetInfo.Name = "checkBox_SetInfo";
            this.checkBox_SetInfo.Size = new System.Drawing.Size(152, 17);
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
            this.checkBox_Write.Location = new System.Drawing.Point(12, 129);
            this.checkBox_Write.Name = "checkBox_Write";
            this.checkBox_Write.Size = new System.Drawing.Size(100, 17);
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
            this.checkBox_AllowDelete.Location = new System.Drawing.Point(12, 152);
            this.checkBox_AllowDelete.Name = "checkBox_AllowDelete";
            this.checkBox_AllowDelete.Size = new System.Drawing.Size(107, 17);
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
            this.checkBox_AllowRename.Location = new System.Drawing.Point(152, 106);
            this.checkBox_AllowRename.Name = "checkBox_AllowRename";
            this.checkBox_AllowRename.Size = new System.Drawing.Size(113, 17);
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
            this.checkBox_Creation.Location = new System.Drawing.Point(12, 106);
            this.checkBox_Creation.Name = "checkBox_Creation";
            this.checkBox_Creation.Size = new System.Drawing.Size(131, 17);
            this.checkBox_Creation.TabIndex = 22;
            this.checkBox_Creation.Text = "Allow creating new file";
            this.checkBox_Creation.UseVisualStyleBackColor = true;
            this.checkBox_Creation.CheckedChanged += new System.EventHandler(this.checkBox_Creation_CheckedChanged);
            // 
            // ProcessFileAccessRights
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 444);
            this.Controls.Add(this.button_ApplyAll);
            this.Controls.Add(this.button_Add);
            this.Controls.Add(this.button_Delete);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox_ProcessRights);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ProcessFileAccessRights";
            this.Text = "Add File Access Control  To The Process";
            this.groupBox1.ResumeLayout(false);
            this.groupBox_ProcessRights.ResumeLayout(false);
            this.groupBox_ProcessRights.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_ApplyAll;
        private System.Windows.Forms.Button button_Add;
        private System.Windows.Forms.Button button_Delete;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView listView_ProcessFileAccessRights;
        private System.Windows.Forms.GroupBox groupBox_ProcessRights;
        private System.Windows.Forms.Button button_Info;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_FileMask;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_AccessFlag;
        private System.Windows.Forms.Button button_AccessFlag;
        private System.Windows.Forms.CheckBox checkBox_SetSecurity;
        private System.Windows.Forms.CheckBox checkBox_QueryInfo;
        private System.Windows.Forms.CheckBox checkBox_Read;
        private System.Windows.Forms.CheckBox checkBox_QuerySecurity;
        private System.Windows.Forms.CheckBox checkBox_SetInfo;
        private System.Windows.Forms.CheckBox checkBox_Write;
        private System.Windows.Forms.CheckBox checkBox_AllowDelete;
        private System.Windows.Forms.CheckBox checkBox_AllowRename;
        private System.Windows.Forms.CheckBox checkBox_Creation;
        private System.Windows.Forms.Button button_SelectFolder;
    }
}