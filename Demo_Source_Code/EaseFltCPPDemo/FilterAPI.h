///////////////////////////////////////////////////////////////////////////////
//
//    (C) Copyright 2014 EaseFilter
//    All Rights Reserved
//
//    This software is part of a licensed software product and may
//    only be used or copied in accordance with the terms of that license.
//
///////////////////////////////////////////////////////////////////////////////

#ifndef __SHARE_TYPE_H__
#define __SHARE_TYPE_H__

//Purchase a license key with the link: http://www.EaseFilter.com/Order.htm
//Email us to request a trial key: info@EaseFilter.com //free email is not accepted.
#define	registerKey "**************************************"

#define MESSAGE_SEND_VERIFICATION_NUMBER	0xFF000001
#define	INET_ADDR_STR_LEN					22
#define MAX_FILE_NAME_LENGTH				1024
#define MAX_SID_LENGTH						256
#define MAX_EXCLUDED_PROCESS_ID				200	
#define MAX_INCLUDED_PROCESS_ID				200
#define MAX_PROTECTED_PROCESS_ID			200
#define MAX_BLOCK_SAVEAS_PROCESS_ID			200
#define MAX_PATH							260
#define	MAX_ERROR_MESSAGE_SIZE				1024
#define	MAX_BUFFER_SIZE						16384
#define BLOCK_SIZE							65536

#define AES_VERIFICATION_KEY			0xccb76e80

typedef enum _FilterType 
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

} FilterType;


//the structure of control meta data of the encrypted file.
#pragma pack (push,1)

typedef struct _AES_DATA 
{
	ULONG		VerificationKey;
	ULONG		AESFlags;
	ULONG		Version;
	UCHAR		IV[16];
	ULONG		EncryptionKeyLength;
	UCHAR		EncryptionKey[32];
	LONGLONG	FileSize;
	ULONG		CryptoType;
	ULONG		PaddingSize;
	//the size of this data structure
	ULONG		AESDataSize;
	//the actual physical file size in disk including the padding and the header.
	LONGLONG	ShadowFileSize;
	ULONG		AccessFlags;
	ULONG		Reserve1;
	ULONG		Reserve2;
	ULONG		TagDataLength;
	WCHAR		TagData[1];
	
} AES_DATA, *PAES_DATA;

//the custom data structure for tagData inside of PAES_DATA
typedef struct _AES_TAG_CONTROL_DATA 
{
	ULONG		VerificationKey;
	ULONG		AESFlags;
	LONGLONG    CreationTime;
	LONGLONG    ExpireTime;
	ULONG		AccessFlags;
	ULONG		LengthOfIncludeProcessNames;
	ULONG		OffsetOfIncludeProcessNames;
	ULONG		LengthOfExcludeProcessNames;
	ULONG		OffsetOfExcludeProcessNames;
	ULONG		LengthOfIncludeUserNames;
	ULONG		OffsetOfIncludeUserNames;
	ULONG		LengthOfExcludeUserNames;
	ULONG		OffsetOfExcludeUserNames;
	ULONG		LengthOfAccountName;
	ULONG		OffsetOfAccountName;
	ULONG		LengthOfComputerId;
	ULONG		OffsetOfComputerId;
	ULONG		LengthOfUserPassword;
	ULONG		OffsetOfUserPassword;

	//the data store here.
	//IncludeProcessNames;
	//ExcludeProcessNames;
	//IncludeUserNames;
	//ExcludeUserNames;
	//AccountNames;
	//ComputerId;
	//UserPassword;
	
} AES_TAG_CONTROL_DATA, *PAES_TAG_CONTROL_DATA;
#pragma pack(pop)


#define MAX_REQUEST_TYPE 32

//the commands were sent to the user mode application by filter driver.
typedef enum _FilterCommand
{	
	/// <summary>
    /// request the read data back with block data or whole cache file name.
    /// </summary>
	MESSAGE_TYPE_RESTORE_BLOCK_OR_FILE	=						0x00000001,
	/// <summary>
    /// request to download the data to the original folder.
    /// </summary>
	MESSAGE_TYPE_RESTORE_FILE_TO_ORIGINAL_FOLDER =				0x00000002,
	/// <summary>
    /// request the directory file list.
    /// </summary>
	MESSAGE_TYPE_GET_FILE_LIST =								0x00000004,
	/// <summary>
    /// request to download whole file to the cache folder.
    /// </summary>
	MESSAGE_TYPE_RESTORE_FILE_TO_CACHE =						0x00000008,
	/// <summary>
    /// send the notification event of the file changed.
    /// </summary>
	MESSAGE_TYPE_SEND_EVENT_NOTIFICATION =						0x00000010,
	/// <summary>
    /// send the notification event of the file was deleted.
    /// </summary>
	MESSAGE_TYPE_DELETE_FILE =									0x00000020,
	/// <summary>
    /// send the notification event of the file was renamed.
    /// </summary>
	MESSAGE_TYPE_RENAME_FILE =									0x00000040,
	/// <summary>
    /// send the file name of the message was stored.
    /// </summary>
	MESSAGE_TYPE_SEND_MESSAGE_FILENAME =						0x00000080,
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
    /// <summary>
    /// send the event when the file IO was blocked by the config setting.
	// if the flag ENABLE_SEND_DENIED_EVENT was enabled in global boolean setting.
    /// </summary>
    FILTER_SEND_DENIED_FILE_IO_EVENT = 0x00010010,
    /// <summary>
    /// send the event when the volume dismount was blocked by the config setting.
	// if the flag ENABLE_SEND_DENIED_EVENT was enabled in global boolean setting.
    /// </summary>
    FILTER_SEND_DENIED_VOLUME_DISMOUNT_EVENT = 0x00010011,
    /// <summary>
    /// send the event when the process creation was blocked by the config setting.
	// if the flag ENABLE_SEND_DENIED_EVENT was enabled in global boolean setting.
    /// </summary>
    FILTER_SEND_DENIED_PROCESS_EVENT = 0x00010012,
    /// <summary>
    /// send the event when the registry access was blocked by the config setting.
	// if the flag ENABLE_SEND_DENIED_EVENT was enabled in global boolean setting.
    /// </summary>
    FILTER_SEND_DENIED_REGISTRY_ACCESS_EVENT = 0x00010013,
	/// <summary>
    /// send the event when the protected process was terminiated ungratefully.
	// if the flag ENABLE_SEND_DENIED_EVENT was enabled in global boolean setting.
    /// </summary>
    FILTER_SEND_DENIED_PROCESS_TERMINATED_EVENT = 0x00010014,
	/// <summary>
    /// send the event when the read from USB was blocked if the flag BLOCK_USB_READ was enabled in volume control flag.
	// if the flag ENABLE_SEND_DENIED_EVENT was enabled in global boolean setting.
    /// </summary>
    FILTER_SEND_DENIED_USB_READ_EVENT = 0x00010015,
	/// <summary>
    /// send the event when the write to USB was blocked if the flag BLOCK_USB_WRITE was enabled in volume control flag.
	// if the flag ENABLE_SEND_DENIED_EVENT was enabled in global boolean setting.
    /// </summary>
    FILTER_SEND_DENIED_USB_WRITE_EVENT = 0x00010016,
	/// <summary>
    /// send process information before the process is going to be terminated.
    /// </summary>
    FILTER_SEND_PRE_TERMINATE_PROCESS_INFO = 0x00010017,
	
}FilterCommand;

//the IO name of the IO operation.
typedef enum _IOName
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

}IOName;

 /// <summary>
