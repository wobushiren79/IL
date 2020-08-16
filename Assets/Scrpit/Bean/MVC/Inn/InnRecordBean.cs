﻿using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
public class InnRecordBean
{
    public int year;
    public int month;
    public int day;

    public int status;

    //消耗材料
    public int consumeIngOilsalt;
    public int consumeIngMeat;
    public int consumeIngRiverfresh;
    public int consumeIngSeafood;
    public int consumeIngVegetables;
    public int consumeIngMelonfruit;
    public int consumeIngWaterwine;
    public int consumeIngFlour;

    //支付工资
    public long payWageS;
    public long payWageM;
    public long payWageL;
    //支付贷款
    public long payLoansS;
    public long payLoansM;
    public long payLoansL;
    //售卖数量
    public List<GameItemsBean> listSellNumber = new List<GameItemsBean>();

    //进账
    public long incomeS;
    public long incomeM;
    public long incomeL;

    //出账
    public long expensesS;
    public long expensesM;
    public long expensesL;

    //好评数量
    public long praiseExcitedNumber;
    public long praiseHappyNumber;
    public long praiseOkayNumber;
    public long praiseOrdinaryNumber;
    public long praiseDisappointedNumber;
    public long praiseAngerNumber;

    public long numberForNormalCustomer;
    public long numberForTeamCustomer;
    public long numberForFriendsCustomer;

    /// <summary>
    /// 增加顾客数量
    /// </summary>
    /// <param name="customerType"></param>
    /// <param name="number"></param>
    public void AddCutomerNumber(CustomerTypeEnum customerType,int number)
    {
        switch (customerType)
        {
            case CustomerTypeEnum.Normal:
                numberForNormalCustomer += number;
                break;
            case CustomerTypeEnum.Team:
                numberForTeamCustomer += number;
                break;
            case CustomerTypeEnum.Friend:
                numberForFriendsCustomer += number;
                break;
        }
    }

    /// <summary>
    /// 增加支付工资
    /// </summary>
    public void AddPayWage(long moneyL, long moneyM, long moneyS)
    {
        payWageS += moneyS;
        payWageM += moneyM;
        payWageL += moneyL;

        expensesS += moneyS;
        expensesM += moneyM;
        expensesL += moneyL;
    }
    
    /// <summary>
    /// 增加贷款还款
    /// </summary>
    /// <param name="moneyL"></param>
    /// <param name="moneyM"></param>
    /// <param name="moneyS"></param>
    public void AddPayLoans(long moneyL, long moneyM, long moneyS)
    {
        payLoansS += moneyS;
        payLoansM += moneyM;
        payLoansL += moneyL;

        expensesS += moneyS;
        expensesM += moneyM;
        expensesL += moneyL;
    }

    /// <summary>
    /// 增加销售数量
    /// </summary>
    /// <param name="id"></param>
    /// <param name="number"></param>
    public void AddSellNumber(long id, int number, long moneyL, long moneyM, long moneyS)
    {
        if (listSellNumber == null)
            listSellNumber = new List<GameItemsBean>();
        GameItemsBean sellItem = null;
        foreach (GameItemsBean item in listSellNumber)
        {
            if (item.itemId == id)
            {
                sellItem = item;
                break;
            }
        }
        if (sellItem == null)
        {
            sellItem = new GameItemsBean(id);
            listSellNumber.Add(sellItem);
        }
        sellItem.itemNumber += number;
        sellItem.priceL += moneyL;
        sellItem.priceM += moneyM;
        sellItem.priceS += moneyS;
        AddIncome(moneyL, moneyM, moneyS);
    }

    /// <summary>
    /// 增加进账
    /// </summary>
    /// <param name="incomeL"></param>
    /// <param name="incomeM"></param>
    /// <param name="incomeS"></param>
    public void AddIncome(long incomeL, long incomeM, long incomeS)
    {
        this.incomeL += incomeL;
        this.incomeM += incomeM;
        this.incomeS += incomeS;
    }

    /// <summary>
    /// 增加评价
    /// </summary>
    /// <param name="type"></param>
    /// <param name="number"></param>
    public void AddPraise(PraiseTypeEnum type, int number)
    {
        switch (type)
        {
            case PraiseTypeEnum.Excited:
                praiseExcitedNumber += number;
                break;
            case PraiseTypeEnum.Happy:
                praiseHappyNumber += number;
                break;
            case PraiseTypeEnum.Okay:
                praiseOkayNumber += number;
                break;
            case PraiseTypeEnum.Ordinary:
                praiseOrdinaryNumber += number;
                break;
            case PraiseTypeEnum.Disappointed:
                praiseDisappointedNumber += number;
                break;
            case PraiseTypeEnum.Anger:
                praiseAngerNumber += number;
                break;
        }
    }

    public long GetTotalCustomer()
    {
        return numberForNormalCustomer + numberForFriendsCustomer + numberForTeamCustomer;
    }

    public long GetTotalPayCustomer()
    {
        long number = 0;
        if (listSellNumber == null)
            return number;
        foreach (GameItemsBean gameItems in listSellNumber)
        {
            number += gameItems.itemNumber;
        }
        return number;
    }
}