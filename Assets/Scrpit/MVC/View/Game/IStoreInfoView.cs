using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public interface IStoreInfoView 
{
    void GetAllStoreInfoSuccess(List<StoreInfoBean> listData);

    void GetAllStoreInfoFail();

    void GetStoreInfoByTypeSuccess(StoreTypeEnum type, List<StoreInfoBean> listData);

    void GetStoreInfoByTypeFail(StoreTypeEnum type);
}