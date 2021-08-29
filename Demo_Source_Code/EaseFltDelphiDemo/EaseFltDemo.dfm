object Form1: TForm1
  Left = 0
  Top = 0
  Caption = 'File Monitor And Protector Demo'
  ClientHeight = 589
  ClientWidth = 1088
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'Tahoma'
  Font.Style = []
  OldCreateOrder = False
  OnClose = FormClose
  OnCreate = FormCreate
  DesignSize = (
    1088
    589)
  PixelsPerInch = 96
  TextHeight = 13
  object PageControl1: TPageControl
    Left = 6
    Top = 0
    Width = 1059
    Height = 569
    ActivePage = TabSheet1
    Anchors = [akLeft, akTop, akRight, akBottom]
    TabOrder = 0
    object TabSheet2: TTabSheet
      Caption = 'Filter Settings'
      ImageIndex = 1
      object Label1: TLabel
        Left = 24
        Top = 24
        Width = 121
        Height = 13
        Caption = 'Managed File Filter Mask:'
      end
      object Label2: TLabel
        Left = 24
        Top = 64
        Width = 66
        Height = 13
        Caption = 'Access Rights'
      end
      object Label3: TLabel
        Left = 536
        Top = 24
        Width = 114
        Height = 13
        Caption = 'Exclude File Filter Mask:'
      end
      object Label4: TLabel
        Left = 536
        Top = 64
        Width = 138
        Height = 13
        Caption = 'Register Monitor File Events:'
      end
      object Label5: TLabel
        Left = 24
        Top = 192
        Width = 94
        Height = 13
        Caption = 'Register Monitor IO'
      end
      object Label6: TLabel
        Left = 536
        Top = 192
        Width = 93
        Height = 13
        Caption = 'Register Control IO'
      end
      object Label7: TLabel
        Left = 24
        Top = 344
        Width = 136
        Height = 13
        Caption = 'Encryption Password Phrase'
      end
      object Edit_IncludeFileFilterMask: TEdit
        Left = 200
        Top = 21
        Width = 281
        Height = 21
        TabOrder = 0
        Text = 'c:\test\*'
      end
      object CheckListBox_FileEvents: TCheckListBox
        Left = 688
        Top = 64
        Width = 281
        Height = 97
        ItemHeight = 13
        Items.Strings = (
          'CREATED'
          'WRITTEN'
          'RENAMED'
          'DELETED'
          'SECURITY_CHANGED'
          'INFO_CHANGED'
          'READ')
        TabOrder = 1
      end
      object Button_SaveSettings: TButton
        Left = 24
        Top = 496
        Width = 75
        Height = 25
        Caption = 'Save Settings'
        TabOrder = 2
        OnClick = Button_SaveSettingsClick
      end
      object Edit_ExcludeFileFilterMask: TEdit
        Left = 688
        Top = 21
        Width = 281
        Height = 21
        TabOrder = 3
      end
      object CheckListBox_AccessFlags: TCheckListBox
        Left = 200
        Top = 64
        Width = 281
        Height = 97
        ItemHeight = 13
        Items.Strings = (
          'ENABLE_FILE_ENCRYPTION'
          'ALLOW_READ_ACCESS'
          'ALLOW_WRITE_ACCESS'
          'ALLOW_FILE_DELETION'
          'ALLOW_FILE_RENAME'
          'ALLOW_FILE_CREATION'
          'ALLOW_FILE_EXECUTION'
          'ALLOW_SECURITY_CHANGE'
          'ALLOW_REMOTE_ACCESS')
        TabOrder = 4
      end
      object CheckListBox_MonitorIO: TCheckListBox
        Left = 200
        Top = 192
        Width = 281
        Height = 97
        ItemHeight = 13
        Items.Strings = (
          'POST_CREATE'
          'POST_FASTIO_READ'
          'POST_CACHE_READ'
          'POST_NOCACHE_READ'
          'POST_PAGING_IO_READ'
          'POST_FASTIO_WRITE'
          'POST_CACHE_WRITE'
          'POST_NOCACHE_WRITE'
          'POST_PAGING_IO_WRITE'
          'POST_QUERY_INFORMATION'
          'POST_SET_INFORMATION'
          'POST_DIRECTORY'
          'POST_QUERY_SECURITY'
          'POST_SET_SECURITY'
          'POST_CLEANUP'
          'POST_CLOSE')
        TabOrder = 5
      end
      object CheckListBox_ControlIO: TCheckListBox
        Left = 688
        Top = 192
        Width = 281
        Height = 97
        ItemHeight = 13
        Items.Strings = (
          'PRE_CREATE'
          'POST_CREATE'
          'PRE_FASTIO_READ'
          'POST_FASTIO_READ'
          'PRE_CACHE_READ'
          'POST_CACHE_READ'
          'PRE_NOCACHE_READ'
          'POST_NOCACHE_READ'
          'PRE_PAGING_IO_READ'
          'POST_PAGING_IO_READ'
          'PRE_FASTIO_WRITE'
          'POST_FASTIO_WRITE'
          'PRE_CACHE_WRITE'
          'POST_CACHE_WRITE'
          'PRE_NOCACHE_WRITE'
          'POST_NOCACHE_WRITE'
          'PRE_PAGING_IO_WRITE'
          'POST_PAGING_IO_WRITE'
          'PRE_QUERY_INFORMATION'
          'POST_QUERY_INFORMATION'
          'PRE_SET_INFORMATION'
          'POST_SET_INFORMATION'
          'PRE_DIRECTORY'
          'POST_DIRECTORY'
          'PRE_QUERY_SECURITY'
          'POST_QUERY_SECURITY'
          'PRE_SET_SECURITY'
          'POST_SET_SECURITY'
          'PRE_CLEANUP'
          'POST_CLEANUP'
          'PRE_CLOSE'
          'POST_CLOSE')
        TabOrder = 6
      end
      object Edit_PasswordPhrase: TEdit
        Left = 200
        Top = 341
        Width = 281
        Height = 21
        TabOrder = 7
      end
    end
    object TabSheet1: TTabSheet
      Caption = 'Filter Console'
      ImageIndex = 1
      object Memo1: TMemo
        Left = 3
        Top = 56
        Width = 1038
        Height = 457
        Lines.Strings = (
          'Memo1')
        TabOrder = 0
      end
      object Button_Start: TButton
        Left = 3
        Top = 16
        Width = 75
        Height = 25
        Caption = 'Start Filter'
        TabOrder = 1
        OnClick = Button_StartClick
      end
      object Button_Stop: TButton
        Left = 112
        Top = 16
        Width = 75
        Height = 25
        Caption = 'Stop Filter'
        TabOrder = 2
        OnClick = Button_StopClick
      end
    end
  end
end
