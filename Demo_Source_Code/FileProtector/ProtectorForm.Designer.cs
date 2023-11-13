using System;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;

using EaseFilter.CommonObjects;

namespace FileProtector
{
    public class FastListView : ListView
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct NMHDR
        {
            public IntPtr hwndFrom;
            public uint idFrom;
            public uint code;
        }

        private const uint NM_CUSTOMDRAW = unchecked((uint)-12);

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x204E)
            {
                NMHDR hdr = (NMHDR)m.GetLParam(typeof(NMHDR));
                if (hdr.code == NM_CUSTOMDRAW)
                {
                    m.Result = (IntPtr)0;
                    return;
                }
            }

            base.WndProc(ref m);
        }
    }

    partial class ProtectorForm
    {
        [DllImport("user32")]
        private static extern bool SendMessage(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);
        private uint LVM_SETTEXTBKCOLOR = 0x1026;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProtectorForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.encryptFileWithToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decryptFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decryptFileWithOffsetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getEncryptedFileIVTagToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_StartFilter = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_Stop = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_ClearMessage = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_LoadMessage = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_UnitTest = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Help = new System.Windows.Forms.ToolStripButton();
            this.listView_Info = new FileProtector.FastListView();
            this.toolStripButton_ApplyTrialKey = new System.Windows.Forms.ToolStripButton();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(974, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Settings";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.encryptFileWithToolStripMenuItem,
            this.decryptFileToolStripMenuItem,
            this.decryptFileWithOffsetToolStripMenuItem,
            this.getEncryptedFileIVTagToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // encryptFileWithToolStripMenuItem
            // 
            this.encryptFileWithToolStripMenuItem.Name = "encryptFileWithToolStripMenuItem";
            this.encryptFileWithToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.encryptFileWithToolStripMenuItem.Text = "Encrypt file with API";
            this.encryptFileWithToolStripMenuItem.Click += new System.EventHandler(this.encryptFileWithToolStripMenuItem_Click);
            // 
            // decryptFileToolStripMenuItem
            // 
            this.decryptFileToolStripMenuItem.Name = "decryptFileToolStripMenuItem";
            this.decryptFileToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.decryptFileToolStripMenuItem.Text = "Decrypt file with API";
            this.decryptFileToolStripMenuItem.Click += new System.EventHandler(this.decryptFileToolStripMenuItem_Click);
            // 
            // decryptFileWithOffsetToolStripMenuItem
            // 
            this.decryptFileWithOffsetToolStripMenuItem.Name = "decryptFileWithOffsetToolStripMenuItem";
            this.decryptFileWithOffsetToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.decryptFileWithOffsetToolStripMenuItem.Text = "Decrypt file with offset";
            this.decryptFileWithOffsetToolStripMenuItem.Click += new System.EventHandler(this.decryptFileWithOffsetToolStripMenuItem_Click);
            // 
            // getEncryptedFileIVTagToolStripMenuItem
            // 
            this.getEncryptedFileIVTagToolStripMenuItem.Name = "getEncryptedFileIVTagToolStripMenuItem";
            this.getEncryptedFileIVTagToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.getEncryptedFileIVTagToolStripMenuItem.Text = "Check Encrypt File Tag Data ";
            this.getEncryptedFileIVTagToolStripMenuItem.Click += new System.EventHandler(this.getEncryptedFileTagdataToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_StartFilter,
            this.toolStripSeparator1,
            this.toolStripButton_Stop,
            this.toolStripSeparator2,
            this.toolStripButton_ClearMessage,
            this.toolStripSeparator3,
            this.toolStripButton_LoadMessage,
            this.toolStripSeparator4,
            this.toolStripButton1,
            this.toolStripButton_UnitTest,
            this.toolStripButton_Help,
            this.toolStripButton_ApplyTrialKey});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(974, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton_StartFilter
            // 
            this.toolStripButton_StartFilter.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_StartFilter.Image")));
            this.toolStripButton_StartFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_StartFilter.Name = "toolStripButton_StartFilter";
            this.toolStripButton_StartFilter.Size = new System.Drawing.Size(103, 22);
            this.toolStripButton_StartFilter.Text = "Start protector";
            this.toolStripButton_StartFilter.Click += new System.EventHandler(this.toolStripButton_StartFilter_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_Stop
            // 
            this.toolStripButton_Stop.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Stop.Image")));
            this.toolStripButton_Stop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Stop.Name = "toolStripButton_Stop";
            this.toolStripButton_Stop.Size = new System.Drawing.Size(103, 22);
            this.toolStripButton_Stop.Text = "Stop protector";
            this.toolStripButton_Stop.Click += new System.EventHandler(this.toolStripButton_Stop_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_ClearMessage
            // 
            this.toolStripButton_ClearMessage.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_ClearMessage.Image")));
            this.toolStripButton_ClearMessage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_ClearMessage.Name = "toolStripButton_ClearMessage";
            this.toolStripButton_ClearMessage.Size = new System.Drawing.Size(108, 22);
            this.toolStripButton_ClearMessage.Text = "Clear messages";
            this.toolStripButton_ClearMessage.Click += new System.EventHandler(this.toolStripButton_ClearMessage_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_LoadMessage
            // 
            this.toolStripButton_LoadMessage.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_LoadMessage.Image")));
            this.toolStripButton_LoadMessage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_LoadMessage.Name = "toolStripButton_LoadMessage";
            this.toolStripButton_LoadMessage.Size = new System.Drawing.Size(107, 22);
            this.toolStripButton_LoadMessage.Text = "Load messages";
            this.toolStripButton_LoadMessage.Click += new System.EventHandler(this.toolStripButton_LoadMessage_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(93, 22);
            this.toolStripButton1.Text = "Event viewer";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripDisplayEvent_Click);
            // 
            // toolStripButton_UnitTest
            // 
            this.toolStripButton_UnitTest.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_UnitTest.Image")));
            this.toolStripButton_UnitTest.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_UnitTest.Name = "toolStripButton_UnitTest";
            this.toolStripButton_UnitTest.Size = new System.Drawing.Size(125, 22);
            this.toolStripButton_UnitTest.Text = "Protector Unit Test";
            this.toolStripButton_UnitTest.Click += new System.EventHandler(this.toolStripButton_UnitTest_Click);
            // 
            // toolStripButton_Help
            // 
            this.toolStripButton_Help.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Help.Image")));
            this.toolStripButton_Help.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Help.Name = "toolStripButton_Help";
            this.toolStripButton_Help.Size = new System.Drawing.Size(52, 22);
            this.toolStripButton_Help.Text = "Help";
            this.toolStripButton_Help.Click += new System.EventHandler(this.toolStripButton_Help_Click);
            // 
            // listView_Info
            // 
            this.listView_Info.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_Info.FullRowSelect = true;
            this.listView_Info.HoverSelection = true;
            this.listView_Info.LabelEdit = true;
            this.listView_Info.Location = new System.Drawing.Point(0, 49);
            this.listView_Info.Name = "listView_Info";
            this.listView_Info.ShowItemToolTips = true;
            this.listView_Info.Size = new System.Drawing.Size(974, 457);
            this.listView_Info.TabIndex = 2;
            this.listView_Info.UseCompatibleStateImageBehavior = false;
            this.listView_Info.View = System.Windows.Forms.View.Details;
            // 
            // toolStripButton_ApplyTrialKey
            // 
            this.toolStripButton_ApplyTrialKey.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_ApplyTrialKey.Image")));
            this.toolStripButton_ApplyTrialKey.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_ApplyTrialKey.Name = "toolStripButton_ApplyTrialKey";
            this.toolStripButton_ApplyTrialKey.Size = new System.Drawing.Size(102, 22);
            this.toolStripButton_ApplyTrialKey.Text = "Apply trial key";
            this.toolStripButton_ApplyTrialKey.Click += new System.EventHandler(this.toolStripButton_ApplyTrialKey_Click);
            // 
            // ProtectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(974, 506);
            this.Controls.Add(this.listView_Info);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ProtectorForm";
            this.Text = "EaseFilter File Protector";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ProtectorForm_FormClosed);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton_StartFilter;
        private System.Windows.Forms.ToolStripButton toolStripButton_Stop;
        private System.Windows.Forms.ToolStripButton toolStripButton_ClearMessage;
        private FastListView listView_Info;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem encryptFileWithToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem decryptFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getEncryptedFileIVTagToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton_LoadMessage;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton toolStripButton_UnitTest;
        private System.Windows.Forms.ToolStripButton toolStripButton_Help;
        private ToolStripMenuItem decryptFileWithOffsetToolStripMenuItem;
        private ToolStripButton toolStripButton_ApplyTrialKey;
    }
}

