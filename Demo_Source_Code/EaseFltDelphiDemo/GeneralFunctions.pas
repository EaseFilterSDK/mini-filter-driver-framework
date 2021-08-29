unit GeneralFunctions;

interface

uses
  Winapi.Windows,
  vcl.dialogs,
  Winapi.Messages,
  System.SysUtils,
  System.Win.Crtl,
  System.Classes,
  System.SyncObjs,
  System.TypInfo,
  FilterAPI,
  GlobalConfig;


function FilterAPIMessageCallback(pSendMessage: PMESSAGE_SEND_DATA; pReplyMessage: PMESSAGE_REPLY_DATA): Bool; stdcall;
procedure FilterAPIDisconnectCallback(); stdcall;
function DriverManagement(Install: Boolean = False): String;
function DisplayFilterMessage(pSendMessage: PMESSAGE_SEND_DATA): Bool;
//function FormatIOName(pSendMessage: PMESSAGE_SEND_DATA): String;
function StartFilterService(MsgCallback: TMessageCallback; GlobalConfig:TGlobalConfig): String;
function StopService(): String;

const
  fnHasNoPrefix = 0;
  fnUNCPrefix = 1;
  fnNonUNCPrefix = 2;
  UNCPrefix = '\\?\UNC';
  NonUNCPrefix = '\\?\';



var
  DLLFilterAPIHandle: NativeInt = 0;
  DriverInstalled: Boolean = False;
  ResultMsgCallback: TMessageCallback;
  ListOfOpenFileIDs: TStringList;
  Lock_OpenFileIDsList: TCriticalSection;
  CloseAppInProgress: Boolean;


implementation

{
function FormatIOName(pSendMessage: PMESSAGE_SEND_DATA): String;
var
    ioType: TEVENT_TYPE;
    messageType: ULONG;
begin
  Result := 'UnKnown IO Type:' + UIntToStr(pSendMessage.MessageType);
  messageType := pSendMessage.MessageType;

  if (messageType = ULONG(FILTER_SEND_FILE_CHANGED_EVENT)) then
  begin
    for ioType := Low(TEVENT_TYPE) to High(TEVENT_TYPE) do
     if(  messageType And ULONG(ioType) > 0) then Result := Result +   Ord(ioType)) + ';';

  end
  else
  begin
  if ((messageType = ULONG(PRE_QUERY_INFORMATION)) and pSendMessage.FsContext = nil )
  then Result := 'PRE_FASTIO_NETWORK_QUERY_OPEN'
  else
  if ((messageType = ULONG(POST_QUERY_INFORMATION)) and pSendMessage.FsContext = nil)
  then Result := 'POST_FASTIO_NETWORK_QUERY_OPEN'
  else Result := GetEnumName(TypeInfo(FilterAPI.MessageType),messageType);
  end;

end;
}

function DisplayFilterMessage(pSendMessage: PMESSAGE_SEND_DATA): Bool;
var
  userName: Array [0 .. 55] of char;
  domainName: Array [0 .. 55] of char;
  userNameSize: ULONG;
  domainNameSize: ULONG;
  snu: SID_NAME_USE;
  ret: Boolean;
  DataString: String;
  FileName: String;
  User: String;
  Domain: String;
  FileInfoStr: String;
  ErrMsg: PChar;
  ErrMsgLen: ULONG;
  ioName: String;

begin
  Result := False;
  if CloseAppInProgress then
    exit;


  Result := False;
  userNameSize := MAX_PATH;
  domainNameSize := MAX_PATH;

  try

    ret := LookupAccountSid(nil, @pSendMessage.Sid, @userName, userNameSize, @domainName, domainNameSize, snu);

    SetLength(User, userNameSize);
    Move(userName[0], User[1], userNameSize * SizeOf(char));

    SetLength(Domain, domainNameSize);
    Move(domainName[0], Domain[1], domainNameSize * SizeOf(char));

    SetLength(DataString, pSendMessage.DataBufferLength div 2);
    Move(pSendMessage.DataBuffer[0], DataString[1], pSendMessage.DataBufferLength);

    SetLength(FileName, pSendMessage.FileNameLength div 2);
    Move(pSendMessage.FileName[0], FileName[1], pSendMessage.FileNameLength);

    //ioName := FormatIOName( pSendMessage);

    FileInfoStr := format('ioType:%x - FILE: %s - DOMAIN: %s - USER: %s' +
                   ' - PROCESSID: %d - THREADID: %d - FileSize: %d' +
                   ' - ATTRIB: %d - ACCESS: %d' +
                   ' - DISP: %d - SHARE: %d' +
                   ' - CREATEOPTIONS: %d - CREATESTATUS: %d' +
                   ' - IOStatus: %x',// +
                   [pSendMessage.MessageType,
                    ExtractFileName(FileName),
                    Domain, User, pSendMessage.ProcessId,
                    pSendMessage.ThreadId, pSendMessage.FileSize,
                    pSendMessage.FileAttributes,
                    pSendMessage.DesiredAccess,
                    pSendMessage.Disposition,
                    pSendMessage.ShareAccess,
                    pSendMessage.CreateOptions,
                    pSendMessage.CreateStatus,
                    pSendMessage.Status]);

    ResultMsgCallback(FileInfoStr);

  except

  end;
end;


