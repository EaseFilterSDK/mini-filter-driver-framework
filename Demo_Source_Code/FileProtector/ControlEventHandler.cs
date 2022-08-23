///////////////////////////////////////////////////////////////////////////////
//
//    (C) Copyright 2011 EaseFilter Technologies
//    All Rights Reserved
//
//    This software is part of a licensed software product and may
//    only be used or copied in accordance with the terms of that license.
//
//    NOTE:  THIS MODULE IS UNSUPPORTED SAMPLE CODE
//
//    This module contains sample code provided for convenience and
//    demonstration purposes only,this software is provided on an 
//    "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, 
//     either express or implied.  
//
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.IO;
using System.Threading;
using System.Reflection;

using EaseFilter.FilterControl;
using EaseFilter.CommonObjects;

namespace FileProtector
{
    
    public class ControlEventHandler : IDisposable
    {

        FastListView listView_Message = null;
        Thread messageThread = null;
        Queue<FileIOEventArgs> messageQueue = new Queue<FileIOEventArgs>();

        AutoResetEvent autoEvent = new AutoResetEvent(false);
        bool disposed = false;


        public ControlEventHandler(FastListView lvMessage)
        {
            this.listView_Message = lvMessage;
            InitListView();
            messageThread = new Thread(new ThreadStart(ProcessMessage));
            messageThread.Name = "ProcessMessage";
            messageThread.Start();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
            }

            autoEvent.Set();
            messageThread.Abort();
            disposed = true;
        }

        ~ControlEventHandler()
        {
            Dispose(false);
        }

        public void InitListView()
        {
            messageQueue.Clear();
            //init ListView control
            listView_Message.Clear();		//clear control
            //create column header for ListView
            listView_Message.Columns.Add("MessageId#", 40, System.Windows.Forms.HorizontalAlignment.Left);
            listView_Message.Columns.Add("Time", 120, System.Windows.Forms.HorizontalAlignment.Left);
            listView_Message.Columns.Add("UserName", 150, System.Windows.Forms.HorizontalAlignment.Left);
            listView_Message.Columns.Add("ProcessName(PID)", 100, System.Windows.Forms.HorizontalAlignment.Left);
            listView_Message.Columns.Add("ThreadId", 60, System.Windows.Forms.HorizontalAlignment.Left);
            listView_Message.Columns.Add("I/O Name", 160, System.Windows.Forms.HorizontalAlignment.Left);
            listView_Message.Columns.Add("FileObject", 100, System.Windows.Forms.HorizontalAlignment.Left);
            listView_Message.Columns.Add("FileName", 350, System.Windows.Forms.HorizontalAlignment.Left);
            listView_Message.Columns.Add("FileSize", 70, System.Windows.Forms.HorizontalAlignment.Left);
            listView_Message.Columns.Add("FileAttributes", 70, System.Windows.Forms.HorizontalAlignment.Left);
            listView_Message.Columns.Add("LastWriteTime", 120, System.Windows.Forms.HorizontalAlignment.Left);
            listView_Message.Columns.Add("Return Status", 100, System.Windows.Forms.HorizontalAlignment.Left);
            listView_Message.Columns.Add("Description", 500, System.Windows.Forms.HorizontalAlignment.Left);
        }

        public void DisplayEventMessage(FileIOEventArgs fileIOEventArgs)
        {
            if (GlobalConfig.OutputMessageToConsole)
            {
                lock (messageQueue)
                {                   
                    if (messageQueue.Count > GlobalConfig.MaximumFilterMessages)
                    {
                        messageQueue.Clear();
                    }

                    messageQueue.Enqueue(fileIOEventArgs);
                }

                autoEvent.Set();
            }

        }


