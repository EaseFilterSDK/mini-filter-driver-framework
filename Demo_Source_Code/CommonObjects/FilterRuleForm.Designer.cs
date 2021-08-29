namespace EaseFilter.CommonObjects
{
    partial class FilterRuleForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FilterRuleForm));
            this.label3 = new System.Windows.Forms.Label();
            this.button_SaveFilterRule = new System.Windows.Forms.Button();
            this.textBox_ExcludeFilterMask = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_IncludeFilterMask = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_ControlSettings = new System.Windows.Forms.Button();
            this.groupBox_AccessControl = new System.Windows.Forms.GroupBox();
            this.button_InfoMonitorIO = new System.Windows.Forms.Button();
            this.button_InfoFileEvents = new System.Windows.Forms.Button();
            this.textBox_SelectedEvents = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.button_SelectedEvents = new System.Windows.Forms.Button();
            this.button_RegisterMonitorIO = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.textBox_MonitorIO = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.textBox_ExcludeUserNames = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.textBox_IncludeUserNames = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.textBox_ExcludeProcessNames = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox_IncludeProcessNames = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.button_SelectExcludePID = new System.Windows.Forms.Button();
            this.textBox_ExcludePID = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button_SelectIncludePID = new System.Windows.Forms.Button();
            this.textBox_IncludePID = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.button_FilterCreateOptions = new System.Windows.Forms.Button();
            this.textBox_FilterCreateOptions = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.button_FilterDisposition = new System.Windows.Forms.Button();
            this.textBox_FilterDisposition = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.button_FilterDesiredAccess = new System.Windows.Forms.Button();
            this.textBox_FilterDesiredAccess = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button_InfoExcludeUserName = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button_InfoCreatOptions = new System.Windows.Forms.Button();
            this.button_InfoDisposition = new System.Windows.Forms.Button();
            this.button_InfoDesiredAccess = new System.Windows.Forms.Button();
            this.button_InfoIncludeUserName = new System.Windows.Forms.Button();
            this.button_InfoExcludeFileFilterMask = new System.Windows.Forms.Button();
            this.button_InfoIncludeProcessName = new System.Windows.Forms.Button();
            this.button_InfoExcludeProcessName = new System.Windows.Forms.Button();
            this.button_InfoFilterMask = new System.Windows.Forms.Button();
            this.checkBox_MonitorEventBuffer = new System.Windows.Forms.CheckBox();
            this.button_MonitorBufferInfo = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox_AccessControl.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(198, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "( split with \';\' for multiple items)";
            // 
            // button_SaveFilterRule
            // 
            this.button_SaveFilterRule.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_SaveFilterRule.Location = new System.Drawing.Point(439, 560);
            this.button_SaveFilterRule.Name = "button_SaveFilterRule";
            this.button_SaveFilterRule.Size = new System.Drawing.Size(75, 23);
            this.button_SaveFilterRule.TabIndex = 8;
            this.button_SaveFilterRule.Text = "Save";
            this.button_SaveFilterRule.UseVisualStyleBackColor = true;
            this.button_SaveFilterRule.Click += new System.EventHandler(this.button_SaveFilter_Click);
            // 
            // textBox_ExcludeFilterMask
            // 
            this.textBox_ExcludeFilterMask.Location = new System.Drawing.Point(200, 46);
            this.textBox_ExcludeFilterMask.Name = "textBox_ExcludeFilterMask";
            this.textBox_ExcludeFilterMask.Size = new System.Drawing.Size(242, 20);
            this.textBox_ExcludeFilterMask.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Exclude file filter mask";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Manage file filter mask";
            // 
            // textBox_IncludeFilterMask
            // 
            this.textBox_IncludeFilterMask.Location = new System.Drawing.Point(200, 16);
            this.textBox_IncludeFilterMask.Name = "textBox_IncludeFilterMask";
            this.textBox_IncludeFilterMask.Size = new System.Drawing.Size(242, 20);
            this.textBox_IncludeFilterMask.TabIndex = 0;
            this.textBox_IncludeFilterMask.Text = "c:\\test\\*";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_ControlSettings);
            this.groupBox1.Location = new System.Drawing.Point(12, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(547, 552);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            // 
            // button_ControlSettings
            // 
            this.button_ControlSettings.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.button_ControlSettings.Location = new System.Drawing.Point(12, 523);
            this.button_ControlSettings.Name = "button_ControlSettings";
            this.button_ControlSettings.Size = new System.Drawing.Size(489, 23);
            this.button_ControlSettings.TabIndex = 12;
            this.button_ControlSettings.Text = "Configure the control filter rule settings";
            this.button_ControlSettings.UseVisualStyleBackColor = false;
            this.button_ControlSettings.Click += new System.EventHandler(this.button_ControlSettings_Click);
            // 
            // groupBox_AccessControl
            // 
            this.groupBox_AccessControl.Controls.Add(this.button_MonitorBufferInfo);
            this.groupBox_AccessControl.Controls.Add(this.checkBox_MonitorEventBuffer);
            this.groupBox_AccessControl.Controls.Add(this.button_InfoMonitorIO);
            this.groupBox_AccessControl.Controls.Add(this.button_InfoFileEvents);
            this.groupBox_AccessControl.Controls.Add(this.textBox_SelectedEvents);
            this.groupBox_AccessControl.Controls.Add(this.label9);
            this.groupBox_AccessControl.Controls.Add(this.label18);
            this.groupBox_AccessControl.Controls.Add(this.button_SelectedEvents);
            this.groupBox_AccessControl.Controls.Add(this.button_RegisterMonitorIO);
            this.groupBox_AccessControl.Controls.Add(this.label16);
            this.groupBox_AccessControl.Controls.Add(this.textBox_MonitorIO);
            this.groupBox_AccessControl.Location = new System.Drawing.Point(12, 403);
            this.groupBox_AccessControl.Name = "groupBox_AccessControl";
            this.groupBox_AccessControl.Size = new System.Drawing.Size(547, 116);
            this.groupBox_AccessControl.TabIndex = 24;
            this.groupBox_AccessControl.TabStop = false;
            this.groupBox_AccessControl.Text = "Monitor Filter Only Settings";
            // 
            // button_InfoMonitorIO
            // 
            this.button_InfoMonitorIO.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
            this.button_InfoMonitorIO.Location = new System.Drawing.Point(513, 56);
            this.button_InfoMonitorIO.Name = "button_InfoMonitorIO";
            this.button_InfoMonitorIO.Size = new System.Drawing.Size(28, 20);
            this.button_InfoMonitorIO.TabIndex = 118;
            this.button_InfoMonitorIO.UseVisualStyleBackColor = true;
            this.button_InfoMonitorIO.Click += new System.EventHandler(this.button_InfoMonitorIO_Click);
            // 
            // button_InfoFileEvents
            // 
            this.button_InfoFileEvents.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
            this.button_InfoFileEvents.Location = new System.Drawing.Point(513, 16);
            this.button_InfoFileEvents.Name = "button_InfoFileEvents";
            this.button_InfoFileEvents.Size = new System.Drawing.Size(28, 20);
            this.button_InfoFileEvents.TabIndex = 119;
            this.button_InfoFileEvents.UseVisualStyleBackColor = true;
            this.button_InfoFileEvents.Click += new System.EventHandler(this.button_InfoFileEvents_Click);
            // 
            // textBox_SelectedEvents
            // 
            this.textBox_SelectedEvents.Location = new System.Drawing.Point(200, 16);
            this.textBox_SelectedEvents.Name = "textBox_SelectedEvents";
            this.textBox_SelectedEvents.ReadOnly = true;
            this.textBox_SelectedEvents.Size = new System.Drawing.Size(242, 20);
            this.textBox_SelectedEvents.TabIndex = 51;
            this.textBox_SelectedEvents.Text = "0";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 23);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(97, 13);
            this.label9.TabIndex = 50;
            this.label9.Text = "Register file events";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(197, 39);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(237, 12);
            this.label18.TabIndex = 72;
            this.label18.Text = "(Receive file event notification after file handle is closed )";
            // 
            // button_SelectedEvents
            // 
            this.button_SelectedEvents.Location = new System.Drawing.Point(460, 16);
            this.button_SelectedEvents.Name = "button_SelectedEvents";
            this.button_SelectedEvents.Size = new System.Drawing.Size(41, 20);
            this.button_SelectedEvents.TabIndex = 52;
            this.button_SelectedEvents.Text = "...";
            this.button_SelectedEvents.UseVisualStyleBackColor = true;
            this.button_SelectedEvents.Click += new System.EventHandler(this.button_SelectedEvents_Click);
            // 
            // button_RegisterMonitorIO
            // 
            this.button_RegisterMonitorIO.Location = new System.Drawing.Point(460, 56);
            this.button_RegisterMonitorIO.Name = "button_RegisterMonitorIO";
            this.button_RegisterMonitorIO.Size = new System.Drawing.Size(41, 20);
            this.button_RegisterMonitorIO.TabIndex = 71;
            this.button_RegisterMonitorIO.Text = "...";
            this.button_RegisterMonitorIO.UseVisualStyleBackColor = true;
            this.button_RegisterMonitorIO.Click += new System.EventHandler(this.button_RegisterMonitorIO_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(10, 56);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(97, 13);
            this.label16.TabIndex = 69;
            this.label16.Text = "Register monitor IO";
            // 
            // textBox_MonitorIO
            // 
            this.textBox_MonitorIO.Location = new System.Drawing.Point(198, 56);
            this.textBox_MonitorIO.Name = "textBox_MonitorIO";
            this.textBox_MonitorIO.ReadOnly = true;
            this.textBox_MonitorIO.Size = new System.Drawing.Size(242, 20);
            this.textBox_MonitorIO.TabIndex = 70;
            this.textBox_MonitorIO.Text = "0";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(198, 179);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(222, 12);
            this.label20.TabIndex = 74;
            this.label20.Text = "( split with \';\' for multiple items, format \"notepad.exe\")";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(197, 246);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(245, 12);
            this.label13.TabIndex = 68;
            this.label13.Text = "(split with \';\' for multiple items, format \"domain\\username\" )";
            // 
            // textBox_ExcludeUserNames
            // 
            this.textBox_ExcludeUserNames.Location = new System.Drawing.Point(200, 261);
            this.textBox_ExcludeUserNames.Name = "textBox_ExcludeUserNames";
            this.textBox_ExcludeUserNames.Size = new System.Drawing.Size(242, 20);
            this.textBox_ExcludeUserNames.TabIndex = 67;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(14, 261);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(102, 13);
            this.label14.TabIndex = 66;
            this.label14.Text = "Exclude user names";
            // 
            // textBox_IncludeUserNames
            // 
            this.textBox_IncludeUserNames.Location = new System.Drawing.Point(200, 223);
            this.textBox_IncludeUserNames.Name = "textBox_IncludeUserNames";
            this.textBox_IncludeUserNames.Size = new System.Drawing.Size(242, 20);
            this.textBox_IncludeUserNames.TabIndex = 65;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(14, 230);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(99, 13);
            this.label15.TabIndex = 64;
            this.label15.Text = "Include user names";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(198, 108);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(222, 12);
            this.label12.TabIndex = 57;
            this.label12.Text = "( split with \';\' for multiple items, format \"notepad.exe\")";
            // 
            // textBox_ExcludeProcessNames
            // 
            this.textBox_ExcludeProcessNames.Location = new System.Drawing.Point(200, 158);
            this.textBox_ExcludeProcessNames.Name = "textBox_ExcludeProcessNames";
            this.textBox_ExcludeProcessNames.Size = new System.Drawing.Size(242, 20);
            this.textBox_ExcludeProcessNames.TabIndex = 56;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(14, 158);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(119, 13);
            this.label11.TabIndex = 55;
            this.label11.Text = "Exclude process names";
            // 
            // textBox_IncludeProcessNames
            // 
            this.textBox_IncludeProcessNames.Location = new System.Drawing.Point(200, 85);
            this.textBox_IncludeProcessNames.Name = "textBox_IncludeProcessNames";
            this.textBox_IncludeProcessNames.Size = new System.Drawing.Size(242, 20);
            this.textBox_IncludeProcessNames.TabIndex = 54;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(15, 92);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(116, 13);
            this.label10.TabIndex = 53;
            this.label10.Text = "Include process names";
            // 
            // button_SelectExcludePID
            // 
            this.button_SelectExcludePID.Location = new System.Drawing.Point(460, 195);
            this.button_SelectExcludePID.Name = "button_SelectExcludePID";
            this.button_SelectExcludePID.Size = new System.Drawing.Size(41, 20);
            this.button_SelectExcludePID.TabIndex = 44;
            this.button_SelectExcludePID.Text = "...";
            this.button_SelectExcludePID.UseVisualStyleBackColor = true;
            this.button_SelectExcludePID.Click += new System.EventHandler(this.button_SelectExcludePID_Click);
            // 
            // textBox_ExcludePID
            // 
            this.textBox_ExcludePID.Location = new System.Drawing.Point(200, 195);
            this.textBox_ExcludePID.Name = "textBox_ExcludePID";
            this.textBox_ExcludePID.ReadOnly = true;
            this.textBox_ExcludePID.Size = new System.Drawing.Size(242, 20);
            this.textBox_ExcludePID.TabIndex = 43;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 197);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(102, 13);
            this.label5.TabIndex = 42;
            this.label5.Text = "Exclude process Ids";
            // 
            // button_SelectIncludePID
            // 
            this.button_SelectIncludePID.Location = new System.Drawing.Point(460, 128);
            this.button_SelectIncludePID.Name = "button_SelectIncludePID";
            this.button_SelectIncludePID.Size = new System.Drawing.Size(41, 20);
            this.button_SelectIncludePID.TabIndex = 41;
            this.button_SelectIncludePID.Text = "...";
            this.button_SelectIncludePID.UseVisualStyleBackColor = true;
            this.button_SelectIncludePID.Click += new System.EventHandler(this.button_SelectIncludePID_Click);
            // 
            // textBox_IncludePID
            // 
            this.textBox_IncludePID.Location = new System.Drawing.Point(200, 128);
            this.textBox_IncludePID.Name = "textBox_IncludePID";
            this.textBox_IncludePID.ReadOnly = true;
            this.textBox_IncludePID.Size = new System.Drawing.Size(242, 20);
            this.textBox_IncludePID.TabIndex = 40;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 127);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(99, 13);
            this.label6.TabIndex = 39;
            this.label6.Text = "Include process Ids";
            // 
            // button_FilterCreateOptions
            // 
            this.button_FilterCreateOptions.Location = new System.Drawing.Point(460, 72);
            this.button_FilterCreateOptions.Name = "button_FilterCreateOptions";
            this.button_FilterCreateOptions.Size = new System.Drawing.Size(41, 20);
            this.button_FilterCreateOptions.TabIndex = 111;
            this.button_FilterCreateOptions.Text = "...";
            this.button_FilterCreateOptions.UseVisualStyleBackColor = true;
            this.button_FilterCreateOptions.Click += new System.EventHandler(this.button_FilterCreateOptions_Click);
            // 
            // textBox_FilterCreateOptions
            // 
            this.textBox_FilterCreateOptions.Location = new System.Drawing.Point(200, 72);
            this.textBox_FilterCreateOptions.Name = "textBox_FilterCreateOptions";
            this.textBox_FilterCreateOptions.ReadOnly = true;
            this.textBox_FilterCreateOptions.Size = new System.Drawing.Size(242, 20);
            this.textBox_FilterCreateOptions.TabIndex = 110;
            this.textBox_FilterCreateOptions.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 77);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(152, 13);
            this.label7.TabIndex = 109;
            this.label7.Text = "Create File With CreateOptions";
            // 
            // button_FilterDisposition
            // 
            this.button_FilterDisposition.Location = new System.Drawing.Point(460, 46);
            this.button_FilterDisposition.Name = "button_FilterDisposition";
            this.button_FilterDisposition.Size = new System.Drawing.Size(41, 20);
            this.button_FilterDisposition.TabIndex = 108;
            this.button_FilterDisposition.Text = "...";
            this.button_FilterDisposition.UseVisualStyleBackColor = true;
            this.button_FilterDisposition.Click += new System.EventHandler(this.button_FilterDisposition_Click);
            // 
            // textBox_FilterDisposition
            // 
            this.textBox_FilterDisposition.Location = new System.Drawing.Point(200, 46);
            this.textBox_FilterDisposition.Name = "textBox_FilterDisposition";
            this.textBox_FilterDisposition.ReadOnly = true;
            this.textBox_FilterDisposition.Size = new System.Drawing.Size(242, 20);
            this.textBox_FilterDisposition.TabIndex = 107;
            this.textBox_FilterDisposition.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 51);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(136, 13);
            this.label8.TabIndex = 106;
            this.label8.Text = "Create File With Disposition";
            // 
            // button_FilterDesiredAccess
            // 
            this.button_FilterDesiredAccess.Location = new System.Drawing.Point(460, 20);
            this.button_FilterDesiredAccess.Name = "button_FilterDesiredAccess";
            this.button_FilterDesiredAccess.Size = new System.Drawing.Size(41, 20);
            this.button_FilterDesiredAccess.TabIndex = 105;
            this.button_FilterDesiredAccess.Text = "...";
            this.button_FilterDesiredAccess.UseVisualStyleBackColor = true;
            this.button_FilterDesiredAccess.Click += new System.EventHandler(this.button_FilterDesiredAccess_Click);
            // 
            // textBox_FilterDesiredAccess
            // 
            this.textBox_FilterDesiredAccess.Location = new System.Drawing.Point(200, 20);
            this.textBox_FilterDesiredAccess.Name = "textBox_FilterDesiredAccess";
            this.textBox_FilterDesiredAccess.ReadOnly = true;
            this.textBox_FilterDesiredAccess.Size = new System.Drawing.Size(242, 20);
            this.textBox_FilterDesiredAccess.TabIndex = 104;
            this.textBox_FilterDesiredAccess.Text = "0";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(10, 25);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(156, 13);
            this.label17.TabIndex = 103;
            this.label17.Text = "Create File With DesiredAccess";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button_InfoExcludeUserName);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.button_InfoIncludeUserName);
            this.groupBox2.Controls.Add(this.button_InfoExcludeFileFilterMask);
            this.groupBox2.Controls.Add(this.button_InfoIncludeProcessName);
            this.groupBox2.Controls.Add(this.button_InfoExcludeProcessName);
            this.groupBox2.Controls.Add(this.button_InfoFilterMask);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label20);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.textBox_ExcludeUserNames);
            this.groupBox2.Controls.Add(this.textBox_IncludeFilterMask);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.textBox_IncludeUserNames);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.textBox_ExcludeFilterMask);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.textBox_ExcludeProcessNames);
            this.groupBox2.Controls.Add(this.textBox_IncludePID);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.button_SelectIncludePID);
            this.groupBox2.Controls.Add(this.textBox_IncludeProcessNames);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.textBox_ExcludePID);
            this.groupBox2.Controls.Add(this.button_SelectExcludePID);
            this.groupBox2.Location = new System.Drawing.Point(12, 10);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(547, 390);
            this.groupBox2.TabIndex = 75;
            this.groupBox2.TabStop = false;
            // 
            // button_InfoExcludeUserName
            // 
            this.button_InfoExcludeUserName.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
            this.button_InfoExcludeUserName.Location = new System.Drawing.Point(460, 261);
            this.button_InfoExcludeUserName.Name = "button_InfoExcludeUserName";
            this.button_InfoExcludeUserName.Size = new System.Drawing.Size(41, 20);
            this.button_InfoExcludeUserName.TabIndex = 117;
            this.button_InfoExcludeUserName.UseVisualStyleBackColor = true;
            this.button_InfoExcludeUserName.Click += new System.EventHandler(this.button_InfoExcludeUserName_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button_InfoCreatOptions);
            this.groupBox3.Controls.Add(this.button_InfoDisposition);
            this.groupBox3.Controls.Add(this.button_InfoDesiredAccess);
            this.groupBox3.Controls.Add(this.textBox_FilterDisposition);
            this.groupBox3.Controls.Add(this.label17);
            this.groupBox3.Controls.Add(this.textBox_FilterDesiredAccess);
            this.groupBox3.Controls.Add(this.button_FilterDesiredAccess);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.button_FilterDisposition);
            this.groupBox3.Controls.Add(this.button_FilterCreateOptions);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.textBox_FilterCreateOptions);
            this.groupBox3.Location = new System.Drawing.Point(0, 287);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(547, 100);
            this.groupBox3.TabIndex = 116;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Filter I/O by the Open Options";
            // 
            // button_InfoCreatOptions
            // 
            this.button_InfoCreatOptions.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
            this.button_InfoCreatOptions.Location = new System.Drawing.Point(513, 72);
            this.button_InfoCreatOptions.Name = "button_InfoCreatOptions";
            this.button_InfoCreatOptions.Size = new System.Drawing.Size(28, 20);
            this.button_InfoCreatOptions.TabIndex = 119;
            this.button_InfoCreatOptions.UseVisualStyleBackColor = true;
            this.button_InfoCreatOptions.Click += new System.EventHandler(this.button_InfoCreatOptions_Click);
            // 
            // button_InfoDisposition
            // 
            this.button_InfoDisposition.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
            this.button_InfoDisposition.Location = new System.Drawing.Point(513, 46);
            this.button_InfoDisposition.Name = "button_InfoDisposition";
            this.button_InfoDisposition.Size = new System.Drawing.Size(28, 20);
            this.button_InfoDisposition.TabIndex = 118;
            this.button_InfoDisposition.UseVisualStyleBackColor = true;
            this.button_InfoDisposition.Click += new System.EventHandler(this.button_InfoDisposition_Click);
            // 
            // button_InfoDesiredAccess
            // 
            this.button_InfoDesiredAccess.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
            this.button_InfoDesiredAccess.Location = new System.Drawing.Point(513, 20);
            this.button_InfoDesiredAccess.Name = "button_InfoDesiredAccess";
            this.button_InfoDesiredAccess.Size = new System.Drawing.Size(28, 20);
            this.button_InfoDesiredAccess.TabIndex = 117;
            this.button_InfoDesiredAccess.UseVisualStyleBackColor = true;
            this.button_InfoDesiredAccess.Click += new System.EventHandler(this.button_InfoDesiredAccess_Click);
            // 
            // button_InfoIncludeUserName
            // 
            this.button_InfoIncludeUserName.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
            this.button_InfoIncludeUserName.Location = new System.Drawing.Point(460, 230);
            this.button_InfoIncludeUserName.Name = "button_InfoIncludeUserName";
            this.button_InfoIncludeUserName.Size = new System.Drawing.Size(41, 20);
            this.button_InfoIncludeUserName.TabIndex = 113;
            this.button_InfoIncludeUserName.UseVisualStyleBackColor = true;
            this.button_InfoIncludeUserName.Click += new System.EventHandler(this.button_InfoIncludeUserName_Click);
            // 
            // button_InfoExcludeFileFilterMask
            // 
            this.button_InfoExcludeFileFilterMask.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
            this.button_InfoExcludeFileFilterMask.Location = new System.Drawing.Point(460, 46);
            this.button_InfoExcludeFileFilterMask.Name = "button_InfoExcludeFileFilterMask";
            this.button_InfoExcludeFileFilterMask.Size = new System.Drawing.Size(41, 20);
            this.button_InfoExcludeFileFilterMask.TabIndex = 115;
            this.button_InfoExcludeFileFilterMask.UseVisualStyleBackColor = true;
            this.button_InfoExcludeFileFilterMask.Click += new System.EventHandler(this.button_InfoExcludeFileFilterMask_Click);
            // 
            // button_InfoIncludeProcessName
            // 
            this.button_InfoIncludeProcessName.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
            this.button_InfoIncludeProcessName.Location = new System.Drawing.Point(460, 85);
            this.button_InfoIncludeProcessName.Name = "button_InfoIncludeProcessName";
            this.button_InfoIncludeProcessName.Size = new System.Drawing.Size(41, 20);
            this.button_InfoIncludeProcessName.TabIndex = 114;
            this.button_InfoIncludeProcessName.UseVisualStyleBackColor = true;
            this.button_InfoIncludeProcessName.Click += new System.EventHandler(this.button_InfoIncludeProcessName_Click);
            // 
            // button_InfoExcludeProcessName
            // 
            this.button_InfoExcludeProcessName.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
            this.button_InfoExcludeProcessName.Location = new System.Drawing.Point(460, 158);
            this.button_InfoExcludeProcessName.Name = "button_InfoExcludeProcessName";
            this.button_InfoExcludeProcessName.Size = new System.Drawing.Size(41, 20);
            this.button_InfoExcludeProcessName.TabIndex = 113;
            this.button_InfoExcludeProcessName.UseVisualStyleBackColor = true;
            this.button_InfoExcludeProcessName.Click += new System.EventHandler(this.button_InfoExcludeProcessName_Click);
            // 
            // button_InfoFilterMask
            // 
            this.button_InfoFilterMask.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
            this.button_InfoFilterMask.Location = new System.Drawing.Point(460, 16);
            this.button_InfoFilterMask.Name = "button_InfoFilterMask";
            this.button_InfoFilterMask.Size = new System.Drawing.Size(41, 20);
            this.button_InfoFilterMask.TabIndex = 112;
            this.button_InfoFilterMask.UseVisualStyleBackColor = true;
            this.button_InfoFilterMask.Click += new System.EventHandler(this.button_InfoFilterMask_Click);
            // 
            // checkBox_MonitorEventBuffer
            // 
            this.checkBox_MonitorEventBuffer.AutoSize = true;
            this.checkBox_MonitorEventBuffer.Location = new System.Drawing.Point(199, 87);
            this.checkBox_MonitorEventBuffer.Name = "checkBox_MonitorEventBuffer";
            this.checkBox_MonitorEventBuffer.Size = new System.Drawing.Size(159, 17);
            this.checkBox_MonitorEventBuffer.TabIndex = 120;
            this.checkBox_MonitorEventBuffer.Text = "Enable Monitor Event Buffer";
            this.checkBox_MonitorEventBuffer.UseVisualStyleBackColor = true;
            // 
            // button_MonitorBufferInfo
            // 
            this.button_MonitorBufferInfo.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
            this.button_MonitorBufferInfo.Location = new System.Drawing.Point(364, 84);
            this.button_MonitorBufferInfo.Name = "button_MonitorBufferInfo";
            this.button_MonitorBufferInfo.Size = new System.Drawing.Size(28, 20);
            this.button_MonitorBufferInfo.TabIndex = 121;
            this.button_MonitorBufferInfo.UseVisualStyleBackColor = true;
            this.button_MonitorBufferInfo.Click += new System.EventHandler(this.button_MonitorBufferInfo_Click);
            // 
            // FilterRuleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(571, 595);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox_AccessControl);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_SaveFilterRule);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FilterRuleForm";
            this.Text = "Filter rule settings";
            this.groupBox1.ResumeLayout(false);
            this.groupBox_AccessControl.ResumeLayout(false);
            this.groupBox_AccessControl.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_ExcludeFilterMask;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_IncludeFilterMask;
        private System.Windows.Forms.Button button_SaveFilterRule;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox_AccessControl;
        private System.Windows.Forms.Button button_SelectExcludePID;
        private System.Windows.Forms.TextBox textBox_ExcludePID;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button_SelectIncludePID;
        private System.Windows.Forms.TextBox textBox_IncludePID;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox_ExcludeProcessNames;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBox_IncludeProcessNames;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textBox_ExcludeUserNames;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox textBox_IncludeUserNames;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button button_SelectedEvents;
        private System.Windows.Forms.TextBox textBox_SelectedEvents;
        private System.Windows.Forms.Button button_RegisterMonitorIO;
        private System.Windows.Forms.TextBox textBox_MonitorIO;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Button button_ControlSettings;
        private System.Windows.Forms.Button button_FilterCreateOptions;
        private System.Windows.Forms.TextBox textBox_FilterCreateOptions;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button_FilterDisposition;
        private System.Windows.Forms.TextBox textBox_FilterDisposition;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button button_FilterDesiredAccess;
        private System.Windows.Forms.TextBox textBox_FilterDesiredAccess;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button_InfoIncludeUserName;
        private System.Windows.Forms.Button button_InfoExcludeFileFilterMask;
        private System.Windows.Forms.Button button_InfoIncludeProcessName;
        private System.Windows.Forms.Button button_InfoExcludeProcessName;
        private System.Windows.Forms.Button button_InfoFilterMask;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button_InfoMonitorIO;
        private System.Windows.Forms.Button button_InfoFileEvents;
        private System.Windows.Forms.Button button_InfoExcludeUserName;
        private System.Windows.Forms.Button button_InfoCreatOptions;
        private System.Windows.Forms.Button button_InfoDisposition;
        private System.Windows.Forms.Button button_InfoDesiredAccess;
        private System.Windows.Forms.Button button_MonitorBufferInfo;
        private System.Windows.Forms.CheckBox checkBox_MonitorEventBuffer;
    }
}