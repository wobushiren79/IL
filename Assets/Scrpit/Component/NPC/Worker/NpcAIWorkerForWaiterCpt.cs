using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using DG.Tweening;

public class NpcAIWorkerForWaiterCpt : NpcAIWokerFoBaseCpt
{
    public enum WaiterIntentEnum
    {
        Idle,//空闲
        GoToGetFood,//获取食物的路上
        SendFood,//运送食物中
        GoToCleanTable,//清理的路上
        CleaningTable,//清理中

        GoToStairsForFirst,//前往一楼楼梯
        GoToStairsForSecond,//前往二楼楼梯
        GoToBed,
        CleaningBed,//清理中
    }
    //订单
    public OrderForBase order;
    //送菜的进度图标
    public GameObject sendPro;
    //清理进度
    public GameObject cleanPro;
    //清理进度
    public GameObject bedPro;
    //移动的目的地
    public Vector3 movePosition;
    //服务员状态
    public WaiterIntentEnum waiterIntent = WaiterIntentEnum.Idle;


    private void Update()
    {
        switch (waiterIntent)
        {
            case WaiterIntentEnum.Idle:
                break;
            case WaiterIntentEnum.GoToGetFood:
                HandleForGoToGetFood();
                break;
            case WaiterIntentEnum.SendFood:
                HandleForSendFood();
                break;
            case WaiterIntentEnum.GoToCleanTable:
                HandleForGoToCleanTable();
                break;
            case WaiterIntentEnum.CleaningTable:
                break;
            case WaiterIntentEnum.GoToStairsForFirst:
                HandleForGoToStairsForFirst();
                break;
            case WaiterIntentEnum.GoToStairsForSecond:
                HandleForGoToStairsForSecond();
                break;
            case WaiterIntentEnum.GoToBed:
                HandleForGoToBed();
                break;
            case WaiterIntentEnum.CleaningBed:
                break;
        }
    }
    /// <summary>
    /// 设置送餐
    /// </summary>
    /// <param name="orderForCustomer"></param>
    public void StartFoodSend(OrderForCustomer orderForCustomer)
    {
        SetIntent(WaiterIntentEnum.GoToGetFood, orderForCustomer);
    }

    /// <summary>
    /// 设置清理
    /// </summary>
    /// <param name="orderForCustomer"></param>
    public void StartFoodClean(OrderForCustomer orderForCustomer)
    {
        SetIntent(WaiterIntentEnum.GoToCleanTable, orderForCustomer);
    }

    /// <summary>
    /// 设置理床
    /// </summary>
    /// <param name="orderForHotel"></param>
    public void StartBedClean(OrderForHotel orderForHotel)
    {
        SetIntent(WaiterIntentEnum.GoToStairsForFirst, orderForHotel);
    }

    /// <summary>
    /// 设置意图
    /// </summary>
    /// <param name="waiterIntent"></param>
    /// <param name="orderForCustomer"></param>
    public void SetIntent(WaiterIntentEnum waiterIntent, OrderForBase order)
    {
        if (gameObject == null)
            return;
        StopAllCoroutines();
        this.waiterIntent = waiterIntent;
        this.order = order;
        switch (waiterIntent)
        {
            case WaiterIntentEnum.Idle:
                SetIntentForIdle();
                break;
            case WaiterIntentEnum.GoToGetFood:
                SetIntentForGoToGetFood(order as OrderForCustomer);
                break;
            case WaiterIntentEnum.SendFood:
                SetIntentForSendFood(order as OrderForCustomer);
                break;
            case WaiterIntentEnum.GoToCleanTable:
                SetIntentForGoToCleanTable(order as OrderForCustomer);
                break;
            case WaiterIntentEnum.CleaningTable:
                SetIntentForCleaningTable(order as OrderForCustomer);
                break;
            case WaiterIntentEnum.GoToStairsForFirst:
                SetIntentForGoToStairsForFirst();
                break;
            case WaiterIntentEnum.GoToStairsForSecond:
                SetIntentForGoToStairsForSecond();
                break;
            case WaiterIntentEnum.GoToBed:
                SetIntentForGoToBed();
                break;
            case WaiterIntentEnum.CleaningBed:
                SetIntentForCleaningBed();
                break;
        }
    }

