﻿using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class ItemDialogPickForSkillCpt : BaseMonoBehaviour
{
    public Image ivIcon;
    public Text tvName;
    public Text tvNumber;
    public Button btSubmit;
    public UIPopupSkillButton infoSkillPopup;

    protected ICallBack callBack;

    public SkillInfoBean skillInfoData;
    public int usedNumber;

    private void Start()
    {
        if (btSubmit != null)
            btSubmit.onClick.AddListener(OnClickSubmit);
    }

    public void SetCallBack(ICallBack callBack)
    {
        this.callBack = callBack;
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="skillInfo"></param>
    /// <param name="usedNumber"></param>
    public void SetData(SkillInfoBean skillInfoData, int usedNumber)
    {
        this.skillInfoData = skillInfoData;
        this.usedNumber = usedNumber;
        int restNumber = skillInfoData.GetRestNumber(usedNumber);
        SetName(skillInfoData.name, restNumber);
        SetNumber(restNumber);
        SetIcon(skillInfoData.icon_key);

        infoSkillPopup.SetData(skillInfoData);
    }

    /// <summary>
    /// 设置名字
    /// </summary>
    /// <param name="name"></param>
    /// <param name="number"></param>
    public void SetName(string name, int number)
    {
        if (tvName != null)
        {
            tvName.text = name;
            if (number <= 0)
            {
                tvName.color = Color.gray;
            }
        }
    }

    /// <summary>
    /// 设置图标
    /// </summary>
    /// <param name="iconKey"></param>
    public void SetIcon(string iconKey)
    {
        Sprite spIcon = IconHandler.Instance.GetIconSpriteByName(iconKey);
        if (ivIcon != null)
            ivIcon.sprite = spIcon;
    }

    /// <summary>
    /// 设置数量
    /// </summary>
    /// <param name="number"></param>
    public void SetNumber(int number)
    {
        if (tvNumber != null)
        {
            tvNumber.text = "x" + number;
            if (number <= 0)
            {
                tvNumber.color = Color.gray;
            }
        }
    }

    /// <summary>
    /// 确认点击
    /// </summary>
    public void OnClickSubmit()
    {
        int restNumber = skillInfoData.GetRestNumber(usedNumber);
        if (restNumber <= 0)
        {
            UIHandler.Instance.ToastHint<ToastView>(ivIcon.sprite,TextHandler.Instance.manager.GetTextById(1046));
            return;
        }
        if (callBack != null)
            callBack.SelectedSkill(skillInfoData);
    }

    /// <summary>
    /// 回调
    /// </summary>
    public interface ICallBack
    {
        void SelectedSkill(SkillInfoBean skillInfo);
    }

}