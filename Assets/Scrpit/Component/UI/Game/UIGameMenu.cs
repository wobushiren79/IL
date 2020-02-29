using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections.Generic;

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
    public GameObject objFoodItemModel;

    private void Start()
    {
        if (btBack != null)
            btBack.onClick.AddListener(OpenMainUI);

    }

    public override void OpenUI()
    {
        base.OpenUI();
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
        CptUtil.RemoveChildsByActive(objFoodListContent.transform);
        List<MenuOwnBean> listMenu = uiGameManager.gameDataManager.gameData.listMenu;

        for (int i = 0; i < listMenu.Count; i++)
        {
            MenuOwnBean itemData = listMenu[i];
            MenuInfoBean menuInfo = uiGameManager.innFoodManager.GetFoodDataById(itemData.menuId);
            if (menuInfo == null)
                continue;
            GameObject foodObj = Instantiate(objFoodItemModel, objFoodListContent.transform);
            foodObj.SetActive(true);
            ItemGameMenuFoodCpt foodCpt = foodObj.GetComponent<ItemGameMenuFoodCpt>();
            foodCpt.SetData(itemData, menuInfo);
        }
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