using UnityEngine;
using UnityEditor;

public class InfoAbilityPopupButton : PopupButtonView<InfoAbilityPopupShow>
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
        popupShow.SetData(mCharacterData);
    }
}