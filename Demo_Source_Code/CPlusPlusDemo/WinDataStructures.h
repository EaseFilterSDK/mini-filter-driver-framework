///////////////////////////////////////////////////////////////////////////////
//
//   This is the Windows data structure.
//
///////////////////////////////////////////////////////////////////////////////

#ifndef __WinDataStructures_H__
#define __WinDataStructures_H__

#define	STATUS_SUCCESS						0
#define STATUS_ACCESS_DENIED				0xC0000022L
#define STATUS_REPARSE						0x00000104L
#define STATUS_NO_MORE_FILES				0x80000006L
#define STATUS_WARNING						(ULONG)0x80000000
#define STATUS_ERROR						(ULONG)0xc0000000

//for Disposition,ShareAccess,DesiredAccess,CreateOptions Please reference Winddows API CreateFile
//http://msdn.microsoft.com/en-us/library/aa363858%28v=vs.85%29.aspx

typedef enum Disposition 
{
    FILE_SUPERSEDE = 0,
    FILE_OPEN,
    FILE_CREATE,
    FILE_OPEN_IF,
    FILE_OVERWRITE,
    FILE_OVERWRITE_IF,
};

typedef enum ShareAccess 
{
    SHARE_READ = 1,
    SHARE_WRITE = 2,
    SHARE_READ_WRITE = 3,
    SHARE_DELETE = 4,
    SHARE_READ_DELETE = 5,
    SHARE_WRITE_DELETE = 6,
    SHARE_READ_WRITE_DELETE = 7,
};

typedef enum CreateOptions 
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
};

//this is the status for post create request.
typedef enum CreateStatus
{
    FILE_SUPERSEDED = 0x00000000,
    FILE_OPENED = 0x00000001,
    FILE_CREATED = 0x00000002,
    FILE_OVERWRITTEN = 0x00000003,
    FILE_EXISTS = 0x00000004,
    FILE_DOES_NOT_EXIST = 0x00000005,
};

//
// Define the file information class values
//
// WARNING:  The order of the following values are assumed by the I/O system.
//           Any changes made here should be reflected there as well.
//

typedef enum _FILE_INFORMATION_CLASS {
    FileDirectoryInformation                         = 1,
    FileFullDirectoryInformation,                   // 2
    FileBothDirectoryInformation,                   // 3
    FileBasicInformation,                           // 4
    FileStandardInformation,                        // 5
    FileInternalInformation,                        // 6
    FileEaInformation,                              // 7
    FileAccessInformation,                          // 8
    FileNameInformation,                            // 9
    FileRenameInformation,                          // 10
    FileLinkInformation,                            // 11
    FileNamesInformation,                           // 12
    FileDispositionInformation,                     // 13
    FilePositionInformation,                        // 14
    FileFullEaInformation,                          // 15
    FileModeInformation,                            // 16
    FileAlignmentInformation,                       // 17
    FileAllInformation,                             // 18
    FileAllocationInformation,                      // 19
    FileEndOfFileInformation,                       // 20
    FileAlternateNameInformation,                   // 21
    FileStreamInformation,                          // 22
    FilePipeInformation,                            // 23
    FilePipeLocalInformation,                       // 24
    FilePipeRemoteInformation,                      // 25
    FileMailslotQueryInformation,                   // 26
    FileMailslotSetInformation,                     // 27
    FileCompressionInformation,                     // 28
    FileObjectIdInformation,                        // 29
    FileCompletionInformation,                      // 30
    FileMoveClusterInformation,                     // 31
    FileQuotaInformation,                           // 32
    FileReparsePointInformation,                    // 33
    FileNetworkOpenInformation,                     // 34
    FileAttributeTagInformation,                    // 35
    FileTrackingInformation,                        // 36
    FileIdBothDirectoryInformation,                 // 37
    FileIdFullDirectoryInformation,                 // 38
    FileValidDataLengthInformation,                 // 39
    FileShortNameInformation,                       // 40
    FileIoCompletionNotificationInformation,        // 41
    FileIoStatusBlockRangeInformation,              // 42
    FileIoPriorityHintInformation,                  // 43
    FileSfioReserveInformation,                     // 44
    FileSfioVolumeInformation,                      // 45
    FileHardLinkInformation,                        // 46
    FileProcessIdsUsingFileInformation,             // 47
    FileNormalizedNameInformation,                  // 48
    FileNetworkPhysicalNameInformation,             // 49
    FileIdGlobalTxDirectoryInformation,             // 50
    FileIsRemoteDeviceInformation,                  // 51
    FileUnusedInformation,                          // 52
    FileNumaNodeInformation,                        // 53
    FileStandardLinkInformation,                    // 54
    FileRemoteProtocolInformation,                  // 55

        //
        //  These are special versions of these operations (defined earlier)
        //  which can be used by kernel mode drivers only to bypass security
        //  access checks for Rename and HardLink operations.  These operations
        //  are only recognized by the IOManager, a file system should never
        //  receive these.
        //

    FileRenameInformationBypassAccessCheck,         // 56
    FileLinkInformationBypassAccessCheck,           // 57

        //
        // End of special information classes reserved for IOManager.
        //

    FileVolumeNameInformation,                      // 58
    FileIdInformation,                              // 59
    FileIdExtdDirectoryInformation,                 // 60
    FileReplaceCompletionInformation,               // 61
    FileHardLinkFullIdInformation,                  // 62
    FileIdExtdBothDirectoryInformation,             // 63
    FileDispositionInformationEx,                   // 64
    FileRenameInformationEx,                        // 65
    FileRenameInformationExBypassAccessCheck,       // 66
    FileDesiredStorageClassInformation,             // 67
    FileStatInformation,                            // 68
    FileMemoryPartitionInformation,                 // 69
    FileStatLxInformation,                          // 70
    FileCaseSensitiveInformation,                   // 71
    FileLinkInformationEx,                          // 72
    FileLinkInformationExBypassAccessCheck,         // 73
    FileStorageReserveIdInformation,                // 74
    FileCaseSensitiveInformationForceAccessCheck,   // 75
    FileKnownFolderInformation,   // 76

    FileMaximumInformation
} FILE_INFORMATION_CLASS, *PFILE_INFORMATION_CLASS;

