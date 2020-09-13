using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
public class NpcAIWorkerForAccountantCpt : NpcAIWokerFoBaseCpt
{
    public enum AccountantIntentEnum
    {
        Idle,//空闲
        GoToAccounting,//结账之前的路上
        Accounting,//结账中
    }
    //点单
    public OrderForBase order;

    //算账进度
    public GameObject accountingPro;
    //移动的目的点
    public Vector3 movePosition;
    //户籍
    public AccountantIntentEnum accountantIntent = AccountantIntentEnum.Idle;

    private void Update()
    {
        switch (accountantIntent)
        {
            case AccountantIntentEnum.Idle:
                break;
            case AccountantIntentEnum.GoToAccounting:
                if (order.CheckOrder())
                {
                    if (npcAIWorker.characterMoveCpt.IsAutoMoveStop())
                    {
                        //设置朝向
                        npcAIWorker.SetCharacterFace(order.counter.GetUserFace());
                        SetIntent(AccountantIntentEnum.Accounting);
                    }
                }
                else
                {
                    SetIntent(AccountantIntentEnum.Idle);
                }
                break;
            case AccountantIntentEnum.Accounting:
                if (!order.CheckOrder())
                {
                    SetIntent(AccountantIntentEnum.Idle);
                }
                break;
        }
    }

    public void StartAccounting(OrderForBase order)
    {
        SetIntent(AccountantIntentEnum.GoToAccounting, order);
    }

    /// <summary>
    /// 设置意图
    /// </summary>
    /// <param name="accountingIntent"></param>
    /// <param name="order"></param>
    public void SetIntent(AccountantIntentEnum accountingIntent, OrderForBase order)
    {
        StopAllCoroutines();
        this.accountantIntent = accountingIntent;
        this.order = order;
        switch (accountingIntent)
        {
            case AccountantIntentEnum.Idle:
                SetIntentForIdle();
                break;
            case AccountantIntentEnum.GoToAccounting:
                SetIntentForGoToAccounting(order);
                break;
            case AccountantIntentEnum.Accounting:
                SetIntentForAccounting(order);
                break;
        }
    }
    public void SetIntent(AccountantIntentEnum accountingIntent)
    {
        SetIntent(accountingIntent, order);
    }

    /// <summary>
    /// 意图-闲置
    /// </summary>
    public void SetIntentForIdle()
    {
        accountingPro.SetActive(false);
        if (order != null && order.counter != null)
            order.counter.SetCounterStatus(BuildCounterCpt.CounterStatusEnum.Idle);
        order = null;
        npcAIWorker.SetIntent(NpcAIWorkerCpt.WorkerIntentEnum.Idle);
    }

    /// <summary>
    /// 意图-前往结算
    /// </summary>
    /// <param name="order"></param>
    public void SetIntentForGoToAccounting(OrderForBase order)
    {
        if (CheckUtil.CheckPath(transform.position, order.counter.GetAccountingPosition()))
        {
            accountingPro.SetActive(true);
            movePosition = order.counter.GetAccountingPosition();
            npcAIWorker.characterMoveCpt.SetDestination(movePosition);
        }
        else
        {
            SetIntent(AccountantIntentEnum.Idle);
        }
    }

    /// <summary>
    /// 意图-结算中
    /// </summary>
    /// <param name="order"></param>
    public void SetIntentForAccounting(OrderForBase order)
    {
        //设置柜台的状态
        order.counter.SetCounterStatus(BuildCounterCpt.CounterStatusEnum.Accounting);
        if (order as OrderForCustomer != null)
        {      
            //开始结算
            StartCoroutine(CoroutineForAccountingForFood(order as OrderForCustomer));
        }
        else if (order as OrderForHotel != null)
        {
            //开始结算
            StartCoroutine(CoroutineForAccountingForBed(order as OrderForHotel));
        }
    }

    /// <summary>
    /// 开始结算
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoroutineForAccountingForFood(OrderForCustomer orderForCustomer)
    {
        float time = npcAIWorker.characterData.CalculationAccountingTime(gameItemsManager);
        yield return new WaitForSeconds(time);
        MenuOwnBean menuOwn = gameDataManager.gameData.GetMenuById(orderForCustomer.foodData.id);
        menuOwn.GetPrice(orderForCustomer.foodData, out long payMoneyL, out long payMoneyM, out long payMoneyS);
        //是否出错
        bool isError = npcAIWorker.characterData.CalculationAccountingCheck(gameItemsManager, out float moreRate);

        long AddMoneyL = (long)(moreRate * payMoneyL);
        long AddMoneyM = (long)(moreRate * payMoneyM);
        long AddMoneyS = (long)(moreRate * payMoneyS);

        if (isError)
        {
            //出错
            //记录数据
            payMoneyL -= AddMoneyL;
            payMoneyM -= AddMoneyM;
            payMoneyS -= AddMoneyS;

            npcAIWorker.characterData.baseInfo.accountantInfo.AddAccountantFail
                (
                payMoneyL, payMoneyM, payMoneyS,
                AddMoneyL, AddMoneyM, AddMoneyS
                );
            //增加经验
            npcAIWorker.characterData.baseInfo.accountantInfo.AddExp(1,out bool isLevelUp);
            if (isLevelUp)
            {
                ToastForLevelUp(WorkerEnum.Accountant);
            }

            //工作者表示抱歉
            npcAIWorker.SetExpression(CharacterExpressionCpt.CharacterExpressionEnum.Wordless);
            //顾客生气
            orderForCustomer.customer.SetExpression(CharacterExpressionCpt.CharacterExpressionEnum.Mad);
        }
        else
        {
            //成功
            payMoneyL += AddMoneyL;
            payMoneyM += AddMoneyM;
            payMoneyS += AddMoneyS;
            //记录数据
            npcAIWorker.characterData.baseInfo.accountantInfo.AddAccountantSuccess
                (
                payMoneyL, payMoneyM, payMoneyS,
                AddMoneyL, AddMoneyM, AddMoneyS
                );
            //增加经验
            npcAIWorker.characterData.baseInfo.accountantInfo.AddExp(1, out bool isLevelUp);
            if (isLevelUp)
            {
                ToastForLevelUp(WorkerEnum.Accountant);
            }
            //如果有额外的加成 工作者和店员都应该高兴
            //orderForCustomer.customer.SetExpression(CharacterExpressionCpt.CharacterExpressionEnum.Love);
        }

        if (npcAIWorker.innHandler != null)
        {
            npcAIWorker.innHandler.PayMoney(orderForCustomer, payMoneyL, payMoneyM, payMoneyS,true);
            //结束订单
            npcAIWorker.innHandler.EndOrder(orderForCustomer);
        }

        //通知离开
        orderForCustomer.customer.SetIntent(NpcAICustomerCpt.CustomerIntentEnum.Leave);

        //检测该柜台是否还有订单并且依旧没有取消改职业。
        //用于中断连续结账
        CharacterWorkerBaseBean characterWorkerData = npcAIWorker.characterData.baseInfo.GetWorkerInfoByType( WorkerEnum.Accountant);
        if (characterWorkerData.isWorking && orderForCustomer.counter.payQueue.Count != 0)
        {
            OrderForBase newOrder =orderForCustomer.counter.payQueue[0];
            orderForCustomer.counter.payQueue.Remove(newOrder);
            StartAccounting(newOrder);
        }
        else
        {
            SetIntent(AccountantIntentEnum.Idle);
        }
    }


