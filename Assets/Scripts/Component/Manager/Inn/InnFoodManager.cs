using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.U2D;
public class InnFoodManager : BaseManager
{
    //食物动画
    public Dictionary<string,AnimationClip> dicFoodAnim = new Dictionary<string, AnimationClip>();

    //菜单数据
    public Dictionary<long, MenuInfoBean> listMenuData => MenuInfoCfg.GetAllData();
    //料理主题数据
    public Dictionary<long, CookingThemeBean> listCookingTheme => CookingThemeCfg.GetAllData();

    private void Awake()
    {
    }

    /// <summary>
    /// 获取食物动画
    /// </summary>
    public AnimationClip GetFoodAnimByName(string name)
    {
       return GetModelForAddressablesSync<AnimationClip>(dicFoodAnim,$"Assets/Anim/Animation/Food/{name}.anim");
    }



    /// <summary>
    /// 通过自己的列表获取食物数据
    /// </summary>
    public List<MenuInfoBean> GetFoodDataListByMenuList(List<MenuOwnBean> listOwnMenu)
    {
        List<MenuInfoBean> listFood = new List<MenuInfoBean>();
        if (listOwnMenu == null)
            return listFood;
        var dicMenu = listMenuData;
        if (dicMenu == null)
            return listFood;
        for (int i = 0; i < listOwnMenu.Count; i++)
        {
            MenuOwnBean itemFoodData = listOwnMenu[i];
            MenuInfoBean menuInfo = GetFoodDataById(itemFoodData.menuId);
            if (menuInfo != null)
                listFood.Add(menuInfo);
        }
        return listFood;
    }

    /// <summary>
    /// 通过列表获取食物数据
    /// </summary>
    public List<MenuInfoBean> GetFoodDataListByItemList(List<ItemBean> listItem)
    {
        List<MenuInfoBean> listFood = new List<MenuInfoBean>();
        if (listItem == null)
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
    /// 根据料理主题随机获取一个料理
    /// </summary>
    public MenuInfoBean GetRandomFoodDataByCookingTheme(CookingThemeBean cookingTheme)
    {
        return RandomUtil.GetRandomDataByDictionary(listMenuData);
    }

    /// <summary>
    /// 随机获取一个料理主题
    /// </summary>
    public CookingThemeBean GetRandomCookingTheme()
    {
        var dicTheme = listCookingTheme;
        if (dicTheme == null || dicTheme.Count == 0)
            return null;
        int randomTempNum = Random.Range(0, dicTheme.Count);
        int i = 0;
        foreach (var item in dicTheme)
        {
            if (i == randomTempNum)
            {
                return item.Value;
            }
            i++;
        }
        return null;
    }

    /// <summary>
    /// 通过主题ID获取料理主题
    /// </summary>
    public CookingThemeBean GetCookingThemeById(long themeId)
    {
        return CookingThemeCfg.GetItemData(themeId);
    }

    /// <summary>
    /// 通过等级获取烹饪主题
    /// </summary>
    public List<CookingThemeBean> GetCookingThemeByLevel(int themeLevel)
    {
        List<CookingThemeBean> listData = new List<CookingThemeBean>();
        var dicTheme = listCookingTheme;
        if (dicTheme == null)
            return listData;
        foreach (var itemData in dicTheme)
        {
            CookingThemeBean itemCookingTheme = itemData.Value;
            if (itemCookingTheme.theme_level == themeLevel)
            {
                listData.Add(itemCookingTheme);
            }
        }
        return listData;
    }

    /// <summary>
    /// 根据ID获取食物数据
    /// </summary>
    public MenuInfoBean GetFoodDataById(long id)
    {
        return MenuInfoCfg.GetItemData(id);
    }
}
