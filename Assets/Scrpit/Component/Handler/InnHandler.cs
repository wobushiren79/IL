using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Diagnostics;

public class InnHandler : BaseMonoBehaviour, IBaseObserver
{
    public enum InnStatusEnum
    {
        Close = 0,
        Open = 1,
    }

    //客栈状态
    private InnStatusEnum innStatus = InnStatusEnum.Close;

    //数据管理
    public GameDataManager gameDataManager;
    public InnFoodManager innFoodManager;
    public ToastManager toastManager;
    //NPC创建
    public NpcWorkerBuilder workerBuilder;
    //游戏数据处理
    public GameDataHandler gameDataHandler;
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
    //音效处理
    protected AudioHandler audioHandler;
    protected GameTimeHandler gameTimeHandler;

    //闹事的人的列表
    public List<NpcAIRascalCpt> rascalrQueue = new List<NpcAIRascalCpt>();
    //排队的人
    public List<OrderForCustomer> cusomerQueue = new List<OrderForCustomer>();
    //排队等待烹饪的食物
    public List<OrderForCustomer> foodQueue = new List<OrderForCustomer>();
    //排队送餐的食物
    public List<OrderForCustomer> sendQueue = new List<OrderForCustomer>();
    //排队清理的食物
    public List<OrderForCustomer> clearQueue = new List<OrderForCustomer>();

    //订单列表
    public List<OrderForCustomer> listOrder = new List<OrderForCustomer>();
    //当天记录流水
    protected InnRecordBean innRecord = new InnRecordBean();

    private void Awake()
    {
        audioHandler = Find<AudioHandler>(ImportantTypeEnum.AudioHandler);
        gameDataHandler= Find<GameDataHandler>(ImportantTypeEnum.GameDataHandler);
        gameTimeHandler = Find<GameTimeHandler>(ImportantTypeEnum.TimeHandler);
        gameTimeHandler.AddObserver(this);
    }

