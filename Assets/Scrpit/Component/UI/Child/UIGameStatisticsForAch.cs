using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIGameStatisticsForAch : BaseUIChildComponent<UIGameStatistics>, AchievementInfoManager.ICallBack
{
    public Text tvNull;
    public GameObject objAchContainer;
    public GameObject objAchItem;

    protected GameDataManager gameDataManager;
    protected AchievementInfoManager achievementInfoManager;

    private void Awake()
    {
        gameDataManager = Find<GameDataManager>(ImportantTypeEnum.GameDataManager);
        achievementInfoManager = Find<AchievementInfoManager>(ImportantTypeEnum.GameDataManager);
    }

    public override void Open()
    {
        base.Open();
        achievementInfoManager.SetCallBack(this);
        achievementInfoManager.GetAllAchievement();
    }

    public override void Close()
    {
        base.Close();
        CptUtil.RemoveChildsByActive(objAchContainer);
    }

    /// <summary>
    /// 创建成就列表
    /// </summary>
    /// <param name="listData"></param>
    public void CreateAchList(List<AchievementInfoBean> listData)
    {
        UserAchievementBean userAchievement = gameDataManager.gameData.GetAchievementData();
        List<long> achievementList = userAchievement.listAchievement;
        //if (achievementList == null || achievementList.Count == 0)
        //{
        //    tvNull.gameObject.SetActive(true);
        //    return;
        //}
        tvNull.gameObject.SetActive(false);
        for (int i = 0; i < listData.Count; i++)
        {
            AchievementInfoBean achievementInfo = listData[i];
            GameObject objItem = Instantiate(objAchContainer, objAchItem);
            ItemGameStatisticsForAchCpt achCpt = objItem.GetComponent<ItemGameStatisticsForAchCpt>();
            if (achievementList.Contains(achievementInfo.id))
            {
                achCpt.SetData(achievementInfo,true);
            }
            else
            {
                achCpt.SetData(achievementInfo,false);
            }
        }
    }

    #region 成就数据回调
    public void GetAchievementInfoSuccess(List<AchievementInfoBean> listData)
    {
        CreateAchList(listData);
    }
    #endregion
}