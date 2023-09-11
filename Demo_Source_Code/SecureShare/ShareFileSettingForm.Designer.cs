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
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox_ShareFolderBlackList = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.textBox_ShareFolderWhiteList = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox_ProtectFolder = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.textBox_ProtectFolderBlackList = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox_ProtectFolderWhiteList = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.button_BrowseDRFolder = new System.Windows.Forms.Button();
            this.textBox_DRFolder = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_BrowseFolder = new System.Windows.Forms.Button();
            this.textBox_ShareFolder = new System.Windows.Forms.TextBox();
            this.button_help = new System.Windows.Forms.Button();
            this.button_ApplySetting = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.textBox_ShareFolderBlackList);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.textBox_ShareFolderWhiteList);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.textBox_ProtectFolder);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.textBox_ProtectFolderBlackList);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.textBox_ProtectFolderWhiteList);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.button_BrowseDRFolder);
            this.groupBox1.Controls.Add(this.textBox_DRFolder);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.button_BrowseFolder);
            this.groupBox1.Controls.Add(this.textBox_ShareFolder);
            this.groupBox1.Location = new System.Drawing.Point(12, 21);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(628, 382);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(211, 179);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(215, 13);
            this.label7.TabIndex = 74;
            this.label7.Text = "You can drop the encrypted file to this folder";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(212, 225);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(189, 12);
            this.label9.TabIndex = 71;
            this.label9.Text = "( split with \';\' , process format \"notepad.exe\" )";
            // 
            // textBox_ShareFolderBlackList
            // 
            this.textBox_ShareFolderBlackList.Location = new System.Drawing.Point(215, 246);
            this.textBox_ShareFolderBlackList.Name = "textBox_ShareFolderBlackList";
            this.textBox_ShareFolderBlackList.Size = new System.Drawing.Size(297, 20);
            this.textBox_ShareFolderBlackList.TabIndex = 68;
            this.textBox_ShareFolderBlackList.Text = "explorer.exe;";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(16, 246);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(147, 13);
            this.label13.TabIndex = 70;
            this.label13.Text = "Unauthorized  process names";
            // 
            // textBox_ShareFolderWhiteList
            // 
            this.textBox_ShareFolderWhiteList.Location = new System.Drawing.Point(215, 203);
            this.textBox_ShareFolderWhiteList.Name = "textBox_ShareFolderWhiteList";
            this.textBox_ShareFolderWhiteList.Size = new System.Drawing.Size(297, 20);
            this.textBox_ShareFolderWhiteList.TabIndex = 67;
            this.textBox_ShareFolderWhiteList.Text = "*";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(19, 203);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(131, 13);
            this.label14.TabIndex = 69;
            this.label14.Text = "Authorized process names";
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
            this.label6.Size = new System.Drawing.Size(133, 13);
            this.label6.TabIndex = 65;
            this.label6.Text = "RealTime Protected Folder";
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
            // textBox_ProtectFolder
            // 
            this.textBox_ProtectFolder.Location = new System.Drawing.Point(215, 18);
            this.textBox_ProtectFolder.Name = "textBox_ProtectFolder";
            this.textBox_ProtectFolder.Size = new System.Drawing.Size(297, 20);
            this.textBox_ProtectFolder.TabIndex = 63;
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
            this.textBox_ProtectFolderWhiteList.Text = "*";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(19, 67);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(131, 13);
            this.label10.TabIndex = 60;
            this.label10.Text = "Authorized process names";
            // 
            // button_BrowseDRFolder
            // 
            this.button_BrowseDRFolder.Location = new System.Drawing.Point(537, 301);
            this.button_BrowseDRFolder.Name = "button_BrowseDRFolder";
            this.button_BrowseDRFolder.Size = new System.Drawing.Size(31, 23);
            this.button_BrowseDRFolder.TabIndex = 6;
            this.button_BrowseDRFolder.Text = "...";
            this.button_BrowseDRFolder.UseVisualStyleBackColor = true;
            // 
            // textBox_DRFolder
            // 
            this.textBox_DRFolder.Location = new System.Drawing.Point(215, 301);
            this.textBox_DRFolder.Name = "textBox_DRFolder";
            this.textBox_DRFolder.Size = new System.Drawing.Size(297, 20);
            this.textBox_DRFolder.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 159);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Shared File Drop Folder";
            // 
            // button_BrowseFolder
            // 
            this.button_BrowseFolder.Location = new System.Drawing.Point(537, 154);
            this.button_BrowseFolder.Name = "button_BrowseFolder";
            this.button_BrowseFolder.Size = new System.Drawing.Size(31, 23);
            this.button_BrowseFolder.TabIndex = 1;
            this.button_BrowseFolder.Text = "...";
            this.button_BrowseFolder.UseVisualStyleBackColor = true;
            this.button_BrowseFolder.Click += new System.EventHandler(this.button_BrowseFolder_Click);
            // 
            // textBox_ShareFolder
            // 
            this.textBox_ShareFolder.Location = new System.Drawing.Point(214, 154);
            this.textBox_ShareFolder.Name = "textBox_ShareFolder";
            this.textBox_ShareFolder.Size = new System.Drawing.Size(298, 20);
            this.textBox_ShareFolder.TabIndex = 0;
            // 
            // button_help
            // 
            this.button_help.Image = ((System.Drawing.Image)(resources.GetObject("button_help.Image")));
            this.button_help.Location = new System.Drawing.Point(34, 413);
            this.button_help.Name = "button_help";
            this.button_help.Size = new System.Drawing.Size(36, 33);
            this.button_help.TabIndex = 77;
            this.button_help.UseVisualStyleBackColor = true;
            this.button_help.Click += new System.EventHandler(this.button_help_Click);
            // 
            // button_ApplySetting
            // 
            this.button_ApplySetting.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_ApplySetting.Location = new System.Drawing.Point(505, 423);
            this.button_ApplySetting.Name = "button_ApplySetting";
            this.button_ApplySetting.Size = new System.Drawing.Size(75, 23);
            this.button_ApplySetting.TabIndex = 2;
            this.button_ApplySetting.Text = "Apply";
            this.button_ApplySetting.UseVisualStyleBackColor = true;
            this.button_ApplySetting.Click += new System.EventHandler(this.button_ApplySettings_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 301);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(133, 13);
            this.label3.TabIndex = 75;
            this.label3.Text = "The DRM data store folder";
            // 
            // ShareFileSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 474);
            this.Controls.Add(this.button_help);
            this.Controls.Add(this.button_ApplySetting);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ShareFileSettingForm";
            this.Text = "Shared File Settings";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_BrowseFolder;
        private System.Windows.Forms.TextBox textBox_ShareFolder;
        private System.Windows.Forms.Button button_ApplySetting;
        private System.Windows.Forms.Button button_BrowseDRFolder;
        private System.Windows.Forms.TextBox textBox_DRFolder;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox_ShareFolderBlackList;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textBox_ShareFolderWhiteList;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox_ProtectFolder;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBox_ProtectFolderBlackList;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBox_ProtectFolderWhiteList;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button button_help;
        private System.Windows.Forms.Label label3;
    }
}