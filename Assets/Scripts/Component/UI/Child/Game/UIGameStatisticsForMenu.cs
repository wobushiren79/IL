using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;

public class UIGameStatisticsForMenu : BaseUIView
{
    public GameObject objMenuContainer;
    public GameObject objMenuModel;

    public override void OpenUI()
    {
        base.OpenUI();
        InitData();
    }

    public override void CloseUI()
    {
        base.CloseUI();
        CptUtil.RemoveChildsByActive(objMenuContainer);
    }

    public void InitData()
    {
        StopAllCoroutines();
        CptUtil.RemoveChildsByActive(objMenuContainer);
        Dictionary<long, MenuInfoBean> listMenu = InnFoodHandler.Instance.manager.listMenuData;
        StartCoroutine(CoroutineForCreateMenuList(listMenu));
    }

    public IEnumerator CoroutineForCreateMenuList(Dictionary<long, MenuInfoBean> listData)
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        foreach (var itemData in listData)
        {
            MenuOwnBean menuOwn= gameData.GetMenuById(itemData.Key);
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