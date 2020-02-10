using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class InfoFoodPopupShow : PopupShowView
{
    public GameObject objItemBaseContainer;
    public GameObject objItemStatisticsContainer;
    public GameObject objItemTextModel;

    protected GameDataManager gameDataManager;
    protected IconDataManager iconDataManager;

    public MenuOwnBean ownData;
    public MenuInfoBean foodData;

    private void Awake()
    {
        gameDataManager = Find<GameDataManager>(ImportantTypeEnum.GameDataManager);
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