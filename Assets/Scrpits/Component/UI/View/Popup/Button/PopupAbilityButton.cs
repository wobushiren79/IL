using UnityEngine;
using UnityEditor;

public class PopupAbilityButton : PopupButtonView<PopupAbilityShow>
{
    private CharacterBean mCharacterData;

    public void SetData(CharacterBean characterData)
    {
        mCharacterData = characterData;
    }

    public override void PopupShow()
    {
        popupShow.SetData(mCharacterData);
    }

    public override void PopupHide()
    {

    }
}