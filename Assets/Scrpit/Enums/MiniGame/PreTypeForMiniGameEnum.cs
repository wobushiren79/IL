using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public enum PreTypeForMiniGameEnum
{

    MiniGameType = 1,
    PlayerNumber = 2,
    UserIds = 3,
    EnemyIds = 4,
    MiniGamePosition = 5,
    TalkMarkIdForWin = 6,
    TalkMarkIdForLose = 7,
    GameReason = 8,

    WinSurvivalTime = 21,
    WinLife = 22,

    BarrageForLaunchSpeed = 101,
    BarrageForLaunchTypes = 102,
    BarrageForLaunchInterval = 103,
    BarrageForLaunchNumber = 104,

    AccountForWinMoneyL = 201,
    AccountForWinMoneyM = 202,
    AccountForWinMoneyS = 203,

    CombatForSurvivalNumber = 301,
    CombatForBringDownNumber = 302,

    CookingForScore = 401,
    CookingForStoryStartId = 402,
    CookingForStoryAuditId = 403,
    CookingForAuditCharacter = 404,
    CookingForCompereCharacter=405,
}

public class PreTypeForMiniGameBean : DataBean<PreTypeForMiniGameEnum>
{
    public PreTypeForMiniGameBean() : base(PreTypeForMiniGameEnum.MiniGameType, "")
    {
    }
}

public class PreTypeForMiniGameEnumTools : DataTools
{
    /// <summary>
    /// 获取前置条件
    /// </summary>
    /// <returns></returns>
    public static List<PreTypeForMiniGameBean> GetListPreData(string data)
    {
        return GetListData<PreTypeForMiniGameBean, PreTypeForMiniGameEnum>(data);
    }

    /// <summary>
    /// 获取玩家人数
    /// </summary>
    /// <param name="data"></param>
    /// <param name="playNumber"></param>
    public static void GetPlayerNumber(string data, out int playNumber)
    {
        playNumber = 0;
        List<PreTypeForMiniGameBean> listPreData = GetListPreData(data);
        foreach (PreTypeForMiniGameBean itemData in listPreData)
        {
            if (itemData.dataType == PreTypeForMiniGameEnum.PlayerNumber)
            {
                playNumber = int.Parse(itemData.data);
                return;
            }
        }
    }

    /// <summary>
    /// 获取游戏类型
    /// </summary>
    /// <param name="data"></param>
    /// <param name="miniGameType"></param>
    public static void GetMiniGameType(string data, out MiniGameEnum miniGameType)
    {
        miniGameType = MiniGameEnum.Combat;
        List<PreTypeForMiniGameBean> listPreData = GetListPreData(data);
        foreach (PreTypeForMiniGameBean itemData in listPreData)
        {
            if (itemData.dataType == PreTypeForMiniGameEnum.MiniGameType)
            {
                miniGameType = (MiniGameEnum)int.Parse(itemData.data);
                return;
            }
        }
    }

