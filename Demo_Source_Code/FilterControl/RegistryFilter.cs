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
using System.Text;
using System.IO;

namespace EaseFilter.FilterControl
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
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 65536 - 12)]
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
        REG_OPENED_EXISTING_KEY = 0x00000002, // Existing Key opened
    }

    public class RegistryEventArgs : FileIOEventArgs
    {
        private static string ValueTypeData(VALUE_DATA_TYPE type, int dataSize, byte[] data)
        {
            string dataStr = string.Empty;

            try
            {
                switch (type)
                {
                    case VALUE_DATA_TYPE.REG_BINARY:
                        {
                            //maximum data size to 256;
                            for (int i = 0; i < 256; i++)
                            {
                                if (i >= dataSize)
                                {
                                    break;
                                }

                                dataStr += string.Format("0x{0:x2}", data[i]);

                            }

                            break;
                        }
                    case VALUE_DATA_TYPE.REG_DWORD:
                        {
                            uint value = BitConverter.ToUInt32(data, 0);
                            dataStr = string.Format("0x{0:x8}({1})", value, value);
                            break;
                        }
                    case VALUE_DATA_TYPE.REG_DWORD_BIG_ENDIAN:
                        {
                            //A 4-byte numerical value whose least significant byte is at the highest address
                            byte leastByte = data[3];
                            byte secondByte = data[2];
                            data[3] = data[0];
                            data[2] = data[1];
                            data[1] = secondByte;
                            data[0] = leastByte;

                            uint value = BitConverter.ToUInt32(data, 0);
                            dataStr = string.Format("0x{0:x8}({1})", value, value);

                            break;
                        }
                    case VALUE_DATA_TYPE.REG_EXPAND_SZ:
                        {
                            //A null-terminated Unicode string, containing unexpanded references to environment variables, such as "%PATH%"
                            dataStr = Encoding.Unicode.GetString(data, 0, dataSize).Replace("\0", "");
                            break;
                        }
                    case VALUE_DATA_TYPE.REG_MULTI_SZ:
                        {
                            //An array of null-terminated strings, terminated by another zero
                            dataStr = Encoding.Unicode.GetString(data, 0, dataSize).Replace("\0", "");
                            break;
                        }
                    case VALUE_DATA_TYPE.REG_SZ:
                        {
                            //A null-terminated Unicode string
                            dataStr = Encoding.Unicode.GetString(data, 0, dataSize).Replace("\0", "");
                            break;
                        }
                    case VALUE_DATA_TYPE.REG_QWORD:
                        {
                            UInt64 value = BitConverter.ToUInt64(data, 0);
                            dataStr = string.Format("0x{0:x16}({1})", value, value);
                            break;
                        }

                    default: break;
                }
            }
            catch (Exception ex)
            {
                dataStr = "get data failed:" + ex.Message;
            }


            return dataStr;
        }

        private static string KeyInformation(KEY_INFORMATION_CLASS keyInfoClass, byte[] keyInformation)
        {
            string keyInfoStr = "(" + keyInfoClass.ToString() + ") ";

            try
            {
                MemoryStream ms = new MemoryStream(keyInformation);
                BinaryReader br = new BinaryReader(ms);

                switch (keyInfoClass)
                {
                    case KEY_INFORMATION_CLASS.KeyBasicInformation:
                        {
                            long lastWriteTime = br.ReadInt64();
                            uint titleIndex = br.ReadUInt32();
                            uint nameLength = br.ReadUInt32();
                            string name = Encoding.Unicode.GetString(keyInformation, (int)ms.Position, (int)nameLength);

                            keyInfoStr += "LastWriteTime:" + DateTime.FromFileTime(lastWriteTime).ToShortDateString();
                            keyInfoStr += " Name:" + name;

                            break;
                        }
                    case KEY_INFORMATION_CLASS.KeyNodeInformation:
                        {
                            long lastWriteTime = br.ReadInt64();
                            uint titleIndex = br.ReadUInt32();
                            uint classOffset = br.ReadUInt32();
                            uint classLength = br.ReadUInt32();
                            uint nameLength = br.ReadUInt32();
                            string name = Encoding.Unicode.GetString(keyInformation, (int)ms.Position, (int)nameLength);
                            string className = Encoding.Unicode.GetString(keyInformation, (int)classOffset, (int)classLength);

                            keyInfoStr += "LastWriteTime:" + DateTime.FromFileTime(lastWriteTime).ToShortDateString();
                            keyInfoStr += " Name:" + name + " ClassName:" + className;

                            break;
                        }
                    case KEY_INFORMATION_CLASS.KeyFullInformation:
                        {
                            long lastWriteTime = br.ReadInt64();
                            uint titleIndex = br.ReadUInt32();
                            uint classOffset = br.ReadUInt32();
                            uint classLength = br.ReadUInt32();
                            uint subKeys = br.ReadUInt32();
                            uint maxNameLen = br.ReadUInt32();
                            uint maxClassLen = br.ReadUInt32();
                            uint values = br.ReadUInt32();
                            uint maxValueNameLen = br.ReadUInt32();
                            uint maxValueDataLen = br.ReadUInt32();
                            uint nameLength = br.ReadUInt32();
                            string className = Encoding.Unicode.GetString(keyInformation, (int)classOffset, (int)classLength);

                            keyInfoStr += "LastWriteTime:" + DateTime.FromFileTime(lastWriteTime).ToShortDateString();
                            keyInfoStr += " subKeys:" + subKeys + " valueEntries:" + values + " ClassName:" + className;

                            break;
                        }
                    case KEY_INFORMATION_CLASS.KeyNameInformation:
                        {
                            uint nameLength = br.ReadUInt32();
                            string name = Encoding.Unicode.GetString(keyInformation, (int)ms.Position, (int)nameLength);
                            keyInfoStr += " Name:" + name;

                            break;
                        }


                    default: break;
                }
            }
            catch (Exception ex)
            {
                keyInfoStr = "get data failed:" + ex.Message;
            }


            return keyInfoStr;
        }

        private static string KeyValueInformation(KEY_VALUE_INFORMATION_CLASS keyValueInfoClass, byte[] keyValueInformation)
        {
            string keyValueInfoStr = string.Empty;

            try
            {
                MemoryStream ms = new MemoryStream(keyValueInformation);
                BinaryReader br = new BinaryReader(ms);

                switch (keyValueInfoClass)
                {
                    case KEY_VALUE_INFORMATION_CLASS.KeyValueBasicInformation:
                        {
                            int titleIndex = br.ReadInt32();
                            int type = br.ReadInt32();
                            int nameLength = br.ReadInt32();
                            keyValueInfoStr = "(" + keyValueInfoClass.ToString() + ") Name:" + Encoding.Unicode.GetString(keyValueInformation, (int)ms.Position, nameLength);

                            break;
                        }
                    case KEY_VALUE_INFORMATION_CLASS.KeyValueFullInformation:
                        {
                            int titleIndex = br.ReadInt32();
                            int type = br.ReadInt32();
                            int dataOffset = br.ReadInt32();
                            int dataLength = br.ReadInt32();
                            int nameLength = br.ReadInt32();
                            keyValueInfoStr = "(" + keyValueInfoClass.ToString() + ") Name:" + Encoding.Unicode.GetString(keyValueInformation, (int)ms.Position, nameLength);
                            keyValueInfoStr += " Type:" + ((VALUE_DATA_TYPE)type).ToString();

                            byte[] dataBuffer = new byte[dataLength];
                            Array.Copy(keyValueInformation, dataOffset, dataBuffer, 0, dataBuffer.Length);

                            keyValueInfoStr += " Data:" + ValueTypeData((VALUE_DATA_TYPE)type, dataBuffer.Length, dataBuffer);

                            break;
                        }
                    case KEY_VALUE_INFORMATION_CLASS.KeyValuePartialInformation:
                        {
                            int titleIndex = br.ReadInt32();
                            int type = br.ReadInt32();
                            int dataLength = br.ReadInt32();
                            keyValueInfoStr += "(" + keyValueInfoClass.ToString() + ") Type:" + ((VALUE_DATA_TYPE)type).ToString();

                            byte[] dataBuffer = new byte[dataLength];
                            Array.Copy(keyValueInformation, ms.Position, dataBuffer, 0, dataBuffer.Length);
                            keyValueInfoStr += " Data:" + ValueTypeData((VALUE_DATA_TYPE)type, dataBuffer.Length, dataBuffer);

                            break;
                        }


                    default: break;
                }
            }
            catch (Exception ex)
            {
                keyValueInfoStr = "get data failed:" + ex.Message;
            }


            return keyValueInfoStr;
        }

        public RegistryEventArgs(FilterAPI.MessageSendData messageSend)
            : base(messageSend)
        {
            RegCallbackClass = (FilterAPI.RegCallbackClass)messageSend.MessageType;
            InfoClass = messageSend.InfoClass;

            if (messageSend.Status == (uint)NtStatus.Status.Success)
            {
                switch (RegCallbackClass)
                {
                    case FilterAPI.RegCallbackClass.Reg_Pre_Create_KeyEx:
                    case FilterAPI.RegCallbackClass.Reg_Post_Create_KeyEx:
                    case FilterAPI.RegCallbackClass.Reg_Pre_Open_KeyEx:
                    case FilterAPI.RegCallbackClass.Reg_Post_Open_KeyEx:
                        {
                            description = CreateOptions.ToString();
                            break;
                        }
                    case FilterAPI.RegCallbackClass.Reg_Pre_Delete_Key:
                    case FilterAPI.RegCallbackClass.Reg_Post_Delete_Key:
                        {
                            description = "registry key is being deleted.";
                            break;
                        }
                    case FilterAPI.RegCallbackClass.Reg_Pre_Set_Value_Key:
                    case FilterAPI.RegCallbackClass.Reg_Post_Set_Value_Key:
                        {
                            VALUE_DATA_TYPE valueType = (VALUE_DATA_TYPE)messageSend.InfoClass;
                            description = "Type:" + valueType.ToString();
                            description += " Data:" + ValueTypeData(valueType, (int)messageSend.DataBufferLength, messageSend.DataBuffer);
                            break;
                        }
                    case FilterAPI.RegCallbackClass.Reg_Pre_Delete_Value_Key:
                    case FilterAPI.RegCallbackClass.Reg_Post_Delete_Value_Key:
                        {
                            description = "registry key's value is being deleted.";
                            break;
                        }
                    case FilterAPI.RegCallbackClass.Reg_Pre_SetInformation_Key:
                    case FilterAPI.RegCallbackClass.Reg_Post_SetInformation_Key:
                        {
                            KEY_SET_INFORMATION_CLASS keySetInformationClass = (KEY_SET_INFORMATION_CLASS)messageSend.InfoClass;
                            description = keySetInformationClass.ToString();
                            break;
                        }
                    case FilterAPI.RegCallbackClass.Reg_Pre_Rename_Key:
                    case FilterAPI.RegCallbackClass.Reg_Post_Rename_Key:
                        {
                            string newName = Encoding.Unicode.GetString(messageSend.DataBuffer);
                            description = "registry key's name is being changed to " + newName;
                            break;
                        }
                    case FilterAPI.RegCallbackClass.Reg_Pre_Enumerate_Key:
                        {
                            KEY_INFORMATION_CLASS keyInformationClass = (KEY_INFORMATION_CLASS)messageSend.InfoClass;
                            description = keyInformationClass.ToString();

                            break;
                        }
                    case FilterAPI.RegCallbackClass.Reg_Post_Enumerate_Key:
                        {
                            KEY_INFORMATION_CLASS keyInformationClass = (KEY_INFORMATION_CLASS)messageSend.InfoClass;
                            description += KeyInformation(keyInformationClass, messageSend.DataBuffer);

                            break;
                        }
                    case FilterAPI.RegCallbackClass.Reg_Pre_Enumerate_Value_Key:
                        {
                            KEY_VALUE_INFORMATION_CLASS keyValuseInformationClass = (KEY_VALUE_INFORMATION_CLASS)messageSend.InfoClass;
                            description = keyValuseInformationClass.ToString();

                            break;
                        }
                    case FilterAPI.RegCallbackClass.Reg_Post_Enumerate_Value_Key:
                        {
                            KEY_VALUE_INFORMATION_CLASS keyValuseInformationClass = (KEY_VALUE_INFORMATION_CLASS)messageSend.InfoClass;
                            description += KeyValueInformation(keyValuseInformationClass, messageSend.DataBuffer);

                            break;
                        }
                    case FilterAPI.RegCallbackClass.Reg_Pre_Query_Key:
                        {
                            KEY_INFORMATION_CLASS keyInformationClass = (KEY_INFORMATION_CLASS)messageSend.InfoClass;
                            description = keyInformationClass.ToString();

                            break;
                        }
                    case FilterAPI.RegCallbackClass.Reg_Post_Query_Key:
                        {
                            KEY_INFORMATION_CLASS keyInformationClass = (KEY_INFORMATION_CLASS)messageSend.InfoClass;
                            description += KeyInformation(keyInformationClass, messageSend.DataBuffer);

                            break;
                        }
                    case FilterAPI.RegCallbackClass.Reg_Pre_Query_Value_Key:
                        {
                            KEY_VALUE_INFORMATION_CLASS keyValuseInformationClass = (KEY_VALUE_INFORMATION_CLASS)messageSend.InfoClass;
                            description = keyValuseInformationClass.ToString();

                            break;
                        }
                    case FilterAPI.RegCallbackClass.Reg_Post_Query_Value_Key:
                        {
                            KEY_VALUE_INFORMATION_CLASS keyValuseInformationClass = (KEY_VALUE_INFORMATION_CLASS)messageSend.InfoClass;
                            description += KeyValueInformation(keyValuseInformationClass, messageSend.DataBuffer);

                            break;
                        }
                    case FilterAPI.RegCallbackClass.Reg_Pre_Query_Multiple_Value_Key:
                        {
                            break;
                        }
                    case FilterAPI.RegCallbackClass.Reg_Post_Query_Multiple_Value_Key:
                        {
                            uint entryCount = messageSend.InfoClass;

                            MemoryStream ms = new MemoryStream(messageSend.DataBuffer);
                            BinaryReader br = new BinaryReader(ms);

                            for (int i = 0; i < entryCount && ms.Position < ms.Length; i++)
                            {
                                long currentOffset = ms.Position;
                                int nextEntryOffset = br.ReadInt32();
                                int valueNameLength = br.ReadInt32();
                                int dataType = br.ReadInt32();
                                int dataLength = br.ReadInt32();
                                byte[] valueName = br.ReadBytes(valueNameLength);
                                byte[] data = br.ReadBytes(dataLength);

                                VALUE_DATA_TYPE type = (VALUE_DATA_TYPE)dataType;
                                description += "Name:" + Encoding.Unicode.GetString(valueName, 0, valueNameLength);
                                description += " Type:" + type.ToString();
                                description += " Data:" + ValueTypeData(type, dataLength, data) + Environment.NewLine;

                                ms.Position = currentOffset + nextEntryOffset;

                            }

                            break;
                        }
                  
                    case FilterAPI.RegCallbackClass.Reg_Pre_Load_Key:
                    case FilterAPI.RegCallbackClass.Reg_Post_Load_Key:
                        {
                            description += "SourceFile:" + Encoding.Unicode.GetString(messageSend.DataBuffer, 0, (int)messageSend.DataBufferLength);
                            break;
                        }
                    case FilterAPI.RegCallbackClass.Reg_Pre_Replace_Key:
                    case FilterAPI.RegCallbackClass.Reg_Post_Replace_Key:
                        {
                            description += "NewFileName:" + Encoding.Unicode.GetString(messageSend.DataBuffer, 0, (int)messageSend.DataBufferLength);
                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Pre_Query_KeyName:
                    case FilterAPI.RegCallbackClass.Reg_Post_Query_KeyName:
                        {
                            break;
                        }
                }
            }

        }

        /// <summary>
        ///The registry callback class.  
        /// </summary>
        public FilterAPI.RegCallbackClass RegCallbackClass { get; set; }
        /// <summary>
        ///  The process ID of the process that created the new process.
        /// </summary>
        public uint InfoClass { get; set; }

        private string description = string.Empty;

        /// <summary>
        /// The description of the IO args
        /// </summary>
        public override string Description
        {
            get { return description; }
        }

        /// <summary>
        /// The data buffer which will return back to the driver.
        /// </summary>
        public byte[] ReturnDataBuffer { get; set; }

    }

    public class RegistryFilter:Filter
    {

        private string processNameFilterMask = string.Empty;
        private string userName = string.Empty;
        private string registryKeyNameFilterMask = string.Empty;

        /// <summary>
        /// Control the registry access for the process with this process Id. 
        /// </summary>
        public uint ProcessId{ get; set;}

        /// <summary>
        /// Control the registry access for the process with this process name if the process Id is 0, or it will skip it. 
        /// </summary>
        public string ProcessNameFilterMask { get { return processNameFilterMask; } set { processNameFilterMask = value; } }

        /// <summary>
        /// Control the registry access for the process with this user name
        /// </summary>
        public string UserName { get { return userName; } set { userName = value; } }

        /// <summary>
        /// Filter the registry access based on the key name filter mask if it was set
        /// </summary>
        public string RegistryKeyNameFilterMask { get { return registryKeyNameFilterMask; } set { registryKeyNameFilterMask = value; } }

        /// <summary>
        /// The the flag to control how to access the registry for the matched process or user
        /// </summary>
        public uint ControlFlag{ get; set;}

        /// <summary>
        /// Register the callback when the registry access notification was triggered
        /// </summary>
        public ulong RegCallbackClass{ get; set;}

        /// <summary>
        /// If it is true, the registry access from the matched process or user will be excluded.
        /// </summary>
        public bool IsExcludeFilter { get; set; }


        /// <summary>
        ///Fires this notification event after the registry request was returned.. 
        /// </summary>
        /// 
        public event EventHandler<RegistryEventArgs> NotifyRegWasBlocked;
        public event EventHandler<RegistryEventArgs> NotifyDeleteKey;
        public event EventHandler<RegistryEventArgs> NotifySetValueKey;
        public event EventHandler<RegistryEventArgs> NotifyDeleteValueKey;
        public event EventHandler<RegistryEventArgs> NotifySetInformationKey;
        public event EventHandler<RegistryEventArgs> NotifyRenameKey;
        public event EventHandler<RegistryEventArgs> NotifyEnumerateKey;
        public event EventHandler<RegistryEventArgs> NotifyEnumerateValueKey;
        public event EventHandler<RegistryEventArgs> NotifyQueryKey;
        public event EventHandler<RegistryEventArgs> NotifyQueryValueKey;
        public event EventHandler<RegistryEventArgs> NotifyQueryMultipleValueKey;
        public event EventHandler<RegistryEventArgs> NotifyCreateKey;
        public event EventHandler<RegistryEventArgs> NotifyOpenKey;
        public event EventHandler<RegistryEventArgs> NotifyKeyHandleClose;
        public event EventHandler<RegistryEventArgs> NotifyCreateKeyEx;
        public event EventHandler<RegistryEventArgs> NotifyOpenKeyEx;
        public event EventHandler<RegistryEventArgs> NotifyFlushKey;
        public event EventHandler<RegistryEventArgs> NotifyLoadKey;
        public event EventHandler<RegistryEventArgs> NotifyUnLoadKey;
        public event EventHandler<RegistryEventArgs> NotifyQueryKeySecurity;
        public event EventHandler<RegistryEventArgs> NotifySetKeySecurity;
        public event EventHandler<RegistryEventArgs> NotifyRestoreKey;
        public event EventHandler<RegistryEventArgs> NotifySaveKey;
        public event EventHandler<RegistryEventArgs> NotifyReplaceKey;
        public event EventHandler<RegistryEventArgs> NotifyQueryKeyName;

        /// <summary>
        /// Fire the control event before the registry request was going to the registry handler.
        /// </summary>
        public event EventHandler<RegistryEventArgs> OnPreDeleteKey;
        public event EventHandler<RegistryEventArgs> OnPreSetValueKey;
        public event EventHandler<RegistryEventArgs> OnPreDeleteValueKey;
        public event EventHandler<RegistryEventArgs> OnPreSetInformationKey;
        public event EventHandler<RegistryEventArgs> OnPreRenameKey;
        public event EventHandler<RegistryEventArgs> OnPreEnumerateKey;
        public event EventHandler<RegistryEventArgs> OnPreEnumerateValueKey;
        public event EventHandler<RegistryEventArgs> OnPreQueryKey;
        public event EventHandler<RegistryEventArgs> OnPreQueryValueKey;
        public event EventHandler<RegistryEventArgs> OnPreQueryMultipleValueKey;
        public event EventHandler<RegistryEventArgs> OnPreCreateKey;
        public event EventHandler<RegistryEventArgs> OnPreOpenKey;
        public event EventHandler<RegistryEventArgs> OnPreKeyHandleClose;
        public event EventHandler<RegistryEventArgs> OnPreCreateKeyEx;
        public event EventHandler<RegistryEventArgs> OnPreOpenKeyEx;
        public event EventHandler<RegistryEventArgs> OnPreFlushKey;
        public event EventHandler<RegistryEventArgs> OnPreLoadKey;
        public event EventHandler<RegistryEventArgs> OnPreUnLoadKey;
        public event EventHandler<RegistryEventArgs> OnPreQueryKeySecurity;
        public event EventHandler<RegistryEventArgs> OnPreSetKeySecurity;
        public event EventHandler<RegistryEventArgs> OnPreRestoreKey;
        public event EventHandler<RegistryEventArgs> OnPreSaveKey;
        public event EventHandler<RegistryEventArgs> OnPreReplaceKey;
        public event EventHandler<RegistryEventArgs> OnPreQueryKeyName;

        public RegistryFilter()
        {
            this.FilterType = FilterAPI.FilterType.REGISTRY_FILTER;
        }

        public override void SendNotification(FilterAPI.MessageSendData messageSend)
        {
            try
            {
                RegistryEventArgs registryEventArgs = new RegistryEventArgs(messageSend);

                if (messageSend.FilterCommand == (uint)FilterAPI.FilterCommand.FILTER_SEND_DENIED_REGISTRY_ACCESS_EVENT)
                {
                    if (null != NotifyRegWasBlocked)
                    {
                        registryEventArgs.EventName = "NotifyRegWasBlocked";
                        NotifyRegWasBlocked(this, registryEventArgs);
                    }

                    return;
                }

                switch (registryEventArgs.RegCallbackClass)
                {

                    case FilterAPI.RegCallbackClass.Reg_Post_Delete_Key:
                        {
                            if (null != NotifyDeleteKey)
                            {
                                registryEventArgs.EventName = "NotifyDeleteKey";
                                NotifyDeleteKey(this, registryEventArgs);
                            }

                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Post_Set_Value_Key:
                        {
                            if (null != NotifySetValueKey)
                            {
                                registryEventArgs.EventName = "NotifySetValueKey";
                                NotifySetValueKey(this, registryEventArgs);
                            }

                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Post_Delete_Value_Key:
                        {
                            if (null != NotifyDeleteValueKey)
                            {
                                registryEventArgs.EventName = "NotifyDeleteValueKey";
                                NotifyDeleteValueKey(this, registryEventArgs);
                            }

                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Post_SetInformation_Key:
                        {
                            if (null != NotifySetInformationKey)
                            {
                                registryEventArgs.EventName = "NotifySetInformationKey";
                                NotifySetInformationKey(this, registryEventArgs);
                            }

                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Post_Rename_Key:
                        {
                            if (null != NotifyRenameKey)
                            {
                                registryEventArgs.EventName = "NotifyRenameKey";
                                NotifyRenameKey(this, registryEventArgs);
                            }

                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Post_Enumerate_Key:
                        {
                            if (null != NotifyEnumerateKey)
                            {
                                registryEventArgs.EventName = "NotifyEnumerateKey";
                                NotifyEnumerateKey(this, registryEventArgs);
                            }

                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Post_Enumerate_Value_Key:
                        {
                            if (null != NotifyEnumerateValueKey)
                            {
                                registryEventArgs.EventName = "NotifyEnumerateValueKey";
                                NotifyEnumerateValueKey(this, registryEventArgs);
                            }

                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Post_Query_Key:
                        {
                            if (null != NotifyQueryKey)
                            {
                                registryEventArgs.EventName = "NotifyQueryKey";
                                NotifyQueryKey(this, registryEventArgs);
                            }

                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Post_Query_Value_Key:
                        {
                            if (null != NotifyQueryValueKey)
                            {
                                registryEventArgs.EventName = "NotifyQueryValueKey";
                                NotifyQueryValueKey(this, registryEventArgs);
                            }

                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Post_Query_Multiple_Value_Key:
                        {
                            if (null != NotifyQueryMultipleValueKey)
                            {
                                registryEventArgs.EventName = "NotifyQueryMultipleValueKey";
                                NotifyQueryMultipleValueKey(this, registryEventArgs);
                            }

                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Post_Create_Key:
                        {
                            if (null != NotifyCreateKey)
                            {
                                registryEventArgs.EventName = "NotifyCreateKey";
                                NotifyCreateKey(this, registryEventArgs);
                            }

                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Post_Open_Key:
                        {
                            if (null != NotifyOpenKey)
                            {
                                registryEventArgs.EventName = "NotifyOpenKey";
                                NotifyOpenKey(this, registryEventArgs);
                            }

                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Post_Key_Handle_Close:
                        {
                            if (null != NotifyKeyHandleClose)
                            {
                                registryEventArgs.EventName = "NotifyKeyHandleClose";
                                NotifyKeyHandleClose(this, registryEventArgs);
                            }

                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Post_Create_KeyEx:
                        {
                            if (null != NotifyCreateKeyEx)
                            {
                                registryEventArgs.EventName = "NotifyCreateKeyEx";
                                NotifyCreateKeyEx(this, registryEventArgs);
                            }

                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Post_Open_KeyEx:
                        {
                            if (null != NotifyOpenKeyEx)
                            {
                                registryEventArgs.EventName = "NotifyOpenKeyEx";
                                NotifyOpenKeyEx(this, registryEventArgs);
                            }

                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Post_Flush_Key:
                        {
                            if (null != NotifyFlushKey)
                            {
                                registryEventArgs.EventName = "NotifyFlushKey";
                                NotifyFlushKey(this, registryEventArgs);
                            }

                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Post_Load_Key:
                        {
                            if (null != NotifyLoadKey)
                            {
                                registryEventArgs.EventName = "NotifyLoadKey";
                                NotifyLoadKey(this, registryEventArgs);
                            }

                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Post_UnLoad_Key:
                        {
                            if (null != NotifyUnLoadKey)
                            {
                                registryEventArgs.EventName = "NotifyUnLoadKey";
                                NotifyUnLoadKey(this, registryEventArgs);
                            }

                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Post_Query_Key_Security:
                        {
                            if (null != NotifyQueryKeySecurity)
                            {
                                registryEventArgs.EventName = "NotifyQueryKeySecurity";
                                NotifyQueryKeySecurity(this, registryEventArgs);
                            }

                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Post_Set_Key_Security:
                        {
                            if (null != NotifySetKeySecurity)
                            {
                                registryEventArgs.EventName = "NotifySetKeySecurity";
                                NotifySetKeySecurity(this, registryEventArgs);
                            }

                            break;
                        }


                    case FilterAPI.RegCallbackClass.Reg_Post_Restore_Key:
                        {
                            if (null != NotifyRestoreKey)
                            {
                                registryEventArgs.EventName = "NotifyRestoreKey";
                                NotifyRestoreKey(this, registryEventArgs);
                            }

                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Post_Save_Key:
                        {
                            if (null != NotifySaveKey)
                            {
                                registryEventArgs.EventName = "NotifySaveKey";
                                NotifySaveKey(this, registryEventArgs);
                            }

                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Post_Replace_Key:
                        {
                            if (null != NotifyReplaceKey)
                            {
                                registryEventArgs.EventName = "NotifyReplaceKey";
                                NotifyReplaceKey(this, registryEventArgs);
                            }

                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Post_Query_KeyName:
                        {
                            if (null != NotifyQueryKeyName)
                            {
                                registryEventArgs.EventName = "NotifyQueryKeyName";
                                NotifyQueryKeyName(this, registryEventArgs);
                            }

                            break;
                        }

                    default: break;
                }
            }
            catch
            {
            }
        }
        

        public override bool ReplyMessage(FilterAPI.MessageSendData messageSend, IntPtr replyDataPtr)
        {
            bool retVal = true;

            try
            {
                FilterAPI.MessageReplyData messageReply = (FilterAPI.MessageReplyData)Marshal.PtrToStructure(replyDataPtr, typeof(FilterAPI.MessageReplyData));

                RegistryEventArgs registryEventArgs = new RegistryEventArgs(messageSend);

                switch (registryEventArgs.RegCallbackClass)
                {
                    case FilterAPI.RegCallbackClass.Reg_Pre_Delete_Key:
                        {
                            if (null != OnPreDeleteKey)
                            {
                                registryEventArgs.EventName = "OnPreDeleteKey";

                                OnPreDeleteKey(this, registryEventArgs);
                                if (registryEventArgs.ReturnStatus != NtStatus.Status.Success)
                                {
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                                else if (registryEventArgs.IsDataModified)
                                {
                                    Array.Copy(registryEventArgs.ReturnDataBuffer, messageReply.DataBuffer, registryEventArgs.ReturnDataBuffer.Length);
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION | (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                            }

                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Pre_Set_Value_Key:
                        {
                            if (null != OnPreSetValueKey)
                            {
                                registryEventArgs.EventName = "OnPreSetValueKey";

                                OnPreSetValueKey(this, registryEventArgs);
                                if (registryEventArgs.ReturnStatus != NtStatus.Status.Success)
                                {
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                                else if (registryEventArgs.IsDataModified)
                                {
                                    Array.Copy(registryEventArgs.ReturnDataBuffer, messageReply.DataBuffer, registryEventArgs.ReturnDataBuffer.Length);
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION | (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                            }

                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Pre_Delete_Value_Key:
                        {
                            if (null != OnPreDeleteValueKey)
                            {
                                registryEventArgs.EventName = "OnPreDeleteValueKey";

                                OnPreDeleteValueKey(this, registryEventArgs);
                                if (registryEventArgs.ReturnStatus != NtStatus.Status.Success)
                                {
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                                else if (registryEventArgs.IsDataModified)
                                {
                                    Array.Copy(registryEventArgs.ReturnDataBuffer, messageReply.DataBuffer, registryEventArgs.ReturnDataBuffer.Length);
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION | (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                            }

                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Pre_SetInformation_Key:
                        {
                            if (null != OnPreSetInformationKey)
                            {
                                registryEventArgs.EventName = "OnPreSetInformationKey";

                                OnPreSetInformationKey(this, registryEventArgs);
                                if (registryEventArgs.ReturnStatus != NtStatus.Status.Success)
                                {
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                                else if (registryEventArgs.IsDataModified)
                                {
                                    Array.Copy(registryEventArgs.ReturnDataBuffer, messageReply.DataBuffer, registryEventArgs.ReturnDataBuffer.Length);
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION | (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                            }

                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Pre_Rename_Key:
                        {
                            if (null != OnPreRenameKey)
                            {
                                registryEventArgs.EventName = "OnPreRenameKey";

                                OnPreRenameKey(this, registryEventArgs);
                                if (registryEventArgs.ReturnStatus != NtStatus.Status.Success)
                                {
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                                else if (registryEventArgs.IsDataModified)
                                {
                                    Array.Copy(registryEventArgs.ReturnDataBuffer, messageReply.DataBuffer, registryEventArgs.ReturnDataBuffer.Length);
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION | (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                            }

                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Pre_Enumerate_Key:
                        {
                            if (null != OnPreEnumerateKey)
                            {
                                registryEventArgs.EventName = "OnPreEnumerateKey";

                                OnPreEnumerateKey(this, registryEventArgs);
                                if (registryEventArgs.ReturnStatus != NtStatus.Status.Success)
                                {
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                                else if (registryEventArgs.IsDataModified)
                                {
                                    Array.Copy(registryEventArgs.ReturnDataBuffer, messageReply.DataBuffer, registryEventArgs.ReturnDataBuffer.Length);
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION | (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                            }

                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Pre_Enumerate_Value_Key:
                        {
                            if (null != OnPreEnumerateValueKey)
                            {
                                registryEventArgs.EventName = "OnPreEnumerateValueKey";

                                OnPreEnumerateValueKey(this, registryEventArgs);
                                if (registryEventArgs.ReturnStatus != NtStatus.Status.Success)
                                {
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                                else if (registryEventArgs.IsDataModified)
                                {
                                    Array.Copy(registryEventArgs.ReturnDataBuffer, messageReply.DataBuffer, registryEventArgs.ReturnDataBuffer.Length);
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION | (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                            }

                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Pre_Query_Key:
                        {
                            if (null != OnPreQueryKey)
                            {
                                registryEventArgs.EventName = "OnPreQueryKey";

                                OnPreQueryKey(this, registryEventArgs);
                                if (registryEventArgs.ReturnStatus != NtStatus.Status.Success)
                                {
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                                else if (registryEventArgs.IsDataModified)
                                {
                                    Array.Copy(registryEventArgs.ReturnDataBuffer, messageReply.DataBuffer, registryEventArgs.ReturnDataBuffer.Length);
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION | (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                            }

                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Pre_Query_Value_Key:
                        {
                            if (null != OnPreQueryValueKey)
                            {
                                registryEventArgs.EventName = "OnPreQueryValueKey";

                                OnPreQueryValueKey(this, registryEventArgs);
                                if (registryEventArgs.ReturnStatus != NtStatus.Status.Success)
                                {
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                                else if (registryEventArgs.IsDataModified)
                                {
                                    Array.Copy(registryEventArgs.ReturnDataBuffer, messageReply.DataBuffer, registryEventArgs.ReturnDataBuffer.Length);
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION | (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                            }

                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Pre_Query_Multiple_Value_Key:
                        {
                            if (null != OnPreQueryMultipleValueKey)
                            {
                                registryEventArgs.EventName = "OnPreQueryMultipleValueKey";

                                OnPreQueryMultipleValueKey(this, registryEventArgs);
                                if (registryEventArgs.ReturnStatus != NtStatus.Status.Success)
                                {
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                                else if (registryEventArgs.IsDataModified)
                                {
                                    Array.Copy(registryEventArgs.ReturnDataBuffer, messageReply.DataBuffer, registryEventArgs.ReturnDataBuffer.Length);
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION | (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                            }

                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Pre_Create_Key:
                        {
                            if (null != OnPreCreateKey)
                            {
                                registryEventArgs.EventName = "OnPreCreateKey";

                                OnPreCreateKey(this, registryEventArgs);
                                if (registryEventArgs.ReturnStatus != NtStatus.Status.Success)
                                {
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                                else if (registryEventArgs.IsDataModified)
                                {
                                    Array.Copy(registryEventArgs.ReturnDataBuffer, messageReply.DataBuffer, registryEventArgs.ReturnDataBuffer.Length);
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION | (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                            }

                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Pre_Open_Key:
                        {
                            if (null != OnPreOpenKey)
                            {
                                registryEventArgs.EventName = "OnPreOpenKey";

                                OnPreOpenKey(this, registryEventArgs);
                                if (registryEventArgs.ReturnStatus != NtStatus.Status.Success)
                                {
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                                else if (registryEventArgs.IsDataModified)
                                {
                                    Array.Copy(registryEventArgs.ReturnDataBuffer, messageReply.DataBuffer, registryEventArgs.ReturnDataBuffer.Length);
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION | (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                            }

                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Pre_Key_Handle_Close:
                        {
                            if (null != OnPreKeyHandleClose)
                            {
                                registryEventArgs.EventName = "OnPreKeyHandleClose";

                                OnPreKeyHandleClose(this, registryEventArgs);
                                if (registryEventArgs.ReturnStatus != NtStatus.Status.Success)
                                {
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                                else if (registryEventArgs.IsDataModified)
                                {
                                    Array.Copy(registryEventArgs.ReturnDataBuffer, messageReply.DataBuffer, registryEventArgs.ReturnDataBuffer.Length);
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION | (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                            }

                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Pre_Create_KeyEx:
                        {
                            if (null != OnPreCreateKeyEx)
                            {
                                registryEventArgs.EventName = "OnPreCreateKeyEx";

                                OnPreCreateKeyEx(this, registryEventArgs);
                                if (registryEventArgs.ReturnStatus != NtStatus.Status.Success)
                                {
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                                else if (registryEventArgs.IsDataModified)
                                {
                                    Array.Copy(registryEventArgs.ReturnDataBuffer, messageReply.DataBuffer, registryEventArgs.ReturnDataBuffer.Length);
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION | (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                            }

                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Pre_Open_KeyEx:
                        {
                            if (null != OnPreOpenKeyEx)
                            {
                                registryEventArgs.EventName = "OnPreOpenKeyEx";

                                OnPreOpenKeyEx(this, registryEventArgs);
                                if (registryEventArgs.ReturnStatus != NtStatus.Status.Success)
                                {
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                                else if (registryEventArgs.IsDataModified)
                                {
                                    Array.Copy(registryEventArgs.ReturnDataBuffer, messageReply.DataBuffer, registryEventArgs.ReturnDataBuffer.Length);
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION | (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                            }

                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Pre_Flush_Key:
                        {
                            if (null != OnPreFlushKey)
                            {
                                registryEventArgs.EventName = "OnPreFlushKey";

                                OnPreFlushKey(this, registryEventArgs);
                                if (registryEventArgs.ReturnStatus != NtStatus.Status.Success)
                                {
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                                else if (registryEventArgs.IsDataModified)
                                {
                                    Array.Copy(registryEventArgs.ReturnDataBuffer, messageReply.DataBuffer, registryEventArgs.ReturnDataBuffer.Length);
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION | (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                            }

                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Pre_Load_Key:
                        {
                            if (null != OnPreLoadKey)
                            {
                                registryEventArgs.EventName = "OnPreLoadKey";

                                OnPreLoadKey(this, registryEventArgs);
                                if (registryEventArgs.ReturnStatus != NtStatus.Status.Success)
                                {
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                                else if (registryEventArgs.IsDataModified)
                                {
                                    Array.Copy(registryEventArgs.ReturnDataBuffer, messageReply.DataBuffer, registryEventArgs.ReturnDataBuffer.Length);
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION | (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                            }

                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Pre_UnLoad_Key:
                        {
                            if (null != OnPreUnLoadKey)
                            {
                                registryEventArgs.EventName = "OnPreUnLoadKey";

                                OnPreLoadKey(this, registryEventArgs);
                                if (registryEventArgs.ReturnStatus != NtStatus.Status.Success)
                                {
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                                else if (registryEventArgs.IsDataModified)
                                {
                                    Array.Copy(registryEventArgs.ReturnDataBuffer, messageReply.DataBuffer, registryEventArgs.ReturnDataBuffer.Length);
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION | (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                            }

                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Pre_Query_Key_Security:
                        {
                            if (null != OnPreQueryKeySecurity)
                            {
                                registryEventArgs.EventName = "OnPreQueryKeySecurity";

                                OnPreQueryKeySecurity(this, registryEventArgs);
                                if (registryEventArgs.ReturnStatus != NtStatus.Status.Success)
                                {
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                                else if (registryEventArgs.IsDataModified)
                                {
                                    Array.Copy(registryEventArgs.ReturnDataBuffer, messageReply.DataBuffer, registryEventArgs.ReturnDataBuffer.Length);
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION | (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                            }

                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Pre_Set_Key_Security:
                        {
                            if (null != OnPreSetKeySecurity)
                            {
                                registryEventArgs.EventName = "OnPreSetKeySecurity";

                                OnPreSetKeySecurity(this, registryEventArgs);
                                if (registryEventArgs.ReturnStatus != NtStatus.Status.Success)
                                {
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                                else if (registryEventArgs.IsDataModified)
                                {
                                    Array.Copy(registryEventArgs.ReturnDataBuffer, messageReply.DataBuffer, registryEventArgs.ReturnDataBuffer.Length);
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION | (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                            }

                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Pre_Restore_Key:
                        {
                            if (null != OnPreRestoreKey)
                            {
                                registryEventArgs.EventName = "OnPreRestoreKey";

                                OnPreRestoreKey(this, registryEventArgs);
                                if (registryEventArgs.ReturnStatus != NtStatus.Status.Success)
                                {
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                                else if (registryEventArgs.IsDataModified)
                                {
                                    Array.Copy(registryEventArgs.ReturnDataBuffer, messageReply.DataBuffer, registryEventArgs.ReturnDataBuffer.Length);
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION | (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                            }

                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Pre_Save_Key:
                        {
                            if (null != OnPreSaveKey)
                            {
                                registryEventArgs.EventName = "OnPreSaveKey";

                                OnPreSaveKey(this, registryEventArgs);
                                if (registryEventArgs.ReturnStatus != NtStatus.Status.Success)
                                {
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                                else if (registryEventArgs.IsDataModified)
                                {
                                    Array.Copy(registryEventArgs.ReturnDataBuffer, messageReply.DataBuffer, registryEventArgs.ReturnDataBuffer.Length);
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION | (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                            }

                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Pre_Replace_Key:
                        {
                            if (null != OnPreReplaceKey)
                            {
                                registryEventArgs.EventName = "OnPreReplaceKey";

                                OnPreReplaceKey(this, registryEventArgs);
                                if (registryEventArgs.ReturnStatus != NtStatus.Status.Success)
                                {
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                                else if (registryEventArgs.IsDataModified)
                                {
                                    Array.Copy(registryEventArgs.ReturnDataBuffer, messageReply.DataBuffer, registryEventArgs.ReturnDataBuffer.Length);
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION | (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                            }

                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Pre_Query_KeyName:
                        {
                            if (null != OnPreQueryKeyName)
                            {
                                registryEventArgs.EventName = "OnPreQueryKeyName";

                                OnPreQueryKeyName(this, registryEventArgs);
                                if (registryEventArgs.ReturnStatus != NtStatus.Status.Success)
                                {
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                                else if (registryEventArgs.IsDataModified)
                                {
                                    Array.Copy(registryEventArgs.ReturnDataBuffer, messageReply.DataBuffer, registryEventArgs.ReturnDataBuffer.Length);
                                    messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION | (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED;
                                    messageReply.ReturnStatus = (uint)registryEventArgs.ReturnStatus;
                                }
                            }

                            break;
                        }

                    default: break;
                }

                Marshal.StructureToPtr(messageReply, replyDataPtr, true);
            }
            catch
            {
            }

            return retVal;
        }
    }
}
