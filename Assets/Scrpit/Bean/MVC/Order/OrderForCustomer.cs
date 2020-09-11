﻿using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class OrderForCustomer : OrderForBase
{
    //订单支付状态
    public OrderStautsForPayEnum orderStauts;
    //顾客类型
    public CustomerTypeEnum customerType;
    //需要的顾客
    public NpcAICustomerCpt customer;
    //需求的食物
    public MenuInfoBean foodData;
    //顾客所在的座位
    public BuildTableCpt table;
    //烹饪的灶台  
    public BuildStoveCpt stove;
    //做好的食物 
    public FoodForCustomerCpt foodCpt;
    //做食物的厨师
    public NpcAIWorkerCpt chef;
    //送餐的人
    public NpcAIWorkerCpt waiterForSend;
    //做出的食物等级 -1 0 1 2
    public int foodLevel;


    public OrderForCustomer(CustomerTypeEnum customerType, NpcAICustomerCpt customer)
    {
        this.customer = customer;
        this.customerType = customerType;
    }

    /// <summary>
    /// 检测订单是否有效
    /// </summary>
    /// <returns></returns>
    public new bool CheckOrder()
    {
        if (foodData == null || customer == null || customer.customerIntent == NpcAICustomerCpt.CustomerIntentEnum.Leave)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}