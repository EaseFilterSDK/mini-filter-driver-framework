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
using System.Runtime.InteropServices;
using System.Text;

namespace EaseFilter.FilterControl
{

    /// <summary>
    /// Fires the monitor events after the file IO was returned from the file system.
    /// </summary>
    public enum MonitorFileIOEvents : ulong
    {
        /// <summary>
        /// Fires this event after an open file IO to an existing file was returned.
        /// </summary>
        OnFileOpen = 0x00000002,
        /// <summary>
        /// Fires this event after a create file IO was returned and a new file was created.
        /// </summary>
        OnFileCreate = 0x0000000200000000,
        /// <summary>
        /// Fires this event after a read file IO was returned.
        /// it includes cache-read,non-cache read, doesn't include paging read
        /// </summary>
        OnFileRead = 0x000002a8,
        /// <summary>
        /// Fires this event after a write file IO was returned.
        /// </summary>
        OnFileWrite = 0x0000a800,
        /// <summary>
        /// Fires this event after a query file size IO was returned.
        /// </summary>
        OnQueryFileSize = 0x0000000800000000,
        /// <summary>
        /// Fires this event after a query file basic information IO was returned.
        /// </summary>
        OnQueryFileBasicInfo = 0x0000002000000000,
        /// <summary>
        /// Fires this event after a query file standard information IO was returned.
        /// </summary>
        OnQueryFileStandardInfo = 0x0000008000000000,
        /// <summary>
        /// Fires this event after a query file network information IO was returned.
        /// </summary>
        OnQueryFileNetworkInfo = 0x0000020000000000,
        /// <summary>
        /// Fires this event after a query file Id IO was returned.
        /// </summary>
        OnQueryFileId = 0x0000080000000000,
        /// <summary>
        /// Fires this event after a query file info IO was returned 
        /// if the query file information class is not 'QueryFileSize','QueryFileBasicInfo'
        /// ,'QueryFileStandardInfo','QueryFileNetworkInfo' or 'QueryFileId'.
        /// </summary>
        OnQueryFileInfo = 0x00080000,
        /// <summary>
        /// Fires this event after a set file size IO was returned.
        /// </summary>
        OnSetFileSize = 0x0000800000000000,
        /// <summary>
        /// Fires this event after set file basic info IO was returned.
        /// </summary>
        OnSetFileBasicInfo = 0x0002000000000000,
        /// <summary>
        /// Fires this event after set file standard info IO was returned.
        /// </summary>
        OnSetFileStandardInfo = 0x0008000000000000,
        /// <summary>
        /// Fires this event after set file network info IO was returned
        /// </summary>
        OnSetFileNetworkInfo = 0x0002000000000000,
        /// <summary>
        /// Fires this event after the move or rename file IO was returned.
        /// </summary>
        OnMoveOrRenameFile = 0x0080000000000000,
        /// <summary>
        /// Fires this event after the delete file IO was returned.
        /// </summary>
        OnDeleteFile = 0x0200000000000000,
        /// <summary>
        /// Fires this event after the set file info IO was returned if 
        /// the information class is not 'SetFileSize','SetFileBasicInfo'
        /// ,'SetFileStandardInfo','SetFileNetworkInfo'.
        /// </summary>
        OnSetFileInfo = 0x00200000,
        /// <summary>
        /// Fires this event after the query directory file info IO was returned.
        /// </summary>
        OnQueryDirectoryFile = 0x00800000,
        /// <summary>
        /// Fires this event after the query file security IO was returned.
        /// </summary>
        OnQueryFileSecurity = 0x02000000,
        /// <summary>
        /// Fires this event after the set file security IO was returned.
        /// </summary>
        OnSetFileSecurity = 0x08000000,
        /// <summary>
        /// Fires this event after file handle close IO was returned.
        /// </summary>
        OnFileHandleClose = 0x20000000,
        /// <summary>
        /// Fires this event after the file close IO was returned.
        /// </summary>
        OnFileClose = 0x80000000,
    }

    partial class FileFilter
    {
        /// <summary>
        /// register the file events, trigger the notifcation when the events were happened after the file was closed 
        /// </summary>
        FilterAPI.MonitorFileEvents registerFileEvents = 0;

