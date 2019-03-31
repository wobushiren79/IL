using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class EquipInfoService
{
    private readonly string mTableName;
    private readonly string mLeftDetailsTableName;
    private readonly string mLeftIntactTableName;

    public EquipInfoService()
    {
        mTableName = "equip_info";
        mLeftDetailsTableName = "equip_info_details_" + GameCommonInfo.gameConfig.language;
        mLeftIntactTableName = "equip_intact_info";
    }

    /// <summary>
    /// 查询所有装备数据
    /// </summary>
    /// <returns></returns>
    public List<EquipInfoBean> QueryAllData()
    {
        return SQliteHandle.LoadTableData<EquipInfoBean>
            (ProjectConfigInfo.DATA_BASE_INFO_NAME, mTableName,
            new string[] { mLeftDetailsTableName,mLeftIntactTableName },
            new string[] { "id" ,"intact_id"},
            new string[] { "equip_id" ,"intact_id"});
    }

    /// <summary>
    /// 根据ID查询数据
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public List<EquipInfoBean> QueryDataByIds(long[] ids)
    {
        string[] leftTable = new string[] { mLeftDetailsTableName, mLeftIntactTableName };
        string[] mainKey = new string[] { "id", "intact_id" };
        string[] leftKey = new string[] { "equip_id", "intact_id" };
        string[] colName = new string[] { mTableName+".id" };
        string[] operations = new string[] { "IN" };
        string values = TypeConversionUtil.ArrayToStringBySplit(ids, ",");
        string[] colValue = new string[] { "(" + values + ")" };
        return SQliteHandle.LoadTableData<EquipInfoBean>(ProjectConfigInfo.DATA_BASE_INFO_NAME, mTableName, leftTable, mainKey, leftKey, colName, operations, colValue);
    }
}