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
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.Security.Principal;
using System.Threading;
using System.Reflection;

using EaseFilter.FilterControl;
using EaseFilter.CommonObjects;

namespace ProcessMon
{

    public class ProcessHandler : IDisposable
    {

        ListView listView_Info = null;
        Thread messageThread = null;
        Queue<ProcessEventArgs> messageQueue = new Queue<ProcessEventArgs>();

        AutoResetEvent autoEvent = new AutoResetEvent(false);
        bool disposed = false;


        public ProcessHandler(ListView lvMessage)
        {
            this.listView_Info = lvMessage;
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

        ~ProcessHandler()
        {
            Dispose(false);
        }

        public void InitListView()
        {
            listView_Info.Clear();		//clear control
            //create column header for ListView
            listView_Info.Columns.Add("#", 40, System.Windows.Forms.HorizontalAlignment.Left);
            listView_Info.Columns.Add("MessageType", 160, System.Windows.Forms.HorizontalAlignment.Left);
            listView_Info.Columns.Add("UserName", 150, System.Windows.Forms.HorizontalAlignment.Left);
            listView_Info.Columns.Add("ImageName(PID)", 200, System.Windows.Forms.HorizontalAlignment.Left);
            listView_Info.Columns.Add("ThreadId", 60, System.Windows.Forms.HorizontalAlignment.Left);
            listView_Info.Columns.Add("Description", 600, System.Windows.Forms.HorizontalAlignment.Left);
        }

        public void DisplayEventMessage(ProcessEventArgs processEventArgs)
        {
            if (GlobalConfig.OutputMessageToConsole)
            {
                lock (messageQueue)
                {
                    if (messageQueue.Count > GlobalConfig.MaximumFilterMessages)
                    {
                        messageQueue.Clear();
                    }

                    messageQueue.Enqueue(processEventArgs);
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
                            Utils.ToDebugger("Service stopped, processMessage thread exited.");
                            return;
                        }
                    }

                    System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
                    stopWatch.Start();

                    while (messageQueue.Count > 0)
                    {
                        ProcessEventArgs processEventArgs;

                        lock (messageQueue)
                        {
                            processEventArgs = (ProcessEventArgs)messageQueue.Dequeue();
                        }

                        string[] filterMessages = new string[0];
                        ListViewItem lvItem = FormatProcessInfo(processEventArgs);
                        itemList.Add(lvItem);

                        if (itemList.Count > GlobalConfig.MaximumFilterMessages)
                        {
                            AddItemToList(itemList);
                            itemList.Clear();

                            Thread.Sleep(1000);
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
                    itemList.Clear();
                    Utils.ToDebugger("ProcessMessage exception:" + ex.Message);
                    EventManager.WriteMessage(160, "ProcessMessage", EventLevel.Error, "ProcessMessage exception:" + ex.Message);
                }
            }

        }

        void AddItemToList(List<ListViewItem> itemList)
        {
            if (itemList.Count < 1)
            {
                return;
            }

            if (listView_Info.InvokeRequired)
            {
                listView_Info.Invoke(new MethodInvoker(delegate { AddItemToList(itemList); }));
            }
            else
            {

                while (listView_Info.Items.Count > 0 && listView_Info.Items.Count + itemList.Count > GlobalConfig.MaximumFilterMessages)
                {
                    //the message records in the list view reached to the maximum value, remove the first one till the record less than the maximum value.
                    listView_Info.Items.Clear();
                }


                if (itemList.Count > 0)
                {
                    listView_Info.Items.AddRange(itemList.ToArray());
                    //  listView_Message.EnsureVisible(listView_Message.Items.Count - 1);

                    itemList.Clear();

                }
            }
        }


        private void AddItemToList(ListViewItem lvItem)
        {
            if (listView_Info.InvokeRequired)
            {
                listView_Info.Invoke(new MethodInvoker(delegate { AddItemToList(lvItem); }));
            }
            else
            {

                while (listView_Info.Items.Count > GlobalConfig.MaximumFilterMessages)
                {
                    listView_Info.Items.RemoveAt(0);
                }

                listView_Info.Items.Add(lvItem);

            }
        }

        public ListViewItem FormatProcessInfo(ProcessEventArgs processArgs)
        {
            ListViewItem lvItem = new ListViewItem();

            try
            {
                string[] listData = new string[listView_Info.Columns.Count];
                int col = 0;
                listData[col++] = processArgs.MessageId.ToString();
                listData[col++] = processArgs.EventName;
                listData[col++] = processArgs.UserName;
                listData[col++] = processArgs.ImageFileName + "  (" + processArgs.ProcessId + ")";
                listData[col++] = processArgs.ThreadId.ToString();
                listData[col++] = processArgs.Description;

                lvItem = new ListViewItem(listData, 0);

                if (processArgs.IoStatus >= NtStatus.Status.Error)
                {
                    lvItem.BackColor = Color.LightGray;
                    lvItem.ForeColor = Color.Red;
                }
                else if (processArgs.IoStatus > NtStatus.Status.Warning)
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
        /// Fires this event when the new process was being created.
        /// </summary>
        public void OnProcessCreation(object sender, ProcessEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

           //   //test block the process creation.
           //    e.ReturnStatus = NtStatus.Status.AccessDenied;
        }

        /// <summary>
        /// Fires this event before the processs was terminiated.
        /// </summary>
        public void OnProcessPreTermination(object sender, ProcessEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //test block the process terminiation.
            //if (e.ImageFileName.IndexOf("cmd.exe") >= 0)
            //{
            //    e.ReturnStatus = NtStatus.Status.AccessDenied;
            //}
        }

        /// <summary>
        /// Fires this event when the process creation was blocked by the setting,
        /// if the control flag 'ENABLE_SEND_PROCESS_DENIED_EVENT' was set.
        /// </summary>
        public void NotifyProcessWasBlocked(object sender, ProcessEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

        }

        /// <summary>
        /// Fires this event after the event was triggered.
        /// </summary>
        public void NotifyProcessTerminated(object sender, ProcessEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
          
        }

        /// <summary>
        /// Fires this event after the event was triggered.
        /// </summary>
        public void NotifyThreadCreation(object sender, ProcessEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

        }

        /// <summary>
        /// Fires this event after the event was triggered.
        /// </summary>
        public void NotifyThreadTerminated(object sender, ProcessEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

        }

        /// <summary>
        /// Fires this event after the event was triggered.
        /// </summary>
        public void NotifyProcessHandleInfo(object sender, ProcessEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

        }

        /// <summary>
        /// Fires this event after the event was triggered.
        /// </summary>
        public void NotifyThreadHandleInfo(object sender, ProcessEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

        }

        /// <summary>
        /// Fires this event after the image was loaded to memory.
        /// </summary>
        public void NotifyImageWasLoaded(object sender, ProcessEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

        }

    }
}