        void ProcessMessage()
        {
            int waitTimeout = 2000; //2seconds
            List<ListViewItem> itemList = new List<ListViewItem>();

            WaitHandle[] waitHandles = new WaitHandle[] { autoEvent, GlobalConfig.stopEvent };

            while (GlobalConfig.isRunning)
            {
                try
                {
                    if (messageQueue.Count == 0)
                    {
                        int result = WaitHandle.WaitAny(waitHandles, waitTimeout);
                        if (!GlobalConfig.isRunning)
                        {
                            return;
                        }
                    }

                    System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
                    stopWatch.Start();

                    while (messageQueue.Count > 0)
                    {
                        FileIOEventArgs fileIOEventArgs = null;

                        lock (messageQueue)
                        {
                            fileIOEventArgs = (FileIOEventArgs)messageQueue.Dequeue();
                        }

                        if (GlobalConfig.DisableDirIO && (fileIOEventArgs.FileAttributes & (uint)FileAttributes.Directory) == (uint)FileAttributes.Directory)
                        {
                            //don't display the directory IO 
                            continue;
                        }

                        string[] filterMessages = new string[0];
                        ListViewItem lvItem = GetFilterMessage(fileIOEventArgs, ref filterMessages);

                        if (null == lvItem)
                        {
                            //no display message.
                            continue;
                        }

                        if (GlobalConfig.EnableLogTransaction)
                        {
                            LogTrasaction(filterMessages);
                        }

                        if (GlobalConfig.OutputMessageToConsole)
                        {
                            itemList.Add(lvItem);

                            if (itemList.Count > GlobalConfig.MaximumFilterMessages)
                            {
                                AddItemToList(itemList);
                                itemList.Clear();

                                Thread.Sleep(1000);
                            }
                        }

                    }

                    if (itemList.Count > 0)
                    {
                        AddItemToList(itemList);
                        itemList.Clear();
                    }
                }
                catch (Exception ex)
                {
                    EventManager.WriteMessage(190, "ProcessMessage", EventLevel.Error, "ProcessMessage failed with error " + ex.Message);
                }

            }


        }

        public void LoadMessageFromLogToConsole()
        {
            if (listView_Message == null)
            {
                return;
            }

            try
            {
                Assembly assembly = System.Reflection.Assembly.GetEntryAssembly();
                string assemblyPath = Path.GetDirectoryName(assembly.Location);

                string filterMessageLogName = GlobalConfig.FilterMessageLogName;
                string logFileName = Path.Combine(assemblyPath, filterMessageLogName);

                if (!File.Exists(logFileName))
                {
                    return;
                }

                string[] logMessage = File.ReadAllLines(logFileName);
                List<ListViewItem> itemList = new List<ListViewItem>();

                foreach (string itemStr in logMessage)
                {
                    if (itemStr.Trim().Length > 0)
                    {
                        string[] listData = itemStr.Split(new char[] { '|' });
                        if (listData.Length >= listView_Message.Columns.Count)
                        {
                            ListViewItem lvItem = new ListViewItem(listData, 0);

                            uint status = 0;
                            string statusStr = listData[listView_Message.Columns.Count - 2];
                            int index1 = statusStr.IndexOf('(');
                            int index2 = statusStr.IndexOf(')');

                            if (index1 >= 0 && index2 > 2)
                            {
                                statusStr = statusStr.Substring(index1 + 3, index2 - index1 - 3);
                                uint.TryParse(statusStr, System.Globalization.NumberStyles.HexNumber, null, out status);

                                if (status >= (uint)NtStatus.Status.Error)
                                {
                                    lvItem.BackColor = Color.LightGray;
                                    lvItem.ForeColor = Color.Red;
                                }
                                else if (status > (uint)NtStatus.Status.Warning)
                                {
                                    lvItem.BackColor = Color.LightGray;
                                    lvItem.ForeColor = Color.Yellow;
                                }
                            }

                            itemList.Add(lvItem);
                        }
                    }
                }

                if (itemList.Count > 0)
                {
                    listView_Message.Items.Clear();
                    AddItemToList(itemList);
                }

            }
            catch (Exception ex)
            {
                EventManager.WriteMessage(265, "LoadLogMessage", EventLevel.Error, "Load log message failed with error " + ex.Message);
            }
        }

        public static void LogTrasaction(string[] filterMessages)
        {
            try
            {
                Assembly assembly = System.Reflection.Assembly.GetEntryAssembly();
                string assemblyPath = Path.GetDirectoryName(assembly.Location);

                string filterMessageLogName = GlobalConfig.FilterMessageLogName;

                string logFileName = Path.Combine(assemblyPath, filterMessageLogName);
                string logFolder = Path.GetDirectoryName(logFileName);

                if (!Directory.Exists(logFolder))
                {
                    Directory.CreateDirectory(logFolder);
                }

                string logMessage = string.Empty;

                for (int i = 0; i < filterMessages.Length; i++)
                {
                    logMessage += filterMessages[i].Trim() + "|";
                }

                if (logMessage.Length > 0)
                {
                    File.AppendAllText(logFileName, logMessage + "\r\n");

                    FileInfo fileInfo = new FileInfo(logFileName);

                    if (fileInfo.Length > GlobalConfig.FilterMessageLogFileSize)
                    {
                        File.Delete(logFileName + ".bak");
                        File.Move(logFileName, logFileName + ".bak");
                    }
                }
            }
            catch (Exception ex)
            {
                EventManager.WriteMessage(172, "LogTrasaction", EventLevel.Error, "Log filter message failed with error " + ex.Message);
            }

        }

