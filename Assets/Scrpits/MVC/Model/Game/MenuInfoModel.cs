using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MenuInfoModel : BaseMVCModel
{

    private MenuInfoService mMenuInfoService;

    public override void InitData()
    {
        mMenuInfoService = new MenuInfoService();
    }

    /// <summary>
    /// 获取所有菜单信息
    /// </summary>
    /// <returns></returns>
    public List<MenuInfoBean> GetAllMenuInfo()
    {
       return mMenuInfoService.QueryAllData();
    }
}