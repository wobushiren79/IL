using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
public class UserAchievementBean 
{
    public List<long> achievementList = new List<long>();//解锁成就列表

    /// <summary>
    /// 是否包含该成就
    /// </summary>
    /// <param name="achId"></param>
    /// <returns></returns>
    public bool CheckHasAchievement(long achId)
    {
       return achievementList.Contains(achId);
    }

    /// <summary>
    /// 添加成就
    /// </summary>
    /// <param name="achId"></param>
    public void AddAchievement(long achId)
    {
        achievementList.Add(achId);
    }


}