using UnityEngine;
using UnityEditor;

public class UIPopupAbilityButton : PopupButtonView<UIPopupAbilityShow>
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