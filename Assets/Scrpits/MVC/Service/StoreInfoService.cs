using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class StoreInfoService : BaseMVCService
{
    public StoreInfoService() : base("store_info", "store_info_details_" + GameDataHandler.Instance.manager.GetGameConfig().language)
    {

    }

    /// <summary>
    /// 查询所有装备数据
    /// </summary>
    /// <returns></returns>
    public List<StoreInfoBean> QueryAllData()
    {
        return BaseQueryAllData<StoreInfoBean>("store_id");
    }

    /// <summary>
    /// 根据ID查询数据
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public List<StoreInfoBean> QueryDataByIds(long[] ids)
    {
        string values = TypeConversionUtil.ArrayToStringBySplit(ids, ",");
        return BaseQueryData<StoreInfoBean>("store_id", tableNameForMain + ".id", "IN", "(" + values + ")");
    }

    /// <summary>
    /// 根据类型查询数据
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public List<StoreInfoBean> QueryDataByType(int type)
    {
        return BaseQueryData<StoreInfoBean>("store_id", tableNameForMain + ".type", type + "");
    }

    /// <summary>
    /// 根据ID删除数据
    /// </summary>
    /// <param name="id"></param>
    public void DeleteDataById(long id)
    {
        bool isDelete = BaseDeleteDataById(id);
        if (isDelete)
            BaseDeleteData(tableNameForLeft, "store_id", id + "");
    }

    /// <summary>
    /// 插入数据
    /// </summary>
    /// <param name="storeInfo"></param>
    public void InsertData(StoreInfoBean storeInfo)
    {
        List<string> listLeftName = new List<string>()
        {
            "store_id",
            "name",
            "content"
        };
        BaseInsertDataWithLeft(storeInfo, listLeftName);
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="storeInfo"></param>
    public void Update(StoreInfoBean storeInfo)
    {
        DeleteDataById(storeInfo.id);
        InsertData(storeInfo);
    }
}