using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class ItemTownArenaCpt : ItemGameBaseCpt
{
    public Text tvTitle;
    public GameObject objPriceL;
    public Text tvPriceL;
    public GameObject objPriceM;
    public Text tvPriceM;
    public GameObject objPriceS;
    public Text tvPriceS;

    public Text tvRule;

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
        SetPrice(miniGameData.preMoneyL, miniGameData.preMoneyM,miniGameData.preMoneyS);
        SetRuleContent(miniGameData.GetListWinConditions());
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
        UIGameManager uiGameManager = GetUIManager<UIGameManager>();
        GameItemsManager gameItemsManager = uiGameManager.gameItemsManager;
        IconDataManager iconDataManager = uiGameManager.iconDataManager;
        InnBuildManager innBuildManager = uiGameManager.innBuildManager;
        InfoItemsPopupShow infoItemsPopupShow = uiGameManager.infoItemsPopup;
        foreach (RewardTypeBean itemReward in listReward)
        {
            GameObject objReward = Instantiate(objRewardContainer, objRewardModel);
            Image ivIcon = CptUtil.GetCptInChildrenByName<Image>(objReward, "Icon");
            Text tvNumber = CptUtil.GetCptInChildrenByName<Text>(objReward, "Text");
            RewardTypeEnumTools.GetRewardDetails(itemReward, iconDataManager, gameItemsManager, innBuildManager);
            ivIcon.sprite = itemReward.spRewardIcon;
            tvNumber.text = "x" + itemReward.rewardNumber;
            if (itemReward.rewardType== RewardTypeEnum.AddItems)
            {
                InfoItemsPopupButton infoItemsPopup = objReward.GetComponent<InfoItemsPopupButton>();
                infoItemsPopup.SetPopupShowView(infoItemsPopupShow);
                ItemsInfoBean itemsInfo = gameItemsManager.GetItemsById(itemReward.rewardId);
                infoItemsPopup.SetData(itemsInfo , itemReward.spRewardIcon);
            }
        }
    }

    /// <summary>
    /// 设置规则说明
    /// </summary>
    /// <param name="content"></param>
    public void SetRuleContent(string content)
    {
        if (tvRule != null)
            tvRule.text = content;
    }
    public void SetRuleContent(List<string> listRule)
    {
        string ruleStr = "";
        for(int i=0;i< listRule.Count; i++)
        {
            string itemRule = listRule[i];
            ruleStr += ((i+1)+"."+ itemRule+"\n");
        }
        SetRuleContent(ruleStr);
    }
    /// <summary>
    /// 设置价格
    /// </summary>
    /// <param name="priceL"></param>
    /// <param name="pirceM"></param>
    /// <param name="priceS"></param>
    public void SetPrice(long priceL,long pirceM, long priceS)
    {
        if (priceL == 0)
            objPriceL.SetActive(false);
        if (pirceM == 0)
            objPriceM.SetActive(false);
        if (priceS == 0)
            objPriceS.SetActive(false);
        tvPriceL.text = "" + priceL;
        tvPriceM.text = "" + pirceM;
        tvPriceS.text = "" + priceS;
    }
}
