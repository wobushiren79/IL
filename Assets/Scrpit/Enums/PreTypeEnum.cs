using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public enum PreTypeEnum
{
    PayMoneyL,//支付金钱L
    PayMoneyM,//支付金钱M
    PayMoneyS,//支付金钱S
    HaveMoneyL,//当前拥有金钱
    HaveMoneyM,//当前拥有金钱
    HaveMoneyS,//当前拥有金钱

    PayGuildCoin,//支付公会硬币
    HaveGuildCoin,//拥有公会硬币

    PayTrophyElementary,  //斗技奖杯
    PayTrophyIntermediate,
    PayTrophyAdvanced,
    PayTrophyLegendary,

    HaveTrophyElementary,
    HaveTrophyIntermediate,
    HaveTrophyAdvanced,
    HaveTrophyLegendary,

    PayItems,//支付道具  ,分隔 前ID 后数量
    HaveItems,//有道具 ,分隔 前ID 后数量
    AttributeForForce,//达标属性

    AttributeForSpeed,
    AttributeForAccount,
    AttributeForCharm,
    AttributeForCook,
    AttributeForLucky,
    AttributeForLife,

    PayIngForOilsalt,//油盐
    PayIngForMeat,//鲜肉
    PayIngForRiverfresh,//河鲜
    PayIngForSeafood,//海鲜
    PayIngForVegetables,//蔬菜
    PayIngForMelonfruit,//水果
    PayIngForWaterwine,//酒水
    PayIngForFlour,//面粉

    OrderNumberForTotal,//总共接待食客客数量
    OrderNumberForHotelTotal,//总计接待住客数量

    GetMoneyL,//赚取金钱
    GetMoneyM,
    GetMoneyS,

    InnPraiseNumberForExcited,//评价数量
    InnPraiseNumberForHappy,
    InnPraiseNumberForOkay,
    InnPraiseNumberForOrdinary,
    InnPraiseNumberForDisappointed,
    InnPraiseNumberForAnger,

    BedNumber,//拥有床位数量
    BedLevelStarNumber,
    BedLevelMoonNumber,
    BedLevelSunNumber,

    MenuNumber,//拥有菜品数量
    MenuLevelStarNumber,
    MenuLevelMoonNumber,
    MenuLevelSunNumber,

    SellMenuNumber,//卖出菜品数量
    HaveMenu,//拥有菜谱

    WorkerForCookFoodNumber,//工作相关统计
    WorkerForCleanFoodNumber,
    WorkerForSendFoodNumber,
    WorkerForCleanBedNumber,
    WorkerForAccountantSuccessNumber,
    WorkerForAccountantFailNumber,
    WorkerForAccostSuccessNumber,
    WorkerForAccostFailNumber,
    WorkerForAccostGuideNumber,
    WorkerForFightSuccessNumber,
    WorkerForFightFailNumber,

    InnPraise,//客栈好评度
    InnAesthetics,//客栈美观值

    NpcFavorabilityLevel,//NPC好感度

    InfiniteTowersMaxLayer//无尽之塔最高层数
}

public class PreTypeBean : DataBean<PreTypeEnum>
{
    public bool isPre;
    public float progress;
    //图标颜色
    public Color colorPreIcon = Color.white;
    public Sprite spPreIcon;
    public string preDescribe;
    //准备失败文字
    public string preFailStr;


    public PreTypeBean() : base(PreTypeEnum.PayMoneyS, "")
    {

    }

}

