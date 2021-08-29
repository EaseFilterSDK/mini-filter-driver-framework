namespace FileProtector
{
    partial class ShareEncryptedFileCreationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShareEncryptedFileCreationForm));
            this.button_CreateShareEncryptedFile = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_FileName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_OpenFile = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_ExcludeUserNames = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_IncludeUserNames = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label_AccessFlags = new System.Windows.Forms.Label();
            this.dateTimePicker_ExpireTime = new System.Windows.Forms.DateTimePicker();
            this.label12 = new System.Windows.Forms.Label();
            this.textBox_FileAccessFlags = new System.Windows.Forms.TextBox();
            this.button_FileAccessFlags = new System.Windows.Forms.Button();
            this.textBox_ExcludeProcessNames = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox_IncludeProcessNames = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_CreateShareEncryptedFile
            // 
            this.button_CreateShareEncryptedFile.Location = new System.Drawing.Point(509, 418);
            this.button_CreateShareEncryptedFile.Name = "button_CreateShareEncryptedFile";
            this.button_CreateShareEncryptedFile.Size = new System.Drawing.Size(75, 23);
            this.button_CreateShareEncryptedFile.TabIndex = 8;
            this.button_CreateShareEncryptedFile.Text = "Start";
            this.button_CreateShareEncryptedFile.UseVisualStyleBackColor = true;
            this.button_CreateShareEncryptedFile.Click += new System.EventHandler(this.button_CreateShareEncryptedFile_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Share file expire time";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Input file name path";
            // 
            // textBox_FileName
            // 
            this.textBox_FileName.Location = new System.Drawing.Point(213, 13);
            this.textBox_FileName.Name = "textBox_FileName";
            this.textBox_FileName.Size = new System.Drawing.Size(242, 20);
            this.textBox_FileName.TabIndex = 0;
            this.textBox_FileName.Text = "c:\\test\\test.txt";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.button_OpenFile);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBox_ExcludeUserNames);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textBox_IncludeUserNames);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label_AccessFlags);
            this.groupBox1.Controls.Add(this.dateTimePicker_ExpireTime);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.textBox_FileAccessFlags);
            this.groupBox1.Controls.Add(this.button_FileAccessFlags);
            this.groupBox1.Controls.Add(this.textBox_ExcludeProcessNames);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.textBox_IncludeProcessNames);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox_FileName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(572, 386);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            // 
            // button_OpenFile
            // 
            this.button_OpenFile.Location = new System.Drawing.Point(461, 11);
            this.button_OpenFile.Name = "button_OpenFile";
            this.button_OpenFile.Size = new System.Drawing.Size(75, 23);
            this.button_OpenFile.TabIndex = 64;
            this.button_OpenFile.Text = "browse";
            this.button_OpenFile.UseVisualStyleBackColor = true;
            this.button_OpenFile.Click += new System.EventHandler(this.button_OpenFile_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(211, 233);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(215, 12);
            this.label3.TabIndex = 63;
            this.label3.Text = "(Seperate with \';\' ,user name format \"domain\\user\" )";
            // 
            // textBox_ExcludeUserNames
            // 
            this.textBox_ExcludeUserNames.Location = new System.Drawing.Point(213, 253);
            this.textBox_ExcludeUserNames.Name = "textBox_ExcludeUserNames";
            this.textBox_ExcludeUserNames.Size = new System.Drawing.Size(242, 20);
            this.textBox_ExcludeUserNames.TabIndex = 62;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 253);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(140, 13);
            this.label4.TabIndex = 61;
            this.label4.Text = "Disallow access user names";
            // 
            // textBox_IncludeUserNames
            // 
            this.textBox_IncludeUserNames.Location = new System.Drawing.Point(213, 213);
            this.textBox_IncludeUserNames.Name = "textBox_IncludeUserNames";
            this.textBox_IncludeUserNames.Size = new System.Drawing.Size(242, 20);
            this.textBox_IncludeUserNames.TabIndex = 60;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 213);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(126, 13);
            this.label5.TabIndex = 59;
            this.label5.Text = "Allow access user names";
            // 
            // label_AccessFlags
            // 
            this.label_AccessFlags.AutoSize = true;
            this.label_AccessFlags.Location = new System.Drawing.Point(16, 333);
            this.label_AccessFlags.Name = "label_AccessFlags";
            this.label_AccessFlags.Size = new System.Drawing.Size(120, 13);
            this.label_AccessFlags.TabIndex = 12;
            this.label_AccessFlags.Text = "File access control flags";
            // 
            // dateTimePicker_ExpireTime
            // 
            this.dateTimePicker_ExpireTime.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dateTimePicker_ExpireTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker_ExpireTime.Location = new System.Drawing.Point(213, 93);
            this.dateTimePicker_ExpireTime.Name = "dateTimePicker_ExpireTime";
            this.dateTimePicker_ExpireTime.Size = new System.Drawing.Size(242, 20);
            this.dateTimePicker_ExpireTime.TabIndex = 58;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(211, 154);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(209, 12);
            this.label12.TabIndex = 57;
            this.label12.Text = "( Seperate with \';\' , process format \"notepad.exe\" )";
            // 
            // textBox_FileAccessFlags
            // 
            this.textBox_FileAccessFlags.Location = new System.Drawing.Point(213, 333);
            this.textBox_FileAccessFlags.Name = "textBox_FileAccessFlags";
            this.textBox_FileAccessFlags.Size = new System.Drawing.Size(242, 20);
            this.textBox_FileAccessFlags.TabIndex = 11;
            this.textBox_FileAccessFlags.Text = "0";
            // 
            // button_FileAccessFlags
            // 
            this.button_FileAccessFlags.Location = new System.Drawing.Point(461, 333);
            this.button_FileAccessFlags.Name = "button_FileAccessFlags";
            this.button_FileAccessFlags.Size = new System.Drawing.Size(41, 20);
            this.button_FileAccessFlags.TabIndex = 14;
            this.button_FileAccessFlags.Text = "...";
            this.button_FileAccessFlags.UseVisualStyleBackColor = true;
            this.button_FileAccessFlags.Click += new System.EventHandler(this.button_FileAccessFlags_Click);
            // 
            // textBox_ExcludeProcessNames
            // 
            this.textBox_ExcludeProcessNames.Location = new System.Drawing.Point(213, 173);
            this.textBox_ExcludeProcessNames.Name = "textBox_ExcludeProcessNames";
            this.textBox_ExcludeProcessNames.Size = new System.Drawing.Size(242, 20);
            this.textBox_ExcludeProcessNames.TabIndex = 56;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(16, 173);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(160, 13);
            this.label11.TabIndex = 55;
            this.label11.Text = "Disallow access  process names";
            // 
            // textBox_IncludeProcessNames
            // 
            this.textBox_IncludeProcessNames.Location = new System.Drawing.Point(213, 133);
            this.textBox_IncludeProcessNames.Name = "textBox_IncludeProcessNames";
            this.textBox_IncludeProcessNames.Size = new System.Drawing.Size(242, 20);
            this.textBox_IncludeProcessNames.TabIndex = 54;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(16, 133);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(143, 13);
            this.label10.TabIndex = 53;
            this.label10.Text = "Allow access process names";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(213, 53);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(242, 20);
            this.textBox1.TabIndex = 65;
            this.textBox1.Text = "c:\\test\\test.txt";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 53);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(108, 13);
            this.label6.TabIndex = 66;
            this.label6.Text = "Output file name path";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(212, 293);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(242, 20);
            this.textBox2.TabIndex = 68;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(16, 293);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(128, 13);
            this.label8.TabIndex = 67;
            this.label8.Text = "Allow access computer Id";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(461, 293);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(41, 20);
            this.button1.TabIndex = 69;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // ShareEncryptedFileCreationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(596, 453);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_CreateShareEncryptedFile);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ShareEncryptedFileCreationForm";
            this.Text = "Create share encrypted file";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_FileName;
        private System.Windows.Forms.Button button_CreateShareEncryptedFile;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox_FileAccessFlags;
        private System.Windows.Forms.Label label_AccessFlags;
        private System.Windows.Forms.Button button_FileAccessFlags;
        private System.Windows.Forms.TextBox textBox_ExcludeProcessNames;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBox_IncludeProcessNames;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_ExcludeUserNames;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_IncludeUserNames;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dateTimePicker_ExpireTime;
        private System.Windows.Forms.Button button_OpenFile;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label8;
    }
}