unit GlobalConfig;

interface

uses
  Winapi.Windows,
  Winapi.Messages,
  System.SysUtils,
  System.Classes,
  System.SyncObjs,
  System.IniFiles,
  Vcl.Menus,
  Vcl.Graphics,
  Vcl.Controls,
  Vcl.Forms,
  Vcl.Dialogs,
  Vcl.StdCtrls,
  Vcl.ComCtrls,
  Vcl.ExtCtrls,
  Vcl.Samples.Spin,
  FileFunctions,
  FilterAPI;


type
  TGlobalConfig = Class

   private

  INIFileName: String;
  GEncoding: TEnCoding;


  function GetInIString(Ident, Sect, Def, FileName: String): String;
  function WriteInIString(Ident, Sect, Str1, FileName: String): bool;
  function VerifySectionExists(Section, Item, FileName: String): Integer;

    public

    FilterType: LongWord;
    ThreadCount: LongWord;
    Timeout: LongWord;
    AccessFlags:LongWord;
    MonitorIOs: LongWord;
    MonitorFileEvents: LongWord;
    ControlIOs: LongWord;

    IncludeFileFilterMask: String;
    ExcludeFileFilterMask: String;
    EncryptionPasswordPhrase: String;

    procedure LoadINI;
    procedure SaveINI;
    function SendConfigSettingsToFilter(MsgCallback: TMessageCallback): String;

    constructor Create(FileName: String);

  End;

implementation

 constructor TGlobalConfig.Create(FileName: String);
 begin

   INIFileName := FileName;
   FilterType := LongWord(FILE_SYSTEM_EASE_FILTER_ALL);
   ThreadCount := 5;
   Timeout := 30;
   AccessFlags := LongWord(ALLOW_MAX_RIGHT_ACCESS);
   MonitorFileEvents :=  4064;
   MonitorIOs :=  2863311530;
   ControlIOs := 0;
   GEncoding := TEnCoding.Unicode;
   IncludeFileFilterMask := 'c:\\test\\*';
   ExcludeFileFilterMask := '';
   EncryptionPasswordPhrase :='';
 end;


procedure TGlobalConfig.LoadINI;
begin

  if Not(FileExists(INIFileName)) then
    exit;
  FilterType := StrToUInt(GetInIString('AppInfo', 'FilterType', '0', INIFileName));
  ThreadCount := StrToUInt(GetInIString('AppInfo', 'ThreadCount', '5', INIFileName));
  Timeout := StrToUInt(GetInIString('AppInfo', 'Timeout', '30', INIFileName));
  AccessFlags := StrToUInt(GetInIString('AppInfo', 'AccessFlags',UIntToStr(AccessFlags), INIFileName));
  MonitorIOs := StrToUInt(GetInIString('AppInfo', 'MonitorIOs', UIntToStr(MonitorIOs), INIFileName));
  MonitorFileEvents := StrToUInt(GetInIString('AppInfo', 'MonitorFileEvents', UIntToStr(MonitorFileEvents), INIFileName));
  ControlIOs := StrToUInt(GetInIString('AppInfo', 'ControlIOs', UIntToStr(ControlIOs), INIFileName));

  IncludeFileFilterMask := GetInIString('AppInfo', 'IncludeFileFilterMask', IncludeFileFilterMask, INIFileName);
  ExcludeFileFilterMask := GetInIString('AppInfo', 'ExcludeFileFilterMask', ExcludeFileFilterMask, INIFileName);
  EncryptionPasswordPhrase := GetInIString('AppInfo', 'EncryptionPasswordPhrase', EncryptionPasswordPhrase, INIFileName);

end;

procedure TGlobalConfig.SaveINI;
begin

  WriteInIString('AppInfo', 'FilterType', UIntToStr(FilterType), INIFileName);
  WriteInIString('AppInfo', 'ThreadCount', UIntToStr(ThreadCount), INIFileName);
  WriteInIString('AppInfo', 'Timeout', UIntToStr(Timeout), INIFileName);
  WriteInIString('AppInfo', 'AccessFlags', UIntToStr(AccessFlags), INIFileName);
  WriteInIString('AppInfo', 'MonitorIOs', UIntToStr(MonitorIOs), INIFileName);
  WriteInIString('AppInfo', 'MonitorFileEvents', UIntToStr(MonitorFileEvents), INIFileName);
  WriteInIString('AppInfo', 'ControlIOs', UIntToStr(ControlIOs), INIFileName);

  WriteInIString('AppInfo', 'IncludeFileFilterMask', IncludeFileFilterMask, INIFileName);
  WriteInIString('AppInfo', 'ExcludeFileFilterMask', ExcludeFileFilterMask, INIFileName);
  WriteInIString('AppInfo', 'EncryptionPasswordPhrase', EncryptionPasswordPhrase, INIFileName);

end;


function TGlobalConfig.VerifySectionExists(Section, Item, FileName: String): Integer;
var
  Str1: String;
  StrList: TStringList;
begin
  Result := 0;
  if Not(FileExists(FileName)) then
    exit;

  Str1 := GetInIString(Section, Item, 'XXXXX', FileName);
  if (Str1 = 'XXXXX') then
    Result := 2;
  if Result = 2 then
    begin
      StrList := TStringList.Create;
      try
        SetFileAttributesW(PWideChar(FileName), 0);
        StrList.LoadFromFile(FileName);
        StrList.Add('');
        StrList.Add('[' + Section + ']');
        StrList.SaveToFile(FileName, GEncoding);
        Result := 1;
      except
      end;
      StrList.Free;
    end;
