using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Linq;
using System.Collections;

public class InnHandler : BaseHandler<InnHandler, InnManager>
{
    public enum InnStatusEnum
    {
        Close = 0,
        Open = 1,
    }

    //客栈状态
    private InnStatusEnum innStatus = InnStatusEnum.Close;

    //客栈桌子处理
    public InnTableHandler innTableHandler;
    //烹饪处理
    public InnCookHandler innCookHandler;
    //服务处理
    public InnWaiterHandler innWaiterHandler;
    //支付处理
    public InnPayHandler innPayHandler;
    //客栈战斗处理
    public InnFightHandler innFightHandler;
    //入口处理
    public InnEntranceHandler innEntranceHandler;
    //住宿处理
    public InnHotelHandler innHotelHandler;

    //闹事的人的列表
    public List<NpcAIRascalCpt> rascalrQueue = new List<NpcAIRascalCpt>();
    //排队就餐的人
    public List<OrderForCustomer> cusomerQueue = new List<OrderForCustomer>();
    //排队等待烹饪的食物
    public List<OrderForCustomer> foodQueue = new List<OrderForCustomer>();
    //排队送餐的食物
    public List<OrderForCustomer> sendQueue = new List<OrderForCustomer>();
    //排队清理的食物
    public List<OrderForCustomer> cleanQueue = new List<OrderForCustomer>();

    //排队等待接待的住宿
    public List<OrderForHotel> hotelQueue = new List<OrderForHotel>();
    //排队清理床单
    public List<OrderForHotel> bedCleanQueue = new List<OrderForHotel>();

    //订单列表
    public List<OrderForBase> listOrder = new List<OrderForBase>();
    //当天记录流水
    protected InnRecordBean innRecord = new InnRecordBean();

    public override void Awake()
    {
        base.Awake();
        innTableHandler = gameObject.AddComponent<InnTableHandler>();
        innCookHandler = gameObject.AddComponent<InnCookHandler>();
        innWaiterHandler = gameObject.AddComponent<InnWaiterHandler>();
        innPayHandler = gameObject.AddComponent<InnPayHandler>();
        innFightHandler = gameObject.AddComponent<InnFightHandler>();
        innEntranceHandler = gameObject.AddComponent<InnEntranceHandler>();
        innHotelHandler = gameObject.AddComponent<InnHotelHandler>();

        GameTimeHandler.Instance.RegisterNotifyForTime(NotifyForTime);
    }
    private void OnDestroy()
    {
        GameTimeHandler.Instance.UnRegisterNotifyForTime(NotifyForTime);
    }
    public void Update()
    {
        HandleForInnOpen();
    }

    /// <summary>
    /// 初始化流水
    /// </summary>
    public void InitRecord()
    {
        innRecord = new InnRecordBean();
        GameTimeHandler.Instance.GetTime(out int year, out int month, out int day);
        innRecord.year = year;
        innRecord.month = month;
        innRecord.day = day;
    }

