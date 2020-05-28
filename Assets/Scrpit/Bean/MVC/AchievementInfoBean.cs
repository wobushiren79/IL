using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
public class AchievementInfoBean : BaseBean
{
    public long ach_id;
    public long pre_ach_id;//前置成就
    public int type;
    public string icon_key;
    public string icon_key_remark;
    public long remark_id;

    public string pre_data;//前置条件
    public string reward_data;//奖励数据

    public string name;
    public string content;

    public AchievementTypeEnum GetAchievementType()
    {
        return (AchievementTypeEnum)type;
    }
}