using kix;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ki.web_ui
  {
  public class usercontrol_class : UserControl
    {

    protected readonly static string APP_HANDLED_ASYNC_POST_BACK_ERROR_MESSAGE_HEADER_VALUE = common.APP_HANDLED_ASYNC_POST_BACK_ERROR_MESSAGE_HEADER_VALUE;
    protected readonly static string APP_HANDLED_ASYNC_POST_BACK_ERROR_MESSAGE_MARK = common.APP_HANDLED_ASYNC_POST_BACK_ERROR_MESSAGE_MARK;
    protected readonly static string CUSTOM_RESPONSE_HEADER_NAME = common.CUSTOM_RESPONSE_HEADER_NAME;
    protected readonly static string SESSION_INTERRUPTED_HEADER_VALUE = common.SESSION_INTERRUPTED_HEADER_VALUE;

    private readonly templatecontrol_class templatecontrol = null;

    public usercontrol_class() : base() // CONSTRUCTOR
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

    protected string AlertMessage
      (
      k.alert_cause_type cause,
      k.alert_state_type state,
      string key,
      string value
      )
      {
      return templatecontrol.AlertMessage(ConfigurationManager.AppSettings["application_name"],cause,state,key,value);
      }

    protected void BackTrack(uint num_backsteps = 1)
      {
      var p = "~/Default.aspx";
      if (Session["waypoint_stack"] != null)
        {
        for (var i = new k.int_positive(); i.val <= num_backsteps; i.val++)
          {
          if ((Session["waypoint_stack"] as Stack).Count > 0)
            {
            p = (Session["waypoint_stack"] as Stack).Pop().ToString();
            }
          }
        }
      Server.Transfer(p);
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
      templatecontrol.EstablishClientSideFunction(Page,profile,body,ClientID);
      }

    public void EstablishClientSideJsBehind()
      {
      templatecontrol.EstablishClientSideJsBehind
        (
        page:Page,
        resolvedClientUrlOfAppRelativeVirtualPath:ResolveClientUrl($"{AppRelativeVirtualPath}")
        );
      }

    protected void EstablishClientSideFunction(k.client_side_function_enumeral_type enumeral)
      {
      templatecontrol.EstablishClientSideFunction(Page,enumeral);
      }

    protected void EstablishClientSideFunction(k.client_side_function_rec_type r)
      {
      templatecontrol.EstablishClientSideFunction(Page,r);
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
      templatecontrol.FileDownload(Page, filename);
      }

    protected void Focus
      (
      Control c,
      bool be_using_scriptmanager = false, // Ideally the default should be true, but for backward compatibility (particularly in WebEMSOF) we default to false.
      bool be_redo = false
      )
      {
      templatecontrol.Focus(Page,c,be_using_scriptmanager,be_redo);
      }

    public string InstanceId()
      {
      return (Page.ToString() + ".UserControl_" + ClientID.Replace("UserControl",k.EMPTY)).Replace("__","_");
      }

    protected void LabelizeAndSetTextForeColor
      (
      TableCell table_cell,
      Color fore_color
      )
      {
      templatecontrol.LabelizeAndSetTextForeColor(table_cell,fore_color);
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

    protected override void OnInit(EventArgs e)
      {
      base.OnInit(e);
      templatecontrol.RegisterSidecarCss(Page,this);
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

    protected void ValidationAlert(bool be_using_scriptmanager)
      {
      templatecontrol.ValidationAlert(Page,be_using_scriptmanager);
      }

    protected void ValidationAlert()
      {
      ValidationAlert(false);
      }

    } // end usercontrol_class
  }
