using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Diagnostics;

public class InnHandler : BaseMonoBehaviour
{
    public enum InnStatusEnum
    {
        Open,
        Close,
    }

    //客栈状态
    private InnStatusEnum innStatus = InnStatusEnum.Close;

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
    //客栈战斗处理
    public InnFightHandler innFightHandler;
    // 入口处理
    public InnEntranceHandler innEntranceHandler;

    //闹事的人的列表
    public List<NpcAIRascalCpt> rascalrQueue = new List<NpcAIRascalCpt>();
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
        workerBuilder.InitWorkerData();
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
            //给闲置的工作人员分配工作
            DistributionWorkForIdleWorker();
        }
    }

    /// <summary>
    /// 关闭客栈
    /// </summary>
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
        //清理排队的客人
        foreach (NpcAICustomerCpt itemCusomter in cusomerQueue)
        {
            Destroy(itemCusomter.gameObject);
        }
        //清理所有桌子
        for (int i = 0; i < innTableHandler.listTableCpt.Count; i++)
        {
            BuildTableCpt buildTableCpt = innTableHandler.listTableCpt[i];
            buildTableCpt.CleanTable();
        };
        //清理所有柜台
        for (int i = 0; i < innPayHandler.listCounterCpt.Count; i++)
        {
            BuildCounterCpt buildCounterCpt = innPayHandler.listCounterCpt[i];
            buildCounterCpt.ClearCounter();
        };
        //清理所有灶台
        for (int i = 0; i < innCookHandler.listStoveCpt.Count; i++)
        {
            BuildStoveCpt buildStoveCpt = innCookHandler.listStoveCpt[i];
            buildStoveCpt.ClearStove();
        };
        //结束所有拉人活动
        foreach (NpcAIWorkerCpt itemWorker in workerBuilder.npcWorkerList)
        {
            if (itemWorker != null && itemWorker.aiForAccost.npcAICustomer != null)
            {
                itemWorker.aiForAccost.npcAICustomer.SetIntent(NpcAICustomerCpt.CustomerIntentEnum.Leave);
            }
        }
        //清理所有捣乱的人
        foreach (NpcAIRascalCpt itemRascal in rascalrQueue)
        {
            Destroy(itemRascal.gameObject);
        }

        rascalrQueue.Clear();
        cusomerQueue.Clear();
        foodQueue.Clear();
        sendQueue.Clear();
        clearQueue.Clear();
        orderList.Clear();
        workerBuilder.ClearAllWork();
    }

    /// <summary>
    /// 开启客栈
    /// </summary>
    public void OpenInn()
    {
        workerBuilder.BuildAllWorker();
        InitInn();
        innStatus = InnStatusEnum.Open;
    }

    /// <summary>
    /// 获取客栈状态
    /// </summary>
    /// <returns></returns>
    public InnStatusEnum GetInnStatus()
    {
        return innStatus;
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
    /// 获取随机客栈内一点
    /// </summary>
    /// <returns></returns>
    public Vector3 GetRandomInnPositon()
    {
        int height= gameDataManager.gameData.GetInnBuildData().innHeight;
        int width = gameDataManager.gameData.GetInnBuildData().innWidth;
        Vector3 position = new Vector3(UnityEngine.Random.Range(1.5f,(float)width-1f), UnityEngine.Random.Range(1.5f, (float)height-1f));
        return position;
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
            //PayMoney(orderForCustomer, 0.5f);
        }
        //如果桌子还属于这个顾客
        switch (orderForCustomer.customer.customerIntent)
        {
            case NpcAICustomerCpt.CustomerIntentEnum.GotoSeat:
            case NpcAICustomerCpt.CustomerIntentEnum.WaitFood:
            case NpcAICustomerCpt.CustomerIntentEnum.Eatting:
                orderForCustomer.table.CleanTable();
                if (orderForCustomer.foodCpt != null)
                    Destroy(orderForCustomer.foodCpt.gameObject);
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
    /// 付钱
    /// </summary>
    /// <param name="food"></param>
    public void PayMoney(OrderForCustomer order, float multiple)
    {
        long getMoneyL = (long)(order.foodData.price_l * multiple);
        long getMoneyM = (long)(order.foodData.price_m * multiple);
        long getMoneyS = (long)(order.foodData.price_s * multiple);
        
        //账本记录
        if (innRecord.sellNumber.ContainsKey(order.foodData.id))
            innRecord.sellNumber[order.foodData.id] += 1;
        else
            innRecord.sellNumber.Add(order.foodData.id, 1);
        innRecord.incomeL += getMoneyL;
        innRecord.incomeS += getMoneyM;
        innRecord.incomeM += getMoneyS;

        //记录+1
        gameDataManager.gameData.ChangeMenuSellNumber(1, order.foodData.id);
        //金钱增加
        gameDataManager.gameData.moneyL += getMoneyL;
        gameDataManager.gameData.moneyM += getMoneyM;
        gameDataManager.gameData.moneyS += getMoneyS;
        //展示特效
        innPayHandler.ShowPayEffects(order.customer.transform.position,getMoneyL,getMoneyM, getMoneyS);
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

    /// <summary>
    /// 给空闲的员工分配工作
    /// </summary>
    public void DistributionWorkForIdleWorker()
    {
        //获取所有工作者
        List<NpcAIWorkerCpt> listWork = workerBuilder.npcWorkerList;
        if (listWork == null)
            return;
        for (int i = 0; i < listWork.Count; i++)
        {
            NpcAIWorkerCpt itemWorker = listWork[i];
            //如果该工作者此时空闲
            if (itemWorker.workerIntent == NpcAIWorkerCpt.WorkerIntentEnum.Idle)
            {
                //通过优先级设置工作
                itemWorker.SetWorkByPriority();
            }
        }
    }

    /// <summary>
    /// 通过不同的工作类型分配不同的工作
    /// </summary>
    public bool DistributionWorkForType(WorkerEnum workType, NpcAIWorkerCpt workNpc)
    {
        switch (workType)
        {
            case WorkerEnum.Accounting:
                if (!CheckUtil.ListIsNull(innPayHandler.listCounterCpt))
                {
                    for (int i = 0; i < innPayHandler.listCounterCpt.Count; i++)
                    {
                        BuildCounterCpt counterCpt = innPayHandler.listCounterCpt[i];
                        if (!CheckUtil.ListIsNull(counterCpt.payQueue))
                        {
                            bool isSuccess = innPayHandler.SetPay(counterCpt.payQueue[0], workNpc);
                            if (isSuccess)
                            {
                                counterCpt.payQueue.RemoveAt(0);
                                return true;
                            }
                        }
                    }
                }
                break;
            case WorkerEnum.Chef:
                //排队做菜处理
                if (!CheckUtil.ListIsNull(foodQueue))
                {
                    bool isSuccess = innCookHandler.SetChefForCook(foodQueue[0], workNpc);
                    if (isSuccess)
                    {
                        foodQueue.RemoveAt(0);
                        return true;
                    }
                }
                break;
            case WorkerEnum.Waiter:
                //排队送菜处理
                if (!CheckUtil.ListIsNull(sendQueue))
                {
                    bool isSuccess = innWaiterHandler.SetSendFood(sendQueue[0], workNpc);
                    if (isSuccess)
                    {
                        sendQueue.RemoveAt(0);
                        return true;
                    }
                }
                //排队清理处理
                if (!CheckUtil.ListIsNull(clearQueue))
                {
                    bool isSuccess = innWaiterHandler.SetClearFood(clearQueue[0], workNpc);
                    if (isSuccess)
                    {
                        clearQueue.RemoveAt(0);
                        return true;
                    }
                }
                break;
            case WorkerEnum.Accost:
                workNpc.SetIntent(NpcAIWorkerCpt.WorkerIntentEnum.Accost);
                return true;
            case WorkerEnum.Beater:
                //分派打架人选
                if (!CheckUtil.ListIsNull(rascalrQueue))
                {
                    NpcAIRascalCpt npcAIRascal = innFightHandler.SetFight(rascalrQueue, workNpc);
                    if (npcAIRascal)
                    {
                        //rascalrQueue.Remove(npcAIRascal);
                        return true;
                    }
                }
                break;
        }
        return false;
    }


}