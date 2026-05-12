using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class AchievementInfoManager : BaseManager
{
    private void Awake()
    {
    }

    /// <summary>
    /// 获取所有成就
    /// </summary>
    public void GetAllAchievement(Action<List<AchievementInfoBean>> action)
    {
        var dicData = AchievementInfoCfg.GetAllData();
        List<AchievementInfoBean> listData = new List<AchievementInfoBean>();
        if (dicData != null)
        {
            foreach (var item in dicData)
                listData.Add(item.Value);
        }
        action?.Invoke(listData);
    }

    /// <summary>
    /// 通过ID获取所有成就
    /// </summary>
    public void GetAchievementByIds(List<long> ids, Action<List<AchievementInfoBean>> action)
    {
        List<AchievementInfoBean> listData = new List<AchievementInfoBean>();
        if (ids == null)
        {
            action?.Invoke(listData);
            return;
        }
        foreach (long id in ids)
        {
            AchievementInfoBean data = AchievementInfoCfg.GetItemData(id);
            if (data != null)
                listData.Add(data);
        }
        action?.Invoke(listData);
    }
}
