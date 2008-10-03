unit ki_web_ui;

INTERFACE

uses
  kix,
  system.web.ui,
  system.web.ui.webcontrols;

type

  //--------------------------------------------------------------------------------------------------------------------------------
  //
  // templatecontrol_class
  //
  //--------------------------------------------------------------------------------------------------------------------------------
  templatecontrol_class = class(system.web.ui.templatecontrol)
  published
    function AddIdentifiedControlToPlaceHolder
      (
      c: control;
      id: string;
      p: placeholder
      )
      : string;
    procedure Alert
      (
      the_page: page;
      cause: alert_cause_type;
      state: alert_state_type;
      key: string;
      value: string;
      be_using_scriptmanager: boolean = FALSE
      );
    function AlertMessage
      (
      application_name: string;
      cause: alert_cause_type;
      state: alert_state_type;
      key: string;
      s: string
      )
      : string;
    procedure ExportToExcel
      (
      the_page: page;
      filename_sans_extension: string;
      excel_string: string
      );
    procedure FileDownload
      (
      the_page: page;
      filename: string
      );
    procedure Focus
      (
      the_page: page;
      c: control;
      be_using_scriptmanager: boolean = FALSE
      );
    procedure RequireConfirmation
      (
      c: webcontrol;
      prompt: string
      );
    procedure SessionSet
      (
      the_page: page;
      name: string;
      value: system.object
      );
    function StringOfControl(c: control): string;
    procedure TransferToPageBinderTab
      (
      the_page: page;
      page_nick: string;
      binder_nick: string;
      tab_index: cardinal
      );
    procedure ValidationAlert
      (
      the_page: page;
      be_using_scriptmanager: boolean = FALSE
      );
  public
    constructor Create;
    procedure EstablishClientSideFunction
      (
      the_page: page;
      profile: string;
      body: string;
      usercontrol_clientid: string = ''
      ); overload;
    procedure EstablishClientSideFunction
      (
      the_page: page;
      enumeral: client_side_function_enumeral_type
      ); overload;
    procedure EstablishClientSideFunction
      (
      the_page: page;
      r: client_side_function_rec_type
      ); overload;
  protected
    procedure OnInit(e: EventArgs); override;
  end;

  //--------------------------------------------------------------------------------------------------------------------------------
  //
  // page_class
  //
  //--------------------------------------------------------------------------------------------------------------------------------
  page_class = class(System.Web.UI.Page)
  published
    type nature_of_visit_type =
      (
      VISIT_COLD_CALL,
      VISIT_INITIAL,
      VISIT_POSTBACK_STANDARD,
      VISIT_POSTBACK_STALE
      );
  public
    constructor Create;
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
    procedure ExportToExcel
      (
      the_page: page;
      filename_sans_extension: string;
      excel_string: string
      );
    procedure FileDownload
      (
      the_page: page;
      filename: string
      );
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
    procedure RequireConfirmation
      (
      c: webcontrol;
      prompt: string
      );
    procedure SessionSet
      (
      name: string;
      value: system.object
      );
    function StringOfControl(c: control): string;
    procedure TransferToPageBinderTab
      (
      page_nick: string;
      binder_nick: string;
      tab_index: cardinal
      );
    procedure ValidationAlert(be_using_scriptmanager: boolean = FALSE);
  strict private
    templatecontrol: templatecontrol_class;
    function NatureOfInvocation
      (
      expected_session_item_name: string;
      be_timeout_behavior_standard: boolean;
      be_landing_from_login: boolean;
      be_cold_call_allowed: boolean
      )
      : nature_of_visit_type;
  end;

  //--------------------------------------------------------------------------------------------------------------------------------
  //
  // usercontrol_class
  //
  //--------------------------------------------------------------------------------------------------------------------------------
  usercontrol_class = class(system.web.ui.usercontrol)
  public
    constructor Create;
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
    procedure ExportToExcel
      (
      the_page: page;
      filename_sans_extension: string;
      excel_string: string
      );
    procedure FileDownload
      (
      the_page: page;
      filename: string
      );
    procedure Focus
      (
      c: control;
      be_using_scriptmanager: boolean = FALSE
      );
    procedure RequireConfirmation
      (
      c: webcontrol;
      prompt: string
      );
    procedure SessionSet
      (
      name: string;
      value: system.object
      );
    function StringOfControl(c: control): string;
    procedure TransferToPageBinderTab
      (
      page_nick: string;
      binder_nick: string;
      tab_index: cardinal
      );
    procedure ValidationAlert(be_using_scriptmanager: boolean = FALSE);
  strict private
    templatecontrol: templatecontrol_class;
  end;

