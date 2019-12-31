using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class StoreInfoManager : BaseMonoBehaviour, IStoreInfoView
{  
    public StoreInfoController mStoreInfoController;
    //商店数据
    public List<StoreInfoBean> listStoreData;

    private ICallBack mCallBack;

    private void Awake()
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