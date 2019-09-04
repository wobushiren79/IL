using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class InnHandler : BaseMonoBehaviour
{
    public enum InnStatusEnum
    {
        Open,
        Close,
    }

    //客栈状态
    public InnStatusEnum innStatus = InnStatusEnum.Close;

    //数据管理
    public GameDataManager gameDataManager;
    public InnFoodManager innFoodManager;
    //NPC创建
    public NpcWorkerBuilder workerBuilder;

    //客栈桌子处理
    public InnTableHandler innTableHandler;
    //烹饪处理
    public InnCookHandler innCookHandler;
    //服务处理
    public InnWaiterHandler innWaiterHandler;
    // 支付处理
    public InnPayHandler innPayHandler;
    // 入口处理
    public InnEntranceHandler innEntranceHandler;

    //排队的人
    public List<NpcAICustomerCpt> cusomerQueue = new List<NpcAICustomerCpt>();
    //排队等待烹饪的食物
    public List<OrderForCustomer> foodQueue = new List<OrderForCustomer>();
    //排队送餐的食物
    public List<OrderForCustomer> sendQueue = new List<OrderForCustomer>();
    //排队清理的食物
    public List<OrderForCustomer> clearQueue = new List<OrderForCustomer>();

    //订单列表
    public List<OrderForCustomer> orderList = new List<OrderForCustomer>();
    //当天记录流水
    public InnRecordBean innRecord = new InnRecordBean();

    /// <summary>
    /// 初始化客栈
    /// </summary>
    public void InitInn()
    {
        innEntranceHandler.InitDoorList();
        innTableHandler.InitTableList();
        innCookHandler.InitStoveList();
        innPayHandler.InitCounterList();
        InitWorker();
    }

    /// <summary>
    /// 初始化员工
    /// </summary>
    public void InitWorker()
    {
        innPayHandler.InitAccountingCpt();
        innCookHandler.InitChefCpt();
        innWaiterHandler.InitWaiterCpt();
    }

    private void FixedUpdate()
    {
        if (innStatus == InnStatusEnum.Open)
        {
            //排队等待处理
            if (!CheckUtil.ListIsNull(cusomerQueue))
            {
                BuildTableCpt tableCpt = innTableHandler.GetIdleTable();
                if (tableCpt != null)
                {
                    //排队成功
                    NpcAICustomerCpt customer = cusomerQueue[0];
                    //添加一个订单
                    OrderForCustomer orderForCustomer = CreateOrder(customer, tableCpt);
                    orderList.Add(orderForCustomer);
                    //设置客户前往座位
                    customer.SetIntent(NpcAICustomerCpt.CustomerIntentEnum.GotoSeat, orderForCustomer);
                    //移除排队列表
                    cusomerQueue.RemoveAt(0);
                }
            }
            //排队支付处理
            if (!CheckUtil.ListIsNull(innPayHandler.listCounterCpt))
            {
                for (int i = 0; i < innPayHandler.listCounterCpt.Count; i++)
                {
                    BuildCounterCpt counterCpt = innPayHandler.listCounterCpt[i];
                    if (!CheckUtil.ListIsNull(counterCpt.payQueue))
                    {
                        bool isSuccess = innPayHandler.SetPay(counterCpt.payQueue[0]);
                        if (isSuccess)
                        {
                            counterCpt.payQueue.RemoveAt(0);
                        }
                    }
                }
            }
            //排队送菜处理
            if (!CheckUtil.ListIsNull(sendQueue))
            {
                bool isSuccess = innWaiterHandler.SetSendFood(sendQueue[0]);
                if (isSuccess)
                {
                    sendQueue.RemoveAt(0);
                }
            }
            //排队做菜处理
            if (!CheckUtil.ListIsNull(foodQueue))
            {
                bool isSuccess = innCookHandler.SetChefForCook(foodQueue[0]);
                if (isSuccess)
                {
                    foodQueue.RemoveAt(0);
                }
            }
            //排队清理处理
            if (!CheckUtil.ListIsNull(clearQueue))
            {
                bool isSuccess = innWaiterHandler.SetClearFood(clearQueue[0]);
                if (isSuccess)
                {
                    clearQueue.RemoveAt(0);
                }
            }
        }
    }

    public void CloseInn()
    {
        innStatus = InnStatusEnum.Close;
        //驱除所有顾客
        for (int i = 0; i < orderList.Count; i++)
        {
            OrderForCustomer orderCusomer = orderList[i];
            if (orderCusomer.customer != null && orderCusomer.customer.gameObject != null)
                Destroy(orderCusomer.customer.gameObject);
            if (orderCusomer.foodCpt != null && orderCusomer.foodCpt.gameObject != null)
                Destroy(orderCusomer.foodCpt.gameObject);
        }
        for (int i = 0; i < innTableHandler.listTableCpt.Count; i++)
        {
            BuildTableCpt buildTableCpt = innTableHandler.listTableCpt[i];
            buildTableCpt.CleanTable();
        };
        for (int i = 0; i < innPayHandler.listCounterCpt.Count; i++)
        {
            BuildCounterCpt buildCounterCpt = innPayHandler.listCounterCpt[i];
            buildCounterCpt.ClearCounter();
        };
        for (int i = 0; i < innCookHandler.listStoveCpt.Count; i++)
        {
            BuildStoveCpt buildStoveCpt = innCookHandler.listStoveCpt[i];
            buildStoveCpt.ClearStove();
        };

        cusomerQueue.Clear();
        foodQueue.Clear();
        sendQueue.Clear();
        clearQueue.Clear();
        orderList.Clear();
        workerBuilder.ClearAllWork();
    }

    public void OpenInn()
    {
        workerBuilder.BuildAllWorker();
        InitInn();
        innStatus = InnStatusEnum.Open;
    }

    /// <summary>
    /// 创建一个订单
    /// </summary>
    /// <param name="npc"></param>
    /// <returns></returns>
    public OrderForCustomer CreateOrder(NpcAICustomerCpt npc, BuildTableCpt table)
    {
        OrderForCustomer order = new OrderForCustomer();
        order.customer = npc;
        order.table = table;
        return order;
    }

    /// <summary>
    /// 强制结束一个订单 不高兴时结束
    /// </summary>
    /// <param name="orderForCustomer"></param>
    public void EndOrderForForce(OrderForCustomer orderForCustomer)
    {
        //如果食物已经做出来了
        if (orderForCustomer.foodCpt != null)
        {
            //支付一半的钱
            PayMoney(orderForCustomer, 0.5f);
        }
        //如果桌子还属于这个顾客
        switch (orderForCustomer.customer.customerIntent)
        {
            case NpcAICustomerCpt.CustomerIntentEnum.GotoSeat:
            case NpcAICustomerCpt.CustomerIntentEnum.WaitFood:
            case NpcAICustomerCpt.CustomerIntentEnum.Eatting:
                orderForCustomer.table.CleanTable();
                break;
        }
    }

    /// <summary>
    /// 点餐
    /// </summary>
    /// <returns></returns>
    public MenuInfoBean OrderForFood(OrderForCustomer orderForCustomer)
    {
        //获取正在出售的菜品
        List<MenuOwnBean> listOwnMenu = gameDataManager.gameData.GetSellMenuList();
        if (listOwnMenu.Count == 0)
            return null;
        //随机获取一个菜品
        MenuOwnBean menuOwnItem = RandomUtil.GetRandomDataByList(listOwnMenu);
        if (menuOwnItem == null)
            return null;
        //食物数据库里有这个数据
        if (innFoodManager.listMenuData.TryGetValue(menuOwnItem.menuId, out MenuInfoBean menuInfo))
        {
            orderForCustomer.foodData = menuInfo;
            foodQueue.Add(orderForCustomer);
            return menuInfo;
        }
        return null;
    }

    /// <summary>
    /// 获取柜台
    /// </summary>
    /// <returns></returns>
    public BuildCounterCpt GetCounter()
    {
        BuildCounterCpt counterCpt = RandomUtil.GetRandomDataByList(innPayHandler.listCounterCpt);
        return counterCpt;
    }


    /// <summary>
    ///  获取随机一个入口附近的坐标
    /// </summary>
    /// <returns></returns>
    public Vector3 GetRandomEntrancePosition()
    {
        return innEntranceHandler.GetRandomEntrancePosition();
    }

    /// <summary>
    /// 付钱
    /// </summary>
    /// <param name="food"></param>
    public void PayMoney(OrderForCustomer order, float multiple)
    {
        //账本记录
        if (innRecord.sellNumber.ContainsKey(order.foodData.id))
            innRecord.sellNumber[order.foodData.id] += 1;
        else
            innRecord.sellNumber.Add(order.foodData.id, 1);
        innRecord.incomeS += order.foodData.price_s;
        innRecord.incomeM += order.foodData.price_m;
        innRecord.incomeL += order.foodData.price_l;
        //记录+1
        gameDataManager.gameData.ChangeMenuSellNumber(1, order.foodData.id);
        //金钱增加
        gameDataManager.gameData.moneyS += (long)(order.foodData.price_s * multiple);
        gameDataManager.gameData.moneyM += (long)(order.foodData.price_m * multiple);
        gameDataManager.gameData.moneyL += (long)(order.foodData.price_l * multiple);
        innPayHandler.ShowPayEffects(
            order.customer.transform.position,
            order.foodData.price_l,
            order.foodData.price_m,
            order.foodData.price_s);
    }

    /// <summary>
    /// 材料消耗记录
    /// </summary>
    public void ConsumeIngRecord(MenuInfoBean foodData)
    {
        innRecord.consumeIngOilsalt += foodData.ing_oilsalt;
        innRecord.consumeIngMeat += foodData.ing_meat;
        innRecord.consumeIngRiverfresh += foodData.ing_riverfresh;
        innRecord.consumeIngSeafood += foodData.ing_seafood;
        innRecord.consumeIngVegetablest += foodData.ing_vegetables;
        innRecord.consumeIngMelonfruit += foodData.ing_melonfruit;
        innRecord.consumeIngWaterwine += foodData.ing_waterwine;
        innRecord.consumeIngFlour += foodData.ing_flour;
    }

}