using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BaseMVCService<T>
{
    private readonly string mTableName;//主表名称
    private readonly string mLeftDetailsTableName;//副标名称

    public BaseMVCService(string tableName): this(tableName,null)
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
    public List<T> BaseQueryData(string leftId, string key,string value)
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

    public List<T> BaseQueryData(string leftId, string key1, string value1,string key2,string value2)
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
        string[] operations = new string[] { "=","=" };
        string[] colValue = new string[] { value1, value2 };
        return SQliteHandle.LoadTableData<T>(ProjectConfigInfo.DATA_BASE_INFO_NAME, mTableName, leftTable, mainKey, leftKey, colName, operations, colValue);
    }
}