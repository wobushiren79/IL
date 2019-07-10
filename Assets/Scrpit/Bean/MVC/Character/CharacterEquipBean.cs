﻿using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class CharacterEquipBean
{
    public long handId;

    public long hatId;
    public long clothesId;
    public long shoesId;

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
}