        void AddItemToList(List<ListViewItem> itemList)
        {
            if (itemList.Count < 1)
            {
                return;
            }

            if (listView_Message.InvokeRequired)
            {
                listView_Message.Invoke(new MethodInvoker(delegate { AddItemToList(itemList); }));
            }
            else
            {

                while (listView_Message.Items.Count > 0 && listView_Message.Items.Count + itemList.Count > GlobalConfig.MaximumFilterMessages)
                {
                    //the message records in the list view reached to the maximum value, remove the first one till the record less than the maximum value.
                    listView_Message.Items.RemoveAt(0);
                }


                if (itemList.Count > 0)
                {
                    listView_Message.Items.AddRange(itemList.ToArray());
                    //  listView_Message.EnsureVisible(listView_Message.Items.Count - 1);

                    itemList.Clear();

                }
            }
        }

        ListViewItem GetFilterMessage(FileIOEventArgs fileIOEventArgs, ref string[] filterMessages)
        {
            ListViewItem lvItem = new ListViewItem();

            try
            {
                string[] listData = new string[listView_Message.Columns.Count];
                int col = 0;
                listData[col++] = fileIOEventArgs.MessageId.ToString();
                listData[col++] = DateTime.FromFileTime(fileIOEventArgs.TransactionTime).ToString("yyyy-MM-ddTHH:mm");
                listData[col++] = fileIOEventArgs.UserName;
                listData[col++] = fileIOEventArgs.ProcessName + "  (" + fileIOEventArgs.ProcessId + ")";
                listData[col++] = fileIOEventArgs.ThreadId.ToString();
                listData[col++] = fileIOEventArgs.EventName;
                listData[col++] = fileIOEventArgs.FileObject.ToString("X");
                listData[col++] = fileIOEventArgs.FileName;
                listData[col++] = fileIOEventArgs.FileSize.ToString();
                listData[col++] = ((FileAttributes)fileIOEventArgs.FileAttributes).ToString();
                listData[col++] = DateTime.FromFileTime(fileIOEventArgs.LastWriteTime).ToString("yyyy-MM-ddTHH:mm");
                listData[col++] = fileIOEventArgs.IOStatusToString();
                listData[col++] = fileIOEventArgs.Description;

                filterMessages = listData;

                lvItem = new ListViewItem(listData, 0);

                if ((uint)fileIOEventArgs.IoStatus >= (uint)NtStatus.Status.Error)
                {
                    lvItem.BackColor = Color.LightGray;
                    lvItem.ForeColor = Color.Red;
                }
                else if ((uint)fileIOEventArgs.IoStatus > (uint)NtStatus.Status.Warning)
                {
                    lvItem.BackColor = Color.LightGray;
                    lvItem.ForeColor = Color.Yellow;
                }


            }
            catch (Exception ex)
            {
                EventManager.WriteMessage(445, "GetFilterMessage", EventLevel.Error, "Add callback message failed." + ex.Message);
                lvItem = null;
            }

            return lvItem;
        }

        /// <summary>
        /// Fires this event after the new file was created, the handle is not closed.
        /// </summary>
        public void OnPreCreateFile(object sender, FileCreateEventArgs e)
        {
             DisplayEventMessage(e);
            //do your job here.

             //if (string.Compare(YourTestFileName, e.FileName, true) == 0)
             //{
             //    //test block the file open here.
             //    e.ReturnStatus = NtStatus.Status.AccessDenied;
             //}
             //else if (string.Compare(unitTestCallbackTestReparseFile, e.FileName, true) == 0)
             //{
             //    //test reparse file open to other file.
             //    e.reparseFileName = unitTestCallbackReparseTargetFile;
             //    e.ReturnStatus = NtStatus.Status.Reparse;
             //    e.IsDataModified = true;
             //}

           
        }
        
