using UnityEngine;
using UnityEditor;

public class NpcAIUserCpt : BaseNpcAI
{
    public override void SetCharacterData(GameItemsManager gameItemsManager, CharacterBean characterBean)
    {
        base.SetCharacterData(gameItemsManager, characterBean);
        characterData.GetAttributes(gameItemsManager,
          out CharacterAttributesBean totalAttributes, out CharacterAttributesBean selfAttributes, out CharacterAttributesBean equipAttributes);
        float speed = 3f + (totalAttributes.speed / 50f);
        characterMoveCpt.SetMoveSpeed(speed);
    }
}