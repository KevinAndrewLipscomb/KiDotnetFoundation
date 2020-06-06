﻿using kix;
using System;
using System.Web;

namespace ki
  {
  public class php_app_module : IHttpModule
    {
    /// <summary>
    /// You will need to configure this module in the Web.config file of your
    /// web and register it with IIS before being able to use it. For more information
    /// see the following link: https://go.microsoft.com/?linkid=8101007
    /// </summary>
    #region IHttpModule Members

    public void Dispose()
      {
      //clean-up code here.
      }

    public void Init(HttpApplication context)
      {
      // Below is an example of how you can handle LogRequest event and provide 
      // custom logging implementation for it
      context.Error += new EventHandler(OnError);
      }

    #endregion

    public void OnError(Object source, EventArgs e)
      {
      var http_application_source = (source as HttpApplication);
      k.EscalatedException
        (
        the_exception:http_application_source.Server.GetLastError(),
        user_identity_name:http_application_source.Request.Url.ToString()
        );
      http_application_source.Response.Redirect(url:"~/exception.phtml");
      }
    }
  }