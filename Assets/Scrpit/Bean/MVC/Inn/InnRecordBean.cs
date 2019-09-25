using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
public class InnRecordBean 
{
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
    public Dictionary<long, int> sellNumber = new Dictionary<long, int>();

 
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
}