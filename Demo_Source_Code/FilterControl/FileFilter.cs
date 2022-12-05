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
using System.Runtime.InteropServices;
using System.IO;
using System.Text;

namespace EaseFilter.FilterControl
{
    public class Filter
    {
        public FilterAPI.FilterType FilterType;
        public uint FilterId;

        public virtual void SendNotification(FilterAPI.MessageSendData messageSend)
        {
        }

        public virtual bool ReplyMessage(FilterAPI.MessageSendData messageSend, IntPtr replyDataPtr)
        {
            return true;
        }
    }

    /// <summary>
    /// A FileFilter is the file filter rule to monitor or control the file I/O by the filter driver.
    /// The file filter rule includes monitor filter rule, control filter rule and encryption filter rule. 
    /// A monitor filter rule allows you to monitor the file change and file I/O activities.
    /// A control filter rule allows you to control the file access or modify the file I/O data.
    /// A encryption filter rule allows you to encrypt or decrypt the file automatically.
    /// </summary>
    public partial class FileFilter : Filter
    {
        /// <summary>
        /// The file filter mask of this filter rule, the 
        /// </summary>
        string fileFilterMask = string.Empty;

        /// <summary>
        /// when it is true, the filter is passthrough filter
        /// </summary>
        bool isPassthroughFilter = false;

        /// <summary>
        /// Skip all the file I/Os when the file open doesn't match with below options when they are not 0.
        /// </summary>
        uint filterDesiredAccess = 0;
        uint filterDisposition = 0;
        uint filterCreateOptions = 0;

        List<string> hidenFileFilterMaskList = new List<string>();
        string reparseFileFilterMask = string.Empty;

        /// <summary>
        /// Filter the file I/Os based on the process name,Id, user name.
        /// </summary>
        List<string> excludeFileFilterMaskList = new List<string>();
        List<string> includeProcessNameList = new List<string>();
        List<string> excludeProcessNameList = new List<string>();
        List<uint> includeProcessIdList = new List<uint>();
        List<uint> excludeProcessIdList = new List<uint>();
        List<string> includeUserNameList = new List<string>();
        List<string> excludeUserNameList = new List<string>();

        /// <summary>
        /// Enable the control file filter rule in boot time.
        /// </summary>
        bool isResident = false;

        /// <summary>
        /// the boolean config setting of the filter, reference FilterAPI.BooleanConfig
        /// </summary>
        uint booleanConfig = 0;

        public FileFilter(string fileFilterMask)
        {
            this.fileFilterMask = fileFilterMask;
        }

        /// <summary>
        /// The file filter mask of the file filter, the filter driver will manage the files based on this filter mask.
        /// This is unique for the file filter, only the file matches the IncludeFileFilterMask, the IO will be managed by this filter.
        /// i.e. c:\test\*, only the IO for files from folder c:\test will be intercepted by the filter driver.
        /// </summary>
        public string IncludeFileFilterMask
        {
            get { return fileFilterMask; }
        }

        /// <summary>
        /// The boolean config setting of the file filter.
        /// </summary>
        public uint BooleanConfig
        {
            get { return booleanConfig; }
            set { booleanConfig = value; }
        }

        /// <summary>
        /// Get or set the exclude file filter mask list, 
        /// skip the file I/Os which the file name matches the exclude file filter mask.
        /// i.e. c:\test\exclude\*, all the IO for files from folder c:\test\exclude will be excluded from this filter.
        /// </summary>
        public List<string> ExcludeFileFilterMaskList
        {
            get { return excludeFileFilterMaskList; }
            set { excludeFileFilterMaskList = value; }
        }

        /// <summary>
        /// Manage the IOs only from the include process name list when it is not empty.
        /// i.e. explorer.exe, only the IO from process explorer will be intercepted by this filter.
        /// </summary>
        public List<string> IncludeProcessNameList
        {
            get { return includeProcessNameList; }
            set { includeProcessNameList = value; }
        }

        /// <summary>
        /// Skip all the IOs which are from the exclude process name list when it is not empty.
        /// i.e. explorer.exe, all the IO from process explorer will be excluded by this filter.
        /// </summary>
        public List<string> ExcludeProcessNameList
        {
            get { return excludeProcessNameList; }
            set { excludeProcessNameList = value; }
        }

        /// <summary>
        /// Manage the IOs only from the include process id list when it is not empty.
        /// Please note that the process Id might be changed when it was reopened.
        /// i.e. 1234, only the IO from process Id 1234 will be intercepted by this filter.
        /// </summary>
        public List<uint> IncludeProcessIdList
        {
            get { return includeProcessIdList; }
            set { includeProcessIdList = value; }
        }

        /// <summary>
        /// Skip all the IOs which are from the exclude process Id list when it is not empty.
        /// i.e. 1234, all the IO from the process Id 1234 will be excluded by this filter.
        /// </summary>
        public List<uint> ExcludeProcessIdList
        {
            get { return excludeProcessIdList; }
            set { excludeProcessIdList = value; }
        }