    /// <summary>
    /// 初始化流水
    /// </summary>
    public void InitRecord()
    {
        innRecord = new InnRecordBean();
        gameTimeHandler.GetTime(out int year, out int month, out int day);
        innRecord.year = year;
        innRecord.month = month;
        innRecord.day = day;
    }

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
                    OrderForCustomer orderForCustomer = cusomerQueue[0];
                    //设置座位
                    orderForCustomer.table = tableCpt;
                    //设置客户前往座位
                    orderForCustomer.customer.SetIntent(NpcAICustomerCpt.CustomerIntentEnum.GotoSeat, orderForCustomer);
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
        for (int i = 0; i < listOrder.Count; i++)
        {
            OrderForCustomer orderCusomer = listOrder[i];
            if (orderCusomer.customer != null && orderCusomer.customer.gameObject != null)
                Destroy(orderCusomer.customer.gameObject);
            if (orderCusomer.foodCpt != null && orderCusomer.foodCpt.gameObject != null)
                Destroy(orderCusomer.foodCpt.gameObject);
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
        foreach (NpcAIWorkerCpt itemWorker in workerBuilder.listNpcWorker)
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
        listOrder.Clear();
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

    public InnRecordBean GetInnRecord()
    {
        innRecord.status = (int)GameCommonInfo.currentDayData.dayStatus;
        return innRecord;
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
        int height = gameDataManager.gameData.GetInnBuildData().innHeight;
        int width = gameDataManager.gameData.GetInnBuildData().innWidth;
        Vector3 position = new Vector3(UnityEngine.Random.Range(1.5f, (float)width - 1f), UnityEngine.Random.Range(1.5f, (float)height - 1f));
        return position;
    }

    /// <summary>
    /// 创建一个订单
    /// </summary>
    /// <param name="npc"></param>
    /// <returns></returns>
    public OrderForCustomer CreateOrder(NpcAICustomerCpt npc)
    {
        OrderForCustomer order = new OrderForCustomer(npc);
        npc.SetOrderForCustomer(order);
        listOrder.Add(order);
        return order;
    }

    /// <summary>
    /// 随机点餐
    /// </summary>
    /// <returns></returns>
    public MenuInfoBean OrderForFood(OrderForCustomer orderForCustomer)
    {
        //获取正在出售的菜品
        List<MenuOwnBean> listOwnMenu = gameDataManager.gameData.GetMenuListForSell();
        if (listOwnMenu.Count == 0)
            return null;
        //随机获取一个菜品
        MenuOwnBean menuOwnItem = RandomUtil.GetRandomDataByList(listOwnMenu);
        if (menuOwnItem == null)
            return null;
        return OrderForFood(orderForCustomer, menuOwnItem);
    }

    /// <summary>
    /// 点餐
    /// </summary>
    /// <param name="orderForCustomer"></param>
    /// <param name="menuOwn"></param>
    /// <returns></returns>
    public MenuInfoBean OrderForFood(OrderForCustomer orderForCustomer, MenuOwnBean menuOwn)
    {
        //食物数据库里有这个数据
        if (menuOwn == null)
            return null;
        return OrderForFood(orderForCustomer, menuOwn.menuId);
    }

    public MenuInfoBean OrderForFood(OrderForCustomer orderForCustomer, long menuId)
    {
        //食物数据库里有这个数据
        MenuInfoBean menuInfo = innFoodManager.GetFoodDataById(menuId);
        if (menuInfo != null && gameDataManager.gameData.CheckIsSellMenu(menuId))
        {
            orderForCustomer.foodData = menuInfo;
            foodQueue.Add(orderForCustomer);
            return menuInfo;
        }
        return null;
    }



    /// <summary>
    /// 强制结束一个订单 
    /// </summary>
    /// <param name="orderForCustomer"></param>
    /// <param name="isPraise">是否评价</param>
    public void EndOrderForForce(OrderForCustomer orderForCustomer, bool isPraise)
    {
        //如果食物已经做出来了
        if (orderForCustomer.table != null)
        {
            //如果桌子上有食物 并且清理列表里面没有 则加入清理列表
            if (orderForCustomer.customer.customerIntent == NpcAICustomerCpt.CustomerIntentEnum.GotoSeat
                || orderForCustomer.customer.customerIntent == NpcAICustomerCpt.CustomerIntentEnum.WaitFood
                || orderForCustomer.customer.customerIntent == NpcAICustomerCpt.CustomerIntentEnum.Eatting)
            {
                orderForCustomer.table.CleanTable();
            }
            //支付一半的钱
            //PayMoney(orderForCustomer, 0.5f);
        }
        //根据心情评价客栈 前提订单里有他
        if (isPraise)
            InnPraise(orderForCustomer.innEvaluation.GetPraise());
        //如果桌子还属于这个顾客
        //如果在排队，移除排队
        if (cusomerQueue.Contains(orderForCustomer))
            cusomerQueue.Remove(orderForCustomer);
        if (foodQueue.Contains(orderForCustomer))
            foodQueue.Remove(orderForCustomer);
        if (sendQueue.Contains(orderForCustomer))
            sendQueue.Remove(orderForCustomer);
        EndOrder(orderForCustomer);
    }

    /// <summary>
    /// 尝试结束订单
    /// </summary>
    /// <param name="orderForCustomer"></param>
    public void EndOrder(OrderForCustomer orderForCustomer)
    {
        //食物已被清理
        if (orderForCustomer.foodCpt == null
            //顾客已经离开
            && listOrder.Contains(orderForCustomer))
        {
            listOrder.Remove(orderForCustomer);
        }
    }

    /// <summary>
    /// 付钱
    /// </summary>
    /// <param name="food"></param>
    public void PayMoney(OrderForCustomer order, long payMoneyL, long payMoneyM, long payMoneyS)
    {
        //最小
        if (payMoneyL == 0 && payMoneyM == 0 && payMoneyS == 0)
        {
            payMoneyS = 1;
        }

        //账本记录
        innRecord.AddSellNumber(order.foodData.id, 1, payMoneyL, payMoneyM, payMoneyS);
        //根据心情评价客栈 前提订单里有他
        InnPraise(order.innEvaluation.GetPraise());
        //记录+1
        gameDataManager.gameData.AddMenuSellNumber(1, order.foodData.id, payMoneyL, payMoneyM, payMoneyS);
        //金钱增加
        gameDataHandler.AddMoney(payMoneyL, payMoneyM, payMoneyS);
        //播放音效
        audioHandler.PlaySound(AudioSoundEnum.PayMoney, new Vector3(order.customer.transform.position.x, order.customer.transform.position.y, Camera.main.transform.position.z));
        //展示特效
        innPayHandler.ShowPayEffects(order.customer.transform.position, payMoneyL, payMoneyM, payMoneyS);
        //结束订单
        EndOrder(order);
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
        innRecord.consumeIngVegetables += foodData.ing_vegetables;
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
        List<NpcAIWorkerCpt> listWork = workerBuilder.listNpcWorker;
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
            case WorkerEnum.Accountant:
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

    /// <summary>
    /// 给客栈评价
    /// </summary>
    public void InnPraise(PraiseTypeEnum praiseType)
    {
        //记录好评数量
        innRecord.AddPraise(praiseType, 1);
        //总记录
        gameDataManager.gameData.GetAchievementData().AddPraise(praiseType, 1);
        //增加评价
        gameDataManager.gameData.GetInnAttributesData().AddPraise((int)praiseType);
    }


    #region 时间回调
    public void ObserbableUpdate<T>(T observable, int type, params object[] obj) where T : UnityEngine.Object
    {
        if (observable == gameTimeHandler)
        {
            if (type == (int)GameTimeHandler.NotifyTypeEnum.NewDay)
            {
                InitRecord();
            }
            else if (type == (int)GameTimeHandler.NotifyTypeEnum.EndDay)
            {

            }
        }
    }
    #endregion
}