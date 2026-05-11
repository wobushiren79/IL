using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class AchievementInfoManager : BaseManager
{
    protected AchievementInfoService achievementInfoService;

    private void Awake()
    {
        achievementInfoService = new AchievementInfoService();
    }

    /// <summary>
    /// 获取所有成就
    /// </summary>
    public void GetAllAchievement(Action<List<AchievementInfoBean>> action)
    {
        List<AchievementInfoBean> listData = achievementInfoService.QueryAllData();
        action?.Invoke(listData);
    }

    /// <summary>
    /// 通过ID获取所有成就
    /// </summary>
    /// <param name="ids"></param>
    public void GetAchievementByIds(List<long> ids, Action<List<AchievementInfoBean>> action)
    {
        List<AchievementInfoBean> listData = achievementInfoService.QueryDataByIds(TypeConversionUtil.ListToArray(ids));
        action?.Invoke(listData);
    }
}