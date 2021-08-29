unit FileFunctions;

interface

uses
  Windows,
  SysUtils,
  FilterAPI;

var
  WinMajorMinorVersion: Double = 0;

const
  FIND_FIRST_EX_CASE_SENSITIVE = $00000001;
  FIND_FIRST_EX_LARGE_FETCH = $00000002;

  UnicodeStringBufferLength = 1024;
  UNICODE_STRING_MAX_BYTES = WORD(65534);
  ntdll = 'ntdll.dll';
  //FILE_OPEN_BY_FILE_ID = $00002000;
  FILE_OPEN = $00000001;

  OBJ_CASE_INSENSITIVE = $00000040;

  FILE_DEVICE_FILE_SYSTEM = $00000009;
  METHOD_NEITHER = 3;
  METHOD_BUFFERED = 0;
  FILE_ANY_ACCESS = 0;
  FILE_SPECIAL_ACCESS = 0;
  FILE_READ_ACCESS = 1;
  FILE_WRITE_ACCESS = 2;

  ERROR_JOURNAL_DELETE_IN_PROGRESS = 1178;
  ERROR_JOURNAL_NOT_ACTIVE  = 1179;
  ERROR_JOURNAL_ENTRY_DELETED = 1181;

  FSCTL_GET_OBJECT_ID = $9009c;
  FSCTL_ENUM_USN_DATA = (FILE_DEVICE_FILE_SYSTEM shl 16) or (FILE_ANY_ACCESS shl 14) or (44 shl 2) or METHOD_NEITHER;
  USNREC_MAJVER_OFFSET = 4;
  USNREC_MINVER_OFFSET = 8;
  USNREC_FR_OFFSET = 8;
  USNREC_PFR_OFFSET = 16;
  USNREC_USN_OFFSET = 24;
  USNREC_TIMESTAMP_OFFSET = 32;
  USNREC_REASON_OFFSET = 40;
  USNREC_SINFO_OFFSET = 44;
  USNREC_SECID_OFFSET = 48;
  USNREC_FA_OFFSET = 52;
  USNREC_FNL_OFFSET = 56;
  USNREC_FN_OFFSET = 58;

  PARTITION_IFS = $07;


    // driver const values
  FILE_DIRECTORY_FILE            = $00000001;
  FILE_WRITE_THROUGH             = $00000002;
  FILE_SEQUENTIAL_ONLY           = $00000004;
  FILE_NO_INTERMEDIATE_BUFFERING = $00000008;
  FILE_SYNCHRONOUS_IO_ALERT      = $00000010;
  FILE_SYNCHRONOUS_IO_NONALERT   = $00000020;
  FILE_NON_DIRECTORY_FILE        = $00000040;
  FILE_CREATE_TREE_CONNECTION    = $00000080;
  FILE_COMPLETE_IF_OPLOCKED      = $00000100;
  FILE_NO_EA_KNOWLEDGE           = $00000200;
  FILE_OPEN_REMOTE_INSTANCE      = $00000400;
  FILE_RANDOM_ACCESS             = $00000800;
  FILE_DELETE_ON_CLOSE           = $00001000;
  FILE_OPEN_BY_FILE_ID           = $00002000;
  FILE_OPEN_FOR_BACKUP_INTENT    = $00004000;
  FILE_NO_COMPRESSION            = $00008000;
  FILE_RESERVE_OPFILTER          = $00100000;
  FILE_OPEN_REPARSE_POINT        = $00200000;
  FILE_OPEN_NO_RECALL            = $00400000;
  FILE_OPEN_FOR_FREE_SPACE_QUERY = $00800000;
  FO_REMOTE_ORIGIN               = $01000000;


type
  NTSTATUS = ULONG;
{$EXTERNALSYM NTSTATUS}
  PNTSTATUS = ^NTSTATUS;
{$EXTERNALSYM PNTSTATUS}
  TNTStatus = NTSTATUS;

