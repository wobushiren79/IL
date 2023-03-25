using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class RevenueCartogramBarView : CartogramBarView
{
    public List<InnRecordBean> listInnRecord;

    public void SetData(List<CartogramDataBean> listCartogramData, List<InnRecordBean> listInnRecord)
    {
        this.listInnRecord = listInnRecord;
        base.SetData(listCartogramData);
    }

    /// <summary>
    /// 创建单个柱状
    /// </summary>
    /// <param name="position"></param>
    /// <param name="itemWidth"></param>
    /// <param name="itemMaxValue"></param>
    /// <param name="itemMaxHeight"></param>
    /// <returns></returns>
    public override CartogramBarForItem CreateItemBar(int position, float itemWidth, float itemMaxValue, float itemMaxHeight)
    {
        CartogramBarForItem barItemCpt = base.CreateItemBar(position, itemWidth, itemMaxValue, itemMaxHeight);
        ((RevenueCartogramBarForItem)barItemCpt).SetRecordData(listInnRecord[position]);
        return barItemCpt;
    }

}