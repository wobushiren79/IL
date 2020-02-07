using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class UserRevenueController : BaseMVCController<UserRevenueModel, IUserRevenueView>
{
    public UserRevenueController(BaseMonoBehaviour content, IUserRevenueView view) : base(content, view)
    {
    }

    public override void InitData()
    {

    }

    public void SetUserRevenue(UserRevenueBean userRevenueData)
    {
        GetModel().SetUserRevenue(userRevenueData);
    }

    public void GetUserRevenueByYear(string userId, int year)
    {
        UserRevenueBean userRevenueData = GetModel().GetUserRevenueByYear(userId, year);
        GetView().GetUserRevenueSuccess(userRevenueData);
    }

    public void GetUserRevenueYear(string userId)
    {
        List<int> listYear = GetModel().GetUserRevenueYear(userId);
        GetView().GetUserRevenueYearSuccess(listYear);
    }

}