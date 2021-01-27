using UnityEngine;
using UnityEditor;

public class NpcAIUserCpt : BaseNpcAI
{

    private void Update()
    {
        if (Input.GetButton(InputInfo.Shift))
        {
            InitCharacterSpeed(1.5f);
        }
        if (Input.GetButtonUp(InputInfo.Shift))
        {
            InitCharacterSpeed(1f);
        }
    }

    public override void SetCharacterData(CharacterBean characterBean)
    {
        base.SetCharacterData(characterBean);
        InitCharacterSpeed(1);
    }

    public void InitCharacterSpeed(float addRate)
    {
        characterData.GetAttributes(out CharacterAttributesBean totalAttributes, out CharacterAttributesBean selfAttributes, out CharacterAttributesBean equipAttributes);
        float speed = 3.25f + (totalAttributes.speed / 50f);
        characterMoveCpt.SetMoveSpeed(speed * addRate);
    }
}