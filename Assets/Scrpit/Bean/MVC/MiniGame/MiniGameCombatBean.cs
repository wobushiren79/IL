using UnityEngine;
using UnityEditor;

public class MiniGameCombatBean : MiniGameBaseBean
{
    //战斗地点
    public Vector3 combatPosition;

    public MiniGameCombatBean()
    {
        gameType = MiniGameEnum.Combat;
    }
}