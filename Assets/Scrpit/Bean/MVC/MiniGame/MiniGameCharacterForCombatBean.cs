using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MiniGameCharacterForCombatBean : MiniGameCharacterBean
{
    //战斗效果加成或负面
    public List<EffectTypeBean> listCombatEffect = new List<EffectTypeBean>();

    //战斗效果执行
    public void CombatEffectExecute()
    {

    }

}