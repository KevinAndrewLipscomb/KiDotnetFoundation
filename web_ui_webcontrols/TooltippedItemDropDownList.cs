using kix;
using System.Data;
using System.Web.UI.WebControls;

namespace ki.web_ui_webcontrols
  {
  public class TooltippedItemDropDownList : DropDownList
    {

    public ListControl Bind
      (
      DataTable dataSource,
      string selected_id = k.EMPTY,
      bool be_available_option_all = false,
      string unselectedLiteral = "-- Select --",
      extensions.ListItemAttributeNameToDataTableColumnNameMap elaborationAttributesMap = null,
      string elabDescriptionKey = "elab-description",
      string separator = " - "
      )
      {
      var result = (this as DropDownList).Bind(dataSource,selected_id,be_available_option_all,unselectedLiteral,elaborationAttributesMap);
      foreach (ListItem item in result.Items)
        {
        var elabDescription = item.Attributes[elabDescriptionKey];
        var elab_description_clause = ((elabDescription?.Length ?? 0) > 0 ? elabDescription : k.EMPTY);
        item.Attributes.Add("title",$"{item.Text}{separator}{elab_description_clause}");
        }
      return result;
      }

    }
  }