        /// <summary>
        /// register the monitor notification of the IOs, this is 16bytes integer,
        /// we need to convert it to 8 bytes registerCallbackMonitorIO, since all the file info query and set IOs are 
        /// using the same messageType.
        /// </summary>
        ulong registerMonitorFileIOEvents = 0;
        /// <summary>
        /// the maximum buffer size for monitor events if it was enabled.
        /// </summary>
        int maxEventBufferSize = 1024 * 1024 * 50;//50MB

        /// <summary>
        ///Fires this event after the file was opened, the handle is not closed. 
        /// </summary>
        public event EventHandler<FileCreateEventArgs> OnFileOpen;
        /// <summary>
        /// Fires this event after the new file was created, the handle is not closed.
        /// </summary>
        public event EventHandler<FileCreateEventArgs> OnNewFileCreate;
        /// <summary>
        /// Fires this event after the read IO was returned.
        /// </summary>
        public event EventHandler<FileReadEventArgs> OnFileRead;
        /// <summary>
        /// Fires this event after the write IO was returned.
        /// </summary>
        public event EventHandler<FileWriteEventArgs> OnFileWrite;
        /// <summary>
        /// Fires this event after the query file size IO was returned.
        /// </summary>
        public event EventHandler<FileSizeEventArgs> OnQueryFileSize;
        /// <summary>
        /// Fires this event after the query file basic information IO was returned.
        /// </summary>
        public event EventHandler<FileBasicInfoEventArgs> OnQueryFileBasicInfo;
        /// <summary>
        /// Fires this event after the query file standard information IO was returned.
        /// </summary>
        public event EventHandler<FileStandardInfoEventArgs> OnQueryFileStandardInfo;
        /// <summary>
        /// Fires this event after the query file network information IO was returned.
        /// </summary>
        public event EventHandler<FileNetworkInfoEventArgs> OnQueryFileNetworkInfo;
        /// <summary>
        /// Fires this event after the query file Id IO was returned.
        /// </summary>
        public event EventHandler<FileIdEventArgs> OnQueryFileId;
        /// <summary>
        /// Fires this event after the query file info was returned.
        /// </summary>
        public event EventHandler<FileInfoArgs> OnQueryFileInfo;
        /// <summary>
        /// Fires this event after the set file size IO was returned.
        /// </summary>
        public event EventHandler<FileSizeEventArgs> OnSetFileSize;
        /// <summary>
        /// Fires this event after the set file basic information was returned.
        /// </summary>
        public event EventHandler<FileBasicInfoEventArgs> OnSetFileBasicInfo;
        /// <summary>
        /// Fires this event after the set file standard information IO was returned.
        /// </summary>
        public event EventHandler<FileStandardInfoEventArgs> OnSetFileStandardInfo;
        /// <summary>
        /// Fires this event after the set file network information IO was returned.
        /// </summary>
        public event EventHandler<FileNetworkInfoEventArgs> OnSetFileNetworkInfo;
        /// <summary>
        /// Fires this event after the file was moved or renamed.
        /// </summary>
        public event EventHandler<FileMoveOrRenameEventArgs> OnMoveOrRenameFile;
        /// <summary>
        /// Fires this event after the delete IO was returned.
        /// </summary>
        public event EventHandler<FileIOEventArgs> OnDeleteFile;
        /// <summary>
        /// Fires this event after the set file info was returned.
        /// </summary>
        public event EventHandler<FileInfoArgs> OnSetFileInfo;
        /// <summary>
        /// Fires this event after the query file security IO was returned.
        /// </summary>
        public event EventHandler<FileSecurityEventArgs> OnQueryFileSecurity;
        /// <summary>
        /// Fires this event after the set file security IO was returned.
        /// </summary>
        public event EventHandler<FileSecurityEventArgs> OnSetFileSecurity;
        /// <summary>
        /// Fires this event after the directory enumeration query IO was returned.
        /// </summary>
        public event EventHandler<FileQueryDirectoryEventArgs> OnQueryDirectoryFile;
        /// <summary>
        /// Fires this event after the file was cleanuped.
        /// </summary>
        public event EventHandler<FileIOEventArgs> OnFileHandleClose;
        /// <summary>
        /// Fires this event after the last file handle of the file was closed.
        /// </summary>
        public event EventHandler<FileIOEventArgs> OnFileClose;
      
