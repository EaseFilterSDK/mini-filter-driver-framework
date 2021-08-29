unit FilterAPI;

interface

uses
  SysUtils,
  Classes,
  Windows;

/// ////////////////////////////////////////////////////////////////////////////
//
// (C) Copyright 2011 EaseFilter Technologies Inc.
// All Rights Reserved
//
// This software is part of a licensed software product and may
// only be used or copied in accordance with the terms of that license.
//
// This header file includes the structures and exported API from the FilterAPI.DLL
//
//
/// ////////////////////////////////////////////////////////////////////////////


// #ifndef __FILTER_API_H__
// #define __FILTER_API_H__

const
  STATUS_ACCESS_DENIED = $C0000022;
  MESSAGE_SEND_VERIFICATION_NUMBER = $FF000001;
  BLOCK_SIZE = 65536;
  MAX_FILE_NAME_LENGTH = 1024;
  MAX_SID_LENGTH = 256;
  MAX_PATH = 260;
  MAX_EXCLUDED_PROCESS_ID = 200;
  MAX_INCLUDED_PROCESS_ID = 200;
  MAX_PROTECTED_PROCESS_ID = 200;
  MAX_BLOCK_SAVEAS_PROCESS_ID = 200;
  MAX_ERROR_MESSAGE_SIZE = 1024;
  MAX_REQUEST_TYPE = 32;

  //
  // define the filter driver request message type
  //
  MESSAGE_TYPE_RESTORE_BLOCK_OR_FILE = $00000001; // required to restore the block data or full data of the stub file
  MESSAGE_TYPE_RESTORE_FILE = $00000002; // required to restore the full data of the stub file, for memory
  // mapping file open or write request, we need to restore the stub file
  MESSAGE_TYPE_RESTORE_FILE_TO_CACHE = $00000008; // require to download the whole file to the cache folder
  MESSAGE_TYPE_SEND_EVENT_NOTIFICATION = $00000010; // the send notification event request, don't need to reply this request.

  EASETAG_KEY = $BBA65D6F;
  PEERTAG_KEY = $BBA65D77;

  STATUS_SUCCESS = 0;
  STATUS_REPARSE = $00000104;
  STATUS_NO_MORE_FILES = $80000006;
  STATUS_WARNING = $80000000;
  STATUS_ERROR = $C0000000;
  STATUS_UNSUCCESSFUL = $C0000001;
  STATUS_END_OF_FILE = $C0000011;

  ENABLE_NO_RECALL_FLAG = $00000001;


type
  TFilterType = (
    FILE_SYSTEM_MONITOR = 0,
    FILE_SYSTEM_CONTROL = 1,
    FILE_SYSTEM_ENCRYPTION = 2,
    FILE_SYSTEM_CONTROL_ENCRYPTION = 3,
    FILE_SYSTEM_MONITOR_ENCRYPTION = 4,
    FILE_SYSTEM_EASE_FILTER_ALL = 5,
    FILE_SYSTEM_MONITOR_CONTROL = 8,
    FILE_SYSTEM_HSM = $10,
    FILE_SYSTEM_CLOUD = $20 );

  type
  FilterCommand = (
    FILTER_SEND_FILE_CHANGED_EVENT = $00010001,
    FILTER_REQUEST_USER_PERMIT = $00010002,
    FILTER_REQUEST_ENCRYPTION_KEY = $00010003,
    FILTER_REQUEST_ENCRYPTION_IV_AND_KEY = $00010004);

type
  PEASETAG_DATA = ^EASETAG_DATA;

  EASETAG_DATA = Record
    EaseTagKey: ULONG;
    Flags: ULONG;
    FileNameLength: ULONG;
    //FileName: Array of Char;
    //FileName: Array [0 .. MAX_FILE_NAME_LENGTH - 1] of Char;
    FileName: WideChar;//Array [0..1024] of WideChar;
  end;