type
  PIO_STATUS_BLOCK = ^IO_STATUS_BLOCK;

  IO_STATUS_BLOCK = record
    case Integer of
      0:
        (Status: NTSTATUS);
      1:
        (Pointer: Pointer;
          Information: ULONG_PTR);
  end;

type
  FILE_INTERNAL_INFORMATION = record
    IndexNumber: LARGE_INTEGER;
  end;

type
  TUnicodeString = Record
    Length: WORD;
    MaximumLength: WORD;
    Buffer: PChar;
  end;
  PUnicodeString = ^TUnicodeString;
  TUNICODE_STRING = TUnicodeString;
  UNICODE_STRING = TUnicodeString;
  PUNICODE_STRING = PUnicodeString;

type
  PObjectAttributes = ^TObjectAttributes;

  TObjectAttributes = record
    Length: Cardinal;
    RootDirectory: THandle;
    ObjectName: PUnicodeString;
    SecurityDescriptor: PSecurityDescriptor; // Pointer;
    SecurityQualityOfService: PSecurityQualityOfService; // Pointer;
    Attributes: Cardinal;
  end;
  OBJECT_ATTRIBUTES = TObjectAttributes;
  POBJECT_ATTRIBUTES = ^OBJECT_ATTRIBUTES;

type
  _NTFSFileInfo = record
    FileReferenceNumber: Int64;
    ParentFileReferenceNumber: Int64;
    USN: UInt64;
    Cancel: Boolean;
    CreationTime: TFileTime;
    LastAccessTime: TFileTime;
    LastWriteTime: TFileTime;
    FileAttributes: ULONG;
    FileSize: LARGE_INTEGER;
    FileName: String; // Array [0..MAX_PATH] of WCHAR;
    FilePath: String; // Array [0..MAX_PATH] of WCHAR;
  end;
  NTFSFileInfo = _NTFSFileInfo;
  PNTFSFileInfo = ^_NTFSFileInfo;
  TNTFSFileInfo = _NTFSFileInfo;


type
  _NTFSFileDetailInfo = record
    FileReferenceNumber: Int64;
    ParentFileReferenceNumber: Int64;
    USN: UInt64;
    USNTime: TFileTime;
    CreationTime: TFileTime;
    LastAccessTime: TFileTime;
    LastWriteTime: TFileTime;
    FileAttributes: ULONG;
    FileSize: Int64; // LARGE_INTEGER;
    FileName: Array [0 .. UnicodeStringBufferLength] of CHAR;
    FilePath: Array [0 .. UnicodeStringBufferLength] of CHAR;
  end;
  NTFSFileDetailInfo = _NTFSFileDetailInfo;
  PNTFSFileDetailInfo = ^_NTFSFileDetailInfo;
  TNTFSFileDetailInfo = _NTFSFileDetailInfo;





TFileEnumCallback = function(fInfo: TNTFSFileInfo): Boolean of object;


function EnumFileEntries(fPath, fMask: String; RecurseFolders: Boolean; FileEnumCallBack: TFileEnumCallback): Boolean;

function AddFileNamePrefix(var FName: String; PrefixType: Integer = 0): Boolean;
function OpenLongFileName(const ALongFileName: String; SharingMode: DWORD): THandle; overload;
function OpenLongFileName(const ALongFileName: WideString; SharingMode: DWORD): THandle;  overload;
function CreateLongFileName(const ALongFileName: String; SharingMode: DWORD): THandle; overload;
function CreateLongFileName(const ALongFileName: WideString; SharingMode: DWORD): THandle; overload;
procedure CustomFileCopy(const ASourceFileName, ADestinationFileName: String);
procedure CustomFileCopyWithHandles(const ASourceFile, ADestinationFile: THandle);
function CustomReadFile(const ASourceFile: THandle; FileSize, BufferSize: Int64; var StartByte: Int64; var Buffer: Array of Byte): DWORD;
procedure CopyFileADSInfo(AFileNameSrc, AFileNameTar: String); // ; var RetBuf: Array of PByteArray);

