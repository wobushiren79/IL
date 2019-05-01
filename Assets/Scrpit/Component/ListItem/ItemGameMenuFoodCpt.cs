using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class ItemGameMenuFoodCpt : BaseMonoBehaviour
{
    public Image ivFood;
    public InnFoodManager innFoodManager;
    public MenuInfoBean foodData;

    public void SetData(MenuInfoBean data)
    {
        foodData = data;
        Sprite spFood = innFoodManager.GetFoodSpriteByName(foodData.icon_key);
        if (ivFood != null)
        {
            ivFood.sprite = spFood;
        }
    }
}