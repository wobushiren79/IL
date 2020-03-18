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
    //回合行动角色力度测试
    protected float roundActionPowerTest;
    //回合行动角色使用道具ID
    protected long roundActionItemsId = 0;
    //回合行动角色使用技能ID
    protected SkillInfoBean roundActionSkill ;

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
    public void SetRoundActionPowerTest(float powerTest)
    {
        this.roundActionPowerTest = powerTest;
    }

    /// <summary>
    /// 获取力量测试
    /// </summary>
    /// <returns></returns>
    public float GetRoundActionPowerTest()
    {
        return roundActionPowerTest;
    }

    /// <summary>
    /// 设置使用物品ID
    /// </summary>
    /// <param name="roundActionItemsId"></param>
    public void SetRoundActionItemsId(long roundActionItemsId)
    {
       this. roundActionItemsId = roundActionItemsId;
    }

    /// <summary>
    /// 获取使用物品ID
    /// </summary>
    /// <returns></returns>
    public long GetRoundActionItemsId()
    {
        return roundActionItemsId;
    }

    /// <summary>
    /// 设置技能ID
    /// </summary>
    /// <param name="roundActionSkillId"></param>
    public void SetRoundActionSkill(SkillInfoBean roundActionSkill)
    {
        this.roundActionSkill = roundActionSkill;
    }

    /// <summary>
    /// 获取技能ID
    /// </summary>
    /// <returns></returns>
    public SkillInfoBean GetRoundActionSkill()
    {
        return roundActionSkill;
    }

    /// <summary>
    /// 结束回合
    /// </summary>
    public void EndRound()
    {
        roundActionCharacter = null;
        roundListTargetCharacter = null;
        roundActionPowerTest = 0;
        roundActionItemsId = 0;
        roundActionSkill = null;
        roundCombatCommand = MiniGameCombatCommand.None;
    }
}