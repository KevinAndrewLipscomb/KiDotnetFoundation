using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.SessionState;
using System.Web.UI;

namespace kix
{
    public static class k
    {
        public const string ACUTE_ACCENT = "\u00B4"; // ´
        public const string APOSTROPHE = "\'";
        public const string CENT_SIGN = "\u00A2"; // ¢
        public const string COMMA= ",";
        public const string COMMA_SPACE = ", ";
        public const string DIAERESIS = "\u00A8"; // ¨
        public const string DOUBLE_APOSTROPHE = "''";
        public const string DOUBLE_QUOTE = "\"\"";
        public const string EMPTY = "";
        public const string HYPHEN = "-";
        public const string INVERTED_EXCLAMATION_MARK = "\u00A1"; // ¡
        public const string NEW_LINE = "\n";
        public const string OPEN_PARENTHESIS = "(";
        public const string PERIOD = ".";
        public const string QUOTE = "\"";
        public const string SEMICOLON = ";";
        public const string SPACE = "\u0020";
        public const string SPACE_HYPHEN_SPACE = " - ";
        public const string SPACE_HYPHENS_SPACE = " -- ";
        public const string TAB = "\t";

        public struct client_side_function_rec_type
        {
            public string profile;
            public string body;
        } // end client_side_function_rec_type

        // ´
        // ¢
        // ¨
        // ¡
        public enum alert_cause_type
        {
            APPDATA,
            DBMS,
            FILESYSTEM,
            LOGIC,
            MEMORY,
            NETWORK,
            OTHER,
            USER,
        } // end alert_cause_type

        public enum alert_state_type
        {
            NORMAL,
            SUCCESS,
            WARNING,
            FAILURE,
            DAMAGE,
        } // end alert_state_type

        public enum client_side_function_enumeral_type
        {
            EL,
            KGS_TO_LBS,
            LBS_TO_KGS,
        } // end client_side_function_enumeral_type

        public enum safe_hint_type
        {
            NONE,
            ALPHA,
            ALPHA_WORDS,
            ALPHANUM,
            CURRENCY_USA,
            DATE_TIME,
            ECMASCRIPT_WORD,
            EMAIL_ADDRESS,
            FINANCIAL_TERMS,
            HOSTNAME,
            HTTP_TARGET,
            HUMAN_NAME,
            HUMAN_NAME_CSV,
            HYPHENATED_ALPHA,
            HYPHENATED_ALPHA_WORDS,
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
            POSTAL_CITY_CSV,
            POSTAL_STREET_ADDRESS,
            PUNCTUATED,
            REAL_NUM,
            REAL_NUM_INCLUDING_NEGATIVE,
        } // end safe_hint_type

        private static bool be_smtp_mail_send_reliable = true;

        public struct subtype<TComparable> where TComparable : IComparable
          {
          //
          private TComparable first;
          private TComparable current;
          private TComparable last;
          //
          public static Exception CONSTRAINT_ERROR = new Exception("kix.k.subtype<TComparable>.CONSTRAINT_ERROR");
          public subtype
            (
            TComparable the_first,
            TComparable the_last
            )
            {
            first = the_first;
            current = the_first;
            last = the_last;
            }
          public TComparable val
            {
            get
              {
              return current;
              }
            set
              {
              if ((value.CompareTo(first) < 0) || (value.CompareTo(last) > 0))
                {
                throw CONSTRAINT_ERROR;
                }
              unchecked
                {
                current = value;
                }
              }
            }
          public TComparable FIRST
            {
            get
              {
              return first;
              }
            }
          public TComparable LAST
            {
            get
              {
              return last;
              }
            }
          }

        public class int_negative
          {
          private subtype<int> current;
          public int_negative()
            {
            current = new subtype<int>(int.MinValue,-1);
            }
          public int val
            {
            get
              {
              return current.val;
              }
            set
              {
              current.val = value;
              }
            }
          }

        public class int_nonnegative
          {
          private subtype<int> current;
          public int_nonnegative()
            {
            current = new subtype<int>(0,int.MaxValue);
            }
          public int val
            {
            get
              {
              return current.val;
              }
            set
              {
              current.val = value;
              }
            }
          }

