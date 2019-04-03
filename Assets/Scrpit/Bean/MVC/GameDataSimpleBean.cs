using UnityEngine;
using UnityEditor;

public class GameDataSimpleBean 
{
    public string userId;//用户ID
    public long money;//用户金钱
    public string innName;//客栈名字
    public CharacterBean userCharacter;//用户角色
    public TimeBean gameTime;//游戏时间

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
        gameDataSimple.innName = gameData.innName;
        gameDataSimple.money = gameData.money;
        gameDataSimple.gameTime = gameData.gameTime;
        gameDataSimple.userCharacter = gameData.userCharacter;
        return gameDataSimple;
    }
}