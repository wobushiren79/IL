using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class ItemGameFamilyCpt : ItemGameBaseCpt
{
    public Text tvName;
    public Text tvCall;
    public CharacterUICpt characterUI;

    public void SetData(CharacterBean characterData)
    {
        SetName(characterData.baseInfo.name);
        SetCall(characterData.baseInfo.name);
        SetCharacterUI(characterData);
    }

    /// <summary>
    /// 设置名字
    /// </summary>
    /// <param name="name"></param>
    public void SetName(string name)
    {
        if (tvName)
            tvName.text = name;
    }

    /// <summary>
    /// 设置称呼
    /// </summary>
    /// <param name="call"></param>
    public void SetCall(string call)
    {
        if (tvCall)
            tvCall.text = call;
    }

    /// <summary>
    /// 设置角色形象
    /// </summary>
    /// <param name="characterData"></param>
    public void SetCharacterUI(CharacterBean characterData)
    {
        if (characterUI)
            characterUI.SetCharacterData(characterData.body, characterData.equips);
    }
}