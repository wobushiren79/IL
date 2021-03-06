﻿using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class PickForSellDialogView : DialogView
{
    public GameObject objItemsContainer;
    public GameObject objItemsModel;

    public Text tvNull;

    public List<StoreInfoBean> listStoreData = new List<StoreInfoBean>();


    public override void InitData()
    {
        base.InitData();
        int number = 0; 
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        List<ItemBean> listBuildData=  gameData.listBuild;
        foreach (ItemBean itemData in  listBuildData)
        {
            foreach (StoreInfoBean itemStore in listStoreData)
            {
                if (itemStore.mark_type == 2 && itemStore.mark_id == itemData.itemId)
                {
                    CreateItem(itemData, itemStore);
                    number++;
                }
            }
        }
        List<ItemBean> listItemData = gameData.listItems;
        foreach (ItemBean itemData in listItemData)
        {
            foreach (StoreInfoBean itemStore in listStoreData)
            {
                if (itemStore.mark_type == 1 && itemStore.mark_id == itemData.itemId)
                {
                    CreateItem(itemData, itemStore);
                    number++;
                }
            }
        }
        if (number == 0)
        {
            tvNull.gameObject.SetActive(true);
        }
    }

    public void SetData(List<StoreInfoBean> listStoreData)
    {
        this.listStoreData = listStoreData;
    }

    public void CreateItem(ItemBean itemData, StoreInfoBean storeInfo)
    {
        GameObject objItem = Instantiate(objItemsContainer, objItemsModel);
        ItemDialogPickForSellCpt itemCpt= objItem.GetComponent<ItemDialogPickForSellCpt>();
        itemCpt.SetData( itemData,  storeInfo);
    }
}