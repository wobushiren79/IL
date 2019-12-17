using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class ItemMiniGameEndResultWinCpt : BaseMonoBehaviour
{
    public Text tvContent;
    public Image ivIcon;
    public CharacterUICpt characterUI;

    /// <summary>
    /// 设置图标
    /// </summary>
    /// <param name="spIcon"></param>
    public void SetIcon(Sprite spIcon)
    {
        if (ivIcon == null)
            return;
        ivIcon.gameObject.SetActive(true);
        if (spIcon != null)
            ivIcon.sprite = spIcon;
    }

    /// <summary>
    /// 设置内容
    /// </summary>
    /// <param name="content"></param>
    public void SetContent(string content)
    {
        if (tvContent != null)
            tvContent.text = content;
    }

    /// <summary>
    /// 设置角色形象
    /// </summary>
    public void SetCharacterUI(CharacterBean characterData)
    {
        if (characterUI == null)
            return;
        characterUI.gameObject.SetActive(true);
        if (characterData != null)
            characterUI.SetCharacterData(characterData.body, characterData.equips);
    }
}