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

using EaseFilter.CommonObjects;

namespace RegMon
{

    public class RegistryMessage : IDisposable
    {

        ListView listView_Message = null;
        Thread messageThread = null;
        Queue<FilterAPI.MessageSendData> messageQueue = new Queue<FilterAPI.MessageSendData>();

        AutoResetEvent autoEvent = new AutoResetEvent(false);
        bool disposed = false;


        public RegistryMessage(ListView lvMessage)
        {
            this.listView_Message = lvMessage;
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

        ~RegistryMessage()
        {
            Dispose(false);
        }

        public void AddMessage(FilterAPI.MessageSendData messageSend)
        {
            lock (messageQueue)
            {
                if (messageQueue.Count > GlobalConfig.MaximumFilterMessages)
                {
                    messageQueue.Clear();
                }

                messageQueue.Enqueue(messageSend);
            }

            autoEvent.Set();

        }


        void ProcessMessage()
        {
            int waitTimeout = 2000; //2seconds
            List<ListViewItem> itemList = new List<ListViewItem>();

            WaitHandle[] waitHandles = new WaitHandle[] { autoEvent, GlobalConfig.stopEvent };

            while (GlobalConfig.isRunning)
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
                    FilterAPI.MessageSendData messageSend;

                    lock (messageQueue)
                    {
                        messageSend = (FilterAPI.MessageSendData)messageQueue.Dequeue();
                    }

                    string[] filterMessages = new string[0];
                    ListViewItem lvItem = FormatRegistryMessage(messageSend);
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
                    listView_Message.Items.Clear();
                }


                if (itemList.Count > 0)
                {
                    listView_Message.Items.AddRange(itemList.ToArray());
                    //  listView_Message.EnsureVisible(listView_Message.Items.Count - 1);

                    itemList.Clear();

                }
            }
        }


        public string FormatDateTime(long lDateTime)
        {
            try
            {
                if (0 == lDateTime)
                {
                    return "0";
                }

                DateTime dateTime = DateTime.FromFileTime(lDateTime);
                string ret = dateTime.ToString("yyyy-MM-ddTHH:mm");
                return ret;
            }
            catch (Exception ex)
            {
                EventManager.WriteMessage(502, "FormatDateTime", EventLevel.Error, "FormatDateTime :" + lDateTime.ToString() + " failed." + ex.Message);
                return ex.Message;
            }
        }

        private string FormatDescription(FilterAPI.PROCESS_INFO processInfo)
        {
            string message = string.Empty;

            switch (processInfo.MessageType)
            {
                case (uint)FilterAPI.FilterCommand.FILTER_SEND_PROCESS_CREATION_INFO:
                    {
                        message = "ParentPid:" + processInfo.ParentProcessId + ";CreatingPid:" + processInfo.CreatingProcessId + ";CreatingTheadId:" + processInfo.CreatingThreadId
                            + ";FileOpenNameAvailable:" + processInfo.FileOpenNameAvailable + ";CommandLine:" + processInfo.CommandLine;
                        break;
                    }
                case (uint)FilterAPI.FilterCommand.FILTER_SEND_PROCESS_HANDLE_INFO:
                case (uint)FilterAPI.FilterCommand.FILTER_SEND_THREAD_HANDLE_INFO:
                    {
                        if (processInfo.Operation == 1)
                        {
                            message = "Create Handle";
                        }
                        else
                        {
                            message = "Duplicate Handle";
                        }

                        message += "; DesiredAccess:" + processInfo.DesiredAccess.ToString("X");
                        break;
                    }

                default:
                    break;
            }

            return message;
        }

        private void AddItemToList(ListViewItem lvItem)
        {
            if (listView_Message.InvokeRequired)
            {
                listView_Message.Invoke(new MethodInvoker(delegate { AddItemToList(lvItem); }));
            }
            else
            {

                while (listView_Message.Items.Count > GlobalConfig.MaximumFilterMessages)
                {
                    listView_Message.Items.RemoveAt(0);
                }

                listView_Message.Items.Add(lvItem);

            }
        }

        string GetRegCallbackClassName(FilterAPI.MessageSendData messageSend)
        {
            if (messageSend.MessageType == (uint)FilterAPI.FilterCommand.FILTER_SEND_REG_CALLBACK_INFO)
            {
                return ((FilterAPI.RegCallbackClass)messageSend.Offset).ToString();
            }
            else
            {
                return "";
            }
        }

        public ListViewItem FormatRegistryMessage(FilterAPI.MessageSendData messageSend)
        {
            ListViewItem lvItem = new ListViewItem();

            try
            {

                string userName = string.Empty;
                string processName = string.Empty;

                FilterAPI.DecodeUserName(messageSend.Sid, out userName);
                FilterAPI.DecodeProcessName(messageSend.ProcessId, out processName);      

                string[] listData = new string[listView_Message.Columns.Count];
                int col = 0;
                listData[col++] = messageSend.MessageId.ToString();
                listData[col++] = FormatDateTime(messageSend.TransactionTime);
                listData[col++] = userName;
                listData[col++] = processName + "  (" + messageSend.ProcessId + ")";
                listData[col++] = messageSend.ThreadId.ToString();
                listData[col++] = GetRegCallbackClassName(messageSend);
                listData[col++] = messageSend.FileName;
                listData[col++] = FilterMessage.FormatStatus(messageSend.Status);
                listData[col++] = RegistryHandler.FormatDescription(messageSend);

                lvItem = new ListViewItem(listData, 0);

                if (messageSend.Status >= (uint)NtStatus.Status.Error)
                {
                    lvItem.BackColor = Color.LightGray;
                    lvItem.ForeColor = Color.Red;
                }
                else if (messageSend.Status > (uint)NtStatus.Status.Warning)
                {
                    lvItem.BackColor = Color.LightGray;
                    lvItem.ForeColor = Color.Yellow;
                }


                if (GlobalConfig.EnableLogTransaction)
                {
                    FilterMessage.LogTrasaction(listData);
                }

                AddItemToList(lvItem);

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

