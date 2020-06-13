using UnityEngine;
using UnityEditor;

public class GambleTrickyCupBean : GambleBaseBean
{
    //杯子数
    public int cupNumber = 2;
    //交换次数
    public int changeNumber = 10;
    //交换间隔时间
    public float changeIntervalTime = 0.2f;

    public GambleTrickyCupBean() : base(GambleTypeEnum.TrickyCup)
    {

    }
}