    /// <summary>
    /// 获取游戏数据
    /// </summary>
    public static MiniGameBaseBean GetMiniGameData(MiniGameBaseBean miniGameData, string data, GameItemsManager gameItemsManager, NpcInfoManager npcInfoManager)
    {
        return GetMiniGameData(miniGameData, data, new List<CharacterBean> { }, gameItemsManager, npcInfoManager);
    }
    public static MiniGameBaseBean GetMiniGameData(MiniGameBaseBean miniGameData, string data, CharacterBean userCharacter, GameItemsManager gameItemsManager, NpcInfoManager npcInfoManager)
    {
        return GetMiniGameData(miniGameData, data, new List<CharacterBean> { userCharacter }, gameItemsManager, npcInfoManager);
    }
    public static MiniGameBaseBean GetMiniGameData(MiniGameBaseBean miniGameData, string data, List<CharacterBean> listPickCharacter, GameItemsManager gameItemsManager, NpcInfoManager npcInfoManager)
    {
        List<PreTypeForMiniGameBean> listPreData = GetListPreData(data);
        List<CharacterBean> listUserData = new List<CharacterBean>();
        List<CharacterBean> listEnemyData = new List<CharacterBean>();
        Vector2 minigamePosition = Vector2.zero;
        //如果没有传入游戏数据则先根据条件生成一个
        if (miniGameData == null)
        {
            GetMiniGameType(data, out MiniGameEnum miniGameType);
            miniGameData = MiniGameEnumTools.GetMiniGameData(miniGameType);
        }

        foreach (PreTypeForMiniGameBean itemPreData in listPreData)
        {
            switch (itemPreData.dataType)
            {
                case PreTypeForMiniGameEnum.MiniGameType:
                    MiniGameEnum miniGameType = (MiniGameEnum)int.Parse(itemPreData.data);
                    miniGameData.gameType = miniGameType;
                    break;
                case PreTypeForMiniGameEnum.GameReason:
                    MiniGameReasonEnum miniGameReason = (MiniGameReasonEnum)int.Parse(itemPreData.data);
                    miniGameData.gameReason = miniGameReason;
                    break;
                case PreTypeForMiniGameEnum.UserIds:
                    long[] userIds = StringUtil.SplitBySubstringForArrayLong(itemPreData.data, ',');
                    listUserData = npcInfoManager.GetCharacterDataByIds(userIds);
                    break;
                case PreTypeForMiniGameEnum.EnemyIds:
                    long[] enemyIds = StringUtil.SplitBySubstringForArrayLong(itemPreData.data, ',');
                    listEnemyData = npcInfoManager.GetCharacterDataByIds(enemyIds);
                    break;
                case PreTypeForMiniGameEnum.MiniGamePosition:
                    float[] arrayPosition = StringUtil.SplitBySubstringForArrayFloat(itemPreData.data, ',');
                    minigamePosition = new Vector2(arrayPosition[0], arrayPosition[1]);
                    break;
                case PreTypeForMiniGameEnum.TalkMarkIdForWin:
                    miniGameData.gameResultWinTalkMarkId = long.Parse(itemPreData.data);
                    break;
                case PreTypeForMiniGameEnum.TalkMarkIdForLose:
                    miniGameData.gameResultLoseTalkMarkId = long.Parse(itemPreData.data);
                    break;
                case PreTypeForMiniGameEnum.WinLife:
                    miniGameData.winLife = long.Parse(itemPreData.data);
                    break;
                case PreTypeForMiniGameEnum.WinSurvivalTime:
                    miniGameData.winSurvivalTime = float.Parse(itemPreData.data);
                    break;
                case PreTypeForMiniGameEnum.BarrageForLaunchInterval:
                case PreTypeForMiniGameEnum.BarrageForLaunchSpeed:
                case PreTypeForMiniGameEnum.BarrageForLaunchTypes:
                case PreTypeForMiniGameEnum.BarrageForLaunchNumber:
                    GetMiniGameDataForBarrage(itemPreData, miniGameData);
                    break;
                case PreTypeForMiniGameEnum.AccountForWinMoneyL:
                case PreTypeForMiniGameEnum.AccountForWinMoneyM:
                case PreTypeForMiniGameEnum.AccountForWinMoneyS:
                    GetMiniGameDataForAccount(itemPreData, miniGameData);
                    break;
                case PreTypeForMiniGameEnum.CombatForBringDownNumber:
                case PreTypeForMiniGameEnum.CombatForSurvivalNumber:
                    GetMiniGameDataForCombat(itemPreData, miniGameData);
                    break;
                case PreTypeForMiniGameEnum.CookingForScore:
                case PreTypeForMiniGameEnum.CookingForStoryStartId:
                case PreTypeForMiniGameEnum.CookingForStoryAuditId:
                case PreTypeForMiniGameEnum.CookingForAuditCharacter:
                case PreTypeForMiniGameEnum.CookingForCompereCharacter:
                    GetMiniGameDataForCook(gameItemsManager, npcInfoManager,itemPreData, miniGameData);
                    break;
            }
        }
        if (miniGameData == null)
            return miniGameData;
        //如果有传入角色则把传入角色也加入到用户数据中
        if (listPickCharacter != null)
        {
            listUserData.AddRange(listPickCharacter);
        }
        miniGameData.miniGamePosition = minigamePosition;
        miniGameData.InitData(gameItemsManager, listUserData, listEnemyData);
        return miniGameData;
    }

    /// <summary>
    /// 获取弹幕游戏数据
    /// </summary>
    /// <param name="itemPreData"></param>
    /// <param name="miniGameData"></param>
    private static void GetMiniGameDataForBarrage(PreTypeForMiniGameBean itemPreData, MiniGameBaseBean miniGameData)
    {
        if (miniGameData.gameType != MiniGameEnum.Barrage)
            return;
        MiniGameBarrageBean miniGameBarrage = (MiniGameBarrageBean)miniGameData;
        switch (itemPreData.dataType)
        {
            case PreTypeForMiniGameEnum.BarrageForLaunchInterval:
                miniGameBarrage.launchInterval = float.Parse(itemPreData.data);
                break;
            case PreTypeForMiniGameEnum.BarrageForLaunchSpeed:
                miniGameBarrage.launchSpeed = float.Parse(itemPreData.data);
                break;
            case PreTypeForMiniGameEnum.BarrageForLaunchTypes:
                MiniGameBarrageEjectorCpt.LaunchTypeEnum[] launchTypes = StringUtil.SplitBySubstringForArrayEnum<MiniGameBarrageEjectorCpt.LaunchTypeEnum>(itemPreData.data, ',');
                miniGameBarrage.launchTypes = launchTypes;
                break;
            case PreTypeForMiniGameEnum.BarrageForLaunchNumber:
                miniGameBarrage.launchNumber = int.Parse(itemPreData.data);
                break;
        }
    }

