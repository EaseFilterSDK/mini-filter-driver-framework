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
using System.Text;

namespace EaseFilter.CommonObjects
{
    static public class WinData
    {
        //for Disposition,ShareAccess,DesiredAccess,CreateOptions Please reference Winddows API CreateFile
        //http://msdn.microsoft.com/en-us/library/aa363858%28v=vs.85%29.aspx

        public enum Disposition : uint
        {
            FILE_SUPERSEDE = 0,
            FILE_OPEN,
            FILE_CREATE,
            FILE_OPEN_IF,
            FILE_OVERWRITE,
            FILE_OVERWRITE_IF,
        }

        public enum ShareAccess : uint
        {
            SHARE_READ = 1,
            SHARE_WRITE = 2,
            SHARE_READ_WRITE = 3,
            SHARE_DELETE = 4,
            SHARE_READ_DELETE = 5,
            SHARE_WRITE_DELETE = 6,
            SHARE_READ_WRITE_DELETE = 7,
        }

        public enum DisiredAccess : uint
        {
            DELETE = 0x00010000,
            READ_CONTROL = 0x00020000,
            WRITE_DAC = 0x00040000,
            WRITE_OWNER = 0x00080000,
            SYNCHRONIZE = 0x00100000,
            STANDARD_RIGHTS_REQUIRED = 0x000F0000,
            STANDARD_RIGHTS_ALL = 0x001F0000,
            SPECIFIC_RIGHTS_ALL = 0x0000FFFF,
            ACCESS_SYSTEM_SECURITY = 0x01000000,
            MAXIMUM_ALLOWED = 0x02000000,
            GENERIC_READ = 0x80000000,
            GENERIC_WRITE = 0x40000000,
            GENERIC_EXECUTE = 0x20000000,
            GENERIC_ALL = 0x10000000,
            FILE_READ_DATA = 1,
            FILE_WRITE_DATA = 2,
            FILE_APPEND_DATA = 0x0004,    // file
            FILE_READ_EA = 0x0008,    // file & directory
            FILE_WRITE_EA = 0x0010,    // file & directory
            FILE_EXECUTE = 0x0020,    // file
            FILE_DELETE_CHILD = 0x0040,    // directory
            FILE_READ_ATTRIBUTES = 0x0080,    // all
            FILE_WRITE_ATTRIBUTES = 0x0100,    // all

        }

        public enum CreateOptions : uint
        {
            FILE_DIRECTORY_FILE = 0x00000001,
            FILE_WRITE_THROUGH = 0x00000002,
            FILE_SEQUENTIAL_ONLY = 0x00000004,
            FILE_NO_INTERMEDIATE_BUFFERING = 0x00000008,
            FILE_SYNCHRONOUS_IO_ALERT = 0x00000010,
            FILE_SYNCHRONOUS_IO_NONALERT = 0x00000020,
            FILE_NON_DIRECTORY_FILE = 0x00000040,
            FILE_CREATE_TREE_CONNECTION = 0x00000080,
            FILE_COMPLETE_IF_OPLOCKED = 0x00000100,
            FILE_NO_EA_KNOWLEDGE = 0x00000200,
            FILE_OPEN_REMOTE_INSTANCE = 0x00000400,
            FILE_RANDOM_ACCESS = 0x00000800,
            FILE_DELETE_ON_CLOSE = 0x00001000,
            FILE_OPEN_BY_FILE_ID = 0x00002000,
            FILE_OPEN_FOR_BACKUP_INTENT = 0x00004000,
            FILE_NO_COMPRESSION = 0x00008000,
            FILE_OPEN_REQUIRING_OPLOCK = 0x00010000,
            FILE_DISALLOW_EXCLUSIVE = 0x00020000,
            FILE_RESERVE_OPFILTER = 0x00100000,
            FILE_OPEN_REPARSE_POINT = 0x00200000,
            FILE_OPEN_NO_RECALL = 0x00400000,
            FILE_OPEN_FOR_FREE_SPACE_QUERY = 0x00800000,
            FO_REMOTE_ORIGIN = 0x01000000,
        }