        public class int_nonpositive
          {
          private subtype<int> current;
          public int_nonpositive()
            {
            current = new subtype<int>(int.MinValue,0);
            }
          public int val
            {
            get
              {
              return current.val;
              }
            set
              {
              current.val = value;
              }
            }
          }

        public class int_positive
          {
          private subtype<int> current;
          public int_positive()
            {
            current = new subtype<int>(1,int.MaxValue);
            }
          public int val
            {
            get
              {
              return current.val;
              }
            set
              {
              current.val = value;
              }
            }
          }

        public class int_sign_range
          {
          private subtype<int> current;
          public int_sign_range()
            {
            current = new subtype<int>(-1,1);
            }
          public int val
            {
            get
              {
              return current.val;
              }
            set
              {
              current.val = value;
              }
            }
          }

        public class decimal_nonnegative
          {
          private subtype<decimal> current;
          public decimal_nonnegative()
            {
            current = new subtype<decimal>(0,decimal.MaxValue);
            }
          public decimal val
            {
            get
              {
              return current.val;
              }
            set
              {
              current.val = value;
              }
            }
          }

        public class decimal_positive
          {
          private subtype<decimal> current;
          public decimal_positive()
            {
            current = new subtype<decimal>(1,decimal.MaxValue);
            }
          public decimal val
            {
            get
              {
              return current.val;
              }
            set
              {
              current.val = value;
              }
            }
          }

        public static decimal AverageDeviation(ArrayList array_list, decimal median_value)
        {
            decimal result;
            int i;
            int n;
            decimal sum;
            n = array_list.Count;
            sum = 0;
            for (i = 0; i <= (n - 1); i ++ )
            {
                sum = sum + Math.Abs((decimal)(array_list[i]) - median_value);
            }
            result = sum / n;
            return result;
        }

        public static bool BeValidDomainPartOfEmailAddress(string email_address)
        {
            bool result;
            ProcessStartInfo process_start_info;
            string significant_part;
            Process work;
            significant_part = email_address.Substring(email_address.LastIndexOf('@') + 1);
            process_start_info = new ProcessStartInfo("nslookup", "-type=MX " + significant_part.Trim());
            process_start_info.RedirectStandardOutput = true;
            process_start_info.RedirectStandardError = true;
            process_start_info.UseShellExecute = false;
            work = Process.Start(process_start_info);
            work.WaitForExit();
            work.Refresh();
            result = work.StandardOutput.ReadToEnd().Contains("mail exchanger = ");

            return result;
        }

        public static bool BeValidDomainPartOfWebAddress(string web_address)
        {
            bool result;
            const int LENGTH_OF_SHORTEST_PRACTICAL_DOMAIN_NAME = 4;
            // a.com
            bool be_valid_domain_part_of_web_address;
            int index_of_slash;
            int significant_length;
            be_valid_domain_part_of_web_address = true;
            index_of_slash = web_address.IndexOf('/');
            if (index_of_slash > LENGTH_OF_SHORTEST_PRACTICAL_DOMAIN_NAME)
            {
                significant_length = index_of_slash;
            }
            else
            {
                significant_length = web_address.Length;
            }
            try {
                Dns.GetHostEntry(web_address.Substring(0, significant_length));
            }
            catch {
                be_valid_domain_part_of_web_address = false;
            }
            result = be_valid_domain_part_of_web_address;
            return result;
        }

        public static bool BeValidNanpNumber(string s)
        {
            bool result;
            bool be_valid_nanp_number;
            string digits;
            //uint nanp_nxx_start;
            var nanp_nxx_start = new subtype<int>(0,3);
            be_valid_nanp_number = false;
            digits = Safe(s, safe_hint_type.NUM);
            nanp_nxx_start.val = 0;
            switch(digits.Length)
            {
                case 7:
                    // These rules are taken from http://en.wikipedia.org/wiki/North_American_Numbering_Plan
                    be_valid_nanp_number = true;
                    break;
                case 10:
                    be_valid_nanp_number = (digits.Substring(0, 1).ToCharArray()[0] >= '2') && (digits.Substring(1, 1).ToCharArray()[0] <= '8') && (digits.Substring(0, 3) != "900");
                    nanp_nxx_start.val = 3;
                    break;
            }
            result = be_valid_nanp_number
              && (digits.Substring(nanp_nxx_start.val, 1).ToCharArray()[0] >= '2')
              && !(digits.Substring((nanp_nxx_start.val + 1), 2) == "11")
              && !((digits.Substring(nanp_nxx_start.val).CompareTo("5550100") >= 0) && (digits.Substring(nanp_nxx_start.val).CompareTo("5550199") <= 0))
              && (digits.Substring(nanp_nxx_start.val) != "5551212")
              && (digits.Substring(nanp_nxx_start.val, 3) != "976");

            return result;
        }

