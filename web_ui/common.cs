using kix;

namespace ki.web_ui
  {
  static class common
    {

    public const string APP_HANDLED_ASYNC_POST_BACK_ERROR_MESSAGE_HEADER_VALUE = "App-Handled-Message";
      // For use with CUSTOM_RESPONSE_HEADER_NAME if we can ever add this combination as an HTTP partial postback response header .
      // Must correspond to the APP_HANDLED_ERROR_MESSAGE_LINE in ErrorHandler.js
    public const string CUSTOM_RESPONSE_HEADER_NAME = "KiAspdotnetFramework-Pragma";
    public const string SESSION_INTERRUPTED_HEADER_VALUE = "Session-Interrupted";
    public const string VALIDATION_ALERT = "Something about the data you just submitted is invalid.  Look for !ERR! indications near the data fields.  A more detailed explanation may appear near the top of the page.";
    //
    public const string APP_HANDLED_ASYNC_POST_BACK_ERROR_MESSAGE_MARK = "-=:" + APP_HANDLED_ASYNC_POST_BACK_ERROR_MESSAGE_HEADER_VALUE + ":=-" + k.NEW_LINE;
      // For use as a workaround since we can't alter HTTP partial postback headers.
      // Must correspond to the APP_HANDLED_ERROR_MESSAGE_LINE in ErrorHandler.js

    }
  }