IMPLEMENTATION

uses
  system.collections,
  system.configuration,
  system.io,
  system.web.mail;

const
  STD_VALIDATION_ALERT = 'Something about the data you just submitted is invalid.  Look for !ERR! indications near the data fields.  A more detailed explanation may appear near the top of the page.';

//==================================================================================================================================
//
// templatecontrol_class
//
//==================================================================================================================================

constructor templatecontrol_class.Create;
begin
  inherited Create;
  // TODO: Add any constructor code here
end;

procedure templatecontrol_class.OnInit(e: system.eventargs);
begin
  inherited OnInit(e);
  templatecontrol := templatecontrol_class.Create;
end;

function templatecontrol_class.AddIdentifiedControlToPlaceHolder
  (
  c: control;
  id: string;
  p: placeholder
  )
  : string;
  //
  // Without specifying an ID for a dynamically-added control, ASP.NET supplies its own ID for the control.  The problem is that
  // ASP.NET may specify one ID for the control at initial page presentation time and another ID at postback page presentation.
  // Because postback events are tied to the ID of the control generating the postback, ASP.NET's ID assignment behavior may result
  // in a postback event that is ignored the first time (but not subsequent times).
  //
begin
  c.id := id;
  p.controls.Add(c);
  AddIdentifiedControlToPlaceHolder := id;
end;

procedure templatecontrol_class.Alert
  (
  the_page: page;
  cause: alert_cause_type;
  state: alert_state_type;
  key: string;
  value: string;
  be_using_scriptmanager: boolean = FALSE
  );
var
  script: string;
begin
  //
  script := EMPTY
  + 'alert("'
  + AlertMessage(configurationmanager.appsettings['application_name'],cause,state,key,value)
    .Replace(NEW_LINE,'\n')
    .Replace(TAB,'\t') + '");';
  //
  if be_using_scriptmanager then begin
    scriptmanager.RegisterStartupScript(the_page,the_page.GetType,key,script,TRUE);
  end else begin
    the_page.clientscript.RegisterStartupScript(the_page.GetType,key,script,TRUE);
  end;
  //
end;

function templatecontrol_class.AlertMessage
  (
  application_name: string;
  cause: alert_cause_type;
  state: alert_state_type;
  key: string;
  s: string
  )
  : string;
begin
  AlertMessage := EMPTY
  + '- - - ---------------------------------------------------- - - -' + NEW_LINE
  + '       issuer:  ' + TAB + application_name + NEW_LINE
  + '       cause:   ' + TAB + enum(cause).tostring.tolower + NEW_LINE
  + '       state:   ' + TAB + enum(state).tostring.tolower + NEW_LINE
  + '       key:     ' + TAB + key.tolower + NEW_LINE
  + '       time:    ' + TAB + datetime.Now.tostring('s') + NEW_LINE
  + '- - - ---------------------------------------------------- - - -' + NEW_LINE
  + NEW_LINE
  + NEW_LINE
  + s + NEW_LINE
  + NEW_LINE;
end;

procedure templatecontrol_class.EstablishClientSideFunction
  (
  the_page: page;
  profile: string;
  body: string;
  usercontrol_clientid: string = ''
  );
begin
  the_page.clientscript.RegisterClientScriptBlock
    (
    the_page.GetType,
    usercontrol_clientid + '__' + profile.Remove(profile.IndexOf(OPEN_PARENTHESIS)),
    'function ' + profile + NEW_LINE
    + ' {' + NEW_LINE
    + ' ' + body.Replace(NEW_LINE,NEW_LINE + SPACE) + NEW_LINE
    + ' }' + NEW_LINE,
    TRUE
    );
end;

procedure templatecontrol_class.EstablishClientSideFunction
  (
  the_page: page;
  enumeral: client_side_function_enumeral_type
  );
begin
  case enumeral of
  EL:
    EstablishClientSideFunction(the_page,'El(id)','return document.getElementById(id);');
  KGS_TO_LBS:
    EstablishClientSideFunction(the_page,'KgsToLbs(num_kgs)','return Math.round(+num_kgs*2.204622);');
  LBS_TO_KGS:
    EstablishClientSideFunction(the_page,'LbsToKgs(num_lbs)','return Math.round(+num_lbs/2.204622);');
  end;
end;

procedure templatecontrol_class.EstablishClientSideFunction
  (
  the_page: page;
  r: client_side_function_rec_type
  );
