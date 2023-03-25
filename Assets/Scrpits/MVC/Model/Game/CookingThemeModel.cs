using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class CookingThemeModel : BaseMVCModel
{
    private CookingThemeService mCookingThemeService;

    public override void InitData()
    {
        mCookingThemeService = new CookingThemeService();
    }

    /// <summary>
    /// 获取所有料理主题
    /// </summary>
    public List<CookingThemeBean> GetAllCookingTheme()
    {
        List<CookingThemeBean> listData = mCookingThemeService.QueryAllTheme();
        return listData;
    }
}