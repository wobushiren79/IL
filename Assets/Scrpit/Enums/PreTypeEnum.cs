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
    public static bool CheckIsAllPre(GameItemsManager gameItemsManager,IconDataManager iconDataManager,CharacterDressManager characterDressManager, GameDataBean gameData, CharacterBean characterData, string data, out string reason)
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
            }
        }
    }
}