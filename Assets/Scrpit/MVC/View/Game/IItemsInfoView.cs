using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public interface IItemsInfoView 
{
    /// <summary>
    /// 获取道具信息成功
    /// </summary>
    /// <param name="listData"></param>
    void GetItemsInfoSuccess(List<ItemsInfoBean> listData);

    /// <summary>
    /// 获取道具信息失败
    /// </summary>
    void GetItemsInfoFail();
}