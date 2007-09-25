unit ki_web_ui;

interface

uses
  ki,
  system.web.ui,
  system.web.ui.webcontrols;

type
  //
  page_class = class(System.Web.UI.Page)
  strict protected
    function AddIdentifiedControlToPlaceHolder
      (
      c: control;
      id: string;
      p: placeholder
      )
      : string;
    procedure Alert
      (
      cause: alert_cause_type;
      state: alert_state_type;
      key: string;
      value: string
      );
    procedure Focus(c: control);
    procedure OnInit(e: eventargs); override;
    procedure RequireConfirmation
      (
      c: webcontrol;
      prompt: string
      );
    procedure ValidationAlert;
  public
    constructor Create;
  end;
  //
  usercontrol_class = class(system.web.ui.usercontrol)
  strict protected
    function AddIdentifiedControlToPlaceHolder
      (
      c: control;
      id: string;
      p: placeholder
      )
      : string;
    procedure Alert
      (
      cause: alert_cause_type;
      state: alert_state_type;
      key: string;
      value: string
      );
    procedure Focus(c: control);
    procedure OnInit(e: eventargs); override;
    procedure RequireConfirmation
      (
      c: webcontrol;
      prompt: string
      );
    procedure ValidationAlert;
  public
    constructor Create;
  end;

implementation

uses
  system.configuration;

const
  STD_VALIDATION_ALERT = 'Something about the data you just submitted is invalid.  Look for !ERR! indications near the data fields.  A more detailed explanation may appear near the top of the page.';

//
// PAGE_CLASS
//

constructor page_class.Create;
begin
  inherited Create;
  // TODO: Add any constructor code here
end;

//
// Without specifying an ID for a dynamically-added control, ASP.NET supplies its own ID for the control.  The problem is that
// ASP.NET may specify one ID for the control at initial page presentation time and another ID at postback page presentation.
// Because postback events are tied to the ID of the control generating the postback, ASP.NET's ID assignment behavior may result
// in a postback event that is ignored the first time (but not subsequent times). 
//
function page_class.AddIdentifiedControlToPlaceHolder
  (
  c: control;
  id: string;
  p: placeholder
  )
  : string;
begin
  c.id := id;
  p.controls.Add(c);
  AddIdentifiedControlToPlaceHolder := id;
end;

procedure page_class.Alert
  (
  cause: alert_cause_type;
  state: alert_state_type;
  key: string;
  value: string
  );
begin
  ki.Alert(page,configurationsettings.appsettings['application_name'],cause,state,key,value);
end;

procedure page_class.Focus(c: control);
begin
  RegisterStartupScript
    (
    'SetFocus',
    '<script language="javascript" type="text/javascript">'
    + 'document.getElementById("' + c.clientid + '").focus();'
    + '</script>'
    );
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

procedure page_class.RequireConfirmation
  (
  c: webcontrol;
  prompt: string
  );
begin
  ki.RequireConfirmation(c,prompt,configurationsettings.appsettings['application_name']);
end;

procedure page_class.ValidationAlert;
begin
  Alert(ki.USER,ki.FAILURE,'stdsvrval',STD_VALIDATION_ALERT);
end;

//
// USERCONTROL_CLASS
//

constructor usercontrol_class.Create;
begin
  inherited Create;
  // TODO: Add any constructor code here
end;

function usercontrol_class.AddIdentifiedControlToPlaceHolder
  (
  c: control;
  id: string;
  p: placeholder
  )
  : string;
begin
  c.id := id;
  p.controls.Add(c);
  AddIdentifiedControlToPlaceHolder := id;
end;

procedure usercontrol_class.Alert
  (
  cause: alert_cause_type;
  state: alert_state_type;
  key: string;
  value: string
  );
begin
  ki.Alert(page,configurationsettings.appsettings['application_name'],cause,state,key,value);
end;

procedure usercontrol_class.Focus(c: control);
begin
  page.RegisterStartupScript
    (
    'SetFocus',
    '<script language="javascript" type="text/javascript">'
    + 'document.getElementById("' + c.clientid + '").focus();'
    + '</script>'
    );
end;

procedure usercontrol_class.OnInit(e: system.eventargs);
begin
  inherited OnInit(e);
end;

procedure usercontrol_class.RequireConfirmation
  (
  c: webcontrol;
  prompt: string
  );
begin
  ki.RequireConfirmation(c,prompt,configurationsettings.appsettings['application_name']);
end;

procedure usercontrol_class.ValidationAlert;
begin
  Alert(ki.USER,ki.FAILURE,'stdsvrval',STD_VALIDATION_ALERT);
end;

end.
