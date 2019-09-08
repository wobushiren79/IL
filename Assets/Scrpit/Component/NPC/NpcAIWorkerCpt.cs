using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class NpcAIWorkerCpt : BaseNpcAI
{
    public enum WorkerIntentEnum
    {
        Idle,//空闲
        WaiterSend,//跑堂
        WaiterClean,//清扫
        Cook,//做菜
        Accounting,//结账
        Accost,//招待
        Beater//打手
    }
    //呼喊
    public CharacterShoutCpt characterShoutCpt;

    //厨师AI控制
    public NpcAIWorkerForChefCpt aiForChef;
    //跑堂AI控制
    public NpcAIWorkerForWaiterCpt aiForWaiter;
    //结账AI控制
    public NpcAIWorkerForAccountingCpt aiForAccounting;

    //客栈数据
    public InnHandler innHandler;
    //游戏数据
    public GameDataManager gameDataManager;
    //工作者的想法
    public WorkerIntentEnum workerIntent = WorkerIntentEnum.Idle;
    //工作信息
    public List<WorkerInfo> listWorkerInfo = new List<WorkerInfo>();

    private void FixedUpdate()
    {
        switch (workerIntent)
        {
            case WorkerIntentEnum.Idle:
                //TODO  瞎逛
                break;
        }
    }

    public override void SetCharacterData(CharacterBean characterBean)
    {
        base.SetCharacterData(characterBean);
        InitWorkerInfo();
    }

    /// <summary>
    /// 通过优先级设置工作
    /// </summary>
    public void SetWorkByPriority()
    {
        foreach (WorkerInfo itemWorkerInfo in listWorkerInfo)
        {
            if (!itemWorkerInfo.isWork)
                continue;
            bool isDistributionSuccess=innHandler.DistributionWorkForType(itemWorkerInfo.worker,this);
            if (isDistributionSuccess)
                return;
        }
    }

    /// <summary>
    /// 初始工作信息
    /// </summary>
    public void InitWorkerInfo()
    {
        listWorkerInfo = characterData.baseInfo.GetAllWorkerInfo();
    }


    /// <summary>
    /// 设置意图
    /// </summary>
    /// <param name="workerIntent"></param>
    /// <param name="orderForCustomer"></param>
    public void SetIntent(WorkerIntentEnum workerIntent, OrderForCustomer orderForCustomer)
    {
        this.workerIntent = workerIntent;
        switch (workerIntent)
        {
            case WorkerIntentEnum.Idle:
                SetIntentForIdle();
                break;
            case WorkerIntentEnum.Cook:
                SetIntentForCook(orderForCustomer);
                break;
            case WorkerIntentEnum.WaiterSend:
                SetIntentForWaiterSend(orderForCustomer);
                break;
            case WorkerIntentEnum.WaiterClean:
                SetIntentForWaiterClear(orderForCustomer);
                break;
            case WorkerIntentEnum.Accounting:
                SetIntentForAccounting(orderForCustomer);
                break;
            case WorkerIntentEnum.Accost:
                break;
            case WorkerIntentEnum.Beater:
                break;
        }
    }

    public void SetIntent(WorkerIntentEnum workerIntent)
    {
        SetIntent(workerIntent, null);
    }

    /// <summary>
    /// 设置闲置
    /// </summary>
    private void SetIntentForIdle()
    {

    }

    /// <summary>
    /// 设置料理
    /// </summary>
    public void SetIntentForCook(OrderForCustomer orderForCustomer)
    {
        aiForChef.SetCook(orderForCustomer);
    }

    /// <summary>
    /// 设置跑堂
    /// </summary>
    /// <param name="stoveCpt"></param>
    public void SetIntentForWaiterSend(OrderForCustomer orderForCustomer)
    {
        aiForWaiter.SetFoodSend(orderForCustomer);
    }

    /// <summary>
    /// 设置清理
    /// </summary>
    /// <param name="stoveCpt"></param>
    public void SetIntentForWaiterClear(OrderForCustomer orderForCustomer)
    {
        aiForWaiter.SetFoodClean(orderForCustomer);
    }

    /// <summary>
    /// 设置结账
    /// </summary>
    /// <param name="customerCpt"></param>
    public void SetIntentForAccounting(OrderForCustomer orderForCustomer)
    {
        aiForAccounting.SetAccounting(orderForCustomer);
    }

}