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

    OrderNumberForTotal,//总共接客数量

    GetMoneyL,//赚取金钱
    GetMoneyM,
    GetMoneyS,

    InnPraiseNumberForExcited,//评价数量
    InnPraiseNumberForHappy,
    InnPraiseNumberForOkay,
    InnPraiseNumberForOrdinary,
    InnPraiseNumberForDisappointed,
    InnPraiseNumberForAnger,

    MenuNumber,//拥有菜品数量
    SellMenuNumber,//卖出菜品数量

    WorkerForCookFoodNumber,//工作相关统计
    WorkerForCleanFoodNumber,
    WorkerForSendFoodNumber,
    WorkerForAccountantSuccessNumber,
    WorkerForAccountantFailNumber,
    WorkerForAccostSuccessNumber,
    WorkerForAccostFailNumber,
    WorkerForFightSuccessNumber,
    WorkerForFightFailNumber,
}

public class PreTypeBean : DataBean<PreTypeEnum>
{
    public bool isPre;
    public float progress;
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
    public static bool CheckIsAllPre(GameItemsManager gameItemsManager, IconDataManager iconDataManager, CharacterDressManager characterDressManager, GameDataBean gameData, CharacterBean characterData, string data, out string reason)
    {
        List<PreTypeBean> listPreData = GetListPreData(data);
        reason = "";
        foreach (var itemPreData in listPreData)
        {
            GetPreDetails(itemPreData, gameData, characterData, iconDataManager, gameItemsManager, characterDressManager, false);
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
    public static PreTypeBean GetPreDetails(PreTypeBean preTypeData, GameDataBean gameData, CharacterBean characterData, IconDataManager iconDataManager, GameItemsManager gameItemsManager, CharacterDressManager characterDressManager, bool isComplete)
    {
        switch (preTypeData.dataType)
        {
            case PreTypeEnum.PayMoneyL:
            case PreTypeEnum.PayMoneyM:
            case PreTypeEnum.PayMoneyS:
                GetPreDetailsForPayMoney(preTypeData, gameData, iconDataManager, isComplete);
                break;
            case PreTypeEnum.HaveMoneyL:
            case PreTypeEnum.HaveMoneyM:
            case PreTypeEnum.HaveMoneyS:
                GetPreDetailsForHaveMoney(preTypeData, gameData, iconDataManager, isComplete);
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
                GetPreDetailsForItems(gameItemsManager, iconDataManager, characterDressManager, preTypeData, gameData, isComplete);
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
                GetPreDetailsForMenuNumber(preTypeData, gameData, iconDataManager, isComplete);
                break;
            case PreTypeEnum.SellMenuNumber:
                GetPreDetailsForSellMenuNumber(preTypeData, gameData, iconDataManager, isComplete);
                break;
            case PreTypeEnum.WorkerForCookFoodNumber:
            case PreTypeEnum.WorkerForCleanFoodNumber:
            case PreTypeEnum.WorkerForSendFoodNumber:
            case PreTypeEnum.WorkerForAccountantSuccessNumber:
            case PreTypeEnum.WorkerForAccountantFailNumber:
            case PreTypeEnum.WorkerForAccostSuccessNumber:
            case PreTypeEnum.WorkerForAccostFailNumber:
            case PreTypeEnum.WorkerForFightSuccessNumber:
            case PreTypeEnum.WorkerForFightFailNumber:
                GetPreDetailsForWorker(preTypeData, gameData, iconDataManager, isComplete);
                        break;
        }
        return preTypeData;
    }
    public static PreTypeBean GetPreDetails(PreTypeBean preTypeData, GameDataBean gameData, IconDataManager iconDataManager, GameItemsManager gameItemsManager, CharacterDressManager characterDressManager, bool isComplete)
    {
        return GetPreDetails(preTypeData, gameData, null, iconDataManager, gameItemsManager, characterDressManager, isComplete);
    }
    public static PreTypeBean GetPreDetails(PreTypeBean preTypeData, GameDataBean gameData, IconDataManager iconDataManager, GameItemsManager gameItemsManager, CharacterDressManager characterDressManager)
    {
        return GetPreDetails(preTypeData, gameData, null, iconDataManager, gameItemsManager, characterDressManager, false);
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
        GameItemsManager gameItemsManager,
        IconDataManager iconDataManager,
        CharacterDressManager characterDressManager,
        PreTypeBean preTypeData, GameDataBean gameData, bool isComplete)
    {
        long[] listItems = StringUtil.SplitBySubstringForArrayLong(preTypeData.data, ',');
        preTypeData.isPre = true;
        long itemsId = listItems[0];
        long itemsNumber = listItems[1];
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
        ItemsInfoBean itemsInfo = gameItemsManager.GetItemsById(itemsId);
        preTypeData.spPreIcon = GeneralEnumTools.GetGeneralSprite(itemsInfo, iconDataManager, gameItemsManager, characterDressManager);
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
    /// 获取支付金钱相关详情
    /// </summary>
    /// <param name="preTypeData"></param>
    /// <param name="gameData"></param>
    /// <param name="iconDataManager"></param>
    /// <param name="isComplete"></param>
    /// <returns></returns>
    private static PreTypeBean GetPreDetailsForPayMoney(PreTypeBean preTypeData, GameDataBean gameData, IconDataManager iconDataManager, bool isComplete)
    {
        string preMoneyStr = "";
        long payMoney = long.Parse(preTypeData.data);
        long haveMoney = 0;
        string iconKey = "";
        switch (preTypeData.dataType)
        {
            case PreTypeEnum.PayMoneyL:
                haveMoney = gameData.moneyL;
                preTypeData.preDescribe = GameCommonInfo.GetUITextById(5001);
                iconKey = "money_3";
                break;
            case PreTypeEnum.PayMoneyM:
                haveMoney = gameData.moneyM;
                preTypeData.preDescribe = GameCommonInfo.GetUITextById(5002);
                iconKey = "money_2";
                break;
            case PreTypeEnum.PayMoneyS:
                haveMoney = gameData.moneyS;
                preTypeData.preDescribe = GameCommonInfo.GetUITextById(5003);
                iconKey = "money_1";
                break;
        }
        preTypeData.preFailStr = GameCommonInfo.GetUITextById(1005);
        if (haveMoney >= payMoney || isComplete)
        {
            preTypeData.isPre = true;
            preMoneyStr = "(" + payMoney + "/" + payMoney + ")";
            preTypeData.progress = 1;
        }
        else
        {
            preTypeData.isPre = false;
            preMoneyStr = "(" + haveMoney + "/" + payMoney + ")";
            preTypeData.progress = haveMoney / (float)payMoney;
        }
        if (iconDataManager != null)
            preTypeData.spPreIcon = iconDataManager.GetIconSpriteByName(iconKey);
        preTypeData.preDescribe = string.Format(preTypeData.preDescribe, preMoneyStr);
        return preTypeData;
    }


    /// <summary>
    /// 获取拥有金钱相关详情
    /// </summary>
    /// <param name="preTypeData"></param>
    /// <param name="gameData"></param>
    /// <param name="iconDataManager"></param>
    /// <param name="isComplete"></param>
    /// <returns></returns>
    private static PreTypeBean GetPreDetailsForHaveMoney(PreTypeBean preTypeData, GameDataBean gameData, IconDataManager iconDataManager, bool isComplete)
    {
        long haveMoney = long.Parse(preTypeData.data);
        long ownMoney = 0;
        string haveMoneyStr = "";
        string iconKey = "";
        switch (preTypeData.dataType)
        {
            case PreTypeEnum.HaveMoneyL:
                ownMoney = gameData.moneyL;
                preTypeData.preDescribe = GameCommonInfo.GetUITextById(5004);
                iconKey = "money_3";
                break;
            case PreTypeEnum.HaveMoneyM:
                ownMoney = gameData.moneyM;
                preTypeData.preDescribe = GameCommonInfo.GetUITextById(5005);
                iconKey = "money_2";
                break;
            case PreTypeEnum.HaveMoneyS:
                ownMoney = gameData.moneyS;
                preTypeData.preDescribe = GameCommonInfo.GetUITextById(5006);
                iconKey = "money_1";
                break;
        }
        preTypeData.preFailStr = GameCommonInfo.GetUITextById(1005);
        if (ownMoney >= haveMoney || isComplete)
        {
            preTypeData.isPre = true;
            haveMoneyStr = "(" + haveMoney + "/" + haveMoney + ")";
            preTypeData.progress = 1;
        }
        else
        {
            preTypeData.isPre = false;
            haveMoneyStr = "(" + ownMoney + "/" + haveMoney + ")";
            preTypeData.progress = ownMoney / (float)haveMoney;
        }
        if (iconDataManager != null)
            preTypeData.spPreIcon = iconDataManager.GetIconSpriteByName(iconKey);
        preTypeData.preDescribe = string.Format(preTypeData.preDescribe, haveMoneyStr);
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
                targetAttributes = characterData.attributes.lucky;
                iconKey = "ui_ability_cook";
                preTypeData.preFailStr = GameCommonInfo.GetUITextById(5013);
                break;
            case PreTypeEnum.AttributeForAccount:
                targetAttributes = characterData.attributes.lucky;
                iconKey = "ui_ability_account";
                preTypeData.preFailStr = GameCommonInfo.GetUITextById(5014);
                break;
            case PreTypeEnum.AttributeForCharm:
                targetAttributes = characterData.attributes.lucky;
                iconKey = "ui_ability_charm";
                preTypeData.preFailStr = GameCommonInfo.GetUITextById(5015);
                break;
            case PreTypeEnum.AttributeForLife:
                targetAttributes = characterData.attributes.lucky;
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
                ingredients = IngredientsEnum.Oilsalt;

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
        switch (preTypeData.dataType)
        {
            case PreTypeEnum.OrderNumberForTotal:
                spIcon = iconDataManager.GetIconSpriteByName("team_2");
                break;
        }
        UserAchievementBean userAchievement = gameData.GetAchievementData();
        long numberTotal = userAchievement.GetNumberForAllCustomer();
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
        preTypeData.preDescribe = string.Format(GameCommonInfo.GetUITextById(5041), preProStr + "");
        preTypeData.preFailStr = string.Format(GameCommonInfo.GetUITextById(5042), dataNumber + "");
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
        switch (preTypeData.dataType)
        {
            case PreTypeEnum.GetMoneyL:
                haveMoney = gameData.moneyL;
                preTypeData.preDescribe = GameCommonInfo.GetUITextById(5007);
                iconKey = "money_3";
                break;
            case PreTypeEnum.GetMoneyM:
                haveMoney = gameData.moneyM;
                preTypeData.preDescribe = GameCommonInfo.GetUITextById(5008);
                iconKey = "money_2";
                break;
            case PreTypeEnum.GetMoneyS:
                haveMoney = gameData.moneyS;
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

    private static PreTypeBean GetPreDetailsForMenuNumber(PreTypeBean preTypeData, GameDataBean gameData, IconDataManager iconDataManager, bool isComplete)
    {
        Sprite spIcon = null;
        long dataNumber = long.Parse(preTypeData.data);
        string preProStr = "";
        switch (preTypeData.dataType)
        {
            case PreTypeEnum.MenuNumber:
                spIcon = iconDataManager.GetIconSpriteByName("ui_features_menu");
                break;
        }
        List<MenuOwnBean> listMenu = gameData.GetMenuList();
        if (listMenu.Count >= dataNumber || isComplete)
        {
            preTypeData.isPre = true;
            preProStr = "(" + dataNumber + "/" + dataNumber + ")";
            preTypeData.progress = 1;
        }
        else
        {
            preTypeData.isPre = false;
            preProStr = "(" + listMenu.Count + "/" + dataNumber + ")";
            preTypeData.progress = listMenu.Count / (float)dataNumber;
        }
        preTypeData.spPreIcon = spIcon;
        preTypeData.preDescribe = string.Format(GameCommonInfo.GetUITextById(5061), preProStr + "");
        preTypeData.preFailStr = string.Format(GameCommonInfo.GetUITextById(5062), dataNumber + "");
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
                    CharacterWorkerForChefBean characterWorkerForChef =  (CharacterWorkerForChefBean)itemWorkerData.baseInfo.GetWorkerInfoByType(WorkerEnum.Chef);
                    workerNumber += characterWorkerForChef.cookNumber;
                break;
                case PreTypeEnum.WorkerForCleanFoodNumber:
                case PreTypeEnum.WorkerForSendFoodNumber:
                    CharacterWorkerForWaiterBean characterWorkerForWaiter = (CharacterWorkerForWaiterBean)itemWorkerData.baseInfo.GetWorkerInfoByType(WorkerEnum.Waiter);
                    if (preTypeData.dataType== PreTypeEnum.WorkerForCleanFoodNumber)
                    {
                        preDesStr = GameCommonInfo.GetUITextById(314);
                        workerNumber += characterWorkerForWaiter.cleanTotalNumber;
                    }
                    else if (preTypeData.dataType == PreTypeEnum.WorkerForSendFoodNumber)
                    {
                        preDesStr = GameCommonInfo.GetUITextById(313);
                        workerNumber += characterWorkerForWaiter.sendTotalNumber;
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
                case PreTypeEnum.PayItems:
                    long[] listItems = StringUtil.SplitBySubstringForArrayLong(itemData.data, ',');
                    long itemsId = listItems[0];
                    long itemsNumber = listItems[1];
                    gameData.AddItemsNumber(itemsId, itemsNumber);
                    break;
                case PreTypeEnum.PayIngForOilsalt:
                    gameData.AddIng(IngredientsEnum.Oilsalt, int.Parse(itemData.data));
                    break;
                case PreTypeEnum.PayIngForMeat:
                    gameData.AddIng(IngredientsEnum.Meat, int.Parse(itemData.data));
                    break;
                case PreTypeEnum.PayIngForRiverfresh:
                    gameData.AddIng(IngredientsEnum.Riverfresh, int.Parse(itemData.data));
                    break;
                case PreTypeEnum.PayIngForSeafood:
                    gameData.AddIng(IngredientsEnum.Seafood, int.Parse(itemData.data));
                    break;
                case PreTypeEnum.PayIngForVegetables:
                    gameData.AddIng(IngredientsEnum.Vegetables, int.Parse(itemData.data));
                    break;
                case PreTypeEnum.PayIngForMelonfruit:
                    gameData.AddIng(IngredientsEnum.Melonfruit, int.Parse(itemData.data));
                    break;
                case PreTypeEnum.PayIngForWaterwine:
                    gameData.AddIng(IngredientsEnum.Waterwine, int.Parse(itemData.data));
                    break;
                case PreTypeEnum.PayIngForFlour:
                    gameData.AddIng(IngredientsEnum.Flour, int.Parse(itemData.data));
                    break;
            }
        }
    }
}