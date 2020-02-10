using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class UIGameStatisticsForInn : UIGameStatisticsDetailsBase<UIGameStatistics>
{
    protected GameDataManager gameDataManager;

    public override void Awake()
    {
        base.Awake();
        gameDataManager = Find<GameDataManager>(ImportantTypeEnum.GameDataManager);
    }

    public override void Open()
    {
        base.Open();
        UserAchievementBean userAchievement = gameDataManager.gameData.GetAchievementData();
        CreateDataList(userAchievement);
    }

    public void CreateDataList(UserAchievementBean userAchievement)
    {
        CptUtil.RemoveChildsByActive(objItemContent);
        AddItemForCustomerNumber(userAchievement.numberForCustomer);
        AddItemForOwnMoney(userAchievement.ownMoneyL, userAchievement.ownMoneyM, userAchievement.ownMoneyS);
        AddItemForGuildCoin(userAchievement.ownGuildCoin);
        AddItemForOwnTrophy(
            userAchievement.ownTrophyElementary, 
            userAchievement.ownTrophyIntermediate, 
            userAchievement.ownTrophyAdvanced, 
            userAchievement.ownTrophyLegendary
            );

        //评价
        AddItemForPraiseNumber(PraiseTypeEnum.Excited, userAchievement.praiseForExcited);
        AddItemForPraiseNumber(PraiseTypeEnum.Happy, userAchievement.praiseForHappy);
        AddItemForPraiseNumber(PraiseTypeEnum.Okay, userAchievement.praiseForOkay);
        AddItemForPraiseNumber(PraiseTypeEnum.Ordinary, userAchievement.praiseForOrdinary);
        AddItemForPraiseNumber(PraiseTypeEnum.Disappointed, userAchievement.praiseForDisappointed);
        AddItemForPraiseNumber(PraiseTypeEnum.Anger, userAchievement.praiseForAnger);
    }

    /// <summary>
    /// 顾客数量
    /// </summary>
    /// <param name="number"></param>
    public void AddItemForCustomerNumber(long number)
    {
        Sprite spIcon = iconDataManager.GetIconSpriteByName("team_2");
        CreateTextItem(spIcon, Color.red, GameCommonInfo.GetUITextById(301), number + GameCommonInfo.GetUITextById(82));
    }

    /// <summary>
    /// 总记拥有
    /// </summary>
    /// <param name="moneyL"></param>
    /// <param name="moneyM"></param>
    /// <param name="moneyS"></param>
    public void AddItemForOwnMoney(long moneyL, long moneyM, long moneyS)
    {
        Sprite spIconS = iconDataManager.GetIconSpriteByName("money_1");
        CreateTextItem(spIconS, GameCommonInfo.GetUITextById(303), moneyS + "");
        Sprite spIconM = iconDataManager.GetIconSpriteByName("money_2");
        CreateTextItem(spIconM, GameCommonInfo.GetUITextById(304), moneyM + "");
        Sprite spIconL = iconDataManager.GetIconSpriteByName("money_3");
        CreateTextItem(spIconL, GameCommonInfo.GetUITextById(305), moneyL + "");
        //CreateMoneyItem(spIconL, GameCommonInfo.GetUITextById(305), moneyL, moneyM, moneyS);
    }

    /// <summary>
    /// 拥有公会硬币
    /// </summary>
    /// <param name="number"></param>
    public void AddItemForGuildCoin(long number)
    {
        Sprite spIconL = iconDataManager.GetIconSpriteByName("guild_coin_2");
        CreateTextItem(spIconL, GameCommonInfo.GetUITextById(306), number + "");
    }

    /// <summary>
    /// 拥有京北
    /// </summary>
    /// <param name="ownTrophyElementary"></param>
    /// <param name="ownTrophyIntermediate"></param>
    /// <param name="ownTrophyAdvanced"></param>
    /// <param name="ownTrophyLegendary"></param>
    public void AddItemForOwnTrophy(
        long ownTrophyElementary, //竞技场初级奖杯
        long ownTrophyIntermediate,//竞技场中级奖杯
        long ownTrophyAdvanced,//竞技场高级奖杯
        long ownTrophyLegendary)//竞技场传说奖杯)
    {
        Sprite spIcon1 = iconDataManager.GetIconSpriteByName("trophy_1_0");
        CreateTextItem(spIcon1, GameCommonInfo.GetUITextById(307), ownTrophyElementary + "");
        Sprite spIcon2 = iconDataManager.GetIconSpriteByName("trophy_1_1");
        CreateTextItem(spIcon2, GameCommonInfo.GetUITextById(308), ownTrophyIntermediate + "");
        Sprite spIcon3 = iconDataManager.GetIconSpriteByName("trophy_1_2");
        CreateTextItem(spIcon3, GameCommonInfo.GetUITextById(309), ownTrophyAdvanced + "");
        Sprite spIcon4 = iconDataManager.GetIconSpriteByName("trophy_1_3");
        CreateTextItem(spIcon4, GameCommonInfo.GetUITextById(310), ownTrophyLegendary + "");
    }

    /// <summary>
    /// 好评数量
    /// </summary>
    /// <param name="praiseType"></param>
    /// <param name="number"></param>
    public void AddItemForPraiseNumber(PraiseTypeEnum praiseType,long number)
    {
        string iconKey = "";
        switch (praiseType)
        {
            case PraiseTypeEnum.Excited:
                iconKey = "customer_mood_0";
                break;
            case PraiseTypeEnum.Happy:
                iconKey = "customer_mood_1";
                break;
            case PraiseTypeEnum.Okay:
                iconKey = "customer_mood_2";
                break;
            case PraiseTypeEnum.Ordinary:
                iconKey = "customer_mood_3";
                break;
            case PraiseTypeEnum.Disappointed:
                iconKey = "customer_mood_4";
                break;
            case PraiseTypeEnum.Anger:
                iconKey = "customer_mood_5";
                break;
        }
        Sprite spIcon = iconDataManager.GetIconSpriteByName(iconKey);
        CreateTextItem(spIcon, GameCommonInfo.GetUITextById(336), number + "");
    }

}