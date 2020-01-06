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

    private void Start()
    {
        if (btSubmit != null)
            btSubmit.onClick.AddListener(SubmitBuy);
    }

    public void RefreshUI()
    {
        SetOwn(storeInfo.mark_type);
    }

    public void SetData(StoreInfoBean itemData)
    {
        InnBuildManager innBuildManager = GetUIManager<UIGameManager>().innBuildManager;

        storeInfo = itemData;
        int aesthetics = 0;
        string iconKey = "";
        string name = "";
        string content = "";
        if (storeInfo.mark_type == 0)
        {
            iconKey = storeInfo.icon_key;
            name = storeInfo.name;
            content = storeInfo.content;
        }
        else
        {
            buildItemData = innBuildManager.GetBuildDataById(itemData.mark_id);
            aesthetics = buildItemData.aesthetics;
            iconKey = buildItemData.icon_key;
            name = buildItemData.name;
            content = buildItemData.content;
        }
        SetPrice(storeInfo.price_l, storeInfo.price_m, storeInfo.price_s);
        SetName(name);
        SetIcon(storeInfo.mark_type, iconKey);
        SetAttribute(storeInfo.mark_type, aesthetics);
        SetContent(storeInfo.mark_type, content);
        SetOwn(storeInfo.mark_type);
    }

    /// <summary>
    /// 设置属性
    /// </summary>
    /// <param name="type"></param>
    /// <param name="a"></param>
    public void SetAttribute(int type, int aesthetics)
    {
        if (type == 0)
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
    public void SetIcon(int type, string iconKey)
    {
        GameItemsManager gameItemsManager = GetUIManager<UIGameManager>().gameItemsManager;
        InnBuildManager innBuildManager = GetUIManager<UIGameManager>().innBuildManager;
        if (gameItemsManager == null)
            return;
        Sprite spIcon = null;
        if (type == 0)
        {
            spIcon = gameItemsManager.GetItemsSpriteByName(iconKey);
        }
        else
        {
            spIcon = innBuildManager.GetFurnitureSpriteByName(iconKey);
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
    public void SetContent(int type, string content)
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
    public void SetOwn(int type)
    {
        if (type == 0)
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
        GameDataManager gameDataManager = GetUIManager<UIGameManager>().gameDataManager;
        ToastManager toastManager = GetUIManager<UIGameManager>().toastManager;
        DialogManager dialogManager = GetUIManager<UIGameManager>().dialogManager;
        InnBuildBean innBuildData = gameDataManager.gameData.GetInnBuildData();
        if (gameDataManager == null || storeInfo == null)
            return;
        if (!gameDataManager.gameData.HasEnoughMoney(storeInfo.price_l, storeInfo.price_m, storeInfo.price_s))
        {
            toastManager.ToastHint(GameCommonInfo.GetUITextById(1005));
            return;
        }
        if (storeInfo.mark_type == 0 && innBuildData.listBuildDay.Count != 0)
        {
            toastManager.ToastHint(GameCommonInfo.GetUITextById(1019));
            return;
        }
        DialogBean dialogBean = new DialogBean();
        if (storeInfo.mark_type == 0)
        {
            dialogBean.content = string.Format(GameCommonInfo.GetUITextById(3010), 1 + "");
        }
        else
            dialogBean.content = string.Format(GameCommonInfo.GetUITextById(3002), buildItemData.name);

        dialogManager.CreateDialog(0, this, dialogBean);
    }

    #region 确认回调
    public void Submit(DialogView dialogView, DialogBean dialogData)
    {
        GameDataManager gameDataManager = GetUIManager<UIGameManager>().gameDataManager;
        ToastManager toastManager = GetUIManager<UIGameManager>().toastManager;
        GameTimeHandler gameTimeHandler = GetUIManager<UIGameManager>().gameTimeHandler;

        gameDataManager.gameData.PayMoney(storeInfo.price_l, storeInfo.price_m, storeInfo.price_s);
        string toastStr;
        if (storeInfo.mark_type == 0)
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
            gameDataManager.gameData.ChangeBuildNumber(buildItemData.id, 1);
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