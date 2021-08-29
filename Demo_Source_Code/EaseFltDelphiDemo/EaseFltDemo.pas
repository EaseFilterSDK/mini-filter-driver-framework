unit EaseFltDemo;

interface

uses
  Winapi.Windows, Winapi.Messages, System.SysUtils, System.Variants, System.Classes, Vcl.Graphics,
  Vcl.Controls, Vcl.Forms, Vcl.Dialogs, Vcl.StdCtrls, Vcl.Samples.Spin,
  Vcl.ComCtrls, Vcl.CheckLst,
  System.SyncObjs,
  FilterAPI,
  GlobalConfig;


type
  TDisplayThread = class(TThread)
    private
      UseForm: TObject;
      procedure SendToDisplay(UseFormIn: TObject);
    protected
      procedure Execute; override;
    public
      fCompletedEvent: THandle; //used to signal completion
      PendingDisplayItems: TStringList;
      constructor Create(UseFormIn: TObject);
      destructor Destroy; override;
      procedure AddDisplayInfo(DisplayStr: String); //; UseFormIn: TObject);
      procedure ClearLists;
      procedure SendToDisplaySync;
  end;

type
  TForm1 = class(TForm)
    PageControl1: TPageControl;
    TabSheet2: TTabSheet;
    Label1: TLabel;
    Edit_IncludeFileFilterMask: TEdit;
    TabSheet1: TTabSheet;
    Label2: TLabel;
    CheckListBox_FileEvents: TCheckListBox;
    Memo1: TMemo;
    Button_Start: TButton;
    Button_Stop: TButton;
    Button_SaveSettings: TButton;
    Label3: TLabel;
    Edit_ExcludeFileFilterMask: TEdit;
    Label4: TLabel;
    CheckListBox_AccessFlags: TCheckListBox;
    Label5: TLabel;
    CheckListBox_MonitorIO: TCheckListBox;
    Label6: TLabel;
    CheckListBox_ControlIO: TCheckListBox;
    Label7: TLabel;
    Edit_PasswordPhrase: TEdit;
    procedure FormCreate(Sender: TObject);
    procedure Button_SaveSettingsClick(Sender: TObject);
    procedure FormClose(Sender: TObject; var Action: TCloseAction);
    function AddMessage(MsgStrIn: String): Boolean;
    procedure Button_StartClick(Sender: TObject);
    procedure Button_StopClick(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;
  GlobalConfig: TGlobalConfig;
  INIFileName: String;
  LDisplayThread: TDisplayThread;
  Lock_DisplayListing: TCriticalSection;
  CloseAppInProgress: BOOL;

implementation

uses
GeneralFunctions;

{$R *.dfm}

procedure TForm1.Button_StartClick(Sender: TObject);
begin

AddMessage(StartFilterService(AddMessage,GlobalConfig));

end;



procedure TForm1.Button_StopClick(Sender: TObject);
begin
CloseAppInProgress := True;
StopService();

end;

function TForm1.AddMessage(MsgStrIn: String): Boolean;
begin

  LDisplayThread.AddDisplayInfo(MsgStrIn);

end;

procedure TForm1.FormClose(Sender: TObject; var Action: TCloseAction);
begin
CloseAppInProgress := True;
end;

procedure TForm1.FormCreate(Sender: TObject);
begin
  LDisplayThread := TDisplayThread.Create(Self);

  INIFileName := ChangeFileExt(Application.ExeName, '.ini');
  Lock_DisplayListing := TCriticalSection.Create;

  Memo1.Clear;

  GlobalConfig := TGlobalConfig.Create(INIFileName);
  GlobalConfig.LoadINI;

  Edit_IncludeFileFilterMask.Text :=  GlobalConfig.IncludeFileFilterMask;
  Edit_ExcludeFileFilterMask.Text := GlobalConfig.ExcludeFileFilterMask;
  Edit_PasswordPhrase.Text := GlobalConfig.EncryptionPasswordPhrase;

  if(GlobalConfig.AccessFlags And LongWord(FILE_ENCRYPTION_RULE) > 0)
  then CheckListBox_AccessFlags.Checked[0] := True;
  if(GlobalConfig.AccessFlags And LongWord(ALLOW_READ_ACCESS) > 0)
  then CheckListBox_AccessFlags.Checked[1] := True;
  if (GlobalConfig.AccessFlags And LongWord(ALLOW_WRITE_ACCESS) > 0)
  then CheckListBox_AccessFlags.Checked[2] := True;
  if(GlobalConfig.AccessFlags And LongWord(ALLOW_FILE_DELETE) > 0)
  then CheckListBox_AccessFlags.Checked[3] := True;
  if( GlobalConfig.AccessFlags And  LongWord(ALLOW_FILE_RENAME) > 0)
  then CheckListBox_AccessFlags.Checked[4] := True;
  if(GlobalConfig.AccessFlags  And LongWord(ALLOW_OPEN_WITH_CREATE_OR_OVERWRITE_ACCESS) >0)
  then CheckListBox_AccessFlags.Checked[5] := True;
  if(GlobalConfig.AccessFlags  And LongWord(ALLOW_FILE_MEMORY_MAPPED) >0)
  then CheckListBox_AccessFlags.Checked[6] := True;
  if(GlobalConfig.AccessFlags  And LongWord(ALLOW_SET_SECURITY_ACCESS) >0)
  then CheckListBox_AccessFlags.Checked[7] := True;
  if(GlobalConfig.AccessFlags  And LongWord(ALLOW_FILE_ACCESS_FROM_NETWORK) >0)
  then CheckListBox_AccessFlags.Checked[8] := True;

  if( GlobalConfig.MonitorFileEvents and LongWord(FILE_CREATEED) > 0)
  then  CheckListBox_FileEvents.Checked[0] := True;
  if( GlobalConfig.MonitorFileEvents and LongWord(FILE_WRITTEN) > 0)
  then  CheckListBox_FileEvents.Checked[1] := True;
  if( GlobalConfig.MonitorFileEvents and LongWord(FILE_RENAMED) > 0)
  then  CheckListBox_FileEvents.Checked[2] := True;
  if( GlobalConfig.MonitorFileEvents and LongWord(FILE_DELETED) > 0)
  then  CheckListBox_FileEvents.Checked[3] := True;
  if( GlobalConfig.MonitorFileEvents and LongWord(FILE_SECURITY_CHANGED) > 0)
  then  CheckListBox_FileEvents.Checked[4] := True;
  if( GlobalConfig.MonitorFileEvents and LongWord(FILE_INFO_CHANGED) > 0)
  then  CheckListBox_FileEvents.Checked[5] := True;
  if( GlobalConfig.MonitorFileEvents and LongWord(FILE_READ) > 0)
  then  CheckListBox_FileEvents.Checked[6] := True;

  if( GlobalConfig.MonitorIOs and LongWord(POST_CREATE) > 0)
  then CheckListBox_MonitorIO.Checked[0] := True;
  if( GlobalConfig.MonitorIOs and LongWord(POST_FASTIO_READ) > 0)
  then CheckListBox_MonitorIO.Checked[1] := True;
  if( GlobalConfig.MonitorIOs and LongWord(POST_CACHE_READ) > 0)
  then CheckListBox_MonitorIO.Checked[2] := True;
  if( GlobalConfig.MonitorIOs and LongWord(POST_NOCACHE_READ) > 0)
  then CheckListBox_MonitorIO.Checked[3] := True;
  if( GlobalConfig.MonitorIOs and LongWord(POST_PAGING_IO_READ) > 0)
  then CheckListBox_MonitorIO.Checked[4] := True;
  if( GlobalConfig.MonitorIOs and LongWord(POST_FASTIO_WRITE) > 0)
  then CheckListBox_MonitorIO.Checked[5] := True;
   if( GlobalConfig.MonitorIOs and LongWord(POST_CACHE_WRITE) > 0)
  then CheckListBox_MonitorIO.Checked[6] := True;
  if( GlobalConfig.MonitorIOs and LongWord(POST_NOCACHE_WRITE) > 0)
  then CheckListBox_MonitorIO.Checked[7] := True;
  if( GlobalConfig.MonitorIOs and LongWord(POST_PAGING_IO_WRITE) > 0)
  then CheckListBox_MonitorIO.Checked[8] := True;
  if( GlobalConfig.MonitorIOs and LongWord(POST_QUERY_INFORMATION) > 0)
  then CheckListBox_MonitorIO.Checked[9] := True;
  if( GlobalConfig.MonitorIOs and LongWord(POST_SET_INFORMATION) > 0)
  then CheckListBox_MonitorIO.Checked[10] := True;
   if( GlobalConfig.MonitorIOs and LongWord(POST_DIRECTORY) > 0)
  then CheckListBox_MonitorIO.Checked[11] := True;
  if( GlobalConfig.MonitorIOs and LongWord(POST_QUERY_SECURITY) > 0)
  then CheckListBox_MonitorIO.Checked[12] := True;
  if( GlobalConfig.MonitorIOs and LongWord(POST_SET_SECURITY) > 0)
  then CheckListBox_MonitorIO.Checked[13] := True;
  if( GlobalConfig.MonitorIOs and LongWord(POST_CLEANUP) > 0)
  then CheckListBox_MonitorIO.Checked[14] := True;
  if( GlobalConfig.MonitorIOs and LongWord(POST_CLOSE) > 0)
  then CheckListBox_MonitorIO.Checked[15] := True;


  if( GlobalConfig.ControlIOs and LongWord(PRE_CREATE) > 0)
  then CheckListBox_ControlIO.Checked[0] := True;
  if( GlobalConfig.ControlIOs and LongWord(POST_CREATE) > 0)
  then CheckListBox_ControlIO.Checked[1] := True;
  if( GlobalConfig.ControlIOs and LongWord(PRE_FASTIO_READ) > 0)
  then CheckListBox_ControlIO.Checked[2] := True;
  if( GlobalConfig.ControlIOs and LongWord(POST_FASTIO_READ) > 0)
  then CheckListBox_ControlIO.Checked[3] := True;
  if( GlobalConfig.ControlIOs and LongWord(PRE_CACHE_READ) > 0)
  then CheckListBox_ControlIO.Checked[4] := True;
   if( GlobalConfig.ControlIOs and LongWord(POST_CACHE_READ) > 0)
  then CheckListBox_ControlIO.Checked[5] := True;
  if( GlobalConfig.ControlIOs and LongWord(PRE_NOCACHE_READ) > 0)
  then CheckListBox_ControlIO.Checked[6] := True;
  if( GlobalConfig.ControlIOs and LongWord(POST_NOCACHE_READ) > 0)
  then CheckListBox_ControlIO.Checked[7] := True;
  if( GlobalConfig.ControlIOs and LongWord(PRE_PAGING_IO_READ) > 0)
  then CheckListBox_ControlIO.Checked[8] := True;
  if( GlobalConfig.ControlIOs and LongWord(POST_PAGING_IO_READ) > 0)
  then CheckListBox_ControlIO.Checked[9] := True;
 if( GlobalConfig.ControlIOs and LongWord(PRE_FASTIO_WRITE) > 0)
  then CheckListBox_ControlIO.Checked[10] := True;
  if( GlobalConfig.ControlIOs and LongWord(POST_FASTIO_WRITE) > 0)
  then CheckListBox_ControlIO.Checked[11] := True;
  if( GlobalConfig.ControlIOs and LongWord(PRE_CACHE_WRITE) > 0)
  then CheckListBox_ControlIO.Checked[12] := True;
  if( GlobalConfig.ControlIOs and LongWord(POST_CACHE_WRITE) > 0)
  then CheckListBox_ControlIO.Checked[13] := True;
  if( GlobalConfig.ControlIOs and LongWord(PRE_NOCACHE_WRITE) > 0)
  then CheckListBox_ControlIO.Checked[14] := True;
   if( GlobalConfig.ControlIOs and LongWord(POST_NOCACHE_WRITE) > 0)
  then CheckListBox_ControlIO.Checked[15] := True;
  if( GlobalConfig.ControlIOs and LongWord(PRE_PAGING_IO_WRITE) > 0)
  then CheckListBox_ControlIO.Checked[16] := True;
  if( GlobalConfig.ControlIOs and LongWord(POST_PAGING_IO_WRITE) > 0)
  then CheckListBox_ControlIO.Checked[17] := True;
  if( GlobalConfig.ControlIOs and LongWord(PRE_QUERY_INFORMATION) > 0)
  then CheckListBox_ControlIO.Checked[18] := True;
  if( GlobalConfig.ControlIOs and LongWord(POST_QUERY_INFORMATION) > 0)
  then CheckListBox_ControlIO.Checked[19] := True;
  if( GlobalConfig.ControlIOs and LongWord(PRE_SET_INFORMATION) > 0)
  then CheckListBox_ControlIO.Checked[20] := True;
  if( GlobalConfig.ControlIOs and LongWord(POST_SET_INFORMATION) > 0)
  then CheckListBox_ControlIO.Checked[21] := True;
  if( GlobalConfig.ControlIOs and LongWord(PRE_DIRECTORY) > 0)
  then CheckListBox_ControlIO.Checked[22] := True;
  if( GlobalConfig.ControlIOs and LongWord(POST_DIRECTORY) > 0)
  then CheckListBox_ControlIO.Checked[23] := True;
  if( GlobalConfig.ControlIOs and LongWord(PRE_QUERY_SECURITY) > 0)
  then CheckListBox_ControlIO.Checked[24] := True;
   if( GlobalConfig.ControlIOs and LongWord(POST_QUERY_SECURITY) > 0)
  then CheckListBox_ControlIO.Checked[25] := True;
  if( GlobalConfig.ControlIOs and LongWord(PRE_SET_SECURITY) > 0)
  then CheckListBox_ControlIO.Checked[26] := True;
  if( GlobalConfig.ControlIOs and LongWord(POST_SET_SECURITY) > 0)
  then CheckListBox_ControlIO.Checked[27] := True;
  if( GlobalConfig.ControlIOs and LongWord(PRE_CLEANUP) > 0)
  then CheckListBox_ControlIO.Checked[28] := True;
  if( GlobalConfig.ControlIOs and LongWord(POST_CLEANUP) > 0)
  then CheckListBox_ControlIO.Checked[29] := True;
  if( GlobalConfig.ControlIOs and LongWord(PRE_CLOSE) > 0)
  then CheckListBox_ControlIO.Checked[30] := True;
  if( GlobalConfig.ControlIOs and LongWord(POST_CLOSE) > 0)
  then CheckListBox_ControlIO.Checked[31] := True;

end;



procedure TForm1.Button_SaveSettingsClick(Sender: TObject);
begin

GlobalConfig.IncludeFileFilterMask := Edit_IncludeFileFilterMask.Text;
GlobalConfig.ExcludeFileFilterMask := Edit_ExcludeFileFilterMask.Text;
GlobalConfig.EncryptionPasswordPhrase := Edit_PasswordPhrase.Text;

GlobalConfig.AccessFlags := LongWord(ALLOW_MAX_RIGHT_ACCESS);
if( not CheckListBox_AccessFlags.Checked[0])
then  GlobalConfig.AccessFlags := GlobalConfig.AccessFlags And (not LongWord(FILE_ENCRYPTION_RULE));
if( not CheckListBox_AccessFlags.Checked[1])
then GlobalConfig.AccessFlags := GlobalConfig.AccessFlags And (not LongWord(ALLOW_READ_ACCESS));
if( not CheckListBox_AccessFlags.Checked[2])
then  GlobalConfig.AccessFlags := GlobalConfig.AccessFlags And (not LongWord(ALLOW_WRITE_ACCESS));
if( not CheckListBox_AccessFlags.Checked[3])
then  GlobalConfig.AccessFlags := GlobalConfig.AccessFlags And (not LongWord(ALLOW_FILE_DELETE));
if( not CheckListBox_AccessFlags.Checked[4])
then  GlobalConfig.AccessFlags := GlobalConfig.AccessFlags And (not LongWord(ALLOW_FILE_RENAME));
if( not CheckListBox_AccessFlags.Checked[5])
then GlobalConfig.AccessFlags := GlobalConfig.AccessFlags And (not LongWord(ALLOW_OPEN_WITH_CREATE_OR_OVERWRITE_ACCESS));
if( not CheckListBox_AccessFlags.Checked[6])
then  GlobalConfig.AccessFlags := GlobalConfig.AccessFlags And (not LongWord(ALLOW_FILE_MEMORY_MAPPED));
if( not CheckListBox_AccessFlags.Checked[7])
then  GlobalConfig.AccessFlags := GlobalConfig.AccessFlags And (not LongWord(ALLOW_SET_SECURITY_ACCESS));
if( not CheckListBox_AccessFlags.Checked[8])
then GlobalConfig.AccessFlags := GlobalConfig.AccessFlags And (not LongWord(ALLOW_FILE_ACCESS_FROM_NETWORK));

GlobalConfig.MonitorFileEvents := 0;
if( CheckListBox_FileEvents.Checked[0])
then  GlobalConfig.MonitorFileEvents := GlobalConfig.MonitorFileEvents or LongWord(FILE_CREATEED);
if( CheckListBox_FileEvents.Checked[1])
then  GlobalConfig.MonitorFileEvents := GlobalConfig.MonitorFileEvents or LongWord(FILE_WRITTEN);
if( CheckListBox_FileEvents.Checked[2])
then  GlobalConfig.MonitorFileEvents := GlobalConfig.MonitorFileEvents or LongWord(FILE_RENAMED);
if( CheckListBox_FileEvents.Checked[3])
then  GlobalConfig.MonitorFileEvents := GlobalConfig.MonitorFileEvents or LongWord(FILE_DELETED);
if( CheckListBox_FileEvents.Checked[4])
then  GlobalConfig.MonitorFileEvents := GlobalConfig.MonitorFileEvents or LongWord(FILE_SECURITY_CHANGED);
if( CheckListBox_FileEvents.Checked[5])
then  GlobalConfig.MonitorFileEvents := GlobalConfig.MonitorFileEvents or LongWord(FILE_INFO_CHANGED);
if( CheckListBox_FileEvents.Checked[6])
then  GlobalConfig.MonitorFileEvents := GlobalConfig.MonitorFileEvents or LongWord(FILE_READ);

GlobalConfig.MonitorIOs := 0;
if( CheckListBox_MonitorIO.Checked[0])
then  GlobalConfig.MonitorIOs := GlobalConfig.MonitorIOs or LongWord(POST_CREATE);
if( CheckListBox_MonitorIO.Checked[1])
then  GlobalConfig.MonitorIOs := GlobalConfig.MonitorIOs or LongWord(POST_FASTIO_READ);
if( CheckListBox_MonitorIO.Checked[2])
then  GlobalConfig.MonitorIOs := GlobalConfig.MonitorIOs or LongWord(POST_CACHE_READ);
if( CheckListBox_MonitorIO.Checked[3])
then  GlobalConfig.MonitorIOs := GlobalConfig.MonitorIOs or LongWord(POST_NOCACHE_READ);
if( CheckListBox_MonitorIO.Checked[4])
then  GlobalConfig.MonitorIOs := GlobalConfig.MonitorIOs or LongWord(POST_PAGING_IO_READ);
if( CheckListBox_MonitorIO.Checked[5])
then  GlobalConfig.MonitorIOs := GlobalConfig.MonitorIOs or LongWord(POST_FASTIO_WRITE);
if( CheckListBox_MonitorIO.Checked[6])
then  GlobalConfig.MonitorIOs := GlobalConfig.MonitorIOs or LongWord(POST_CACHE_WRITE);
if( CheckListBox_MonitorIO.Checked[7])
then  GlobalConfig.MonitorIOs := GlobalConfig.MonitorIOs or LongWord(POST_NOCACHE_WRITE);
if( CheckListBox_MonitorIO.Checked[8])
then  GlobalConfig.MonitorIOs := GlobalConfig.MonitorIOs or LongWord(POST_PAGING_IO_WRITE);
if( CheckListBox_MonitorIO.Checked[9])
then  GlobalConfig.MonitorIOs := GlobalConfig.MonitorIOs or LongWord(POST_QUERY_INFORMATION);
if( CheckListBox_MonitorIO.Checked[10])
then  GlobalConfig.MonitorIOs := GlobalConfig.MonitorIOs or LongWord(POST_SET_INFORMATION);
if( CheckListBox_MonitorIO.Checked[11])
then  GlobalConfig.MonitorIOs := GlobalConfig.MonitorIOs or LongWord(POST_DIRECTORY);
if( CheckListBox_MonitorIO.Checked[12])
then  GlobalConfig.MonitorIOs := GlobalConfig.MonitorIOs or LongWord(POST_QUERY_SECURITY);
if( CheckListBox_MonitorIO.Checked[13])
then  GlobalConfig.MonitorIOs := GlobalConfig.MonitorIOs or LongWord(POST_SET_SECURITY);
if( CheckListBox_MonitorIO.Checked[14])
then  GlobalConfig.MonitorIOs := GlobalConfig.MonitorIOs or LongWord(POST_CLEANUP);
if( CheckListBox_MonitorIO.Checked[15])
then  GlobalConfig.MonitorIOs := GlobalConfig.MonitorIOs or LongWord(POST_CLOSE);

GlobalConfig.ControlIOs := 0;
if( CheckListBox_ControlIO.Checked[0])
then  GlobalConfig.ControlIOs := GlobalConfig.ControlIOs or LongWord(PRE_CREATE);
if( CheckListBox_ControlIO.Checked[1])
then  GlobalConfig.ControlIOs := GlobalConfig.ControlIOs or LongWord(POST_CREATE);
if( CheckListBox_ControlIO.Checked[2])
then  GlobalConfig.ControlIOs := GlobalConfig.ControlIOs or LongWord(PRE_FASTIO_READ);
if( CheckListBox_ControlIO.Checked[3])
then  GlobalConfig.ControlIOs := GlobalConfig.ControlIOs or LongWord(POST_FASTIO_READ);
if( CheckListBox_ControlIO.Checked[4])
then  GlobalConfig.ControlIOs := GlobalConfig.ControlIOs or LongWord(PRE_CACHE_READ);
if( CheckListBox_ControlIO.Checked[5])
then  GlobalConfig.ControlIOs := GlobalConfig.ControlIOs or LongWord(POST_CACHE_READ);
if( CheckListBox_ControlIO.Checked[6])
then  GlobalConfig.ControlIOs := GlobalConfig.ControlIOs or LongWord(PRE_NOCACHE_READ);
if( CheckListBox_ControlIO.Checked[7])
then  GlobalConfig.ControlIOs := GlobalConfig.ControlIOs or LongWord(POST_NOCACHE_READ);
if( CheckListBox_ControlIO.Checked[8])
then  GlobalConfig.ControlIOs := GlobalConfig.ControlIOs or LongWord(PRE_PAGING_IO_READ);
if( CheckListBox_ControlIO.Checked[9])
then  GlobalConfig.ControlIOs := GlobalConfig.ControlIOs or LongWord(POST_PAGING_IO_READ);
if( CheckListBox_ControlIO.Checked[10])
then  GlobalConfig.ControlIOs := GlobalConfig.ControlIOs or LongWord(PRE_FASTIO_WRITE);
if( CheckListBox_ControlIO.Checked[11])
then  GlobalConfig.ControlIOs := GlobalConfig.ControlIOs or LongWord(POST_FASTIO_WRITE);
if( CheckListBox_ControlIO.Checked[12])
then  GlobalConfig.ControlIOs := GlobalConfig.ControlIOs or LongWord(PRE_CACHE_WRITE);
if( CheckListBox_ControlIO.Checked[13])
then  GlobalConfig.ControlIOs := GlobalConfig.ControlIOs or LongWord(POST_CACHE_WRITE);
if( CheckListBox_ControlIO.Checked[14])
then  GlobalConfig.ControlIOs := GlobalConfig.ControlIOs or LongWord(PRE_NOCACHE_WRITE);
if( CheckListBox_ControlIO.Checked[15])
then  GlobalConfig.ControlIOs := GlobalConfig.ControlIOs or LongWord(POST_NOCACHE_WRITE);
if( CheckListBox_ControlIO.Checked[16])
then  GlobalConfig.ControlIOs := GlobalConfig.ControlIOs or LongWord(PRE_PAGING_IO_WRITE);
if( CheckListBox_ControlIO.Checked[17])
then  GlobalConfig.ControlIOs := GlobalConfig.ControlIOs or LongWord(POST_PAGING_IO_WRITE);
if( CheckListBox_ControlIO.Checked[18])
then  GlobalConfig.ControlIOs := GlobalConfig.ControlIOs or LongWord(PRE_QUERY_INFORMATION);
if( CheckListBox_ControlIO.Checked[19])
then  GlobalConfig.ControlIOs := GlobalConfig.ControlIOs or LongWord(POST_QUERY_INFORMATION);
if( CheckListBox_ControlIO.Checked[20])
then  GlobalConfig.ControlIOs := GlobalConfig.ControlIOs or LongWord(PRE_SET_INFORMATION);
if( CheckListBox_ControlIO.Checked[21])
then  GlobalConfig.ControlIOs := GlobalConfig.ControlIOs or LongWord(POST_SET_INFORMATION);
if( CheckListBox_ControlIO.Checked[22])
then  GlobalConfig.ControlIOs := GlobalConfig.ControlIOs or LongWord(PRE_DIRECTORY);
if( CheckListBox_ControlIO.Checked[23])
then  GlobalConfig.ControlIOs := GlobalConfig.ControlIOs or LongWord(POST_DIRECTORY);
if( CheckListBox_ControlIO.Checked[24])
then  GlobalConfig.ControlIOs := GlobalConfig.ControlIOs or LongWord(PRE_QUERY_SECURITY);
if( CheckListBox_ControlIO.Checked[25])
then  GlobalConfig.ControlIOs := GlobalConfig.ControlIOs or LongWord(POST_QUERY_SECURITY);
if( CheckListBox_ControlIO.Checked[26])
then  GlobalConfig.ControlIOs := GlobalConfig.ControlIOs or LongWord(PRE_SET_SECURITY);
if( CheckListBox_ControlIO.Checked[27])
then  GlobalConfig.ControlIOs := GlobalConfig.ControlIOs or LongWord(POST_SET_SECURITY);
if( CheckListBox_ControlIO.Checked[28])
then  GlobalConfig.ControlIOs := GlobalConfig.ControlIOs or LongWord(PRE_CLEANUP);
if( CheckListBox_ControlIO.Checked[29])
then  GlobalConfig.ControlIOs := GlobalConfig.ControlIOs or LongWord(POST_CLEANUP);
if( CheckListBox_ControlIO.Checked[30])
then  GlobalConfig.ControlIOs := GlobalConfig.ControlIOs or LongWord(PRE_CLOSE);
if( CheckListBox_ControlIO.Checked[31])
then  GlobalConfig.ControlIOs := GlobalConfig.ControlIOs or LongWord(POST_CLOSE);

 GlobalConfig.SaveINI;


end;




/////////////////////////////////////////////////////////////////////////////
constructor TDisplayThread.Create(UseFormIn: TObject);
begin
  inherited Create(True);
  PendingDisplayItems := TStringList.Create;
  UseForm := TForm1(UseFormIn);
  FreeOnTerminate := True;
  fCompletedEvent := CreateEvent(nil, False, False, nil);
  Resume;
end;

procedure TDisplayThread.ClearLists;
begin
  if PendingDisplayItems = nil then
    exit;

  try
    if (Assigned(UseForm)) and (Assigned(PendingDisplayItems)) then
      begin
        PendingDisplayItems.Clear;
      end;
  except

  end;

end;

destructor TDisplayThread.Destroy;
begin
  try

    UseForm := nil;
    PendingDisplayItems.Free;
    PendingDisplayItems := nil;

  except
  end;
  inherited Destroy;
end;

procedure TDisplayThread.Execute;
const
  SendMax = 1;
begin
//  {$IFNDEF WIN64}
  NameThreadForDebugging('DisplayThread');
//  {$ENDIF}

  while (UseForm <> nil) and (Not Terminated) do
     try
      if CloseAppInProgress then break;

       SendToDisplay(Form1);

    except
    end;

  //send one more time to flush last Vars...
  //SendToDisplay(UseForm);

end;

procedure TDisplayThread.SendToDisplay(UseFormIn: TObject);
begin
  if CloseAppInProgress then
    begin
      UseForm := UseFormIn;
      exit;
    end;
  try
    UseForm := UseFormIn;
    Synchronize(SendToDisplaySync);
  except
  end;
end;

procedure TDisplayThread.SendToDisplaySync;
var
  I, N: integer;
  StrOut: String;
  tStr: String;
  tCount: Cardinal;
  UseMethod: Integer;
begin
  UseMethod := 0;
  try
    tStr := '';

    with TForm1(UseForm) do
      begin
        try

          if PendingDisplayItems.Count > 0 then
            begin
              I := 0;
              try
                Lock_DisplayListing.Acquire;
                tCount := GetTickCount;
                while PendingDisplayItems.Count > 0 do
                  begin
                    //if CanceledByUser then break;
                    Memo1.Lines.Add(PendingDisplayItems[0]);
                    PendingDisplayItems.Delete(0);
                    Inc(I);
                    if (I > 1000) or
                       ((GetTickCount - tCount) > 500) then break;  //limit display time.....
                  end;
              except
              end;
              Lock_DisplayListing.Release;
            end
          else
            Sleep(1);

        except
        end;

      end; //end with


  finally

  end;

end;

procedure TDisplayThread.AddDisplayInfo(DisplayStr: String);
begin
  try
    PendingDisplayItems.Add(DisplayStr);
  finally

  end;

end;

end.
