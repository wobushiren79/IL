using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MiniGameCombatEffectBean 
{
    //效果类型
    public EffectTypeEnum effectType;
    //图标
    public Sprite spIcon;
    //效果大小
    public float effectData;
    //持续回合
    public int durationForRound;
    //效果特效名字
    public string effectPSName;
    //图标备注Id
    public string iconMarkId;
    public void CompleteEffect(MiniGameCharacterForCombatBean miniGameCharacter,NpcAIMiniGameCombatCpt npcCpt)
    {
        switch (effectType)
        {
            case EffectTypeEnum.AddLife:
                miniGameCharacter.characterCurrentLife += (int)effectData;
                if (miniGameCharacter.characterCurrentLife> miniGameCharacter.characterMaxLife)
                {
                    miniGameCharacter.characterCurrentLife = miniGameCharacter.characterMaxLife;
                }
                //显示增加的血量
                npcCpt.ShowTextInfo("+" + (int)effectData,Color.green);
                //更新血量显示
                npcCpt.characterLifeCpt.SetData(miniGameCharacter.characterCurrentLife, miniGameCharacter.characterMaxLife);
                break;
        }
        durationForRound--;
    }
}