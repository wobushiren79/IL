using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;

public class ItemGameCerpenterCpt : ItemGameBaseCpt, DialogView.IDialogCallBack
{
    public Image ivIcon;
    public Text tvName;
    public Text tvContent;
    public Text tvOwn;
    public Button btSubmit;

    public GameObject objPriceL;
    public Text tvPriceL;
    public GameObject objPriceM;
    public Text tvPriceM;
    public GameObject objPriceS;
    public Text tvPriceS;

    public StoreInfoBean storeInfo;
    public BuildItemBean buildItemData;

    private void Start()
    {
        if (btSubmit != null)
            btSubmit.onClick.AddListener(SubmitBuy);
    }

    public void RefreshUI()
    {
        SetOwn();
    }

    public void SetData(StoreInfoBean itemData)
    {
        InnBuildManager innBuildManager= GetUIManager<UIGameManager>().innBuildManager;
        storeInfo = itemData;
        buildItemData= innBuildManager.GetBuildDataById(itemData.mark_id);
        SetPrice(storeInfo.price_l, storeInfo.price_m, storeInfo.price_s);

        if (storeInfo.id > 300000 && storeInfo.id <= 300010)
        {
            SetIcon(storeInfo.id, storeInfo.icon_key);
            SetName(storeInfo.name);
            SetContent(storeInfo.id, storeInfo.content, storeInfo.mark_x + "", storeInfo.mark_y + "");
        }
        else
        {
            SetIcon(storeInfo.id, buildItemData.icon_key);
            SetName(buildItemData.name);
            SetContent(storeInfo.id, buildItemData.content, storeInfo.mark_x + "", storeInfo.mark_y + "");
        }
           
        SetOwn();
    }


    /// <summary>
    /// 设置图标
    /// </summary>
    /// <param name="iconKey"></param>
    /// <param name="mark"></param>
    /// <param name="markId"></param>
    public void SetIcon(long id,string iconKey)
    {
        GameItemsManager gameItemsManager = GetUIManager<UIGameManager>().gameItemsManager;
        InnBuildManager innBuildManager = GetUIManager<UIGameManager>().innBuildManager;
        if (gameItemsManager == null)
            return;
        Sprite spIcon = null;
        if (id > 300000 && id < 300010)
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
    public void SetContent(long id, string content, string markX, string markY)
    {
        if (tvContent != null)
        {
            if (id > 300000 && id < 300010)
                tvContent.text = string.Format(content, markX, markY);
            else
                tvContent.text = content;
        }
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
    public void SetOwn()
    {
        GameDataManager gameDataManager = GetUIManager<UIGameManager>().gameDataManager;
        if (tvOwn == null)
            return;
        tvOwn.text = (GameCommonInfo.GetUITextById(4001) +"\n"+ gameDataManager.gameData.GetBuildNumber(storeInfo.mark_id));
    }

    /// <summary>
    /// 购买确认
    /// </summary>
    public void SubmitBuy()
    {
        GameDataManager gameDataManager = GetUIManager<UIGameManager>().gameDataManager;
        ToastView toastView = GetUIManager<UIGameManager>().toastView;
        DialogManager dialogManager = GetUIManager<UIGameManager>().dialogManager;
        if (gameDataManager == null || storeInfo == null)
            return;
        if (!gameDataManager.gameData.HasEnoughMoney(storeInfo.price_l, storeInfo.price_m, storeInfo.price_s))
        {
            toastView.ToastHint(GameCommonInfo.GetUITextById(1005));
            return;
        }
        DialogBean dialogBean = new DialogBean();
        if (storeInfo.id > 300000 && storeInfo.id < 300010)
            dialogBean.content = string.Format(GameCommonInfo.GetUITextById(3003), storeInfo.name);
        else
            dialogBean.content = string.Format(GameCommonInfo.GetUITextById(3002), buildItemData.name);

        dialogManager.CreateDialog(0, this, dialogBean);
    }

    #region 确认回调
    public void Submit(DialogView dialogView, DialogBean dialogData)
    {
        GameDataManager gameDataManager = GetUIManager<UIGameManager>().gameDataManager;
        ToastView toastView = GetUIManager<UIGameManager>().toastView;

        gameDataManager.gameData.PayMoney(storeInfo.price_l, storeInfo.price_m, storeInfo.price_s);
        string toastStr ;
        if (storeInfo.id > 300000 && storeInfo.id < 300010)
        {  
            gameDataManager.gameData.GetInnBuildData().buildLevel = int.Parse(storeInfo.mark);
            gameDataManager.gameData.GetInnBuildData().innWidth = storeInfo.mark_x;
            gameDataManager.gameData.GetInnBuildData().innHeight = storeInfo.mark_y;
            gameDataManager.gameData.GetInnBuildData().InitFloor();
            gameDataManager.gameData.GetInnBuildData().InitWall();
            GetUIComponent<UITownCarpenter>().RefreshUI();
            toastStr = string.Format(GameCommonInfo.GetUITextById(1011), storeInfo.name);
        }
        else
        {
            gameDataManager.gameData.ChangeBuildNumber(buildItemData.id,1);
            RefreshUI();
            toastStr = string.Format(GameCommonInfo.GetUITextById(1010), buildItemData.name);
        }
        toastView.ToastHint(ivIcon.sprite, toastStr);
    }

    public void Cancel(DialogView dialogView, DialogBean dialogData)
    {

    }
    #endregion
}