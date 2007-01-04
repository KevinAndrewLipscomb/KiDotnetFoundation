UNIT ki;

INTERFACE

uses
  borland.data.provider,
  system.configuration,
  system.data,
  system.io,
  system.security.cryptography,
  system.text,
  system.text.regularexpressions,
  system.web,
  system.web.mail,
  system.web.security,
  system.web.ui,
  system.web.ui.htmlcontrols,
  system.Web.UI.WebControls;

const ID = '$Id$';

const
  APOSTROPHE = '''';
  DOUBLE_APOSTROPHE = '''''';
  DOUBLE_QUOTE = '""';
  NEW_LINE = #10;
  QUOTE = '"';
  SPACE = #32;
  TAB = #9;

type
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

function Digest(source_string: string): string;

procedure ExportToExcel
  (
  page: system.web.ui.page;
  filename_sans_extension: string;
  excel_string: string
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

procedure SmtpMailSend(mail_message: MailMessage); overload;
procedure SmtpMailSend
  (
  from: string;
  &to: string;
  subject: string;
  message_text: string
  );
  overload;

function StringOfControl(c: control): string;

IMPLEMENTATION

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

procedure ExportToExcel
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
      allow := '0-9AMPamp:\/ ';
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
  smtpmail.Send(mail_message);
end;

procedure SmtpMailSend
  (
  from: string;
  &to: string;
  subject: string;
  message_text: string
  );
var
  mail_message: mailmessage;
begin
  mail_message := mailmessage.Create;
  mail_message.from := from;
  mail_message.&to := &to;
  mail_message.subject := subject;
  mail_message.body := message_text;
  SmtpMailSend(mail_message);
end;

END.
