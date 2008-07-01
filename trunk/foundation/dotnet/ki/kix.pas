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
  system.web.ui,
  system.web.ui.htmlcontrols,
  system.Web.UI.WebControls;

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

procedure Alert
  (
  page: page;
  application_name: string;
  cause: alert_cause_type;
  state: alert_state_type;
  key: string;
  s: string
  );

function AverageDeviation
  (
  array_list: arraylist;
  median_value: decimal
  )
  : decimal;

function BeValidDomainPartOfEmailAddress(email_address: string): boolean;

function BooleanOfYesNo(yn: string): boolean;

function Digest(source_string: string): string;

function DomainNameOfIpAddress(ip_address: string): string;

procedure EstablishClientSideFunction
  (
  page: page;
  profile: string;
  body: string;
  usercontrol_clientid: string= ''
  );
  overload;

procedure EstablishClientSideFunction
  (
  page: page;
  enumeral: client_side_function_enumeral_type
  );
  overload;

procedure EstablishClientSideFunction
  (
  page: page;
  r: client_side_function_rec_type
  );
  overload;

function ExpandAsperandBase(s: string): string;

function ExpandTildePath(s: string): string;

procedure ExportToExcel
  (
  page: system.web.ui.page;
  filename_sans_extension: string;
  excel_string: string
  );

procedure FileDownload
  (
  the_page: page;
  filename: string
  );

function Has
  (
  the_string_array: string_array;
  the_string: string
  )
  : boolean;

function Median(sorted_array_list: arraylist): decimal;

function Percentile
  (
  p: cardinal;
  sorted_array_list: arraylist
  )
  : decimal;

procedure RequireConfirmation
  (
  c: webcontrol;
  prompt: string;
  application_name: string
  );

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
  be_html: boolean = FALSE;
  cc: string = '';
  bcc: string = ''
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
  page.clientscript.RegisterStartupScript
    (
    page.GetType,
    key,
    'alert("- - - ---------------------------------------------------- - - -\n'
    +      '       issuer:  \t' + application_name + '\n'
    +      '       cause:   \t' + enum(cause).tostring.tolower + '\n'
    +      '       state:   \t' + enum(state).tostring.tolower + '\n'
    +      '       key:     \t' + key.tolower + '\n'
    +      '       time:    \t' + datetime.Now.tostring('s') + '\n'
    +      '- - - ---------------------------------------------------- - - -\n\n\n'
    +    s.Replace(NEW_LINE,'\n') + '\n\n"'
    + ' );',
    TRUE
    );
end;

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
  be_valid_domain_part_of_email_address: boolean;
begin
  be_valid_domain_part_of_email_address := TRUE;
  try
    dns.GetHostEntry(email_address.Substring(email_address.LastIndexOf('@') + 1));
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

PROCEDURE EstablishClientSideFunction
  (
  page: page;
  profile: string;
  body: string;
  usercontrol_clientid: string = ''
  );
begin
  page.clientscript.RegisterClientScriptBlock
    (
    page.GetType,
    usercontrol_clientid + '__' + profile.Remove(profile.IndexOf(OPEN_PARENTHESIS)),
    'function ' + profile + NEW_LINE
    + ' {' + NEW_LINE
    + ' ' + body.Replace(NEW_LINE,NEW_LINE + SPACE) + NEW_LINE
    + ' }' + NEW_LINE,
    TRUE
    );
end;

PROCEDURE EstablishClientSideFunction
  (
  page: page;
  enumeral: client_side_function_enumeral_type
  );
begin
  case enumeral of
  EL:
    EstablishClientSideFunction(page,'El(id)','return document.getElementById(id);');
  KGS_TO_LBS:
    EstablishClientSideFunction(page,'KgsToLbs(num_kgs)','return Math.round(+num_kgs*2.204622);');
  LBS_TO_KGS:
    EstablishClientSideFunction(page,'LbsToKgs(num_lbs)','return Math.round(+num_lbs/2.204622);');
  end;
end;

procedure EstablishClientSideFunction
  (
  page: page;
  r: client_side_function_rec_type
  );
begin
  EstablishClientSideFunction(page,r.profile,r.body);
end;

FUNCTION ExpandAsperandBase(s: string): string;
begin
  ExpandAsperandBase := s
    .Replace('@',configurationmanager.appsettings['ssl_base_path']);
end;

FUNCTION ExpandTildePath(s: string): string;
begin
  ExpandTildePath := s
    .Replace('\','/')
    .Replace('~','/' + configurationmanager.appsettings['virtual_directory_name']);
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

PROCEDURE FileDownload
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

PROCEDURE RequireConfirmation
  (
  c: webcontrol;
  prompt: string;
  application_name: string
  );
begin
  c.attributes.Add
    (
    'onclick',
    'return confirm("- - - ---------------------------------------------------- - - -\n'
    +               '       issuer:  \t' + application_name + '\n'
    +               '       state:   \twarning\n'
    +               '       time:    \t' + datetime.Now.tostring('s') + '\n'
    +               '- - - ---------------------------------------------------- - - -\n\n\n'
    +               prompt.Replace(NEW_LINE,'\n') + '\n\n"'
    + ');'
    );
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
  msg: mailmessage;
  streamwriter: system.io.streamwriter;
begin
  //
  streamwriter := system.io.streamwriter.Create(scratch_pathname);
  system.web.ui.control(c).RenderControl(system.web.ui.htmltextwriter.Create(streamwriter));
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
    configurationmanager.appsettings['sender_email_address'],
    configurationmanager.appsettings['sender_email_address'],
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
