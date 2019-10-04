using UnityEngine;
using UnityEditor;

public class InfoAbilityPopupButton : PopupButtonView
{
    private CharacterBean mCharacterData;

    public void SetData(CharacterBean characterData)
    {
        mCharacterData = characterData;
    }

    public override void ClosePopup()
    {
    }

    public override void OpenPopup()
    {
        ((InfoAbilityPopupShow)popupShow).SetData(mCharacterData);
    }
}