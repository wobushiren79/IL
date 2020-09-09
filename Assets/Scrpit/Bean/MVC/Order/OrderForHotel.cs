using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class OrderForHotel 
{
    public OrderHotelStatusEnum orderHotelStatus;
    //顾客
    public NpcAICustomerForHotelCpt customer;
    //分配的床位
    public BuildBedCpt bed;

    //上下楼梯的位置
    public Vector3 layerFirstStairsPosition;
    public Vector3 layerSecondStairsPosition;

    public OrderForHotel(NpcAICustomerForHotelCpt customer, BuildBedCpt bed)
    {
        this.customer = customer;
        this.bed = bed;
        orderHotelStatus= OrderHotelStatusEnum.Start;
    }

    public void SetOrderStatus(OrderHotelStatusEnum orderHotelStatus)
    {
        this.orderHotelStatus = orderHotelStatus;
    }
    public OrderHotelStatusEnum GetOrderStatus()
    {
        return orderHotelStatus;
    }
}