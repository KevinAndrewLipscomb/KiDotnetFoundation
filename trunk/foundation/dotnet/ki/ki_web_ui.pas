unit ki_web_ui;

interface

uses
  kix,
  system.web.ui,
  system.web.ui.webcontrols;

type
  //
  page_class = class(System.Web.UI.Page)
  protected
    procedure OnInit(e: EventArgs); override;
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
    procedure BackTrack(num_backsteps: cardinal = 1);
    procedure BeginBreadCrumbTrail;
    procedure DropCrumbAndTransferTo(the_path: string);
    procedure EstablishClientSideFunction
      (
      profile: string;
      body: string
      );
      overload;
    procedure EstablishClientSideFunction(enumeral: client_side_function_enumeral_type); overload;
    procedure EstablishClientSideFunction(r: client_side_function_rec_type); overload;
    procedure Focus(c: control);
  protected
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
  protected
    procedure OnInit(e: EventArgs); override;
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
    procedure DropCrumbAndTransferTo(the_path: string);
    procedure EstablishClientSideFunction
      (
      profile: string;
      body: string
      );
      overload;
    procedure EstablishClientSideFunction(enumeral: client_side_function_enumeral_type); overload;
    procedure EstablishClientSideFunction(r: client_side_function_rec_type); overload;
    procedure Focus(c: control);
  protected
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
  system.collections,
  system.configuration,
  system.io;

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
  kix.Alert(page,configurationmanager.appsettings['application_name'],cause,state,key,value);
end;

procedure page_class.BackTrack(num_backsteps: cardinal = 1);
var
  i: cardinal;
  p: string;
begin
  for i := 1 to num_backsteps do begin
    p := stack(session['waypoint_stack']).Pop.tostring;
  end;
  server.Transfer(p);
end;

procedure page_class.BeginBreadCrumbTrail;
begin
  session.Remove('waypoint_stack');
  session.Add('waypoint_stack',stack.Create);
end;

procedure page_class.DropCrumbAndTransferTo(the_path: string);
var
  current: string;
begin
  current := path.GetFileName(request.CurrentExecutionFilePath);
  if (stack(session['waypoint_stack']).count = 0) or (stack(session['waypoint_stack']).Peek.tostring <> current) then begin
    stack(session['waypoint_stack']).Push(current);
  end;
  server.Transfer(the_path);
end;

procedure page_class.EstablishClientSideFunction
  (
  profile: string;
  body: string
  );
begin
  kix.EstablishClientSideFunction(page,profile,body);
end;

procedure page_class.EstablishClientSideFunction(enumeral: client_side_function_enumeral_type);
begin
  kix.EstablishClientSideFunction(page,enumeral);
end;

procedure page_class.EstablishClientSideFunction(r: client_side_function_rec_type);
begin
  kix.EstablishClientSideFunction(page,r);
end;

procedure page_class.Focus(c: control);
begin
  clientscript.RegisterStartupScript(page.GetType,'SetFocus','document.getElementById("' + c.clientid + '").focus();',TRUE);
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
  kix.RequireConfirmation(c,prompt,configurationmanager.appsettings['application_name']);
end;

procedure page_class.ValidationAlert;
begin
  Alert(kix.USER,kix.FAILURE,'stdsvrval',STD_VALIDATION_ALERT);
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
  kix.Alert(page,configurationmanager.appsettings['application_name'],cause,state,key,value);
end;

procedure usercontrol_class.DropCrumbAndTransferTo(the_path: string);
begin
  stack(session['waypoint_stack']).Push(path.GetFileName(request.CurrentExecutionFilePath));
  server.Transfer(the_path);
end;

procedure usercontrol_class.EstablishClientSideFunction
  (
  profile: string;
  body: string
  );
begin
  kix.EstablishClientSideFunction(page,profile,body,self.clientid);
end;

procedure usercontrol_class.EstablishClientSideFunction(enumeral: client_side_function_enumeral_type);
begin
  kix.EstablishClientSideFunction(page,enumeral);
end;

procedure usercontrol_class.EstablishClientSideFunction(r: client_side_function_rec_type);
begin
  kix.EstablishClientSideFunction(page,r);
end;

procedure usercontrol_class.Focus(c: control);
begin
  page.clientscript.RegisterStartupScript
    (
    page.GetType,
    'SetFocus',
    'if (!document.getElementById("' + c.clientid + '").disabled) {document.getElementById("' + c.clientid + '").focus();}',
    TRUE
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
  kix.RequireConfirmation(c,prompt,configurationmanager.appsettings['application_name']);
end;

procedure usercontrol_class.ValidationAlert;
begin
  Alert(kix.USER,kix.FAILURE,'stdsvrval',STD_VALIDATION_ALERT);
end;

end.
