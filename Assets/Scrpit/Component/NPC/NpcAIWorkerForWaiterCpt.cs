using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using static NpcAICustomerCpt;

public class NpcAIWorkerForWaiterCpt : BaseMonoBehaviour
{

    private NpcAIWorkerCpt mNpcAIWorker;

    public OrderForCustomer orderForCustomer;
    //送菜的进度图标
    public GameObject sendPro;
    //清理进度
    public GameObject clearPro;

    public enum WaiterStatue
    {
        Idle,//空闲
        GoToGetFood,//获取食物的路上
        SendFood,//运送食物中
        GoToClear,//清理的路上
        Clear,//清理中
    }

    //服务员状态
    public WaiterStatue waiterStatue = WaiterStatue.Idle;
    
    private void Start()
    {
        mNpcAIWorker = GetComponent<NpcAIWorkerCpt>();
    }

    private void FixedUpdate()
    {
        switch (waiterStatue)
        {
            case WaiterStatue.GoToGetFood:

                if (!CheckCustomerLeave() && mNpcAIWorker.characterMoveCpt.IsAutoMoveStop())
                {
                    Transform waitTake = CptUtil.GetCptInChildrenByName<Transform>(gameObject, "Take");
                    orderForCustomer.foodCpt.transform.SetParent(waitTake);
                    orderForCustomer.foodCpt.transform.localScale = new Vector3(0.5f, 0.5f, 1);
                    orderForCustomer.foodCpt.transform.localPosition = new Vector3(0, 0.1f, 0);
                   
                    mNpcAIWorker.characterMoveCpt.SetDestination(orderForCustomer.table.GetTablePosition());
                    waiterStatue = WaiterStatue.SendFood;
                    sendPro.SetActive(true);
                }
                break;
            case WaiterStatue.SendFood:
                if (CheckCustomerLeave())
                {
                    return;
                }
                if (mNpcAIWorker.characterMoveCpt.IsAutoMoveStop())
                {
                    orderForCustomer.foodCpt.transform.SetParent(orderForCustomer.table.GetTable().transform);
                    orderForCustomer.foodCpt.transform.localPosition = new Vector3(0f, 0.1f, 0);
                    orderForCustomer.foodCpt.transform.localScale = new Vector3(1, 1, 1);
                    waiterStatue = WaiterStatue.Idle;
                    mNpcAIWorker.workerIntent = NpcAIWorkerCpt.WorkerIntentEnum.Idle;
                    sendPro.SetActive(false);
                    orderForCustomer.customer.SetIntent(CustomerIntentEnum.Eatting);
                }
                break;
            case WaiterStatue.GoToClear:
                if (mNpcAIWorker.characterMoveCpt.IsAutoMoveStop())
                {
                    waiterStatue = WaiterStatue.Clear;
                    clearPro.SetActive(true);
                    orderForCustomer.table.SetTableStatus(BuildTableCpt.TableStateEnum.Cleaning);
                    StartCoroutine(StartClear());
                }
                break;
        }
    }

    /// <summary>
    /// 检测顾客是否离开
    /// </summary>
    /// <returns></returns>
    public bool CheckCustomerLeave()
    {
        if (orderForCustomer.customer == null || orderForCustomer.customer.intentType == NpcAICustomerCpt.CustomerIntentEnum.Leave)
        {
            Destroy(orderForCustomer.foodCpt.gameObject);
            SetStatusIdle();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetFoodSend(OrderForCustomer orderForCustomer)
    {
        if (CheckUtil.CheckPath(transform.position, orderForCustomer.stove.GetTakeFoodPosition()))
        {
            this.orderForCustomer = orderForCustomer;
            mNpcAIWorker.characterMoveCpt.SetDestination(orderForCustomer.stove.GetTakeFoodPosition());
            waiterStatue = WaiterStatue.GoToGetFood;
            sendPro.SetActive(true);
        }
        else
        {
            SetStatusIdle();
        }

    }

    public void SetFoodClear(OrderForCustomer orderForCustomer)
    {
        this.orderForCustomer = orderForCustomer;
        mNpcAIWorker.characterMoveCpt.SetDestination(orderForCustomer.foodCpt.transform.position);
        waiterStatue = WaiterStatue.GoToClear;
        clearPro.SetActive(true);
    }

    public IEnumerator StartClear()
    {
        yield return new WaitForSeconds(5);
        this.orderForCustomer.table.ClearTable();
        SetStatusIdle();
    }

    /// <summary>
    /// 设置当前员工无事可做
    /// </summary>
    public void SetStatusIdle()
    {
        waiterStatue = WaiterStatue.Idle;
        mNpcAIWorker.workerIntent = NpcAIWorkerCpt.WorkerIntentEnum.Idle;
        sendPro.SetActive(false);
        clearPro.SetActive(false);
    }
}