/// The volume control flag.
/// </summary>
typedef enum  _VolumeControlFlag 
{
    /// <summary>
    /// Get all the attached volumes' information.
    /// </summary>
    GET_ATTACHED_VOLUME_INFO    = 0x00000001,
	/// <summary>
    /// Get a notification when the filter driver attached to a volume.
    /// </summary>
    VOLUME_ATTACHED_NOTIFICATION    = 0x00000002,
    /// <summary>
    /// Get a notification when the filter driver detached from a volume.
    /// </summary>
    VOLUME_DETACHED_NOTIFICATION    = 0x00000004,
	/// <summary>
    ///Prevent the attched volumes from being formatted or dismounted.
    /// </summary>
    BLOCK_VOLUME_DISMOUNT			= 0x00000008,
	/// <summary>
    ///Block the read from the USB disk.
    /// </summary>
    BLOCK_USB_READ = 0x00000010,
    /// <summary>
    ///Block the write to the USB disk
    /// </summary>
    BLOCK_USB_WRITE = 0x00000020,

                                  
}VolumeControlFlag;


//
//the structure of the attached volume information
typedef struct _VOLUME_INFO
{	
    /// <summary>
    /// The length of the volume name.
    /// </summary>
    ULONG VolumeNameLength;
    /// <summary>
    /// The volume name buffer.
    /// </summary>
    WCHAR VolumeName[MAX_FILE_NAME_LENGTH];
    /// <summary>
    /// The length of the volume dos file name.
    /// </summary>
    ULONG VolumeDosNameLength;
    /// <summary>
    /// The volume dos file name buffer.
    /// </summary>
    WCHAR VolumeDosName[MAX_FILE_NAME_LENGTH];
	/// <summary>
	///the volume file system type.
	/// </summary>
	ULONG VolumeFilesystemType;    
    /// <summary>
    ///the Characteristics of the attached device object if existed. 
    /// </summary>
    ULONG DeviceCharacteristics;


} VOLUME_INFO, *PVOLUME_INFO;

 /// <summary>
/// process filter driver control flag.
/// </summary>
typedef enum  _ProcessControlFlag 
{
    /// <summary>
    /// deny the new process creation if the flag is on
    /// </summary>
    DENY_NEW_PROCESS_CREATION = 0x00000001,
	/// <summary>
    /// send the callback reqeust before the process is going to be terminated.
	/// you can block the process termination in the callback function.
    /// </summary>
    PROCESS_PRE_TERMINATION_REQUEST = 0x00000002,
    /// <summary>
    /// Get a notification when a new process is being created.
    /// </summary>
    PROCESS_CREATION_NOTIFICATION      = 0x00000100,
    /// <summary>
    ///get a notification when a process was termiated 
    /// </summary>
    PROCESS_TERMINATION_NOTIFICATION   = 0x00000200,
    /// <summary>
    /// get a notification for process handle operations, when a handle for a process
    /// is being created or duplicated.
    /// </summary>
    PROCESS_HANDLE_OP_NOTIFICATION     = 0x00000400,
    /// <summary>
    /// get a notifcation when a new thread is being created.
    /// </summary>
    THREAD_CREATION_NOTIFICATION       = 0x00000800,
    /// <summary>
    /// get a notification when a thread was termiated 
    /// </summary>
    THREAD_TERMINIATION_NOTIFICATION   = 0x00001000,
    /// <summary>
    /// get a notification for thread handle operations, when a handle for a process
    /// is being created or duplicated.
    /// </summary>
    THREAD_HANDLE_OP_NOTIFICATION      = 0x00002000,
                                  
}ProcessControlFlag;

/// <summary>
/// This is for registry filter driver only, the registry access control flag
/// </summary>
typedef enum  _RegControlFlag
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
    REG_MAX_ACCESS_FLAG = 0xFFFFFFFF,

}RegControlFlag;

/// <summary>
/// This is for registry filter driver only, the registry callback class, you can block or modify the registry access in pre-callback
/// or monitor the registry access in post-callback
/// </summary>
typedef enum  _RegCallbackClass
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

	//high field for reg callback class
	#define    Reg_Pre_Load_Key  (ULONGLONG)0x100000000
	#define    Reg_Post_Load_Key  (ULONGLONG)0x200000000
	#define    Reg_Pre_UnLoad_Key  (ULONGLONG)0x400000000
	#define    Reg_Post_UnLoad_Key  (ULONGLONG)0x800000000
	#define    Reg_Pre_Query_Key_Security  (ULONGLONG)0x1000000000
	#define	   Reg_Post_Query_Key_Security  (ULONGLONG)0x2000000000
	#define    Reg_Pre_Set_Key_Security  (ULONGLONG)0x4000000000
	#define    Reg_Post_Set_Key_Security  (ULONGLONG)0x8000000000
		//
		// per-object context cleanup
		//
	#define    Reg_Callback_Object_Context_Cleanup  (ULONGLONG)0x10000000000
		//
		// new in Vista SP2 
		//
	#define    Reg_Pre_Restore_Key  (ULONGLONG)0x20000000000
	#define    Reg_Post_Restore_Key  (ULONGLONG)0x40000000000
	#define    Reg_Pre_Save_Key  (ULONGLONG)0x80000000000
	#define    Reg_Post_Save_Key  (ULONGLONG)0x100000000000
	#define    Reg_Pre_Replace_Key  (ULONGLONG)0x200000000000
	#define    Reg_Post_Replace_Key  (ULONGLONG)0x400000000000

	//
	//new in Windows 10
	//
	#define    Reg_Pre_Query_KeyName  (ULONGLONG)0x800000000000
	#define    Reg_Post_Query_KeyName  (ULONGLONG)0x1000000000000

	#define    Max_Reg_Callback_Class  (ULONGLONG)0xFFFFFFFFFFFFFFFF


}RegCallbackClass;

