using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class BaseMVCService<T>
{
    public readonly string mTableName;//主表名称
    public readonly string mLeftDetailsTableName;//副标名称

    public BaseMVCService(string tableName) : this(tableName, null)
    {

    }

    public BaseMVCService(string tableName, string leftDetailsTableName)
    {
        mTableName = tableName;
        mLeftDetailsTableName = leftDetailsTableName;
    }

    public string GetTableName()
    {
        return mTableName;
    }

    public string GetLeftTableName()
    {
        return mLeftDetailsTableName;
    }

    /// <summary>
    /// 查询所有数据
    /// </summary>
    /// <returns></returns>
    public List<T> BaseQueryAllData()
    {
        if (mTableName == null)
        {
            LogUtil.LogError("查询数据失败，没有表名");
            return null;
        }
        return SQliteHandle.LoadTableData<T>(ProjectConfigInfo.DATA_BASE_INFO_NAME, mTableName);
    }

    /// <summary>
    /// 链表查询所有数据
    /// </summary>
    /// <param name="leftId"></param>
    /// <returns></returns>
    public List<T> BaseQueryAllData(string leftId)
    {
        if (mLeftDetailsTableName == null)
        {
            LogUtil.LogError("查询数据失败，没有关联的副表");
            return null;
        }
        return SQliteHandle.LoadTableData<T>
            (ProjectConfigInfo.DATA_BASE_INFO_NAME, mTableName,
            new string[] { mLeftDetailsTableName },
            new string[] { "id" },
            new string[] { leftId });
    }

    /// <summary>
    /// 查询数据 单个条件
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public List<T> BaseQueryData(string key, string value)
    {
        string[] leftTable = new string[] { mLeftDetailsTableName };
        string[] mainKey = new string[] { "id" };
        string[] colName = new string[] { key };
        string[] operations = new string[] { "=" };
        string[] colValue = new string[] { value };
        return SQliteHandle.LoadTableDataByCol<T>(ProjectConfigInfo.DATA_BASE_INFO_NAME, mTableName, colName, operations, colValue);
    }

    /// <summary>
    /// 链表查询数据 单个数据
    /// </summary>
    /// <param name="leftId"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public List<T> BaseQueryData(string leftId, string key, string value)
    {
        if (mTableName == null)
        {
            LogUtil.LogError("查询数据失败，没有表名");
            return null;
        }
        if (mLeftDetailsTableName == null)
        {
            LogUtil.LogError("查询数据失败，没有关联的副表");
            return null;
        }
        string[] leftTable = new string[] { mLeftDetailsTableName };
        string[] mainKey = new string[] { "id" };
        string[] leftKey = new string[] { leftId };
        string[] colName = new string[] { key };
        string[] operations = new string[] { "=" };
        string[] colValue = new string[] { value };
        return SQliteHandle.LoadTableData<T>(ProjectConfigInfo.DATA_BASE_INFO_NAME, mTableName, leftTable, mainKey, leftKey, colName, operations, colValue);
    }

    public List<T> BaseQueryData(string leftId, string key1, string value1, string key2, string value2)
    {
        if (mTableName == null)
        {
            LogUtil.LogError("查询数据失败，没有表名");
            return null;
        }
        if (mLeftDetailsTableName == null)
        {
            LogUtil.LogError("查询数据失败，没有关联的副表");
            return null;
        }
        string[] leftTable = new string[] { mLeftDetailsTableName };
        string[] mainKey = new string[] { "id" };
        string[] leftKey = new string[] { leftId };
        string[] colName = new string[] { key1, key2 };
        string[] operations = new string[] { "=", "=" };
        string[] colValue = new string[] { value1, value2 };
        return SQliteHandle.LoadTableData<T>(ProjectConfigInfo.DATA_BASE_INFO_NAME, mTableName, leftTable, mainKey, leftKey, colName, operations, colValue);
    }

    /// <summary>
    /// 通过ID删除数据
    /// </summary>
    /// <param name="itemsInfo"></param>
    public void BaseDeleteDataById(long id)
    {
        if (id == 0)
            return;
        string[] colKeys = new string[] { "id" };
        string[] operations = new string[] { "=" };
        string[] colValues = new string[] { id + "", };
        SQliteHandle.DeleteTableDataAndLeft(ProjectConfigInfo.DATA_BASE_INFO_NAME, mTableName, colKeys, operations, colValues);
    }

    /// <summary>
    /// 插入数据
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="itemData"></param>
    public void BaseInsertData(string tableName, T itemData)
    {
        //插入数据
        Dictionary<string, object> mapData = ReflexUtil.GetAllNameAndValue(itemData);
        List<string> listKeys = new List<string>();
        List<string> listValues = new List<string>();
        foreach (var item in mapData)
        {
            string itemKey = item.Key;
            string valueStr = Convert.ToString(item.Value);
            listKeys.Add(item.Key);
            if (item.Value is string)
            {
                if (CheckUtil.StringIsNull(valueStr))
                    listValues.Add("null");
                else
                    listValues.Add("'" + valueStr + "'");
            }
            else
            {
                listValues.Add(valueStr);
            }
        }
        SQliteHandle.InsertValues(ProjectConfigInfo.DATA_BASE_INFO_NAME, tableName, TypeConversionUtil.ListToArray(listKeys), TypeConversionUtil.ListToArray(listValues));
    }

    /// <summary>
    /// 链表插入数据
    /// </summary>
    /// <param name="itemData"></param>
    /// <param name="listLeftName"></param>
    public void BaseInsertDataWithLeft(T itemData, List<string> listLeftName)
    {
        //插入数据
        Dictionary<string, object> mapData = ReflexUtil.GetAllNameAndValue(itemData);
        List<string> listMainKeys = new List<string>();
        List<string> listMainValues = new List<string>();
        List<string> listLeftKeys = new List<string>();
        List<string> listLeftValues = new List<string>();
        foreach (var item in mapData)
        {
            string itemKey = item.Key;
            if (listLeftName.Contains(itemKey))
            {
                string valueStr = Convert.ToString(item.Value);
                listLeftKeys.Add(item.Key);
                if (item.Value is string)
                {
                    if (CheckUtil.StringIsNull(valueStr))
                        listLeftValues.Add("null");
                    else
                        listLeftValues.Add("'" + valueStr + "'");
                }
                else if (item.Value == null)
                {
                    listLeftValues.Add("null");
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
                    if (CheckUtil.StringIsNull(valueStr))
                        listMainValues.Add("null");
                    else
                        listMainValues.Add("'" + valueStr + "'");
                }
                else if (item.Value==null)
                {
                    listMainValues.Add("null");
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
}