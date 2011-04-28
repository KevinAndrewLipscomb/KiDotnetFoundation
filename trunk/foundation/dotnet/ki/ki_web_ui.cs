using kix;
using System;
using System.Collections;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ki_web_ui
{

    static class common
    {
        public const string STD_VALIDATION_ALERT = "Something about the data you just submitted is invalid.  Look for !ERR! indications near the data fields.  A more detailed explanation may appear near the top of the page.";
    }
  
    // --------------------------------------------------------------------------------------------------------------------------------
    // templatecontrol_class
    // --------------------------------------------------------------------------------------------------------------------------------
    public class templatecontrol_class: System.Web.UI.TemplateControl
    {
        // ==================================================================================================================================
        // templatecontrol_class
        // ==================================================================================================================================
        //Constructor  Create()
        public templatecontrol_class() : base()
        {
            // TODO: Add any constructor code here

        }
        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);
            this.TemplateControl = new templatecontrol_class();
        }

        public string AddIdentifiedControlToPlaceHolder(Control c, string id, PlaceHolder p)
        {
            string result;
            // Without specifying an ID for a dynamically-added control, ASP.NET supplies its own ID for the control.  The problem is that
            // ASP.NET may specify one ID for the control at initial page presentation time and another ID at postback page presentation.
            // Because postback events are tied to the ID of the control generating the postback, ASP.NET's ID assignment behavior may result
            // in a postback event that is ignored the first time (but not subsequent times).
            c.ID = id;
            p.Controls.Add(c);
            result = id;
            return result;
        }

        public void Alert(Page the_page, k.alert_cause_type cause, k.alert_state_type state, string key, string value, bool be_using_scriptmanager)
        {
            string script;


            script = k.EMPTY
            + "alert(\""
            + AlertMessage(ConfigurationManager.AppSettings["application_name"], cause, state, key, value)
              .Replace(Convert.ToString(k.NEW_LINE), "\\n")
              .Replace(k.TAB, "\\t") + "\");";
            if (be_using_scriptmanager)
            {


                ScriptManager.RegisterStartupScript(the_page, the_page.GetType(), key, script, true);
            }
            else
            {
                the_page.ClientScript.RegisterStartupScript(the_page.GetType(), key, script, true);
            }

        }

        public void Alert(Page the_page, k.alert_cause_type cause, k.alert_state_type state, string key, string value)
        {
            Alert(the_page, cause, state, key, value, false);
        }

        public string AlertMessage(string application_name, k.alert_cause_type cause, k.alert_state_type state, string key, string s)
        {
            string result;









            result = k.EMPTY
            + "- - - ---------------------------------------------------- - - -"
            + k.NEW_LINE
            + "       issuer:  " + k.TAB + application_name + k.NEW_LINE
            + "       cause:   " + k.TAB + ((k.alert_cause_type)(cause)).ToString().ToLower() + k.NEW_LINE
            + "       state:   " + k.TAB + ((k.alert_state_type)(state)).ToString().ToLower() + k.NEW_LINE
            + "       key:     " + k.TAB + key.ToLower() + k.NEW_LINE
            + "       time:    " + k.TAB + DateTime.Now.ToString("s") + k.NEW_LINE
            + "- - - ---------------------------------------------------- - - -" + k.NEW_LINE
            + k.NEW_LINE
            + k.NEW_LINE
            + s + k.NEW_LINE
            + k.NEW_LINE;
            return result;
        }

        public void EstablishClientSideFunction(Page the_page, string profile, string body, string usercontrol_clientid)
        {
            the_page.ClientScript.RegisterClientScriptBlock
              (
              the_page.GetType(),
              usercontrol_clientid + "__" + profile.Remove(profile.IndexOf(k.OPEN_PARENTHESIS)),
              "function " + profile + k.NEW_LINE
              + " {" + k.NEW_LINE
              + ' ' + body.Replace(Convert.ToString(k.NEW_LINE), Convert.ToString(k.NEW_LINE + k.SPACE)) + k.NEW_LINE
              + " }" + k.NEW_LINE,
              true
              );
        }

        public void EstablishClientSideFunction(Page the_page, string profile, string body)
        {
            EstablishClientSideFunction(the_page, profile, body, "");
        }

        public void EstablishClientSideFunction(Page the_page, k.client_side_function_enumeral_type enumeral)
        {
            switch(enumeral)
            {
                case k.client_side_function_enumeral_type.EL:
                    EstablishClientSideFunction(the_page, "El(id)", "return document.getElementById(id);");
                    break;
                case k.client_side_function_enumeral_type.KGS_TO_LBS:
                    EstablishClientSideFunction(the_page, "KgsToLbs(num_kgs)", "return Math.round(+num_kgs*2.204622);");
                    break;
                case k.client_side_function_enumeral_type.LBS_TO_KGS:
                    EstablishClientSideFunction(the_page, "LbsToKgs(num_lbs)", "return Math.round(+num_lbs/2.204622);");
                    break;
            }
        }

        public void EstablishClientSideFunction(Page the_page, k.client_side_function_rec_type r)
        {
            EstablishClientSideFunction(the_page, r.profile, r.body);
        }

        public void EstablishGoogleWebFontLoader
          (
          Page the_page,
          string web_font_config
          )
          {
          the_page.ClientScript.RegisterClientScriptBlock
            (
            the_page.GetType(),
            "GoogleWebFontLoader",
            "WebFontConfig = { " + web_font_config + " };"
            + " (function ()"
            +   " {"
            +   " var wf = document.createElement('script');"
            +   " wf.src = ('https:' == document.location.protocol ? 'https' : 'http') + '://ajax.googleapis.com/ajax/libs/webfont/1/webfont.js';"
            +   " wf.type = 'text/javascript';"
            +   " wf.async = 'true';"
            +   " var s = document.getElementsByTagName('script')[0];"
            +   " s.parentNode.insertBefore(wf, s);"
            +   " }"
            + " )();",
            true
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
            the_page.GetType(),
            "UpdatePanelCompliantTimeoutHandler",
            "var redirect_timer; clearTimeout(redirect_timer); redirect_timer = setTimeout('window.location.href=\"" + path_to_timeout_page + "\";'," + redirect_timeout.ToString() + ");",
            true
            );
          }

        public void ExportToExcel(System.Web.UI.Page the_page, string filename_sans_extension, string excel_string)
        {
            the_page.Response.Clear();
            the_page.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename_sans_extension + ".xls");
            the_page.Response.BufferOutput = true;
            the_page.Response.ContentType = "application/vnd.ms-excel";
            the_page.EnableViewState = false;
            the_page.Response.Write(excel_string);
            the_page.Response.End();
        }

        public void FileDownload(Page the_page, string filename)
        {
            the_page.Response.Clear();
            the_page.Response.AppendHeader("Content-Disposition", "attachment; filename=" + System.IO.Path.GetFileName(filename));
            the_page.Response.BufferOutput = true;
            the_page.Response.ContentType = "application/octet-stream";
            the_page.EnableViewState = false;
            the_page.Response.TransmitFile(filename);
            the_page.Response.End();
        }

        public void Focus(Page the_page, Control c, bool be_using_scriptmanager)
        {
            if (be_using_scriptmanager)
            {


                ScriptManager.RegisterStartupScript(the_page, the_page.GetType(), "SetFocus", "if (!document.getElementById(\"" + c.ClientID + "\").disabled) {document.getElementById(\"" + c.ClientID + "\").focus();}", true);
            }
            else
            {
                this.Page.ClientScript.RegisterStartupScript(the_page.GetType(), "SetFocus", "if (!document.getElementById(\"" + c.ClientID + "\").disabled) {document.getElementById(\"" + c.ClientID + "\").focus();}", true);
            }
        }

        public void Focus(Page the_page, Control c)
        {
            Focus(the_page, c, false);
        }

        public void RequireConfirmation(WebControl c, string prompt)
        {





            c.Attributes.Add("onclick", "return confirm(\"- - - ---------------------------------------------------- - - -\\n" + "       issuer:  \\t" + ConfigurationManager.AppSettings["application_name"] + "\\n" + "       state:   \\twarning\\n" + "       time:    \\t" + DateTime.Now.ToString("s") + "\\n" + "- - - ---------------------------------------------------- - - -\\n\\n\\n" + prompt.Replace(Convert.ToString(k.NEW_LINE), "\\n") + "\\n\\n\"" + ");");
        }

        public void SessionSet(Page the_page, string name, object value)
        {
            the_page.Session.Remove(name);
            the_page.Session.Add(name, value);
        }

    public string ShieldedQueryStringOfHashtable
      (
      Page the_page,
      Hashtable hash_table
      )
      {
      var ascii_encoding = new ASCIIEncoding();
      var bytearrayed_serialized_hashtable = ascii_encoding.GetBytes(new JavaScriptSerializer().Serialize(hash_table));
      var cipher = new RijndaelManaged();
      cipher.Mode = CipherMode.ECB;
      cipher.Key = ascii_encoding.GetBytes(ConfigurationManager.AppSettings["query_string_protection_password"]);
      return "q=" + the_page.Server.UrlEncode(Convert.ToBase64String(cipher.CreateEncryptor().TransformFinalBlock(bytearrayed_serialized_hashtable,0,bytearrayed_serialized_hashtable.Length)));
      }

        public string StringOfControl(Control c)
        {
            string result;
            System.IO.StringWriter stringwriter;
            stringwriter = new System.IO.StringWriter();
            c.RenderControl(new System.Web.UI.HtmlTextWriter(stringwriter));
            result = stringwriter.ToString();
            return result;
        }

        public void TransferToPageBinderTab(Page the_page, string page_nick, string binder_nick, uint tab_index)
        {
            SessionSet(the_page, "UserControl_" + binder_nick + "_binder_selected_tab", (tab_index));
            the_page.Server.Transfer(page_nick + ".aspx");
        }

        public void ValidationAlert(Page the_page, bool be_using_scriptmanager)
        {
            Alert(the_page, k.alert_cause_type.USER, k.alert_state_type.FAILURE, "stdsvrval", common.STD_VALIDATION_ALERT, be_using_scriptmanager);
        }

        public void ValidationAlert(Page the_page)
        {
            ValidationAlert(the_page, false);
        }

    } // end templatecontrol_class

    // --------------------------------------------------------------------------------------------------------------------------------
    // page_class
    // --------------------------------------------------------------------------------------------------------------------------------
    public class page_class: System.Web.UI.Page
    {
        private templatecontrol_class templatecontrol = null;
        // ==================================================================================================================================
        // PAGE_CLASS
        // ==================================================================================================================================
        //Constructor  Create()
        public page_class() : base()
        {
            templatecontrol = new templatecontrol_class();
        }
        protected string AddIdentifiedControlToPlaceHolder(Control c, string id, PlaceHolder p)
        {
            string result;
            result = templatecontrol.AddIdentifiedControlToPlaceHolder(c, id, p);
            return result;
        }

        protected void Alert(k.alert_cause_type cause, k.alert_state_type state, string key, string value, bool be_using_scriptmanager)
        {
            templatecontrol.Alert(this.Page, cause, state, key, value, be_using_scriptmanager);
        }

        protected void Alert(k.alert_cause_type cause, k.alert_state_type state, string key, string value)
        {
            Alert(cause, state, key, value, false);
        }

        protected void BackTrack(uint num_backsteps)
        {
            uint i;
            var p = "~/Default.aspx";
            var session_index = new k.subtype<int>(0,Session.Count);
            var be_page_p_found = false;
            var key = k.EMPTY;
            if ((this.Session["waypoint_stack"] != null))
            {
                for (i = 1; i <= num_backsteps; i ++ )
                {
                    if (((this.Session["waypoint_stack"]) as Stack).Count > 0)
                    {
                        p = ((this.Session["waypoint_stack"]) as Stack).Pop().ToString();
                        for (session_index.val = Session.Count; !be_page_p_found && session_index.val > 0; session_index.val-- )
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
            this.Server.Transfer(p);
        }

        protected void BackTrack()
        {
            BackTrack(1);
        }

        protected void BeginBreadCrumbTrail()
        {
            SessionSet("waypoint_stack", new Stack());
        }

        protected T Message<T>
          (
          string folder_name,
          string aspx_name
          )
          {
          return (T)(Session["msg_" + folder_name + "." + aspx_name]);
          }

        protected void DropCrumbAndTransferTo
          (
          string the_path,
          string anchor_name
          )
          {
          var current = Path.GetFileName(Request.CurrentExecutionFilePath);
          if ((Session["waypoint_stack"] != null) && ((((Session["waypoint_stack"]) as Stack).Count == 0) || (((Session["waypoint_stack"]) as Stack).Peek().ToString() != current)))
            {
              ((Session["waypoint_stack"]) as Stack).Push(current);
            }
          if (anchor_name == k.EMPTY)
            {
            Server.Transfer(the_path);
            }
          else
            {
            Response.Redirect(the_path + "#" + anchor_name);
            }
          }
        protected void DropCrumbAndTransferTo(string the_path)
          {
          DropCrumbAndTransferTo(the_path,k.EMPTY);
          }

        protected void EstablishClientSideFunction(string profile, string body)
        {
            templatecontrol.EstablishClientSideFunction(this.Page, profile, body);
        }

        protected void EstablishClientSideFunction(k.client_side_function_enumeral_type enumeral)
        {
            templatecontrol.EstablishClientSideFunction(this.Page, enumeral);
        }

        protected void EstablishClientSideFunction(k.client_side_function_rec_type r)
        {
            templatecontrol.EstablishClientSideFunction(this.Page, r);
        }

        protected void EstablishGoogleWebFontLoader(string web_font_config)
          {
          templatecontrol.EstablishGoogleWebFontLoader(this.Page,web_font_config);
          }

        public void EstablishUpdatePanelCompliantTimeoutHandler
          (
          int redirect_timeout,
          string path_to_timeout_page
          )
          {
          templatecontrol.EstablishUpdatePanelCompliantTimeoutHandler(this.Page,redirect_timeout,path_to_timeout_page);
          }

        protected void ExportToExcel(System.Web.UI.Page the_page, string filename_sans_extension, string excel_string)
        {
            templatecontrol.ExportToExcel(this.Page, filename_sans_extension, excel_string);
        }

        protected void FileDownload(Page the_page, string filename)
        {
            templatecontrol.FileDownload(this.Page, filename);
        }

        protected void Focus(Control c, bool be_using_scriptmanager)
        {
            templatecontrol.Focus(this.Page, c, be_using_scriptmanager);
        }

        protected void Focus(Control c)
        {
            Focus(c, false);
        }

    protected Hashtable HashtableOfShieldedRequest()
      {
      var ascii_encoding = new ASCIIEncoding();
      var unbase64ed_query_string = Convert.FromBase64String(Request["q"]);
      var cipher = new RijndaelManaged();
      cipher.Mode = CipherMode.ECB;
      cipher.Key = ascii_encoding.GetBytes(ConfigurationManager.AppSettings["query_string_protection_password"]);
      return new JavaScriptSerializer().Deserialize<Hashtable>(ascii_encoding.GetString(cipher.CreateDecryptor().TransformFinalBlock(unbase64ed_query_string,0,unbase64ed_query_string.Length)));
      }

        public void MessageDropCrumbAndTransferTo
          (
          object msg,
          string folder_name,
          string aspx_name,
          string anchor_name
          )
          {
          SessionSet("msg_" + folder_name + "." + aspx_name,msg);
          DropCrumbAndTransferTo(aspx_name + ".aspx",anchor_name);
          }
        public void MessageDropCrumbAndTransferTo
          (
          object msg,
          string folder_name,
          string aspx_name
          )
          {
          MessageDropCrumbAndTransferTo(msg,folder_name,aspx_name,k.EMPTY);
          }

        private nature_of_visit_type NatureOfInvocation(string expected_session_item_name, bool be_timeout_behavior_standard, bool be_landing_from_login, bool be_cold_call_allowed)
        {
            nature_of_visit_type result;
            bool be_cold_call;
            if (!this.IsPostBack)
            {
                if (be_landing_from_login)
                {
                    be_cold_call_allowed = false;
                    be_cold_call = (this.Session["user_id"] == null) || (this.Session["username"] == null);
                }
                else
                {
                    be_cold_call = (this.Request.ServerVariables["URL"] == this.Request.CurrentExecutionFilePath);
                // The request for this page could not have been the result of a Server.Transfer call, and the session state is therefore
                // unknown.  This is rarely allowed.
                }
                if (be_cold_call)
                {
                    result = nature_of_visit_type.VISIT_COLD_CALL;
                    if (!be_cold_call_allowed)
                    {
                        this.Session.Clear();
                        this.Server.Transfer("~/login.aspx");
                    }
                }
                else
                {
                    result = nature_of_visit_type.VISIT_INITIAL;
                }
            }
            else
            {
                if ((this.Session[expected_session_item_name] != null))
                {
                    result = nature_of_visit_type.VISIT_POSTBACK_STANDARD;
                }
                else
                {
                    result = nature_of_visit_type.VISIT_POSTBACK_STALE;
                    if (be_timeout_behavior_standard)
                    {
                        this.Server.Transfer("~/timeout.aspx");
                    }
                }
            }
            return result;
        }

        protected nature_of_visit_type NatureOfLanding(string expected_session_item_name, bool be_timeout_behavior_standard)
        {
            nature_of_visit_type result;
            result = NatureOfInvocation(expected_session_item_name, be_timeout_behavior_standard, true, false);
            return result;
        }

        protected nature_of_visit_type NatureOfLanding(string expected_session_item_name)
        {
            return NatureOfLanding(expected_session_item_name, true);
        }

        protected nature_of_visit_type NatureOfVisit(string expected_session_item_name, bool be_timeout_behavior_standard)
        {
            nature_of_visit_type result;
            result = NatureOfInvocation(expected_session_item_name, be_timeout_behavior_standard, false, false);
            return result;
        }

        protected nature_of_visit_type NatureOfVisit(string expected_session_item_name)
        {
            return NatureOfVisit(expected_session_item_name, true);
        }

        protected nature_of_visit_type NatureOfVisitUnlimited(string expected_session_item_name, bool be_timeout_behavior_standard)
        {
            nature_of_visit_type result;
            result = NatureOfInvocation(expected_session_item_name, be_timeout_behavior_standard, false, true);
            return result;
        }

        protected nature_of_visit_type NatureOfVisitUnlimited(string expected_session_item_name)
        {
            return NatureOfVisitUnlimited(expected_session_item_name, true);
        }

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);
        }

        protected void RequireConfirmation(WebControl c, string prompt)
        {
            templatecontrol.RequireConfirmation(c, prompt);
        }

        protected void SessionSet(string name, object value)
        {
            templatecontrol.SessionSet(this.Page, name, value);
        }

    protected string ShieldedQueryStringOfHashtable(Hashtable hash_table)
      {
      return templatecontrol.ShieldedQueryStringOfHashtable(Page,hash_table);
      }

        protected string StringOfControl(Control c)
        {
            string result;
            result = templatecontrol.StringOfControl(c);
            return result;
        }

        protected void TransferToPageBinderTab(string page_nick, string binder_nick, uint tab_index)
        {
            templatecontrol.TransferToPageBinderTab(this.Page, page_nick, binder_nick, tab_index);
        }

        protected void ValidationAlert(bool be_using_scriptmanager)
        {
            templatecontrol.ValidationAlert(this.Page, be_using_scriptmanager);
        }

        protected void ValidationAlert()
        {
            ValidationAlert(false);
        }

        public enum nature_of_visit_type
        {
            VISIT_COLD_CALL,
            VISIT_INITIAL,
            VISIT_POSTBACK_STANDARD,
            VISIT_POSTBACK_STALE,
        } // end nature_of_visit_type

    } // end page_class

    // --------------------------------------------------------------------------------------------------------------------------------
    // usercontrol_class
    // --------------------------------------------------------------------------------------------------------------------------------
    public class usercontrol_class: System.Web.UI.UserControl
    {
        private templatecontrol_class templatecontrol = null;
        // ==================================================================================================================================
        // USERCONTROL_CLASS
        // ==================================================================================================================================
        //Constructor  Create()
        public usercontrol_class() : base()
        {
            templatecontrol = new templatecontrol_class();
        }
        protected string AddIdentifiedControlToPlaceHolder(Control c, string id, PlaceHolder p)
        {
            string result;
            result = templatecontrol.AddIdentifiedControlToPlaceHolder(c, id, p);
            return result;
        }

        protected void Alert(k.alert_cause_type cause, k.alert_state_type state, string key, string value, bool be_using_scriptmanager)
        {
            templatecontrol.Alert(this.Page, cause, state, key, value, be_using_scriptmanager);
        }

        protected void Alert(k.alert_cause_type cause, k.alert_state_type state, string key, string value)
        {
            Alert(cause, state, key, value, false);
        }

        protected string AlertMessage(k.alert_cause_type cause, k.alert_state_type state, string key, string value)
        {
            string result;


            result = templatecontrol.AlertMessage(ConfigurationManager.AppSettings["application_name"], cause, state, key, value);
            return result;
        }

        protected void BackTrack(uint num_backsteps)
        {
            uint i;
            string p;
            p = "~/Default.aspx";
            if ((this.Session["waypoint_stack"] != null))
            {
                for (i = 1; i <= num_backsteps; i ++ )
                {
                    if (((this.Session["waypoint_stack"]) as Stack).Count > 0)
                    {
                        p = ((this.Session["waypoint_stack"]) as Stack).Pop().ToString();
                    }
                }
            }
            this.Server.Transfer(p);
        }

        protected void BackTrack()
        {
            BackTrack(1);
        }

        protected void DropCrumbAndTransferTo
          (
          string the_path,
          string anchor_name
          )
          {
          ((Session["waypoint_stack"]) as Stack).Push(Path.GetFileName(Request.CurrentExecutionFilePath));
          if (anchor_name == k.EMPTY)
            {
            Server.Transfer(the_path);
            }
          else
            {
            Response.Redirect(the_path + "#" + anchor_name);
            }
          }
        protected void DropCrumbAndTransferTo(string the_path)
          {
          DropCrumbAndTransferTo(the_path,k.EMPTY);
          }

        protected void EstablishClientSideFunction(string profile, string body)
        {
            templatecontrol.EstablishClientSideFunction(this.Page, profile, body, this.ClientID);
        }

        protected void EstablishClientSideFunction(k.client_side_function_enumeral_type enumeral)
        {
            templatecontrol.EstablishClientSideFunction(this.Page, enumeral);
        }

        protected void EstablishClientSideFunction(k.client_side_function_rec_type r)
        {
            templatecontrol.EstablishClientSideFunction(this.Page, r);
        }

        protected void EstablishGoogleWebFontLoader(string web_font_config)
          {
          templatecontrol.EstablishGoogleWebFontLoader(this.Page,web_font_config);
          }

        public void EstablishUpdatePanelCompliantTimeoutHandler
          (
          int redirect_timeout,
          string path_to_timeout_page
          )
          {
          templatecontrol.EstablishUpdatePanelCompliantTimeoutHandler(this.Page,redirect_timeout,path_to_timeout_page);
          }

        protected void ExportToExcel(System.Web.UI.Page the_page, string filename_sans_extension, string excel_string)
        {
            templatecontrol.ExportToExcel(this.Page, filename_sans_extension, excel_string);
        }

        protected void FileDownload(Page the_page, string filename)
        {
            templatecontrol.FileDownload(this.Page, filename);
        }

        protected void Focus(Control c, bool be_using_scriptmanager)
        {
            templatecontrol.Focus(this.Page, c, be_using_scriptmanager);
        }

        protected void Focus(Control c)
        {
            Focus(c, false);
        }

        public void MessageDropCrumbAndTransferTo
          (
          object msg,
          string folder_name,
          string aspx_name,
          string anchor_name
          )
          {
          SessionSet("msg_" + folder_name + "." + aspx_name,msg);
          DropCrumbAndTransferTo(aspx_name + ".aspx",anchor_name);
          }
        public void MessageDropCrumbAndTransferTo
          (
          object msg,
          string folder_name,
          string aspx_name
          )
          {
          MessageDropCrumbAndTransferTo(msg,folder_name,aspx_name,k.EMPTY);
          }

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);
        }

        protected void RequireConfirmation(WebControl c, string prompt)
        {
            templatecontrol.RequireConfirmation(c, prompt);
        }

        protected void SessionSet(string name, object value)
        {
            templatecontrol.SessionSet(this.Page, name, value);
        }

    protected string ShieldedQueryStringOfHashtable(Hashtable hash_table)
      {
      return templatecontrol.ShieldedQueryStringOfHashtable(Page,hash_table);
      }

        protected string StringOfControl(Control c)
        {
            string result;
            result = templatecontrol.StringOfControl(c);
            return result;
        }

        protected void TransferToPageBinderTab(string page_nick, string binder_nick, uint tab_index)
        {
            templatecontrol.TransferToPageBinderTab(this.Page, page_nick, binder_nick, tab_index);
        }

        protected void ValidationAlert(bool be_using_scriptmanager)
        {
            templatecontrol.ValidationAlert(this.Page, be_using_scriptmanager);
        }

        protected void ValidationAlert()
        {
            ValidationAlert(false);
        }

    } // end usercontrol_class

}