//the I/O types of the monitor or control filter can intercept if you register the class.
//the monitor IO only can register the post IO, it meant you can get the notification after the IO was completed.
//the control IO can register both pre IO before the IO was processed by the file system and post IO after the IO 
//was processed by the file system, the driver will wait for the response of the callback function.
typedef  enum _IOCallbackClass
{
	//this is the IRP_MJ_CREATE which requests to open a handle 
	PRE_CREATE							= 0x00000001,
	POST_CREATE							= 0x00000002,

	//this is the filter only intercept the IO when the file was created.
	#define PRE_NEW_FILE_CREATED  (ULONGLONG)0x0000000100000000
	#define POST_NEW_FILE_CREATED  (ULONGLONG)0x0000000200000000

	//this is the fast I/O read,return true when the data is in cache,
	//if the data is not in cache, a new IRP cache read request will be generated.
	PRE_FASTIO_READ						= 0x00000004,
	POST_FASTIO_READ					= 0x00000008,

	//this is the IRP_MJ_READ cache read, the data will be read from the cache, 
	//if the data is not in cache, a paging read request will be generated.
	PRE_CACHE_READ						= 0x00000010,
	POST_CACHE_READ						= 0x00000020,

	//this is the IRP_MJ_READ no cache read, the data read will be bypassed the cache manager.
	PRE_NOCACHE_READ					= 0x00000040,
	POST_NOCACHE_READ					= 0x00000080,

	//this is the IRP_MJ_READ paging read, the data will be read from the disk to the cache.
	PRE_PAGING_IO_READ					= 0x00000100,
	POST_PAGING_IO_READ					= 0x00000200,

	//this is the fast I/O write,the data was written to the cache if the request is satisfied immediately,
	//or a IRP cache write will be generated.
	PRE_FASTIO_WRITE					= 0x00000400,
	POST_FASTIO_WRITE					= 0x00000800,

	//this is the IRP_MJ_WRITE I/O cache write,the data was written to the cache, the IRP paging write will be gernerated after the cache write.
	PRE_CACHE_WRITE						= 0x00001000,
	POST_CACHE_WRITE					= 0x00002000,

	//this is the IRP_MJ_WRITE no cache write, the data read will be written to the disk directly and bypass the cache manager.
	PRE_NOCACHE_WRITE					= 0x00004000,
	POST_NOCACHE_WRITE					= 0x00008000,

	//this is the IRP_MJ_WRITE paging write, the data will be written from the cache to the disk.
	PRE_PAGING_IO_WRITE					= 0x00010000,
	POST_PAGING_IO_WRITE				= 0x00020000,

	//this the IRP_QUERY_INFORMATION to query the file information. 
	//if you register this class, the filter driver will intercept all requests to query information.
	//if you want to only filter speecific query,i.e. only file size, you can register the below specific class.
	PRE_QUERY_INFORMATION				= 0x00040000,
	POST_QUERY_INFORMATION				= 0x00080000,

	//this the IRP_QUERY_INFORMATION  with specific info class to query file size
	#define PRE_QUERY_FILE_SIZE  (ULONGLONG)0x0000000400000000
	#define POST_QUERY_FILE_SIZE  (ULONGLONG)0x0000000800000000

	//this the IRP_QUERY_INFORMATION  with specific info class to query file basic information
	#define PRE_QUERY_FILE_BASIC_INFO  (ULONGLONG)0x0000001000000000
	#define POST_QUERY_FILE_BASIC_INFO  (ULONGLONG)0x0000002000000000

	//this the IRP_QUERY_INFORMATION  with specific info class to query file standard information
	#define PRE_QUERY_FILE_STANDARD_INFO  (ULONGLONG)0x0000004000000000
	#define POST_QUERY_FILE_STANDARD_INFO  (ULONGLONG)0x0000008000000000

	//this the IRP_QUERY_INFORMATION  with specific info class to query file network information
	#define PRE_QUERY_FILE_NETWORK_INFO  (ULONGLONG)0x0000010000000000
	#define POST_QUERY_FILE_NETWORK_INFO  (ULONGLONG)0x0000020000000000

	//this the IRP_QUERY_INFORMATION  with specific info class to query file Id
	#define PRE_QUERY_FILE_ID  (ULONGLONG)0x0000040000000000
	#define POST_QUERY_FILE_ID  (ULONGLONG)0x0000080000000000

	//this the IRP_SET_INFORMATION to set the file information.
	//if you register this class,the filter driver will intercept all requests to set the file information.
	//if you want to only intercept specific set information, you can register below specific class.
	PRE_SET_INFORMATION					= 0x00100000,
	POST_SET_INFORMATION				= 0x00200000,

	//this the IRP_SET_INFORMATION with specific info class to set file size
	#define PRE_SET_FILE_SIZE  (ULONGLONG)0x0000400000000000
	#define POST_SET_FILE_SIZE  (ULONGLONG)0x0000800000000000

	//this the IRP_SET_INFORMATION with specific info class to set file basic information
	#define PRE_SET_FILE_BASIC_INFO  (ULONGLONG)0x0001000000000000
	#define POST_SET_FILE_BASIC_INFO  (ULONGLONG)0x0002000000000000

	//this the IRP_SET_INFORMATION with specific info class to set file standard information
	#define PRE_SET_FILE_STANDARD_INFO  (ULONGLONG)0x0004000000000000
	#define POST_SET_FILE_STANDARD_INFO  (ULONGLONG)0x0008000000000000

	//this the IRP_SET_INFORMATION with specific info class to set file network information
	#define PRE_SET_FILE_NETWORK_INFO  (ULONGLONG)0x0010000000000000
	#define POST_SET_FILE_NETWORK_INFO  (ULONGLONG)0x0020000000000000

	//this the IRP_SET_INFORMATION with specific info class to rename the file
	#define PRE_RENAME_FILE   (ULONGLONG)0x0040000000000000
	#define POST_RENAME_FILE  (ULONGLONG)0x0080000000000000

	//this the IRP_SET_INFORMATION with specific info class to delete the file
	#define PRE_DELETE_FILE  (ULONGLONG)0x0100000000000000
	#define POST_DELETE_FILE  (ULONGLONG)0x0200000000000000

	//this the IRP_MJ_DIRECTORY_CONTROL to query the file directory information.
	PRE_DIRECTORY						= 0x00400000,
	POST_DIRECTORY						= 0x00800000,

	//this the IRP_MJ_QUERY_SECURITY to query the file security information.
	PRE_QUERY_SECURITY					= 0x01000000,	
	POST_QUERY_SECURITY					= 0x02000000,

	//this the IRP_MJ_SET_SECURITY to set the file security information.
	PRE_SET_SECURITY					= 0x04000000,
	POST_SET_SECURITY					= 0x08000000,

	//this the IRP_MJ_CLEANUP to close the file handle.
	PRE_CLEANUP							= 0x10000000,
	POST_CLEANUP						= 0x20000000,

	//this the IRP_MJ_CLOSE to close the file I/O.
	PRE_CLOSE							= 0x40000000,
	POST_CLOSE							= 0x80000000UL, 

}IOCallbackClass;

//the flags of the access control to the file.
typedef enum _AccessFlag
{
    /// <summary>
    /// Filter driver will skip all the IO if the file name match the include file mask.
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
    /// Allow the file to be mapped.
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
    /// for filter driver with least access right to the file.
    /// </summary>
    LEAST_ACCESS_FLAG = 0xf0000000,
    /// <summary>
    /// Allow the maximum right access.
    /// </summary>
	ALLOW_MAX_RIGHT_ACCESS	= 0xfffffff0,
	
}AccessFlag;



//this is the boolean data of the user mode app sending to the filter.
//this is the boolean configuration of the filter driver.
typedef enum _BooleanConfig 
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
    /// the encrypted file's meta data was embeded in the reparse point tag, it is for the previous version 5.0.
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
	//  this flag is for custom usage.
	/// </summary>
	ENABLE_WRITE_WITH_ZERO_DATA_AND_SEND_DATA = 0x00002000,
	/// <summary>
	/// if it is true, the portable massive storage will be treated as USB.
	//	this is for the volume control flag for BLOCK_USB_READ,BLOCK_USB_WRITE
	/// </summary>
	DISABLE_REMOVABLE_MEDIA_AS_USB = 0x00004000,
	/// <summary>
	/// if it is true, it will block the encrypted file to be renamed to different folder.
	/// </summary>
	DISABLE_RENAME_ENCRYPTED_FILE = 0x00008000,
    /// <summary>
    /// if it is true, it will disable the file synchronization for file reading for CloudTier.
    /// </summary>
    DISABLE_FILE_SYNCHRONIZATION = 0x00010000,
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

} BooleanConfig;


