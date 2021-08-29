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
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

using EaseFilter.FilterControl;

namespace  EaseFilter.CommonObjects
{
    public class FilterService
    {
        public static bool AuthorizeFileAccess(FilterAPI.MessageSendData messageSend, ref FilterAPI.MessageReplyData messageReply)
        {
            bool ret = true;

            try
            {
                messageReply.MessageId = messageSend.FilterRuleId;
                messageReply.MessageType = messageSend.MessageType;
                messageReply.ReturnStatus = (uint)NtStatus.Status.Success;

                messageReply.MessageId = messageSend.FilterRuleId;
                messageReply.MessageType = messageSend.MessageType;

                //
                //here you can control all the registered IO requests,block the access or modify the I/O data base on the file IO information from MessageSend struture
                //
                //

                //if you don't want to change anything to this IO request, just let it pass through as below setting:
                //messageReply.FilterStatus = 0;
                //messageReply.ReturnStatus = (uint)NtStatus.Status.Success;

                //if you want to block the access this IO request before it goes down to the file system, you can return the status as below,
                //it is only for pre IO requests, it means the user IO reuqests will be completed here instead of going down to the file system.
                //messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                //messageReply.ReturnStatus = (uint)NtStatus.Status.AccessDenied;

                //if you want to modify the IO data and complete the pre IO with your own data, you can return status as below:
                // messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION | (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED;
                // messageReply.DataBufferLength = the return data buffer length.
                // messageReply.DataBuffer = the data you want to return.
                // messageReply.ReturnStatus = (uint)NtStatus.Status.Success;

                FilterAPI.MessageType messageType = (FilterAPI.MessageType)messageSend.MessageType;
                WinData.FileInfomationClass infoClass = (WinData.FileInfomationClass)messageSend.InfoClass;

                uint dataLength = messageSend.DataBufferLength;
                byte[] data = messageSend.DataBuffer;

                //here is some IO information for your reference:
                if ((messageSend.CreateOptions & (uint)WinData.CreateOptions.FO_REMOTE_ORIGIN) > 0)
                {
                    //this is file access request comes from remote network server
                }

                //you can check the file create option with this data:
               //"DesiredAccess: messageSend.DesiredAccess 
               //"Disposition:" + ((WinData.Disposition)messageSend.Disposition).ToString();
               //"ShareAccess:" + ((WinData.ShareAccess)messageSend.SharedAccess).ToString();
               //"CreateOptions:"messageSend.CreateOptions


                //Here is the demo to copy file content before it was deleted.-----------------------------------------------
                bool isFileDeleting = false;
                if (messageSend.Status == (uint)NtStatus.Status.Success)
                {
                    if (messageType == FilterAPI.MessageType.POST_CREATE)
                    {
                        if ((messageSend.CreateOptions & (uint)WinData.CreateOptions.FILE_DELETE_ON_CLOSE) > 0)
                        {
                            isFileDeleting = true;
                        }
                    }
                    else if (messageType == FilterAPI.MessageType.PRE_SET_INFORMATION)
                    {
                        if (infoClass == WinData.FileInfomationClass.FileDispositionInformation)
                        {
                            isFileDeleting = true;
                        }
                    }

                    if (isFileDeleting)
                    {

                        //IntPtr fileHandle = IntPtr.Zero;
                        //bool retVal = FilterAPI.GetFileHandleInFilter(messageSend.FileName,(uint)FileAccess.Read, ref fileHandle);                        
                        //if (retVal)
                        //{
                        //    SafeFileHandle sHandle = new SafeFileHandle(fileHandle, true);
                        //    FileStream fileStream = new FileStream(sHandle, FileAccess.Read);

                        //    //copy your data here...

                        //    fileStream.Close();
                        //}

                    }
                }
                //End -----------------------------------------------
                            


                switch (messageType)
                {
                    case FilterAPI.MessageType.PRE_CREATE:
                        {
                           
                            //here you reparse the file open to another new file name 

                            //string newReparseFileName = "\\??\\c:\\myNewFile.txt";
                            //byte[] returnData = Encoding.Unicode.GetBytes(newReparseFileName);
                            //Array.Copy(returnData, messageReply.DataBuffer, returnData.Length);
                            //messageReply.DataBufferLength = (uint)returnData.Length;
                            //messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                            //messageReply.ReturnStatus = (uint)NtStatus.Status.Reparse;                            

                            break;
                        }


                    case FilterAPI.MessageType.PRE_CACHE_READ:
                    case FilterAPI.MessageType.POST_CACHE_READ:
                    case FilterAPI.MessageType.PRE_NOCACHE_READ:
                    case FilterAPI.MessageType.POST_NOCACHE_READ:
                    case FilterAPI.MessageType.PRE_PAGING_IO_READ:
                    case FilterAPI.MessageType.POST_PAGING_IO_READ:
                    case FilterAPI.MessageType.PRE_CACHE_WRITE:
                    case FilterAPI.MessageType.POST_CACHE_WRITE:
                    case FilterAPI.MessageType.PRE_NOCACHE_WRITE:
                    case FilterAPI.MessageType.POST_NOCACHE_WRITE:
                    case FilterAPI.MessageType.PRE_PAGING_IO_WRITE:
                    case FilterAPI.MessageType.POST_PAGING_IO_WRITE:
                        {


                            //byte[] returnData = //new data you want to modify the read/write data;
                            //Array.Copy(returnData, messageReply.DataBuffer, returnData.Length);
                            //messageReply.DataBufferLength = (uint)returnData.Length;

                            ////for pre IO,use this one
                            //// messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION | (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED;

                            // messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED;
                            // messageReply.ReturnStatus = (uint)NtStatus.Status.Success;


                            break;
                        }
                    case FilterAPI.MessageType.PRE_SET_INFORMATION:
                    case FilterAPI.MessageType.POST_SET_INFORMATION:
                    case FilterAPI.MessageType.PRE_QUERY_INFORMATION:
                    case FilterAPI.MessageType.POST_QUERY_INFORMATION:
                        {
                            switch (infoClass)
                            {
                                case WinData.FileInfomationClass.FileRenameInformation:
                                    {
                                        if (FilterAPI.MessageType.PRE_SET_INFORMATION == messageType)
                                        {
                                            string blockFileName = @"c:\filterTest\blockRename.txt";
                                            //test block rename to blockFileName, it needs to register PRE_SET_INFORMATION;
                                            if (string.Compare(messageSend.FileName, blockFileName, true) == 0)
                                            {                                                
                                                EventManager.WriteMessage(179, "IOAccessControl", EventLevel.Warning, "Block rename for file:" + messageSend.FileName);
                                                ret = false;
                                                break;
                                            }
                                        }

                                        //you can block file rename as below
                                        //messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                                        //messageReply.ReturnStatus = (uint)NtStatus.Status.AccessDenied;
                                        break;
                                    }
                                case WinData.FileInfomationClass.FileDispositionInformation:
                                    {
                                        //you can block file delete as below
                                        //messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                                        //messageReply.ReturnStatus = (uint)NtStatus.Status.AccessDenied;
                                        break;
                                    }
                                case WinData.FileInfomationClass.FileEndOfFileInformation:
                                    {
                                       //change file size
                                        break;
                                    }
                                case WinData.FileInfomationClass.FileBasicInformation:
                                    {
                                        //file basic information
                                        break;
                                    }

                                case WinData.FileInfomationClass.FileStandardInformation:
                                    {
                                       //file standard information
                                        break;
                                    }
                                case WinData.FileInfomationClass.FileNetworkOpenInformation:
                                    {
                                       //file network information
                                        break;
                                    }

                                case WinData.FileInfomationClass.FileInternalInformation:
                                    {
                                        //file internal inofrmation
                                        break;
                                    }
                                default:
                                    {
                                        break;
                                    }
                            }

                            break;
                        }
                }
             
               
            }
            catch (Exception ex)
            {
                EventManager.WriteMessage(174, "IOAccessControl", EventLevel.Error, "IOAccessControl failed." + ex.Message);
            }

            return ret;
        }
       
    }
}