        public static bool BooleanOfYesNo(string yn)
        {
            bool result;
            result = (yn.ToUpper() == "YES");
            return result;
        }

        public static string Digest(string source_string)
        {
            string result;
            byte[] byte_buf = new byte[20 + 1];
            uint i;
            string target_string;
            target_string = EMPTY;
            byte_buf = new SHA1Managed().ComputeHash(new ASCIIEncoding().GetBytes(source_string));
            for (i = 1; i <= 20; i ++ )
            {
                target_string = target_string + byte_buf[i - 1].ToString("x2");
            }
            result = target_string;
            return result;
        }

        public static string DomainNameOfIpAddress(string ip_address)
        {
            string result;
            result = Dns.GetHostEntry(ip_address).HostName;
            return result;
        }

        public static string EscalatedException(System.Exception the_exception, string user_identity_name, HttpSessionState session)
          {
          var user_designator = k.EMPTY;
          if (user_identity_name == EMPTY)
            {
            user_designator = "unknown";
            }
          else
            {
            user_designator = user_identity_name;
            }
          var notification_message = "[EXCEPTION]" + NEW_LINE + the_exception.ToString() + NEW_LINE + NEW_LINE + "[HRESULT]" + NEW_LINE + HresultAnalysis(the_exception) + NEW_LINE + NEW_LINE;
          if (user_identity_name != EMPTY)
            {
            notification_message += "[USER]" + NEW_LINE + user_designator + NEW_LINE + NEW_LINE;
            }
          if ((session != null))
            {
            notification_message += "[SESSION]" + NEW_LINE;
            if (session.Count > 0)
              {
              var i = new subtype<int>(0,session.Count);
              for (i.val = 0; i.val <= (session.Count - 1); i.val++ )
                {
                notification_message += session.Keys[i.val].ToString() + " = " + session[i.val].ToString() + NEW_LINE;
                }
              }
            }
          SmtpMailSend(ConfigurationManager.AppSettings["sender_email_address"], ConfigurationManager.AppSettings["sender_email_address"], "EXCEPTION REPORT", notification_message);
          SmtpMailSend(ConfigurationManager.AppSettings["sender_email_address"], ConfigurationManager.AppSettings["sysadmin_sms_address"], "CRASH", user_designator);
          return notification_message;
          }

        public static string EscalatedException(System.Exception the_exception)
        {
            return EscalatedException(the_exception, EMPTY);
        }

        public static string EscalatedException(System.Exception the_exception, string user_identity_name)
        {
            return EscalatedException(the_exception, user_identity_name, null);
        }

        public static string ExpandAsperand(string s)
        {
            string result;


            result = s.Replace("@", ConfigurationManager.AppSettings["runtime_root_fullspec"]);
            return result;
        }

        public static string ExpandTildePath(string s)
        {
            string result;


            result = s.Replace("\\", "/").Replace("~", "/" + ConfigurationManager.AppSettings["virtual_directory_name"]);
            return result;
        }

        public static string FormatAsNanpPhoneNum(string digits, bool be_for_international_audience)
        {
            string result;
            string format_as_nanp_phone_num;
            format_as_nanp_phone_num = EMPTY;
            if ((digits.Length == 10) && BeValidNanpNumber(digits))
            {
                if (be_for_international_audience)
                {
                    format_as_nanp_phone_num = "+1-";
                }
                format_as_nanp_phone_num = format_as_nanp_phone_num + digits.Substring(0, 3) + HYPHEN + digits.Substring(3, 3) + HYPHEN + digits.Substring(6);
            }
            result = format_as_nanp_phone_num;
            return result;
        }

        public static string FormatAsNanpPhoneNum(string digits)
        {
            return FormatAsNanpPhoneNum(digits, false);
        }