//this is the data structure of the filter driver sending data to the user mode app.
typedef struct _MESSAGE_SEND_DATA 
{
	 /// <summary>
    ///the verification number which verifiys the data structure integerity.
    /// </summary>
    ULONG VerificationNumber;
    /// <summary>
    /// This is the command Id which was sent by filter driver.
    /// </summary>
    ULONG FilterCommand;     
    /// <summary>
    /// This is the  sequential message Id, just for reference.
    /// </summary>
    ULONG MessageId;          
    /// <summary>
    /// This the filter rule Id associated to the filter rule.
    /// </summary>
    ULONG FilterRuleId;       
    /// <summary>
    /// This is the ip address of the remote computer when it accesses the file via SMB.
    /// it is only effected for win 7 or later OS.
    /// </summary>
    WCHAR RemoteIP[INET_ADDR_STR_LEN];	
    /// <summary>
    ///the address of FileObject,it is equivalent to file handle,it is unique per file stream open.
    /// </summary>
    PVOID FileObject;       
    /// <summary>
    ///the address of FsContext,it is unique per file.
    /// </summary>
    PVOID FsContext;        
    /// <summary>
    ///the message type of the file I/O, registry class.
    /// </summary>
    LONGLONG MessageType;        
    /// <summary>
    ///the process ID for the process associated with the thread that originally requested the I/O operation.
    /// </summary>
    ULONG ProcessId;          
    /// <summary>
    ///the thread ID which requested the I/O operation.
    /// </summary>
    ULONG ThreadId;           
    /// <summary>
    ///the read/write offset.
    /// </summary>
    LONGLONG Offset;             
    /// <summary>
    ///the read/write length.
    /// </summary>
    ULONG Length;             
    /// <summary>
    ///the size of the file for the I/O operation.
    /// </summary>
    LONGLONG FileSize;           
    /// <summary>
    ///the transaction time in UTC of this request.
    /// </summary>
    LONGLONG TransactionTime;    
    /// <summary>
    ///the creation time in UTC of the file.
    /// </summary>
    LONGLONG CreationTime;       
    /// <summary>
    ///the last access time in UTC of the file.
    /// </summary>
    LONGLONG LastAccessTime;     
    /// <summary>
    ///the last write time in UTC of the file.
    /// </summary>
    LONGLONG LastWriteTime;      
    /// <summary>
    ///the file attributes.
    /// </summary>
    ULONG FileAttributes;     
    /// <summary>
    ///the DesiredAccess for file open, please reference CreateFile windows API.
    /// </summary>
    ULONG DesiredAccess;      
    /// <summary>
    ///the Disposition for file open, please reference CreateFile windows API.
    /// </summary>
    ULONG Disposition;        
    /// <summary>
    ///the SharedAccess for file open, please reference CreateFile windows API.
    /// </summary>
    ULONG ShareAccess;       
    /// <summary>
    ///the CreateOptions for file open, please reference CreateFile windows API.
    /// </summary>
    ULONG CreateOptions;      
    /// <summary>
    ///the CreateStatus after file was openned, please reference CreateFile windows API.
    /// </summary>
    ULONG CreateStatus;       
    /// <summary>
    ///this is the information class for security/directory/information IO 
    /// </summary>
    ULONG InfoClass;          
    /// <summary>
    ///the I/O status which returned from file system.
    /// </summary>
    ULONG Status;
    /// <summary>
    /// the return I/O (read/write) length 
    /// </summary>
    ULONG ReturnLength;
    /// <summary>
    ///the file name length in byte.
    /// </summary>
    ULONG FileNameLength;     
    /// <summary>
    ///the file name of the I/O operation.
    /// </summary>
   	WCHAR FileName[MAX_FILE_NAME_LENGTH];
    /// <summary>
    ///the length of the security identifier.
    /// </summary>
    ULONG SidLength;          
    /// <summary>
    ///the security identifier data.
    /// </summary>
    UCHAR Sid[MAX_SID_LENGTH];               
    /// <summary>
    ///the data buffer length.
    /// </summary>
    ULONG DataBufferLength;   
    /// <summary>
    ///the data buffer which contains read/write/query information/set information data.
    /// </summary>
    UCHAR DataBuffer[MAX_BUFFER_SIZE];	  

} MESSAGE_SEND_DATA, *PMESSAGE_SEND_DATA;


//
//the structure of the new creating process	information
typedef struct _PROCESS_INFO
{
	
    /// <summary>
    ///The process ID of the parent process for the new process. Note that the parent process is not necessarily the same process as the process that created the new process.  
    /// </summary>
    ULONG ParentProcessId;
    /// <summary>
    ///  The process ID of the process that created the new process.
    /// </summary>
    ULONG CreatingProcessId;
    /// <summary>
    /// The thread ID of the thread that created the new process.
    /// </summary>
    ULONG CreatingThreadId;
    /// <summary>
    ///An ACCESS_MASK value that specifies the access rights to grant for the handle for OB_PRE_CREATE_HANDLE_INFORMATION.
    /// </summary>
    ULONG DesiredAccess;
    /// <summary>
    ///The type of handle operation. This member might be one of the following values:OB_OPERATION_HANDLE_CREATE,OB_OPERATION_HANDLE_DUPLICATE
    /// </summary>
    ULONG Operation;
    /// <summary>
    /// A Boolean value that specifies whether the ImageFileName member contains the exact file name that is used to open the process executable file.
    /// </summary>
    ULONG FileOpenNameAvailable;
    /// <summary>
    /// The length of the command line.
    /// </summary>
    ULONG CommandLineLength;
    /// <summary>
    /// The command that is used to execute the process.
    /// </summary>
    WCHAR	CommandLine[MAX_FILE_NAME_LENGTH];	

} PROCESS_INFO, *PPROCESS_INFO;

//The status return to filter,instruct filter driver what action needs to be done.
typedef enum _FilterStatus 
{
	FILTER_MESSAGE_IS_DIRTY			= 0x00000001, //Set this flag if the reply message need to be processed.
	FILTER_COMPLETE_PRE_OPERATION	= 0x00000002, //Set this flag if complete the pre operation. 
	FILTER_DATA_BUFFER_IS_UPDATED	= 0x00000004, //Set this flag if the databuffer was updated.
	FILTER_BLOCK_DATA_WAS_RETURNED	= 0x00000008, //Set this flag if return read block databuffer to filter.
	FILTER_CACHE_FILE_WAS_RETURNED	= 0x00000010, //Set this flag if the whole cache file was downloaded.
	FILTER_REHYDRATE_FILE_VIA_CACHE_FILE	= 0x00000020, //Set this flag if the whole cache file was downloaded and stub file needs to be rehydrated.

} FilterStatus, *PFilterStatus;

//this is the enumeration of the file I/O events.
typedef enum _FileEventType
{
	FILE_WAS_CREATED				= 0x00000020,
	FILE_WAS_WRITTEN				= 0x00000040,
	FILE_WAS_RENAMED				= 0x00000080,
	FILE_WAS_DELETED				= 0x00000100,
	FILE_SECURITY_CHANGED			= 0x00000200,
	FILE_INFO_CHANGED				= 0x00000400,
	FILE_WAS_READ					= 0x00000800,
    FILE_WAS_COPIED                 = 0x00001000,

} FileEventType, *PFileEventType;

//This is the enumeration of the file copy flags.
typedef enum _FILE_COPY_FLAG
{
    //this is the source file for copy in the open.
    CREATE_FLAG_FILE_SOURCE_OPEN_FOR_COPY = 0x00000001,
    //this is the dest file for copy in the open.
    CREATE_FLAG_FILE_DEST_OPEN_FOR_COPY = 0x00000002,
    //this is the source file read for file copy.
    READ_FLAG_FILE_SOURCE_FOR_COPY = 0x00000004,
    //this is the destination file write for file copy.
    WRITE_FLAG_FILE_DEST_FOR_COPY = 0x00000008,

} FILE_COPY_FLAG;

