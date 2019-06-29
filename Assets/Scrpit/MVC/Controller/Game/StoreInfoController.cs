using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class StoreInfoController : BaseMVCController<StoreInfoModel, IStoreInfoView>
{
    public StoreInfoController(BaseMonoBehaviour content, IStoreInfoView view) : base(content, view)
    {

    }

    public override void InitData()
    {

    }

    /// <summary>
    /// 查询所有商店信息
    /// </summary>
    public void GetAllStoreInfo()
    {
        List<StoreInfoBean> listData = GetModel().GetAllStoreInfo();
        if (!CheckUtil.ListIsNull(listData))
        {
            GetView().GetAllStoreInfoSuccess(listData);
        }
        else
        {
            GetView().GetAllStoreInfoFail();
        }
    }

    /// <summary>
    /// 获取市场数据
    /// </summary>
    public void GetMarketStoreInfo()
    {
        GetStoreInfoByType(9);
    }

    /// <summary>
    /// 获取杂货店数据
    /// </summary>
    public void GetGroceryInfo()
    {
        GetStoreInfoByType(1);
    }

    /// <summary>
    /// 查询所有商店信息
    /// </summary>
    public void GetStoreInfoByType(int type)
    {
        List<StoreInfoBean> listData = GetModel().GetStoreInfoByType(type);
        if (!CheckUtil.ListIsNull(listData))
        {
            GetView().GetStoreInfoByTypeSuccess(listData);
        }
        else
        {
            GetView().GetStoreInfoByTypeFail();
        }
    }
}