﻿using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIPopupItemsShow : PopupShowView
{
    public Image ivIcon;
    public Text tvName;
    public GameObject objContent;
    public Text tvContent;
    public Text tvType;

    public GameObject objAttributeContainer;
    public GameObject objAttributeModel;

    public Color colorForAttribute;

    public ItemsInfoBean itemsInfoData;


    /// <summary>
    /// 设置文本内容
    /// </summary>
    /// <param name="content"></param>
    public void SetData(Sprite spIcon, ItemsInfoBean data)
    {
        if (data == null)
            return;
        this.itemsInfoData = data;
        SetIcon(spIcon);
        SetName(data.name);
        SetContent(data);
        SetType(data.GetItemsType());
        SetAttributes(data);
    }

    public void SetName(string name)
    {
        if (tvName != null)
            tvName.text = name;
    }

    public void SetContent(ItemsInfoBean data)
    {
        string content = "???";
        if (data.GetItemsType() == GeneralEnum.Menu && data.content.IsNull())
        {
            MenuInfoBean menuInfo = InnFoodHandler.Instance.manager.GetFoodDataById(data.add_id);
            if (menuInfo != null)
                content = menuInfo.content;
        }
        else
        {
            content = data.content;
        }
        if (content.IsNull())
        {
            objContent.SetActive(false);
        }
        else
        {
            objContent.SetActive(true);
            if (tvContent != null)
                tvContent.text = content;
        }
    }

    public void SetIcon(Sprite spIcon)
    {
        if (ivIcon != null)
            ivIcon.sprite = spIcon;
    }

    public void SetType(GeneralEnum type)
    {
        string typeStr = TextHandler.Instance.manager.GetTextById(400) + "：" + GeneralEnumTools.GetGeneralName(type);
        if (tvType != null)
            tvType.text = typeStr;
    }

    /// <summary>
    /// 设置属性
    /// </summary>
    /// <param name="data"></param>
    public void SetAttributes(ItemsInfoBean data)
    {
        CptUtil.RemoveChildsByActive(objAttributeContainer);
        CreateItemAttributes("ui_ability_life", data.add_life, AttributesTypeEnumTools.GetAttributesName(AttributesTypeEnum.Life), colorForAttribute);
        CreateItemAttributes("ui_ability_cook", data.add_cook, TextHandler.Instance.manager.GetTextById(1), colorForAttribute);
        CreateItemAttributes("ui_ability_speed", data.add_speed, AttributesTypeEnumTools.GetAttributesName(AttributesTypeEnum.Speed), colorForAttribute);
        CreateItemAttributes("ui_ability_account", data.add_account, AttributesTypeEnumTools.GetAttributesName(AttributesTypeEnum.Account), colorForAttribute);
        CreateItemAttributes("ui_ability_charm", data.add_charm, AttributesTypeEnumTools.GetAttributesName(AttributesTypeEnum.Charm), colorForAttribute);
        CreateItemAttributes("ui_ability_force", data.add_force, AttributesTypeEnumTools.GetAttributesName(AttributesTypeEnum.Force), colorForAttribute);
        CreateItemAttributes("ui_ability_lucky", data.add_lucky, AttributesTypeEnumTools.GetAttributesName(AttributesTypeEnum.Lucky), colorForAttribute);

        if (data.effect.IsNull())
            return;
        List<BufferTypeBean> listEffectData = BufferTypeEnumTools.GetListEffectData(data.effect);
        //获取详情
        EffectDetailsEnumTools.GetEffectDetailsForCombat(data.effect_details, out string effectPSName, out int durationForRound);

        if (listEffectData == null)
            return;
        foreach (BufferTypeBean itemData in listEffectData)
        {
            BufferTypeEnumTools.GetEffectDetails(itemData, null);
            string describe = itemData.effectDescribe;
            if (durationForRound != 0)
            {
                describe += ("\n" + string.Format(TextHandler.Instance.manager.GetTextById(502), "" + durationForRound));
            }
            CreateItemAttributes(itemData.spIcon, describe);
        }
    }

    /// <summary>
    /// 创建效果信息
    /// </summary>
    /// <param name="attributes"></param>
    /// <param name="attributesStr"></param>
    private void CreateItemAttributes(Sprite spIcon, string details)
    {
        GameObject objItem = Instantiate(objAttributeContainer, objAttributeModel);
        ItemBaseTextCpt itemAttributes = objItem.GetComponent<ItemBaseTextCpt>();
        itemAttributes.SetData(spIcon, Color.white, details, "");
    }

    /// <summary>
    /// 创建属性信息
    /// </summary>
    /// <param name="attributes"></param>
    /// <param name="attributesStr"></param>
    private void CreateItemAttributes(string iconKey, int attributes, string attributesStr, Color colorIcon)
    {
        if (attributes == 0)
            return;
        GameObject objItem = Instantiate(objAttributeContainer, objAttributeModel);
        ItemBaseTextCpt itemAttributes = objItem.GetComponent<ItemBaseTextCpt>();
        Sprite spIcon = IconHandler.Instance.GetIconSpriteByName(iconKey);
        itemAttributes.SetData(spIcon, colorForAttribute, attributesStr + "+" + attributes, colorIcon, "");
    }
}