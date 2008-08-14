unit ki_web_ui;

interface

uses
  kix,
  system.web.ui,
  system.web.ui.webcontrols;

type
  //
  nature_of_visit_type =
    (
    VISIT_COLD_CALL,
    VISIT_INITIAL,
    VISIT_POSTBACK_STANDARD,
    VISIT_POSTBACK_STALE
    );
  //
  page_class = class(System.Web.UI.Page)
  private
    function NatureOfInvocation
      (
      expected_session_item_name: string;
      be_timeout_behavior_standard: boolean;
      be_landing_from_login: boolean;
      be_cold_call_allowed: boolean
      )
      : nature_of_visit_type;
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
      value: string;
      be_using_scriptmanager: boolean = FALSE
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
    procedure Focus
      (
      c: control;
      be_using_scriptmanager: boolean = FALSE
      );
    function NatureOfLanding
      (
      expected_session_item_name: string;
      be_timeout_behavior_standard: boolean = TRUE
      )
      : nature_of_visit_type;
    function NatureOfVisit
      (
      expected_session_item_name: string;
      be_timeout_behavior_standard: boolean = TRUE
      )
      : nature_of_visit_type;
    function NatureOfVisitUnlimited
      (
      expected_session_item_name: string;
      be_timeout_behavior_standard: boolean = TRUE
      )
      : nature_of_visit_type;
    procedure SessionSet
      (
      name: string;
      value: system.object
      );
    procedure TransferToPageBinderTab
      (
      page_nick: string;
      binder_nick: string;
      tab_index: cardinal
      );
  protected
    procedure OnInit(e: EventArgs); override;
    procedure RequireConfirmation
      (
      c: webcontrol;
      prompt: string
      );
    procedure ValidationAlert(be_using_scriptmanager: boolean = FALSE);
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
      value: string;
      be_using_scriptmanager: boolean = FALSE
      );
    function AlertMessage
      (
      cause: alert_cause_type;
      state: alert_state_type;
      key: string;
      value: string
      )
      : string;
    procedure DropCrumbAndTransferTo(the_path: string);
    procedure EstablishClientSideFunction
      (
      profile: string;
      body: string
      );
      overload;
    procedure EstablishClientSideFunction(enumeral: client_side_function_enumeral_type); overload;
    procedure EstablishClientSideFunction(r: client_side_function_rec_type); overload;
    procedure Focus
      (
      c: control;
      be_using_scriptmanager: boolean = FALSE
      );
    procedure SessionSet
      (
      name: string;
      value: system.object
      );
    procedure TransferToPageBinderTab
      (
      page_nick: string;
      binder_nick: string;
      tab_index: cardinal
      );
  protected
    procedure RequireConfirmation
      (
      c: webcontrol;
      prompt: string
      );
    procedure ValidationAlert(be_using_scriptmanager: boolean = FALSE);
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
  value: string;
  be_using_scriptmanager: boolean = FALSE
  );
begin
  kix.Alert(page,configurationmanager.appsettings['application_name'],cause,state,key,value,be_using_scriptmanager);
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
  SessionSet('waypoint_stack',stack.Create);
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

procedure page_class.Focus
  (
  c: control;
  be_using_scriptmanager: boolean = FALSE
  );
begin
  if be_using_scriptmanager then begin
    scriptmanager.RegisterStartupScript(page,page.GetType,'SetFocus','document.getElementById("' + c.clientid + '").focus();',TRUE);
  end else begin
    clientscript.RegisterStartupScript(page.GetType,'SetFocus','document.getElementById("' + c.clientid + '").focus();',TRUE);
  end;
end;

function page_class.NatureOfInvocation
  (
  expected_session_item_name: string;
  be_timeout_behavior_standard: boolean;
  be_landing_from_login: boolean;
  be_cold_call_allowed: boolean
  )
  : nature_of_visit_type;
var
  be_cold_call: boolean;
begin
  if not IsPostBack then begin
    if be_landing_from_login then begin
      be_cold_call_allowed := FALSE;
      be_cold_call := (session['user_id'] = nil) or (session['username'] = nil);
    end else begin
      be_cold_call := (request.servervariables['URL'] = request.currentexecutionfilepath);
        // The request for this page could not have been the result of a server.Transfer call, and the session state is therefore
        // unknown.  This is rarely allowed.
    end;
    //
    if be_cold_call then begin
      NatureOfInvocation := VISIT_COLD_CALL;
      if not be_cold_call_allowed then begin
        session.Clear;
        server.Transfer('~/login.aspx');
      end;
    end else begin
      NatureOfInvocation := VISIT_INITIAL;
    end;
    //
  end else begin
    if assigned(session[expected_session_item_name]) then begin
      NatureOfInvocation := VISIT_POSTBACK_STANDARD;
    end else begin
      NatureOfInvocation := VISIT_POSTBACK_STALE;
      if be_timeout_behavior_standard then begin
        server.Transfer('~/timeout.aspx');
      end;
    end;
  end;
