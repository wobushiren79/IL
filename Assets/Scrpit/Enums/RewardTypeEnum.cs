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
    RandomAddItems,//随机增加道具
    AddChefExp,//增加职业经验
    AddWaiterExp,
    AddAccountantExp,
    AddAccostExp,
    AddBeaterExp,
}

public class RewardTypeBean : DataBean<RewardTypeEnum>
{
    public string rewardDescribe;
    public Sprite spRewardIcon;
    public long rewardId;
    public int rewardNumber = 1;
    public CharacterBean workerCharacterData;
    public bool isRecord = true; //是否记录

    public RewardTypeBean() : base(RewardTypeEnum.AddMoneyS, "")
    {
    }
    public RewardTypeBean(RewardTypeEnum dataType, string data) : base(dataType, data)
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
            case RewardTypeEnum.RandomAddItems:
                data = GetRewardDetailsForRandomItems(data, iconDataManager, gameItemsManager);
                break;
            case RewardTypeEnum.AddBuildItems:
                data = GetRewardDetailsForBuildItems(data, iconDataManager, innBuildManager);
                break;
            case RewardTypeEnum.AddArenaTrophyElementary:
            case RewardTypeEnum.AddArenaTrophyIntermediate:
            case RewardTypeEnum.AddArenaTrophyAdvanced:
            case RewardTypeEnum.AddArenaTrophyLegendary:
                GetRewardDetailsForAddTrophy(data, iconDataManager);
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

