namespace AutoEncryptDemo
{
    partial class DRMForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DRMForm));
            this.button_ApplySettings = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.textBox_authorizedProcessNames = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.dateTimePicker_ExpireTime = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker_ExpireDate = new System.Windows.Forms.DateTimePicker();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox_UnauthorizedProcessNames = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_AuthorizedUserNames = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_UnauthorizedUserNames = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_GetComputerId = new System.Windows.Forms.Button();
            this.textBox_ComputerId = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButton_StoreDRMToServer = new System.Windows.Forms.RadioButton();
            this.radioButton_EmbedDRM = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_ApplySettings
            // 
            this.button_ApplySettings.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_ApplySettings.Location = new System.Drawing.Point(353, 336);
            this.button_ApplySettings.Name = "button_ApplySettings";
            this.button_ApplySettings.Size = new System.Drawing.Size(114, 23);
            this.button_ApplySettings.TabIndex = 15;
            this.button_ApplySettings.Text = "Apply Settings";
            this.button_ApplySettings.UseVisualStyleBackColor = true;
            this.button_ApplySettings.Click += new System.EventHandler(this.button_ApplySettings_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Share file expire time";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(16, 137);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(131, 13);
            this.label10.TabIndex = 53;
            this.label10.Text = "Authorized process names";
            // 
            // textBox_authorizedProcessNames
            // 
            this.textBox_authorizedProcessNames.Location = new System.Drawing.Point(213, 134);
            this.textBox_authorizedProcessNames.Name = "textBox_authorizedProcessNames";
            this.textBox_authorizedProcessNames.Size = new System.Drawing.Size(242, 20);
            this.textBox_authorizedProcessNames.TabIndex = 6;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(211, 158);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(244, 12);
            this.label12.TabIndex = 57;
            this.label12.Text = "( split with \';\' , process format \"notepad.exe;wordpad.exe\")";
            // 
            // dateTimePicker_ExpireTime
            // 
            this.dateTimePicker_ExpireTime.CustomFormat = " HH:mm:ss";
            this.dateTimePicker_ExpireTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePicker_ExpireTime.Location = new System.Drawing.Point(335, 97);
            this.dateTimePicker_ExpireTime.Name = "dateTimePicker_ExpireTime";
            this.dateTimePicker_ExpireTime.ShowUpDown = true;
            this.dateTimePicker_ExpireTime.Size = new System.Drawing.Size(120, 20);
            this.dateTimePicker_ExpireTime.TabIndex = 5;
            // 
            // dateTimePicker_ExpireDate
            // 
            this.dateTimePicker_ExpireDate.CustomFormat = "yyyy-MM-dd";
            this.dateTimePicker_ExpireDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker_ExpireDate.Location = new System.Drawing.Point(212, 97);
            this.dateTimePicker_ExpireDate.Name = "dateTimePicker_ExpireDate";
            this.dateTimePicker_ExpireDate.Size = new System.Drawing.Size(120, 20);
            this.dateTimePicker_ExpireDate.TabIndex = 71;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(17, 180);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(147, 13);
            this.label11.TabIndex = 77;
            this.label11.Text = "Unauthorized  process names";
            // 
            // textBox_UnauthorizedProcessNames
            // 
            this.textBox_UnauthorizedProcessNames.Location = new System.Drawing.Point(214, 180);
            this.textBox_UnauthorizedProcessNames.Name = "textBox_UnauthorizedProcessNames";
            this.textBox_UnauthorizedProcessNames.Size = new System.Drawing.Size(242, 20);
            this.textBox_UnauthorizedProcessNames.TabIndex = 74;
            this.textBox_UnauthorizedProcessNames.Text = "explorer.exe;";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 220);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(114, 13);
            this.label5.TabIndex = 78;
            this.label5.Text = "Authorized user names";
            // 
            // textBox_AuthorizedUserNames
            // 
            this.textBox_AuthorizedUserNames.Location = new System.Drawing.Point(214, 217);
            this.textBox_AuthorizedUserNames.Name = "textBox_AuthorizedUserNames";
            this.textBox_AuthorizedUserNames.Size = new System.Drawing.Size(242, 20);
            this.textBox_AuthorizedUserNames.TabIndex = 75;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 260);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(127, 13);
            this.label4.TabIndex = 79;
            this.label4.Text = "Unauthorized user names";
            // 
            // textBox_UnauthorizedUserNames
            // 
            this.textBox_UnauthorizedUserNames.Location = new System.Drawing.Point(214, 260);
            this.textBox_UnauthorizedUserNames.Name = "textBox_UnauthorizedUserNames";
            this.textBox_UnauthorizedUserNames.Size = new System.Drawing.Size(242, 20);
            this.textBox_UnauthorizedUserNames.TabIndex = 76;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(209, 237);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(195, 12);
            this.label3.TabIndex = 80;
            this.label3.Text = "(split with \';\' ,user name format \"domain\\user\" )";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_GetComputerId);
            this.groupBox1.Controls.Add(this.textBox_ComputerId);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBox_UnauthorizedUserNames);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textBox_AuthorizedUserNames);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.textBox_UnauthorizedProcessNames);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.dateTimePicker_ExpireDate);
            this.groupBox1.Controls.Add(this.dateTimePicker_ExpireTime);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.textBox_authorizedProcessNames);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(488, 309);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            // 
            // button_GetComputerId
            // 
            this.button_GetComputerId.Location = new System.Drawing.Point(457, 61);
            this.button_GetComputerId.Name = "button_GetComputerId";
            this.button_GetComputerId.Size = new System.Drawing.Size(25, 23);
            this.button_GetComputerId.TabIndex = 16;
            this.button_GetComputerId.Text = "...";
            this.button_GetComputerId.UseVisualStyleBackColor = true;
            this.button_GetComputerId.Click += new System.EventHandler(this.button_GetComputerId_Click);
            // 
            // textBox_ComputerId
            // 
            this.textBox_ComputerId.Location = new System.Drawing.Point(212, 61);
            this.textBox_ComputerId.Name = "textBox_ComputerId";
            this.textBox_ComputerId.Size = new System.Drawing.Size(242, 20);
            this.textBox_ComputerId.TabIndex = 82;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 64);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(116, 13);
            this.label6.TabIndex = 83;
            this.label6.Text = "Authorized computer Id";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButton_StoreDRMToServer);
            this.groupBox2.Controls.Add(this.radioButton_EmbedDRM);
            this.groupBox2.Location = new System.Drawing.Point(19, 19);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(436, 36);
            this.groupBox2.TabIndex = 81;
            this.groupBox2.TabStop = false;
            // 
            // radioButton_StoreDRMToServer
            // 
            this.radioButton_StoreDRMToServer.AutoSize = true;
            this.radioButton_StoreDRMToServer.Location = new System.Drawing.Point(194, 13);
            this.radioButton_StoreDRMToServer.Name = "radioButton_StoreDRMToServer";
            this.radioButton_StoreDRMToServer.Size = new System.Drawing.Size(146, 17);
            this.radioButton_StoreDRMToServer.TabIndex = 1;
            this.radioButton_StoreDRMToServer.Text = "Store DRM data to server";
            this.radioButton_StoreDRMToServer.UseVisualStyleBackColor = true;
            // 
            // radioButton_EmbedDRM
            // 
            this.radioButton_EmbedDRM.AutoSize = true;
            this.radioButton_EmbedDRM.Checked = true;
            this.radioButton_EmbedDRM.Location = new System.Drawing.Point(7, 13);
            this.radioButton_EmbedDRM.Name = "radioButton_EmbedDRM";
            this.radioButton_EmbedDRM.Size = new System.Drawing.Size(164, 17);
            this.radioButton_EmbedDRM.TabIndex = 0;
            this.radioButton_EmbedDRM.TabStop = true;
            this.radioButton_EmbedDRM.Text = "Embed DRM to encrypted file";
            this.radioButton_EmbedDRM.UseVisualStyleBackColor = true;
            // 
            // DRMForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 394);
            this.Controls.Add(this.button_ApplySettings);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DRMForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Embed DRM data to the encrypted file";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_ApplySettings;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBox_authorizedProcessNames;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.DateTimePicker dateTimePicker_ExpireTime;
        private System.Windows.Forms.DateTimePicker dateTimePicker_ExpireDate;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBox_UnauthorizedProcessNames;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_AuthorizedUserNames;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_UnauthorizedUserNames;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox_ComputerId;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioButton_StoreDRMToServer;
        private System.Windows.Forms.RadioButton radioButton_EmbedDRM;
        private System.Windows.Forms.Button button_GetComputerId;
    }
}