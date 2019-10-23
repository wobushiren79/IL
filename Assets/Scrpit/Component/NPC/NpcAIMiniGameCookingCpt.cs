using UnityEngine;
using UnityEditor;

public class NpcAIMiniGameCookingCpt : BaseNpcAI
{
    public enum MiniGameCookingNpcType
    {
        Player,//参与者
        Auditer,//评审员
        Compere//主持
    }
    private MiniGameCookingNpcType mNpcType;
    public MiniGameCharacterBean characterMiniGameData;

    public void SetNpcType(MiniGameCookingNpcType npcType)
    {
        mNpcType = npcType;
    }

    public MiniGameCookingNpcType GetNpcType()
    {
        return mNpcType;
    }

    /// <summary>
    /// 设置NPC数据
    /// </summary>
    /// <param name="characterMiniGameData"></param>
    public void SetData(MiniGameCharacterBean characterMiniGameData)
    {
        this.characterMiniGameData = characterMiniGameData;
        SetCharacterData(characterMiniGameData.characterData);
    }

    public void OpenAI()
    {
        characterMoveCpt.navMeshAgent.enabled = true;
    }
}