        public static bool Has(string[] the_string_array, string the_string)
        {
            bool result;
            uint i;
            uint len;
            result = false;
            if (the_string_array != null)
            {

                len = (uint)((System.Array)(the_string_array)).Length;
                i = 0;
                while ((i < len) && (the_string_array[i] != the_string))
                {
                    i = i + 1;
                }
                result = (i < len);
            }
            return result;
        }

        public static string HresultAnalysis(System.Exception the_exception)
          {
          unchecked
            {
            string result;
            string code;
            string facility;
            uint facility_id;
            uint hresult;
            bool be_ntstatus;
            string microsoft_or_not;
            string severity;
            facility = "unknown";
            severity = EMPTY;

            hresult = ((uint)(Marshal.GetHRForException(the_exception)));
            be_ntstatus = ((hresult & 0x10000000) != 0);


            code = ((ushort)(hresult % 0x00010000)).ToString("X");
            if (!be_ntstatus)
            {
                // "N": it's not an NTSTATUS
                if ((hresult & 0x80000000) != 0)
                {
                    // "S"
                    severity = "FAILURE";
                }
                else
                {
                    severity = "SUCCESS";
                }
            }
            else
            {
                switch(hresult / 0xC0000000)
                {
                    case 3:
                        // NTSTATUS "Sev"
                        severity = "ERROR";
                        break;
                    case 2:
                        severity = "WARNING";
                        break;
                    case 1:
                        severity = "INFO";
                        break;
                    case 0:
                        severity = "SUCCESS";
                        break;
                }
            }
            if ((hresult & 0x20000000) != 0)
            {
                // "C"
                microsoft_or_not = "NONMICROSOFT";
            }
            else
            {
                microsoft_or_not = "MICROSOFT";
                facility_id = (hresult % 0x10000000) / 0x00010000;
                if (!be_ntstatus)
                {
                    switch(facility_id)
                    {
                        case 00:
                            facility = "NULL";
                            break;
                        case 01:
                            facility = "RPC";
                            break;
                        case 02:
                            facility = "DISPATCH";
                            break;
                        case 03:
                            facility = "STORAGE";
                            break;
                        case 04:
                            facility = "ITF";
                            break;
                        case 07:
                            facility = "WIN32";
                            break;
                        case 08:
                            facility = "WINDOWS";
                            break;
                        case 09:
                            facility = "SECURITY|SSPI";
                            break;
                        case 10:
                            facility = "CONTROL";
                            break;
                        case 11:
                            facility = "CERT";
                            break;
                        case 12:
                            facility = "INTERNET";
                            break;
                        case 13:
                            facility = "MEDIASERVER";
                            break;
                        case 14:
                            facility = "MSMQ";
                            break;
                        case 15:
                            facility = "SETUPAPI";
                            break;
                        case 16:
                            facility = "SCARD";
                            break;
                        case 17:
                            facility = "COMPLUS";
                            break;
                        case 18:
                            facility = "AAF";
                            break;
                        case 19:
                            facility = "URT";
                            break;
                        case 20:
                            facility = "ACS";
                            break;
                        case 21:
                            facility = "DPLAY";
                            break;
                        case 22:
                            facility = "UMI";
                            break;
                        case 23:
                            facility = "SXS";
                            break;
                        case 24:
                            facility = "WINDOWS_CE";
                            break;
                        case 25:
                            facility = "HTTP";
                            break;
                        case 26:
                            facility = "USERMODE_COMMONLOG";
                            break;
                        case 31:
                            facility = "USERMODE_FILTER_MANAGER";
                            break;
                        case 32:
                            facility = "BACKGROUNDCOPY";
                            break;
                        case 33:
                            facility = "CONFIGURATION";
                            break;
                        case 34:
                            facility = "STATE_MANAGEMENT";
                            break;
                        case 35:
                            facility = "METADIRECTORY";
                            break;
                        case 36:
                            facility = "WINDOWSUPDATE";
                            break;
                        case 37:
                            facility = "DIRECTORYSERVICE";
                            break;
                        case 38:
                            facility = "GRAPHICS";
                            break;
                        case 39:
                            facility = "SHELL";
                            break;
                        case 40:
                            facility = "TPM_SERVICES";
                            break;
                        case 41:
                            facility = "TPM_SOFTWARE";
                            break;
                        case 48:
                            facility = "PLA";
                            break;
                        case 49:
                            facility = "FVE";
                            break;
                        case 50:
                            facility = "FWP";
                            break;
                        case 51:
                            facility = "WINRM";
                            break;
                        case 52:
                            facility = "NDIS";
                            break;
                        case 53:
                            facility = "USERMODE_HYPERVISOR";
                            break;
                        case 54:
                            facility = "CMI";
                            break;
                        case 80:
                            facility = "WINDOWS_DEFENDER";
                            break;
                    }
                }
                else
                {
                    switch(facility_id)
                    {
                        case 01:
                            // is an NTSTATUS
                            facility = "DEBUGGER";
                            break;
                        case 02:
                            facility = "RPC_RUNTIME";
                            break;
                        case 03:
                            facility = "RPC_STUBS";
                            break;
                        case 04:
                            facility = "IO";
                            break;
                        case 07:
                            facility = "NTWIN32";
                            break;
                        case 09:
                            facility = "NTSSPI";
                            break;
                        case 10:
                            facility = "TERMINAL_SERVER";
                            break;
                        case 11:
                            facility = "MUI";
                            break;
                        case 16:
                            facility = "USB";
                            break;
                        case 17:
                            facility = "HID";
                            break;
                        case 18:
                            facility = "FIREWIRE";
                            break;
                        case 19:
                            facility = "CLUSTER";
                            break;
                        case 20:
                            facility = "ACPI";
                            break;
                        case 21:
                            facility = "SXS";
                            break;
                        case 25:
                            facility = "TRANSACTION";
                            break;
                        case 26:
                            facility = "COMMONLOG";
                            break;
                        case 27:
                            facility = "VIDEO";
                            break;
                        case 28:
                            facility = "FILTER_MANAGER";
                            break;
                        case 29:
                            facility = "MONITOR";
                            break;
                        case 30:
                            facility = "GRAPHICS_KERNEL";
                            break;
                        case 32:
                            facility = "DRIVER_FRAMEWORK";
                            break;
                        case 33:
                            facility = "FVE";
                            break;
                        case 34:
                            facility = "FWP";
                            break;
                        case 35:
                            facility = "NDIS";
                            break;
                        case 53:
                            facility = "HYPERVISOR";
                            break;
                        case 54:
                            facility = "IPSEC";
                            break;
                        case 55:
                            facility = "MAXIMUM_VALUE";
                            break;
                    }
                }
            }

            result = "0x" + hresult.ToString("X") + ":  %" + microsoft_or_not + '.' + facility + "--" + severity + "--0x" + code;
            return result;
            }
          }

