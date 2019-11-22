using UnityEngine;
using UnityEditor;

public class NpcAIMiniGameDebateCpt : BaseNpcAI
{
    public MiniGameCharacterForDebateBean miniGameCharacterData;

    public void SetData(MiniGameCharacterForDebateBean miniGameCharacterData)
    {
        this.miniGameCharacterData = miniGameCharacterData;
        SetCharacterData(miniGameCharacterData.characterData);
    }
}