using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public enum RewardTypeEnum 
{
    AddWorkerNumber,//增加工作人数上限
    AddMoneyL,//增加金钱
    AddMoneyM,
    AddMoneyS,
    AddGuildCoin,//增加公会硬币
    AddItems,//增加道具
    AddBuildItems,//增加建筑材料
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
    public static string GetRewardDescribe(RewardTypeEnum rewardType,string data)
    {
        string rewardDescribe = "";
        switch (rewardType)
        {
            case RewardTypeEnum.AddWorkerNumber:
                rewardDescribe = string.Format(GameCommonInfo.GetUITextById(6001),data);
                break;
            case RewardTypeEnum.AddMoneyL:
                rewardDescribe = string.Format(GameCommonInfo.GetUITextById(6002), data);
                break;
            case RewardTypeEnum.AddMoneyM:
                rewardDescribe = string.Format(GameCommonInfo.GetUITextById(6003), data);
                break;
            case RewardTypeEnum.AddMoneyS:
                rewardDescribe = string.Format(GameCommonInfo.GetUITextById(6004), data);
                break;
            case RewardTypeEnum.AddGuildCoin:
                rewardDescribe = string.Format(GameCommonInfo.GetUITextById(6005), data);
                break;
        }
        return rewardDescribe;
    }

    /// <summary>
    /// 获取前置图标
    /// </summary>
    /// <param name="preType"></param>
    /// <param name="iconDataManager"></param>
    /// <returns></returns>
    public static Sprite GetRewardSprite(RewardTypeEnum rewardType, IconDataManager iconDataManager)
    {
        Sprite spIcon = null;
        switch (rewardType)
        {
            case RewardTypeEnum.AddWorkerNumber:
                spIcon = iconDataManager.GetIconSpriteByName("ui_features_worker");
                break;
            case RewardTypeEnum.AddMoneyL:
                spIcon = iconDataManager.GetIconSpriteByName("money_3");
                break;
            case RewardTypeEnum.AddMoneyM:
                spIcon = iconDataManager.GetIconSpriteByName("money_2");
                break;
            case RewardTypeEnum.AddMoneyS:
                spIcon = iconDataManager.GetIconSpriteByName("money_1");
                break;
            case RewardTypeEnum.AddGuildCoin:
                spIcon = iconDataManager.GetIconSpriteByName("guild_coin_2");
                break;
        }
        return spIcon;
    }

    /// <summary>
    /// 完成所有奖励
    /// </summary>
    /// <param name="reward_data"></param>
    /// <param name="gameData"></param>
    public static void CompleteReward(string data, GameDataBean gameData)
    {
        Dictionary<RewardTypeEnum, string> listReward= GetRewardData(data);
        foreach (var itemData in listReward)
        {
            RewardTypeEnum rewardType = itemData.Key;
            switch (rewardType)
            {
                case RewardTypeEnum.AddWorkerNumber:
                    int addWorkerNumber = int.Parse(itemData.Value);
                    gameData.workerNumberLimit += addWorkerNumber;
                    break;
            }
        }
    }
}