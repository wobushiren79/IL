using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class AchievementInfoService : BaseMVCService
{
    public AchievementInfoService() : base("achievement_info", "achievement_info_details_" + GameDataHandler.Instance.manager.GetGameConfig().language)
    {

    }

    /// <summary>
    /// 查询所有成就数据
    /// </summary>
    /// <returns></returns>
    public List<AchievementInfoBean> QueryAllData()
    {
        return BaseQueryAllData<AchievementInfoBean>("ach_id");
    }

    /// <summary>
    /// 根据ID查询数据
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public List<AchievementInfoBean> QueryDataByIds(long[] ids)
    {
        string values = TypeConversionUtil.ArrayToStringBySplit(ids, ",");
        return BaseQueryData<AchievementInfoBean>("ach_id", tableNameForMain + ".id", "IN", "(" + values + ")");
    }

    /// <summary>
    /// 根据类型查询数据
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public List<AchievementInfoBean> QueryDataByType(int type)
    {
        return BaseQueryData<AchievementInfoBean>("ach_id", tableNameForMain + ".type", type + "");
    }

    /// <summary>
    /// 通过ID删除数据
    /// </summary>
    /// <param name="id"></param>
    public void DeleteDataById(long id)
    {
        bool isDelete = BaseDeleteData(tableNameForMain, "id", id + "");
        if (isDelete)
            BaseDeleteData(tableNameForLeft, "ach_id", id + "");
    }

    /// <summary>
    /// 插入数据
    /// </summary>
    /// <param name="achievementInfo"></param>
    public void InsertData(AchievementInfoBean achievementInfo)
    {
        List<string> listLeftName = new List<string>() { "ach_id","name","content"};
        BaseInsertDataWithLeft(achievementInfo, listLeftName);
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="achievementInfo"></param>
    public void Update(AchievementInfoBean achievementInfo)
    {
        DeleteDataById(achievementInfo.id);
        InsertData(achievementInfo);
    }
}