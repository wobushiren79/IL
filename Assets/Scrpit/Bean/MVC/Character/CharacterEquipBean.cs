using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
public class CharacterEquipBean
{
    public long handId;
    public long hatId;
    public long clothesId;
    public long shoesId;

    //学会的书籍
    public List<long> listLearnBook=new List<long>();

    /// <summary>
    /// 获取装备的属性加成
    /// </summary>
    /// <param name="gameItemsManager"></param>
    /// <returns></returns>
    public CharacterAttributesBean GetEquipAttributes(GameItemsManager gameItemsManager)
    {
        CharacterAttributesBean attributesBean = new CharacterAttributesBean();
        ItemsInfoBean handItem = gameItemsManager.GetItemsById(handId);
        ItemsInfoBean hatItem = gameItemsManager.GetItemsById(hatId);
        ItemsInfoBean clothesItem = gameItemsManager.GetItemsById(clothesId);
        ItemsInfoBean shoesItem = gameItemsManager.GetItemsById(shoesId);
        if (handItem != null)
            AddAttributes(attributesBean, handItem);
        if (hatItem != null)
            AddAttributes(attributesBean, hatItem);
        if (clothesItem != null)
            AddAttributes(attributesBean, clothesItem);
        if (shoesItem != null)
            AddAttributes(attributesBean, shoesItem);
        return attributesBean;
    }

    private void AddAttributes(CharacterAttributesBean attributesBean, ItemsInfoBean itemsInfo)
    {
        attributesBean.cook += itemsInfo.add_cook;
        attributesBean.speed += itemsInfo.add_speed;
        attributesBean.account += itemsInfo.add_account;
        attributesBean.charm += itemsInfo.add_charm;
        attributesBean.force += itemsInfo.add_force;
        attributesBean.lucky += itemsInfo.add_lucky;
        attributesBean.loyal += itemsInfo.add_loyal;
    }

    /// <summary>
    /// 检测是否学习过该书籍
    /// </summary>
    /// <param name="bookId"></param>
    /// <returns></returns>
    public bool CheckLearnBook(long bookId)
    {
        for (int i = 0; i < listLearnBook.Count; i++)
        {
           long itemId= listLearnBook[i];
            if (itemId == bookId)
            {
                return true;
            }
        }
        return false;
    }

}