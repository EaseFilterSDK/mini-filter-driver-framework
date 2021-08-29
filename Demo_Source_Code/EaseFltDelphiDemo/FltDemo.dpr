program FltDemo;

uses
  Vcl.Forms,
  EaseFltDemo in 'EaseFltDemo.pas' {Form1},
  GlobalConfig in 'GlobalConfig.pas';

{$R *.res}

begin
  Application.Initialize;
  Application.MainFormOnTaskbar := True;
  Application.CreateForm(TForm1, Form1);
  Application.Run;
end.
