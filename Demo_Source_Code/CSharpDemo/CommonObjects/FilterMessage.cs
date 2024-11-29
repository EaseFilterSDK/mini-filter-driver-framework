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

namespace EaseFilter.CommonObjects
{
    public class ByteArrayComparer : IEqualityComparer<byte[]>
    {
        public bool Equals(byte[] left, byte[] right)
        {
            if (left == null || right == null)
            {
                return left == right;
            }
            if (left.Length != right.Length)
            {
                return false;
            }
            for (int i = 0; i < left.Length; i++)
            {
                if (left[i] != right[i])
                {
                    return false;
                }
            }
            return true;
        }

        public int GetHashCode(byte[] key)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            int sum = 0;
            foreach (byte cur in key)
            {
                sum += cur;
            }
            return sum;
        }
    }

    public class FilterMessage : IDisposable
    {
  
        ListView listView_Message = null;
        Thread messageThread = null;
        Queue<FilterAPI.MessageSendData> messageQueue = new Queue<FilterAPI.MessageSendData>();

        AutoResetEvent autoEvent = new AutoResetEvent(false);
        bool disposed = false;


        public FilterMessage(ListView lvMessage)
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

        ~FilterMessage()
        {
            Dispose(false);
        }

        public void InitListView()
        {
            messageQueue.Clear();
            //init ListView control
            listView_Message.Clear();		//clear control
            //create column header for ListView
            listView_Message.Columns.Add("#", 40, System.Windows.Forms.HorizontalAlignment.Left);
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

        public void AddMessage(FilterAPI.MessageSendData messageSend)
        {
            lock (messageQueue)
            {
                if (messageQueue.Count > GlobalConfig.MaximumFilterMessages)
                {
                    messageQueue.Clear();
                }


                if (messageSend.MessageType == (uint)FilterAPI.FilterCommand.FILTER_SEND_PROCESS_CREATION_INFO
             || messageSend.MessageType == (uint)FilterAPI.FilterCommand.FILTER_SEND_PROCESS_TERMINATION_INFO
            || messageSend.MessageType == (uint)FilterAPI.FilterCommand.FILTER_SEND_THREAD_CREATION_INFO
            || messageSend.MessageType == (uint)FilterAPI.FilterCommand.FILTER_SEND_THREAD_TERMINATION_INFO)
                {
                    //this is the process filter command, we don't display the message.
                    return;
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

                while (messageQueue.Count > 0 )
                {
                    FilterAPI.MessageSendData messageSend;

                    lock (messageQueue)
                    {
                        messageSend = (FilterAPI.MessageSendData)messageQueue.Dequeue();
                    }

                    if (GlobalConfig.DisableDirIO && (messageSend.FileAttributes & (uint)FileAttributes.Directory) == (uint)FileAttributes.Directory)
                    {
                        //don't display the directory IO 
                        continue;
                    }

                    string[] filterMessages = new string[0];
                    ListViewItem lvItem = GetFilterMessage(messageSend, ref filterMessages);

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

                foreach(string itemStr in logMessage)
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
                                statusStr = statusStr.Substring(index1 + 3, index2 - index1 -3 );
                                uint.TryParse(statusStr,  System.Globalization.NumberStyles.HexNumber,null, out status);

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


        public static string FormatDescription(FilterAPI.MessageSendData messageSend)
        {

            string message = string.Empty;
            try
            {
               if (messageSend.Status == (uint)NtStatus.Status.Success)
                {

                    if (messageSend.MessageType == (uint)FilterAPI.FilterCommand.FILTER_SEND_FILE_CHANGED_EVENT
                        && messageSend.InfoClass == (uint)FilterAPI.EVENTTYPE.RENAMED)
                    {
                        string newFileName = string.Empty;

                        if (messageSend.DataBufferLength > 0)
                        {
                            byte[] buffer = new byte[messageSend.DataBufferLength];
                            Array.Copy(messageSend.DataBuffer, buffer, buffer.Length);
                            newFileName = Encoding.Unicode.GetString(buffer);
                            if (newFileName.IndexOf((char)0) > 0)
                            {
                                newFileName = newFileName.Remove(newFileName.IndexOf((char)0));
                            }
                        }

                        message = "File " + messageSend.FileName + " was renamed to " + newFileName;
                        return message;
                    }
                    else if(    messageSend.MessageType == (uint)FilterAPI.FilterCommand.FILTER_SEND_ATTACHED_VOLUME_INFO
                             ||  messageSend.MessageType ==(uint)FilterAPI.FilterCommand.FILTER_SEND_DETACHED_VOLUME_INFO)
                        {
                            FilterAPI.VOLUME_INFO volumeInfo = new FilterAPI.VOLUME_INFO();
                            GCHandle pinnedPacket = GCHandle.Alloc(messageSend.DataBuffer, GCHandleType.Pinned);
                            volumeInfo = (FilterAPI.VOLUME_INFO)Marshal.PtrToStructure(
                                pinnedPacket.AddrOfPinnedObject(), typeof(FilterAPI.VOLUME_INFO));

                            message += "VolumeName[" + volumeInfo.VolumeName + "]  ";
                            message += "VolumeDosName[" + volumeInfo.VolumeDosName + "]  ";
                            message += "VolumeFilesystemType[" + ((WinData.FLT_FILESYSTEM_TYPE)volumeInfo.VolumeFilesystemType).ToString() + "]  ";

                            uint deviceCharacteristics = volumeInfo.DeviceCharacteristics;
                            string charaterMessage = string.Empty;
                            foreach (WinData.DeviceObject_Characteristics character in Enum.GetValues(typeof(WinData.DeviceObject_Characteristics)))
                            {
                                if ((deviceCharacteristics & (uint)character) > 0)
                                {
                                    charaterMessage += character.ToString() + ";";
                                }
                            }

                            message += "DeviceCharacteristics[" + charaterMessage + "]";

                            pinnedPacket.Free();
  
                            return message;
                        }
                }
                else
                {
                    if (messageSend.MessageType == (uint)FilterAPI.FilterCommand.FILTER_REQUEST_ENCRYPTION_IV_AND_KEY_AND_ACCESSFLAG
                          || messageSend.MessageType == (uint)FilterAPI.FilterCommand.FILTER_REQUEST_ENCRYPTION_IV_AND_KEY)
                    {
                        //this is the error message to read the shared secure file
                        byte[] errorBuffer = new byte[messageSend.DataBufferLength];
                        Array.Copy(messageSend.DataBuffer, errorBuffer, errorBuffer.Length);
                        message = UnicodeEncoding.Unicode.GetString(errorBuffer);
                        return message;

                    }

                    return message;
                }

                switch ((FilterAPI.MessageType)messageSend.MessageType)
                {                   
                    case FilterAPI.MessageType.POST_CREATE:
                        {
                            if ((messageSend.CreateOptions & (uint)WinData.CreateOptions.FILE_DELETE_ON_CLOSE) > 0)
                            {
                                message = "file was marked as deleted.";
                            }

                            if (messageSend.Status == (uint)NtStatus.Status.Success)
                            {
                                //the create status is meaningful only when the status is succeeded.
                                message += "CreateStatus:" + ((WinData.CreateStatus)messageSend.CreateStatus).ToString();
                            }

                            message += " DesiredAccess:" + FormatDesiredAccess(messageSend.DesiredAccess);
                            message += " Disposition:" + ((WinData.Disposition)messageSend.Disposition).ToString();
                            message += " ShareAccess:" + ((WinData.ShareAccess)messageSend.SharedAccess).ToString();
                            message += " CreateOptions:" + FormatCreateOptions(messageSend.CreateOptions);

                            break;

                        }

                    case FilterAPI.MessageType.POST_CACHE_READ:
                    case FilterAPI.MessageType.POST_FASTIO_READ:
                    case FilterAPI.MessageType.POST_NOCACHE_READ:
                    case FilterAPI.MessageType.POST_PAGING_IO_READ:
                        {
                            message = "read offset:" + messageSend.Offset + " read length:" + messageSend.Length + ",returnLength:" + messageSend.DataBufferLength;

                            if ((GlobalConfig.BooleanConfig & (uint)FilterAPI.BooleanConfig.ENABLE_SEND_DATA_BUFFER) > 0)
                            {
                                message += ",return data:";
                                //read data is here
                                message += Encoding.ASCII.GetString(messageSend.DataBuffer);
                            }

                            break;
                        }

                    case FilterAPI.MessageType.POST_CACHE_WRITE:
                    case FilterAPI.MessageType.POST_FASTIO_WRITE:
                    case FilterAPI.MessageType.POST_NOCACHE_WRITE:
                    case FilterAPI.MessageType.POST_PAGING_IO_WRITE:
                        {
                            message = "write offset:" + messageSend.Offset + " length:" + messageSend.Length + ",writtenLength:" + messageSend.DataBufferLength;
                           
                            if ((GlobalConfig.BooleanConfig & (uint)FilterAPI.BooleanConfig.ENABLE_SEND_DATA_BUFFER) > 0)
                            {
                                message += ",writing data:";
                                //written data is here
                                message += Encoding.ASCII.GetString(messageSend.DataBuffer);
                            }

                            break;
                        }
                    case FilterAPI.MessageType.POST_SET_INFORMATION:
                        {
                            message = FormatInformationDataBuffer((WinData.FileInfomationClass)messageSend.InfoClass, messageSend.DataBufferLength, messageSend.DataBuffer);

                            break;
                        }
                    case FilterAPI.MessageType.POST_QUERY_INFORMATION:
                        {
                            message = FormatInformationDataBuffer((WinData.FileInfomationClass)messageSend.InfoClass, messageSend.DataBufferLength, messageSend.DataBuffer);

                            break;
                        }
                    case FilterAPI.MessageType.POST_SET_SECURITY:
                        {
                            message = "set security with information class:" + FormatSecurityInfoClass(messageSend.InfoClass);
                            break;
                        }
                    case FilterAPI.MessageType.POST_QUERY_SECURITY:
                        {
                            message = "query security with information class:" + FormatSecurityInfoClass(messageSend.InfoClass);
                            break;
                        }
                    case FilterAPI.MessageType.POST_DIRECTORY:
                        {
                            message = "browse directory with information class:" + ((WinData.FileInfomationClass)messageSend.InfoClass).ToString();
                            break;
                        }
                    case FilterAPI.MessageType.POST_CLEANUP:
                        {
                            message = "all file handles to fileObject " + messageSend.FileObject.ToString("X") + " were closed.";
                            break;
                        }
                    case FilterAPI.MessageType.POST_CLOSE:
                        {
                            message = "all system references to fileObject " + messageSend.FileObject.ToString("X") + " were closed.";
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                EventManager.WriteMessage(318, "FormatDescription", EventLevel.Error, "Format description failed with error " + ex.Message);
            }


            return message;
        }


        public static string FormatIOName(FilterAPI.MessageSendData messageSend)
        {
            string ioName = string.Empty;

            try
            {
                if (messageSend.MessageType == (uint)FilterAPI.FilterCommand.FILTER_SEND_FILE_CHANGED_EVENT)
                {
                    foreach (FilterAPI.EVENTTYPE eventType in Enum.GetValues(typeof(FilterAPI.EVENTTYPE)))
                    {
                        if (eventType != FilterAPI.EVENTTYPE.NONE && ((messageSend.InfoClass & (uint)eventType) > 0))
                        {
                            if (ioName.Length > 0)
                            {
                                ioName = ioName + " ," + eventType.ToString();
                            }
                            else
                            {
                                ioName = eventType.ToString();
                            }
                        }
                    }

                }
                else if (       messageSend.MessageType >= (uint)FilterAPI.FilterCommand.FILTER_REQUEST_ENCRYPTION_IV_AND_KEY_AND_TAGDATA
                          &&    messageSend.MessageType <= (uint)FilterAPI.FilterCommand.FILTER_SEND_DETACHED_VOLUME_INFO )
                {
                    ioName = ((FilterAPI.FilterCommand)messageSend.MessageType).ToString();

                }
                else if (messageSend.MessageType == (uint)FilterAPI.MessageType.PRE_QUERY_INFORMATION)
                {
                    if (messageSend.FsContext.ToInt64() == 0)
                    {
                        ioName = "PRE_FASTIO_NETWORK_QUERY_OPEN";
                    }
                    else
                    {
                        ioName = "PRE_QUERY_INFORMATION";
                    }
                }
                else if (messageSend.MessageType == (uint)FilterAPI.MessageType.POST_QUERY_INFORMATION)
                {
                    if (messageSend.FsContext.ToInt64() == 0)
                    {
                        ioName = "POST_FASTIO_NETWORK_QUERY_OPEN";
                    }
                    else
                    {
                        ioName = "POST_QUERY_INFORMATION";
                    }
                }
                else
                {
                    ioName = ((FilterAPI.MessageType)messageSend.MessageType).ToString();
                }
            }
            catch (Exception ex)
            {
                EventManager.WriteMessage(592, "FormatIOName", EventLevel.Error, "Get IO name from " + messageSend.MessageType.ToString("X") + " exception:" + ex.Message);
            }

            if (ioName.Trim().Length == 0)
            {
                ioName = "UnKnown IO Type (0x" + messageSend.MessageType.ToString("X") + ")";
            }

            return ioName;
        }

        ListViewItem GetFilterMessage(FilterAPI.MessageSendData messageSend, ref string[] filterMessages)
        {
            ListViewItem lvItem = new ListViewItem();

            try
            {

                string userName = string.Empty;
                string processName = string.Empty;

                FilterAPI.DecodeUserName(messageSend.Sid, out userName);
                FilterAPI.DecodeProcessName(messageSend.ProcessId, out processName);             

                if ((messageSend.CreateOptions & (uint)WinData.CreateOptions.FO_REMOTE_ORIGIN) > 0)
                {
                    //this is the request comes from remote server
                    string remoteIp = string.Empty;
                    if (messageSend.DataBufferLength > 0 && ((uint)FilterAPI.MessageType.POST_CREATE == messageSend.MessageType) )
                    {
                        byte[] buffer = new byte[messageSend.DataBufferLength];
                        Array.Copy(messageSend.DataBuffer, buffer, buffer.Length);
                        remoteIp = Encoding.Unicode.GetString(buffer);
                        userName += ",RemoteIP:" + remoteIp;
                    }
                    else
                    {
                        userName += ",Accessed from remote computer.";
                    }
                }

                string[] listData = new string[listView_Message.Columns.Count];
                int col = 0;
                listData[col++] = messageSend.MessageId.ToString();
                listData[col++] = FormatDateTime(messageSend.TransactionTime);
                listData[col++] = userName;
                listData[col++] = processName + "  (" + messageSend.ProcessId + ")";
                listData[col++] = messageSend.ThreadId.ToString();
                listData[col++] = FormatIOName(messageSend);
                listData[col++] = messageSend.FileObject.ToString("X");
                listData[col++] = messageSend.FileName;
                listData[col++] = messageSend.FileSize.ToString();
                listData[col++] = ((FileAttributes)messageSend.FileAttributes).ToString();
                listData[col++] = FormatDateTime(messageSend.LastWriteTime);
                listData[col++] = FormatStatus(messageSend.Status);
                listData[col++] = FormatDescription(messageSend);

                filterMessages = listData;

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


            }
            catch (Exception ex)
            {
                EventManager.WriteMessage(445, "GetFilterMessage", EventLevel.Error, "Add callback message failed." + ex.Message);
                lvItem = null;
            }

            return lvItem;
        }


        public static string FormatDesiredAccess(uint desiredAccess)
        {
            string ret = string.Empty;

            foreach (WinData.DisiredAccess access in Enum.GetValues(typeof(WinData.DisiredAccess)))
            {
                if (access == (WinData.DisiredAccess)((uint)access & desiredAccess))
                {
                    ret += access.ToString() + "; ";
                }
            }

            return ret;
        }

        public static string FormatCreateOptions(uint createOptions)
        {
            string ret = string.Empty;

            foreach (WinData.CreateOptions option in Enum.GetValues(typeof(WinData.CreateOptions)))
            {
                if (option == (WinData.CreateOptions)((uint)option & createOptions))
                {
                    ret += option.ToString() + "; ";
                }
            }

            if (string.IsNullOrEmpty(ret))
            {
                ret = "(0x)" + createOptions.ToString("X");
            }

            return ret;
        }

       public static string FormatDateTime(long lDateTime)
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

       static  string FormatInformationDataBuffer(WinData.FileInfomationClass infoClass, uint dataLength, byte[] data)
        {
            string retValue = string.Empty;

            try
            {
                switch (infoClass)
                {
                    case WinData.FileInfomationClass.FileRenameInformation:
                        {
                            //destination name
                            string newFileName = Encoding.Unicode.GetString(data);
                            if (newFileName.IndexOf((char)0) > 0)
                            {
                                newFileName = newFileName.Remove(newFileName.IndexOf((char)0));
                            }

                            retValue = "file was renamed to " + newFileName;
                            break;
                        }
                    case WinData.FileInfomationClass.FileDispositionInformation:
                        {
                            //the deletion only complete in the post_cleanup I/O
                            retValue = "file was marked as deleted.";
                            break;
                        }
                    case WinData.FileInfomationClass.FileEndOfFileInformation:
                        {
                            long newFileSize = BitConverter.ToInt64(data, 0);
                            retValue = "file size was changed to:" + newFileSize.ToString();
                            break;
                        }
                    case WinData.FileInfomationClass.FileBasicInformation:
                        {
                            WinData.FileBasicInformation basiInfo = new WinData.FileBasicInformation();
                            GCHandle pinnedPacket = GCHandle.Alloc(data, GCHandleType.Pinned);
                            basiInfo = (WinData.FileBasicInformation)Marshal.PtrToStructure(
                                pinnedPacket.AddrOfPinnedObject(), typeof(WinData.FileBasicInformation));
                            pinnedPacket.Free();

                            retValue = "FileBasicInformation result,creation time:" + FormatDateTime(basiInfo.CreationTime) + "  ";
                            retValue += "last access time:" + FormatDateTime(basiInfo.LastAccessTime) + "   ";
                            retValue += "last write time:" + FormatDateTime(basiInfo.LastWriteTime) + "   ";
                            retValue += "file attributes:" + ((FileAttributes)basiInfo.FileAttributes).ToString();
                            break;
                        }

                    case WinData.FileInfomationClass.FileStandardInformation:
                        {
                            WinData.FileStandardInformation standardInfo = new WinData.FileStandardInformation();
                            GCHandle pinnedPacket = GCHandle.Alloc(data, GCHandleType.Pinned);
                            standardInfo = (WinData.FileStandardInformation)Marshal.PtrToStructure(
                                pinnedPacket.AddrOfPinnedObject(), typeof(WinData.FileStandardInformation));
                            pinnedPacket.Free();

                            retValue = "FileStandardInformation result, file size:" + standardInfo.EndOfFile.ToString() + "   ";
                            retValue += "allocation size:" + standardInfo.AllocationSize.ToString() + "   ";
                            retValue += "isDirectory:" + standardInfo.Directory.ToString();
                            break;
                        }
                    case WinData.FileInfomationClass.FileNetworkOpenInformation:
                        {
                            WinData.FileNetworkInformation networkInfo = new WinData.FileNetworkInformation();
                            GCHandle pinnedPacket = GCHandle.Alloc(data, GCHandleType.Pinned);
                            networkInfo = (WinData.FileNetworkInformation)Marshal.PtrToStructure(
                                pinnedPacket.AddrOfPinnedObject(), typeof(WinData.FileNetworkInformation));
                            pinnedPacket.Free();

                            retValue = "FileNetworkOpenInformation result,creation time:" + FormatDateTime(networkInfo.CreationTime) + "   ";
                            retValue += "last access time:" + FormatDateTime(networkInfo.LastAccessTime) + "   ";
                            retValue += "last write time:" + FormatDateTime(networkInfo.LastWriteTime) + "   ";
                            retValue += "file size:" + networkInfo.FileSize.ToString() + "   ";
                            retValue += "file attributes:" + ((FileAttributes)networkInfo.FileAttributes).ToString();
                            break;
                        }

                    case WinData.FileInfomationClass.FileInternalInformation:
                        {
                            long fileId = BitConverter.ToInt64(data, 0);
                            retValue = "FileInternalInformation result, fileId: (0x)" + fileId.ToString("X");
                            break;
                        }

                    default:
                        {
                            retValue = infoClass.ToString() + "(" + ((uint)infoClass).ToString("X") + ")";
                            break;
                        }
                }

            }
            catch (Exception ex)
            {
                EventManager.WriteMessage(594, "FormatInformationDataBuffer", EventLevel.Error, "Format data failed." + ex.Message);
                retValue = ex.Message;
            }

            return retValue;
        }

        public static string FormatStatus(uint status)
        {
            string ret = string.Empty;

            foreach (NtStatus.Status ntStatus in Enum.GetValues(typeof(NtStatus.Status)))
            {
                if (status == (uint)ntStatus)
                {
                    ret = ntStatus.ToString() + "(0x" + status.ToString("X") + ")";
                }
            }

            if (string.IsNullOrEmpty(ret))
            {
                ret = "(0x" + status.ToString("X") + ")";
            }

            return ret;
        }

       public static string FormatSecurityInfoClass(uint infoClass)
        {
            string ret = string.Empty;

            foreach (WinData.SecurityInformation security in Enum.GetValues(typeof(WinData.SecurityInformation)))
            {
                if ((infoClass & (int)security) > 0)
                {
                    if (ret.Length > 0)
                    {
                        ret += ";";
                    }

                    ret += security.ToString();
                }
            }

            return ret;
        }


    }
}