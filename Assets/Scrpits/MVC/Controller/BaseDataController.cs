using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BaseDataController : BaseMVCController<BaseDataModel, IBaseDataView>
{
    public BaseDataController(BaseMonoBehaviour content, IBaseDataView view) : base(content, view)
    {

    }
    public override void InitData()
    {

    }

    public void GetBaseData(BaseDataTypeEnum baseDataType, out string data)
    {
        BaseInfoBean baseInfo = BaseInfoCfg.GetItemData((long)baseDataType);
        data = baseInfo.content;
    }

    public void GetBaseData(BaseDataTypeEnum baseDataType,out int data)
    {
        GetBaseData( baseDataType, out string dataStr);
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