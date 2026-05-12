using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class StoreInfoManager : BaseManager
{
    public void Awake()
    {
    }

    private List<StoreInfoBean> GetStoreInfoByType(int type)
    {
        List<StoreInfoBean> listData = new List<StoreInfoBean>();
        var dicData = StoreInfoCfg.GetAllData();
        if (dicData == null)
            return listData;
        foreach (var item in dicData)
        {
            if (item.Value.type == type)
                listData.Add(item.Value);
        }
        return listData;
    }

    /// <summary>
    /// 获取市场数据
    /// </summary>
    public void GetStoreInfoForMarket(Action<List<StoreInfoBean>> action)
    {
        action?.Invoke(GetStoreInfoByType((int)StoreTypeEnum.Market));
    }

    /// <summary>
    /// 获取绸缎庄数据
    /// </summary>
    public void GetStoreInfoForDress(Action<List<StoreInfoBean>> action)
    {
        action?.Invoke(GetStoreInfoByType((int)StoreTypeEnum.Dress));
    }

    /// <summary>
    /// 获取百宝阁数据
    /// </summary>
    public void GetStoreInfoForGrocery(Action<List<StoreInfoBean>> action)
    {
        action?.Invoke(GetStoreInfoByType((int)StoreTypeEnum.Grocery));
    }

    /// <summary>
    /// 获取建筑坊数据
    /// </summary>
    public void GetStoreInfoForCarpenter(Action<List<StoreInfoBean>> action)
    {
        action?.Invoke(GetStoreInfoByType((int)StoreTypeEnum.Carpenter));
    }

    /// <summary>
    /// 获取建筑坊床数据
    /// </summary>
    public void GetStoreInfoForCarpenterBed(Action<List<StoreInfoBean>> action)
    {
        action?.Invoke(GetStoreInfoByType((int)StoreTypeEnum.CarpenterBed));
    }

    /// <summary>
    /// 获取药店数据
    /// </summary>
    public void GetStoreInfoForPharmacy(Action<List<StoreInfoBean>> action)
    {
        action?.Invoke(GetStoreInfoByType((int)StoreTypeEnum.Pharmacy));
    }

    /// <summary>
    /// 获取公会角色提升数据
    /// </summary>
    public void GetStoreInfoForGuildImprove(Action<List<StoreInfoBean>> action)
    {
        action?.Invoke(GetStoreInfoByType((int)StoreTypeEnum.Improve));
    }

    /// <summary>
    /// 获取公会商品
    /// </summary>
    public void GetStoreInfoForGuildGoods(Action<List<StoreInfoBean>> action)
    {
        action?.Invoke(GetStoreInfoByType((int)StoreTypeEnum.Guild));
    }

    /// <summary>
    /// 获取公会客栈升级相关数据
    /// </summary>
    public void GetStoreInfoForGuildInnLevel(Action<List<StoreInfoBean>> action)
    {
        action?.Invoke(GetStoreInfoByType((int)StoreTypeEnum.InnLevel));
    }

    /// <summary>
    /// 获取竞技场信息
    /// </summary>
    public void GetStoreInfoForArenaInfo(Action<List<StoreInfoBean>> action)
    {
        action?.Invoke(GetStoreInfoByType((int)StoreTypeEnum.ArenaInfo));
    }

    /// <summary>
    /// 获取竞技场商品
    /// </summary>
    public void GetStoreInfoForArenaGoods(Action<List<StoreInfoBean>> action)
    {
        action?.Invoke(GetStoreInfoByType((int)StoreTypeEnum.ArenaGoods));
    }
}
