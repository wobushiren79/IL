using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
public class NpcAIWorkerForAccountingCpt : BaseMonoBehaviour
{
    private NpcAIWorkerCpt mNpcAIWorker;
    public OrderForCustomer orderForCustomer;
    //客栈处理
    public InnHandler innHandler;
    
    //算账进度
    public GameObject accountingPro;
    private void Start()
    {
        mNpcAIWorker = GetComponent<NpcAIWorkerCpt>();
    }

    public enum AccountingStatue
    {
        Idle,//空闲
        GoToAccounting,//结账之前的路上
        Accounting,//结账中
    }

    //厨师状态
    public AccountingStatue accountingStatue = AccountingStatue.Idle;

    private void FixedUpdate()
    {
        switch (accountingStatue)
        {
            case AccountingStatue.GoToAccounting:
                if (!CheckCustomerLeave() && mNpcAIWorker.characterMoveCpt.IsAutoMoveStop())
                {
                    accountingStatue = AccountingStatue.Accounting;
                    StartCoroutine(StartAccounting());
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
            StopAllCoroutines();
            SetStatusIdle();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetAccounting(OrderForCustomer  orderForCustomer)
    {
        if (CheckUtil.CheckPath(transform.position, orderForCustomer.counter.GetAccountingPosition()))
        {
            this.orderForCustomer = orderForCustomer;
            accountingStatue = AccountingStatue.GoToAccounting;
            mNpcAIWorker.characterMoveCpt.SetDestination(orderForCustomer.counter.GetAccountingPosition());
            accountingPro.SetActive(true);
        }
        else
        {
            SetStatusIdle();
        }

    }

    public IEnumerator StartAccounting()
    {
        yield return new WaitForSeconds(5);
        if (innHandler != null)
            innHandler.PayMoney(orderForCustomer,1);
        orderForCustomer.customer.SetIntent(NpcAICustomerCpt.CustomerIntentEnum.Leave);
        SetStatusIdle();
    }

    public void SetStatusIdle()
    {
        accountingStatue = AccountingStatue.Idle;
        mNpcAIWorker.workerIntent = NpcAIWorkerCpt.WorkerIntentEnum.Idle;
        if (orderForCustomer != null && orderForCustomer.counter != null)
            orderForCustomer.counter.workerCpt = null;
        accountingPro.SetActive(false);
    }
}