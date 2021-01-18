using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class UIGameMenu : BaseUIComponent, TextSearchView.ICallBack
{
    //返回按钮
    [Header("控件")]
    public Button btBack;
    public Text tvOilsalt;
    public Text tvMeat;
    public Text tvRiverfresh;
    public Text tvSeafood;
    public Text tvVegetables;
    public Text tvMelonfruit;
    public Text tvWaterwine;
    public Text tvflour;

    public ScrollGridVertical gridVertical;

    [Header("排序")]
    public Button btSortDef;
    public Button btSortPrice;
    public Button btSortName;
    public Button btSortRarity;
    public Button btSortSell;
    public Button btSortLevel;
    public Button btSortLevelUp;
    public Button btSortTime;

    public Button btSellAll;
    public Button btStopAll;
    public TextSearchView textSearchView;

    public List<MenuOwnBean> listMenu = new List<MenuOwnBean>();

    private void Start()
    {
        if (btBack != null)
            btBack.onClick.AddListener(OpenMainUI);
        if (btSortDef != null)
            btSortDef.onClick.AddListener(OnClickForSortDef);
        if (btSortName != null)
            btSortName.onClick.AddListener(OnClickForSortName);
        if (btSortRarity != null)
            btSortRarity.onClick.AddListener(OnClickForSortRarity);
        if (btSortSell != null)
            btSortSell.onClick.AddListener(OnClickForSortSell);
        if (btSortLevel != null)
            btSortLevel.onClick.AddListener(OnClickForSortLevel);
        if (btSortLevelUp != null)
            btSortLevelUp.onClick.AddListener(OnClickForSortLevelUp);
        if (btSortPrice != null)
            btSortPrice.onClick.AddListener(OnClickForSortPrice);
        if (btSortTime != null)
            btSortTime.onClick.AddListener(OnClickForSortTime);

        if (btSellAll != null)
            btSellAll.onClick.AddListener(OnClickForSellAll);
        if (btStopAll != null)
            btStopAll.onClick.AddListener(OnClickForStopAll);

        if (gridVertical)
            gridVertical.AddCellListener(OnCellForFoodItems);
        if (textSearchView)
            textSearchView.SetCallBack(this);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        List<MenuOwnBean> listMenu = uiGameManager.gameDataManager.gameData.listMenu;
        this.listMenu.Clear();
        this.listMenu.AddRange(listMenu);
        gridVertical.SetCellCount(listMenu.Count);
        gridVertical.RefreshAllCells();
    }

    private void Update()
    {
        SetIngredients();
    }

    /// <summary>
    /// 设置材料
    /// </summary>
    public void SetIngredients()
    {
        if (tvOilsalt != null)
        {
            tvOilsalt.text = IngredientsEnumTools.GetIngredientName(IngredientsEnum.Oilsalt) + " " + uiGameManager.gameDataManager.gameData.ingOilsalt;
        }
        if (tvMeat != null)
        {
            tvMeat.text = IngredientsEnumTools.GetIngredientName(IngredientsEnum.Meat) + " " + uiGameManager.gameDataManager.gameData.ingMeat;
        }
        if (tvRiverfresh != null)
        {
            tvRiverfresh.text = IngredientsEnumTools.GetIngredientName(IngredientsEnum.Riverfresh) + " " + uiGameManager.gameDataManager.gameData.ingRiverfresh;
        }
        if (tvSeafood != null)
        {
            tvSeafood.text = IngredientsEnumTools.GetIngredientName(IngredientsEnum.Seafood) + " " + uiGameManager.gameDataManager.gameData.ingSeafood;
        }
        if (tvVegetables != null)
        {
            tvVegetables.text = IngredientsEnumTools.GetIngredientName(IngredientsEnum.Vegetables) + " " + uiGameManager.gameDataManager.gameData.ingVegetables;
        }
        if (tvMelonfruit != null)
        {
            tvMelonfruit.text = IngredientsEnumTools.GetIngredientName(IngredientsEnum.Melonfruit) + " " + uiGameManager.gameDataManager.gameData.ingMelonfruit;
        }
        if (tvWaterwine != null)
        {
            tvWaterwine.text = IngredientsEnumTools.GetIngredientName(IngredientsEnum.Waterwine) + " " + uiGameManager.gameDataManager.gameData.ingWaterwine;
        }
        if (tvflour != null)
        {
            tvflour.text = IngredientsEnumTools.GetIngredientName(IngredientsEnum.Flour) + " " + uiGameManager.gameDataManager.gameData.ingFlour;
        }
    }

    public void OnCellForFoodItems(ScrollGridCell itemCell)
    {
        int index = itemCell.index;
        MenuOwnBean itemData = listMenu[index];
        MenuInfoBean menuInfo = InnFoodHandler.Instance.manager.GetFoodDataById(itemData.menuId);

        ItemGameMenuFoodCpt foodCpt = itemCell.GetComponent<ItemGameMenuFoodCpt>();
        foodCpt.SetData(itemData, menuInfo);
    }

    /// <summary>
    /// 默认排序点击
    /// </summary>
    public void OnClickForSortDef()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        List<MenuOwnBean> listMenu = uiGameManager.gameDataManager.gameData.listMenu;
        this.listMenu.Clear();
        this.listMenu.AddRange(listMenu);
        gridVertical.SetCellCount(listMenu.Count);
        gridVertical.RefreshAllCells();
    }

    /// <summary>
    /// 名字排序点击
    /// </summary>
    public void OnClickForSortName()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        //TODO 名字排序有问题
        this.listMenu =  this.listMenu.OrderByDescending(
            (data)=> 
            {
                MenuInfoBean menuInfo = InnFoodHandler.Instance.manager.GetFoodDataById(data.menuId);
                return menuInfo.name;
            }).ToList();
        gridVertical.SetCellCount(listMenu.Count);
        gridVertical.RefreshAllCells();
    }

    /// <summary>
    /// 稀有度排序点击
    /// </summary>
    public void OnClickForSortRarity()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        this.listMenu = this.listMenu.OrderByDescending(
            (data) =>
            {
                MenuInfoBean menuInfo = InnFoodHandler.Instance.manager.GetFoodDataById(data.menuId);
                return menuInfo.rarity;
             }).ToList();
        gridVertical.SetCellCount(listMenu.Count);
        gridVertical.RefreshAllCells();
    }

    /// <summary>
    /// 等级排序点击
    /// </summary>
    public void OnClickForSortLevel()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        this.listMenu = this.listMenu.OrderByDescending(
            (data) =>
            {
                return data.menuLevel;
            }).ToList();
        gridVertical.SetCellCount(listMenu.Count);
        gridVertical.RefreshAllCells();
    }

    /// <summary>
    /// 售卖排序点击
    /// </summary>
    public void OnClickForSortSell()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        this.listMenu = this.listMenu.OrderByDescending(
            (data) =>
            {
                return data.isSell;
            }).ToList();
        gridVertical.SetCellCount(listMenu.Count);
        gridVertical.RefreshAllCells();
    }

    /// <summary>
    /// 是否升级排序点击
    /// </summary>
    public void OnClickForSortLevelUp()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        this.listMenu = this.listMenu.OrderByDescending(
            (data) =>
            {
                return data.menuStatus;
            }).ToList();
        gridVertical.SetCellCount(listMenu.Count);
        gridVertical.RefreshAllCells();
    }
    /// <summary>
    /// 是否升级排序点击
    /// </summary>
    public void OnClickForSortPrice()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        this.listMenu = this.listMenu.OrderByDescending(
            (data) =>
            {
                MenuInfoBean menuInfo = InnFoodHandler.Instance.manager.GetFoodDataById(data.menuId);
                data.GetPrice(menuInfo,out long priceL, out long priceM, out long priceS);
                return priceS;
            }).ToList();
        gridVertical.SetCellCount(listMenu.Count);
        gridVertical.RefreshAllCells();
    }

    /// <summary>
    /// 制作时间排序点击
    /// </summary>
    public void OnClickForSortTime()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        this.listMenu = this.listMenu.OrderByDescending(
            (data) =>
            {
                MenuInfoBean menuInfo = InnFoodHandler.Instance.manager.GetFoodDataById(data.menuId);
                return menuInfo.cook_time;
            }).ToList();
        gridVertical.SetCellCount(listMenu.Count);
        gridVertical.RefreshAllCells();
    }

    public void OnClickForSellAll()
    {
        SelectAllChange(true);
    }

    public void OnClickForStopAll()
    {
        SelectAllChange(false);
    }

    /// <summary>
    /// 售卖状态设置
    /// </summary>
    /// <param name="isSell"></param>
    protected void SelectAllChange( bool isSell)
    {
        if (CheckUtil.ListIsNull(listMenu))
            return;
        for (int i=0;i< listMenu.Count;i++)
        {
            MenuOwnBean itemMenu =  listMenu[i];
            itemMenu.isSell= isSell;
        }
        gridVertical.SetCellCount(listMenu.Count);
        gridVertical.RefreshAllCells();
    }

    /// <summary>
    /// 返回游戏主UI
    /// </summary>
    public void OpenMainUI()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForBack);
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameMain));
    }


    #region  搜索文本回调
    public void SearchTextStart(string text)
    {
        this.listMenu = this.listMenu.OrderByDescending(data => {
            MenuInfoBean menuInfo = InnFoodHandler.Instance.manager.GetFoodDataById(data.menuId);
            if (menuInfo.name.Contains(text))
            {
                return true;
            }
            else
            {
                return false;
            }
        }).ToList();
        gridVertical.RefreshAllCells();
    }
    #endregion
}