using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
public class NpcAIWorkerForAccountingCpt : BaseMonoBehaviour
{
    private NpcAIWorkerCpt mNpcAIWorker;
    public NpcAICustomerCpt customerCpt;

    //算账进度
    public GameObject  accountingPro;
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
    public AccountingStatue accountingStatue= AccountingStatue.Idle;

    private void FixedUpdate()
    {
        switch (accountingStatue)
        {
            case AccountingStatue.GoToAccounting:
                if (!CheckCustomerLeave()&&Vector2.Distance(transform.position, customerCpt.counterCpt.GetAccountingPosition()) < 0.1f)
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
        if (customerCpt == null || customerCpt.intentType == NpcAICustomerCpt.CustomerIntentEnum.Leave)
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

    public void SetAccounting(NpcAICustomerCpt customerCpt)
    {
        if (CheckUtil.CheckPath(transform.position, customerCpt.counterCpt.GetAccountingPosition())) {
            this.customerCpt = customerCpt;
            accountingStatue = AccountingStatue.GoToAccounting;
            mNpcAIWorker.characterMoveCpt.SetDestination(customerCpt.counterCpt.GetAccountingPosition());
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
        SetStatusIdle();
        customerCpt.SetDestinationByIntent(NpcAICustomerCpt.CustomerIntentEnum.Leave);
   
    }

    public void SetStatusIdle()
    {
        accountingStatue = AccountingStatue.Idle;
        mNpcAIWorker.workerIntent = NpcAIWorkerCpt.WorkerIntentEnum.Idle;
        if (customerCpt != null&& customerCpt.counterCpt!=null)
        customerCpt.counterCpt.workerCpt = null;
        accountingPro.SetActive(false);
    }
}