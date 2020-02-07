using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public interface IUserRevenueView
{
    /// <summary>
    /// 获取用户营收年份成功
    /// </summary>
    /// <param name="listYear"></param>
    void GetUserRevenueYearSuccess(List<int> listYear);

    /// <summary>
    /// 获取用户营收成功
    /// </summary>
    /// <param name="userRevenue"></param>
    void GetUserRevenueSuccess(UserRevenueBean userRevenue);

    /// <summary>
    /// 获取用户营收失败
    /// </summary>
    void GetUserRevenueFail();
}