using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ItemsInfoService
{
    private readonly string mTableName;
    private readonly string mLeftDetailsTableName;
    private readonly string mLeftIntactTableName;

    public ItemsInfoService()
    {
        mTableName = "items_info";
        mLeftDetailsTableName = "items_info_details_" + GameCommonInfo.GameConfig.language;
        mLeftIntactTableName = "items_intact_info";
    }

    /// <summary>
    /// 查询所有装备数据
    /// </summary>
    /// <returns></returns>
    public List<ItemsInfoBean> QueryAllData()
    {
        return SQliteHandle.LoadTableData<ItemsInfoBean>
            (ProjectConfigInfo.DATA_BASE_INFO_NAME, mTableName,
            new string[] { mLeftDetailsTableName,mLeftIntactTableName },
            new string[] { "id" ,"intact_id"},
            new string[] { "items_id" ,"intact_id"});
    }

    /// <summary>
    /// 根据ID查询数据
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public List<ItemsInfoBean> QueryDataByIds(long[] ids)
    {
        string[] leftTable = new string[] { mLeftDetailsTableName, mLeftIntactTableName };
        string[] mainKey = new string[] { "id", "intact_id" };
        string[] leftKey = new string[] { "items_id", "intact_id" };
        string[] colName = new string[] { mTableName+".id" };
        string[] operations = new string[] { "IN" };
        string values = TypeConversionUtil.ArrayToStringBySplit(ids, ",");
        string[] colValue = new string[] { "(" + values + ")" };
        return SQliteHandle.LoadTableData<ItemsInfoBean>(ProjectConfigInfo.DATA_BASE_INFO_NAME, mTableName, leftTable, mainKey, leftKey, colName, operations, colValue);
    }
}