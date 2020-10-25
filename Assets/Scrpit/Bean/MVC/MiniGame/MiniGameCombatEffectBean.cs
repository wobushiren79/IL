﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MiniGameCombatEffectBean
{
    public List<EffectTypeBean> listEffectTypeData;
    //持续回合
    public int durationForRound;
    //效果特效名字
    public string effectPSName;
    //图标备注Id
    public string iconMarkId;
    //技能释放人
    public NpcAIMiniGameCombatCpt actionCharacter;
    //技能受益人
    public NpcAIMiniGameCombatCpt targetCharacter;

    public void CompleteEffect(GameItemsManager gameItemsManager, AudioHandler audioHandler, MiniGameCharacterForCombatBean miniGameCharacter)
    {
        //回复生命处理
        int addLife = EffectTypeEnumTools.GetTotalLife(gameItemsManager, targetCharacter.characterData, listEffectTypeData);
        if (addLife > 0)
        {
            //显示增加的血量
            targetCharacter.ShowTextInfo("+" + addLife, Color.green);
            //更新血量显示
            targetCharacter.characterLifeCpt.SetData(miniGameCharacter.characterCurrentLife, miniGameCharacter.characterMaxLife);
        }
        //伤害处理
        int addDamage = EffectTypeEnumTools.GetTotalDamage(gameItemsManager, actionCharacter.characterData, listEffectTypeData);
        if (addDamage > 0)
        {
            targetCharacter.UnderAttack(1, addDamage);
        }
    }

    /// <summary>
    /// 检测是否需要延迟
    /// </summary>
    /// <returns></returns>
    public bool CheckIsDelay()
    {
        if (!CheckUtil.ListIsNull(listEffectTypeData))
        {
            for (int i = 0; i < listEffectTypeData.Count; i++)
            {
                EffectTypeBean itemData = listEffectTypeData[i];
                bool isDelay = EffectTypeEnumTools.CheckIsDelay(itemData);
                if (isDelay)
                    return true;
            }
        }
        return false;
    }

}