using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class ItemsInfoService
{
    private readonly string mTableName;
    private readonly string mLeftDetailsTableName;
    private readonly string mLeftIntactTableName;

    public ItemsInfoService()
    {
        mTableName = "items_info";
        mLeftDetailsTableName = "items_info_details_" + GameCommonInfo.GameConfig.language;
        mLeftIntactTableName = "items_intact_info";
    }

    /// <summary>
    /// 查询所有装备数据
    /// </summary>
    /// <returns></returns>
    public List<ItemsInfoBean> QueryAllData()
    {
        return SQliteHandle.LoadTableData<ItemsInfoBean>
            (ProjectConfigInfo.DATA_BASE_INFO_NAME, mTableName,
            new string[] { mLeftDetailsTableName, mLeftIntactTableName },
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
        string[] leftTable = new string[] { mLeftDetailsTableName, mLeftIntactTableName };
        string[] mainKey = new string[] { "id", "intact_id" };
        string[] leftKey = new string[] { "items_id", "intact_id" };
        string[] colName = new string[] { mTableName + ".id" };
        string[] operations = new string[] { "IN" };
        string values = TypeConversionUtil.ArrayToStringBySplit(ids, ",");
        string[] colValue = new string[] { "(" + values + ")" };
        return SQliteHandle.LoadTableData<ItemsInfoBean>(ProjectConfigInfo.DATA_BASE_INFO_NAME, mTableName, leftTable, mainKey, leftKey, colName, operations, colValue);
    }

    public void DeleteData(ItemsInfoBean itemsInfo)
    {
        if (itemsInfo == null)
            return;
        string[] colKeys = new string[] { "id" };
        string[] operations = new string[] { "=" };
        string[] colValues = new string[] { itemsInfo.id + "", };
        SQliteHandle.DeleteTableDataAndLeft(ProjectConfigInfo.DATA_BASE_INFO_NAME, mTableName, colKeys, operations, colValues);
    }

    public void InsertData(ItemsInfoBean itemsInfo)
    {
        //插入数据
        Dictionary<string, object> mapData = ReflexUtil.GetAllNameAndValue(itemsInfo);
        List<string> listMainKeys = new List<string>();
        List<string> listMainValues = new List<string>();
        List<string> listLeftKeys = new List<string>();
        List<string> listLeftValues = new List<string>();
        foreach (var item in mapData)
        {
            string itemKey = item.Key;
            if (itemKey.Equals("name") || itemKey.Equals("content") || itemKey.Equals("items_id"))
            {
                string valueStr = Convert.ToString(item.Value);
                listLeftKeys.Add(item.Key);
                if (item.Value is string)
                {
                    listLeftValues.Add("'" + valueStr + "'");
                }
                else
                {
                    listLeftValues.Add(valueStr);
                }
            }
            else
            {
                string valueStr = Convert.ToString(item.Value);
                listMainKeys.Add(item.Key);
                if (item.Value is string)
                {
                    listMainValues.Add("'" + valueStr + "'");
                }
                else
                {
                    listMainValues.Add(valueStr);
                }
            }
        }
        SQliteHandle.InsertValues(ProjectConfigInfo.DATA_BASE_INFO_NAME, mTableName, TypeConversionUtil.ListToArray(listMainKeys), TypeConversionUtil.ListToArray(listMainValues));
        SQliteHandle.InsertValues(ProjectConfigInfo.DATA_BASE_INFO_NAME, mLeftDetailsTableName, TypeConversionUtil.ListToArray(listLeftKeys), TypeConversionUtil.ListToArray(listLeftValues));
    }

    public void UpdateData(ItemsInfoBean itemsInfo)
    {
        if (itemsInfo == null)
            return;
        //先删除该ID下的所有数据
        DeleteData(itemsInfo);
        //插入数据
        InsertData(itemsInfo);
    }

}