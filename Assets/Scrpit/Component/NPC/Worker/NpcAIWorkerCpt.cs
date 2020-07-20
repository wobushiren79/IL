using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Collections;

public class NpcAIWorkerCpt : BaseNpcAI
{
    public enum WorkerNotifyEnum
    {
        StatusChange = 1,//状态改变
    }

    public enum WorkerIntentEnum
    {
        Idle,//空闲
        Daze,//发呆
        WaiterSend,//跑堂
        WaiterClean,//清扫
        Cook,//做菜
        Accounting,//结账
        Accost,//招待
        Beater//打手
    }

    //厨师AI控制
    public NpcAIWorkerForChefCpt aiForChef;
    //跑堂AI控制
    public NpcAIWorkerForWaiterCpt aiForWaiter;
    //结账AI控制
    public NpcAIWorkerForAccountantCpt aiForAccountant;
    //招待AI控制
    public NpcAIWorkerForAccost aiForAccost;
    //打手AI控制
    public NpcAIWorkerForBeaterCpt aiForBeater;

    //客栈数据
    public InnHandler innHandler;
    //工作者的想法
    public WorkerIntentEnum workerIntent = WorkerIntentEnum.Idle;
    //工作信息
    public List<CharacterWorkerBaseBean> listWorkerInfo = new List<CharacterWorkerBaseBean>();

    private void FixedUpdate()
    {
        switch (workerIntent)
        {
            case WorkerIntentEnum.Idle:
                HandleForIdle();
                break;
        }
    }

    /// <summary>
    /// 获取工作者状态
    /// </summary>
    /// <param name="statusStr"></param>
    /// <returns></returns>
    public WorkerIntentEnum GetWorkerStatus(out string statusStr)
    {
        statusStr = "???";
        switch (workerIntent)
        {
            case WorkerIntentEnum.Idle:
                statusStr = GameCommonInfo.GetUITextById(171);
                break;
            case WorkerIntentEnum.Daze:
                statusStr = GameCommonInfo.GetUITextById(178);
                break;
            case WorkerIntentEnum.WaiterSend:
                statusStr = GameCommonInfo.GetUITextById(172);
                break;
            case WorkerIntentEnum.WaiterClean:
                statusStr = GameCommonInfo.GetUITextById(173);
                break;
            case WorkerIntentEnum.Cook:
                statusStr = GameCommonInfo.GetUITextById(174);
                break;
            case WorkerIntentEnum.Accounting:
                statusStr = GameCommonInfo.GetUITextById(175);
                break;
            case WorkerIntentEnum.Accost:
                statusStr = GameCommonInfo.GetUITextById(176);
                break;
            case WorkerIntentEnum.Beater:
                statusStr = GameCommonInfo.GetUITextById(177);
                break;
        }
        return workerIntent;
    }

    /// <summary>
    /// 设置角色数据
    /// </summary>
    /// <param name="characterBean"></param>
    public override void SetCharacterData(CharacterBean characterBean)
    {
        base.SetCharacterData(characterBean);
        InitWorkerInfo();
    }

    /// <summary>
    /// 通过优先级设置工作
    /// </summary>
    public bool SetWorkByPriority()
    {   
        //如果该工作者此时空闲
        if (workerIntent != WorkerIntentEnum.Idle)
        {
            return false;
        }
        foreach (CharacterWorkerBaseBean itemWorkerInfo in listWorkerInfo)
        {
            if (!itemWorkerInfo.isWorking)
                continue;
            bool isDistributionSuccess = innHandler.DistributionWorkForType(itemWorkerInfo.workerType, this);
            if (isDistributionSuccess)
                return true;
        }
        return false;
    }

    /// <summary>
    /// 初始工作信息
    /// </summary>
    public void InitWorkerInfo()
    {
        listWorkerInfo = characterData.baseInfo.GetAllWorkerInfo();
    }

    /// <summary>
    /// 处理-闲置
    /// </summary>
    public void HandleForIdle()
    {

    }

