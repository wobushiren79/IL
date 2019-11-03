using UnityEngine;
using UnityEditor;

public class FoodForCoverCpt : BaseMonoBehaviour
{
    public FoodForCustomerCpt foodCustomerCpt;
    public InnFoodManager innFoodManager;

    private void Awake()
    {
        innFoodManager = FindObjectOfType<InnFoodManager>();
    }

    public void SetData(MenuInfoBean menuInfo)
    {
        foodCustomerCpt.SetData(innFoodManager, menuInfo,0);
    }
}