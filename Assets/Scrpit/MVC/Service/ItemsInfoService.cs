using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class ItemsInfoService : BaseMVCService
{
    private readonly string mLeftIntactTableName;

    public ItemsInfoService() : base("items_info", "items_info_details_" + GameCommonInfo.GameConfig.language)
    {
        mLeftIntactTableName = "items_intact_info";
    }

    /// <summary>
    /// 查询所有装备数据
    /// </summary>
    /// <returns></returns>
    public List<ItemsInfoBean> QueryAllData()
    {
        return SQliteHandle.LoadTableData<ItemsInfoBean>
            (ProjectConfigInfo.DATA_BASE_INFO_NAME, tableNameForMain,
            new string[] { tableNameForLeft, mLeftIntactTableName },
            new string[] { "id", "intact_id" },
            new string[] { "items_id", "intact_id" });
    }

    /// <summary>
    /// 根据ID查询数据
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public List<ItemsInfoBean> QueryDataByIds(long[] ids)
    {
        string[] leftTable = new string[] { tableNameForLeft, mLeftIntactTableName };
        string[] mainKey = new string[] { "id", "intact_id" };
        string[] leftKey = new string[] { "items_id", "intact_id" };
        string[] colName = new string[] { tableNameForMain + ".id" };
        string[] operations = new string[] { "IN" };
        string values = TypeConversionUtil.ArrayToStringBySplit(ids, ",");
        string[] colValue = new string[] { "(" + values + ")" };
        return SQliteHandle.LoadTableData<ItemsInfoBean>(ProjectConfigInfo.DATA_BASE_INFO_NAME, tableNameForMain, leftTable, mainKey, leftKey, colName, operations, colValue);
    }

    /// <summary>
    /// 删除数据
    /// </summary>
    /// <param name="itemsInfo"></param>
    public bool DeleteData(ItemsInfoBean itemsInfo)
    {
        if (itemsInfo == null)
            return false;
        return BaseDeleteDataWithLeft("id", "items_id", itemsInfo.id+"");
    }

    /// <summary>
    /// 插入数据
    /// </summary>
    /// <param name="itemsInfo"></param>
    public void InsertData(ItemsInfoBean itemsInfo)
    {
        List<string> listLeftName = new List<string>
        {
            "items_id",
            "content",
            "name"
        };
        BaseInsertDataWithLeft(itemsInfo, listLeftName);
    }

    public void UpdateData(ItemsInfoBean itemsInfo)
    {
        if (itemsInfo == null)
            return;
        //先删除该ID下的所有数据
        if (DeleteData(itemsInfo))
        {
            //插入数据
            InsertData(itemsInfo);
        }
        
    }

}