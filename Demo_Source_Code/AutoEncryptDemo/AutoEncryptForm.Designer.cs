﻿namespace AutoEncryptDemo
{
    partial class AutoEncryptForm
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
            this.button_Start = new System.Windows.Forms.Button();
            this.textBox_EncrptFolder = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_AuthorizedProcessesForEncryptFolder = new System.Windows.Forms.TextBox();
            this.button_Stop = new System.Windows.Forms.Button();
            this.textBox_AuthorizedProcessesForDecryptFolder = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_DecryptFolder = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_PassPhrase = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_AuthorizedUsersForEncryptFolder = new System.Windows.Forms.TextBox();
            this.checkBox_EnableUniqueEncryptionKey = new System.Windows.Forms.CheckBox();
            this.button_EncryptFolder = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_Start
            // 
            this.button_Start.Location = new System.Drawing.Point(34, 363);
            this.button_Start.Name = "button_Start";
            this.button_Start.Size = new System.Drawing.Size(86, 23);
            this.button_Start.TabIndex = 0;
            this.button_Start.Text = "Start Service";
            this.button_Start.UseVisualStyleBackColor = true;
            this.button_Start.Click += new System.EventHandler(this.button_Start_Click);
            // 
            // textBox_EncrptFolder
            // 
            this.textBox_EncrptFolder.Location = new System.Drawing.Point(34, 42);
            this.textBox_EncrptFolder.Name = "textBox_EncrptFolder";
            this.textBox_EncrptFolder.Size = new System.Drawing.Size(320, 20);
            this.textBox_EncrptFolder.TabIndex = 1;
            this.textBox_EncrptFolder.Text = "c:\\AutoEncryptFolder";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(184, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Auto Encryption Folder In Computer A";
            // 
            // textBox_AuthorizedProcessesForEncryptFolder
            // 
            this.textBox_AuthorizedProcessesForEncryptFolder.Location = new System.Drawing.Point(34, 88);
            this.textBox_AuthorizedProcessesForEncryptFolder.Name = "textBox_AuthorizedProcessesForEncryptFolder";
            this.textBox_AuthorizedProcessesForEncryptFolder.Size = new System.Drawing.Size(320, 20);
            this.textBox_AuthorizedProcessesForEncryptFolder.TabIndex = 3;
            this.textBox_AuthorizedProcessesForEncryptFolder.Text = "notepad.exe;wordpad.exe";
            // 
            // button_Stop
            // 
            this.button_Stop.Location = new System.Drawing.Point(267, 363);
            this.button_Stop.Name = "button_Stop";
            this.button_Stop.Size = new System.Drawing.Size(87, 23);
            this.button_Stop.TabIndex = 5;
            this.button_Stop.Text = "Stop Service";
            this.button_Stop.UseVisualStyleBackColor = true;
            this.button_Stop.Click += new System.EventHandler(this.button_Stop_Click);
            // 
            // textBox_AuthorizedProcessesForDecryptFolder
            // 
            this.textBox_AuthorizedProcessesForDecryptFolder.Location = new System.Drawing.Point(34, 327);
            this.textBox_AuthorizedProcessesForDecryptFolder.Name = "textBox_AuthorizedProcessesForDecryptFolder";
            this.textBox_AuthorizedProcessesForDecryptFolder.Size = new System.Drawing.Size(320, 20);
            this.textBox_AuthorizedProcessesForDecryptFolder.TabIndex = 6;
            this.textBox_AuthorizedProcessesForDecryptFolder.Text = "notepad.exe;wordpad.exe";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(34, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(324, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Authorized Processes To Decrypt Files (* or empty for all processes)";
            // 
            // textBox_DecryptFolder
            // 
            this.textBox_DecryptFolder.Location = new System.Drawing.Point(34, 276);
            this.textBox_DecryptFolder.Name = "textBox_DecryptFolder";
            this.textBox_DecryptFolder.Size = new System.Drawing.Size(320, 20);
            this.textBox_DecryptFolder.TabIndex = 8;
            this.textBox_DecryptFolder.Text = "c:\\RawEncryptedFiles";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 260);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(323, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Auto DeEncryption Folder In Computer B ( copy encrypted file here)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 311);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(189, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Authorized Processes To Decrypt Files";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(34, 196);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(258, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "PassPhrase (must be the same for computer A and B)";
            // 
            // textBox_PassPhrase
            // 
            this.textBox_PassPhrase.Location = new System.Drawing.Point(34, 212);
            this.textBox_PassPhrase.Name = "textBox_PassPhrase";
            this.textBox_PassPhrase.Size = new System.Drawing.Size(320, 20);
            this.textBox_PassPhrase.TabIndex = 12;
            this.textBox_PassPhrase.Text = "your password";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(34, 116);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(267, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Authorized Users To Decrypt Files  (domain\\usename1)";
            // 
            // textBox_AuthorizedUsersForEncryptFolder
            // 
            this.textBox_AuthorizedUsersForEncryptFolder.Location = new System.Drawing.Point(34, 132);
            this.textBox_AuthorizedUsersForEncryptFolder.Name = "textBox_AuthorizedUsersForEncryptFolder";
            this.textBox_AuthorizedUsersForEncryptFolder.Size = new System.Drawing.Size(320, 20);
            this.textBox_AuthorizedUsersForEncryptFolder.TabIndex = 14;
            // 
            // checkBox_EnableUniqueEncryptionKey
            // 
            this.checkBox_EnableUniqueEncryptionKey.AutoSize = true;
            this.checkBox_EnableUniqueEncryptionKey.Location = new System.Drawing.Point(34, 167);
            this.checkBox_EnableUniqueEncryptionKey.Name = "checkBox_EnableUniqueEncryptionKey";
            this.checkBox_EnableUniqueEncryptionKey.Size = new System.Drawing.Size(328, 17);
            this.checkBox_EnableUniqueEncryptionKey.TabIndex = 16;
            this.checkBox_EnableUniqueEncryptionKey.Text = "Authorize encryption or decryption in user mode application level";
            this.checkBox_EnableUniqueEncryptionKey.UseVisualStyleBackColor = true;
            this.checkBox_EnableUniqueEncryptionKey.CheckedChanged += new System.EventHandler(this.checkBox_EnableUniqueEncryptionKey_CheckedChanged);
            // 
            // button_EncryptFolder
            // 
            this.button_EncryptFolder.Location = new System.Drawing.Point(364, 40);
            this.button_EncryptFolder.Name = "button_EncryptFolder";
            this.button_EncryptFolder.Size = new System.Drawing.Size(26, 23);
            this.button_EncryptFolder.TabIndex = 17;
            this.button_EncryptFolder.Text = "...";
            this.button_EncryptFolder.UseVisualStyleBackColor = true;
            this.button_EncryptFolder.Click += new System.EventHandler(this.button_EncryptFolder_Click);
            // 
            // AutoEncryptForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 410);
            this.Controls.Add(this.button_EncryptFolder);
            this.Controls.Add(this.checkBox_EnableUniqueEncryptionKey);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBox_AuthorizedUsersForEncryptFolder);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox_PassPhrase);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox_DecryptFolder);
            this.Controls.Add(this.textBox_AuthorizedProcessesForDecryptFolder);
            this.Controls.Add(this.button_Stop);
            this.Controls.Add(this.textBox_AuthorizedProcessesForEncryptFolder);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_EncrptFolder);
            this.Controls.Add(this.button_Start);
            this.Name = "AutoEncryptForm";
            this.Text = "Auto Encryption Demo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AutoEncryptForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_Start;
        private System.Windows.Forms.TextBox textBox_EncrptFolder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_AuthorizedProcessesForEncryptFolder;
        private System.Windows.Forms.Button button_Stop;
        private System.Windows.Forms.TextBox textBox_AuthorizedProcessesForDecryptFolder;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_DecryptFolder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_PassPhrase;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox_AuthorizedUsersForEncryptFolder;
        private System.Windows.Forms.CheckBox checkBox_EnableUniqueEncryptionKey;
        private System.Windows.Forms.Button button_EncryptFolder;
    }
}

