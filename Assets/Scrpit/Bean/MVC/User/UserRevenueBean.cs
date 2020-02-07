using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
public class UserRevenueBean
{
    public string userId;
    public int year;

    public List<UserRevenueMonthBean> listMonthData;
}

[Serializable]
public class UserRevenueMonthBean
{
    public int month;
    public List<UserRevenueDayBean> listDayData;

    /// <summary>
    /// 转换为图表信息
    /// </summary>
    /// <returns></returns>
    public List<CartogramDataBean> GetListCartogramData()
    {
        List<CartogramDataBean> listCartogramData = new List<CartogramDataBean>();
        if (listDayData == null)
            return listCartogramData;
        foreach (UserRevenueDayBean itemDay in listDayData)
        {
            CartogramDataBean cartogramData = new CartogramDataBean();
            cartogramData.key = itemDay.day;
            cartogramData.value_1 = itemDay.money_l;
            cartogramData.value_2 = itemDay.money_m;
            cartogramData.value_3 = itemDay.money_s;
            cartogramData.value_4 = itemDay.status;
            listCartogramData.Add(cartogramData);
        }
        return listCartogramData;
    }
}

[Serializable]
public class UserRevenueDayBean
{
    public int day;
    public int status;//状态 0休息 1营业
    public long money_l;
    public long money_m;
    public long money_s;
}