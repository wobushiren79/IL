using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.U2D;
public class InnFoodManager : BaseManager, IMenuInfoView, ICookingThemeView
{
    //食物动画
    public Dictionary<string,AnimationClip> dicFoodAnim = new Dictionary<string, AnimationClip>();
    
    //菜单数据
    public Dictionary<long, MenuInfoBean> listMenuData;
    //料理主题数据
    public Dictionary<long, CookingThemeBean> listCookingTheme;  

    public MenuInfoController mMenuInfoController;
    public CookingThemeController mCookingThemeController;

    private void Awake()
    {
        mMenuInfoController = new MenuInfoController(this, this);
        mCookingThemeController = new CookingThemeController(this, this);

        mMenuInfoController.GetAllMenuInfo();
        mCookingThemeController.GetAllCookingTheme();
    }

    /// <summary>
    /// 获取食物动画
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public AnimationClip GetFoodAnimByName(string name)
    {
       return GetModelForAddressablesSync<AnimationClip>(dicFoodAnim,$"Assets/Anim/Animation/Food/{name}.anim");
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
            MenuInfoBean menuInfo = GetFoodDataById(itemFoodData.menuId);
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
    /// 根据料理主题随机获取一个料理
    /// </summary>
    public MenuInfoBean GetRandomFoodDataByCookingTheme(CookingThemeBean cookingTheme)
    {
        //TODO
        return RandomUtil.GetRandomDataByDictionary(listMenuData);
    }

    /// <summary>
    /// 随机获取一个料理主题
    /// </summary>
    /// <returns></returns>
    public CookingThemeBean GetRandomCookingTheme()
    {
        int randomTempNum = Random.Range(0,listCookingTheme.Count);
        int i = 0;
        foreach (var item in listCookingTheme)
        {
            if(i == randomTempNum)
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
    /// <param name="themeId"></param>
    /// <returns></returns>
    public CookingThemeBean GetCookingThemeById(long themeId)
    {
        if (listCookingTheme.TryGetValue(themeId, out CookingThemeBean cookingTheme))
        {
            return cookingTheme;
        }
        return null;
    }

    /// <summary>
    /// 通过等级获取烹饪主题
    /// </summary>
    /// <param name="themeLevel"></param>
    /// <returns></returns>
    public List<CookingThemeBean> GetCookingThemeByLevel(int themeLevel)
    {
        List<CookingThemeBean> listData = new List<CookingThemeBean>();
        if (listCookingTheme == null)
            return listData;
        foreach ( var itemData in listCookingTheme)
        {
            CookingThemeBean itemCookingTheme = itemData.Value;
            if (itemCookingTheme.theme_level== themeLevel)
            {
                listData.Add(itemCookingTheme);
            }
        }
        return listData;
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

    #region 料理主题回调
    public void GetAllCookingThemeSuccess(List<CookingThemeBean> listData)
    {
        listCookingTheme = new Dictionary<long, CookingThemeBean>();
        if (listData != null)
            foreach (CookingThemeBean itemData in listData)
            {
                listCookingTheme.Add(itemData.id, itemData);
            }
    }

    public void GetAllCookingThemeFail()
    {
    }
    #endregion
}