        //this is the status for post create request.
        public enum CreateStatus : uint
        {
            FILE_SUPERSEDED = 0x00000000,
            FILE_OPENED = 0x00000001,
            FILE_CREATED = 0x00000002,
            FILE_OVERWRITTEN = 0x00000003,
            FILE_EXISTS = 0x00000004,
            FILE_DOES_NOT_EXIST = 0x00000005,
        }

        ////end of create structure-------------------------------------------------------------------------------------------------

        //
        // Define the file information class values,for more information please reference
        // http://msdn.microsoft.com/en-us/library/ff543439%28v=vs.85%29.aspx
        //
        public enum FileInfomationClass : uint
        {
            FileDirectoryInformation = 1,
            FileFullDirectoryInformation,   // 2
            FileBothDirectoryInformation,   // 3
            FileBasicInformation,           // 4
            FileStandardInformation,        // 5
            FileInternalInformation,        // 6
            FileEaInformation,              // 7
            FileAccessInformation,          // 8
            FileNameInformation,            // 9
            FileRenameInformation,          // 10
            FileLinkInformation,            // 11
            FileNamesInformation,           // 12
            FileDispositionInformation,     // 13
            FilePositionInformation,        // 14
            FileFullEaInformation,          // 15
            FileModeInformation,            // 16
            FileAlignmentInformation,       // 17
            FileAllInformation,             // 18
            FileAllocationInformation,      // 19
            FileEndOfFileInformation,       // 20
            FileAlternateNameInformation,   // 21
            FileStreamInformation,          // 22
            FilePipeInformation,            // 23
            FilePipeLocalInformation,       // 24
            FilePipeRemoteInformation,      // 25
            FileMailslotQueryInformation,   // 26
            FileMailslotSetInformation,     // 27
            FileCompressionInformation,     // 28
            FileObjectIdInformation,        // 29
            FileCompletionInformation,      // 30
            FileMoveClusterInformation,     // 31
            FileQuotaInformation,           // 32
            FileReparsePointInformation,    // 33
            FileNetworkOpenInformation,     // 34
            FileAttributeTagInformation,    // 35
            FileTrackingInformation,        // 36
            FileIdBothDirectoryInformation, // 37
            FileIdFullDirectoryInformation, // 38
            FileValidDataLengthInformation, // 39
            FileShortNameInformation,       // 40
            FileIoCompletionNotificationInformation, // 41
            FileIoStatusBlockRangeInformation,       // 42
            FileIoPriorityHintInformation,           // 43
            FileSfioReserveInformation,              // 44
            FileSfioVolumeInformation,               // 45
            FileHardLinkInformation,                 // 46
            FileProcessIdsUsingFileInformation,      // 47
            FileNormalizedNameInformation,           // 48
            FileNetworkPhysicalNameInformation,      // 49
            FileIdGlobalTxDirectoryInformation,      // 50
            FileIsRemoteDeviceInformation,           // 51
            FileAttributeCacheInformation,           // 52
            FileNumaNodeInformation,                 // 53
            FileStandardLinkInformation,             // 54
            FileRemoteProtocolInformation,           // 55
            FileMaximumInformation
        }

        public enum SecurityInformation : uint
        {
            OWNER_SECURITY_INFORMATION = 0x00000001,
            GROUP_SECURITY_INFORMATION = 0x00000002,
            DACL_SECURITY_INFORMATION = 0x00000004,
            SACL_SECURITY_INFORMATION = 0x00000008,
            LABEL_SECURITY_INFORMATION = 0x00000010,
        }

        public struct FileBasicInformation
        {
            public long CreationTime;
            public long LastAccessTime;
            public long LastWriteTime;
            public long ChangeTime;
            public uint FileAttributes;
        }

        public struct FileStandardInformation
        {
            public long AllocationSize;
            public long EndOfFile;
            public uint NumberOfLinks;
            public bool DeletePending;
            public bool Directory;
        }

        public struct FileNetworkInformation
        {
            public long CreationTime;
            public long LastAccessTime;
            public long LastWriteTime;
            public long ChangeTime;
            public long AllocationSize;
            public long FileSize;
            public uint FileAttributes;
        }

        //end of information structure-----------------------------------------------------------------------------------

        public enum FLT_FILESYSTEM_TYPE : uint
        {

