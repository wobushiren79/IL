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
    /// 获取所有伤害减免
    /// </summary>
    /// <returns></returns>
    public int GetTotalDef(GameItemsManager gameItemsManager, int damage)
    {
        foreach (MiniGameCombatEffectBean itemData in listCombatEffect)
        {
            damage = EffectTypeEnumTools.GetTotalDef(gameItemsManager, characterData, itemData.listEffectTypeData, damage);
        }
        return damage;
    }

    /// <summary>
    /// 获取所有属性加成
    /// </summary>
    /// <param name="force"></param>
    /// <returns></returns>
    public void GetTotalAttributes(CharacterAttributesBean  addAttributesData)
    {
        foreach (MiniGameCombatEffectBean itemData in listCombatEffect)
        {
            EffectTypeEnumTools.GetTotalAttributes(itemData.listEffectTypeData, addAttributesData);
        }
    }

}