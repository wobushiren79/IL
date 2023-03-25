using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
public class UserLoansBean 
{
    //借贷金额
    public long moneyS;
    //借贷利率
    public float loansRate;

    //每日还款金额
    public long moneySForDay;
    //还款天数
    public int loansDays;
    //剩余还款天数
    public int residueDays;

    public UserLoansBean(long moneyS, float loansRate, int loansDays)
    {
        this.moneyS = moneyS;
        this.loansRate = loansRate;
        this.loansDays = loansDays;
        this.residueDays = loansDays;

        long loansMoneyS = (long)(moneyS *( 1+ loansRate));
        this.moneySForDay = loansMoneyS / loansDays;
    }
}