//
// Define the various device characteristics flags
//
//typedef enum _File_Characteristics
//{
//	FILE_REMOVABLE_MEDIA						=	0x00000001,
//	FILE_READ_ONLY_DEVICE                       =	0x00000002,
//	FILE_FLOPPY_DISKETTE                        =	0x00000004,
//	FILE_WRITE_ONCE_MEDIA                       =	0x00000008,
//	FILE_REMOTE_DEVICE                          =	0x00000010,
//	FILE_DEVICE_IS_MOUNTED                      =	0x00000020,
//	FILE_VIRTUAL_VOLUME                         =	0x00000040,
//	FILE_AUTOGENERATED_DEVICE_NAME              =	0x00000080,
//	FILE_DEVICE_SECURE_OPEN                     =	0x00000100,
//	FILE_CHARACTERISTIC_PNP_DEVICE              =	0x00000800,
//	FILE_CHARACTERISTIC_TS_DEVICE               =	0x00001000,
//	FILE_CHARACTERISTIC_WEBDAV_DEVICE           =	0x00002000,
//	FILE_CHARACTERISTIC_CSV                     =	0x00010000,
//	FILE_DEVICE_ALLOW_APPCONTAINER_TRAVERSAL    =	0x00020000,
//	FILE_PORTABLE_DEVICE                        =	0x00040000,
//
//}File_Characteristics,*PFile_Characteristics;



//This is the return data structure from user mode to the filter driver.
typedef struct _MESSAGE_REPLY_DATA 
{
	ULONG		MessageId;
	ULONG		MessageType;	
	ULONG		ReturnStatus;
	ULONG		FilterStatus;
	union {
		struct {
				ULONG		DataBufferLength;
				UCHAR		DataBuffer[BLOCK_SIZE];		
		} Data;
		struct {
				ULONG		SizeOfData;
				struct{
							ULONG		AccessFlag;
							ULONG		IVLength;
							UCHAR		IV[16];
							ULONG		EncryptionKeyLength;
							UCHAR		EncryptionKey[32];	
							ULONG		TagDataLength;
							UCHAR		TagData[1];		
					 }Data;
		} AESData;
		struct {
				ULONG		UserNameLength;
				WCHAR		UserName[1];				
		} UserInfo;
		struct {
				ULONG		FileNameLength;
				WCHAR		FileName[1];				
		} FileInfo;

	}ReplyData;
  
} MESSAGE_REPLY_DATA, *PMESSAGE_REPLY_DATA;

#define STATUS_ACCESS_DENIED				0xC0000022L

/// <summary>
/// install the filter driver service, it request the administrator privilege
/// </summary>
extern "C" __declspec(dllexport) 
BOOL 
InstallDriver();

/// <summary>
/// uninstall the filter driver service. 
/// </summary>
extern "C" __declspec(dllexport) 
BOOL
UnInstallDriver();

/// <summary>
/// set the registration key to enable the filter driver service.
/// </summary>
extern "C" __declspec(dllexport) 
BOOL
SetRegistrationKey(char* key);

typedef BOOL (__stdcall *Proto_Message_Callback)(
   IN		PMESSAGE_SEND_DATA pSendMessage,
   IN OUT	PMESSAGE_REPLY_DATA pReplyMessage);

typedef VOID (__stdcall *Proto_Disconnect_Callback)();

/// <summary>
/// Create the filter driver connection with callback settings
/// </summary>
/// <param name="threadCount">the number of working threads waitting for the callback message</param>
/// <param name="filterCallback">the callback function</param>
/// <param name="disconnectCallback">the disconnect callback function</param>
extern "C" __declspec(dllexport) 
BOOL
RegisterMessageCallback(
	ULONG ThreadCount,
	Proto_Message_Callback MessageCallback,
	Proto_Disconnect_Callback DisconnectCallback );

extern "C" __declspec(dllexport) 
VOID
Disconnect();

extern "C" __declspec(dllexport) 
BOOL
GetLastErrorMessage(WCHAR* Buffer, PULONG BufferLength);

extern "C" __declspec(dllexport)
BOOL
SetIntegerData(ULONG dataControlId, LONGLONG data );

extern "C" __declspec(dllexport)
BOOL
SetStringData(ULONG stringControlId, WCHAR* stringData);

/// <summary>
/// reset the filter driver config settings to the default value.
/// </summary>
extern "C" __declspec(dllexport)
BOOL
ResetConfigData();

extern "C" __declspec(dllexport)  
BOOL
ProtectCurrentProcess();

extern "C" __declspec(dllexport)  
BOOL
StopProtectCurrentProcess();

extern "C" __declspec(dllexport)  
BOOL
SetFilterType(ULONG FilterType);

/// <summary>
/// set the filter driver boolean config setting based on the enum booleanConfig
/// </summary>
extern "C" __declspec(dllexport)  
BOOL
SetBooleanConfig(ULONG booleanConfig);

/// <summary>
/// set the maximum monitor event buffer size if monitorBuffer was enabled.
/// </summary>
extern "C" __declspec(dllexport) 
BOOL
SetMaxMonitorEventBuffersize(ULONG maxBufferSize);

/// <summary>
/// set the maiximun wait time of the filter driver sending message to service.
/// </summary>
extern "C" __declspec(dllexport)  
BOOL
SetConnectionTimeout(ULONG TimeOutInSeconds);

extern "C" __declspec(dllexport)  
BOOL
SetVolumeControlFlag(ULONG volumeControlFlag);

//obsolete
extern "C" __declspec(dllexport) 
BOOL 
AddFilterRule(ULONG AccessFlag, WCHAR* FilterMask, WCHAR* FilterMask2 = NULL,ULONG keyLength = 0,PUCHAR key = NULL);

//obsolete
extern "C" __declspec(dllexport) 
BOOL 
AddNewFilterRule(ULONG accessFlag, WCHAR* filterMask,BOOL isResident = FALSE);

/// <summary>
/// Add the new filter rule to the filter driver.
/// </summary>
/// <param name="accessFlag">access control rights of the file IO to the files which match the filter mask</param>
/// <param name="filterMask">the filter rule file filter mask, it must be unique.</param>
/// <param name="isResident">if it is true, the filter rule will be added to the registry, get protection in boot time.</param>
/// <param name="filterRuleId">the id to identify the filter rule, it will show up in messageId field of messageSend structure if the callback is registered</param>
extern "C" __declspec(dllexport) 
BOOL 
AddFileFilterRule(ULONG accessFlag, WCHAR* filterMask,BOOL isResident = FALSE, ULONG filterRuleId = 0 );

/// <summary>
/// Remove the filter rule from the filter driver.
/// </summary>
/// <param name="filterMask">the filter rule file filter mask</param>
extern "C" __declspec(dllexport) 
BOOL 
RemoveFilterRule(WCHAR* FilterMask);

/// <summary>
///Set an encryption folder, every encrypted file has the unique iv key, 
///the encrypted information will be appended to the file as an header, 
///the filter driver will know if the file was encrypted by checking the header.
/// </summary>
extern "C" __declspec(dllexport) 
BOOL 
AddEncryptionKeyToFilterRule(WCHAR* filterMask,ULONG encryptionKeyLength,PUCHAR encryptionKey);