    /// <summary>
    /// 初始化客栈
    /// </summary>
    public void InitInn()
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        innEntranceHandler.InitEntranceList();
        innTableHandler.InitTableList();
        innCookHandler.InitStoveList();
        innPayHandler.InitCounterList();
        innHotelHandler.InitBedList(gameData.GetInnBuildData());
        InitWorker();
    }

    /// <summary>
    /// 初始化员工
    /// </summary>
    public void InitWorker()
    {
        NpcHandler.Instance.builderForWorker.InitWorkerData();
    }

    /// <summary>
    /// 关闭客栈
    /// </summary>
    public void CloseInn()
    {
        AudioHandler.Instance.StopMusic();
        innStatus = InnStatusEnum.Close;

        //删除所有顾客
        //驱除所有顾客
        NpcHandler.Instance.builderForCustomer.ClearNpc();
        //清理事件NPC
        NpcHandler.Instance.builderForEvent.ClearNpc();
        //清理家族NPC
        NpcHandler.Instance.builderForFamily.ClearNpc();

        for (int i = 0; i < listOrder.Count; i++)
        {
            OrderForBase orderForBase = listOrder[i];
            if (orderForBase as OrderForCustomer != null)
            {
                OrderForCustomer orderCusomer = orderForBase as OrderForCustomer;
                //清理所有食物
                if (orderCusomer.foodCpt != null && orderCusomer.foodCpt.gameObject != null)
                    Destroy(orderCusomer.foodCpt.gameObject);
            }
        }
        //清理所有桌子
        innTableHandler.CleanAllTable();
        //清理所有柜台
        innPayHandler.CleanAllCounter();
        //清理所有灶台
        innCookHandler.CleanAllStove();
        //清理所有的床
        innHotelHandler.CleanAllBed();

        //结束所有拉人活动 
        //结束所有引路活动
        for (int i = 0; i < NpcHandler.Instance.builderForWorker.listNpcWorker.Count; i++)
        {
            NpcAIWorkerCpt itemWorker = NpcHandler.Instance.builderForWorker.listNpcWorker[i];
            if (itemWorker != null && itemWorker.aiForAccost.npcAICustomer != null)
            {
                itemWorker.aiForAccost.npcAICustomer.SetIntent(NpcAICustomerCpt.CustomerIntentEnum.Leave);
            }
        }

        NpcHandler.Instance.builderForWorker.ClearAllWork();

        rascalrQueue.Clear();
        cusomerQueue.Clear();
        foodQueue.Clear();
        sendQueue.Clear();
        cleanQueue.Clear();
        listOrder.Clear();

        hotelQueue.Clear();
        bedCleanQueue.Clear();
    }

    /// <summary>
    /// 开启客栈
    /// </summary>
    public void OpenInn()
    {
        AudioHandler.Instance.PlayMusicForLoop(AudioMusicEnum.Game);
        NpcHandler.Instance.builderForWorker.BuildAllWorker();
        InitInn();
        innStatus = InnStatusEnum.Open;
        //生成客人
        NpcHandler.Instance.builderForCustomer.StartBuildCustomer();
    }

    public InnRecordBean GetInnRecord()
    {
        innRecord.status = (int)GameTimeHandler.Instance.GetDayStatus();
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
    public BuildCounterCpt GetCounter(Vector3 position)
    {
        BuildCounterCpt counterCpt = null;
        GameConfigBean gameConfig = GameDataHandler.Instance.manager.GetGameConfig();
        if (gameConfig.statusForCheckOut == 0)
        {
            //选择最近的柜台
            counterCpt = innPayHandler.GetCloseCounter(position);
        }
        else if (gameConfig.statusForCheckOut == 1)
        {
            //随机选择一个柜台
            counterCpt = RandomUtil.GetRandomDataByList(innPayHandler.listCounterCpt);
        }
        else
        {
            //选择人少柜台
            counterCpt = innPayHandler.GetLessCounter();
        }
        return counterCpt;
    }

    /// <summary>
    ///  获取随机一个入口附近的坐标 默认获取最近的坐标
    /// </summary>
    /// <returns></returns>
    public Vector3 GetRandomEntrancePosition()
    {
        return innEntranceHandler.GetRandomEntrancePosition();
    }
    public Vector3 GetCloseRandomEntrancePosition(Vector3 position)
    {
        List<BuildDoorCpt> listDoor = innEntranceHandler.GetEntranceList();
        float dis = 0;
        BuildDoorCpt targetDoor = null;
        for (int i = 0; i < listDoor.Count; i++)
        {
            BuildDoorCpt buildDoor = listDoor[i];
            float disTemp = Vector3.Distance(position, buildDoor.transform.position);
            if (dis == 0 || disTemp < dis)
            {
                dis = disTemp;
                targetDoor = buildDoor;
            }
        }
        if (targetDoor == null)
            return Vector3.zero;
        return GameUtil.GetTransformInsidePosition2D(targetDoor.transform);
    }

    /// <summary>
    /// 获取最近的楼梯
    /// </summary>
    /// <returns></returns>
    public BuildStairsCpt GetCloseStairs(Vector3 position)
    {
        return innEntranceHandler.GetCloseStairs(position);
    }

    /// <summary>
    /// 获取楼梯的上下位置
    /// </summary>
    /// <returns></returns>
    public void GetStairsByRemarkId(string markId, out Vector3 layerFirstPosition, out Vector3 layerSecondPosition)
    {
        layerFirstPosition = Vector3.zero;
        layerSecondPosition = Vector3.zero;
        innEntranceHandler.GetStairsPosition(markId, out layerFirstPosition, out layerSecondPosition);
    }

    /// <summary>
    /// 获取随机客栈内一点
    /// </summary>
    /// <returns></returns>
    public Vector3 GetRandomInnPositon()
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        int height = gameData.GetInnBuildData().innHeight;
        int width = gameData.GetInnBuildData().innWidth;
        Vector3 position = new Vector3(UnityEngine.Random.Range(2f, (float)width - 2f), UnityEngine.Random.Range(2f, (float)height - 2f));
        return position;
    }

    /// <summary>
    /// 创建一个订单
    /// </summary>
    /// <param name="npc"></param>
    /// <returns></returns>
    public OrderForCustomer CreateOrder(NpcAICustomerCpt npc)
    {
        OrderForCustomer order = new OrderForCustomer(npc.customerType, npc);
        npc.SetOrderForCustomer(order);
        listOrder.Add(order);
        return order;
    }

    /// <summary>
    /// 创建一个住宿订单
    /// </summary>
    /// <param name="npc"></param>
    /// <param name="buildBedCpt"></param>
    /// <returns></returns>
    public OrderForHotel CreateOrderForHotel(NpcAICustomerForHotelCpt npc, BuildBedCpt buildBedCpt)
    {
        OrderForHotel order = new OrderForHotel(npc, buildBedCpt);
        hotelQueue.Add(order);
        listOrder.Add(order);
        return order;
    }

    /// <summary>
    /// 随机点餐
    /// </summary>
    /// <returns></returns>
    public MenuInfoBean OrderForFood(OrderForCustomer orderForCustomer)
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        //获取正在出售的菜品
        List<MenuOwnBean> listOwnMenu = gameData.GetMenuListForSell();
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

    /// <summary>
    /// 点指定食物
    /// </summary>
    /// <param name="orderForCustomer"></param>
    /// <param name="menuId"></param>
    /// <returns></returns>
    public MenuInfoBean OrderForFood(OrderForCustomer orderForCustomer, long menuId)
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        //食物数据库里有这个数据
        MenuInfoBean menuInfo = InnFoodHandler.Instance.manager.GetFoodDataById(menuId);
        if (menuInfo != null && gameData.CheckIsSellMenu(menuId))
        {
            orderForCustomer.foodData = menuInfo;
            foodQueue.Add(orderForCustomer);
            RecordCustomerForMenu(orderForCustomer, menuId);
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
        //如果已经安排了位置
        if (orderForCustomer.table != null)
        {
            //如果食物还没有吃完时 则直接删除食物
            if (orderForCustomer.customer.customerIntent == NpcAICustomerCpt.CustomerIntentEnum.GotoSeat
                || orderForCustomer.customer.customerIntent == NpcAICustomerCpt.CustomerIntentEnum.WaitFood
                || orderForCustomer.customer.customerIntent == NpcAICustomerCpt.CustomerIntentEnum.Eatting)
            {
                orderForCustomer.table.CleanTable();
                //如果食物是正在做或者还没有送过来，也直接删除
                if (orderForCustomer.foodCpt != null)
                {
                    Destroy(orderForCustomer.foodCpt.gameObject);
                }
            }
            //如果食物已经吃完，则应该在清理列表中
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

    public void EndOrderForForce(OrderForHotel orderForHotel, bool isPraise)
    {
        if (orderForHotel == null)
            return;
        EndOrder(orderForHotel);
        //根据心情评价客栈
        if (isPraise)
        {
            InnPraise(orderForHotel.innEvaluation.GetPraise());
        }
        //先移除排队列表
        if (hotelQueue.Contains(orderForHotel))
        {
            hotelQueue.Remove(orderForHotel);
        }
        //顾客离开
        if (orderForHotel.customer != null)
        {
            //如果顾客在2楼 先下楼
            if (orderForHotel.customer.customerHotelIntent == NpcAICustomerForHotelCpt.CustomerHotelIntentEnum.Sleep
             || orderForHotel.customer.customerHotelIntent == NpcAICustomerForHotelCpt.CustomerHotelIntentEnum.GoToBed
             || orderForHotel.customer.customerHotelIntent == NpcAICustomerForHotelCpt.CustomerHotelIntentEnum.GoToPay
             || orderForHotel.customer.customerHotelIntent == NpcAICustomerForHotelCpt.CustomerHotelIntentEnum.GoToStairsForSecond
             )
            {
                orderForHotel.customer.SetIntent(NpcAICustomerForHotelCpt.CustomerHotelIntentEnum.GoToStairsForSecond);
            }
            else
            {
                orderForHotel.customer.SetIntent(NpcAICustomerForHotelCpt.CustomerHotelIntentEnum.Leave);
            }
        }
        //床还原  如果是排队时候因为心情降低这里不还原
        if (orderForHotel.bed != null && orderForHotel.GetOrderStatus() != OrderHotelStatusEnum.Pay)
        {
            orderForHotel.bed.CleanBed();
        }
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

    public void EndOrder(OrderForHotel orderForHotel)
    {
        orderForHotel.SetOrderStatus(OrderHotelStatusEnum.End);
        if (listOrder.Contains(orderForHotel))
        {
            listOrder.Remove(orderForHotel);
        }
    }


    /// <summary>
    /// 结算所有客户
    /// </summary>
    public void SettlementAllCustomer()
    {
        if (listOrder.IsNull())
            return;
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        for (int i = 0; i < listOrder.Count; i++)
        {
            OrderForBase itemOrder = listOrder[i];
            if (itemOrder == null)
            {
                continue;
            }
            if (itemOrder as OrderForCustomer != null)
            {
                OrderForCustomer orderForCustomer = itemOrder as OrderForCustomer;
                if (orderForCustomer.customer != null &&
                              (orderForCustomer.customer.customerIntent == NpcAICustomerCpt.CustomerIntentEnum.Eatting
                              || orderForCustomer.customer.customerIntent == NpcAICustomerCpt.CustomerIntentEnum.GotoPay
                              || orderForCustomer.customer.customerIntent == NpcAICustomerCpt.CustomerIntentEnum.WaitPay
                               || orderForCustomer.customer.customerIntent == NpcAICustomerCpt.CustomerIntentEnum.Pay))
                {
                    if (orderForCustomer.foodData == null)
                        continue;
                    MenuOwnBean menuOwn = gameData.GetMenuById(orderForCustomer.foodData.id);
                    menuOwn.GetPrice(orderForCustomer.foodData, out long payMoneyL, out long payMoneyM, out long payMoneyS);
                    PayMoney(itemOrder, payMoneyL, payMoneyM, payMoneyS, false);
                }
            }
            else if (itemOrder as OrderForHotel != null)
            {
                OrderForHotel orderForHotel = itemOrder as OrderForHotel;
                BuildBedBean buildBedData = orderForHotel.bed.buildBedData;
                PayMoney(itemOrder,
                    buildBedData.priceL * orderForHotel.sleepTime,
                    buildBedData.priceM * orderForHotel.sleepTime,
                    buildBedData.priceS * orderForHotel.sleepTime,
                    false);
            }
        }
    }


    /// <summary>
    /// 付钱
    /// </summary>
    /// <param name="food"></param>
    public void PayMoney(OrderForBase order, long payMoneyL, long payMoneyM, long payMoneyS, bool isPlaySound)
    {
        //最小
        if (payMoneyL == 0 && payMoneyM == 0 && payMoneyS == 0)
        {
            payMoneyS = 1;
        }
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        UserAchievementBean userAchievement = gameData.GetAchievementData();
        Vector3 payEffectsPosition = Vector3.zero;
        if (order as OrderForCustomer != null)
        {
            OrderForCustomer orderForCustomer = order as OrderForCustomer;
            //账本记录
            innRecord.AddSellNumber(orderForCustomer.foodData.id, 1, payMoneyL, payMoneyM, payMoneyS);
            //根据心情评价客栈 前提订单里有他
            InnPraise(orderForCustomer.innEvaluation.GetPraise());
            //记录+1
            gameData.AddMenuSellNumber(1, orderForCustomer.foodData.id, payMoneyL, payMoneyM, payMoneyS, out bool isMenuLevelUp);
            //成就+1
            userAchievement.AddNumberForCustomerFoodComplete(orderForCustomer.customer.customerType, 1);
            if (isMenuLevelUp)
            {
                Sprite spFoodIcon = InnFoodHandler.Instance.manager.GetFoodSpriteByName(orderForCustomer.foodData.icon_key);
                UIHandler.Instance.ToastHint<ToastView>(spFoodIcon, string.Format(TextHandler.Instance.manager.GetTextById(1131), orderForCustomer.foodData.name));
            }
            payEffectsPosition = orderForCustomer.customer.transform.position;
        }
        else if (order as OrderForHotel != null)
        {
            OrderForHotel orderForHotel = order as OrderForHotel;
            //账本记录
            innRecord.AddHotelNumber(1, payMoneyL, payMoneyM, payMoneyS);
            //根据心情评价客栈 前提订单里有他
            InnPraise(orderForHotel.innEvaluation.GetPraise());
            //记录+1
            gameData.AddBedSellNumber(
                orderForHotel.bed.buildBedData.remarkId,
                1,
                orderForHotel.sleepTime,
                payMoneyL, payMoneyM, payMoneyS,
                out bool isBedLevelUp);
            //成就+1
            userAchievement.AddNumberForCustomerHotelComplete(1);
            if (isBedLevelUp)
            {
                Sprite spBedIcon = IconHandler.Instance.GetIconSpriteByName("worker_waiter_bed_pro_2");
                UIHandler.Instance.ToastHint<ToastView>(spBedIcon, string.Format(TextHandler.Instance.manager.GetTextById(1131), orderForHotel.bed.buildBedData.bedName));
            }
            payEffectsPosition = orderForHotel.customer.transform.position;
        }

        //金钱增加
        GameDataHandler.Instance.AddMoney( payMoneyL, payMoneyM, payMoneyS, payEffectsPosition);
        //播放音效
        if (isPlaySound)
        {
            AudioHandler.Instance.PlaySound(AudioSoundEnum.PayMoney);
            //展示特效
            //innPayHandler.ShowPayEffects(payEffectsPosition, payMoneyL, payMoneyM, payMoneyS);
        }
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
        NpcWorkerBuilder npcWorkerBuilder = NpcHandler.Instance.builderForWorker;
        if (npcWorkerBuilder == null)
            return;
        List<NpcAIWorkerCpt> listWork = npcWorkerBuilder.listNpcWorker;
        if (listWork == null)
            return;
        for (int i = 0; i < listWork.Count; i++)
        {
            NpcAIWorkerCpt itemWorker = listWork[i];
            //通过优先级设置工作
            itemWorker.SetWorkByPriority();
        }
    }

    /// <summary>
    /// 通过不同的工作类型分配不同的工作
    /// </summary>
    public bool DistributionWorkForType(WorkerDetilsEnum workDetailsType, NpcAIWorkerCpt workNpc)
    {
        switch (workDetailsType)
        {
            case WorkerDetilsEnum.AccountantForPay:
                if (!innPayHandler.listCounterCpt.IsNull())
                {
                    for (int i = 0; i < innPayHandler.listCounterCpt.Count; i++)
                    {
                        BuildCounterCpt counterCpt = innPayHandler.listCounterCpt[i];
                        if (!counterCpt.payQueue.IsNull())
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
            case WorkerDetilsEnum.ChefForCook:
                //排队做菜处理
                if (!foodQueue.IsNull())
                {
                    bool isSuccess = innCookHandler.SetChefForCook(foodQueue[0], workNpc);
                    if (isSuccess)
                    {
                        foodQueue.RemoveAt(0);
                        return true;
                    }
                }
                break;
            case WorkerDetilsEnum.WaiterForSend:
                //排队送菜处理
                if (!sendQueue.IsNull())
                {
                    OrderForCustomer orderForSend = sendQueue[0];
                    bool isSuccess = innWaiterHandler.SetSendFood(orderForSend, workNpc);
                    if (isSuccess)
                    {
                        sendQueue.Remove(orderForSend);
                        return true;
                    }
                }
                break;
            case WorkerDetilsEnum.WaiterForCleanTable:
                //排队清理处理
                if (!cleanQueue.IsNull())
                {
                    //搜寻最近的桌子
                    OrderForCustomer clearItem = null;
                    float distance = float.MaxValue;
                    for (int i = 0; i < cleanQueue.Count; i++)
                    {
                        OrderForCustomer itemOrder = cleanQueue[i];
                        float tempDistance = Vector3.Distance(itemOrder.table.GetTablePosition(), workNpc.transform.position);
                        if (tempDistance < distance)
                        {
                            distance = tempDistance;
                            clearItem = itemOrder;
                        }
                    }
                    if (clearItem == null)
                        return false;
                    bool isSuccess = innWaiterHandler.SetCleanFood(clearItem, workNpc);
                    if (isSuccess)
                    {
                        cleanQueue.Remove(clearItem);
                        return true;
                    }
                }
                break;
            case WorkerDetilsEnum.WaiterForCleanBed:
                //排队床单清理处理
                if (!bedCleanQueue.IsNull())
                {
                    OrderForHotel orderForHotel = bedCleanQueue[0];
                    bool isSuccess = innWaiterHandler.SetCleanBed(orderForHotel, workNpc);
                    if (isSuccess)
                    {
                        bedCleanQueue.Remove(orderForHotel);
                        return true;
                    }
                }
                break;
            case WorkerDetilsEnum.AccostForSolicit:
                workNpc.SetIntent(NpcAIWorkerCpt.WorkerIntentEnum.AccostSolicit);
                return true;
            case WorkerDetilsEnum.AccostForGuide:
                //等待接待
                if (!hotelQueue.IsNull())
                {
                    OrderForHotel orderForHotel = hotelQueue[0];
                    workNpc.SetIntent(NpcAIWorkerCpt.WorkerIntentEnum.AccostGuide, orderForHotel);
                    hotelQueue.Remove(orderForHotel);
                    return true;
                }
                break;
            case WorkerDetilsEnum.BeaterForDrive:
                //分派打架人选
                if (!rascalrQueue.IsNull())
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
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        //记录好评数量
        innRecord.AddPraise(praiseType, 1);
        //总记录
        gameData.GetAchievementData().AddPraise(praiseType, 1);
        //增加评价
        gameData.GetInnAttributesData().AddPraise((int)praiseType);
    }

    /// <summary>
    /// 记录顾客 用于顾客正式进店
    /// </summary>
    public void RecordCustomer(OrderForBase order)
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        //成就记录
        UserAchievementBean userAchievement = gameData.GetAchievementData();
        if (order as OrderForCustomer != null)
        {
            OrderForCustomer orderForCustomer = order as OrderForCustomer;
            //如果是团队需要记录团队ID
            if (orderForCustomer.customerType == CustomerTypeEnum.Team)
            {
                NpcTeamBean teamData = ((NpcAICustomerForGuestTeamCpt)orderForCustomer.customer).teamData;
                userAchievement.AddNumberForCustomerFood(orderForCustomer.customerType, teamData.id + "", 1);
            }
            else if (orderForCustomer.customerType == CustomerTypeEnum.Friend)
            {
                CharacterBean characterData = ((NpcAICostomerForFriendCpt)orderForCustomer.customer).characterData;
                userAchievement.AddNumberForCustomerFood(orderForCustomer.customerType, characterData.baseInfo.characterId, 1);
            }
            else
            {
                CharacterBean characterData = orderForCustomer.customer.characterData;
                userAchievement.AddNumberForCustomerFood(orderForCustomer.customerType, characterData.baseInfo.characterId, 1);
            }
            //流水记录
            innRecord.AddCutomerForFoodNumber(orderForCustomer.customerType, 1);
        }
        else if (order as OrderForHotel != null)
        {
            OrderForHotel orderForHotel = order as OrderForHotel;
            userAchievement.AddNumberForCustomerHotel(1);
            //流水记录
            innRecord.AddCutomerForHotelNumber(1);
        }

    }

    /// <summary>
    /// 记录顾客点菜
    /// </summary>
    public void RecordCustomerForMenu(OrderForCustomer order, long menuId)
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        //成就记录
        UserAchievementBean userAchievement = gameData.GetAchievementData();
        //如果是团队
        if (order.customerType == CustomerTypeEnum.Team)
        {
            NpcTeamBean teamData = ((NpcAICustomerForGuestTeamCpt)order.customer).teamData;
            userAchievement.AddMenuForCustomer(order.customerType, teamData.id + "", menuId);
        }
        else if (order.customerType == CustomerTypeEnum.Friend)
        {
            CharacterBean characterData = ((NpcAICostomerForFriendCpt)order.customer).characterData;
            userAchievement.AddMenuForCustomer(order.customerType, characterData.baseInfo.characterId, menuId);
        }
    }

    /// <summary>
    /// 判断是否还有床位
    /// </summary>
    /// <returns></returns>
    public BuildBedCpt GetIdleBed()
    {
        BuildBedCpt buildBedCpt = innHotelHandler.GetIdleBed();
        return buildBedCpt;
    }

    protected float timerForInnHandle = 0;
    /// <summary>
    /// 处理客栈营业
    /// </summary>
    /// <returns></returns>
    public void HandleForInnOpen()
    {
        if (innStatus == InnStatusEnum.Open)
        {
            //每隔0.5s 检测一次
            timerForInnHandle += Time.time;
            if (timerForInnHandle <= 0.2f)
            {
                return;
            }
            timerForInnHandle = 0;

            //排队等待处理
            HandleForCusomerQueue();
            //给闲置的工作人员分配工作
            DistributionWorkForIdleWorker();
        }
    }


    /// <summary>
    /// 处理-排队
    /// </summary>
    public void HandleForCusomerQueue()
    {
        if (!cusomerQueue.IsNull())
        {
            BuildTableCpt tableCpt = innTableHandler.GetIdleTable();
            if (tableCpt != null)
            {
                //排队成功
                OrderForCustomer orderForCustomer = cusomerQueue[0];
                //移除排队列表
                cusomerQueue.RemoveAt(0);
                //设置座位
                orderForCustomer.table = tableCpt;
                //设置客户前往座位
                orderForCustomer.customer.SetIntent(NpcAICustomerCpt.CustomerIntentEnum.GotoSeat, orderForCustomer);
            }
        }
    }


    #region 时间回调
    public void NotifyForTime(GameTimeHandler.NotifyTypeEnum notifyType, float timeHour)
    {
        if (notifyType == GameTimeHandler.NotifyTypeEnum.NewDay)
        {
            InitRecord();
        }
        else if (notifyType == GameTimeHandler.NotifyTypeEnum.EndDay)
        {

        }
    }
    #endregion
}