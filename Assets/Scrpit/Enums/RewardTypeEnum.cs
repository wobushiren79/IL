﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public enum RewardTypeEnum
{
    AddWorkerNumber,//增加工作人数上限
    AddWorker,//增加工作人员
    AddMoneyL,//增加金钱
    AddMoneyM,
    AddMoneyS,
    AddGuildCoin,//增加公会硬币
    AddItems,//增加道具
    AddBuildItems,//增加建筑材料

}

public class RewardTypeBean
{
    public RewardTypeEnum rewardType;
    public string rewardData;

    public string rewardDescribe;
    public Sprite spRewardIcon;
    public CharacterBean workerCharacterData;

    public RewardTypeBean(RewardTypeEnum rewardType, string rewardData)
    {
        this.rewardData = rewardData;
        this.rewardType = rewardType;
    }
}

public class RewardTypeEnumTools
{
    /// <summary>
    /// 获取奖励数据
    /// </summary>
    /// <returns></returns>
    public static List<RewardTypeBean> GetListRewardData(string data)
    {
        List<RewardTypeBean> listRewardData = new List<RewardTypeBean>();
        if (CheckUtil.StringIsNull(data))
        {
            return listRewardData;
        }
        List<string> listData = StringUtil.SplitBySubstringForListStr(data, '|');
        foreach (string itemData in listData)
        {
            if (CheckUtil.StringIsNull(itemData))
                continue;
            List<string> itemListData = StringUtil.SplitBySubstringForListStr(itemData, ':');
            RewardTypeEnum rewardType = EnumUtil.GetEnum<RewardTypeEnum>(itemListData[0]);
            string rewardValue = itemListData[1];
            listRewardData.Add(new RewardTypeBean(rewardType, rewardValue));
        }
        return listRewardData;
    }

    /// <summary>
    /// 根据类型获取奖励数据
    /// </summary>
    /// <param name="rewardType"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static List<RewardTypeBean> GetListRewardDataByType(RewardTypeEnum rewardType, string data)
    {
        List<RewardTypeBean> listAllReward = GetListRewardData(data);
        List<RewardTypeBean> listReward = new List<RewardTypeBean>();
        foreach (RewardTypeBean itemReward in listAllReward)
        {
            if (itemReward.rewardType == rewardType)
            {
                listReward.Add(itemReward);
            }
        }
        return listReward;
    }

    /// <summary>
    /// 获取奖励描述
    /// </summary>
    /// <returns></returns>
    public static RewardTypeBean GetRewardDetails(RewardTypeBean rewardData, IconDataManager iconDataManager, GameItemsManager gameItemsManager, InnBuildManager innBuildManager)
    {
        return GetRewardDetails(rewardData, iconDataManager, gameItemsManager, innBuildManager, null);
    }
    public static RewardTypeBean GetRewardDetails(RewardTypeBean rewardData, IconDataManager iconDataManager, GameItemsManager gameItemsManager, InnBuildManager innBuildManager, NpcInfoManager npcInfoManager)
    {
        switch (rewardData.rewardType)
        {
            case RewardTypeEnum.AddWorkerNumber:
                rewardData.spRewardIcon = iconDataManager.GetIconSpriteByName("ui_features_worker");
                rewardData.rewardDescribe = string.Format(GameCommonInfo.GetUITextById(6001), rewardData.rewardData);
                break;
            case RewardTypeEnum.AddWorker:
                long workerId = long.Parse(rewardData.rewardData);
                rewardData.workerCharacterData = npcInfoManager.GetCharacterDataById(workerId);
                break;
            case RewardTypeEnum.AddMoneyL:
                rewardData.spRewardIcon = iconDataManager.GetIconSpriteByName("money_3");
                rewardData.rewardDescribe = string.Format(GameCommonInfo.GetUITextById(6002), rewardData.rewardData);
                break;
            case RewardTypeEnum.AddMoneyM:
                rewardData.spRewardIcon = iconDataManager.GetIconSpriteByName("money_2");
                rewardData.rewardDescribe = string.Format(GameCommonInfo.GetUITextById(6003), rewardData.rewardData);
                break;
            case RewardTypeEnum.AddMoneyS:
                rewardData.spRewardIcon = iconDataManager.GetIconSpriteByName("money_1");
                rewardData.rewardDescribe = string.Format(GameCommonInfo.GetUITextById(6004), rewardData.rewardData);
                break;
            case RewardTypeEnum.AddGuildCoin:
                rewardData.spRewardIcon = iconDataManager.GetIconSpriteByName("guild_coin_2");
                rewardData.rewardDescribe = string.Format(GameCommonInfo.GetUITextById(6005), rewardData.rewardData);
                break;
            case RewardTypeEnum.AddItems:
                rewardData = GetRewardDetailsForItems(rewardData, iconDataManager, gameItemsManager);
                break;
            case RewardTypeEnum.AddBuildItems:
                GetRewardDetailsForBuildItems(rewardData, iconDataManager, innBuildManager);
                break;
        }
        return rewardData;
    }

