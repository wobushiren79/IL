using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class StoreInfoService : BaseMVCService
{

    public StoreInfoService() : base("store_info", "store_info_details_" + GameCommonInfo.GameConfig.language)
    {

    }

    /// <summary>
    /// 查询所有装备数据
    /// </summary>
    /// <returns></returns>
    public List<StoreInfoBean> QueryAllData()
    {
        return BaseQueryAllData<StoreInfoBean>("goods_id");
    }

    /// <summary>
    /// 根据ID查询数据
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public List<StoreInfoBean> QueryDataByIds(long[] ids)
    {
        string values = TypeConversionUtil.ArrayToStringBySplit(ids, ",");
        return BaseQueryData<StoreInfoBean>("goods_id", tableNameForMain + ".id", "IN", "(" + values + ")");
    }

    /// <summary>
    /// 根据类型查询数据
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public List<StoreInfoBean> QueryDataByType(int type)
    {
        return BaseQueryData<StoreInfoBean>("goods_id", tableNameForMain + ".type", type + "");
    }
}