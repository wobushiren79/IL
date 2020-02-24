using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class GameDataSimpleBean 
{
    public string userId;//用户ID
    public long moneyL;//用户金钱
    public long moneyS;
    public long moneyM;
    public long guildCoin;
    public string innName;//客栈名字
    public CharacterBean userCharacter;//用户角色
    public TimeBean gameTime;//游戏时间
    public TimeBean playTime;//游玩时间

    /// <summary>
    /// 获取数据
    /// </summary>
    /// <param name="gameData"></param>
    /// <returns></returns>
    public static GameDataSimpleBean ToSimpleData(GameDataBean gameData)
    {
        if (gameData == null)
            return null;
        GameDataSimpleBean gameDataSimple = new GameDataSimpleBean();
        gameDataSimple.userId = gameData.userId;
        gameDataSimple.innName = gameData.innAttributes.innName;
        gameDataSimple.moneyL= gameData.moneyL;
        gameDataSimple.moneyS = gameData.moneyS;
        gameDataSimple.moneyM = gameData.moneyM;
        gameDataSimple.gameTime = gameData.gameTime;
        gameDataSimple.userCharacter = gameData.userCharacter;
        gameDataSimple.guildCoin = gameData.guildCoin;
        gameDataSimple.playTime = gameData.playTime;
        return gameDataSimple;
    }
}