    /// <summary>
    /// 获取算账游戏游戏数据
    /// </summary>
    /// <param name="itemPreData"></param>
    /// <param name="miniGameData"></param>
    private static void GetMiniGameDataForAccount(PreTypeForMiniGameBean itemPreData, MiniGameBaseBean miniGameData)
    {
        if (miniGameData.gameType != MiniGameEnum.Account)
            return;
        MiniGameAccountBean miniGameAccount = (MiniGameAccountBean)miniGameData;
        switch (itemPreData.dataType)
        {
            case PreTypeForMiniGameEnum.AccountForWinMoneyL:
                miniGameData.winMoneyL = int.Parse(itemPreData.data);
                break;
            case PreTypeForMiniGameEnum.AccountForWinMoneyM:
                miniGameData.winMoneyM = int.Parse(itemPreData.data);
                break;
            case PreTypeForMiniGameEnum.AccountForWinMoneyS:
                miniGameData.winMoneyS = int.Parse(itemPreData.data);
                break;
        }
    }

    /// <summary>
    /// 获取战斗游戏数据
    /// </summary>
    /// <param name="itemPreData"></param>
    /// <param name="miniGameData"></param>
    private static void GetMiniGameDataForCombat(PreTypeForMiniGameBean itemPreData, MiniGameBaseBean miniGameData)
    {
        if (miniGameData.gameType != MiniGameEnum.Combat)
            return;
        MiniGameCombatBean miniGameCombat = (MiniGameCombatBean)miniGameData;
        switch (itemPreData.dataType)
        {
            case PreTypeForMiniGameEnum.CombatForBringDownNumber:
                miniGameData.winBringDownNumber = int.Parse(itemPreData.data);
                break;
            case PreTypeForMiniGameEnum.CombatForSurvivalNumber:
                miniGameData.winSurvivalNumber = int.Parse(itemPreData.data);
                break;
        }
    }

    /// <summary>
    /// 获取烹饪游戏数据
    /// </summary>
    /// <param name="itemPreData"></param>
    /// <param name="miniGameData"></param>
    private static void GetMiniGameDataForCook(GameItemsManager gameItemsManager, NpcInfoManager npcInfoManager, PreTypeForMiniGameBean itemPreData, MiniGameBaseBean miniGameData)
    {
        if (miniGameData.gameType != MiniGameEnum.Cooking)
            return;
        MiniGameCookingBean miniGameCooking = (MiniGameCookingBean)miniGameData;
        //审核人员
        List<CharacterBean> listAuditData = new List<CharacterBean>();
        //主持人
        List<CharacterBean> listCompereData = new List<CharacterBean>();

        switch (itemPreData.dataType)
        {
            case PreTypeForMiniGameEnum.CookingForScore:
                miniGameData.winScore = int.Parse(itemPreData.data);
                break;
            case PreTypeForMiniGameEnum.CookingForStoryStartId:
                miniGameCooking.storyGameStartId = long.Parse(itemPreData.data);
                break;
            case PreTypeForMiniGameEnum.CookingForStoryAuditId:
                miniGameCooking.storyGameAuditId = long.Parse(itemPreData.data);
                break;
            case PreTypeForMiniGameEnum.CookingForAuditCharacter:
                long[] auditIds=   StringUtil.SplitBySubstringForArrayLong(itemPreData.data,',');
                listAuditData = npcInfoManager.GetCharacterDataByIds(auditIds);
                //评审人员只有5位
                listAuditData = RandomUtil.GetRandomDataByListForNumberNR(listAuditData, 5);
                //如果评审人员不够 就随机增加小镇人员
                if (listAuditData.Count < 5)
                {
                    int tempNumber = 5 - listAuditData.Count;
                    List<CharacterBean> listTempCharacterData = npcInfoManager.GetCharacterDataByType(NpcTypeEnum.Town);
                    listAuditData.AddRange(RandomUtil.GetRandomDataByListForNumberNR(listTempCharacterData, tempNumber));
                }
                break;
            case PreTypeForMiniGameEnum.CookingForCompereCharacter:
                long[] compereIds = StringUtil.SplitBySubstringForArrayLong(itemPreData.data, ',');
                listCompereData = npcInfoManager.GetCharacterDataByIds(compereIds);
                break;
        }

        miniGameCooking.InitData(gameItemsManager, null, null, listAuditData, listCompereData);
    }
}