        /// <summary>
        /// Fires this event after the new file was created, the handle is not closed.
        /// </summary>
        public void OnPostCreateFile(object sender, FileCreateEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }

        /// <summary>
        /// Fires this event after the read IO was returned.
        /// </summary>
        public void OnPreFileRead(object sender, FileReadEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //if (string.Compare(YourTestFileName, e.FileName, true) == 0)
            //{
            //    //test block the file read here.
            //    e.ReturnStatus = NtStatus.Status.AccessDenied;
            //}
        }

        /// <summary>
        /// Fires this event after the read IO was returned.
        /// </summary>
        public void OnPostFileRead(object sender, FileReadEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }

        /// <summary>
        /// Fires this event after the write IO was returned.
        /// </summary>
        public void OnPreFileWrite(object sender, FileWriteEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //if (string.Compare(YourTestFileName, e.FileName, true) == 0)
            //{
            //    //test block the file write here.
            //    e.ReturnStatus = NtStatus.Status.AccessDenied;
            //}
        }

        /// <summary>
        /// Fires this event after the write IO was returned.
        /// </summary>
        public void OnPostFileWrite(object sender, FileWriteEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }

        /// <summary>
        /// Fires this event after the query file size IO was returned.
        /// </summary>
        public void OnPreQueryFileSize(object sender, FileSizeEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //if (string.Compare(YourTestFileName, e.FileName, true) == 0)
            //{
            //    //test block the file size query here.
            //    e.ReturnStatus = NtStatus.Status.AccessDenied;
            //}
            //else if (string.Compare(YourTestFileName, e.FileName, true) == 0)
            //{
            //  //test replace the file size with your own, complete the pre-query IO.
            //  e.fileSizeToQueryOrSet = 1000;//change the file size with your own
            //  e.ReturnStatus = NtStatus.Status.Success;
            //  e.IsDataModified = true;
            //}
        }

        /// <summary>
        /// Fires this event after the query file size IO was returned.
        /// </summary>
        public void OnPostQueryFileSize(object sender, FileSizeEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //if (string.Compare(YourTestFileName, e.FileName, true) == 0)
            //{
            //  //test replace the file size with your own.
            //  e.fileSizeToQueryOrSet = 1000;//change the file size with your own
            //  e.ReturnStatus = NtStatus.Status.Success;
            //  e.IsDataModified = true;
            //}
        }

        /// <summary>
        /// Fires this event after the query file basic information IO was returned.
        /// </summary>
        public void OnPreQueryFileBasicInfo(object sender, FileBasicInfoEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //if (string.Compare(YourTestFileName, e.FileName, true) == 0)
            //{
            //    //test block the file basic info query here.
            //    e.ReturnStatus = NtStatus.Status.AccessDenied;
            //}
            //else if (string.Compare(YourTestFileName, e.FileName, true) == 0)
            //{
            //  //test replace the basic file info with your own, complete the pre-query IO.
                //WinData.FileBasicInformation basicInfo = new WinData.FileBasicInformation();
                //basicInfo.CreationTime = basicInfo.LastWriteTime = DateTime.Now.ToFileTime();
                //e.basicInfo = basicInfo;//change the file basic info with your own
                //e.ReturnStatus = NtStatus.Status.Success;
                //  e.IsDataModified = true;
            //}
        }

        /// <summary>
        /// Fires this event after the query file basic information IO was returned.
        /// </summary>
        public void OnPostQueryFileBasicInfo(object sender, FileBasicInfoEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //  //test replace the basic file info with your own, complete the pre-query IO.
            //WinData.FileBasicInformation basicInfo = new WinData.FileBasicInformation();
            //basicInfo.CreationTime = basicInfo.LastWriteTime = DateTime.Now.ToFileTime();
            //e.basicInfo = basicInfo;//change the file basic info with your own
            //e.ReturnStatus = NtStatus.Status.Success;
            //  e.IsDataModified = true;
        }

