using UnityEngine;
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
    AddArenaTrophyElementary,//初级竞技场奖杯
    AddArenaTrophyIntermediate,//中级竞技场奖杯
    AddArenaTrophyAdvanced,//高级竞技场奖杯
    AddArenaTrophyLegendary,//传说竞技场奖杯
    AddIngOilsalt,//油盐
    AddIngMeat,//肉类
    AddIngRiverfresh,//河鲜
    AddIngSeafood,//海鲜
    AddIngVegetables,//蔬菜
    AddIngMelonfruit,//瓜果
    AddIngWaterwine,//酒水
    AddIngFlour,//面粉
}

public class RewardTypeBean : DataBean<RewardTypeEnum>
{
    public string rewardDescribe;
    public Sprite spRewardIcon;
    public long rewardId;
    public int rewardNumber = 1;
    public CharacterBean workerCharacterData;

    public RewardTypeBean() : base(RewardTypeEnum.AddMoneyS, "")
    {
    }
    public RewardTypeBean(RewardTypeEnum dataType,string data) : base(dataType, data)
    {
    }

    public RewardTypeEnum GetRewardType()
    {
        return dataType;
    }
}

public class RewardTypeEnumTools : DataTools
{
    /// <summary>
    /// 获取奖励数据
    /// </summary>
    /// <returns></returns>
    public static List<RewardTypeBean> GetListRewardData(string data)
    {
        return GetListData<RewardTypeBean, RewardTypeEnum>(data);
    }

    /// <summary>
    /// 根据类型获取奖励数据
    /// </summary>
    /// <param name="dataType"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static List<RewardTypeBean> GetListRewardDataByType(RewardTypeEnum dataType, string data)
    {
        List<RewardTypeBean> listAllReward = GetListRewardData(data);
        List<RewardTypeBean> listReward = new List<RewardTypeBean>();
        foreach (RewardTypeBean itemReward in listAllReward)
        {
            if (itemReward.dataType == dataType)
            {
                listReward.Add(itemReward);
            }
        }
        return listReward;
    }

    public static RewardTypeBean GetRewardDetails(RewardTypeBean data, IconDataManager iconDataManager, GameItemsManager gameItemsManager, InnBuildManager innBuildManager, NpcInfoManager npcInfoManager)
    {
        switch (data.dataType)
        {
            case RewardTypeEnum.AddWorkerNumber:
                data.spRewardIcon = iconDataManager.GetIconSpriteByName("ui_features_worker");
                data.rewardDescribe = string.Format(GameCommonInfo.GetUITextById(6001), data.data);
                data.rewardNumber = int.Parse(data.data);
                break;
            case RewardTypeEnum.AddWorker:
                long workerId = long.Parse(data.data);
                data.workerCharacterData = npcInfoManager.GetCharacterDataById(workerId);
                data.rewardDescribe = string.Format(GameCommonInfo.GetUITextById(6011), data.workerCharacterData.baseInfo.name);
                break;
            case RewardTypeEnum.AddMoneyL:
            case RewardTypeEnum.AddMoneyM:
            case RewardTypeEnum.AddMoneyS:
                GetRewardDetailsForAddMoney(data, iconDataManager);
                break;
            case RewardTypeEnum.AddGuildCoin:
                data.spRewardIcon = iconDataManager.GetIconSpriteByName("guild_coin_2");
                data.rewardDescribe = string.Format(GameCommonInfo.GetUITextById(6005), data.data);
                data.rewardNumber = int.Parse(data.data);
                break;
            case RewardTypeEnum.AddItems:
                data = GetRewardDetailsForItems(data, iconDataManager, gameItemsManager);
                break;
            case RewardTypeEnum.AddBuildItems:
                data = GetRewardDetailsForBuildItems(data, iconDataManager, innBuildManager);
                break;
            case RewardTypeEnum.AddArenaTrophyElementary:
            case RewardTypeEnum.AddArenaTrophyIntermediate:
            case RewardTypeEnum.AddArenaTrophyAdvanced:
            case RewardTypeEnum.AddArenaTrophyLegendary:
                GetRewardDetailsForAddTrophy(data,iconDataManager);
                break;
            case RewardTypeEnum.AddIngOilsalt:
            case RewardTypeEnum.AddIngMeat:
            case RewardTypeEnum.AddIngRiverfresh:
            case RewardTypeEnum.AddIngSeafood:
            case RewardTypeEnum.AddIngVegetables:
            case RewardTypeEnum.AddIngMelonfruit:
            case RewardTypeEnum.AddIngWaterwine:
            case RewardTypeEnum.AddIngFlour:
                GetRewardDetailsForIng(iconDataManager, data);
                break;
        }
        return data;
    }


