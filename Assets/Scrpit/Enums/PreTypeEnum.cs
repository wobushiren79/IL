using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public enum PreTypeEnum 
{
    PayMoneyL,//支付金钱L
    PayMoneyM,//支付金钱M
    PayMoneyS,//支付金钱S
}

public class PreTypeEnumTools
{
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
    public static string GetPreDescribe(PreTypeEnum preType,string preValue)
    {
        string preDescribe = "";
        switch (preType)
        {
            case PreTypeEnum.PayMoneyL:
                preDescribe = string.Format(GameCommonInfo.GetUITextById(5001), preValue);
                break;
            case PreTypeEnum.PayMoneyM:
                preDescribe = string.Format(GameCommonInfo.GetUITextById(5002), preValue);
                break;
            case PreTypeEnum.PayMoneyS:
                preDescribe = string.Format(GameCommonInfo.GetUITextById(5003), preValue);
                break;
        }
        return preDescribe;
    }

    /// <summary>
    /// 获取前置图标
    /// </summary>
    /// <param name="preType"></param>
    /// <param name="iconDataManager"></param>
    /// <returns></returns>
    public static Sprite GetPreSprite(PreTypeEnum preType, IconDataManager  iconDataManager)
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
        }
        return spIcon;
    }

}