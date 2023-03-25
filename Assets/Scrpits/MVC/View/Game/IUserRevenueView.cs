using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public interface IUserRevenueView
{
    /// <summary>
    /// 获取用户营收年份成功
    /// </summary>
    /// <param name="listYear"></param>
    void GetUserRevenueYearSuccess(List<int> listYear,Action<List<int>> action);

    /// <summary>
    /// 获取用户营收成功
    /// </summary>
    /// <param name="userRevenue"></param>
    void GetUserRevenueSuccess(UserRevenueBean userRevenue, Action<UserRevenueBean> action);

    /// <summary>
    /// 获取用户营收失败
    /// </summary>
    void GetUserRevenueFail();
}