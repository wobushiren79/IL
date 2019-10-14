using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class ItemMiniGameCombatCharacterInfoCpt : ItemGameBaseCpt
{
    public CharacterUICpt characterUI;
    public Text tvName;
    public Text tvLife;
    public Text tvForce;
    public Slider sliderLife;

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="gameCharacterData"></param>
    public void SetData(MiniGameCharacterBean gameCharacterData)
    {
        SetCharacterUI(gameCharacterData.characterData);
        SetName(gameCharacterData.characterData.baseInfo.name);
        SetLife(gameCharacterData.characterCurrentLife, gameCharacterData.characterMaxLife);

        GameItemsManager gameItemsManager = GetUIManager<UIGameManager>().gameItemsManager;
        gameCharacterData.characterData.GetAttributes(gameItemsManager, out CharacterAttributesBean characterAttributes);
        SetForce(characterAttributes.force);
    }

    /// <summary>
    /// 设置角色图标
    /// </summary>
    /// <param name="characterData"></param>
    public void SetCharacterUI(CharacterBean characterData)
    {
        if (characterUI != null)
        {
            characterUI.SetCharacterData(characterData.body, characterData.equips);
        }
    }

    /// <summary>
    /// 设置姓名
    /// </summary>
    /// <param name="name"></param>
    public void SetName(string name)
    {
        if (tvName != null)
        {
            tvName.text = name;
        }
    }

    /// <summary>
    /// 设置生命值
    /// </summary>
    /// <param name="currentLife"></param>
    /// <param name="maxLife"></param>
    public void SetLife(int currentLife, int maxLife)
    {
        if (tvLife != null)
            tvLife.text = currentLife + "/" + maxLife;
        if (sliderLife != null)
            sliderLife.value = (float)currentLife / (float)maxLife;
    }

    /// <summary>
    /// 设置武力
    /// </summary>
    /// <param name="force"></param>
    public void SetForce(int force)
    {
        if (tvForce != null)
            tvForce.text = "" + force;
    }
}