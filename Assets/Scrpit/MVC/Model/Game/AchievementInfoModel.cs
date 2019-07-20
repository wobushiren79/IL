using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class AchievementInfoModel : BaseMVCModel
{
    private AchievementInfoService mAchievementInfoService;

    public override void InitData()
    {
        mAchievementInfoService = new AchievementInfoService();
    }

    /// <summary>
    /// 获取所有成就信息
    /// </summary>
    /// <returns></returns>
    public List<AchievementInfoBean> GetAllAchievementInfo()
    {
        List<AchievementInfoBean> listData = mAchievementInfoService.QueryAllData();
        return listData;
    }

    /// <summary>
    /// 根据类型获取成就信息
    /// </summary>
    /// <returns></returns>
    public List<AchievementInfoBean> GeAchievementInfoByType(int type)
    {
        List<AchievementInfoBean> listData = mAchievementInfoService.QueryDataByType(type);
        return listData;
    }
}