{///the I/O types of the monitor or control filter can intercept. }
type
  MessageType = (
    PRE_CREATE = $00000001,
    POST_CREATE = $00000002,
    PRE_FASTIO_READ = $00000004,
    POST_FASTIO_READ = $00000008,
    PRE_CACHE_READ = $00000010,
    POST_CACHE_READ = $00000020,
    PRE_NOCACHE_READ = $00000040,
    POST_NOCACHE_READ = $00000080,
    PRE_PAGING_IO_READ = $00000100,
    POST_PAGING_IO_READ	= $00000200,
    PRE_FASTIO_WRITE = $00000400,
    POST_FASTIO_WRITE	= $00000800,
    PRE_CACHE_WRITE	= $00001000,
    POST_CACHE_WRITE = $00002000,
    PRE_NOCACHE_WRITE	= $00004000,
    POST_NOCACHE_WRITE = $00008000,
    PRE_PAGING_IO_WRITE	= $00010000,
    POST_PAGING_IO_WRITE = $00020000,
    PRE_QUERY_INFORMATION	= $00040000,
    POST_QUERY_INFORMATION = $00080000,
    PRE_SET_INFORMATION	= $00100000,
    POST_SET_INFORMATION = $00200000,
    PRE_DIRECTORY	= $00400000,
    POST_DIRECTORY = $00800000,
    PRE_QUERY_SECURITY = $01000000,
    POST_QUERY_SECURITY	= $02000000,
    PRE_SET_SECURITY = $04000000,
    POST_SET_SECURITY	= $08000000,
    PRE_CLEANUP	= $10000000,
    POST_CLEANUP = $20000000,
    PRE_CLOSE	= $40000000,
    POST_CLOSE = $80000000);


{///the flags of the access control to the file. }
type
  AccessFlag = (
    /// Filter driver will skip all the IO if the file name match the include file mask.
    EXCLUDE_FILTER_RULE = $00000000,
    /// Block the file open.
    EXCLUDE_FILE_ACCESS = $00000001,
    /// Reparse the file open to the new file name if the reparse file mask was added.
    REPARSE_FILE_OPEN = $00000002,
   /// Hide the files from the folder directory list if the hide file mask was added.
    HIDE_FILES_IN_DIRECTORY_BROWSING = $00000004,
    /// Enable the transparent file encryption if the encryption key was added.
    FILE_ENCRYPTION_RULE = $00000008,
    /// Allow the file open to access the file's security information.
    ALLOW_OPEN_WTIH_ACCESS_SYSTEM_SECURITY = $00000010,
    /// Allow the file open for read access.
    ALLOW_OPEN_WITH_READ_ACCESS	= $00000020,
    /// Allow the file open for write access.
    ALLOW_OPEN_WITH_WRITE_ACCESS = $00000040,
    /// Allow the file open for create new file or overwrite access.
    ALLOW_OPEN_WITH_CREATE_OR_OVERWRITE_ACCESS = $00000080,
    /// Allow the file open for delete.
    ALLOW_OPEN_WITH_DELETE_ACCESS		= $00000100,
    /// Allow to read the file data.
    ALLOW_READ_ACCESS= $00000200,
    /// Allow write data to the file.
    ALLOW_WRITE_ACCESS = $00000400,
    /// Allow to query file information.
    ALLOW_QUERY_INFORMATION_ACCESS = $00000800,
    /// Allow to change the file information:file attribute,file size,file name,delete file
    ALLOW_SET_INFORMATION = $00001000,
    /// Allow to rename the file.
    ALLOW_FILE_RENAME = $00002000,
    /// Allow to delete the file.
    ALLOW_FILE_DELETE = $00004000,
    /// Allow to change file size.
    ALLOW_FILE_SIZE_CHANGE = $00008000,
    /// Allow query the file security information.
    ALLOW_QUERY_SECURITY_ACCESS = $00010000,
    /// Allow change the file security information.
    ALLOW_SET_SECURITY_ACCESS = $00020000,
    /// Allow to browse the directory file list.
    ALLOW_DIRECTORY_LIST_ACCESS	= $00040000,
    /// Allow the remote access via share folder.
    ALLOW_FILE_ACCESS_FROM_NETWORK = $00080000,
    /// Allow to encrypt the new file if the encryption filter rule is enabled.
    ALLOW_NEW_FILE_ENCRYPTION	= $00100000,
    /// Allow the application to create a new file after it opened the protected file.
    ALLOW_ALL_SAVE_AS = $00200000,
    /// Allow the application in the inlcude process list to create a new file after it opened the protected file.
    ALLOW_INCLUDE_PROCESS_SAVE_AS = $00400000,
    /// Allow the file to be executed.
   	ALLOW_FILE_MEMORY_MAPPED = $00800000,

    ALLOW_MAX_RIGHT_ACCESS =  $fffffff0);



type // this is the data structure which send data from kernel to user mode.
  PMESSAGE_SEND_DATA = ^MESSAGE_SEND_DATA;
  MESSAGE_SEND_DATA = Record
    MessageId: ULONG;
    FileObject: Pointer;
    FsContext: Pointer;
    MessageType: ULONG;
    ProcessId: ULONG;
    ThreadId: ULONG;
    Offset: Int64; // read/write offset
    Length: ULong; // read/write length
    FileSize: Int64;
    TransactionTime: Int64;
    CreationTime: Int64;
    LastAccessTime: Int64;
    LastWriteTime: Int64;
    FileAttributes: ULONG;
    // The disired access,share access and disposition for Create request.
    DesiredAccess: ULONG;
    Disposition: ULONG;
    ShareAccess: ULONG;
    CreateOptions: ULONG;
    CreateStatus: ULONG;
    // For QueryInformation,SetInformation,Directory request it is information class
    // For QuerySecurity and SetSecurity request,it is securityInformation.
    InfoClass: ULONG;
    Status: ULONG;
    FileNameLength: ULONG;
    FileName: Array [0 .. MAX_FILE_NAME_LENGTH - 1] of CHAR; // WCHAR			FileName[MAX_FILE_NAME_LENGTH];
    SidLength: ULONG;
    Sid: Array [0 .. MAX_SID_LENGTH - 1] of Byte; // UCHAR			Sid[MAX_SID_LENGTH];
    DataBufferLength: ULONG;
    DataBuffer: Array [0 .. BLOCK_SIZE - 1] of Byte; // UCHAR			DataBuffer[BLOCK_SIZE];
    VerificationNumber: ULONG;
  end;


  // This the structure return back to filter,only for call back filter.
  PMESSAGE_REPLY_DATA = ^MESSAGE_REPLY_DATA;
  MESSAGE_REPLY_DATA = Record
    MessageId: ULONG;
    MessageType: ULONG;
    ReturnStatus: ULONG;
    FilterStatus: ULONG;
    DataBufferLength: ULONG;
    DataBuffer: Array [0 .. BLOCK_SIZE - 1] of BYTE; // UCHAR			DataBuffer[BLOCK_SIZE];
  end;


type
  // this is the data structure which send data from kernel to user mode.
  TEVENT_TYPE = (
  FILE_CREATEED = $00000020,
  FILE_WRITTEN = $00000040,
  FILE_RENAMED = $00000080,
  FILE_DELETED = $00000100,
  FILE_SECURITY_CHANGED = $00000200,
  FILE_INFO_CHANGED	= $00000400,
  FILE_READ = $00000800);

  // The status return to filter,instruct filter what process needs to be done.
  FilterStatus = (
    FILTER_MESSAGE_IS_DIRTY = $00000001, // Set this flag if the reply message need to be processed.
    FILTER_COMPLETE_PRE_OPERATION = $00000002, // Set this flag if complete the pre operation.
    FILTER_DATA_BUFFER_IS_UPDATED = $00000004, // Set this flag if the databuffer was updated.
    BLOCK_DATA_WAS_RETURNED = $00000008, // Set this flag if return read block databuffer to filter.
    CACHE_FILE_WAS_RETURNED = $00000010,
    RESTORE_STUB_FILE_WITH_CACHE_FILE = $00000020); // Set this flag if the cache file was restored.


type // this is the data structure which send data from kernel to user mode.
  PProto_Message_Callback = ^Proto_Message_Callback;
  Proto_Message_Callback = Record
    pSendMessage: MESSAGE_SEND_DATA;
    pReplyMessage: MESSAGE_REPLY_DATA;
  end;

type // this is the data structure which send data from kernel to user mode.
  PProto_Disconnect_Callback = ^Proto_Disconnect_Callback;
  Proto_Disconnect_Callback = Record
  end;

type
  TMessageCallback = function(MsgOut: String): Boolean of object;

type
  /// ///////////////////////////////////////////////////////////////////////////
  // FilterAPI.DLL INterface functions
  TInstallDriver = function(): ULONG; cdecl;
  TUninstallDriver = function(): ULONG; cdecl;
  TSetRegistrationKey = function(key: PAnsiChar): ULONG; cdecl; // C++ apparently needs this value to be an AnsiString.......
  TIsDriverServiceRunning = function(): BOOL; cdecl;
  TRegisterMessageCallback = function(ThreadCount: ULONG; MessageCallback: Pointer; DisconnectCallback: Pointer): BOOL; cdecl;
  TDisconnect = procedure(); cdecl;
  TGetLastErrorMessage = function(Buffer: PChar; var BufferLength: ULONG): ULONG; cdecl;
  TSetFilterType = function(FilterType:ULONG): ULONG; cdecl;
  TResetConfigData = function(): ULONG; cdecl;
  TSetBooleanConfig = function(booleanConfig: ULONG): ULONG; cdecl;
  TSetConnectionTimeout = function(TimeOutInSeconds: ULONG): ULONG; cdecl;

  TAddNewFilterRule = function(AccessFlag: ULONG; FilterMask: PChar; IsResident:BOOL): ULONG; cdecl;
  TAddExcludeFileMaskToFilterRule = function(FilterMask: PChar; ExcludeFileFilterMask:PChar): ULONG; cdecl;
  TAddHiddenFileMaskToFilterRule = function(FilterMask: PChar; HiddenFileFilterMask:PChar): ULONG; cdecl;
  TAddReparseFileMaskToFilterRule = function(FilterMask: PChar; ReparseFileFilterMask:PChar): ULONG; cdecl;
  TAddEncryptionKeyToFilterRule = function(FilterMask: PChar; EncryptionKeyLength:ULONG; EncryptionKey:PBYTE): ULONG; cdecl;
  TAddIncludeProcessNameToFilterRule = function(FilterMask: PChar; ProcessName:PChar): ULONG; cdecl;   //process name format:  notepad.exe
  TAddExcludeProcessNameToFilterRule = function(FilterMask: PChar; ProcessName:PChar): ULONG; cdecl;   //process name format:  notepad.exe
  TAddIncludeProcessIdToFilterRule = function(FilterMask: PChar; IncludeProcessId:ULONG): ULONG; cdecl;
  TAddExcludeProcessIdToFilterRule = function(FilterMask: PChar; ExcludeProcessId:ULONG): ULONG; cdecl;
  TAddIncludeUserNameToFilterRule = function(FilterMask: PChar; UserName:PChar): ULONG; cdecl; //UserName format:  domainName(or computer name)\userName.exe
  TAddExcludeUserNameToFilterRule = function(FilterMask: PChar; UserName:PChar): ULONG; cdecl; //UserName format:  domainName(or computer name)\userName.exe
  TRegisterEventTypeToFilterRule = function(FilterMask: PChar; EventType:ULONG): ULONG; cdecl; //only works if Monitor Filter feature was enabled
  TRegisterMoinitorIOToFilterRule = function(FilterMask: PChar; RegisterIO:ULONG): ULONG; cdecl; //only works if Monitor Filter feature was enabled
  TRegisterControlIOToFilterRule = function(FilterMask: PChar; RegisterIO:ULONG): ULONG; cdecl; //only works if Control Filter feature was enabled
  TAddProcessRightsToFilterRule = function(FilterMask: PChar;ProcessName:PChar; AccessFlags:ULONG): ULONG; cdecl;
  TAddUserRightsToFilterRule = function(FilterMask: PChar;UserName:PChar; AccessFlags:ULONG): ULONG; cdecl;

  TAddExcludedProcessId = function(ProcessId: ULONG): ULONG; cdecl;
  TRemoveExcludeProcessId = function(ProcessId: ULONG): ULONG; cdecl;
  TAddIncludedProcessId = function(ProcessID: ULONG): ULONG; cdecl;
  TRemoveIncludedProcessId = function(ProcessID: ULONG): ULONG; cdecl;
  TAddProtectedProcessId = function(ProcessID: ULONG): ULONG; cdecl;
  TRemoveProtectedProcessId = function(ProcessID: ULONG): ULONG; cdecl;

  TAESEncryptFile = function(FileName: LPCTSTR; KeyLength: DWORD; EncryptionKey: PBYTE; ivLength: DWORD; iv: PBYTE; AddIVTag: BOOL ): ULONG; cdecl;
  TAESDecryptFile = function(FileName: LPCTSTR; KeyLength: DWORD; EncryptionKey: PBYTE; ivLength: DWORD; iv: PBYTE): ULONG; cdecl;

  TAESEncryptFileToFile = function(FileName: LPCTSTR; DestFileName: LPCTSTR; KeyLength: DWORD; EncryptionKey: PBYTE; ivLength: DWORD; iv: PBYTE; AddIVTag: BOOL ): ULONG; cdecl;
  TAESDecryptFileToFile = function(FileName: LPCTSTR; DestFileName: LPCTSTR; KeyLength: DWORD; EncryptionKey: PBYTE; ivLength: DWORD; iv: PBYTE): ULONG; cdecl;

  TGetFileHandleInFilter = function(FileName: PChar; DesiredAccess: ULONG; var FileHandle: THandle): ULONG; cdecl;
  TOpenStubFile = function(FileName: LPCTSTR; dwDesiredAccess: DWORD; dwShareMode: DWORD; var pHandle: THandle): ULONG; cdecl;
  TCreateStubFile = function(FileName: LPCTSTR; FileSize: LONGLONG; FileAttributes: ULONG; tagDataLength: ULONG;
  // tagData: PEASETAG_DATA;
  tagData: Pointer; overwriteIfExist: BOOL; var pHandle: THandle): ULONG; cdecl;

  TGetTagData = function(hFile: THandle; var tagDataLength: ULONG; var tagData: Array of Byte): ULONG; cdecl;
  TRemoveTagData = function(hFile: THandle; UpdateTimeStamp: Boolean = false): ULONG; cdecl;
  TAddTagData = function(hFile: THandle; tagDataLength: ULONG; tagData: PBYTE): ULONG; cdecl;


function GetLastFilterAPIErrorMsg: String;
function LoadFilterAPI(DLLName: String; var DLLFilterAPIHandle: NativeInt; var ErrMsg: String): NativeInt;

var
  InstallDriver: TInstallDriver = nil;
  UninstallDriver: TUninstallDriver = nil;
  SetRegistrationKey: TSetRegistrationKey = nil;
  IsDriverServiceRunning: TIsDriverServiceRunning = nil;
  RegisterMessageCallback: TRegisterMessageCallback = nil;
  Disconnect: TDisconnect = nil;
  GetFDLastErrorMessage: TGetLastErrorMessage = nil;
  SetFilterType: TSetFilterType = nil;
  ResetConfigData: TResetConfigData = nil;
  SetBooleanConfig: TSetBooleanConfig = nil;
  SetConnectionTimeout: TSetConnectionTimeout = nil;

  AddIncludedProcessId: TAddIncludedProcessId = nil;
  RemoveExcludedProcessId: TRemoveIncludedProcessId = nil;
  AddExcludedProcessId: TAddExcludedProcessId = nil;
  RemoveExcludeProcessId: TRemoveExcludeProcessId = nil;

  AddProtectedProcessId: TAddProtectedProcessId = nil;
  RemoveProtectedProcessId: TRemoveProtectedProcessId = nil;

  GetFileHandleInFilter: TGetFileHandleInFilter = nil;
  OpenStubFile: TOpenStubFile = nil;
  CreateStubFile: TCreateStubFile = nil;
  GetTagData: TGetTagData = nil;
  RemoveTagData: TRemoveTagData = nil;
  AddTagData: TAddTagData = nil;

  AddNewFilterRule: TAddNewFilterRule = nil;
  AddExcludeFileMaskToFilterRule: TAddExcludeFileMaskToFilterRule = nil;
  AddHiddenFileMaskToFilterRule: TAddHiddenFileMaskToFilterRule = nil;
  AddReparseFileMaskToFilterRule: TAddReparseFileMaskToFilterRule = nil;
  AddEncryptionKeyToFilterRule: TAddEncryptionKeyToFilterRule = nil;
  AddIncludeProcessNameToFilterRule: TAddIncludeProcessNameToFilterRule = nil;
  AddExcludeProcessNameToFilterRule: TAddExcludeProcessNameToFilterRule = nil;
  AddIncludeProcessIdToFilterRule: TAddIncludeProcessIdToFilterRule = nil;
  AddExcludeProcessIdToFilterRule: TAddExcludeProcessIdToFilterRule = nil;
  AddIncludeUserNameToFilterRule: TAddIncludeUserNameToFilterRule = nil;
  AddExcludeUserNameToFilterRule: TAddExcludeUserNameToFilterRule = nil;
  RegisterEventTypeToFilterRule: TRegisterEventTypeToFilterRule = nil;
  RegisterMoinitorIOToFilterRule: TRegisterMoinitorIOToFilterRule = nil;
  RegisterControlIOToFilterRule: TRegisterControlIOToFilterRule = nil;
  AddProcessRightsToFilterRule: TAddProcessRightsToFilterRule = nil;
  AddUserRightsToFilterRule: TAddUserRightsToFilterRule = nil;

  AESEncryptFile: TAESEncryptFile = nil;
  AESDecryptFile: TAESDecryptFile = nil;

  AESEncryptFileToFile: TAESEncryptFileToFile = nil;
  AESDecryptFileToFile: TAESDecryptFileToFile = nil;


implementation


function LoadFilterAPI(DLLName: String; var DLLFilterAPIHandle: NativeInt; var ErrMsg: String): NativeInt;
begin

  Result := 0;
  ErrMsg := '';

  if (DLLFilterAPIHandle > 0) then
    exit; // only need to get this handle once during active session

  SetLastError(0);
  DLLFilterAPIHandle := 0;
  DLLFilterAPIHandle := LoadLibrary(PChar(DLLName));
  Result := GetLastError;

  if (Result <> ERROR_INVALID_HANDLE) and (DLLFilterAPIHandle > 0) then
    begin
      Result := 0;

      InstallDriver := GetProcAddress(DLLFilterAPIHandle, 'InstallDriver');
      UninstallDriver := GetProcAddress(DLLFilterAPIHandle, 'UnInstallDriver');
      SetRegistrationKey := GetProcAddress(DLLFilterAPIHandle, 'SetRegistrationKey');
      IsDriverServiceRunning := GetProcAddress(DLLFilterAPIHandle, 'IsDriverServiceRunning');
      RegisterMessageCallback := GetProcAddress(DLLFilterAPIHandle, 'RegisterMessageCallback');
      Disconnect := GetProcAddress(DLLFilterAPIHandle, 'Disconnect');
      GetFDLastErrorMessage := GetProcAddress(DLLFilterAPIHandle, 'GetLastErrorMessage');
      SetFilterType := GetProcAddress(DLLFilterAPIHandle, 'SetFilterType');
      ResetConfigData := GetProcAddress(DLLFilterAPIHandle, 'ResetConfigData');
      SetBooleanConfig := GetProcAddress(DLLFilterAPIHandle, 'SetBooleanConfig');
      SetConnectionTimeout := GetProcAddress(DLLFilterAPIHandle, 'SetConnectionTimeout');
      GetFileHandleInFilter := GetProcAddress(DLLFilterAPIHandle, 'GetFileHandleInFilter');
      OpenStubFile := GetProcAddress(DLLFilterAPIHandle, 'OpenStubFile');
      CreateStubFile := GetProcAddress(DLLFilterAPIHandle, 'CreateStubFile');
      GetTagData := GetProcAddress(DLLFilterAPIHandle, 'GetTagData');
      RemoveTagData := GetProcAddress(DLLFilterAPIHandle, 'RemoveTagData');
      AddTagData := GetProcAddress(DLLFilterAPIHandle, 'AddTagData');

      AddExcludedProcessId := GetProcAddress(DLLFilterAPIHandle, 'AddExcludedProcessId');
      RemoveExcludeProcessId := GetProcAddress(DLLFilterAPIHandle, 'RemoveExcludeProcessId');
      AddExcludedProcessId := GetProcAddress(DLLFilterAPIHandle, 'AddExcludedProcessId');
      AddIncludedProcessId := GetProcAddress(DLLFilterAPIHandle, 'AddIncludedProcessId');

      AddProtectedProcessId := GetProcAddress(DLLFilterAPIHandle, 'AddProtectedProcessId');
      RemoveProtectedProcessId := GetProcAddress(DLLFilterAPIHandle, 'RemoveProtectedProcessId');

      AddNewFilterRule := GetProcAddress(DLLFilterAPIHandle, 'AddNewFilterRule');
      AddExcludeFileMaskToFilterRule:= GetProcAddress(DLLFilterAPIHandle, 'AddExcludeFileMaskToFilterRule');
      AddHiddenFileMaskToFilterRule:= GetProcAddress(DLLFilterAPIHandle, 'AddHiddenFileMaskToFilterRule');
      AddReparseFileMaskToFilterRule:= GetProcAddress(DLLFilterAPIHandle, 'AddReparseFileMaskToFilterRule');
      AddEncryptionKeyToFilterRule:= GetProcAddress(DLLFilterAPIHandle, 'AddEncryptionKeyToFilterRule');
      AddIncludeProcessNameToFilterRule:= GetProcAddress(DLLFilterAPIHandle, 'AddIncludeProcessNameToFilterRule');
      AddExcludeProcessNameToFilterRule:= GetProcAddress(DLLFilterAPIHandle, 'AddExcludeProcessNameToFilterRule');
      AddIncludeProcessIdToFilterRule:= GetProcAddress(DLLFilterAPIHandle, 'AddIncludeProcessIdToFilterRule');
      AddExcludeProcessIdToFilterRule:= GetProcAddress(DLLFilterAPIHandle, 'AddIncludeProcessIdToFilterRule');
      AddIncludeUserNameToFilterRule:= GetProcAddress(DLLFilterAPIHandle, 'AddIncludeUserNameToFilterRule');
      AddExcludeUserNameToFilterRule:= GetProcAddress(DLLFilterAPIHandle, 'AddExcludeUserNameToFilterRule');
      RegisterEventTypeToFilterRule:= GetProcAddress(DLLFilterAPIHandle, 'RegisterEventTypeToFilterRule');
      RegisterMoinitorIOToFilterRule:= GetProcAddress(DLLFilterAPIHandle, 'RegisterMoinitorIOToFilterRule');
      RegisterControlIOToFilterRule:= GetProcAddress(DLLFilterAPIHandle, 'RegisterControlIOToFilterRule');
      AddProcessRightsToFilterRule:= GetProcAddress(DLLFilterAPIHandle, 'AddProcessRightsToFilterRule');
      AddUserRightsToFilterRule:= GetProcAddress(DLLFilterAPIHandle, 'AddUserRightsToFilterRule');

      AESEncryptFile := GetProcAddress(DLLFilterAPIHandle, 'AESEncryptFile');
      AESDecryptFile := GetProcAddress(DLLFilterAPIHandle, 'AESDecryptFile');
      AESEncryptFileToFile := GetProcAddress(DLLFilterAPIHandle, 'AESEncryptFileToFile');
      AESDecryptFileToFile := GetProcAddress(DLLFilterAPIHandle, 'AESDecryptFileToFile');


    end
  else
    begin
      DLLFilterAPIHandle := 0;
      if Result = 0 then
        Result := ERROR_DLL_INIT_FAILED;
      ErrMsg := format('An error occured while loading %s (%s) (%d)', [DLLName, SysErrorMessage(Result), Result]);
    end;

end;

function GetLastFilterAPIErrorMsg: String;
var
  buffer: array [0 .. 1024] of char;
  // Buffer: PChar;//array [0..1024] of char;
  BufferLength: ULONG;
begin
  BufferLength := 1024;
  GetFDLastErrorMessage(buffer, BufferLength);
  if BufferLength > 2 then // account for CRLF for blank return
    Result := ' - ' + WideString(buffer)
  else
    Result := '';

end;


end.