    public void SetIntent(WaiterIntentEnum waiterIntent)
    {
        SetIntent(waiterIntent, order);
    }

    /// <summary>
    /// 意图-闲置
    /// </summary>
    public void SetIntentForIdle()
    {
        sendPro.SetActive(false);
        cleanPro.SetActive(false);
        bedPro.SetActive(false);
        order = null;
        npcAIWorker.SetIntent(NpcAIWorkerCpt.WorkerIntentEnum.Idle);
    }

    /// <summary>
    /// 意图-前往取餐
    /// </summary>
    /// <param name="orderForCustomer"></param>
    public void SetIntentForGoToGetFood(OrderForCustomer orderForCustomer)
    {
        if (CheckUtil.CheckPath(transform.position, orderForCustomer.stove.GetTakeFoodPosition()))
        {
            sendPro.SetActive(true);
            movePosition = orderForCustomer.stove.GetTakeFoodPosition();
            npcAIWorker.characterMoveCpt.SetDestination(movePosition);
        }
        else
        {
            SetIntent(WaiterIntentEnum.Idle);
        }
    }

    /// <summary>
    /// 意图-送餐
    /// </summary>
    /// <param name="orderForCustomer"></param>
    public void SetIntentForSendFood(OrderForCustomer orderForCustomer)
    {
        sendPro.SetActive(true);
        npcAIWorker.SetTake(orderForCustomer.foodCpt.gameObject);
        orderForCustomer.foodCpt.transform.DOLocalMove(Vector3.zero, 0.2f);
        //orderForCustomer.foodCpt.transform.localPosition = Vector3.zero;
        movePosition = orderForCustomer.table.GetTablePosition();
        npcAIWorker.characterMoveCpt.SetDestination(movePosition);
    }

    /// <summary>
    /// 意图-前去清理
    /// </summary>
    /// <param name="orderForCustomer"></param>
    public void SetIntentForGoToCleanTable(OrderForCustomer orderForCustomer)
    {
        cleanPro.SetActive(true);
        movePosition = orderForCustomer.foodCpt.transform.position;
        npcAIWorker.characterMoveCpt.SetDestination(movePosition);
    }

    /// <summary>
    /// 意图-清理中
    /// </summary>
    /// <param name="orderForCustomer"></param>
    public void SetIntentForCleaningTable(OrderForCustomer orderForCustomer)
    {
        orderForCustomer.table.SetTableStatus(BuildTableCpt.TableStatusEnum.Cleaning);
        cleanPro.SetActive(true);
        StartCoroutine(CoroutineForCleanTable());
    }

    /// <summary>
    /// 意图-前往二楼
    /// </summary>
    public void SetIntentForGoToStairsForFirst()
    {
        bedPro.SetActive(true);
        OrderForHotel orderForHotel = order as OrderForHotel;
        BuildStairsCpt buildStairs = npcAIWorker.innHandler.GetCloseStairs(transform.position);
        if (buildStairs == null)
        {
            npcAIWorker.SetShout(GameCommonInfo.GetUITextById(13402));
            SetIntent(WaiterIntentEnum.Idle);
            return;
        }
        npcAIWorker.innHandler.GetStairsByRemarkId(buildStairs.remarkId, out Vector3 layerFirstPosition, out Vector3 layerSecondPosition);
        orderForHotel.layerFirstStairsPositionForClean = layerFirstPosition;
        orderForHotel.layerSecondStairsPositionForClean = layerSecondPosition;
        npcAIWorker.SetCharacterMove(layerFirstPosition);
    }

