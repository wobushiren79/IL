using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
public class UIGameStatisticsForCustomer : BaseUIView
{
    public Text tvNormalCustomerNumber;
    public Text tvTeamCustomerNumber;
    public Text tvFriendCustomerNumber;

    public GameObject objTeamCustomerContainer;
    public GameObject objFriendCustomerContainer;
    public GameObject objItemCustomerModel;

    public override void OpenUI()
    {
        base.OpenUI();
        InitNormalCustomer();
        StartCoroutine(InitTeamCustomer());
        InitFriendCustomer();

        GameUtil.RefreshRectTransform((RectTransform)objTeamCustomerContainer.transform);
        GameUtil.RefreshRectTransform((RectTransform)objFriendCustomerContainer.transform);
    }

    /// <summary>
    /// 初始化普通客人
    /// </summary>
    public void InitNormalCustomer()
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        UserAchievementBean userAchievement = gameData.GetAchievementData();
        if (tvNormalCustomerNumber != null)
            tvNormalCustomerNumber.text = TextHandler.Instance.manager.GetTextById(323) + " " + userAchievement.GetNumberForCustomerFoodByType(CustomerTypeEnum.Normal) + TextHandler.Instance.manager.GetTextById(82);
    }

    /// <summary>
    /// 初始化客人团队
    /// </summary>
    public IEnumerator InitTeamCustomer()
    {
        CptUtil.RemoveChildsByActive(objTeamCustomerContainer);
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        UserAchievementBean userAchievement = gameData.GetAchievementData();
        if (tvTeamCustomerNumber != null)
            tvTeamCustomerNumber.text = TextHandler.Instance.manager.GetTextById(323) + " " + userAchievement.GetNumberForCustomerFoodByType(CustomerTypeEnum.Team) + TextHandler.Instance.manager.GetTextById(82);
        //查询所有团队
        List<NpcTeamBean> listNpcTeamData = NpcTeamHandler.Instance.manager.GetCustomerTeam();
        for (int i = 0; i < listNpcTeamData.Count; i++)
        {
            NpcTeamBean itemNpcTeamData = listNpcTeamData[i];
            GameObject objItem = Instantiate(objTeamCustomerContainer, objItemCustomerModel);
            ItemGameStatisticsForCustomerCpt itemCustomer = objItem.GetComponent<ItemGameStatisticsForCustomerCpt>();
            long[] teamLeaderIds = itemNpcTeamData.GetTeamLeaderId();
            CharacterBean teamLeaderData = NpcInfoHandler.Instance.manager.GetCharacterDataById(teamLeaderIds[0]);
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

        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        UserAchievementBean userAchievement = gameData.GetAchievementData();

        //设置数量
        if (tvFriendCustomerNumber != null)
            tvFriendCustomerNumber.text = TextHandler.Instance.manager.GetTextById(323) + " " + userAchievement.GetNumberForCustomerFoodByType(CustomerTypeEnum.Friend) + TextHandler.Instance.manager.GetTextById(82);
        List<CharacterFavorabilityBean> listData = gameData.listCharacterFavorability;

        foreach (CharacterFavorabilityBean itemData in listData)
        {
            CharacterBean itemCharacterData = NpcInfoHandler.Instance.manager.GetCharacterDataById(itemData.characterId);
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