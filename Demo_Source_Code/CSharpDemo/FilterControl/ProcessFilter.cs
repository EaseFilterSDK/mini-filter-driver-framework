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

namespace EaseFilter.FilterControl
{
    public class ProcessFilter : FileFilter
    {
        private string processNameFilterMask = string.Empty;

        public event EventHandler<ProcessEventArgs> OnProcessCreation;
        public event EventHandler<ProcessEventArgs> OnProcessPreTermination;
        public event EventHandler<ProcessEventArgs> NotifyProcessWasBlocked;
        public event EventHandler<ProcessEventArgs> NotifyProcessTerminated;
        public event EventHandler<ProcessEventArgs> NotifyThreadCreation;
        public event EventHandler<ProcessEventArgs> NotifyThreadTerminated;
        public event EventHandler<ProcessEventArgs> NotifyProcessHandleInfo;
        public event EventHandler<ProcessEventArgs> NotifyThreadHandleInfo;

        Dictionary<string, uint> fileAccessRights = new Dictionary<string, uint>();
        /// <summary>
        /// the process name filter mask, i.e. "notepad.exe","c:\\windows\\*.exe" 
        /// </summary>
        public string ProcessNameFilterMask { get { return processNameFilterMask; } set { processNameFilterMask = value; } }

        /// <summary>
        /// The control flag to the processes which match the processNameFilterMask
        /// </summary>
        public uint ControlFlag { get; set; }

        /// <summary>
        /// Using the process Id to monitor or control the file access rights instead of the process name if it is not 0.
        /// </summary>
        public uint ProcessId { get; set; }

        /// <summary>
        /// The file access rights to the processes,the key is the FileFilterMask, the value is the access flag.
        /// i.e. <c:\myfolder\*,ALLOW_MAX_RIGHT_ACCESS>
        /// </summary>
        public Dictionary<string, uint> FileAccessRights
        {
            get { return fileAccessRights; }
            set { fileAccessRights = value; }
        }

        public ProcessFilter(string processNameFilterMask)
            : base(processNameFilterMask)
        {
            ProcessNameFilterMask = processNameFilterMask;
            this.FilterType = FilterAPI.FilterType.PROCESS_FILTER;
        }

        public override void SendNotification(FilterAPI.MessageSendData messageSend)
        {

            ProcessEventArgs processEventArgs = new ProcessEventArgs(messageSend);
            if (messageSend.FilterCommand == (uint)FilterAPI.FilterCommand.FILTER_SEND_DENIED_PROCESS_CREATION_EVENT)
            {
                if (null != NotifyProcessWasBlocked)
                {
                    processEventArgs.EventName = "NewProcessCreationWasBlocked";
                    NotifyProcessWasBlocked(this, processEventArgs);
                }
            }
            else if (messageSend.FilterCommand == (uint)FilterAPI.FilterCommand.FILTER_SEND_DENIED_PROCESS_TERMINATED_EVENT)
            {
                if (null != NotifyProcessWasBlocked)
                {
                    processEventArgs.EventName = "ProcessTerminationWasBlocked";
                    NotifyProcessWasBlocked(this, processEventArgs);
                }
            }
            else if (messageSend.FilterCommand == (uint)FilterAPI.FilterCommand.FILTER_SEND_PROCESS_TERMINATION_INFO)
            {
                if (null != NotifyProcessTerminated)
                {
                    processEventArgs.EventName = "NotifyProcessTerminated";
                    NotifyProcessTerminated(this, processEventArgs);
                }
            }
            else if (messageSend.FilterCommand == (uint)FilterAPI.FilterCommand.FILTER_SEND_THREAD_CREATION_INFO)
            {
                if (null != NotifyThreadCreation)
                {
                    processEventArgs.EventName = "NotifyThreadCreation";
                    NotifyThreadCreation(this, processEventArgs);
                }
            }
            else if (messageSend.FilterCommand == (uint)FilterAPI.FilterCommand.FILTER_SEND_THREAD_TERMINATION_INFO)
            {
                if (null != NotifyThreadTerminated)
                {
                    processEventArgs.EventName = "NotifyThreadTerminated";
                    NotifyThreadTerminated(this, processEventArgs);
                }
            }
            else if (messageSend.FilterCommand == (uint)FilterAPI.FilterCommand.FILTER_SEND_PROCESS_HANDLE_INFO)
            {
                if (null != NotifyProcessHandleInfo)
                {
                    processEventArgs.EventName = "NotifyProcessHandleInfo";
                    NotifyProcessHandleInfo(this, processEventArgs);
                }
            }
            else if (messageSend.FilterCommand == (uint)(uint)FilterAPI.FilterCommand.FILTER_SEND_THREAD_HANDLE_INFO)
            {
                if (null != NotifyThreadHandleInfo)
                {
                    processEventArgs.EventName = "NotifyThreadHandleInfo";
                    NotifyThreadHandleInfo(this, processEventArgs);
                }
            }
            else
            {
                base.SendNotification(messageSend);
            }

        }

