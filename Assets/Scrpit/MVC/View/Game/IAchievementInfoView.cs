using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public interface IAchievementInfoView 
{
    void GetAchievementInfoSuccess(List<AchievementInfoBean> listData);

    void GetAchievementInfoFail();
}