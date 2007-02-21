unit drop_down_date;

interface

uses System.Data, System.Drawing, System.Web, System.Web.UI,
     System.Web.UI.WebControls, System.Web.UI.HtmlControls;
     
type
    /// <summary>
    /// Summary description for WebUserControl1.
    /// </summary>
  TWebUserControl1 = class(System.Web.UI.UserControl)
  {$REGION 'Designer Managed Code'}
  strict private
    procedure InitializeComponent;
  {$ENDREGION}
  strict private
    procedure Page_Load(sender: System.Object; e: System.EventArgs);
  strict protected
    DropDownList_year: System.Web.UI.WebControls.DropDownList;
    DropDownList_month: System.Web.UI.WebControls.DropDownList;
    DropDownList_day: System.Web.UI.WebControls.DropDownList;
    procedure OnInit(e: System.EventArgs); override;
  private
    { Private Declarations }
  public
    { Public Declarations }
  end;

implementation

procedure TWebUserControl1.Page_Load(sender: System.Object; e: System.EventArgs);
var
  i: cardinal;
begin
  DropDownList_month.items.Add(ListItem.Create('--',''));
  DropDownList_month.items.Add(ListItem.Create('1','Jan'));
  DropDownList_month.items.Add(ListItem.Create('2','Feb'));
  DropDownList_month.items.Add(ListItem.Create('3','Mar'));
  DropDownList_month.items.Add(ListItem.Create('4','Apr'));
  DropDownList_month.items.Add(ListItem.Create('5','May'));
  DropDownList_month.items.Add(ListItem.Create('6','Jun'));
  DropDownList_month.items.Add(ListItem.Create('7','Jul'));
  DropDownList_month.items.Add(ListItem.Create('8','Aug'));
  DropDownList_month.items.Add(ListItem.Create('9','Sep'));
  DropDownList_month.items.Add(ListItem.Create('10','Oct'));
  DropDownList_month.items.Add(ListItem.Create('11','Nov'));
  DropDownList_month.items.Add(ListItem.Create('12','Dec'));
  DropDownList_day.items.Add(ListItem.Create('--',''));
  for i := 1 to 31 do begin
    DropDownList_day.items.Add(ListItem.Create(i.tostring,i.tostring));
  end;
  DropDownList_year.items.Add(ListItem.Create('--',''));
  for i := uint32(session['drop_down_date_min_year']) to uint32(session['drop_down_date_max_year']) do begin
    DropDownList_year.items.Add(ListItem.Create(i.tostring,i.tostring));
  end;
end;

procedure TWebUserControl1.OnInit(e: System.EventArgs);
begin
  //
  // Required for Designer support
  //
  InitializeComponent;
  inherited OnInit(e);
end;

{$REGION 'Designer Managed Code'}
/// <summary>
/// Required method for Designer support -- do not modify
/// the contents of this method with the code editor.
/// </summary>
procedure TWebUserControl1.InitializeComponent;
begin
  Include(Self.Load, Self.Page_Load);
end;
{$ENDREGION}


end.