begin
  EstablishClientSideFunction(the_page,r.profile,r.body);
end;

procedure templatecontrol_class.ExportToExcel
  (
  the_page: system.web.ui.page;
  filename_sans_extension: string;
  excel_string: string
  );
begin
  the_page.response.Clear;
  the_page.response.AppendHeader
    (
    'Content-Disposition',
    'attachment; filename=' + filename_sans_extension + '.xls'
    );
  the_page.response.bufferoutput := TRUE;
  the_page.response.contenttype := 'application/vnd.ms-excel';
  the_page.enableviewstate := FALSE;
  the_page.response.Write(excel_string);
  the_page.response.&End;
end;

procedure templatecontrol_class.FileDownload
  (
  the_page: page;
  filename: string
  );
begin
  the_page.response.Clear;
  the_page.response.AppendHeader('Content-Disposition','attachment; filename=' + system.io.path.GetFileName(filename));
  the_page.response.bufferoutput := TRUE;
  the_page.response.contenttype := 'application/octet-stream';
  the_page.enableviewstate := FALSE;
  the_page.response.TransmitFile(filename);
  the_page.response.&End;
end;

procedure templatecontrol_class.Focus
  (
  the_page: page;
  c: control;
  be_using_scriptmanager: boolean = FALSE
  );
begin
  if be_using_scriptmanager then begin
    scriptmanager.RegisterStartupScript
      (
      the_page,
      the_page.GetType,
      'SetFocus',
      'if (!document.getElementById("' + c.clientid + '").disabled) {document.getElementById("' + c.clientid + '").focus();}',
      TRUE
      );
  end else begin
    page.clientscript.RegisterStartupScript
      (
      the_page.GetType,
      'SetFocus',
      'if (!document.getElementById("' + c.clientid + '").disabled) {document.getElementById("' + c.clientid + '").focus();}',
      TRUE
      );
  end;
end;

procedure templatecontrol_class.RequireConfirmation
  (
  c: webcontrol;
  prompt: string
  );
begin
  c.attributes.Add
    (
    'onclick',
    'return confirm("- - - ---------------------------------------------------- - - -\n'
    +               '       issuer:  \t' + configurationmanager.appsettings['application_name'] + '\n'
    +               '       state:   \twarning\n'
    +               '       time:    \t' + datetime.Now.tostring('s') + '\n'
    +               '- - - ---------------------------------------------------- - - -\n\n\n'
    +               prompt.Replace(NEW_LINE,'\n') + '\n\n"'
    + ');'
    );
end;

procedure templatecontrol_class.SessionSet
  (
  the_page: page;
  name: string;
  value: system.object
  );
begin
  the_page.session.Remove(name);
  the_page.session.Add(name,value);
end;

function templatecontrol_class.StringOfControl(c: control): string;
var
  stringwriter: system.io.stringwriter;
begin
  stringwriter := system.io.stringwriter.Create;
  c.RenderControl(system.web.ui.htmltextwriter.Create(stringwriter));
  StringOfControl := stringwriter.tostring;
end;

procedure templatecontrol_class.TransferToPageBinderTab
  (
  the_page: page;
  page_nick: string;
  binder_nick: string;
  tab_index: cardinal
  );
begin
  SessionSet(the_page,'UserControl_' + binder_nick + '_binder_selected_tab',system.object(tab_index));
  the_page.server.Transfer(page_nick + '.aspx');
end;

procedure templatecontrol_class.ValidationAlert
  (
  the_page: page;
  be_using_scriptmanager: boolean = FALSE
  );
begin
  Alert(the_page,kix.USER,kix.FAILURE,'stdsvrval',STD_VALIDATION_ALERT,be_using_scriptmanager);
end;

//==================================================================================================================================
//
// PAGE_CLASS
//
//==================================================================================================================================

constructor page_class.Create;
begin
  inherited Create;
  templatecontrol := templatecontrol_class.Create;
end;

function page_class.AddIdentifiedControlToPlaceHolder
  (
  c: control;
  id: string;
  p: placeholder
  )
  : string;
begin
  AddIdentifiedControlToPlaceHolder := templatecontrol.AddIdentifiedControlToPlaceHolder(c,id,p);
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
  templatecontrol.Alert(page,cause,state,key,value,be_using_scriptmanager);
end;

procedure page_class.BackTrack(num_backsteps: cardinal = 1);
var
  i: cardinal;
  p: string;
