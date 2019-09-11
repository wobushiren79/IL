using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class DateInfoModel : BaseMVCModel
{
    private DateInfoService mDateInfoService;

    public override void InitData()
    {
        mDateInfoService = new DateInfoService();
    }

    /// <summary>
    /// 通过月份获取日期数据
    /// </summary>
    /// <param name="month"></param>
    /// <returns></returns>
    public List<DateInfoBean> GetDateInfoByMonth(int month)
    {
       return mDateInfoService.QueayDataByMonth(month);
    }

}