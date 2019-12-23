using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MenuInfoService 
{
    private readonly string tableNameForMain;
    private readonly string tableNameForLeft;

    public MenuInfoService()
    {
        tableNameForMain = "menu_info";
        tableNameForLeft = "menu_info_details_" + GameCommonInfo.GameConfig.language;
    }

    /// <summary>
    /// 查询所有菜单数据
    /// </summary>
    /// <returns></returns>
    public List<MenuInfoBean> QueryAllData()
    {
        return SQliteHandle.LoadTableData<MenuInfoBean>
            (ProjectConfigInfo.DATA_BASE_INFO_NAME, tableNameForMain,
            new string[] { tableNameForLeft },
            new string[] { "id" },
            new string[] { "menu_id" });
    }

    /// <summary>
    /// 根据ID查询数据
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public List<MenuInfoBean> QueryDataByIds(long[] ids)
    {
        string[] leftTable = new string[] { tableNameForLeft };
        string[] mainKey = new string[] { "id" };
        string[] leftKey = new string[] { "menu_id" };
        string[] colName = new string[] { tableNameForMain + ".id" };
        string[] operations = new string[] { "IN" };
        string values = TypeConversionUtil.ArrayToStringBySplit(ids, ",");
        string[] colValue = new string[] { "(" + values + ")" };
        return SQliteHandle.LoadTableData<MenuInfoBean>(ProjectConfigInfo.DATA_BASE_INFO_NAME, tableNameForMain, leftTable, mainKey, leftKey, colName, operations, colValue);
    }
}