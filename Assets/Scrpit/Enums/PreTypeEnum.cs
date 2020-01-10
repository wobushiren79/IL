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
        Dictionary<PreTypeEnum, string> listPreData = GetPreData(data);
        foreach (var itemPreData in listPreData)
        {
            PreTypeEnum preType = itemPreData.Key;
            GetPreDescribe(preType, itemPreData.Value, gameData, out bool isPre, out float progress);
            if (!isPre)
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
    public static Dictionary<PreTypeEnum, string> GetPreData(string data)
    {
        Dictionary<PreTypeEnum, string> preData = new Dictionary<PreTypeEnum, string>();
        List<string> listData = StringUtil.SplitBySubstringForListStr(data, '|');
        foreach (string itemData in listData)
        {
            if (CheckUtil.StringIsNull(itemData))
                continue;
            List<string> itemListData = StringUtil.SplitBySubstringForListStr(itemData, ':');
            PreTypeEnum preType = EnumUtil.GetEnum<PreTypeEnum>(itemListData[0]);
            string preValue = itemListData[1];
            preData.Add(preType, preValue);
        }
        return preData;
    }

    /// <summary>
    /// 获取前置描述
    /// </summary>
    /// <param name="rewardType"></param>
    /// <returns></returns>
    public static string GetPreDescribe(PreTypeEnum preType, string preValue, GameDataBean gameData, out bool isPre, out float progress)
    {
        string preDescribe = "";
        isPre = false;
        progress = 0;
        //支付金钱 L
        if (preType == PreTypeEnum.PayMoneyL)
        {
            string preMoneyLStr = "";
            long payMoneyL = long.Parse(preValue);
            if (gameData.moneyL >= payMoneyL)
            {
                isPre = true;
                preMoneyLStr = "(" + payMoneyL + "/" + payMoneyL + ")";
                progress = 1;
            }
            else
            {
                isPre = false;
                preMoneyLStr = "(" + gameData.moneyL + "/" + payMoneyL + ")";
                progress = gameData.moneyL / (float)payMoneyL;
            }
            preDescribe = string.Format(GameCommonInfo.GetUITextById(5001), preMoneyLStr);
        }
        //支付金钱 M
        else if (preType == PreTypeEnum.PayMoneyM)
        {
            string preMoneyMStr = "";
            long payMoneyM = long.Parse(preValue);
            if (gameData.moneyM >= payMoneyM)
            {
                isPre = true;
                preMoneyMStr = "(" + payMoneyM + "/" + payMoneyM + ")";
                progress = 1;
            }
            else
            {
                isPre = false;
                preMoneyMStr = "(" + gameData.moneyM + "/" + payMoneyM + ")";
                progress = gameData.moneyM / (float)payMoneyM;
            }
            preDescribe = string.Format(GameCommonInfo.GetUITextById(5002), preMoneyMStr);
        }
        //支付金钱 S
        else if (preType == PreTypeEnum.PayMoneyS)
        {
            string preMoneySStr = "";
            long payMoneS = long.Parse(preValue);
            if (gameData.moneyS >= payMoneS)
            {
                isPre = true;
                preMoneySStr = "(" + payMoneS + "/" + payMoneS + ")";
                progress = 1;
            }
            else
            {
                isPre = false;
                preMoneySStr = "(" + gameData.moneyS + "/" + payMoneS + ")";
                progress = gameData.moneyS / (float)payMoneS;
            }
            preDescribe = string.Format(GameCommonInfo.GetUITextById(5003), preMoneySStr);
        }
        //拥有金钱 L
        else if (preType == PreTypeEnum.HaveMoneyL)
        {
            string haveMoneyLStr = "";
            long haveMoneyL = long.Parse(preValue);
            if (gameData.moneyL >= haveMoneyL)
            {
                isPre = true;
                haveMoneyLStr = "(" + haveMoneyL + "/" + haveMoneyL + ")";
                progress = 1;
            }
            else
            {
                isPre = false;
                haveMoneyLStr = "(" + gameData.moneyL + "/" + haveMoneyL + ")";
                progress = gameData.moneyL / (float)haveMoneyL;
            }
            preDescribe = string.Format(GameCommonInfo.GetUITextById(5004), haveMoneyLStr);
        }
        //拥有金钱 M
        else if (preType == PreTypeEnum.HaveMoneyM)
        {
            string haveMoneyMStr = "";
            long haveMoneyM = long.Parse(preValue);
            if (gameData.moneyM >= haveMoneyM)
            {
                isPre = true;
                haveMoneyMStr = "(" + haveMoneyM + "/" + haveMoneyM + ")";
                progress = 1;
            }
            else
            {
                isPre = false;
                haveMoneyMStr = "(" + gameData.moneyM + "/" + haveMoneyM + ")";
                progress = gameData.moneyM / (float)haveMoneyM;
            }
            preDescribe = string.Format(GameCommonInfo.GetUITextById(5005), haveMoneyMStr);
        }
        //拥有金钱 S
        else if (preType == PreTypeEnum.HaveMoneyS)
        {
            string haveMoneySStr = "";
            long haveMoneyS = long.Parse(preValue);
            if (gameData.moneyS >= haveMoneyS)
            {
                isPre = true;
                haveMoneySStr = "(" + haveMoneyS + "/" + haveMoneyS + ")";
                progress = 1;
            }
            else
            {
                isPre = false;
                haveMoneySStr = "(" + gameData.moneyS + "/" + haveMoneyS + ")";
                progress = gameData.moneyS / (float)haveMoneyS;
            }
            preDescribe = string.Format(GameCommonInfo.GetUITextById(5006), haveMoneySStr);
        }
        return preDescribe;
    }

    /// <summary>
    /// 获取前置图标
    /// </summary>
    /// <param name="preType"></param>
    /// <param name="iconDataManager"></param>
    /// <returns></returns>
    public static Sprite GetPreSprite(PreTypeEnum preType, IconDataManager iconDataManager)
    {
        Sprite spIcon = null;
        switch (preType)
        {
            case PreTypeEnum.PayMoneyL:
                spIcon = iconDataManager.GetIconSpriteByName("money_3");
                break;
            case PreTypeEnum.PayMoneyM:
                spIcon = iconDataManager.GetIconSpriteByName("money_2");
                break;
            case PreTypeEnum.PayMoneyS:
                spIcon = iconDataManager.GetIconSpriteByName("money_1");
                break;
            case PreTypeEnum.HaveMoneyL:
                spIcon = iconDataManager.GetIconSpriteByName("money_3");
                break;
            case PreTypeEnum.HaveMoneyM:
                spIcon = iconDataManager.GetIconSpriteByName("money_2");
                break;
            case PreTypeEnum.HaveMoneyS:
                spIcon = iconDataManager.GetIconSpriteByName("money_1");
                break;
        }
        return spIcon;
    }

    /// <summary>
    /// 完成前置条件
    /// </summary>
    /// <param name="data"></param>
    /// <param name="gameData"></param>
    public static void CompletePre(string data, GameDataBean gameData)
    {
        Dictionary<PreTypeEnum, string> listPre = GetPreData(data);
        foreach (var itemData in listPre)
        {
            PreTypeEnum preType = itemData.Key;
            switch (preType)
            {
                case PreTypeEnum.PayMoneyL:
                    long moneyL = long.Parse(itemData.Value);
                    gameData.PayMoney(moneyL, 0, 0);
                    break;
                case PreTypeEnum.PayMoneyM:
                    long moneyM = long.Parse(itemData.Value);
                    gameData.PayMoney(0, moneyM, 0);
                    break;
                case PreTypeEnum.PayMoneyS:
                    long moneyS = long.Parse(itemData.Value);
                    gameData.PayMoney(0, 0, moneyS);
                    break;
            }
        }
    }

}