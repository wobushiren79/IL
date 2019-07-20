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
    /// 查询所有商店信息
    /// </summary>
    public void GetAllAchievementInfo()
    {
        List<AchievementInfoBean> listData = GetModel().GetAllAchievementInfo();
        if (!CheckUtil.ListIsNull(listData))
        {
            GetView().GetAllAchievementInfoSuccess(listData);
        }
        else
        {
            GetView().GetAllAchievementInfoFail();
        }
    }
}