using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class ItemGameFamilyCpt : ItemGameBaseCpt
{
    public Text ui_TVName;
    public Text ui_TVCall;
    public CharacterUICpt ui_CharacterUI;
    public Image ui_CharacterChild;

    private void Awake()
    {
        AutoLinkUI();
    }

    public void SetData(CharacterForFamilyBean characterData)
    {
        SetName(characterData.baseInfo.name);
        SetCall(characterData.GetFamilyName());
        SetCharacterUI(characterData);
    }

    /// <summary>
    /// 设置名字
    /// </summary>
    /// <param name="name"></param>
    public void SetName(string name)
    {
        if (ui_TVName)
            ui_TVName.text = name;
    }

    /// <summary>
    /// 设置称呼
    /// </summary>
    /// <param name="call"></param>
    public void SetCall(string call)
    {
        if (ui_TVCall)
            ui_TVCall.text = call;
    }

    /// <summary>
    /// 设置角色形象
    /// </summary>
    /// <param name="characterData"></param>
    public void SetCharacterUI(CharacterForFamilyBean characterData)
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        if (characterData.GetFamilyType()!= FamilyTypeEnum.Mate && !characterData.CheckIsGrowUp(gameData.gameTime))
        {
            ui_CharacterChild.gameObject.SetActive(true);
            ui_CharacterUI.gameObject.SetActive(false);
        }
        else
        {
            ui_CharacterChild.gameObject.SetActive(false);
            ui_CharacterUI.gameObject.SetActive(true);
        }
        if (ui_CharacterUI)
            ui_CharacterUI.SetCharacterData(characterData.body, characterData.equips);
    }
}