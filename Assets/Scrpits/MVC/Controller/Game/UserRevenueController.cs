using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class UserRevenueController : BaseMVCController<UserRevenueModel, IUserRevenueView>
{
    public UserRevenueController(BaseMonoBehaviour content, IUserRevenueView view) : base(content, view)
    {
    }

    public override void InitData()
    {

    }

    public void SetUserRevenue(string userId, InnRecordBean innRecordData)
    {
        UserRevenueBean userRevenueData = GetModel().GetUserRevenueByYear(userId, innRecordData.year);
        if (userRevenueData == null)
        {
            userRevenueData = new UserRevenueBean();
            userRevenueData.year = innRecordData.year;
            userRevenueData.userId = userId;
        }
        if (userRevenueData.listMonthData == null)
        {
            userRevenueData.listMonthData = new List<UserRevenueMonthBean>();
        }
        bool hasMonthData = false;
        foreach (UserRevenueMonthBean itemMonth in userRevenueData.listMonthData)
        {
            if (itemMonth.month == innRecordData.month)
            {
                if (itemMonth.listDayData == null)
                    itemMonth.listDayData = new List<InnRecordBean>();
                itemMonth.listDayData.Add(innRecordData);
                hasMonthData = true;
            }
        }
        if (!hasMonthData)
        {
            UserRevenueMonthBean itemMonth = new UserRevenueMonthBean();
            itemMonth.month = innRecordData.month;
            itemMonth.listDayData = new List<InnRecordBean>();
            itemMonth.listDayData.Add(innRecordData);
            userRevenueData.listMonthData.Add(itemMonth);
        }
        SetUserRevenue(userRevenueData);
    }

    public void SetUserRevenue(UserRevenueBean userRevenueData)
    {
        GetModel().SetUserRevenue(userRevenueData);
    }

    public void GetUserRevenueByYear(string userId, int year, Action<UserRevenueBean> action)
    {
        UserRevenueBean userRevenueData = GetModel().GetUserRevenueByYear(userId, year);
        GetView().GetUserRevenueSuccess(userRevenueData, action);
    }

    public void GetUserRevenueYear(string userId, Action<List<int>> action)
    {
        List<int> listYear = GetModel().GetUserRevenueYear(userId);
        GetView().GetUserRevenueYearSuccess(listYear, action);
    }

}