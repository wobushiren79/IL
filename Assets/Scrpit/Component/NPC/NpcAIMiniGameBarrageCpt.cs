using UnityEngine;
using UnityEditor;

public class NpcAIMiniGameBarrageCpt : BaseNpcAI
{
    //弹幕游戏处理
    public MiniGameBarrageHandler gameBarrageHandler;
    //迷你游戏数据
    public MiniGameCharacterBean characterMiniGameData;

    //血 粒子
    public ParticleSystem psBlood;

    /// <summary>
    /// 设置NPC数据
    /// </summary>
    /// <param name="characterMiniGameData"></param>
    public void SetData(MiniGameCharacterBean characterMiniGameData)
    {
        this.characterMiniGameData = characterMiniGameData;
    }

    /// <summary>
    ///遭到攻击
    /// </summary>
    /// <param name="damage"></param>
    public void LifeDamage(int damage)
    {
        characterMiniGameData.characterCurrentLife -= damage;
        psBlood.Play();
        //如果是控制的角色并且生命值
        if (characterMiniGameData.characterType == 1)
        {

        }
    }

}