namespace FileProtector
{
    partial class EncryptedFileForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EncryptedFileForm));
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_PassPhrase = new System.Windows.Forms.TextBox();
            this.button_Start = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox_TargetFileName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button_OpenFile = new System.Windows.Forms.Button();
            this.checkBox_DisplayPassword = new System.Windows.Forms.CheckBox();
            this.textBox_FileName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Password phrase";
            // 
            // textBox_PassPhrase
            // 
            this.textBox_PassPhrase.Location = new System.Drawing.Point(172, 23);
            this.textBox_PassPhrase.Name = "textBox_PassPhrase";
            this.textBox_PassPhrase.Size = new System.Drawing.Size(206, 20);
            this.textBox_PassPhrase.TabIndex = 1;
            this.textBox_PassPhrase.UseSystemPasswordChar = true;
            // 
            // button_Start
            // 
            this.button_Start.Location = new System.Drawing.Point(469, 223);
            this.button_Start.Name = "button_Start";
            this.button_Start.Size = new System.Drawing.Size(75, 23);
            this.button_Start.TabIndex = 2;
            this.button_Start.Text = "Start";
            this.button_Start.UseVisualStyleBackColor = true;
            this.button_Start.Click += new System.EventHandler(this.button_Start_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox_TargetFileName);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.button_OpenFile);
            this.groupBox1.Controls.Add(this.checkBox_DisplayPassword);
            this.groupBox1.Controls.Add(this.textBox_FileName);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox_PassPhrase);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(23, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(521, 194);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // textBox_TargetFileName
            // 
            this.textBox_TargetFileName.Location = new System.Drawing.Point(172, 134);
            this.textBox_TargetFileName.Name = "textBox_TargetFileName";
            this.textBox_TargetFileName.Size = new System.Drawing.Size(206, 20);
            this.textBox_TargetFileName.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 134);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 15);
            this.label5.TabIndex = 9;
            this.label5.Text = "Target File name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(169, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(153, 15);
            this.label4.TabIndex = 8;
            this.label4.Text = "(default key size is 256bits)";
            // 
            // button_OpenFile
            // 
            this.button_OpenFile.Location = new System.Drawing.Point(407, 86);
            this.button_OpenFile.Name = "button_OpenFile";
            this.button_OpenFile.Size = new System.Drawing.Size(75, 23);
            this.button_OpenFile.TabIndex = 7;
            this.button_OpenFile.Text = "browse";
            this.button_OpenFile.UseVisualStyleBackColor = true;
            this.button_OpenFile.Click += new System.EventHandler(this.button_OpenFile_Click);
            // 
            // checkBox_DisplayPassword
            // 
            this.checkBox_DisplayPassword.AutoSize = true;
            this.checkBox_DisplayPassword.Location = new System.Drawing.Point(393, 26);
            this.checkBox_DisplayPassword.Name = "checkBox_DisplayPassword";
            this.checkBox_DisplayPassword.Size = new System.Drawing.Size(125, 19);
            this.checkBox_DisplayPassword.TabIndex = 5;
            this.checkBox_DisplayPassword.Text = "Display password";
            this.checkBox_DisplayPassword.UseVisualStyleBackColor = true;
            this.checkBox_DisplayPassword.CheckedChanged += new System.EventHandler(this.checkBox_DisplayPassword_CheckedChanged);
            // 
            // textBox_FileName
            // 
            this.textBox_FileName.Location = new System.Drawing.Point(172, 88);
            this.textBox_FileName.Name = "textBox_FileName";
            this.textBox_FileName.Size = new System.Drawing.Size(206, 20);
            this.textBox_FileName.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 15);
            this.label3.TabIndex = 3;
            this.label3.Text = "Source File name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(169, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(245, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "(it must be the same as the one in filter rule)";
            // 
            // EncryptedFileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(573, 272);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_Start);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EncryptedFileForm";
            this.Text = "EncryptionForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_PassPhrase;
        private System.Windows.Forms.Button button_Start;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox_FileName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBox_DisplayPassword;
        private System.Windows.Forms.Button button_OpenFile;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_TargetFileName;
        private System.Windows.Forms.Label label5;
    }
}