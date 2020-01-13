using UnityEngine;
using UnityEditor;

public enum MiniGameEnum 
{
    Cooking,//烹饪游戏
    Barrage,//弹幕游戏
    Combat,//战斗游戏
    Account,//算账游戏
    Debate,//辩论游戏 （斗魅）
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
            case MiniGameEnum.Account:
                miniGameData = new MiniGameAccountBean();
                break;
            case MiniGameEnum.Cooking:
                miniGameData = new MiniGameCookingBean();
                break;
            case MiniGameEnum.Combat:
                miniGameData = new MiniGameCombatBean();
                break;
            case MiniGameEnum.Barrage:
                miniGameData = new MiniGameBarrageBean();
                break;
            case MiniGameEnum.Debate:
                miniGameData = new MiniGameDebateBean();
                break;
        }
        return miniGameData;
    }
}