        public static decimal Median(ArrayList sorted_array_list)
        {
            decimal result;
            result = Percentile(50, sorted_array_list);
            return result;
        }

        public static decimal Percentile(uint p, ArrayList sorted_array_list)
        {
            decimal result;
            decimal interpolation_factor;
            int practical_index;
            decimal lower_value;
            int n;
            decimal virtual_index;
            n = sorted_array_list.Count;
            if (n == 0)
            {
                result = 0;
            }
            else
            {
                if (n == 1)
                {

                    result = (decimal)(sorted_array_list[0]);
                }
                else
                {
                    virtual_index = p * (n - 1) / 100;
                    if (virtual_index >= (n - 1))
                    {

                        result = (decimal)(sorted_array_list[n - 1]);
                    }
                    else
                    {




                        practical_index = decimal.ToInt32(decimal.Floor(virtual_index));
                        interpolation_factor = virtual_index - practical_index;

                        lower_value = (decimal)(sorted_array_list[practical_index]);

                        result = lower_value + interpolation_factor * ((decimal)(sorted_array_list[practical_index + 1]) - lower_value);
                    }
                }
            }
            return result;
        }

        public static void RunCommandIteratedOverArguments
          (
          string command,
          ArrayList arguments,
          string working_directory,
          out string stdout,
          out string stderr
          )
          {
          //
          stderr = k.EMPTY;
          stdout = k.EMPTY;
          //
          if (arguments.Count > 0)
            {
            for (uint i = 0; i < arguments.Count; i++)
              {
              ProcessStartInfo process_start_info = new ProcessStartInfo(command, (arguments[(int)i] as string));
              process_start_info.RedirectStandardOutput = true;
              process_start_info.RedirectStandardError = true;
              process_start_info.UseShellExecute = false;
              process_start_info.WorkingDirectory = working_directory;
              Process work = Process.Start(process_start_info);
              work.WaitForExit();
              work.Refresh();
              stderr = stderr + work.StandardError.ReadToEnd() + k.NEW_LINE;
              stdout = stdout + work.StandardOutput.ReadToEnd() + k.NEW_LINE;
              }
            }
          }

