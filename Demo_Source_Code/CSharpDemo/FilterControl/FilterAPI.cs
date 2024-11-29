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


namespace EaseFilter.FilterControl
{

    static public class FilterAPI
    {
        public const int MAX_FILE_NAME_LENGTH = 1024;
        public const int MAX_SID_LENGTH = 256;
        public const int INET_ADDR_STR_LEN = 22;
        public const int MAX_MESSAGE_LENGTH = 65536;
        public const int MAX_PATH = 260;
        public const int MAX_ERROR_MESSAGE_SIZE = 1024;
        public const uint MESSAGE_SEND_VERIFICATION_NUMBER = 0xFF000001;        
        public const uint GENERIC_WRITE = 0x40000000;
        public const uint AES_TAG_KEY = 0xccb76e80;

        //default AES encrypted file header size
        public const uint MAX_AES_HEADER_SIZE = 1024;
        public const uint MAX_AES_TAG_SIZE = 910;

        public const uint FILE_FLAG_OPEN_REPARSE_POINT =  0x00200000;
        public const uint FILE_FLAG_OPEN_NO_RECALL =  0x00100000;
        public const uint FILE_FLAG_NO_BUFFERING =  0x20000000;
        public const uint FILE_ATTRIBUTE_REPARSE_POINT =  (uint)FileAttributes.ReparsePoint;

