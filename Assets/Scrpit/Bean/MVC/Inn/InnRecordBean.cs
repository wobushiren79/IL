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

    public int status;

    //消耗材料
    public int consumeIngOilsalt;
    public int consumeIngMeat;
    public int consumeIngRiverfresh;
    public int consumeIngSeafood;
    public int consumeIngVegetablest;
    public int consumeIngMelonfruit;
    public int consumeIngWaterwine;
    public int consumeIngFlour;

    //售卖数量
    public List<ItemBean> listSellNumber = new List<ItemBean>();

    //进账
    public long incomeS;
    public long incomeM;
    public long incomeL;

    //出账
    public long expensesS;
    public long expensesM;
    public long expensesL;

    //好评数量
    public long praiseGoodNumber;
    //差评数量
    public long praiseBadNumber;

    /// <summary>
    /// 增加销售数量
    /// </summary>
    /// <param name="id"></param>
    /// <param name="number"></param>
    public void AddSellNumber(long id,int number)
    {
        if (listSellNumber == null)
            listSellNumber = new List<ItemBean>();

        foreach (ItemBean item in listSellNumber)
        {
            if (item.itemId == id)
            {
                item.itemNumber += number;
                return;
            }
        }
        ItemBean itemBean = new ItemBean(id, number);
        listSellNumber.Add(itemBean);
    }

    /// <summary>
    /// 增加进账
    /// </summary>
    /// <param name="incomeL"></param>
    /// <param name="incomeM"></param>
    /// <param name="incomeS"></param>
    public void AddIncome(long incomeL,long incomeM,long incomeS)
    {
        this.incomeL += incomeL;
        this.incomeM += incomeM;
        this.incomeS += incomeS;
    }
}