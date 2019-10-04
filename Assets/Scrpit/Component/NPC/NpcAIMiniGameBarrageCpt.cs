using UnityEngine;
using UnityEditor;

public class NpcAIMiniGameBarrageCpt : BaseNpcAI
{
    //迷你游戏数据
    public MiniGameCharacterBean characterMiniGameData;
    public ParticleSystem psBlood;

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
    }

}