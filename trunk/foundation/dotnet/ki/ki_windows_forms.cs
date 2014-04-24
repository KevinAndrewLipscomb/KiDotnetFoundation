using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace ki_windows_forms
  {

  public class webbrowsercontext_class : ApplicationContext
    {

    //--
    //
    // PRIVATE
    //
    //--

    int navigation_counter;
    Thread the_thread;

    // initialize the WebBrowser
    private void Init()
      {
      browser = new WebBrowser();
      // set WebBrowser event handlers
      browser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(web_browser_DocumentCompleted);
      browser.Navigating += new WebBrowserNavigatingEventHandler(web_browser_Navigating);
      // initialise the navigation counter
      navigation_counter = 0;
      }

    // Navigating event handle
    void web_browser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
      {
      // navigation count increases by one
      navigation_counter++;
      }

    // DocumentCompleted event handle
    void web_browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
      {
      }

    //--
    //
    // PROTECTED
    //
    //--

    // dipose the WebBrowser control and the form and its controls
    protected override void Dispose(bool disposing)
      {
      if (the_thread == null)
        {
        Marshal.Release(browser.Handle);
        Dispose();
        base.Dispose(disposing);
        }
      else
        {
        the_thread.Abort();
        the_thread = null;
        }
      }

    //--
    //
    // PUBLIC
    //
    //--

    public WebBrowser browser;

    public int NavigationCounter
      {
      get { return navigation_counter; }
      }

    /// <summary>
    /// class constructor 
    /// </summary>
    /// <param name="auto_reset_event">functionality to keep the main thread waiting</param>
    public webbrowsercontext_class()
      {
      the_thread = new Thread
        (
        new ThreadStart
          (
          delegate
            {
            Init();
            Application.Run(this); 
            }
          )
        );
      // set thread to STA state before starting
      the_thread.SetApartmentState(ApartmentState.STA);
      the_thread.Start();
      }

    }

  }
