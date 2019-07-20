using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public interface IAchievementInfoView 
{
    void GetAllAchievementInfoSuccess(List<AchievementInfoBean> listData);

    void GetAllAchievementInfoFail();
}