using UnityEngine;
using System.Collections.Generic;

public abstract class BaseDataStorageImpl<T>
{
    //数据保存路径
    private string mDataStoragePath;

    /// <summary>
    /// 初始化参数
    /// </summary>
    public BaseDataStorageImpl()
    {
        mDataStoragePath = Application.persistentDataPath;
    }

    /// <summary>
    /// 获取数据保存路径
    /// </summary>
    /// <returns></returns>
    public string GetDataStoragePath()
    {
        return mDataStoragePath;
    }

    /// <summary>
    /// 设置数据保存路径
    /// </summary>
    /// <param name="dataStoragePath"></param>
    public void SetDataStoragePath(string dataStoragePath)
    {
        this.mDataStoragePath = dataStoragePath;
    }

    /// <summary>
    /// 基础-保存单个数据
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="dataBean"></param>
    public void BaseStartSaveData(string fileName, T dataBean)
    {
        if (CheckUtil.StringIsNull(fileName))
        {
            LogUtil.Log("保存文件失败-没有文件名称");
            return;
        }
        if (dataBean == null)
        {
            LogUtil.Log("保存文件失败-没有数据");
            return;
        }
        string strData = JsonUtil.ToJson(dataBean);
        FileUtil.CreateTextFile(mDataStoragePath, fileName, strData);
    }

   /// <summary>
   /// 基础-保存数据列表
   /// </summary>
   /// <param name="fileName"></param>
   /// <param name="dataBeanList"></param>
    public void BaseStartSaveDataForList(string fileName, List<T> dataBeanList)
    {
        if (fileName == null)
        {
            LogUtil.Log("保存文件失败-没有文件名称");
            return;
        }
        if (dataBeanList == null)
        {
            LogUtil.Log("保存文件失败-没有数据");
            return;
        }
        DataStorageListBean<T> handBean = new DataStorageListBean<T>();
        handBean.listData = dataBeanList;
        string strData = JsonUtil.ToJson(handBean);
        FileUtil.CreateTextFile(mDataStoragePath, fileName, strData);
    }

    /// <summary>
    /// 基础-读取单个数据
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public T BaseStartLoadData(string fileName)
    {
        if (fileName == null)
        {
            LogUtil.Log("读取文件失败-没有文件名称");
            return default(T);
        }
        string strData = FileUtil.LoadTextFile(mDataStoragePath + "/" + fileName);
        if (strData == null)
            return default(T);
        T data = JsonUtil.FromJson<T>(strData);
        return data;
    }

    /// <summary>
    /// 基础-读取列表数据
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public List<T> BaseStartLoadDataForList(string fileName)
    {
        if (fileName == null)
        {
            LogUtil.Log("读取文件失败-没有文件名称");
            return null;
        }
        string strData = FileUtil.LoadTextFile(mDataStoragePath + "/" + fileName);
        if (strData == null)
            return null;
        DataStorageListBean<T> handBean=  JsonUtil.FromJson<DataStorageListBean<T>>(strData);
        if (handBean == null)
            return null;
        return handBean.listData;
    }
}