function GetWindowsMajorMinorVersionNumber: Double;
function GetPWinSysDir: String;

function DeleteFolder(DirName: PChar; Recurse, BreakOnFailure: Boolean): Boolean;

const
  BUFFER_SIZE = sizeof(Int64) + $10000;

  fnHasNoPrefix = 0;
  fnUNCPrefix = 1;
  fnNonUNCPrefix = 2;
  UNCPrefix = '\\?\UNC';
  NonUNCPrefix = '\\?\';



implementation

//uses
//  GeneralBaseFunctions;


function EnumFileEntries(fPath, fMask: String; RecurseFolders: Boolean; FileEnumCallBack: TFileEnumCallback): Boolean;
var
  h: THandle;
  tStr: String;//wfa : TWin32FindData;
  wfa: ^WIN32_FIND_DATA;
  show: Boolean;
  dwAdditionalFlags: DWORD;
  indexInfoLevels: _FINDEX_INFO_LEVELS;
  indexSearchOps: _FINDEX_SEARCH_OPS;
  fInfo: NTFSFileInfo;
  FullPath: String;
begin
  New(wfa);
  dwAdditionalFlags := 0;
  indexInfoLevels := FindExInfoStandard;  //(FindExInfoStandard, FindExInfoBasic, FindExInfoMaxInfoLevel);
  indexSearchOps := FindExSearchNameMatch; //(FindExSearchNameMatch, FindExSearchLimitToDirectories, FindExSearchLimitToDevices)
  Result := False;
  if Length(fMask) = 0 then
    fMask := '*';

  if WinMajorMinorVersion < 1 then
    WinMajorMinorVersion := GetWindowsMajorMinorVersionNumber;

  if WinMajorMinorVersion > 6.0 then  //6.0 = Windows 2008 NON R2 - does not always support FindFirstFileEx
    h := THandle(Windows.FindFirstFileEx(PChar(IncludeTrailingPathDelimiter(fPath) + fMask), indexInfoLevels, wfa, indexSearchOps, nil, dwAdditionalFlags))
  else
    h := THandle(Windows.FindFirstFile(PChar(IncludeTrailingPathDelimiter(fPath) + fMask), wfa^));

  if h <> INVALID_HANDLE_VALUE then
    begin
      repeat
        if (WideString(wfa.cFileName) <> '.') and
           (WideString(wfa.cFileName) <> '..') then
          begin
            if ((wfa.dwFileAttributes and FILE_ATTRIBUTE_DIRECTORY) > 0) then
              begin
                if (Not RecurseFolders) or
                   ((wfa.dwFileAttributes and FILE_ATTRIBUTE_REPARSE_POINT) > 0) then
                  begin
                    //Windows.FindNextFile(h, wfa^);
                    tStr := '';
                    continue;
                  end
                else if  (Not EnumFileEntries(fPath + IncludeTrailingPathDelimiter(wfa.cFileName), fMask, RecurseFolders, FileEnumCallBack)) then
                  begin
                    tStr := '';
                    break;
                  end;
              end;


            fInfo.FileReferenceNumber := 0;
            fInfo.ParentFileReferenceNumber := 0;
            fInfo.FileName := wfa.cFileName;
            fInfo.FilePath := fPath;//ExtractFilePath(fPath);
            fInfo.FileSize.LowPart := wfa.nFileSizeLow;
            fInfo.FileSize.HighPart := wfa.nFileSizeHigh;
            fInfo.FileAttributes := wfa.dwFileAttributes;
            fInfo.CreationTime := wfa.ftCreationTime;
            fInfo.LastAccessTime := wfa.ftLastAccessTime;
            fInfo.LastWriteTime := wfa.ftLastWriteTime;
            if not FileEnumCallBack(fInfo) then
              break;
          end;
      until not Windows.FindNextFile(h, wfa^);
    end;
  Windows.FindClose(h);
  Result := True;
