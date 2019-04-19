using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class InnFoodManager : BaseManager,IMenuInfoView
{
    public List<IconBean> listFoodIcon;
    public List<IconBean> listFoodLastIcon;

    public List<MenuInfoBean> listMenuData;

    public MenuInfoController mMenuInfoController;

    /// <summary>
    /// 通过名字获取食物图标
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetFoodSpriteByName(string name)
    {
       return GetSpriteByName(name, listFoodIcon);
    }
    /// <summary>
    /// 通过名字获取食物图标
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetFoodLastSpriteByName(string name)
    {
        return GetSpriteByName(name, listFoodLastIcon);
    }
    private void Awake()
    {
        mMenuInfoController = new MenuInfoController(this,this);
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