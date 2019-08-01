using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class ItemGameGroceryCpt : ItemGameBaseCpt, DialogView.IDialogCallBack
{
    [Header("控件")]
    public GameObject objCook;
    public Text tvCook;
    public GameObject objSpeed;
    public Text tvSpeed;
    public GameObject objAccount;
    public Text tvAccount;
    public GameObject objCharm;
    public Text tvCharm;
    public GameObject objForce;
    public Text tvForce;
    public GameObject objLucky;
    public Text tvLucky;
    public GameObject objCookBook;
    public Text tvCookBook;

    public RectTransform rtIcon;
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
    public GameObject objGuildCoin;
    public Text tvGuildCoin;


    [Header("数据")]
    public StoreInfoBean storeInfo;
    public ItemsInfoBean itemsInfo;

    private void Start()
    {
        if (btSubmit != null)
            btSubmit.onClick.AddListener(SubmitBuy);
    }


    public void SetData(StoreInfoBean storeInfo)
    {
        this.storeInfo = storeInfo;
        this.itemsInfo = GetUIManager<UIGameManager>().gameItemsManager.GetItemsById(storeInfo.mark_id);
        if (itemsInfo == null || storeInfo == null)
            return;
        SetIcon(storeInfo.icon_key, storeInfo.mark, storeInfo.mark_id);
        SetPrice(storeInfo.price_l, storeInfo.price_m, storeInfo.price_s, storeInfo.guild_coin);
        SetName(itemsInfo.name);
        SetContent(itemsInfo.content);
        SetOwn();
        int cookBookNumber = itemsInfo.items_type == (int)GeneralEnum.Menu ? 1 : 0;
        SetAttribute(
            cookBookNumber,
            itemsInfo.add_cook,
            itemsInfo.add_speed,
            itemsInfo.add_account,
            itemsInfo.add_charm,
            itemsInfo.add_force,
            itemsInfo.add_lucky);
    }

    public void RefreshUI()
    {
        SetOwn();
    }

    /// <summary>
    /// 设置图标
    /// </summary>
    /// <param name="iconKey"></param>
    /// <param name="mark"></param>
    /// <param name="markId"></param>
    public void SetIcon(string iconKey, string mark, long markId)
    {
        GameItemsManager gameItemsManager = GetUIManager<UIGameManager>().gameItemsManager;
        CharacterDressManager characterDressManager = GetUIManager<UIGameManager>().characterDressManager;
        if (gameItemsManager == null)
            return;
        Sprite spIcon = null;
        Vector2 offsetMin = new Vector2(0, 0);
        Vector2 offsetMax = new Vector2(0, 0);
        switch (mark)
        {
            case "1":
                spIcon = characterDressManager.GetHatSpriteByName(iconKey);
                offsetMin = new Vector2(-50, -100);
                offsetMax = new Vector2(50, 0);
                break;
            case "2":
                spIcon = characterDressManager.GetClothesSpriteByName(iconKey);
                offsetMin = new Vector2(-50, -25);
                offsetMax = new Vector2(50, 75);
                break;
            case "3":
                spIcon = characterDressManager.GetShoesSpriteByName(iconKey);
                offsetMin = new Vector2(-50, 0);
                offsetMax = new Vector2(50, 100);
                break;
            default:
                spIcon = gameItemsManager.GetItemsSpriteByName(iconKey);
                break;
        }
        if (ivIcon != null && spIcon != null)
            ivIcon.sprite = spIcon;
        if (rtIcon != null)
        {
            rtIcon.offsetMin = offsetMin;
            rtIcon.offsetMax = offsetMax;
        }

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
    public void SetContent(string content)
    {
        if (tvContent != null)
            tvContent.text = content;
    }

    /// <summary>
    /// 设置价格
    /// </summary>
    public void SetPrice(long priceL, long priceM, long priceS, long coin)
    {
        if (priceL == 0 && objPriceL != null)
            objPriceL.SetActive(false);
        if (priceM == 0 && objPriceM != null)
            objPriceM.SetActive(false);
        if (priceS == 0 && objPriceS != null)
            objPriceS.SetActive(false);
        if (coin == 0 && objGuildCoin != null)
            objGuildCoin.SetActive(false);
        if (tvPriceL != null)
            tvPriceL.text = priceL + "";
        if (tvPriceM != null)
            tvPriceM.text = priceM + "";
        if (tvPriceS != null)
            tvPriceS.text = priceS + "";
        if (tvGuildCoin != null)
            tvGuildCoin.text = coin + "";
    }

    /// <summary>
    /// 设置拥有数量
    /// </summary>
    public void SetOwn()
    {
        if (tvOwn == null)
            return;
        tvOwn.text = (GameCommonInfo.GetUITextById(4001) + GetUIManager<UIGameManager>().gameDataManager.gameData.GetItemsNumber(storeInfo.mark_id));
    }

    /// <summary>
    /// 设置属性
    /// </summary>
    /// <param name="add_cook"></param>
    /// <param name="add_speed"></param>
    /// <param name="add_account"></param>
    /// <param name="add_charm"></param>
    /// <param name="add_force"></param>
    /// <param name="add_lucky"></param>
    public void SetAttribute(int add_book, int add_cook, int add_speed, int add_account, int add_charm, int add_force, int add_lucky)
    {
        if (objCook != null && add_cook == 0)
            objCook.SetActive(false);
        if (objSpeed != null && add_speed == 0)
            objSpeed.SetActive(false);
        if (objAccount != null && add_account == 0)
            objAccount.SetActive(false);
        if (objCharm != null && add_charm == 0)
            objCharm.SetActive(false);
        if (objForce != null && add_force == 0)
            objForce.SetActive(false);
        if (objLucky != null && add_lucky == 0)
            objLucky.SetActive(false);
        if (objCookBook != null && add_book == 0)
            objCookBook.SetActive(false);
        if (tvCook != null)
            tvCook.text = GameCommonInfo.GetUITextById(1) + "+" + add_cook;
        if (tvSpeed != null)
            tvSpeed.text = GameCommonInfo.GetUITextById(2) + "+" + add_speed;
        if (tvAccount != null)
            tvAccount.text = GameCommonInfo.GetUITextById(3) + "+" + add_account;
        if (tvCharm != null)
            tvCharm.text = GameCommonInfo.GetUITextById(4) + "+" + add_charm;
        if (tvForce != null)
            tvForce.text = GameCommonInfo.GetUITextById(5) + "+" + add_force;
        if (tvLucky != null)
            tvLucky.text = GameCommonInfo.GetUITextById(6) + "+" + add_lucky;
        if (tvCookBook != null)
            tvCookBook.text = GameCommonInfo.GetUITextById(7) + "+" + add_book;
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
        dialogBean.content = string.Format(GameCommonInfo.GetUITextById(3002), itemsInfo.name);
        dialogManager.CreateDialog(0, this, dialogBean);
    }

    #region 提交回调
    public virtual void Submit(DialogView dialogView)
    {
        GameDataManager gameDataManager = GetUIManager<UIGameManager>().gameDataManager;
        ToastView toastView = GetUIManager<UIGameManager>().toastView;
        if (gameDataManager == null || storeInfo == null)
            return;
        if (!gameDataManager.gameData.HasEnoughMoney(storeInfo.price_l, storeInfo.price_m, storeInfo.price_s))
        {
            toastView.ToastHint(GameCommonInfo.GetUITextById(1005));
            return;
        }
        gameDataManager.gameData.PayMoney(storeInfo.price_l, storeInfo.price_m, storeInfo.price_s);
        toastView.ToastHint(ivIcon.sprite, string.Format(GameCommonInfo.GetUITextById(1010), itemsInfo.name));
        gameDataManager.gameData.AddNewItems(storeInfo.mark_id, 1);
        RefreshUI();
    }

    public virtual void Cancel(DialogView dialogView)
    {

    }
    #endregion
}