using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class UIPopupFoodShow : PopupShowView
{
    public GameObject objLevel;
    public GameObject objLevelProgress;
    public Image ivLevel;
    public Text tvLevelName;
    public Text tvDetails;
    public ProgressView proLevel;


    public GameObject objResearch;
    public Text tvResearcherName;
    public Text tvResearcherAbility;
    public ProgressView proResearch;

    public GameObject objItemBaseContainer;
    public GameObject objItemStatisticsContainer;
    public GameObject objItemTextModel;

    public MenuOwnBean ownData;
    public MenuInfoBean foodData;
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
        //设置详情
        SetDetails(foodData.content);

        GameUtil.RefreshRectTransform((RectTransform)tvDetails.transform.parent.transform);
    }

    public void SetDetails(string details)
    {
        if (tvDetails != null)
            tvDetails.text = details;
    }

    /// <summary>
    /// 设置等级
    /// </summary>
    public void SetLevel(MenuOwnBean ownData)
    {
        LevelTypeEnum level = ownData.GetMenuLevel(out string levelStr, out int nextLevelExp);
        if ((int)level >= 3)
        {
            objLevelProgress.SetActive(false);
        }
        else
        {
            objLevelProgress.SetActive(true);
        }
        Sprite spIcon = ownData.GetMenuLevelIcon();
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
        proLevel.SetData(nextLevelExp, ownData.menuExp);
        if (ownData.GetMenuStatus() == ResearchStatusEnum.WaitForResearch)
        {
            proLevel.SetContent(TextHandler.Instance.manager.GetTextById(287));
        }
    }

    /// <summary>
    /// 设置研究数据
    /// </summary>
    /// <param name="ownData"></param>
    public void SetResearch(MenuOwnBean ownData)
    {
        if (ownData.GetMenuStatus() == ResearchStatusEnum.Normal)
        {
            objResearch.SetActive(false);
            return;
        }
        objResearch.SetActive(true);
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        CharacterBean researcher = ownData.GetResearchCharacter(gameData);
        float progress = ownData.GetResearchProgress(out long completeExp, out long researchExp);
        if (researcher != null)
        {
            tvResearcherName.text = researcher.baseInfo.name;
            long addExp = researcher.CalculationMenuResearchAddExp();
            tvResearcherAbility.text = addExp + "/s";
        }
        else
        {
            tvResearcherName.text = "无";
            tvResearcherAbility.text = "0/s";
        }
        //设置进度
        proResearch.SetData(completeExp, researchExp);
    }

    /// <summary>
    /// 创建制作事件Item
    /// </summary>
    /// <param name="makeTime"></param>
    public void AddItemForMakeTime(float makeTime)
    {
        Sprite spIcon = IconHandler.Instance.GetIconSpriteByName("hourglass_1");
        CreateItem(objItemBaseContainer, spIcon, TextHandler.Instance.manager.GetTextById(40), makeTime + TextHandler.Instance.manager.GetTextById(38));
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
                ingNameStr = TextHandler.Instance.manager.GetTextById(21);
                iconKey = "ui_ing_oilsalt";
                break;
            case IngredientsEnum.Meat://鲜肉
                ingNameStr = TextHandler.Instance.manager.GetTextById(22);
                iconKey = "ui_ing_meat";
                break;
            case IngredientsEnum.Riverfresh://河鲜
                ingNameStr = TextHandler.Instance.manager.GetTextById(23);
                iconKey = "ui_ing_riverfresh";
                break;
            case IngredientsEnum.Seafood://海鲜
                ingNameStr = TextHandler.Instance.manager.GetTextById(24);
                iconKey = "ui_ing_seafood";
                break;
            case IngredientsEnum.Vegetables://蔬菜
                ingNameStr = TextHandler.Instance.manager.GetTextById(25);
                iconKey = "ui_ing_vegetables";
                break;
            case IngredientsEnum.Melonfruit://瓜果
                ingNameStr = TextHandler.Instance.manager.GetTextById(26);
                iconKey = "ui_ing_melonfruit";
                break;
            case IngredientsEnum.Waterwine://酒水
                ingNameStr = TextHandler.Instance.manager.GetTextById(27);
                iconKey = "ui_ing_waterwine";
                break;
            case IngredientsEnum.Flour://面粉
                ingNameStr = TextHandler.Instance.manager.GetTextById(28);
                iconKey = "ui_ing_flour";
                break;
        }
        Sprite spIcon = IconHandler.Instance.GetIconSpriteByName(iconKey);
        CreateItem(objItemBaseContainer, spIcon, ingNameStr, number + "");
    }

    /// <summary>
    /// 增加销售数量
    /// </summary>
    /// <param name="number"></param>
    public void AddItemForSellNumber(long number)
    {
        Sprite spIcon = IconHandler.Instance.GetIconSpriteByName("ui_features_menu");
        CreateItem(objItemStatisticsContainer, spIcon, Color.red, TextHandler.Instance.manager.GetTextById(332), number + "");
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
                contentStr = TextHandler.Instance.manager.GetTextById(333);
                break;
            case MoneyEnum.M:
                iconKey = "money_2";
                contentStr = TextHandler.Instance.manager.GetTextById(334);
                break;
            case MoneyEnum.S:
                iconKey = "money_1";
                contentStr = TextHandler.Instance.manager.GetTextById(335);
                break;
        }
        Sprite spIcon = IconHandler.Instance.GetIconSpriteByName(iconKey);
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