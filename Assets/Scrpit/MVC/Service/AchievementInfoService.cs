using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class AchievementInfoService 
{
    private readonly string mTableName;
    private readonly string mLeftDetailsTableName;

    public AchievementInfoService()
    {
        mTableName = "achievement_info";
        mLeftDetailsTableName = "achievement_info_details_" + GameCommonInfo.gameConfig.language;
    }

    /// <summary>
    /// 查询所有成就数据
    /// </summary>
    /// <returns></returns>
    public List<AchievementInfoBean> QueryAllData()
    {
        return SQliteHandle.LoadTableData<AchievementInfoBean>
            (ProjectConfigInfo.DATA_BASE_INFO_NAME, mTableName,
            new string[] { mLeftDetailsTableName },
            new string[] { "id" },
            new string[] { "ach_id" });
    }

    /// <summary>
    /// 根据ID查询数据
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public List<AchievementInfoBean> QueryDataByIds(long[] ids)
    {
        string[] leftTable = new string[] { mLeftDetailsTableName };
        string[] mainKey = new string[] { "id" };
        string[] leftKey = new string[] { "ach_id" };
        string[] colName = new string[] { mTableName + ".id" };
        string[] operations = new string[] { "IN" };
        string values = TypeConversionUtil.ArrayToStringBySplit(ids, ",");
        string[] colValue = new string[] { "(" + values + ")" };
        return SQliteHandle.LoadTableData<AchievementInfoBean>(ProjectConfigInfo.DATA_BASE_INFO_NAME, mTableName, leftTable, mainKey, leftKey, colName, operations, colValue);
    }

    /// <summary>
    /// 根据类型查询数据
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public List<AchievementInfoBean> QueryDataByType(int type)
    {
        string[] leftTable = new string[] { mLeftDetailsTableName };
        string[] mainKey = new string[] { "id" };
        string[] leftKey = new string[] { "ach_id" };
        string[] colName = new string[] { mTableName + ".type" };
        string[] operations = new string[] { "=" };
        string[] colValue = new string[] { type + "" };
        return SQliteHandle.LoadTableData<AchievementInfoBean>(ProjectConfigInfo.DATA_BASE_INFO_NAME, mTableName, leftTable, mainKey, leftKey, colName, operations, colValue);
    }
}