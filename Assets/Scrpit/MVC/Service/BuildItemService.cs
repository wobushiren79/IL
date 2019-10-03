using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BuildItemService 
{
    private readonly string mTableName;
    private readonly string mLeftDetailsTableName;
    private readonly string mLeftIntactTableName;

    public BuildItemService()
    {
        mTableName = "build_item";
        mLeftDetailsTableName = "build_item_details_" + GameCommonInfo.GameConfig.language;
    }

    /// <summary>
    /// 查询所有装修物品数据
    /// </summary>
    /// <returns></returns>
    public List<BuildItemBean> QueryAllData()
    {
        return SQliteHandle.LoadTableData<BuildItemBean>
            (ProjectConfigInfo.DATA_BASE_INFO_NAME, mTableName,
            new string[] { mLeftDetailsTableName },
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
        string[] leftTable = new string[] { mLeftDetailsTableName, mLeftIntactTableName };
        string[] mainKey = new string[] { "id", "intact_id" };
        string[] leftKey = new string[] { "equip_id", "intact_id" };
        string[] colName = new string[] { mTableName + ".build_type" };
        string[] operations = new string[] { "=" };
        string[] colValue = new string[] { ""+ type };
        return SQliteHandle.LoadTableData<BuildItemBean>(ProjectConfigInfo.DATA_BASE_INFO_NAME, mTableName, leftTable, mainKey, leftKey, colName, operations, colValue);
    }

}