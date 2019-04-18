using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MenuInfoService 
{
    private readonly string mTableName;
    private readonly string mLeftDetailsTableName;

    public MenuInfoService()
    {
        mTableName = "menu_info";
        mLeftDetailsTableName = "menu_info_details_" + GameCommonInfo.gameConfig.language;
    }

    /// <summary>
    /// 查询所有菜单数据
    /// </summary>
    /// <returns></returns>
    public List<MenuInfoBean> QueryAllData()
    {
        return SQliteHandle.LoadTableData<MenuInfoBean>
            (ProjectConfigInfo.DATA_BASE_INFO_NAME, mTableName,
            new string[] { mLeftDetailsTableName },
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
        string[] leftTable = new string[] { mLeftDetailsTableName };
        string[] mainKey = new string[] { "id" };
        string[] leftKey = new string[] { "menu_id" };
        string[] colName = new string[] { mTableName + ".id" };
        string[] operations = new string[] { "IN" };
        string values = TypeConversionUtil.ArrayToStringBySplit(ids, ",");
        string[] colValue = new string[] { "(" + values + ")" };
        return SQliteHandle.LoadTableData<MenuInfoBean>(ProjectConfigInfo.DATA_BASE_INFO_NAME, mTableName, leftTable, mainKey, leftKey, colName, operations, colValue);
    }
}