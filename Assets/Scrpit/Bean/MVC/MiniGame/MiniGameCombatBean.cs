using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MiniGameCombatBean : MiniGameBaseBean
{
    //回合的行动角色数据
    protected NpcAIMiniGameCombatCpt roundActionCharacter;
    //回合对象角色数据
    protected List<NpcAIMiniGameCombatCpt> roundListTargetCharacter;

    public enum MiniGameCombatStatusEnum
    {
        Rounding = 1,//回合进行中
        OurRound = 2,//我方回合
        EnemyRound = 3,//敌方回合
    }
    //战斗状态
    protected MiniGameCombatStatusEnum combatStatus = MiniGameCombatStatusEnum.Rounding;

    public MiniGameCombatBean()
    {
        gameType = MiniGameEnum.Combat;
    }

    /// <summary>
    /// 获取战斗状态
    /// </summary>
    /// <returns></returns>
    public MiniGameCombatStatusEnum GetCombatStatus()
    {
        return combatStatus;
    }

    /// <summary>
    /// 设置战斗状态
    /// </summary>
    /// <param name="miniGameCombatStatus"></param>
    public void SetCombatStatus(MiniGameCombatStatusEnum miniGameCombatStatus)
    {
        combatStatus = miniGameCombatStatus;
    }

    /// <summary>
    /// 设置回合行动角色
    /// </summary>
    /// <param name="roundActionCharacter"></param>
    public void SetRoundActionCharacter(NpcAIMiniGameCombatCpt roundActionCharacter)
    {
        this.roundActionCharacter = roundActionCharacter;
    }

    /// <summary>
    /// 获取回合行动角色
    /// </summary>
    /// <returns></returns>
    public NpcAIMiniGameCombatCpt GetRoundActionCharacter()
    {
        return roundActionCharacter;
    }

    /// <summary>
    /// 设置回合目标角色
    /// </summary>
    /// <param name="roundActionCharacter"></param>
    public void SetRoundTargetCharacter(NpcAIMiniGameCombatCpt roundActionCharacter)
    {
        SetRoundTargetCharacter(new List<NpcAIMiniGameCombatCpt>() { roundActionCharacter });
    }
    public void SetRoundTargetCharacter(List<NpcAIMiniGameCombatCpt> roundActionCharacter)
    {
        if (roundListTargetCharacter == null)
            roundListTargetCharacter = new List<NpcAIMiniGameCombatCpt>();
        roundListTargetCharacter.AddRange(roundActionCharacter);
    }

    /// <summary>
    /// 结束回合
    /// </summary>
    public void EndRound()
    {
        roundActionCharacter = null;
        roundListTargetCharacter = null;
    }
}