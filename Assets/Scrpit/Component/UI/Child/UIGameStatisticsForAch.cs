using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;

public class UIGameStatisticsForAch : BaseUIChildComponent<UIGameStatistics>
{
    public Text tvNull;
    public GameObject objAchContainer;
    public GameObject objAchItem;

    protected GameDataManager gameDataManager;

    public override void Awake()
    {
        base.Awake();
        gameDataManager = Find<GameDataManager>(ImportantTypeEnum.GameDataManager);
    }

    public override void Open()
    {
        base.Open();
        Action<List<AchievementInfoBean>> callBack = SetAchievementInfoData;
        AchievementInfoHandler.Instance.manager.GetAllAchievement(callBack);
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
    public IEnumerator CreateAchList(List<AchievementInfoBean> listData)
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
            if (i % ProjectConfigInfo.ITEM_REFRESH_NUMBER == 0)
                yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    /// 设置成就数据 
    /// </summary>
    /// <param name="listData"></param>
    public void SetAchievementInfoData(List<AchievementInfoBean> listData)
    {
        StartCoroutine(CreateAchList(listData));
    }
}