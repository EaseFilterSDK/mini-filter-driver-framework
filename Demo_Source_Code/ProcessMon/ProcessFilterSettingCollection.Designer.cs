namespace ProcessMon
{
    partial class ProcessFilterSettingCollection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProcessFilterSettingCollection));
            this.groupBox_AccessControl = new System.Windows.Forms.GroupBox();
            this.button_ModifyFilter = new System.Windows.Forms.Button();
            this.button_Apply = new System.Windows.Forms.Button();
            this.button_DeleteFilter = new System.Windows.Forms.Button();
            this.button_AddFilter = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listView_FilterRules = new System.Windows.Forms.ListView();
            this.groupBox_AccessControl.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox_AccessControl
            // 
            this.groupBox_AccessControl.Controls.Add(this.button_ModifyFilter);
            this.groupBox_AccessControl.Controls.Add(this.button_Apply);
            this.groupBox_AccessControl.Controls.Add(this.button_DeleteFilter);
            this.groupBox_AccessControl.Controls.Add(this.button_AddFilter);
            this.groupBox_AccessControl.Controls.Add(this.groupBox1);
            this.groupBox_AccessControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_AccessControl.Location = new System.Drawing.Point(0, 0);
            this.groupBox_AccessControl.Name = "groupBox_AccessControl";
            this.groupBox_AccessControl.Size = new System.Drawing.Size(537, 445);
            this.groupBox_AccessControl.TabIndex = 26;
            this.groupBox_AccessControl.TabStop = false;
            // 
            // button_ModifyFilter
            // 
            this.button_ModifyFilter.Location = new System.Drawing.Point(139, 399);
            this.button_ModifyFilter.Name = "button_ModifyFilter";
            this.button_ModifyFilter.Size = new System.Drawing.Size(103, 23);
            this.button_ModifyFilter.TabIndex = 85;
            this.button_ModifyFilter.Text = "Modify Filter Rule";
            this.button_ModifyFilter.UseVisualStyleBackColor = true;
            this.button_ModifyFilter.Click += new System.EventHandler(this.button_ModifyFilter_Click);
            // 
            // button_Apply
            // 
            this.button_Apply.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_Apply.Location = new System.Drawing.Point(383, 399);
            this.button_Apply.Name = "button_Apply";
            this.button_Apply.Size = new System.Drawing.Size(138, 23);
            this.button_Apply.TabIndex = 84;
            this.button_Apply.Text = "Apply Filter Rule Settings";
            this.button_Apply.UseVisualStyleBackColor = true;
            this.button_Apply.Click += new System.EventHandler(this.button_Apply_Click_1);
            // 
            // button_DeleteFilter
            // 
            this.button_DeleteFilter.Location = new System.Drawing.Point(257, 399);
            this.button_DeleteFilter.Name = "button_DeleteFilter";
            this.button_DeleteFilter.Size = new System.Drawing.Size(103, 23);
            this.button_DeleteFilter.TabIndex = 83;
            this.button_DeleteFilter.Text = "Delete Filter Rule";
            this.button_DeleteFilter.UseVisualStyleBackColor = true;
            this.button_DeleteFilter.Click += new System.EventHandler(this.button_DeleteFilter_Click);
            // 
            // button_AddFilter
            // 
            this.button_AddFilter.Location = new System.Drawing.Point(7, 399);
            this.button_AddFilter.Name = "button_AddFilter";
            this.button_AddFilter.Size = new System.Drawing.Size(116, 23);
            this.button_AddFilter.TabIndex = 82;
            this.button_AddFilter.Text = "Add New Filter Rule";
            this.button_AddFilter.UseVisualStyleBackColor = true;
            this.button_AddFilter.Click += new System.EventHandler(this.button_AddFilter_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listView_FilterRules);
            this.groupBox1.Location = new System.Drawing.Point(7, 32);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(517, 355);
            this.groupBox1.TabIndex = 81;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Process Filter Rule Collection";
            // 
            // listView_FilterRules
            // 
            this.listView_FilterRules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_FilterRules.FullRowSelect = true;
            this.listView_FilterRules.Location = new System.Drawing.Point(3, 16);
            this.listView_FilterRules.Name = "listView_FilterRules";
            this.listView_FilterRules.Size = new System.Drawing.Size(511, 336);
            this.listView_FilterRules.TabIndex = 1;
            this.listView_FilterRules.UseCompatibleStateImageBehavior = false;
            this.listView_FilterRules.View = System.Windows.Forms.View.Details;
            // 
            // ProcessFilterSettingCollection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(537, 445);
            this.Controls.Add(this.groupBox_AccessControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ProcessFilterSettingCollection";
            this.Text = "Process Filter Rule Settings";
            this.groupBox_AccessControl.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox_AccessControl;
        private System.Windows.Forms.Button button_Apply;
        private System.Windows.Forms.Button button_DeleteFilter;
        private System.Windows.Forms.Button button_AddFilter;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView listView_FilterRules;
        private System.Windows.Forms.Button button_ModifyFilter;
    }
}