    /// <summary>
    /// 获取奖杯详情
    /// </summary>
    /// <param name="rewardTypeData"></param>
    /// <param name="iconDataManager"></param>
    /// <returns></returns>
    private static RewardTypeBean GetRewardDetailsForAddTrophy(RewardTypeBean rewardTypeData, IconDataManager iconDataManager)
    {
        string iconKey = "";
        string rewardDescribe = "???";
        switch (rewardTypeData.dataType)
        {
            case RewardTypeEnum.AddArenaTrophyElementary:
                iconKey = "trophy_1_0";
                rewardDescribe = string.Format(GameCommonInfo.GetUITextById(6006), rewardTypeData.data);
                break;
            case RewardTypeEnum.AddArenaTrophyIntermediate:
                iconKey = "trophy_1_1";
                rewardDescribe = string.Format(GameCommonInfo.GetUITextById(6007), rewardTypeData.data);
                break;
            case RewardTypeEnum.AddArenaTrophyAdvanced:
                iconKey = "trophy_1_2";
                rewardDescribe = string.Format(GameCommonInfo.GetUITextById(6008), rewardTypeData.data);
                break;
            case RewardTypeEnum.AddArenaTrophyLegendary:
                iconKey = "trophy_1_3";
                rewardDescribe = string.Format(GameCommonInfo.GetUITextById(6009), rewardTypeData.data);
                break;
        }
        rewardTypeData.spRewardIcon = iconDataManager.GetIconSpriteByName(iconKey);
        rewardTypeData.rewardDescribe = string.Format(GameCommonInfo.GetUITextById(6009), rewardTypeData.data);
        rewardTypeData.rewardNumber = int.Parse(rewardTypeData.data);
        return rewardTypeData;
    }

    /// <summary>
    /// 获取金钱详情
    /// </summary>
    /// <param name="rewardTypeData"></param>
    /// <param name="iconDataManager"></param>
    /// <returns></returns>
    private static RewardTypeBean GetRewardDetailsForAddMoney(RewardTypeBean rewardTypeData, IconDataManager iconDataManager)
    {
        string iconKey = "";
        string rewardDescribe = "???";
        switch (rewardTypeData.dataType)
        {
            case RewardTypeEnum.AddMoneyL:
                iconKey = "money_3";
                rewardDescribe= string.Format(GameCommonInfo.GetUITextById(6002), rewardTypeData.data);
                break;
            case RewardTypeEnum.AddMoneyM:
                iconKey = "money_2";
                rewardDescribe = string.Format(GameCommonInfo.GetUITextById(6003), rewardTypeData.data);
                break;
            case RewardTypeEnum.AddMoneyS:
                iconKey = "money_1";
                rewardDescribe = string.Format(GameCommonInfo.GetUITextById(6004), rewardTypeData.data);
                break;
        }
        rewardTypeData.spRewardIcon = iconDataManager.GetIconSpriteByName(iconKey);
        rewardTypeData.rewardDescribe = rewardDescribe;
        rewardTypeData.rewardNumber = int.Parse(rewardTypeData.data);
        return rewardTypeData;
    }

    /// <summary>
    /// 获取建筑数据详情
    /// </summary>
    /// <param name="data"></param>
    /// <param name="iconDataManager"></param>
    /// <param name="innBuildManager"></param>
    /// <returns></returns>
    private static RewardTypeBean GetRewardDetailsForBuildItems(RewardTypeBean data, IconDataManager iconDataManager, InnBuildManager innBuildManager)
    {
        string[] listBuildItemsData = StringUtil.SplitBySubstringForArrayStr(data.data, ',');
        long buildItemId = long.Parse(listBuildItemsData[0]);
        BuildItemBean buildItemInfo = innBuildManager.GetBuildDataById(buildItemId);
        data.rewardDescribe = buildItemInfo.name;
        if (listBuildItemsData.Length == 2)
        {
            data.rewardNumber = int.Parse(listBuildItemsData[2]);
        }
        data.rewardId = buildItemId;
        data.rewardDescribe += (" x" + data.rewardNumber);
        data.spRewardIcon = innBuildManager.GetFurnitureSpriteByName(buildItemInfo.icon_key);
        return data;
    }