        public static string Safe(string source_string, safe_hint_type hint)
        {
            string result;
            string MODIFIED_LIBERAL_SET = "0-9a-zA-Z@#$%&()\\-+=,/.:? " + ACUTE_ACCENT + CENT_SIGN + DIAERESIS + INVERTED_EXCLAMATION_MARK;
            string allow;
            string scratch_string;
            allow = EMPTY;
            switch(hint)
            {
                //
                // Be extremely protective here:
                // -  Escape ("\") the following four characters: ]\^-
                // -  For scalars, do not allow punctuation.
                // -  When in doubt, don't allow it.
                // This routine is not intended to assure that data is submitted in proper
                // format.  It is intended to protect against SQL insertion attacks.
                //
                case safe_hint_type.ALPHA:
                    allow = "a-zA-Z";
                    break;
                case safe_hint_type.ALPHA_WORDS:
                    allow = "a-zA-Z ";
                    break;
                case safe_hint_type.ALPHANUM:
                    allow = "0-9a-zA-Z";
                    break;
                case safe_hint_type.CURRENCY_USA:
                    allow = "0-9$,.";
                    break;
                case safe_hint_type.DATE_TIME:
                    allow = "0-9:\\-/ ";
                    break;
                case safe_hint_type.ECMASCRIPT_WORD:
                    allow = "0-9a-zA-Z_";
                    break;
                case safe_hint_type.EMAIL_ADDRESS:
                    allow = "0-9a-zA-Z_.@\\-+";
                    break;
                case safe_hint_type.FINANCIAL_TERMS:
                    allow = "0-9a-zA-Z@#$%()\\-+=,/. " + CENT_SIGN;
                    break;
                case safe_hint_type.HOSTNAME:
                    allow = "0-9a-zA-Z_\\-.";
                    break;
                case safe_hint_type.HTTP_TARGET:
                    allow = "0-9a-zA-Z\\-_./";
                    break;
                case safe_hint_type.HUMAN_NAME:
                    allow = "a-zA-Z\\-. " + ACUTE_ACCENT;
                    break;
                case safe_hint_type.HUMAN_NAME_CSV:
                    allow = "a-zA-Z\\-,. " + ACUTE_ACCENT;
                    break;
                case safe_hint_type.HYPHENATED_ALPHA:
                    allow = "a-zA-Z\\-";
                    break;
                case safe_hint_type.HYPHENATED_ALPHA_WORDS:
                    allow = "a-zA-Z\\- ";
                    break;
                case safe_hint_type.HYPHENATED_ALPHANUM:
                    allow = "a-zA-Z0-9\\-";
                    break;
                case safe_hint_type.HYPHENATED_NUM:
                    allow = "0-9\\-";
                    break;
                case safe_hint_type.HYPHENATED_UNDERSCORED_ALPHANUM:
                    allow = "a-zA-Z0-9\\-_";
                    break;
                case safe_hint_type.KI_SORT_EXPRESSION:
                    allow = "a-zA-Z_%\\*()\\-+,./ ";
                    break;
                case safe_hint_type.MAKE_MODEL:
                    allow = "0-9a-zA-Z#*()\\-+/. ";
                    break;
                case safe_hint_type.MEMO:
                    allow = MODIFIED_LIBERAL_SET + "\\n\\r\\t";
                    break;
                case safe_hint_type.NUM:
                    allow = "0-9";
                    break;
                case safe_hint_type.ORG_NAME:
                    allow = "0-9a-zA-Z#&\\-,. " + ACUTE_ACCENT;
                    break;
                case safe_hint_type.PHONE_NUM:
                    allow = "0-9-+() ";
                    break;
                case safe_hint_type.POSTAL_CITY:
                    allow = "a-zA-Z\\-. " + ACUTE_ACCENT;
                    break;
                case safe_hint_type.POSTAL_CITY_CSV:
                    allow = "a-zA-Z\\-,. " + ACUTE_ACCENT;
                    break;
                case safe_hint_type.POSTAL_STREET_ADDRESS:
                    allow = "0-9a-zA-Z#\\-,(). " + ACUTE_ACCENT;
                    break;
                case safe_hint_type.PUNCTUATED:
                    allow = MODIFIED_LIBERAL_SET;
                    break;
                case safe_hint_type.REAL_NUM:
                    allow = "0-9.";
                    break;
                case safe_hint_type.REAL_NUM_INCLUDING_NEGATIVE:
                    allow = "0-9.\\-";
                    break;
            }
            if (allow == EMPTY)
            {
                scratch_string = EMPTY;
            }
            else
            {
                scratch_string = source_string;
                scratch_string = scratch_string.Replace(QUOTE, DIAERESIS);
                scratch_string = scratch_string.Replace(APOSTROPHE, ACUTE_ACCENT);
                scratch_string = scratch_string.Replace(SEMICOLON, INVERTED_EXCLAMATION_MARK);
                scratch_string = Regex.Replace(scratch_string, "[^" + allow + ']', EMPTY);
            }
            result = scratch_string;
            return result;
        }