        public override bool FireControlEvents(FilterAPI.MessageSendData messageSend, ref FilterAPI.MessageReplyData messageReply)
        {
            if (messageSend.MessageType == (uint)FilterAPI.FilterCommand.FILTER_SEND_PROCESS_CREATION_INFO)
            {
                if (null != OnProcessCreation)
                {
                    ProcessEventArgs processEventArgs = new ProcessEventArgs(messageSend);
                    processEventArgs.EventName = "OnProcessCreation";

                    OnProcessCreation(this, processEventArgs);

                    //you can block the process creation if return status is not success status.
                    messageReply.ReturnStatus = (uint)processEventArgs.ReturnStatus;

                }
            }
            else if (messageSend.MessageType == (uint)FilterAPI.FilterCommand.FILTER_SEND_PRE_TERMINATE_PROCESS_INFO)
            {
                if (null != OnProcessPreTermination)
                {
                    ProcessEventArgs processEventArgs = new ProcessEventArgs(messageSend);
                    processEventArgs.EventName = "OnProcessPreTermination";

                    OnProcessPreTermination(this, processEventArgs);

                    //you can block the process termination if return status is not success status.
                    messageReply.ReturnStatus = (uint)processEventArgs.ReturnStatus;

                }
            }
            else
            {
                base.FireControlEvents(messageSend, ref messageReply);
            }


            return true;
        }

    }
    public class ProcessEventArgs : FileIOEventArgs
    {
        public ProcessEventArgs(FilterAPI.MessageSendData messageSend)
            : base(messageSend)
        {
            FileName = ImageFileName = messageSend.FileName;

            if (messageSend.DataBufferLength > 0)
            {
                GCHandle pinnedPacket = GCHandle.Alloc(messageSend.DataBuffer, GCHandleType.Pinned);
                FilterAPI.PROCESS_INFO processInfo = (FilterAPI.PROCESS_INFO)Marshal.PtrToStructure(
                    pinnedPacket.AddrOfPinnedObject(), typeof(FilterAPI.PROCESS_INFO));

                ParentProcessId = processInfo.ParentProcessId;
                CreatingProcessId = processInfo.CreatingProcessId;
                CreatingThreadId = processInfo.CreatingThreadId;
                DesiredAccess = processInfo.DesiredAccess;
                Operation = processInfo.Operation;
                FileOpenNameAvailable = processInfo.FileOpenNameAvailable;                
                CommandLine = processInfo.CommandLine;

                switch (messageSend.FilterCommand)
                {
                    case (uint)FilterAPI.FilterCommand.FILTER_SEND_PROCESS_CREATION_INFO:
                        {
                            Description = "ParentPid:" + ParentProcessId + ";CreatingPid:" + CreatingProcessId + ";CreatingThreadId:" + CreatingThreadId
                                + ";FileOpenNameAvailable:" + FileOpenNameAvailable + ";CommandLine:" + CommandLine;

                            break;
                        }
                    case (uint)FilterAPI.FilterCommand.FILTER_SEND_PROCESS_HANDLE_INFO:
                    case (uint)FilterAPI.FilterCommand.FILTER_SEND_THREAD_HANDLE_INFO:
                        {
                            if (processInfo.Operation == 1)
                            {
                                Description = "Create Handle";
                            }
                            else
                            {
                                Description = "Duplicate Handle";
                            }

                            Description += "; DesiredAccess:" + processInfo.DesiredAccess.ToString("X");
                            break;
                        }

                }

                pinnedPacket.Free();
            }
        }

        /// <summary>
        ///The process ID of the parent process for the new process. Note that the parent process is not necessarily the same process as the process that created the new process.  
        /// </summary>
        public uint ParentProcessId { get; set; }
        /// <summary>
        ///  The process ID of the process that created the new process.
        /// </summary>
        public uint CreatingProcessId { get; set; }
        /// <summary>
        /// The thread ID of the thread that created the new process.
        /// </summary>
        public uint CreatingThreadId { get; set; }
        /// <summary>
        ///An ACCESS_MASK value that specifies the access rights to grant for the handle
        /// </summary>
        public uint DesiredAccess { get; set; }
        /// <summary>
        ///The type of handle operation. This member might be one of the following values:OB_OPERATION_HANDLE_CREATE,OB_OPERATION_HANDLE_DUPLICATE
        /// </summary>
        public uint Operation { get; set; }
        /// <summary>
        /// A Boolean value that specifies whether the ImageFileName member contains the exact file name that is used to open the process executable file.
        /// </summary>
        public bool FileOpenNameAvailable { get; set; }
        /// <summary>
        /// The file name of the executable. If the FileOpenNameAvailable member is TRUE, the string specifies the exact file name that is used to open the executable file. 
        /// If FileOpenNameAvailable is FALSE, the operating system might provide only a partial name.
        /// </summary>
        public string ImageFileName { get; set; }
        /// <summary>
        /// The command that is used to execute the process.
        /// </summary>
        public string CommandLine { get; set; }
     
    }


    
}
