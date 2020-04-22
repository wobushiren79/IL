using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MiniGameCharacterForCombatBean : MiniGameCharacterBean
{
    //战斗效果加成或负面
    public List<MiniGameCombatEffectBean> listCombatEffect = new List<MiniGameCombatEffectBean>();
    //用户使用过的技能
    public Dictionary<long, int> listUsedSkill = new Dictionary<long, int>();

    /// <summary>
    /// 增加使用的技能
    /// </summary>
    /// <param name="id"></param>
    /// <param name="number"></param>
    public void AddUsedSkill(long id,int number)
    {
        if (listUsedSkill.TryGetValue(id,out int value))
        {
            listUsedSkill[id] += value;
        }
        else
        {
            listUsedSkill.Add(id, number);
        }
        if (listUsedSkill[id] <= 0)
        {
            listUsedSkill.Remove(id);
        }
    }

    /// <summary>
    /// 获取所有效果的伤害加成
    /// </summary>
    /// <returns></returns>
    public int GetEffectDamageRate(int damage)
    {
        List<EffectTypeBean> listEffect = new List<EffectTypeBean>();
        foreach (MiniGameCombatEffectBean itemData in listCombatEffect)
        {
            listEffect.Add(itemData.effectTypeData);
        }
        return EffectTypeEnumTools.GetEffectDamageRate(listEffect, damage);
    }

    /// <summary>
    /// 获取所有武力加成
    /// </summary>
    /// <param name="force"></param>
    /// <returns></returns>
    public int GetEffectForceRate(int force)
    {
        List<EffectTypeBean> listEffect = new List<EffectTypeBean>();
        foreach (MiniGameCombatEffectBean itemData in listCombatEffect)
        {
            listEffect.Add(itemData.effectTypeData);
        }
        return EffectTypeEnumTools.GetEffectForceRate(listEffect, force);
    }

    /// <summary>
    /// 获取所有速度加成
    /// </summary>
    /// <param name="speed"></param>
    /// <returns></returns>
    public int GetEffectSpeedRate(int speed)
    {
        List<EffectTypeBean> listEffect = new List<EffectTypeBean>();
        foreach (MiniGameCombatEffectBean itemData in listCombatEffect)
        {
            listEffect.Add(itemData.effectTypeData);
        }
        return EffectTypeEnumTools.GetEffectSpeedRate(listEffect, speed);
    }
}