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
}

public class PreTypeBean
{
    public PreTypeEnum preType;
    public string preData;

    public bool isPre;
    public float progress;
    public Sprite spPreIcon;
    public string preDescribe;

    public PreTypeBean(PreTypeEnum preType, string preData)
    {
        this.preType = preType;
        this.preData = preData;
    }
}

public class PreTypeEnumTools
{
    /// <summary>
    /// 检测是否全部准备就绪
    /// </summary>
    /// <param name="gameData"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static bool CheckIsAllPre(GameDataBean gameData, string data)
    {
        List<PreTypeBean> listPreData = GetListPreData(data);
        foreach (var itemPreData in listPreData)
        {
            GetPreDetails(itemPreData, gameData, null);
            if (!itemPreData.isPre)
            {
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
        List<PreTypeBean> listPreData = new List<PreTypeBean>();
        List<string> listData = StringUtil.SplitBySubstringForListStr(data, '|');
        foreach (string itemData in listData)
        {
            if (CheckUtil.StringIsNull(itemData))
                continue;
            List<string> itemListData = StringUtil.SplitBySubstringForListStr(itemData, ':');
            PreTypeEnum preType = EnumUtil.GetEnum<PreTypeEnum>(itemListData[0]);
            string preValue = itemListData[1];
            listPreData.Add(new PreTypeBean(preType, preValue));
        }
        return listPreData;
    }

    /// <summary>
    /// 获取前置描述
    /// </summary>
    /// <param name="rewardType"></param>
    /// <returns></returns>
    public static PreTypeBean GetPreDetails(PreTypeBean preTypeData, GameDataBean gameData, IconDataManager iconDataManager, bool isComplete)
    {
        switch (preTypeData.preType)
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
        }
        return preTypeData;
    }
    public static PreTypeBean GetPreDetails(PreTypeBean preTypeData, GameDataBean gameData, IconDataManager iconDataManager)
    {
        return GetPreDetails(preTypeData, gameData, iconDataManager, false);
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
        long payMoney = long.Parse(preTypeData.preData);
        long haveMoney = 0;
        string iconKey = "";
        switch (preTypeData.preType)
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
        if (gameData.moneyL >= payMoney || isComplete)
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
        long haveMoney = long.Parse(preTypeData.preData);
        long ownMoney = 0;
        string haveMoneyStr = "";
        string iconKey = "";
        switch (preTypeData.preType)
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
    /// 完成前置条件
    /// </summary>
    /// <param name="data"></param>
    /// <param name="gameData"></param>
    public static void CompletePre(string data, GameDataBean gameData)
    {
        List<PreTypeBean> listPreData = GetListPreData(data);
        foreach (var itemData in listPreData)
        {
            PreTypeEnum preType = itemData.preType;
            switch (preType)
            {
                case PreTypeEnum.PayMoneyL:
                    long moneyL = long.Parse(itemData.preData);
                    gameData.PayMoney(moneyL, 0, 0);
                    break;
                case PreTypeEnum.PayMoneyM:
                    long moneyM = long.Parse(itemData.preData);
                    gameData.PayMoney(0, moneyM, 0);
                    break;
                case PreTypeEnum.PayMoneyS:
                    long moneyS = long.Parse(itemData.preData);
                    gameData.PayMoney(0, 0, moneyS);
                    break;
            }
        }
    }

}