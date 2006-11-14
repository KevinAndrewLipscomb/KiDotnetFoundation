unit ki_web_ui;

interface

uses
  system.web.ui;

type
  page_class = class(System.Web.UI.Page)
  strict protected
    procedure OnInit(e: eventargs); override;
  public
    constructor Create;
  end;

implementation

constructor page_class.Create;
begin
  inherited Create;
  // TODO: Add any constructor code here
end;

procedure page_class.OnInit(e: system.eventargs);
var
  cookie_header: string;
begin
  inherited OnInit(e);
  if context.session <> nil then begin
    if session.IsNewSession then begin
      cookie_header := request.headers['cookie'];
      if (cookie_header <> nil) and (cookie_header.IndexOf('ASP.NET_SessionId') >= 0) then begin
        server.Transfer('~/timeout.aspx');
      end;
    end;
  end;
end;

end.
