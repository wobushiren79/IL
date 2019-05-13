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

    public long incomeS;
    public long incomeM;
    public long incomeL;

    public long expensesS;
    public long expensesM;
    public long expensesL;
}