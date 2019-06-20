using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class ItemGameGoodsMarketCpt : BaseMonoBehaviour
{
    public Image ivIcon;
    public Text tvName;
    public Text tvPirce;
    public InputField etNumber;
    public Button btNumberAdd;
    public Button btNumberSub;
    public Button btSubmit;
    public Text tvOwn;

    public StoreInfoBean goodsData;
    public GameDataManager gameDataManager;
    public ToastView toastView;

    private void Start()
    {
        if (btSubmit != null)
            btSubmit.onClick.AddListener(BuyGoods);
        if (btNumberAdd != null)
            btNumberAdd.onClick.AddListener(AddGoodsNumber);
        if (btNumberSub != null)
            btNumberSub.onClick.AddListener(SubGoodsNumber);
        if (etNumber != null)
            etNumber.onValueChanged.AddListener(InputNumberChange);
    }

    public void RreshData()
    {
        SetOwn();
    }

    public void SetOwn()
    {
        if (goodsData == null|| tvOwn==null)
            return;
        int ingType = int.Parse(goodsData.mark);
        long ownNumber = 0;
        switch (ingType)
        {
            case 1:
                ownNumber = gameDataManager.gameData.ingOilsalt;
                break;
            case 2:
                ownNumber = gameDataManager.gameData.ingMeat;
                break;
            case 3:
                ownNumber = gameDataManager.gameData.ingRiverfresh;
                break;
            case 4:
                ownNumber = gameDataManager.gameData.ingSeafood;
                break;
            case 5:
                ownNumber = gameDataManager.gameData.ingVegetables;
                break;
            case 6:
                ownNumber = gameDataManager.gameData.ingMelonfruit;
                break;
            case 7:
                ownNumber = gameDataManager.gameData.ingWaterwine;
                break;
            case 8:
                ownNumber = gameDataManager.gameData.ingFlour;
                break;
        }
        tvOwn.text = ownNumber + "";
    }

    public void BuyGoods()
    {
        int ingType = int.Parse(goodsData.mark);
        int buyNumber = int.Parse(etNumber.text);
        if (buyNumber<=0)
        {
            toastView.ToastHint("至少需要购买1件商品！");
            return;
        }
        if (!gameDataManager.gameData.HasEnoughMoney(goodsData.price_l* buyNumber, goodsData.price_m * buyNumber, goodsData.price_s * buyNumber))
        {
            toastView.ToastHint("没有足够的金钱！");
            return;
        }
        gameDataManager.gameData.PayMoney(goodsData.price_l * buyNumber, goodsData.price_m * buyNumber, goodsData.price_s * buyNumber);
        switch (ingType)
        {
            case 1:
                gameDataManager.gameData.ingOilsalt+= buyNumber;
                break;
            case 2:
                gameDataManager.gameData.ingMeat += buyNumber;
                break;
            case 3:
                 gameDataManager.gameData.ingRiverfresh += buyNumber;
                break;
            case 4:
                gameDataManager.gameData.ingSeafood += buyNumber;
                break;
            case 5:
                gameDataManager.gameData.ingVegetables += buyNumber;
                break;
            case 6:
                gameDataManager.gameData.ingMelonfruit += buyNumber;
                break;
            case 7:
                gameDataManager.gameData.ingWaterwine += buyNumber;
                break;
            case 8:
                gameDataManager.gameData.ingFlour += buyNumber;
                break;
        }
        RreshData();
        toastView.ToastHint(ivIcon.sprite, "购入 "+ buyNumber +"份"+ goodsData.name+" 共花费 "+tvPirce.text +"文！");
    }

    /// <summary>
    ///  减少商品数量
    /// </summary>
    public void SubGoodsNumber()
    {
        SetNumber(int.Parse(etNumber.text) - 1);
    }

    /// <summary>
    /// 增加商品数量
    /// </summary>
    public void AddGoodsNumber()
    {
        SetNumber(int.Parse(etNumber.text) + 1);
    }

    /// <summary>
    /// 输入改变
    /// </summary>
    /// <param name="value"></param>
    public void InputNumberChange(string value)
    {
        if (CheckUtil.StringIsNull(value))
        {
            etNumber.text = "0";
        }
        else
        {
            SetPrice(goodsData.price_l, goodsData.price_m, goodsData.price_s, int.Parse(value));
        }
    }

    public void SetData(StoreInfoBean goodsData,Sprite spIcon) {
        if (goodsData == null)
            return;
        this.goodsData = goodsData;
        SetName(goodsData.name);
        SetIcon(spIcon);
        SetPrice(goodsData.price_l, goodsData.price_m, goodsData.price_s,1);
        SetNumber(1);
        RreshData();
    }

    public void SetIcon(Sprite spIcon)
    {
        if (ivIcon != null)
            ivIcon.sprite = spIcon;
    }

    public void SetName(string name)
    {
        if (tvName != null)
            tvName.text = name;
    }

    public void SetPrice(long price_l,long price_m,long price_s,long number)
    {
        if (tvPirce != null)
            tvPirce.text= price_s * number+ "";
    }

    public void SetNumber(int number)
    {
        if (etNumber != null)
        {
            if (number < 0)
                number = 0;
            etNumber.text = number + "";
        }
    }
}