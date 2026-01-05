using kix;
using System.Data;
using System.Web.UI.WebControls;

namespace ki_web_ui_webcontrols
  {
  static public class extensions_class
    {

    static public void Bind
      (
      this BaseDataList baseDataList,
      object dataSource
      )
      {
      baseDataList.SetDataSource(dataSource).DataBind();
      }

    static public void Bind
      (
      this ListControl listControl,
      DataTable dataSource,
      string selected_id = k.EMPTY,
      bool be_available_option_all = false,
      string unselectedLiteral = "-- Select --"
      )
      {
      listControl.DataValueField = dataSource.Columns[0].ColumnName;
      listControl.DataTextField = dataSource.Columns[1].ColumnName;
      listControl.SetDataSource(dataSource).DataBind();
      if (be_available_option_all && unselectedLiteral.Length > 0)
        {
        listControl.Items.Insert
          (
          index: 0,
          item: new ListItem()
            {
            Text = unselectedLiteral,
            Value = k.EMPTY
            }
          );
        }
      if (selected_id.Length > 0)
        {
        listControl.SelectedValue = selected_id;
        }
      }

    static public void Bind
      (
      this ListItemCollection listItemCollection,
      DataTable dataSource
      )
      {
      var dataValueField = dataSource.Columns[0].ColumnName;
      var dataTextField = dataSource.Columns[1].ColumnName;
      foreach (DataRow dataRow in dataSource.Rows)
        {
        var text = $"{dataRow[dataTextField]}" ?? k.EMPTY;
        var value = $"{dataRow[dataValueField]}" ?? k.EMPTY;
        listItemCollection.Add(item:new ListItem(text,value));
        }
      }

    static public BaseDataList SetDataSource
      (
      this BaseDataList baseDataList,
      object dataSource
      )
      {
      baseDataList.DataSource = dataSource;
      return baseDataList;
      }

    static public BaseDataBoundControl SetDataSource
      (
      this BaseDataBoundControl baseDataBoundControl,
      object dataSource
      )
      {
      baseDataBoundControl.DataSource = dataSource;
      return baseDataBoundControl;
      }

    }
  }
