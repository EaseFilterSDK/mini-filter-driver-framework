namespace AutoFileCryptTool
{
    partial class SetupPasswordForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupPasswordForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_Password1 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox_Password2 = new System.Windows.Forms.TextBox();
            this.button_NewPassword = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter master password";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Confirm the password";
            // 
            // textBox_Password1
            // 
            this.textBox_Password1.Location = new System.Drawing.Point(176, 16);
            this.textBox_Password1.Name = "textBox_Password1";
            this.textBox_Password1.Size = new System.Drawing.Size(131, 20);
            this.textBox_Password1.TabIndex = 2;
            this.textBox_Password1.UseSystemPasswordChar = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox_Password2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBox_Password1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 23);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(359, 86);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // textBox_Password2
            // 
            this.textBox_Password2.Location = new System.Drawing.Point(176, 53);
            this.textBox_Password2.Name = "textBox_Password2";
            this.textBox_Password2.Size = new System.Drawing.Size(131, 20);
            this.textBox_Password2.TabIndex = 3;
            this.textBox_Password2.UseSystemPasswordChar = true;
            // 
            // button_NewPassword
            // 
            this.button_NewPassword.Location = new System.Drawing.Point(296, 127);
            this.button_NewPassword.Name = "button_NewPassword";
            this.button_NewPassword.Size = new System.Drawing.Size(75, 23);
            this.button_NewPassword.TabIndex = 4;
            this.button_NewPassword.Text = "Ok";
            this.button_NewPassword.UseVisualStyleBackColor = true;
            this.button_NewPassword.Click += new System.EventHandler(this.button_NewPassword_Click);
            // 
            // SetupPasswordForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(391, 162);
            this.Controls.Add(this.button_NewPassword);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SetupPasswordForm";
            this.Text = "Setup Master Password For First Usage";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_Password1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox_Password2;
        private System.Windows.Forms.Button button_NewPassword;
    }
}