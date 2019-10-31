using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class InfoFoodPopupShow : PopupShowView
{
    public GameObject objTime;
    public Text tvTime;

    public GameObject objOilsalt;
    public GameObject objMeat;
    public GameObject objRiverfresh;
    public GameObject objSeafood;
    public GameObject objVegetables;
    public GameObject objMelonfruit;
    public GameObject objWaterwine;
    public GameObject objFlour;

    public GameDataManager gameDataManager;

    public MenuOwnBean ownData;
    public MenuInfoBean foodData;

    //public override void Update()
    //{
    //    base.Update();
    //    if (gameDataManager == null)
    //        return;

    //    if (foodData.ing_oilsalt <= gameDataManager.gameData.ingOilsalt)
    //        tvIngOilsalt.color = Color.black;
    //    else
    //        tvIngOilsalt.color = Color.red;

    //    if (foodData.ing_meat <= gameDataManager.gameData.ingMeat)
    //        tvIngMeat.color = Color.black;
    //    else
    //        tvIngMeat.color = Color.red;

    //    if (foodData.ing_riverfresh <= gameDataManager.gameData.ingRiverfresh)
    //        tvIngRiverfresh.color = Color.black;
    //    else
    //        tvIngRiverfresh.color = Color.red;

    //    if (foodData.ing_seafood <= gameDataManager.gameData.ingSeafood)
    //        tvIngSeafood.color = Color.black;
    //    else
    //        tvIngSeafood.color = Color.red;

    //    if (foodData.ing_vegetables <= gameDataManager.gameData.ingVegetables)
    //        tvIngVegetables.color = Color.black;
    //    else
    //        tvIngVegetables.color = Color.red;

    //    if (foodData.ing_melonfruit <= gameDataManager.gameData.ingMelonfruit)
    //        tvIngMelonfruit.color = Color.black;
    //    else
    //        tvIngMelonfruit.color = Color.red;

    //    if (foodData.ing_waterwine <= gameDataManager.gameData.ingWaterwine)
    //        tvIngWaterwine.color = Color.black;
    //    else
    //        tvIngWaterwine.color = Color.red;

    //    if (foodData.ing_flour <= gameDataManager.gameData.ingFlour)
    //        tvIngFlour.color = Color.black;
    //    else
    //        tvIngFlour.color = Color.red;
    //}


    public void SetData(MenuOwnBean ownData, MenuInfoBean foodData)
    {
        SetData(ownData, foodData, true);
    }
    public void SetData(MenuOwnBean ownData, MenuInfoBean foodData,bool isShowTime)
    {
        this.ownData = ownData;
        this.foodData = foodData;

        if (ownData != null && foodData != null)
        {
            //制作时间
            if (tvTime != null)
            {
                if (isShowTime)
                    objTime.SetActive(true);
                else
                    objTime.SetActive(false);
                tvTime.text = foodData.cook_time + GameCommonInfo.GetUITextById(38);
            }
             
            //油烟类
            SetItemForIng(IngredientsEnum.Oilsalt, foodData.ing_oilsalt);
            //肉类
            SetItemForIng(IngredientsEnum.Meat, foodData.ing_meat);
            //河鲜
            SetItemForIng(IngredientsEnum.Riverfresh, foodData.ing_riverfresh);
            //海鲜
            SetItemForIng(IngredientsEnum.Seafood, foodData.ing_seafood);
            //蔬菜
            SetItemForIng(IngredientsEnum.Vegetablest, foodData.ing_vegetables);
            //瓜果
            SetItemForIng(IngredientsEnum.Melonfruit, foodData.ing_melonfruit);
            //酒水
            SetItemForIng(IngredientsEnum.Waterwine, foodData.ing_waterwine);
            //面粉
            SetItemForIng(IngredientsEnum.Flour, foodData.ing_flour);
        }
    }

    /// <summary>
    /// 创建食材Item
    /// </summary>
    /// <param name="ingredient"></param>
    /// <param name="number"></param>
    public void SetItemForIng(IngredientsEnum ingredient, int number)
    {
        string ingNameStr = "???";
        GameObject objItem = null;
        switch (ingredient)
        {
            case IngredientsEnum.Oilsalt://油盐
                objItem = objOilsalt;
                ingNameStr = GameCommonInfo.GetUITextById(21);
                break;
            case IngredientsEnum.Meat://鲜肉
                objItem = objMeat;
                ingNameStr = GameCommonInfo.GetUITextById(22);
                break;
            case IngredientsEnum.Riverfresh://河鲜
                objItem = objRiverfresh;
                ingNameStr = GameCommonInfo.GetUITextById(23);
                break;
            case IngredientsEnum.Seafood://海鲜
                objItem = objSeafood;
                ingNameStr = GameCommonInfo.GetUITextById(24);
                break;
            case IngredientsEnum.Vegetablest://蔬菜
                objItem = objVegetables;
                ingNameStr = GameCommonInfo.GetUITextById(25);
                break;
            case IngredientsEnum.Melonfruit://瓜果
                objItem = objMelonfruit;
                ingNameStr = GameCommonInfo.GetUITextById(26);
                break;
            case IngredientsEnum.Waterwine://酒水
                objItem = objWaterwine;
                ingNameStr = GameCommonInfo.GetUITextById(27);
                break;
            case IngredientsEnum.Flour://面粉
                objItem = objFlour;
                ingNameStr = GameCommonInfo.GetUITextById(28);
                break;
        }

        if (number == 0)
            objItem.SetActive(false);
        else
            objItem.SetActive(true);

        Text tvName = CptUtil.GetCptInChildrenByName<Text>(objItem, "Name");
        tvName.text = ingNameStr;
        Text tvNumber = CptUtil.GetCptInChildrenByName<Text>(objItem, "Number");
        tvNumber.text = "" + number;
    }
}