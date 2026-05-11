using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BuildCounterCpt : BaseBuildItemCpt
{
    public enum CounterStatusEnum
    {
        Idle = 0,
        Ready = 1,
        Accounting = 2
    }

    public GameObject objPayPosition;
    public GameObject objPaccountingPosition;

    //排队算账的订单
    public List<OrderForBase> payQueue = new List<OrderForBase>();

    public CounterStatusEnum counterStatus = CounterStatusEnum.Idle;

    /// <summary>
    /// 获取付款位置
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPayPosition()
    {
        return GameUtil.GetTransformInsidePosition2D(objPayPosition.transform);
    }

    /// <summary>
    /// 获取算账位置
    /// </summary>
    /// <returns></returns>
    public Vector3 GetAccountingPosition()
    {
        return objPaccountingPosition.transform.position;
    }

    public void SetCounterStatus(CounterStatusEnum counterStatus)
    {
        this.counterStatus = counterStatus;
    }

    public void ClearCounter()
    {
        payQueue.Clear();
        SetCounterStatus(CounterStatusEnum.Idle);
    }


}