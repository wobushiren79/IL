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

    WinSurvivalTime = 21,
    WinLife = 22,
}

public class PreTypeForMiniGameBean
{
    public PreTypeForMiniGameEnum preType;
    public string preData;


    public PreTypeForMiniGameBean(PreTypeForMiniGameEnum preType, string preData)
    {
        this.preType = preType;
        this.preData = preData;
    }
}

public class PreTypeForMiniGameEnumTools
{
    /// <summary>
    /// 获取前置条件
    /// </summary>
    /// <returns></returns>
    public static List<PreTypeForMiniGameBean> GetListPreData(string data)
    {
        List<PreTypeForMiniGameBean> listPreData = new List<PreTypeForMiniGameBean>();
        List<string> listData = StringUtil.SplitBySubstringForListStr(data, '|');
        foreach (string itemData in listData)
        {
            if (CheckUtil.StringIsNull(itemData))
                continue;
            List<string> itemListData = StringUtil.SplitBySubstringForListStr(itemData, ':');
            PreTypeForMiniGameEnum preType = EnumUtil.GetEnum<PreTypeForMiniGameEnum>(itemListData[0]);
            string preValue = itemListData[1];
            listPreData.Add(new PreTypeForMiniGameBean(preType, preValue));
        }
        return listPreData;
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
            if (itemData.preType == PreTypeForMiniGameEnum.PlayerNumber)
            {
                playNumber = int.Parse(itemData.preData);
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
            if (itemData.preType == PreTypeForMiniGameEnum.MiniGameType)
            {
                miniGameType = (MiniGameEnum)int.Parse(itemData.preData);
                return;
            }
        }
    }

    /// <summary>
    /// 获取游戏数据
    /// </summary>
    public static MiniGameBaseBean GetMiniGameData(MiniGameBaseBean miniGameData, string data, GameItemsManager gameItemsManager, NpcInfoManager npcInfoManager)
    {
        return GetMiniGameData(miniGameData, data, null, gameItemsManager, npcInfoManager);
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
            switch (itemPreData.preType)
            {
                case PreTypeForMiniGameEnum.MiniGameType:
                    MiniGameEnum miniGameType = (MiniGameEnum)int.Parse(itemPreData.preData);
                    miniGameData.gameType = miniGameType;
                    break;
                case PreTypeForMiniGameEnum.UserIds:
                    long[] userIds = StringUtil.SplitBySubstringForArrayLong(itemPreData.preData, ',');
                    listUserData = npcInfoManager.GetCharacterDataByIds(userIds);
                    break;
                case PreTypeForMiniGameEnum.EnemyIds:
                    long[] enemyIds = StringUtil.SplitBySubstringForArrayLong(itemPreData.preData, ',');
                    listEnemyData = npcInfoManager.GetCharacterDataByIds(enemyIds);
                    break;
                case PreTypeForMiniGameEnum.MiniGamePosition:
                    float[] arrayPosition = StringUtil.SplitBySubstringForArrayFloat(itemPreData.preData, ',');
                    minigamePosition = new Vector2(arrayPosition[0], arrayPosition[1]);
                    break;
                case PreTypeForMiniGameEnum.TalkMarkIdForWin:
                    miniGameData.gameResultWinTalkMarkId = long.Parse(itemPreData.preData);
                    break;
                case PreTypeForMiniGameEnum.TalkMarkIdForLose:
                    miniGameData.gameResultLoseTalkMarkId = long.Parse(itemPreData.preData);
                    break;
                case PreTypeForMiniGameEnum.WinLife:
                    miniGameData.winLife = long.Parse(itemPreData.preData);
                    break;
                case PreTypeForMiniGameEnum.WinSurvivalTime:
                    miniGameData.winSurvivalTime = float.Parse(itemPreData.preData);
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

}