        /// <summary>
        /// Manage the IOs only from the include user name list when it is not empty.
        /// i.e. myDomainName\myUserName, only the IO from this user myDomainName\myUserName will be intercepted by this filter.
        /// </summary>
        public List<string> IncludeUserNameList
        {
            get { return includeUserNameList; }
            set { includeUserNameList = value; }
        }

        /// <summary>
        /// Skip all the IOs which are from the exclude user name list when it is not empty.
        /// i.e. myDomainName\myUserName, all the IO from this user myDomainName\myUserName will be excluded by this filter.
        /// </summary>
        public List<string> ExcludeUserNameList
        {
            get { return excludeUserNameList; }
            set { excludeUserNameList = value; }
        }

        /// <summary>
        /// Skip all the file I/Os when the file open doesn't match below option when it is not 0.
        /// i.e. GENERIC_READ, only file opens with read access will be intercepted by this filter.
        /// </summary>
        public uint FilterDesiredAccess
        {
            get { return filterDesiredAccess; }
            set { filterDesiredAccess = value; }
        }

        /// <summary>
        /// Skip all the file I/Os when the file open doesn't match below option when it is not 0.
        /// i.e. OPEN_EXISTING, only file opens with existing file will be intercepted by this filter.
        /// </summary>
        public uint FilterDisposition
        {
            get { return filterDisposition; }
            set { filterDisposition = value; }
        }

        /// <summary>
        /// Skip all the file I/Os when the file open doesn't match below option when it is not 0.
        /// i.e. FILE_OPEN_REPARSE_POINT, only file opens with FILE_OPEN_REPARSE_POINT will be intercepted by this filter.
        /// </summary>
        public uint FilterCreateOptions
        {
            get { return filterCreateOptions; }
            set { filterCreateOptions = value; }
        }

        /// <summary>
        /// Set the passthrough filter when it is true.
        /// skip all the file I/Os of the files in this filter when it is true
        /// </summary>
        public bool IsPassthroughFilter
        {
            get { return isPassthroughFilter; }
            set { isPassthroughFilter = value; }
        }

        /// <summary>
        /// if it is true it will send the read or write data to callback deletegate or it won't send the data.
        /// </summary>
        public bool EnableSendReadOrWriteBuffer
        {
            get
            {
                return (booleanConfig & (uint)FilterAPI.BooleanConfig.ENABLE_SEND_DATA_BUFFER) > 0;
            }

            set
            {
                if (value)
                {
                    booleanConfig |= (uint)FilterAPI.BooleanConfig.ENABLE_SEND_DATA_BUFFER;
                }
                else
                {
                    booleanConfig &= ~(uint)FilterAPI.BooleanConfig.ENABLE_SEND_DATA_BUFFER;
                }
            }
        }


        public override bool ReplyMessage(FilterAPI.MessageSendData messageSend, IntPtr replyDataPtr)
        {
            bool retVal = true;

            try
            {
                FilterAPI.MessageReplyData messageReply = (FilterAPI.MessageReplyData)Marshal.PtrToStructure(replyDataPtr, typeof(FilterAPI.MessageReplyData));
                if (messageSend.FilterCommand == (uint)FilterAPI.FilterCommand.FILTER_REQUEST_ENCRYPTION_IV_AND_KEY
                    || messageSend.FilterCommand == (uint)FilterAPI.FilterCommand.FILTER_REQUEST_ENCRYPTION_IV_AND_KEY_AND_TAGDATA)
                {
                    RequestEncryptionKey(messageSend, ref messageReply);
                }
                else
                {
                    FireControlEvents(messageSend, ref messageReply);
                }

                Marshal.StructureToPtr(messageReply, replyDataPtr, true);
            }
            catch
            {
            }

            return retVal;
        }



    }

    #region event args definition
    /// <summary>
    /// This is the create options when the process opens the file, normally every file IO needs to open/create a file first,
    /// the process will get a file handle which associated to the FileObject in filter driver till the process closes the handle.
    /// </summary>
    public class CreateFileOptions
    {
        public uint DesiredAccess { get; set; }
        public uint Disposition { get; set; }
        public uint CreateOptions { get; set; }
        public uint SharedAccess { get; set; }
        public override string ToString()
        {
            string description = string.Empty;

            description += "DesiredAccess:" + WinData.DesiredAccessToString(DesiredAccess);
            description += " Disposition:" + ((WinData.Disposition)Disposition).ToString();
            description += " ShareAccess:" + ((WinData.ShareAccess)SharedAccess).ToString();
            description += " CreateOptions:" + WinData.CreateOptionsToString(CreateOptions);

            return description;
        }
    }


