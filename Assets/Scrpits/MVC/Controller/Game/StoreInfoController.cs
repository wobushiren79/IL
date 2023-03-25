using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

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
    public void GetAllStoreInfo(Action<List<StoreInfoBean>> action)
    {
        List<StoreInfoBean> listData = GetModel().GetAllStoreInfo();
        if (!listData.IsNull())
        {
            GetView().GetAllStoreInfoSuccess(listData, action);
        }
        else
        {
            GetView().GetAllStoreInfoFail();
        }
    }

    /// <summary>
    /// 获取杂货店数据
    /// </summary>
    public void GetGroceryInfo(Action<List<StoreInfoBean>> action)
    {
        GetStoreInfoByType(StoreTypeEnum.Grocery, action);
    }

    /// <summary>
    /// 获取服装店数据
    /// </summary>
    public void GetDressStoreInfo(Action<List<StoreInfoBean>> action)
    {
        GetStoreInfoByType(StoreTypeEnum.Dress, action);
    }

    /// <summary>
    /// 获取建筑坊数据
    /// </summary>
    public void GetCarpenterInfo(Action<List<StoreInfoBean>> action)
    {
        GetStoreInfoByType(StoreTypeEnum.Carpenter, action);
    }
    /// <summary>
    /// 获取建筑坊床数据
    /// </summary>
    public void GetCarpenterBedInfo(Action<List<StoreInfoBean>> action)
    {
        GetStoreInfoByType(StoreTypeEnum.CarpenterBed, action);
    }
    /// <summary>
    /// 获取建筑坊数据
    /// </summary>
    public void GetPharmacyInfo(Action<List<StoreInfoBean>> action)
    {
        GetStoreInfoByType(StoreTypeEnum.Pharmacy, action);
    }

    /// <summary>
    /// 获取市场数据
    /// </summary>
    public void GetMarketStoreInfo(Action<List<StoreInfoBean>> action)
    {
        GetStoreInfoByType(StoreTypeEnum.Market, action);
    }

    /// <summary>
    /// 获取公会商店数据
    /// </summary>
    public void GetGuildStoreInfo(Action<List<StoreInfoBean>> action)
    {
        GetStoreInfoByType(StoreTypeEnum.Guild, action);
    }

    /// <summary>
    /// 获取角色提升数据
    /// </summary>
    public void GetGuildImproveForCharacter(Action<List<StoreInfoBean>> action)
    {
        GetStoreInfoByType(StoreTypeEnum.Improve, action);
    }

    /// <summary>
    /// 获取公会客栈升级数据
    /// </summary>
    public void GetGuildInnLevel(Action<List<StoreInfoBean>> action)
    {
        GetStoreInfoByType(StoreTypeEnum.InnLevel, action);
    }

    /// <summary>
    /// 获取竞技场信息
    /// </summary>
    public void GetArenaInfo(Action<List<StoreInfoBean>> action)
    {
        GetStoreInfoByType(StoreTypeEnum.ArenaInfo, action);
    }

    /// <summary>
    /// 获取竞技场商品
    /// </summary>
    public void GetArenaGoods(Action<List<StoreInfoBean>> action)
    {
        GetStoreInfoByType(StoreTypeEnum.ArenaGoods, action);
    }

    /// <summary>
    /// 查询所有商店信息
    /// </summary>
    public void GetStoreInfoByType(StoreTypeEnum type, Action<List<StoreInfoBean>> action)
    {
        List<StoreInfoBean> listData = GetModel().GetStoreInfoByType(type);
        if (!listData.IsNull())
        {
            GetView().GetStoreInfoByTypeSuccess(type, listData, action);
        }
        else
        {
            GetView().GetStoreInfoByTypeFail(type);
        }
    }
}