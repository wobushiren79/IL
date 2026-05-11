using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class StoreInfoManager : BaseManager
{
    protected StoreInfoService storeInfoService;
    //商店数据
    public List<StoreInfoBean> listStoreData;

    public void Awake()
    {
        storeInfoService = new StoreInfoService();
    }


    /// <summary>
    /// 获取市场数据
    /// </summary>
    public void GetStoreInfoForMarket(Action<List<StoreInfoBean>> action)
    {
        List<StoreInfoBean> listData = storeInfoService.QueryDataByType((int)StoreTypeEnum.Market);
        action?.Invoke(listData);
    }

    /// <summary>
    /// 获取绸缎庄数据
    /// </summary>
    public void GetStoreInfoForDress(Action<List<StoreInfoBean>> action)
    {
        List<StoreInfoBean> listData = storeInfoService.QueryDataByType((int)StoreTypeEnum.Dress);
        action?.Invoke(listData);
    }

    /// <summary>
    /// 获取百宝阁数据
    /// </summary>
    public void GetStoreInfoForGrocery(Action<List<StoreInfoBean>> action)
    {
        List<StoreInfoBean> listData = storeInfoService.QueryDataByType((int)StoreTypeEnum.Grocery);
        action?.Invoke(listData);
    }

    /// <summary>
    /// 获取建筑坊数据
    /// </summary>
    public void GetStoreInfoForCarpenter(Action<List<StoreInfoBean>> action)
    {
        List<StoreInfoBean> listData = storeInfoService.QueryDataByType((int)StoreTypeEnum.Carpenter);
        action?.Invoke(listData);
    }
    /// <summary>
    /// 获取建筑坊床数据
    /// </summary>
    public void GetStoreInfoForCarpenterBed(Action<List<StoreInfoBean>> action)
    {
        List<StoreInfoBean> listData = storeInfoService.QueryDataByType((int)StoreTypeEnum.CarpenterBed);
        action?.Invoke(listData);
    }
    /// <summary>
    /// 获取药店数据
    /// </summary>
    public void GetStoreInfoForPharmacy(Action<List<StoreInfoBean>> action)
    {
        List<StoreInfoBean> listData = storeInfoService.QueryDataByType((int)StoreTypeEnum.Pharmacy);
        action?.Invoke(listData);
    }
    /// <summary>
    /// 获取公会角色提升数据
    /// </summary>
    public void GetStoreInfoForGuildImprove(Action<List<StoreInfoBean>> action)
    {
        List<StoreInfoBean> listData = storeInfoService.QueryDataByType((int)StoreTypeEnum.Improve);
        action?.Invoke(listData);
    }

    /// <summary>
    /// 获取公会商品
    /// </summary>
    public void GetStoreInfoForGuildGoods(Action<List<StoreInfoBean>> action)
    {
        List<StoreInfoBean> listData = storeInfoService.QueryDataByType((int)StoreTypeEnum.Guild);
        action?.Invoke(listData);
    }

    /// <summary>
    /// 获取公会客栈升级相关数据
    /// </summary>
    public void GetStoreInfoForGuildInnLevel(Action<List<StoreInfoBean>> action)
    {
        List<StoreInfoBean> listData = storeInfoService.QueryDataByType((int)StoreTypeEnum.InnLevel);
        action?.Invoke(listData);
    }

    /// <summary>
    /// 获取竞技场信息
    /// </summary>
    public void GetStoreInfoForArenaInfo(Action<List<StoreInfoBean>> action)
    {
        List<StoreInfoBean> listData = storeInfoService.QueryDataByType((int)StoreTypeEnum.ArenaInfo);
        action?.Invoke(listData);
    }

    /// <summary>
    /// 获取竞技场商品
    /// </summary>
    public void GetStoreInfoForArenaGoods(Action<List<StoreInfoBean>> action)
    {
        List<StoreInfoBean> listData = storeInfoService.QueryDataByType((int)StoreTypeEnum.ArenaGoods);
        action?.Invoke(listData);
    }
}