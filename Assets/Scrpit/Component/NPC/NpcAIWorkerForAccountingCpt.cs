﻿using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
public class NpcAIWorkerForAccountingCpt : NpcAIWokerFoBaseCpt
{
    public enum AccountingIntentEnum
    {
        Idle,//空闲
        GoToAccounting,//结账之前的路上
        Accounting,//结账中
    }
    //点单
    public OrderForCustomer orderForCustomer;
    //算账进度
    public GameObject accountingPro;
    //移动的目的点
    public Vector3 movePosition;
    //厨师状态
    public AccountingIntentEnum accountingIntent = AccountingIntentEnum.Idle;


    private void FixedUpdate()
    {
        switch (accountingIntent)
        {
            case AccountingIntentEnum.Idle:
                break;
            case AccountingIntentEnum.GoToAccounting:
                if (orderForCustomer.CheckOrder())
                {
                    if (npcAIWorker.characterMoveCpt.IsAutoMoveStop())
                    {
                        //设置朝向
                        npcAIWorker.SetCharacterFace(orderForCustomer.counter.GetUserFace());
                        SetIntent(AccountingIntentEnum.Accounting);
                    }
                }
                else
                {
                    SetIntent(AccountingIntentEnum.Idle);
                }
                break;
            case AccountingIntentEnum.Accounting:
                if (!orderForCustomer.CheckOrder())
                {
                    SetIntent(AccountingIntentEnum.Idle);
                }
                break;
        }
    }

    public void StartAccounting(OrderForCustomer orderForCustomer)
    {
        SetIntent(AccountingIntentEnum.GoToAccounting, orderForCustomer);
    }

    /// <summary>
    /// 设置意图
    /// </summary>
    /// <param name="accountingIntent"></param>
    /// <param name="orderForCustomer"></param>
    public void SetIntent(AccountingIntentEnum accountingIntent, OrderForCustomer orderForCustomer)
    {
        this.accountingIntent = accountingIntent;
        this.orderForCustomer = orderForCustomer;
        switch (accountingIntent)
        {
            case AccountingIntentEnum.Idle:
                SetIntentForIdle();
                break;
            case AccountingIntentEnum.GoToAccounting:
                SetIntentForGoToAccounting(orderForCustomer);
                break;
            case AccountingIntentEnum.Accounting:
                SetIntentForAccounting(orderForCustomer);
                break;
        }
    }
    public void SetIntent(AccountingIntentEnum accountingIntent)
    {
        SetIntent(accountingIntent, orderForCustomer);
    }

    /// <summary>
    /// 意图-闲置
    /// </summary>
    public void SetIntentForIdle()
    {
        StopAllCoroutines();
        accountingPro.SetActive(false);
        npcAIWorker.SetIntent(NpcAIWorkerCpt.WorkerIntentEnum.Idle);
        if (orderForCustomer != null && orderForCustomer.counter != null)
            orderForCustomer.counter.SetCounterStatus(BuildCounterCpt.CounterStatusEnum.Idle);
        orderForCustomer = null;
    }

    /// <summary>
    /// 意图-前往结算
    /// </summary>
    /// <param name="orderForCustomer"></param>
    public void SetIntentForGoToAccounting(OrderForCustomer orderForCustomer)
    {
        if (CheckUtil.CheckPath(transform.position, orderForCustomer.counter.GetAccountingPosition()))
        {
            accountingPro.SetActive(true);
            movePosition = orderForCustomer.counter.GetAccountingPosition();
            npcAIWorker.characterMoveCpt.SetDestination(movePosition);
        }
        else
        {
            SetIntent(AccountingIntentEnum.Idle);
        }
    }

    /// <summary>
    /// 意图-结算中
    /// </summary>
    /// <param name="orderForCustomer"></param>
    public void SetIntentForAccounting(OrderForCustomer orderForCustomer)
    {
        //设置柜台的状态
        orderForCustomer.counter.SetCounterStatus(BuildCounterCpt.CounterStatusEnum.Accounting);
        //开始结算
        StartCoroutine(StartAccounting());
    }

    public IEnumerator StartAccounting()
    {
        yield return new WaitForSeconds(5);
        //是否出错
        bool isError = false;
        if (isError)
        {
            //出错
            //记录数据
            npcAIWorker.characterData.baseInfo.accountingInfo.AddAccountingFail
                (orderForCustomer.foodData.price_l, orderForCustomer.foodData.price_m, orderForCustomer.foodData.price_s,0.5f);
            //增加经验
            npcAIWorker.characterData.baseInfo.accountingInfo.AddExp(1);

            //工作者表示抱歉
            npcAIWorker.SetExpression(CharacterExpressionCpt.CharacterExpressionEnum.Wordless);
            //顾客生气
            orderForCustomer.customer.SetExpression(CharacterExpressionCpt.CharacterExpressionEnum.Mad);
        }
        else
        {
            //成功
            //记录数据
            npcAIWorker.characterData.baseInfo.accountingInfo.AddAccountingSuccess
                (orderForCustomer.foodData.price_l, orderForCustomer.foodData.price_m, orderForCustomer.foodData.price_s,0);
            //增加经验
            npcAIWorker.characterData.baseInfo.accountingInfo.AddExp(2);

            //TODO 如果有额外的加成 工作者和店员都应该高兴

        }

        if (npcAIWorker.innHandler != null)
            npcAIWorker.innHandler.PayMoney(orderForCustomer, 1);
        //通知离开
        orderForCustomer.customer.SetIntent(NpcAICustomerCpt.CustomerIntentEnum.Leave);
        SetIntent(AccountingIntentEnum.Idle);
    }

}