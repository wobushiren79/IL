using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class AchievementInfoService 
{
    private readonly string tableNameForMain;
    private readonly string tableNameForLeft;

    public AchievementInfoService()
    {
        tableNameForMain = "achievement_info";
        tableNameForLeft = "achievement_info_details_" + GameCommonInfo.GameConfig.language;
    }

    /// <summary>
    /// 查询所有成就数据
    /// </summary>
    /// <returns></returns>
    public List<AchievementInfoBean> QueryAllData()
    {
        return SQliteHandle.LoadTableData<AchievementInfoBean>
            (ProjectConfigInfo.DATA_BASE_INFO_NAME, tableNameForMain,
            new string[] { tableNameForLeft },
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
        string[] leftTable = new string[] { tableNameForLeft };
        string[] mainKey = new string[] { "id" };
        string[] leftKey = new string[] { "ach_id" };
        string[] colName = new string[] { tableNameForMain + ".id" };
        string[] operations = new string[] { "IN" };
        string values = TypeConversionUtil.ArrayToStringBySplit(ids, ",");
        string[] colValue = new string[] { "(" + values + ")" };
        return SQliteHandle.LoadTableData<AchievementInfoBean>(ProjectConfigInfo.DATA_BASE_INFO_NAME, tableNameForMain, leftTable, mainKey, leftKey, colName, operations, colValue);
    }

    /// <summary>
    /// 根据类型查询数据
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public List<AchievementInfoBean> QueryDataByType(int type)
    {
        string[] leftTable = new string[] { tableNameForLeft };
        string[] mainKey = new string[] { "id" };
        string[] leftKey = new string[] { "ach_id" };
        string[] colName = new string[] { tableNameForMain + ".type" };
        string[] operations = new string[] { "=" };
        string[] colValue = new string[] { type + "" };
        return SQliteHandle.LoadTableData<AchievementInfoBean>(ProjectConfigInfo.DATA_BASE_INFO_NAME, tableNameForMain, leftTable, mainKey, leftKey, colName, operations, colValue);
    }
}