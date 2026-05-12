using kix;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ki.web_ui
  {
  public class templatecontrol_class : TemplateControl
    {

    #pragma warning disable CA1822 // Member does not access instance data and can be marked as static

    public templatecontrol_class() : base() // CONSTRUCTOR
      {
      }

    internal void LabelizeAndSetTextForeColor
      (
      TableCell table_cell,
      Color fore_color
      )
      {
      var the_label = new Label();
      the_label.ForeColor = fore_color;
      the_label.Text = table_cell.Text;
      table_cell.Text = k.EMPTY;
      table_cell.Controls.Add(the_label);
      }

    protected override void OnInit(EventArgs e)
      {
      base.OnInit(e);
      TemplateControl = new templatecontrol_class();
      }

    public void RegisterSidecarCss
      (
      Page page,
      Control host
      )
      {
      if (page is not null && page.Header is not null && host is TemplateControl)
        {
        var cssVirtualPath = k.EMPTY;
        var cssId = k.EMPTY;
        var beRegistrationNeeded = false;
        var hostAppRelativeVirtualPath = (host as TemplateControl).AppRelativeVirtualPath;
        if (hostAppRelativeVirtualPath.Length > 0)
          {
          var virtualDir = VirtualPathUtility.GetDirectory(hostAppRelativeVirtualPath) ?? "~/";
          var cssFileName = $"{Path.GetFileName(hostAppRelativeVirtualPath)}.css";
          cssVirtualPath = $"{virtualDir.TrimEnd('/')}/{cssFileName}";
          if (File.Exists(page.Server.MapPath(cssVirtualPath)))
            {
            cssId = $"{host.GetType().Name.Replace('.','_')}__sidecar_css";
            beRegistrationNeeded = page.Header.FindControl(cssId) is null;
            }
          }
        if (beRegistrationNeeded)
          {
          var htmlLink = new HtmlLink
            {
            Href = page.ResolveUrl(cssVirtualPath),
            ID = cssId
            };
          htmlLink.Attributes["rel"] = "stylesheet";
          htmlLink.Attributes["type"] = "text/css";
          page.Header.Controls.Add(htmlLink);
          }
        }
      }

    public void AddCopyFromFeatureToControl
      (
      Page the_page,
      string instance_function_name,
      Dictionary<WebControl,WebControl> target_source_dictionary,
      WebControl c
      )
      {
      EstablishClientSideFunction
        (
        the_page:the_page,
        enumeral: k.client_side_function_enumeral_type.EL
        );
      //
      var body = k.EMPTY;
      foreach (var target_source in target_source_dictionary)
        {
        body += "El('" + target_source.Key.ClientID + "').value = El('" + target_source.Value.ClientID + "').value;" + k.NEW_LINE;
        }
      //
      EstablishClientSideFunction
        (
        the_page:the_page,
        profile: instance_function_name + "()",
        body: body
        );
      //
      c.Attributes.Add
        (
        key: "OnClick",
        value: instance_function_name + "(); return false;" // Appending "return false;" prevents a postback.
        );
      }

    public string AddIdentifiedControlToPlaceHolder
      (
      Page the_page,
      Control c,
      string id,
      PlaceHolder p,
      string instance_context_id_for_freshening = k.EMPTY
      )
      {
      //
      // Without specifying an ID for a dynamically-added control, ASP.NET supplies its own ID for the control.  The problem is that
      // ASP.NET may specify one ID for the control at initial page presentation time and another ID at postback page presentation.
      // Because postback events are tied to the ID of the control generating the postback, ASP.NET's ID assignment behavior may result
      // in a postback event that is ignored the first time (but not subsequent times).
      //
      if (instance_context_id_for_freshening.Length > 0)
        {
        the_page.Session.Remove((instance_context_id_for_freshening + (instance_context_id_for_freshening.Contains(".UserControl_") ? "_" : ".UserControl_") + id.Replace("UserControl",k.EMPTY)).Replace("__","_") + ".p");
        }
      c.ID = id;
      p.Controls.Add(c);
      return id;
      }

    public void Alert
      (
      Page the_page,
      k.alert_cause_type cause,
      k.alert_state_type state,
      string key,
      string value,
      bool be_using_scriptmanager = false // Ideally the default should be true, but for backward compatibility (particularly in WebEMSOF) we default to false.
      )
      {
      var script = "alert(\"" + AlertMessage(ConfigurationManager.AppSettings["application_name"],cause,state,key,value).Replace(Convert.ToString(k.NEW_LINE),"\\n").Replace(k.TAB,"\\t") + "\");";
      if (be_using_scriptmanager)
        {
        ScriptManager.RegisterStartupScript
          (
          control:the_page,
          type:the_page.GetType(),
          key:key,
          script:script,
          addScriptTags:true
          );
        }
      else
        {
        the_page.ClientScript.RegisterStartupScript
          (
          type:the_page.GetType(),
          key:key,
          script:script,
          addScriptTags:true
          );
        }
      }

    public string AlertMessage
      (
      string application_name,
      k.alert_cause_type cause,
      k.alert_state_type state,
      string key,
      string s
      )
      {
      return k.EMPTY
      + "- - - ---------------------------------------------------- - - -"
      + k.NEW_LINE
      + "       issuer:  " + k.TAB + application_name + k.NEW_LINE
      + "       cause:   " + k.TAB + cause.ToString().ToLower() + k.NEW_LINE
      + "       state:   " + k.TAB + state.ToString().ToLower() + k.NEW_LINE
      + "       key:     " + k.TAB + key.ToLower() + k.NEW_LINE
      + "       time:    " + k.TAB + DateTime.Now.ToString("s") + k.NEW_LINE
      + "- - - ---------------------------------------------------- - - -" + k.NEW_LINE
      + k.NEW_LINE
      + k.NEW_LINE
      + s + k.NEW_LINE
      + k.NEW_LINE;
      }

    public void EstablishClientSideFunction
      (
      Page the_page,
      string profile,
      string body,
      string usercontrol_clientid = k.EMPTY
      )
      {
      the_page.ClientScript.RegisterClientScriptBlock
        (
        the_page.GetType(),
        usercontrol_clientid + "__" + profile.Remove(profile.IndexOf(k.OPEN_PARENTHESIS)),
        "function " + profile + k.NEW_LINE
        + " {" + k.NEW_LINE
        + ' ' + body.Replace(Convert.ToString(k.NEW_LINE),Convert.ToString(k.NEW_LINE + k.SPACE)) + k.NEW_LINE
        + " }" + k.NEW_LINE,
        true
        );
      }

    public void EstablishClientSideFunction
      (
      Page the_page,
      k.client_side_function_enumeral_type enumeral
      )
      {
      if (enumeral == k.client_side_function_enumeral_type.EL)
        {
        EstablishClientSideFunction
          (
          the_page:the_page,
          profile:"El(id)",
          body:"return document.getElementById(id);"
          );
        }
      else if (enumeral == k.client_side_function_enumeral_type.KGS_TO_LBS)
        {
        EstablishClientSideFunction
          (
          the_page:the_page,
          profile:"KgsToLbs(num_kgs)",
          body:"return Math.round(+num_kgs*2.204622);"
          );
        }
      else if (enumeral == k.client_side_function_enumeral_type.LBS_TO_KGS)
        {
        EstablishClientSideFunction
          (
          the_page:the_page,
          profile:"LbsToKgs(num_lbs)",
          body:"return Math.round(+num_lbs/2.204622);"
          );
        }
      else if (enumeral == k.client_side_function_enumeral_type.REMOVE_EL)
        {
        EstablishClientSideFunction
          (
          the_page:the_page,
          profile:"RemoveEl(id)",
          body:"condemned_el = El(id); condemned_el.parentNode.removeChild(condemned_el);"
          );
        }
      }

    public void EstablishClientSideFunction
      (
      Page the_page,
      k.client_side_function_rec_type r
      )
      {
      EstablishClientSideFunction
        (
        the_page:the_page,
        profile:r.profile,
        body:r.body
        );
      }

    public void EstablishClientSideJsBehind
      (
      Page page,
      string resolvedClientUrlOfAppRelativeVirtualPath
      )
      {
      var jsSpec = $"{resolvedClientUrlOfAppRelativeVirtualPath}.js";
      ScriptManager.RegisterClientScriptInclude
        (
        page:page,
        type:page.GetType(),
        key:jsSpec,
        url:jsSpec
        );
      }

    public void EstablishFormReenablementScript(Page the_page)
      {
      the_page.ClientScript.RegisterClientScriptBlock
        (
        GetType(),
        "FormReenablementScript",
        "window.onLoad = document.getElementById('Form_control').disabled = false;" + k.NEW_LINE,
        true
        );
      }

    public void EstablishGoogleWebFontLoader
      (
      Page the_page,
      string web_font_config
      )
      {
      //
      // NOTE: The value of the "key" query parameter must be registered on the Google Developers Console as an API key with a Referrer value of "frompaper2web.com/*".
      //
      the_page.ClientScript.RegisterClientScriptInclude
        (
        key:"GoogleWebFontLoaderInclude",
        url:$"https://ajax.googleapis.com/ajax/libs/webfont/1.6.26/webfont.js?key={ConfigurationManager.ConnectionStrings["google_static_maps_api_key"].ConnectionString}"
        );
      the_page.ClientScript.RegisterClientScriptBlock
        (
        type:the_page.GetType(),
        key:"GoogleWebFontLoaderBlock",
        script:"WebFont.load({" + web_font_config + "});",
        addScriptTags:true
        );
      }

    public void EstablishUpdatePanelCompliantTimeoutHandler
      (
      Page the_page,
      int redirect_timeout,
      string path_to_timeout_page
      )
      {
      the_page.ClientScript.RegisterClientScriptBlock
        (
        type:the_page.GetType(),
        key:"UpdatePanelCompliantTimeoutHandler",
        script:"var redirect_timer; clearTimeout(redirect_timer); redirect_timer = setTimeout('window.location.href=\"" + path_to_timeout_page + "\";'," + redirect_timeout.ToString() + ");" + k.NEW_LINE,
        addScriptTags:true
        );
      }

    public void ExportToCsv
      (
      Page the_page,
      string filename_sans_extension,
      string csv_string
      )
      {
      the_page.Response.ClearHeaders(); // Clear out the effects of generating no-cache & no-store headers in UserControl_precontent.
      the_page.Response.Clear();
      the_page.Response.AppendHeader("Content-Disposition", "attachment; filename=\"" + filename_sans_extension + ".csv\"");  //Don't enclose filename in apostrophes.
      the_page.Response.BufferOutput = true;
      the_page.Response.ContentType = "text/csv";
      the_page.EnableViewState = false;
      the_page.Response.Write(csv_string);
      the_page.Response.End();
      }

    public void ExportToExcel
      (
      Page the_page,
      string filename_sans_extension,
      string excel_string
      )
      {
      //
      // Note that if the excel_string you supply to this routine is actually in HTML or XMLSS format and the end-user uses MS Excel 2007 or later, Excel will prompt the end-user whether to continue opening the file,
      // because this routine sends the file with an XLS extension (which implies BFF content).  Adjusting the ContentType (MIME type) will not help.  Consider alerting the end-user that it's ok to respond "Yes" to
      // continue opening the file in Excel.
      //
      the_page.Response.ClearHeaders(); // Clear out the effects of generating no-cache & no-store headers in UserControl_precontent.
      the_page.Response.Clear();
      the_page.Response.AppendHeader("Content-Disposition", "attachment; filename=\"" + filename_sans_extension + ".xls\"");  //Don't wrap filename in apostrophes.
      the_page.Response.BufferOutput = true;
      the_page.Response.ContentType = "application/vnd.ms-excel";
      the_page.EnableViewState = false;
      the_page.Response.Write(excel_string);
      the_page.Response.End();
      }

    public void FileDownload
      (
      Page the_page,
      string filename
      )
      {
      the_page.Response.ClearHeaders(); // Clear out the effects of generating no-cache & no-store headers in UserControl_precontent.
      the_page.Response.Clear();
      the_page.Response.AppendHeader("Content-Disposition", "attachment; filename=\"" + Path.GetFileName(filename) + "\"");  //Don't enclose filename in apostrophes.
      the_page.Response.BufferOutput = true;
      the_page.Response.ContentType = "application/octet-stream";
      the_page.EnableViewState = false;
      the_page.Response.TransmitFile(filename);
      the_page.Response.End();
      }

    //
    // This routine is most likely insufficient and deprecated.  Use the control's built-in .Focus() method instead.
    //
    public void Focus
      (
      Page the_page,
      Control c,
      bool be_using_scriptmanager = false, // Ideally the default should be true, but for backward compatibility (particularly in WebEMSOF) we default to false.
      bool be_redo = false
      )
      {
      var key = "SetFocus";
      var script = k.EMPTY
      + " if (!document.getElementById(\"" + c.ClientID + "\").disabled)"
      +   " {"
      +   " document.getElementById(\"" + c.ClientID + "\").focus();";
      if (be_redo)
        //
        // Place cursor at end of input.  Inefficiency necessary for cross-browser compatibility.
        //
        {
        script += k.EMPTY
        + " var v = document.getElementById(\"" + c.ClientID + "\").value;"
        + " document.getElementById(\"" + c.ClientID + "\").value = '';"
        + " document.getElementById(\"" + c.ClientID + "\").value = v;";
        }
      script += k.EMPTY
      +   " }";
      if (be_using_scriptmanager)
        {
        ScriptManager.RegisterStartupScript
          (
          control:the_page,
          type:the_page.GetType(),
          key:key,
          script:script,
          addScriptTags:true
          );
        }
      else
        {
        Page.ClientScript.RegisterStartupScript
          (
          type:the_page.GetType(),
          key:key,
          script:script,
          addScriptTags:true
          );
        }
      }

    public void MessageBack
      (
      Page the_page,
      object msg,
      string folder_name,
      string aspx_name
      )
      {
      SessionSet
        (
        the_page:the_page,
        name:"msg_" + folder_name + "." + aspx_name,
        value:msg
        );
      }

    public void RequireConfirmation(WebControl c, string prompt)
      {
      c.Attributes.Add
        (
        key:"onclick",
        value:"if(!confirm(\"- - - ---------------------------------------------------- - - -\\n"
        +                   "       issuer:  \\t" + ConfigurationManager.AppSettings["application_name"] + "\\n"
        +                   "       state:   \\twarning\\n" + "       time:    \\t" + DateTime.Now.ToString("s") + "\\n"
        +                   "- - - ---------------------------------------------------- - - -\\n\\n\\n"
        + prompt.Replace(Convert.ToString(k.NEW_LINE),"\\n")
        +                   "\\n\\n\""
        +                   ")) return false;"
        );
      }

    public void SessionSet
      (
      Page the_page,
      string name,
      object value
      )
      {
      the_page.Session.Remove(name);
      the_page.Session.Add(name,value);
      }

    public string ShieldedQueryStringOfHashtable
      (
      Page the_page,
      Hashtable hash_table,
      bool do_compress = false
      )
      {
      return "q=" + the_page.Server.UrlEncode(k.ShieldedValueOfHashtable(hash_table,do_compress));
      }

    public string StringOfControl(Control c)
      {
      var string_writer = new StringWriter();
      using var html_text_writer = new HtmlTextWriter(string_writer);
      c.RenderControl(html_text_writer);
      return string_writer.ToString();
      }

    public void TransferToPageBinderTab
      (
      Page the_page,
      string page_nick,
      string binder_nick,
      uint tab_index
      )
      {
      SessionSet(the_page,"UserControl_" + binder_nick + "_binder_selected_tab",tab_index);
      the_page.Server.Transfer(page_nick + ".aspx");
      }

    public void ValidationAlert
      (
      Page the_page,
      bool be_using_scriptmanager
      )
      {
      Alert
        (
        the_page:the_page,
        cause:k.alert_cause_type.USER,
        state:k.alert_state_type.FAILURE,
        key:"stdsvrval",
        value:common.VALIDATION_ALERT,
        be_using_scriptmanager:be_using_scriptmanager
        );
      }

    public void ValidationAlert(Page the_page)
      {
      ValidationAlert
        (
        the_page:the_page,
        be_using_scriptmanager:false
        );
      }

    #pragma warning restore CA1822 // Member does not access instance data and can be marked as static

    } // end templatecontrol_class
  }
