using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class StoreInfoService 
{
    private readonly string mTableName;
    private readonly string mLeftDetailsTableName;

    public StoreInfoService()
    {
        mTableName = "store_info";
        mLeftDetailsTableName = "store_info_details_" + GameCommonInfo.gameConfig.language;
    }

    /// <summary>
    /// 查询所有装备数据
    /// </summary>
    /// <returns></returns>
    public List<StoreInfoBean> QueryAllData()
    {
        return SQliteHandle.LoadTableData<StoreInfoBean>
            (ProjectConfigInfo.DATA_BASE_INFO_NAME, mTableName,
            new string[] { mLeftDetailsTableName },
            new string[] { "id"},
            new string[] { "goods_id" });
    }

    /// <summary>
    /// 根据ID查询数据
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public List<StoreInfoBean> QueryDataByIds(long[] ids)
    {
        string[] leftTable = new string[] { mLeftDetailsTableName };
        string[] mainKey = new string[] { "id" };
        string[] leftKey = new string[] { "goods_id" };
        string[] colName = new string[] { mTableName + ".id" };
        string[] operations = new string[] { "IN" };
        string values = TypeConversionUtil.ArrayToStringBySplit(ids, ",");
        string[] colValue = new string[] { "(" + values + ")" };
        return SQliteHandle.LoadTableData<StoreInfoBean>(ProjectConfigInfo.DATA_BASE_INFO_NAME, mTableName, leftTable, mainKey, leftKey, colName, operations, colValue);
    }

    /// <summary>
    /// 根据类型查询数据
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public List<StoreInfoBean> QueryDataByType(int type)
    {
        string[] leftTable = new string[] { mLeftDetailsTableName };
        string[] mainKey = new string[] { "id" };
        string[] leftKey = new string[] { "goods_id" };
        string[] colName = new string[] { mTableName + ".type" };
        string[] operations = new string[] { "=" };
        string[] colValue = new string[] {  type +"" };
        return SQliteHandle.LoadTableData<StoreInfoBean>(ProjectConfigInfo.DATA_BASE_INFO_NAME, mTableName, leftTable, mainKey, leftKey, colName, operations, colValue);

    }
}