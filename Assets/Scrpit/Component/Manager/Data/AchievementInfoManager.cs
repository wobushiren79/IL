using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class AchievementInfoManager : BaseManager, IAchievementInfoView
{
    protected AchievementInfoController achievementInfoController;
    protected ICallBack callBack;

    protected List<AchievementInfoBean> listAchData = new List<AchievementInfoBean>();

    private void Awake()
    {
        achievementInfoController = new AchievementInfoController(this,this);
    }

    public void SetCallBack(ICallBack callBack)
    {
        this.callBack = callBack;
    }

    /// <summary>
    /// 获取所有成就
    /// </summary>
    public void GetAllAchievement()
    {
        achievementInfoController.GetAllAchievementInfo();
    }

    /// <summary>
    /// 通过ID获取所有成就
    /// </summary>
    /// <param name="ids"></param>
    public void GetAchievementByIds(List<long> ids)
    {
        achievementInfoController.GetAchievementInfoByIds(ids);
    }

    public void GetAchievementInfoFail()
    {

    }

    public void GetAchievementInfoSuccess(List<AchievementInfoBean> listData)
    {
        this.listAchData = listData;
        if (callBack != null)
            callBack.GetAchievementInfoSuccess(listData);
    }

    public interface ICallBack
    {
        void GetAchievementInfoSuccess(List<AchievementInfoBean> listData);
    }
}