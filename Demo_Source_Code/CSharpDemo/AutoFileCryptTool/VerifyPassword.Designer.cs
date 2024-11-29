namespace AutoFileCryptTool
{
    partial class VerifyPasswordForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VerifyPasswordForm));
            this.button_VerifyPassword = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_Password1 = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_VerifyPassword
            // 
            this.button_VerifyPassword.Location = new System.Drawing.Point(281, 84);
            this.button_VerifyPassword.Name = "button_VerifyPassword";
            this.button_VerifyPassword.Size = new System.Drawing.Size(75, 23);
            this.button_VerifyPassword.TabIndex = 6;
            this.button_VerifyPassword.Text = "Ok";
            this.button_VerifyPassword.UseVisualStyleBackColor = true;
            this.button_VerifyPassword.Click += new System.EventHandler(this.button_VerifyPassword_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBox_Password1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(359, 60);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
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
            // textBox_Password1
            // 
            this.textBox_Password1.Location = new System.Drawing.Point(140, 16);
            this.textBox_Password1.Name = "textBox_Password1";
            this.textBox_Password1.Size = new System.Drawing.Size(204, 20);
            this.textBox_Password1.TabIndex = 2;
            this.textBox_Password1.UseSystemPasswordChar = true;
            // 
            // VerifyPassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 119);
            this.Controls.Add(this.button_VerifyPassword);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "VerifyPassword";
            this.Text = "Verify Password";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_VerifyPassword;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_Password1;
    }
}