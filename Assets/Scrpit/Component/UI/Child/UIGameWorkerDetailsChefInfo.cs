using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIGameWorkerDetailsChefInfo : UIGameStatisticsDetailsBase<UIGameWorkerDetails>
{
    [Header("控件")]
    public GameObject objItemMenuContent;
    public GameObject objItemMenuModel;

    [Header("数据")]
    public CharacterWorkerForChefBean chefData;

    public void SetData(InnFoodManager innFoodManager, CharacterWorkerForChefBean chefData)
    {
        this.chefData = chefData;
        CptUtil.RemoveChildsByName(objItemContent.transform, "Item", true);
        AddCookNumber(chefData.cookNumber);
        //AddCookTime(chefData.cookTime);
        AddFoodData(innFoodManager, chefData.listFoodCookNumer);
    }


    /// <summary>
    /// 设置料理数量
    /// </summary>
    /// <param name="number"></param>
    public void AddCookNumber(long number)
    {
        Sprite spIcon = GetSpriteByName("worker_cook_pro_2");
        CreateTextItem(spIcon, GameCommonInfo.GetUITextById(311), number + "");
    }

    /// <summary>
    /// 设置料理时间
    /// </summary>
    /// <param name="time"></param>
    public void AddCookTime(float time)
    {
        Sprite spIcon = GetSpriteByName("hourglass_1");
        CreateTextItem(spIcon, GameCommonInfo.GetUITextById(312), time + GameCommonInfo.GetUITextById(38));
    }

    /// <summary>
    /// 设置食物数据
    /// </summary>
    /// <param name="innFoodManager"></param>
    /// <param name="listFood"></param>
    public void AddFoodData(InnFoodManager innFoodManager, List<ItemBean> listFood)
    {
        CptUtil.RemoveChildsByActive(objItemMenuContent.transform);
        if (innFoodManager == null || listFood == null || listFood.Count == 0)
        {
            objItemMenuContent.SetActive(false);
            return;
        }
        objItemMenuContent.SetActive(true);
        foreach (ItemBean itemData in listFood)
        {
            //获取食物数据
            MenuInfoBean menuInfo = innFoodManager.GetFoodDataById(itemData.itemId);
            if (menuInfo == null)
                continue;
            //获取食物图标
            Sprite spFoodIcon = innFoodManager.GetFoodSpriteByName(menuInfo.icon_key);
            //生成Item
            GameObject foodInfoCpt = Instantiate(objItemMenuContent, objItemMenuModel);
            //设置数据
            ItemGameWokerDetailsChefFoodInfoCpt itemCpt = foodInfoCpt.GetComponent<ItemGameWokerDetailsChefFoodInfoCpt>();
            itemCpt.SetData(menuInfo.name, itemData.itemNumber, spFoodIcon);
        }
    }
}