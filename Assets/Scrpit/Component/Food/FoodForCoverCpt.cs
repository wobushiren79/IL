using UnityEngine;
using UnityEditor;
using DG.Tweening;
public class FoodForCoverCpt : BaseMonoBehaviour
{
    public SpriteRenderer srCover;


    public FoodForCustomerCpt foodCustomerCpt;
    public InnFoodManager innFoodManager;
    public MenuInfoBean menuInfo;
    

    private void Awake()
    {
        innFoodManager = FindObjectOfType<InnFoodManager>();
    }

    public void SetData(MenuInfoBean menuInfo)
    {
        foodCustomerCpt.gameObject.SetActive(false);
        this.menuInfo = menuInfo;
        foodCustomerCpt.SetData(innFoodManager, menuInfo,0);
    }

    /// <summary>
    /// 展示食物
    /// </summary>
    public void ShowFood()
    {
        foodCustomerCpt.gameObject.SetActive(true);
        foodCustomerCpt.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 1).From().SetEase(Ease.OutBack);
        srCover.DOFade(0, 1);
        srCover.transform.DOLocalMoveY(1,1);
    }

    /// <summary>
    /// 消灭食物
    /// </summary>
    public void FinshFood()
    {
        foodCustomerCpt.FinishFood(menuInfo);
    }
}