    public IEnumerator CoroutineForAccountingForBed(OrderForHotel orderForHotel)
    {
        float time = npcAIWorker.characterData.CalculationAccountingTime(gameItemsManager);
        yield return new WaitForSeconds(time);
        orderForHotel.bed.GetPrice(out long basePriceS, out long addPriceS);
        long payMoneyL = 0;
        long payMoneyM = 0;
        long payMoneyS = basePriceS + addPriceS;
        //是否出错
        bool isError = npcAIWorker.characterData.CalculationAccountingCheck(gameItemsManager, out float moreRate);

        long AddMoneyL = (long)(moreRate * payMoneyL);
        long AddMoneyM = (long)(moreRate * payMoneyM);
        long AddMoneyS = (long)(moreRate * payMoneyS);

        if (isError)
        {
            //出错
            //记录数据
            payMoneyL -= AddMoneyL;
            payMoneyM -= AddMoneyM;
            payMoneyS -= AddMoneyS;

            npcAIWorker.characterData.baseInfo.accountantInfo.AddAccountantFail
                (
                payMoneyL, payMoneyM, payMoneyS,
                AddMoneyL, AddMoneyM, AddMoneyS
                );
            //增加经验
            npcAIWorker.characterData.baseInfo.accountantInfo.AddExp(1, out bool isLevelUp);
            if (isLevelUp)
            {
                ToastForLevelUp(WorkerEnum.Accountant);
            }

            //工作者表示抱歉
            npcAIWorker.SetExpression(CharacterExpressionCpt.CharacterExpressionEnum.Wordless);
            //顾客生气
            orderForHotel.customer.SetExpression(CharacterExpressionCpt.CharacterExpressionEnum.Mad);
        }
        else
        {
            //成功
            payMoneyL += AddMoneyL;
            payMoneyM += AddMoneyM;
            payMoneyS += AddMoneyS;
            //记录数据
            npcAIWorker.characterData.baseInfo.accountantInfo.AddAccountantSuccess
                (
                payMoneyL, payMoneyM, payMoneyS,
                AddMoneyL, AddMoneyM, AddMoneyS
                );
            //增加经验
            npcAIWorker.characterData.baseInfo.accountantInfo.AddExp(1, out bool isLevelUp);
            if (isLevelUp)
            {
                ToastForLevelUp(WorkerEnum.Accountant);
            }
            //如果有额外的加成 工作者和店员都应该高兴
            //orderForCustomer.customer.SetExpression(CharacterExpressionCpt.CharacterExpressionEnum.Love);
        }
        //加上小时数
        payMoneyL = payMoneyL * orderForHotel.sleepTime;
        payMoneyM = payMoneyM * orderForHotel.sleepTime;
        payMoneyS = payMoneyS * orderForHotel.sleepTime;

        if (npcAIWorker.innHandler != null)
        {
            npcAIWorker.innHandler.PayMoney(orderForHotel, payMoneyL, payMoneyM, payMoneyS, true);
            //结束订单
            npcAIWorker.innHandler.EndOrder(orderForHotel);
        }

        //检测该柜台是否还有订单并且依旧没有取消改职业。如果有的话继续结账  
        //用于中断连续结账
        CharacterWorkerBaseBean characterWorkerData = npcAIWorker.characterData.baseInfo.GetWorkerInfoByType(WorkerEnum.Accountant);
        if (characterWorkerData.isWorking && orderForHotel.counter.payQueue.Count != 0)
        {
            OrderForBase newOrder = orderForHotel.counter.payQueue[0];
            orderForHotel.counter.payQueue.Remove(newOrder);
            StartAccounting(newOrder);
        }
        else
        {
            SetIntent(AccountantIntentEnum.Idle);
        }

        //通知离开
        orderForHotel.customer.SetIntent(NpcAICustomerForHotelCpt.CustomerHotelIntentEnum.Leave);
    }
}