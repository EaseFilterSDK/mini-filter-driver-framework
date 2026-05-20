
namespace ZeroTrustDemo
{
    partial class ZeroTrustForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ZeroTrustForm));
            this.button_ProtectedFolder = new System.Windows.Forms.Button();
            this.button_Stop = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_ProtectedFolder = new System.Windows.Forms.TextBox();
            this.button_Start = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox_Encryption = new System.Windows.Forms.CheckBox();
            this.textBox_PassPhrase = new System.Windows.Forms.TextBox();
            this.textBox_AccessRights = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button_AccessRights = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_AddProcessRights = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_ModifyProcessRights = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_RemoveProcess = new System.Windows.Forms.ToolStripButton();
            this.listView_ProcessRights = new System.Windows.Forms.ListView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.listView_UserRights = new System.Windows.Forms.ListView();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_AddUserRights = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_ModifyUserRights = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_RemoveUser = new System.Windows.Forms.ToolStripButton();
            this.button_Info = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_ProtectedFolder
            // 
            this.button_ProtectedFolder.Location = new System.Drawing.Point(430, 20);
            this.button_ProtectedFolder.Name = "button_ProtectedFolder";
            this.button_ProtectedFolder.Size = new System.Drawing.Size(26, 23);
            this.button_ProtectedFolder.TabIndex = 40;
            this.button_ProtectedFolder.Text = "...";
            this.button_ProtectedFolder.UseVisualStyleBackColor = true;
            this.button_ProtectedFolder.Click += new System.EventHandler(this.button_ProtectedFolder_Click);
            // 
            // button_Stop
            // 
            this.button_Stop.Location = new System.Drawing.Point(318, 428);
            this.button_Stop.Name = "button_Stop";
            this.button_Stop.Size = new System.Drawing.Size(87, 23);
            this.button_Stop.TabIndex = 29;
            this.button_Stop.Text = "Stop Service";
            this.button_Stop.UseVisualStyleBackColor = true;
            this.button_Stop.Click += new System.EventHandler(this.button_Stop_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 27;
            this.label1.Text = "Protected Folder";
            // 
            // textBox_ProtectedFolder
            // 
            this.textBox_ProtectedFolder.Location = new System.Drawing.Point(145, 22);
            this.textBox_ProtectedFolder.Name = "textBox_ProtectedFolder";
            this.textBox_ProtectedFolder.Size = new System.Drawing.Size(260, 20);
            this.textBox_ProtectedFolder.TabIndex = 26;
            this.textBox_ProtectedFolder.Text = "c:\\protectedFolder\\*";
            this.textBox_ProtectedFolder.TextChanged += new System.EventHandler(this.textBox_ProtectedFolder_TextChanged);
            // 
            // button_Start
            // 
            this.button_Start.Location = new System.Drawing.Point(15, 424);
            this.button_Start.Name = "button_Start";
            this.button_Start.Size = new System.Drawing.Size(86, 23);
            this.button_Start.TabIndex = 25;
            this.button_Start.Text = "Start Service";
            this.button_Start.UseVisualStyleBackColor = true;
            this.button_Start.Click += new System.EventHandler(this.button_Start_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_Info);
            this.groupBox1.Controls.Add(this.checkBox_Encryption);
            this.groupBox1.Controls.Add(this.textBox_PassPhrase);
            this.groupBox1.Controls.Add(this.textBox_AccessRights);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.button_AccessRights);
            this.groupBox1.Controls.Add(this.textBox_ProtectedFolder);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.button_ProtectedFolder);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(484, 113);
            this.groupBox1.TabIndex = 45;
            this.groupBox1.TabStop = false;
            // 
            // checkBox_Encryption
            // 
            this.checkBox_Encryption.AutoSize = true;
            this.checkBox_Encryption.Location = new System.Drawing.Point(15, 77);
            this.checkBox_Encryption.Name = "checkBox_Encryption";
            this.checkBox_Encryption.Size = new System.Drawing.Size(221, 17);
            this.checkBox_Encryption.TabIndex = 92;
            this.checkBox_Encryption.Text = "Enable file encryption.  Enter passphrase:";
            this.checkBox_Encryption.UseVisualStyleBackColor = true;
            this.checkBox_Encryption.CheckedChanged += new System.EventHandler(this.checkBox_Encryption_CheckedChanged);
            // 
            // textBox_PassPhrase
            // 
            this.textBox_PassPhrase.Enabled = false;
            this.textBox_PassPhrase.Location = new System.Drawing.Point(242, 77);
            this.textBox_PassPhrase.Name = "textBox_PassPhrase";
            this.textBox_PassPhrase.Size = new System.Drawing.Size(163, 20);
            this.textBox_PassPhrase.TabIndex = 93;
            this.textBox_PassPhrase.UseSystemPasswordChar = true;
            // 
            // textBox_AccessRights
            // 
            this.textBox_AccessRights.Location = new System.Drawing.Point(145, 48);
            this.textBox_AccessRights.Name = "textBox_AccessRights";
            this.textBox_AccessRights.Size = new System.Drawing.Size(260, 20);
            this.textBox_AccessRights.TabIndex = 41;
            this.textBox_AccessRights.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 13);
            this.label2.TabIndex = 42;
            this.label2.Text = "Folder Access Rights";
            // 
            // button_AccessRights
            // 
            this.button_AccessRights.Location = new System.Drawing.Point(430, 47);
            this.button_AccessRights.Name = "button_AccessRights";
            this.button_AccessRights.Size = new System.Drawing.Size(26, 23);
            this.button_AccessRights.TabIndex = 43;
            this.button_AccessRights.Text = "...";
            this.button_AccessRights.UseVisualStyleBackColor = true;
            this.button_AccessRights.Click += new System.EventHandler(this.button_AccessRights_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.toolStrip1);
            this.groupBox2.Controls.Add(this.listView_ProcessRights);
            this.groupBox2.Location = new System.Drawing.Point(-4, 119);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(463, 140);
            this.groupBox2.TabIndex = 46;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "WhiteList Processes";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_AddProcessRights,
            this.toolStripSeparator5,
            this.toolStripButton_ModifyProcessRights,
            this.toolStripSeparator4,
            this.toolStripButton_RemoveProcess});
            this.toolStrip1.Location = new System.Drawing.Point(3, 16);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(457, 25);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton_AddProcessRights
            // 
            this.toolStripButton_AddProcessRights.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_AddProcessRights.Image")));
            this.toolStripButton_AddProcessRights.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_AddProcessRights.Name = "toolStripButton_AddProcessRights";
            this.toolStripButton_AddProcessRights.Size = new System.Drawing.Size(128, 22);
            this.toolStripButton_AddProcessRights.Text = "Add Process Rights";
            this.toolStripButton_AddProcessRights.Click += new System.EventHandler(this.toolStripButton_AddProcessRights_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_ModifyProcessRights
            // 
            this.toolStripButton_ModifyProcessRights.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_ModifyProcessRights.Image")));
            this.toolStripButton_ModifyProcessRights.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_ModifyProcessRights.Name = "toolStripButton_ModifyProcessRights";
            this.toolStripButton_ModifyProcessRights.Size = new System.Drawing.Size(144, 22);
            this.toolStripButton_ModifyProcessRights.Text = "Modify Process Rights";
            this.toolStripButton_ModifyProcessRights.Click += new System.EventHandler(this.toolStripButton_ModifyProcessRights_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_RemoveProcess
            // 
            this.toolStripButton_RemoveProcess.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_RemoveProcess.Image")));
            this.toolStripButton_RemoveProcess.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_RemoveProcess.Name = "toolStripButton_RemoveProcess";
            this.toolStripButton_RemoveProcess.Size = new System.Drawing.Size(113, 22);
            this.toolStripButton_RemoveProcess.Text = "Remove Process";
            this.toolStripButton_RemoveProcess.Click += new System.EventHandler(this.toolStripButton_RemoveProcess_Click);
            // 
            // listView_ProcessRights
            // 
            this.listView_ProcessRights.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.listView_ProcessRights.FullRowSelect = true;
            this.listView_ProcessRights.HideSelection = false;
            this.listView_ProcessRights.HoverSelection = true;
            this.listView_ProcessRights.Location = new System.Drawing.Point(3, 43);
            this.listView_ProcessRights.Margin = new System.Windows.Forms.Padding(2);
            this.listView_ProcessRights.MultiSelect = false;
            this.listView_ProcessRights.Name = "listView_ProcessRights";
            this.listView_ProcessRights.ShowItemToolTips = true;
            this.listView_ProcessRights.Size = new System.Drawing.Size(457, 94);
            this.listView_ProcessRights.TabIndex = 4;
            this.listView_ProcessRights.UseCompatibleStateImageBehavior = false;
            this.listView_ProcessRights.View = System.Windows.Forms.View.Details;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.listView_UserRights);
            this.groupBox3.Controls.Add(this.toolStrip2);
            this.groupBox3.Location = new System.Drawing.Point(3, 265);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(459, 157);
            this.groupBox3.TabIndex = 47;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "WhiteList Users";
            // 
            // listView_UserRights
            // 
            this.listView_UserRights.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.listView_UserRights.FullRowSelect = true;
            this.listView_UserRights.HideSelection = false;
            this.listView_UserRights.HoverSelection = true;
            this.listView_UserRights.Location = new System.Drawing.Point(3, 43);
            this.listView_UserRights.Margin = new System.Windows.Forms.Padding(2);
            this.listView_UserRights.MultiSelect = false;
            this.listView_UserRights.Name = "listView_UserRights";
            this.listView_UserRights.ShowItemToolTips = true;
            this.listView_UserRights.Size = new System.Drawing.Size(453, 111);
            this.listView_UserRights.TabIndex = 7;
            this.listView_UserRights.UseCompatibleStateImageBehavior = false;
            this.listView_UserRights.View = System.Windows.Forms.View.Details;
            // 
            // toolStrip2
            // 
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_AddUserRights,
            this.toolStripSeparator1,
            this.toolStripButton_ModifyUserRights,
            this.toolStripSeparator2,
            this.toolStripButton_RemoveUser});
            this.toolStrip2.Location = new System.Drawing.Point(3, 16);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(453, 25);
            this.toolStrip2.TabIndex = 6;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripButton_AddUserRights
            // 
            this.toolStripButton_AddUserRights.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_AddUserRights.Image")));
            this.toolStripButton_AddUserRights.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_AddUserRights.Name = "toolStripButton_AddUserRights";
            this.toolStripButton_AddUserRights.Size = new System.Drawing.Size(111, 22);
            this.toolStripButton_AddUserRights.Text = "Add User Rights";
            this.toolStripButton_AddUserRights.Click += new System.EventHandler(this.toolStripButton_AddUserRights_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_ModifyUserRights
            // 
            this.toolStripButton_ModifyUserRights.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_ModifyUserRights.Image")));
            this.toolStripButton_ModifyUserRights.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_ModifyUserRights.Name = "toolStripButton_ModifyUserRights";
            this.toolStripButton_ModifyUserRights.Size = new System.Drawing.Size(127, 22);
            this.toolStripButton_ModifyUserRights.Text = "Modify User Rights";
            this.toolStripButton_ModifyUserRights.Click += new System.EventHandler(this.toolStripButton_ModifyUserRights_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_RemoveUser
            // 
            this.toolStripButton_RemoveUser.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_RemoveUser.Image")));
            this.toolStripButton_RemoveUser.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_RemoveUser.Name = "toolStripButton_RemoveUser";
            this.toolStripButton_RemoveUser.Size = new System.Drawing.Size(132, 22);
            this.toolStripButton_RemoveUser.Text = "Remove User Rights";
            this.toolStripButton_RemoveUser.Click += new System.EventHandler(this.toolStripButton_RemoveUser_Click);
            // 
            // button_Info
            // 
            this.button_Info.Image = global::ZeroTrustDemo.Properties.Resources.about;
            this.button_Info.Location = new System.Drawing.Point(430, 77);
            this.button_Info.Name = "button_Info";
            this.button_Info.Size = new System.Drawing.Size(26, 20);
            this.button_Info.TabIndex = 115;
            this.button_Info.UseVisualStyleBackColor = true;
            this.button_Info.Click += new System.EventHandler(this.button_Info_Click);
            // 
            // ZeroTrustForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 459);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_Stop);
            this.Controls.Add(this.button_Start);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ZeroTrustForm";
            this.Text = "EaseFilter Zero Trust Demo";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ZeroTrustForm_FormClosed);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button_ProtectedFolder;
        private System.Windows.Forms.Button button_Stop;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_ProtectedFolder;
        private System.Windows.Forms.Button button_Start;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox_AccessRights;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_AccessRights;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListView listView_ProcessRights;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton_AddProcessRights;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton toolStripButton_ModifyProcessRights;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton toolStripButton_RemoveProcess;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton toolStripButton_AddUserRights;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton_ModifyUserRights;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButton_RemoveUser;
        private System.Windows.Forms.ListView listView_UserRights;
        private System.Windows.Forms.CheckBox checkBox_Encryption;
        private System.Windows.Forms.TextBox textBox_PassPhrase;
        private System.Windows.Forms.Button button_Info;
    }
}