    /// <summary>
    /// 设置意图
    /// </summary>
    /// <param name="workerIntent"></param>
    /// <param name="orderForCustomer"></param>
    /// <param name="npcAIRascal"></param>
    public void SetIntent(WorkerIntentEnum workerIntent, OrderForCustomer orderForCustomer, NpcAIRascalCpt npcAIRascal)
    {
        StopAllCoroutines();
        RemoveStatusIconByType(CharacterStatusIconEnum.Pro);
        this.workerIntent = workerIntent;
        switch (workerIntent)
        {
            case WorkerIntentEnum.Idle:
                SetIntentForIdle();
                break;
            case WorkerIntentEnum.Daze:
                SetIntentForDaze();
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
                SetIntentForAccost();
                break;
            case WorkerIntentEnum.Beater:
                SetIntentForBeater(npcAIRascal);
                break;
        }
        NotifyAllObserver((int)WorkerNotifyEnum.StatusChange, (int)workerIntent);
    }

    public void SetIntent(WorkerIntentEnum workerIntent)
    {
        SetIntent(workerIntent, null, null);
    }
    public void SetIntent(WorkerIntentEnum workerIntent, OrderForCustomer orderForCustomer)
    {
        SetIntent(workerIntent, orderForCustomer, null);
    }
    public void SetIntent(WorkerIntentEnum workerIntent, NpcAIRascalCpt npcAIRascal)
    {
        SetIntent(workerIntent, null, npcAIRascal);
    }

    /// <summary>
    /// 设置闲置
    /// </summary>
    public void SetIntentForIdle()
    {
        //有一定概率发呆
        if (characterData.CalculationWorkerDaze(gameItemsManager, gameDataManager))
        {
            SetIntent(WorkerIntentEnum.Daze);
        }
        else
        {
            //闲逛或者待在原地
            int action = UnityEngine.Random.Range(0, 2);
            if (action == 0)
            {
                //闲逛
                Vector3 movePosition = innHandler.GetRandomInnPositon();
                SetCharacterMove(movePosition);
            }
            StartCoroutine(CoroutineForIdle());
        }
    }


    /// <summary>
    /// 意图 -发呆
    /// </summary>
    public void SetIntentForDaze()
    {
        float dazeTime = UnityEngine.Random.Range(10f, 30f);
        StartCoroutine(CoroutineForDaze(dazeTime));
    }

    /// <summary>
    /// 设置料理
    /// </summary>
    public void SetIntentForCook(OrderForCustomer orderForCustomer)
    {
        aiForChef.StartCook(orderForCustomer);
    }

    /// <summary>
    /// 设置跑堂
    /// </summary>
    /// <param name="stoveCpt"></param>
    public void SetIntentForWaiterSend(OrderForCustomer orderForCustomer)
    {
        aiForWaiter.StartFoodSend(orderForCustomer);
    }

    /// <summary>
    /// 设置清理
    /// </summary>
    /// <param name="stoveCpt"></param>
    public void SetIntentForWaiterClear(OrderForCustomer orderForCustomer)
    {
        aiForWaiter.StartFoodClean(orderForCustomer);
    }

    /// <summary>
    /// 设置结账
    /// </summary>
    /// <param name="customerCpt"></param>
    public void SetIntentForAccounting(OrderForCustomer orderForCustomer)
    {
        aiForAccountant.StartAccounting(orderForCustomer);
    }

    /// <summary>
    /// 设置招待
    /// </summary>
    public void SetIntentForAccost()
    {
        aiForAccost.StartAccost();
    }

    /// <summary>
    /// 设置打手工作
    /// </summary>
    public void SetIntentForBeater(NpcAIRascalCpt npcAIRascal)
    {
        aiForBeater.StartFight(npcAIRascal);
    }

    /// <summary>
    /// 协程 发呆
    /// </summary>
    /// <param name="dazeTime"></param>
    /// <returns></returns>
    public IEnumerator CoroutineForDaze(float dazeTime)
    {
        Sprite spDaze = iconDataManager.GetIconSpriteByName("daze_1");
        string markId = SystemUtil.GetUUID(SystemUtil.UUIDTypeEnum.N);
        AddStatusIconForPro(spDaze, null, markId);
        yield return new WaitForSeconds(dazeTime);
        SetIntent(WorkerIntentEnum.Idle);
    }

    /// <summary>
    /// 协程 闲置
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoroutineForIdle()
    {
        yield return new WaitForSeconds(5);
        SetIntent(WorkerIntentEnum.Idle);
    }
}