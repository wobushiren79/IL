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
    public List<InnRecordBean> listDayData;

    /// <summary>
    /// 转换为图表信息-进账
    /// </summary>
    /// <returns></returns>
    public List<CartogramDataBean> GetListCartogramDataForIncome()
    {
        List<CartogramDataBean> listCartogramData = new List<CartogramDataBean>();
        if (listDayData == null)
            return listCartogramData;
        foreach (InnRecordBean itemDay in listDayData)
        {
            CartogramDataBean cartogramData = new CartogramDataBean();
            itemDay.GetTotalIncome(out long incomeL, out long incomeM, out long incomeS);
            cartogramData.key = itemDay.day;
            cartogramData.value_3 = incomeL;
            cartogramData.value_2 = incomeM;
            cartogramData.value_1 = incomeS;
            cartogramData.value_4 = itemDay.status;
            listCartogramData.Add(cartogramData);
        }
        return listCartogramData;
    }
}