    /// <summary>
    /// This is the general information for every file IO which was sent by filter driver if you register the callback IO.
    /// </summary>
    public  class FileIOEventArgs : EventArgs
    {
        public FileIOEventArgs(FilterAPI.MessageSendData messageSend)
        {
            string userName = string.Empty;
            string processName = string.Empty;

            Utils.DecodeUserName(messageSend.Sid, out userName);
            Utils.DecodeProcessName(messageSend.ProcessId, out processName);

            UserName = userName;
            ProcessName = processName;
            FileObject = messageSend.FileObject;
            FsContext = messageSend.FsContext;
            MessageId = messageSend.MessageId;
            FilterRuleId = messageSend.FilterRuleId;
            TransactionTime = messageSend.TransactionTime;
            CreationTime = messageSend.CreationTime;
            LastWriteTime = messageSend.LastWriteTime;
            ProcessId = messageSend.ProcessId;
            ThreadId = messageSend.ThreadId;
            FileName = messageSend.FileName;
            FileSize = messageSend.FileSize;
            FileAttributes = messageSend.FileAttributes;
            IoStatus = (NtStatus.Status)messageSend.Status;

            SharedAccess = messageSend.SharedAccess;
            CreateFileOptions createOptions = new CreateFileOptions();
            createOptions.CreateOptions = messageSend.CreateOptions;
            createOptions.DesiredAccess = messageSend.DesiredAccess;
            createOptions.Disposition = messageSend.Disposition;
            createOptions.SharedAccess = messageSend.SharedAccess;
            CreateOptions = createOptions;

            IsRemoteAccess = false;
            RemoteIp = "LocalHost";
            if ((messageSend.CreateOptions & (uint)WinData.CreateOptions.FO_REMOTE_ORIGIN) > 0)
            {
                //this is the request comes from remote server
                IsRemoteAccess = true;
                RemoteIp = Encoding.Unicode.GetString(messageSend.RemoteIP);
            }
        }

        public string IOStatusToString()
        {
            string ret = string.Empty;
            uint status = (uint)IoStatus;

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

        /// <summary>
        /// The Message Id.
        /// </summary>
        public uint MessageId { get; set; }
        /// <summary>
        /// The filter rule Id.
        /// </summary>
        public uint FilterRuleId { get; set; }
        /// <summary>
        ///This is the IO completion status, either STATUS_SUCCESS if the requested operation was completed successfully 
        ///or an informational, warning, or error STATUS_XXX value, only effect on post IO.
        /// </summary>
        public NtStatus.Status IoStatus { get; set; }
        /// <summary>
        /// the transaction time in UTC of this IO request
        /// </summary>
        public long TransactionTime { get; set; }
        /// <summary>
        /// The fileObject is an unique Id for the file I/O from open till the close.
        /// </summary>
        public IntPtr FileObject { get; set; }
        /// <summary>
        /// A pointer to the FSRTL_ADVANCED_FCB_HEADER header structure that  is contained 
        /// within a file-system-specific structure,it is unique per file.
        /// </summary>
        public IntPtr FsContext { get; set; }
        /// <summary>
        /// The process Id who initiates the IO.
        /// </summary>
        public uint ProcessId { get; set; }
        /// <summary>
        /// The process name who initiates the IO.
        /// </summary>
        public string ProcessName { get; set; }
        /// <summary>
        /// The thread Id who initiates the IO.
        /// </summary>
        public uint ThreadId { get; set; }
        /// <summary>
        /// The user name who initiates the IO.
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// The file name of the file IO.
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// The file size of the file IO.
        /// </summary>
        public long FileSize { get; set; }
        /// <summary>
        /// The creation time in UTC of the file.
        /// </summary>
        public long CreationTime { get; set; }
        /// <summary>
        /// The last write time in UTC of the file.
        /// </summary>
        public long LastWriteTime { get; set; }
        /// <summary>
        /// The file attributes of the file IO.
        /// </summary>
        public uint FileAttributes { get; set; }
        /// <summary>
        /// The file create options of the file IO.
        /// </summary>
        public CreateFileOptions CreateOptions { get; set; }
        /// <summary>
        /// The SharedAccess for file open, please reference CreateFile windows API.
        /// </summary>
        public uint SharedAccess { get; set; }
        /// <summary>
        /// The name of the event which uses this eventargs
        /// </summary>
        public string EventName { get; set; }
        /// <summary>
        /// The file open was from the network
        /// </summary>
        public bool IsRemoteAccess { get; set; }
        /// <summary>
        /// The IP address of the remote computer who is opening the file.
        /// This feature is enabled only for Win7 or later OS.
        /// </summary>
        public string RemoteIp { get; set; }
        
        string description = string.Empty;
        /// <summary>
        /// The description of the IO.
        /// </summary>
        public virtual string Description 
        {
            get { return description; }
            set { description = value; } 
        }
        /// <summary>
        /// Change the return status of the IO, it is only for control filter or encryption filter
        /// set the status to AccessDenied if you want to block this IO.
        /// </summary>
        public NtStatus.Status ReturnStatus { get; set; }
        /// <summary>
        /// Set it to true, if the return data was modified. 
        /// </summary>
        public bool IsDataModified { get; set; }
    }

