using System.Web.UI.WebControls;

namespace ki.web_ui
  {
  public abstract class usercontrol_subjoined_attribute_capture_class : usercontrol_class
    {

    public abstract void ServerValidate
      (
      object source,
      ServerValidateEventArgs args
      );

    public abstract void Submit();

    }
  }