/// <summary>
///Set an encryption folder, all encrypted files use the same encryption key and IV key. 
/// </summary>
extern "C" __declspec(dllexport) 
BOOL 
AddEncryptionKeyAndIVToFilterRule(WCHAR* filterMask,ULONG encryptionKeyLength,PUCHAR encryptionKey,ULONG ivLength, PUCHAR iv);
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
extern "C" __declspec(dllexport) 
BOOL 
AddReparseFileMaskToFilterRule(WCHAR* filterMask,  WCHAR* reparseFilterMask);
/// <summary>
/// Hide the files from the browsing file list for the filter rule.
/// </summary>
/// <param name="filterMask">the filter rule file filter mask.</param>
/// <param name="hiddenFileFilterMask">the file filter mask for the hidden files.</param>
extern "C" __declspec(dllexport) 
BOOL 
AddHiddenFileMaskToFilterRule(WCHAR* filterMask,  WCHAR* hiddenFileFilterMask);

/// <summary>
/// Exclude the file IO from this filter rule if the files match the excludeFileFilterMask.
/// </summary>
extern "C" __declspec(dllexport) 
BOOL 
AddExcludeFileMaskToFilterRule(WCHAR* filterMask,  WCHAR* excludeFileFilterMask);

/// <summary>
/// only manage the IO of the filter rule for the processes in the included process id list 
/// </summary>
/// <param name="filterMask">the file filter mask of the filter rule</param>
/// <param name="includeProcessId">the included process Id</param>
extern "C" __declspec(dllexport) 
BOOL 
AddIncludeProcessIdToFilterRule(WCHAR* filterMask, ULONG includePID);

/// <summary>
/// skip the IO of the filter rule for the processes in the excluded process id list
/// </summary>
/// <param name="filterMask">the file filter mask of the filter rule</param>
/// <param name="excludeProcessId">the excluded process Id</param>
extern "C" __declspec(dllexport) 
BOOL 
AddExcludeProcessIdToFilterRule(WCHAR* filterMask, ULONG excludePID);

/// <summary>
/// only manage the IO of the filter rule for the processes in the included process list 
/// </summary>
/// <param name="filterMask">the file filter mask of the filter rule</param>
/// <param name="processName">the include process name filter mask, process name format:notepad.exe</param>
extern "C" __declspec(dllexport) 
BOOL 
AddIncludeProcessNameToFilterRule(WCHAR* filterMask,  WCHAR* processName);

/// <summary>
/// skip the IO of the filter rule for the processes in the excluded process list
/// </summary>
/// <param name="filterMask">the file filter mask of the filter rule</param>
/// <param name="processName">the include process name filter mask</param>
extern "C" __declspec(dllexport) 
BOOL 
AddExcludeProcessNameToFilterRule(WCHAR* filterMask,  WCHAR* processName);

/// <summary>
///  only manage the IO of the filter rule for user name in the included user name list 
/// </summary>
/// <param name="filterMask">the file filter mask of the filter rule</param>
/// <param name="userName">the included user name, format:domainName(or computerName)\userName.exe</param>
extern "C" __declspec(dllexport) 
BOOL 
AddIncludeUserNameToFilterRule(WCHAR* filterMask,  WCHAR* userName);

/// <summary>
///skip the IO of the filter rule for user name in the excluded user name list 
/// </summary>
/// <param name="filterMask">the file filter mask of the filter rule</param>
/// <param name="userName">the excluded user name, format:domainName(or computerName)\userName.exe</param>
extern "C" __declspec(dllexport) 
BOOL 
AddExcludeUserNameToFilterRule(WCHAR* filterMask,  WCHAR* processName);

/// <summary>
/// Filter the I/O based on the file create options,the IO will be skipped if the filter option is not 0
/// and the file create options don't match the filter.
/// </summary>
/// <param name="filterMask">the file filter mask of the filter rule</param>
/// <param name="filterByDesiredAccess">if it is not 0, the filter driver will check if the file creation option "DesiredAccess" matches the filter</param>
/// <param name="filterByDisposition">if it is not 0, the filter driver will check if the file creation option "Disposition" matches the filter</param>
/// <param name="filterByCreateOptions">if it is not 0, the filter driver will check if the file creation option "CreateOptions" matches the filter</param>
extern "C" __declspec(dllexport) 
BOOL 
AddCreateFilterToFilterRule(WCHAR* filterMask, ULONG filterByDesiredAccess, ULONG filterByDisposition,ULONG filterByCreateOptions);


/// <summary>
///obsolete API,use RegisterFileChangedEventsToFilterRule instead.
/// Register the file I/O event types for the filter rule, get the notification when the I/O was triggered
/// after the file handle was closed.
/// </summary>
/// <param name="filterMask">the file filter mask of the filter rule</param>
/// <param name="eventType">the I/O event types,reference the FileEventType enumeration.</param>
extern "C" __declspec(dllexport) 
BOOL 
RegisterEventTypeToFilterRule(WCHAR* filterMask, ULONG eventType);

/// <summary>
/// Register the file changed events for the filter rule, get the notification when the I/O was triggered
/// after the file handle was closed.
/// </summary>
/// <param name="filterMask">the file filter mask of the filter rule</param>
/// <param name="eventType">the I/O event types,reference the FileEventType enumeration.</param>
extern "C" __declspec(dllexport) 
BOOL 
RegisterFileChangedEventsToFilterRule(WCHAR* filterMask, ULONG eventType);

/// <summary>
/// Register the callback I/O for monitor filter driver to the filter rule.
/// </summary>
/// <param name="filterMask">the file filter mask of the filter rule</param>
/// <param name="registerIO">the specific I/Os you want to monitor</param>
extern "C" __declspec(dllexport) 
BOOL 
RegisterMonitorIOToFilterRule(WCHAR* filterMask, ULONGLONG registerIO);

/// <summary>
/// Register the callback I/O for control filter driver to the filter rule, you can change, block and pass the I/O
/// in your callback funtion.
/// </summary>
/// <param name="filterMask">the file filter mask of the filter rule</param>
/// <param name="registerIO">the specific I/Os you want to monitor or control</param>
extern "C" __declspec(dllexport) 
BOOL 
RegisterControlIOToFilterRule(WCHAR* filterMask, ULONGLONG registerIO);

/// <summary>
/// Set the access rights to the specific process
/// </summary>
/// <param name="filterMask">the file filter mask of the filter rule</param>
/// <param name="processName">the process name will be added the access rights, e.g. notepad.exe or c:\windows\*.exe</param>
/// <param name="accessFlags">the access rights</param>
extern "C" __declspec(dllexport) 
BOOL 
AddProcessRightsToFilterRule(WCHAR* filterMask,  WCHAR* processName, ULONG accessFlags);

/// <summary>
/// Remove the acces right setting for specific processes from filter rule
/// </summary>
/// <param name="filterMask">tthe filter rule file filter mask</param>
/// <param name="processName">the process name filter mask</param>
extern "C" __declspec(dllexport) 
BOOL
RemoveProcessRightsFromFilterRule(WCHAR* filterMask,  WCHAR* processName);

/// <summary>
/// Set the access control flags to process with the processId
/// </summary>
/// <param name="filterMask">the filter rule file filter mask</param>
/// <param name="processId">the process Id which will be added the access control flags</param>
/// <param name="accessFlags">the access control flags</param>
extern "C" __declspec(dllexport) 
BOOL 
AddProcessIdRightsToFilterRule(WCHAR* filterMask,  ULONG processId, ULONG accessFlags);

/// <summary>
/// Remove the acces right setting for specific process from filter rule
/// </summary>
/// <param name="filterMask">the filter rule file filter mask</param>
/// <param name="processName">the process Id</param>
extern "C" __declspec(dllexport) 
BOOL 
RemoveProcessIdRightsFromFilterRule(WCHAR* filterMask,  ULONG processId);

