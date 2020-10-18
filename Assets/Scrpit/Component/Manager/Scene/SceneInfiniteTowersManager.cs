using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneInfiniteTowersManager : BaseMonoBehaviour
{
    public Transform tfLayerForNormal_1;
    public Transform tfLayerForBoss_1;

    public List<SignForInfiniteTowersCpt>  listSignForLayer = new List<SignForInfiniteTowersCpt>();

    /// <summary>
    /// 设置层数
    /// </summary>
    /// <param name="layer"></param>
    public void SetSignForLayer(long layer)
    {
        foreach (SignForInfiniteTowersCpt itemSign in listSignForLayer)
        {
            itemSign.SetData(layer+"");
        }
    }

    /// <summary>
    /// 获取普通战斗地点
    /// </summary>
    /// <returns></returns>
    public Vector3 GetNormalCombatPosition()
    {
        return tfLayerForNormal_1.position;
    }

    /// <summary>
    /// 获取BOSS战斗地点
    /// </summary>
    /// <returns></returns>
    public Vector3 GetBossCombatPosition()
    {
        return tfLayerForBoss_1.position;
    }
}