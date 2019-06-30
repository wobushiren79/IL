using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class InnFoodManager : BaseManager, IMenuInfoView
{
    public IconBeanDictionary listFoodIcon;

    public List<MenuInfoBean> listMenuData;

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
    public List<MenuInfoBean> GetFoodDataListByMenuList(List<MenuOwnBean> listMenu)
    {
        List<MenuInfoBean> listFood = new List<MenuInfoBean>();
        if (listMenuData == null || listMenu == null)
            return listFood;
        for (int i = 0; i < listMenuData.Count; i++)
        {
            MenuInfoBean itemFoodData = listMenuData[i];
            for (int f = 0; f < listMenu.Count; f++)
            {
                MenuOwnBean itemMenuData = listMenu[f];
                if (itemMenuData.menuId == itemFoodData.id)
                {
                    listFood.Add(itemFoodData);
                    break;
                }
            }
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
        if (listMenuData == null)
            return null;
        for (int i = 0; i < listMenuData.Count; i++)
        {
            MenuInfoBean itemFoodData = listMenuData[i];
            if (itemFoodData.id == id)
            {
                return itemFoodData;
            }
        }
        return null;
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
        this.listMenuData = listData;
    }
    #endregion
}