    /// <summary>
    /// 获取道具的奖励详情
    /// </summary>
    /// <param name="data"></param>
    /// <param name="iconDataManager"></param>
    /// <param name="gameItemsManager"></param>
    /// <returns></returns>
    private static RewardTypeBean GetRewardDetailsForItems(RewardTypeBean data, IconDataManager iconDataManager, GameItemsManager gameItemsManager)
    {
        string[] listItemsData = StringUtil.SplitBySubstringForArrayStr(data.data, ',');
        long itemId = long.Parse(listItemsData[0]);
        ItemsInfoBean itemsInfo = gameItemsManager.GetItemsById(itemId);
        data.rewardDescribe = itemsInfo.name;
        if (listItemsData.Length == 2)
        {
            data.rewardNumber = int.Parse(listItemsData[1]);
        }
        data.rewardId = itemId;
        data.rewardDescribe += (" x" + data.rewardNumber);
        data.spRewardIcon = GeneralEnumTools.GetGeneralSprite(itemsInfo, iconDataManager, gameItemsManager, null, true);
        return data;
    }

    /// <summary>
    /// 获取食材的奖励详情
    /// </summary>
    /// <param name="rewardTypeData"></param>
    /// <param name="ingName"></param>
    /// <returns></returns>
    private static RewardTypeBean GetRewardDetailsForIng(IconDataManager iconDataManager, RewardTypeBean rewardTypeData)
    {
        string ingName = "???";
        string iconKey = "";
        switch (rewardTypeData.dataType)
        {
            case RewardTypeEnum.AddIngOilsalt:
                ingName = GameCommonInfo.GetUITextById(21);
                iconKey = "ui_ing_oilsalt";
                break;
            case RewardTypeEnum.AddIngMeat:
                ingName = GameCommonInfo.GetUITextById(22);
                iconKey = "ui_ing_meat";
                break;
            case RewardTypeEnum.AddIngRiverfresh:
                ingName = GameCommonInfo.GetUITextById(23);
                iconKey = "ui_ing_riverfresh";
                break;
            case RewardTypeEnum.AddIngSeafood:
                ingName = GameCommonInfo.GetUITextById(24);
                iconKey = "ui_ing_seafood";
                break;
            case RewardTypeEnum.AddIngVegetables:
                ingName = GameCommonInfo.GetUITextById(25);
                iconKey = "ui_ing_vegetables";
                break;
            case RewardTypeEnum.AddIngMelonfruit:
                ingName = GameCommonInfo.GetUITextById(26);
                iconKey = "ui_ing_melonfruit";
                break;
            case RewardTypeEnum.AddIngWaterwine:
                ingName = GameCommonInfo.GetUITextById(27);
                iconKey = "ui_ing_waterwine";
                break;
            case RewardTypeEnum.AddIngFlour:
                ingName = GameCommonInfo.GetUITextById(28);
                iconKey = "ui_ing_flour";
                break;
        }
        rewardTypeData.rewardDescribe = string.Format(GameCommonInfo.GetUITextById(6010), ingName, rewardTypeData.data);
        rewardTypeData.rewardNumber = int.Parse(rewardTypeData.data);
        rewardTypeData.spRewardIcon = iconDataManager.GetIconSpriteByName(iconKey);
        return rewardTypeData;
    }

    /// <summary>
    /// 完成所有奖励
    /// </summary>
    /// <param name="reward_data"></param>
    /// <param name="gameData"></param>
    public static void CompleteReward(ToastManager toastManager, NpcInfoManager npcInfoManager, IconDataManager iconDataManager,GameItemsManager gameItemsManager,InnBuildManager innBuildManager, GameDataManager gameDataManager, string data)
    {
        List<RewardTypeBean> listRewardData = GetListRewardData(data);
        CompleteReward(toastManager , npcInfoManager, iconDataManager,gameItemsManager, innBuildManager, gameDataManager,  listRewardData);
    }

