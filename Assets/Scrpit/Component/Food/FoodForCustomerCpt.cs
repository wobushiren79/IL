using UnityEngine;
using UnityEditor;
using DG.Tweening;

public class FoodForCustomerCpt : BaseMonoBehaviour
{
    //食物样式
    public SpriteRenderer srFood;

    public GameObject objBadFood;
    public GameObject objGoodFood;
    public GameObject objPrefectFood;

    //食物动画
    public Animator animForFood;
    protected AnimatorOverrideController aocForFood;
    public AnimationClip animForOriginalClip;

    //食物数据管理
    protected InnFoodManager innFoodManager;

    public void Awake()
    {
        innFoodManager = Find<InnFoodManager>( ImportantTypeEnum.FoodManager);

        aocForFood = new AnimatorOverrideController(animForFood.runtimeAnimatorController);
        animForFood.runtimeAnimatorController = aocForFood;
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="foodData"></param>
    public void SetData(InnFoodManager innFoodManager,MenuInfoBean foodData,int foodLevel)
    {
        this.innFoodManager = innFoodManager;
        if (foodData != null && innFoodManager != null)
        {
            //设置图标
            srFood.sprite = innFoodManager.GetFoodSpriteByName(foodData.icon_key);
            //设置动画
            if (!CheckUtil.StringIsNull(foodData.anim_key))
            {
                AnimationClip animationClip = innFoodManager.GetFoodAnimByName(foodData.anim_key);
                if (animationClip != null)
                {
                    aocForFood["Original"] = animationClip;
                }
            }
        }

        objBadFood.SetActive(false);
        objGoodFood.SetActive(false);
        objPrefectFood.SetActive(false);
        if (foodData != null && foodData.GetRarity()== RarityEnum.SuperRare)
        {
            objPrefectFood.SetActive(true);
        }
        //switch (foodLevel)
        //{
        //    case -1:
        //        objBadFood.SetActive(true);
        //        break;
        //    case 0:
        //        break;
        //    case 1:
        //        objGoodFood.SetActive(true);
        //        break;
        //    case 2:
        //        objPrefectFood.SetActive(true);
        //        break;
        //}


    }

    /// <summary>
    /// 吃完食物
    /// </summary>
    public void FinishFood(MenuInfoBean foodData)
    {
        //还原动画
        aocForFood["Original"] = animForOriginalClip;

        if (foodData != null && innFoodManager != null)
        {
            srFood.sprite = innFoodManager.GetFoodLastSpriteByName(foodData.icon_key);
        }
   
        objBadFood.SetActive(false);
        objGoodFood.SetActive(false);
        objPrefectFood.SetActive(false);


        //食物完结动画
        transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 0.3f);


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