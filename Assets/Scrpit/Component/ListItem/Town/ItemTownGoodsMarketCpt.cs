using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class ItemTownGoodsMarketCpt : ItemGameBaseCpt,DialogView.IDialogCallBack
{
    public Image ivIcon;
    public Text tvName;
    public Text tvPirce;
    public InputField etNumber;
    public Button btNumberAdd;
    public Button btNumberSub;
    public Button btSubmit;
    public Text tvOwn;
    public Image ivRiseAndFall;

    //涨跌图标
    public Sprite spRise;
    public Sprite spNormal;
    public Sprite spFall;

    public StoreInfoBean goodsData;

    public long price_l;
    public long price_m;
    public long price_s;

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

    /// <summary>
    /// 刷新数据
    /// </summary>
    public void RreshData()
    {
        SetOwn((IngredientsEnum)goodsData.mark_type);
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="goodsData"></param>
    /// <param name="spIcon"></param>
    public void SetData(StoreInfoBean goodsData, Sprite spIcon)
    {
        if (goodsData == null)
            return;
        this.goodsData = goodsData;
        SetName(goodsData.name);
        SetIcon(spIcon);
        SetNumber(1);

        List<string> listRiseAndFall = StringUtil.SplitBySubstringForListStr(goodsData.mark, '|');
        SeasonsEnum[] listRise = StringUtil.SplitBySubstringForArrayEnum<SeasonsEnum>(listRiseAndFall[0], ',');
        SeasonsEnum[] listFall = StringUtil.SplitBySubstringForArrayEnum<SeasonsEnum>(listRiseAndFall[1], ',');
        SetRiseAndFall(listRise.ToList(), listFall.ToList());
        SetPrice(price_l, price_m, price_s, 1);
        RreshData();
    }

    public void BuyGoods()
    {
        int buyNumber = int.Parse(etNumber.text);
        DialogManager dialogManager = GetUIManager<UIGameManager>().dialogManager;
        DialogBean dialogData = new DialogBean();
        dialogData.content = string.Format(GameCommonInfo.GetUITextById(3009), tvPirce.text, buyNumber, goodsData.name);
        dialogManager.CreateDialog(DialogEnum.Normal, this, dialogData);
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
        if (CheckUtil.StringIsNull(value) || value.Contains("-"))
        {
            etNumber.text = "";
        }
        else
        {
            SetPrice(price_l, price_m, price_s, int.Parse(value));
        }
    }

    /// <summary>
    /// 设置拥有数量
    /// </summary>
    public void SetOwn(IngredientsEnum ingType)
    {
        if (goodsData == null || tvOwn == null)
            return;
        GameDataManager gameDataManager = GetUIManager<UIGameManager>().gameDataManager;

        long ownNumber = 0;

        switch (ingType)
        {
            case IngredientsEnum.Oilsalt:
                ownNumber = gameDataManager.gameData.ingOilsalt;
                break;
            case IngredientsEnum.Meat:
                ownNumber = gameDataManager.gameData.ingMeat;
                break;
            case IngredientsEnum.Riverfresh:
                ownNumber = gameDataManager.gameData.ingRiverfresh;
                break;
            case IngredientsEnum.Seafood:
                ownNumber = gameDataManager.gameData.ingSeafood;
                break;
            case IngredientsEnum.Vegetablest:
                ownNumber = gameDataManager.gameData.ingVegetables;
                break;
            case IngredientsEnum.Melonfruit:
                ownNumber = gameDataManager.gameData.ingMelonfruit;
                break;
            case IngredientsEnum.Waterwine:
                ownNumber = gameDataManager.gameData.ingWaterwine;
                break;
            case IngredientsEnum.Flour:
                ownNumber = gameDataManager.gameData.ingFlour;
                break;
        }
        tvOwn.text = ownNumber + "";
    }

    /// <summary>
    /// 设置图标
    /// </summary>
    /// <param name="spIcon"></param>
    public void SetIcon(Sprite spIcon)
    {
        if (ivIcon != null)
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
    /// 设置价格
    /// </summary>
    /// <param name="price_l"></param>
    /// <param name="price_m"></param>
    /// <param name="price_s"></param>
    /// <param name="number"></param>
    public void SetPrice(long price_l, long price_m, long price_s, long number)
    {
        if (tvPirce != null)
            tvPirce.text = price_s * number + "";
    }

    /// <summary>
    /// 设置数量
    /// </summary>
    /// <param name="number"></param>
    public void SetNumber(int number)
    {
        if (etNumber != null)
        {
            if (number < 0)
                number = 0;
            etNumber.text = number + "";
        }
    }

    /// <summary>
    /// 设置涨跌
    /// </summary>
    public void SetRiseAndFall(List<SeasonsEnum> listSeasonsRise, List<SeasonsEnum> listSeasonsFall)
    {
        GameDataManager gameDataManager = GetUIManager<UIGameManager>().gameDataManager;
        SeasonsEnum currentSeasons = (SeasonsEnum)gameDataManager.gameData.gameTime.month;
        //季节变化价格浮动
        if (listSeasonsRise.Contains(currentSeasons))
        {
            //price_l = goodsData.price_l * 2;
            //price_m = goodsData.price_m * 2;
            price_s = goodsData.price_s * 2;

        }
        else if (listSeasonsFall.Contains(currentSeasons))
        {
            //price_l = goodsData.price_l / 2;
            //price_m = goodsData.price_m / 2;
            price_s = goodsData.price_s / 2;
        }
        else
        {
            //price_l = goodsData.price_l;
            //price_m = goodsData.price_m;
            price_s = goodsData.price_s;
        }

        //随机浮动
        //price_l += Random.Range(-1, 2);
        //price_m += Random.Range(-1, 2);
        price_s += Random.Range(-2, 3);

        //好感加成
        CharacterFavorabilityBean marketBossFavorability= gameDataManager.gameData.GetCharacterFavorability(10001);
        int addPriceForFavorability = (int)System.Math.Round(price_s * (marketBossFavorability.favorabilityLevel * 0.04f), 0);
        price_s = price_s - addPriceForFavorability;

        //最低价格控制
        if (price_s < 1)
            price_s = 1;

        if (price_l > goodsData.price_l || price_m > goodsData.price_m || price_s > goodsData.price_s)
        {
            ivRiseAndFall.sprite = spRise;
        }
        else if (price_l < goodsData.price_l || price_m < goodsData.price_m || price_s < goodsData.price_s)
        {
            ivRiseAndFall.sprite = spFall;
        }
        else
        {
            ivRiseAndFall.sprite = spNormal;
        }
    }

    #region 弹窗回调
    public void Submit(DialogView dialogView, DialogBean dialogBean)
    {
        int buyNumber = int.Parse(etNumber.text);
        IngredientsEnum ingType = (IngredientsEnum)goodsData.mark_type;
        GameDataManager gameDataManager = GetUIManager<UIGameManager>().gameDataManager;
        ToastManager toastManager = GetUIManager<UIGameManager>().toastManager;
        if (buyNumber <= 0)
        {
            toastManager.ToastHint(GameCommonInfo.GetUITextById(1017));
            return;
        }
        if (!gameDataManager.gameData.HasEnoughMoney(price_l * buyNumber, price_m * buyNumber, price_s * buyNumber))
        {
            toastManager.ToastHint(GameCommonInfo.GetUITextById(1005));
            return;
        }
        gameDataManager.gameData.PayMoney(price_l * buyNumber, price_m * buyNumber, price_s * buyNumber);
        gameDataManager.gameData.AddIng(ingType, buyNumber);
        RreshData();
        toastManager.ToastHint(ivIcon.sprite, string.Format(GameCommonInfo.GetUITextById(1018), buyNumber, goodsData.name, tvPirce.text));
    }

    public void Cancel(DialogView dialogView, DialogBean dialogBean)
    {
    }
    #endregion
}