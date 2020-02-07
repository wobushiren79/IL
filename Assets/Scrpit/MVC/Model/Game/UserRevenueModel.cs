using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class UserRevenueModel : BaseMVCModel
{
    public UserRevenueService userRevenueService;

    public override void InitData()
    {
        userRevenueService = new UserRevenueService();
    }

    /// <summary>
    /// 设置营收数据
    /// </summary>
    /// <param name="userRevenue"></param>
    public void SetUserRevenue(UserRevenueBean userRevenue)
    {
        userRevenueService.UpdateDataByYear(userRevenue);
    }

    /// <summary>
    /// 根据年份获取营收数据
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="year"></param>
    /// <returns></returns>
    public UserRevenueBean GetUserRevenueByYear(string userId,int year)
    {
       return userRevenueService.QueryDataByYear(userId,year);
    }

   /// <summary>
   /// 获取营收年份
   /// </summary>
   /// <returns></returns>
    public List<int> GetUserRevenueYear(string userId)
    {
        return userRevenueService.QueryYear(userId);
    }

}