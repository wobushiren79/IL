using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MiniGameCombatBean : MiniGameBaseBean
{
    //回合行动指令
    protected MiniGameCombatCommand roundCombatCommand = MiniGameCombatCommand.None;
    //回合的行动角色数据
    protected NpcAIMiniGameCombatCpt roundActionCharacter;
    //回合对象角色数据
    protected List<NpcAIMiniGameCombatCpt> roundListTargetCharacter;
    //力度测试
    protected float powerTest;

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
    /// 获取回合目标角色
    /// </summary>
    /// <returns></returns>
    public NpcAIMiniGameCombatCpt GetRoundTargetCharacter()
    {
        return roundListTargetCharacter[0];
    }
    public List<NpcAIMiniGameCombatCpt> GetRoundTargetListCharacter()
    {
        return roundListTargetCharacter;
    }


    /// <summary>
    /// 设置回合行动指令
    /// </summary>
    /// <param name="roundCombatCommand"></param>
    public void SetRoundActionCommand(MiniGameCombatCommand roundCombatCommand)
    {
        this.roundCombatCommand = roundCombatCommand;
    }
    /// <summary>
    /// 返回回合行动指令
    /// </summary>
    /// <param name="roundCombatCommand"></param>
    public MiniGameCombatCommand GetRoundActionCommand()
    {
        return roundCombatCommand;
    }

    /// <summary>
    /// 设置测试
    /// </summary>
    /// <param name="powerTest"></param>
    public void SetPowerTest(float powerTest)
    {
        this.powerTest = powerTest;
    }

    /// <summary>
    /// 获取力量测试
    /// </summary>
    /// <returns></returns>
    public float GetPowerTest()
    {
        return powerTest;
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