using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BuildItemService 
{
    private readonly string tableNameForMain;
    private readonly string tableNameForLeft;

    public BuildItemService()
    {
        tableNameForMain = "build_item";
        tableNameForLeft = "build_item_details_" + GameCommonInfo.GameConfig.language;
    }

    /// <summary>
    /// 查询所有装修物品数据
    /// </summary>
    /// <returns></returns>
    public List<BuildItemBean> QueryAllData()
    {
        return SQliteHandle.LoadTableData<BuildItemBean>
            (ProjectConfigInfo.DATA_BASE_INFO_NAME, tableNameForMain,
            new string[] { tableNameForLeft },
            new string[] { "id" },
            new string[] { "build_id" });
    }

    /// <summary>
    /// 根据类型查询数据
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public List<BuildItemBean> QueryDataByType(int type)
    {
        string[] leftTable = new string[] { tableNameForLeft };
        string[] mainKey = new string[] { "id" };
        string[] leftKey = new string[] { "build_id" };
        string[] colName = new string[] { tableNameForMain + ".build_type" };
        string[] operations = new string[] { "=" };
        string[] colValue = new string[] { ""+ type };
        return SQliteHandle.LoadTableData<BuildItemBean>(ProjectConfigInfo.DATA_BASE_INFO_NAME, tableNameForMain, leftTable, mainKey, leftKey, colName, operations, colValue);
    }

}