            case RewardTypeEnum.AddChefExp://增加职业经验
            case RewardTypeEnum.AddWaiterExp:
            case RewardTypeEnum.AddAccountantExp:
            case RewardTypeEnum.AddAccostExp:
            case RewardTypeEnum.AddBeaterExp:
                GetRewardDetailsForWorkerExp(iconDataManager, data);
                break;
        }
        return data;
    }

    protected static RewardTypeBean GetRewardDetailsForWorkerExp(IconDataManager iconDataManager, RewardTypeBean rewardTypeData)
    {
        Sprite spriteIcon = null;
        string rewardDescribe = "???";
        switch (rewardTypeData.dataType)
        {
            case RewardTypeEnum.AddChefExp:
                spriteIcon = WorkerEnumTools.GetWorkerSprite(iconDataManager, WorkerEnum.Chef);
                rewardDescribe = WorkerEnumTools.GetWorkerName(WorkerEnum.Chef) + string.Format(GameCommonInfo.GetUITextById(6021), rewardTypeData.data);
                break;
            case RewardTypeEnum.AddWaiterExp:
                spriteIcon = WorkerEnumTools.GetWorkerSprite(iconDataManager, WorkerEnum.Waiter);
                rewardDescribe = WorkerEnumTools.GetWorkerName(WorkerEnum.Waiter) + string.Format(GameCommonInfo.GetUITextById(6021), rewardTypeData.data);
                break;
            case RewardTypeEnum.AddAccountantExp:
                spriteIcon = WorkerEnumTools.GetWorkerSprite(iconDataManager, WorkerEnum.Accountant);
                rewardDescribe = WorkerEnumTools.GetWorkerName(WorkerEnum.Accountant) + string.Format(GameCommonInfo.GetUITextById(6021), rewardTypeData.data);
                break;
            case RewardTypeEnum.AddAccostExp:
                spriteIcon = WorkerEnumTools.GetWorkerSprite(iconDataManager, WorkerEnum.Accost);
                rewardDescribe = WorkerEnumTools.GetWorkerName(WorkerEnum.Accost) + string.Format(GameCommonInfo.GetUITextById(6021), rewardTypeData.data);
                break;
            case RewardTypeEnum.AddBeaterExp:
                spriteIcon = WorkerEnumTools.GetWorkerSprite(iconDataManager, WorkerEnum.Beater);
                rewardDescribe = WorkerEnumTools.GetWorkerName(WorkerEnum.Beater) + string.Format(GameCommonInfo.GetUITextById(6021), rewardTypeData.data);
                break;
        }
        rewardTypeData.spRewardIcon = spriteIcon;
        rewardTypeData.rewardDescribe = rewardDescribe;
        rewardTypeData.rewardNumber = int.Parse(rewardTypeData.data);
        return rewardTypeData;
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
        int[] listData = StringUtil.SplitBySubstringForArrayInt(rewardTypeData.data,',');
        switch (rewardTypeData.dataType)
        {
            case RewardTypeEnum.AddArenaTrophyElementary:
                iconKey = "trophy_1_0";
                rewardDescribe = string.Format(GameCommonInfo.GetUITextById(6006), listData[0]+"");
                break;
            case RewardTypeEnum.AddArenaTrophyIntermediate:
                iconKey = "trophy_1_1";
                rewardDescribe = string.Format(GameCommonInfo.GetUITextById(6007), listData[0] + "");
                break;
            case RewardTypeEnum.AddArenaTrophyAdvanced:
                iconKey = "trophy_1_2";
                rewardDescribe = string.Format(GameCommonInfo.GetUITextById(6008), listData[0] + "");
                break;
            case RewardTypeEnum.AddArenaTrophyLegendary:
                iconKey = "trophy_1_3";
                rewardDescribe = string.Format(GameCommonInfo.GetUITextById(6009), listData[0] + "");
                break;
        }
        rewardTypeData.spRewardIcon = iconDataManager.GetIconSpriteByName(iconKey);
        rewardTypeData.rewardDescribe = rewardDescribe;
        rewardTypeData.rewardNumber = listData[0];
        if (listData.Length > 1)
        {
            int isRecord = listData[1];
            rewardTypeData.isRecord = (isRecord == 0 ? false : true);
        }
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
                rewardDescribe = string.Format(GameCommonInfo.GetUITextById(6002), rewardTypeData.data);
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
            data.rewardNumber = int.Parse(listBuildItemsData[1]);
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
    /// 获取随机道具的奖励详情
    /// </summary>
    /// <param name="data"></param>
    /// <param name="iconDataManager"></param>
    /// <param name="gameItemsManager"></param>
    /// <returns></returns>
    private static RewardTypeBean GetRewardDetailsForRandomItems(RewardTypeBean data, IconDataManager iconDataManager, GameItemsManager gameItemsManager)
    {
        long[] listItemsData = StringUtil.SplitBySubstringForArrayLong(data.data, ',');
        long randomItemsId = RandomUtil.GetRandomDataByArray(listItemsData);
        ItemsInfoBean itemsInfo = gameItemsManager.GetItemsById(randomItemsId);
        data.rewardDescribe = itemsInfo.name;
        data.rewardNumber = 1;
        data.rewardId = randomItemsId;
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
        Sprite spIcon = null;
        switch (rewardTypeData.dataType)
        {
            case RewardTypeEnum.AddIngOilsalt:
                ingName = GameCommonInfo.GetUITextById(21);
                spIcon = IngredientsEnumTools.GetIngredientIcon(iconDataManager, IngredientsEnum.Oilsalt);
                break;
            case RewardTypeEnum.AddIngMeat:
                ingName = GameCommonInfo.GetUITextById(22);
                spIcon = IngredientsEnumTools.GetIngredientIcon(iconDataManager, IngredientsEnum.Meat);
                break;
            case RewardTypeEnum.AddIngRiverfresh:
                ingName = GameCommonInfo.GetUITextById(23);
                spIcon = IngredientsEnumTools.GetIngredientIcon(iconDataManager, IngredientsEnum.Riverfresh);
                break;
            case RewardTypeEnum.AddIngSeafood:
                ingName = GameCommonInfo.GetUITextById(24);
                spIcon = IngredientsEnumTools.GetIngredientIcon(iconDataManager, IngredientsEnum.Seafood);
                break;
            case RewardTypeEnum.AddIngVegetables:
                ingName = GameCommonInfo.GetUITextById(25);
                spIcon = IngredientsEnumTools.GetIngredientIcon(iconDataManager, IngredientsEnum.Vegetables);
                break;
            case RewardTypeEnum.AddIngMelonfruit:
                ingName = GameCommonInfo.GetUITextById(26);
                spIcon = IngredientsEnumTools.GetIngredientIcon(iconDataManager, IngredientsEnum.Melonfruit);
                break;
            case RewardTypeEnum.AddIngWaterwine:
                ingName = GameCommonInfo.GetUITextById(27);
                spIcon = IngredientsEnumTools.GetIngredientIcon(iconDataManager, IngredientsEnum.Waterwine);
                break;
            case RewardTypeEnum.AddIngFlour:
                ingName = GameCommonInfo.GetUITextById(28);
                spIcon = IngredientsEnumTools.GetIngredientIcon(iconDataManager, IngredientsEnum.Flour);
                break;
        }
        rewardTypeData.rewardDescribe = string.Format(GameCommonInfo.GetUITextById(6010), ingName, rewardTypeData.data);
        rewardTypeData.rewardNumber = int.Parse(rewardTypeData.data);
        rewardTypeData.spRewardIcon = spIcon;
        return rewardTypeData;
    }

    /// <summary>
    /// 完成所有奖励
    /// </summary>
    /// <param name="reward_data"></param>
    /// <param name="gameData"></param>
    public static void CompleteReward(ToastManager toastManager, NpcInfoManager npcInfoManager, IconDataManager iconDataManager, GameItemsManager gameItemsManager, InnBuildManager innBuildManager, GameDataManager gameDataManager, List<CharacterBean> listCharacterData, string data)
    {
        List<RewardTypeBean> listRewardData = GetListRewardData(data);
        CompleteReward(toastManager, npcInfoManager, iconDataManager, gameItemsManager, innBuildManager, gameDataManager, listCharacterData, listRewardData);
    }

    public static void CompleteReward(ToastManager toastManager, NpcInfoManager npcInfoManager, IconDataManager iconDataManager, GameItemsManager gameItemsManager, InnBuildManager innBuildManager, GameDataManager gameDataManager, List<CharacterBean> listCharacterData, List<RewardTypeBean> listRewardData)
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
                    toastManager.ToastHint(itemData.spRewardIcon, string.Format(GameCommonInfo.GetUITextById(6012), addMoneyS + ""));
                    break;
                case RewardTypeEnum.AddArenaTrophyElementary:
                    long addTrophy1 = itemData.rewardNumber;
                    gameData.AddArenaTrophy(addTrophy1, 0, 0, 0,itemData.isRecord);
                    toastManager.ToastHint(itemData.spRewardIcon, string.Format(GameCommonInfo.GetUITextById(6099), string.Format(GameCommonInfo.GetUITextById(6006), addTrophy1 + "")));
                    break;
                case RewardTypeEnum.AddArenaTrophyIntermediate:
                    long addTrophy2 = itemData.rewardNumber;
                    gameData.AddArenaTrophy(0, addTrophy2, 0, 0, itemData.isRecord);
                    toastManager.ToastHint(itemData.spRewardIcon, string.Format(GameCommonInfo.GetUITextById(6099), string.Format(GameCommonInfo.GetUITextById(6007), addTrophy2 + "")));
                    break;
                case RewardTypeEnum.AddArenaTrophyAdvanced:
                    long addTrophy3 = itemData.rewardNumber;
                    gameData.AddArenaTrophy(0, 0, addTrophy3, 0, itemData.isRecord);
                    toastManager.ToastHint(itemData.spRewardIcon, string.Format(GameCommonInfo.GetUITextById(6099), string.Format(GameCommonInfo.GetUITextById(6008), addTrophy3 + "")));
                    break;
                case RewardTypeEnum.AddArenaTrophyLegendary:
                    long addTrophy4 = itemData.rewardNumber;
                    gameData.AddArenaTrophy(0, 0, 0, addTrophy4, itemData.isRecord);
                    toastManager.ToastHint(itemData.spRewardIcon, string.Format(GameCommonInfo.GetUITextById(6099), string.Format(GameCommonInfo.GetUITextById(6009), addTrophy4 + "")));
                    break;
                case RewardTypeEnum.AddItems:
                case RewardTypeEnum.RandomAddItems:
                    gameData.AddItemsNumber(itemData.rewardId, itemData.rewardNumber);
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
                case RewardTypeEnum.AddChefExp:
                    CompleteRewardForExp(listCharacterData, WorkerEnum.Chef, itemData.rewardNumber);
                    break;
                case RewardTypeEnum.AddWaiterExp:
                    CompleteRewardForExp(listCharacterData, WorkerEnum.Waiter, itemData.rewardNumber);
                    break;
                case RewardTypeEnum.AddAccountantExp:
                    CompleteRewardForExp(listCharacterData, WorkerEnum.Accountant, itemData.rewardNumber);
                    break;
                case RewardTypeEnum.AddAccostExp:
                    CompleteRewardForExp(listCharacterData, WorkerEnum.Accost, itemData.rewardNumber);
                    break;
                case RewardTypeEnum.AddBeaterExp:
                    CompleteRewardForExp(listCharacterData, WorkerEnum.Beater, itemData.rewardNumber);
                    break;
            }
        }
    }

    protected static void CompleteRewardForExp(List<CharacterBean> listCharacterData,WorkerEnum workerType,int exp)
    {
        if (CheckUtil.ListIsNull(listCharacterData))
            return;

        foreach (CharacterBean itemData in listCharacterData)
        {
            CharacterWorkerBaseBean characterWorker =  itemData.baseInfo.GetWorkerInfoByType(workerType);
            characterWorker.AddExp(exp,out bool isLevelUp);
        }
    }

    /// <summary>
    /// 根据层数获取奖励
    /// </summary>
    /// <param name="layer"></param>
    /// <param name="normalBuildRate"></param>
    /// <returns></returns>
    public static List<RewardTypeBean> GetRewardItemsForInfiniteTowers(int layer, int totalLucky)
    {
        List<RewardTypeBean> listReward = new List<RewardTypeBean>();
        long addExp = 0;
        long addMoneyS = 0;

        //获取稀有物品概率
        float normalBuildRate = 0.25f + 0.0025f * (totalLucky / 3f);
        float rateRate = 0.05f + 0.0005f * totalLucky;

        if (layer % 10 == 0)
        {
            //添加经验奖励
            addExp = layer * 10;
            //添加金钱奖励
            addMoneyS = layer * 100;
            //BOSS奖励
            string rewardForNormalStr = "";
            string rewardForRareStr = "";
            string rewardForNormalBuildStr = "";
            string rewardForRareBuildStr = "";
            switch (layer)
            {
                case 10:
                case 20:
                case 30:
                    GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.InfiniteTowersNormalRewardForLevel1, out rewardForNormalStr);
                    GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.InfiniteTowersRareRewardForLevel1, out rewardForRareStr);
                    GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.InfiniteTowersNormalBuildRewardForLevel1, out rewardForNormalBuildStr);
                    GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.InfiniteTowersRareBuildRewardForLevel1, out rewardForRareBuildStr);
                    break;
                case 40:
                case 50:
                case 60:
                    GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.InfiniteTowersNormalRewardForLevel2, out rewardForNormalStr);
                    GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.InfiniteTowersRareRewardForLevel2, out rewardForRareStr);
                    GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.InfiniteTowersNormalBuildRewardForLevel2, out rewardForNormalBuildStr);
                    GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.InfiniteTowersRareBuildRewardForLevel2, out rewardForRareBuildStr);
                    break;
                case 70:
                case 80:
                case 90:
                    GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.InfiniteTowersNormalRewardForLevel3, out rewardForNormalStr);
                    GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.InfiniteTowersRareRewardForLevel3, out rewardForRareStr);
                    GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.InfiniteTowersNormalBuildRewardForLevel3, out rewardForNormalBuildStr);
                    GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.InfiniteTowersRareBuildRewardForLevel3, out rewardForRareBuildStr);
                    break;
                default:
                    GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.InfiniteTowersNormalRewardForLevel4, out rewardForNormalStr);
                    GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.InfiniteTowersRareRewardForLevel4, out rewardForRareStr);
                    GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.InfiniteTowersNormalBuildRewardForLevel4, out rewardForNormalBuildStr);
                    GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.InfiniteTowersRareBuildRewardForLevel4, out rewardForRareBuildStr);
                    break;
            }
            if (CheckUtil.StringIsNull(rewardForNormalStr))
            {
                //必定随机获得一个物品
                RewardTypeBean rewardForItems = GetRandomRewardForData(RewardTypeEnum.AddItems, rewardForNormalStr);
                listReward.Add(rewardForItems);
                //有一定概率获得建筑物
                float randomTemp = UnityEngine.Random.Range(0f, 1f);
                if (!CheckUtil.StringIsNull(rewardForNormalBuildStr) && randomTemp <= normalBuildRate)
                {
                    RewardTypeBean rewardForBuild = GetRandomRewardForData(RewardTypeEnum.AddBuildItems, rewardForNormalBuildStr);
                    listReward.Add(rewardForBuild);
                }
            }
            if (CheckUtil.StringIsNull(rewardForRareStr))
            {
                //有一定概率获得稀有物品
                float randomTemp = UnityEngine.Random.Range(0f, 1f);
                if (randomTemp <= rateRate)
                {
                    RewardTypeBean rewardForItems = GetRandomRewardForData(RewardTypeEnum.AddItems, rewardForRareStr);
                    listReward.Add(rewardForItems);
                }
            }
        }
        else
        {
            //添加经验奖励
            addExp = layer;
            //添加金钱奖励
            addMoneyS = layer * 10;
        }

        RewardTypeBean rewardForExp = new RewardTypeBean(RewardTypeEnum.AddBeaterExp, addExp + "");
        listReward.Add(rewardForExp);
        RewardTypeBean rewardForMoneyS = new RewardTypeBean(RewardTypeEnum.AddMoneyS, addMoneyS + "");
        listReward.Add(rewardForMoneyS);

        return listReward;
    }

    /// <summary>
    /// 根据运气获取敌人装备
    /// </summary>
    /// <returns></returns>
    public static List<RewardTypeBean> GetRewardEnemyEquipForInfiniteTowers(List<CharacterBean> listEnemyData, int layer, int totalLucky)
    {
        List<RewardTypeBean> listReward = new List<RewardTypeBean>();
        if (layer % 10 == 0)
        {
            float getRate = 0.1f + 0.001f * totalLucky;
            float randomRate = UnityEngine.Random.Range(0f, 1f);
            if (randomRate <= getRate)
            {
                CharacterBean randomEnemy = RandomUtil.GetRandomDataByList(listEnemyData);
                if (randomEnemy.equips.hatId != 0)
                {
                    listReward.Add(new RewardTypeBean(RewardTypeEnum.AddItems, randomEnemy.equips.hatId + ""));
                }
                if (randomEnemy.equips.clothesId != 0)
                {
                    listReward.Add(new RewardTypeBean(RewardTypeEnum.AddItems, randomEnemy.equips.clothesId + ""));
                }
                if (randomEnemy.equips.shoesId != 0)
                {
                    listReward.Add(new RewardTypeBean(RewardTypeEnum.AddItems, randomEnemy.equips.shoesId + ""));
                }
            }
        }
        return listReward;
    }

    protected static RewardTypeBean GetRandomRewardForData(RewardTypeEnum rewardType, string rewardListStr)
    {
        if (CheckUtil.StringIsNull(rewardListStr))
        {
            return null;
        }
        List<string> listReward = StringUtil.SplitBySubstringForListStr(rewardListStr, '|');
        string randomReward = RandomUtil.GetRandomDataByList(listReward);
        RewardTypeBean rewardData = new RewardTypeBean(rewardType, randomReward + "");
        return rewardData;
    }
}