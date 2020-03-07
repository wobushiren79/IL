using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public class ItemTownCerpenterCpt : ItemGameBaseCpt, DialogView.IDialogCallBack
{
    public Image ivIcon;
    public Text tvName;
    public Text tvContent;
    public Button btSubmit;

    public GameObject objPriceL;
    public Text tvPriceL;
    public GameObject objPriceM;
    public Text tvPriceM;
    public GameObject objPriceS;
    public Text tvPriceS;

    public GameObject objAttribute;
    public Text tvAttribute;

    public GameObject objOwn;
    public Text tvOwn;

    public StoreInfoBean storeInfo;
    public BuildItemBean buildItemData;

    public InfoPromptPopupButton infoPromptPopup;

    private void Awake()
    {
        UIGameManager uiGameManager = GetUIManager<UIGameManager>();
        if (btSubmit != null)
            btSubmit.onClick.AddListener(SubmitBuy);
        if (infoPromptPopup != null)
            infoPromptPopup.SetPopupShowView(uiGameManager.infoPromptPopup);
    }

    public void RefreshUI()
    {
        SetOwn((StoreForCarpenterTypeEnum)storeInfo.store_goods_type);
    }

    public void SetData(StoreInfoBean itemData)
    {
        InnBuildManager innBuildManager = GetUIManager<UIGameManager>().innBuildManager;

        storeInfo = itemData;
        float aesthetics = 0;
        string iconKey = "";
        string name = "";
        string content = "";
        StoreForCarpenterTypeEnum type = (StoreForCarpenterTypeEnum)storeInfo.store_goods_type;
        if (type == StoreForCarpenterTypeEnum.Expansion)
        {
            iconKey = storeInfo.icon_key;
            name = storeInfo.name;
            content = storeInfo.content;
        }
        else
        {
            buildItemData = innBuildManager.GetBuildDataById(itemData.mark_id);
            if (buildItemData != null)
            {
                aesthetics = buildItemData.aesthetics;
                iconKey = buildItemData.icon_key;
                name = buildItemData.name;
                content = buildItemData.content;
            }

        }

        SetPrice(storeInfo.price_l, storeInfo.price_m, storeInfo.price_s);
        SetName(name);
        SetIcon(type, buildItemData, itemData);
        SetAttribute(type, aesthetics);
        SetContent(type, content);
        SetOwn(type);
        SetPopup(content);
    }

    /// <summary>
    /// 设置弹出框
    /// </summary>
    /// <param name="content"></param>
    public void SetPopup(string content)
    {
        if (infoPromptPopup != null)
            infoPromptPopup.SetContent(content);
    }

    /// <summary>
    /// 设置属性
    /// </summary>
    /// <param name="type"></param>
    /// <param name="a"></param>
    public void SetAttribute(StoreForCarpenterTypeEnum type, float aesthetics)
    {
        if (type == StoreForCarpenterTypeEnum.Expansion)
        {
            objAttribute.gameObject.SetActive(false);
        }
        else
        {
            objAttribute.gameObject.SetActive(true);
        }
        if (tvAttribute != null)
            tvAttribute.text = GameCommonInfo.GetUITextById(10) + "+" + aesthetics;
    }

    /// <summary>
    /// 设置图标
    /// </summary>
    /// <param name="iconKey"></param>
    /// <param name="mark"></param>
    /// <param name="markId"></param>
    public void SetIcon(StoreForCarpenterTypeEnum type, BuildItemBean buildItem, StoreInfoBean storeInfo)
    {
        IconDataManager iconDataManager = GetUIManager<UIGameManager>().iconDataManager;
        InnBuildManager innBuildManager = GetUIManager<UIGameManager>().innBuildManager;
        if (iconDataManager == null)
            return;
        Sprite spIcon = null;
        if (type == StoreForCarpenterTypeEnum.Expansion)
        {
            spIcon = iconDataManager.GetIconSpriteByName(storeInfo.icon_key);
        }
        else
        {
            spIcon = BuildItemTypeEnumTools.GetBuildItemSprite(innBuildManager, buildItem);
        }

        if (ivIcon != null && spIcon != null)
            ivIcon.sprite = spIcon;
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
    /// 设置描述
    /// </summary>
    /// <param name="content"></param>
    public void SetContent(StoreForCarpenterTypeEnum type, string content)
    {
        if (tvContent != null)
            tvContent.text = content;
    }

    /// <summary>
    /// 设置价格
    /// </summary>
    public void SetPrice(long priceL, long priceM, long priceS)
    {
        if (priceL == 0)
            objPriceL.SetActive(false);
        if (priceM == 0)
            objPriceM.SetActive(false);
        if (priceS == 0)
            objPriceS.SetActive(false);
        tvPriceL.text = priceL + "";
        tvPriceM.text = priceM + "";
        tvPriceS.text = priceS + "";
    }

    /// <summary>
    /// 设置拥有数量
    /// </summary>
    public void SetOwn(StoreForCarpenterTypeEnum type)
    {
        if (type == StoreForCarpenterTypeEnum.Expansion)
        {
            objOwn.gameObject.SetActive(false);
        }
        else
        {
            objOwn.gameObject.SetActive(true);
            GameDataManager gameDataManager = GetUIManager<UIGameManager>().gameDataManager;
            if (tvOwn == null)
                return;
            tvOwn.text = (GameCommonInfo.GetUITextById(4001) + "\n" + gameDataManager.gameData.GetBuildNumber(storeInfo.mark_id));
        }
    }

    /// <summary>
    /// 购买确认
    /// </summary>
    public void SubmitBuy()
    {
        UIGameManager uiGameManager = GetUIManager<UIGameManager>();
        GameDataManager gameDataManager = uiGameManager.gameDataManager;
        ToastManager toastManager = uiGameManager.toastManager;
        DialogManager dialogManager = uiGameManager.dialogManager;
        AudioHandler audioHandler = uiGameManager.audioHandler;

        audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);

        InnBuildBean innBuildData = gameDataManager.gameData.GetInnBuildData();
        if (gameDataManager == null || storeInfo == null)
            return;
        //检测金钱
        if (!gameDataManager.gameData.HasEnoughMoney(storeInfo.price_l, storeInfo.price_m, storeInfo.price_s))
        {
            toastManager.ToastHint(GameCommonInfo.GetUITextById(1005));
            return;
        }
        //检测是否正在修建客栈
        if (storeInfo.store_goods_type == (int)StoreForCarpenterTypeEnum.Expansion && innBuildData.listBuildDay.Count != 0)
        {
            toastManager.ToastHint(GameCommonInfo.GetUITextById(1019));
            return;
        }
        DialogBean dialogBean = new DialogBean();
        if (storeInfo.store_goods_type == (int)StoreForCarpenterTypeEnum.Expansion)
        {
            dialogBean.content = string.Format(GameCommonInfo.GetUITextById(3010), 1 + "");
        }
        else if (storeInfo.store_goods_type == (int)StoreForCarpenterTypeEnum.Wall)
        {
            dialogBean.content = string.Format(GameCommonInfo.GetUITextById(3013), buildItemData.name + "");
        }
        else
            dialogBean.content = string.Format(GameCommonInfo.GetUITextById(3002), buildItemData.name);

        dialogManager.CreateDialog(DialogEnum.Normal, this, dialogBean);
    }

    #region 确认回调
    public void Submit(DialogView dialogView, DialogBean dialogData)
    {
        GameDataManager gameDataManager = GetUIManager<UIGameManager>().gameDataManager;
        ToastManager toastManager = GetUIManager<UIGameManager>().toastManager;
        GameTimeHandler gameTimeHandler = GetUIManager<UIGameManager>().gameTimeHandler;
        InnBuildManager innBuildManager = GetUIManager<UIGameManager>().innBuildManager;

        gameDataManager.gameData.PayMoney(storeInfo.price_l, storeInfo.price_m, storeInfo.price_s);
        string toastStr;
        if (storeInfo.store_goods_type == (int)StoreForCarpenterTypeEnum.Expansion)
        {
            InnBuildBean innBuildData = gameDataManager.gameData.GetInnBuildData();
            innBuildData.buildLevel = int.Parse(storeInfo.mark);
            innBuildData.buildInnWidth = storeInfo.mark_x;
            innBuildData.buildInnHeight = storeInfo.mark_y;
            //设置修建天数
            List<TimeBean> listBuildDay = new List<TimeBean>();
            listBuildDay.Add(gameTimeHandler.GetAfterDay(1));
            innBuildData.listBuildDay = listBuildDay;

            GetUIComponent<UITownCarpenter>().RefreshUI();
            toastStr = string.Format(GameCommonInfo.GetUITextById(1011), storeInfo.name);
        }
        else
        {
            gameDataManager.gameData.AddBuildNumber(buildItemData.id, 1);
            RefreshUI();
            toastStr = string.Format(GameCommonInfo.GetUITextById(1010), buildItemData.name);
        }
        toastManager.ToastHint(ivIcon.sprite, toastStr);
    }

    public void Cancel(DialogView dialogView, DialogBean dialogData)
    {

    }
    #endregion
}