end;

function page_class.NatureOfLanding
  (
  expected_session_item_name: string;
  be_timeout_behavior_standard: boolean = TRUE
  )
  : nature_of_visit_type;
begin
  NatureOfLanding := NatureOfInvocation(expected_session_item_name,be_timeout_behavior_standard,TRUE,FALSE);
end;

function page_class.NatureOfVisit
  (
  expected_session_item_name: string;
  be_timeout_behavior_standard: boolean = TRUE
  )
  : nature_of_visit_type;
begin
  NatureOfVisit := NatureOfInvocation(expected_session_item_name,be_timeout_behavior_standard,FALSE,FALSE);
end;

function page_class.NatureOfVisitUnlimited
  (
  expected_session_item_name: string;
  be_timeout_behavior_standard: boolean = TRUE
  )
  : nature_of_visit_type;
begin
  NatureOfVisitUnlimited := NatureOfInvocation(expected_session_item_name,be_timeout_behavior_standard,FALSE,TRUE);
end;

procedure page_class.OnInit(e: system.eventargs);
begin
  inherited OnInit(e);
end;

procedure page_class.RequireConfirmation
  (
  c: webcontrol;
  prompt: string
  );
begin
  kix.RequireConfirmation(c,prompt,configurationmanager.appsettings['application_name']);
end;

procedure page_class.SessionSet
  (
  name: string;
  value: system.object
  );
begin
  session.Remove(name);
  session.Add(name,value);
end;

procedure page_class.TransferToPageBinderTab
  (
  page_nick: string;
  binder_nick: string;
  tab_index: cardinal
  );
begin
  SessionSet('UserControl_' + binder_nick + '_binder_selected_tab',system.object(tab_index));
  server.Transfer(page_nick + '.aspx');
end;

procedure page_class.ValidationAlert(be_using_scriptmanager: boolean = FALSE);
begin
  Alert(kix.USER,kix.FAILURE,'stdsvrval',STD_VALIDATION_ALERT,be_using_scriptmanager);
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
  value: string;
  be_using_scriptmanager: boolean = FALSE
  );
begin
  kix.Alert(page,configurationmanager.appsettings['application_name'],cause,state,key,value,be_using_scriptmanager);
end;

function usercontrol_class.AlertMessage
  (
  cause: alert_cause_type;
  state: alert_state_type;
  key: string;
  value: string
  )
  : string;
begin
  AlertMessage := kix.AlertMessage(configurationmanager.appsettings['application_name'],cause,state,key,value);
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

procedure usercontrol_class.Focus
  (
  c: control;
  be_using_scriptmanager: boolean = FALSE
  );
begin
  if be_using_scriptmanager then begin
    scriptmanager.RegisterStartupScript
      (
      page,
      page.GetType,
      'SetFocus',
      'if (!document.getElementById("' + c.clientid + '").disabled) {document.getElementById("' + c.clientid + '").focus();}',
      TRUE
      );
  end else begin
    page.clientscript.RegisterStartupScript
      (
      page.GetType,
      'SetFocus',
      'if (!document.getElementById("' + c.clientid + '").disabled) {document.getElementById("' + c.clientid + '").focus();}',
      TRUE
      );
  end;
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

procedure usercontrol_class.SessionSet
  (
  name: string;
  value: system.object
  );
begin
  session.Remove(name);
  session.Add(name,value);
end;

procedure usercontrol_class.TransferToPageBinderTab
  (
  page_nick: string;
  binder_nick: string;
  tab_index: cardinal
  );
begin
  SessionSet('UserControl_' + binder_nick + '_binder_selected_tab',system.object(tab_index));
  server.Transfer(page_nick + '.aspx');
end;

procedure usercontrol_class.ValidationAlert(be_using_scriptmanager: boolean = FALSE);
begin
  Alert(kix.USER,kix.FAILURE,'stdsvrval',STD_VALIDATION_ALERT,be_using_scriptmanager);
end;

end.
