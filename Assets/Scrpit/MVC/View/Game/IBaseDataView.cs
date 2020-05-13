using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public interface IBaseDataView
{
    void GetBaseDataSuccess(List<BaseDataBean> listData);

    void GetBaseDataFail();
}