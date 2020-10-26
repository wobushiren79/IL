using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class InfoCharacterPopupShow : PopupShowView
{
    public CharacterUICpt characterUI;
    public CharacterAttributeView attributeView;
    public Text tvName;

    public GameObject objEffectContainer;
    public GameObject objEffectModel;


    protected GameItemsManager gameItemsManager;

    public override void Awake()
    {
        base.Awake();
        gameItemsManager = Find<GameItemsManager>(ImportantTypeEnum.GameItemsManager);
    }

    public void SetData(CharacterBean characterData, List<MiniGameCombatEffectBean> listCombatData)
    {
        SetEffect(listCombatData);
        SetCharacterUI(characterData);
        SetAttributeView(characterData);
        SetName(characterData.baseInfo.titleName + " " + characterData.baseInfo.name);
    }

    /// <summary>
    /// 设置角色信息
    /// </summary>
    /// <param name="characterData"></param>
    public void SetCharacterUI(CharacterBean characterData)
    {
        if (characterUI != null)
            characterUI.SetCharacterData(characterData.body, characterData.equips);
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="characterData"></param>
    public void SetAttributeView(CharacterBean characterData)
    {
        characterData.GetAttributes(gameItemsManager, out CharacterAttributesBean characterAttributes);
        if (attributeView != null)
        {
            attributeView.SetData(characterAttributes);
        }
    }

    /// <summary>
    /// 设置名字
    /// </summary>
    /// <param name="name"></param>
    public void SetName(string name)
    {
        if (tvName != null)
            tvName.text = name;
    }

    /// <summary>
    /// 设置状态
    /// </summary>
    public void SetEffect(List<MiniGameCombatEffectBean> listEffect)
    {
        CptUtil.RemoveChildsByActive(objEffectContainer);

        //如果没有战斗信息 则隐藏展示栏
        if (CheckUtil.ListIsNull(listEffect))
        {
            objEffectContainer.SetActive(false);
        }
        else
        {
            objEffectContainer.SetActive(true);
            for (int i = 0; i < listEffect.Count; i++)
            {
                MiniGameCombatEffectBean itemEffectData = listEffect[i];
                if (itemEffectData.listEffectTypeData!=null)
                {
                    for(int f = 0; f < itemEffectData.listEffectTypeData.Count; f++)
                    {
                        EffectTypeBean effectTypeData = itemEffectData.listEffectTypeData[f];
                        GameObject objEffectItem = Instantiate(objEffectContainer, objEffectModel);
                        ItemBaseTextCpt itemEffect = objEffectItem.GetComponent<ItemBaseTextCpt>();
                        itemEffect.SetData(effectTypeData.spIcon, "", effectTypeData.effectDescribe);
                    }
                }
            }
        }
    }
}