public class PreTypeEnumTools : DataTools
{
    /// <summary>
    /// 检测是否全部准备就绪
    /// </summary>
    /// <param name="gameData"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static bool CheckIsAllPre(IconDataManager iconDataManager,InnFoodManager innFoodManager, NpcInfoManager npcInfoManager, GameDataBean gameData, CharacterBean characterData, string data, out string reason)
    {
        List<PreTypeBean> listPreData = GetListPreData(data);
        reason = "";
        foreach (var itemPreData in listPreData)
        {
            GetPreDetails(itemPreData, gameData, characterData, iconDataManager,innFoodManager, npcInfoManager, false);
            if (!itemPreData.isPre)
            {
                reason = itemPreData.preFailStr;
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 获取前置条件
    /// </summary>
    /// <returns></returns>
    public static List<PreTypeBean> GetListPreData(string data)
    {
        return GetListData<PreTypeBean, PreTypeEnum>(data);
    }

    /// <summary>
    /// 获取前置详情
    /// </summary>
    /// <param name="rewardType"></param>
    /// <returns></returns>
    public static PreTypeBean GetPreDetails(PreTypeBean preTypeData, GameDataBean gameData, CharacterBean characterData, IconDataManager iconDataManager, InnFoodManager innFoodManager, NpcInfoManager npcInfoManager, bool isComplete)
    {
        switch (preTypeData.dataType)
        {
            case PreTypeEnum.PayMoneyL:
            case PreTypeEnum.PayMoneyM:
            case PreTypeEnum.PayMoneyS:
            case PreTypeEnum.PayGuildCoin:
            case PreTypeEnum.PayTrophyElementary:
            case PreTypeEnum.PayTrophyIntermediate:
            case PreTypeEnum.PayTrophyAdvanced:
            case PreTypeEnum.PayTrophyLegendary:
                GetPreDetailsForPay(preTypeData, gameData, iconDataManager, isComplete);
                break;
            case PreTypeEnum.HaveMoneyL:
            case PreTypeEnum.HaveMoneyM:
            case PreTypeEnum.HaveMoneyS:
            case PreTypeEnum.HaveGuildCoin:
            case PreTypeEnum.HaveTrophyElementary:
            case PreTypeEnum.HaveTrophyIntermediate:
            case PreTypeEnum.HaveTrophyAdvanced:
            case PreTypeEnum.HaveTrophyLegendary:
                GetPreDetailsForHave(preTypeData, gameData, iconDataManager, isComplete);
                break;
            case PreTypeEnum.AttributeForForce:
            case PreTypeEnum.AttributeForSpeed:
            case PreTypeEnum.AttributeForLucky:
            case PreTypeEnum.AttributeForCook:
            case PreTypeEnum.AttributeForCharm:
            case PreTypeEnum.AttributeForAccount:
            case PreTypeEnum.AttributeForLife:
                GetPreDetailsForAttributes(preTypeData, characterData, iconDataManager);
                break;
            case PreTypeEnum.PayItems:
            case PreTypeEnum.HaveItems:
                GetPreDetailsForItems(iconDataManager, preTypeData, gameData, isComplete);
                break;

            case PreTypeEnum.PayIngForOilsalt:
            case PreTypeEnum.PayIngForMeat:
            case PreTypeEnum.PayIngForRiverfresh:
            case PreTypeEnum.PayIngForSeafood:
            case PreTypeEnum.PayIngForVegetables:
            case PreTypeEnum.PayIngForMelonfruit:
            case PreTypeEnum.PayIngForWaterwine:
            case PreTypeEnum.PayIngForFlour:
                GetPreDetailsForPayIng(preTypeData, gameData, iconDataManager);
                break;
            case PreTypeEnum.OrderNumberForTotal:
            case PreTypeEnum.OrderNumberForHotelTotal:
                GetPreDetailsForOrderNumber(preTypeData, gameData, iconDataManager, isComplete);
                break;
            case PreTypeEnum.GetMoneyL:
            case PreTypeEnum.GetMoneyM:
            case PreTypeEnum.GetMoneyS:
                GetPreDetailsForGetMoney(preTypeData, gameData, iconDataManager, isComplete);
                break;
            case PreTypeEnum.InnPraiseNumberForExcited:
            case PreTypeEnum.InnPraiseNumberForHappy:
            case PreTypeEnum.InnPraiseNumberForOkay:
            case PreTypeEnum.InnPraiseNumberForOrdinary:
            case PreTypeEnum.InnPraiseNumberForDisappointed:
            case PreTypeEnum.InnPraiseNumberForAnger:
                GetPreDetailsForInnPraiseNumber(preTypeData, gameData, iconDataManager, isComplete);
                break;
            case PreTypeEnum.MenuNumber:
            case PreTypeEnum.MenuLevelStarNumber:
            case PreTypeEnum.MenuLevelMoonNumber:
            case PreTypeEnum.MenuLevelSunNumber:
                GetPreDetailsForMenuNumber(preTypeData, gameData, iconDataManager, isComplete);
                break;
            case PreTypeEnum.BedNumber:
            case PreTypeEnum.BedLevelStarNumber:
            case PreTypeEnum.BedLevelMoonNumber:
            case PreTypeEnum.BedLevelSunNumber:
                GetPreDetailsForBedNumber(preTypeData, gameData, iconDataManager, isComplete);
                break;
            case PreTypeEnum.SellMenuNumber:
                GetPreDetailsForSellMenuNumber(preTypeData, gameData, iconDataManager, isComplete);
                break;
            case PreTypeEnum.HaveMenu:
                GetPreDetailsForHaveMenu(preTypeData, gameData, innFoodManager);
                break;
            case PreTypeEnum.WorkerForCookFoodNumber:

            case PreTypeEnum.WorkerForCleanFoodNumber:
            case PreTypeEnum.WorkerForSendFoodNumber:
            case PreTypeEnum.WorkerForCleanBedNumber:

            case PreTypeEnum.WorkerForAccountantSuccessNumber:
            case PreTypeEnum.WorkerForAccountantFailNumber:
            case PreTypeEnum.WorkerForAccostSuccessNumber:
            case PreTypeEnum.WorkerForAccostFailNumber:
            case PreTypeEnum.WorkerForAccostGuideNumber:
            case PreTypeEnum.WorkerForFightSuccessNumber:
            case PreTypeEnum.WorkerForFightFailNumber:
                GetPreDetailsForWorker(preTypeData, gameData, iconDataManager, isComplete);
                break;
            case PreTypeEnum.InnPraise:
                GetPreDetailsForInnPraise(preTypeData, gameData, iconDataManager, isComplete);
                break;
            case PreTypeEnum.InnAesthetics:
                GetPreDetailsForInnAesthetics(preTypeData, gameData, iconDataManager, isComplete);
                break;
            case PreTypeEnum.NpcFavorabilityLevel:
                GetPreDetailsForNpcFavorabilityLevel(gameData, preTypeData, npcInfoManager);
                break;
            case PreTypeEnum.InfiniteTowersMaxLayer:
                GetPreDetailsForInfiniteTowersMaxLayer(preTypeData, gameData, iconDataManager, isComplete);
                break;
        }
        return preTypeData;
    }
    public static PreTypeBean GetPreDetails(PreTypeBean preTypeData, GameDataBean gameData, IconDataManager iconDataManager, InnFoodManager innFoodManager, NpcInfoManager npcInfoManager, bool isComplete)
    {
        return GetPreDetails(preTypeData, gameData, null, iconDataManager, innFoodManager, npcInfoManager, isComplete);
    }
    public static PreTypeBean GetPreDetails(PreTypeBean preTypeData, GameDataBean gameData, IconDataManager iconDataManager,InnFoodManager innFoodManager, NpcInfoManager npcInfoManager)
    {
        return GetPreDetails(preTypeData, gameData, null, iconDataManager, innFoodManager, npcInfoManager, false);
    }

    /// <summary>
    /// 获取拥有物品详情
    /// </summary>
    /// <param name="gameItemsManager"></param>
    /// <param name="iconDataManager"></param>
    /// <param name="characterDressManager"></param>
    /// <param name="preTypeData"></param>
    /// <param name="gameData"></param>
    /// <param name="isComplete"></param>
    /// <returns></returns>
    private static PreTypeBean GetPreDetailsForItems(
        IconDataManager iconDataManager,
        PreTypeBean preTypeData, GameDataBean gameData, bool isComplete)
    {
        long[] listItems = StringUtil.SplitBySubstringForArrayLong(preTypeData.data, ',');
        preTypeData.isPre = true;
        long itemsId = listItems[0];
        long itemsNumber = 1;
        if (listItems.Length >= 2)
        {
            itemsNumber = listItems[1];
        }
        gameData.CheckHasItems(itemsId, out bool hasItems, out long number);
        if ((hasItems && number >= itemsNumber) || isComplete)
        {
            preTypeData.progress = 1;
            preTypeData.isPre = true;
        }
        else
        {
            preTypeData.progress = number / (float)itemsNumber;
            preTypeData.isPre = false;
        }
        ItemsInfoBean itemsInfo = GameItemsHandler.Instance.manager.GetItemsById(itemsId);
        preTypeData.spPreIcon = GeneralEnumTools.GetGeneralSprite(itemsInfo, iconDataManager);
        switch (preTypeData.dataType)
        {
            case PreTypeEnum.HaveItems:
                preTypeData.preDescribe = string.Format(GameCommonInfo.GetUITextById(5021), itemsInfo.name, itemsNumber + "");
                break;
            case PreTypeEnum.PayItems:
                preTypeData.preDescribe = string.Format(GameCommonInfo.GetUITextById(5022), itemsInfo.name, itemsNumber + "");
                break;
        }
        preTypeData.preFailStr = string.Format(GameCommonInfo.GetUITextById(5023), itemsInfo.name, itemsNumber + "");
        return preTypeData;
    }

    /// <summary>
    /// 获取支付相关详情
    /// </summary>
    /// <param name="preTypeData"></param>
    /// <param name="gameData"></param>
    /// <param name="iconDataManager"></param>
    /// <param name="isComplete"></param>
    /// <returns></returns>
    private static PreTypeBean GetPreDetailsForPay(PreTypeBean preTypeData, GameDataBean gameData, IconDataManager iconDataManager, bool isComplete)
    {
        string preStr = "";
        long pay = long.Parse(preTypeData.data);
        long have = 0;
        string iconKey = "";
        switch (preTypeData.dataType)
        {
            case PreTypeEnum.PayMoneyL:
                have = gameData.moneyL;
                preTypeData.preDescribe = GameCommonInfo.GetUITextById(5001);
                preTypeData.preFailStr = GameCommonInfo.GetUITextById(1005);
                iconKey = "money_3";
                break;
            case PreTypeEnum.PayMoneyM:
                have = gameData.moneyM;
                preTypeData.preDescribe = GameCommonInfo.GetUITextById(5002);
                preTypeData.preFailStr = GameCommonInfo.GetUITextById(1005);
                iconKey = "money_2";
                break;
            case PreTypeEnum.PayMoneyS:
                have = gameData.moneyS;
                preTypeData.preDescribe = GameCommonInfo.GetUITextById(5003);
                preTypeData.preFailStr = GameCommonInfo.GetUITextById(1005);
                iconKey = "money_1";
                break;
            case PreTypeEnum.PayGuildCoin:
                have = gameData.guildCoin;
                preTypeData.preDescribe = GameCommonInfo.GetUITextById(5201);
                preTypeData.preFailStr = GameCommonInfo.GetUITextById(1012);
                iconKey = "guild_coin_2";
                break;
            case PreTypeEnum.PayTrophyElementary:
                have = gameData.trophyElementary;
                preTypeData.preDescribe = GameCommonInfo.GetUITextById(5203);
                preTypeData.preFailStr = GameCommonInfo.GetUITextById(1021);
                iconKey = "trophy_1_0";
                break;
            case PreTypeEnum.PayTrophyIntermediate:
                have = gameData.trophyIntermediate;
                preTypeData.preDescribe = GameCommonInfo.GetUITextById(5204);
                preTypeData.preFailStr = GameCommonInfo.GetUITextById(1021);
                iconKey = "trophy_1_1";
                break;
            case PreTypeEnum.PayTrophyAdvanced:
                have = gameData.trophyAdvanced;
                preTypeData.preDescribe = GameCommonInfo.GetUITextById(5205);
                preTypeData.preFailStr = GameCommonInfo.GetUITextById(1021);
                iconKey = "trophy_1_2";
                break;
            case PreTypeEnum.PayTrophyLegendary:
                have = gameData.trophyLegendary;
                preTypeData.preDescribe = GameCommonInfo.GetUITextById(5206);
                preTypeData.preFailStr = GameCommonInfo.GetUITextById(1021);
                iconKey = "trophy_1_3";
                break;
        }
        if (have >= pay || isComplete)
        {
            preTypeData.isPre = true;
            preStr = "(" + pay + "/" + pay + ")";
            preTypeData.progress = 1;
        }
        else
        {
            preTypeData.isPre = false;
            preStr = "(" + have + "/" + pay + ")";
            preTypeData.progress = have / (float)pay;
        }
        if (iconDataManager != null)
            preTypeData.spPreIcon = iconDataManager.GetIconSpriteByName(iconKey);
        preTypeData.preDescribe = string.Format(preTypeData.preDescribe, preStr);
        return preTypeData;
    }


    /// <summary>
    /// 获取拥有相关详情
    /// </summary>
    /// <param name="preTypeData"></param>
    /// <param name="gameData"></param>
    /// <param name="iconDataManager"></param>
    /// <param name="isComplete"></param>
    /// <returns></returns>
    private static PreTypeBean GetPreDetailsForHave(PreTypeBean preTypeData, GameDataBean gameData, IconDataManager iconDataManager, bool isComplete)
    {
        long have = long.Parse(preTypeData.data);
        long own = 0;
        string haveStr = "";
        string iconKey = "";
        switch (preTypeData.dataType)
        {
            case PreTypeEnum.HaveMoneyL:
                own = gameData.moneyL;
                preTypeData.preFailStr = GameCommonInfo.GetUITextById(1005);
                preTypeData.preDescribe = GameCommonInfo.GetUITextById(5004);
                iconKey = "money_3";
                break;
            case PreTypeEnum.HaveMoneyM:
                own = gameData.moneyM;
                preTypeData.preFailStr = GameCommonInfo.GetUITextById(1005);
                preTypeData.preDescribe = GameCommonInfo.GetUITextById(5005);
                iconKey = "money_2";
                break;
            case PreTypeEnum.HaveMoneyS:
                own = gameData.moneyS;
                preTypeData.preFailStr = GameCommonInfo.GetUITextById(1005);
                preTypeData.preDescribe = GameCommonInfo.GetUITextById(5006);
                iconKey = "money_1";
                break;
            case PreTypeEnum.PayGuildCoin:
                own = gameData.guildCoin;
                preTypeData.preDescribe = GameCommonInfo.GetUITextById(5202);
                preTypeData.preFailStr = GameCommonInfo.GetUITextById(1012);
                iconKey = "guild_coin_2";
                break;
            case PreTypeEnum.PayTrophyElementary:
                own = gameData.trophyElementary;
                preTypeData.preDescribe = GameCommonInfo.GetUITextById(5207);
                preTypeData.preFailStr = GameCommonInfo.GetUITextById(1021);
                iconKey = "trophy_1_0";
                break;
            case PreTypeEnum.PayTrophyIntermediate:
                own = gameData.trophyIntermediate;
                preTypeData.preDescribe = GameCommonInfo.GetUITextById(5208);
                preTypeData.preFailStr = GameCommonInfo.GetUITextById(1021);
                iconKey = "trophy_1_1";
                break;
            case PreTypeEnum.PayTrophyAdvanced:
                own = gameData.trophyAdvanced;
                preTypeData.preDescribe = GameCommonInfo.GetUITextById(5209);
                preTypeData.preFailStr = GameCommonInfo.GetUITextById(1021);
                iconKey = "trophy_1_2";
                break;
            case PreTypeEnum.PayTrophyLegendary:
                own = gameData.trophyLegendary;
                preTypeData.preDescribe = GameCommonInfo.GetUITextById(5210);
                preTypeData.preFailStr = GameCommonInfo.GetUITextById(1021);
                iconKey = "trophy_1_3";
                break;
        }

        if (own >= have || isComplete)
        {
            preTypeData.isPre = true;
            haveStr = "(" + have + "/" + have + ")";
            preTypeData.progress = 1;
        }
        else
        {
            preTypeData.isPre = false;
            haveStr = "(" + own + "/" + have + ")";
            preTypeData.progress = own / (float)have;
        }
        if (iconDataManager != null)
            preTypeData.spPreIcon = iconDataManager.GetIconSpriteByName(iconKey);
        preTypeData.preDescribe = string.Format(preTypeData.preDescribe, haveStr);
        return preTypeData;
    }

    /// <summary>
    /// 获取属性相关详情
    /// </summary>
    /// <param name="preTypeData"></param>
    /// <param name="characterData"></param>
    /// <param name="iconDataManager"></param>
    /// <returns></returns>
    private static PreTypeBean GetPreDetailsForAttributes(PreTypeBean preTypeData, CharacterBean characterData, IconDataManager iconDataManager)
    {
        if (characterData == null)
            return preTypeData;
        int targetAttributes = 0;
        string iconKey = "";
        int dataAttributes = int.Parse(preTypeData.data);
        switch (preTypeData.dataType)
        {
            case PreTypeEnum.AttributeForForce:
                targetAttributes = characterData.attributes.force;
                iconKey = "ui_ability_force";
                preTypeData.preFailStr = GameCommonInfo.GetUITextById(5010);
                break;
            case PreTypeEnum.AttributeForSpeed:
                targetAttributes = characterData.attributes.speed;
                iconKey = "ui_ability_speed";
                preTypeData.preFailStr = GameCommonInfo.GetUITextById(5011);
                break;
            case PreTypeEnum.AttributeForLucky:
                targetAttributes = characterData.attributes.lucky;
                iconKey = "ui_ability_lucky";
                preTypeData.preFailStr = GameCommonInfo.GetUITextById(5012);
                break;
            case PreTypeEnum.AttributeForCook:
                targetAttributes = characterData.attributes.cook;
                iconKey = "ui_ability_cook";
                preTypeData.preFailStr = GameCommonInfo.GetUITextById(5013);
                break;
            case PreTypeEnum.AttributeForAccount:
                targetAttributes = characterData.attributes.account;
                iconKey = "ui_ability_account";
                preTypeData.preFailStr = GameCommonInfo.GetUITextById(5014);
                break;
            case PreTypeEnum.AttributeForCharm:
                targetAttributes = characterData.attributes.charm;
                iconKey = "ui_ability_charm";
                preTypeData.preFailStr = GameCommonInfo.GetUITextById(5015);
                break;
            case PreTypeEnum.AttributeForLife:
                targetAttributes = characterData.attributes.life;
                iconKey = "ui_ability_life";
                preTypeData.preFailStr = GameCommonInfo.GetUITextById(5016);
                break;
        }
        if (iconDataManager != null)
            preTypeData.spPreIcon = iconDataManager.GetIconSpriteByName(iconKey);
        preTypeData.preFailStr = string.Format(preTypeData.preFailStr, dataAttributes + "");
        if (targetAttributes < dataAttributes)
        {
            preTypeData.isPre = false;
        }
        else
        {
            preTypeData.isPre = true;
        }
        return preTypeData;
    }


    /// <summary>
    /// 获取支付食材相关详情
    /// </summary>
    /// <param name="preTypeData"></param>
    /// <param name="gameData"></param>
    /// <param name="iconDataManager"></param>
    /// <returns></returns>
    private static PreTypeBean GetPreDetailsForPayIng(PreTypeBean preTypeData, GameDataBean gameData, IconDataManager iconDataManager)
    {
        Sprite spIcon = null;
        int dataIng = int.Parse(preTypeData.data);
        IngredientsEnum ingredients = IngredientsEnum.Oilsalt;
        switch (preTypeData.dataType)
        {
            case PreTypeEnum.PayIngForOilsalt:
                ingredients = IngredientsEnum.Oilsalt;
                break;
            case PreTypeEnum.PayIngForMeat:
                ingredients = IngredientsEnum.Meat;
                break;
            case PreTypeEnum.PayIngForRiverfresh:
                ingredients = IngredientsEnum.Riverfresh;
                break;
            case PreTypeEnum.PayIngForSeafood:
                ingredients = IngredientsEnum.Seafood;
                break;
            case PreTypeEnum.PayIngForVegetables:
                ingredients = IngredientsEnum.Vegetables;
                break;
            case PreTypeEnum.PayIngForMelonfruit:
                ingredients = IngredientsEnum.Melonfruit;
                break;
            case PreTypeEnum.PayIngForWaterwine:
                ingredients = IngredientsEnum.Waterwine;
                break;
            case PreTypeEnum.PayIngForFlour:
                ingredients = IngredientsEnum.Flour;
                break;
        }
        string ingName = IngredientsEnumTools.GetIngredientName(ingredients);
        spIcon = IngredientsEnumTools.GetIngredientIcon(iconDataManager, ingredients);
        if (gameData.HasEnoughIng(ingredients, dataIng))
        {
            preTypeData.isPre = true;
        }
        else
        {
            preTypeData.isPre = false;
        }
        preTypeData.spPreIcon = spIcon;
        preTypeData.preDescribe = string.Format(GameCommonInfo.GetUITextById(5031), ingName, dataIng + "");
        preTypeData.preFailStr = string.Format(GameCommonInfo.GetUITextById(5023), ingName, dataIng + "");
        return preTypeData;
    }

    /// <summary>
    /// 获取订单数量相关详情
    /// </summary>
    /// <param name="preTypeData"></param>
    /// <param name="gameData"></param>
    /// <param name="iconDataManager"></param>
    /// <returns></returns>
    private static PreTypeBean GetPreDetailsForOrderNumber(PreTypeBean preTypeData, GameDataBean gameData, IconDataManager iconDataManager, bool isComplete)
    {
        Sprite spIcon = null;
        long dataNumber = long.Parse(preTypeData.data);
        string preProStr = "";
        string preDescribeTitle = "";
        string preFailTitle = "";
        UserAchievementBean userAchievement = gameData.GetAchievementData();
        long numberTotal = 0;
        switch (preTypeData.dataType)
        {
            case PreTypeEnum.OrderNumberForTotal:
                spIcon = iconDataManager.GetIconSpriteByName("team_2");
                preDescribeTitle = GameCommonInfo.GetUITextById(5041);
                preFailTitle = GameCommonInfo.GetUITextById(5042);
                numberTotal = userAchievement.GetNumberForAllCustomerFood();
                break;
            case PreTypeEnum.OrderNumberForHotelTotal:
                spIcon = iconDataManager.GetIconSpriteByName("worker_waiter_bed_pro_2");
                preDescribeTitle = GameCommonInfo.GetUITextById(5043);
                preFailTitle = GameCommonInfo.GetUITextById(5044);
                numberTotal = userAchievement.GetNumberForAllCustomerHotel();
                break;
        }

        if (numberTotal >= dataNumber || isComplete)
        {
            preTypeData.isPre = true;
            preProStr = "(" + dataNumber + "/" + dataNumber + ")";
            preTypeData.progress = 1;
        }
        else
        {
            preTypeData.isPre = false;
            preProStr = "(" + numberTotal + "/" + dataNumber + ")";
            preTypeData.progress = numberTotal / (float)dataNumber;
        }
        preTypeData.spPreIcon = spIcon;
        preTypeData.preDescribe = string.Format(preDescribeTitle, preProStr + "");
        preTypeData.preFailStr = string.Format(preFailTitle, dataNumber + "");
        return preTypeData;
    }

    /// <summary>
    /// 获取支付金钱相关详情
    /// </summary>
    /// <param name="preTypeData"></param>
    /// <param name="gameData"></param>
    /// <param name="iconDataManager"></param>
    /// <param name="isComplete"></param>
    /// <returns></returns>
    private static PreTypeBean GetPreDetailsForGetMoney(PreTypeBean preTypeData, GameDataBean gameData, IconDataManager iconDataManager, bool isComplete)
    {
        string preMoneyStr = "";
        long getMoney = long.Parse(preTypeData.data);
        long haveMoney = 0;
        string iconKey = "";
        UserAchievementBean userAchievement = gameData.GetAchievementData();
        switch (preTypeData.dataType)
        {
            case PreTypeEnum.GetMoneyL:
                haveMoney = userAchievement.ownMoneyL;
                preTypeData.preDescribe = GameCommonInfo.GetUITextById(5007);
                iconKey = "money_3";
                break;
            case PreTypeEnum.GetMoneyM:
                haveMoney = userAchievement.ownMoneyM;
                preTypeData.preDescribe = GameCommonInfo.GetUITextById(5008);
                iconKey = "money_2";
                break;
            case PreTypeEnum.GetMoneyS:
                haveMoney = userAchievement.ownMoneyS;
                preTypeData.preDescribe = GameCommonInfo.GetUITextById(5009);
                iconKey = "money_1";
                break;
        }
        if (haveMoney >= getMoney || isComplete)
        {
            preTypeData.isPre = true;
            preMoneyStr = "(" + getMoney + "/" + getMoney + ")";
            preTypeData.progress = 1;
        }
        else
        {
            preTypeData.isPre = false;
            preMoneyStr = "(" + haveMoney + "/" + getMoney + ")";
            preTypeData.progress = haveMoney / (float)getMoney;
        }
        if (iconDataManager != null)
            preTypeData.spPreIcon = iconDataManager.GetIconSpriteByName(iconKey);
        preTypeData.preDescribe = string.Format(preTypeData.preDescribe, preMoneyStr);
        preTypeData.preFailStr = GameCommonInfo.GetUITextById(1024);
        return preTypeData;
    }

    private static PreTypeBean GetPreDetailsForInnPraiseNumber(PreTypeBean preTypeData, GameDataBean gameData, IconDataManager iconDataManager, bool isComplete)
    {
        Sprite spIcon = null;
        long dataNumber = long.Parse(preTypeData.data);
        string preProStr = "";
        string preDesStr = "";
        PraiseTypeEnum praiseType = PraiseTypeEnum.Happy;
        switch (preTypeData.dataType)
        {
            case PreTypeEnum.InnPraiseNumberForExcited:
                praiseType = PraiseTypeEnum.Excited;
                preDesStr = GameCommonInfo.GetUITextById(5051);
                spIcon = iconDataManager.GetIconSpriteByName("customer_mood_0");
                break;
            case PreTypeEnum.InnPraiseNumberForHappy:
                praiseType = PraiseTypeEnum.Happy;
                preDesStr = GameCommonInfo.GetUITextById(5052);
                spIcon = iconDataManager.GetIconSpriteByName("customer_mood_1");
                break;
            case PreTypeEnum.InnPraiseNumberForOkay:
                praiseType = PraiseTypeEnum.Okay;
                preDesStr = GameCommonInfo.GetUITextById(5053);
                spIcon = iconDataManager.GetIconSpriteByName("customer_mood_2");
                break;
            case PreTypeEnum.InnPraiseNumberForOrdinary:
                praiseType = PraiseTypeEnum.Ordinary;
                preDesStr = GameCommonInfo.GetUITextById(5054);
                spIcon = iconDataManager.GetIconSpriteByName("customer_mood_3");
                break;
            case PreTypeEnum.InnPraiseNumberForDisappointed:
                praiseType = PraiseTypeEnum.Disappointed;
                preDesStr = GameCommonInfo.GetUITextById(5055);
                spIcon = iconDataManager.GetIconSpriteByName("customer_mood_4");
                break;
            case PreTypeEnum.InnPraiseNumberForAnger:
                praiseType = PraiseTypeEnum.Anger;
                preDesStr = GameCommonInfo.GetUITextById(5056);
                spIcon = iconDataManager.GetIconSpriteByName("customer_mood_5");
                break;
        }
        UserAchievementBean userAchievement = gameData.GetAchievementData();
        long praiseNumber = userAchievement.GetPraiseNumber(praiseType);
        if (praiseNumber >= dataNumber || isComplete)
        {
            preTypeData.isPre = true;
            preProStr = "(" + dataNumber + "/" + dataNumber + ")";
            preTypeData.progress = 1;
        }
        else
        {
            preTypeData.isPre = false;
            preProStr = "(" + praiseNumber + "/" + dataNumber + ")";
            preTypeData.progress = praiseNumber / (float)dataNumber;
        }
        preTypeData.spPreIcon = spIcon;
        preTypeData.preDescribe = string.Format(preDesStr, preProStr + "");
        preTypeData.preFailStr = GameCommonInfo.GetUITextById(5057);
        return preTypeData;
    }

    /// <summary>
    /// 获取菜单数量相关详情
    /// </summary>
    /// <param name="preTypeData"></param>
    /// <param name="gameData"></param>
    /// <param name="iconDataManager"></param>
    /// <param name="isComplete"></param>
    /// <returns></returns>
    private static PreTypeBean GetPreDetailsForMenuNumber(PreTypeBean preTypeData, GameDataBean gameData, IconDataManager iconDataManager, bool isComplete)
    {
        Sprite spIcon = null;
        long dataNumber = long.Parse(preTypeData.data);
        string preProStr = "";
        spIcon = iconDataManager.GetIconSpriteByName("ui_features_menu");
        int menuNumber = 0;
        string content = "";
        string contentFail = "";
        string levelStr = "";
        switch (preTypeData.dataType)
        {
            case PreTypeEnum.MenuNumber:
                List<MenuOwnBean> listMenu = gameData.GetMenuList();
                menuNumber = listMenu.Count;
                content = GameCommonInfo.GetUITextById(5061);
                contentFail = GameCommonInfo.GetUITextById(5062);
                break;
            case PreTypeEnum.MenuLevelStarNumber:
                menuNumber = gameData.GetMenuNumberByLevel(LevelTypeEnum.Star);
                content = GameCommonInfo.GetUITextById(5063);
                contentFail = GameCommonInfo.GetUITextById(5064);
                levelStr = LevelTypeEnumTools.GetLevelStr(LevelTypeEnum.Star);
                break;
            case PreTypeEnum.MenuLevelMoonNumber:
                menuNumber = gameData.GetMenuNumberByLevel(LevelTypeEnum.Moon);
                content = GameCommonInfo.GetUITextById(5063);
                contentFail = GameCommonInfo.GetUITextById(5064);
                levelStr = LevelTypeEnumTools.GetLevelStr(LevelTypeEnum.Moon);
                break;
            case PreTypeEnum.MenuLevelSunNumber:
                menuNumber = gameData.GetMenuNumberByLevel(LevelTypeEnum.Sun);
                content = GameCommonInfo.GetUITextById(5063);
                contentFail = GameCommonInfo.GetUITextById(5064);
                levelStr = LevelTypeEnumTools.GetLevelStr(LevelTypeEnum.Sun);
                break;

        }

        if (menuNumber >= dataNumber || isComplete)
        {
            preTypeData.isPre = true;
            preProStr = "(" + dataNumber + "/" + dataNumber + ")";
            preTypeData.progress = 1;
        }
        else
        {
            preTypeData.isPre = false;
            preProStr = "(" + menuNumber + "/" + dataNumber + ")";
            preTypeData.progress = menuNumber / (float)dataNumber;
        }
        preTypeData.spPreIcon = spIcon;
        preTypeData.preDescribe = string.Format(content, preProStr + "", levelStr);
        preTypeData.preFailStr = string.Format(contentFail, dataNumber + "", levelStr);
        return preTypeData;
    }

    /// <summary>
    /// 获取菜单数量相关详情
    /// </summary>
    /// <param name="preTypeData"></param>
    /// <param name="gameData"></param>
    /// <param name="iconDataManager"></param>
    /// <param name="isComplete"></param>
    /// <returns></returns>
    private static PreTypeBean GetPreDetailsForBedNumber(PreTypeBean preTypeData, GameDataBean gameData, IconDataManager iconDataManager, bool isComplete)
    {
        Sprite spIcon = null;
        long dataNumber = long.Parse(preTypeData.data);
        string preProStr = "";
        spIcon = iconDataManager.GetIconSpriteByName("worker_waiter_bed_pro_2");
        int bedNumber = 0;
        string content = "";
        string contentFail = "";
        string levelStr = "";
        switch (preTypeData.dataType)
        {
            case PreTypeEnum.BedNumber:
                List<BuildBedBean> listBed = gameData.listBed;
                bedNumber = listBed.Count;
                content = GameCommonInfo.GetUITextById(5065);
                contentFail = GameCommonInfo.GetUITextById(5066);
                break;
            case PreTypeEnum.BedLevelStarNumber:
                bedNumber = gameData.GetBedNumberByLevel(LevelTypeEnum.Star);
                content = GameCommonInfo.GetUITextById(5067);
                contentFail = GameCommonInfo.GetUITextById(5068);
                levelStr = LevelTypeEnumTools.GetLevelStr(LevelTypeEnum.Star);
                break;
            case PreTypeEnum.BedLevelMoonNumber:
                bedNumber = gameData.GetBedNumberByLevel(LevelTypeEnum.Moon);
                content = GameCommonInfo.GetUITextById(5067);
                contentFail = GameCommonInfo.GetUITextById(5068);
                levelStr = LevelTypeEnumTools.GetLevelStr(LevelTypeEnum.Moon);
                break;
            case PreTypeEnum.BedLevelSunNumber:
                bedNumber = gameData.GetBedNumberByLevel(LevelTypeEnum.Sun);
                content = GameCommonInfo.GetUITextById(5067);
                contentFail = GameCommonInfo.GetUITextById(5068);
                levelStr = LevelTypeEnumTools.GetLevelStr(LevelTypeEnum.Sun);
                break;

        }

        if (bedNumber >= dataNumber || isComplete)
        {
            preTypeData.isPre = true;
            preProStr = "(" + dataNumber + "/" + dataNumber + ")";
            preTypeData.progress = 1;
        }
        else
        {
            preTypeData.isPre = false;
            preProStr = "(" + bedNumber + "/" + dataNumber + ")";
            preTypeData.progress = bedNumber / (float)dataNumber;
        }
        preTypeData.spPreIcon = spIcon;
        preTypeData.preDescribe = string.Format(content, preProStr + "", levelStr);
        preTypeData.preFailStr = string.Format(contentFail, dataNumber + "", levelStr);
        return preTypeData;
    }


    private static PreTypeBean GetPreDetailsForSellMenuNumber(PreTypeBean preTypeData, GameDataBean gameData, IconDataManager iconDataManager, bool isComplete)
    {
        Sprite spIcon = null;
        long dataNumber = long.Parse(preTypeData.data);
        string preProStr = "";
        switch (preTypeData.dataType)
        {
            case PreTypeEnum.SellMenuNumber:
                spIcon = iconDataManager.GetIconSpriteByName("ui_features_menu");
                break;
        }
        List<MenuOwnBean> listMenu = gameData.GetMenuList();
        long sellNumber = 0;
        foreach (MenuOwnBean itemMenu in listMenu)
        {
            sellNumber += itemMenu.sellNumber;
        }
        if (sellNumber >= dataNumber || isComplete)
        {
            preTypeData.isPre = true;
            preProStr = "(" + dataNumber + "/" + dataNumber + ")";
            preTypeData.progress = 1;
        }
        else
        {
            preTypeData.isPre = false;
            preProStr = "(" + sellNumber + "/" + dataNumber + ")";
            preTypeData.progress = sellNumber / (float)dataNumber;
        }
        preTypeData.spPreIcon = spIcon;
        preTypeData.preDescribe = string.Format(GameCommonInfo.GetUITextById(5071), preProStr + "");
        preTypeData.preFailStr = GameCommonInfo.GetUITextById(5072);
        return preTypeData;
    }

    private static PreTypeBean GetPreDetailsForHaveMenu(PreTypeBean preTypeData, GameDataBean gameData, InnFoodManager innFoodManager)
    {
        Sprite spIcon = null;
        long[] menuIds = StringUtil.SplitBySubstringForArrayLong(preTypeData.data, ',');

        bool hasAllMenu = true;
        List<MenuOwnBean> listMenu = gameData.GetMenuList();
        string foodListStr = "";
        string noFoodStr = "";
        foreach (long menuId in menuIds)
        {
            MenuInfoBean menuInfo = innFoodManager.GetFoodDataById(menuId);
            spIcon = innFoodManager.GetFoodSpriteByName(menuInfo.icon_key);
            foodListStr += ("《" + menuInfo.name + "》");
            bool hasMenu = gameData.CheckHasMenu(menuId);
            if (!hasMenu)
            {
                noFoodStr = "《" + menuInfo.name + "》";
                hasAllMenu = false;
            }
        }
        preTypeData.isPre = hasAllMenu;
        preTypeData.spPreIcon = spIcon;

        preTypeData.preDescribe = string.Format(GameCommonInfo.GetUITextById(5073), foodListStr + "");
        preTypeData.preFailStr = string.Format(GameCommonInfo.GetUITextById(5074), noFoodStr + "");
        return preTypeData;

    }
    private static PreTypeBean GetPreDetailsForWorker(PreTypeBean preTypeData, GameDataBean gameData, IconDataManager iconDataManager, bool isComplete)
    {
        Sprite spIcon = null;
        long dataNumber = long.Parse(preTypeData.data);
        long workerNumber = 0;
        string preDesStr = "";
        string preProStr = "";
        List<CharacterBean> listWorker = gameData.GetAllCharacterData();
        foreach (CharacterBean itemWorkerData in listWorker)
        {
            switch (preTypeData.dataType)
            {
                case PreTypeEnum.WorkerForCookFoodNumber:
                    preDesStr = GameCommonInfo.GetUITextById(311);
                    CharacterWorkerForChefBean characterWorkerForChef = (CharacterWorkerForChefBean)itemWorkerData.baseInfo.GetWorkerInfoByType(WorkerEnum.Chef);
                    workerNumber += characterWorkerForChef.cookNumber;
                    break;
                case PreTypeEnum.WorkerForCleanFoodNumber:
                case PreTypeEnum.WorkerForSendFoodNumber:
                case PreTypeEnum.WorkerForCleanBedNumber:
                    CharacterWorkerForWaiterBean characterWorkerForWaiter = (CharacterWorkerForWaiterBean)itemWorkerData.baseInfo.GetWorkerInfoByType(WorkerEnum.Waiter);
                    if (preTypeData.dataType == PreTypeEnum.WorkerForCleanFoodNumber)
                    {
                        preDesStr = GameCommonInfo.GetUITextById(314);
                        workerNumber += characterWorkerForWaiter.cleanTotalNumber;
                    }
                    else if (preTypeData.dataType == PreTypeEnum.WorkerForSendFoodNumber)
                    {
                        preDesStr = GameCommonInfo.GetUITextById(313);
                        workerNumber += characterWorkerForWaiter.sendTotalNumber;
                    }
                    else if (preTypeData.dataType == PreTypeEnum.WorkerForCleanBedNumber)
                    {
                        preDesStr = GameCommonInfo.GetUITextById(348);
                        workerNumber += characterWorkerForWaiter.cleanBedTotalNumber;
                    }
                    break;
                case PreTypeEnum.WorkerForAccountantSuccessNumber:
                case PreTypeEnum.WorkerForAccountantFailNumber:
                    CharacterWorkerForAccountantBean characterWorkerForAccountant = (CharacterWorkerForAccountantBean)itemWorkerData.baseInfo.GetWorkerInfoByType(WorkerEnum.Accountant);
                    if (preTypeData.dataType == PreTypeEnum.WorkerForAccountantSuccessNumber)
                    {
                        preDesStr = GameCommonInfo.GetUITextById(318);
                        workerNumber += characterWorkerForAccountant.accountingSuccessNumber;
                    }
                    else if (preTypeData.dataType == PreTypeEnum.WorkerForAccountantFailNumber)
                    {
                        preDesStr = GameCommonInfo.GetUITextById(320);
                        workerNumber += characterWorkerForAccountant.accountingErrorNumber;
                    }
          

                    break;
                case PreTypeEnum.WorkerForAccostSuccessNumber:
                case PreTypeEnum.WorkerForAccostFailNumber:
                case PreTypeEnum.WorkerForAccostGuideNumber:
                    CharacterWorkerForAccostBean characterWorkerForAccost = (CharacterWorkerForAccostBean)itemWorkerData.baseInfo.GetWorkerInfoByType(WorkerEnum.Accost);
                    if (preTypeData.dataType == PreTypeEnum.WorkerForAccostSuccessNumber)
                    {
                        preDesStr = GameCommonInfo.GetUITextById(324);
                        workerNumber += characterWorkerForAccost.accostSuccessNumber;
                    }
                    else if (preTypeData.dataType == PreTypeEnum.WorkerForAccostFailNumber)
                    {
                        preDesStr = GameCommonInfo.GetUITextById(325);
                        workerNumber += characterWorkerForAccost.accostFailNumber;
                    }
                    else if (preTypeData.dataType == PreTypeEnum.WorkerForAccostGuideNumber)
                    {
                        preDesStr = GameCommonInfo.GetUITextById(347);
                        workerNumber += characterWorkerForAccost.guideNumber;
                    }
                    break;
                case PreTypeEnum.WorkerForFightSuccessNumber:
                case PreTypeEnum.WorkerForFightFailNumber:
                    CharacterWorkerForBeaterBean characterWorkerForBeater = (CharacterWorkerForBeaterBean)itemWorkerData.baseInfo.GetWorkerInfoByType(WorkerEnum.Beater);
                    if (preTypeData.dataType == PreTypeEnum.WorkerForFightSuccessNumber)
                    {
                        preDesStr = GameCommonInfo.GetUITextById(329);
                        workerNumber += characterWorkerForBeater.fightWinNumber;
                    }
                    else if (preTypeData.dataType == PreTypeEnum.WorkerForFightFailNumber)
                    {
                        preDesStr = GameCommonInfo.GetUITextById(330);
                        workerNumber += characterWorkerForBeater.fightLoseNumber;
                    }
                    break;
            }
        }
        if (workerNumber >= dataNumber || isComplete)
        {
            preTypeData.isPre = true;
            preProStr = "(" + dataNumber + "/" + dataNumber + ")";
            preTypeData.progress = 1;
        }
        else
        {
            preTypeData.isPre = false;
            preProStr = "(" + workerNumber + "/" + dataNumber + ")";
            preTypeData.progress = workerNumber / (float)dataNumber;
        }
        preTypeData.spPreIcon = spIcon;
        preTypeData.preDescribe = preDesStr + preProStr;
        preTypeData.preFailStr = GameCommonInfo.GetUITextById(5081);
        return preTypeData;
    }

    /// <summary>
    /// 获取客栈好评相关详情
    /// </summary>
    /// <param name="preTypeData"></param>
    /// <param name="gameData"></param>
    /// <param name="iconDataManager"></param>
    /// <param name="isComplete"></param>
    /// <returns></returns>
    private static PreTypeBean GetPreDetailsForInnPraise(PreTypeBean preTypeData, GameDataBean gameData, IconDataManager iconDataManager, bool isComplete)
    {
        Sprite spIcon = null;
        float dataNumber = float.Parse(preTypeData.data);
        string preProStr = "";
        string preDesStr = "";
        switch (preTypeData.dataType)
        {
            case PreTypeEnum.InnPraise:
                preDesStr = GameCommonInfo.GetUITextById(5091);
                spIcon = iconDataManager.GetIconSpriteByName("ui_praise");
                break;
        }
        InnAttributesBean innAttributes = gameData.GetInnAttributesData();
        innAttributes.GetPraise(out int maxPraise, out int praise);
        float praiseRate = (float)praise / maxPraise;
        if (praiseRate >= dataNumber || isComplete)
        {
            preTypeData.isPre = true;
            preProStr = "(" + dataNumber * 100 + "%/" + dataNumber * 100 + "%)";
            preTypeData.progress = 1;
        }
        else
        {
            preTypeData.isPre = false;
            preProStr = "(" + praiseRate * 100 + "%/" + dataNumber * 100 + "%)";
            preTypeData.progress = praiseRate;
        }
        preTypeData.spPreIcon = spIcon;
        preTypeData.preDescribe = preDesStr + preProStr;
        preTypeData.preFailStr = GameCommonInfo.GetUITextById(5092);
        return preTypeData;
    }

    /// <summary>
    /// 获取客栈美观值相关详情
    /// </summary>
    /// <param name="preTypeData"></param>
    /// <param name="gameData"></param>
    /// <param name="iconDataManager"></param>
    /// <param name="isComplete"></param>
    /// <returns></returns>
    private static PreTypeBean GetPreDetailsForInnAesthetics(PreTypeBean preTypeData, GameDataBean gameData, IconDataManager iconDataManager, bool isComplete)
    {
        Sprite spIcon = null;
        float dataNumber = float.Parse(preTypeData.data);
        string preProStr = "";
        string preDesStr = "";
        switch (preTypeData.dataType)
        {
            case PreTypeEnum.InnAesthetics:
                preDesStr = GameCommonInfo.GetUITextById(5101);
                spIcon = iconDataManager.GetIconSpriteByName("ui_aesthetics");
                break;
        }
        InnAttributesBean innAttributes = gameData.GetInnAttributesData();
        innAttributes.GetAesthetics(out float maxAesthetics, out float aesthetics);
        if (aesthetics >= dataNumber || isComplete)
        {
            preTypeData.isPre = true;
            preProStr = "(" + dataNumber + "/" + dataNumber + ")";
            preTypeData.progress = 1;
        }
        else
        {
            preTypeData.isPre = false;
            preProStr = "(" + aesthetics + "/" + dataNumber + ")";
            preTypeData.progress = aesthetics / dataNumber;
        }
        preTypeData.spPreIcon = spIcon;
        preTypeData.preDescribe = preDesStr + preProStr;
        preTypeData.preFailStr = GameCommonInfo.GetUITextById(5102);
        return preTypeData;
    }

    /// <summary>
    /// 获取出现详情  NPC好感
    /// </summary>
    /// <param name="gameData"></param>
    /// <param name="conditionData"></param>
    /// <returns></returns>
    protected static PreTypeBean GetPreDetailsForNpcFavorabilityLevel(GameDataBean gameData, PreTypeBean preData, NpcInfoManager npcInfoManager)
    {
        long[] listData = StringUtil.SplitBySubstringForArrayLong(preData.data, ',');
        long npcId = listData[0];
        long npcFavorabilityLevel = listData[1];
        CharacterFavorabilityBean characterFavorability = gameData.GetCharacterFavorability(npcId);
        if (characterFavorability.GetFavorabilityLevel() >= npcFavorabilityLevel)
        {
            preData.isPre = true;
        }
        else
        {
            preData.isPre = false;
        }
        CharacterBean characterData = npcInfoManager.GetCharacterDataById(npcId);
        preData.preDescribe = string.Format(GameCommonInfo.GetUITextById(5221), characterData.baseInfo.name, npcFavorabilityLevel + "");
        preData.preFailStr = string.Format(GameCommonInfo.GetUITextById(5222), characterData.baseInfo.name, npcFavorabilityLevel + "");
        return preData;
    }
    /// <summary>
    /// 获取出现详情  NPC好感
    /// </summary>
    /// <param name="gameData"></param>
    /// <param name="conditionData"></param>
    /// <returns></returns>
    protected static PreTypeBean GetPreDetailsForInfiniteTowersMaxLayer(PreTypeBean preTypeData, GameDataBean gameData, IconDataManager iconDataManager, bool isComplete)
    {
        Sprite spIcon = iconDataManager.GetIconSpriteByName("ui_praise");
        int maxLayer = int.Parse(preTypeData.data);
        string preProStr = string.Format(GameCommonInfo.GetUITextById(5111), maxLayer+"");
        string preDesStr = "";
        UserAchievementBean userAchievement = gameData.GetAchievementData();
        int currrentLayer = userAchievement.maxInfiniteTowersLayer;
        if (currrentLayer >= maxLayer || isComplete)
        {
            preTypeData.isPre = true;
            preProStr += "(" + maxLayer + "/" + maxLayer + ")";
            preTypeData.progress = 1;
        }
        else
        {
            preTypeData.isPre = false;
            preProStr += "(" + currrentLayer + "/" + maxLayer + ")";
            preTypeData.progress = (float)currrentLayer / maxLayer;
        }
        preTypeData.spPreIcon = spIcon;
        preTypeData.preDescribe = preDesStr + preProStr;
        preTypeData.preFailStr = GameCommonInfo.GetUITextById(5112);
        return preTypeData;
    }

    /// <summary>
    /// 完成前置条件
    /// </summary>
    /// <param name="data"></param>
    /// <param name="gameData"></param>
    public static void CompletePre(string data, GameDataBean gameData)
    {
        List<PreTypeBean> listPreData = GetListPreData(data);
        CompletePre(listPreData, gameData);
    }

    /// <summary>
    ///  完成前置条件
    /// </summary>
    /// <param name="listPreData"></param>
    /// <param name="gameData"></param>
    public static void CompletePre(List<PreTypeBean> listPreData, GameDataBean gameData)
    {
        foreach (var itemData in listPreData)
        {
            PreTypeEnum preType = itemData.dataType;
            switch (preType)
            {
                case PreTypeEnum.PayMoneyL:
                    long moneyL = long.Parse(itemData.data);
                    gameData.PayMoney(moneyL, 0, 0);
                    break;
                case PreTypeEnum.PayMoneyM:
                    long moneyM = long.Parse(itemData.data);
                    gameData.PayMoney(0, moneyM, 0);
                    break;
                case PreTypeEnum.PayMoneyS:
                    long moneyS = long.Parse(itemData.data);
                    gameData.PayMoney(0, 0, moneyS);
                    break;
                case PreTypeEnum.PayGuildCoin:
                    long guildCoin = long.Parse(itemData.data);
                    gameData.PayGuildCoin(guildCoin);
                    break;
                case PreTypeEnum.PayTrophyElementary:
                    long trophyElementary = long.Parse(itemData.data);
                    gameData.PayTrophy(trophyElementary, 0, 0, 0);
                    break;
                case PreTypeEnum.PayTrophyIntermediate:
                    long trophyIntermediate = long.Parse(itemData.data);
                    gameData.PayTrophy(0, trophyIntermediate, 0, 0);
                    break;
                case PreTypeEnum.PayTrophyAdvanced:
                    long trophyAdvanced = long.Parse(itemData.data);
                    gameData.PayTrophy(0, 0, trophyAdvanced, 0);
                    break;
                case PreTypeEnum.PayTrophyLegendary:
                    long trophyLegendary = long.Parse(itemData.data);
                    gameData.PayTrophy(0, 0, 0, trophyLegendary);
                    break;

                case PreTypeEnum.PayItems:
                    long[] listItems = StringUtil.SplitBySubstringForArrayLong(itemData.data, ',');
                    long itemsId = listItems[0];
                    long itemsNumber = 1;
                    if (listItems.Length >= 2)
                    {
                        itemsNumber = listItems[1];
                    }
                    gameData.AddItemsNumber(itemsId, -itemsNumber);
                    break;
                case PreTypeEnum.PayIngForOilsalt:
                    gameData.AddIng(IngredientsEnum.Oilsalt, -int.Parse(itemData.data));
                    break;
                case PreTypeEnum.PayIngForMeat:
                    gameData.AddIng(IngredientsEnum.Meat, -int.Parse(itemData.data));
                    break;
                case PreTypeEnum.PayIngForRiverfresh:
                    gameData.AddIng(IngredientsEnum.Riverfresh, -int.Parse(itemData.data));
                    break;
                case PreTypeEnum.PayIngForSeafood:
                    gameData.AddIng(IngredientsEnum.Seafood, -int.Parse(itemData.data));
                    break;
                case PreTypeEnum.PayIngForVegetables:
                    gameData.AddIng(IngredientsEnum.Vegetables, -int.Parse(itemData.data));
                    break;
                case PreTypeEnum.PayIngForMelonfruit:
                    gameData.AddIng(IngredientsEnum.Melonfruit, -int.Parse(itemData.data));
                    break;
                case PreTypeEnum.PayIngForWaterwine:
                    gameData.AddIng(IngredientsEnum.Waterwine, -int.Parse(itemData.data));
                    break;
                case PreTypeEnum.PayIngForFlour:
                    gameData.AddIng(IngredientsEnum.Flour, -int.Parse(itemData.data));
                    break;
            }
        }
    }
}