using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class StoreInfoManager : BaseMonoBehaviour, IStoreInfoView
{  
    private StoreInfoController mStoreInfoController;
    //商店数据
    public List<StoreInfoBean> listStoreData;

    private ICallBack mCallBack;

    public void Awake()
    {
        mStoreInfoController = new StoreInfoController(this, this);
    }

    public void SetCallBack(ICallBack mCallBack)
    {
        this.mCallBack = mCallBack;
    }

    /// <summary>
    /// 获取市场数据
    /// </summary>
    public void GetStoreInfoForMarket()
    {
        mStoreInfoController.GetMarketStoreInfo();
    }

    /// <summary>
    /// 获取绸缎庄数据
    /// </summary>
    public void GetStoreInfoForDress()
    {
        mStoreInfoController.GetDressStoreInfo();
    }

    /// <summary>
    /// 获取百宝阁数据
    /// </summary>
    public void GetStoreInfoForGrocery()
    {
        mStoreInfoController.GetGroceryInfo();
    }

    /// <summary>
    /// 获取建筑坊数据
    /// </summary>
    public void GetStoreInfoForCarpenter()
    {
        mStoreInfoController.GetCarpenterInfo();
    }

    /// <summary>
    /// 获取建筑坊数据
    /// </summary>
    public void GetStoreInfoForPharmacy()
    {
        mStoreInfoController.GetPharmacyInfo();
    }
    /// <summary>
    /// 获取公会角色提升数据
    /// </summary>
    public void GetStoreInfoForGuildImprove()
    {
        mStoreInfoController.GetGuildImproveForCharacter();
    }

    /// <summary>
    /// 获取公会商品
    /// </summary>
    public void GetStoreInfoForGuildGoods()
    {
        mStoreInfoController.GetGuildStoreInfo();
    }

    /// <summary>
    /// 获取公会客栈升级相关数据
    /// </summary>
    public void GetStoreInfoForGuildInnLevel()
    {
        mStoreInfoController.GetGuildInnLevel();
    }

    /// <summary>
    /// 获取竞技场信息
    /// </summary>
    public void GetStoreInfoForArenaInfo()
    {
        mStoreInfoController.GetArenaInfo();
    }

    /// <summary>
    /// 获取竞技场商品
    /// </summary>
    public void GetStoreInfoForArenaGoods()
    {
        mStoreInfoController.GetArenaGoods();
    }

    #region 数据回调
    public void GetAllStoreInfoFail()
    {
    }

    public void GetAllStoreInfoSuccess(List<StoreInfoBean> listData)
    {
    }

    public void GetStoreInfoByTypeFail(StoreTypeEnum type)
    {
    }

    public void GetStoreInfoByTypeSuccess(StoreTypeEnum type, List<StoreInfoBean> listData)
    {
        listStoreData = listData;
        switch (type)
        {
            case StoreTypeEnum.Market:
                break;
        }
        if (mCallBack != null)
        {
            mCallBack.GetStoreInfoSuccess(type,listStoreData);
        }
    }
    #endregion

    public interface ICallBack
    {
       void GetStoreInfoSuccess(StoreTypeEnum type, List<StoreInfoBean> listData);
    }

}