        /// <summary>
        /// Fires this event when the file was changed after the file handle closed
        /// </summary>
        public event EventHandler<FileChangeEventArgs> NotifyFileWasChanged;

        #region monitor filter property

        /// <summary>
        ///  Register the file events, trigger the notifcation when the events were happened after the file was closed. 
        ///  it is only enabled for monitor filter driver
        /// </summary>
        public FilterAPI.MonitorFileEvents FileChangeEventFilter
        {
            get { return registerFileEvents; }
            set { registerFileEvents = value; }
        }

        /// <summary>
        /// If it is true, the filter driver will send the monitor events asynchronously, the monitor event buffer will be used. 
        /// if it is false, the filter driver will send the monitor events synchronously, block and wait till the events being picked up.
        /// </summary>
        public bool EnableMonitorEventBuffer
        {
            get { return (booleanConfig & (uint)FilterAPI.BooleanConfig.ENABLE_MONITOR_EVENT_BUFFER) > 0; }
            set
            {
                if (value)
                {
                    booleanConfig |= (uint)FilterAPI.BooleanConfig.ENABLE_MONITOR_EVENT_BUFFER;
                }
                else
                {
                    booleanConfig &= ~(uint)FilterAPI.BooleanConfig.ENABLE_MONITOR_EVENT_BUFFER;
                }
            }

        }

        /// <summary>
        /// The maximum buffer size to keep the monitor event messages.
        /// </summary>
        public int MaxMonitorEventBufferSize
        {
            get { return maxEventBufferSize; }
            set { maxEventBufferSize = value; }
        }

        /// <summary>
        /// The monitor file IO events which will be registered to the filter driver.
        /// </summary>
        public ulong MonitorFileIOEventFilter
        {
            get { return registerMonitorFileIOEvents; }
            set { registerMonitorFileIOEvents = value; }
        }

      
        /// <summary>
        /// Below is the properties to check if the monitor file IO events were registered, if yes, the associated events will be fired.
        /// </summary>
        public bool IsOnFileOpenRegister { get { return (registerMonitorFileIOEvents & (ulong)MonitorFileIOEvents.OnFileOpen) > 0; } }
        public bool IsOnFileCreateRegister { get { return (registerMonitorFileIOEvents & (ulong)MonitorFileIOEvents.OnFileCreate) > 0; } }
        public bool IsOnQueryFileInfoRegister { get { return (registerMonitorFileIOEvents & (ulong)MonitorFileIOEvents.OnQueryFileInfo) > 0; } }
        public bool IsOnQueryFileSizeRegister { get { return (registerMonitorFileIOEvents & (ulong)MonitorFileIOEvents.OnQueryFileSize) > 0; } }
        public bool IsOnQueryBasicInfoRegister { get { return (registerMonitorFileIOEvents & (ulong)MonitorFileIOEvents.OnQueryFileBasicInfo) > 0; } }
        public bool IsOnQueryFileStandardInfoRegister { get { return (registerMonitorFileIOEvents & (ulong)MonitorFileIOEvents.OnQueryFileStandardInfo) > 0; } }
        public bool IsOnQueryFileNetworkInfoRegister { get { return (registerMonitorFileIOEvents & (ulong)MonitorFileIOEvents.OnQueryFileNetworkInfo) > 0; } }
        public bool IsOnQueryFileIdRegister { get { return (registerMonitorFileIOEvents & (ulong)MonitorFileIOEvents.OnQueryFileId) > 0; } }
        public bool IsOnDeleteFileRegister { get { return (registerMonitorFileIOEvents & (ulong)MonitorFileIOEvents.OnDeleteFile) > 0; } }
        public bool IsOnMoveOrRenameFileRegister { get { return (registerMonitorFileIOEvents & (ulong)MonitorFileIOEvents.OnMoveOrRenameFile) > 0; } }
        public bool IsOnSetFileBasicInfoRegister { get { return (registerMonitorFileIOEvents & (ulong)MonitorFileIOEvents.OnSetFileBasicInfo) > 0; } }
        public bool IsOnSetFileNetworkInfoRegister { get { return (registerMonitorFileIOEvents & (ulong)MonitorFileIOEvents.OnSetFileNetworkInfo) > 0; } }
        public bool IsOnSetFileSizeRegister { get { return (registerMonitorFileIOEvents & (ulong)MonitorFileIOEvents.OnSetFileSize) > 0; } }
        public bool IsOnSetFileStandardInfoRegister { get { return (registerMonitorFileIOEvents & (ulong)MonitorFileIOEvents.OnSetFileStandardInfo) > 0; } }
        public bool IsOnSetFileInfoRegister { get { return (registerMonitorFileIOEvents & (ulong)MonitorFileIOEvents.OnSetFileInfo) > 0; } }

