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
using System.Collections;
using System.Collections.Generic;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Security.Cryptography;


namespace EaseFilter.CommonObjects
{

    static public class FilterAPI
    {
        public delegate Boolean FilterDelegate(IntPtr sendData, IntPtr replyData);
        public delegate void DisconnectDelegate();
        static GCHandle gchFilter;
        static GCHandle gchDisconnect;
        static bool isFilterStarted = false;
        public const int MAX_FILE_NAME_LENGTH = 1024;
        public const int MAX_SID_LENGTH = 256;
        public const int MAX_MESSAGE_LENGTH = 65536;
        public const int MAX_PATH = 260;
        public const int MAX_ERROR_MESSAGE_SIZE = 1024;
        public const uint MESSAGE_SEND_VERIFICATION_NUMBER = 0xFF000001;        
        public const uint GENERIC_WRITE = 0x40000000;
        public const uint AES_TAG_KEY = 0xccb76e80;

        public const uint FILE_FLAG_OPEN_REPARSE_POINT =  0x00200000;
        public const uint FILE_FLAG_OPEN_NO_RECALL =  0x00100000;
        public const uint FILE_FLAG_NO_BUFFERING =  0x20000000;
        public const uint FILE_ATTRIBUTE_REPARSE_POINT =  (uint)FileAttributes.ReparsePoint;

        static Dictionary<string, string> userNameTable = new Dictionary<string, string>();
        static Dictionary<uint, string> processNameTable = new Dictionary<uint, string>();

