using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public enum RewardTypeEnum 
{
    AddWorkerNumber,//增加工作人数上限
}

public class RewardTypeEnumTools
{
    /// <summary>
    /// 获取奖励数据
    /// </summary>
    /// <returns></returns>
    public static Dictionary<RewardTypeEnum, string> GetRewardData(string data)
    {
        Dictionary<RewardTypeEnum, string> rewardData = new Dictionary<RewardTypeEnum, string>();
        List<string> listData = StringUtil.SplitBySubstringForListStr(data, '|');
        foreach (string itemData in listData)
        {
            if (CheckUtil.StringIsNull(itemData))
                continue;
            List<string> itemListData = StringUtil.SplitBySubstringForListStr(itemData, ':');
            RewardTypeEnum rewardType = EnumUtil.GetEnum<RewardTypeEnum>(itemListData[0]);
            string rewardValue = itemListData[1];
            rewardData.Add(rewardType, rewardValue);
        }
        return rewardData;
    }

    /// <summary>
    /// 获取奖励描述
    /// </summary>
    /// <param name="rewardType"></param>
    /// <returns></returns>
    public static string GetRewardDescribe(RewardTypeEnum rewardType)
    {
        string rewardDescribe = "";
        switch (rewardType)
        {
            case RewardTypeEnum.AddWorkerNumber:
                break;
        }
        return rewardDescribe;
    }
}