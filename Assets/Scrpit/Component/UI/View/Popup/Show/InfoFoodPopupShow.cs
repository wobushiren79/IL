using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class InfoFoodPopupShow : PopupShowView
{
    public GameObject objLevel;
    public GameObject objLevelProgress;
    public Image ivLevel;
    public Text tvLevelName;
    public Text tvLevelProgress;
    public Slider progressLevel;

    public GameObject objResearch;
    public Text tvResearcherName;
    public Text tvResearcherAbility;
    public Text tvResearchProgress;
    public Slider progressResearch;

    public GameObject objItemBaseContainer;
    public GameObject objItemStatisticsContainer;
    public GameObject objItemTextModel;

    protected GameDataManager gameDataManager;
    protected IconDataManager iconDataManager;
    protected GameItemsManager gameItemsManager;

    public MenuOwnBean ownData;
    public MenuInfoBean foodData;

    public override void Awake()
    {
        base.Awake();
        gameDataManager = Find<GameDataManager>(ImportantTypeEnum.GameDataManager);
        gameItemsManager = Find<GameItemsManager>(ImportantTypeEnum.GameItemsManager);
        iconDataManager = Find<IconDataManager>(ImportantTypeEnum.UIManager);
    }

    public void SetData(MenuOwnBean ownData, MenuInfoBean foodData)
    {
        SetData(ownData, foodData, true);
    }

    public void SetData(MenuOwnBean ownData, MenuInfoBean foodData, bool isShowTime)
    {
        this.ownData = ownData;
        this.foodData = foodData;
        CptUtil.RemoveChildsByActive(objItemBaseContainer);
        CptUtil.RemoveChildsByActive(objItemStatisticsContainer);
        if (ownData != null && foodData != null)
        {
            //制作时间
            if (isShowTime)
                AddItemForMakeTime(foodData.cook_time);
            //油烟类
            AddItemForIng(IngredientsEnum.Oilsalt, foodData.ing_oilsalt);
            //肉类
            AddItemForIng(IngredientsEnum.Meat, foodData.ing_meat);
            //河鲜
            AddItemForIng(IngredientsEnum.Riverfresh, foodData.ing_riverfresh);
            //海鲜
            AddItemForIng(IngredientsEnum.Seafood, foodData.ing_seafood);
            //蔬菜
            AddItemForIng(IngredientsEnum.Vegetables, foodData.ing_vegetables);
            //瓜果
            AddItemForIng(IngredientsEnum.Melonfruit, foodData.ing_melonfruit);
            //酒水
            AddItemForIng(IngredientsEnum.Waterwine, foodData.ing_waterwine);
            //面粉
            AddItemForIng(IngredientsEnum.Flour, foodData.ing_flour);

            AddItemForSellNumber(ownData.sellNumber);
            AddItemForSellMoney(MoneyEnum.L, ownData.sellMoneyL);
            AddItemForSellMoney(MoneyEnum.M, ownData.sellMoneyM);
            AddItemForSellMoney(MoneyEnum.S, ownData.sellMoneyS);
        }
        //设置等级相关
        SetLevel(ownData);
        //设置研究相关
        SetResearch(ownData);
    }

    /// <summary>
    /// 设置等级
    /// </summary>
    public void SetLevel(MenuOwnBean ownData)
    {
        int level = ownData.GetMenuLevel(out string levelStr, out int nextLevelExp);
        Sprite spIcon = ownData.GetMenuLevelIcon(iconDataManager);
        if (spIcon == null)
        {
            ivLevel.gameObject.SetActive(false);
        }
        else
        {
            ivLevel.gameObject.SetActive(true);
        }
        ivLevel.sprite = spIcon;
        tvLevelName.text = levelStr;
        progressLevel.value = ownData.menuExp / (float)nextLevelExp;
        tvLevelProgress.text = ownData.menuExp + "/" + nextLevelExp;
        if (ownData.GetMenuStatus() == MenuStatusEnum.WaitForResearch)
        {
            tvLevelProgress.text += GameCommonInfo.GetUITextById(287);
        }
        if (level >= 3)
        {
            objLevelProgress.SetActive(false);
        }
        else
        {
            objLevelProgress.SetActive(true);
        }
    }

    /// <summary>
    /// 设置研究数据
    /// </summary>
    /// <param name="ownData"></param>
    public void SetResearch(MenuOwnBean ownData)
    {
        if (ownData.GetMenuStatus() == MenuStatusEnum.Normal)
        {
            objResearch.SetActive(false);
            return;
        }
        objResearch.SetActive(true);
        CharacterBean researcher = ownData.GetResearchCharacter(gameDataManager.gameData);
        float progress = ownData.GetResearchProgress(out long completeExp);
        if (researcher != null)
        {
            tvResearcherName.text = researcher.baseInfo.name;
            long addExp = researcher.CalculationMenuResearchAddExp(gameItemsManager);
            tvResearcherAbility.text = addExp + "/s";
        }
        else
        {
            tvResearcherName.text = "无";
            tvResearcherAbility.text = "0/s";
        }
        //设置进度
        tvResearchProgress.text = ownData.researchExp + "/" + completeExp;
        progressResearch.value = progress;
    }

    /// <summary>
    /// 创建制作事件Item
    /// </summary>
    /// <param name="makeTime"></param>
    public void AddItemForMakeTime(int makeTime)
    {
        Sprite spIcon = iconDataManager.GetIconSpriteByName("hourglass_1");
        CreateItem(objItemBaseContainer, spIcon, GameCommonInfo.GetUITextById(40), makeTime + GameCommonInfo.GetUITextById(38));
    }

    /// <summary>
    /// 创建食材Item
    /// </summary>
    /// <param name="ingredient"></param>
    /// <param name="number"></param>
    public void AddItemForIng(IngredientsEnum ingredient, int number)
    {
        if (number == 0)
            return;
        string ingNameStr = "???";
        string iconKey = "";
        switch (ingredient)
        {
            case IngredientsEnum.Oilsalt://油盐
                ingNameStr = GameCommonInfo.GetUITextById(21);
                iconKey = "ui_ing_oilsalt";
                break;
            case IngredientsEnum.Meat://鲜肉
                ingNameStr = GameCommonInfo.GetUITextById(22);
                iconKey = "ui_ing_meat";
                break;
            case IngredientsEnum.Riverfresh://河鲜
                ingNameStr = GameCommonInfo.GetUITextById(23);
                iconKey = "ui_ing_riverfresh";
                break;
            case IngredientsEnum.Seafood://海鲜
                ingNameStr = GameCommonInfo.GetUITextById(24);
                iconKey = "ui_ing_seafood";
                break;
            case IngredientsEnum.Vegetables://蔬菜
                ingNameStr = GameCommonInfo.GetUITextById(25);
                iconKey = "ui_ing_vegetables";
                break;
            case IngredientsEnum.Melonfruit://瓜果
                ingNameStr = GameCommonInfo.GetUITextById(26);
                iconKey = "ui_ing_melonfruit";
                break;
            case IngredientsEnum.Waterwine://酒水
                ingNameStr = GameCommonInfo.GetUITextById(27);
                iconKey = "ui_ing_waterwine";
                break;
            case IngredientsEnum.Flour://面粉
                ingNameStr = GameCommonInfo.GetUITextById(28);
                iconKey = "ui_ing_flour";
                break;
        }
        Sprite spIcon = iconDataManager.GetIconSpriteByName(iconKey);
        CreateItem(objItemBaseContainer, spIcon, ingNameStr, number + "");
    }

    /// <summary>
    /// 增加销售数量
    /// </summary>
    /// <param name="number"></param>
    public void AddItemForSellNumber(long number)
    {
        Sprite spIcon = iconDataManager.GetIconSpriteByName("ui_features_menu");
        CreateItem(objItemStatisticsContainer, spIcon, Color.red, GameCommonInfo.GetUITextById(332), number + "");
    }

    /// <summary>
    /// 增加销售金额
    /// </summary>
    /// <param name="moneyType"></param>
    /// <param name="money"></param>
    public void AddItemForSellMoney(MoneyEnum moneyType, long money)
    {
        if (money == 0)
            return;
        string iconKey = "";
        string contentStr = "";
        switch (moneyType)
        {
            case MoneyEnum.L:
                iconKey = "money_3";
                contentStr = GameCommonInfo.GetUITextById(333);
                break;
            case MoneyEnum.M:
                iconKey = "money_2";
                contentStr = GameCommonInfo.GetUITextById(334);
                break;
            case MoneyEnum.S:
                iconKey = "money_1";
                contentStr = GameCommonInfo.GetUITextById(335);
                break;
        }
        Sprite spIcon = iconDataManager.GetIconSpriteByName(iconKey);
        CreateItem(objItemStatisticsContainer, spIcon, contentStr, money + "");
    }

    /// <summary>
    /// 创建Item
    /// </summary>
    /// <param name="spIcon"></param>
    /// <param name="name"></param>
    /// <param name="content"></param>
    protected void CreateItem(GameObject objContainer, Sprite spIcon, Color colorIcon, string name, string content)
    {
        GameObject objItem = Instantiate(objContainer, objItemTextModel);
        ItemPopupFoodTextCpt itemCpt = objItem.GetComponent<ItemPopupFoodTextCpt>();
        itemCpt.SetData(spIcon, colorIcon, name, content);
    }

    protected void CreateItem(GameObject objContainer, Sprite spIcon, string name, string content)
    {
        CreateItem(objContainer, spIcon, Color.white, name, content);
    }
}