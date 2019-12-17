using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections.Generic;

public class UIGameMenu : BaseUIComponent
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
        GameDataManager gameDataManager = GetUIMananger<UIGameManager>().gameDataManager;
        if (tvOilsalt != null)
        {
            tvOilsalt.text =GameCommonInfo.GetUITextById(21) + gameDataManager.gameData.ingOilsalt;
        }
        if (tvMeat != null)
        {
            tvMeat.text = GameCommonInfo.GetUITextById(22) + gameDataManager.gameData.ingMeat;
        }
        if (tvRiverfresh != null)
        {
            tvRiverfresh.text = GameCommonInfo.GetUITextById(23) + gameDataManager.gameData.ingRiverfresh;
        }
        if (tvSeafood != null)
        {
            tvSeafood.text = GameCommonInfo.GetUITextById(24) + gameDataManager.gameData.ingSeafood;
        }
        if (tvVegetables != null)
        {
            tvVegetables.text = GameCommonInfo.GetUITextById(25) + gameDataManager.gameData.ingVegetables;
        }
        if (tvMelonfruit != null)
        {
            tvMelonfruit.text = GameCommonInfo.GetUITextById(26) + gameDataManager.gameData.ingMelonfruit;
        }
        if (tvWaterwine != null)
        {
            tvWaterwine.text = GameCommonInfo.GetUITextById(27) + gameDataManager.gameData.ingWaterwine;
        }
        if (tvflour != null)
        {
            tvflour.text = GameCommonInfo.GetUITextById(28) + gameDataManager.gameData.ingFlour;
        }
    }

    public void CreateFoodList()
    {
        GameDataManager gameDataManager = GetUIMananger<UIGameManager>().gameDataManager;
        InnFoodManager innFoodManager = GetUIMananger<UIGameManager>().innFoodManager;
        if (gameDataManager == null || innFoodManager == null)
            return;
        CptUtil.RemoveChildsByActive(objFoodListContent.transform);
        List<MenuOwnBean> listMenu = gameDataManager.gameData.listMenu;

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
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameMain));
    }

}