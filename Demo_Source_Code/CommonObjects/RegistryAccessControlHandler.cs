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


namespace  EaseFilter.CommonObjects
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

        public static string FormatDescription(FilterAPI.MessageSendData messageSend)
        {
            string descrption = string.Empty;
            FilterAPI.RegCallbackClass regCallbackClass = (FilterAPI.RegCallbackClass)messageSend.Offset;

            try
            {

                if (messageSend.Status != (uint)FilterAPI.NTSTATUS.STATUS_SUCCESS)
                {
                    return "";
                }

                switch (regCallbackClass)
                {
                    case FilterAPI.RegCallbackClass.Reg_Pre_Delete_Key:
                    case FilterAPI.RegCallbackClass.Reg_Post_Delete_Key:
                        {
                            descrption = "registry key is being deleted.";
                            break;
                        }
                    case FilterAPI.RegCallbackClass.Reg_Pre_Set_Value_Key:
                    case FilterAPI.RegCallbackClass.Reg_Post_Set_Value_Key:
                        {
                            VALUE_DATA_TYPE valueType = (VALUE_DATA_TYPE)messageSend.InfoClass;
                            descrption = "Type:" + valueType.ToString();
                            descrption += " Data:" + ValueTypeData(valueType, (int)messageSend.DataBufferLength, messageSend.DataBuffer);
                            break;
                        }
                    case FilterAPI.RegCallbackClass.Reg_Pre_Delete_Value_Key:
                    case FilterAPI.RegCallbackClass.Reg_Post_Delete_Value_Key:
                        {
                            descrption = "registry key's value is being deleted.";
                            break;
                        }
                    case FilterAPI.RegCallbackClass.Reg_Pre_SetInformation_Key:
                    case FilterAPI.RegCallbackClass.Reg_Post_SetInformation_Key:
                        {
                            KEY_SET_INFORMATION_CLASS keySetInformationClass = (KEY_SET_INFORMATION_CLASS)messageSend.InfoClass;
                            descrption = keySetInformationClass.ToString();
                            break;
                        }
                    case FilterAPI.RegCallbackClass.Reg_Pre_Rename_Key:
                    case FilterAPI.RegCallbackClass.Reg_Post_Rename_Key:
                        {
                            string newName = Encoding.Unicode.GetString(messageSend.DataBuffer);
                            descrption = "registry key's name is being changed to " + newName;
                            break;
                        }
                    case FilterAPI.RegCallbackClass.Reg_Pre_Enumerate_Key:
                        {
                            KEY_INFORMATION_CLASS keyInformationClass = (KEY_INFORMATION_CLASS)messageSend.InfoClass;
                            descrption = keyInformationClass.ToString();

                            break;
                        }
                    case FilterAPI.RegCallbackClass.Reg_Post_Enumerate_Key:
                        {
                            KEY_INFORMATION_CLASS keyInformationClass = (KEY_INFORMATION_CLASS)messageSend.InfoClass;
                            descrption += KeyInformation(keyInformationClass, messageSend.DataBuffer);

                            break;
                        }
                    case FilterAPI.RegCallbackClass.Reg_Pre_Enumerate_Value_Key:
                        {
                            KEY_VALUE_INFORMATION_CLASS keyValuseInformationClass = (KEY_VALUE_INFORMATION_CLASS)messageSend.InfoClass;
                            descrption = keyValuseInformationClass.ToString();

                            break;
                        }
                    case FilterAPI.RegCallbackClass.Reg_Post_Enumerate_Value_Key:
                        {
                            KEY_VALUE_INFORMATION_CLASS keyValuseInformationClass = (KEY_VALUE_INFORMATION_CLASS)messageSend.InfoClass;
                            descrption += KeyValueInformation(keyValuseInformationClass, messageSend.DataBuffer);

                            break;
                        }
                    case FilterAPI.RegCallbackClass.Reg_Pre_Query_Key:
                        {
                            KEY_INFORMATION_CLASS keyInformationClass = (KEY_INFORMATION_CLASS)messageSend.InfoClass;
                            descrption = keyInformationClass.ToString();

                            break;
                        }
                    case FilterAPI.RegCallbackClass.Reg_Post_Query_Key:
                        {
                            KEY_INFORMATION_CLASS keyInformationClass = (KEY_INFORMATION_CLASS)messageSend.InfoClass;
                            descrption += KeyInformation(keyInformationClass, messageSend.DataBuffer);

                            break;
                        }
                    case FilterAPI.RegCallbackClass.Reg_Pre_Query_Value_Key:
                        {
                            KEY_VALUE_INFORMATION_CLASS keyValuseInformationClass = (KEY_VALUE_INFORMATION_CLASS)messageSend.InfoClass;
                            descrption = keyValuseInformationClass.ToString();

                            break;
                        }
                    case FilterAPI.RegCallbackClass.Reg_Post_Query_Value_Key:
                        {
                            //for unit test
                            if (messageSend.FileName.IndexOf("EaseFilter") > 0)
                            {
                                //this is our test key.
                                RegistryUnitTest.postQueryValueKeyPassed = true;
                            }

                            KEY_VALUE_INFORMATION_CLASS keyValuseInformationClass = (KEY_VALUE_INFORMATION_CLASS)messageSend.InfoClass;
                            descrption += KeyValueInformation(keyValuseInformationClass, messageSend.DataBuffer);

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

                            for (int i = 0; i < entryCount&&ms.Position < ms.Length; i++)
                            {
                                long currentOffset = ms.Position;
                                int nextEntryOffset = br.ReadInt32();
                                int valueNameLength = br.ReadInt32();
                                int dataType = br.ReadInt32();
                                int dataLength = br.ReadInt32();
                                byte[] valueName = br.ReadBytes(valueNameLength);
                                byte[] data = br.ReadBytes(dataLength);

                                VALUE_DATA_TYPE type = (VALUE_DATA_TYPE)dataType;
                                descrption += "Name:" + Encoding.Unicode.GetString(valueName, 0, valueNameLength);
                                descrption += " Type:" + type.ToString();
                                descrption += " Data:" + ValueTypeData(type,dataLength, data) + Environment.NewLine;

                                ms.Position = currentOffset + nextEntryOffset;

                            }

                            break;
                        }
                    case FilterAPI.RegCallbackClass.Reg_Pre_Create_KeyEx:
                    case FilterAPI.RegCallbackClass.Reg_Post_Create_KeyEx:
                    case FilterAPI.RegCallbackClass.Reg_Pre_Open_KeyEx:
                    case FilterAPI.RegCallbackClass.Reg_Post_Open_KeyEx:
                        {
                            descrption += FormatCreateDescription(messageSend);
                            break;
                        }
                    case FilterAPI.RegCallbackClass.Reg_Pre_Load_Key:
                    case FilterAPI.RegCallbackClass.Reg_Post_Load_Key:
                        {
                            descrption += "SourceFile:" + Encoding.Unicode.GetString(messageSend.DataBuffer, 0, (int)messageSend.DataBufferLength);
                            break;
                        }
                    case FilterAPI.RegCallbackClass.Reg_Pre_Replace_Key:
                    case FilterAPI.RegCallbackClass.Reg_Post_Replace_Key:
                        {
                            descrption += "NewFileName:" + Encoding.Unicode.GetString(messageSend.DataBuffer, 0, (int)messageSend.DataBufferLength);
                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Pre_Query_KeyName:
                    case FilterAPI.RegCallbackClass.Reg_Post_Query_KeyName:
                        {
                            break;
                        }

                    default: descrption = "unsupported registry callback class:" + regCallbackClass.ToString(); break;
                }
            }
            catch (Exception ex)
            {
                descrption = "Format description failed, return error:" + ex.Message;
            }

            return descrption;
        }


        public static bool AuthorizeRegistryAccess(FilterAPI.MessageSendData messageSend, ref FilterAPI.MessageReplyData messageReply)
        {
            bool ret = true;

            try
            {

                messageReply.MessageId = messageSend.MessageId;
                messageReply.MessageType = messageSend.MessageType;
                messageReply.ReturnStatus = (uint)FilterAPI.NTSTATUS.STATUS_SUCCESS;


                //
                //here you can control registry request,block the access or modify the registry data.
                //
                //

                //if you don't want to change anything to this registry request, just let it pass through as below setting:
                //messageReply.FilterStatus = 0;
                //messageReply.ReturnStatus = (uint)NtStatus.Status.Success;

                //if you want to block the access this registry request, you can return the status as below,
                //it is only for pre callback requests.
                //messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                //messageReply.ReturnStatus = (uint)NtStatus.Status.AccessDenied;

                //if you want to modify the registry data and complete the pre IO with your own data, you can return status as below:
                // messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION | (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED;
                // messageReply.DataBufferLength = the return data buffer length.
                // messageReply.DataBuffer = the data you want to return.
                // messageReply.ReturnStatus = (uint)NtStatus.Status.Success;

                FilterAPI.RegCallbackClass regCallbackClass = (FilterAPI.RegCallbackClass)messageSend.Offset;

                uint dataLength = messageSend.DataBufferLength;
                byte[] data = messageSend.DataBuffer;

                switch (regCallbackClass)
                {

                    case FilterAPI.RegCallbackClass.Reg_Pre_Query_Value_Key:
                        {
                            KEY_VALUE_INFORMATION_CLASS keyValuseInformationClass = (KEY_VALUE_INFORMATION_CLASS)messageSend.InfoClass;
                            IntPtr keyValueInfoPtr = Marshal.UnsafeAddrOfPinnedArrayElement(data, 0);

                            
                            if (messageSend.FileName.IndexOf("EaseFilter") <= 0)
                            {
                                //this is not our unit test key
                                break;
                            }

                            //below code is for unit test to demo how to complete pre-callback registry call with our own data.
                            EventManager.WriteMessage(400, "AuthorizeRegistryAccess", EventLevel.Error, "Reg_Pre_Query_Value_Key keyValuseInformationClass:" + keyValuseInformationClass.ToString());

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

                                        MemoryStream ms = new MemoryStream(messageReply.DataBuffer);
                                        BinaryWriter bw = new BinaryWriter(ms);
                                        bw.Write(titleIndex);
                                        bw.Write(type);
                                        bw.Write(valueNameLength);
                                        bw.Write(valueName);

                                        messageReply.DataBufferLength = (uint)ms.Position;
                                        messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION | (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED;

                                        
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
                                        uint type =  (uint)VALUE_DATA_TYPE.REG_DWORD;                                        
                                        uint testData = 12345;
                                        uint testDataLength = sizeof(uint);
                                        byte[] valueName = Encoding.Unicode.GetBytes("value1");
                                        uint valueNameLength =(uint)valueName.Length;
                                        uint dataOffset = 5 * sizeof(uint) + valueNameLength;
                                        
                                        MemoryStream ms = new MemoryStream(messageReply.DataBuffer);
                                        BinaryWriter bw = new BinaryWriter(ms);
                                        bw.Write(titleIndex);
                                        bw.Write(type);
                                        bw.Write(dataOffset);
                                        bw.Write(testDataLength);
                                        bw.Write(valueNameLength);
                                        bw.Write(valueName);
                                        bw.Write(testData);

                                        messageReply.DataBufferLength = (uint)ms.Position;
                                        messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION | (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED;


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

                                        MemoryStream ms = new MemoryStream(messageReply.DataBuffer);
                                        BinaryWriter bw = new BinaryWriter(ms);

                                        bw.Write(titleIndex);
                                        bw.Write(type);
                                        bw.Write(testDataLength);
                                        bw.Write(testData);

                                        messageReply.DataBufferLength = (uint)ms.Position;

                                        messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION | (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED;
                                        messageReply.ReturnStatus = (uint)FilterAPI.NTSTATUS.STATUS_SUCCESS;

                                        break;
                                    }


                                default: break;
                            }
                            break;
                        }

                    case FilterAPI.RegCallbackClass.Reg_Pre_Create_KeyEx:
                    case FilterAPI.RegCallbackClass.Reg_Pre_Open_KeyEx:
                        {
                            //this is our unit test key
                            if (messageSend.FileName.IndexOf("EaseFilter") > 0 )
                            {
                                //NOT allow to create new registry key.

                                messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                                messageReply.ReturnStatus = (uint)NtStatus.Status.AccessDenied;
                            }

                            break;
                        }

                    default: break;
                }
            }
            catch (Exception ex)
            {
                EventManager.WriteMessage(400, "AuthorizeRegistryAccess", EventLevel.Error, "AuthorizeRegistryAccess exception:" + ex.Message);
            }

            return ret;
        }

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

        private static string FormatCreateDescription(FilterAPI.MessageSendData messageSend)
        {

            uint createOptions = messageSend.CreateOptions;
            uint desiredAccess = messageSend.DesiredAccess;
            uint dispositions = messageSend.Disposition;

            string message = string.Empty;

            message += "createOptions:";
            foreach (REG_CREATE_OPTIONS createOption in Enum.GetValues(typeof(REG_CREATE_OPTIONS)))
            {
                if (createOption == (REG_CREATE_OPTIONS)((uint)createOption & createOptions))
                {
                    message += createOption.ToString() + "; ";
                }
            }

            message += "(0x" + createOptions.ToString("X") + ");\r\n";

            message += "desiredAccess:";
            foreach (REG_ACCESS_MASK accessMask in Enum.GetValues(typeof(REG_ACCESS_MASK)))
            {
                if (accessMask == (REG_ACCESS_MASK)((uint)accessMask & desiredAccess))
                {
                    message += accessMask.ToString() + "; ";
                }
            }

            message += "(0x" + desiredAccess.ToString("X") + ");\r\n";

            if (dispositions > 0)
            {
                message += "dispositions:";
                foreach (REG_DISPOSITION disposition in Enum.GetValues(typeof(REG_DISPOSITION)))
                {
                    if (disposition == (REG_DISPOSITION)((uint)disposition & dispositions))
                    {
                        message += disposition.ToString() + "; ";
                    }
                }

                message += "(0x" + desiredAccess.ToString("X") + ");";
            }

            return message;
        }

    }
}
