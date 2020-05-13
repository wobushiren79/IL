using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BaseDataModel : BaseMVCModel
{
    protected BaseDataService baseDataService;

    public override void InitData()
    {
        baseDataService = new BaseDataService();
    }

    public List<BaseDataBean> GetAllBaseData()
    {
        return baseDataService.QueryAllData();
    }


}