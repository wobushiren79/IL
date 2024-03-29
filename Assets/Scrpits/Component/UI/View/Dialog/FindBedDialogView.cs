﻿using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class FindBedDialogView : DialogView
{
    public GameObject objContent;

    public BedShowView bedShowView;
    public Text tvName;
    public Text tvPrice;
    public Image ivCardBG;
    public Image ivHaloBG;

    public UIPopupBedButton popupBedButton;

    public BuildBedBean buildBedData;

    public Color colorForNormal;
    public Color colorForRare;
    public Sprite spBGNormal;
    public Sprite spBGRare;

    public Button btContinue;

    protected Action<DialogView, DialogBean> actionContinue;

    public override void Start()
    {
        base.Start();
        StartAnim();
        btContinue.onClick.AddListener(OnClickForContinue);
    }

    /// <summary>
    /// 开始动画
    /// </summary>
    public void StartAnim()
    {
        btContinue.gameObject.SetActive(false);
        ui_Submit.gameObject.SetActive(false);
        ui_Cancel.gameObject.SetActive(false);
        if (objContent != null)
        {
            objContent.transform.DOScale(new Vector3(0, 0, 0), 1).From().SetEase(Ease.OutBack).OnComplete(delegate ()
            {
                ui_Submit.gameObject.SetActive(true);
                ui_Cancel.gameObject.SetActive(true);
                btContinue.gameObject.SetActive(true);
                ui_Submit.transform.DOScale(new Vector3(0, 0, 0), 0.2f).From().SetEase(Ease.OutBack);
                ui_Cancel.transform.DOScale(new Vector3(0, 0, 0), 0.2f).From().SetEase(Ease.OutBack);
                objContent.transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 5).SetLoops(-1, LoopType.Yoyo);
            });
        }
    }

    /// <summary>
    /// 设置继续回调
    /// </summary>
    public void SetCallBackForContinue(Action<DialogView, DialogBean> actionContinue)
    {
        this.actionContinue += actionContinue;
    }

    /// <summary>
    /// 点击续购
    /// </summary>
    public void OnClickForContinue()
    {
        actionContinue?.Invoke(this, dialogData);
        CancelOnClick();
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="characterData"></param>
    public void SetData(BuildBedBean buildBedData)
    {
        this.buildBedData = buildBedData;
        SetName(buildBedData.bedName);
        SetPrice(buildBedData.priceL, buildBedData.priceM, buildBedData.priceS);
        SetBedUI (buildBedData);
        SetPopupInfo(buildBedData);
        SetBackground(buildBedData.GetRarity());
    }

    /// <summary>
    /// 设置背景
    /// </summary>
    /// <param name="npcType"></param>
    public void SetBackground(RarityEnum rarity)
    {
        if (ivCardBG == null || ivHaloBG == null)
            return;
        if (rarity != RarityEnum.Normal)
        {
            ivCardBG.sprite = spBGRare;
            ivHaloBG.color = colorForRare;
            tvName.color = colorForRare;
        }
        else
        {
            ivCardBG.sprite = spBGNormal;
            ivHaloBG.color = colorForNormal;
            tvName.color = colorForNormal;
        }
    }

    /// <summary>
    /// 设置弹窗框数据
    /// </summary>
    /// <param name="characterData"></param>
    public void SetPopupInfo(BuildBedBean buildBedData)
    {
        if (popupBedButton != null)
        {
            popupBedButton.SetData(buildBedData);
        }
    }

    /// <summary>
    /// 设置角色UI
    /// </summary>
    /// <param name="characterData"></param>
    public void SetBedUI(BuildBedBean buildBedData)
    {
        if (bedShowView != null)
        {
            bedShowView.SetData(buildBedData);
        }
    }

    /// <summary>
    /// 设置名字
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
    /// 设置日薪
    /// </summary>
    /// <param name="price_l"></param>
    /// <param name="price_m"></param>
    /// <param name="price_s"></param>
    public void SetPrice(long price_l, long price_m, long price_s)
    {
        if (tvPrice != null)
        {
            string priceStr = "";
            if (price_l > 0)
                priceStr += price_l + TextHandler.Instance.manager.GetTextById(16);
            if (price_m > 0)
                priceStr += price_m + TextHandler.Instance.manager.GetTextById(17);
            if (price_s > 0)
                priceStr += price_s + TextHandler.Instance.manager.GetTextById(18);
            tvPrice.text = priceStr  + "/" + TextHandler.Instance.manager.GetTextById(37);
        }
    }
}