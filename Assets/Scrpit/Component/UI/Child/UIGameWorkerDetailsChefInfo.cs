using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIGameWorkerDetailsChefInfo : BaseMonoBehaviour
{
    [Header("控件")]
    public Text tvCookNumber;

    public GameObject objItemContent;
    public GameObject objItemModel;

    [Header("数据")]
    public CharacterWorkerForChefBean chefData;

    public void SetData(InnFoodManager innFoodManager,CharacterWorkerForChefBean chefData)
    {
        this.chefData = chefData;
        SetCookNumber(chefData.cookNumber);
        SetFoodData(innFoodManager, chefData.listFoodCookNumer);
    }

    //设置烹饪数量
    public void SetCookNumber(long number)
    {
        if (tvCookNumber != null)
            tvCookNumber.text = number + "";
    }

    /// <summary>
    /// 设置食物市局
    /// </summary>
    /// <param name="innFoodManager"></param>
    /// <param name="listFood"></param>
    public void SetFoodData(InnFoodManager innFoodManager, List<ItemBean> listFood)
    {
        CptUtil.RemoveChildsByActive(objItemContent.transform);
        if (innFoodManager == null || listFood == null || listFood.Count == 0)
        {
            objItemContent.SetActive(false);
            return;
        }
        objItemContent.SetActive(true);
        foreach (ItemBean itemData in listFood)
        {
            //获取食物数据
            MenuInfoBean menuInfo = innFoodManager.GetFoodDataById(itemData.itemId);
            if (menuInfo == null)
                continue;
            //获取食物图标
            Sprite spFoodIcon= innFoodManager.GetFoodSpriteByName(menuInfo.icon_key);
            //生成Item
            GameObject foodInfoCpt=  Instantiate(objItemModel, objItemContent.transform);
            foodInfoCpt.SetActive(true);
            //设置数据
            ItemGameWokerDetailsChefFoodInfoCpt itemCpt= foodInfoCpt.GetComponent<ItemGameWokerDetailsChefFoodInfoCpt>();
            itemCpt.SetData(menuInfo.name, itemData.itemNumber, spFoodIcon);
        }
    }
}