        #endregion //monitor filter property

        /// <summary>
        /// Process the notification events, no return needed for the filter driver.
        /// </summary>
        /// <param name="messageSend"></param>
        public override void SendNotification(FilterAPI.MessageSendData messageSend)
        {

            try
            {
                if (messageSend.MessageType == (uint)FilterAPI.FilterCommand.FILTER_SEND_FILE_CHANGED_EVENT)
                {                    
                    if (null != NotifyFileWasChanged)
                    {
                        FileChangeEventArgs fileChangeEventArgs = new FileChangeEventArgs(messageSend);
                        NotifyFileWasChanged(this, fileChangeEventArgs);
                    }
                }
                else if (messageSend.MessageType == (uint)FilterAPI.MessageType.POST_CREATE)
                {
                    FileCreateEventArgs fileCreateEventArgs = new FileCreateEventArgs(messageSend);
                    if (fileCreateEventArgs.isNewFileCreated && IsOnFileCreateRegister)
                    {
                        fileCreateEventArgs.EventName = "OnNewFileCreate";
                        if (IsOnFileCreateRegister && null != OnNewFileCreate)
                        {
                            //new file was created.
                            OnNewFileCreate(this, fileCreateEventArgs);
                        }
                    }
                    else if (IsOnFileOpenRegister )
                    {
                        fileCreateEventArgs.EventName = "OnFileOpen";
                        if (null != OnFileOpen)
                        {
                            OnFileOpen(this, fileCreateEventArgs);
                        }
                    }

                    if (fileCreateEventArgs.isDeleteOnClose)
                    {
                        fileCreateEventArgs = new FileCreateEventArgs(messageSend);
                        fileCreateEventArgs.EventName = "OnDeleteFile";

                        if (null != OnDeleteFile && IsOnDeleteFileRegister )
                        {
                            OnDeleteFile(this, fileCreateEventArgs);
                        }
                    }
                }
                else if (messageSend.MessageType == (uint)FilterAPI.MessageType.PRE_CACHE_READ
                        || messageSend.MessageType == (uint)FilterAPI.MessageType.PRE_FASTIO_READ
                        || messageSend.MessageType == (uint)FilterAPI.MessageType.PRE_NOCACHE_READ
                        || messageSend.MessageType == (uint)FilterAPI.MessageType.PRE_PAGING_IO_READ)
                {
                    if (null != OnFileRead)
                    {
                        FileReadEventArgs fileReadEventArgs = new FileReadEventArgs(messageSend);
                        fileReadEventArgs.EventName = "OnPreFileRead-" + fileReadEventArgs.readType;

                        OnFileRead(this, fileReadEventArgs);
                    }
                }
                else if (messageSend.MessageType == (uint)FilterAPI.MessageType.POST_CACHE_READ
                  || messageSend.MessageType == (uint)FilterAPI.MessageType.POST_FASTIO_READ
                  || messageSend.MessageType == (uint)FilterAPI.MessageType.POST_NOCACHE_READ
                  || messageSend.MessageType == (uint)FilterAPI.MessageType.POST_PAGING_IO_READ)
                {
                    if (null != OnFileRead)
                    {
                        FileReadEventArgs fileReadEventArgs = new FileReadEventArgs(messageSend);
                        fileReadEventArgs.EventName = "OnFileRead-" + fileReadEventArgs.readType;

                        OnFileRead(this, fileReadEventArgs);
                    }
                }
                else if (messageSend.MessageType == (uint)FilterAPI.MessageType.PRE_CACHE_WRITE
                     || messageSend.MessageType == (uint)FilterAPI.MessageType.PRE_FASTIO_WRITE
                     || messageSend.MessageType == (uint)FilterAPI.MessageType.PRE_NOCACHE_WRITE
                     || messageSend.MessageType == (uint)FilterAPI.MessageType.PRE_PAGING_IO_WRITE)
                {
                    //if the event was subscribed 
                    if (null != OnFileWrite)
                    {
                        FileWriteEventArgs fileWriteEventArgs = new FileWriteEventArgs(messageSend);
                        fileWriteEventArgs.EventName = "OnPreFileWrite-" + fileWriteEventArgs.writeType;

                        OnFileWrite(this, fileWriteEventArgs);
                    }
                }
                else if (messageSend.MessageType == (uint)FilterAPI.MessageType.POST_CACHE_WRITE
                || messageSend.MessageType == (uint)FilterAPI.MessageType.POST_FASTIO_WRITE
                || messageSend.MessageType == (uint)FilterAPI.MessageType.POST_NOCACHE_WRITE
                || messageSend.MessageType == (uint)FilterAPI.MessageType.POST_PAGING_IO_WRITE)
                {
                    //if the event was subscribed 
                    if (null != OnFileWrite)
                    {
                        FileWriteEventArgs fileWriteEventArgs = new FileWriteEventArgs(messageSend);
                        fileWriteEventArgs.EventName = "OnFileWrite-" + fileWriteEventArgs.writeType;

                        OnFileWrite(this, fileWriteEventArgs);
                    }
                }
                else if (messageSend.MessageType == (uint)FilterAPI.MessageType.POST_QUERY_INFORMATION)
                {
                    if (messageSend.InfoClass == (uint)WinData.FileInfomationClass.FileEndOfFileInformation)
                    {
                        //if the event was subscribed 
                        if (null != OnQueryFileSize && IsOnQueryFileSizeRegister)
                        {
                            FileSizeEventArgs fileSizeArgs = new FileSizeEventArgs(messageSend);
                            fileSizeArgs.EventName = "OnQueryFileSize";

                            OnQueryFileSize(this, fileSizeArgs);
                        }
                    }
                    else if (messageSend.InfoClass == (uint)WinData.FileInfomationClass.FileInternalInformation)
                    {
                        //if the event was subscribed 
                        if (null != OnQueryFileId && IsOnQueryFileIdRegister)
                        {
                            FileIdEventArgs fileIdArgs = new FileIdEventArgs(messageSend);
                            fileIdArgs.EventName = "OnQueryFileId";

                            OnQueryFileId(this, fileIdArgs);
                        }
                    }
                    else if (messageSend.InfoClass == (uint)WinData.FileInfomationClass.FileBasicInformation)
                    {
                        //if the event was subscribed 
                        if (null != OnQueryFileBasicInfo && IsOnQueryBasicInfoRegister)
                        {
                            FileBasicInfoEventArgs fileBasicInfoArgs = new FileBasicInfoEventArgs(messageSend);
                            fileBasicInfoArgs.EventName = "OnQueryFileBasicInfo";

                            OnQueryFileBasicInfo(this, fileBasicInfoArgs);
                        }
                    }
                    else if (messageSend.InfoClass == (uint)WinData.FileInfomationClass.FileStandardInformation)
                    {
                        //if the event was subscribed 
                        if (null != OnQueryFileStandardInfo && IsOnQueryFileStandardInfoRegister)
                        {
                            FileStandardInfoEventArgs fileStandardInfoArgs = new FileStandardInfoEventArgs(messageSend);
                            fileStandardInfoArgs.EventName = "OnQueryFileStandardInfo";

                            OnQueryFileStandardInfo(this, fileStandardInfoArgs);
                        }
                    }
                    else if (messageSend.InfoClass == (uint)WinData.FileInfomationClass.FileNetworkOpenInformation)
                    {
                        //if the event was subscribed 
                        if (null != OnQueryFileNetworkInfo && IsOnQueryFileNetworkInfoRegister)
                        {
                            FileNetworkInfoEventArgs fileNetworkInfoArgs = new FileNetworkInfoEventArgs(messageSend);
                            fileNetworkInfoArgs.EventName = "OnQueryFileNetworkInfo";

                            if (messageSend.FsContext.ToInt64() == 0)
                            {
                                //this is the fast-io-network-query
                                fileNetworkInfoArgs.EventName += ";FastIO";
                            }

                            OnQueryFileNetworkInfo(this, fileNetworkInfoArgs);
                        }
                    }
                    else if (null != OnQueryFileInfo && IsOnQueryFileInfoRegister)
                    {
                        FileInfoArgs fileInfoArgs = new FileInfoArgs(messageSend);
                        fileInfoArgs.EventName = "OnQueryFileInfo：" + fileInfoArgs.FileInfoClass.ToString();

                        OnQueryFileInfo(this, fileInfoArgs);
                    }

                    //below is the different infomation class of the query file info IO
                }
                else if (messageSend.MessageType == (uint)FilterAPI.MessageType.POST_SET_INFORMATION)
                {

                    if (messageSend.InfoClass == (uint)WinData.FileInfomationClass.FileEndOfFileInformation)
                    {
                        //if the event was subscribed 
                        if (null != OnSetFileSize && IsOnSetFileSizeRegister)
                        {
                            FileSizeEventArgs fileSizeArgs = new FileSizeEventArgs(messageSend);
                            fileSizeArgs.EventName = "OnSetFileSize";

                            OnSetFileSize(this, fileSizeArgs);
                        }
                    }
                    else if (messageSend.InfoClass == (uint)WinData.FileInfomationClass.FileBasicInformation)
                    {
                        //if the event was subscribed 
                        if (null != OnSetFileBasicInfo && IsOnSetFileBasicInfoRegister)
                        {
                            FileBasicInfoEventArgs fileBasicInfoArgs = new FileBasicInfoEventArgs(messageSend);
                            fileBasicInfoArgs.EventName = "OnSetFileBasicInfo";

                            OnSetFileBasicInfo(this, fileBasicInfoArgs);
                        }
                    }
                    else if (messageSend.InfoClass == (uint)WinData.FileInfomationClass.FileStandardInformation)
                    {
                        //if the event was subscribed 
                        if (null != OnSetFileStandardInfo && IsOnSetFileStandardInfoRegister)
                        {
                            FileStandardInfoEventArgs fileStandardInfoArgs = new FileStandardInfoEventArgs(messageSend);
                            fileStandardInfoArgs.EventName = "OnSetFileStandardInfo";

                            OnSetFileStandardInfo(this, fileStandardInfoArgs);
                        }
                    }
                    else if (messageSend.InfoClass == (uint)WinData.FileInfomationClass.FileNetworkOpenInformation)
                    {
                        //if the event was subscribed 
                        if (null != OnSetFileNetworkInfo && IsOnSetFileNetworkInfoRegister)
                        {
                            FileNetworkInfoEventArgs fileNetworkInfoArgs = new FileNetworkInfoEventArgs(messageSend);
                            fileNetworkInfoArgs.EventName = "OnSetFileNetworkInfo";

                            OnSetFileNetworkInfo(this, fileNetworkInfoArgs);
                        }
                    }
                    else if (messageSend.InfoClass == (uint)WinData.FileInfomationClass.FileRenameInformation
                        || messageSend.InfoClass == (uint)WinData.FileInfomationClass.FileRenameInformationEx)
                    {
                        //if the event was subscribed 
                        if (null != OnMoveOrRenameFile && IsOnMoveOrRenameFileRegister)
                        {
                            FileMoveOrRenameEventArgs fileRenameArgs = new FileMoveOrRenameEventArgs(messageSend);
                            fileRenameArgs.EventName = "OnMoveOrRenameFile";

                            OnMoveOrRenameFile(this, fileRenameArgs);
                        }
                    }
                    else if (messageSend.InfoClass == (uint)WinData.FileInfomationClass.FileDispositionInformation
                        || messageSend.InfoClass == (uint)WinData.FileInfomationClass.FileDispositionInformationEx)
                    {
                        //if the event was subscribed 
                        if (null != OnDeleteFile && IsOnDeleteFileRegister)
                        {
                            FileIOEventArgs fileIOArgs = new FileIOEventArgs(messageSend);
                            fileIOArgs.EventName = "OnDeleteFile";

                            OnDeleteFile(this, fileIOArgs);
                        }
                    }
                    else if (null != OnSetFileInfo && IsOnSetFileInfoRegister)
                    {
                        //for all other information class, we will fire this event
                        FileInfoArgs fileInfoArgs = new FileInfoArgs(messageSend);
                        fileInfoArgs.EventName = "OnSetFileInfo：" + fileInfoArgs.FileInfoClass.ToString();

                        OnSetFileInfo(this, fileInfoArgs);
                    }

                }
                else if (messageSend.MessageType == (uint)FilterAPI.MessageType.POST_QUERY_SECURITY)
                {
                    //if the event was subscribed 
                    if (null != OnQueryFileSecurity)
                    {
                        FileSecurityEventArgs fileSecurityArgs = new FileSecurityEventArgs(messageSend);
                        fileSecurityArgs.EventName = "OnQueryFileSecurity";

                        OnQueryFileSecurity(this, fileSecurityArgs);
                    }
                }
                else if (messageSend.MessageType == (uint)FilterAPI.MessageType.POST_SET_SECURITY)
                {
                    //if the event was subscribed 
                    if (null != OnSetFileSecurity)
                    {
                        FileSecurityEventArgs fileSecurityArgs = new FileSecurityEventArgs(messageSend);
                        fileSecurityArgs.EventName = "OnSetFileSecurity";

                        OnSetFileSecurity(this, fileSecurityArgs);
                    }
                }
                else if (messageSend.MessageType == (uint)FilterAPI.MessageType.POST_DIRECTORY)
                {
                    //if the event was subscribed 
                    if (null != OnQueryDirectoryFile)
                    {
                        FileQueryDirectoryEventArgs directoryArgs = new FileQueryDirectoryEventArgs(messageSend);
                        directoryArgs.EventName = "OnQueryDirectoryFile";

                        OnQueryDirectoryFile(this, directoryArgs);
                    }
                }
                else if (messageSend.MessageType == (uint)FilterAPI.MessageType.POST_CLEANUP)
                {
                    //if the event was subscribed 
                    if (null != OnFileHandleClose)
                    {
                        FileIOEventArgs fileIOArgs = new FileIOEventArgs(messageSend);
                        fileIOArgs.EventName = "OnFileHandleClose";

                        fileIOArgs.Description = "The opened file handle was closed.";

                        OnFileHandleClose(this, fileIOArgs);
                    }
                }
                else if (messageSend.MessageType == (uint)FilterAPI.MessageType.POST_CLOSE)
                {
                    //if the event was subscribed 
                    if (null != OnFileClose)
                    {
                        FileIOEventArgs fileIOArgs = new FileIOEventArgs(messageSend);
                        fileIOArgs.EventName = "OnFileClose";

                        fileIOArgs.Description = "All system references to fileObject " + messageSend.FileObject.ToString("X") + " were closed.";

                        OnFileClose(this, fileIOArgs);
                    }
                }
                else
                {
                    string errorMessage = "\r\nThe messageType:" + messageSend.MessageType.ToString("X") + " can't be handled in monitor filter, messageId:" 
                        + messageSend.MessageId + ",name:" + messageSend.FileName + "\r\n";
                    Console.WriteLine(errorMessage);
                    Utils.ToDebugger(errorMessage);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("SendNotification exception:" + ex.Message);
                Utils.ToDebugger("\n\n\nSendNotification exception:" + ex.Message);
            }
        }

    }
}
