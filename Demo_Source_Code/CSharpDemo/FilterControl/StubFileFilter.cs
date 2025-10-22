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
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace EaseFilter.FilterControl
{
    partial class FileFilter
    {
        /// <summary>
        /// Fires this event when the stub file requests for read.
        /// </summary>
        public event EventHandler<StubFileEventArgs> OnFilterStubFileRead;

        public bool StubFileHandler(FilterAPI.MessageSendData messageSend, IntPtr replyDataPtr)
        {
            bool retVal = true;

            //if the event was subscribed 
            if (null != OnFilterStubFileRead)
            {
                StubFileEventArgs stubFileArgs = new StubFileEventArgs(messageSend, replyDataPtr);
                OnFilterStubFileRead(this, stubFileArgs);
            }

            return retVal;
        }


    }


    public class StubFileEventArgs : FileIOEventArgs
    {
        public StubFileEventArgs(FilterAPI.MessageSendData messageSend, IntPtr replyDataPtr)
            : base(messageSend)
        {
            if (messageSend.DataBufferLength > 0)
            {
                TagDataLength = messageSend.DataBufferLength;
                TagData = DataBuffer;
            }

            filterCommand = messageSend.FilterCommand;

            if (messageSend.FilterCommand == (uint)FilterAPI.FilterCommand.MESSAGE_TYPE_RESTORE_FILE_TO_CACHE)
            {
                DownloadFullFileToCache = true;
            }
                        
            if(messageSend.FilterCommand == (uint)FilterAPI.FilterCommand.MESSAGE_TYPE_RESTORE_BLOCK_OR_FILE)
            {
                DownloadBockData = true;
            }

            ReadOffset = messageSend.Offset;
            ReadLength = messageSend.Length;

            replyData = replyDataPtr;
            messageReply = (FilterAPI.MessageReplyData)Marshal.PtrToStructure(replyDataPtr, typeof(FilterAPI.MessageReplyData));

            messageReply.MessageId = MessageId;
            messageReply.MessageType = (uint)(FilterAPI.MessageType)messageSend.MessageType;
            messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.CACHE_FILE_WAS_RETURNED;

        }

        /// <summary>
        /// the length of the tag data.
        /// </summary>
        public uint TagDataLength = 0;
        /// <summary>
        /// The tag data of the virtual stub file.
        /// </summary>
        public byte[] TagData = null;
        /// <summary>
        /// The read offset
        /// </summary>
        public long ReadOffset = 0;
        /// <summary>
        /// The length of the Read
        /// </summary>
        public uint ReadLength = 0;

        /// <summary>
        /// If it is true, the filter requests the full cache file to be downloaded for the virtul stub file.
        /// </summary>
        public bool DownloadFullFileToCache = false;
        /// <summary>
        /// If it is true, the filter requests the block data of the stub file.
        /// </summary>
        public bool DownloadBockData = false;

        uint returnBufferLength = 0;
        string returnCacheFileName = string.Empty;
        FilterAPI.FilterStatus filterStatus = FilterAPI.FilterStatus.BLOCK_DATA_WAS_RETURNED;
        NtStatus.Status returnStatus = NtStatus.Status.Success;

        FilterAPI.MessageReplyData messageReply = new FilterAPI.MessageReplyData();
        IntPtr replyData = IntPtr.Zero;

        public uint filterCommand = 0;

        /// <summary>
        /// The length of the return buffer.
        /// </summary>
        public uint ReturnBufferLength
        {
            get { return returnBufferLength; }
            set
            {
                returnBufferLength = value;
                messageReply.DataBufferLength = value;
                Marshal.StructureToPtr(messageReply, replyData, true);
            }
        }
        /// <summary>
        /// The return buffer.
        /// </summary>
        public byte[] ReturnBuffer { get { return messageReply.DataBuffer; } }

        /// <summary>
        /// The return cache file name.
        /// </summary>
        public string ReturnCacheFileName
        {
            get { return returnCacheFileName; }
            set
            {
                returnCacheFileName = value;
                returnBufferLength = (uint)value.Length * 2;
                messageReply.DataBufferLength = (uint)value.Length * 2;
                Array.Copy(Encoding.Unicode.GetBytes(value), messageReply.DataBuffer, messageReply.DataBufferLength);

                Marshal.StructureToPtr(messageReply, replyData, true);
            }
        }

        /// <summary>
        /// the return filter status to tell the filter driver how to handle the return buffer.
        /// </summary>
        public FilterAPI.FilterStatus FilterStatus
        {
            get { return filterStatus; }
            set
            {
                filterStatus = value;
                messageReply.FilterStatus = (uint)value;

                Marshal.StructureToPtr(messageReply, replyData, true);
            }
        }

        /// <summary>
        /// the return status of the I/O request.
        /// </summary>
        public NtStatus.Status ReturnStatusToFilter
        {
            get { return returnStatus; }
            set
            {
                returnStatus = value;
                messageReply.ReturnStatus = (uint)value;

                Marshal.StructureToPtr(messageReply, replyData, true);
            }
        }
    }

    
}
