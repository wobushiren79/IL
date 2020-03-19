using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIGameWorkerDetailsSkillInfo : BaseUIChildComponent<UIGameWorkerDetails> , SkillInfoManager.ICallBack
{
    public Text tvNull;
    public GameObject objSkillItemContainer;
    public GameObject objSkillItemModel;

    protected SkillInfoManager skillInfoManager;
    protected IconDataManager iconDataManager;
    private void Awake()
    {
        skillInfoManager = Find<SkillInfoManager>(ImportantTypeEnum.SkillManager);
        iconDataManager = Find<IconDataManager>(ImportantTypeEnum.UIManager);
    }

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
        skillInfoManager.SetCallBack(this);
        skillInfoManager.GetSkillByIds(listSkill);    
    }

    public void CreateSkillList(List<SkillInfoBean> listData)
    {
        CptUtil.RemoveChildsByActive(objSkillItemContainer);
        if(CheckUtil.ListIsNull(listData))
        {
            tvNull.gameObject.SetActive(true);
            return;
        }
        else
        {
            tvNull.gameObject.SetActive(false);
        }

        foreach (SkillInfoBean itemSkill in listData)
        {
            GameObject objItem= Instantiate(objSkillItemContainer, objSkillItemModel);
            ItemBaseTextCpt itemBaseText= objItem.GetComponent<ItemBaseTextCpt>();
            InfoSkillPopupButton infoSkillPopup = objItem.GetComponent<InfoSkillPopupButton>();

            Sprite spIcon= iconDataManager.GetIconSpriteByName(itemSkill.icon_key);
            itemBaseText.SetData(spIcon,itemSkill.name,"");
            infoSkillPopup.SetData(itemSkill);
        }
    }

    #region 数据回调
    public void GetSkillInfoSuccess(List<SkillInfoBean> listData)
    {
        CreateSkillList(listData);
    }
    #endregion
}