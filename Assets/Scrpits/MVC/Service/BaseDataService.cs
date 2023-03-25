using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BaseDataService : BaseMVCService
{
    public BaseDataService() : base("base_data", "base_data")
    {

    }

    public List<BaseDataBean> QueryAllData()
    {
        List<BaseDataBean> listData = BaseQueryAllData<BaseDataBean>();
        return listData; 
    }
}