    /// <summary>
    /// The callback information of the file change event.
    /// </summary>
    public class FileChangedEventArgs : FileIOEventArgs
    {
        public FileChangedEventArgs(FilterAPI.MessageSendData messageSend)
            : base(messageSend)
        {

            if ((messageSend.InfoClass & (uint)FilterAPI.FileChangedEvents.NotifyFileWasCreated) > 0)
            {
                eventType = FilterAPI.FileChangedEvents.NotifyFileWasCreated;
                EventName = "NotifyFileWasCreated;";
                Description = "The new file " + FileName + " was created.";
            }

            if ((messageSend.InfoClass & (uint)FilterAPI.FileChangedEvents.NotifyFileWasWritten) > 0)
            {
                eventType |= FilterAPI.FileChangedEvents.NotifyFileWasWritten;
                EventName += "NotifyFileWasWritten;";
                Description += "The file " + FileName + " was written.";
            }

            if ((messageSend.InfoClass & (uint)FilterAPI.FileChangedEvents.NotifyFileWasDeleted) > 0)
            {
                eventType |= FilterAPI.FileChangedEvents.NotifyFileWasDeleted;
                EventName += "NotifyFileWasDeleted;";
                Description += "The file " + FileName + " was deleted.";
            }

            if ((messageSend.InfoClass & (uint)FilterAPI.FileChangedEvents.NotifyFileInfoWasChanged) > 0)
            {
                eventType |= FilterAPI.FileChangedEvents.NotifyFileInfoWasChanged;
                EventName += "NotifyFileInfoWasChanged;";
                Description += "The file " + FileName + " information was changed.";
            }

            if ((messageSend.InfoClass & (uint)FilterAPI.FileChangedEvents.NotifyFileWasRenamed) > 0)
            {
                if (messageSend.DataBufferLength > 0)
                {
                    newFileName = Encoding.Unicode.GetString(messageSend.DataBuffer);
                    if (newFileName.IndexOf((char)0) > 0)
                    {
                        newFileName = newFileName.Remove(newFileName.IndexOf((char)0));
                    }
                }

                eventType |= FilterAPI.FileChangedEvents.NotifyFileWasRenamed;
                EventName += "NotifyFileWasRenamed;";
                Description += "The file " + FileName + " was renamed to " + newFileName + ".";
            }

            if ((messageSend.InfoClass & (uint)FilterAPI.FileChangedEvents.NotifyFileSecurityWasChanged) > 0)
            {
                eventType |= FilterAPI.FileChangedEvents.NotifyFileSecurityWasChanged;
                EventName += "NotifyFileSecurityWasChanged;";
                Description += "The file " + FileName + " security was changed.";
            }

            if ((messageSend.InfoClass & (uint)FilterAPI.FileChangedEvents.NotifyFileWasRead) > 0)
            {
                eventType |= FilterAPI.FileChangedEvents.NotifyFileWasRead;
                EventName += "NotifyFileWasRead;";
                Description += "The file " + FileName + " was read.";
            }

        }

        /// <summary>
        /// The new file name of the rename IO
        /// </summary>
        public string newFileName { get; set; }
        /// <summary>
        /// The event type of the file change event.
        /// </summary>
        public FilterAPI.FileChangedEvents eventType { get; set; }
      
    }

    public class FileCreateEventArgs : FileIOEventArgs
    {
        public FileCreateEventArgs(FilterAPI.MessageSendData messageSend)
            : base(messageSend)
        {           
            if ((messageSend.CreateOptions & (uint)WinData.CreateOptions.FILE_DELETE_ON_CLOSE) > 0)
            {
                isDeleteOnClose = true;
            }
            else
            {
                isDeleteOnClose = false;
            }

            if (    messageSend.Status == 0 
                && ((uint)FilterAPI.MessageType.POST_CREATE == messageSend.MessageType))
            {
                if ((WinData.CreateStatus)messageSend.CreateStatus == WinData.CreateStatus.FILE_CREATED
                    || messageSend.CreateStatus == (uint)WinData.CreateStatus.FILE_SUPERSEDED )
                {
                    isNewFileCreated = true;
                }
                else
                {
                    isNewFileCreated = false;
                }
            }
        }
    
        /// <summary>
        /// The file will be deleted on close when it was true.
        /// </summary>
        public bool isDeleteOnClose{ get; set; }
        /// <summary>
        /// Reparse the file open to this new file name for control filter.
        /// the file name format should like this "\\??\\c:\\mynewfolder\\myNewFile.txt";
        /// </summary>
        public string reparseFileName { get; set; }
        /// <summary>
        /// It indicates that the new file was created if it is true.
        /// </summary>
        public bool isNewFileCreated { get; set; }

