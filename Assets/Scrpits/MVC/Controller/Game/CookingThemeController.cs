using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class CookingThemeController : BaseMVCController<CookingThemeModel, ICookingThemeView>
{
    public CookingThemeController(BaseMonoBehaviour content, ICookingThemeView view) : base(content, view)
    {

    }

    public override void InitData()
    {

    }

    /// <summary>
    /// 获取所有料理主题
    /// </summary>
    public void GetAllCookingTheme()
    {
        List<CookingThemeBean> listData = GetModel().GetAllCookingTheme();
        if (listData != null)
            GetView().GetAllCookingThemeSuccess(listData);
        else
            GetView().GetAllCookingThemeFail();
    }
}