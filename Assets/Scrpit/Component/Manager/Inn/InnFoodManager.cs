using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class InnFoodManager : BaseManager, IMenuInfoView
{
    public IconBeanDictionary listFoodIcon;

    public Dictionary<long, MenuInfoBean> listMenuData;

    public MenuInfoController mMenuInfoController;

    /// <summary>
    /// 通过名字获取食物图标
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetFoodSpriteByName(string name)
    {
        return GetSpriteByName(name + "_0", listFoodIcon);
    }
    /// <summary>
    /// 通过名字获取食物图标
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetFoodLastSpriteByName(string name)
    {
        return GetSpriteByName(name + "_1", listFoodIcon);
    }

    /// <summary>
    /// 通过自己的列表获取食物数据
    /// </summary>
    /// <param name="listMenu"></param>
    public List<MenuInfoBean> GetFoodDataListByMenuList(List<MenuOwnBean> listOwnMenu)
    {
        List<MenuInfoBean> listFood = new List<MenuInfoBean>();
        if (listMenuData == null || listOwnMenu == null)
            return listFood;
        for (int i = 0; i < listOwnMenu.Count; i++)
        {
            MenuOwnBean itemFoodData = listOwnMenu[i];
            MenuInfoBean menuInfo= GetFoodDataById(itemFoodData.menuId);
            if (menuInfo != null)
                listFood.Add(menuInfo);
        }
        return listFood;
    }

    /// <summary>
    /// 通过列表获取食物数据
    /// </summary>
    /// <param name="listItem"></param>
    /// <returns></returns>
    public List<MenuInfoBean> GetFoodDataListByItemList(List<ItemBean> listItem)
    {
        List<MenuInfoBean> listFood = new List<MenuInfoBean>();
        if (listMenuData == null || listItem == null)
            return listFood;
        for (int i = 0; i < listItem.Count; i++)
        {
            ItemBean itemFoodData = listItem[i];
            MenuInfoBean menuInfo = GetFoodDataById(itemFoodData.itemId);
            if (menuInfo != null)
                listFood.Add(menuInfo);
        }
        return listFood;
    }

    /// <summary>
    /// 根据ID获取食物数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public MenuInfoBean GetFoodDataById(long id)
    {
        return GetDataById(id, listMenuData);
    }

    private void Awake()
    {
        mMenuInfoController = new MenuInfoController(this, this);
    }
    private void Start()
    {
        mMenuInfoController.GetAllMenuInfo();
    }

    #region 菜单回调
    public void GetAllMenuInfFail()
    {

    }

    public void GetAllMenuInfoSuccess(List<MenuInfoBean> listData)
    {
        listMenuData = new Dictionary<long, MenuInfoBean>();
        if (listData != null)
            foreach (MenuInfoBean itemData in listData)
            {
                listMenuData.Add(itemData.id, itemData);
            }
    }
    #endregion
}