        /// <summary>
        /// The description of the IO args
        /// </summary>
        public override string Description
        {
            get
            {
                string message = CreateOptions.ToString();

                if (isNewFileCreated)
                {
                    message = "The new file was created." + message;
                }

                if (isDeleteOnClose)
                {
                    message = "The file will be deleted on close." + message;
                }


                return message;
            }
        }
    }

    public class FileReadEventArgs : FileIOEventArgs
    {
        public FileReadEventArgs(FilterAPI.MessageSendData messageSend)
            : base(messageSend)
        {
            offset = messageSend.Offset;
            readLength = messageSend.Length;
            returnReadLength = messageSend.ReturnLength;

            if (messageSend.DataBufferLength > 0 && messageSend.DataBufferLength <= messageSend.DataBuffer.Length)
            {
                buffer = new byte[messageSend.DataBufferLength];
                Array.Copy(messageSend.DataBuffer, buffer, buffer.Length);
            }

            if (messageSend.MessageType == (uint)FilterAPI.MessageType.POST_NOCACHE_READ
                || messageSend.MessageType == (uint)FilterAPI.MessageType.PRE_NOCACHE_READ)
            {
                readType = "NonCacheRead";
            }
            else if (   messageSend.MessageType == (uint)FilterAPI.MessageType.PRE_PAGING_IO_READ
                       || messageSend.MessageType == (uint)FilterAPI.MessageType.POST_PAGING_IO_READ)
            {
                readType = "PagingIORead";
            }
            else
            {
                readType = "CacheRead";
            }
        }

        /// <summary>
        /// The read offset
        /// </summary>
        public long offset { get; set; }
        /// <summary>
        /// The length of the read
        /// </summary>
        public uint readLength { get; set; }
        /// <summary>
        /// The return length of the read
        /// </summary>
        public uint returnReadLength { get; set; }
        /// <summary>
        /// The read data buffer
        /// </summary>
        public byte[] buffer { get; set; }
        /// <summary>
        /// The read type
        /// </summary>
        public string readType { get; set; }

        /// <summary>
        /// The description of the IO args
        /// </summary>
        public override string Description
        {
            get
            {
                string message = "ReadOffset:" + offset + ";ReadLength:" + readLength + ";returnReadLength:" + returnReadLength;
                return message;
            }
            
        }
    }

    public class FileWriteEventArgs : FileIOEventArgs
    {
        public FileWriteEventArgs(FilterAPI.MessageSendData messageSend)
            : base(messageSend)
        {
            offset = messageSend.Offset;
            writeLength = messageSend.Length;
            writtenLength = messageSend.ReturnLength;
            bufferLength = messageSend.DataBufferLength;

            if (messageSend.DataBufferLength > 0 && messageSend.DataBufferLength <= messageSend.DataBuffer.Length )
            {
                buffer = new byte[messageSend.DataBufferLength];
                Array.Copy(messageSend.DataBuffer, buffer, buffer.Length);
            }

            if (    messageSend.MessageType == (uint)FilterAPI.MessageType.POST_NOCACHE_WRITE
                || messageSend.MessageType == (uint)FilterAPI.MessageType.PRE_NOCACHE_WRITE)
            {
                writeType= "NonCacheWrite";
            }
            else if (messageSend.MessageType == (uint)FilterAPI.MessageType.PRE_PAGING_IO_WRITE
                ||messageSend.MessageType == (uint)FilterAPI.MessageType.POST_PAGING_IO_WRITE)
            {
                 writeType = "PagingIOWrite";
            }
            else
            {
                writeType = "CacheWrite";
            }
        }

        /// <summary>
        /// The write offset
        /// </summary>
        public long offset { get; set; }
        /// <summary>
        /// The length of the write
        /// </summary>
        public uint writeLength { get; set; }
        /// <summary>
        /// The length of the written
        /// </summary>
        public uint writtenLength { get; set; }
        /// <summary>
        /// The data buffer length
        /// </summary>
        public uint bufferLength { get; set; }
        /// <summary>
        /// The write data buffer
        /// </summary>
        public byte[] buffer { get; set; }
        /// <summary>
        /// The write type
        /// </summary>
        public string writeType { get; set; }

        /// <summary>
        /// The description of the IO args
        /// </summary>
        public override string Description
        {
            get
            {
                string message = "WriteOffset:" + offset + ",writeLength:" + writeLength + ",WrittenLength:" + writtenLength;
                return message;
            }
            
        }
    }