/// <summary>
/// Set the access control rights to specific users
/// </summary>
/// <param name="filterMask">the filter rule file filter mask</param>
/// <param name="userName">the user name you want to set the access right</param>
/// <param name="accessFlags">the access rights</param>
extern "C" __declspec(dllexport) 
BOOL 
AddUserRightsToFilterRule(WCHAR* filterMask,  WCHAR* userName, ULONG accessFlags);

/// <summary>
/// Get sha256 hash of the file, you need to allocate the 32 bytes array to get the sha256 hash.
/// hashBytesLength is the input byte array length, and the outpou lenght of the hash.
/// </summary>
extern "C" __declspec(dllexport) 
BOOL
Sha256HashFile(
	LPCTSTR					sourceFileName,
	BYTE*					hashBytes,
	ULONG*					hashBytesLength);

/// <summary>
/// Add the access rights to the process which has the same sha256 hash as the setting.
/// </summary>
/// <param name="filterMask">The filter rule file filter mask.</param>
/// <param name="imageSha256">the sha256 hash of the executable binary file.</param>
/// <param name="hashLength">the length of the sha256 hash, by default is 32.</param>
/// <param name="accessFlags">the access flags for the setting process.</param>
/// <returns>return true if it is succeeded.</returns>
extern "C" __declspec(dllexport) 
BOOL 
AddSha256ProcessAccessRightsToFilterRule(
	WCHAR* filterMask, 
	PUCHAR imageSha256,
	ULONG hashLength, 
	ULONG accessFlags);

/// <summary>
/// Add the access rights of the process which was signed with the code certificate
/// to the filter rule.
/// </summary>
/// <param name="filterMask">The filter rule file filter mask.</param>
/// <param name="imageSha256">the sha256 hash of the executable binary file.</param>
/// <param name="hashLength">the length of the sha256 hash, by default is 32.</param>
/// <param name="accessFlags">the access flags for the setting process.</param>
/// <returns>return true if it is succeeded.</returns>
extern "C" __declspec(dllexport) 
BOOL 
AddSignedProcessAccessRightsToFilterRule(
	WCHAR* filterMask, 
	WCHAR* certificateName,
	ULONG lengthOfCertificate, 
	ULONG accessFlags);

/// <summary>
/// Add the boolean config setting to a filter rule.
/// Reference BooleanConfig settings
/// </summary>
/// <param name="filterMask">the filter rule file filter mask</param>
/// <param name="booleanConfig">the boolean config setting</param>
extern "C" __declspec(dllexport) 
BOOL 
AddBooleanConfigToFilterRule(WCHAR* filterMask, ULONG booleanConfig);

extern "C" __declspec(dllexport) 
BOOL 
RemoveBooleanConfigFromFilterRule(WCHAR* filterMask);

extern "C" __declspec(dllexport) 
BOOL 
AddExcludedProcessId(ULONG ProcessId);

extern "C" __declspec(dllexport) 
BOOL 
RemoveExcludeProcessId(ULONG ProcessId);

extern "C" __declspec(dllexport)
BOOL 
AddIncludedProcessId(ULONG processId);

extern "C" __declspec(dllexport) 
BOOL 
RemoveIncludeProcessId(ULONG processId);

extern "C" __declspec(dllexport)
BOOL 
AddProtectedProcessId(ULONG processId);

extern "C" __declspec(dllexport) 
BOOL 
RemoveProtectedProcessId(ULONG processId);

extern "C" __declspec(dllexport)
BOOL 
AddBlockSaveAsProcessId(ULONG processId);

extern "C" __declspec(dllexport) 
BOOL 
RemoveBlockSaveAsProcessId(ULONG processId);

extern "C" __declspec(dllexport) 
BOOL 
RegisterIoRequest(ULONG RequestRegistration);

extern "C" __declspec(dllexport) 
BOOL	
GetFileHandleInFilter(WCHAR* FileName,ULONG  DesiredAccess,HANDLE*	FileHandle);

extern "C" __declspec(dllexport) 
BOOL
CloseFileHandleInFilter(HANDLE hFile);

extern "C" __declspec(dllexport) 
BOOL
IsDriverServiceRunning();

extern "C" __declspec(dllexport) 
BOOL
OpenStubFile(
    LPCTSTR fileName,
    DWORD   dwDesiredAccess,
    DWORD   dwShareMode,
    PHANDLE pHandle );

extern "C" __declspec(dllexport) 
BOOL
CreateFileAPI(
	LPCTSTR		fileName,
	DWORD		dwDesiredAccess,
	DWORD		dwShareMode,
	DWORD		dwCreationDisposition,
	DWORD		dwFlagsAndAttributes,
	PHANDLE		pHandle );

extern "C" __declspec(dllexport) 
BOOL
CreateStubFile(
	LPCTSTR		fileName,
	LONGLONG	fileSize,
	ULONG		fileAttributes,
	ULONG		tagDataLength,
	BYTE*		tagData,
	BOOL		overwriteIfExist,
	PHANDLE		pHandle );

extern "C" __declspec(dllexport) 
BOOL
GetTagData(
	HANDLE hFile,
	PULONG tagDataLength,
	BYTE*  tagData);

extern "C" __declspec(dllexport) 
BOOL  
RemoveTagData(
    HANDLE hFile ,
	BOOLEAN	updateTimeStamp = FALSE);

extern "C" __declspec(dllexport) 
BOOL 
AddTagData(
    HANDLE  hFile, 
    ULONG   tagDataLength,
	BYTE*	tagData );

extern "C" __declspec(dllexport) 
BOOL 
AddReparseTagData(
	LPCTSTR		fileName,
    ULONG		tagDataLength,
	BYTE*		tagData );

extern "C" __declspec(dllexport) 
BOOL  
QueryAllocatedRanges(
    IN HANDLE							hFile, 
    IN LONGLONG                         queryOffset,
    IN LONGLONG                         queryLength,
    IN OUT PFILE_ALLOCATED_RANGE_BUFFER allocatedBuffer,
    IN ULONG                            allocatedBufferSize,   
    OUT ULONG                           *returnBufferLength  );

extern "C" __declspec(dllexport) 
BOOL
SetFileSize(
	HANDLE			hFile,
	LONGLONG		fileSize);

extern "C" __declspec(dllexport) 
BOOL
AESEncryptFile(
	LPCTSTR					fileName,
	ULONG					keyLength,
	BYTE*					key,
	ULONG					ivLength = 0,
	BYTE*					iv = NULL,
	BOOLEAN					addAESData = TRUE);

extern "C" __declspec(dllexport) 
BOOL
AESDecryptFile(
	LPCTSTR		fileName,
	ULONG		keyLength,
	BYTE*		key,
	ULONG		ivLength = 0,
	BYTE*		iv = NULL);

extern "C" __declspec(dllexport) 
BOOL
AESEncryptFileToFileWithTag(
	LPCTSTR					sourceFileName,
	LPCTSTR					destFileName,
	ULONG					keyLength,
	BYTE*					key,
	ULONG					ivLength,
	BYTE*					iv,
	ULONG					tagDataLength,
	BYTE*					tagData );

extern "C" __declspec(dllexport) 
BOOL
AESEncryptFileWithTag(
	LPCTSTR					fileName,
	ULONG					keyLength,
	BYTE*					key,
	ULONG					ivLength,
	BYTE*					iv,
	ULONG					tagDataLength,
	BYTE*					tagData );

