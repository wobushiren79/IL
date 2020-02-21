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
        GoToClear,//清理的路上
        Cleaning,//清理中
    }

    public GameObject objTake;
    //订单
    public OrderForCustomer orderForCustomer;
    //送菜的进度图标
    public GameObject sendPro;
    //清理进度
    public GameObject clearPro;
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
                if (orderForCustomer.CheckOrder())
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
                break;
            case WaiterIntentEnum.SendFood:
                if (orderForCustomer.CheckOrder())
                {
                    if (npcAIWorker.characterMoveCpt.IsAutoMoveStop())
                    {
                        //记录数据
                        npcAIWorker.characterData.baseInfo.waiterInfo.AddSendNumber(1);
                        //增加经验
                        npcAIWorker.characterData.baseInfo.waiterInfo.AddExp(1);
                        //放下食物
                        orderForCustomer.foodCpt.transform.SetParent(orderForCustomer.table.GetTable().transform);
                        orderForCustomer.foodCpt.transform.DOLocalMove(Vector3.zero,0.2f);
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
                break;
            case WaiterIntentEnum.GoToClear:
                if (npcAIWorker.characterMoveCpt.IsAutoMoveStop())
                {
                    SetIntent(WaiterIntentEnum.Cleaning, orderForCustomer);
                }
                break;
            case WaiterIntentEnum.Cleaning:

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
        SetIntent(WaiterIntentEnum.GoToClear, orderForCustomer);
    }

    /// <summary>
    /// 设置意图
    /// </summary>
    /// <param name="waiterIntent"></param>
    /// <param name="orderForCustomer"></param>
    public void SetIntent(WaiterIntentEnum waiterIntent, OrderForCustomer orderForCustomer)
    {
        this.waiterIntent = waiterIntent;
        this.orderForCustomer = orderForCustomer;
        switch (waiterIntent)
        {
            case WaiterIntentEnum.Idle:
                SetIntentForIdle();
                break;
            case WaiterIntentEnum.GoToGetFood:
                SetIntentForGoToGetFood(orderForCustomer);
                break;
            case WaiterIntentEnum.SendFood:
                SetIntentForSendFood(orderForCustomer);
                break;
            case WaiterIntentEnum.GoToClear:
                SetIntentForGoToClear(orderForCustomer);
                break;
            case WaiterIntentEnum.Cleaning:
                SetIntentForCleaning(orderForCustomer);
                break;
        }
    }

    public void SetIntent(WaiterIntentEnum waiterIntent)
    {
        SetIntent(waiterIntent, orderForCustomer);
    }

    /// <summary>
    /// 意图-闲置
    /// </summary>
    public void SetIntentForIdle()
    {
        StopAllCoroutines();
        sendPro.SetActive(false);
        clearPro.SetActive(false);
        npcAIWorker.SetIntent(NpcAIWorkerCpt.WorkerIntentEnum.Idle);
        orderForCustomer = null;
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
        orderForCustomer.foodCpt.transform.SetParent(objTake.transform);
        orderForCustomer.foodCpt.transform.DOLocalMove(Vector3.zero, 0.2f);
        //orderForCustomer.foodCpt.transform.localPosition = Vector3.zero;
        movePosition = orderForCustomer.table.GetTablePosition();
        npcAIWorker.characterMoveCpt.SetDestination(movePosition);
    }

    /// <summary>
    /// 意图-前去清理
    /// </summary>
    /// <param name="orderForCustomer"></param>
    public void SetIntentForGoToClear(OrderForCustomer orderForCustomer)
    {
        clearPro.SetActive(true);
        movePosition = orderForCustomer.foodCpt.transform.position;
        npcAIWorker.characterMoveCpt.SetDestination(movePosition);
    }

    /// <summary>
    /// 意图-清理中
    /// </summary>
    /// <param name="orderForCustomer"></param>
    public void SetIntentForCleaning(OrderForCustomer orderForCustomer)
    {
        orderForCustomer.table.SetTableStatus(BuildTableCpt.TableStatusEnum.Cleaning);
        clearPro.SetActive(true);
        StartCoroutine(StartClean());
    }

    /// <summary>
    /// 开始清理
    /// </summary>
    /// <returns></returns>
    public IEnumerator StartClean()
    {
        float cleanTime = npcAIWorker.characterData.CalculationWaiterCleanTime(gameItemsManager);
        npcAIWorker.characterData.baseInfo.waiterInfo.AddCleanTime(cleanTime);
        yield return new WaitForSeconds(cleanTime);
        //记录数据
        npcAIWorker.characterData.baseInfo.waiterInfo.AddCleanNumber(1);
        //增加经验
        npcAIWorker.characterData.baseInfo.waiterInfo.AddExp(1);
        // 清理桌子
        orderForCustomer.table.CleanTable();
        //结束订单
        npcAIWorker.innHandler.EndOrder(orderForCustomer);
        SetIntent(WaiterIntentEnum.Idle);
    }


}