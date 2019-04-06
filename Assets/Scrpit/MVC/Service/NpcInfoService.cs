using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class NpcInfoService 
{
    private readonly string mTableName;
    private readonly string mLeftDetailsTableName;

    public NpcInfoService()
    {
        mTableName = "npc_info";
        mLeftDetailsTableName = "npc_info_details_" + GameCommonInfo.gameConfig.language;
    }

    /// <summary>
    /// 查询所有装备数据
    /// </summary>
    /// <returns></returns>
    public List<NpcInfoBean> QueryAllData()
    {
        return SQliteHandle.LoadTableData<NpcInfoBean>
            (ProjectConfigInfo.DATA_BASE_INFO_NAME, mTableName,
            new string[] { mLeftDetailsTableName },
            new string[] { "id" },
            new string[] { "npc_id" });
    }

    /// <summary>
    /// 根据ID查询数据
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public List<NpcInfoBean> QueryDataByIds(long[] ids)
    {
        string[] leftTable = new string[] { mLeftDetailsTableName};
        string[] mainKey = new string[] { "id" };
        string[] leftKey = new string[] { "npc_id" };
        string[] colName = new string[] { mTableName + ".id" };
        string[] operations = new string[] { "IN" };
        string values = TypeConversionUtil.ArrayToStringBySplit(ids, ",");
        string[] colValue = new string[] { "(" + values + ")" };
        return SQliteHandle.LoadTableData<NpcInfoBean>(ProjectConfigInfo.DATA_BASE_INFO_NAME, mTableName, leftTable, mainKey, leftKey, colName, operations, colValue);
    }

    /// <summary>
    /// 根据Type查询数据
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public List<NpcInfoBean> QueryDataByType(int[] type)
    {
        string[] leftTable = new string[] { mLeftDetailsTableName };
        string[] mainKey = new string[] { "id" };
        string[] leftKey = new string[] { "npc_id" };
        string[] colName = new string[] { mTableName + ".npc_type" };
        string[] operations = new string[] { "IN" };
        string values = TypeConversionUtil.ArrayToStringBySplit(type, ",");
        string[] colValue = new string[] { "(" + values + ")" };
        return SQliteHandle.LoadTableData<NpcInfoBean>(ProjectConfigInfo.DATA_BASE_INFO_NAME, mTableName, leftTable, mainKey, leftKey, colName, operations, colValue);
    }
}