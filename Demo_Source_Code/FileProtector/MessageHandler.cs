﻿///////////////////////////////////////////////////////////////////////////////
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

    public class MessageHandler : IDisposable
    {

        FastListView listView_Message = null;
        Thread messageThread = null;
        Queue<FileIOEventArgs> messageQueue = new Queue<FileIOEventArgs>();

        AutoResetEvent autoEvent = new AutoResetEvent(false);
        bool disposed = false;

        public MessageHandler(FastListView lvMessage)
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

        ~MessageHandler()
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
                    EventManager.WriteMessage(190, "ProcessMessage", EventLevel.Error, "Process message exception:" + ex.Message);
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
                if (fileIOEventArgs.IsRemoteAccess)
                {
                    listData[col++] = fileIOEventArgs.UserName + " IsRemoteAccess " + fileIOEventArgs.RemoteIp; ;
                }
                else
                {
                    listData[col++] = fileIOEventArgs.UserName;
                }
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

    }
}
