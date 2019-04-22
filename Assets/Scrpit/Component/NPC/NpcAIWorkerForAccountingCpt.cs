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
                if (Vector2.Distance(transform.position, customerCpt.counterCpt.GetAccountingPosition()) < 0.1f)
                {
                    accountingStatue = AccountingStatue.Accounting;
                    StartCoroutine(StartAccounting());
                }
                break;
        }
    }

    public void SetAccounting(NpcAICustomerCpt customerCpt)
    {
        this.customerCpt = customerCpt;
        accountingStatue = AccountingStatue.GoToAccounting;
        mNpcAIWorker.characterMoveCpt.SetDestination(customerCpt.counterCpt.GetAccountingPosition());
        accountingPro.SetActive(true);
    }

    public IEnumerator StartAccounting()
    {
        yield return new WaitForSeconds(5);
        accountingStatue = AccountingStatue.Idle;
        mNpcAIWorker.workerIntent = NpcAIWorkerCpt.WorkerIntentEnum.Idle;
        customerCpt.counterCpt.workerCpt = null;
        customerCpt.SetDestinationByIntent(NpcAICustomerCpt.CustomerIntentEnum.Leave);
        accountingPro.SetActive(false);
    }
}