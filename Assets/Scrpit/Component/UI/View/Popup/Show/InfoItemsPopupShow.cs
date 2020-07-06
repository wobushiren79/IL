using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class InfoItemsPopupShow : PopupShowView
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

    protected IconDataManager iconDataManager;
    protected InnFoodManager innFoodManager;
    public override void Awake()
    {
        base.Awake();
        iconDataManager = Find<IconDataManager>(ImportantTypeEnum.UIManager);
        innFoodManager = Find<InnFoodManager>(ImportantTypeEnum.FoodManager);
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
        if(data.GetItemsType() == GeneralEnum.Menu && CheckUtil.StringIsNull(data.content))
        {
           MenuInfoBean menuInfo= innFoodManager.GetFoodDataById(data.add_id);
            if (menuInfo != null)
                content = menuInfo.content;
        }
        else
        {
            content = data.content;
        }
        if (CheckUtil.StringIsNull(content))
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
        CreateItemAttributes("ui_ability_life", data.add_life, GameCommonInfo.GetUITextById(9), colorForAttribute);
        CreateItemAttributes("ui_ability_cook", data.add_cook, GameCommonInfo.GetUITextById(1),colorForAttribute);
        CreateItemAttributes("ui_ability_speed", data.add_speed, GameCommonInfo.GetUITextById(2), colorForAttribute);
        CreateItemAttributes("ui_ability_account", data.add_account, GameCommonInfo.GetUITextById(3), colorForAttribute);
        CreateItemAttributes("ui_ability_charm", data.add_charm, GameCommonInfo.GetUITextById(4), colorForAttribute);
        CreateItemAttributes("ui_ability_force", data.add_force, GameCommonInfo.GetUITextById(5), colorForAttribute);
        CreateItemAttributes("ui_ability_lucky", data.add_lucky, GameCommonInfo.GetUITextById(6), colorForAttribute);
  
        if (CheckUtil.StringIsNull(data.effect))
            return;
        List<EffectTypeBean> listEffectData= EffectTypeEnumTools.GetListEffectData(data.effect);
        //获取详情
        EffectDetailsEnumTools.GetEffectDetailsForCombat(data.effect_details, out string effectPSName, out int durationForRound);

        if (listEffectData == null)
            return;
        foreach (EffectTypeBean itemData in listEffectData)
        {
            EffectTypeEnumTools.GetEffectDetails(iconDataManager, itemData);
            string describe = itemData.effectDescribe;
            if (durationForRound != 0)
            {
                describe +=("\n"+ string.Format(GameCommonInfo.GetUITextById(502),""+ durationForRound));
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
    private void CreateItemAttributes(string iconKey, int attributes, string attributesStr,Color colorIcon)
    {
        if (attributes == 0)
            return;
        GameObject objItem = Instantiate(objAttributeContainer, objAttributeModel);
        ItemBaseTextCpt itemAttributes = objItem.GetComponent<ItemBaseTextCpt>();
        Sprite spIcon = iconDataManager.GetIconSpriteByName(iconKey);
        itemAttributes.SetData(spIcon, colorForAttribute, attributesStr + "+" + attributes, colorIcon, "");
    }
}