begin
  if assigned(session['waypoint_stack']) then begin
    for i := 1 to num_backsteps do begin
      p := stack(session['waypoint_stack']).Pop.tostring;
    end;
  end else begin
    p := '~/Default.aspx';
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
  templatecontrol.EstablishClientSideFunction(page,profile,body);
end;

procedure page_class.EstablishClientSideFunction(enumeral: client_side_function_enumeral_type);
begin
  templatecontrol.EstablishClientSideFunction(page,enumeral);
end;

procedure page_class.EstablishClientSideFunction(r: client_side_function_rec_type);
begin
  templatecontrol.EstablishClientSideFunction(page,r);
end;

procedure page_class.ExportToExcel
  (
  the_page: system.web.ui.page;
  filename_sans_extension: string;
  excel_string: string
  );
begin
  templatecontrol.ExportToExcel(page,filename_sans_extension,excel_string);
end;

procedure page_class.FileDownload
  (
  the_page: page;
  filename: string
  );
begin
  templatecontrol.FileDownload(page,filename);
end;

procedure page_class.Focus
  (
  c: control;
  be_using_scriptmanager: boolean = FALSE
  );
begin
  templatecontrol.Focus(page,c,be_using_scriptmanager);
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
  templatecontrol.RequireConfirmation(c,prompt);
end;

procedure page_class.SessionSet
  (
  name: string;
  value: system.object
  );
begin
  templatecontrol.SessionSet(page,name,value);
end;

function page_class.StringOfControl(c: control): string;
begin
  StringOfControl := templatecontrol.StringOfControl(c);
end;

procedure page_class.TransferToPageBinderTab
  (
  page_nick: string;
  binder_nick: string;
  tab_index: cardinal
  );
begin
  templatecontrol.TransferToPageBinderTab(page,page_nick,binder_nick,tab_index);
end;

procedure page_class.ValidationAlert(be_using_scriptmanager: boolean = FALSE);
begin
  templatecontrol.ValidationAlert(page,be_using_scriptmanager);
end;

//==================================================================================================================================
//
// USERCONTROL_CLASS
//
//==================================================================================================================================

constructor usercontrol_class.Create;
begin
  inherited Create;
  templatecontrol := templatecontrol_class.Create;
end;

function usercontrol_class.AddIdentifiedControlToPlaceHolder
  (
  c: control;
  id: string;
  p: placeholder
  )
  : string;
begin
  AddIdentifiedControlToPlaceHolder := templatecontrol.AddIdentifiedControlToPlaceHolder(c,id,p);
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
  templatecontrol.Alert(page,cause,state,key,value,be_using_scriptmanager);
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
  AlertMessage := templatecontrol.AlertMessage(configurationmanager.appsettings['application_name'],cause,state,key,value);
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
  templatecontrol.EstablishClientSideFunction(page,profile,body,self.clientid);
end;

procedure usercontrol_class.EstablishClientSideFunction(enumeral: client_side_function_enumeral_type);
begin
  templatecontrol.EstablishClientSideFunction(page,enumeral);
end;

procedure usercontrol_class.EstablishClientSideFunction(r: client_side_function_rec_type);
begin
  templatecontrol.EstablishClientSideFunction(page,r);
end;

procedure usercontrol_class.ExportToExcel
  (
  the_page: system.web.ui.page;
  filename_sans_extension: string;
  excel_string: string
  );
begin
  templatecontrol.ExportToExcel(page,filename_sans_extension,excel_string);
end;

procedure usercontrol_class.FileDownload
  (
  the_page: page;
  filename: string
  );
begin
  templatecontrol.FileDownload(page,filename);
end;

procedure usercontrol_class.Focus
  (
  c: control;
  be_using_scriptmanager: boolean = FALSE
  );
begin
  templatecontrol.Focus(page,c,be_using_scriptmanager);
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
  templatecontrol.RequireConfirmation(c,prompt);
end;

procedure usercontrol_class.SessionSet
  (
  name: string;
  value: system.object
  );
begin
  templatecontrol.SessionSet(page,name,value);
end;

function usercontrol_class.StringOfControl(c: control): string;
begin
  StringOfControl := templatecontrol.StringOfControl(c);
end;

procedure usercontrol_class.TransferToPageBinderTab
  (
  page_nick: string;
  binder_nick: string;
  tab_index: cardinal
  );
begin
  templatecontrol.TransferToPageBinderTab(page,page_nick,binder_nick,tab_index);
end;

procedure usercontrol_class.ValidationAlert(be_using_scriptmanager: boolean = FALSE);
begin
  templatecontrol.ValidationAlert(page,be_using_scriptmanager);
end;

end.
