using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneInfiniteTowersManager : BaseMonoBehaviour
{
    public Transform tfLayerForNormal_1;
    public Transform tfLayerForBoss_1;

    public List<SignForInfiniteTowersCpt> listSignForLayer = new List<SignForInfiniteTowersCpt>();

    /// <summary>
    /// 设置层数
    /// </summary>
    /// <param name="layer"></param>
    public void SetSignForLayer(long layer)
    {
        foreach (SignForInfiniteTowersCpt itemSign in listSignForLayer)
        {
            itemSign.SetData(layer + GameCommonInfo.GetUITextById(83));
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

    /// <summary>
    /// 根据层数获取战斗地点
    /// </summary>
    /// <param name="layer"></param>
    /// <returns></returns>
    public Vector3 GetCombatPostionByLayer(long layer)
    {
        if (layer % 10 == 0)
        {
            return GetBossCombatPosition();
        }
        else
        {
            return GetNormalCombatPosition();
        }
    }

    /// <summary>
    /// 根据层数获取敌人能力
    /// </summary>
    /// <param name="layer"></param>
    /// <returns></returns>
    public CharacterAttributesBean GetEnemyAttributesByLayer(int layer)
    {
        CharacterAttributesBean characterAttributes = new CharacterAttributesBean();
        int baseAttributes = layer + 4;
        characterAttributes.InitAttributes(
            baseAttributes * 10,
            baseAttributes,
            baseAttributes + Random.Range(-1, 2),
            baseAttributes,
            baseAttributes,
            baseAttributes,
            baseAttributes);
        return characterAttributes;
    }
}