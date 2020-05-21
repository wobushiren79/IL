using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
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
        InitTeamCustomer();
        InitFriendCustomer();
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
    public void InitTeamCustomer()
    {
        UserAchievementBean userAchievement = uiComponent.uiGameManager.gameDataManager.gameData.GetAchievementData();
        if (tvTeamCustomerNumber != null)
            tvTeamCustomerNumber.text = GameCommonInfo.GetUITextById(323) + " " + userAchievement.GetNumberForCustomerByType(CustomerTypeEnum.Team) + GameCommonInfo.GetUITextById(82);

    }

    /// <summary>
    /// 初始化朋友
    /// </summary>
    public void InitFriendCustomer()
    {
        GameDataManager gameDataManager = uiComponent.uiGameManager.gameDataManager;
        UserAchievementBean userAchievement = gameDataManager.gameData.GetAchievementData();
        gameDataManager.gameData.GetCharacterFavorability();
        if (tvFriendCustomerNumber != null)
            tvFriendCustomerNumber.text = GameCommonInfo.GetUITextById(323) + " " + userAchievement.GetNumberForCustomerByType(CustomerTypeEnum.Friend) + GameCommonInfo.GetUITextById(82);

    }
}