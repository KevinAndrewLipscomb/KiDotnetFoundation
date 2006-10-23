unit ki.web.ui;

interface

uses
  ki.common,
  system.web.ui,
  system.web.ui.htmlcontrols;

type
  Page = class(System.Web.UI.Page)
  private
    the_form: htmlform;
    typeof_htmlform: system.type;
  strict protected
    function GetServerForm
      (
      parent: controlcollection;
      var server_form: htmlform
      )
      : boolean;
    procedure OnInit(e: eventargs); override;
    procedure OnPreRender(e: eventargs); override;
  public
    constructor Create;
  end;

implementation

constructor Page.Create;
begin
  inherited Create;
  // TODO: Add any constructor code here
  the_form := htmlform.Create;
  typeof_htmlform := htmlform.Create.GetType;
end;

function Page.GetServerForm
  (
  parent: controlcollection;
  var server_form: htmlform
  )
  : boolean;
var
  found: boolean;
  i: cardinal;
begin
  found := FALSE;
  i := 0;
  while (i < cardinal(parent.Count)) and (not found) do begin
    if parent[i].GetType = typeof_htmlform then begin
      server_form := htmlform(parent[i]);
      found := TRUE;
    end else if parent[i].HasControls then begin
      found := GetServerForm(parent[i].controls,server_form);
    end;
    i := i + 1;
  end;
  GetServerForm := found;
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

procedure Page.OnPreRender(e: eventargs);
const
  MONIKER_HORIZONTAL = 'smartscroll_x';
  MONIKER_VERTICAL = 'smartscroll_y';
var
  dummy_boolean: boolean;
  hdnLeft: htmlinputhidden;
  hdnTop: htmlinputhidden;
  leftValue: string;
  topValue: string;
begin
  dummy_boolean := GetServerForm(Page.Controls,the_form);

  topValue := Request.Form[MONIKER_VERTICAL];
  leftValue := Request.Form[MONIKER_HORIZONTAL];

  if (topValue = system.string.EMPTY) or (leftValue = system.string.EMPTY) then begin
    topValue := '0';
    leftValue := '0';
  end;

  hdnLeft := HtmlInputHidden.Create;
  hdnLeft.ID := MONIKER_HORIZONTAL;
  hdnLeft.Name := MONIKER_HORIZONTAL;
  hdnLeft.Value := leftValue;
  the_form.Controls.Add(hdnLeft);


  hdnTop := HtmlInputHidden.Create;
  hdnTop.ID := MONIKER_VERTICAL;
  hdnTop.Name := MONIKER_VERTICAL;
  hdnTop.Value := topValue;
  the_form.Controls.Add(hdnTop);

  page.RegisterStartupScript
    (
    'SmartScroller',
    '<!-- SmartScroller ASP.NET Generated Code -->' + NEW_LINE
    + '  <script language = "javascript">' + NEW_LINE
    + '    <!--' + NEW_LINE
    + '    function SmartScroller_GetCoords()' + NEW_LINE
    + '      {' + NEW_LINE
    + '      var scrollX, scrollY;' + NEW_LINE
    + '      if (document.all)' + NEW_LINE
    + '        {' + NEW_LINE
    + '        if (!document.documentElement.scrollLeft)' + NEW_LINE
    + '          scrollX = document.body.scrollLeft;' + NEW_LINE
    + '        else' + NEW_LINE
    + '          scrollX = document.documentElement.scrollLeft;' + NEW_LINE
    + '        if (!document.documentElement.scrollTop)' + NEW_LINE
    + '          scrollY = document.body.scrollTop;' + NEW_LINE
    + '        else' + NEW_LINE
    + '          scrollY = document.documentElement.scrollTop;' + NEW_LINE
    + '        }' + NEW_LINE
    + '      else' + NEW_LINE
    + '        {' + NEW_LINE
    + '        scrollX = window.pageXOffset;' + NEW_LINE
    + '        scrollY = window.pageYOffset;' + NEW_LINE
    + '        }' + NEW_LINE
    + '      document.forms["' + the_form.clientid + '"].' + hdnLeft.clientiD + '.value = scrollX;' + NEW_LINE
    + '      document.forms["' + the_form.clientid + '"].' + hdnTop.clientiD + '.value = scrollY;' + NEW_LINE
    + '      }' + NEW_LINE
    + '    function SmartScroller_Scroll()' + NEW_LINE
    + '      {' + NEW_LINE
    + '      var x = document.forms["' + the_form.clientid + '"].' + hdnLeft.clientiD + '.value;' + NEW_LINE
    + '      var y = document.forms["' + the_form.clientid + '"].' + hdnTop.clientiD + '.value;' + NEW_LINE
    + '      window.scrollTo(x, y);' + NEW_LINE
    + '      }' + NEW_LINE
    + '    window.onload = SmartScroller_Scroll;' + NEW_LINE
    + '    window.onscroll = SmartScroller_GetCoords;' + NEW_LINE
    + '    window.onclick = SmartScroller_GetCoords;' + NEW_LINE
    + '    window.onkeypress = SmartScroller_GetCoords;' + NEW_LINE
    + '    // -->' + NEW_LINE
    + '  </script>' + NEW_LINE
    + '<!-- End SmartScroller ASP.NET Generated Code -->' + NEW_LINE
    );

    inherited OnPreRender(e);
end;

end.