    public class FileSizeEventArgs : FileIOEventArgs
    {
        public FileSizeEventArgs(FilterAPI.MessageSendData messageSend)
            : base(messageSend)
        {
            if (messageSend.DataBufferLength > 0)
            {
                fileSizeToQueryOrSet = BitConverter.ToInt64(messageSend.DataBuffer, 0);
            }
        }

        /// <summary>
        /// The size of the file used by FileEndOfFileInformation class
        /// </summary>
        public long fileSizeToQueryOrSet { get; set; }

        /// <summary>
        /// The description of the IO args
        /// </summary>
        public override string Description
        {
            get
            {
                return "FileSize:" + fileSizeToQueryOrSet;
            }
            
        }
    }

    public class FileIdEventArgs : FileIOEventArgs
    {
        public FileIdEventArgs(FilterAPI.MessageSendData messageSend)
            : base(messageSend)
        {
            if (messageSend.DataBufferLength > 0)
            {
                fileId = BitConverter.ToInt64(messageSend.DataBuffer, 0);
            }
        }
        /// <summary>
        /// The file Id used by FileInternalInformation class
        /// </summary>
        public long fileId { get; set; }
        /// <summary>
        /// The description of the IO args
        /// </summary>
        public override string Description
        {
            get
            {
                if (fileId > 0)
                {
                    return "FileId:0x" + fileId.ToString("X");
                }
                else
                {
                    return "";
                }
            }
            
        }
    }

    public class FileBasicInfoEventArgs : FileIOEventArgs
    {
        public FileBasicInfoEventArgs(FilterAPI.MessageSendData messageSend)
            : base(messageSend)
        {
            if (messageSend.DataBufferLength > 0)
            {
                GCHandle pinnedPacket = GCHandle.Alloc(messageSend.DataBuffer, GCHandleType.Pinned);
                basicInfo = (WinData.FileBasicInformation)Marshal.PtrToStructure(
                    pinnedPacket.AddrOfPinnedObject(), typeof(WinData.FileBasicInformation));
                pinnedPacket.Free();
            }

        }
        /// <summary>
        /// The file basic information
        /// </summary>
        public WinData.FileBasicInformation basicInfo { get; set; }

        public override string Description
        {
            get
            {
                if (basicInfo.CreationTime > 0)
                {
                    string desc = "FileBasicInformation,creation time:" + DateTime.FromFileTime(basicInfo.CreationTime).ToString("yyyy-MM-ddTHH:mm") + "   ";
                    desc += "last access time:" + DateTime.FromFileTime(basicInfo.LastAccessTime).ToString("yyyy-MM-ddTHH:mm") + "   ";
                    desc += "last write time:" + DateTime.FromFileTime(basicInfo.LastWriteTime).ToString("yyyy-MM-ddTHH:mm") + "   ";
                    desc += "file attributes:" + ((FileAttributes)basicInfo.FileAttributes).ToString();

                    return desc;
                }
                else return "";
            }
            
        }
    }

    public class FileStandardInfoEventArgs : FileIOEventArgs
    {
        public FileStandardInfoEventArgs(FilterAPI.MessageSendData messageSend)
            : base(messageSend)
        {
            if (messageSend.DataBufferLength > 0)
            {
                GCHandle pinnedPacket = GCHandle.Alloc(messageSend.DataBuffer, GCHandleType.Pinned);
                standardInfo = (WinData.FileStandardInformation)Marshal.PtrToStructure(
                    pinnedPacket.AddrOfPinnedObject(), typeof(WinData.FileStandardInformation));
                pinnedPacket.Free();
            }
         
        }
        /// <summary>
        /// The file standard information
        /// </summary>
        public WinData.FileStandardInformation standardInfo { get; set; }

         public override string Description
        {
            get
            {
                if (standardInfo.EndOfFile > 0)
                {
                    string desc = "FileStandardInformation, file size:" + standardInfo.EndOfFile.ToString() + "   ";
                    desc += "allocation size:" + standardInfo.AllocationSize.ToString() + "   ";
                    desc += "isDirectory:" + standardInfo.Directory.ToString();

                    return desc;
                }
                else return "";
            }
            
        }
    }

    public class FileNetworkInfoEventArgs : FileIOEventArgs
    {
        public FileNetworkInfoEventArgs(FilterAPI.MessageSendData messageSend)
            : base(messageSend)
        {
            if (messageSend.DataBufferLength > 0)
            {
                GCHandle pinnedPacket = GCHandle.Alloc(messageSend.DataBuffer, GCHandleType.Pinned);
                networkInfo = (WinData.FileNetworkInformation)Marshal.PtrToStructure(
                    pinnedPacket.AddrOfPinnedObject(), typeof(WinData.FileNetworkInformation));
                pinnedPacket.Free();
            }
     
        }
        /// <summary>
        /// The file network information
        /// </summary>
        public WinData.FileNetworkInformation networkInfo { get; set; }

