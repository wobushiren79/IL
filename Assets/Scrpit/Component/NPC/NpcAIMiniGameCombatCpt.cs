using UnityEngine;
using UnityEditor;

public class NpcAIMiniGameCombatCpt : BaseNpcAI
{
    //战斗游戏处理
    public MiniGameCombatHandler gameCombatHandler;
    //角色数据
    public MiniGameCharacterBean characterMiniGameData;
    //角色血量
    public CharacterLifeCpt characterLifeCpt;
    //角色出血特效
    public ParticleSystem psBlood;
    //选中特效
    public ParticleSystem psSelected;

    /// <summary>
    /// 设置NPC数据
    /// </summary>
    /// <param name="characterMiniGameData"></param>
    public void SetData(MiniGameCharacterBean characterMiniGameData)
    {
        this.characterMiniGameData = characterMiniGameData;
        SetCharacterData(characterMiniGameData.characterData);
    }

    /// <summary>
    /// 设置选中
    /// </summary>
    /// <param name="isSelected"></param>
    public void SetSelected(bool isSelected)
    {
        if (isSelected)
        {
            psSelected.gameObject.SetActive(true);
            psSelected.Play();
        }
        else
            psSelected.gameObject.SetActive(false);
    }
}