        //for encryption default IV key
        public static byte[] DEFAULT_IV_TAG = { 0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

        public enum FilterType : byte
        {     
            /// <summary>
            /// File system control filter driver
            /// </summary>
            CONTROL_FILTER = 0x01,
            /// <summary>
            /// File system encryption filter driver
            /// </summary>
            ENCRYPTION_FILTER = 0x02,
            /// <summary>
            /// File system monitor filter driver
            /// </summary>
            MONITOR_FILTER = 0x04,
            /// <summary>
            /// File system registry filter driver
            /// </summary>
            REGISTRY_FILTER = 0x08,
            /// <summary>
            /// File system process filter driver
            /// </summary>
            PROCESS_FILTER = 0x10,
            /// <summary>
            /// File system hierarchical storage management filter driver
            /// </summary>
            HSM_FILTER = 0x40,
            /// <summary>
            /// File system cloud storage filter driver
            /// </summary>
            CLOUD_FILTER = 0x80,
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
            ///Use the encryption key and iv from the user mode service,you can use the custom encryption key,iv and taga data per file.
            /// </summary>
            ENCRYPT_FILE_WITH_KEY_IV_AND_TAGDATA_FROM_SERVICE,
        }

        //This is the enumeration of the file copy flags.
        public enum FILE_COPY_FLAG:uint
        {
            //this is the source file for copy in the open.
            CREATE_FLAG_FILE_SOURCE_OPEN_FOR_COPY = 0x00000001,
            //this is the dest file for copy in the open.
            CREATE_FLAG_FILE_DEST_OPEN_FOR_COPY = 0x00000002,
            //this is the source file read for file copy.
            READ_FLAG_FILE_SOURCE_FOR_COPY = 0x00000004,
            //this is the destination file write for file copy.
            WRITE_FLAG_FILE_DEST_FOR_COPY = 0x00000008,

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
            /// the encrypted file's meta data was embedded in the reparse point tag.
            /// </summary>
            ENCRYPT_FILE_WITH_REPARSE_POINT_TAG = 0x00000020,
            /// <summary>
            /// for encryption rule, get the encryption key and IV from user mode for the encrypted files.
            /// </summary>
            REQUEST_ENCRYPT_KEY_AND_IV_FROM_SERVICE = 0x00000040,
            /// <summary>
            /// for control filter, if it is enabled, the control filter rule will be applied in boot time.
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
            /// if it is enabled, it will reopen the file during rehydration of the stub file.
            /// </summary>
            ENABLE_REOPEN_FILE_ON_REHYDRATION = 0x00000400,
            /// <summary>
            /// if it is true, it will enable the buffer for monitor events and send asynchronously, 
            /// or the monitor event will wait till the service picks up the events.
            /// </summary>
            ENABLE_MONITOR_EVENT_BUFFER = 0x00000800,
            /// <summary>
            /// if it is true, it will send the event when it was blocked by the config setting.
            /// </summary>
            ENABLE_SEND_DENIED_EVENT = 0x00001000,
            /// <summary>
            /// if it is true, and the write access is false,the write will return success 
            /// with zero data being written to the file, and send the write data to the user mode.
            /// this flag is reserved for custom feature.
            /// </summary>
            ENABLE_WRITE_WITH_ZERO_DATA_AND_SEND_DATA = 0x00002000,
            /// <summary>
            /// if it is true, the portable massive storage won't be treated as USB.
            //	this is for the volume control flag for BLOCK_USB_READ,BLOCK_USB_WRITE
            /// </summary>
            DISABLE_REMOVABLE_MEDIA_AS_USB = 0x00004000,
            /// <summary>
            /// if it is true, it will block the encrypted file to be renamed to different folder.
            /// </summary>
            DISABLE_RENAME_ENCRYPTED_FILE = 0x00008000,
            /// <summary>
            /// if it is true, the data protection will continue even the service process is stopped.
            /// </summary>
            ENABLE_PROTECTION_IF_SERVICE_STOPPED = 0x00020000,
            /// <summary>
            /// if it is true and write encrypt info to cache is enabled, it will signal the system thread to write cache data to disk right away.
            /// </summary>
            ENABLE_SIGNAL_WRITE_ENCRYPT_INFO_EVENT = 0x00020000,
            /// <summary>
            ///enable this feature when accessFlag "ALLOW_SAVE_AS" or "ALLOW_COPY_PROTECTED_FILES_OUT" was disabled.
            ///by default we don't enable this feature, because of the drawback of these two flags were disabled 
            ///which will block all new file creation of the process which was read the protected files.
            /// </summary>
            ENABLE_BLOCK_SAVE_AS_FLAG = 0x00040000,

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
            /// send the process termination information
            /// </summary>
            FILTER_SEND_PROCESS_TERMINATION_INFO = 0x00010009,
            /// <summary>
            /// send the new thread creation information
            /// </summary>
            FILTER_SEND_THREAD_CREATION_INFO = 0x0001000a,
            /// <summary>
            /// send the thread termination information
            /// </summary>
            FILTER_SEND_THREAD_TERMINATION_INFO = 0x0001000b,
            /// <summary>
            /// send the process handle operations information
            /// </summary>
            FILTER_SEND_PROCESS_HANDLE_INFO = 0x0001000c,
            /// <summary>
            /// send the thread handle operations information
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
            /// <summary>
            /// send the event when the file IO was blocked by the config setting.
            /// </summary>
            FILTER_SEND_DENIED_FILE_IO_EVENT = 0x00010010,
            /// <summary>
            /// send the event when the volume dismount was blocked by the config setting.
            /// </summary>
            FILTER_SEND_DENIED_VOLUME_DISMOUNT_EVENT = 0x00010011,
            /// <summary>
            /// send the event when the process creation was blocked by the config setting.
            /// </summary>
            FILTER_SEND_DENIED_PROCESS_CREATION_EVENT = 0x00010012,
            /// <summary>
            /// send the event when the registry access was blocked by the config setting.
            /// </summary>
            FILTER_SEND_DENIED_REGISTRY_ACCESS_EVENT = 0x00010013,
            /// <summary>
            /// send the event when the protected process was terminiated ungratefully.
            /// </summary>
            FILTER_SEND_DENIED_PROCESS_TERMINATED_EVENT = 0x00010014,
            /// <summary>
            /// send the event when the read from USB was blocked.
            /// </summary>
            FILTER_SEND_DENIED_USB_READ_EVENT = 0x00010015,
            /// <summary>
            /// send the event when the write to USB was blocked.
            /// </summary>
            FILTER_SEND_DENIED_USB_WRITE_EVENT = 0x00010016,
            /// <summary>
            /// send process information before it was terminiated.
            /// </summary>
            FILTER_SEND_PRE_TERMINATE_PROCESS_INFO = 0x00010017,

        }

        public enum IOEventName
        {
            /// <summary>
            /// Fires this event before the file create IO was going down to the file system.
            /// </summary>
            IOPreFileCreate = 0x00020001,
            /// <summary>
            /// Fires this event after the file create IO was returned from the file system.
            /// </summary>
            IOPostFileCreate,
            /// <summary>
            /// Fires this event before the file read IO was going down to the file system.
            /// </summary>
            IOPreFileRead,
            /// <summary>
            /// Fires this event after the file read IO was returned from the file system.
            /// </summary>
            IOPostFileRead,
            /// <summary>
            /// Fires this event before the file write IO was going down to the file system.
            /// </summary>
            IOPreFileWrite,
            /// <summary>
            /// Fires this event after the file write IO was returned from the file system.
            /// </summary>
            IOPostFileWrite,
            /// <summary>
            /// Fires this event before the query file size IO was going down to the file system.
            /// </summary>
            IOPreQueryFileSize,
            /// <summary>
            /// Fires this event after the query file size IO was returned from the file system.
            /// </summary>
            IOPostQueryFileSize,
            /// <summary>
            /// Fires this event before the query file basic info IO was going down to the file system.
            /// </summary>
            IOPreQueryFileBasicInfo,
            /// <summary>
            /// Fires this event after the query file basic info IO was returned from the file system.
            /// </summary>
            IOPostQueryFileBasicInfo,
            /// <summary>
            /// Fires this event before the query file standard info IO was going to the file system.
            /// </summary>
            IOPreQueryFileStandardInfo,
            /// <summary>
            /// Fires this event after the query file standard info IO was returned from the file system.
            /// </summary>
            IOPostQueryFileStandardInfo,
            /// <summary>
            /// Fires this event before the query file network info IO was going down to the file system.
            /// </summary>
            IOPreQueryFileNetworkInfo,
            /// <summary>
            /// Fires this event after the query file network info IO was returned from the file system.
            /// </summary>
            IOPostQueryFileNetworkInfo,
            /// <summary>
            /// Fires this event before the query file Id IO was going down to the file system.
            /// </summary>
            IOPreQueryFileId,
            /// <summary>
            /// Fires this event after the query file Id IO was returned from the file system.
            /// </summary>
            IOPostQueryFileId,
            /// <summary>
            /// Fires this event before the query file info IO was going down to the file system
            /// if the query file information class is not 'QueryFileSize','QueryFileBasicInfo'
            /// ,'QueryFileStandardInfo','QueryFileNetworkInfo' or 'QueryFileId'.
            /// </summary>
            IOPreQueryFileInfo,
            /// <summary>
            /// Fires this event after the query file info IO was returned from the file system.
            /// </summary>
            IOPostQueryFileInfo,
            /// <summary>
            /// Fires this event before the set file size IO was going down to the file system.
            /// </summary>         
            IOPreSetFileSize,
            /// <summary>
            /// Fires this event after the set file size IO was returned from the file system.
            /// </summary>
            IOPostSetFileSize,
            /// <summary>
            /// Fires this event before the set file basic info IO was going down to the file system.
            /// </summary>
            IOPreSetFileBasicInfo,
            /// <summary>
            /// Fires this event after the set file basic info IO was returned from the file system.
            /// </summary>
            IOPostSetFileBasicInfo,
            /// <summary>
            /// Fires this event before the set file standard info IO was going down to the file system.
            /// </summary>
            IOPreSetFileStandardInfo,
            /// <summary>
            /// Fires this event after the set file standard info IO was returned from the file system.
            /// </summary>
            IOPostSetFileStandardInfo,
            /// <summary>
            /// Fires this event before the set file network info was going down to the file system.
            /// </summary>
            IOPreSetFileNetworkInfo,
            /// <summary>
            /// Fires this event after the set file network info was returned from the file system.
            /// </summary>
            IOPostSetFileNetworkInfo,
            /// <summary>
            /// Fires this event before the file move or rename IO was going down to the file system.
            /// </summary>
            IOPreMoveOrRenameFile,
            /// <summary>
            /// Fires this event after the file move or rename IO was returned from the file system.
            /// </summary>
            IOPostMoveOrRenameFile,
            /// <summary>
            /// Fires this event before the file delete IO was going down to the file system.
            /// </summary>
            IOPreDeleteFile,
            /// <summary>
            /// Fires this event after the file delete IO was returned from the file system.
            /// </summary>
            IOPostDeleteFile,
            /// <summary>
            /// Fires this event before the set file info IO was going down to the file system
            /// if the information class is not 'SetFileSize','SetFileBasicInfo'
            /// ,'SetFileStandardInfo','SetFileNetworkInfo'.
            /// </summary>
            IOPreSetFileInfo,
            /// <summary>
            /// Fires this event after the set file info IO was returned from the file system.
            /// </summary>
            IOPostSetFileInfo,
            /// <summary>
            /// Fires this event before the query directory file info was going down to the file system.
            /// </summary>
            IOPreQueryDirectoryFile,
            /// <summary>
            /// Fires this event after the query directory file info was returned from the file system.
            /// </summary>
            IOPostQueryDirectoryFile,
            /// <summary>
            /// Fires this event before the query file security IO was going down to the file system.
            /// </summary>
            IOPreQueryFileSecurity,
            /// <summary>
            /// Fires this event after the query file security IO was returned from the file system.
            /// </summary>
            IOPostQueryFileSecurity,
            /// <summary>
            /// Fires this event before the set file security IO was going down to the file system.
            /// </summary>
            IOPreSetFileSecurity,
            /// <summary>
            /// Fires thiis event after the set file security IO was returned from the file system.
            /// </summary>
            IOPostSetFileSecurity,
            /// <summary>
            /// Fire this event before the file handle close IO was going down to the file system.
            /// </summary>
            IOPreFileHandleClose,
            /// <summary>
            /// Fires this event after the file handle close IO was returned from the file system.
            /// </summary>
            IOPostFileHandleClose,
            /// <summary>
            /// Fires this event before the file close IO was going down to the file system.
            /// </summary>
            IOPreFileClose,
            /// <summary>
            /// Fires this event after the file close IO was returned from the file system.
            /// </summary>
            IOPostFileClose,               
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
        /// The file changed events for monitor filter, it will be fired after the file handle was closed.
        /// </summary>
        public enum FileChangedEvents:uint
        {
            /// <summary>
            /// Fires this event when the new file was created after the file handle closed
            /// </summary>
            NotifyFileWasCreated = 0x00000020,
            /// <summary>
            /// Fires this event when the file was written after the file handle closed
            /// </summary>
            NotifyFileWasWritten = 0x00000040,
            /// <summary>
            /// Fires this event when the file was moved or renamed after the file handle closed
            /// </summary>
            NotifyFileWasRenamed = 0x00000080,
            /// <summary>
            /// Fires this event when the file was deleted after the file handle closed
            /// </summary>
            NotifyFileWasDeleted = 0x00000100,
            /// <summary>
            /// Fires this event when the file's security was changed after the file handle closed
            /// </summary>
            NotifyFileSecurityWasChanged = 0x00000200,
            /// <summary>
            /// Fires this event when the file's information was changed after the file handle closed
            /// </summary>
            NotifyFileInfoWasChanged = 0x00000400,
            /// <summary>
            /// Fires this event when the file's data was read after the file handle closed
            /// </summary>
            NotifyFileWasRead = 0x00000800,
            /// <summary>
            /// This is only for Windows 11, version 22H2 or later OS.
            /// Fires this event when the file was copied after the file handle closed
            /// </summary>
            NotifyFileWasCopied = 0x00001000,
        }

        /// <summary>
        /// Fires all file events it this value was set.
        /// </summary>
        public const uint NotifyAllFileEvents = 0xfffffff0;

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
            ALLOW_OPEN_WITH_ACCESS_SYSTEM_SECURITY = 0x00000010,
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
            /// prevent the protected files from being copying out to the USB when the flag value is 0.
            /// </summary>
            ALLOW_COPY_PROTECTED_FILES_TO_USB = 0x04000000, 
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
     
        /// <summary>
        /// This is the data structure the filter driver send request to the user mode service.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct MessageSendData
        {
            /// <summary>
            ///the verification number which verifies the data structure integerity.
            /// </summary>
            public uint VerificationNumber;
            /// <summary>
            /// This is the command Id which was sent by filter driver.
            /// </summary>
            public uint FilterCommand;     
            /// <summary>
            /// This is the  sequential message Id, just for reference.
            /// </summary>
            public uint MessageId;          
            /// <summary>
            /// This the filter rule Id associated to the filter rule.
            /// </summary>
            public uint FilterRuleId;       
            /// <summary>
            /// This is the ip address of the remote computer when it accesses the file via SMB.
            /// it is only effected for win 7 or later OS.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = INET_ADDR_STR_LEN*2)]
            public byte[] RemoteIP;    
            /// <summary>
            ///the address of FileObject,it is equivalent to file handle,it is unique per file stream open.
            /// </summary>
            public IntPtr FileObject;       
            /// <summary>
            ///the address of FsContext,it is unique per file.
            /// </summary>
            public IntPtr FsContext;        
            /// <summary>
            ///the message type of the file I/O, registry class.
            /// </summary>
            public ulong MessageType;        
            /// <summary>
            ///the process ID for the process associated with the thread that originally requested the I/O operation.
            /// </summary>
            public uint ProcessId;          
            /// <summary>
            ///the thread ID which requested the I/O operation.
            /// </summary>
            public uint ThreadId;           
            /// <summary>
            ///the read/write offset.
            /// </summary>
            public long Offset;             
            /// <summary>
            ///the read/write length.
            /// </summary>
            public uint Length;             
            /// <summary>
            ///the size of the file for the I/O operation.
            /// </summary>
            public long FileSize;           
            /// <summary>
            ///the transaction time in UTC of this request.
            /// </summary>
            public long TransactionTime;    
            /// <summary>
            ///the creation time in UTC of the file.
            /// </summary>
            public long CreationTime;       
            /// <summary>
            ///the last access time in UTC of the file.
            /// </summary>
            public long LastAccessTime;     
            /// <summary>
            ///the last write time in UTC of the file.
            /// </summary>
            public long LastWriteTime;      
            /// <summary>
            ///the file attributes.
            /// </summary>
            public uint FileAttributes;     
            /// <summary>
            ///the DesiredAccess for file open, please reference CreateFile windows API.
            /// </summary>
            public uint DesiredAccess;      
            /// <summary>
            ///the Disposition for file open, please reference CreateFile windows API.
            /// </summary>
            public uint Disposition;        
            /// <summary>
            ///the SharedAccess for file open, please reference CreateFile windows API.
            /// </summary>
            public uint SharedAccess;       
            /// <summary>
            ///the CreateOptions for file open, please reference CreateFile windows API.
            /// </summary>
            public uint CreateOptions;      
            /// <summary>
            ///the CreateStatus after file was openned, please reference CreateFile windows API.
            /// </summary>
            public uint CreateStatus;       
            /// <summary>
            ///this is the information class for security/directory/information IO 
            /// </summary>
            public uint InfoClass;          
            /// <summary>
            ///the I/O status which returned from file system.
            /// </summary>
            public uint Status;
            /// <summary>
            /// the return I/O (read/write) length 
            /// </summary>
            public uint ReturnLength;
            /// <summary>
            ///the file name length in byte.
            /// </summary>
            public uint FileNameLength;     
            /// <summary>
            ///the file name of the I/O operation.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_FILE_NAME_LENGTH)]
            public string FileName;         
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
            ///the data buffer length.
            /// </summary>
            public uint DataBufferLength;   
            /// <summary>
            ///the data buffer which contains read/write/query information/set information data.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_MESSAGE_LENGTH)]
            public byte[] DataBuffer;       
            
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
            /// <summary>
            ///Block the read from the USB disk.
            /// </summary>
            BLOCK_USB_READ = 0x00000010,
            /// <summary>
            ///Block the write to the USB disk
            /// </summary>
            BLOCK_USB_WRITE = 0x00000020,

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
            /// The length of the command line.
            /// </summary>
            public uint CommandLineLength;
            /// <summary>
            /// The command that is used to execute the process.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_FILE_NAME_LENGTH)]
            public string CommandLine;
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
        /// Create the filter driver connection with callback settings
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
        /// set the maximum monitor event buffer size if monitorBuffer was enabled.
        /// </summary>
        /// <param name="maxMonitorEventBufferSize"></param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool SetMaxMonitorEventBuffersize(uint maxMonitorEventBufferSize);

      /// <summary>
        /// If the encrypt write buffer size is greater than 0, then the small buffer encryption write will be combined together to a bigger buffer,
        /// and write it to the disk.
      /// </summary>
      /// <param name="encryptWriteBufferSize"></param>
      /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool SetEncryptWriteBufferSize(uint encryptWriteBufferSize);

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
        ///the encrypted information will be appended to the file as an header, 
        ///the filter driver will know if the file was encrypted by checking the header.
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
        ///Set an encryption folder, all encrypted files use the same encryption key and IV key. 
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
        ///only manage the file IO for the processes in the included process list
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
        /// Register the file changed events for the filter rule, get the notification when the I/O was triggered
        /// after the file handle was closed.
        /// </summary>
        /// <param name="filterMask">the file filter mask of the filter rule</param>
        /// <param name="eventType">the I/O event types,reference the FileEventType enumeration.</param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool RegisterFileChangedEventsToFilterRule(
        [MarshalAs(UnmanagedType.LPWStr)]string filterMask,
        uint eventType);

        /// <summary>
        /// Register the callback I/O for monitor filter driver to the filter rule.
        /// </summary>
        /// <param name="filterMask">the file filter mask of the filter rule</param>
        /// <param name="registerIO">the specific I/Os you want to monitor</param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool RegisterMonitorIOToFilterRule(
        [MarshalAs(UnmanagedType.LPWStr)]string filterMask,
        ulong registerIO);

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
        ulong registerIO);

