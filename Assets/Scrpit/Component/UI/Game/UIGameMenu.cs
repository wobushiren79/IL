using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class UIGameMenu : UIGameComponent
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

    [Header("模型")]
    public GameObject objFoodListContent;
    public GameObject objFoodItemModelFor0;
    public GameObject objFoodItemModelFor1;
    public GameObject objFoodItemModelFor2;
    public GameObject objFoodItemModelFor3;

    [Header("排序")]
    public Button btSortDef;
    public Button btSortName;
    public Button btSortRarity;
    public Button btSortSell;
    public Button btSortLevel;
    public Button btSortLevelUp;
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
    }

    public override void OpenUI()
    {
        base.OpenUI();
        List<MenuOwnBean> listMenu = uiGameManager.gameDataManager.gameData.listMenu;
        this.listMenu.Clear();
        this.listMenu.AddRange(listMenu);
        CreateFoodList();
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

    public void CreateFoodList()
    {
        StopAllCoroutines();
        CptUtil.RemoveChildsByActive(objFoodListContent.transform);
        StartCoroutine(CoroutineForCreateFoodList());
    }

    public IEnumerator CoroutineForCreateFoodList()
    {
        for (int i = 0; i < listMenu.Count; i++)
        {
            MenuOwnBean itemData = listMenu[i];
            MenuInfoBean menuInfo = uiGameManager.innFoodManager.GetFoodDataById(itemData.menuId);
            if (menuInfo == null)
                continue;
            GameObject objModel = null;
            LevelTypeEnum level = itemData.GetMenuLevel(uiGameManager.innFoodManager, out string levelStr, out int nextLevelExp);
            if (level == LevelTypeEnum.Star)
            {
                objModel = objFoodItemModelFor1;
            }
            else if (level == LevelTypeEnum.Moon)
            {
                objModel = objFoodItemModelFor2;
            }
            else if (level == LevelTypeEnum.Sun)
            {
                objModel = objFoodItemModelFor3;
            }
            else
            {
                objModel = objFoodItemModelFor0;
            }

            GameObject foodObj = Instantiate(objFoodListContent, objModel);
            ItemGameMenuFoodCpt foodCpt = foodObj.GetComponent<ItemGameMenuFoodCpt>();
            foodCpt.SetData(itemData, menuInfo);
            yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    /// 默认排序点击
    /// </summary>
    public void OnClickForSortDef()
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        List<MenuOwnBean> listMenu = uiGameManager.gameDataManager.gameData.listMenu;
        this.listMenu.Clear();
        this.listMenu.AddRange(listMenu);
        CreateFoodList();
    }

    /// <summary>
    /// 名字排序点击
    /// </summary>
    public void OnClickForSortName()
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        //TODO 名字排序有问题
        this.listMenu =  this.listMenu.OrderByDescending(
            (data)=> 
            {
                MenuInfoBean menuInfo = uiGameManager.innFoodManager.GetFoodDataById(data.menuId);
                return menuInfo.name;
            }).ToList();
        CreateFoodList();
    }

    /// <summary>
    /// 稀有度排序点击
    /// </summary>
    public void OnClickForSortRarity()
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        this.listMenu = this.listMenu.OrderByDescending(
            (data) =>
            {
                MenuInfoBean menuInfo = uiGameManager.innFoodManager.GetFoodDataById(data.menuId);
                return menuInfo.rarity;
             }).ToList();
        CreateFoodList();
    }

    /// <summary>
    /// 等级排序点击
    /// </summary>
    public void OnClickForSortLevel()
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        this.listMenu = this.listMenu.OrderByDescending(
            (data) =>
            {
                return data.menuLevel;
            }).ToList();
        CreateFoodList();
    }

    /// <summary>
    /// 售卖排序点击
    /// </summary>
    public void OnClickForSortSell()
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        this.listMenu = this.listMenu.OrderByDescending(
            (data) =>
            {
                return data.isSell;
            }).ToList();
        CreateFoodList();
    }

    /// <summary>
    /// 是否升级排序点击
    /// </summary>
    public void OnClickForSortLevelUp()
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        this.listMenu = this.listMenu.OrderByDescending(
            (data) =>
            {
                return data.menuStatus;
            }).ToList();
        CreateFoodList();
    }

    /// <summary>
    /// 返回游戏主UI
    /// </summary>
    public void OpenMainUI()
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForBack);
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameMain));
    }

}