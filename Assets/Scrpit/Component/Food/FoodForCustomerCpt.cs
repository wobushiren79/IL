using UnityEngine;
using UnityEditor;

public class FoodForCustomerCpt : BaseMonoBehaviour
{
    //食物数据
    public MenuForCustomer foodData;
    //食物样式
    public SpriteRenderer srFood;
    //食物数据管理
    public InnFoodManager innFoodManager;

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="foodData"></param>
    public void SetData(MenuForCustomer foodData)
    {
        this.foodData = foodData;
        if (foodData == null)
            return;
        if (foodData.food != null)
        {
            srFood.sprite = innFoodManager.GetFoodSpriteByName(foodData.food.icon_key);
        }
    }

    /// <summary>
    /// 吃完食物
    /// </summary>
    public void FinishFood()
    {
        srFood.sprite = innFoodManager.GetFoodLastSpriteByName(foodData.food.icon_key);
    }
}