        /// <summary>
        /// Fires this event after the query file standard information IO was returned.
        /// </summary>
        public void OnPreQueryFileStandardInfo(object sender, FileStandardInfoEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //if (string.Compare(YourTestFileName, e.FileName, true) == 0)
            //{
            //    //test block the file standard info query here.
            //    e.ReturnStatus = NtStatus.Status.AccessDenied;
            //}
            //else if (string.Compare(YourTestFileName, e.FileName, true) == 0)
            //{
            //  //test replace the file standard info with your own, complete the pre-query IO.
                //WinData.FileStandardInformation standardInfo = new WinData.FileStandardInformation();
                //standardInfo.EndOfFile = 1000; //new file size
                //e.standardInfo = standardInfo;//change the file basic info with your own
                //e.ReturnStatus = NtStatus.Status.Success;
                //e.IsDataModified = true;
            //}
        }

        /// <summary>
        /// Fires this event after the query file standard information IO was returned.
        /// </summary>
        public void OnPostQueryFileStandardInfo(object sender, FileStandardInfoEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //  //test replace the file standard info with your own
            //WinData.FileStandardInformation standardInfo = new WinData.FileStandardInformation();
            //standardInfo.EndOfFile = 1000; //new file size
            //e.standardInfo = standardInfo;//change the file basic info with your own
            //e.ReturnStatus = NtStatus.Status.Success;
            //e.IsDataModified = true;
        }

        /// <summary>
        /// Fires this event after the query file network information IO was returned.
        /// </summary>
        public void OnPreQueryFileNetworkInfo(object sender, FileNetworkInfoEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //if (string.Compare(YourTestFileName, e.FileName, true) == 0)
            //{
            //    //test block the file network info query here.
            //    e.ReturnStatus = NtStatus.Status.AccessDenied;
            //}
            //else if (string.Compare(YourTestFileName, e.FileName, true) == 0)
            //{
            //  //test replace the file standard info with your own, complete the pre-query IO.
                //WinData.FileNetworkInformation networkInfo = new WinData.FileNetworkInformation();
                //networkInfo.CreationTime = networkInfo.LastWriteTime = networkInfo.ChangeTime = DateTime.Now.ToFileTime(); //new file time
                //e.networkInfo = networkInfo;//change the file networkInfo  with your own
                //e.ReturnStatus = NtStatus.Status.Success;
                //e.IsDataModified = true;
            //}
        }

        /// <summary>
        /// Fires this event after the query file network information IO was returned.
        /// </summary>
        public void OnPostQueryFileNetworkInfo(object sender, FileNetworkInfoEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //  //test replace the file standard info with your own, complete the pre-query IO.
            //WinData.FileNetworkInformation networkInfo = new WinData.FileNetworkInformation();
            //networkInfo.CreationTime = networkInfo.LastWriteTime = networkInfo.ChangeTime = DateTime.Now.ToFileTime(); //new file time
            //e.networkInfo = networkInfo;//change the file networkInfo  with your own
            //e.ReturnStatus = NtStatus.Status.Success;
            //e.IsDataModified = true;

        }

        /// <summary>
        /// Fires this event after the query file Id IO was returned.
        /// </summary>
        public void OnPreQueryFileId(object sender, FileIdEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //if (string.Compare(YourTestFileName, e.FileName, true) == 0)
            //{
            //    //test block the file Id query here.
            //    e.ReturnStatus = NtStatus.Status.AccessDenied;
            //}
        }

        /// <summary>
        /// Fires this event after the query file Id IO was returned.
        /// </summary>
        public void OnPostQueryFileId(object sender, FileIdEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }

        /// <summary>
        /// Fires this event after the query file info IO was returned.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnPreQueryFileInfo(object sender, FileInfoArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //the QueryFileInfo includes all the information class as "WinData.FileInfomationClass"
            //we already handled some common query information class as above "Query file size, file basic info, file standard info, file network info, file Id."
            //all other classes were handled here, for more information please reference "FltQueryInformationFile".