    public static void CompleteReward(ToastManager toastManager, NpcInfoManager npcInfoManager, IconDataManager iconDataManager, GameItemsManager gameItemsManager, InnBuildManager innBuildManager, GameDataManager gameDataManager, List<RewardTypeBean> listRewardData)
    {
        GameDataBean gameData = gameDataManager.gameData;
        foreach (var itemData in listRewardData)
        {
            GetRewardDetails(itemData, iconDataManager, gameItemsManager, innBuildManager, npcInfoManager);
            RewardTypeEnum dataType = itemData.dataType;
            switch (dataType)
            {
                case RewardTypeEnum.AddWorkerNumber:
                    int addWorkerNumber = itemData.rewardNumber;
                    gameData.workerNumberLimit += addWorkerNumber;
                    break;
                case RewardTypeEnum.AddWorker:
                    gameData.AddWorkCharacter(itemData.workerCharacterData);
                    toastManager.ToastHint(string.Format(GameCommonInfo.GetUITextById(6011), itemData.workerCharacterData.baseInfo.name));
                    break;
                case RewardTypeEnum.AddGuildCoin:
                    long addGuildCoin = itemData.rewardNumber;
                    gameData.AddGuildCoin(addGuildCoin);
                    toastManager.ToastHint(itemData.spRewardIcon, string.Format(GameCommonInfo.GetUITextById(6099), itemData.rewardDescribe));
                    break;
                case RewardTypeEnum.AddMoneyL:
                    long addMoneyL = itemData.rewardNumber;
                    gameData.AddMoney(addMoneyL, 0, 0);
                    toastManager.ToastHint(itemData.spRewardIcon, string.Format(GameCommonInfo.GetUITextById(6014), addMoneyL + ""));
                    break;
                case RewardTypeEnum.AddMoneyM:
                    long addMoneyM = itemData.rewardNumber;
                    gameData.AddMoney(0, addMoneyM, 0);
                    toastManager.ToastHint(itemData.spRewardIcon, string.Format(GameCommonInfo.GetUITextById(6013), addMoneyM + ""));
                    break;
                case RewardTypeEnum.AddMoneyS:
                    long addMoneyS = itemData.rewardNumber;
                    gameData.AddMoney(0, 0, addMoneyS);
                    toastManager.ToastHint(itemData.spRewardIcon, string.Format(GameCommonInfo.GetUITextById(6012), addMoneyS+""));
                    break;
                case RewardTypeEnum.AddItems:
                    gameData.AddNewItems(itemData.rewardId, itemData.rewardNumber);
                    toastManager.ToastHint(itemData.spRewardIcon, string.Format(GameCommonInfo.GetUITextById(6099), itemData.rewardDescribe));
                    break;
                case RewardTypeEnum.AddBuildItems:
                    gameData.AddBuildNumber(itemData.rewardId, itemData.rewardNumber);
                    toastManager.ToastHint(itemData.spRewardIcon, string.Format(GameCommonInfo.GetUITextById(6099), itemData.rewardDescribe));
                    break;

                case RewardTypeEnum.AddIngOilsalt:
                    gameData.AddIng(IngredientsEnum.Oilsalt, itemData.rewardNumber);
                    toastManager.ToastHint(itemData.spRewardIcon, string.Format(GameCommonInfo.GetUITextById(6099), itemData.rewardDescribe));
                    break;
                case RewardTypeEnum.AddIngMeat:
                    gameData.AddIng(IngredientsEnum.Meat, itemData.rewardNumber);
                    toastManager.ToastHint(itemData.spRewardIcon, string.Format(GameCommonInfo.GetUITextById(6099), itemData.rewardDescribe));
                    break;
                case RewardTypeEnum.AddIngRiverfresh:
                    gameData.AddIng(IngredientsEnum.Riverfresh, itemData.rewardNumber);
                    toastManager.ToastHint(itemData.spRewardIcon, string.Format(GameCommonInfo.GetUITextById(6099), itemData.rewardDescribe));
                    break;
                case RewardTypeEnum.AddIngSeafood:
                    gameData.AddIng(IngredientsEnum.Seafood, itemData.rewardNumber);
                    toastManager.ToastHint(itemData.spRewardIcon, string.Format(GameCommonInfo.GetUITextById(6099), itemData.rewardDescribe));
                    break;
                case RewardTypeEnum.AddIngVegetables:
                    gameData.AddIng(IngredientsEnum.Vegetables, itemData.rewardNumber);
                    toastManager.ToastHint(itemData.spRewardIcon, string.Format(GameCommonInfo.GetUITextById(6099), itemData.rewardDescribe));
                    break;
                case RewardTypeEnum.AddIngMelonfruit:
                    gameData.AddIng(IngredientsEnum.Melonfruit, itemData.rewardNumber);
                    toastManager.ToastHint(itemData.spRewardIcon, string.Format(GameCommonInfo.GetUITextById(6099), itemData.rewardDescribe));
                    break;
                case RewardTypeEnum.AddIngWaterwine:
                    gameData.AddIng(IngredientsEnum.Waterwine, itemData.rewardNumber);
                    toastManager.ToastHint(itemData.spRewardIcon, string.Format(GameCommonInfo.GetUITextById(6099), itemData.rewardDescribe));
                    break;
                case RewardTypeEnum.AddIngFlour:
                    gameData.AddIng(IngredientsEnum.Flour, itemData.rewardNumber);
                    toastManager.ToastHint(itemData.spRewardIcon, string.Format(GameCommonInfo.GetUITextById(6099), itemData.rewardDescribe));
                    break;
            }
        }
    }

}