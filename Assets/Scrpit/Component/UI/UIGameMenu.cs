using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections.Generic;

public class UIGameMenu : BaseUIComponent
{
    //返回按钮
    public Button btBack;
    public Text tvOilsalt;
    public Text tvMeat;
    public Text tvRiverfresh;
    public Text tvSeafood;
    public Text tvVegetables;
    public Text tvMelonfruit;
    public Text tvWaterwine;
    public Text tvflour;

    public GameDataManager gameDataManager;
    public InnFoodManager innFoodManager;

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
        if (tvOilsalt != null)
        {
            tvOilsalt.text = "油盐 " + gameDataManager.gameData.ingOilsalt;
        }
        if (tvMeat != null)
        {
            tvMeat.text = "鲜肉 " + gameDataManager.gameData.ingMeat;
        }
        if (tvRiverfresh != null)
        {
            tvRiverfresh.text = "河鲜 " + gameDataManager.gameData.ingRiverfresh;
        }
        if (tvSeafood != null)
        {
            tvSeafood.text = "海鲜 " + gameDataManager.gameData.ingSeafood;
        }
        if (tvVegetables != null)
        {
            tvVegetables.text = "蔬菜 " + gameDataManager.gameData.ingVegetables;
        }
        if (tvMelonfruit != null)
        {
            tvMelonfruit.text = "瓜果 " + gameDataManager.gameData.ingMelonfruit;
        }
        if (tvWaterwine != null)
        {
            tvWaterwine.text = "酒水 " + gameDataManager.gameData.ingWaterwine;
        }
        if (tvflour != null)
        {
            tvflour.text = "面粉 " + gameDataManager.gameData.ingFlour;
        }
    }

    public void CreateFoodList()
    {
        if (gameDataManager == null || innFoodManager == null)
            return;
        CptUtil.RemoveChildsByActive(objFoodListContent.transform);
        List<MenuOwnBean> listMenu = gameDataManager.gameData.menuList;

        for (int i = 0; i < listMenu.Count; i++)
        {
            MenuOwnBean itemData = listMenu[i];
            MenuInfoBean menuInfo = innFoodManager.GetFoodDataById(itemData.menuId);
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
        uiManager.OpenUIAndCloseOtherByName("Main");
    }

}