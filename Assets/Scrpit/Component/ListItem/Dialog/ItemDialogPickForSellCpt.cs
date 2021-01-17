using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class ItemDialogPickForSellCpt : BaseMonoBehaviour, DialogView.IDialogCallBack
{
    public Image ivIcon;
    public Text tvName;
    public Text tvOwn;
    public Button btSub;
    public Button btAdd;
    public Button btSell;
    public InputField etSellNumber;
    public PriceShowView priceShowView;

    protected GameItemsManager gameItemsManager;
    protected InnBuildManager innBuildManager;
    protected IconDataManager iconDataManager;
    protected CharacterDressManager characterDressManager;
    protected GameDataManager gameDataManager;
    protected DialogManager dialogManager;
    public ItemBean itemData;
    public StoreInfoBean storeInfo;

    public long sellNumber = 1;
    protected int sellRate = 3;

    public void Awake()
    {
        gameDataManager = Find<GameDataManager>(ImportantTypeEnum.GameDataManager);
        gameItemsManager = Find<GameItemsManager>(ImportantTypeEnum.GameItemsManager);
        innBuildManager = Find<InnBuildManager>(ImportantTypeEnum.BuildManager);
        iconDataManager = Find<IconDataManager>(ImportantTypeEnum.UIManager);
        characterDressManager = Find<CharacterDressManager>(ImportantTypeEnum.CharacterManager);
        dialogManager = Find<DialogManager>(ImportantTypeEnum.DialogManager);
        if (etSellNumber)
            etSellNumber.onEndEdit.AddListener(OnEndEditForNumber);
        if (btSub)
            btSub.onClick.AddListener(OnClickForSub);
        if (btAdd)
            btAdd.onClick.AddListener(OnClickForAdd);
        if (btSell)
            btSell.onClick.AddListener(OnClickForSell);
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="itemData"></param>
    /// <param name="storeInfo"></param>
    public void SetData(ItemBean itemData, StoreInfoBean storeInfo)
    {
        this.itemData = itemData;
        this.storeInfo = storeInfo;
        Sprite spIcon = null;
        string name = "???";
        if (storeInfo.mark_type == 1)
        {
            ItemsInfoBean itemsInfo = GameItemsHandler.Instance.manager.GetItemsById(itemData.itemId);

            spIcon = GeneralEnumTools.GetGeneralSprite(itemsInfo, iconDataManager);
            if (itemsInfo != null)
            {
                name = itemsInfo.name;
            }
        }
        else if (storeInfo.mark_type == 2)
        {
            BuildItemBean buildItem = innBuildManager.GetBuildDataById(itemData.itemId);
            spIcon = BuildItemTypeEnumTools.GetBuildItemSprite(innBuildManager, buildItem);
            if (buildItem != null)
            {
                name = buildItem.name;
            }
        }
        SetIcon(spIcon);
        SetPrice(sellNumber, storeInfo);
        SetName(name);
        SetOwn();
        SetSellNumber(sellNumber);
    }

    /// <summary>
    /// 刷新数据
    /// </summary>
    public void RefreshItem()
    {
        if (itemData == null || itemData.itemNumber <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            SetOwn();
            SetSellNumber(sellNumber);
        }
    }

    /// <summary>
    /// 设置图标
    /// </summary>
    /// <param name="spIcon"></param>
    public void SetIcon(Sprite spIcon)
    {
        if (ivIcon != null)
        {
            ivIcon.sprite = spIcon;
        }
    }

    /// <summary>
    /// 设置价格
    /// </summary>
    /// <param name="number"></param>
    /// <param name="storeInfo"></param>
    public void SetPrice(long number, StoreInfoBean storeInfo)
    {

        if (priceShowView != null)
            priceShowView.SetPrice(number,
                   storeInfo.price_l / sellRate, storeInfo.price_m / sellRate, storeInfo.price_s / sellRate,
                   storeInfo.guild_coin / sellRate,
                   storeInfo.trophy_elementary / sellRate, storeInfo.trophy_intermediate / sellRate, storeInfo.trophy_advanced / sellRate, storeInfo.trophy_legendary / sellRate);
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
    /// 设置拥有数量
    /// </summary>
    public virtual void SetOwn()
    {
        if (tvOwn == null)
            return;
        tvOwn.text = (GameCommonInfo.GetUITextById(4001) + "\n" + itemData.itemNumber);
    }

    /// <summary>
    /// 设置出售数量
    /// </summary>
    /// <param name="number"></param>
    public void SetSellNumber(long number)
    {
        if (number < 1)
        {
            number = 1;
        }
        else if (number > itemData.itemNumber)
        {
            number = itemData.itemNumber;
        }
        sellNumber = number;
        if (etSellNumber != null)
        {
            etSellNumber.text = number + "";
        }
        SetPrice(sellNumber, storeInfo);
    }

    public void OnClickForAdd()
    {
        SetSellNumber(sellNumber + 1);
    }
    public void OnClickForSub()
    {
        SetSellNumber(sellNumber - 1);
    }

    //点击售卖
    public void OnClickForSell()
    {
        DialogBean dialogData = new DialogBean();
        dialogData.content = string.Format(GameCommonInfo.GetUITextById(3102), tvName.text, sellNumber + "");
        DialogHandler.Instance.CreateDialog<DialogView>(DialogEnum.Normal, this, dialogData);
    }

    /// <summary>
    /// 数值修改监听
    /// </summary>
    /// <param name="numberStr"></param>
    public void OnEndEditForNumber(string numberStr)
    {
        if (long.TryParse(numberStr, out long number))
        {
            SetSellNumber(number);
        }
        else
        {
            SetSellNumber(sellNumber);
        }
    }

    #region 确认回调
    public void Submit(DialogView dialogView, DialogBean dialogBean)
    {
        gameDataManager.gameData.AddArenaTrophy
            (sellNumber * (storeInfo.trophy_elementary / sellRate),
            sellNumber * (storeInfo.trophy_intermediate / sellRate),
            sellNumber * (storeInfo.trophy_advanced / sellRate),
            sellNumber * (storeInfo.trophy_legendary / sellRate));
        gameDataManager.gameData.AddGuildCoin(sellNumber * (storeInfo.guild_coin / sellRate));
        gameDataManager.gameData.AddMoney
            (sellNumber * (storeInfo.price_l / sellRate),
            sellNumber * (storeInfo.price_m / sellRate),
            sellNumber * (storeInfo.price_s / sellRate));

        if (storeInfo.mark_type == 1)
        {
            gameDataManager.gameData.AddItemsNumber(itemData.itemId, -sellNumber);
        }
        else if (storeInfo.mark_type == 2)
        {
            gameDataManager.gameData.AddBuildNumber(itemData.itemId, -sellNumber);
        }

        RefreshItem();
    }

    public void Cancel(DialogView dialogView, DialogBean dialogBean)
    {

    }
    #endregion
}