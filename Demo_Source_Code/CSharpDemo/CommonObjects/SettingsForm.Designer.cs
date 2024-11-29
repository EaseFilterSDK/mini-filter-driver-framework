namespace EaseFilter.CommonObjects
{
    partial class SettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_InfoBlockUSBRead = new System.Windows.Forms.Button();
            this.button_InfoBlockUSBWrite = new System.Windows.Forms.Button();
            this.checkBox_BlockUSBWrite = new System.Windows.Forms.CheckBox();
            this.checkBox_BlockUSBRead = new System.Windows.Forms.CheckBox();
            this.button_InfoGetVolumeInfo = new System.Windows.Forms.Button();
            this.button_InfoNewVolumeInfo = new System.Windows.Forms.Button();
            this.button_InfoHideDirIO = new System.Windows.Forms.Button();
            this.button_InfoBlockVolumeFormatting = new System.Windows.Forms.Button();
            this.button_InfoVolumeDetach = new System.Windows.Forms.Button();
            this.button_InfoConnectionThreads = new System.Windows.Forms.Button();
            this.button_InfoConnectionTimeout = new System.Windows.Forms.Button();
            this.button_InfoMessageOutput = new System.Windows.Forms.Button();
            this.button_InfoLogMessage = new System.Windows.Forms.Button();
            this.button_InfoProtectPid = new System.Windows.Forms.Button();
            this.button_InfoSendBuffer = new System.Windows.Forms.Button();
            this.checkBox_CallbackVolumeDetached = new System.Windows.Forms.CheckBox();
            this.checkBox_CallbackVolumeAttached = new System.Windows.Forms.CheckBox();
            this.checkBox_BlockFormatting = new System.Windows.Forms.CheckBox();
            this.checkBox_GetVolumeInfo = new System.Windows.Forms.CheckBox();
            this.checkBox_SendBuffer = new System.Windows.Forms.CheckBox();
            this.checkBox_DisableDir = new System.Windows.Forms.CheckBox();
            this.checkBox_OutputMessageToConsole = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.button_SelectProtectPID = new System.Windows.Forms.Button();
            this.textBox_ProtectedPID = new System.Windows.Forms.TextBox();
            this.label_protector1 = new System.Windows.Forms.Label();
            this.button_EditFilterRule = new System.Windows.Forms.Button();
            this.button_DeleteFilter = new System.Windows.Forms.Button();
            this.button_AddFilter = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listView_FilterRules = new System.Windows.Forms.ListView();
            this.checkBox_TransactionLog = new System.Windows.Forms.CheckBox();
            this.textBox_TransactionLog = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox_MaximumFilterMessage = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_ConnectionTimeout = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_ConnectionThreads = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_ApplyOptions = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_InfoBlockUSBRead);
            this.groupBox1.Controls.Add(this.button_InfoBlockUSBWrite);
            this.groupBox1.Controls.Add(this.checkBox_BlockUSBWrite);
            this.groupBox1.Controls.Add(this.checkBox_BlockUSBRead);
            this.groupBox1.Controls.Add(this.button_InfoGetVolumeInfo);
            this.groupBox1.Controls.Add(this.button_InfoNewVolumeInfo);
            this.groupBox1.Controls.Add(this.button_InfoHideDirIO);
            this.groupBox1.Controls.Add(this.button_InfoBlockVolumeFormatting);
            this.groupBox1.Controls.Add(this.button_InfoVolumeDetach);
            this.groupBox1.Controls.Add(this.button_InfoConnectionThreads);
            this.groupBox1.Controls.Add(this.button_InfoConnectionTimeout);
            this.groupBox1.Controls.Add(this.button_InfoMessageOutput);
            this.groupBox1.Controls.Add(this.button_InfoLogMessage);
            this.groupBox1.Controls.Add(this.button_InfoProtectPid);
            this.groupBox1.Controls.Add(this.button_InfoSendBuffer);
            this.groupBox1.Controls.Add(this.checkBox_CallbackVolumeDetached);
            this.groupBox1.Controls.Add(this.checkBox_CallbackVolumeAttached);
            this.groupBox1.Controls.Add(this.checkBox_BlockFormatting);
            this.groupBox1.Controls.Add(this.checkBox_GetVolumeInfo);
            this.groupBox1.Controls.Add(this.checkBox_SendBuffer);
            this.groupBox1.Controls.Add(this.checkBox_DisableDir);
            this.groupBox1.Controls.Add(this.checkBox_OutputMessageToConsole);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.button_SelectProtectPID);
            this.groupBox1.Controls.Add(this.textBox_ProtectedPID);
            this.groupBox1.Controls.Add(this.label_protector1);
            this.groupBox1.Controls.Add(this.button_EditFilterRule);
            this.groupBox1.Controls.Add(this.button_DeleteFilter);
            this.groupBox1.Controls.Add(this.button_AddFilter);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.checkBox_TransactionLog);
            this.groupBox1.Controls.Add(this.textBox_TransactionLog);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.textBox_MaximumFilterMessage);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.textBox_ConnectionTimeout);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.textBox_ConnectionThreads);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(3, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(632, 552);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // button_InfoBlockUSBRead
            // 
            this.button_InfoBlockUSBRead.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
            this.button_InfoBlockUSBRead.Location = new System.Drawing.Point(206, 300);
            this.button_InfoBlockUSBRead.Name = "button_InfoBlockUSBRead";
            this.button_InfoBlockUSBRead.Size = new System.Drawing.Size(26, 20);
            this.button_InfoBlockUSBRead.TabIndex = 131;
            this.button_InfoBlockUSBRead.UseVisualStyleBackColor = true;
            this.button_InfoBlockUSBRead.Click += new System.EventHandler(this.button_InfoBlockUSBRead_Click);
            // 
            // button_InfoBlockUSBWrite
            // 
            this.button_InfoBlockUSBWrite.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
            this.button_InfoBlockUSBWrite.Location = new System.Drawing.Point(550, 300);
            this.button_InfoBlockUSBWrite.Name = "button_InfoBlockUSBWrite";
            this.button_InfoBlockUSBWrite.Size = new System.Drawing.Size(26, 20);
            this.button_InfoBlockUSBWrite.TabIndex = 130;
            this.button_InfoBlockUSBWrite.UseVisualStyleBackColor = true;
            this.button_InfoBlockUSBWrite.Click += new System.EventHandler(this.button_InfoBlockUSBWrite_Click);
            // 
            // checkBox_BlockUSBWrite
            // 
            this.checkBox_BlockUSBWrite.AutoSize = true;
            this.checkBox_BlockUSBWrite.Location = new System.Drawing.Point(341, 300);
            this.checkBox_BlockUSBWrite.Name = "checkBox_BlockUSBWrite";
            this.checkBox_BlockUSBWrite.Size = new System.Drawing.Size(115, 17);
            this.checkBox_BlockUSBWrite.TabIndex = 129;
            this.checkBox_BlockUSBWrite.Text = "Block write to USB";
            this.checkBox_BlockUSBWrite.UseVisualStyleBackColor = true;
            // 
            // checkBox_BlockUSBRead
            // 
            this.checkBox_BlockUSBRead.AutoSize = true;
            this.checkBox_BlockUSBRead.Location = new System.Drawing.Point(13, 300);
            this.checkBox_BlockUSBRead.Name = "checkBox_BlockUSBRead";
            this.checkBox_BlockUSBRead.Size = new System.Drawing.Size(125, 17);
            this.checkBox_BlockUSBRead.TabIndex = 128;
            this.checkBox_BlockUSBRead.Text = "Block read from USB";
            this.checkBox_BlockUSBRead.UseVisualStyleBackColor = true;
            // 
            // button_InfoGetVolumeInfo
            // 
            this.button_InfoGetVolumeInfo.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
            this.button_InfoGetVolumeInfo.Location = new System.Drawing.Point(206, 248);
            this.button_InfoGetVolumeInfo.Name = "button_InfoGetVolumeInfo";
            this.button_InfoGetVolumeInfo.Size = new System.Drawing.Size(26, 20);
            this.button_InfoGetVolumeInfo.TabIndex = 127;
            this.button_InfoGetVolumeInfo.UseVisualStyleBackColor = true;
            this.button_InfoGetVolumeInfo.Click += new System.EventHandler(this.button_InfoGetVolumeInfo_Click);
            // 
            // button_InfoNewVolumeInfo
            // 
            this.button_InfoNewVolumeInfo.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
            this.button_InfoNewVolumeInfo.Location = new System.Drawing.Point(206, 274);
            this.button_InfoNewVolumeInfo.Name = "button_InfoNewVolumeInfo";
            this.button_InfoNewVolumeInfo.Size = new System.Drawing.Size(26, 20);
            this.button_InfoNewVolumeInfo.TabIndex = 126;
            this.button_InfoNewVolumeInfo.UseVisualStyleBackColor = true;
            this.button_InfoNewVolumeInfo.Click += new System.EventHandler(this.button_InfoNewVolumeInfo_Click);
            // 
            // button_InfoHideDirIO
            // 
            this.button_InfoHideDirIO.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
            this.button_InfoHideDirIO.Location = new System.Drawing.Point(207, 219);
            this.button_InfoHideDirIO.Name = "button_InfoHideDirIO";
            this.button_InfoHideDirIO.Size = new System.Drawing.Size(26, 20);
            this.button_InfoHideDirIO.TabIndex = 125;
            this.button_InfoHideDirIO.UseVisualStyleBackColor = true;
            this.button_InfoHideDirIO.Click += new System.EventHandler(this.button_InfoHideDirIO_Click);
            // 
            // button_InfoBlockVolumeFormatting
            // 
            this.button_InfoBlockVolumeFormatting.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
            this.button_InfoBlockVolumeFormatting.Location = new System.Drawing.Point(550, 275);
            this.button_InfoBlockVolumeFormatting.Name = "button_InfoBlockVolumeFormatting";
            this.button_InfoBlockVolumeFormatting.Size = new System.Drawing.Size(26, 20);
            this.button_InfoBlockVolumeFormatting.TabIndex = 124;
            this.button_InfoBlockVolumeFormatting.UseVisualStyleBackColor = true;
            this.button_InfoBlockVolumeFormatting.Click += new System.EventHandler(this.button_InfoBlockVolumeFormatting_Click);
            // 
            // button_InfoVolumeDetach
            // 
            this.button_InfoVolumeDetach.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
            this.button_InfoVolumeDetach.Location = new System.Drawing.Point(550, 247);
            this.button_InfoVolumeDetach.Name = "button_InfoVolumeDetach";
            this.button_InfoVolumeDetach.Size = new System.Drawing.Size(26, 20);
            this.button_InfoVolumeDetach.TabIndex = 123;
            this.button_InfoVolumeDetach.UseVisualStyleBackColor = true;
            this.button_InfoVolumeDetach.Click += new System.EventHandler(this.button_InfoVolumeDetach_Click);
            // 
            // button_InfoConnectionThreads
            // 
            this.button_InfoConnectionThreads.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
            this.button_InfoConnectionThreads.Location = new System.Drawing.Point(550, 60);
            this.button_InfoConnectionThreads.Name = "button_InfoConnectionThreads";
            this.button_InfoConnectionThreads.Size = new System.Drawing.Size(28, 20);
            this.button_InfoConnectionThreads.TabIndex = 122;
            this.button_InfoConnectionThreads.UseVisualStyleBackColor = true;
            this.button_InfoConnectionThreads.Click += new System.EventHandler(this.button_InfoConnectionThreads_Click);
            // 
            // button_InfoConnectionTimeout
            // 
            this.button_InfoConnectionTimeout.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
            this.button_InfoConnectionTimeout.Location = new System.Drawing.Point(550, 99);
            this.button_InfoConnectionTimeout.Name = "button_InfoConnectionTimeout";
            this.button_InfoConnectionTimeout.Size = new System.Drawing.Size(28, 20);
            this.button_InfoConnectionTimeout.TabIndex = 121;
            this.button_InfoConnectionTimeout.UseVisualStyleBackColor = true;
            this.button_InfoConnectionTimeout.Click += new System.EventHandler(this.button_InfoConnectionTimeout_Click);
            // 
            // button_InfoMessageOutput
            // 
            this.button_InfoMessageOutput.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
            this.button_InfoMessageOutput.Location = new System.Drawing.Point(550, 135);
            this.button_InfoMessageOutput.Name = "button_InfoMessageOutput";
            this.button_InfoMessageOutput.Size = new System.Drawing.Size(28, 20);
            this.button_InfoMessageOutput.TabIndex = 120;
            this.button_InfoMessageOutput.UseVisualStyleBackColor = true;
            this.button_InfoMessageOutput.Click += new System.EventHandler(this.button_InfoMessageOutput_Click);
            // 
            // button_InfoLogMessage
            // 
            this.button_InfoLogMessage.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
            this.button_InfoLogMessage.Location = new System.Drawing.Point(550, 187);
            this.button_InfoLogMessage.Name = "button_InfoLogMessage";
            this.button_InfoLogMessage.Size = new System.Drawing.Size(28, 20);
            this.button_InfoLogMessage.TabIndex = 119;
            this.button_InfoLogMessage.UseVisualStyleBackColor = true;
            this.button_InfoLogMessage.Click += new System.EventHandler(this.button_InfoLogMessage_Click);
            // 
            // button_InfoProtectPid
            // 
            this.button_InfoProtectPid.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
            this.button_InfoProtectPid.Location = new System.Drawing.Point(587, 21);
            this.button_InfoProtectPid.Name = "button_InfoProtectPid";
            this.button_InfoProtectPid.Size = new System.Drawing.Size(28, 20);
            this.button_InfoProtectPid.TabIndex = 118;
            this.button_InfoProtectPid.UseVisualStyleBackColor = true;
            this.button_InfoProtectPid.Click += new System.EventHandler(this.button_InfoProtectPid_Click);
            // 
            // button_InfoSendBuffer
            // 
            this.button_InfoSendBuffer.Image = global::EaseFilter.CommonObjects.Properties.Resources.about;
            this.button_InfoSendBuffer.Location = new System.Drawing.Point(548, 219);
            this.button_InfoSendBuffer.Name = "button_InfoSendBuffer";
            this.button_InfoSendBuffer.Size = new System.Drawing.Size(26, 20);
            this.button_InfoSendBuffer.TabIndex = 73;
            this.button_InfoSendBuffer.UseVisualStyleBackColor = true;
            this.button_InfoSendBuffer.Click += new System.EventHandler(this.button_InfoSendBuffer_Click);
            // 
            // checkBox_CallbackVolumeDetached
            // 
            this.checkBox_CallbackVolumeDetached.AutoSize = true;
            this.checkBox_CallbackVolumeDetached.Location = new System.Drawing.Point(341, 247);
            this.checkBox_CallbackVolumeDetached.Name = "checkBox_CallbackVolumeDetached";
            this.checkBox_CallbackVolumeDetached.Size = new System.Drawing.Size(189, 17);
            this.checkBox_CallbackVolumeDetached.TabIndex = 72;
            this.checkBox_CallbackVolumeDetached.Text = "Notify when volume was detached";
            this.checkBox_CallbackVolumeDetached.UseVisualStyleBackColor = true;
            // 
            // checkBox_CallbackVolumeAttached
            // 
            this.checkBox_CallbackVolumeAttached.AutoSize = true;
            this.checkBox_CallbackVolumeAttached.Location = new System.Drawing.Point(13, 274);
            this.checkBox_CallbackVolumeAttached.Name = "checkBox_CallbackVolumeAttached";
            this.checkBox_CallbackVolumeAttached.Size = new System.Drawing.Size(187, 17);
            this.checkBox_CallbackVolumeAttached.TabIndex = 71;
            this.checkBox_CallbackVolumeAttached.Text = "Notify when new volume attached";
            this.checkBox_CallbackVolumeAttached.UseVisualStyleBackColor = true;
            // 
            // checkBox_BlockFormatting
            // 
            this.checkBox_BlockFormatting.AutoSize = true;
            this.checkBox_BlockFormatting.Location = new System.Drawing.Point(341, 275);
            this.checkBox_BlockFormatting.Name = "checkBox_BlockFormatting";
            this.checkBox_BlockFormatting.Size = new System.Drawing.Size(167, 17);
            this.checkBox_BlockFormatting.TabIndex = 70;
            this.checkBox_BlockFormatting.Text = "Block volumes from formatting";
            this.checkBox_BlockFormatting.UseVisualStyleBackColor = true;
            // 
            // checkBox_GetVolumeInfo
            // 
            this.checkBox_GetVolumeInfo.AutoSize = true;
            this.checkBox_GetVolumeInfo.Location = new System.Drawing.Point(13, 248);
            this.checkBox_GetVolumeInfo.Name = "checkBox_GetVolumeInfo";
            this.checkBox_GetVolumeInfo.Size = new System.Drawing.Size(179, 17);
            this.checkBox_GetVolumeInfo.TabIndex = 69;
            this.checkBox_GetVolumeInfo.Text = "Get attached volume information";
            this.checkBox_GetVolumeInfo.UseVisualStyleBackColor = true;
            // 
            // checkBox_SendBuffer
            // 
            this.checkBox_SendBuffer.AutoSize = true;
            this.checkBox_SendBuffer.Location = new System.Drawing.Point(341, 219);
            this.checkBox_SendBuffer.Name = "checkBox_SendBuffer";
            this.checkBox_SendBuffer.Size = new System.Drawing.Size(166, 17);
            this.checkBox_SendBuffer.TabIndex = 68;
            this.checkBox_SendBuffer.Text = "Enable send read/write buffer";
            this.checkBox_SendBuffer.UseVisualStyleBackColor = true;
            // 
            // checkBox_DisableDir
            // 
            this.checkBox_DisableDir.AutoSize = true;
            this.checkBox_DisableDir.Location = new System.Drawing.Point(13, 219);
            this.checkBox_DisableDir.Name = "checkBox_DisableDir";
            this.checkBox_DisableDir.Size = new System.Drawing.Size(155, 17);
            this.checkBox_DisableDir.TabIndex = 67;
            this.checkBox_DisableDir.Text = "Hide directory IO messages";
            this.checkBox_DisableDir.UseVisualStyleBackColor = true;
            // 
            // checkBox_OutputMessageToConsole
            // 
            this.checkBox_OutputMessageToConsole.AutoSize = true;
            this.checkBox_OutputMessageToConsole.Checked = true;
            this.checkBox_OutputMessageToConsole.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_OutputMessageToConsole.Location = new System.Drawing.Point(398, 135);
            this.checkBox_OutputMessageToConsole.Name = "checkBox_OutputMessageToConsole";
            this.checkBox_OutputMessageToConsole.Size = new System.Drawing.Size(155, 17);
            this.checkBox_OutputMessageToConsole.TabIndex = 63;
            this.checkBox_OutputMessageToConsole.Text = "Output message to console";
            this.checkBox_OutputMessageToConsole.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(204, 155);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(233, 12);
            this.label12.TabIndex = 61;
            this.label12.Text = "(The maximum message items to display in the console)";
            // 
            // button_SelectProtectPID
            // 
            this.button_SelectProtectPID.Location = new System.Drawing.Point(548, 20);
            this.button_SelectProtectPID.Name = "button_SelectProtectPID";
            this.button_SelectProtectPID.Size = new System.Drawing.Size(30, 20);
            this.button_SelectProtectPID.TabIndex = 56;
            this.button_SelectProtectPID.Text = "...";
            this.button_SelectProtectPID.UseVisualStyleBackColor = true;
            this.button_SelectProtectPID.Click += new System.EventHandler(this.button_SelectProtectPID_Click);
            // 
            // textBox_ProtectedPID
            // 
            this.textBox_ProtectedPID.Location = new System.Drawing.Point(207, 21);
            this.textBox_ProtectedPID.Name = "textBox_ProtectedPID";
            this.textBox_ProtectedPID.ReadOnly = true;
            this.textBox_ProtectedPID.Size = new System.Drawing.Size(324, 20);
            this.textBox_ProtectedPID.TabIndex = 55;
            // 
            // label_protector1
            // 
            this.label_protector1.AutoSize = true;
            this.label_protector1.Location = new System.Drawing.Point(10, 20);
            this.label_protector1.Name = "label_protector1";
            this.label_protector1.Size = new System.Drawing.Size(114, 13);
            this.label_protector1.TabIndex = 54;
            this.label_protector1.Text = "Add protect process Id";
            // 
            // button_EditFilterRule
            // 
            this.button_EditFilterRule.Location = new System.Drawing.Point(236, 513);
            this.button_EditFilterRule.Name = "button_EditFilterRule";
            this.button_EditFilterRule.Size = new System.Drawing.Size(92, 23);
            this.button_EditFilterRule.TabIndex = 53;
            this.button_EditFilterRule.Text = "Edit Filter Rule";
            this.button_EditFilterRule.UseVisualStyleBackColor = true;
            this.button_EditFilterRule.Click += new System.EventHandler(this.button_EditFilterRule_Click);
            // 
            // button_DeleteFilter
            // 
            this.button_DeleteFilter.Location = new System.Drawing.Point(461, 513);
            this.button_DeleteFilter.Name = "button_DeleteFilter";
            this.button_DeleteFilter.Size = new System.Drawing.Size(114, 23);
            this.button_DeleteFilter.TabIndex = 52;
            this.button_DeleteFilter.Text = "Delete Filter Rule";
            this.button_DeleteFilter.UseVisualStyleBackColor = true;
            this.button_DeleteFilter.Click += new System.EventHandler(this.button_DeleteFilter_Click);
            // 
            // button_AddFilter
            // 
            this.button_AddFilter.Location = new System.Drawing.Point(9, 513);
            this.button_AddFilter.Name = "button_AddFilter";
            this.button_AddFilter.Size = new System.Drawing.Size(127, 23);
            this.button_AddFilter.TabIndex = 51;
            this.button_AddFilter.Text = "Add New Filter Rule";
            this.button_AddFilter.UseVisualStyleBackColor = true;
            this.button_AddFilter.Click += new System.EventHandler(this.button_AddFilter_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.listView_FilterRules);
            this.groupBox2.Location = new System.Drawing.Point(6, 340);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(572, 167);
            this.groupBox2.TabIndex = 50;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Filter Rule Settings Collection";
            // 
            // listView_FilterRules
            // 
            this.listView_FilterRules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_FilterRules.FullRowSelect = true;
            this.listView_FilterRules.Location = new System.Drawing.Point(3, 16);
            this.listView_FilterRules.Name = "listView_FilterRules";
            this.listView_FilterRules.Size = new System.Drawing.Size(566, 148);
            this.listView_FilterRules.TabIndex = 1;
            this.listView_FilterRules.UseCompatibleStateImageBehavior = false;
            this.listView_FilterRules.View = System.Windows.Forms.View.Details;
            // 
            // checkBox_TransactionLog
            // 
            this.checkBox_TransactionLog.AutoSize = true;
            this.checkBox_TransactionLog.Location = new System.Drawing.Point(398, 187);
            this.checkBox_TransactionLog.Name = "checkBox_TransactionLog";
            this.checkBox_TransactionLog.Size = new System.Drawing.Size(143, 17);
            this.checkBox_TransactionLog.TabIndex = 44;
            this.checkBox_TransactionLog.Text = "Enable log filter message";
            this.checkBox_TransactionLog.UseVisualStyleBackColor = true;
            // 
            // textBox_TransactionLog
            // 
            this.textBox_TransactionLog.Location = new System.Drawing.Point(206, 185);
            this.textBox_TransactionLog.Name = "textBox_TransactionLog";
            this.textBox_TransactionLog.Size = new System.Drawing.Size(164, 20);
            this.textBox_TransactionLog.TabIndex = 43;
            this.textBox_TransactionLog.Text = "filterMessage.log";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 185);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(148, 13);
            this.label7.TabIndex = 42;
            this.label7.Text = "File name to log filter message";
            // 
            // textBox_MaximumFilterMessage
            // 
            this.textBox_MaximumFilterMessage.Location = new System.Drawing.Point(206, 133);
            this.textBox_MaximumFilterMessage.Name = "textBox_MaximumFilterMessage";
            this.textBox_MaximumFilterMessage.Size = new System.Drawing.Size(164, 20);
            this.textBox_MaximumFilterMessage.TabIndex = 41;
            this.textBox_MaximumFilterMessage.Text = "500";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 133);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 13);
            this.label6.TabIndex = 40;
            this.label6.Text = "Maximum records";
            // 
            // textBox_ConnectionTimeout
            // 
            this.textBox_ConnectionTimeout.Location = new System.Drawing.Point(206, 100);
            this.textBox_ConnectionTimeout.Name = "textBox_ConnectionTimeout";
            this.textBox_ConnectionTimeout.Size = new System.Drawing.Size(325, 20);
            this.textBox_ConnectionTimeout.TabIndex = 37;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 100);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(151, 13);
            this.label5.TabIndex = 36;
            this.label5.Text = "Filter driver connection timeout";
            // 
            // textBox_ConnectionThreads
            // 
            this.textBox_ConnectionThreads.Location = new System.Drawing.Point(207, 61);
            this.textBox_ConnectionThreads.Name = "textBox_ConnectionThreads";
            this.textBox_ConnectionThreads.Size = new System.Drawing.Size(324, 20);
            this.textBox_ConnectionThreads.TabIndex = 33;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(145, 13);
            this.label1.TabIndex = 32;
            this.label1.Text = "Maximum connection threads";
            // 
            // button_ApplyOptions
            // 
            this.button_ApplyOptions.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_ApplyOptions.Location = new System.Drawing.Point(464, 570);
            this.button_ApplyOptions.Name = "button_ApplyOptions";
            this.button_ApplyOptions.Size = new System.Drawing.Size(114, 23);
            this.button_ApplyOptions.TabIndex = 1;
            this.button_ApplyOptions.Text = "Apply Settings";
            this.button_ApplyOptions.UseVisualStyleBackColor = true;
            this.button_ApplyOptions.Click += new System.EventHandler(this.button_ApplyOptions_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 612);
            this.Controls.Add(this.button_ApplyOptions);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingsForm";
            this.Text = "Filter Driver Global Settings";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox_ConnectionTimeout;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_ConnectionThreads;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_ApplyOptions;
        private System.Windows.Forms.CheckBox checkBox_TransactionLog;
        private System.Windows.Forms.TextBox textBox_TransactionLog;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox_MaximumFilterMessage;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView listView_FilterRules;
        private System.Windows.Forms.Button button_DeleteFilter;
        private System.Windows.Forms.Button button_AddFilter;
        private System.Windows.Forms.Button button_EditFilterRule;
        private System.Windows.Forms.Button button_SelectProtectPID;
        private System.Windows.Forms.TextBox textBox_ProtectedPID;
        private System.Windows.Forms.Label label_protector1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox checkBox_OutputMessageToConsole;
        private System.Windows.Forms.CheckBox checkBox_DisableDir;
        private System.Windows.Forms.CheckBox checkBox_SendBuffer;
        private System.Windows.Forms.CheckBox checkBox_CallbackVolumeDetached;
        private System.Windows.Forms.CheckBox checkBox_CallbackVolumeAttached;
        private System.Windows.Forms.CheckBox checkBox_BlockFormatting;
        private System.Windows.Forms.CheckBox checkBox_GetVolumeInfo;
        private System.Windows.Forms.Button button_InfoSendBuffer;
        private System.Windows.Forms.Button button_InfoGetVolumeInfo;
        private System.Windows.Forms.Button button_InfoNewVolumeInfo;
        private System.Windows.Forms.Button button_InfoHideDirIO;
        private System.Windows.Forms.Button button_InfoBlockVolumeFormatting;
        private System.Windows.Forms.Button button_InfoVolumeDetach;
        private System.Windows.Forms.Button button_InfoConnectionThreads;
        private System.Windows.Forms.Button button_InfoConnectionTimeout;
        private System.Windows.Forms.Button button_InfoMessageOutput;
        private System.Windows.Forms.Button button_InfoLogMessage;
        private System.Windows.Forms.Button button_InfoProtectPid;
        private System.Windows.Forms.Button button_InfoBlockUSBRead;
        private System.Windows.Forms.Button button_InfoBlockUSBWrite;
        private System.Windows.Forms.CheckBox checkBox_BlockUSBWrite;
        private System.Windows.Forms.CheckBox checkBox_BlockUSBRead;
    }
}