            FLT_FSTYPE_UNKNOWN,         //an UNKNOWN file system type
            FLT_FSTYPE_RAW,             //Microsoft's RAW file system       (\FileSystem\RAW)
            FLT_FSTYPE_NTFS,            //Microsoft's NTFS file system      (\FileSystem\Ntfs)
            FLT_FSTYPE_FAT,             //Microsoft's FAT file system       (\FileSystem\Fastfat)
            FLT_FSTYPE_CDFS,            //Microsoft's CDFS file system      (\FileSystem\Cdfs)
            FLT_FSTYPE_UDFS,            //Microsoft's UDFS file system      (\FileSystem\Udfs)
            FLT_FSTYPE_LANMAN,          //Microsoft's LanMan Redirector     (\FileSystem\MRxSmb)
            FLT_FSTYPE_WEBDAV,          //Microsoft's WebDav redirector     (\FileSystem\MRxDav)
            FLT_FSTYPE_RDPDR,           //Microsoft's Terminal Server redirector    (\Driver\rdpdr)
            FLT_FSTYPE_NFS,             //Microsoft's NFS file system       (\FileSystem\NfsRdr)
            FLT_FSTYPE_MS_NETWARE,      //Microsoft's NetWare redirector    (\FileSystem\nwrdr)
            FLT_FSTYPE_NETWARE,         //Novell's NetWare redirector
            FLT_FSTYPE_BSUDF,           //The BsUDF CD-ROM driver           (\FileSystem\BsUDF)
            FLT_FSTYPE_MUP,             //Microsoft's Mup redirector        (\FileSystem\Mup)
            FLT_FSTYPE_RSFX,            //Microsoft's WinFS redirector      (\FileSystem\RsFxDrv)
            FLT_FSTYPE_ROXIO_UDF1,      //Roxio's UDF writeable file system (\FileSystem\cdudf_xp)
            FLT_FSTYPE_ROXIO_UDF2,      //Roxio's UDF readable file system  (\FileSystem\UdfReadr_xp)
            FLT_FSTYPE_ROXIO_UDF3,      //Roxio's DVD file system           (\FileSystem\DVDVRRdr_xp)
            FLT_FSTYPE_TACIT,           //Tacit FileSystem                  (\Device\TCFSPSE)
            FLT_FSTYPE_FS_REC,          //Microsoft's File system recognizer (\FileSystem\Fs_rec)
            FLT_FSTYPE_INCD,            //Nero's InCD file system           (\FileSystem\InCDfs)
            FLT_FSTYPE_INCD_FAT,        //Nero's InCD FAT file system       (\FileSystem\InCDFat)
            FLT_FSTYPE_EXFAT,           //Microsoft's EXFat FILE SYSTEM     (\FileSystem\exfat)
            FLT_FSTYPE_PSFS,            //PolyServ's file system            (\FileSystem\psfs)
            FLT_FSTYPE_GPFS             //IBM General Parallel File System  (\FileSystem\gpfs)

        }

        public enum  DeviceObject_Characteristics:uint
        {
        	FILE_REMOVABLE_MEDIA						=	0x00000001,
        	FILE_READ_ONLY_DEVICE                       =	0x00000002,
        	FILE_FLOPPY_DISKETTE                        =	0x00000004,
        	FILE_WRITE_ONCE_MEDIA                       =	0x00000008,
        	FILE_REMOTE_DEVICE                          =	0x00000010,
        	FILE_DEVICE_IS_MOUNTED                      =	0x00000020,
        	FILE_VIRTUAL_VOLUME                         =	0x00000040,
        	FILE_AUTOGENERATED_DEVICE_NAME              =	0x00000080,
        	FILE_DEVICE_SECURE_OPEN                     =	0x00000100,
        	FILE_CHARACTERISTIC_PNP_DEVICE              =	0x00000800,
        	FILE_CHARACTERISTIC_TS_DEVICE               =	0x00001000,
        	FILE_CHARACTERISTIC_WEBDAV_DEVICE           =	0x00002000,
        	FILE_CHARACTERISTIC_CSV                     =	0x00010000,
        	FILE_DEVICE_ALLOW_APPCONTAINER_TRAVERSAL    =	0x00020000,
        	FILE_PORTABLE_DEVICE                        =	0x00040000,
        
        }
    }
}
