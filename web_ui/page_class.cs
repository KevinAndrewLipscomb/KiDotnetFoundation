using kix;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ki.web_ui
  {
  public class page_class : Page
    {

    protected readonly static string APP_HANDLED_ASYNC_POST_BACK_ERROR_MESSAGE_HEADER_VALUE = common.APP_HANDLED_ASYNC_POST_BACK_ERROR_MESSAGE_HEADER_VALUE;
    protected readonly static string APP_HANDLED_ASYNC_POST_BACK_ERROR_MESSAGE_MARK = common.APP_HANDLED_ASYNC_POST_BACK_ERROR_MESSAGE_MARK;
    protected readonly static string CUSTOM_RESPONSE_HEADER_NAME = common.CUSTOM_RESPONSE_HEADER_NAME;
    protected readonly static string SESSION_INTERRUPTED_HEADER_VALUE = common.SESSION_INTERRUPTED_HEADER_VALUE;

    public enum nature_of_visit_type
      {
      VISIT_COLD_CALL,
      VISIT_INITIAL,
      VISIT_POSTBACK_STANDARD,
      VISIT_POSTBACK_STALE,
      }

    private readonly templatecontrol_class templatecontrol = null;

    public page_class() : base() // CONSTRUCTOR
      {
      templatecontrol = new templatecontrol_class();
      }

    public void AddCopyFromFeatureToControl
      (
      string instance_function_name,
      Dictionary<WebControl,WebControl> target_source_dictionary,
      WebControl c
      )
      {
      templatecontrol.AddCopyFromFeatureToControl
        (
        the_page:Page,
        instance_function_name:instance_function_name,
        target_source_dictionary:target_source_dictionary,
        c:c
        );
      }

    protected string AddIdentifiedControlToPlaceHolder
      (
      Control c,
      string id,
      PlaceHolder p,
      string instance_context_id_for_freshening = k.EMPTY
      )
      {
      return templatecontrol.AddIdentifiedControlToPlaceHolder(Page,c,id,p,instance_context_id_for_freshening);
      }

    protected void Alert
      (
      k.alert_cause_type cause,
      k.alert_state_type state,
      string key,
      string value,
      bool be_using_scriptmanager = false // Ideally the default should be true, but for backward compatibility (particularly in WebEMSOF) we default to false.
      )
      {
      templatecontrol.Alert(Page,cause,state,key,value,be_using_scriptmanager);
      }

    protected void AlertAndBackTrack
      (
      k.alert_cause_type cause,
      k.alert_state_type state,
      string key,
      string value,
      uint num_backsteps = 1
      )
      {
      var script = "alert(\"" + templatecontrol.AlertMessage(ConfigurationManager.AppSettings["application_name"],cause,state,key,value).Replace(Convert.ToString(k.NEW_LINE),"\\n").Replace(k.TAB,"\\t") + "\");";
      Response.Write("<script>" + script + "</script>");
      BackTrack(num_backsteps);
      }

    protected void AlertAndWindowHistoryBack
      (
      k.alert_cause_type cause,
      k.alert_state_type state,
      string key,
      string value
      )
      {
      var script = "alert(\"" + templatecontrol.AlertMessage(ConfigurationManager.AppSettings["application_name"],cause,state,key,value).Replace(Convert.ToString(k.NEW_LINE),"\\n").Replace(k.TAB,"\\t") + "\");";
      Response.Write
        (
        "<script>"
        + script
        + " window.history.back();"
        + "</script>"
        );
      }

    protected void BackTrack(uint num_backsteps = 1)
      {
      var p = "~/Default.aspx";
      var session_index = new k.subtype<int>(0,Session.Count);
      var be_page_p_found = false;
      var key = k.EMPTY;
      if (Session["waypoint_stack"] != null)
        {
        for (var i = new k.int_positive(); i.val <= num_backsteps; i.val++)
          {
          if ((Session["waypoint_stack"] as Stack).Count > 0)
            {
            p = (Session["waypoint_stack"] as Stack).Pop().ToString();
            for (session_index.val = Session.Count; !be_page_p_found && session_index.val > 0; session_index.val--)
              {
              key = Session.Keys[session_index.val - 1].ToString();
              if (key.EndsWith(".p"))
                {
                Session.Remove(key);
                be_page_p_found = !key.Contains("UserControl");
                }
              }
            }
          }
        }
      if (!File.Exists(path:Server.MapPath(".") + "/" + p))
        {
        if (Session["waypoint_stack"] != null)
          {
          (Session["waypoint_stack"] as Stack).Clear();
          }
        Server.Transfer("~/Default.aspx");
        }
      Server.Transfer(p);
      }

    protected void BeginBreadCrumbTrail()
      {
      SessionSet("waypoint_stack",new Stack());
      }

    protected void LabelizeAndSetTextForeColor
      (
      TableCell table_cell,
      Color fore_color
      )
      {
      templatecontrol.LabelizeAndSetTextForeColor(table_cell,fore_color);
      }

    protected void ClearMessage
      (
      string folder_name,
      string aspx_name
      )
      {
      Session.Remove($"msg_{folder_name}.{aspx_name}");
      }

    protected T Message<T>
      (
      string folder_name,
      string aspx_name
      )
      {
      return (T)Session[$"msg_{folder_name}.{aspx_name}"];
      }

    protected void DropCrumbAndTransferTo
      (
      string the_path,
      string anchor_name = k.EMPTY
      )
      {
      var current = Path.GetFileName(Request.CurrentExecutionFilePath);
      if (Session["waypoint_stack"] != null && ((Session["waypoint_stack"] as Stack).Count == 0 || (Session["waypoint_stack"] as Stack).Peek().ToString() != current))
        {
        (Session["waypoint_stack"] as Stack).Push(current);
        }
      if (anchor_name.Length == 0)
        {
        Server.Transfer(the_path);
        }
      else
        {
        Response.Redirect(the_path + "#" + anchor_name);
        }
      }

    protected void EstablishClientSideFunction
      (
      string profile,
      string body
      )
      {
      templatecontrol.EstablishClientSideFunction(Page,profile,body);
      }

    protected void EstablishClientSideFunction(k.client_side_function_enumeral_type enumeral)
    {
    templatecontrol.EstablishClientSideFunction(Page,enumeral);
    }

    protected void EstablishClientSideFunction(k.client_side_function_rec_type r)
      {
      templatecontrol.EstablishClientSideFunction(Page,r);
      }

    public void EstablishClientSideJsBehind()
      {
      templatecontrol.EstablishClientSideJsBehind
        (
        page:Page,
        resolvedClientUrlOfAppRelativeVirtualPath:ResolveClientUrl($"{AppRelativeVirtualPath}")
        );
      }

    protected void EstablishFormReenablementScript()
      {
      templatecontrol.EstablishFormReenablementScript(Page);
      }

    protected void EstablishGoogleWebFontLoader(string web_font_config)
      {
      templatecontrol.EstablishGoogleWebFontLoader(Page,web_font_config);
      }

    public void EstablishUpdatePanelCompliantTimeoutHandler
      (
      int redirect_timeout,
      string path_to_timeout_page
      )
      {
      templatecontrol.EstablishUpdatePanelCompliantTimeoutHandler(Page,redirect_timeout,path_to_timeout_page);
      }

    protected void ExportToCsv
      (
      string filename_sans_extension,
      string csv_string
      )
      {
      templatecontrol.ExportToCsv(Page,filename_sans_extension,csv_string);
      }

    protected void ExportToExcel
      (
      string filename_sans_extension,
      string excel_string
      )
      {
      templatecontrol.ExportToExcel(Page,filename_sans_extension,excel_string);
      }

    protected void FileDownload
      (
      string filename
      )
      {
      templatecontrol.FileDownload(Page,filename);
      }

    //
    // This routine is most likely insufficient and deprecated.  Use the control's built-in .Focus() method instead.
    //
    protected void Focus
      (
      Control c,
      bool be_using_scriptmanager = false, // Ideally the default should be true, but for backward compatibility (particularly in WebEMSOF) we default to false.
      bool be_redo = false
      )
      {
      templatecontrol.Focus(Page,c,be_using_scriptmanager,be_redo);
      }

    protected Hashtable HashtableOfShieldedRequest
      (
      string name = "q",
      bool do_uncompress = false
      )
      {
      return new JavaScriptSerializer().Deserialize<Hashtable>
        (
        k.StringOfShieldedValue
          (
          shielded_value:Request[name],
          do_uncompress:do_uncompress
          )
        );
      }

    public string InstanceId()
      {
      return Page.ToString();
      }

    public void MessageBack
      (
      object msg,
      string folder_name,
      string aspx_name
      )
      {
      templatecontrol.MessageBack(Page,msg,folder_name,aspx_name);
      }

    public void MessageDropCrumbAndTransferTo
      (
      object msg,
      string folder_name,
      string aspx_name,
      string anchor_name = k.EMPTY
      )
      {
      SessionSet("msg_" + folder_name + "." + aspx_name,msg);
      DropCrumbAndTransferTo(aspx_name + ".aspx",anchor_name);
      }

    private nature_of_visit_type NatureOfInvocation
      (
      string expected_session_item_name,
      bool be_timeout_behavior_standard,
      bool be_landing_from_login,
      bool be_cold_call_allowed
      )
      {
      nature_of_visit_type nature_of_invocation;
      bool be_cold_call;
      if (!IsPostBack)
        {
        if (be_landing_from_login)
          {
          be_cold_call_allowed = false;
          be_cold_call = ((ClaimsPrincipal)User).Claims.Where(claim => claim.Type == "user_id").FirstOrDefault() == null || string.IsNullOrEmpty(HttpContext.Current?.User?.Identity?.Name);
          }
        else
          {
          be_cold_call = Request.ServerVariables["URL"] == Request.CurrentExecutionFilePath;
          }
        if (be_cold_call)
          {
          nature_of_invocation = nature_of_visit_type.VISIT_COLD_CALL;
          //
          // The request for this page could not have been the result of a Server.Transfer call, and the session state is therefore unknown.  This is rarely allowed.
          //
          if (!be_cold_call_allowed)
            {
            Session.Clear();
            Server.Transfer("~/login.aspx");
            }
          }
        else
          {
          nature_of_invocation = nature_of_visit_type.VISIT_INITIAL;
          }
        }
      else
        {
        if (Session[expected_session_item_name] != null)
          {
          nature_of_invocation = nature_of_visit_type.VISIT_POSTBACK_STANDARD;
          }
        else
          {
          nature_of_invocation = nature_of_visit_type.VISIT_POSTBACK_STALE;
          if (be_timeout_behavior_standard)
            {
            Server.Transfer("~/timeout.aspx");
            }
          }
        }
      return nature_of_invocation;
      }

    protected nature_of_visit_type NatureOfLanding
      (
      string expected_session_item_name,
      bool be_timeout_behavior_standard = true
      )
      {
      return NatureOfInvocation
        (
        expected_session_item_name:expected_session_item_name,
        be_timeout_behavior_standard:be_timeout_behavior_standard,
        be_landing_from_login:true,
        be_cold_call_allowed:false
        );
      }

    protected nature_of_visit_type NatureOfVisit
      (
      string expected_session_item_name,
      bool be_timeout_behavior_standard = true
      )
      {
      return NatureOfInvocation
        (
        expected_session_item_name:expected_session_item_name,
        be_timeout_behavior_standard:be_timeout_behavior_standard,
        be_landing_from_login:false,
        be_cold_call_allowed:false
        );
      }

    protected nature_of_visit_type NatureOfVisitUnlimited
      (
      string expected_session_item_name,
      bool be_timeout_behavior_standard = true
      )
      {
      return NatureOfInvocation
        (
        expected_session_item_name:expected_session_item_name,
        be_timeout_behavior_standard:be_timeout_behavior_standard,
        be_landing_from_login:false,
        be_cold_call_allowed:true
        );
      }

    protected override void OnInit(EventArgs e)
      {
      base.OnInit(e);
      if (Context.Session != null)
        {
        ViewStateUserKey = Session.SessionID; // Prevents Cross-Site Request Forgery attacks (and bugs?)
        }
      templatecontrol.RegisterSidecarCss(Page,this);
      }

    protected override void OnPreRender(EventArgs e)
      {
      base.OnPreRender(e);
      if (ConfigurationManager.AppSettings["application_name"].EndsWith("_x"))
        {
        Response.AppendHeader("X-Robots-Tag","noindex, nofollow");
        }
      }

    protected void RequireConfirmation
      (
      WebControl c,
      string prompt
      )
      {
      templatecontrol.RequireConfirmation(c,prompt);
      }

    protected void SessionSet
      (
      string name,
      object value
      )
      {
      templatecontrol.SessionSet(Page,name,value);
      }

    protected string ShieldedQueryStringOfHashtable
      (
      Hashtable hash_table,
      bool do_compress = false
      )
      {
      return templatecontrol.ShieldedQueryStringOfHashtable(Page,hash_table,do_compress);
      }

    protected static string ShieldedValueOfHashtable(Hashtable hash_table)
      {
      return k.ShieldedValueOfHashtable(hash_table);
      }

    protected string StringOfControl(Control c)
      {
      return templatecontrol.StringOfControl(c);
      }

    protected void TransferToPageBinderTab
      (
      string page_nick,
      string binder_nick,
      uint tab_index
      )
      {
      templatecontrol.TransferToPageBinderTab(Page,page_nick,binder_nick,tab_index);
      }

    protected void ValidationAlert(bool be_using_scriptmanager = false) // Ideally the default should be true, but for backward compatibility (particularly in WebEMSOF) we default to false.
      {
      templatecontrol.ValidationAlert(Page,be_using_scriptmanager);
      }
      
    } // end page_class
  }
