using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class OrderForHotel : OrderForBase
{
    public OrderHotelStatusEnum orderHotelStatus;
    //顾客
    public NpcAICustomerForHotelCpt customer;
    //分配的床位
    public BuildBedCpt bed;
    //睡觉时间
    public int sleepTime;

    //上下楼梯的位置
    public Vector3 layerFirstStairsPosition;
    public Vector3 layerSecondStairsPosition;

    //上下楼梯的位置 -清理专用
    public Vector3 layerFirstStairsPositionForClean;
    public Vector3 layerSecondStairsPositionForClean;
    public OrderForHotel(NpcAICustomerForHotelCpt customer, BuildBedCpt bed)
    {
        this.customer = customer;
        this.bed = bed;
        orderHotelStatus= OrderHotelStatusEnum.Start;
        sleepTime = UnityEngine.Random.Range(1, 3);
    }

    public void SetOrderStatus(OrderHotelStatusEnum orderHotelStatus)
    {
        this.orderHotelStatus = orderHotelStatus;
    }
    public OrderHotelStatusEnum GetOrderStatus()
    {
        return orderHotelStatus;
    }
    /// <summary>
    /// 检测订单是否有效
    /// </summary>
    /// <returns></returns>
    public override bool CheckOrder()
    {
        if (customer == null || orderHotelStatus== OrderHotelStatusEnum.End || customer.customerHotelIntent == NpcAICustomerForHotelCpt.CustomerHotelIntentEnum.Leave)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

}