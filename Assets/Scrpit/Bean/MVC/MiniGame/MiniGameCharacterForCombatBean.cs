using UnityEngine;
using UnityEditor;

public class MiniGameCharacterForCombatBean : MiniGameCharacterBean
{
    //是否防御状态
    public bool combatIsDefend;

    //战斗数据初始化
    public void CombatInit()
    {
        combatIsDefend = false;
    }

}