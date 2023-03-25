using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MenuInfoController : BaseMVCController<MenuInfoModel, IMenuInfoView>
{
    public MenuInfoController(BaseMonoBehaviour content, IMenuInfoView view) : base(content, view)
    {

    }

    public override void InitData()
    {

    }

    /// <summary>
    /// 获取所有菜单信息
    /// </summary>
    public void GetAllMenuInfo()
    {
        List<MenuInfoBean> listData = GetModel().GetAllMenuInfo();
        if (listData.IsNull())
        {
            GetView().GetAllMenuInfFail();
        }
        else
        {
            GetView().GetAllMenuInfoSuccess(listData);
        }
    }
}