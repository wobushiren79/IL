using UnityEngine;
using UnityEditor;
using DG.Tweening;

public class FoodForCustomerCpt : BaseMonoBehaviour
{
    //食物样式
    public SpriteRenderer srFood;

    public GameObject objGoodFood;
    public GameObject objPrefectFood;

    //食物动画
    public Animator animForFood;
    protected AnimatorOverrideController aocForFood;
    public AnimationClip animForOriginalClip;


    public void Awake()
    {
        aocForFood = new AnimatorOverrideController(animForFood.runtimeAnimatorController);
        animForFood.runtimeAnimatorController = aocForFood;
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="foodData"></param>
    public void SetData(MenuInfoBean foodData,int foodLevel)
    {
        if (foodData != null )
        {
            //设置图标
            srFood.sprite = InnFoodHandler.Instance.manager.GetFoodSpriteByName(foodData.icon_key);
            //设置动画
            if (!foodData.anim_key.IsNull())
            {
                AnimationClip animationClip = InnFoodHandler.Instance.manager.GetFoodAnimByName(foodData.anim_key);
                if (animationClip != null)
                {
                    animForFood.enabled = true;
                    aocForFood["Original"] = animationClip;   
                }
                else
                {
                    animForFood.enabled = false;
                }
            }
            else
            {
                animForFood.enabled = false;
            }
        }

        objGoodFood.SetActive(false);
        objPrefectFood.SetActive(false);
        if (foodData != null)
        {
            RarityEnum rarity = foodData.GetRarity();
            switch (rarity)
            {
                case RarityEnum.SuperRare:
                    objPrefectFood.SetActive(true);
                    break;
                case RarityEnum.SuperiorSuperRare:
                    objGoodFood.SetActive(true);
                    objPrefectFood.SetActive(true);
                    break;
            }
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

        if (foodData != null)
        {
            srFood.sprite = InnFoodHandler.Instance.manager.GetFoodLastSpriteByName(foodData.icon_key);
        }
   
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