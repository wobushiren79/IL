using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public interface IEquipInfoView 
{
    /// <summary>
    /// 获取装备信息成功
    /// </summary>
    /// <param name="listData"></param>
    void GetEquipInfoSuccess(List<EquipInfoBean> listData);

    /// <summary>
    /// 获取装备信息失败
    /// </summary>
    void GetEquipInfoFail();
}