        /// <summary>
        /// Filter the I/O based on the file create options,the IO will be skipped if the filter option is not 0
        /// and the file create options don't match the filter.
        /// </summary>
        /// <param name="filterMask">the file filter mask of the filter rule</param>
        /// <param name="filterByDesiredAccess">if it is not 0, the filter driver will check if the file creation option "DesiredAccess" matches the filter</param>
        /// <param name="filterByDisposition">if it is not 0, the filter driver will check if the file creation option "Disposition" matches the filter</param>
        /// <param name="filterByCreateOptions">if it is not 0, the filter driver will check if the file creation option "CreateOptions" matches the filter</param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AddCreateFilterToFilterRule(
        [MarshalAs(UnmanagedType.LPWStr)]string filterMask,
        uint filterByDesiredAccess,
        uint filterByDisposition,
        uint filterByCreateOptions);

        /// <summary>
        /// Set the access rights to the specific process
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
        /// Get sha256 hash of the file, you need to allocate the 32 bytes array to get the sha256 hash.
        /// hashBytesLength is the input byte array length, and the outpout length of the hash.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="hashBytes"></param>
        /// <param name="hashBytesLength"></param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool Sha256HashFile(
            [MarshalAs(UnmanagedType.LPWStr)]string fileName,
            byte[] hashBytes,
            ref uint hashBytesLength);

        /// <summary>
        /// Add the access rights of the process with the sha256 hash to the filter rule.
        /// allows you to set the access rights to your trusted process.
        /// </summary>
        /// <param name="filterMask">The filter rule file filter mask.</param>
        /// <param name="imageSha256">the sha256 hash of the executable binary file.</param>
        /// <param name="hashLength">the length of the sha256 hash, by default is 32.</param>
        /// <param name="accessFlags">the access flags for the setting process.</param>
        /// <returns>return true if it is succeeded.</returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AddSha256ProcessAccessRightsToFilterRule(
        [MarshalAs(UnmanagedType.LPWStr)]string filterMask,
        byte[] imageSha256,
        uint hashLength,
        uint accessFlags);

        /// <summary>
        /// Add the access rights of the process which was signed with the certificate to the filter rule.
        /// allows you to set the access rights to your trusted process.
        /// </summary>
        /// <param name="filterMask">The filter rule file filter mask.</param>
        /// <param name="certificateName">the subject name of the code certificate to sign the process.</param>
        /// <param name="lengthOfCertificate">the length of the certificate name</param>
        /// <param name="accessFlags">the access flags for the setting process.</param>
        /// <returns>return true if it is succeeded.</returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AddSignedProcessAccessRightsToFilterRule(
        [MarshalAs(UnmanagedType.LPWStr)]string filterMask,
        [MarshalAs(UnmanagedType.LPWStr)]string certificateName,
        uint lengthOfCertificate,
        uint accessFlags);

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
        /// Remove the access control rights to specific users
        /// </summary>
        /// <param name="filterMask">the filter rule file filter mask</param>
        /// <param name="userName">the user name you want to set the access right</param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool RemoveUserRightsFromFilterRule(
        [MarshalAs(UnmanagedType.LPWStr)]string filterMask,
        [MarshalAs(UnmanagedType.LPWStr)]string userName);

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
            /// <summary>
            /// send the registry event when registry access was blocked by the above setting,
            /// when it was set to true.
            /// </summary>
            ENABLE_FILTER_SEND_DENIED_REG_EVENT = 0x80000000,
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
        /// <param name="processId">set the processId if you want to filter by id instead of the process name</param>
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
            bool isExcludeFilter,
            uint filterRuleId);

        /// <summary>
        /// Add exclude process name to the registry filter rule.
        /// </summary>
        /// <param name="processNameFilterMask">the registry filter rule process name filter mask</param>
        /// <param name="registryKeyNameFilterMask">the registry filter rule registry key filter mask</param>
        /// <param name="excludeProcessNameMask">the exclude process name mask</param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AddExcludeProcessNameToRegistryFilterRule(
        [MarshalAs(UnmanagedType.LPWStr)] string processNameFilterMask,
        [MarshalAs(UnmanagedType.LPWStr)] string registryKeyNameFilterMask,
        [MarshalAs(UnmanagedType.LPWStr)] string excludeProcessNameMask);

        /// <summary>
        /// Add exclude user name to the registry filter rule.
        /// </summary>
        /// <param name="processNameFilterMask">the registry filter rule process name filter mask</param>
        /// <param name="registryKeyNameFilterMask">the registry filter rule registry key name filter mask</param>
        /// <param name="excludeUserNameMask">the exclude user name mask</param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AddExcludeUserNameToRegistryFilterRule(
        [MarshalAs(UnmanagedType.LPWStr)] string processNameFilterMask,
        [MarshalAs(UnmanagedType.LPWStr)] string registryKeyNameFilterMask,
        [MarshalAs(UnmanagedType.LPWStr)] string excludeUserNameMask);

        /// <summary>
        /// Add exclude key name to the registry filter rule.
        /// </summary>
        /// <param name="processNameFilterMask">the registry filter rule process name filter mask</param>
        /// <param name="registryKeyNameFilterMask">the registry filter rule registry key name filter mask</param>
        /// <param name="excludeKeyNameMask">the key user name mask</param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AddExcludeKeyNameToRegistryFilterRule(
        [MarshalAs(UnmanagedType.LPWStr)] string processNameFilterMask,
        [MarshalAs(UnmanagedType.LPWStr)] string registryKeyNameFilterMask,
        [MarshalAs(UnmanagedType.LPWStr)] string excludeKeyNameMask);

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

        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool RemoveRegistryFilterRuleByRegKeyName(
            uint registryKeyNameLength,
            [MarshalAs(UnmanagedType.LPWStr)]string registryKeyName);

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
            /// if this flag is enabled, it will send the notification when the new process creation was blocked.	
            /// </summary>
            ENABLE_SEND_PROCESS_DENIED_EVENT = 0x00000002,
            /// <summary>
            /// send the callback request before the process is going to be terminated.
            /// you can block the process termination in the callback function.
            /// </summary>
            PROCESS_PRE_TERMINATION_REQUEST = 0x00000004,
            /// <summary>
            /// Get a notification when a new process is being created.
            /// </summary>
            PROCESS_CREATION_NOTIFICATION = 0x00000100,
            /// <summary>
            ///get a notification when a process was terminated 
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
            /// get a notification when a thread was terminated 
            /// </summary>
            THREAD_TERMINATION_NOTIFICATION = 0x00001000,
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

        /// <summary>
        /// Add exclude process name to the process filter rule.
        /// </summary>
        /// <param name="processNameMask">the process filter rule process name filter mask</param>
        /// <param name="excludeProcessNameMask">the exclude process name mask</param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AddExcludeProcessNameToProcessFilterRule(
        [MarshalAs(UnmanagedType.LPWStr)] string processNameMask,
        [MarshalAs(UnmanagedType.LPWStr)] string excludeProcessNameMask);

        /// <summary>
        /// Add exclude user name to the process filter rule.
        /// </summary>
        /// <param name="processNameMask">the process filter rule process name filter mask</param>
        /// <param name="excludeUserNameMask">the exclude user name mask</param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AddExcludeUserNameToProcessFilterRule(
        [MarshalAs(UnmanagedType.LPWStr)] string processNameMask,
        [MarshalAs(UnmanagedType.LPWStr)] string excludeUserNameMask);

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
        /// <param name="filterRuleId">the filterRuleId associated to the callback event</param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AddFileCallbackIOToProcessByName(
        uint processNameMaskLength,
        [MarshalAs(UnmanagedType.LPWStr)]string processNameMask,
        uint fileNameMaskLength,
        [MarshalAs(UnmanagedType.LPWStr)]string fileNameMask,
        ulong monitorIO,
        ulong controlIO,
        uint filterRuleId );

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

        /// <summary>
        /// Get the subject name of the certificate which the process was signed 
        /// </summary>
        /// <param name="processName">the signed process name</param>
        /// <param name="certificateSubjectName">the subject name of the certificate</param>
        /// <param name="sizeOfCertificateSubjectName">the size of the subject name</param>
        /// <param name="signedTime">the signed time</param>
        /// <returns>return true if the process was signed correctly, or return false.</returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool GetSignerInfo(
        [MarshalAs(UnmanagedType.LPWStr)]string processName,
        [MarshalAs(UnmanagedType.LPWStr)]string certificateSubjectName,
        ref uint sizeOfCertificateSubjectName,
        ref long signedTime);

        //---------------Process filter APIs   END-----------------------------------------------

        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool RegisterIoRequest(uint requestRegistration);

        /// <summary>
        /// Open file bypass the filter driver and security check here.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="dwDesiredAccess"></param>
        /// <param name="fileHandle"></param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool GetFileHandleInFilter(
             [MarshalAs(UnmanagedType.LPWStr)]string fileName,
             uint dwDesiredAccess,
             ref IntPtr fileHandle);

        /// <summary>
        /// Close the file handle which was opened by GetFileHandleInFilter.
        /// </summary>
        /// <param name="fileHandle"></param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool CloseFileHandleInFilter(
             IntPtr fileHandle);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool ConvertSidToStringSid(
            [In] IntPtr sid,
            [Out] out IntPtr sidString);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LocalFree(IntPtr hMem);

        [DllImport("kernel32", SetLastError = true)]
        public static extern uint GetCurrentProcessId();

        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool CreateFileAPI(
             [MarshalAs(UnmanagedType.LPWStr)]string fileName,
              uint dwDesiredAccess,
              uint dwShareMode,
              uint dwCreationDisposition,
              uint dwFlagsAndAttributes,
              ref IntPtr fileHandle);    
   

        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AESEncryptDecryptBuffer(
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
        public static extern bool AESDecryptFile(
             [MarshalAs(UnmanagedType.LPWStr)]string fileName,
             uint keyLength,
             byte[] encryptionKey,
             uint ivLength,
             byte[] iv);
         

        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AESDecryptFileToFile(
             [MarshalAs(UnmanagedType.LPWStr)]string sourceFileName,
             [MarshalAs(UnmanagedType.LPWStr)]string destFileName,
             uint keyLength,
             byte[] encryptionKey,
             uint ivLength,
             byte[] iv);

        /// <summary>
        /// Decrypt the encrypted file at offset and length to a buffer array.
        /// </summary>
        /// <param name="encryptedFileName">The encrypted file name</param>
        /// <param name="keyLength">the number of the bytes of the encryption key</param>
        /// <param name="encryptionKey">the encryption key byte array</param>
        /// <param name="ivLength">the lenght of the iv key, set it to 0 if the AES header was embedded.</param>
        /// <param name="iv">the iv key, set it to null if the AES header was embedded.</param>
        /// <param name="offset">the offset which the decryption will start</param>
        /// <param name="bytesToDecrypt">the number of bytes to decrypt</param>
        /// <param name="decryptedBuffer">the decrypted buffer array to receive the decrypted data, 
        /// the buffer size must be greater or equal than the bytesToDecrypt</param>
        /// <param name="bytesDecrypted">the length of the return decrytped buffer</param>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AESDecryptBytes(
             [MarshalAs(UnmanagedType.LPWStr)]string encryptedFileName,
             uint keyLength,
             byte[] encryptionKey,
             uint ivLength,
             byte[] iv,
             long offset,
             int bytesToDecrypt,
             byte[] decryptedBuffer,
             ref int bytesDecrypted);

  
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

        /// <summary>
        /// Get the computerId 
        /// </summary>
        /// <returns></returns>
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern uint GetComputerId();
   

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

    }
}
