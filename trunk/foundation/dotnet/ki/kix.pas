UNIT kix;

INTERFACE

uses
  system.collections,
  system.configuration,
  system.data,
  system.io,
  system.net,
  system.security.cryptography,
  system.text,
  system.text.regularexpressions,
  system.web,
  system.web.mail,
  system.web.security,
  system.web.sessionstate;

const
  ACUTE_ACCENT = #180;  //´
  APOSTROPHE = '''';
  CENT_SIGN = #155;  //¢
  COMMA = ',';
  COMMA_SPACE = ', ';
  DIAERESIS = #168;  //¨
  DOUBLE_APOSTROPHE = '''''';
  DOUBLE_QUOTE = '""';
  EMPTY = '';
  HYPHEN = '-';
  INVERTED_EXCLAMATION_MARK = #161;  //¡
  NEW_LINE = #10;
  OPEN_PARENTHESIS = '(';
  PERIOD = '.';
  QUOTE = '"';
  SEMICOLON = ';';
  SPACE = #32;
  SPACE_HYPHEN_SPACE = ' - ';
  SPACE_HYPHENS_SPACE = ' -- ';
  TAB = #9;

type
  alert_cause_type =
    (
    APPDATA,
    DBMS,
    FILESYSTEM,
    LOGIC,
    MEMORY,
    NETWORK,
    OTHER,
    USER
    );
  alert_state_type =
    (
    NORMAL,
    SUCCESS,
    WARNING,
    FAILURE,
    DAMAGE
    );
  client_side_function_enumeral_type =
    (
    EL,
    KGS_TO_LBS,
    LBS_TO_KGS
    );
  client_side_function_rec_type =
    RECORD
    profile: string;
    body: string;
    END;
  safe_hint_type =
    (
    NONE,
    ALPHA,
    ALPHANUM,
    CURRENCY_USA,
    DATE_TIME,
    ECMASCRIPT_WORD,
    EMAIL_ADDRESS,
    FINANCIAL_TERMS,
    HOSTNAME,
    HTTP_TARGET,
    HUMAN_NAME,
    HUMAN_NAME_CSV,
    HYPHENATED_ALPHA,
    HYPHENATED_ALPHANUM,
    HYPHENATED_NUM,
    HYPHENATED_UNDERSCORED_ALPHANUM,
    KI_SORT_EXPRESSION,
    MAKE_MODEL,
    MEMO,
    NUM,
    ORG_NAME,
    PHONE_NUM,
    POSTAL_CITY,
    POSTAL_STREET_ADDRESS,
    PUNCTUATED,
    REAL_NUM,
    REAL_NUM_INCLUDING_NEGATIVE
    );
  string_array = array of string;

function AverageDeviation
  (
  array_list: arraylist;
  median_value: decimal
  )
  : decimal;

function BeValidDomainPartOfEmailAddress(email_address: string): boolean;

function BeValidDomainPartOfWebAddress(web_address: string): boolean;

function BooleanOfYesNo(yn: string): boolean;

function Digest(source_string: string): string;

function DomainNameOfIpAddress(ip_address: string): string;

function EscalatedException
  (
  the_exception: exception;
  user_identity_name: string = EMPTY;
  session: httpsessionstate = NIL
  )
  : string;

function ExpandAsperand(s: string): string;

function ExpandTildePath(s: string): string;

function Has
  (
  the_string_array: string_array;
  the_string: string
  )
  : boolean;

function HresultAnalysis(the_exception: exception): string;

function Median(sorted_array_list: arraylist): decimal;

function Percentile
  (
  p: cardinal;
  sorted_array_list: arraylist
  )
  : decimal;

function Safe
  (
  source_string: string;
  hint: safe_hint_type = NONE
  )
  : string;

procedure SendControlAsAttachmentToEmailMessage
  (
  c: system.object;
  scratch_pathname: string;
  from_address: string;
  to_target: string;
  cc_target: string;
  subject: string;
  body: string
  );

procedure SilentAlarm(the_exception: exception);

procedure SmtpMailSend(mail_message: MailMessage); overload;
procedure SmtpMailSend
  (
  from: string;
  &to: string;
  subject: string;
  message_string: string;
  be_html: boolean = FALSE;
  cc: string = '';
  bcc: string = ''
  );
  overload;

function YesNoOf(b: boolean): string;

IMPLEMENTATION

uses
  system.diagnostics,
  system.runtime.interopservices,
  system.web.ui;

FUNCTION AverageDeviation
  (
  array_list: arraylist;
  median_value: decimal
  )
  : decimal;
var
  i: cardinal;
  n: cardinal;
  sum: decimal;
begin
  n := array_list.Count;
  for i := 0 to (n - 1) do begin
    sum := sum + math.Abs(decimal(array_list[i]) - median_value);
  end;
  AverageDeviation := sum/n;
end;

FUNCTION BeValidDomainPartOfEmailAddress(email_address: string): boolean;
var
  process_start_info: processstartinfo;
  significant_part: string;
  work: process;
begin
  //
  significant_part := email_address.Substring(email_address.LastIndexOf('@') + 1);
  //
  process_start_info := processstartinfo.Create('nslookup','-type=MX ' + significant_part.Trim);
  process_start_info.RedirectStandardOutput := TRUE;
  process_start_info.RedirectStandardError := TRUE;
  process_start_info.UseShellExecute := FALSE;
  //
  work := process.Start(process_start_info);
  work.WaitForExit;
  work.Refresh;
  //
  BeValidDomainPartOfEmailAddress := work.StandardOutput.ReadToEnd.Contains('mail exchanger = ');
  //
end;

FUNCTION BeValidDomainPartOfWebAddress(web_address: string): boolean;
const
  LENGTH_OF_SHORTEST_PRACTICAL_DOMAIN_NAME = 4;  // a.com
var
  be_valid_domain_part_of_web_address: boolean;
  index_of_slash: integer;
  significant_length: integer;
begin
  be_valid_domain_part_of_web_address := TRUE;
  index_of_slash := web_address.IndexOf('/');
  if index_of_slash > LENGTH_OF_SHORTEST_PRACTICAL_DOMAIN_NAME then begin
    significant_length := index_of_slash
  end else begin
    significant_length := web_address.length;
  end;
  try
    dns.GetHostEntry(web_address.Substring(0,significant_length));
  except
    be_valid_domain_part_of_web_address := FALSE;
  end;
  BeValidDomainPartOfWebAddress := be_valid_domain_part_of_web_address;
end;

function BooleanOfYesNo(yn: string): boolean;
begin
  BooleanOfYesNo := (yn.ToUpper = 'YES');
end;

FUNCTION Digest(source_string: string): string;
var
  byte_buf: array[1..20] of byte;
  i: cardinal;
  target_string: string;
begin
  target_string := EMPTY;
  byte_buf := sha1managed.Create.ComputeHash(asciiencoding.Create.GetBytes(source_string));
  for i := 1 to 20 do begin
    target_string := target_string + byte_buf[i].tostring('x2');
  end;
  Digest := target_string;
end;

FUNCTION DomainNameOfIpAddress(ip_address: string): string;
begin
  DomainNameOfIpAddress := dns.GetHostEntry(ip_address).HostName;
end;

FUNCTION EscalatedException
  (
  the_exception: exception;
  user_identity_name: string = EMPTY;
  session: httpsessionstate = NIL
  )
  : string;
var
  notification_message: string;
  lcv: cardinal;
  user_designator: string;
begin
  //
  if user_identity_name = EMPTY then begin
    user_designator := 'unknown';
  end else begin
    user_designator := user_identity_name;
  end;
  //
  notification_message := EMPTY
  + '[EXCEPTION]' + NEW_LINE
  + the_exception.tostring + NEW_LINE
  + NEW_LINE
  + '[HRESULT]' + NEW_LINE
  + HresultAnalysis(the_exception) + NEW_LINE
  + NEW_LINE;
  //
  if user_identity_name <> EMPTY then begin
    notification_message := notification_message
    + '[USER]' + NEW_LINE
    + user_designator + NEW_LINE
    + NEW_LINE;
  end;
  //
  if assigned(session) then begin
    notification_message := notification_message
    + '[SESSION]' + NEW_LINE;
    if session.count > 0 then begin
      for lcv := 0 to (session.count - 1) do begin
        notification_message := notification_message + session.keys[lcv].tostring + ' = ' + session.item[lcv].tostring + NEW_LINE;
      end;
    end;
  end;
  //
  SmtpMailSend
    (
    configurationmanager.appsettings['sender_email_address'],
    configurationmanager.appsettings['sender_email_address'],
    'EXCEPTION REPORT',
    notification_message
    );
  SmtpMailSend
    (
    configurationmanager.appsettings['sender_email_address'],
    configurationmanager.appsettings['sysadmin_sms_address'],
    'CRASH',
    user_identity_name
    );
  //
  EscalatedException := notification_message;
end;

FUNCTION ExpandAsperand(s: string): string;
begin
  ExpandAsperand := s.Replace('@',configurationmanager.appsettings['runtime_root_fullspec']);
end;

FUNCTION ExpandTildePath(s: string): string;
begin
  ExpandTildePath := s
    .Replace('\','/')
    .Replace('~','/' + configurationmanager.appsettings['virtual_directory_name']);
end;

FUNCTION Has
  (
  the_string_array: string_array;
  the_string: string
  )
  : boolean;
var
  i: cardinal;
  len: cardinal;
begin
  Has := FALSE;
  if the_string_array <> nil then begin
    len := system.array(the_string_array).length;
    i := 0;
    while (i < len) and (the_string_array[i] <> the_string) do begin
      i := i + 1;
    end;
    Has := (i < len);
  end;
end;

FUNCTION HresultAnalysis(the_exception: exception): string;
var
  code: string;
  facility: string;
  facility_id: uint32;
  hresult: uint32;
  be_ntstatus: boolean;
  microsoft_or_not: string;
  severity: string;
begin
  facility := 'unknown';
  hresult := uint32(marshal.GetHrForException(the_exception));
  be_ntstatus := boolean(hresult and $10000000);
  code := uint16(hresult mod $00010000).tostring('X');
  if not be_ntstatus then begin // "N": it's not an NTSTATUS
    if boolean(hresult and $80000000) then begin // "S"
      severity := 'FAILURE';
    end else begin
      severity := 'SUCCESS';
    end;
  end else begin
    case hresult div $C0000000 of // NTSTATUS "Sev"
    3: severity := 'ERROR';
    2: severity := 'WARNING';
    1: severity := 'INFO';
    0: severity := 'SUCCESS';
    end;
  end;
  if boolean(hresult and $20000000) then begin // "C"
    microsoft_or_not := 'NONMICROSOFT';
  end else begin
    microsoft_or_not := 'MICROSOFT';
    facility_id := (hresult mod $10000000) div $00010000;
    if not be_ntstatus then begin
      case facility_id of
      00: facility := 'NULL';
      01: facility := 'RPC';
      02: facility := 'DISPATCH';
      03: facility := 'STORAGE';
      04: facility := 'ITF';
      07: facility := 'WIN32';
      08: facility := 'WINDOWS';
      09: facility := 'SECURITY|SSPI';
      10: facility := 'CONTROL';
      11: facility := 'CERT';
      12: facility := 'INTERNET';
      13: facility := 'MEDIASERVER';
      14: facility := 'MSMQ';
      15: facility := 'SETUPAPI';
      16: facility := 'SCARD';
      17: facility := 'COMPLUS';
      18: facility := 'AAF';
      19: facility := 'URT';
      20: facility := 'ACS';
      21: facility := 'DPLAY';
      22: facility := 'UMI';
      23: facility := 'SXS';
      24: facility := 'WINDOWS_CE';
      25: facility := 'HTTP';
      26: facility := 'USERMODE_COMMONLOG';
      31: facility := 'USERMODE_FILTER_MANAGER';
      32: facility := 'BACKGROUNDCOPY';
      33: facility := 'CONFIGURATION';
      34: facility := 'STATE_MANAGEMENT';
      35: facility := 'METADIRECTORY';
      36: facility := 'WINDOWSUPDATE';
      37: facility := 'DIRECTORYSERVICE';
      38: facility := 'GRAPHICS';
      39: facility := 'SHELL';
      40: facility := 'TPM_SERVICES';
      41: facility := 'TPM_SOFTWARE';
      48: facility := 'PLA';
      49: facility := 'FVE';
      50: facility := 'FWP';
      51: facility := 'WINRM';
      52: facility := 'NDIS';
      53: facility := 'USERMODE_HYPERVISOR';
      54: facility := 'CMI';
      80: facility := 'WINDOWS_DEFENDER';
      end;
    end else begin // is an NTSTATUS
      case facility_id of
      01: facility := 'DEBUGGER';
      02: facility := 'RPC_RUNTIME';
      03: facility := 'RPC_STUBS';
      04: facility := 'IO';
      07: facility := 'NTWIN32';
      09: facility := 'NTSSPI';
      10: facility := 'TERMINAL_SERVER';
      11: facility := 'MUI';
      16: facility := 'USB';
      17: facility := 'HID';
      18: facility := 'FIREWIRE';
      19: facility := 'CLUSTER';
      20: facility := 'ACPI';
      21: facility := 'SXS';
      25: facility := 'TRANSACTION';
      26: facility := 'COMMONLOG';
      27: facility := 'VIDEO';
      28: facility := 'FILTER_MANAGER';
      29: facility := 'MONITOR';
      30: facility := 'GRAPHICS_KERNEL';
      32: facility := 'DRIVER_FRAMEWORK';
      33: facility := 'FVE';
      34: facility := 'FWP';
      35: facility := 'NDIS';
      53: facility := 'HYPERVISOR';
      54: facility := 'IPSEC';
      55: facility := 'MAXIMUM_VALUE';
      end;
    end;
  end;
  HResultAnalysis := '0x' + hresult.tostring('X') + ':  %' + microsoft_or_not + '.' + facility + '--' + severity + '--0x' + code;
end;

FUNCTION Median(sorted_array_list: arraylist): decimal;
begin
  Median := Percentile(50,sorted_array_list);
end;

FUNCTION Percentile
  (
  p: cardinal;
  sorted_array_list: arraylist
  )
  : decimal;
var
  interpolation_factor: decimal;
  practical_index: integer;
  lower_value: decimal;
  n: cardinal;
  virtual_index: decimal;
begin
  n := sorted_array_list.Count;
  if n = 0 then begin
    Percentile := 0;
  end else begin
    if n = 1 then begin
      Percentile := decimal(sorted_array_list[0]);
    end else begin
      virtual_index := p*(n - 1)/100;
      if virtual_index >= (n - 1) then begin
        Percentile := decimal(sorted_array_list[n - 1]);
      end else begin
        practical_index := decimal.ToInt32(decimal.Floor(virtual_index));
        interpolation_factor := virtual_index - practical_index;
        lower_value := decimal(sorted_array_list[practical_index]);
        Percentile := lower_value + interpolation_factor*(decimal(sorted_array_list[practical_index + 1]) - lower_value);
      end;
    end;
  end;
end;

FUNCTION Safe
  (
  source_string: string;
  hint: safe_hint_type = NONE
  )
  : string;
const
  MODIFIED_LIBERAL_SET = '0-9a-zA-Z@#$%&()\-+=,/. ' + ACUTE_ACCENT + CENT_SIGN + DIAERESIS + INVERTED_EXCLAMATION_MARK;
var
  allow: string;
  scratch_string: string;
begin
  allow := EMPTY;
  case hint of
  //
  // Be extremely protective here:
  // -  Escape ("\") the following four characters: ]\^-
  // -  For scalars, do not allow punctuation.
  // -  When in doubt, don't allow it.
  //
  // This routine is not intended to assure that data is submitted in proper
  // format.  It is intended to protect against SQL insertion attacks.
  //
  ALPHA:
    allow := 'a-zA-Z';
  ALPHANUM:
    allow := '0-9a-zA-Z';
  CURRENCY_USA:
    allow := '0-9$,.';
  DATE_TIME:
    allow := '0-9:\-/ ';
  ECMASCRIPT_WORD:
    allow := '0-9a-zA-Z_';
  EMAIL_ADDRESS:
    allow := '0-9a-zA-Z_.@\-';
  FINANCIAL_TERMS:
    allow := '0-9a-zA-Z@#$%()\-+=,/. ' + CENT_SIGN;
  HOSTNAME:
    allow := '0-9a-zA-Z_\-.';
  HTTP_TARGET:
    allow := '0-9a-zA-Z\-_./';
  HUMAN_NAME:
    allow := 'a-zA-Z\-. ' + ACUTE_ACCENT;
  HUMAN_NAME_CSV:
    allow := 'a-zA-Z\-,. ' + ACUTE_ACCENT;
  HYPHENATED_ALPHA:
    allow := 'a-zA-z\-';
  HYPHENATED_ALPHANUM:
    allow := 'a-zA-z0-9\-';
  HYPHENATED_NUM:
    allow := '0-9\-';
  HYPHENATED_UNDERSCORED_ALPHANUM:
    allow := 'a-zA-z0-9\-_';
  KI_SORT_EXPRESSION:
    allow := 'a-zA-z%,. ';
  MAKE_MODEL:
    allow := '0-9a-zA-Z#*()\-+/. ';
  MEMO:
    allow := MODIFIED_LIBERAL_SET + '\n\r\t';
  NUM:
    allow := '0-9';
  ORG_NAME:
    allow := '0-9a-zA-Z#&\-,. ' + ACUTE_ACCENT;
  PHONE_NUM:
    allow := '0-9-+() ';
  POSTAL_CITY:
    allow := 'a-zA-Z\-. ' + ACUTE_ACCENT;
  POSTAL_STREET_ADDRESS:
    allow := '0-9a-zA-Z#\-,(). ' + ACUTE_ACCENT;
  PUNCTUATED:
    allow := MODIFIED_LIBERAL_SET;
  REAL_NUM:
    allow := '0-9.';
  REAL_NUM_INCLUDING_NEGATIVE:
    allow := '0-9.\-';
  end;
  //
  if allow = EMPTY then begin
    scratch_string := EMPTY;
  end else begin
    scratch_string := source_string;
    scratch_string := scratch_string.Replace(QUOTE,DIAERESIS);
    scratch_string := scratch_string.Replace(APOSTROPHE,ACUTE_ACCENT);
    scratch_string := scratch_string.Replace(SEMICOLON,INVERTED_EXCLAMATION_MARK);
    scratch_string := regex.Replace(scratch_string,'[^' + allow + ']',EMPTY);
  end;
  //
  Safe := scratch_string;
end;

PROCEDURE SendControlAsAttachmentToEmailMessage
  (
  c: system.object;
  scratch_pathname: string;
  from_address: string;
  to_target: string;
  cc_target: string;
  subject: string;
  body: string
  );
var
  msg: mailmessage;
  streamwriter: system.io.streamwriter;
begin
  //
  streamwriter := system.io.streamwriter.Create(scratch_pathname);
  control(c).RenderControl(system.web.ui.htmltextwriter.Create(streamwriter));
  streamwriter.Close;
  //
  msg := mailmessage.Create;
  msg.from := from_address;
  msg.&to := to_target;
  msg.cc := cc_target;
  msg.subject := subject;
  msg.body := body;
  msg.attachments.Add(mailattachment.Create(scratch_pathname));
  //
  SmtpMailSend(msg);
  //
  &file.Delete(scratch_pathname);
  //
end;

PROCEDURE SilentAlarm(the_exception: exception);
begin
  SmtpMailSend
    (
    configurationmanager.appsettings['sender_email_address'],
    configurationmanager.appsettings['sender_email_address'],
    'SILENT ALARM',
    '[EXCEPTION]' + NEW_LINE
    + the_exception.tostring + NEW_LINE
    + NEW_LINE
    + '[HRESULT]' + NEW_LINE
    + HresultAnalysis(the_exception) + NEW_LINE
    );
end;

procedure SmtpMailSend(mail_message: MailMessage);
const
  CDO_BASIC = 1;
  CDO_SEND_USING_PORT = 2;
var
  smtp_password: string;
  smtp_server: string;
  smtp_username: string;
begin
  smtp_server := configurationmanager.appsettings['smtp_server'];
  smtp_username := configurationmanager.appsettings['smtp_username'];
  smtp_password := configurationmanager.appsettings['smtp_password'];
  with mail_message do begin
    fields.Add('http://schemas.microsoft.com/cdo/configuration/smtpserver',smtp_server);
    fields.Add('http://schemas.microsoft.com/cdo/configuration/smtpserverport',system.object(25));
    fields.Add('http://schemas.microsoft.com/cdo/configuration/sendusing',system.object(CDO_SEND_USING_PORT));
    if (smtp_username <> EMPTY) and (smtp_password <> EMPTY) then begin
      fields.Add('http://schemas.microsoft.com/cdo/configuration/smtpauthenticate',system.object(CDO_BASIC));
      fields.Add('http://schemas.microsoft.com/cdo/configuration/sendusername',smtp_username);
      fields.Add('http://schemas.microsoft.com/cdo/configuration/sendpassword',smtp_password);
    end;
  end;
  smtpmail.smtpserver := smtp_server;
  try
    smtpmail.Send(mail_message);
  except
    on e: exception do SilentAlarm(e);
  end;
end;

procedure SmtpMailSend
  (
  from: string;
  &to: string;
  subject: string;
  message_string: string;
  be_html: boolean = FALSE;
  cc: string = '';
  bcc: string = ''
  );
var
  mail_message: mailmessage;
begin
  mail_message := mailmessage.Create;
  mail_message.from := from;
  mail_message.&to := &to;
  mail_message.subject := subject;
  mail_message.body := message_string;
  if be_html then begin
    mail_message.bodyformat := mailformat.HTML;
  end;
  mail_message.cc := cc;
  mail_message.bcc := bcc;
  SmtpMailSend(mail_message);
end;

function YesNoOf(b: boolean): string;
begin
  if b then begin
    YesNoOf := 'Yes';
  end else begin
    YesNoOf := 'No';
  end;
end;

END.
