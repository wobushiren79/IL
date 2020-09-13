using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Collections;
using Pathfinding;

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
        WaiterBed,//理床
        Cook,//做菜
        Accounting,//结账
        AccostSolicit,//招待
        AccostGuide,//引导
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
    //所有的工作类型
    public List<WorkerDetilsEnum> listWorkerDetailsType = new List<WorkerDetilsEnum>();
    //是否开启偷懒
    public bool dazeEnabled = true;
    //偷懒缓冲时间
    public float dazeBufferTime = 0;


    public override void Awake()
    {
        base.Awake();
        innHandler = Find<InnHandler>(ImportantTypeEnum.InnHandler);
        listWorkerDetailsType = EnumUtil.GetEnumValue<WorkerDetilsEnum>();
    }

    private void FixedUpdate()
    {
        switch (workerIntent)
        {
            case WorkerIntentEnum.Idle:
                HandleForIdle();
                break;
        }

        if (dazeBufferTime > 0)
        {
            dazeBufferTime -= Time.deltaTime;
        }
    }

    /// <summary>
    /// 设置偷懒开关
    /// </summary>
    /// <param name="isOpenDaze"></param>
    public void SetDazeEnabled(bool dazeEnabled)
    {
        this.dazeEnabled = dazeEnabled;
    }

    /// <summary>
    /// 设置偷懒缓冲时间
    /// </summary>
    /// <param name="bufferTime"></param>
    public void SetDazeBufferTime(float bufferTime)
    {
        dazeBufferTime = bufferTime;
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
            case WorkerIntentEnum.WaiterBed:
                statusStr = GameCommonInfo.GetUITextById(180);
                break;
            case WorkerIntentEnum.Cook:
                statusStr = GameCommonInfo.GetUITextById(174);
                break;
            case WorkerIntentEnum.Accounting:
                statusStr = GameCommonInfo.GetUITextById(175);
                break;
            case WorkerIntentEnum.AccostSolicit:
                statusStr = GameCommonInfo.GetUITextById(176);
                break;
            case WorkerIntentEnum.AccostGuide:
                statusStr = GameCommonInfo.GetUITextById(179);
                break;
            case WorkerIntentEnum.Beater:
                statusStr = GameCommonInfo.GetUITextById(177);
                break;
        }
        return workerIntent;
    }

    public WorkerIntentEnum GetWorkerStatus()
    {
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

        for (int i = 0; i < listWorkerDetailsType.Count; i++)
        {
            WorkerDetilsEnum workerDetils = listWorkerDetailsType[i];
            CharacterWorkerBaseBean characterWorker;
            switch (workerDetils)
            {
                case WorkerDetilsEnum.ChefForCook:
                    characterWorker =  characterData.baseInfo.GetWorkerInfoByType( WorkerEnum.Chef);
                    if (!characterWorker.isWorking)
                        continue;
                    break;
                case WorkerDetilsEnum.WaiterForSend:
                    characterWorker = characterData.baseInfo.GetWorkerInfoByType(WorkerEnum.Waiter);
                    if (!((CharacterWorkerForWaiterBean)characterWorker).isWorkingForSend)
                        continue;
                    break;
                case WorkerDetilsEnum.WaiterForCleanTable:
                    characterWorker = characterData.baseInfo.GetWorkerInfoByType(WorkerEnum.Waiter);
                    if (!((CharacterWorkerForWaiterBean)characterWorker).isWorkingForCleanTable)
                        continue;
                    break;
                case WorkerDetilsEnum.WaiterForCleanBed:
                    characterWorker = characterData.baseInfo.GetWorkerInfoByType(WorkerEnum.Waiter);
                    if (!((CharacterWorkerForWaiterBean)characterWorker).isWorkingCleanBed)
                        continue;
                    break;
                case WorkerDetilsEnum.AccountantForPay:
                    characterWorker = characterData.baseInfo.GetWorkerInfoByType(WorkerEnum.Accountant);
                    if (!characterWorker.isWorking)
                        continue;
                    break;
                case WorkerDetilsEnum.AccostForSolicit:
                    characterWorker = characterData.baseInfo.GetWorkerInfoByType(WorkerEnum.Accost);
                    if (!((CharacterWorkerForAccostBean)characterWorker).isWorkingForSolicit)
                        continue;
                    break;
                case WorkerDetilsEnum.AccostForGuide:
                    characterWorker = characterData.baseInfo.GetWorkerInfoByType(WorkerEnum.Accost);
                    if (!((CharacterWorkerForAccostBean)characterWorker).isWorkingForGuide)
                        continue;
                    break;
                case WorkerDetilsEnum.BeaterForDrive:
                    characterWorker = characterData.baseInfo.GetWorkerInfoByType(WorkerEnum.Beater);
                    if (!characterWorker.isWorking)
                        continue;
                    break;
            }

            bool isDistributionSuccess = innHandler.DistributionWorkForType(workerDetils, this);
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
    public void SetIntent(WorkerIntentEnum workerIntent, OrderForCustomer orderForCustomer, NpcAIRascalCpt npcAIRascal, OrderForHotel orderForHotel)
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
                SetIntentForWaiterCleanTable(orderForCustomer);
                break;
            case WorkerIntentEnum.WaiterBed:
                SetIntentForWaiterCleanBed(orderForHotel);
                break;
            case WorkerIntentEnum.Accounting:
                if (orderForCustomer!=null)
                    SetIntentForAccounting(orderForCustomer);
                else if (orderForHotel!=null)
                    SetIntentForAccounting(orderForHotel);     
                break;
            case WorkerIntentEnum.AccostSolicit:
                SetIntentForAccostSolicit();
                break;
            case WorkerIntentEnum.AccostGuide:
                SetIntentForAccostGuide(orderForHotel);
                break;
            case WorkerIntentEnum.Beater:
                SetIntentForBeater(npcAIRascal);
                break;
        }
        NotifyAllObserver((int)WorkerNotifyEnum.StatusChange, (int)workerIntent);
    }

    public void SetIntent(WorkerIntentEnum workerIntent, OrderForCustomer orderForCustomer, NpcAIRascalCpt npcAIRascal)
    {
        SetIntent(workerIntent, orderForCustomer, npcAIRascal,null);
    }

    public void SetIntent(WorkerIntentEnum workerIntent)
    {
        SetIntent(workerIntent, null, null);
    }
    public void SetIntent(WorkerIntentEnum workerIntent, OrderForCustomer orderForCustomer)
    {
        SetIntent(workerIntent, orderForCustomer, null);
    }
    public void SetIntent(WorkerIntentEnum workerIntent, OrderForHotel orderForHotel)
    {
        SetIntent(workerIntent, null, null, orderForHotel);
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
        if (dazeEnabled && dazeBufferTime <= 0 && characterData.CalculationWorkerDaze(gameItemsManager, gameDataManager))
        {
            SetIntent(WorkerIntentEnum.Daze);
        }
        else
        {
            //闲逛或者待在原地
            int action = UnityEngine.Random.Range(0, 5);
            if (action == 0)
            {
                //闲逛 有问题
                Vector3 movePosition = innHandler.GetRandomInnPositon();
                bool canGo = CheckUtil.CheckPath(transform.position, movePosition);
                if (canGo)
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
        characterData.baseInfo.AddDazeNumber(1);
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
    public void SetIntentForWaiterCleanTable(OrderForCustomer orderForCustomer)
    {
        aiForWaiter.StartFoodClean(orderForCustomer);
    }

    /// <summary>
    /// 设置理床
    /// </summary>
    /// <param name="orderForHotel"></param>
    public void SetIntentForWaiterCleanBed(OrderForHotel orderForHotel)
    {
        aiForWaiter.StartBedClean(orderForHotel);
    }

    /// <summary>
    /// 设置结账
    /// </summary>
    /// <param name="customerCpt"></param>
    public void SetIntentForAccounting(OrderForBase order)
    {
        aiForAccountant.StartAccounting(order);
    }

    /// <summary>
    /// 设置招待
    /// </summary>
    public void SetIntentForAccostSolicit()
    {
        aiForAccost.StartAccostSolicit();
    }

    /// <summary>
    /// 设置引路
    /// </summary>
    public void SetIntentForAccostGuide(OrderForHotel orderForHotel)
    {
        aiForAccost.StartAccostGuide(orderForHotel);
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