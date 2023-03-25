using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class AchievementInfoManager : BaseManager, IAchievementInfoView
{
    protected AchievementInfoController achievementInfoController;

    private void Awake()
    {
        achievementInfoController = new AchievementInfoController(this,this);
    }

    /// <summary>
    /// 获取所有成就
    /// </summary>
    public void GetAllAchievement(Action<List<AchievementInfoBean>> action)
    {
        achievementInfoController.GetAllAchievementInfo(action);
    }

    /// <summary>
    /// 通过ID获取所有成就
    /// </summary>
    /// <param name="ids"></param>
    public void GetAchievementByIds(List<long> ids, Action<List<AchievementInfoBean>> action)
    {
        achievementInfoController.GetAchievementInfoByIds(ids, action);
    }

    public void GetAchievementInfoFail()
    {

    }

    public void GetAchievementInfoSuccess(List<AchievementInfoBean> listData,Action<List<AchievementInfoBean>> action)
    {
        action?.Invoke(listData);
    }

}