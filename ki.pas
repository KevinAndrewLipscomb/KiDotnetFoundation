UNIT ki;

INTERFACE

uses
  borland.data.provider,
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
  system.web.ui,
  system.web.ui.htmlcontrols,
  system.Web.UI.WebControls;

const
  APOSTROPHE = '''';
  DOUBLE_APOSTROPHE = '''''';
  DOUBLE_QUOTE = '""';
  NEW_LINE = #10;
  QUOTE = '"';
  SPACE = #32;
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
  //
  alert_state_type =
    (
    NORMAL,
    SUCCESS,
    WARNING,
    FAILURE,
    DAMAGE
    );
  //
  safe_hint_type =
    (
    NONE,
    ALPHA,
    ALPHANUM,
    CURRENCY_USA,
    DATE_TIME,
    ECMASCRIPT_WORD,
    EMAIL_ADDRESS,
    HOSTNAME,
    HUMAN_NAME,
    HUMAN_NAME_CSV,
    HYPHENATED_ALPHA,
    HYPHENATED_ALPHANUM,
    HYPHENATED_NUM,
    HYPHENATED_UNDERSCORED_ALPHANUM,
    KI_SORT_EXPRESSION,
    MAKE_MODEL,
    NARRATIVE,
    NUM,
    ORG_NAME,
    PHONE_NUM,
    POSTAL_CITY,
    POSTAL_STREET_ADDRESS,
    REAL_NUM,
    REAL_NUM_INCLUDING_NEGATIVE
    );
  string_array = array of string;

procedure Alert
  (
  page: page;
  application_name: string;
  cause: alert_cause_type;
  state: alert_state_type;
  key: string;
  s: string
  );

function BeValidDomainPartOfEmailAddress(email_address: string): boolean;

function BooleanOfYesNo(yn: string): boolean;

function Digest(source_string: string): string;

function DomainNameOfIpAddress(ip_address: string): string;

function ExpandTildePath(s: string): string;

procedure ExportToExcel
  (
  page: system.web.ui.page;
  filename_sans_extension: string;
  excel_string: string
  );

function Has
  (
  the_string_array: string_array;
  the_string: string
  )
  : boolean;

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

procedure SilentAlarm(e: exception);

procedure SmtpMailSend(mail_message: MailMessage); overload;
procedure SmtpMailSend
  (
  from: string;
  &to: string;
  subject: string;
  message_string: string;
  be_html: boolean = FALSE
  );
  overload;

function StringOfControl(c: control): string;

function YesNoOf(b: boolean): string;

IMPLEMENTATION

PROCEDURE Alert
  (
  page: page;
  application_name: string;
  cause: alert_cause_type;
  state: alert_state_type;
  key: string;
  s: string
  );
begin
  if not page.IsStartupScriptRegistered(key) then begin
    page.RegisterStartupScript
      (
      key,
      system.string.EMPTY
      + '<script language="javascript" type="text/javascript">'
      + ' alert("- - - ---------------------------------------------------- - - -\n'
      +         '       issuer:  \t' + application_name + '\n'
      +         '       cause:   \t' + enum(cause).tostring.tolower + '\n'
      +         '       state:   \t' + enum(state).tostring.tolower + '\n'
      +         '       key:     \t' + key.tolower + '\n'
      +         '       time:    \t' + datetime.Now.tostring('s') + '\n'
      +         '- - - ---------------------------------------------------- - - -\n\n\n'
      +    s.Replace(NEW_LINE,'\n') + '\n\n"'
      + ' )'
      + '</script>'
      );
  end;
end;

FUNCTION BeValidDomainPartOfEmailAddress(email_address: string): boolean;
var
  be_valid_domain_part_of_email_address: boolean;
begin
  be_valid_domain_part_of_email_address := TRUE;
  try
    dns.GetHostByName(email_address.Substring(email_address.LastIndexOf('@') + 1));
  except
    be_valid_domain_part_of_email_address := FALSE;
  end;
  BeValidDomainPartOfEmailAddress := be_valid_domain_part_of_email_address;
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
  target_string := system.string.EMPTY;
  byte_buf := sha1managed.Create.ComputeHash(asciiencoding.Create.GetBytes(source_string));
  for i := 1 to 20 do begin
    target_string := target_string + byte_buf[i].tostring('x2');
  end;
  Digest := target_string;
end;

FUNCTION DomainNameOfIpAddress(ip_address: string): string;
begin
  DomainNameOfIpAddress := dns.GetHostByAddress(ip_address).HostName;
end;

FUNCTION ExpandTildePath(s: string): string;
begin
  ExpandTildePath := s
    .Replace('\','/')
    .Replace('~','/' + configurationsettings.appsettings['virtual_directory_name']);
end;

PROCEDURE ExportToExcel
  (
  page: system.web.ui.page;
  filename_sans_extension: string;
  excel_string: string
  );
begin
  page.response.Clear;
  page.response.AppendHeader
    (
    'Content-Disposition',
    'attachment; filename=' + filename_sans_extension + '.xls'
    );
  page.response.bufferoutput := TRUE;
  page.response.contenttype := 'application/vnd.ms-excel';
  page.enableviewstate := FALSE;
  page.response.Write(excel_string);
  page.response.&End;
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
  len := system.array(the_string_array).length;
  i := 0;
  while (i < len) and (the_string_array[i] <> the_string) do begin
    i := i + 1;
  end;
  Has := (i < len);
end;

FUNCTION Safe
  (
  source_string: string;
  hint: safe_hint_type = NONE
  )
  : string;
var
  allow: string;
  scratch_string: string;
begin
  allow := system.string.EMPTY;
  case hint of
    //
    // Be extremely protective here:
    // -  Escape ("\") the following twelve characters: [\^$.|?*+()-
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
      allow := '0-9\$\,\.';
    DATE_TIME:
      allow := '0-9:\-\/ ';
    ECMASCRIPT_WORD:
      allow := '0-9a-zA-Z_';
    EMAIL_ADDRESS:
      allow := '0-9a-zA-Z_\.@\-';
    HOSTNAME:
      allow := '0-9a-zA-Z_\-\.';
    HUMAN_NAME:
      allow := 'a-zA-Z\-\. ' + APOSTROPHE;
    HUMAN_NAME_CSV:
      allow := 'a-zA-Z\-,\. ' + APOSTROPHE;
    HYPHENATED_ALPHA:
      allow := 'a-zA-z\-';
    HYPHENATED_ALPHANUM:
      allow := 'a-zA-z0-9\-';
    HYPHENATED_NUM:
      allow := '0-9\-';
    HYPHENATED_UNDERSCORED_ALPHANUM:
      allow := 'a-zA-z0-9\-_';
    KI_SORT_EXPRESSION:
      allow := 'a-zA-z%,\. ';
    MAKE_MODEL:
      allow := '0-9a-zA-Z#\*\(\)\-\+/\. ';
    NARRATIVE:
      allow := '0-9a-zA-Z#\(\)\-,/\. ';
    NUM:
      allow := '0-9';
    ORG_NAME:
      allow := '0-9a-zA-Z#&\-,\. ' + APOSTROPHE;
    PHONE_NUM:
      allow := '0-9-\+\(\) ';
    POSTAL_CITY:
      allow := 'a-zA-Z\-\. ' + APOSTROPHE;
    POSTAL_STREET_ADDRESS:
      allow := '0-9a-zA-Z#\-,\(\)\. ' + APOSTROPHE;
    REAL_NUM:
      allow := '0-9\.';
    REAL_NUM_INCLUDING_NEGATIVE:
      allow := '0-9\.\-';
  end;
  //
  if allow = system.string.EMPTY then
    scratch_string := system.string.EMPTY
  else
    begin
    scratch_string := regex.Replace(source_string,'[^' + allow + ']',system.string.EMPTY);
    regex.Replace(scratch_string,APOSTROPHE,DOUBLE_APOSTROPHE);
    regex.Replace(scratch_string,QUOTE,DOUBLE_QUOTE);
    regex.Replace(scratch_string,';',':,');
    end;
  //
  Safe := scratch_string;
end;

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
var
  msg: system.web.mail.mailmessage;
  streamwriter: system.io.streamwriter;
begin
  //
  streamwriter := system.io.streamwriter.Create(scratch_pathname);
  system.web.ui.control(c).RenderControl(system.web.ui.htmltextwriter.Create(streamwriter));
  streamwriter.Close;
  //
  msg := system.web.mail.mailmessage.Create;
  msg.from := from_address;
  msg.&to := to_target;
  msg.cc := cc_target;
  msg.subject := subject;
  msg.body := body;
  msg.attachments.Add(system.web.mail.mailattachment.Create(scratch_pathname));
  //
  SmtpMailSend(msg);
  //
  &file.Delete(scratch_pathname);
  //
end;

function StringOfControl(c: control): string;
var
  stringwriter: system.io.stringwriter;
begin
  stringwriter := system.io.stringwriter.Create;
  c.RenderControl(system.web.ui.htmltextwriter.Create(stringwriter));
  StringOfControl := stringwriter.tostring;
end;

PROCEDURE SilentAlarm(e: exception);
begin
  SmtpMailSend
    (
    configurationsettings.appsettings['sender_email_address'],
    configurationsettings.appsettings['sender_email_address'],
    'SILENT ALARM',
    '[MESSAGE]' + NEW_LINE
    + e.message + NEW_LINE
    + NEW_LINE
    + '[STACKTRACE]' + NEW_LINE
    + e.stacktrace + NEW_LINE
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
  smtp_server := ConfigurationSettings.AppSettings['smtp_server'];
  smtp_username := ConfigurationSettings.AppSettings['smtp_username'];
  smtp_password := ConfigurationSettings.AppSettings['smtp_password'];
  with mail_message do begin
    fields.Add('http://schemas.microsoft.com/cdo/configuration/smtpserver',smtp_server);
    fields.Add('http://schemas.microsoft.com/cdo/configuration/smtpserverport',system.object(25));
    fields.Add('http://schemas.microsoft.com/cdo/configuration/sendusing',system.object(CDO_SEND_USING_PORT));
    if (smtp_username <> system.string.Empty) and (smtp_password <> system.string.Empty) then begin
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
  be_html: boolean = FALSE
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
    mail_message.bodyformat := system.web.mail.mailformat.HTML;
  end;
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
