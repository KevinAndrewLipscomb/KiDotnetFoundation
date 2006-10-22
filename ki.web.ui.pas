unit ki.web.ui;

interface

uses
  system.web.ui;

type
  Page = class(System.Web.UI.Page)
  private
    { Private Declarations }
  strict protected
    procedure OnInit(e: eventargs); override;
  public
    constructor Create;
  end;

implementation

constructor Page.Create;
begin
  inherited Create;
  // TODO: Add any constructor code here
end;

procedure Page.OnInit(e: system.eventargs);
var
  cookie_header: string;
begin
  inherited OnInit(e);
  if context.session <> nil then begin
    if session.IsNewSession then begin
      cookie_header := request.headers['cookie'];
      if (cookie_header <> nil) and (cookie_header.IndexOf('ASP.NET_SessionId') >= 0) then begin
        server.Transfer('/timeout.aspx');
      end;
    end;
  end;
end;

end.
