using System;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections;
using kix;
namespace ki_web_ui
{
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

        public void Alert(Page the_page, alert_cause_type cause, alert_state_type state, string key, string value, bool be_using_scriptmanager)
        {
            string script;


            script = kix.Units.kix.EMPTY
            + "alert(\""
            + AlertMessage(ConfigurationManager.AppSettings["application_name"], cause, state, key, value)
              .Replace(Convert.ToString(kix.Units.kix.NEW_LINE), "\\n")
              .Replace(kix.Units.kix.TAB, "\\t") + "\");";
            if (be_using_scriptmanager)
            {


                ScriptManager.RegisterStartupScript(the_page, the_page.GetType(), key, script, true);
            }
            else
            {
                the_page.ClientScript.RegisterStartupScript(the_page.GetType(), key, script, true);
            }

        }

        public void Alert(Page the_page, alert_cause_type cause, alert_state_type state, string key, string value)
        {
            Alert(the_page, cause, state, key, value, false);
        }

        public string AlertMessage(string application_name, alert_cause_type cause, alert_state_type state, string key, string s)
        {
            string result;









            result = kix.Units.kix.EMPTY
            + "- - - ---------------------------------------------------- - - -"
            + kix.Units.kix.NEW_LINE
            + "       issuer:  " + kix.Units.kix.TAB + application_name + kix.Units.kix.NEW_LINE
            + "       cause:   " + kix.Units.kix.TAB + ((alert_cause_type)(cause)).ToString().ToLower() + kix.Units.kix.NEW_LINE
            + "       state:   " + kix.Units.kix.TAB + ((alert_state_type)(state)).ToString().ToLower() + kix.Units.kix.NEW_LINE
            + "       key:     " + kix.Units.kix.TAB + key.ToLower() + kix.Units.kix.NEW_LINE
            + "       time:    " + kix.Units.kix.TAB + DateTime.Now.ToString("s") + kix.Units.kix.NEW_LINE
            + "- - - ---------------------------------------------------- - - -" + kix.Units.kix.NEW_LINE
            + kix.Units.kix.NEW_LINE
            + kix.Units.kix.NEW_LINE
            + s + kix.Units.kix.NEW_LINE
            + kix.Units.kix.NEW_LINE;
            return result;
        }

        public void EstablishClientSideFunction(Page the_page, string profile, string body, string usercontrol_clientid)
        {
            the_page.ClientScript.RegisterClientScriptBlock
              (
              the_page.GetType(),
              usercontrol_clientid + "__" + profile.Remove(profile.IndexOf(kix.Units.kix.OPEN_PARENTHESIS)),
              "function " + profile + kix.Units.kix.NEW_LINE
              + " {" + kix.Units.kix.NEW_LINE
              + ' ' + body.Replace(Convert.ToString(kix.Units.kix.NEW_LINE), Convert.ToString(kix.Units.kix.NEW_LINE + kix.Units.kix.SPACE)) + kix.Units.kix.NEW_LINE
              + " }" + kix.Units.kix.NEW_LINE,
              true
              );
        }

        public void EstablishClientSideFunction(Page the_page, string profile, string body)
        {
            EstablishClientSideFunction(the_page, profile, body, "");
        }

        public void EstablishClientSideFunction(Page the_page, client_side_function_enumeral_type enumeral)
        {
            switch(enumeral)
            {
                case kix.client_side_function_enumeral_type.EL:
                    EstablishClientSideFunction(the_page, "El(id)", "return document.getElementById(id);");
                    break;
                case kix.client_side_function_enumeral_type.KGS_TO_LBS:
                    EstablishClientSideFunction(the_page, "KgsToLbs(num_kgs)", "return Math.round(+num_kgs*2.204622);");
                    break;
                case kix.client_side_function_enumeral_type.LBS_TO_KGS:
                    EstablishClientSideFunction(the_page, "LbsToKgs(num_lbs)", "return Math.round(+num_lbs/2.204622);");
                    break;
            }
        }

