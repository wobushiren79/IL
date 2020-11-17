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
        AddItemForCustomerNumber(
            userAchievement.GetNumberForAllCustomerFood(),
            userAchievement.GetNumberForAllCustomerFoodComplete(),
            userAchievement.GetNumberForAllCustomerHotel(),
            userAchievement.GetNumberForAllCustomerHotelComplete());

        //金钱
        AddItemForOwnMoney(MoneyEnum.L, userAchievement.ownMoneyL);
        AddItemForOwnMoney(MoneyEnum.M, userAchievement.ownMoneyM);
        AddItemForOwnMoney(MoneyEnum.S, userAchievement.ownMoneyS);

        //金钱
        AddItemForPayMoney(MoneyEnum.L, userAchievement.expendMoneyL);
        AddItemForPayMoney(MoneyEnum.M, userAchievement.expendMoneyM);
        AddItemForPayMoney(MoneyEnum.S, userAchievement.expendMoneyS);

        //公会勋章
        AddItemForGuildCoin(userAchievement.ownGuildCoin);
        //奖杯
        AddItemForOwnTrophy(TrophyTypeEnum.Elementary, userAchievement.ownTrophyElementary);
        AddItemForOwnTrophy(TrophyTypeEnum.Intermediate, userAchievement.ownTrophyIntermediate);
        AddItemForOwnTrophy(TrophyTypeEnum.Advanced, userAchievement.ownTrophyAdvanced);
        AddItemForOwnTrophy(TrophyTypeEnum.Legendary, userAchievement.ownTrophyLegendary);
        //评价
        AddItemForPraiseNumber(PraiseTypeEnum.Excited, userAchievement.praiseForExcited);
        AddItemForPraiseNumber(PraiseTypeEnum.Happy, userAchievement.praiseForHappy);
        AddItemForPraiseNumber(PraiseTypeEnum.Okay, userAchievement.praiseForOkay);
        AddItemForPraiseNumber(PraiseTypeEnum.Ordinary, userAchievement.praiseForOrdinary);
        AddItemForPraiseNumber(PraiseTypeEnum.Disappointed, userAchievement.praiseForDisappointed);
        AddItemForPraiseNumber(PraiseTypeEnum.Anger, userAchievement.praiseForAnger);
        //单日最高赚取金钱
        AddItemForMaxDayGetMoney(
            userAchievement.maxDayGetMoneyL, userAchievement.maxDayGetMoneyM, userAchievement.maxDayGetMoneyS,
            userAchievement.maxDayGetMoneyForHotelL, userAchievement.maxDayGetMoneyForHotelM, userAchievement.maxDayGetMoneyForHotelS);
        //单日最高客流量
        AddItemForMaxDayCompleteOrder(userAchievement.maxDayCompleteOrder, userAchievement.maxDayCompleteOrderForHotel);
    }

    /// <summary>
    /// 单日最高获取金钱
    /// </summary>
    /// <param name="moneyL"></param>
    /// <param name="moneyM"></param>
    /// <param name="moneyS"></param>
    public void AddItemForMaxDayGetMoney(long moneyFoodL, long moneyFoodM, long moneyFoodS, long moneyHotelL, long moneyHotelM, long moneyHotelS)
    {
        Sprite spIcon_1 = iconDataManager.GetIconSpriteByName("money_1");
        CreateTextItem(spIcon_1, Color.white, GameCommonInfo.GetUITextById(340), moneyFoodS + GameCommonInfo.GetUITextById(18));
        Sprite spIcon_2 = iconDataManager.GetIconSpriteByName("money_1");
        CreateTextItem(spIcon_2, Color.white, GameCommonInfo.GetUITextById(345), moneyHotelS + GameCommonInfo.GetUITextById(18));
    }

    /// <summary>
    /// 单日最高完成订单数量
    /// </summary>
    /// <param name="orderNumber"></param>
    public void AddItemForMaxDayCompleteOrder(long orderFoodNumber,long orderHotelNumber)
    {
        Sprite spIcon_1 = iconDataManager.GetIconSpriteByName("ach_ordernumber_1");
        CreateTextItem(spIcon_1, Color.white, GameCommonInfo.GetUITextById(341), orderFoodNumber + "");
        Sprite spIcon_2 = iconDataManager.GetIconSpriteByName("worker_waiter_bed_pro_2");
        CreateTextItem(spIcon_2, Color.white, GameCommonInfo.GetUITextById(346), orderHotelNumber + "");
    }

    /// <summary>
    /// 顾客数量
    /// </summary>
    /// <param name="number"></param>
    public void AddItemForCustomerNumber(
        long numberForCustomerFood, 
        long numberForCustomerFoodComplete,
        long numberForCustomerHotel,
        long numberForCustomerHotelComplete)
    {
        Sprite spIcon_1 = iconDataManager.GetIconSpriteByName("team_2");
        CreateTextItem(spIcon_1, Color.red, GameCommonInfo.GetUITextById(301), numberForCustomerFood + GameCommonInfo.GetUITextById(82));
        Sprite spIcon_2 = iconDataManager.GetIconSpriteByName("team_2");
        CreateTextItem(spIcon_2, Color.red, GameCommonInfo.GetUITextById(338), numberForCustomerFoodComplete + GameCommonInfo.GetUITextById(82));
        Sprite spIcon_3 = iconDataManager.GetIconSpriteByName("worker_waiter_bed_pro_2");
        CreateTextItem(spIcon_3, Color.white, GameCommonInfo.GetUITextById(342), numberForCustomerHotel + GameCommonInfo.GetUITextById(82));
        Sprite spIcon_4 = iconDataManager.GetIconSpriteByName("worker_waiter_bed_pro_2");
        CreateTextItem(spIcon_4, Color.white, GameCommonInfo.GetUITextById(343), numberForCustomerHotelComplete + GameCommonInfo.GetUITextById(82));
    }

    /// <summary>
    /// 总记拥有
    /// </summary>
    /// <param name="moneyL"></param>
    /// <param name="moneyM"></param>
    /// <param name="moneyS"></param>
    public void AddItemForOwnMoney(MoneyEnum moneyType,  long money)
    {
        string iconKey = "";
        string contentStr = "";
        switch(moneyType)
        {
            case MoneyEnum.L:
                iconKey = "money_3";
                contentStr = GameCommonInfo.GetUITextById(305);
                break;
            case MoneyEnum.M:
                iconKey = "money_2";
                contentStr = GameCommonInfo.GetUITextById(304);
                break;
            case MoneyEnum.S:
                iconKey = "money_1";
                contentStr = GameCommonInfo.GetUITextById(303);
                break;
        }
        Sprite spIcon = iconDataManager.GetIconSpriteByName(iconKey);
        CreateTextItem(spIcon, contentStr, money + "");
        //CreateMoneyItem(spIconL, GameCommonInfo.GetUITextById(305), moneyL, moneyM, moneyS);
    }

    /// <summary>
    /// 总记支出
    /// </summary>
    /// <param name="moneyL"></param>
    /// <param name="moneyM"></param>
    /// <param name="moneyS"></param>
    public void AddItemForPayMoney(MoneyEnum moneyType, long money)
    {
        string iconKey = "";
        string contentStr = "";
        switch (moneyType)
        {
            case MoneyEnum.L:
                iconKey = "money_3";
                contentStr = GameCommonInfo.GetUITextById(363);
                break;
            case MoneyEnum.M:
                iconKey = "money_2";
                contentStr = GameCommonInfo.GetUITextById(362);
                break;
            case MoneyEnum.S:
                iconKey = "money_1";
                contentStr = GameCommonInfo.GetUITextById(361);
                break;
        }
        Sprite spIcon = iconDataManager.GetIconSpriteByName(iconKey);
        CreateTextItem(spIcon, contentStr,Color.red, money + "");
    }

    /// <summary>
    /// 拥有公会硬币
    /// </summary>
    /// <param name="number"></param>
    public void AddItemForGuildCoin(long number)
    {
        Sprite spIcon = iconDataManager.GetIconSpriteByName("guild_coin_2");
        CreateTextItem(spIcon, GameCommonInfo.GetUITextById(306), number + "");
    }

    /// <summary>
    /// 拥有奖杯
    /// </summary>
    /// <param name="trophyType"></param>
    /// <param name="number"></param>
    public void AddItemForOwnTrophy(TrophyTypeEnum trophyType,long number)
    {
        string iconKey = "";
        string contentStr = "";
        switch (trophyType)
        {
            case TrophyTypeEnum.Elementary:
                iconKey = "trophy_1_0";
                contentStr = GameCommonInfo.GetUITextById(307);
                break;
            case TrophyTypeEnum.Intermediate:
                iconKey = "trophy_1_1";
                contentStr = GameCommonInfo.GetUITextById(308);
                break;
            case TrophyTypeEnum.Advanced:
                iconKey = "trophy_1_2";
                contentStr = GameCommonInfo.GetUITextById(309);
                break;
            case TrophyTypeEnum.Legendary:
                iconKey = "trophy_1_3";
                contentStr = GameCommonInfo.GetUITextById(310);
                break;
        }
        Sprite spIcon = iconDataManager.GetIconSpriteByName(iconKey);
        CreateTextItem(spIcon, contentStr, number + "");
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