end;

function AddFileNamePrefix(var FName: String; PrefixType: Integer = 0): Boolean;
begin       // fnUNCPrefix = 1; fnNonUNCPrefix = 2
  Result := False;
  if Length(FName) < 240 then
    exit;

  if PrefixType = fnHasNoPrefix then
    begin
      if (Length(FName) > 1) and
         (FName[2] = '\') then
        PrefixType := 1
      else
        PrefixType := 2;
    end;


  if PrefixType = fnUNCPrefix then
    begin
      FName := UNCPrefix + Copy(FName, 2, Length(FName)); //remove one of the leading '\'
      Result := True;
    end
  else if PrefixType = fnNonUNCPrefix then
    begin
      FName := NonUNCPrefix + FName;
      Result := True;
    end;
end;

function OpenLongFileName(const ALongFileName: String; SharingMode: DWORD): THandle; overload;
var
  fName: String;
begin
  fName := ALongFileName;
  if (Length(fName) > 1) and
     (fName[2] = '\') then
    AddFileNamePrefix(fName, fnUNCPrefix)
  else
    AddFileNamePrefix(fName, fnNonUNCPrefix);

  Result := CreateFile(PChar(fName), GENERIC_READ, SharingMode,
          nil, OPEN_EXISTING,
          FILE_ATTRIBUTE_NORMAL +
          FILE_FLAG_RANDOM_ACCESS +
          SECURITY_IMPERSONATION, 0);
end;

function OpenLongFileName(const ALongFileName: WideString; SharingMode: DWORD): THandle;  overload;
begin
  if CompareMem(@(WideCharToString(PWideChar(ALongFileName))[1]), @('\\'[1]), 2) then
    { Allready an UNC path }
    Result := CreateFileW(PWideChar(ALongFileName), GENERIC_READ, SharingMode, nil, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, 0)
  else
    Result := CreateFileW(PWideChar('\\?\' + ALongFileName), GENERIC_READ, SharingMode, nil, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, 0);
end;

function CreateLongFileName(const ALongFileName: String; SharingMode: DWORD): THandle; overload;
begin
  if CompareMem(@(ALongFileName[1]), @('\\'[1]), 2) then
    { Allready an UNC path }
    Result := CreateFileW(PWideChar(WideString(ALongFileName)), GENERIC_WRITE, SharingMode, nil, CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, 0)
  else
    Result := CreateFileW(PWideChar(WideString('\\?\' + ALongFileName)), GENERIC_WRITE, SharingMode, nil, CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, 0);
end;

function CreateLongFileName(const ALongFileName: WideString; SharingMode: DWORD): THandle; overload;
begin
  if CompareMem(@(WideCharToString(PWideChar(ALongFileName))[1]), @('\\'[1]), 2) then
    { Allready an UNC path }
    Result := CreateFileW(PWideChar(ALongFileName), GENERIC_WRITE, SharingMode, nil, CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, 0)
  else
    Result := CreateFileW(PWideChar('\\?\' + ALongFileName), GENERIC_WRITE, SharingMode, nil, CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, 0);
end;


procedure CustomFileCopy(const ASourceFileName, ADestinationFileName: String);
var
  ASourceFile, ADestinationFile: THandle;
begin
  ASourceFile := OpenLongFileName(ASourceFileName, 0);
  if ASourceFile <> 0 then
  try
    try
      ADestinationFile :=  CreateLongFileName(ADestinationFileName, FILE_SHARE_READ);
      CustomFileCopyWithHandles(ASourceFile, ADestinationFile);
    finally
      CloseHandle(ADestinationFile);
    end;
  finally
    CloseHandle(ASourceFile);
  end;

  CopyFileADSInfo(ASourceFileName, ADestinationFileName);

end;

procedure CustomFileCopyWithHandles(const ASourceFile, ADestinationFile: THandle);
const
  BufferSize = 1024 * 16; // 4KB blocks, change this to tune your speed
var
  // Buffer : array of Byte;
  Buffer: array [0 .. BufferSize - 1] of Char;
  FileSize: Int64;
  BytesRead, byteswritten, BytesWritten2: DWORD;
begin
  // SetLength(Buffer, BufferSize);
  if ASourceFile <> 0 then
    try
      FileSize := FileSeek(ASourceFile, 0, FILE_END);
      FileSeek(ASourceFile, 0, FILE_BEGIN);
      if ADestinationFile <> 0 then
        try
          while (FileSize - FileSeek(ASourceFile, 0, FILE_CURRENT)) >= BufferSize do
            begin
              if (not Readfile(ASourceFile, Buffer[0], BufferSize, BytesRead, nil)) and (BytesRead = 0) then
                Continue;
              WriteFile(ADestinationFile, Buffer[0], BytesRead, byteswritten, nil);
              if byteswritten < BytesRead then
                begin
                  WriteFile(ADestinationFile, Buffer[byteswritten], BytesRead - byteswritten, BytesWritten2, nil);
                  if (BytesWritten2 + byteswritten) < BytesRead then
                    RaiseLastOSError;
                end;
            end;
          if FileSeek(ASourceFile, 0, FILE_CURRENT) < FileSize then
            begin
              if (not Readfile(ASourceFile, Buffer[0], FileSize - FileSeek(ASourceFile, 0, FILE_CURRENT), BytesRead, nil)) and (BytesRead = 0) then
                Readfile(ASourceFile, Buffer[0], FileSize - FileSeek(ASourceFile, 0, FILE_CURRENT), BytesRead, nil);
              WriteFile(ADestinationFile, Buffer[0], BytesRead, byteswritten, nil);
              if byteswritten < BytesRead then
                begin
                  WriteFile(ADestinationFile, Buffer[byteswritten], BytesRead - byteswritten, BytesWritten2, nil);
                  if (BytesWritten2 + byteswritten) < BytesRead then
                    RaiseLastOSError;
                end;
            end;

          //PreSetFileSize(ADestinationFile, FileSize);
        finally
        end;
    finally

    end;

end;

function CustomReadFile(const ASourceFile: THandle; FileSize, BufferSize: Int64; var StartByte: Int64; var Buffer: Array of Byte): DWORD;
var
  BytesRead: DWORD;
  tInt: Integer;
  tInt1: Integer;
begin
  BytesRead := 0;
  Result := 0;
  if ASourceFile <> 0 then
    begin
      tInt := FileSeek(ASourceFile, 0, FILE_CURRENT);
//      tInt1 := FileSeek(ASourceFile, StartByte, FILE_BEGIN);
      if (FileSize - tInt) >= BufferSize then
        begin
          Readfile(ASourceFile, Buffer[0], BufferSize, BytesRead, nil);
        end
      else if tInt < FileSize then
        begin
          if (not Readfile(ASourceFile, Buffer[0], FileSize - tInt, BytesRead, nil)) and (BytesRead = 0) then
            Readfile(ASourceFile, Buffer[0], FileSize - tInt, BytesRead, nil);
        end;
    end;

  Result := BytesRead;

end;

function CopyFileStream(FileSrc, FileTar: String): Integer;
const
  FILE_ATTRIBUTE_NOT_CONTENT_INDEXED = 8192;
  ALLOWED_ATTRIBUTES = (FILE_ATTRIBUTE_ARCHIVE or FILE_ATTRIBUTE_HIDDEN or FILE_ATTRIBUTE_READONLY or FILE_ATTRIBUTE_SYSTEM or
    FILE_ATTRIBUTE_NOT_CONTENT_INDEXED);

var
  byHandleInfo: BY_HANDLE_FILE_INFORMATION;
  hInFile, hOutFile: THandle;
  buf: Array [0 .. ((64 * 1024) - 1)] of Byte;
  dwBytesRead, dwBytesWritten: DWORD;
  iRetCode: Integer;
begin
  Result := 0;
  try
    hInFile := CreateFile(PChar(FileSrc), GENERIC_READ, FILE_SHARE_READ, nil, OPEN_EXISTING, FILE_FLAG_SEQUENTIAL_SCAN, 0);
    if (hInFile = INVALID_HANDLE_VALUE) then
      begin
        Result := GetLastError();
        exit;
      end;

    if (Not GetFileInformationByHandle(hInFile, byHandleInfo)) then
      begin
        CloseHandle(hInFile);
        Result := GetLastError();
        exit;
      end;

    hOutFile := CreateFile(PChar(FileTar), GENERIC_WRITE, FILE_SHARE_READ, nil, CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL or FILE_FLAG_SEQUENTIAL_SCAN, 0);
    if (hOutFile = INVALID_HANDLE_VALUE) then
      begin
        CloseHandle(hInFile);
        Result := GetLastError();
        exit;
      end;

    while True do
      begin
        if (Not ReadFile(hInFile, buf, sizeof(buf), dwBytesRead, nil)) then
          begin
            CloseHandle(hInFile);
            CloseHandle(hOutFile);
            Result := GetLastError();
            exit;
          end;

        if (dwBytesRead > 0) then
          begin
            if (Not WriteFile(hOutFile, buf, dwBytesRead, dwBytesWritten, nil)) then
              begin
                CloseHandle(hInFile);
                CloseHandle(hOutFile);
                Result := GetLastError();
                exit;
              end;

            if (dwBytesWritten < dwBytesRead) then
              begin
                CloseHandle(hInFile);
                CloseHandle(hOutFile);
                Result := ERROR_HANDLE_DISK_FULL; // GetLastError();
                exit;
              end;

          end;

        if (dwBytesRead <> sizeof(buf)) then
          break;
      end; // while loop

    CloseHandle(hInFile);

    // Set output file attributes
    if (not SetFileTime(hOutFile, @byHandleInfo.ftCreationTime, @byHandleInfo.ftLastAccessTime, @byHandleInfo.ftLastWriteTime)) then
      begin
        CloseHandle(hOutFile);
        Result := GetLastError();
        exit;
      end;

    CloseHandle(hOutFile);
    if (not SetFileAttributesW(PChar(FileTar), byHandleInfo.dwFileAttributes and ALLOWED_ATTRIBUTES)) then
      begin
        Result := GetLastError();
        exit;
      end;
  except
  end;
end;


procedure CopyFileADSInfo(AFileNameSrc, AFileNameTar: String); // ; var RetBuf: Array of PByteArray);
// ===============================================================
// Load a string list with ADS (Alternate Data Stream) names
// the integer(AList.Object[?]) contains the File Size
// ===============================================================
var
  hHandleSrc, hHandleTar: THandle;
  sName: String;
  iNumRead, iLo, iHi: DWORD;
  pCtx: Pointer;
  pBuffer, pBytePtr: PByte;
  pWsi: PWin32StreamID absolute pBuffer;
  wszStreamName: array [0 .. MAX_PATH] of WideChar;
  N: Integer;
begin

  pCtx := nil;
  hHandleSrc := CreateFile(PChar(AFileNameSrc), GENERIC_READ, 0, nil, OPEN_EXISTING, FILE_FLAG_BACKUP_SEMANTICS, 0);
  if (hHandleSrc = INVALID_HANDLE_VALUE) then
    begin
      exit;
    end;

  { hHandleTar := CreateFile(PChar(AFileNameTar), GENERIC_READ or GENERIC_WRITE,
    FILE_SHARE_READ or FILE_SHARE_WRITE or FILE_SHARE_DELETE,
    nil, OPEN_EXISTING, 0, 0);

    if (hHandleTar = INVALID_HANDLE_VALUE) then
    begin
    CloseHandle(hHandleSrc);
    exit;
    end;
  }
  GetMem(pBuffer, 4096);

  while True do
    begin
      // We are at the start of a stream header. read it.
      pBytePtr := pBuffer;
      if BackupRead(hHandleSrc, pBytePtr, 20, iNumRead, FALSE, True, pCtx) then
        begin
          if iNumRead = 0 then
            break
          else
            begin // Can we get a stream name ?
              sName := '';

              if pWsi.dwStreamNameSize > 0 then
                begin
                  FillChar(wszStreamName, sizeof(wszStreamName), 0);
                  BackupRead(hHandleSrc, PByte(wszStreamName[0]), pWsi.dwStreamNameSize, iNumRead, FALSE, True, pCtx);

                  sName := wszStreamName;
                  if Length(sName) > 0 then
                    begin
                      if iNumRead <> pWsi.dwStreamNameSize then
                        break
                      else
                        begin // we have a name
                          N := Pos(':', Copy(sName, 2, MaxInt));
                          if N > 1 then
                            sName := Copy(sName, 1, N);
                          CopyFileStream(AFileNameSrc + sName, AFileNameTar + sName);
                        end;
                    end
                  else
                    break;
                end;

              // Skip to start of next stream data
              if pWsi.Size > 0 then
                BackupSeek(hHandleSrc, high(DWORD), high(DWORD), iLo, iHi, Pointer(pCtx));
            end;
        end
      else
        break;
    end;

  // Release the context
  BackupRead(hHandleSrc, nil, 0, iNumRead, True, FALSE, pCtx);

  CloseHandle(hHandleSrc);

  FreeMem(pBuffer);
end;

function GetWindowsMajorMinorVersionNumber: Double;
var
  osInfo: TOsVersionInfo;
  CSDVer: String;
begin
  Result := 0;
  osInfo.dwOsVersionInfoSize := sizeof(osInfo);
  GetVersionEx(osInfo);
  Result := StrToFloat(IntToStr(osInfo.dwMajorVersion) + FormatSettings.DecimalSeparator + IntToStr(osInfo.dwMinorVersion));
end;

function GetPWinSysDir: String;
var
  PWinSysDirArray: Array [0 .. 511] of Char;
begin
  try
    GetSystemDirectory(PWinSysDirArray, 255);
    Result := IncludeTrailingPathDelimiter(WideString(PWinSysDirArray));
  except
    Result := '';
  end;
end;


function DeleteFolder(DirName: PChar; Recurse, BreakOnFailure: Boolean): Boolean;
var
  DosError: Integer;
  MySearchRec: TSearchRec;
  WFileName, DirNameWS: String;
  FileAttr: DWord;
  PathFilter: String;
label Jump;
begin
  Result := False;
  try
    DirNameWS := DirName;
    DirName := PChar(IncludeTrailingPathDelimiter(DirNameWS));
    PathFilter := DirName + '*';
    DosError := FindFirst(PathFilter, faAnyFile, MySearchRec);

    if (DosError = 0) then
      begin
        repeat
          WFileName := MySearchRec.FindData.cFileName;
          FileAttr := MySearchRec.Attr;

          if ((FileAttr and faDirectory) > 0) and ((WFileName = '.') or (WFileName = '..')) then
            goto Jump;

          if ((FileAttr and faDirectory) > 0) then
            begin
              if (Recurse) then
                Result := DeleteFolder(DirName, Recurse, BreakOnFailure);
            end
          else
            Result := DeleteFile(IncludeTrailingPathDelimiter(DirName) + WFileName);

          if (Not Result) and (BreakOnFailure) then
            break;
        Jump:
          DosError := FindNext(MySearchRec);

        until (DosError <> 0);
      end;
  finally
    try
      FindClose(MySearchRec);
    except

    end;

  end;
end;

end.
