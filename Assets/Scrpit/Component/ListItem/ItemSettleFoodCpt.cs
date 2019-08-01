using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
public class ItemSettleFoodCpt : ItemGameBaseCpt
{
    public Image ivIcon;
    public Text tvContent;
    public Text tvPriceS;
    public Text tvPriceM;
    public Text tvPriceL;

    public void SetData(long foodId, int number)
    {
        InnFoodManager innFoodManager= GetUIManager<UIGameManager>().innFoodManager;
        MenuInfoBean foodData = innFoodManager.GetFoodDataById(foodId);
        Sprite foodIcon = innFoodManager.GetFoodSpriteByName(foodData.icon_key);
        if (ivIcon != null)
            ivIcon.sprite = foodIcon;
        if (tvContent != null)
            tvContent.text = foodData.name+" x"+ number;
        if (tvPriceL != null)
            tvPriceL.text = foodData.price_l * number + "";
        if (tvPriceM != null)
            tvPriceM.text = foodData.price_m * number + "";
        if (tvPriceS != null)
            tvPriceS.text = foodData.price_s * number + "";
    }
}