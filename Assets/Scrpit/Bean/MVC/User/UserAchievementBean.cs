using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
public class UserAchievementBean 
{
    public List<long> listAchievement = new List<long>();//解锁成就列表

    public long ownIngOilsalt;//油盐
    public long ownIngMeat;//肉类
    public long ownIngRiverfresh;//河鲜
    public long ownIngSeafood;//海鲜
    public long ownIngVegetables;//蔬菜
    public long ownIngMelonfruit;//瓜果
    public long ownIngWaterwine;//酒水
    public long ownIngFlour;//面粉

    /// <summary>
    /// 是否包含该成就
    /// </summary>
    /// <param name="achId"></param>
    /// <returns></returns>
    public bool CheckHasAchievement(long achId)
    {
       return listAchievement.Contains(achId);
    }

    /// <summary>
    /// 添加成就
    /// </summary>
    /// <param name="achId"></param>
    public void AddAchievement(long achId)
    {
        listAchievement.Add(achId);
    }


}