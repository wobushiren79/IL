using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public interface IDateInfoView
{
    /// <summary>
    /// 获取日期成功
    /// </summary>
    /// <param name="dateInfo"></param>
    void GetDateInfoSuccess(List<DateInfoBean> dateInfo);

    /// <summary>
    /// 获取日期失败
    /// </summary>
    void GetDateInfoFail();
}