extern "C" __declspec(dllexport) 
BOOL
AESEncryptFileToFile(
	LPCTSTR					sourceFileName,
	LPCTSTR					destFileName,
	ULONG					keyLength,
	BYTE*					key,
	ULONG					ivLength,
	BYTE*					iv,
	BOOLEAN					addAESData );

extern "C" __declspec(dllexport) 
BOOL
AESDecryptFileWithKey(
	LPCTSTR		fileName,
	ULONG		keyLength,
	BYTE*		key );

extern "C" __declspec(dllexport) 
BOOL
AESDecryptFileToFile(
	LPCTSTR					sourceFileName,
	LPCTSTR					destFileName,
	ULONG					keyLength,
	BYTE*					key,
	ULONG					ivLength = 0,
	BYTE*					iv = NULL);
 
extern "C" __declspec(dllexport) 
BOOL
AESEncryptDecryptBuffer(
	BYTE*					inputBuffer,
	BYTE*					outputBuffer,
	ULONG					bufferLength,
	LONGLONG				offset,
	BYTE*					key,
	ULONG					keyLength,
	BYTE*					ivKey,
	ULONG					ivLength);

extern "C" __declspec(dllexport) 
ULONG
GetComputerId();

extern "C" __declspec(dllexport) 
BOOL
ActivateLicense(
	BYTE*					buffer,
	ULONG					bufferLength);


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
extern "C" __declspec(dllexport) 
BOOL
AddRegistryFilterRule(	    
	ULONG		prcoessNameLength,
	WCHAR*		processName,
	ULONG		processId, 
	ULONG		userNameLength,
	WCHAR*		userName, 
	ULONG		keyNameLength,
	WCHAR*		keyName, 
	ULONG		accessFlag,
	ULONGLONG	regCallbackClass,
	BOOL		isExcludeFilter,
	ULONG		filterRuleId);

/// <summary>
/// Add registry access control filter entry with process name, if process name filter mask matches the proces,
/// it will set the access flag to the process.
/// </summary>
/// <param name="processNameLength">The length of the process name string in bytes</param>
/// <param name="processName">The process name to be filtered, all processes if it is '*' </param>
/// <param name="accessFlag">The access control flag to the registry</param>
/// <param name="regCallbackClass">The registered callback registry access class</param>
/// <param name="isExcludeFilter">Skip all the registry access from this process filter mask if it is true.</param>
extern "C" __declspec(dllexport) 
BOOL
AddRegistryFilterRuleByProcessName(
	ULONG		processNameLength,
	WCHAR*		processName, 
	ULONG		accessFlag,
	ULONGLONG	regCallbackClass,
	BOOL		isExcludeFilter );

extern "C" __declspec(dllexport) 
BOOL
AddRegistryFilterRuleByProcessId(
	ULONG		processId,
	ULONG		accessFlag,
	ULONGLONG	regCallbackClass,
	BOOL		isExcludeFilter );

extern "C" __declspec(dllexport) 
BOOL
RemoveRegistryFilterRuleByProcessId(
	ULONG		processId );

extern "C" __declspec(dllexport) 
BOOL
RemoveRegistryFilterRuleByProcessName(
	ULONG		prcoessNameLength,
	WCHAR*		processName );

/// <summary>
/// Add the process filter entry,get notification of new process/thread creation or termination.
/// prevent the unauthorized excutable binaries from running.
/// </summary>
/// <param name="processNameMaskLength">the process name mask length</param>
/// <param name="processNameMask">the process name mask, i.e. c:\myfolder\*.exe</param>
/// <param name="controlFlag">the control flag of the process</param>
extern "C" __declspec(dllexport) 
BOOL
AddProcessFilterRule(	
	ULONG		prcoessNameMaskLength,
	WCHAR*		processNameMask,
	ULONG		controlFlag,
	ULONG		filterRuleId = 0 );

extern "C" __declspec(dllexport) 
BOOL
RemoveProcessFilterRule(
	ULONG		prcoessNameMaskLength,
	WCHAR*		processNameMask );

/// <summary>
/// Add the file control access rights to the process
/// </summary>
/// <param name="processNameMaskLength">the length of the process name filter mask</param>
/// <param name="processNameMask">the process name filter mask</param>
/// <param name="fileNameMaskLength">the length of the file name filter mask</param>
/// <param name="fileNameMask">the file name filter mask</param>
/// <param name="AccessFlag">set the file access control flag if the control filter is enabled</param>
extern "C" __declspec(dllexport) 
BOOL
AddFileControlToProcessByName(	
	ULONG		prcoessNameMaskLength,
	WCHAR*		processNameMask,
	ULONG		fileNameMaskLength,
	WCHAR*		fileNameMask,
	ULONG		AccessFlag );

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
extern "C" __declspec(dllexport) 
BOOL
AddFileCallbackIOToProcessByName(	
	ULONG		prcoessNameMaskLength,
	WCHAR*		processNameMask,
	ULONG		fileNameMaskLength,
	WCHAR*		fileNameMask,
	ULONG		monitorIOs,
	ULONG		controlIOs,
	ULONG		filterByDesiredAccess,
    ULONG		filterByDisposition,
    ULONG		filterByCreateOptions);

extern "C" __declspec(dllexport) 
BOOL
RemoveFileControlFromProcessByName(	
	ULONG		prcoessNameMaskLength,
	WCHAR*		processNameMask,
	ULONG		fileNameMaskLength,
	WCHAR*		fileNameMask);

/// <summary>
/// This is the API to add the file access rights of the specific files to the specific processes by process Id
/// This feature requires the control filter was enabled
/// </summary>
/// <param name="processId">the process Id it will be filtered</param>
/// <param name="fileNameMaskLength">the length of the file name filter mask</param>
/// <param name="fileNameMask">the file name filter mask</param>
/// <param name="AccessFlag">the file access control flag</param>
extern "C" __declspec(dllexport) 
BOOL
AddFileControlToProcessById(	
	ULONG		prcoessId,
	ULONG		fileNameMaskLength,
	WCHAR*		fileNameMask,
	ULONG		AccessFlag );

extern "C" __declspec(dllexport) 
BOOL
RemoveFileControlFromProcessById(	
	ULONG		prcoessId,
	ULONG		fileNameMaskLength,
	WCHAR*		fileNameMask);

extern "C" __declspec(dllexport) 
BOOL
SetAESVersion(ULONG version);

extern "C" __declspec(dllexport) 
BOOL
SetAESHeaderSize(ULONG headerSize);

extern "C" __declspec(dllexport) 
BOOL
EnableReparsePointTagEncryption();

extern "C" __declspec(dllexport) 
BOOL
AddAESHeader(
	LPCTSTR		fileName,
	ULONG		headerSize,
	BYTE*		header);

extern "C" __declspec(dllexport) 
BOOL
GetAESHeader(
	LPCTSTR		fileName,
	PULONG		headerSize,
	BYTE*		header);

extern "C" __declspec(dllexport) 
BOOL
SetHeaderFlags(
 	LPCTSTR		fileName,
	ULONG		aesFlags,
	ULONG		accessFlags );

extern "C" __declspec(dllexport) 
BOOL
GetAESTagData(
	LPCTSTR		fileName,
	PULONG		tagDataSize,
	BYTE*		tagData);

extern "C" __declspec(dllexport) 
BOOL
GetAESIV(
	LPCTSTR		fileName,
	PULONG		ivSize,
	BYTE*		ivBuffer);

#endif//__SHARE_TYPE_H__
