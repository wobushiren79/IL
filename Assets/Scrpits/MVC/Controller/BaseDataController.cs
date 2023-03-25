using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BaseDataController : BaseMVCController<BaseDataModel, IBaseDataView>
{
    public Dictionary<BaseDataTypeEnum, string> mapBaseData;

    public BaseDataController(BaseMonoBehaviour content, IBaseDataView view) : base(content, view)
    {

    }

    public override void InitData()
    {

    }

    public void InitBaseData()
    {
        List<BaseDataBean> listData = GetModel().GetAllBaseData();
        if (listData == null)
            return;
        mapBaseData = new Dictionary<BaseDataTypeEnum, string>();
        foreach (BaseDataBean itemData in listData)
        {
            //BaseDataTypeEnum baseDataType = itemData.name.GetEnum<BaseDataTypeEnum>();
           // mapBaseData.Add(baseDataType, itemData.content);
        }
    }

    public void GetBaseData(BaseDataTypeEnum baseDataType, out string data)
    {
        if (mapBaseData.TryGetValue(baseDataType, out string value))
        {
            data = value;
        }
        else
        {
            data = null;
        }
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