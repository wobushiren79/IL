using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public interface IMenuInfoView 
{
    /// <summary>
    /// 获取所有菜单成功
    /// </summary>
    /// <param name="listData"></param>
    void GetAllMenuInfoSuccess(List<MenuInfoBean> listData);

    /// <summary>
    /// 获取所有菜单失败
    /// </summary>
    void GetAllMenuInfFail();
}