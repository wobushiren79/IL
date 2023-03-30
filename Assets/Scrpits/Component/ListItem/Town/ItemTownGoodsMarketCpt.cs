using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class ItemTownGoodsMarketCpt : ItemGameBaseCpt, DialogView.IDialogCallBack
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

        List<string> listRiseAndFall = goodsData.mark.SplitForListStr('|');
        SeasonsEnum[] listRise = new SeasonsEnum[0];
        SeasonsEnum[] listFall = new SeasonsEnum[0];
        if (listRiseAndFall.Count>=1)
        {
            listRise = listRiseAndFall[0].SplitForArrayEnum<SeasonsEnum>(',');
        }
        if (listRiseAndFall.Count >= 2)
        {
            listFall = listRiseAndFall[1].SplitForArrayEnum<SeasonsEnum>(',');
        }
        SetRiseAndFall(listRise.ToList(), listFall.ToList());

    
        SetPrice(price_l, price_m, price_s, 1);
        RreshData();
    }

    public void BuyGoods()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        int buyNumber = int.Parse(etNumber.text);
        DialogBean dialogData = new DialogBean();
        dialogData.content = string.Format(TextHandler.Instance.manager.GetTextById(3009), tvPirce.text, buyNumber, goodsData.name);
        dialogData.dialogType = DialogEnum.Normal;
        dialogData.callBack = this;
        UIHandler.Instance.ShowDialog<DialogView>(dialogData);
    }

    /// <summary>
    ///  减少商品数量
    /// </summary>
    public void SubGoodsNumber()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        SetNumber(int.Parse(etNumber.text) - 1);
    }

    /// <summary>
    /// 增加商品数量
    /// </summary>
    public void AddGoodsNumber()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        SetNumber(int.Parse(etNumber.text) + 1);
    }

    /// <summary>
    /// 输入改变
    /// </summary>
    /// <param name="value"></param>
    public void InputNumberChange(string value)
    {
        if (value.IsNull() || value.Contains("-"))
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
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();

        long ownNumber = 0;

        switch (ingType)
        {
            case IngredientsEnum.Oilsalt:
                ownNumber = gameData.ingOilsalt;
                break;
            case IngredientsEnum.Meat:
                ownNumber = gameData.ingMeat;
                break;
            case IngredientsEnum.Riverfresh:
                ownNumber = gameData.ingRiverfresh;
                break;
            case IngredientsEnum.Seafood:
                ownNumber = gameData.ingSeafood;
                break;
            case IngredientsEnum.Vegetables:
                ownNumber = gameData.ingVegetables;
                break;
            case IngredientsEnum.Melonfruit:
                ownNumber = gameData.ingMelonfruit;
                break;
            case IngredientsEnum.Waterwine:
                ownNumber = gameData.ingWaterwine;
                break;
            case IngredientsEnum.Flour:
                ownNumber = gameData.ingFlour;
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
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        SeasonsEnum currentSeasons = (SeasonsEnum)gameData.gameTime.month;
        //季节变化价格浮动
        if (listSeasonsRise.Contains(currentSeasons))
        {
            //price_l = goodsData.price_l * 2;
            //price_m = goodsData.price_m * 2;
            price_s = (long)(goodsData.price_s * 1.5f);

        }
        else if (listSeasonsFall.Contains(currentSeasons))
        {
            //price_l = goodsData.price_l / 2;
            //price_m = goodsData.price_m / 2;
            price_s = (long)(goodsData.price_s / 1.5f);
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
        if ((IngredientsEnum)goodsData.mark_type == IngredientsEnum.Seafood)
        {
            //海鲜涨幅比较高
            price_s += Random.Range(-15, 16);
        }
        else
        {
            price_s += Random.Range(-2, 3);
        }


        //好感加成
        //CharacterFavorabilityBean marketBossFavorability= gameData.GetCharacterFavorability(10001);
        //int addPriceForFavorability = (int)System.Math.Round(price_s * (marketBossFavorability.favorabilityLevel * 0.04f), 0);
        //price_s = price_s - addPriceForFavorability;

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
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        if (buyNumber <= 0)
        {
            UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.manager.GetTextById(1017));
            return;
        }
        if (!gameData.HasEnoughMoney(price_l * buyNumber, price_m * buyNumber, price_s * buyNumber))
        {
            UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.manager.GetTextById(1005));
            return;
        }
        gameData.PayMoney(price_l * buyNumber, price_m * buyNumber, price_s * buyNumber);
        gameData.AddIng(ingType, buyNumber);
        RreshData();
        UIHandler.Instance.ToastHint<ToastView>(ivIcon.sprite, string.Format(TextHandler.Instance.manager.GetTextById(1018), buyNumber, goodsData.name, tvPirce.text));
    }

    public void Cancel(DialogView dialogView, DialogBean dialogBean)
    {
    }
    #endregion
}