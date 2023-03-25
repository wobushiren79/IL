using UnityEngine;
using UnityEditor;
using System;

public class ItemMiniGameCombatCharacterRoundCpt : ItemGameBaseCpt
{
    public CharacterUICpt characterUI;
    public MiniGameCharacterForCombatBean gameCharacterData;

    public ParticleSystem psSelected;
    public int speedForMove = 0;

    public void RefreshUI()
    {
        
        //获取角色属性
        gameCharacterData.characterData.GetAttributes(gameCharacterData, out CharacterAttributesBean characterAttributes);
        speedForMove = characterAttributes.speed;
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="gameCharacterData"></param>
    public void SetData(MiniGameCharacterForCombatBean gameCharacterData)
    {
        this.gameCharacterData = gameCharacterData;
        SetCharacterUI(gameCharacterData.characterData);
        RefreshUI();
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
    /// 设置状态
    /// </summary>
    /// <param name="isSelected"></param>
    public void SetStatus(bool isSelected)
    {
        if (isSelected)
        {
            psSelected.gameObject.SetActive(true);
            psSelected.Play();
        }
        else
        {
            psSelected.gameObject.SetActive(false);
        }
    }
}