        public override string Description
        {
            get
            {
                if (networkInfo.CreationTime > 0)
                {
                    string desc = "FileNetworkOpenInformation, creation time:" + DateTime.FromFileTime(networkInfo.CreationTime).ToString("yyyy-MM-ddTHH:mm") + "   ";
                    desc += "last access time:" + DateTime.FromFileTime(networkInfo.LastAccessTime) + "   ";
                    desc += "last write time:" + DateTime.FromFileTime(networkInfo.LastWriteTime) + "   ";
                    desc += "file size:" + networkInfo.FileSize.ToString() + "   ";
                    desc += "file attributes:" + ((FileAttributes)networkInfo.FileAttributes).ToString();

                    return desc;
                }
                else return "";
            }
            
        }

    }

    public class FileMoveOrRenameEventArgs : FileIOEventArgs
    {
        public FileMoveOrRenameEventArgs(FilterAPI.MessageSendData messageSend)
            : base(messageSend)
        {
            if (messageSend.DataBufferLength > 0)
            {
                newFileName = Encoding.Unicode.GetString(messageSend.DataBuffer);
                if (newFileName.IndexOf((char)0) > 0)
                {
                    newFileName = newFileName.Remove(newFileName.IndexOf((char)0));
                }
            }
        }
        /// <summary>
        /// The new file name of the move or rename IO
        /// </summary>
        public string newFileName { get; set; }

        /// <summary>
        /// The description of the event args
        /// </summary>
        public override string Description
        {
            get
            {
                if (null != newFileName && newFileName.Length > 0)
                {
                    string desc = "The file " + FileName + " was renamed to " + newFileName;
                    return desc;
                }
                else return "";
            }
            
        }

    }

    public class FileInfoArgs : FileIOEventArgs
    {
        public FileInfoArgs(FilterAPI.MessageSendData messageSend)
            : base(messageSend)
        {
            FileInfoClass = (WinData.FileInfomationClass)messageSend.InfoClass;

            if (messageSend.DataBufferLength > 0)
            {
                DataBuffer = new byte[messageSend.DataBufferLength];
                Array.Copy(messageSend.DataBuffer, DataBuffer, messageSend.DataBufferLength);
            }
        }
        /// <summary>
        /// The information class of the IO
        /// </summary>
        public WinData.FileInfomationClass FileInfoClass { get; set; }
        /// <summary>
        /// The information data of the file associated to the info class.
        /// </summary>
        public byte[] DataBuffer { get; set; }

        /// <summary>
        /// The description of the event args
        /// </summary>
        public override string Description
        {
            get
            {
                return "FileInformationClass:" + FileInfoClass.ToString();
            }

        }

    }

    public class FileSecurityEventArgs : FileIOEventArgs
    {
        public FileSecurityEventArgs(FilterAPI.MessageSendData messageSend)
            : base(messageSend)
        {
            securityInformation = (WinData.SecurityInformation)messageSend.InfoClass;
            if (messageSend.DataBufferLength > 0)
            {
                securityBuffer = new byte[messageSend.DataBufferLength];
                Array.Copy(messageSend.DataBuffer, securityBuffer, messageSend.DataBufferLength);
            }

        }

        /// <summary>
        /// The security information to be queried/set
        /// </summary>
        public WinData.SecurityInformation securityInformation { get; set; }
        public byte[] securityBuffer { get; set; }

        /// <summary>
        /// The description of the event args
        /// </summary>
        public override string Description
        {
            get
            {
                string securityInfo = string.Empty;
                foreach (WinData.SecurityInformation security in Enum.GetValues(typeof(WinData.SecurityInformation)))
                {
                    if (((uint)securityInformation & (uint)security) > 0)
                    {
                        if (securityInfo.Length > 0)
                        {
                            securityInfo += ";";
                        }

                        securityInfo += security.ToString();
                    }
                }

                return securityInfo;
            }
            
        }
    }

    public class FileQueryDirectoryEventArgs : FileIOEventArgs
    {
        public FileQueryDirectoryEventArgs(FilterAPI.MessageSendData messageSend)
            : base(messageSend)
        {
            fileInfomationClass = (WinData.FileInfomationClass)messageSend.InfoClass;
            if (messageSend.DataBufferLength > 0)
            {
                directoryBuffer = new byte[messageSend.DataBufferLength];
                Array.Copy(messageSend.DataBuffer, directoryBuffer, messageSend.DataBufferLength);
            }
        }

        /// <summary>
        /// The type of information to be returned about files in the directory.
        /// </summary>
        public WinData.FileInfomationClass fileInfomationClass { get; set; }
        /// <summary>
        /// The buffer that receives the desired information about the file. 
        /// The structure of the information returned in the buffer is defined by the FileInformationClass.
        /// </summary>
        public byte[] directoryBuffer { get; set; }

