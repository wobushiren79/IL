using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MiniGameCombatEffectBean 
{
    public EffectTypeBean effectTypeData;
    //持续回合
    public int durationForRound;
    //效果特效名字
    public string effectPSName;
    //图标备注Id
    public string iconMarkId;

    public void CompleteEffect(MiniGameCharacterForCombatBean miniGameCharacter,NpcAIMiniGameCombatCpt npcCpt)
    {
        switch (effectTypeData.dataType)
        {
            case EffectTypeEnum.AddLife:
                miniGameCharacter.characterCurrentLife += (int)effectTypeData.effectData;
                if (miniGameCharacter.characterCurrentLife> miniGameCharacter.characterMaxLife)
                {
                    miniGameCharacter.characterCurrentLife = miniGameCharacter.characterMaxLife;
                }
                //显示增加的血量
                npcCpt.ShowTextInfo("+" + (int)effectTypeData.effectData, Color.green);
                //更新血量显示
                npcCpt.characterLifeCpt.SetData(miniGameCharacter.characterCurrentLife, miniGameCharacter.characterMaxLife);
                break;
            case EffectTypeEnum.Damage:
                float damageRate =  miniGameCharacter.GetEffectDamageRate((int)effectTypeData.effectData);
                break;
        }
        durationForRound--;
    }


}