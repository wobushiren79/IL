using UnityEngine;
using UnityEditor;

public enum MiniGameEnum 
{
    Cooking=1,//烹饪游戏
    Barrage=2,//弹幕游戏
    Account = 3,//算账游戏
    Debate = 4,//辩论游戏 （斗魅）
    Combat =5,//战斗游戏
}

public class MiniGameEnumTools
{
    /// <summary>
    /// 获取基础游戏数据
    /// </summary>
    /// <param name="miniGameType"></param>
    /// <returns></returns>
    public static MiniGameBaseBean GetMiniGameData(MiniGameEnum miniGameType)
    {
        MiniGameBaseBean miniGameData = null;
        switch (miniGameType)
        {
            case MiniGameEnum.Cooking:
                miniGameData = new MiniGameCookingBean();
                break;
            case MiniGameEnum.Barrage:
                miniGameData = new MiniGameBarrageBean();
                break;
            case MiniGameEnum.Account:
                miniGameData = new MiniGameAccountBean();
                break;
            case MiniGameEnum.Debate:
                miniGameData = new MiniGameDebateBean();
                break;
            case MiniGameEnum.Combat:
                miniGameData = new MiniGameCombatBean();
                break;
        }
        return miniGameData;
    }
}