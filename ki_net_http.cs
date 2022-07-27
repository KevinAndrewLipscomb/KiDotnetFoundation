using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ki_net_http
{

  static class common
    {
    }
  
  // ==================================================================================================================================
  // httpresponsemessage__class
  // ==================================================================================================================================
  public class httpresponsemessage_class : HttpResponseMessage
    {

    public httpresponsemessage_class() : base() // CONSTRUCTOR
      {
      base.Headers.CacheControl = new CacheControlHeaderValue()
        {
        MaxAge = TimeSpan.Zero,
        MustRevalidate = true,
        NoCache = true,
        NoStore = true,
        };
      }

    } // end httpresponsemessage_class

  }
