public class BaseDataService
{
    public void GetBaseData(BaseDataTypeEnum baseDataType, out string data)
    {
        BaseInfoBean baseInfo = BaseInfoCfg.GetItemData((long)baseDataType);
        data = baseInfo != null ? baseInfo.content : string.Empty;
    }

    public void GetBaseData(BaseDataTypeEnum baseDataType, out int data)
    {
        GetBaseData(baseDataType, out string dataStr);
        data = (!dataStr.IsNull()) ? int.Parse(dataStr) : 0;
    }

    public void GetBaseData(BaseDataTypeEnum baseDataType, out long data)
    {
        GetBaseData(baseDataType, out string dataStr);
        data = (!dataStr.IsNull()) ? long.Parse(dataStr) : 0;
    }

    public void GetBaseData(BaseDataTypeEnum baseDataType, out float data)
    {
        GetBaseData(baseDataType, out string dataStr);
        data = (!dataStr.IsNull()) ? float.Parse(dataStr) : 0;
    }
}
