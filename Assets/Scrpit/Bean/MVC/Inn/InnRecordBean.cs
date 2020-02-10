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

    /// <summary>
    /// 增加销售数量
    /// </summary>
    /// <param name="id"></param>
    /// <param name="number"></param>
    public void AddSellNumber(long id, int number, long moneyL, long moneyM, long moneyS)
    {
        if (listSellNumber == null)
            listSellNumber = new List<GameItemsBean>();

        foreach (GameItemsBean item in listSellNumber)
        {
            if (item.itemId == id)
            {
                item.itemNumber += number;
                return;
            }
        }

        GameItemsBean itemBean = new GameItemsBean(id, number);
        itemBean.priceL += moneyL;
        itemBean.priceM += moneyM;
        itemBean.priceS += moneyS;
        incomeL += moneyL;
        incomeM += moneyM;
        incomeS += moneyS;
        listSellNumber.Add(itemBean);
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
}