    /// <summary>
    /// 获取奖励 增加的金钱
    /// </summary>
    /// <param name="data"></param>
    /// <param name="addMoneyL"></param>
    /// <param name="addMoneyM"></param>
    /// <param name="addMoneyS"></param>
    public static void GetRewardForAddMoney(string data, out long addMoneyL, out long addMoneyM, out long addMoneyS)
    {
        addMoneyL = 0;
        addMoneyM = 0;
        addMoneyS = 0;
        List<RewardTypeBean> listAllReward = GetListRewardData(data);
        foreach (RewardTypeBean rewardItem in listAllReward)
        {
            if (rewardItem.rewardType == RewardTypeEnum.AddMoneyL)
            {
                addMoneyL = long.Parse(rewardItem.rewardData);
            }
            else if (rewardItem.rewardType == RewardTypeEnum.AddMoneyM)
            {
                addMoneyM = long.Parse(rewardItem.rewardData);
            }
            else if (rewardItem.rewardType == RewardTypeEnum.AddMoneyS)
            {
                addMoneyS = long.Parse(rewardItem.rewardData);
            }
        }
    }

    /// <summary>
    /// 获取建筑数据详情
    /// </summary>
    /// <param name="rewardData"></param>
    /// <param name="iconDataManager"></param>
    /// <param name="innBuildManager"></param>
    /// <returns></returns>
    private static RewardTypeBean GetRewardDetailsForBuildItems(RewardTypeBean rewardData, IconDataManager iconDataManager, InnBuildManager innBuildManager)
    {
        string[] listBuildItemsData = StringUtil.SplitBySubstringForArrayStr(rewardData.rewardData, ',');
        long buildItemId = long.Parse(listBuildItemsData[0]);
        int buildItemNumber = 1;
        BuildItemBean buildItemInfo = innBuildManager.GetBuildDataById(buildItemId);
        rewardData.rewardDescribe = buildItemInfo.name;
        if (listBuildItemsData.Length == 2)
        {
            buildItemNumber = int.Parse(listBuildItemsData[2]);
        }
        rewardData.rewardDescribe += (" x" + buildItemNumber);
        rewardData.spRewardIcon = innBuildManager.GetFurnitureSpriteByName(buildItemInfo.icon_key);
        return rewardData;
    }

    /// <summary>
    /// 获取道具的奖励详情
    /// </summary>
    /// <param name="rewardData"></param>
    /// <param name="iconDataManager"></param>
    /// <param name="gameItemsManager"></param>
    /// <returns></returns>
    private static RewardTypeBean GetRewardDetailsForItems(RewardTypeBean rewardData, IconDataManager iconDataManager, GameItemsManager gameItemsManager)
    {
        string[] listItemsData = StringUtil.SplitBySubstringForArrayStr(rewardData.rewardData, ',');
        long itemId = long.Parse(listItemsData[0]);
        int itemNumber = 1;
        ItemsInfoBean itemsInfo = gameItemsManager.GetItemsById(itemId);
        rewardData.rewardDescribe = itemsInfo.name;
        if (listItemsData.Length == 2)
        {
            itemNumber = int.Parse(listItemsData[1]);
        }
        rewardData.rewardDescribe += (" x" + itemNumber);
        rewardData.spRewardIcon = GeneralEnumTools.GetGeneralSprite(itemsInfo, iconDataManager, gameItemsManager, null, true);
        return rewardData;
    }

    /// <summary>
    /// 完成所有奖励
    /// </summary>
    /// <param name="reward_data"></param>
    /// <param name="gameData"></param>
    public static void CompleteReward(string data, GameDataBean gameData)
    {
        List<RewardTypeBean> listRewardData = GetListRewardData(data);
        CompleteReward(listRewardData, gameData);
    }
    public static void CompleteReward(List<RewardTypeBean> listRewardData, GameDataBean gameData)
    {
        foreach (var itemData in listRewardData)
        {
            RewardTypeEnum rewardType = itemData.rewardType;
            switch (rewardType)
            {
                case RewardTypeEnum.AddWorkerNumber:
                    int addWorkerNumber = int.Parse(itemData.rewardData);
                    gameData.workerNumberLimit += addWorkerNumber;
                    break;
                case RewardTypeEnum.AddGuildCoin:
                    long addGuildCoin = long.Parse(itemData.rewardData);
                    gameData.AddGuildCoin(addGuildCoin);
                    break;
                case RewardTypeEnum.AddMoneyL:
                    long addMoneyL = long.Parse(itemData.rewardData);
                    gameData.AddMoney(addMoneyL, 0, 0);
                    break;
                case RewardTypeEnum.AddMoneyM:
                    long addMoneyM = long.Parse(itemData.rewardData);
                    gameData.AddMoney(addMoneyM, 0, 0);
                    break;
                case RewardTypeEnum.AddMoneyS:
                    long addMoneyS = long.Parse(itemData.rewardData);
                    gameData.AddMoney(addMoneyS, 0, 0);
                    break;
                case RewardTypeEnum.AddItems:
                    string[] listItemsData = StringUtil.SplitBySubstringForArrayStr(itemData.rewardData, ',');
                    long itemId = long.Parse(listItemsData[0]);
                    int itemNumber = 1;
                    if (listItemsData.Length == 2)
                    {
                        itemNumber = int.Parse(listItemsData[1]);
                    }
                    for (int i = 0; i < itemNumber; i++)
                    {
                        gameData.AddNewItems(itemId, 1);
                    }
                    break;
                case RewardTypeEnum.AddBuildItems:
                    string[] listBuildItemsData = StringUtil.SplitBySubstringForArrayStr(itemData.rewardData, ',');
                    long buildItemId = long.Parse(listBuildItemsData[0]);
                    int buildItemNumber = 1;
                    if (listBuildItemsData.Length == 2)
                    {
                        buildItemNumber = int.Parse(listBuildItemsData[1]);
                    }
                    gameData.AddBuildNumber(buildItemId, buildItemNumber);
                    break;
            }
        }
    }

}