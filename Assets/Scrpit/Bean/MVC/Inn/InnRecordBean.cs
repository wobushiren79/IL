using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
public class InnRecordBean
{
    public int year;
    public int month;
    public int day;

    //枚举DayEnum
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
    //基础食材支付
    public long payIngS;
    public long payIngM;
    public long payIngL;

    //售卖数量
    public List<GameItemsBean> listSellNumber = new List<GameItemsBean>();

    //住宿进账
    public long incomeForHotelS;
    public long incomeForHotelM;
    public long incomeForHotelL;

    //食物进账
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

    //住宿
    public long numberForHotelCustomer;
    public long numberForHotelCustomerComplete;

    /// <summary>
    /// 增加顾客数量
    /// </summary>
    /// <param name="customerType"></param>
    /// <param name="number"></param>
    public void AddCutomerForFoodNumber(CustomerTypeEnum customerType,int number)
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

    public void AddCutomerForHotelNumber(int number)
    {
        numberForHotelCustomer += number;
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
    /// 增加基础食材金钱消耗
    /// </summary>
    public void AddPayIng(long moneyL, long moneyM, long moneyS)
    {
        payIngS += moneyS;
        payIngM += moneyM;
        payIngL += moneyL;

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
        AddIncomeForFood(moneyL, moneyM, moneyS);
    }

    public void AddHotelNumber(int number, long moneyL, long moneyM, long moneyS)
    {
        numberForHotelCustomerComplete += number;
        AddIncomeForHotel(moneyL, moneyM, moneyS);
    }

    /// <summary>
    /// 增加进账
    /// </summary>
    /// <param name="incomeL"></param>
    /// <param name="incomeM"></param>
    /// <param name="incomeS"></param>
    public void AddIncomeForFood(long incomeL, long incomeM, long incomeS)
    {
        this.incomeL += incomeL;
        this.incomeM += incomeM;
        this.incomeS += incomeS;
    }

    public void AddIncomeForHotel(long incomeL, long incomeM, long incomeS)
    {
        this.incomeForHotelL += incomeL;
        this.incomeForHotelM += incomeM;
        this.incomeForHotelS += incomeS;
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

    /// <summary>
    /// 获取总计食物订单
    /// </summary>
    /// <returns></returns>
    public long GetTotalCompleteCustomerForFood()
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

    /// <summary>
    /// 获取总计完成食物订单
    /// </summary>
    /// <returns></returns>
    public long GetTotalCustomerForFood()
    {
        return numberForNormalCustomer + numberForFriendsCustomer + numberForTeamCustomer;
    }

    /// <summary>
    /// 获取总计住宿订单
    /// </summary>
    /// <returns></returns>
    public long GetTotalCustomerForHotel()
    {
        return numberForHotelCustomer;
    }

    /// <summary>
    /// 获取总计完成住宿订单
    /// </summary>
    /// <returns></returns>
    public long GetTotalCompleteCustomerForHotel()
    {
        return numberForHotelCustomerComplete;
    }

    /// <summary>
    /// 获取所有收入
    /// </summary>
    /// <param name="incomeL"></param>
    /// <param name="incomeM"></param>
    /// <param name="incomeS"></param>
    public void GetTotalIncome(out long incomeL, out long incomeM, out long incomeS)
    {
        incomeL = this.incomeForHotelL + this.incomeL;
        incomeM = this.incomeForHotelM + this.incomeM;
        incomeS = this.incomeForHotelS + this.incomeS;
    }

    /// <summary>
    /// 获取所有支出
    /// </summary>
    /// <param name="incomeL"></param>
    /// <param name="incomeM"></param>
    /// <param name="incomeS"></param>
    public void GetTotalExpenses(out long expensesL, out long expensesM, out long expensesS)
    {
        expensesL = this.expensesL;
        expensesM = this.expensesM;
        expensesS = this.expensesS;
    }


}