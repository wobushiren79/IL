using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class StoreInfoManager : BaseManager, IStoreInfoView
{  
    protected StoreInfoController storeInfoController;
    //商店数据
    public List<StoreInfoBean> listStoreData;

    public void Awake()
    {
        storeInfoController = new StoreInfoController(this, this);
    }


    /// <summary>
    /// 获取市场数据
    /// </summary>
    public void GetStoreInfoForMarket(Action<List<StoreInfoBean>> action)
    {
        storeInfoController.GetMarketStoreInfo(action);
    }

    /// <summary>
    /// 获取绸缎庄数据
    /// </summary>
    public void GetStoreInfoForDress(Action<List<StoreInfoBean>> action)
    {
        storeInfoController.GetDressStoreInfo(action);
    }

    /// <summary>
    /// 获取百宝阁数据
    /// </summary>
    public void GetStoreInfoForGrocery(Action<List<StoreInfoBean>> action)
    {
        storeInfoController.GetGroceryInfo(action);
    }

    /// <summary>
    /// 获取建筑坊数据
    /// </summary>
    public void GetStoreInfoForCarpenter(Action<List<StoreInfoBean>> action)
    {
        storeInfoController.GetCarpenterInfo(action);
    }
    /// <summary>
    /// 获取建筑坊床数据
    /// </summary>
    public void GetStoreInfoForCarpenterBed(Action<List<StoreInfoBean>> action)
    {
        storeInfoController.GetCarpenterBedInfo(action);
    }
    /// <summary>
    /// 获取建筑坊数据
    /// </summary>
    public void GetStoreInfoForPharmacy(Action<List<StoreInfoBean>> action)
    {
        storeInfoController.GetPharmacyInfo(action);
    }
    /// <summary>
    /// 获取公会角色提升数据
    /// </summary>
    public void GetStoreInfoForGuildImprove(Action<List<StoreInfoBean>> action)
    {
        storeInfoController.GetGuildImproveForCharacter(action);
    }

    /// <summary>
    /// 获取公会商品
    /// </summary>
    public void GetStoreInfoForGuildGoods(Action<List<StoreInfoBean>> action)
    {
        storeInfoController.GetGuildStoreInfo(action);
    }

    /// <summary>
    /// 获取公会客栈升级相关数据
    /// </summary>
    public void GetStoreInfoForGuildInnLevel(Action<List<StoreInfoBean>> action)
    {
        storeInfoController.GetGuildInnLevel(action);
    }

    /// <summary>
    /// 获取竞技场信息
    /// </summary>
    public void GetStoreInfoForArenaInfo(Action<List<StoreInfoBean>> action)
    {
        storeInfoController.GetArenaInfo(action);
    }

    /// <summary>
    /// 获取竞技场商品
    /// </summary>
    public void GetStoreInfoForArenaGoods(Action<List<StoreInfoBean>> action)
    {
        storeInfoController.GetArenaGoods(action);
    }

    #region 数据回调
    public void GetAllStoreInfoSuccess(List<StoreInfoBean> listData, Action<List<StoreInfoBean>> action)
    {
        action?.Invoke(listData);
    }

    public void GetAllStoreInfoFail()
    {

    }

    public void GetStoreInfoByTypeSuccess(StoreTypeEnum type, List<StoreInfoBean> listData, Action<List<StoreInfoBean>> action)
    {
        action?.Invoke(listData);
    }

    public void GetStoreInfoByTypeFail(StoreTypeEnum type)
    {

    }
    #endregion

}