using UnityEngine;
using UnityEditor;
using DG.Tweening;

public class FoodForCustomerCpt : BaseMonoBehaviour
{
    //食物样式
    public SpriteRenderer srFood;
    //食物数据管理
    public InnFoodManager innFoodManager;

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="foodData"></param>
    public void SetData(MenuInfoBean foodData)
    {
        if (foodData != null && innFoodManager != null)
            srFood.sprite = innFoodManager.GetFoodSpriteByName(foodData.icon_key);
    }

    /// <summary>
    /// 吃完食物
    /// </summary>
    public void FinishFood(MenuInfoBean foodData)
    {
        if (foodData != null && innFoodManager != null)
            srFood.sprite = innFoodManager.GetFoodLastSpriteByName(foodData.icon_key);
    }

    /// <summary>
    /// 食材创建动画
    /// </summary>
    public void CreateAnim()
    {
        transform.DOKill();
        transform.localScale = new Vector3(1, 1, 1);
        transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0.5f).From().SetEase(Ease.OutBack);
    }
}