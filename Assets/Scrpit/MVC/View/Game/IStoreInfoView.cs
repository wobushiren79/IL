using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public interface IStoreInfoView 
{
    void GetAllStoreInfoSuccess(List<StoreInfoBean> listData, Action<List<StoreInfoBean>> action);

    void GetAllStoreInfoFail();

    void GetStoreInfoByTypeSuccess(StoreTypeEnum type, List<StoreInfoBean> listData,Action<List<StoreInfoBean>> action);

    void GetStoreInfoByTypeFail(StoreTypeEnum type);
}