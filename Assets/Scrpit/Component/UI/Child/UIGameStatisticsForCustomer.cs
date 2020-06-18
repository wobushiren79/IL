using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
public class UIGameStatisticsForCustomer : BaseUIChildComponent<UIGameStatistics>
{
    public Text tvNormalCustomerNumber;
    public Text tvTeamCustomerNumber;
    public Text tvFriendCustomerNumber;

    public GameObject objTeamCustomerContainer;
    public GameObject objFriendCustomerContainer;
    public GameObject objItemCustomerModel;

    public override void Open()
    {
        base.Open();
        InitNormalCustomer();
        StartCoroutine(InitTeamCustomer());
        InitFriendCustomer();

        GameUtil.RefreshRectViewHight((RectTransform)objTeamCustomerContainer.transform, true);
        GameUtil.RefreshRectViewHight((RectTransform)objFriendCustomerContainer.transform, true);
    }

    /// <summary>
    /// 初始化普通客人
    /// </summary>
    public void InitNormalCustomer()
    {
        UserAchievementBean userAchievement = uiComponent.uiGameManager.gameDataManager.gameData.GetAchievementData();
        if (tvNormalCustomerNumber != null)
            tvNormalCustomerNumber.text = GameCommonInfo.GetUITextById(323) + " " + userAchievement.GetNumberForCustomerByType(CustomerTypeEnum.Normal) + GameCommonInfo.GetUITextById(82);
    }

    /// <summary>
    /// 初始化客人团队
    /// </summary>
    public IEnumerator InitTeamCustomer()
    {
        CptUtil.RemoveChildsByActive(objTeamCustomerContainer);
        NpcInfoManager npcInfoManager = uiComponent.uiGameManager.npcInfoManager;
        NpcTeamManager npcTeamManager = uiComponent.uiGameManager.npcTeamManager;

        UserAchievementBean userAchievement = uiComponent.uiGameManager.gameDataManager.gameData.GetAchievementData();
        if (tvTeamCustomerNumber != null)
            tvTeamCustomerNumber.text = GameCommonInfo.GetUITextById(323) + " " + userAchievement.GetNumberForCustomerByType(CustomerTypeEnum.Team) + GameCommonInfo.GetUITextById(82);
        //查询所有团队
        List<NpcTeamBean> listNpcTeamData = npcTeamManager.GetCustomerTeam();
        for (int i = 0; i < listNpcTeamData.Count; i++)
        {
            NpcTeamBean itemNpcTeamData = listNpcTeamData[i];
            GameObject objItem = Instantiate(objTeamCustomerContainer, objItemCustomerModel);
            ItemGameStatisticsForCustomerCpt itemCustomer = objItem.GetComponent<ItemGameStatisticsForCustomerCpt>();
            long[] teamLeaderIds = itemNpcTeamData.GetTeamLeaderId();
            CharacterBean teamLeaderData = npcInfoManager.GetCharacterDataById(teamLeaderIds[0]);
            UserCustomerBean userCustomerData = userAchievement.GetCustomerData(CustomerTypeEnum.Team, itemNpcTeamData.id + "");
            //检测是否解锁该顾客团队
            if (userAchievement.CheckHasTeamCustomer(itemNpcTeamData.id + ""))
            {
                long number = 0;
                if (userCustomerData != null)
                    number = userCustomerData.number;
                itemCustomer.SetData(teamLeaderData, true, itemNpcTeamData.name, number, itemNpcTeamData.id + "");
            }
            else
            {
                long number = 0;
                if (userCustomerData != null)
                    number = userCustomerData.number;
                itemCustomer.SetData(teamLeaderData, false, itemNpcTeamData.name, number, itemNpcTeamData.id + "");
            }
            if (i %  ProjectConfigInfo.ITEM_REFRESH_NUMBER == 0)
                yield return new WaitForEndOfFrame();
        }

    }

    /// <summary>
    /// 初始化朋友
    /// </summary>
    public void InitFriendCustomer()
    {
        CptUtil.RemoveChildsByActive(objFriendCustomerContainer);

        NpcInfoManager npcInfoManager = uiComponent.uiGameManager.npcInfoManager;
        GameDataManager gameDataManager = uiComponent.uiGameManager.gameDataManager;
        UserAchievementBean userAchievement = gameDataManager.gameData.GetAchievementData();

        //设置数量
        if (tvFriendCustomerNumber != null)
            tvFriendCustomerNumber.text = GameCommonInfo.GetUITextById(323) + " " + userAchievement.GetNumberForCustomerByType(CustomerTypeEnum.Friend) + GameCommonInfo.GetUITextById(82);
        List<CharacterFavorabilityBean> listData = gameDataManager.gameData.listCharacterFavorability;

        foreach (CharacterFavorabilityBean itemData in listData)
        {
            CharacterBean itemCharacterData = npcInfoManager.GetCharacterDataById(itemData.characterId);
            //如果是小镇居民
            if (itemCharacterData.baseInfo.characterType == (int)NpcTypeEnum.Town)
            {
                GameObject objItem = Instantiate(objFriendCustomerContainer, objItemCustomerModel);
                ItemGameStatisticsForCustomerCpt itemCustomer = objItem.GetComponent<ItemGameStatisticsForCustomerCpt>();
                UserCustomerBean userCustomerData = userAchievement.GetCustomerData(CustomerTypeEnum.Friend, itemCharacterData.baseInfo.characterId + "");
                long number = 0;
                if (userCustomerData != null)
                {
                    number = userCustomerData.number;
                }
                itemCustomer.SetData(itemCharacterData, true, null, number);
            }
        }
    }
}