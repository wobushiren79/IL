using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class PopupCharacterButton : PopupButtonView<PopupCharacterShow>
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

    public override void ClosePopup()
    {
    }

    public override void OpenPopup()
    {
        popupShow.SetData(characterData, listCombatData);
    }
}