using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;

public class ItemGameDialogPickCharacterCpt : BaseMonoBehaviour
{
    public CharacterUICpt characterUI;
    public Text tvName;

    public void SetData(CharacterBean characterData)
    {
        SetCharacterUI(characterData);
        SetName(characterData.baseInfo.name);
    }

    /// <summary>
    /// 设置角色形象
    /// </summary>
    /// <param name="characterData"></param>
    public void SetCharacterUI(CharacterBean characterData)
    {
        if (characterUI!=null)
        {
            characterUI.SetCharacterData(characterData.body, characterData.equips);
        }
    }

    /// <summary>
    /// 设置角色姓名
    /// </summary>
    /// <param name="name"></param>
    public void SetName(string name)
    {
        if (tvName != null)
            tvName.text = name;
    }
}