function FilterAPIMessageCallback(pSendMessage: PMESSAGE_SEND_DATA; pReplyMessage: PMESSAGE_REPLY_DATA): Bool;
begin
  Result := False;
  if CloseAppInProgress then
    exit;

  DisplayFilterMessage(pSendMessage);

  if( pReplyMessage <> nil) then
  begin

  //for control filter, you can deny the pre-IO access as below.
   //pReplyMessage.ReturnStatus := ULONG(STATUS_ACCESS_DENIED);
   //pReplyMessage.FilterStatus = ULONG(FILTER_COMPLETE_PRE_OPERATION);

   //by default allow the IO access as below:
   pReplyMessage.ReturnStatus := ULONG(STATUS_SUCCESS);

   end;

    Result := True;

end;



procedure FilterAPIDisconnectCallback();
begin
  //ShowMessage('FilterAPIDisconnectCallback');
end;


function StartFilterService(MsgCallback: TMessageCallback; GlobalConfig:TGlobalConfig): String;
var
  ErrMsg: String;
  RetVal: ULONG;
  ret: Boolean;
  PIDValue: ULONG;
  AppName: String;
  FileCount: ULONG;
  AppPID: ULONG;
begin

  ResultMsgCallback := MsgCallback;
  ErrMsg := DriverManagement(TRUE);
  if (Pos('FILTERAPI', UpperCase(ErrMsg)) > 0) then
    begin
      Result := ErrMsg;
      exit;
    end;

  //Purchase a license key with the link: http://www.easefilter.com/Order.htm
  //Email us to request a trial key: info@easefilter.com //free email is not accepted.
  RetVal := SetRegistrationKey('BA110-3DD98-88593-C186E-C3F3F-68096');

  if (not IsDriverServiceRunning) then
    begin
      ret := UnInstallDriver = 1;
      if (not ret) then
				begin
					Result := 'UnInstallDriver Failed' + GetLastFilterAPIErrorMsg;
          //exit;
				end
      else
				Result := 'UnInstall filter driver succeeded!';

      ret := InstallDriver = 1;
      if (not ret) then
				begin
					Result := 'InstallDriver Failed' + GetLastFilterAPIErrorMsg;
          exit;
				end;

				Result := 'Install filter driver succeeded!';
			end;



  if RetVal <> 1 then
    begin
      Result := 'SetRegistrationKey Failed' + GetLastFilterAPIErrorMsg;
      exit;
    end
  else
    begin
      Result := 'SetRegistrationKey - Passed';

      ret := RegisterMessageCallback(GlobalConfig.ThreadCount, @FilterAPIMessageCallback, @FilterAPIDisconnectCallback);
      if not ret then
        Result := 'RegisterMessageCallback Failed' + GetLastFilterAPIErrorMsg;
    end;

   GlobalConfig.SendConfigSettingsToFilter(MsgCallback);


end;



function StopService(): String;
var
  ErrMsg: String;
begin
  Result := '';

  if (DLLFilterAPIHandle > 0) then
    begin
    try
    Disconnect();
    Result := 'Filter Service Stoped!';
    except
    end;

    end;

end;



function DriverManagement(Install: Boolean = False): String;
var
  fName: String;
  ErrMsg: String;
  RetVal: Integer;
begin
  Result := '';

  if (DLLFilterAPIHandle > 0) then
    begin
      if (Not Install) then
        begin
          // -----------------
          if DriverInstalled then
            begin
              if DLLFilterAPIHandle > 0 then
                try
                  FreeLibrary(DLLFilterAPIHandle);
                finally
                  DLLFilterAPIHandle := 0;
                  Result := 'FilterAPI Unloaded';
                end;

              try
                if UnInstallDriver = 0 then
                  Result := 'Uninstalled Driver!';
                DriverInstalled := False;
              except
              end;

            end;
        end;
      exit;
    end;

  fName := 'FilterAPI.dll';
  // -----------------
  RetVal := LoadFilterAPI(fName, DLLFilterAPIHandle, ErrMsg);
  if Length(ErrMsg) > 0 then
    begin
      Result := 'Failed To Load DLL: ' + ErrMsg + ' (' + fName + ')';
      exit;
    end
  else
    begin
      Result := 'Success - Loading DLL (' + fName + ')';
    end;


  if (Not DriverInstalled) and Install then
    begin
      // -----------------
      if InstallDriver < 2 then
        begin
          DriverInstalled := TRUE;
          Result := 'Driver Installed ' + GetLastFilterAPIErrorMsg;
        end
      else
        Result := 'InstallDriver Return Value' + GetLastFilterAPIErrorMsg;
    end
  else if (Not Install) then
    begin
      // -----------------
      // if DriverInstalled then
      begin
        if UnInstallDriver = 0 then
          Result := 'Uninstalled Driver!';
        DriverInstalled := False;
      end;

      if DLLFilterAPIHandle > 0 then
        begin
          FreeLibrary(DLLFilterAPIHandle);
          DLLFilterAPIHandle := 0;
          Result := 'FilterAPI Unloaded';
        end;
    end;

end;

Initialization

  ListOfOpenFileIDs := TStringList.Create;
  Lock_OpenFileIDsList := TCriticalSection.Create;

Finalization
  ListOfOpenFileIDs.Free;
  Lock_OpenFileIDsList.Free;


end.
