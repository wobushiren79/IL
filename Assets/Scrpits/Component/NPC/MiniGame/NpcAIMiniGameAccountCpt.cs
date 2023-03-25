using UnityEngine;
using UnityEditor;

public class NpcAIMiniGameAccountCpt : BaseNpcAI
{
    public MiniGameCharacterForAccountBean characterMiniGameData;

    /// <summary>
    /// 设置NPC数据
    /// </summary>
    /// <param name="characterMiniGameData"></param>
    public void SetData(MiniGameCharacterForAccountBean characterMiniGameData)
    {
        this.characterMiniGameData = characterMiniGameData;
        SetCharacterData(characterMiniGameData.characterData);
    }
}