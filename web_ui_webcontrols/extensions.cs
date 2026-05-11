using kix;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace ki.web_ui_webcontrols
{
  static public class extensions
    {

    static public BaseDataBoundControl Bind
      (
      this BaseDataBoundControl baseDataBoundControl,
      object dataSource
      )
      {
      baseDataBoundControl.SetDataSource(dataSource).DataBind();
      return baseDataBoundControl;
      }

    static public BaseDataList Bind
      (
      this BaseDataList baseDataList,
      object dataSource
      )
      {
      baseDataList.SetDataSource(dataSource).DataBind();
      return baseDataList;
      }

    static public ListControl Bind
      (
      this ListControl listControl,
      DataTable dataSource,
      string selected_id = k.EMPTY,
      bool be_available_option_all = false,
      string unselectedLiteral = "-- Select --",
      ListItemAttributeNameToDataTableColumnNameMap elaborationAttributesMap = null
      )
      {
      listControl.DataValueField = dataSource.Columns[0].ColumnName;
      listControl.DataTextField = dataSource.Columns[1].ColumnName;
      listControl.SetDataSource(dataSource).DataBind();
      var beManagedPlaceholderItem = ManagePlaceholderItem(listControl.Items,be_available_option_all,unselectedLiteral);
      if (selected_id.Length > 0)
        {
        listControl.SelectedValue = selected_id;
        }
      if (elaborationAttributesMap is not null)
        {
        AddElaborationAttributes(listControl.Items,dataSource,elaborationAttributesMap,beManagedPlaceholderItem);
        }
      return listControl;
      }

    static public ListItemCollection Bind
      (
      this ListItemCollection listItemCollection,
      DataTable dataSource,
      bool be_available_option_all = false,
      string unselectedLiteral = "-- Select --",
      ListItemAttributeNameToDataTableColumnNameMap elaborationAttributesMap = null
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
      var beManagedPlaceholderItem = ManagePlaceholderItem(listItemCollection,be_available_option_all,unselectedLiteral);
      if (elaborationAttributesMap is not null)
        {
        AddElaborationAttributes(listItemCollection,dataSource,elaborationAttributesMap,beManagedPlaceholderItem);
        }
      return listItemCollection;
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

    public class ListItemAttributeNameToDataTableColumnNameMap : Dictionary<string,string> { }

    static private void AddElaborationAttributes
      (
      ListItemCollection listItemCollection,
      DataTable dataSource,
      ListItemAttributeNameToDataTableColumnNameMap elaborationAttributesMap,
      bool beManagedPlaceholderItem
      )
      {
      var listItem = (ListItem)null;
      var numListItems = listItemCollection.Count;
      var licIndex = new k.subtype<int>(0,numListItems);
      if(beManagedPlaceholderItem)
        {
        listItem = listItemCollection[licIndex.val++];
        foreach(var entry in elaborationAttributesMap)
          {
          listItem.Attributes.Add(entry.Key,k.EMPTY);
          }
        }
      var dataSourceRows = dataSource.Rows;
      var numDataSourceRows = dataSourceRows.Count;
      var dataSourceRowsIndex = new k.subtype<int>(0,numDataSourceRows);
      while(licIndex.val < numListItems)
        {
        listItem = listItemCollection[licIndex.val];
        foreach(var entry in elaborationAttributesMap)
          {
          listItem.Attributes.Add(entry.Key,dataSourceRows[dataSourceRowsIndex.val][entry.Value].ToString());
          }
        licIndex.val++;
        dataSourceRowsIndex.val++;
        }
      }

    static private bool ManagePlaceholderItem
      (
      ListItemCollection listItemCollection,
      bool be_available_option_all,
      string unselectedLiteral
      )
      {
      var result = false;
      if (be_available_option_all && unselectedLiteral.Length > 0)
        {
        listItemCollection.Insert
          (
          index: 0,
          item: new ListItem()
            {
            Text = unselectedLiteral,
            Value = k.EMPTY
            }
          );
        result = true;
        }
      return result;
      }

    }
  }
