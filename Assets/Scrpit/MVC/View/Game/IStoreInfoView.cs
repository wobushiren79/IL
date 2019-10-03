using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public interface IStoreInfoView 
{
    void GetAllStoreInfoSuccess(List<StoreInfoBean> listData);

    void GetAllStoreInfoFail();

    void GetStoreInfoByTypeSuccess(int type, List<StoreInfoBean> listData);

    void GetStoreInfoByTypeFail(int type);
}