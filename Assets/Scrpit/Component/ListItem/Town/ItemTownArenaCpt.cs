using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class ItemTownArenaCpt : ItemGameBaseCpt
{
    public Text tvTitle;

    public GameObject objRewardContainer;
    public GameObject objRewardModel;

    public void SetData(MiniGameBaseBean miniGameData)
    {
        InitDataForType(miniGameData.gameType, miniGameData);
    }

    public void InitDataForType(MiniGameEnum gameType, MiniGameBaseBean miniGameData)
    {
        SetTitle(miniGameData.GetGameName());
        SetReward(miniGameData.listReward);
        switch (gameType)
        {
            case MiniGameEnum.Cooking:

                break;
            case MiniGameEnum.Barrage:
                break;
            case MiniGameEnum.Account:
                break;
            case MiniGameEnum.Debate:
                break;
            case MiniGameEnum.Combat:
                break;
        }
    }

    public void InitDataForCooking(MiniGameBaseBean miniGameData)
    {
        MiniGameCookingBean miniGameCookingData = (MiniGameCookingBean)miniGameData;
    }

    /// <summary>
    /// 设置标题
    /// </summary>
    /// <param name="title"></param>
    public void SetTitle(string title)
    {
        if (tvTitle != null)
        {
            tvTitle.text = title;
        }
    }

    /// <summary>
    /// 设置奖励
    /// </summary>
    /// <param name="listReward"></param>
    public void SetReward(List<RewardTypeBean> listReward)
    {
        UIGameManager uiGameManager= GetUIManager<UIGameManager>();
        GameItemsManager gameItemsManager = uiGameManager.gameItemsManager;
        IconDataManager iconDataManager = uiGameManager.iconDataManager;
        InnBuildManager innBuildManager = uiGameManager.innBuildManager;
        foreach (RewardTypeBean itemReward in listReward)
        {
            GameObject objReward = Instantiate(objRewardContainer, objRewardModel);
            Image ivIcon = CptUtil.GetCptInChildrenByName<Image>(objReward, "Icon");
            Image ivNumber = CptUtil.GetCptInChildrenByName<Image>(objReward, "Text");
            RewardTypeEnumTools.GetRewardDetails(itemReward, iconDataManager, gameItemsManager, innBuildManager);
            ivIcon.sprite = itemReward.spRewardIcon;
        }
    }

}
