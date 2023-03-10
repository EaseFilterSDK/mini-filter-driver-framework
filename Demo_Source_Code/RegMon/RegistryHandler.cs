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

using EaseFilter.FilterControl;
using EaseFilter.CommonObjects;

namespace RegMon
{
    public enum VALUE_DATA_TYPE : uint
    {
        REG_NONE = 0,
        REG_SZ = 1,
        REG_EXPAND_SZ = 2,
        REG_BINARY = 3,
        REG_DWORD = 4,
        REG_DWORD_BIG_ENDIAN = 5,
        REG_LINK = 6,
        REG_MULTI_SZ = 7,
        REG_RESOURCE_LIST = 8,
        REG_FULL_RESOURCE_DESCRIPTOR = 9,
        REG_RESOURCE_REQUIREMENTS_LIST = 10,
        REG_QWORD = 11
    }

    public enum KEY_VALUE_INFORMATION_CLASS : uint
    {
        KeyValueBasicInformation = 0,
        KeyValueFullInformation = 1,
        KeyValuePartialInformation = 2,
        KeyValueFullInformationAlign64 = 3,
        KeyValuePartialInformationAlign64 = 4,
        MaxKeyValueInfoClass = 5
    }

    public struct KEY_VALUE_BASIC_INFORMATION
    {
        public uint TitleIndex;
        public uint Type;
        public uint NameLength;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 65536 - 12)]
        public byte[] Name;
    }
    public struct KEY_VALUE_FULL_INFORMATION
    {
        public uint TitleIndex;
        public uint Type;
        public uint DataOffset;
        public uint DataLength;
        public uint NameLength;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 65536 - 20)]
        public byte[] Name;
    }

    public struct KEY_VALUE_PARTIAL_INFORMATION
    {
        public uint TitleIndex;
        public uint Type;
        public uint DataLength;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 65536 - 12 )]
        public byte[] Data;
    }

    public struct KEY_VALUE_ENTRY
    {
        public uint NextEntryOffset;
        public uint ValueNameLength;        
        public uint DataType;
        public uint DataLength;
        public byte[] ValueName;
        //public byte[] Data;
    }

    public enum KEY_SET_INFORMATION_CLASS : uint
    {
        KeyWriteTimeInformation = 0,
        KeyWow64FlagsInformation,
        KeyControlFlagsInformation,
        KeySetVirtualizationInformation,
        KeySetDebugInformation,
        KeySetHandleTagsInformation,
        MaxKeySetInfoClass  // MaxKeySetInfoClass should always be the last enum
    }
    public enum KEY_INFORMATION_CLASS : uint
    {
        KeyBasicInformation = 0,
        KeyNodeInformation,
        KeyFullInformation,
        KeyNameInformation,
        KeyCachedInformation,
        KeyFlagsInformation,
        KeyVirtualizationInformation,
        KeyHandleTagsInformation,
        KeyTrustInformation,
        KeyLayerInformation,
        MaxKeyInfoClass
    }

    public struct KEY_BASIC_INFORMATION
    {
        public long LastWriteTime;
        public uint TitleIndex;
        public uint NameLength;
        public byte[] Name;
    }

    public struct KEY_NODE_INFORMATION
    {
        public long LastWriteTime;
        public uint TitleIndex;
        public uint ClassOffset;
        public uint ClassLength;
        public uint NameLength;
        public byte[] Name;
    }

    public struct KEY_FULL_INFORMATION
    {
        public long LastWriteTime;
        public uint TitleIndex;
        public uint ClassOffset;
        public uint ClassLength;
        public uint SubKeys;
        public uint MaxNameLen;
        public uint MaxClassLen;
        public uint Values;
        public uint MaxValueNameLen;
        public uint MaxValueDataLen;
        public byte[] Class;
    }

    public struct KEY_NAME_INFORMATION
    {
        public uint NameLength;
        public byte[] Name;
    }

    public struct KEY_CACHED_INFORMATION
    {
        public long LastWriteTime;
        public uint TitleIndex;
        public uint SubKeys;
        public uint MaxNameLen;
        public uint MaxClassLen;
        public uint Values;
        public uint MaxValueNameLen;
        public uint MaxValueDataLen;
        public uint NameLength;
    }

    public struct KEY_VIRTUALIZATION_INFORMATION
    {
        public bool VirtualizationCandidate;
        public bool VirtualizationEnabled;
        public bool VirtualTarget;
        public bool VirtualStore;
        public bool VirtualSource;
    }

    public enum REG_ACCESS_MASK : uint
    {
        KEY_QUERY_VALUE = 1,
        KEY_SET_VALUE = 2,
        KEY_CREATE_SUB_KEY = 4,
        KEY_ENUMERATE_SUB_KEYS = 8,
        KEY_NOTIFY = 16,
        KEY_CREATE_LINK = 32,
        KEY_WRITE = 131078,
        KEY_EXECUTE = 131097,
        KEY_READ = 131097,
        KEY_ALL_ACCESS = 983103,
    }

    public enum REG_CREATE_OPTIONS : uint
    {
        REG_OPTION_VOLATILE = 0x00000001,   // Key is not preserved when system is rebooted
        REG_OPTION_CREATE_LINK = 0x00000002,   // Created key is a symbolic link
        REG_OPTION_BACKUP_RESTORE = 0x00000004,   // open for backup or restore special access rules privilege required
        REG_OPTION_OPEN_LINK = 0x00000008,   // Open symbolic link
    }

    public enum REG_DISPOSITION
    {
        REG_CREATED_NEW_KEY = 0x00000001,  // New Registry Key created
        REG_OPENED_EXISTING_KEY =0x00000002, // Existing Key opened
    }

    public class RegistryHandler
    {
        ListView listView_Info = null;
        Thread messageThread = null;
        Queue<RegistryEventArgs> messageQueue = new Queue<RegistryEventArgs>();

        AutoResetEvent autoEvent = new AutoResetEvent(false);
        bool disposed = false;


        public void InitListView()
        {
            listView_Info.Clear();		//clear control
            //create column header for ListView
            listView_Info.Columns.Add("#", 40, System.Windows.Forms.HorizontalAlignment.Left);
            listView_Info.Columns.Add("Time", 100, System.Windows.Forms.HorizontalAlignment.Left);
            listView_Info.Columns.Add("UserName", 150, System.Windows.Forms.HorizontalAlignment.Left);
            listView_Info.Columns.Add("ProcessName(PID)", 100, System.Windows.Forms.HorizontalAlignment.Left);
            listView_Info.Columns.Add("ThreadId", 60, System.Windows.Forms.HorizontalAlignment.Left);
            listView_Info.Columns.Add("RegCallbackClassName", 160, System.Windows.Forms.HorizontalAlignment.Left);
            listView_Info.Columns.Add("KeyName", 300, System.Windows.Forms.HorizontalAlignment.Left);
            listView_Info.Columns.Add("Return Status", 100, System.Windows.Forms.HorizontalAlignment.Left);
            listView_Info.Columns.Add("Description", 400, System.Windows.Forms.HorizontalAlignment.Left);
        }

        public RegistryHandler(ListView lvMessage)
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

        ~RegistryHandler()
        {
            Dispose(false);
        }

         public void DisplayEventMessage(RegistryEventArgs registryEventArgs)
        {
            if (GlobalConfig.OutputMessageToConsole)
            {
                lock (messageQueue)
                {
                    if (messageQueue.Count > GlobalConfig.MaximumFilterMessages)
                    {
                        messageQueue.Clear();
                    }

                    messageQueue.Enqueue(registryEventArgs);
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
                    RegistryEventArgs registryEventMessage;

                    lock (messageQueue)
                    {
                        registryEventMessage = (RegistryEventArgs)messageQueue.Dequeue();
                    }

                    ListViewItem lvItem = FormatRegistryMessage(registryEventMessage);
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

        public ListViewItem FormatRegistryMessage(RegistryEventArgs registryEventArgs)
        {
            ListViewItem lvItem = new ListViewItem();

            try
            {

                string[] listData = new string[listView_Info.Columns.Count];
                int col = 0;
                listData[col++] = registryEventArgs.MessageId.ToString();
                listData[col++] = FormatDateTime(registryEventArgs.TransactionTime);
                listData[col++] = registryEventArgs.UserName;
                listData[col++] = registryEventArgs.ProcessName + "  (" + registryEventArgs.ProcessId + ")";
                listData[col++] = registryEventArgs.ThreadId.ToString();
                listData[col++] = registryEventArgs.EventName;
                listData[col++] = registryEventArgs.FileName;
                listData[col++] = registryEventArgs.IOStatusToString();
                listData[col++] = registryEventArgs.Description;

                lvItem = new ListViewItem(listData, 0);

                if (registryEventArgs.IoStatus >= NtStatus.Status.Error)
                {
                    lvItem.BackColor = Color.LightGray;
                    lvItem.ForeColor = Color.Red;
                }
                else if (registryEventArgs.IoStatus > NtStatus.Status.Warning)
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
        /// Fires this event when the registry access was blocked by the setting,
        /// if the control flag "ENABLE_FILTER_SEND_DENIED_REG_EVENT" was enabled.
        /// </summary>
        public void NotifyRegWasBlocked(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }

        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void OnPreQueryValueKey(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //   //test block the registry event.
            //    e.ReturnStatus = NtStatus.Status.AccessDenied;

            KEY_VALUE_INFORMATION_CLASS keyValuseInformationClass = (KEY_VALUE_INFORMATION_CLASS)e.InfoClass;

            //test to replace the query value with your own data.
            if (e.FileName.IndexOf("EaseFilter") > 0)
            {

                //below code is to demo how to complete pre-callback registry call with our own data.
                switch (keyValuseInformationClass)
                {
                    case KEY_VALUE_INFORMATION_CLASS.KeyValueBasicInformation:
                        {
                            //public struct KEY_VALUE_BASIC_INFORMATION
                            // {
                            //     public uint TitleIndex;
                            //     public uint Type;
                            //     public uint NameLength;
                            //     public byte[] Name;
                            // }

                            uint titleIndex = 0;
                            uint type = (uint)VALUE_DATA_TYPE.REG_DWORD;
                            byte[] valueName = Encoding.Unicode.GetBytes("value1");
                            uint valueNameLength = (uint)valueName.Length;

                            MemoryStream ms = new MemoryStream();
                            BinaryWriter bw = new BinaryWriter(ms);
                            bw.Write(titleIndex);
                            bw.Write(type);
                            bw.Write(valueNameLength);
                            bw.Write(valueName);

                            e.ReturnDataBuffer = ms.ToArray();
                            e.IsDataModified = true;

                            break;
                        }
                    case KEY_VALUE_INFORMATION_CLASS.KeyValueFullInformation:
                        {
                            //KeyValueFullInformation class structure
                            //public uint TitleIndex;
                            //public uint Type;
                            //public uint DataOffset;
                            //public uint DataLength;
                            //public uint NameLength;
                            //public byte[] Name;

                            uint titleIndex = 0;
                            uint type = (uint)VALUE_DATA_TYPE.REG_DWORD;
                            uint testData = 12345;
                            uint testDataLength = sizeof(uint);
                            byte[] valueName = Encoding.Unicode.GetBytes("value1");
                            uint valueNameLength = (uint)valueName.Length;
                            uint dataOffset = 5 * sizeof(uint) + valueNameLength;

                            MemoryStream ms = new MemoryStream();
                            BinaryWriter bw = new BinaryWriter(ms);
                            bw.Write(titleIndex);
                            bw.Write(type);
                            bw.Write(dataOffset);
                            bw.Write(testDataLength);
                            bw.Write(valueNameLength);
                            bw.Write(valueName);
                            bw.Write(testData);

                            e.ReturnDataBuffer = ms.ToArray();
                            e.IsDataModified = true;

                            break;
                        }
                    case KEY_VALUE_INFORMATION_CLASS.KeyValuePartialInformation:
                        {
                            // public struct KEY_VALUE_PARTIAL_INFORMATION
                            //{
                            //    public uint TitleIndex;
                            //    public uint Type;
                            //    public uint DataLength;
                            //    public byte[] Data;
                            //}

                            uint titleIndex = 0;
                            uint type = (uint)VALUE_DATA_TYPE.REG_DWORD;
                            uint testData = 12345;
                            uint testDataLength = sizeof(uint);

                            MemoryStream ms = new MemoryStream();
                            BinaryWriter bw = new BinaryWriter(ms);

                            bw.Write(titleIndex);
                            bw.Write(type);
                            bw.Write(testDataLength);
                            bw.Write(testData);

                            e.ReturnDataBuffer = ms.ToArray();
                            e.IsDataModified = true;

                            break;
                        }


                    default: break;
                }
            }

        }
        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void  OnPreDeleteKey(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //   //test block the registry event.
            //    e.ReturnStatus = NtStatus.Status.AccessDenied;
        }

        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void  OnPreSetValueKey(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //   //test block the registry event.
            //    e.ReturnStatus = NtStatus.Status.AccessDenied;
        }
        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void  OnPreDeleteValueKey(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //   //test block the registry event.
            //    e.ReturnStatus = NtStatus.Status.AccessDenied;
        }
        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void  OnPreSetInformationKey(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //   //test block the registry event.
            //    e.ReturnStatus = NtStatus.Status.AccessDenied;
        }
        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void  OnPreRenameKey(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //   //test block the registry event.
            //    e.ReturnStatus = NtStatus.Status.AccessDenied;
        }        
        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void  OnPreEnumerateKey(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //   //test block the registry event.
            //    e.ReturnStatus = NtStatus.Status.AccessDenied;
        }
        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void  OnPreEnumerateValueKey(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //   //test block the registry event.
            //    e.ReturnStatus = NtStatus.Status.AccessDenied;
        }
        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void  OnPreQueryKey(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //   //test block the registry event.
            //    e.ReturnStatus = NtStatus.Status.AccessDenied;
        }       
        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void  OnPreQueryMultipleValueKey(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //   //test block the registry event.
            //    e.ReturnStatus = NtStatus.Status.AccessDenied;
        }
        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void  OnPreCreateKey(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //   //test block the registry event.
            //    e.ReturnStatus = NtStatus.Status.AccessDenied;
        }
        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void  OnPreOpenKey(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //   //test block the registry event.
            //    e.ReturnStatus = NtStatus.Status.AccessDenied;
        }
        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void  OnPreKeyHandleClose(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //   //test block the registry event.
            //    e.ReturnStatus = NtStatus.Status.AccessDenied;
        }
        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void  OnPreCreateKeyEx(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //   //test block the registry event.
            //    e.ReturnStatus = NtStatus.Status.AccessDenied;
        }
        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void  OnPreOpenKeyEx(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //   //test block the registry event.
            //    e.ReturnStatus = NtStatus.Status.AccessDenied;
        }
        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void  OnPreFlushKey(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //   //test block the registry event.
            //    e.ReturnStatus = NtStatus.Status.AccessDenied;
        }
        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void  OnPreLoadKey(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //   //test block the registry event.
            //    e.ReturnStatus = NtStatus.Status.AccessDenied;
        }
        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void  OnPreUnLoadKey(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //   //test block the registry event.
            //    e.ReturnStatus = NtStatus.Status.AccessDenied;
        }
        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void  OnPreQueryKeySecurity(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //   //test block the registry event.
            //    e.ReturnStatus = NtStatus.Status.AccessDenied;
        }
        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void  OnPreSetKeySecurity(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //   //test block the registry event.
            //    e.ReturnStatus = NtStatus.Status.AccessDenied;
        }
        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void  OnPreRestoreKey(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //   //test block the registry event.
            //    e.ReturnStatus = NtStatus.Status.AccessDenied;
        }
        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void  OnPreSaveKey(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //   //test block the registry event.
            //    e.ReturnStatus = NtStatus.Status.AccessDenied;
        }
        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void  OnPreReplaceKey(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //   //test block the registry event.
            //    e.ReturnStatus = NtStatus.Status.AccessDenied;
        }
        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void  OnPreQueryKeyName(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //   //test block the registry event.
            //    e.ReturnStatus = NtStatus.Status.AccessDenied;
        }
        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void  NotifyDeleteKey(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }
        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void  NotifySetValueKey(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }
        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void  NotifyDeleteValueKey(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }
        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void  NotifySetInformationKey(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }
        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void  NotifyRenameKey(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

        }
        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void  NotifyEnumerateKey(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

        }
        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void  NotifyEnumerateValueKey(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

        }
        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void  NotifyQueryKey(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

        }
        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void  NotifyQueryValueKey(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

        }
        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void  NotifyQueryMultipleValueKey(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

        }
        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void  NotifyCreateKey(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

        }
        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void  NotifyOpenKey(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

        }
        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void  NotifyKeyHandleClose(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

        }
        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void  NotifyCreateKeyEx(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

        }
        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void  NotifyOpenKeyEx(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

        }
        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void  NotifyFlushKey(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

        }
        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void  NotifyLoadKey(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

        }
        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void  NotifyUnLoadKey(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

        }
        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void  NotifyQueryKeySecurity(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

        }
        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void  NotifySetKeySecurity(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

        }
        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void  NotifyRestoreKey(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

        }
        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void  NotifySaveKey(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

        }
        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void  NotifyReplaceKey(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

        }
        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void  NotifyQueryKeyName(object sender, RegistryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

        }

    }
}