            //if (string.Compare(YourTestFileName, e.FileName, true) == 0)
            //{
            //    //test block the file info query here.
            //    e.ReturnStatus = NtStatus.Status.AccessDenied;
            //}
        }

        /// <summary>
        /// Fires this event after the query file info IO was returned.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnPostQueryFileInfo(object sender, FileInfoArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //to get the query file info result, first you need to get the information class:
            //e.FileInfoClass, then get the structure of the class
            //e.DataBuffer has the information data of the file associated to the info class. 

        }

        /// <summary>
        /// Fires this event after the set file size IO was returned.
        /// </summary>
        public void OnPreSetFileSize(object sender, FileSizeEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //if (string.Compare(YourTestFileName, e.FileName, true) == 0)
            //{
            //    //test block the set file size IO.
            //  e.fileSizeToQueryOrSet is the file size will be set
            //    e.ReturnStatus = NtStatus.Status.AccessDenied;
            //}
        }

        /// <summary>
        /// Fires this event after the set file size IO was returned.
        /// </summary>
        public void OnPostSetFileSize(object sender, FileSizeEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }

        /// <summary>
        /// Fires this event after the set file basic information was returned.
        /// </summary>
        public void OnPreSetFileBasicInfo(object sender, FileBasicInfoEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //if (string.Compare(YourTestFileName, e.FileName, true) == 0)
            //{
            //    //test block the set file basic info IO.
            //  e.basicInfo is the file basic info will be set
            //    e.ReturnStatus = NtStatus.Status.AccessDenied;
            //}
        }

        /// <summary>
        /// Fires this event after the set file basic information was returned.
        /// </summary>
        public void OnPostSetFileBasicInfo(object sender, FileBasicInfoEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }

        /// <summary>
        /// Fires this event after the set file standard information IO was returned.
        /// </summary>
        public void OnPreSetFileStandardInfo(object sender, FileStandardInfoEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //if (string.Compare(YourTestFileName, e.FileName, true) == 0)
            //{
            //    //test block the set file standard info IO.
            //  e.standardInfo is the file standard info will be set
            //    e.ReturnStatus = NtStatus.Status.AccessDenied;
            //}
        }

        /// <summary>
        /// Fires this event after the set file standard information IO was returned.
        /// </summary>
        public void OnPostSetFileStandardInfo(object sender, FileStandardInfoEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }


        /// <summary>
        /// Fires this event after the set file network information IO was returned.
        /// </summary>
        public void OnPreSetFileNetworkInfo(object sender, FileNetworkInfoEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.


            //if (string.Compare(YourTestFileName, e.FileName, true) == 0)
            //{
            //    //test block the set file standard info IO.
            //  e.networkInfo is the file standard info will be set
            //    e.ReturnStatus = NtStatus.Status.AccessDenied;
            //}
        }

        /// <summary>
        /// Fires this event after the set file network information IO was returned.
        /// </summary>
        public void OnPostSetFileNetworkInfo(object sender, FileNetworkInfoEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }

        /// <summary>
        /// Fires this event after the file was moved or renamed.
        /// </summary>
        public void OnPreMoveOrRenameFile(object sender, FileMoveOrRenameEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //if (string.Compare(YourTestFileName, e.FileName, true) == 0)
            //{
            //    //test block the file rename here.
            //      e.newFileName is the new file name will be changed
            //    e.ReturnStatus = NtStatus.Status.AccessDenied;
            //}
        }

        /// <summary>
        /// Fires this event after the file was moved or renamed.
        /// </summary>
        public void OnPostMoveOrRenameFile(object sender, FileMoveOrRenameEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }

        /// <summary>
        /// Fires this event after the delete IO was returned.
        /// </summary>
        public void OnPreDeleteFile(object sender, FileIOEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //if (string.Compare(YourTestFileName, e.FileName, true) == 0)
            //{
            //test block the file delete here.
            e.ReturnStatus = NtStatus.Status.AccessDenied;
            //}
        }

        /// <summary>
        /// Fires this event after the delete IO was returned.
        /// </summary>
        public void OnPostDeleteFile(object sender, FileIOEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }

        /// <summary>
        /// Fires this event after the set file info IO was returned.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnPreSetFileInfo(object sender, FileInfoArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //the SetFileInfo includes all the information class as "WinData.FileInfomationClass"
            //we already handled some common set information class as above "Set file size, file basic info, file standard info, file network info, rename, delete."
            //all other classes were handled here, for more information please reference "FltSetInformationFile".

            //if (string.Compare(YourTestFileName, e.FileName, true) == 0)
            //{
            //    //test block the set file info here.
            //    e.ReturnStatus = NtStatus.Status.AccessDenied;
            //}
        }

        /// <summary>
        /// Fires this event after the set file info IO was returned.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnPostSetFileInfo(object sender, FileInfoArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }

        /// <summary>
        /// Fires this event after the query file security IO was returned.
        /// </summary>
        public void OnPreQueryFileSecurity(object sender, FileSecurityEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //if (string.Compare(YourTestFileName, e.FileName, true) == 0)
            //{
            //    //test block the query file security IO here.
            //    e.ReturnStatus = NtStatus.Status.AccessDenied;
            //}
        }

        /// <summary>
        /// Fires this event after the query file security IO was returned.
        /// </summary>
        public void OnPostQueryFileSecurity(object sender, FileSecurityEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //you can get the security data return from the file system.
            //e.securityBuffer
        }

        /// <summary>
        /// Fires this event after the set file security IO was returned.
        /// </summary>
        public void OnPreSetFileSecurity(object sender, FileSecurityEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //if (string.Compare(YourTestFileName, e.FileName, true) == 0)
            //{
            //    //test block the set file security IO here.
            //    e.ReturnStatus = NtStatus.Status.AccessDenied;
            //}
        }

        /// <summary>
        /// Fires this event after the set file security IO was returned.
        /// </summary>
        public void OnPostSetFileSecurity(object sender, FileSecurityEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }


        /// <summary>
        /// Fires this event after the directory enumeration query IO was returned.
        /// </summary>
        public void OnPreQueryDirectoryFile(object sender, FileQueryDirectoryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.


            //if (string.Compare(YourTestFileName, e.FileName, true) == 0)
            //{
            //    //test block the query directory file IO here if you block the file browsing.
            //    e.ReturnStatus = NtStatus.Status.AccessDenied;
            //}
        }

        /// <summary>
        /// Fires this event after the directory enumeration query IO was returned.
        /// </summary>
        public void OnPostQueryDirectoryFile(object sender, FileQueryDirectoryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //get the data of the directory file list based on the information class.
            //e.fileInfomationClass;
            //e.directoryBuffer;

        }


        /// <summary>
        /// Fires this event after the file was cleanuped.
        /// </summary>
        public void OnPreFileHandleClose(object sender, FileIOEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }

        /// <summary>
        /// Fires this event after the file was cleanuped.
        /// </summary>
        public void OnPostFileHandleClose(object sender, FileIOEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }

        /// <summary>
        /// Fires this event after the last file handle of the file was closed.
        /// </summary>
        public void OnPreFileClose(object sender, FileIOEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }

        /// <summary>
        /// Fires this event after the last file handle of the file was closed.
        /// </summary>
        public void OnPostFileClose(object sender, FileIOEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }

        /// <summary>
        /// Fires this event when the filter driver attached to a volume.
        /// </summary>
        public void NotifyFilterAttachToVolume(object sender, VolumeInfoEventArgs e)
        {
            DisplayEventMessage(e);

            //if ((e.DeviceCharacteristics & WinData.DeviceObject_Characteristics.FILE_REMOVABLE_MEDIA) > 0)
            //{
            //    //this is the USB drive, here is the USB dos volume name or drive letter.
            //    //e.VolumeDosName

            //    //you can setup the new filter rule for this USB drive here, for example you block this USB as below:
            //    FileFilter usbFilter = new FileFilter(e.VolumeDosName + "*");
            //    usbFilter.AccessFlags = FilterAPI.AccessFlag.LEAST_ACCESS_FLAG;

            //    if (!FilterAPI.AddFileFilterRule((uint)usbFilter.AccessFlags, usbFilter.IncludeFileFilterMask, usbFilter.IsResident, usbFilter.FilterId))
            //    {
            //        string lastError = "Send filter rule failed:" + FilterAPI.GetLastErrorMessage();
            //    }
            //}
        }

        /// <summary>
        /// Fires this event when a volume was detached from the filter driver.
        /// </summary>
        public void NotifyFilterDetachFromVolume(object sender, VolumeInfoEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }

        /// <summary>
        /// Fires this event when a volume was detached from the filter driver.
        /// </summary>
        public void NotifiyFileIOWasBlocked(object sender, DeniedFileIOEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }

        /// <summary>
        /// Fires this event when a volume was detached from the filter driver.
        /// </summary>
        public void NotifiyProcessTerminatedWasBlocked(object sender, DeniedProcessTerminatedEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }


        /// <summary>
        /// Fires this event when a volume was detached from the filter driver.
        /// </summary>
        public void NotifyUSBReadWasBlocked(object sender, DeniedUSBReadEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }


        /// <summary>
        /// Fires this event when a volume was detached from the filter driver.
        /// </summary>
        public void NotifyUSBWriteWasBlocked(object sender, DeniedUSBWriteEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }

    }
}