    /// <summary>
    /// 前往二楼楼梯
    /// </summary>
    public void SetIntentForGoToStairsForSecond()
    {
        BuildStairsCpt buildStairs = npcAIWorker.innHandler.GetCloseStairs(transform.position);
        bedPro.SetActive(false);
        OrderForHotel orderForHotel = order as OrderForHotel;

        npcAIWorker.innHandler.GetStairsByRemarkId(buildStairs.remarkId, out Vector3 layerFirstPosition, out Vector3 layerSecondPosition);
        orderForHotel.layerFirstStairsPositionForClean = layerFirstPosition;
        orderForHotel.layerSecondStairsPositionForClean = layerSecondPosition;
        npcAIWorker.SetCharacterMove(layerSecondPosition);
    }

    /// <summary>
    /// 前往床
    /// </summary>
    public void SetIntentForGoToBed()
    {
        bedPro.SetActive(true);
        OrderForHotel orderForHotel = order as OrderForHotel;
        Vector3 sleepPosition = orderForHotel.bed.GetSleepPosition();
        if (!CheckUtil.CheckPath(sleepPosition, transform.position))
        {
            //不能到达
            npcAIWorker.SetShout(GameCommonInfo.GetUITextById(13405));
            SetIntent(WaiterIntentEnum.GoToStairsForSecond);
            return;
        }
        npcAIWorker.SetCharacterMove(sleepPosition);
    }

    /// <summary>
    /// 清理床
    /// </summary>
    public void SetIntentForCleaningBed()
    {
        bedPro.SetActive(true);
        StartCoroutine(CoroutineForCleanBed());
    }


    /// <summary>
    /// 处理-前往获取食物
    /// </summary>
    public void HandleForGoToGetFood()
    {
        OrderForCustomer orderForCustomer = order as OrderForCustomer;
        if (order.CheckOrder())
        {
            if (npcAIWorker.characterMoveCpt.IsAutoMoveStop())
            {
                SetIntent(WaiterIntentEnum.SendFood, orderForCustomer);
            }
        }
        else
        {
            //删除食物
            if (orderForCustomer.foodCpt != null)
                Destroy(orderForCustomer.foodCpt.gameObject);
            SetIntent(WaiterIntentEnum.Idle);
        }
    }

    /// <summary>
    /// 处理-前往送食物
    /// </summary>
    public void HandleForSendFood()
    {
        OrderForCustomer orderForCustomer = order as OrderForCustomer;
        if (order.CheckOrder())
        {
            if (npcAIWorker.characterMoveCpt.IsAutoMoveStop())
            {
                //记录数据
                npcAIWorker.characterData.baseInfo.waiterInfo.AddSendNumber(1);
                //增加经验
                npcAIWorker.characterData.baseInfo.waiterInfo.AddExp(2, out bool isLevelUp);
                if (isLevelUp)
                {
                    ToastForLevelUp(WorkerEnum.Waiter);
                }
                //放下食物
                orderForCustomer.foodCpt.transform.SetParent(orderForCustomer.table.GetTable().transform);
                orderForCustomer.foodCpt.transform.DOLocalMove(Vector3.zero, 0.2f);
                //orderForCustomer.foodCpt.transform.localPosition = Vector3.zero;
                //通知客人吃饭
                orderForCustomer.customer.SetIntent(NpcAICustomerCpt.CustomerIntentEnum.Eatting);
                //设置闲置
                SetIntent(WaiterIntentEnum.Idle);
            }
        }
        else
        {
            //删除食物
            if (orderForCustomer.foodCpt != null)
                Destroy(orderForCustomer.foodCpt.gameObject);
            SetIntent(WaiterIntentEnum.Idle);
        }
    }

    /// <summary>
    /// 处理-清理桌子
    /// </summary>
    public void HandleForGoToCleanTable()
    {
        if (npcAIWorker.characterMoveCpt.IsAutoMoveStop())
        {
            SetIntent(WaiterIntentEnum.CleaningTable, order);
        }
    }

