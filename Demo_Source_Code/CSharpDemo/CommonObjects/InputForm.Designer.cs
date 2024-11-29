namespace EaseFilter.CommonObjects
{
    partial class InputForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InputForm));
            this.label_InputPrompt = new System.Windows.Forms.Label();
            this.textBox_Input = new System.Windows.Forms.TextBox();
            this.button_Ok = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button_GetFilePath = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label_InputPrompt
            // 
            this.label_InputPrompt.AutoSize = true;
            this.label_InputPrompt.Location = new System.Drawing.Point(20, 8);
            this.label_InputPrompt.Name = "label_InputPrompt";
            this.label_InputPrompt.Size = new System.Drawing.Size(117, 13);
            this.label_InputPrompt.TabIndex = 0;
            this.label_InputPrompt.Text = "Input the new file name";
            // 
            // textBox_Input
            // 
            this.textBox_Input.Location = new System.Drawing.Point(23, 33);
            this.textBox_Input.Name = "textBox_Input";
            this.textBox_Input.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.textBox_Input.Size = new System.Drawing.Size(446, 20);
            this.textBox_Input.TabIndex = 1;
            // 
            // button_Ok
            // 
            this.button_Ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_Ok.Location = new System.Drawing.Point(394, 59);
            this.button_Ok.Name = "button_Ok";
            this.button_Ok.Size = new System.Drawing.Size(75, 20);
            this.button_Ok.TabIndex = 2;
            this.button_Ok.Text = "Ok";
            this.button_Ok.UseVisualStyleBackColor = true;
            this.button_Ok.Click += new System.EventHandler(this.button_Ok_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // button_GetFilePath
            // 
            this.button_GetFilePath.Location = new System.Drawing.Point(475, 33);
            this.button_GetFilePath.Name = "button_GetFilePath";
            this.button_GetFilePath.Size = new System.Drawing.Size(27, 20);
            this.button_GetFilePath.TabIndex = 3;
            this.button_GetFilePath.Text = "...";
            this.button_GetFilePath.UseVisualStyleBackColor = true;
            this.button_GetFilePath.Click += new System.EventHandler(this.button_GetFilePath_Click);
            // 
            // InputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 91);
            this.Controls.Add(this.button_GetFilePath);
            this.Controls.Add(this.button_Ok);
            this.Controls.Add(this.textBox_Input);
            this.Controls.Add(this.label_InputPrompt);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "InputForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Rename";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_InputPrompt;
        private System.Windows.Forms.TextBox textBox_Input;
        private System.Windows.Forms.Button button_Ok;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button_GetFilePath;
    }
}