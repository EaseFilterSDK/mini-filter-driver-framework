namespace  SecureShare
{
    partial class ShareFileSettingForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShareFileSettingForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBox_DRMFolder = new System.Windows.Forms.TextBox();
            this.button_DRMFolder = new System.Windows.Forms.Button();
            this.dateTimePicker_ExpireDate = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker_ExpireTime = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox_SharedFileDropFolder = new System.Windows.Forms.TextBox();
            this.button_BrowseFolder = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox_UnauthorizedUserNames = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.textBox_AuthorizedUserNames = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox_AutoEncryptFolder = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.textBox_ProtectFolderBlackList = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox_ProtectFolderWhiteList = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.button_help = new System.Windows.Forms.Button();
            this.button_ApplySetting = new System.Windows.Forms.Button();
            this.radioButton_DRMInLocal = new System.Windows.Forms.RadioButton();
            this.radioButton_DRMInServer = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.dateTimePicker_ExpireDate);
            this.groupBox1.Controls.Add(this.dateTimePicker_ExpireTime);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.textBox_UnauthorizedUserNames);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.textBox_AuthorizedUserNames);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.textBox_AutoEncryptFolder);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.textBox_ProtectFolderBlackList);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.textBox_ProtectFolderWhiteList);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Location = new System.Drawing.Point(12, 21);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(596, 482);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.radioButton_DRMInServer);
            this.groupBox3.Controls.Add(this.radioButton_DRMInLocal);
            this.groupBox3.Controls.Add(this.textBox_DRMFolder);
            this.groupBox3.Controls.Add(this.button_DRMFolder);
            this.groupBox3.Location = new System.Drawing.Point(0, 366);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(596, 110);
            this.groupBox3.TabIndex = 77;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "DRM data location settings";
            // 
            // textBox_DRMFolder
            // 
            this.textBox_DRMFolder.Location = new System.Drawing.Point(214, 29);
            this.textBox_DRMFolder.Name = "textBox_DRMFolder";
            this.textBox_DRMFolder.Size = new System.Drawing.Size(298, 20);
            this.textBox_DRMFolder.TabIndex = 0;
            // 
            // button_DRMFolder
            // 
            this.button_DRMFolder.Location = new System.Drawing.Point(537, 27);
            this.button_DRMFolder.Name = "button_DRMFolder";
            this.button_DRMFolder.Size = new System.Drawing.Size(31, 23);
            this.button_DRMFolder.TabIndex = 1;
            this.button_DRMFolder.Text = "...";
            this.button_DRMFolder.UseVisualStyleBackColor = true;
            this.button_DRMFolder.Click += new System.EventHandler(this.button_DRMFolder_Click);
            // 
            // dateTimePicker_ExpireDate
            // 
            this.dateTimePicker_ExpireDate.CustomFormat = "yyyy-MM-dd";
            this.dateTimePicker_ExpireDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker_ExpireDate.Location = new System.Drawing.Point(214, 243);
            this.dateTimePicker_ExpireDate.Name = "dateTimePicker_ExpireDate";
            this.dateTimePicker_ExpireDate.Size = new System.Drawing.Size(120, 20);
            this.dateTimePicker_ExpireDate.TabIndex = 79;
            // 
            // dateTimePicker_ExpireTime
            // 
            this.dateTimePicker_ExpireTime.CustomFormat = " HH:mm:ss";
            this.dateTimePicker_ExpireTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePicker_ExpireTime.Location = new System.Drawing.Point(337, 243);
            this.dateTimePicker_ExpireTime.Name = "dateTimePicker_ExpireTime";
            this.dateTimePicker_ExpireTime.ShowUpDown = true;
            this.dateTimePicker_ExpireTime.Size = new System.Drawing.Size(120, 20);
            this.dateTimePicker_ExpireTime.TabIndex = 78;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 243);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 13);
            this.label4.TabIndex = 77;
            this.label4.Text = "Share file expire time";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.textBox_SharedFileDropFolder);
            this.groupBox2.Controls.Add(this.button_BrowseFolder);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(0, 287);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(596, 81);
            this.groupBox2.TabIndex = 76;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "The recipient-side folder to access the encrypted files";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(213, 52);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(250, 13);
            this.label7.TabIndex = 74;
            this.label7.Text = "You can drop the encrypted shared file to this folder";
            // 
            // textBox_SharedFileDropFolder
            // 
            this.textBox_SharedFileDropFolder.Location = new System.Drawing.Point(214, 29);
            this.textBox_SharedFileDropFolder.Name = "textBox_SharedFileDropFolder";
            this.textBox_SharedFileDropFolder.Size = new System.Drawing.Size(298, 20);
            this.textBox_SharedFileDropFolder.TabIndex = 0;
            // 
            // button_BrowseFolder
            // 
            this.button_BrowseFolder.Location = new System.Drawing.Point(537, 27);
            this.button_BrowseFolder.Name = "button_BrowseFolder";
            this.button_BrowseFolder.Size = new System.Drawing.Size(31, 23);
            this.button_BrowseFolder.TabIndex = 1;
            this.button_BrowseFolder.Text = "...";
            this.button_BrowseFolder.UseVisualStyleBackColor = true;
            this.button_BrowseFolder.Click += new System.EventHandler(this.button_BrowseFolder_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(169, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Encrypted Shared File Drop Folder";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(214, 216);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(130, 12);
            this.label3.TabIndex = 75;
            this.label3.Text = "( domain\\user1;domain\\user2 )";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(213, 172);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(130, 12);
            this.label9.TabIndex = 71;
            this.label9.Text = "( domain\\user1;domain\\user2 )";
            // 
            // textBox_UnauthorizedUserNames
            // 
            this.textBox_UnauthorizedUserNames.Location = new System.Drawing.Point(216, 193);
            this.textBox_UnauthorizedUserNames.Name = "textBox_UnauthorizedUserNames";
            this.textBox_UnauthorizedUserNames.Size = new System.Drawing.Size(297, 20);
            this.textBox_UnauthorizedUserNames.TabIndex = 68;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(16, 196);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(130, 13);
            this.label13.TabIndex = 70;
            this.label13.Text = "Unauthorized  user names";
            // 
            // textBox_AuthorizedUserNames
            // 
            this.textBox_AuthorizedUserNames.Location = new System.Drawing.Point(216, 150);
            this.textBox_AuthorizedUserNames.Name = "textBox_AuthorizedUserNames";
            this.textBox_AuthorizedUserNames.Size = new System.Drawing.Size(297, 20);
            this.textBox_AuthorizedUserNames.TabIndex = 67;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(16, 150);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(114, 13);
            this.label14.TabIndex = 69;
            this.label14.Text = "Authorized user names";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(211, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(247, 13);
            this.label2.TabIndex = 66;
            this.label2.Text = "The file will be encrypted automatically in this folder";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(162, 13);
            this.label6.TabIndex = 65;
            this.label6.Text = "Auto Encrypted Share File Folder";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(537, 15);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(31, 23);
            this.button1.TabIndex = 64;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // textBox_AutoEncryptFolder
            // 
            this.textBox_AutoEncryptFolder.Location = new System.Drawing.Point(215, 18);
            this.textBox_AutoEncryptFolder.Name = "textBox_AutoEncryptFolder";
            this.textBox_AutoEncryptFolder.Size = new System.Drawing.Size(297, 20);
            this.textBox_AutoEncryptFolder.TabIndex = 63;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(212, 89);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(189, 12);
            this.label12.TabIndex = 62;
            this.label12.Text = "( split with \';\' , process format \"notepad.exe\" )";
            // 
            // textBox_ProtectFolderBlackList
            // 
            this.textBox_ProtectFolderBlackList.Location = new System.Drawing.Point(215, 110);
            this.textBox_ProtectFolderBlackList.Name = "textBox_ProtectFolderBlackList";
            this.textBox_ProtectFolderBlackList.Size = new System.Drawing.Size(297, 20);
            this.textBox_ProtectFolderBlackList.TabIndex = 59;
            this.textBox_ProtectFolderBlackList.Text = "explorer.exe;";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(16, 110);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(147, 13);
            this.label11.TabIndex = 61;
            this.label11.Text = "Unauthorized  process names";
            // 
            // textBox_ProtectFolderWhiteList
            // 
            this.textBox_ProtectFolderWhiteList.Location = new System.Drawing.Point(215, 67);
            this.textBox_ProtectFolderWhiteList.Name = "textBox_ProtectFolderWhiteList";
            this.textBox_ProtectFolderWhiteList.Size = new System.Drawing.Size(297, 20);
            this.textBox_ProtectFolderWhiteList.TabIndex = 58;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(16, 67);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(131, 13);
            this.label10.TabIndex = 60;
            this.label10.Text = "Authorized process names";
            // 
            // button_help
            // 
            this.button_help.Image = global::SecureShare.Properties.Resources.about;
            this.button_help.Location = new System.Drawing.Point(31, 509);
            this.button_help.Name = "button_help";
            this.button_help.Size = new System.Drawing.Size(36, 23);
            this.button_help.TabIndex = 77;
            this.button_help.UseVisualStyleBackColor = true;
            this.button_help.Click += new System.EventHandler(this.button_help_Click);
            // 
            // button_ApplySetting
            // 
            this.button_ApplySetting.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_ApplySetting.Location = new System.Drawing.Point(449, 509);
            this.button_ApplySetting.Name = "button_ApplySetting";
            this.button_ApplySetting.Size = new System.Drawing.Size(75, 23);
            this.button_ApplySetting.TabIndex = 2;
            this.button_ApplySetting.Text = "Apply";
            this.button_ApplySetting.UseVisualStyleBackColor = true;
            this.button_ApplySetting.Click += new System.EventHandler(this.button_ApplySettings_Click);
            // 
            // radioButton_DRMInLocal
            // 
            this.radioButton_DRMInLocal.AutoSize = true;
            this.radioButton_DRMInLocal.Location = new System.Drawing.Point(19, 27);
            this.radioButton_DRMInLocal.Name = "radioButton_DRMInLocal";
            this.radioButton_DRMInLocal.Size = new System.Drawing.Size(167, 17);
            this.radioButton_DRMInLocal.TabIndex = 76;
            this.radioButton_DRMInLocal.Text = "Store DRM data in local folder";
            this.radioButton_DRMInLocal.UseVisualStyleBackColor = true;
            // 
            // radioButton_DRMInServer
            // 
            this.radioButton_DRMInServer.AutoSize = true;
            this.radioButton_DRMInServer.Checked = true;
            this.radioButton_DRMInServer.Location = new System.Drawing.Point(19, 69);
            this.radioButton_DRMInServer.Name = "radioButton_DRMInServer";
            this.radioButton_DRMInServer.Size = new System.Drawing.Size(535, 17);
            this.radioButton_DRMInServer.TabIndex = 77;
            this.radioButton_DRMInServer.TabStop = true;
            this.radioButton_DRMInServer.Text = "Store DRM data in EaseFilter Server, allows you to grant or revoke the shared fil" +
    "e access anytime anywhere ";
            this.radioButton_DRMInServer.UseVisualStyleBackColor = true;
            // 
            // ShareFileSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(630, 550);
            this.Controls.Add(this.button_help);
            this.Controls.Add(this.button_ApplySetting);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ShareFileSettingForm";
            this.Text = "Auto Encrypt Share File Settings";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_BrowseFolder;
        private System.Windows.Forms.TextBox textBox_SharedFileDropFolder;
        private System.Windows.Forms.Button button_ApplySetting;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox_UnauthorizedUserNames;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textBox_AuthorizedUserNames;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox_AutoEncryptFolder;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBox_ProtectFolderBlackList;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBox_ProtectFolderWhiteList;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button button_help;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DateTimePicker dateTimePicker_ExpireDate;
        private System.Windows.Forms.DateTimePicker dateTimePicker_ExpireTime;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBox_DRMFolder;
        private System.Windows.Forms.Button button_DRMFolder;
        private System.Windows.Forms.RadioButton radioButton_DRMInServer;
        private System.Windows.Forms.RadioButton radioButton_DRMInLocal;
    }
}