using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class GambleBaseBean
{
    public GambleTypeEnum gambleType;

    //下注金额
    public long betForMoneyL;
    public long betForMoneyM;
    public long betForMoneyS;

    //下注上限
    public long betMaxForMoneyL;
    public long betMaxForMoneyM;
    public long betMaxForMoneyS;


    public GambleBaseBean(GambleTypeEnum gambleType)
    {
        this.gambleType = gambleType;
    }

    /// <summary>
    /// 设置下注上限
    /// </summary>
    /// <param name="betMaxForMoneyL"></param>
    /// <param name="betMaxForMoneyM"></param>
    /// <param name="betMaxForMoneyS"></param>
    public void SetBetMax(long betMaxForMoneyL, long betMaxForMoneyM, long betMaxForMoneyS)
    {
        this.betMaxForMoneyL = betMaxForMoneyL;
        this.betMaxForMoneyM = betMaxForMoneyM;
        this.betMaxForMoneyS = betMaxForMoneyS;
    }


}