        //for encryption default IV key
        public static byte[] DEFAULT_IV_TAG = { 0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

        public enum FilterType : byte
        {     
            /// <summary>
            /// File system control filter driver
            /// </summary>
            FILE_SYSTEM_CONTROL = 0x01,
            /// <summary>
            /// File system encryption filter driver
            /// </summary>
            FILE_SYSTEM_ENCRYPTION = 0x02,
            /// <summary>
            /// File system monitor filter driver
            /// </summary>
            FILE_SYSTEM_MONITOR = 0x04,
            /// <summary>
            /// File system registry filter driver
            /// </summary>
            FILE_SYSTEM_REGISTRY = 0x08,
            /// <summary>
            /// File system process filter driver
            /// </summary>
            FILE_SYSTEM_PROCESS = 0x10,
            /// <summary>
            /// File system hierarchical storage management filter driver
            /// </summary>
            FILE_SYSTEM_HSM = 0x40,
            /// <summary>
            /// File system cloud storage filter driver
            /// </summary>
            FILE_SYSTEM_CLOUD = 0x80,
        }

        public enum EncryptionMethod : uint
        {
            /// <summary>
            /// Use the same encryption key and iv from the filter rule for all files, there are no reparse point tag data added to encrypted file,
            /// it supports all file systems. You can't identify if the file is encrypted or not by API.
            /// </summary>
            ENCRYPT_FILE_WITH_SAME_KEY_AND_IV = 0,
            /// <summary>
            /// Use the same encryption key from the filter rule for all files, use unique iv key per file, a meta data header will be appended to the 
            /// end of the encyrpted file. You can identify if the file is encrypted by checking the header.
            /// </summary>
            ENCRYPT_FILE_WITH_SAME_KEY_AND_UNIQUE_IV,
            /// <summary>
            ///Use the encryption key and iv from the user mode service,you can control how to use encryption key and iv per file.
            /// </summary>
            ENCRYPT_FILE_WITH_KEY_AND_IV_FROM_SERVICE,
            /// <summary>
            ///Use the same encryption key from the filter rule for all files, use unique iv key per file, a meta data tag will be embeded to the 
            /// the encyrpted file in the reparse point tag extended attribute. You can identify if the file is encrypted by checking the reparse point tag.
            /// </summary>
            ENCRYPT_FILE_WITH_REPARSE_POINT_DATA,
        }

        public enum BooleanConfig : uint
        {
            /// <summary>
            ///for easetag, if it was true, after the reparsepoint file was opened, it won't restore data back for read and write. 
            /// </summary>
            ENABLE_NO_RECALL_FLAG = 0x00000001,
            /// <summary>
            /// if it is true, the filter driver can't be unloaded.
            /// </summary>
            DISABLE_FILTER_UNLOAD_FLAG = 0x00000002,
            /// <summary>
            /// for virtual file, it will set offline attribute if it is true.
            /// </summary>            
            ENABLE_SET_OFFLINE_FLAG = 0x00000004,
            /// <summary>
            /// for encryption, it is true, it will use the default IV tag to encrypt the files.
            /// </summary>
            ENABLE_DEFAULT_IV_TAG = 0x00000008,
            /// <summary>
            /// if it is enabled, it will send the message data to a persistent file, or it will send the event to service right away. 
            /// </summary>
            ENABLE_ADD_MESSAGE_TO_FILE = 0x00000010,
            /// <summary>
            /// the encrypted file's meta data was embeded in the reparse point tag.
            /// </summary>
            ENCRYPT_FILE_WITH_REPARSE_POINT_TAG = 0x00000020,
            /// <summary>
            /// for encryption rule, get the encryption key and IV from user mode for the encrypted files.
            /// </summary>
            REQUEST_ENCRYPT_KEY_AND_IV_FROM_SERVICE = 0x00000040,
            /// <summary>
            /// for control filter, if it is enabled, the control filte rulle will be applied in boot time.
            /// </summary>
            ENABLE_PROTECTION_IN_BOOT_TIME = 0x00000080,
            /// <summary>
            /// for encryption rule, get the encryption key and IV and tag data which will attach to the file.
            /// </summary>
            REQUEST_ENCRYPT_KEY_IV_AND_TAGDATA_FROM_SERVICE = 0x00000100,
            /// <summary>
            /// if it is enabled, it will send the read/write databuffer to user mode.
            /// </summary>
            ENABLE_SEND_DATA_BUFFER = 0x00000200,
            /// <summary>
            /// if it is enabled, it will reopen the file when rehydration of the stub file.
            /// </summary>
            ENABLE_REOPEN_FILE_ON_REHYDRATION = 0x00000400,           
         
        }

        /// <summary>
        /// The command sent by filter driver
        /// </summary>
        public enum FilterCommand
        {
            /// <summary>
            /// send the notification event of the file was changed.
            /// </summary>
            FILTER_SEND_FILE_CHANGED_EVENT = 0x00010001,
            /// <summary>
            /// request the file open permission.
            /// </summary>
            FILTER_REQUEST_USER_PERMIT = 0x00010002,
            /// <summary>
            /// request the encryption key for the file open or creation.
            /// </summary>
            FILTER_REQUEST_ENCRYPTION_KEY = 0x00010003,
            /// <summary>
            /// request the encryption iv and key for the encrypted file open or to encrypt the file
            /// </summary>
            FILTER_REQUEST_ENCRYPTION_IV_AND_KEY = 0x00010004,
            /// <summary>
            /// request the encryption iv, key and access flags for the encrypted file open or to encrypt the file 
            /// </summary>
            FILTER_REQUEST_ENCRYPTION_IV_AND_KEY_AND_ACCESSFLAG = 0x00010005,
            /// <summary>
            /// request the encryption iv, key and access flags  and tag data for file encryption 
            /// </summary>
            FILTER_REQUEST_ENCRYPTION_IV_AND_KEY_AND_TAGDATA = 0x00010006,
            /// <summary>
            /// send the registry access notification class information
            /// </summary>
            FILTER_SEND_REG_CALLBACK_INFO = 0x00010007,
            /// <summary>
            /// send the new process creation information
            /// </summary>
            FILTER_SEND_PROCESS_CREATION_INFO = 0x00010008,
            /// <summary>
            /// send the process termination ifnormation
            /// </summary>
            FILTER_SEND_PROCESS_TERMINATION_INFO = 0x00010009,
            /// <summary>
            /// send the new thread creation information
            /// </summary>
            FILTER_SEND_THREAD_CREATION_INFO = 0x0001000a,
            /// <summary>
            /// send the thread termination ifnormation
            /// </summary>
            FILTER_SEND_THREAD_TERMINATION_INFO = 0x0001000b,
            /// <summary>
            /// send the process handle operations information
            /// </summary>
            FILTER_SEND_PROCESS_HANDLE_INFO = 0x0001000c,
            /// <summary>
            /// send the thread handle operations ifnormation
            /// </summary>
            FILTER_SEND_THREAD_HANDLE_INFO = 0x0001000d,
            /// <summary>
            /// send the volume information when it was attached
            /// </summary>
            FILTER_SEND_ATTACHED_VOLUME_INFO = 0x0001000e,
            /// <summary>
            /// send the volume information when it was detached
            /// </summary>
            FILTER_SEND_DETACHED_VOLUME_INFO = 0x0001000f,

        }


        /// <summary>
        /// the message type of the filter driver send the file IO request 
        /// </summary>
        public enum MessageType : uint
        {           
            PRE_CREATE = 0x00000001,
            POST_CREATE = 0x00000002,
            PRE_FASTIO_READ = 0x00000004,
            POST_FASTIO_READ = 0x00000008,
            PRE_CACHE_READ = 0x00000010,
            POST_CACHE_READ = 0x00000020,
            PRE_NOCACHE_READ = 0x00000040,
            POST_NOCACHE_READ = 0x00000080,
            PRE_PAGING_IO_READ = 0x00000100,
            POST_PAGING_IO_READ = 0x00000200,
            PRE_FASTIO_WRITE = 0x00000400,
            POST_FASTIO_WRITE = 0x00000800,
            PRE_CACHE_WRITE = 0x00001000,
            POST_CACHE_WRITE = 0x00002000,
            PRE_NOCACHE_WRITE = 0x00004000,
            POST_NOCACHE_WRITE = 0x00008000,
            PRE_PAGING_IO_WRITE = 0x00010000,
            POST_PAGING_IO_WRITE = 0x00020000,
            PRE_QUERY_INFORMATION = 0x00040000,
            POST_QUERY_INFORMATION = 0x00080000,
            PRE_SET_INFORMATION = 0x00100000,
            POST_SET_INFORMATION = 0x00200000,
            PRE_DIRECTORY = 0x00400000,
            POST_DIRECTORY = 0x00800000,
            PRE_QUERY_SECURITY = 0x01000000,
            POST_QUERY_SECURITY = 0x02000000,
            PRE_SET_SECURITY = 0x04000000,
            POST_SET_SECURITY = 0x08000000,
            PRE_CLEANUP = 0x10000000,
            POST_CLEANUP = 0x20000000,
            PRE_CLOSE = 0x40000000,
            POST_CLOSE = 0x80000000,

        }

        /// <summary>
        /// the file IO event type will be sent by the filter driver if the IO was happened.
        /// </summary>
        public enum EVENTTYPE : uint
        {
            NONE = 0,
            CREATED = 0x00000020,
            WRITTEN = 0x00000040,
            RENAMED = 0x00000080,
            DELETED = 0x00000100,
            SECURITY_CHANGED = 0x00000200,
            INFO_CHANGED = 0x00000400,
            READ = 0x00000800,
        }

        public enum NTSTATUS : uint
        {
            STATUS_SUCCESS = 0,
            STATUS_UNSUCCESSFUL = 0xc0000001,
            STATUS_ACCESS_DENIED = 0xC0000022,
        }

        /// <summary>
        /// The maximum file access right
        /// </summary>
        public const uint ALLOW_MAX_RIGHT_ACCESS = 0xfffffff0;

        /// <summary>
        /// The change file access right
        /// </summary>
        public const uint ALLOW_FILE_CHANGE_ACCESS = (uint)(AccessFlag.ALLOW_WRITE_ACCESS
               | AccessFlag.ALLOW_FILE_DELETE 
               | AccessFlag.ALLOW_FILE_SIZE_CHANGE
               | AccessFlag.ALLOW_OPEN_WITH_CREATE_OR_OVERWRITE_ACCESS
               | AccessFlag.ALLOW_OPEN_WITH_DELETE_ACCESS
               | AccessFlag.ALLOW_SET_INFORMATION
               | AccessFlag.ALLOW_SET_SECURITY_ACCESS
               | AccessFlag.ALLOW_FILE_RENAME);

        /// <summary>
        /// The read file access right
        /// </summary>
        public const uint ALLOW_FILE_READ_ACCESS = (uint)(AccessFlag.ALLOW_OPEN_WITH_READ_ACCESS
                                                |AccessFlag.ALLOW_READ_ACCESS
                                                |AccessFlag.ALLOW_QUERY_INFORMATION_ACCESS
                                                |AccessFlag.ALLOW_QUERY_SECURITY_ACCESS
                                                |AccessFlag.ALLOW_FILE_MEMORY_MAPPED
                                                |AccessFlag.ALLOW_DIRECTORY_LIST_ACCESS
                                                |AccessFlag.ALLOW_READ_ENCRYPTED_FILES );

        /// <summary>
        /// control the access rights of the file IO,set the accessFlag to LEAST_ACCESS_FLAG if you want to least access rights to the files. 
        /// </summary>
        public enum AccessFlag : uint
        {
            /// <summary>
            /// Filter driver will skip all the IO when the accessFlag equal 0 if the file name match the include file mask.
            /// </summary>
            EXCLUDE_FILTER_RULE = 0X00000000,
            /// <summary>
            /// Block the file open. 
            /// </summary>
            EXCLUDE_FILE_ACCESS = 0x00000001,
            /// <summary>
            /// Reparse the file open to the new file name if the reparse file mask was added.
            /// </summary>
            ENABLE_REPARSE_FILE_OPEN = 0x00000002,
            /// <summary>
            /// Hide the files from the folder directory list if the hide file mask was added.
            /// </summary>
            ENABLE_HIDE_FILES_IN_DIRECTORY_BROWSING = 0x00000004,
            /// <summary>
            /// Enable the transparent file encryption if the encryption key was added.
            /// </summary>
            ENABLE_FILE_ENCRYPTION_RULE = 0x00000008,
            /// <summary>
            /// Allow the file open to access the file's security information.
            /// </summary>
            ALLOW_OPEN_WTIH_ACCESS_SYSTEM_SECURITY = 0x00000010,
            /// <summary>
            /// Allow the file open for read access.
            /// </summary>
            ALLOW_OPEN_WITH_READ_ACCESS = 0x00000020,
            /// <summary>
            /// Allow the file open for write access.
            /// </summary>
            ALLOW_OPEN_WITH_WRITE_ACCESS = 0x00000040,
            /// <summary>
            /// Allow the file open for create new file or overwrite access.
            /// </summary>
            ALLOW_OPEN_WITH_CREATE_OR_OVERWRITE_ACCESS = 0x00000080,
            /// <summary>
            /// Allow the file open for delete.
            /// </summary>
            ALLOW_OPEN_WITH_DELETE_ACCESS = 0x00000100,
            /// <summary>
            /// Allow to read the file data.
            /// </summary>
            ALLOW_READ_ACCESS = 0x00000200,
            /// <summary>
            /// Allow write data to the file.
            /// </summary>
            ALLOW_WRITE_ACCESS = 0x00000400,
            /// <summary>
            /// Allow to query file information.
            /// </summary>
            ALLOW_QUERY_INFORMATION_ACCESS = 0x00000800,
           /// <summary>
           /// Allow to change the file information:file attribute,file size,file name,delete file
           /// </summary>
            ALLOW_SET_INFORMATION = 0x00001000,
            /// <summary>
            /// Allow to rename the file.
            /// </summary>
            ALLOW_FILE_RENAME = 0x00002000,
            /// <summary>
            /// Allow to delete the file.
            /// </summary>
            ALLOW_FILE_DELETE = 0x00004000,
            /// <summary>
            /// Allow to change file size.
            /// </summary>
            ALLOW_FILE_SIZE_CHANGE = 0x00008000,
            /// <summary>
            /// Allow query the file security information.
            /// </summary>
            ALLOW_QUERY_SECURITY_ACCESS = 0x00010000,
            /// <summary>
            /// Allow change the file security information.
            /// </summary>
            ALLOW_SET_SECURITY_ACCESS = 0x00020000,
            /// <summary>
            /// Allow to browse the directory file list.
            /// </summary>
            ALLOW_DIRECTORY_LIST_ACCESS = 0x00040000,
            /// <summary>
            /// Allow the remote access via share folder.
            /// </summary>
            ALLOW_FILE_ACCESS_FROM_NETWORK = 0x00080000,
            /// <summary>
            /// Allow to encrypt the new file if the encryption filter rule is enabled, .
            /// </summary>
            ALLOW_ENCRYPT_NEW_FILE = 0x00100000,
            /// <summary>
            /// Allow the application to read the encrypted files, or it will return encrypted data.
            /// </summary>
            ALLOW_READ_ENCRYPTED_FILES = 0x00200000,
            /// <summary>
            /// Allow the application to create a new file after it opened the protected file.
            /// </summary>
            ALLOW_ALL_SAVE_AS = 0x00400000,
            /// <summary>
            /// Allow copy protected files out of the protected folder if ALLOW_ALL_SAVE_AS is enabled.
            /// </summary>
            ALLOW_COPY_PROTECTED_FILES_OUT = 0x00800000,
            /// <summary>
            /// Allow the file to be mapped into memory access.
            /// </summary>
            ALLOW_FILE_MEMORY_MAPPED = 0x01000000,
            /// <summary>
            /// if the encryption filter rule is enabled, it will encrypt unencrypted data on read when the flag value is 0.
            /// </summary>
            DISABLE_ENCRYPT_DATA_ON_READ = 0x02000000, 
            /// <summary>
            /// If it is not exclude filter rule,the access flag can't be 0, at least you need to include this flag
            /// for filter driver to process this filter rule.
            /// </summary>
            LEAST_ACCESS_FLAG = 0xf0000000,
          //  ALLOW_MAX_RIGHT_ACCESS = 0xfffffff0,
        }

        const uint AES_VERIFICATION_KEY	 = 0xccb76e80;
        const int default_header_size = 1024;

        /// <summary>
        /// this is the header structure of the encrypted file
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct AES_HEADER
        {
            public uint VerificationKey;
            public uint AESFlags;
            public uint Version;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] IV;
            public uint EncryptionKeyLength;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] EncryptionKey;
            public long FileSize;
            public uint CryptoType;
            public uint PaddingSize;
            //the size of this data structure
            public uint AESDataSize;
            //the actual physical file size in disk including the padding and the header.
            public long ShadowFileSize;
            public uint AccessFlags;
            public uint Reserve1;
            public uint Reserve2;
            public uint TagDataLength;
            /// <summary>
            /// the maximum tag data size if the header size - sizeof(AES_HEADER);
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = default_header_size)]
            public byte[] TagData;

        };
     
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct MessageSendData
        {
            public uint MessageId;          //this is the request sequential number.
            public IntPtr FileObject;       //the address of FileObject,it is equivalent to file handle,it is unique per file stream open.
            public IntPtr FsContext;        //the address of FsContext,it is unique per file.
            public uint MessageType;        //the I/O request type.
            public uint ProcessId;          //the process ID for the process associated with the thread that originally requested the I/O operation.
            public uint ThreadId;           //the thread ID which requested the I/O operation.
            public long Offset;             //the read/write offset.
            public uint Length;             //the read/write length.
            public long FileSize;           //the size of the file for the I/O operation.
            public long TransactionTime;    //the transaction time in UTC of this request.
            public long CreationTime;       //the creation time in UTC of the file.
            public long LastAccessTime;     //the last access time in UTC of the file.
            public long LastWriteTime;      //the last write time in UTC of the file.
            public uint FileAttributes;     //the file attributes.
            public uint DesiredAccess;      //the DesiredAccess for file open, please reference CreateFile windows API.
            public uint Disposition;        //the Disposition for file open, please reference CreateFile windows API.
            public uint SharedAccess;       //the SharedAccess for file open, please reference CreateFile windows API.
            public uint CreateOptions;      //the CreateOptions for file open, please reference CreateFile windows API.
            public uint CreateStatus;       //the CreateStatus after file was openned, please reference CreateFile windows API.
            public uint InfoClass;          //the information class or security information
            public uint Status;             //the I/O status which returned from file system.
            public uint FileNameLength;     //the file name length in byte.
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_FILE_NAME_LENGTH)]
            public string FileName;         //the file name of the I/O operation.
            public uint SidLength;          //the length of the security identifier.
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_SID_LENGTH)]
            public byte[] Sid;              //the security identifier data.
            public uint DataBufferLength;   //the data buffer length.
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_MESSAGE_LENGTH)]
            public byte[] DataBuffer;       //the data buffer which contains read/write/query information/set information data.
            public uint VerificationNumber; //the verification number which verifiys the data structure integerity.
        }

        
        /// <summary>
        /// The attached volume control flag.
        /// </summary>
        public enum VolumeControlFlag : uint
        {
            /// <summary>
            /// Get all the attached volumes' information.
            /// </summary>
            GET_ATTACHED_VOLUME_INFO = 0x00000001,
            /// <summary>
            /// Get a notification when the filter driver attached to a volume.
            /// </summary>
            VOLUME_ATTACHED_NOTIFICATION = 0x00000002,
            /// <summary>
            /// Get a notification when the filter driver detached from a volume.
            /// </summary>
            VOLUME_DETACHED_NOTIFICATION = 0x00000004,
            /// <summary>
            /// Prevent the attched volumes from being formatted or dismounted.
            /// </summary>
            BLOCK_VOLUME_DISMOUNT = 0x00000008,

        }

       /// <summary>
      /// the structure of the attached volume information
      /// </summary>
      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
      public struct VOLUME_INFO
      {
          /// <summary>
          /// The length of the volume name.
          /// </summary>
          public uint VolumeNameLength;
          /// <summary>
          /// The volume name buffer.
          /// </summary>
          [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_FILE_NAME_LENGTH)]
          public string VolumeName;
          /// <summary>
          /// The length of the volume dos file name.
          /// </summary>
          public uint VolumeDosNameLength;
          /// <summary>
          /// The volume dos file name buffer.
          /// </summary>
          [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_FILE_NAME_LENGTH)]
          public string VolumeDosName;
          /// <summary>
          ///the volume file system type.
          /// </summary>
          public uint VolumeFilesystemType;
          /// <summary>
          ///the Characteristics of the attached device object if existed. 
          /// </summary>
          public uint DeviceCharacteristics;

      }

    /// <summary>
    /// Set the volume control to get the notification of the attached volume information.
    /// </summary>
    /// <param name="volumeControlFlag"></param>
    /// <returns></returns>
      [DllImport("FilterAPI.dll", SetLastError = true)]
      public static extern bool SetVolumeControlFlag(uint volumeControlFlag);

        /// <summary>
        /// the data structuct sent by the process filter driver
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct PROCESS_INFO
        {
            /// <summary>
            ///this is the request sequential number. 
            /// </summary>
            public uint MessageId;
            /// <summary>
            /// reserve data
            /// </summary>
            public IntPtr reserve1;
            /// <summary>
            /// reserve data
            /// </summary>
            public IntPtr reserve2;        
            /// <summary>
            ///the process message  type.
            /// </summary>
            public uint MessageType;
            /// <summary>
            ///the transaction time in UTC of this message.
            /// </summary>
            public long TransactionTime;
            /// <summary>
            //the current process ID of the process.
            /// </summary>
            public uint ProcessId;
            /// <summary>
            ///the thread ID of the current operation thread.
            /// </summary>
            public uint ThreadId;
            /// <summary>
            ///The process ID of the parent process for the new process. Note that the parent process is not necessarily the same process as the process that created the new process.  
            /// </summary>
            public uint ParentProcessId;
            /// <summary>
            ///  The process ID of the process that created the new process.
            /// </summary>
            public uint CreatingProcessId;
            /// <summary>
            /// The thread ID of the thread that created the new process.
            /// </summary>
            public uint CreatingThreadId;
            /// <summary>
            ///An ACCESS_MASK value that specifies the access rights to grant for the handle
            /// </summary>
            public uint DesiredAccess;
            /// <summary>
            ///The type of handle operation. This member might be one of the following values:OB_OPERATION_HANDLE_CREATE,OB_OPERATION_HANDLE_DUPLICATE
            /// </summary>
            public uint Operation;
            /// <summary>
            /// A Boolean value that specifies whether the ImageFileName member contains the exact file name that is used to open the process executable file.
            /// </summary>
            public bool FileOpenNameAvailable;
            /// <summary>
            ///the length of the security identifier.
            /// </summary>
            public uint SidLength;
            /// <summary>
            ///the security identifier data.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_SID_LENGTH)]
            public byte[] Sid;
            /// <summary>
            /// The length of the image file name.
            /// </summary>
            public uint ImageFileNameLength;
            /// <summary>
            /// The file name of the executable. If the FileOpenNameAvailable member is TRUE, the string specifies the exact file name that is used to open the executable file. 
            /// If FileOpenNameAvailable is FALSE, the operating system might provide only a partial name.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_FILE_NAME_LENGTH)]
            public string ImageFileName;
            /// <summary>
            /// The length of the command line.
            /// </summary>
            public uint CommandLineLength;
            /// <summary>
            /// The command that is used to execute the process.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_FILE_NAME_LENGTH)]
            public string CommandLine;
            /// <summary>
            ///the status which returned from file system.
            /// </summary>
            public uint Status;             
            /// <summary>
            ///the verification number which verifiys the data structure integerity. 
            /// </summary>
            public uint VerificationNumber;
        }

        /// <summary>
        /// the filter status which will be returned to the filter driver,
        /// the filter driver will process the IO based the return status.
        /// </summary>
        public enum FilterStatus : uint
        {
            FILTER_MESSAGE_IS_DIRTY = 0x00000001,           //the data buffer was updated.
            FILTER_COMPLETE_PRE_OPERATION = 0x00000002,     //ONLY FOR PRE CALL OPERATION,the IO won't pass down to the lower drivers and file system.
            FILTER_DATA_BUFFER_IS_UPDATED = 0x00000004,     //only for pre create,to reparse the file open to the new file name.	
            BLOCK_DATA_WAS_RETURNED = 0x00000008,           //Set this flag if return read block databuffer to filter.
            CACHE_FILE_WAS_RETURNED = 0x00000010,           //Set this flag if the stub file was restored.
        }    

        /// <summary>
        /// this is the data structure which will be returned back to the filter driver.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct MessageReplyData
        {
            public uint MessageId;
            public uint MessageType;
            public uint ReturnStatus;
            public uint FilterStatus;
            public uint DataBufferLength;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 65536)]
            public byte[] DataBuffer;
        }

        /// <summary>
        /// set the filter driver boolean config setting based on the enum booleanConfig
        /// </summary>
        /// <param name="booleanConfig"></param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool SetBooleanConfig(uint booleanConfig);

        /// <summary>
        /// install the filter driver service, it request the administrator privilege
        /// </summary>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool InstallDriver();

        /// <summary>
        /// uninstall the filter driver service. 
        /// </summary>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool UnInstallDriver();

        /// <summary>
        /// to check if the filter driver service is running.
        /// </summary>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool IsDriverServiceRunning();

        /// <summary>
        /// set the registration key to enable the filter driver service.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool SetRegistrationKey([MarshalAs(UnmanagedType.LPStr)]string key);

        /// <summary>
        /// disconnect the communication channel of the filter driver.
        /// </summary>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool Disconnect();

        /// <summary>
        /// get the last error message if the filter driver API return false.
        /// </summary>
        /// <param name="lastError"></param>
        /// <param name="messageLength"></param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool GetLastErrorMessage(
            [MarshalAs(UnmanagedType.LPWStr)] 
            string lastError,
            ref int messageLength);

        /// <summary>
        /// start the filter driver service with callback settings
        /// </summary>
        /// <param name="threadCount">the number of working threads waitting for the callback message</param>
        /// <param name="filterCallback">the callback function</param>
        /// <param name="disconnectCallback">the disconnect callback function</param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool RegisterMessageCallback(
            int threadCount,
            IntPtr filterCallback,
            IntPtr disconnectCallback);

        /// <summary>
        /// reset the filter driver config settings to the default value.
        /// </summary>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool ResetConfigData();

        /// <summary>
        /// set the filter driver type.
        /// </summary>
        /// <param name="filterType"></param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool SetFilterType(uint filterType);

        /// <summary>
        /// set the maiximun wait time of the filter driver sending message to service.
        /// </summary>
        /// <param name="timeOutInSeconds"></param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool SetConnectionTimeout(uint timeOutInSeconds);

        /// <summary>
        /// Add the new filter rule to the filter driver.
        /// </summary>
        /// <param name="accessFlag">access control rights of the file IO to the files which match the filter mask</param>
        /// <param name="filterMask">the filter rule file filter mask, it must be unique.</param>
        /// <param name="isResident">if it is true, the filter rule will be added to the registry, get protection in boot time.</param>
        /// <param name="filterRuleId">the id to identify the filter rule, it will show up in messageId field of messageSend structure if the callback is registered</param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AddFileFilterRule(
         uint accessFlag,
        [MarshalAs(UnmanagedType.LPWStr)]string filterMask,
         bool isResident,
         uint filterRuleId );


        /// <summary>
        ///Set an encryption folder, every encrypted file has the unique iv key, 
        ///the encrypted information was embedded into to the file as an extended attribute called reparse point, 
        ///it only can be supported in NTFS. The same folder can mix encrypted files and unencrypted files,
        ///the filter driver will know if the file was encrypted by checking the file reparse point attribute.
        ///Since the reparse point attribute can’t be transferred, the encrypted file can’t be shared or copied outside of your computer with normal method, 
        ///you need to append the reparse point data to the end of the file, and recreate the new reparse point data to the new file after you copied it to the new computer.
        /// </summary>
        /// <param name="filterMask"></param>
        /// <param name="encryptionKeyLength"></param>
        /// <param name="encryptionKey"></param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AddEncryptionKeyToFilterRule(
        [MarshalAs(UnmanagedType.LPWStr)]string filterMask,
         uint encryptionKeyLength,
         byte[] encryptionKey);

        /// <summary>
        ///Set an encryption folder, assume all files inside the folder were encrypted, all encrypted files use the same encryption key and IV key. 
        ///The encryption file doesn’t embed any encryption information, you can’t tell if the file was encrypted or not by checking the file information, 
        ///you can share or transfer the encrypted file without limitation. To check if the file was encrypted, you need to stop the encryption filter driver service, 
        ///then open the encrypted file, you will get cipher data instead of the clear data. 
        /// </summary>
        /// <param name="filterMask"></param>
        /// <param name="encryptionKeyLength"></param>
        /// <param name="encryptionKey"></param>
        /// <param name="ivLength"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AddEncryptionKeyAndIVToFilterRule(
        [MarshalAs(UnmanagedType.LPWStr)]string filterMask,
         uint encryptionKeyLength,
         byte[] encryptionKey,
         uint ivLength,
         byte[] iv);

        /// <summary>
        /// Exclude the file IO from this filter rule if the files match the excludeFileFilterMask.
        /// </summary>
        /// <param name="filterMask">the filter rule file filter mask</param>
        /// <param name="excludeFileFilterMask">the file filter mask for the excluded files</param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AddExcludeFileMaskToFilterRule(
        [MarshalAs(UnmanagedType.LPWStr)]string filterMask,
        [MarshalAs(UnmanagedType.LPWStr)]string excludeFileFilterMask);

        /// <summary>
        /// Hide the files from the browsing file list for the filter rule.
        /// </summary>
        /// <param name="filterMask">the filter rule file filter mask.</param>
        /// <param name="hiddenFileFilterMask">the file filter mask for the hidden files.</param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AddHiddenFileMaskToFilterRule(
        [MarshalAs(UnmanagedType.LPWStr)]string filterMask,
        [MarshalAs(UnmanagedType.LPWStr)]string hiddenFileFilterMask);

        /// <summary>
        /// reparse the file open to the other file if the file matches the file filter mask.
        /// 
        ///For example:
        ///FilterMask = c:\test\*txt
        ///ReparseFilterMask = d:\reparse\*doc
        ///If you open file c:\test\MyTest.txt, it will reparse to the file d:\reparse\MyTest.doc.
        /// </summary>
        /// <param name="filterMask">the filter rule file filter mask</param>
        /// <param name="reparseFileFilterMask">reparse file filter mask</param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AddReparseFileMaskToFilterRule(
        [MarshalAs(UnmanagedType.LPWStr)]string filterMask,
        [MarshalAs(UnmanagedType.LPWStr)]string reparseFileFilterMask);

        /// <summary>
        /// only manage the IO of the filter rule for the processes in the included process list 
        /// </summary>
        /// <param name="filterMask">the file filter mask of the filter rule</param>
        /// <param name="processName">the include process name filter mask, process name format:notepad.exe</param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AddIncludeProcessNameToFilterRule(
        [MarshalAs(UnmanagedType.LPWStr)]string filterMask,
        [MarshalAs(UnmanagedType.LPWStr)]string processName);

        /// <summary>
        /// skip the IO of the filter rule for the processes in the excluded process list
        /// </summary>
        /// <param name="filterMask">the file filter mask of the filter rule</param>
        /// <param name="processName">the include process name filter mask</param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AddExcludeProcessNameToFilterRule(
        [MarshalAs(UnmanagedType.LPWStr)]string filterMask,
        [MarshalAs(UnmanagedType.LPWStr)]string processName);

        /// <summary>
        /// only manage the IO of the filter rule for the processes in the included process id list 
        /// </summary>
        /// <param name="filterMask">the file filter mask of the filter rule</param>
        /// <param name="includeProcessId">the included process Id</param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AddIncludeProcessIdToFilterRule(
        [MarshalAs(UnmanagedType.LPWStr)]string filterMask,
        uint includeProcessId);

        /// <summary>
        /// skip the IO of the filter rule for the processes in the excluded process id list
        /// </summary>
        /// <param name="filterMask">the file filter mask of the filter rule</param>
        /// <param name="excludeProcessId">the excluded process Id</param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AddExcludeProcessIdToFilterRule(
        [MarshalAs(UnmanagedType.LPWStr)]string filterMask,
        uint excludeProcessId);

        /// <summary>
        ///  only manage the IO of the filter rule for user name in the included user name list 
        /// </summary>
        /// <param name="filterMask">the file filter mask of the filter rule</param>
        /// <param name="userName">the included user name, format:domainName(or computerName)\userName.exe</param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AddIncludeUserNameToFilterRule(
        [MarshalAs(UnmanagedType.LPWStr)]string filterMask,
        [MarshalAs(UnmanagedType.LPWStr)]string userName);

        /// <summary>
        ///skip the IO of the filter rule for user name in the excluded user name list 
        /// </summary>
        /// <param name="filterMask">the file filter mask of the filter rule</param>
        /// <param name="userName">the excluded user name, format:domainName(or computerName)\userName.exe</param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AddExcludeUserNameToFilterRule(
        [MarshalAs(UnmanagedType.LPWStr)]string filterMask,
        [MarshalAs(UnmanagedType.LPWStr)]string userName);

        /// <summary>
        /// Register the I/O event types for the filter rule, get the notification when the I/O was triggered.
        /// </summary>
        /// <param name="filterMask">the file filter mask of the filter rule</param>
        /// <param name="eventType">the I/O event types,reference the FileEventType enumeration.</param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool RegisterEventTypeToFilterRule(
        [MarshalAs(UnmanagedType.LPWStr)]string filterMask,
        uint eventType);

        /// <summary>
        /// Register the callback I/O for monitor filter driver to the filter rule.
        /// </summary>
        /// <param name="filterMask">the file filter mask of the filter rule</param>
        /// <param name="registerIO">the specific I/Os you want to monitor</param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool RegisterMoinitorIOToFilterRule(
        [MarshalAs(UnmanagedType.LPWStr)]string filterMask,
        uint registerIO);

        /// <summary>
        /// Register the callback I/O for control filter driver to the filter rule, you can change, block and pass the I/O
        /// in your callback funtion.
        /// </summary>
        /// <param name="filterMask">the file filter mask of the filter rule</param>
        /// <param name="registerIO">the specific I/Os you want to monitor or control</param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool RegisterControlIOToFilterRule(
        [MarshalAs(UnmanagedType.LPWStr)]string filterMask,
        uint registerIO);

        /// <summary>
        /// Filter the register I/O, only when the file I/Os open with the filter options,the callback I/O will be triggerred
        /// and send to the user mode service, for example you only want to get callback for the file open with write access.
        /// </summary>
        /// <param name="filterMask">the file filter mask of the filter rule</param>
        /// <param name="filterByDesiredAccess">if it is not 0, callback when file opens with this DesiredAccess</param>
        /// <param name="filterByDisposition">if it is not 0, callback when file opens with this Disposition</param>
        /// <param name="filterByCreateOptions">if it is not 0, callback when file opens with this CreateOptions</param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AddRegisterIOFilterToFilterRule(
        [MarshalAs(UnmanagedType.LPWStr)]string filterMask,
        uint filterByDesiredAccess,
        uint filterByDisposition,
        uint filterByCreateOptions);

        /// <summary>
        /// Set the access rigths to the specific process
        /// </summary>
        /// <param name="filterMask">the file filter mask of the filter rule</param>
        /// <param name="processName">the process name will be added the access rights, e.g. notepad.exe or c:\windows\*.exe</param>
        /// <param name="accessFlags">the access rights</param>
        /// <returns>return true if it succeeds</returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AddProcessRightsToFilterRule(
        [MarshalAs(UnmanagedType.LPWStr)]string filterMask,
        [MarshalAs(UnmanagedType.LPWStr)]string processName,
        uint accessFlags);

        /// <summary>
        /// Remove the acces right setting for specific processes from filter rule
        /// </summary>
        /// <param name="filterMask">tthe filter rule file filter mask</param>
        /// <param name="processName">the process name filter mask</param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool RemoveProcessRightsFromFilterRule(
        [MarshalAs(UnmanagedType.LPWStr)]string filterMask,
        [MarshalAs(UnmanagedType.LPWStr)]string processName);

        /// <summary>
        /// Set the access control flags to process with the processId
        /// </summary>
        /// <param name="filterMask">the filter rule file filter mask</param>
        /// <param name="processId">the process Id which will be added the access control flags</param>
        /// <param name="accessFlags">the access control flags</param>
        /// <returns>return true if it succeeds</returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AddProcessIdRightsToFilterRule(
        [MarshalAs(UnmanagedType.LPWStr)]string filterMask,
        uint processId,
        uint accessFlags);

        /// <summary>
        /// Remove the acces right setting for specific process from filter rule
        /// </summary>
        /// <param name="filterMask">the filter rule file filter mask</param>
        /// <param name="processName">the process Id</param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool RemoveProcessIdRightsFromFilterRule(
        [MarshalAs(UnmanagedType.LPWStr)]string filterMask,
        uint processId);

        /// <summary>
        /// Set the access control rights to specific users
        /// </summary>
        /// <param name="filterMask">the filter rule file filter mask</param>
        /// <param name="userName">the user name you want to set the access right</param>
        /// <param name="accessFlags">the access rights</param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AddUserRightsToFilterRule(
        [MarshalAs(UnmanagedType.LPWStr)]string filterMask,
        [MarshalAs(UnmanagedType.LPWStr)]string userName,
        uint accessFlags);

        /// <summary>
        /// Add the boolean config setting to a filter rule.
        /// Reference BooleanConfig settings
        /// </summary>
        /// <param name="filterMask">the filter rule file filter mask</param>
        /// <param name="booleanConfig">the boolean config setting</param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AddBooleanConfigToFilterRule(
        [MarshalAs(UnmanagedType.LPWStr)]string filterMask,
        uint booleanConfig);

        /// <summary>
        /// Remove the filter rule from the filter driver.
        /// </summary>
        /// <param name="filterMask">the filter rule file filter mask</param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool RemoveFilterRule(
        [MarshalAs(UnmanagedType.LPWStr)] string filterMask);

        /// <summary>
        /// Add the process Id to include process list, only the process Id in the list will be managed by the filter driver,
        /// or all the file IO from the process Id which is not in the list will be skipped. This is the global settings, it will affect all filter rules.
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AddIncludedProcessId(uint processId);

        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool RemoveIncludeProcessId(uint processId);

        /// <summary>
        ///Add the process Id to the exclude process list, all the file IO from the process Id which is in the excluded list will be skipped. 
        ///This is the global settings, it will affect all filter rules. 
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AddExcludedProcessId(uint processId);

        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool RemoveExcludeProcessId(uint processId);


        //---------------Registry access control APIs-----------------------------------
        /// <summary>
        /// The maximum registry access right flag
        /// </summary>
        public const uint MAX_REGITRY_ACCESS_FLAG = 0xFFFFFFFF;

        /// <summary>
        /// Allow read registry access right
        /// </summary>
        public const uint ALLOW_READ_REGITRY_ACCESS_FLAG = (uint)(RegControlFlag.REG_ALLOW_OPEN_KEY
            | RegControlFlag.REG_ALLOW_QUERY_KEY | RegControlFlag.REG_ALLOW_ENUMERATE_KEY | RegControlFlag.REG_ALLOW_QUERY_VALUE_KEY
            | RegControlFlag.REG_ALLOW_QUERY_KEY_SECURITY | RegControlFlag.REG_ALLOW_QUERY_KEYNAME);

        /// <summary>
        /// Allow change registry access right
        /// </summary>
        public const uint ALLOW_CHANGE_REGITRY_ACCESS_FLAG = (uint)(RegControlFlag.REG_ALLOW_CREATE_KEY
            | RegControlFlag.REG_ALLOW_RENAME_KEY | RegControlFlag.REG_ALLOW_DELETE_KEY | RegControlFlag.REG_ALLOW_SET_VALUE_KEY_INFORMATION
            | RegControlFlag.REG_ALLOW_DELETE_VALUE_KEY | RegControlFlag.REG_ALLOW_SET_KEY_SECRUITY | RegControlFlag.REG_ALLOW_RESTORE_KEY
            | RegControlFlag.REG_ALLOW_REPLACE_KEY | RegControlFlag.REG_ALLOW_LOAD_KEY);

        /// <summary>
        /// the registry access control flag, allow or deny the registry access based on the registry filter rule.
        /// </summary>
        public enum RegControlFlag : uint
        {
            REG_ALLOW_OPEN_KEY = 0x00000001,
            REG_ALLOW_CREATE_KEY = 0x00000002,
            REG_ALLOW_QUERY_KEY = 0x00000004,
            REG_ALLOW_RENAME_KEY = 0x00000008,
            REG_ALLOW_DELETE_KEY = 0x00000010,
            REG_ALLOW_SET_VALUE_KEY_INFORMATION = 0x00000020,
            REG_ALLOW_SET_INFORMATION_KEY = 0x00000040,
            REG_ALLOW_ENUMERATE_KEY = 0x00000080,
            REG_ALLOW_QUERY_VALUE_KEY = 0x00000100,
            REG_ALLOW_ENUMERATE_VALUE_KEY = 0x00000200,
            REG_ALLOW_QUERY_MULTIPLE_VALUE_KEY = 0x00000400,
            REG_ALLOW_DELETE_VALUE_KEY = 0x00000800,
            REG_ALLOW_QUERY_KEY_SECURITY = 0x00001000,
            REG_ALLOW_SET_KEY_SECRUITY = 0x00002000,
            REG_ALLOW_RESTORE_KEY = 0x00004000,
            REG_ALLOW_REPLACE_KEY = 0x00008000,
            REG_ALLOW_SAVE_KEY = 0x00010000,
            REG_ALLOW_FLUSH_KEY = 0x00020000,
            REG_ALLOW_LOAD_KEY = 0x00040000,
            REG_ALLOW_UNLOAD_KEY = 0x00080000,
            REG_ALLOW_KEY_CLOSE = 0x00100000,
            REG_ALLOW_QUERY_KEYNAME = 0x00200000,
        }       

        /// <summary>
        /// this is the value which will register all the registry notification callback class.
        /// </summary>
        public const ulong MAX_REG_CALLBACK_CLASS = 0xFFFFFFFFFFFFFFFF;

        /// <summary>
        /// the registry callback class, you can block or modify the registry access in pre-callback
        /// or monitor the registry access in post-callback
        /// </summary>
        public enum RegCallbackClass : ulong
        {
            Reg_Pre_Delete_Key = 0x00000001,
            Reg_Pre_Set_Value_Key = 0x00000002,
            Reg_Pre_Delete_Value_Key = 0x00000004,
            Reg_Pre_SetInformation_Key = 0x00000008,
            Reg_Pre_Rename_Key = 0x00000010,
            Reg_Pre_Enumerate_Key = 0x00000020,
            Reg_Pre_Enumerate_Value_Key = 0x00000040,
            Reg_Pre_Query_Key = 0x00000080,
            Reg_Pre_Query_Value_Key = 0x00000100,
            Reg_Pre_Query_Multiple_Value_Key = 0x00000200,
            Reg_Pre_Create_Key = 0x00000400,
            Reg_Post_Create_Key = 0x00000800,
            Reg_Pre_Open_Key = 0x00001000,
            Reg_Post_Open_Key = 0x00002000,
            Reg_Pre_Key_Handle_Close = 0x00004000,
            //
            // .Net only
            //    
            Reg_Post_Delete_Key = 0x00008000,
            Reg_Post_Set_Value_Key = 0x00010000,
            Reg_Post_Delete_Value_Key = 0x00020000,
            Reg_Post_SetInformation_Key = 0x00040000,
            Reg_Post_Rename_Key = 0x00080000,
            Reg_Post_Enumerate_Key = 0x00100000,
            Reg_Post_Enumerate_Value_Key = 0x00200000,
            Reg_Post_Query_Key = 0x00400000,
            Reg_Post_Query_Value_Key = 0x00800000,
            Reg_Post_Query_Multiple_Value_Key = 0x01000000,
            Reg_Post_Key_Handle_Close = 0x02000000,
            Reg_Pre_Create_KeyEx = 0x04000000,
            Reg_Post_Create_KeyEx = 0x08000000,
            Reg_Pre_Open_KeyEx = 0x10000000,
            Reg_Post_Open_KeyEx = 0x20000000,
            //
            // new to Windows Vista
            //
            Reg_Pre_Flush_Key = 0x40000000,
            Reg_Post_Flush_Key = 0x80000000,
            Reg_Pre_Load_Key = 0x100000000,
            Reg_Post_Load_Key = 0x200000000,
            Reg_Pre_UnLoad_Key = 0x400000000,
            Reg_Post_UnLoad_Key = 0x800000000,
            Reg_Pre_Query_Key_Security = 0x1000000000,
            Reg_Post_Query_Key_Security = 0x2000000000,
            Reg_Pre_Set_Key_Security = 0x4000000000,
            Reg_Post_Set_Key_Security = 0x8000000000,
            //
            // per-object context cleanup
            //
            Reg_Callback_Object_Context_Cleanup = 0x10000000000,
            //
            // new in Vista SP2 
            //
            Reg_Pre_Restore_Key = 0x20000000000,
            Reg_Post_Restore_Key = 0x40000000000,
            Reg_Pre_Save_Key = 0x80000000000,
            Reg_Post_Save_Key = 0x100000000000,
            Reg_Pre_Replace_Key = 0x200000000000,
            Reg_Post_Replace_Key = 0x400000000000,

            //
            // new in Windows 10 
            //
            Reg_Pre_Query_KeyName = 0x800000000000,
            Reg_Post_Query_KeyName = 0x1000000000000,

        }

        /// <summary>
        /// Add registry access control filter entry with process name filter mask, user name filter mask and registry
        /// key filter mask
        /// </summary>
        /// <param name="processNameLength">The length of the process name string in bytes</param>
        /// <param name="processName">The process name to be filtered, all processes if it is '*' </param>
        /// <param name="processId">set the processId if you want filter with id instead of the process name</param>
        /// <param name="userNameLength">the user name length if you want to filter the user name</param>
        /// <param name="userName">the user name filter mask</param>
        /// <param name="registryKeyNameLength">set the registry key name filter if you want to filter by the key name</param>
        /// <param name="registryKeyNameFilterMask">the registry key name filter mask</param>
        /// <param name="accessFlag">The access control flag to the registry</param>
        /// <param name="regCallbackClass">The registered callback registry access class</param>
        /// <param name="isExcludeFilter">Skip all the registry access from this process filter mask if it is true.</param>
        /// <returns>return true if the entry was added successfully</returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AddRegistryFilterRule(
            uint processNameLength,
            [MarshalAs(UnmanagedType.LPWStr)]string processName,
            uint processId,
            uint userNameLength,
            [MarshalAs(UnmanagedType.LPWStr)]string userName,
            uint registryKeyNameLength,
            [MarshalAs(UnmanagedType.LPWStr)]string registryKeyNameFilterMask,
            uint accessFlag,
            ulong regCallbackClass,
            bool isExcludeFilter);

        /// <summary>
        /// Add registry access control filter entry with process name, if process name filter mask matches the proces,
        /// it will set the access flag to the process.
        /// </summary>
        /// <param name="processNameLength">The length of the process name string in bytes</param>
        /// <param name="processName">The process name to be filtered, all processes if it is '*' </param>
        /// <param name="accessFlag">The access control flag to the registry</param>
        /// <param name="regCallbackClass">The registered callback registry access class</param>
        /// <param name="isExcludeFilter">Skip all the registry access from this process filter mask if it is true.</param>
        /// <returns>return true if the entry was added successfully</returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AddRegistryFilterRuleByProcessName(
            uint processNameLength,
            [MarshalAs(UnmanagedType.LPWStr)]string processName,
            uint accessFlag,
            ulong regCallbackClass,
            bool isExcludeFilter);

        /// <summary>
        /// Add registry access control filter entry with processId, if processId  matches the proces,
        /// it will set the access flag to the process.
        /// </summary>
        /// <param name="processId">The process Id to be filtered</param>
        /// <param name="accessFlag">The access control flag to the registry</param>
        /// <param name="regCallbackClass">The registered callback registry access class</param>
        /// <param name="isExcludeFilter">Skip all the registry access from this process filter mask if it is true.</param>
        /// <returns>return true if the entry was added successfully</returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AddRegistryFilterRuleByProcessId(
            uint processId,
            uint accessFlag,
            ulong regCallbackClass,
            bool isExcludeFilter);

        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool RemoveRegistryFilterRuleByProcessId(
            uint processId);

        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool RemoveRegistryFilterRuleByProcessName(
            uint processNameLength,
            [MarshalAs(UnmanagedType.LPWStr)]string processName);

        //---------------Registry access control APIs END-----------------------------------

       //---------------Process filter APIs-----------------------------------------------
        /// <summary>
        /// process control flag.
        /// </summary>
        public enum ProcessControlFlag : uint
        {
            /// <summary>
            /// deny the new process creation if the flag is on
            /// </summary>
            DENY_NEW_PROCESS_CREATION = 0x00000001,
            /// <summary>
            /// Get a notification when a new process is being created.
            /// </summary>
            PROCESS_CREATION_NOTIFICATION = 0x00000100,
            /// <summary>
            ///get a notification when a process was termiated 
            /// </summary>
            PROCESS_TERMINATION_NOTIFICATION = 0x00000200,
            /// <summary>
            /// get a notification for process handle operations, when a handle for a process
            /// is being created or duplicated.
            /// </summary>
            PROCESS_HANDLE_OP_NOTIFICATION = 0x00000400,
            /// <summary>
            /// get a notifcation when a new thread is being created.
            /// </summary>
            THREAD_CREATION_NOTIFICATION = 0x00000800,
            /// <summary>
            /// get a notification when a thread was termiated 
            /// </summary>
            THREAD_TERMINIATION_NOTIFICATION = 0x00001000,
            /// <summary>
            /// get a notification for thread handle operations, when a handle for a thread
            /// is being created or duplicated.
            /// </summary>
            THREAD_HANDLE_OP_NOTIFICATION = 0x00002000,
        }

        /// <summary>
        /// Add the process filter entry,get notification of new process/thread creation or termination.
        /// prevent the unauthorized excutable binaries from running.
        /// </summary>
        /// <param name="processNameMaskLength">the process name mask length</param>
        /// <param name="processNameMask">the process name mask, i.e. c:\myfolder\*.exe</param>
        /// <param name="controlFlag">the control flag of the process</param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AddProcessFilterRule( 
        uint processNameMaskLength,
        [MarshalAs(UnmanagedType.LPWStr)]string processNameMask,
        uint controlFlag,
        uint filterRuleId );

        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool RemoveProcessFilterRule(
        uint processNameMaskLength,
        [MarshalAs(UnmanagedType.LPWStr)]string processNameMask);

        /// <summary>
        /// Add the file control access rights to the process
        /// </summary>
        /// <param name="processNameMaskLength">the length of the process name filter mask</param>
        /// <param name="processNameMask">the process name filter mask</param>
        /// <param name="fileNameMaskLength">the length of the file name filter mask</param>
        /// <param name="fileNameMask">the file name filter mask</param>
        /// <param name="AccessFlag">set the file access control flag if the control filter is enabled</param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AddFileControlToProcessByName(	
        uint processNameMaskLength,
        [MarshalAs(UnmanagedType.LPWStr)]string processNameMask,
        uint fileNameMaskLength,
        [MarshalAs(UnmanagedType.LPWStr)]string fileNameMask,
        uint AccessFlag);

        /// <summary>
        /// Remove the file access rights from the process name
        /// </summary>
        /// <param name="processNameMaskLength"></param>
        /// <param name="processNameMask"></param>
        /// <param name="fileNameMaskLength"></param>
        /// <param name="fileNameMask"></param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool RemoveFileControlFromProcessByName(
        uint processNameMaskLength,
        [MarshalAs(UnmanagedType.LPWStr)]string processNameMask,
        uint fileNameMaskLength,
        [MarshalAs(UnmanagedType.LPWStr)]string fileNameMask);

        /// <summary>
        /// register the file callback IO for the process with the filter option.
        /// </summary>
        /// <param name="processNameMaskLength">the length of the process name filter mask</param>
        /// <param name="processNameMask">the process name filter mask</param>
        /// <param name="fileNameMaskLength">the length of the file name filter mask</param>
        /// <param name="fileNameMask">the file name filter mask</param>
        /// <param name="monitorIOs">register the monitor IO callback if the monitor filter driver is enabled</param>
        /// <param name="controlIOs">register the control IO callback if the control filter driver is enabled</param>
        /// <param name="filterByDesiredAccess">if it is not 0, callback when file opens with this DesiredAccess</param>
        /// <param name="filterByDisposition">if it is not 0, callback when file opens with this Disposition</param>
        /// <param name="filterByCreateOptions">if it is not 0, callback when file opens with this CreateOptions</param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AddFileCallbackIOToProcessByName(
        uint processNameMaskLength,
        [MarshalAs(UnmanagedType.LPWStr)]string processNameMask,
        uint fileNameMaskLength,
        [MarshalAs(UnmanagedType.LPWStr)]string fileNameMask,
        uint monitorIO,
        uint controlIO,
        uint filterByDesiredAccess,
        uint filterByDisposition,
        uint filterByCreateOptions);

        /// <summary>
        /// This is the API to add the file access rights of the specific files to the specific processes by process Id
        /// This feature requires the control filter was enabled
        /// </summary>
        /// <param name="processId">the process Id it will be filtered</param>
        /// <param name="fileNameMaskLength">the length of the file name filter mask</param>
        /// <param name="fileNameMask">the file name filter mask</param>
        /// <param name="AccessFlag">the file access control flag</param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AddFileControlToProcessById(	
        uint processId,
        uint fileNameMaskLength,
        [MarshalAs(UnmanagedType.LPWStr)]string fileNameMask,
        uint AccessFlag );

        /// <summary>
        /// Remove the file access entry by process Id
        /// </summary>
        /// <param name="processId"></param>
        /// <param name="fileNameMaskLength"></param>
        /// <param name="fileNameMask"></param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool RemoveFileControlFromProcessById(
        uint processId,
        uint fileNameMaskLength,
        [MarshalAs(UnmanagedType.LPWStr)]string fileNameMask);

        /// <summary>
        /// prevent the process  being terminated, only support OS vista or later version.
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AddProtectedProcessId(uint processId);

        /// <summary>
        /// prevent the process from creating the new file after it opened the protected file.
        /// the process can't save the protected file to the other file name.
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AddBlockSaveAsProcessId(uint processId);

        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool RemoveBlockSaveAsProcessId(uint processId);

        //---------------Process filter APIs   END-----------------------------------------------

        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool RegisterIoRequest(uint requestRegistration);

        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool GetFileHandleInFilter(
             [MarshalAs(UnmanagedType.LPWStr)]string fileName,
             uint dwDesiredAccess,
             ref IntPtr fileHandle);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool ConvertSidToStringSid(
            [In] IntPtr sid,
            [Out] out IntPtr sidString);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LocalFree(IntPtr hMem);

        [DllImport("kernel32", SetLastError = true)]
        public static extern uint GetCurrentProcessId();

        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int QueryDosDeviceW(
        [MarshalAs(UnmanagedType.LPWStr)]string dosName,
        [MarshalAs(UnmanagedType.LPWStr)]ref string volumeName,
        int volumeNameLength);

        [DllImport("FilterAPI.dll", SetLastError = true)]
        private static extern bool CreateFileAPI(
             [MarshalAs(UnmanagedType.LPWStr)]string fileName,
              uint dwDesiredAccess,
              uint dwShareMode,
              uint dwCreationDisposition,
              uint dwFlagsAndAttributes,
              ref IntPtr fileHandle);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool SetFileTime(SafeFileHandle hFile,
                                        [In] ref long lpCreationTime,
                                        [In] ref long lpLastAccessTime,
                                        [In] ref long lpLastWriteTime);

        [DllImport("FilterAPI.dll", SetLastError = true)]
        private static extern bool CreateStubFile(
             [MarshalAs(UnmanagedType.LPWStr)]string fileName,
             long fileSize,  //if it is 0 and the file exist,it will use the current file size.
              uint fileAttributes, //if it is 0 and the file exist, it will use the current file attributes.
              uint tagDataLength, //if it is 0, then no reparsepoint will be created.
              IntPtr tagData,
              bool overwriteIfExist,
              ref IntPtr fileHandle);

        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool OpenStubFile(
            [MarshalAs(UnmanagedType.LPWStr)]string fileName,
             FileAccess access,
             FileShare share,
             ref IntPtr fileHandle);

        [DllImport("FilterAPI.dll", SetLastError = true)]
        private static extern bool QueryAllocatedRanges(
                IntPtr fileHandle,
                long queryOffset,
                long queryLength,
                IntPtr allocatedRangesBuffer,
                int allocatedRangesBufferSize,
                ref uint returnedLength);

        [DllImport("FilterAPI.dll", SetLastError = true)]
        private static extern bool AESEncryptDecryptBuffer(
                IntPtr inputBuffer,
                IntPtr outputBuffer,
                uint bufferLength,
                long offset,
                byte[] encryptionKey,
                uint keyLength,
                byte[] iv,
                uint ivLength);

        /// <summary>
        /// Encrypt the file,  if addHeader is true then the iv data will be embedded to the encrypted file,
        /// or there are no meta data attached.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="keyLength"></param>
        /// <param name="encryptionKey"></param>
        /// <param name="ivLength"></param>
        /// <param name="iv"></param>
        /// <param name="addHeader"></param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AESEncryptFile(
             [MarshalAs(UnmanagedType.LPWStr)]string fileName,
             uint keyLength,
             byte[] encryptionKey,
             uint ivLength,
             byte[] iv,
             bool addHeader);

         /// <summary>
        /// Encrypt the file, the iv data and the tag data will be embedded to the encrypted file.
         /// </summary>
         /// <param name="fileName"></param>
         /// <param name="keyLength"></param>
         /// <param name="encryptionKey"></param>
         /// <param name="ivLength"></param>
         /// <param name="tagDataLength"></param>
         /// <param name="tagData"></param>
         /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AESEncryptFileWithTag(
             [MarshalAs(UnmanagedType.LPWStr)]string fileName,
             uint keyLength,
             byte[] encryptionKey,
             uint ivLength,
             byte[] iv,
             uint tagDataLength,
             byte[] tagData);   

        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AESDecryptFile(
             [MarshalAs(UnmanagedType.LPWStr)]string fileName,
             uint keyLength,
             byte[] encryptionKey,
             uint ivLength,
             byte[] iv);

         /// <summary>
        /// Encrypt the source file to the dest file, if addIVTag is true then the iv data will be embedded to the encrypted file,
        /// or there are no meta data attached.
         /// </summary>
         /// <param name="sourceFileName"></param>
         /// <param name="destFileName"></param>
         /// <param name="keyLength"></param>
         /// <param name="encryptionKey"></param>
         /// <param name="ivLength"></param>
         /// <param name="iv"></param>
         /// <param name="addIVTag"></param>
         /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AESEncryptFileToFile(
             [MarshalAs(UnmanagedType.LPWStr)]string sourceFileName,
             [MarshalAs(UnmanagedType.LPWStr)]string destFileName,
             uint keyLength,
             byte[] encryptionKey,
             uint ivLength,
             byte[] iv,
             bool addIVTag);

        /// <summary>
        /// Encrypt the source file to the dest file, the iv data and the tag data will be embedded to the encrypted file.
        /// </summary>
        /// <param name="sourceFileName"></param>
        /// <param name="destFileName"></param>
        /// <param name="keyLength"></param>
        /// <param name="encryptionKey"></param>
        /// <param name="ivLength"></param>
        /// <param name="iv"></param>
        /// <param name="tagDataLength"></param>
        /// <param name="tagData"></param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AESEncryptFileToFileWithTag(
             [MarshalAs(UnmanagedType.LPWStr)]string sourceFileName,
             [MarshalAs(UnmanagedType.LPWStr)]string destFileName,
             uint keyLength,
             byte[] encryptionKey,
             uint ivLength,
             byte[] iv,
             uint tagDataLength,
             byte[] tagData);   

        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AESDecryptFileToFile(
             [MarshalAs(UnmanagedType.LPWStr)]string sourceFileName,
             [MarshalAs(UnmanagedType.LPWStr)]string destFileName,
             uint keyLength,
             byte[] encryptionKey,
             uint ivLength,
             byte[] iv);

        /// <summary>
        /// Set the AES Data to the encrypted file
        /// </summary>
        /// <param name="fileName">the encrypted file name</param>
        /// <param name="headerSize">the size of the AESData</param>
        /// <param name="header">the AESData structure</param>
        /// <returns></returns>
         [DllImport("FilterAPI.dll", SetLastError = true)]
         public static extern bool AddAESHeader(
             [MarshalAs(UnmanagedType.LPWStr)]string fileName,
             uint headerSize,
             byte[] header);

          /// <summary>
        /// get the AES Data from the encrypted file
        /// </summary>
        /// <param name="fileName">the encrypted file name</param>
        /// <param name="headerSize">the size of the AESData</param>
        /// <param name="header">the byte array to store the AESData structure</param>
        /// <returns></returns>
         [DllImport("FilterAPI.dll", SetLastError = true)]
         public static extern bool GetAESHeader(
             [MarshalAs(UnmanagedType.LPWStr)]string fileName,
             ref uint headerSize, 
             byte[] header);

        /// <summary>
        /// Set the AESFlags and AccessFlags in the AES header
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="aesFlags"></param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
         public static extern bool SetHeaderFlags(
             [MarshalAs(UnmanagedType.LPWStr)]string fileName,
             uint aesFlags,
             uint accessFlags );

        /// <summary>
        /// Get the tag data which was set in the AES header
        /// </summary>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool GetAESTagData(
            [MarshalAs(UnmanagedType.LPWStr)]string fileName,
            ref uint tagDataSize,
            byte[] tagData);

        /// <summary>
        /// Get the IV data from an encrypted file's header
        /// </summary>
        /// <param name="fileName">the encrypted file name</param>
        /// <param name="ivSize">the pass in/out iv buffer size</param>
        /// <param name="ivBuffer">the iv buffer</param>
        /// <returns>return true if it gets the right iv data</returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool GetAESIV(
            [MarshalAs(UnmanagedType.LPWStr)]string fileName,
            ref uint ivSize,
            byte[] ivBuffer);


        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AddReparseTagData(
            [MarshalAs(UnmanagedType.LPWStr)]string fileName,
            int tagDataLength,
            IntPtr tagData);

        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool RemoveTagData(
              IntPtr fileHandle);

        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AddTagData(
              IntPtr fileHandle,
              int tagDataLength,
              IntPtr tagData);

        public static bool EmbedDRPolicyDataToFile(
              string fileName,
              byte[] drPolicyData,
              out string lastError )
        {

            bool ret = false;
            lastError = string.Empty;

            try
            {              
                GCHandle pinnedArray = GCHandle.Alloc(drPolicyData, GCHandleType.Pinned);
                IntPtr pointer = pinnedArray.AddrOfPinnedObject();

                ret = AddReparseTagData(fileName, drPolicyData.Length, pointer);

                pinnedArray.Free();

                if (!ret)
                {
                    lastError = GetLastErrorMessage();
                }
            }
            catch (Exception ex)
            {
                ret = false;
                lastError = ex.Message;
            }
          
            return ret;

        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr handle);

        /// <summary>
        /// Return true if it succeeds to check the iv tag, if ivLenght > 0, it returns ivTag, or there are no ivTag data.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="ivLength"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        private static extern bool GetIVTag(
             [MarshalAs(UnmanagedType.LPWStr)]string fileName,
             ref uint ivLength,
             IntPtr iv,
             ref uint aesFlag);

        /// <summary>
        /// the buffer length has to be 36 or more.
        /// </summary>
        /// <param name="outputBuffer"></param>
        /// <param name="outputBufferLength"></param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool GetUniqueComputerId(
                IntPtr outputBuffer,
                ref uint outputBufferLength);

        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool ActivateLicense(
                IntPtr outputBuffer,
                uint outputBufferLength);

        public enum EncryptType
        {
            Decryption = 0,
            Encryption ,
        }

        public static bool GetUniqueComputerId(ref string myComputerId,ref string lastError)
        {
            bool retVal = false;
            byte[] computerId = new byte[52];
            GCHandle gcHandle = GCHandle.Alloc(computerId, GCHandleType.Pinned);

            try
            {
                uint computerIdLength = (uint)computerId.Length;
                IntPtr computerIdPtr = Marshal.UnsafeAddrOfPinnedArrayElement(computerId, 0);
                retVal = FilterAPI.GetUniqueComputerId(computerIdPtr, ref computerIdLength);

                if (!retVal || computerIdLength <= 0)
                {
                    lastError = GetLastErrorMessage();
                    return false;
                }

                Array.Resize(ref computerId, (int)computerIdLength);
                myComputerId = UnicodeEncoding.Unicode.GetString(computerId);

                return true;
            }
            catch (Exception ex)
            {
                lastError = "Get computerId got exception,system return error:" + ex.Message;
                return false;
            }
            finally
            {
                gcHandle.Free();
            }

        }

        /// <summary>
        /// To open encrypted file without the filter driver interception, read the raw data with the return file handle.
        /// The caller is reponsible to close the file handle.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileHandle"></param>
        /// <param name="lastError"></param>
        /// <returns></returns>
        public static bool OpenRawEnCyptedFile(string fileName, out IntPtr fileHandle, out string lastError)
        {
            fileHandle = IntPtr.Zero;
            lastError = string.Empty;
            uint bypassFilterFileAttributes = FILE_FLAG_OPEN_REPARSE_POINT|FILE_FLAG_OPEN_NO_RECALL|FILE_FLAG_NO_BUFFERING|FILE_ATTRIBUTE_REPARSE_POINT;

            try
            {               
                if (!CreateFileAPI(fileName, (uint)FileAccess.Read, (uint)FileShare.None, (uint)FileMode.Open, bypassFilterFileAttributes, ref fileHandle))
                {
                    lastError = FilterAPI.GetLastErrorMessage();
                    return false;
                }
            }
            catch (Exception ex)
            {
                lastError = "OpenRawEnCyptedFile " + fileName + " got exception,system return error:" + ex.Message;
                return false;
            }

            return true;
        }


        //public static bool GetIVTag(string fileName, ref byte[] iv,ref uint aesFlag, out string lastError)
        //{
        //    bool ret = false;
        //    IntPtr tagPtr = IntPtr.Zero;
        //    uint ivLength = 16;

        //    lastError = string.Empty;

        //    tagPtr = Marshal.AllocHGlobal((int)ivLength);
        //    ret = GetIVTag(fileName, ref ivLength, tagPtr, ref aesFlag);

        //    if (!ret)
        //    {
        //        lastError = GetLastErrorMessage();
        //    }
        //    else if (ivLength > 0)
        //    {
        //        iv = new byte[ivLength];
        //        Marshal.Copy(tagPtr, iv, 0, (int)ivLength);
        //    }
        //    else
        //    {
        //        iv = new byte[0]; ;
        //    }

        //    if (tagPtr != IntPtr.Zero)
        //    {
        //        Marshal.FreeHGlobal(tagPtr);
        //    }

        //    return ret;
        //}


   
        public static string AESEncryptDecryptStr(string inStr, EncryptType encryptType)
        {
           
            if (string.IsNullOrEmpty(inStr))
            {
               return string.Empty;
            }

            byte[] inbuffer = null;

            if (encryptType == EncryptType.Encryption)
            {
                inbuffer = ASCIIEncoding.UTF8.GetBytes(inStr);
            }
            else if (encryptType == EncryptType.Decryption)
            {
                inbuffer = Convert.FromBase64String(inStr);
            }
            else
            {
                throw new Exception("Failed to encrypt decrypt string, the encryptType " + encryptType.ToString() + " doesn't know.");
            }

            byte[] outBuffer = new byte[inbuffer.Length];

            GCHandle gcHandleIn = GCHandle.Alloc(inbuffer, GCHandleType.Pinned);
            GCHandle gcHandleOut = GCHandle.Alloc(outBuffer, GCHandleType.Pinned);

            IntPtr inBufferPtr = Marshal.UnsafeAddrOfPinnedArrayElement(inbuffer, 0);
            IntPtr outBufferPtr = Marshal.UnsafeAddrOfPinnedArrayElement(outBuffer, 0);

            try
            {
                bool retVal = AESEncryptDecryptBuffer(inBufferPtr, outBufferPtr, (uint)inbuffer.Length, 0, null, 0, null, 0);

                if (encryptType == EncryptType.Encryption)
                {
                    return Convert.ToBase64String(outBuffer);
                }
                else //if (encryptType == EncryptType.Decryption)
                {
                    return ASCIIEncoding.UTF8.GetString(outBuffer);
                }
            }
            finally
            {
                gcHandleIn.Free();
                gcHandleOut.Free();
            }

        }

    
        public static void AESEncryptDecryptBuffer(byte[] inbuffer, long offset, byte[] key, byte[] IV)
        {
            if (null == inbuffer || inbuffer.Length == 0)
            {
                throw new Exception("Failed to encrypt decrypt buffer, the input buffer can't be null");
            }

            GCHandle gcHandle = GCHandle.Alloc(inbuffer, GCHandleType.Pinned);

            try
            {
                IntPtr inBufferPtr = Marshal.UnsafeAddrOfPinnedArrayElement(inbuffer, 0);

                uint keyLength = 0;
                uint IVLength = 0;

                if (key != null)
                {
                    keyLength = (uint)key.Length;
                }

                if (IV != null)
                {
                    IVLength = (uint)IV.Length;
                }


                bool retVal = AESEncryptDecryptBuffer(inBufferPtr, inBufferPtr, (uint)inbuffer.Length, offset, key, keyLength, IV, IVLength);

                if (!retVal)
                {
                    throw new Exception("Failed to encrypt buffer, return error:" + GetLastErrorMessage());
                }
            }
            finally
            {
                gcHandle.Free();
            }

            return ;
        }

        public static bool DecodeUserName(byte[]sid, out string userName)
        {
            bool ret = true;

            IntPtr sidStringPtr = IntPtr.Zero;
            string sidString = string.Empty;

            userName = string.Empty;
            
            try
            {
               
                IntPtr sidBuffer = Marshal.UnsafeAddrOfPinnedArrayElement(sid, 0);

                if (FilterAPI.ConvertSidToStringSid(sidBuffer, out sidStringPtr))
                {

                    sidString = Marshal.PtrToStringAuto(sidStringPtr);

                    lock (userNameTable)
                    {
                        //check the user name cache table
                        if (userNameTable.ContainsKey(sidString))
                        {
                            userName = userNameTable[sidString];
                            return ret;
                        }
                    }

                    try
                    {
                        SecurityIdentifier secIdentifier = new SecurityIdentifier(sidString);
                        IdentityReference reference = secIdentifier.Translate(typeof(NTAccount));
                        userName = reference.Value;
                    }
                    catch
                    {
                    }

                    lock (userNameTable)
                    {
                        //check the user name cache table
                        if (!userNameTable.ContainsKey(sidString))
                        {
                            userNameTable.Add(sidString, userName);
                        }
                    }
                }
                else
                {
                    string errorMessage = "Convert sid to sid string failed with error " + Marshal.GetLastWin32Error();
                    Console.WriteLine(errorMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Convert sid to user name got exception:{0}", ex.Message));
                ret = false;

            }
            finally
            {
                if (sidStringPtr != null && sidStringPtr != IntPtr.Zero)
                {
                    FilterAPI.LocalFree(sidStringPtr);
                }
            }

            return ret;
        }

        public static bool DecodeProcessName(uint processId, out string processName)
        {
            bool ret = true;
            processName = string.Empty;


            //this is the optimization of the process to get the process name from the process Id
            //it is not reliable for this process, since the process Id is reuasble when the process was ternmiated.
            lock (processNameTable)
            {
                if (processNameTable.ContainsKey(processId))
                {
                    processName = processNameTable[processId];
                    return true;
                }
            }

            try
            {
                System.Diagnostics.Process requestProcess = System.Diagnostics.Process.GetProcessById((int)processId);
                processName = requestProcess.ProcessName;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Convert pid to process name got exception:{0}", ex.Message));
                ret = false;
            }

            lock (processNameTable)
            {
                if (!processNameTable.ContainsKey(processId))
                {
                    processNameTable.Add(processId,processName);
                }
            }

            return ret;
        }


        public static string GetLastErrorMessage()
        {
            int len = 1024;
            string lastError = new string((char)0, len);

            if (!GetLastErrorMessage(lastError, ref len))
            {
                lastError = new string((char)0, len);
                if (!GetLastErrorMessage(lastError, ref len))
                {
                    return "failed to get last error message.";
                }
            }

            if (lastError.IndexOf((char)0) >= 0)
            {
                lastError = lastError.Substring(0, lastError.IndexOf((char)0));
            }

            return lastError;
        }

        static bool IsDriverChanged()
        {
            bool ret = false;

            try
            {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetEntryAssembly();
                string localPath = Path.GetDirectoryName(assembly.Location);
                string driverName = Path.Combine(localPath, "EaseFlt.sys");

                if (File.Exists(driverName))
                {
                    string driverInstalledPath = Path.Combine(Environment.SystemDirectory, "drivers\\easeflt.sys");

                    if (File.Exists(driverInstalledPath))
                    {
                        FileInfo fsInstalled = new FileInfo(driverInstalledPath);
                        FileInfo fsToInstall = new FileInfo(driverName);

                        if (fsInstalled.LastWriteTime < fsToInstall.LastWriteTime)
                        {
                            //it needs to install new the driver.
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }

            }
            catch (Exception ex)
            {
                ret = false;

                EventManager.WriteMessage(630, "IsDriverChanged", EventLevel.Error, "Check IsDriverChanged failed with error:" + ex.Message);
            }

            return ret;
        }

        static public bool StartFilter(int threadCount, string registerKey,FilterDelegate filterCallback, DisconnectDelegate disconnectCallback,ref string lastError)
        {
          
            bool ret = true;

            try
            {
                if (IsDriverChanged() || !FilterAPI.IsDriverServiceRunning())
                {
                    //uninstall or install driver needs the Admin permission.

                    FilterAPI.UnInstallDriver();

                    //wait for 3 seconds for the uninstallation completed.
                    System.Threading.Thread.Sleep(3000);

                    ret = FilterAPI.InstallDriver();
                    if (!ret)
                    {
                        lastError = "Installed driver failed with error:" + FilterAPI.GetLastErrorMessage();
                        return false;
                    }
                    else
                    {
                        isFilterStarted = false;
                        EventManager.WriteMessage(59, "InstallDriver", EventLevel.Information, "Install filter driver succeeded.");
                    }
                }


                if (!isFilterStarted)
                {

                    if (!SetRegistrationKey(registerKey))
                    {
                        lastError = "Set registration key failed with error:" + GetLastErrorMessage();
                        return false;
                    }

                    gchFilter = GCHandle.Alloc(filterCallback);
                    IntPtr filterCallbackPtr = Marshal.GetFunctionPointerForDelegate(filterCallback);

                    gchDisconnect = GCHandle.Alloc(disconnectCallback);
                    IntPtr disconnectCallbackPtr = Marshal.GetFunctionPointerForDelegate(disconnectCallback);

                    isFilterStarted = RegisterMessageCallback(threadCount, filterCallbackPtr, disconnectCallbackPtr);
                    if (!isFilterStarted)
                    {
                        lastError = "RegisterMessageCallback failed with error:" + GetLastErrorMessage();
                        return false;
                    }

                    ret = true;

                }
            }
            catch (Exception ex)
            {
                ret = false;
                lastError = "Start filter failed with error " + ex.Message;
            }
            finally
            {
                if (!ret)
                {
                    lastError = lastError + " Make sure you run this application as administrator.";
                }
            }

            return ret;
        }

        static public void StopFilter()
        {
            if (isFilterStarted)
            {
                Disconnect();
                gchFilter.Free();
                gchDisconnect.Free();
                isFilterStarted = false;
            }

            return;
        }

        static public bool IsFilterStarted
        {
            get { return isFilterStarted; }
        }

    }
}