        public void EstablishClientSideFunction(Page the_page, client_side_function_rec_type r)
        {
            EstablishClientSideFunction(the_page, r.profile, r.body);
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





            c.Attributes.Add("onclick", "return confirm(\"- - - ---------------------------------------------------- - - -\\n" + "       issuer:  \\t" + ConfigurationManager.AppSettings["application_name"] + "\\n" + "       state:   \\twarning\\n" + "       time:    \\t" + DateTime.Now.ToString("s") + "\\n" + "- - - ---------------------------------------------------- - - -\\n\\n\\n" + prompt.Replace(Convert.ToString(kix.Units.kix.NEW_LINE), "\\n") + "\\n\\n\"" + ");");
        }

        public void SessionSet(Page the_page, string name, object value)
        {
            the_page.Session.Remove(name);
            the_page.Session.Add(name, value);
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
            Alert(the_page, kix.alert_cause_type.USER, kix.alert_state_type.FAILURE, "stdsvrval", Units.ki_web_ui.STD_VALIDATION_ALERT, be_using_scriptmanager);
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

        protected void Alert(alert_cause_type cause, alert_state_type state, string key, string value, bool be_using_scriptmanager)
        {
            templatecontrol.Alert(this.Page, cause, state, key, value, be_using_scriptmanager);
        }

        protected void Alert(alert_cause_type cause, alert_state_type state, string key, string value)
        {
            Alert(cause, state, key, value, false);
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

        protected void BeginBreadCrumbTrail()
        {
            SessionSet("waypoint_stack", new Stack());
        }

        protected void DropCrumbAndTransferTo(string the_path)
        {
            string current;
            current = Path.GetFileName(this.Request.CurrentExecutionFilePath);
            if ((((this.Session["waypoint_stack"]) as Stack).Count == 0) || (((this.Session["waypoint_stack"]) as Stack).Peek().ToString() != current))
            {
                ((this.Session["waypoint_stack"]) as Stack).Push(current);
            }
            this.Server.Transfer(the_path);
        }

        protected void EstablishClientSideFunction(string profile, string body)
        {
            templatecontrol.EstablishClientSideFunction(this.Page, profile, body);
        }

        protected void EstablishClientSideFunction(client_side_function_enumeral_type enumeral)
        {
            templatecontrol.EstablishClientSideFunction(this.Page, enumeral);
        }

        protected void EstablishClientSideFunction(client_side_function_rec_type r)
        {
            templatecontrol.EstablishClientSideFunction(this.Page, r);
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

        protected void Alert(alert_cause_type cause, alert_state_type state, string key, string value, bool be_using_scriptmanager)
        {
            templatecontrol.Alert(this.Page, cause, state, key, value, be_using_scriptmanager);
        }

        protected void Alert(alert_cause_type cause, alert_state_type state, string key, string value)
        {
            Alert(cause, state, key, value, false);
        }

        protected string AlertMessage(alert_cause_type cause, alert_state_type state, string key, string value)
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

        protected void DropCrumbAndTransferTo(string the_path)
        {
            ((this.Session["waypoint_stack"]) as Stack).Push(Path.GetFileName(this.Request.CurrentExecutionFilePath));
            this.Server.Transfer(the_path);
        }

        protected void EstablishClientSideFunction(string profile, string body)
        {
            templatecontrol.EstablishClientSideFunction(this.Page, profile, body, this.ClientID);
        }

        protected void EstablishClientSideFunction(client_side_function_enumeral_type enumeral)
        {
            templatecontrol.EstablishClientSideFunction(this.Page, enumeral);
        }

        protected void EstablishClientSideFunction(client_side_function_rec_type r)
        {
            templatecontrol.EstablishClientSideFunction(this.Page, r);
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

namespace ki_web_ui.Units
{
    public class ki_web_ui
    {
        public const string STD_VALIDATION_ALERT = "Something about the data you just submitted is invalid.  Look for !ERR! indications near the data fields.  A more detailed explanation may appear near the top of the page.";
    } // end ki_web_ui

}

