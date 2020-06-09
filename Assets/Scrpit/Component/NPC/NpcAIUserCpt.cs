using UnityEngine;
using UnityEditor;

public class NpcAIUserCpt : BaseNpcAI
{
    public override void SetCharacterData(GameItemsManager gameItemsManager, CharacterBean characterBean)
    {
        base.SetCharacterData(gameItemsManager, characterBean);
        characterMoveCpt.SetMoveSpeed(2);
    }
}