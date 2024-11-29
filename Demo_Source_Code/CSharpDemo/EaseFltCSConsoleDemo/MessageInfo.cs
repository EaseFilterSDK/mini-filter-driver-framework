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
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.IO;
using System.Text;
using System.Threading;

using EaseFilter.CommonObjects;

namespace EaseFltCSConsoleDemo
{
    public static class MessageInfo
    {
        static object displayLock = new object();

        public static void DisplayFilterMessage(FilterAPI.MessageSendData messageSend)
        {
            lock (displayLock)
            {
                try
                {
                    string userName = string.Empty;
                    string strSid = string.Empty;

                    IntPtr sidBuffer = Marshal.UnsafeAddrOfPinnedArrayElement(messageSend.Sid, 0);
                    ConvertSidToUserNameAndStringSid(sidBuffer, out userName, out strSid);

                    string processName = string.Empty;

                    try
                    {
                        System.Diagnostics.Process requestProcess = System.Diagnostics.Process.GetProcessById((int)messageSend.ProcessId);
                        processName = requestProcess.ProcessName;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Convert process id to process name failed." + ex.Message);
                    }

                    Console.WriteLine(string.Format("\r\n\r\nId#{0}", messageSend.MessageId.ToString()));
                    Console.WriteLine(string.Format("TransactionTime:{0}", FormatDateTime(messageSend.TransactionTime)));
                    Console.WriteLine(string.Format("UserName:{0}", userName));
                    Console.WriteLine(string.Format("ProcessName:{0}", processName));
                    Console.WriteLine(string.Format("ProcessId:{0}", messageSend.ProcessId.ToString()));
                    Console.WriteLine(string.Format("ThreadId:{0}", messageSend.ThreadId.ToString()));
                    Console.WriteLine(string.Format("FileObject:{0}", messageSend.FileObject.ToString("X")));
                    Console.WriteLine(string.Format("FsContext:{0}", messageSend.FsContext.ToString("X")));
                    Console.WriteLine(string.Format("MessageType:{0}", ((FilterAPI.MessageType)messageSend.MessageType).ToString()));
                    Console.WriteLine(string.Format("FileName:{0}", messageSend.FileName));
                    Console.WriteLine(string.Format("FileSize:{0}", messageSend.FileSize.ToString()));
                    Console.WriteLine(string.Format("FileAttributes:{0}", ((FileAttributes)messageSend.FileAttributes).ToString()));
                    Console.WriteLine(string.Format("CreationTime:{0}", FormatDateTime(messageSend.CreationTime)));
                    Console.WriteLine(string.Format("LastWriteTime:{0}", FormatDateTime(messageSend.LastWriteTime)));

                    if (((FilterAPI.MessageType)messageSend.MessageType == FilterAPI.MessageType.PRE_CREATE)
                        || ((FilterAPI.MessageType)messageSend.MessageType == FilterAPI.MessageType.POST_CREATE))
                    {
                        Console.WriteLine("---------------Start create I/O related information ---------------");
                        Console.WriteLine(string.Format("DesiredAccess:(0x{0:x}){1}", messageSend.DesiredAccess, FormatDesiredAccess(messageSend.DesiredAccess)));
                        Console.WriteLine(string.Format("Disposition:(0x{0:x}){1}", messageSend.Disposition, ((WinData.Disposition)messageSend.Disposition).ToString()));
                        Console.WriteLine(string.Format("SharedAccess:{0}", ((WinData.ShareAccess)messageSend.SharedAccess).ToString()));
                        Console.WriteLine(string.Format("CreateOptions:(0x{0:x}){1}", messageSend.CreateOptions, FormatCreateOptions(messageSend.CreateOptions)));

                        if (messageSend.Status == (uint)NtStatus.Status.Success
                              && ((FilterAPI.MessageType)messageSend.MessageType == FilterAPI.MessageType.POST_CREATE))
                        {
                            //the create status is meaningful only when the status is succeeded.
                            Console.WriteLine(string.Format("CreateStatus:{0}", ((WinData.CreateStatus)messageSend.CreateStatus).ToString()));
                        }

                        Console.WriteLine("---------------End create I/O related information ---------------");
                    }

                    Console.WriteLine(string.Format("I/O return status:{0}", FormatStatus(messageSend.Status)));

                    if (messageSend.InfoClass > 0)
                    {
                        Console.WriteLine(string.Format("InfoClass:{0}", FormatInfoClass((FilterAPI.MessageType)messageSend.MessageType, messageSend.InfoClass)));
                    }

                    if (messageSend.Length > 0)
                    {
                        Console.WriteLine(string.Format("Read/Write Offset:{0}", messageSend.Offset.ToString()));
                        Console.WriteLine(string.Format("Read/Write Length:{0}", messageSend.Length.ToString()));
                    }

                    if (messageSend.DataBufferLength > 0)
                    {
                        Console.WriteLine(string.Format("Data Buffer Length:{0}", messageSend.DataBufferLength.ToString()));
                        Console.WriteLine(string.Format("Data:{0}", FormatDataBuffer((FilterAPI.MessageType)messageSend.MessageType,
                                                            (WinData.FileInfomationClass)messageSend.InfoClass,
                                                            messageSend.DataBufferLength,
                                                            messageSend.DataBuffer)));
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("DisplayFilterMessage failed." + ex.Message);
                }
            }

            return ;
        }


        static bool ConvertSidToUserNameAndStringSid(IntPtr sidBuffer, out string userName, out string userSid)
        {
            bool ret = true;

            IntPtr sidStringPtr = IntPtr.Zero;
            string sidString = string.Empty;

            userName = string.Empty;
            userSid = string.Empty;

            try
            {
                if (FilterAPI.ConvertSidToStringSid(sidBuffer, out sidStringPtr))
                {
                    sidString = Marshal.PtrToStringAuto(sidStringPtr);
                    SecurityIdentifier secIdentifier = new SecurityIdentifier(sidString);
                    IdentityReference reference = secIdentifier.Translate(typeof(NTAccount));
                    userName = reference.Value;
                    userSid = secIdentifier.ToString();
                }
                else
                {
                    string errorMessage = "Convert sid to sid string failed, error code:" + Marshal.GetLastWin32Error();
                    Console.WriteLine(errorMessage);
                }
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Convert sid to user name got exception:{0}", ex.Message);
                Console.WriteLine(errorMessage);
                userName = errorMessage;
                ret = false;

            }
            finally
            {
                if (sidStringPtr != null && sidStringPtr != IntPtr.Zero)
                {
                    IntPtr res = FilterAPI.LocalFree(sidStringPtr);
                }
            }

            return ret;
        }

        static string FormatDesiredAccess(uint desiredAccess)
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

        static string FormatCreateOptions(uint createOptions)
        {
            string ret = string.Empty;

            foreach (WinData.CreateOptions option in Enum.GetValues(typeof(WinData.CreateOptions)))
            {
                if (option == (WinData.CreateOptions)((uint)option & createOptions))
                {
                    ret += option.ToString() + "|";
                }
            }

            if (string.IsNullOrEmpty(ret))
            {
                ret = "(0x)" + createOptions.ToString("X");
            }

            return ret;
        }

        static string FormatDateTime(long lDateTime)
        {
            try
            {
                if (0 == lDateTime)
                {
                    return "0";
                }

                DateTime dateTime = DateTime.FromFileTime(lDateTime);
                string ret = dateTime.ToShortDateString() + " " + dateTime.ToShortTimeString();
                return ret;
            }
            catch (Exception ex)
            {
                Console.WriteLine("FormatDateTime :" + lDateTime.ToString() + " failed." + ex.Message);
                return ex.Message;
            }
        }

        static string FormatDataBuffer(FilterAPI.MessageType messageType, WinData.FileInfomationClass infoClass, uint dataLength, byte[] data)
        {
            string ret = string.Empty;

            try
            {
                switch (messageType)
                {
                    case FilterAPI.MessageType.PRE_SET_INFORMATION:
                    case FilterAPI.MessageType.POST_SET_INFORMATION:
                    case FilterAPI.MessageType.PRE_QUERY_INFORMATION:
                    case FilterAPI.MessageType.POST_QUERY_INFORMATION:
                        switch (infoClass)
                        {
                            case WinData.FileInfomationClass.FileRenameInformation:
                                {
                                    //destination name
                                    ret = "Destination name:" + Encoding.Unicode.GetString(data);
                                    break;
                                }
                            case WinData.FileInfomationClass.FileDispositionInformation:
                                {
                                    ret = "Delete file.";
                                    break;
                                }
                            case WinData.FileInfomationClass.FileEndOfFileInformation:
                                {
                                    long newFileSize = BitConverter.ToInt64(data, 0);
                                    ret = "Change file size to:" + newFileSize.ToString();
                                    break;
                                }
                            case WinData.FileInfomationClass.FileBasicInformation:
                                {
                                    WinData.FileBasicInformation basiInfo = new WinData.FileBasicInformation();
                                    GCHandle pinnedPacket = GCHandle.Alloc(data, GCHandleType.Pinned);
                                    basiInfo = (WinData.FileBasicInformation)Marshal.PtrToStructure(
                                        pinnedPacket.AddrOfPinnedObject(), typeof(WinData.FileBasicInformation));
                                    pinnedPacket.Free();

                                    ret = "creation time:" + FormatDateTime(basiInfo.CreationTime) + "  ";
                                    ret += "last access time:" + FormatDateTime(basiInfo.LastAccessTime) + "   ";
                                    ret += "last write time:" + FormatDateTime(basiInfo.LastWriteTime) + "   ";
                                    ret += "file attributes:" + ((FileAttributes)basiInfo.FileAttributes).ToString();
                                    break;
                                }

                            case WinData.FileInfomationClass.FileStandardInformation:
                                {
                                    WinData.FileStandardInformation standardInfo = new WinData.FileStandardInformation();
                                    GCHandle pinnedPacket = GCHandle.Alloc(data, GCHandleType.Pinned);
                                    standardInfo = (WinData.FileStandardInformation)Marshal.PtrToStructure(
                                        pinnedPacket.AddrOfPinnedObject(), typeof(WinData.FileStandardInformation));
                                    pinnedPacket.Free();

                                    ret = "File size:" + standardInfo.EndOfFile.ToString() + "   ";
                                    ret += "Allocation size:" + standardInfo.AllocationSize.ToString() + "   ";
                                    ret += "IsDirectory:" + standardInfo.Directory.ToString();
                                    break;
                                }
                            case WinData.FileInfomationClass.FileNetworkOpenInformation:
                                {
                                    WinData.FileNetworkInformation networkInfo = new WinData.FileNetworkInformation();
                                    GCHandle pinnedPacket = GCHandle.Alloc(data, GCHandleType.Pinned);
                                    networkInfo = (WinData.FileNetworkInformation)Marshal.PtrToStructure(
                                        pinnedPacket.AddrOfPinnedObject(), typeof(WinData.FileNetworkInformation));
                                    pinnedPacket.Free();

                                    ret = "creation time:" + FormatDateTime(networkInfo.CreationTime) + "   ";
                                    ret += "last access time:" + FormatDateTime(networkInfo.LastAccessTime) + "   ";
                                    ret += "last write time:" + FormatDateTime(networkInfo.LastWriteTime) + "   ";
                                    ret += "file size:" + networkInfo.FileSize.ToString() + "   ";
                                    ret += "file attributes:" + ((FileAttributes)networkInfo.FileAttributes).ToString();
                                    break;
                                }

                            case WinData.FileInfomationClass.FileInternalInformation:
                                {
                                    long fileId = BitConverter.ToInt64(data, 0);
                                    ret = "FileId: (0x)" + fileId.ToString("X");
                                    break;
                                }
                        }

                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Format data failed." + ex.Message);
                ret = ex.Message;
            }

            return ret;
        }

        static string FormatStatus(uint status)
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

        static string FormatInfoClass(FilterAPI.MessageType messageType, uint infoClass)
        {
            string ret = string.Empty;

            if (FilterAPI.MessageType.PRE_QUERY_SECURITY == messageType
                || FilterAPI.MessageType.PRE_SET_SECURITY == messageType
                || FilterAPI.MessageType.POST_QUERY_SECURITY == messageType
                || FilterAPI.MessageType.POST_SET_SECURITY == messageType)
            {
                ret = ((WinData.SecurityInformation)(infoClass)).ToString();
            }
            else if (infoClass > 0)
            {
                ret = ((WinData.FileInfomationClass)infoClass).ToString();
            }
            else
            {
                ret = string.Empty;
            }

            return ret;
        }


    }
}