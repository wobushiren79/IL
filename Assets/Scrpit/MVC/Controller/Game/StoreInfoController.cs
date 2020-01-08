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
    /// 获取杂货店数据
    /// </summary>
    public void GetGroceryInfo()
    {
        GetStoreInfoByType(StoreTypeEnum.Grocery);
    }

    /// <summary>
    /// 获取服装店数据
    /// </summary>
    public void GetDressStoreInfo()
    {
        GetStoreInfoByType(StoreTypeEnum.Dress);
    }

    /// <summary>
    /// 获取建筑坊数据
    /// </summary>
    public void GetCarpenterInfo()
    {
        GetStoreInfoByType(StoreTypeEnum.Carpenter);
    }

    /// <summary>
    /// 获取市场数据
    /// </summary>
    public void GetMarketStoreInfo()
    {
        GetStoreInfoByType(StoreTypeEnum.Market);
    }

    /// <summary>
    /// 获取公会商店数据
    /// </summary>
    public void GetGuildStoreInfo()
    {
        GetStoreInfoByType(StoreTypeEnum.Guild);
    }

    /// <summary>
    /// 获取角色提升数据
    /// </summary>
    public void GetGuildImproveForCharacter()
    {
        GetStoreInfoByType(StoreTypeEnum.Improve);
    }

    /// <summary>
    /// 获取公会客栈升级数据
    /// </summary>
    public void GetGuildInnLevel()
    {
        GetStoreInfoByType(StoreTypeEnum.InnLevel);
    }

    /// <summary>
    /// 查询所有商店信息
    /// </summary>
    public void GetStoreInfoByType(StoreTypeEnum type)
    {
        List<StoreInfoBean> listData = GetModel().GetStoreInfoByType(type);
        if (!CheckUtil.ListIsNull(listData))
        {
            GetView().GetStoreInfoByTypeSuccess(type, listData);
        }
        else
        {
            GetView().GetStoreInfoByTypeFail(type);
        }
    }
}