end;


/////////////////////////////////////
/// Reg Functions
function TGlobalConfig.GetInIString(Ident, Sect, Def, FileName: String): String;
var
  IniFileName: TMemIniFile;
begin
  IniFileName := nil;
  if (Sect = '') or (Ident = '') then
    begin
      Result := Def;
      exit;
    end;

  try
    IniFileName := TMemIniFile.Create(FileName, GEncoding);
    Result := IniFileName.ReadString(Ident, Sect, Def);
  except
  end;

  if IniFileName <> nil then
    IniFileName.Free; // Read Only No UpdateFile Needed
end;


function TGlobalConfig.WriteInIString(Ident, Sect, Str1, FileName: String): bool;
var
  IniFileName: TMemIniFile;
  FATTR: Integer;
begin
  IniFileName := nil;
  try
    IniFileName := TMemIniFile.Create(FileName, GEncoding);
    FATTR := FileGetAttr(FileName);
    if (FATTR < 0) or ((FATTR and faReadonly) = 0) then
      begin
        IniFileName.WriteString(Ident, Sect, Str1);
      end;
  finally
    if IniFileName <> nil then
      begin
        try
          IniFileName.UpdateFile;
        except
          try
            Sleep(0);
            IniFileName.UpdateFile;
          except
          end;
        end;
        IniFileName.Free;
      end;
    Result := true;
  end;
end;

function TGlobalConfig.SendConfigSettingsToFilter(MsgCallback: TMessageCallback): String;
var
  ErrMsg: String;
  RetVal: ULONG;
  ret: Boolean;
  PIDValue: ULONG;
  AppName: String;
  FileCount: ULONG;
  AppPID: ULONG;
begin

  // Reset the filter config setting.
  RetVal := ResetConfigData();
  if (RetVal <> 1) then
    begin
      MsgCallback('ResetConfigData failed. - ' + GetLastFilterAPIErrorMsg);
      exit;
    end;

    MsgCallback('Reset config data succeeded.');

   RetVal := SetFilterType(FilterType);
  if (RetVal <> 1) then
    begin
      MsgCallback('SetFilterType failed. - ' + GetLastFilterAPIErrorMsg);
      exit;
    end;

   MsgCallback('SetFilterType - ' + UIntToStr(FilterType));

  PIDValue := GetCurrentProcessId;
  RetVal := AddExcludedProcessId(PIDValue);
  if (RetVal <> 1) then
    begin
      MsgCallback('AddExcludedProcessId - ' + UIntToStr(PIDValue) + ' failed.' + GetLastFilterAPIErrorMsg);
      exit;
    end;

  MsgCallback('AddExcludedProcessId - ' + UIntToStr(PIDValue));

  // Set filter maiximum wait for user mode response time out.
  RetVal := SetConnectionTimeout(Timeout);
  if (RetVal <> 1) then
    begin
        MsgCallback('SetConnectionTimeout - ' + UIntToStr(Timeout) + ' failed.' + GetLastFilterAPIErrorMsg);
      exit;
    end;

  MsgCallback('SetConnectionTimeout - ' + UIntToStr(Timeout));

  RetVal := AddNewFilterRule(AccessFlags,PChar(IncludeFileFilterMask),false);
  if (RetVal <> 1) then
    begin
         MsgCallback('AddNewFilterRule - ' + IncludeFileFilterMask + ' failed.' + GetLastFilterAPIErrorMsg);
      exit;
    end;

  MsgCallback('AddNewFilterRule - ' + IncludeFileFilterMask + ' succeeded.');

  RetVal := AddExcludeFileMaskToFilterRule(PChar(IncludeFileFilterMask), PChar(ExcludeFileFilterMask));
  if (RetVal <> 1) then
    begin
         MsgCallback('AddExcludeFileMaskToFilterRule - ' + ExcludeFileFilterMask + ' failed.' + GetLastFilterAPIErrorMsg);
      exit;
    end;

  MsgCallback('AddExcludeFileMaskToFilterRule - ' + ExcludeFileFilterMask + ' succeeded.');

   RetVal := RegisterEventTypeToFilterRule(PChar(IncludeFileFilterMask),MonitorFileEvents);
  if (RetVal <> 1) then
    begin
         MsgCallback('RegisterEventTypeToFilterRule - ' + UIntToStr(MonitorFileEvents) + ' failed.' + GetLastFilterAPIErrorMsg);
      exit;
    end;

  MsgCallback('RegisterEventTypeToFilterRule - ' + UIntToStr(MonitorFileEvents) + ' succeeded.');

   RetVal := RegisterMoinitorIOToFilterRule(PChar(IncludeFileFilterMask),MonitorIOs);
  if (RetVal <> 1) then
    begin
         MsgCallback('RegisterMoinitorIOToFilterRule - ' + UIntToStr(MonitorIOs) + ' failed.' + GetLastFilterAPIErrorMsg);
      exit;
    end;

  MsgCallback('RegisterMoinitorIOToFilterRule - ' + UIntToStr(MonitorIOs) + ' succeeded.');

   RetVal := RegisterControlIOToFilterRule(PChar(IncludeFileFilterMask),ControlIOs);
  if (RetVal <> 1) then
    begin
         MsgCallback('RegisterControlIOToFilterRule - ' + UIntToStr(ControlIOs) + ' failed.' + GetLastFilterAPIErrorMsg);
      exit;
    end;

  MsgCallback('RegisterControlIOToFilterRule - ' + UIntToStr(ControlIOs) + ' succeeded.');

  Result := '';

end;


end.
