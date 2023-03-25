using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public interface IAchievementInfoView 
{
    void GetAchievementInfoSuccess(List<AchievementInfoBean> listData,Action<List<AchievementInfoBean>> action);

    void GetAchievementInfoFail();
}