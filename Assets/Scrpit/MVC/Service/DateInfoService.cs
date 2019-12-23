using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class DateInfoService : BaseMVCService
{

    public string mLeftId = "date_id";

    public DateInfoService() : base("date_info", "date_info_details_" + GameCommonInfo.GameConfig.language)
    {

    }

    /// <summary>
    /// 查询所有装备数据
    /// </summary>
    /// <returns></returns>
    public List<DateInfoBean> QueryAllData()
    {
        return BaseQueryAllData<DateInfoBean>(mLeftId);
    }

    /// <summary>
    /// 根据月份查询数据
    /// </summary>
    /// <param name="month"></param>
    /// <returns></returns>
    public List<DateInfoBean> QueayDataByMonth(int month)
    {
        return BaseQueryData<DateInfoBean>(mLeftId, GetTableName() + ".month", month + "");
    }

    /// <summary>
    /// 根据具体的月份和日 查询数据
    /// </summary>
    /// <param name="month"></param>
    /// <param name="day"></param>
    /// <returns></returns>
    public List<DateInfoBean> QuearyDateByMonthAndDay(int month, int day)
    {
        return BaseQueryData<DateInfoBean>(mLeftId, GetTableName() + ".month", month + "", GetTableName() + ".day", day + "");
    }
}