﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;

public class UIGameStatisticsForMenu : BaseUIChildComponent<UIGameStatistics>
{
    public GameObject objMenuContainer;
    public GameObject objMenuModel;

    public override void Open()
    {
        base.Open();
        InitData();
    }

    public override void Close()
    {
        base.Close();
        CptUtil.RemoveChildsByActive(objMenuContainer);
    }

    public void InitData()
    {
        StopAllCoroutines();
        CptUtil.RemoveChildsByActive(objMenuContainer);
        Dictionary<long, MenuInfoBean> listMenu= uiComponent.uiGameManager.innFoodManager.listMenuData;
        StartCoroutine(CoroutineForCreateMenuList(listMenu));
    }

    public IEnumerator CoroutineForCreateMenuList(Dictionary<long, MenuInfoBean> listData)
    {
        GameDataManager gameDataManager = uiComponent.uiGameManager.gameDataManager;
        foreach (var itemData in listData)
        {
            MenuOwnBean menuOwn= gameDataManager.gameData.GetMenuById(itemData.Key);
            CreateMenuItem(menuOwn,itemData.Value);
            yield return new WaitForEndOfFrame();
        }
    }

    public void CreateMenuItem(MenuOwnBean menuOwn,MenuInfoBean menuInfo)
    {
         GameObject objMenuItem = Instantiate(objMenuContainer, objMenuModel);
         ItemGameStatisticsForMenuCpt itemCpt = objMenuItem.GetComponent<ItemGameStatisticsForMenuCpt>();
         itemCpt.SetData(menuOwn, menuInfo);
    }
}