using System.Web.UI.WebControls;

namespace ki.web_ui
  {
  static public class extensions_class
    {

    static public string UntieKeyFromSelection
      (
      this ListControl wandering_list_control
      )
      {
      return wandering_list_control.SelectedValue.Split(':')[0];
      }

    static public string UntieValueFromSelection
      (
      this ListControl wandering_list_control
      )
      {
      return wandering_list_control.SelectedValue.Split(':')[1];
      }

    static public void TieKeyToPotentialValuesForWanderingListControl
      (
      this ListControl wandering_list_control,
      string key
      )
      {
      foreach (ListItem item in wandering_list_control.Items)
        {
        item.Value = $"{key}:{item.Value}";
        }
      }

    }
  }