        public static string Safe(string source_string)
        {
            return Safe(source_string, safe_hint_type.NONE);
        }

        public static void SendControlAsAttachmentToEmailMessage(object c, string scratch_pathname, string from_address, string to_target, string cc_target, string subject, string body)
        {
            //
            // For this to work, the ASP.NET Machine Account (ASPNET) [on IIS5] or the NETWORK SERVICE account [on IIS7] must have write permission for the folder specified by p.path.  Configure this on the Security tab of the folder's Properties.  If the Security tab is missing, open Windows Explorer / Tools /
            // Folder Options... / View, and in the Advanced Settings, clear the "Use simple file sharing" checkbox.
            //
            System.IO.StreamWriter streamwriter = new System.IO.StreamWriter(scratch_pathname);
            ((c) as Control).RenderControl(new System.Web.UI.HtmlTextWriter(streamwriter));
            streamwriter.Close();
            //
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(from_address);
            if (to_target != k.EMPTY)
              {
              msg.To.Add(to_target);
              }
            if (cc_target != k.EMPTY)
              {
              msg.CC.Add(cc_target);
              }
            msg.Subject = subject;
            msg.Body = body;
            msg.Attachments.Add(new Attachment(scratch_pathname));
            SmtpMailSend(msg);
            //
            System.IO.File.Delete(scratch_pathname);

        }

        public static void SilentAlarm(System.Exception the_exception)
          {
          be_smtp_mail_send_reliable = false;
          SmtpMailSend(ConfigurationManager.AppSettings["sender_email_address"], ConfigurationManager.AppSettings["sender_email_address"], "SILENT ALARM", "[EXCEPTION]" + NEW_LINE + the_exception.ToString() + NEW_LINE + NEW_LINE + "[HRESULT]" + NEW_LINE + HresultAnalysis(the_exception) + NEW_LINE);
          be_smtp_mail_send_reliable = true;
          }

        public static void SmtpMailSend(MailMessage mail_message)
        {
            //
            // Instead of supplying a Body, which System.Net.Mail encodes as quoted-printable, supply an AlternateView and force its encoding to 7bit.
            //
            AlternateView alternate_view = AlternateView.CreateAlternateViewFromString(mail_message.Body,mail_message.BodyEncoding,(mail_message.IsBodyHtml ? "text/html" : null));
            alternate_view.TransferEncoding = TransferEncoding.SevenBit;
            mail_message.AlternateViews.Add(alternate_view);
            mail_message.Body = null;
            try
              {
              (new SmtpClient(ConfigurationManager.AppSettings["smtp_server"])).Send(mail_message);
              }
            catch(System.Exception e)
              {
              if (be_smtp_mail_send_reliable)
                {
                SilentAlarm(e);
                }
              else
                {
                throw;
                }
              }
            mail_message.Dispose();
        }