//
// Define the various structures which are returned on query operations
//

typedef struct _FILE_BASIC_INFORMATION {
    LARGE_INTEGER CreationTime;
    LARGE_INTEGER LastAccessTime;
    LARGE_INTEGER LastWriteTime;
    LARGE_INTEGER ChangeTime;
    ULONG FileAttributes;
} FILE_BASIC_INFORMATION, *PFILE_BASIC_INFORMATION;

typedef struct _FILE_STANDARD_INFORMATION {
    LARGE_INTEGER AllocationSize;
    LARGE_INTEGER EndOfFile;
    ULONG NumberOfLinks;
    BOOLEAN DeletePending;
    BOOLEAN Directory;
} FILE_STANDARD_INFORMATION, *PFILE_STANDARD_INFORMATION;


typedef struct _FILE_POSITION_INFORMATION {
    LARGE_INTEGER CurrentByteOffset;
} FILE_POSITION_INFORMATION, *PFILE_POSITION_INFORMATION;


typedef struct _FILE_NETWORK_OPEN_INFORMATION {
    LARGE_INTEGER CreationTime;
    LARGE_INTEGER LastAccessTime;
    LARGE_INTEGER LastWriteTime;
    LARGE_INTEGER ChangeTime;
    LARGE_INTEGER AllocationSize;
    LARGE_INTEGER EndOfFile;
    ULONG FileAttributes;
} FILE_NETWORK_OPEN_INFORMATION, *PFILE_NETWORK_OPEN_INFORMATION;



//
// NtQueryDirectoryFile return types:
//
//      FILE_DIRECTORY_INFORMATION
//      FILE_FULL_DIR_INFORMATION
//      FILE_ID_FULL_DIR_INFORMATION
//      FILE_BOTH_DIR_INFORMATION
//      FILE_ID_BOTH_DIR_INFORMATION
//      FILE_NAMES_INFORMATION
//      FILE_OBJECTID_INFORMATION
//

typedef struct _FILE_DIRECTORY_INFORMATION {
    ULONG NextEntryOffset;
    ULONG FileIndex;
    LARGE_INTEGER CreationTime;
    LARGE_INTEGER LastAccessTime;
    LARGE_INTEGER LastWriteTime;
    LARGE_INTEGER ChangeTime;
    LARGE_INTEGER EndOfFile;
    LARGE_INTEGER AllocationSize;
    ULONG FileAttributes;
    ULONG FileNameLength;
    WCHAR FileName[1];
} FILE_DIRECTORY_INFORMATION, *PFILE_DIRECTORY_INFORMATION;

typedef struct _FILE_FULL_DIR_INFORMATION {
    ULONG NextEntryOffset;
    ULONG FileIndex;
    LARGE_INTEGER CreationTime;
    LARGE_INTEGER LastAccessTime;
    LARGE_INTEGER LastWriteTime;
    LARGE_INTEGER ChangeTime;
    LARGE_INTEGER EndOfFile;
    LARGE_INTEGER AllocationSize;
    ULONG FileAttributes;
    ULONG FileNameLength;
    ULONG EaSize;
    WCHAR FileName[1];
} FILE_FULL_DIR_INFORMATION, *PFILE_FULL_DIR_INFORMATION;

