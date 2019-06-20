using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class StoreInfoModel : BaseMVCModel
{
    private StoreInfoService mStoreInfoService;

    public override void InitData()
    {
        mStoreInfoService = new StoreInfoService();
    }

    /// <summary>
    /// 获取所有商店信息
    /// </summary>
    /// <returns></returns>
    public List<StoreInfoBean> GetAllStoreInfo()
    {
        List<StoreInfoBean> listData = mStoreInfoService.QueryAllData();
        return listData;
    }

    /// <summary>
    /// 根据类型获取商店信息
    /// </summary>
    /// <returns></returns>
    public List<StoreInfoBean> GetStoreInfoByType(int type)
    {
        List<StoreInfoBean> listData = mStoreInfoService.QueryDataByType(type);
        return listData;
    }
}