        public static void SmtpMailSend(string from, string to, string subject, string message_string, bool be_html, string cc, string bcc, string reply_to)
        {
            const string DOUBLE_COMMA = k.COMMA + k.COMMA;
            MailMessage mail_message = new MailMessage();
            //
            to = to.Trim(new char[] {Convert.ToChar(k.COMMA)}).Replace(k.SPACE,k.EMPTY);
            while (to.Contains(DOUBLE_COMMA))
              {
              to = to.Replace(DOUBLE_COMMA,k.COMMA);
              }
            //
            cc = cc.Trim(new char[] {Convert.ToChar(k.COMMA)}).Replace(k.SPACE,k.EMPTY);
            while (cc.Contains(DOUBLE_COMMA))
              {
              cc = cc.Replace(DOUBLE_COMMA,k.COMMA);
              }
            //
            bcc = bcc.Trim(new char[] {Convert.ToChar(k.COMMA)}).Replace(k.SPACE,k.EMPTY);
            while (bcc.Contains(DOUBLE_COMMA))
              {
              bcc = bcc.Replace(DOUBLE_COMMA,k.COMMA);
              }
            //
            mail_message.From = new MailAddress(from);
            if (to != k.EMPTY)
              {
              mail_message.To.Add(to);
              }
            mail_message.Subject = subject;
            mail_message.Body = message_string;
            mail_message.IsBodyHtml = be_html;
            if (cc != k.EMPTY)
              {
              mail_message.CC.Add(cc);
              }
            if (bcc != k.EMPTY)
              {
              mail_message.Bcc.Add(bcc);
              }
            if (reply_to != k.EMPTY)
              {
              mail_message.ReplyTo = new MailAddress(reply_to);
              }
            SmtpMailSend(mail_message);
        }

        public static void SmtpMailSend(string from, string to, string subject, string message_string)
        {
            SmtpMailSend(from, to, subject, message_string, false);
        }

        public static void SmtpMailSend(string from, string to, string subject, string message_string, bool be_html)
        {
            SmtpMailSend(from, to, subject, message_string, be_html, EMPTY);
        }

        public static void SmtpMailSend(string from, string to, string subject, string message_string, bool be_html, string cc)
        {
            SmtpMailSend(from, to, subject, message_string, be_html, cc, EMPTY);
        }

        public static void SmtpMailSend(string from, string to, string subject, string message_string, bool be_html, string cc, string bcc)
        {
            SmtpMailSend(from, to, subject, message_string, be_html, cc, bcc, EMPTY);
        }

        public static string WrapText
          (
          string
            t,
          string
            insert_string,
          char[]
            break_char_array,
          short
            max_line_len
          )
          {
          var line_list = new List<string>();
          if (t.Length == 0)
            {
            return t;
            }
          else
            {
            var scratch_line = k.EMPTY;
            var source_line_array = t.Split(new string[] {NEW_LINE,"\r\n"},StringSplitOptions.None);
            foreach (var current_phrase in source_line_array)
              {
              var word_array = current_phrase.Split(break_char_array);
              foreach (var current_word in word_array)
                {
                if ((scratch_line.Length > max_line_len) || ((scratch_line.Length + current_word.Length) > max_line_len))
                  {
                  //
                  // Add what has accumulated on the current_line to the line_list, and reinitialize the scratch_line.
                  //
                  line_list.Add(scratch_line);
                  scratch_line = k.EMPTY;
                  }
                if (scratch_line.Length > 0)
                  {
                  //
                  // Append a space and the current_word to the non-empty scratch_line.
                  //
                  scratch_line += k.SPACE + current_word;
                  }
                else
                  {
                  //
                  // Start the empty scratch_line with the current_word.
                  //
                  scratch_line += current_word;
                  }
                }
              if (scratch_line.Length > 0)
                {
                //
                // Add what has accumulated on the current_line to the line_list, and reinitialize the scratch_line.
                //
                line_list.Add(scratch_line);
                scratch_line = k.EMPTY;
                }
              }
            var line_list_count = line_list.Count;
            var i = new subtype<int>(1,line_list_count);
            var wrap_text = k.EMPTY;
            while (i.val < line_list.Count)
              {
              wrap_text += line_list[i.val - 1].Trim() + insert_string;
              i.val++;
              }
            return wrap_text + line_list[i.val - 1];
            }
          }

        public static string YesNoOf(bool b)
        {
            string result;
            if (b)
            {
                result = "Yes";
            }
            else
            {
                result = "No";
            }
            return result;
        }

    } // end k

}

