namespace EaseFilter.FolderLocker
{
    partial class AccountForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AccountForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_EmailAddress = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox_Password = new System.Windows.Forms.TextBox();
            this.button_SignIn = new System.Windows.Forms.Button();
            this.button_SignUp = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter  email address";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 19);
            this.label2.TabIndex = 1;
            this.label2.Text = "Enter the password";
            // 
            // textBox_EmailAddress
            // 
            // 
            // 
            // 
            this.textBox_EmailAddress.Lines = new string[] {
        "user@myemail.com"};
            this.textBox_EmailAddress.Location = new System.Drawing.Point(176, 16);
            this.textBox_EmailAddress.Name = "textBox_EmailAddress";
            this.textBox_EmailAddress.PasswordChar = '\0';
            this.textBox_EmailAddress.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBox_EmailAddress.SelectedText = "";
            this.textBox_EmailAddress.SelectionLength = 0;
            this.textBox_EmailAddress.SelectionStart = 0;
            this.textBox_EmailAddress.ShortcutsEnabled = true;
            this.textBox_EmailAddress.Size = new System.Drawing.Size(162, 20);
            this.textBox_EmailAddress.TabIndex = 2;
            this.textBox_EmailAddress.Text = "user@myemail.com";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox_Password);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBox_EmailAddress);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 56);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(359, 86);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // textBox_Password
            // 
            // 
            // 
            // 
     
            this.textBox_Password.Lines = new string[0];
            this.textBox_Password.Location = new System.Drawing.Point(176, 53);
            this.textBox_Password.MaxLength = 32767;
            this.textBox_Password.Name = "textBox_Password";
            this.textBox_Password.PasswordChar = '●';
            this.textBox_Password.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBox_Password.SelectedText = "";
            this.textBox_Password.SelectionLength = 0;
            this.textBox_Password.SelectionStart = 0;
            this.textBox_Password.ShortcutsEnabled = true;
            this.textBox_Password.Size = new System.Drawing.Size(162, 20);
            this.textBox_Password.TabIndex = 3;
            this.textBox_Password.UseSystemPasswordChar = true;
            this.textBox_Password.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_Password_KeyDown);
            // 
            // button_SignIn
            // 
            this.button_SignIn.Location = new System.Drawing.Point(188, 160);
            this.button_SignIn.Name = "button_SignIn";
            this.button_SignIn.TabIndex = 4;
            this.button_SignIn.Text = "Sign In";
            this.button_SignIn.UseVisualStyleBackColor = true;
            this.button_SignIn.Click += new System.EventHandler(this.button_SignIn_Click);
            // 
            // button_SignUp
            // 
            this.button_SignUp.Location = new System.Drawing.Point(296, 160);
            this.button_SignUp.Name = "button_SignUp";
            this.button_SignUp.TabIndex = 5;
            this.button_SignUp.Text = "Register";
            this.button_SignUp.UseVisualStyleBackColor = true;
            this.button_SignUp.Click += new System.EventHandler(this.button_SignUp_Click);
            // 
            // AccountForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(391, 197);
            this.Controls.Add(this.button_SignUp);
            this.Controls.Add(this.button_SignIn);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AccountForm";
            this.Text = "Account Information";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private   System.Windows.Forms.Label label1;
        private   System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_EmailAddress;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox_Password;
        private System.Windows.Forms.Button button_SignIn;
        private System.Windows.Forms.Button button_SignUp;
    }
}