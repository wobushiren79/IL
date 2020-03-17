using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MiniGameCharacterForCombatBean : MiniGameCharacterBean
{
    //战斗效果加成或负面
    public List<MiniGameCombatEffectBean> listCombatEffect = new List<MiniGameCombatEffectBean>();

}