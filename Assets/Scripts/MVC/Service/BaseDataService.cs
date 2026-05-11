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

    public void GetBaseData(BaseDataTypeEnum baseDataType, out string data)
    {
        BaseInfoBean baseInfo = BaseInfoCfg.GetItemData((long)baseDataType);
        data = baseInfo.content;
    }

    public void GetBaseData(BaseDataTypeEnum baseDataType, out int data)
    {
        GetBaseData(baseDataType, out string dataStr);
        if (!dataStr.IsNull())
        {
            data = int.Parse(dataStr);
        }
        else
        {
            data = 0;
        }
    }

    public void GetBaseData(BaseDataTypeEnum baseDataType, out long data)
    {
        GetBaseData(baseDataType, out string dataStr);
        if (!dataStr.IsNull())
        {
            data = long.Parse(dataStr);
        }
        else
        {
            data = 0;
        }
    }

    public void GetBaseData(BaseDataTypeEnum baseDataType, out float data)
    {
        GetBaseData(baseDataType, out string dataStr);
        if (!dataStr.IsNull())
        {
            data = float.Parse(dataStr);
        }
        else
        {
            data = 0;
        }
    }
}