    /// <summary>
    /// 处理-到达楼梯
    /// </summary>
    public void HandleForGoToStairsForFirst()
    {
        OrderForHotel orderForHotel = order as OrderForHotel;
        if (npcAIWorker.characterMoveCpt.IsAutoMoveStop())
        {
            transform.position = orderForHotel.layerSecondStairsPositionForClean;
            SetIntent(WaiterIntentEnum.GoToBed);
        }
    }

    /// <summary>
    /// 处理-到达楼梯
    /// </summary>
    public void HandleForGoToStairsForSecond()
    {
        OrderForHotel orderForHotel = order as OrderForHotel;
        if (npcAIWorker.characterMoveCpt.IsAutoMoveStop())
        {
            transform.position = orderForHotel.layerFirstStairsPositionForClean;
            SetIntent(WaiterIntentEnum.Idle);
        }
    }

    /// <summary>
    /// 处理-到达床
    /// </summary>
    public void HandleForGoToBed()
    {
        if (npcAIWorker.characterMoveCpt.IsAutoMoveStop())
        {
            SetIntent(WaiterIntentEnum.CleaningBed);
        }
    }

    /// <summary>
    /// 开始清理
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoroutineForCleanTable()
    {
        //AudioHandler.Instance.PlaySound(AudioSoundEnum.Clean);
        //计算清理时间
        float cleanTime = npcAIWorker.characterData.CalculationWaiterCleanTime();
        yield return new WaitForSeconds(cleanTime);
        OrderForCustomer orderForCustomer = order as OrderForCustomer;
        //记录数据
        npcAIWorker.characterData.baseInfo.waiterInfo.AddCleanTableNumber(1);
        //增加经验
        npcAIWorker.characterData.baseInfo.waiterInfo.AddExp(2, out bool isLevelUp);
        if (isLevelUp)
        {
            ToastForLevelUp(WorkerEnum.Waiter);
        }
        // 清理桌子
        orderForCustomer.table.CleanTable();
        //结束订单
        npcAIWorker.innHandler.EndOrder(orderForCustomer);
        SetIntent(WaiterIntentEnum.Idle);
    }

    public IEnumerator CoroutineForCleanBed()
    {
        //计算清理时间
        float cleanTime = npcAIWorker.characterData.CalculationWaiterCleanTime();
        yield return new WaitForSeconds(cleanTime);
        OrderForHotel orderForHotel = order as OrderForHotel;
        //记录数据
        npcAIWorker.characterData.baseInfo.waiterInfo.AddCleanBedNumber(1);
        //增加经验
        npcAIWorker.characterData.baseInfo.waiterInfo.AddExp(4, out bool isLevelUp);
        if (isLevelUp)
        {
            ToastForLevelUp(WorkerEnum.Waiter);
        }
        //清理
        orderForHotel.bed.CleanBed(); 

        //检测是否还有订单并且依旧没有取消改职业。如果有的话继续清理
        //用于中断连续清理
        CharacterWorkerForWaiterBean characterWorkerData = (CharacterWorkerForWaiterBean)npcAIWorker.characterData.baseInfo.GetWorkerInfoByType(WorkerEnum.Waiter);
        if (characterWorkerData.isWorkingCleanBed && npcAIWorker.innHandler.bedCleanQueue.Count != 0)
        {
            //搜寻最近的床位
            OrderForHotel clearItem = null;
            float distance = float.MaxValue;
            foreach (OrderForHotel itemOrder in npcAIWorker.innHandler.bedCleanQueue)
            {
                float tempDistance = Vector3.Distance(itemOrder.bed.GetSleepPosition(), transform.position);
                if (tempDistance < distance)
                {
                    distance = tempDistance;
                    clearItem = itemOrder;
                }
            }
            if (clearItem!=null)
            {
                npcAIWorker.innHandler.bedCleanQueue.Remove(clearItem);
                SetIntent(WaiterIntentEnum.GoToBed, clearItem);
            }
            else
            {
                SetIntent(WaiterIntentEnum.GoToStairsForSecond);
            }
          }
        else
        {
            SetIntent(WaiterIntentEnum.GoToStairsForSecond);
        }

    }


}