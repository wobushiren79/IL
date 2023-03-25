using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using DG.Tweening;
public class MiniGameCookingStoveCpt : BaseMonoBehaviour
{
    public GameObject objCookingPrePosition;
    public GameObject objCookingMakingPosition;
    public GameObject objCookingEndPosition;
    public GameObject objFoodPosition;
    //做好的食物模型
    public GameObject objFoodModel;

    public SpriteRenderer srIngredientPre;
    public GameObject objEffects;

    public MenuInfoBean menuInfo;


    /// <summary>
    /// 设置菜品信息
    /// </summary>
    /// <param name="menuInfo"></param>
    public void SetMenuInfo(MenuInfoBean menuInfo)
    {
        this.menuInfo = menuInfo;
    }

    /// <summary>
    /// 开启灶台
    /// </summary>
    public void OpenStove()
    {
        objEffects.SetActive(true);
    }

    /// <summary>
    /// 关闭灶台
    /// </summary>
    public void CloseStove()
    {
        objEffects.SetActive(false);
    }

    /// <summary>
    /// 获取料理准备阶段位置
    /// </summary>
    /// <returns></returns>
    public Vector3 GetCookingPrePosition()
    {
        return objCookingPrePosition.transform.position;
    }

    /// <summary>
    /// 获取料理制作阶段位置
    /// </summary>
    /// <returns></returns>
    public Vector3 GetCookingMakingPosition()
    {
        return objCookingMakingPosition.transform.position;
    }

    /// <summary>
    /// 获取料理结尾阶段位置
    /// </summary>
    /// <returns></returns>
    public Vector3 GetCookingEndPosition()
    {
        return objCookingEndPosition.transform.position;
    }

    /// <summary>
    /// 修改准备的食材
    /// </summary>
    public void ChangeIngredientPre()
    {
        if (menuInfo == null)
            return;
        List<IngredientsEnum> listIng = new List<IngredientsEnum>();
        if (menuInfo.ing_oilsalt != 0)
            listIng.Add(IngredientsEnum.Oilsalt);
        if (menuInfo.ing_meat != 0)
            listIng.Add(IngredientsEnum.Meat);
        if (menuInfo.ing_riverfresh != 0)
            listIng.Add(IngredientsEnum.Riverfresh);
        if (menuInfo.ing_seafood != 0)
            listIng.Add(IngredientsEnum.Seafood);
        if (menuInfo.ing_vegetables != 0)
            listIng.Add(IngredientsEnum.Vegetables);
        if (menuInfo.ing_melonfruit != 0)
            listIng.Add(IngredientsEnum.Melonfruit);
        if (menuInfo.ing_waterwine != 0)
            listIng.Add(IngredientsEnum.Waterwine);
        if (menuInfo.ing_flour != 0)
            listIng.Add(IngredientsEnum.Flour);
        IngredientsEnum ingredientType = RandomUtil.GetRandomDataByList(listIng);
        string iconKey = "ingredient_";
        switch (ingredientType)
        {
            case IngredientsEnum.Oilsalt:
                iconKey += "oilsalt_1";
                break;
            case IngredientsEnum.Meat:
                iconKey += "meat_1";
                break;
            case IngredientsEnum.Riverfresh:
                iconKey += "riverfresh_1";
                break;
            case IngredientsEnum.Seafood:
                iconKey += "seafood_1";
                break;
            case IngredientsEnum.Vegetables:
                iconKey += "vegetables_1";
                break;
            case IngredientsEnum.Melonfruit:
                iconKey += "melonfruit_1";
                break;
            case IngredientsEnum.Waterwine:
                iconKey += "waterwine_1";
                break;
            case IngredientsEnum.Flour:
                iconKey += "flour_1";
                break;
        }
        Sprite spIcon = GameItemsHandler.Instance.manager.GetItemsSpriteByName(iconKey);
        if (srIngredientPre != null)
        {
            srIngredientPre.sprite = spIcon;
            srIngredientPre.transform.localScale = new Vector3(1,1,1);
            srIngredientPre.transform.DOScale(Vector3.zero, 0.5f).From().SetEase(Ease.OutBack);
        }   
    }

    /// <summary>
    /// 创建食物
    /// </summary>
    public FoodForCoverCpt CreateFood()
    {
        GameObject objFood = Instantiate(objFoodPosition, objFoodModel, objFoodPosition.transform.position);
        FoodForCoverCpt foodForCover = objFood.GetComponent<FoodForCoverCpt>();
        foodForCover.SetData(menuInfo);
        return foodForCover;
    }

    /// <summary>
    /// 清理灶台食材
    /// </summary>
    public void ClearStoveIngredient()
    {
        if (srIngredientPre!=null)
        {
            srIngredientPre.sprite = null;
        }
    }
}