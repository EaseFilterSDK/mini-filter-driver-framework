namespace EaseFilter.FolderLocker
{
    partial class ShareFileForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShareFileForm));
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_FileName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_UnauthorizedUserNames = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_AuthorizedUserNames = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_UnauthorizedProcessNames = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox_TargetName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.dateTimePicker_ExpireDate = new System.Windows.Forms.DateTimePicker();
            this.button_OpenFile = new System.Windows.Forms.Button();
            this.dateTimePicker_ExpireTime = new System.Windows.Forms.DateTimePicker();
            this.label12 = new System.Windows.Forms.Label();
            this.textBox_authorizedProcessNames = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.button_CreateFile = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Share file expire time";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "File name to be shared";
            // 
            // textBox_FileName
            // 
            this.textBox_FileName.Location = new System.Drawing.Point(213, 20);
            this.textBox_FileName.Name = "textBox_FileName";
            this.textBox_FileName.Size = new System.Drawing.Size(242, 20);
            this.textBox_FileName.TabIndex = 2;
            this.textBox_FileName.Text = "c:\\test\\test.txt";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBox_UnauthorizedUserNames);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textBox_AuthorizedUserNames);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.textBox_UnauthorizedProcessNames);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.textBox_TargetName);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.dateTimePicker_ExpireDate);
            this.groupBox1.Controls.Add(this.button_OpenFile);
            this.groupBox1.Controls.Add(this.dateTimePicker_ExpireTime);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.textBox_authorizedProcessNames);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox_FileName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(538, 309);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(209, 230);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(195, 12);
            this.label3.TabIndex = 80;
            this.label3.Text = "(split with \';\' ,user name format \"domain\\user\" )";
            // 
            // textBox_UnauthorizedUserNames
            // 
            this.textBox_UnauthorizedUserNames.Location = new System.Drawing.Point(214, 253);
            this.textBox_UnauthorizedUserNames.Name = "textBox_UnauthorizedUserNames";
            this.textBox_UnauthorizedUserNames.Size = new System.Drawing.Size(242, 20);
            this.textBox_UnauthorizedUserNames.TabIndex = 76;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 253);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(127, 13);
            this.label4.TabIndex = 79;
            this.label4.Text = "Unauthorized user names";
            // 
            // textBox_AuthorizedUserNames
            // 
            this.textBox_AuthorizedUserNames.Location = new System.Drawing.Point(214, 210);
            this.textBox_AuthorizedUserNames.Name = "textBox_AuthorizedUserNames";
            this.textBox_AuthorizedUserNames.Size = new System.Drawing.Size(242, 20);
            this.textBox_AuthorizedUserNames.TabIndex = 75;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 213);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(114, 13);
            this.label5.TabIndex = 78;
            this.label5.Text = "Authorized user names";
            // 
            // textBox_UnauthorizedProcessNames
            // 
            this.textBox_UnauthorizedProcessNames.Location = new System.Drawing.Point(214, 173);
            this.textBox_UnauthorizedProcessNames.Name = "textBox_UnauthorizedProcessNames";
            this.textBox_UnauthorizedProcessNames.Size = new System.Drawing.Size(242, 20);
            this.textBox_UnauthorizedProcessNames.TabIndex = 74;
            this.textBox_UnauthorizedProcessNames.Text = "explorer.exe;";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(17, 173);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(147, 13);
            this.label11.TabIndex = 77;
            this.label11.Text = "Unauthorized  process names";
            // 
            // textBox_TargetName
            // 
            this.textBox_TargetName.Location = new System.Drawing.Point(214, 54);
            this.textBox_TargetName.Name = "textBox_TargetName";
            this.textBox_TargetName.Size = new System.Drawing.Size(242, 20);
            this.textBox_TargetName.TabIndex = 72;
            this.textBox_TargetName.Text = "c:\\test\\test.txt";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 54);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(118, 13);
            this.label6.TabIndex = 73;
            this.label6.Text = "Target shared file name";
            // 
            // dateTimePicker_ExpireDate
            // 
            this.dateTimePicker_ExpireDate.CustomFormat = "yyyy-MM-dd";
            this.dateTimePicker_ExpireDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker_ExpireDate.Location = new System.Drawing.Point(212, 90);
            this.dateTimePicker_ExpireDate.Name = "dateTimePicker_ExpireDate";
            this.dateTimePicker_ExpireDate.Size = new System.Drawing.Size(120, 20);
            this.dateTimePicker_ExpireDate.TabIndex = 71;
            // 
            // button_OpenFile
            // 
            this.button_OpenFile.Location = new System.Drawing.Point(470, 18);
            this.button_OpenFile.Name = "button_OpenFile";
            this.button_OpenFile.Size = new System.Drawing.Size(51, 23);
            this.button_OpenFile.TabIndex = 3;
            this.button_OpenFile.Text = "browse";
            this.button_OpenFile.UseVisualStyleBackColor = true;
            this.button_OpenFile.Click += new System.EventHandler(this.button_OpenFile_Click);
            // 
            // dateTimePicker_ExpireTime
            // 
            this.dateTimePicker_ExpireTime.CustomFormat = " HH:mm:ss";
            this.dateTimePicker_ExpireTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePicker_ExpireTime.Location = new System.Drawing.Point(335, 90);
            this.dateTimePicker_ExpireTime.Name = "dateTimePicker_ExpireTime";
            this.dateTimePicker_ExpireTime.ShowUpDown = true;
            this.dateTimePicker_ExpireTime.Size = new System.Drawing.Size(120, 20);
            this.dateTimePicker_ExpireTime.TabIndex = 5;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(211, 151);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(246, 12);
            this.label12.TabIndex = 57;
            this.label12.Text = "( split with \';\' , process format \"notepad.exe;wordpad.exe\" )";
            // 
            // textBox_authorizedProcessNames
            // 
            this.textBox_authorizedProcessNames.Location = new System.Drawing.Point(213, 127);
            this.textBox_authorizedProcessNames.Name = "textBox_authorizedProcessNames";
            this.textBox_authorizedProcessNames.Size = new System.Drawing.Size(242, 20);
            this.textBox_authorizedProcessNames.TabIndex = 6;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(16, 130);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(131, 13);
            this.label10.TabIndex = 53;
            this.label10.Text = "Authorized process names";
            // 
            // button_CreateFile
            // 
            this.button_CreateFile.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_CreateFile.Location = new System.Drawing.Point(419, 335);
            this.button_CreateFile.Name = "button_CreateFile";
            this.button_CreateFile.Size = new System.Drawing.Size(114, 23);
            this.button_CreateFile.TabIndex = 15;
            this.button_CreateFile.Text = "Create Share File";
            this.button_CreateFile.UseVisualStyleBackColor = true;
            this.button_CreateFile.Click += new System.EventHandler(this.button_CreateFile_Click);
            // 
            // ShareFileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(571, 394);
            this.Controls.Add(this.button_CreateFile);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ShareFileForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "EaseFilter Secure File Creation";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_FileName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox_authorizedProcessNames;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.DateTimePicker dateTimePicker_ExpireTime;
        private System.Windows.Forms.Button button_OpenFile;
        private System.Windows.Forms.Button button_CreateFile;
        private System.Windows.Forms.DateTimePicker dateTimePicker_ExpireDate;
        private System.Windows.Forms.TextBox textBox_TargetName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_UnauthorizedUserNames;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_AuthorizedUserNames;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_UnauthorizedProcessNames;
        private System.Windows.Forms.Label label11;
    }
}