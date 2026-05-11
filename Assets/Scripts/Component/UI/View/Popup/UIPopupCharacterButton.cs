using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class UIPopupCharacterButton : PopupButtonView<UIPopupCharacterShow>
{
    protected CharacterBean characterData;
    protected List<MiniGameCombatEffectBean> listCombatData;


    public void SetData(CharacterBean characterData)
    {
        SetData(characterData, null);
    }

    public void SetData(CharacterBean characterData,List<MiniGameCombatEffectBean> listCombatData)
    {
        this.characterData = characterData;
        this.listCombatData = listCombatData;
    }

    public override void PopupShow()
    {
        popupShow.SetData(characterData, listCombatData);
    }

    public override void PopupHide()
    {

    }
}