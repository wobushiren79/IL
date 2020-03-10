using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class InfoItemsPopupShow : PopupShowView
{
    public Image ivIcon;
    public Text tvName;
    public Text tvContent;
    public Text tvType;

    public GameObject objAttributeContainer;
    public GameObject objAttributeModel;

    public Color colorForAttribute;

    public ItemsInfoBean itemsInfoData;

    protected IconDataManager iconDataManager;

    private void Awake()
    {
        iconDataManager = Find<IconDataManager>(ImportantTypeEnum.UIManager);
    }

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
        SetContent(data.content);
        SetType(data.GetItemsType());
        SetAttributes(data);
    }

    public void SetName(string name)
    {
        if (tvName != null)
            tvName.text = name;
    }

    public void SetContent(string content)
    {
        if (tvContent != null)
            tvContent.text = content;
    }

    public void SetIcon(Sprite spIcon)
    {
        if (ivIcon != null)
            ivIcon.sprite = spIcon;
    }

    public void SetType(GeneralEnum type)
    {
        string typeStr = GameCommonInfo.GetUITextById(400) + "：" + GeneralEnumTools.GetGeneralName(type);
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
        CreateItemAttributes("ui_ability_cook", data.add_cook, GameCommonInfo.GetUITextById(1));
        CreateItemAttributes("ui_ability_speed", data.add_speed, GameCommonInfo.GetUITextById(2));
        CreateItemAttributes("ui_ability_account", data.add_account, GameCommonInfo.GetUITextById(3));
        CreateItemAttributes("ui_ability_charm", data.add_charm, GameCommonInfo.GetUITextById(4));
        CreateItemAttributes("ui_ability_force", data.add_force, GameCommonInfo.GetUITextById(5));
        CreateItemAttributes("ui_ability_lucky", data.add_lucky, GameCommonInfo.GetUITextById(6));
        if (CheckUtil.StringIsNull(data.effect))
            return;
        List<EffectTypeBean> listEffectData= EffectTypeEnumTools.GetListEffectData(data.effect);
        if (listEffectData == null)
            return;
        foreach (EffectTypeBean itemData in listEffectData)
        {
            EffectTypeEnumTools.GetEffectDetails(iconDataManager, itemData);
            CreateItemAttributes(itemData.spIcon, itemData.effectDescribe);
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
        itemAttributes.SetData(spIcon, colorForAttribute, details, "");
    }

    /// <summary>
    /// 创建属性信息
    /// </summary>
    /// <param name="attributes"></param>
    /// <param name="attributesStr"></param>
    private void CreateItemAttributes(string iconKey, int attributes, string attributesStr)
    {
        if (attributes == 0)
            return;
        GameObject objItem = Instantiate(objAttributeContainer, objAttributeModel);
        ItemBaseTextCpt itemAttributes = objItem.GetComponent<ItemBaseTextCpt>();
        Sprite spIcon = iconDataManager.GetIconSpriteByName(iconKey);
        itemAttributes.SetData(spIcon, colorForAttribute, attributesStr + "+" + attributes, colorForAttribute, "");
    }
}