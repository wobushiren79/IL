using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class DateInfoController : BaseMVCController<DateInfoModel, IDateInfoView>
{

    public DateInfoController(BaseMonoBehaviour content, IDateInfoView view) : base(content, view)
    {
    }

    public override void InitData()
    {
    }

    /// <summary>
    /// 通过月份获取数据
    /// </summary>
    /// <param name="month"></param>
    public void GetDateInfoByMonth(int month)
    {
        List<DateInfoBean> listData= GetModel().GetDateInfoByMonth(month);
        if (listData == null)
        {
            GetView().GetDateInfoFail();
        }
        else
        {
            GetView().GetDateInfoSuccess(listData);
        }
    }
}