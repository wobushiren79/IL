using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class AchievementInfoController : BaseMVCController<AchievementInfoModel, IAchievementInfoView>
{
    public AchievementInfoController(BaseMonoBehaviour content, IAchievementInfoView view) : base(content, view)
    {
    }

    public override void InitData()
    {
    }
    /// <summary>
    /// 查询所有成就信息
    /// </summary>
    public void GetAllAchievementInfo(Action<List<AchievementInfoBean>> action)
    {
        List<AchievementInfoBean> listData = GetModel().GetAllAchievementInfo();
        if (!listData.IsNull())
        {
            GetView().GetAchievementInfoSuccess(listData, action);
        }
        else
        {
            GetView().GetAchievementInfoFail();
        }
    }

    /// <summary>
    /// 通过ID查询成就
    /// </summary>
    /// <param name="ids"></param>
    public void GetAchievementInfoByIds(List<long> ids,Action<List<AchievementInfoBean>> action)
    {
        List<AchievementInfoBean> listData = GetModel().GeAchievementInfoByIds(ids);
        if (!listData.IsNull())
        {
            GetView().GetAchievementInfoSuccess(listData, action);
        }
        else
        {
            GetView().GetAchievementInfoFail();
        }
    }
}