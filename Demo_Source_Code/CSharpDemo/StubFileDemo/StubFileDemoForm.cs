using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

using EaseFilter.CommonObjects;
using EaseFilter.FilterControl;

namespace  StubFileDemo
{
    public partial class StubFileDemoForm : Form
    {       
        Boolean isMessageDisplayed = false;

        public StubFileDemoForm()
        {
            InitializeComponent();

            string lastError = string.Empty;
            Utils.CopyOSPlatformDependentFiles(ref lastError);

            StartPosition = FormStartPosition.CenterScreen;            
            GlobalConfig.EventLevel = EventLevel.Verbose;

            DisplayVersion();

        }

        ~StubFileDemoForm()
        {
            FilterWorker.StopService();
        }

        private void DisplayVersion()
        {
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            try
            {
                string filterDllPath = Path.Combine(GlobalConfig.AssemblyPath, "FilterAPI.Dll");
                version = FileVersionInfo.GetVersionInfo(filterDllPath).ProductVersion;
            }
            catch (Exception ex)
            {
                EventManager.WriteMessage(43, "LoadFilterAPI Dll", EventLevel.Error, "FilterAPI.dll can't be found." + ex.Message);
            }

            this.Text += "    Version:  " + version;
        }



        private void toolStripButton_StartFilter_Click(object sender, EventArgs e)
        {
            string lastError = string.Empty;

            if (!FilterWorker.StartService(ref lastError))
            {
                    MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                    MessageBox.Show("Start filter failed." + lastError, "StartFilter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
            }

            toolStripButton_StartFilter.Enabled = false;
            toolStripButton_Stop.Enabled = true;

            FilterWorker.listView_Info = listView_Info;
            FilterWorker.InitListView();

            EventManager.WriteMessage(102, "StartFilter", EventLevel.Information, "Start filter service succeeded.");

        }

        private void toolStripButton_Stop_Click(object sender, EventArgs e)
        {
            FilterWorker.StopService();

            toolStripButton_StartFilter.Enabled = true;
            toolStripButton_Stop.Enabled = false;
        }

        private void toolStripButton_ClearMessage_Click(object sender, EventArgs e)
        {
            listView_Info.Clear();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsForm settingForm = new SettingsForm();
            settingForm.StartPosition = FormStartPosition.CenterParent;
            settingForm.ShowDialog();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            EventForm.DisplayEventForm();
        }

        //private void createTestStubFileWithToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    TestStubFileForms testStubFileForm = new TestStubFileForms();
        //    testStubFileForm.ShowDialog();
        //}

      
        private void uninstallDriverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FilterWorker.StopService();
            FilterAPI.UnInstallDriver();
        }

        private void installDriverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FilterAPI.InstallDriver();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FilterWorker.StopService();
            Application.Exit();
        }

        private void demoForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            FilterWorker.StopService();
        }
     
        private void StubFileDemoForm_Shown(object sender, EventArgs e)
        {
            if (!isMessageDisplayed)
            {
                isMessageDisplayed = true;

                CreateTestFiles();

                string testMessage = "1.The test source files were created in the following folder: " + GlobalConfig.TestSourceFolder + "\r\n\r\n";
                testMessage += "2. The test stub files were created in the following folder: " + GlobalConfig.TestStubFolder + "\r\n\r\n";
                testMessage += "3. When the stub file is read, there are three options for returning data, depending on the configuration settings:" + "\r\n\r\n";
                testMessage += "   a. Return only the requested block of data.\r\n";
                testMessage += "   b. Return the cache file name.\r\n";
                testMessage += "   c. Rehydrate the stub file.\r\n\r\n";
                testMessage += "4. When the stub file is renamed or written, the cache file name must be returned, and the stub file will be rehydrated first.\r\n\r\n";

                MessageBox.Show(testMessage,"Test Stub File Creation", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        private void toolStripButton_Help_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.easefilter.com/cloud/stub-file-demo.htm");
        }

        private void toolStripButton_ApplyTrialKey_Click(object sender, EventArgs e)
        {
            
        }

        static public void CreateTestFiles()
        {
            if (!Directory.Exists(GlobalConfig.TestSourceFolder))
            {
                Directory.CreateDirectory(GlobalConfig.TestSourceFolder);
            }

            CreateTestSourceFiles();

            CreateTestStubFiles(GlobalConfig.TestSourceFolder);
        }

        /// <summary>
        /// Here will create some test source file with file content, it is only for demo purpose.
        /// for your own application, you need to get the file content from the remote host.
        /// </summary>
        static public void CreateTestSourceFiles()
        {
            //create 5 test source file here.                    
            for (int i = 1; i < 10; i++)
            {
                string testStr = string.Empty;
                for (int j = 0; j < i * 10240; j++)
                {
                    int rem = 0;
                    Math.DivRem(j, 26, out rem);
                    testStr += (char)('a' + rem);
                    if (rem == 25)
                    {
                        testStr += Environment.NewLine;
                    }
                }

                string testFileName = Path.Combine(GlobalConfig.TestSourceFolder, "testFile." + i.ToString() + ".txt");

                File.Delete(testFileName);
                File.AppendAllText(testFileName, testStr);
            }
        }

        /// <summary>
        /// A stub file is a file with 1024 bytes size, it contains the meta data of the virtual file and your own custom tag data.
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        static public bool CreateTestStubFiles(string folder)
        {

            try
            {
                string[] dirs = Directory.GetDirectories(folder);

                bool ret = false;

                foreach (string dir in dirs)
                {
                    CreateTestStubFiles(dir);
                }

                string[] files = Directory.GetFiles(folder);

                foreach (string file in files)
                {
                    string stubFileName = GlobalConfig.TestStubFolder + file.Substring(GlobalConfig.TestSourceFolder.Length);

                    string stubFolder = Path.GetDirectoryName(stubFileName);
                    if (!Directory.Exists(stubFolder))
                    {
                        Directory.CreateDirectory(stubFolder);
                    }

                    if (File.Exists(stubFileName))
                    {
                        File.Delete(stubFileName);
                    }

                    FileInfo fileInfo = new FileInfo(file);

                    //Here, we store the source file’s name in the tag data of the stub file.
                    //You can retrieve this tag data in the filter’s callback function whenever the stub file is accessed.
                    byte[] tagData = ASCIIEncoding.Unicode.GetBytes(file);

                    try
                    {
                        ret = FilterAPI.CreateVirtualStubFile(stubFileName, fileInfo.Length, (uint)tagData.Length, tagData);
                        if (!ret)
                        {
                            EventManager.WriteMessage(100, "createTestStubFile", EventLevel.Error, "Create stub file:" + stubFileName + " failed.\n" + FilterAPI.GetLastErrorMessage());
                            continue;
                        }

                        FileInfo stubInfo = new FileInfo(stubFileName);
                        stubInfo.Attributes |= FileAttributes.Offline;

                    }
                    catch (Exception ex)
                    {
                        EventManager.WriteMessage(150, "createTestStubFile", EventLevel.Error, "Create stub file:" + stubFileName + " failed.\n" + ex.Message);
                    }

                }

                return ret;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Create test stub file got exception:" + ex.Message, "StubFile", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

        }


    }
}