        /// <summary>
        /// The description of the event args
        /// </summary>
        public override string Description
        {
            get
            {
                return "Query directory with information class:" + fileInfomationClass.ToString();
            }
            
        }
    }

    public class DeniedFileIOEventArgs : FileIOEventArgs
    {
        public DeniedFileIOEventArgs(FilterAPI.MessageSendData messageSend)
            : base(messageSend)
        {
            EventName = ((FilterAPI.MessageType)messageSend.MessageType).ToString() + " was blocked.";

            if (messageSend.DataBufferLength > 0 && (FilterAPI.AccessFlag)messageSend.InfoClass == FilterAPI.AccessFlag.ALLOW_FILE_RENAME)
            {
                string newFileName = Encoding.Unicode.GetString(messageSend.DataBuffer);
                if (newFileName.IndexOf((char)0) > 0)
                {
                    newFileName = newFileName.Remove(newFileName.IndexOf((char)0));
                }

                Description = "Rename file " + messageSend.FileName + " to " + newFileName + " was blocked by the setting.";
            }
            else
            {
                Description = "File access control flag " + ((FilterAPI.AccessFlag)messageSend.InfoClass).ToString() + " was disabled by the setting.";
            }
        }

    }

    public class DeniedUSBReadEventArgs : FileIOEventArgs
    {
        public DeniedUSBReadEventArgs(FilterAPI.MessageSendData messageSend)
            : base(messageSend)
        {
            EventName = "BlockUSBRead";
            Description = "Reading file from USB was blocked by the setting.";
        }
    }

    public class DeniedUSBWriteEventArgs : FileIOEventArgs
    {
        public DeniedUSBWriteEventArgs(FilterAPI.MessageSendData messageSend)
            : base(messageSend)
        {
            EventName = "BlockUSBWrite";

            if (messageSend.InfoClass == (uint)FilterAPI.AccessFlag.ALLOW_COPY_PROTECTED_FILES_TO_USB)
            {
                Description = "Copy the protected file to USB was blocked by the setting.";
            }
            else
            {
                Description = "Writting the file to USB was blocked by the setting.";
            }
        }
    }

  
    public class DeniedProcessTerminatedEventArgs : FileIOEventArgs
    {
        public DeniedProcessTerminatedEventArgs(FilterAPI.MessageSendData messageSend)
            : base(messageSend)
        {
            EventName = "DeniedProcessTerminated";
            Description = "Block killing process " + ProcessName + " ungratefully.";
        }

    }

    public class VolumeInfoEventArgs : FileIOEventArgs
    {
        public VolumeInfoEventArgs(FilterAPI.MessageSendData messageSend)
            : base(messageSend)
        {
            if (messageSend.DataBufferLength > 0)
            {
                GCHandle pinnedPacket = GCHandle.Alloc(messageSend.DataBuffer, GCHandleType.Pinned);
                FilterAPI.VOLUME_INFO volumeInfo = (FilterAPI.VOLUME_INFO)Marshal.PtrToStructure(
                    pinnedPacket.AddrOfPinnedObject(), typeof(FilterAPI.VOLUME_INFO));
                pinnedPacket.Free();

                VolumeName = volumeInfo.VolumeName;
                VolumeDosName = volumeInfo.VolumeDosName;
                VolumeFilesystemType = (WinData.FLT_FILESYSTEM_TYPE)volumeInfo.VolumeFilesystemType;
                DeviceCharacteristics = (WinData.DeviceObject_Characteristics)volumeInfo.DeviceCharacteristics;

                FileName = Description;
            }


        }

        /// <summary>
        /// the volume name
        /// </summary>
        public string VolumeName{ get; set;}
        /// <summary>
        /// the volume dos name
        /// </summary>
        public string VolumeDosName {get; set;}
        /// <summary>
        /// the volume file system type.
        /// </summary>
        public WinData.FLT_FILESYSTEM_TYPE VolumeFilesystemType {get;set;}
        /// <summary>
        /// the Characteristics of the attached device object if existed. 
        /// </summary>
        public WinData.DeviceObject_Characteristics DeviceCharacteristics{get;set;}

        public override string  Description
        {
	          get 
              {
		        string desc = "VolumeName[" + VolumeName + "]  ";
                    desc += "VolumeDosName[" + VolumeDosName + "]  ";
                    desc += "VolumeFilesystemType[" + VolumeFilesystemType.ToString() + "]  ";

                    string charaterMessage = string.Empty;
                    foreach (WinData.DeviceObject_Characteristics character in Enum.GetValues(typeof(WinData.DeviceObject_Characteristics)))
                    {
                        if (((uint)DeviceCharacteristics & (uint)character) > 0)
                        {
                            charaterMessage += character.ToString() + ";";
                        }
                    }

                    desc += "DeviceCharacteristics[" + charaterMessage + "]";

                  return desc;
	        }
	        set{	Description = value;}

        }
    }

    #endregion event args definition


   
}
