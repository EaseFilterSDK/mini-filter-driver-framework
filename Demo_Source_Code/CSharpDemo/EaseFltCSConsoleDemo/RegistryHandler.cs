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
using System.Security.Principal;
using System.Threading;

using EaseFilter.FilterControl;
using EaseFilter.CommonObjects;

namespace EaseFltCSConsoleDemo
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
        bool disposed = false;
        public RegistryHandler()
        {
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

            disposed = true;
        }

        ~RegistryHandler()
        {
            Dispose(false);
        }

        public void DisplayEventMessage(RegistryEventArgs registryEventArgs)
        {
            try
            {
                string message = string.Empty;
                message += "RegistryFilter-MessageId:" + registryEventArgs.MessageId.ToString() + "\r\n";
                message += "UserName:" + registryEventArgs.UserName + "\r\n";
                message += "ProcessName:" + registryEventArgs.ProcessName + "  (" + registryEventArgs.ProcessId + ")" + "\r\n";
                message += "ThreadId:" + registryEventArgs.ThreadId.ToString() + "\r\n";
                message += "EventName:" + registryEventArgs.EventName + "\r\n";
                message += "FileName:" + registryEventArgs.FileName + "\r\n";
                message += "IOStatus:" + registryEventArgs.IOStatusToString() + "\r\n";
                message += "Description:" + registryEventArgs.Description + "\r\n";

                if ((uint)registryEventArgs.IoStatus >= (uint)NtStatus.Status.Error)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else if ((uint)registryEventArgs.IoStatus > (uint)NtStatus.Status.Warning)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.WriteLine(message);

            }
            catch (Exception ex)
            {
                Console.WriteLine("DisplayEventMessage failed." + ex.Message);
            }

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
