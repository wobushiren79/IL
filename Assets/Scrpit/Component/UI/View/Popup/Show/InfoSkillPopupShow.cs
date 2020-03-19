using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class InfoSkillPopupShow : PopupShowView
{
    public Image ivIcon;
    public Text tvName;
    public Text tvContent;

    public GameObject objAttributeContainer;
    public GameObject objAttributeModel;

    public Color colorForAttribute;

    public SkillInfoBean skillInfoData;

    protected IconDataManager iconDataManager;

    public override void Awake()
    {
        base.Awake();
        iconDataManager = Find<IconDataManager>(ImportantTypeEnum.UIManager);
    }

    /// <summary>
    /// 设置文本内容
    /// </summary>
    /// <param name="content"></param>
    public void SetData(SkillInfoBean skillInfoData)
    {
        if (skillInfoData == null)
            return;
        this.skillInfoData = skillInfoData;
        SetIcon(skillInfoData.icon_key);
        SetName(skillInfoData.name);
        SetContent(skillInfoData.content);
        SetAttributes(skillInfoData);
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

    public void SetIcon(string iconKey)
    {
        Sprite spIcon = iconDataManager.GetIconSpriteByName(iconKey);
        if (ivIcon != null)
            ivIcon.sprite = spIcon;
    }

    /// <summary>
    /// 设置属性
    /// </summary>
    /// <param name="data"></param>
    public void SetAttributes(SkillInfoBean data)
    {
        CptUtil.RemoveChildsByActive(objAttributeContainer);
        CreateItemAttributes("hourglass_1", GameCommonInfo.GetUITextById(510)+ " "+ data.GetUseNumber());


        if (CheckUtil.StringIsNull(data.effect))
            return;
        List<EffectTypeBean> listEffectData = EffectTypeEnumTools.GetListEffectData(data.effect);
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
                describe += ("\n" + string.Format(GameCommonInfo.GetUITextById(502), "" + durationForRound));
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

    private void CreateItemAttributes(string iconKey, string details)
    {
        Sprite spIcon = iconDataManager.GetIconSpriteByName(iconKey);
        CreateItemAttributes(spIcon, details);
    }
}