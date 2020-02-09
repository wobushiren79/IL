using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

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
    public void GetAllAchievementInfo()
    {
        List<AchievementInfoBean> listData = GetModel().GetAllAchievementInfo();
        if (!CheckUtil.ListIsNull(listData))
        {
            GetView().GetAchievementInfoSuccess(listData);
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
    public void GetAchievementInfoByIds(List<long> ids)
    {
        List<AchievementInfoBean> listData = GetModel().GeAchievementInfoByIds(ids);
        if (!CheckUtil.ListIsNull(listData))
        {
            GetView().GetAchievementInfoSuccess(listData);
        }
        else
        {
            GetView().GetAchievementInfoFail();
        }
    }
}