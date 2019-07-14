using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class ItemGameGroceryCpt : BaseMonoBehaviour, DialogView.IDialogCallBack
{
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

    public GameDataManager gameDataManager;
    public CharacterDressManager characterDressManager;
    public GameItemsManager gameItemsManager;

    public ToastView toastView;
    public DialogManager dialogManager;

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
        this.itemsInfo = gameItemsManager.GetItemsById(storeInfo.mark_id);
        if (itemsInfo == null || storeInfo == null)
            return;
        SetIcon(storeInfo.icon_key, storeInfo.mark, storeInfo.mark_id);
        SetPrice(storeInfo.price_l, storeInfo.price_m, storeInfo.price_s);
        SetName(itemsInfo.name);
        SetContent(itemsInfo.content);
        SetOwn();
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

        if (gameItemsManager == null)
            return;
        Sprite spIcon = null;
        Vector2 offsetMin = new Vector2(0,0);
        Vector2 offsetMax = new Vector2(0, 0);
        switch (mark)
        {
            case "1":
                spIcon = characterDressManager.GetHatSpriteByName(iconKey);
                offsetMin = new Vector2(-50, -100);
                offsetMax = new Vector2(50, 0 );
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
        if (tvOwn == null)
            return;
        tvOwn.text = ("拥有\n" + gameDataManager.gameData.GetItemsNumber(storeInfo.mark_id));
    }

    /// <summary>
    /// 购买确认
    /// </summary>
    public void SubmitBuy()
    {
        if (gameDataManager == null || storeInfo == null)
            return;
        if (!gameDataManager.gameData.HasEnoughMoney(storeInfo.price_l, storeInfo.price_m, storeInfo.price_s))
        {
            toastView.ToastHint(GameCommonInfo.GetUITextById(1005));
            return;
        }
        DialogBean dialogBean = new DialogBean();
        dialogBean.content =string.Format(GameCommonInfo.GetUITextById(3002), itemsInfo.name);
        dialogManager.CreateDialog(0, this, dialogBean);
    }

    #region 提交回调
    public void Submit(DialogView dialogView)
    {
        if (gameDataManager == null || storeInfo == null)
            return;
        if (!gameDataManager.gameData.HasEnoughMoney(storeInfo.price_l, storeInfo.price_m, storeInfo.price_s))
        {
            toastView.ToastHint(GameCommonInfo.GetUITextById(1005));
            return;
        }
        gameDataManager.gameData.PayMoney(storeInfo.price_l, storeInfo.price_m, storeInfo.price_s);
        toastView.ToastHint(string.Format(GameCommonInfo.GetUITextById(1010), itemsInfo.name));
        ItemBean itemBean = new ItemBean
        {
            itemId = storeInfo.mark_id,
            itemNumber = 1
        };
        gameDataManager.gameData.itemsList.Add(itemBean);
        RefreshUI();
    }

    public void Cancel(DialogView dialogView)
    {

    }
    #endregion
}