typedef struct _FILE_ID_FULL_DIR_INFORMATION {
    ULONG NextEntryOffset;
    ULONG FileIndex;
    LARGE_INTEGER CreationTime;
    LARGE_INTEGER LastAccessTime;
    LARGE_INTEGER LastWriteTime;
    LARGE_INTEGER ChangeTime;
    LARGE_INTEGER EndOfFile;
    LARGE_INTEGER AllocationSize;
    ULONG FileAttributes;
    ULONG FileNameLength;
    ULONG EaSize;
    LARGE_INTEGER FileId;
    WCHAR FileName[1];
} FILE_ID_FULL_DIR_INFORMATION, *PFILE_ID_FULL_DIR_INFORMATION;

typedef struct _FILE_BOTH_DIR_INFORMATION {
    ULONG NextEntryOffset;
    ULONG FileIndex;
    LARGE_INTEGER CreationTime;
    LARGE_INTEGER LastAccessTime;
    LARGE_INTEGER LastWriteTime;
    LARGE_INTEGER ChangeTime;
    LARGE_INTEGER EndOfFile;
    LARGE_INTEGER AllocationSize;
    ULONG FileAttributes;
    ULONG FileNameLength;
    ULONG EaSize;
    CCHAR ShortNameLength;
    WCHAR ShortName[12];
    WCHAR FileName[1];
} FILE_BOTH_DIR_INFORMATION, *PFILE_BOTH_DIR_INFORMATION;

typedef struct _FILE_ID_BOTH_DIR_INFORMATION {
    ULONG NextEntryOffset;
    ULONG FileIndex;
    LARGE_INTEGER CreationTime;
    LARGE_INTEGER LastAccessTime;
    LARGE_INTEGER LastWriteTime;
    LARGE_INTEGER ChangeTime;
    LARGE_INTEGER EndOfFile;
    LARGE_INTEGER AllocationSize;
    ULONG FileAttributes;
    ULONG FileNameLength;
    ULONG EaSize;
    CCHAR ShortNameLength;
    WCHAR ShortName[12];
    LARGE_INTEGER FileId;
    WCHAR FileName[1];
} FILE_ID_BOTH_DIR_INFORMATION, *PFILE_ID_BOTH_DIR_INFORMATION;

typedef struct _FILE_NAMES_INFORMATION {
    ULONG NextEntryOffset;
    ULONG FileIndex;
    ULONG FileNameLength;
    WCHAR FileName[1];
} FILE_NAMES_INFORMATION, *PFILE_NAMES_INFORMATION;

typedef struct _FILE_ID_GLOBAL_TX_DIR_INFORMATION {
    ULONG NextEntryOffset;
    ULONG FileIndex;
    LARGE_INTEGER CreationTime;
    LARGE_INTEGER LastAccessTime;
    LARGE_INTEGER LastWriteTime;
    LARGE_INTEGER ChangeTime;
    LARGE_INTEGER EndOfFile;
    LARGE_INTEGER AllocationSize;
    ULONG FileAttributes;
    ULONG FileNameLength;
    LARGE_INTEGER FileId;
    GUID LockingTransactionId;
    ULONG TxInfoFlags;
    WCHAR FileName[1];
} FILE_ID_GLOBAL_TX_DIR_INFORMATION, *PFILE_ID_GLOBAL_TX_DIR_INFORMATION;

typedef enum _FLT_FILESYSTEM_TYPE {

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
    FLT_FSTYPE_GPFS,            //IBM General Parallel File System  (\FileSystem\gpfs)
    FLT_FSTYPE_NPFS,            //Microsoft's Named Pipe file system(\FileSystem\npfs)
    FLT_FSTYPE_MSFS,            //Microsoft's Mailslot file system  (\FileSystem\msfs)
    FLT_FSTYPE_CSVFS,           //Microsoft's Cluster Shared Volume file system  (\FileSystem\csvfs)
    FLT_FSTYPE_REFS,            //Microsoft's ReFS file system      (\FileSystem\Refs or \FileSystem\Refsv1)
    FLT_FSTYPE_OPENAFS,         //OpenAFS file system               (\Device\AFSRedirector)
    FLT_FSTYPE_CIMFS            //Composite Image file system       (\FileSystem\cimfs)

} FLT_FILESYSTEM_TYPE, * PFLT_FILESYSTEM_TYPE;

#endif