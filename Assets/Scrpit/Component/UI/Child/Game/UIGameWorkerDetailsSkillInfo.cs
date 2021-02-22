using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class UIGameWorkerDetailsSkillInfo : BaseUIChildComponent<UIGameWorkerDetails>
{
    public Text tvNull;
    public GameObject objSkillItemContainer;
    public GameObject objSkillItemModel;

    protected List<long> listSkill = new List<long>();
    public override void Open()
    {
        base.Open();
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="listSkill"></param>
    public void SetData(List<long> listSkill)
    {
        this.listSkill = listSkill;
        SkillInfoHandler.Instance.manager.GetAllSkills(SetSkillInfoData);
    }

    public void CreateSkillList(List<SkillInfoBean> listData)
    {
        CptUtil.RemoveChildsByActive(objSkillItemContainer);
        if (CheckUtil.ListIsNull(listData))
        {
            tvNull.gameObject.SetActive(true);
            return;
        }
        else
        {
            tvNull.gameObject.SetActive(false);
        }
        for (int i = 0; i < listData.Count; i++)
        {
            SkillInfoBean itemSkill = listData[i];
            if (!listSkill.Contains(itemSkill.id))
            {
                continue;
            }
            GameObject objItem = Instantiate(objSkillItemContainer, objSkillItemModel);
            ItemBaseTextCpt itemBaseText = objItem.GetComponent<ItemBaseTextCpt>();
            PopupSkillButton infoSkillPopup = objItem.GetComponent<PopupSkillButton>();

            Sprite spIcon = IconDataHandler.Instance.manager.GetIconSpriteByName(itemSkill.icon_key);
            itemBaseText.SetData(spIcon, itemSkill.name, "");
            infoSkillPopup.SetData(itemSkill);
        }
    }

    #region 数据回调
    public void SetSkillInfoData(List<SkillInfoBean> listData)
    {
        CreateSkillList(listData);
    }
    #endregion
}