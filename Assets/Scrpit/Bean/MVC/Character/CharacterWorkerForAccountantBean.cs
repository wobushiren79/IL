using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class CharacterWorkerForAccountantBean : CharacterWorkerBaseBean
{
    //总计算账次数
    public long accountingTotalNumber;
    //结算失误次数
    public long accountingErrorNumber;
    //结算成功次数
    public long accountingSuccessNumber;

    //算账失误 失去的金钱
    public long loseMoneyL;
    public long lostMoneyM;
    public long lostMoneyS;

    //成功后额外获取的金钱
    public long moreMoneyL;
    public long moreMoneyM;
    public long moreMoneyS;

    //总计结算金钱
    public long accountingMoneyL;
    public long accountingMoneyM;
    public long accountingMoneyS;

    //总计结算时间
    public float accountingTotalTime;

    public CharacterWorkerForAccountantBean()
    {
        workerType = WorkerEnum.Accountant;
    }

    /// <summary>
    ///  增加结算成功次数和金钱
    /// </summary>
    /// <param name="moneyL"></param>
    /// <param name="moneyM"></param>
    /// <param name="moneyS"></param>
    /// <param name="moreProportion">多余获取比例</param>
    public void AddAccountantSuccess(long moneyL, long moneyM, long moneyS, long addMoneyL, long addMoneyM, long addMoneyS)
    {
        accountingTotalNumber += 1;
        accountingMoneyL += moneyL;
        accountingMoneyM += moneyM;
        accountingMoneyS += moneyS;

        accountingSuccessNumber += 1;
        moreMoneyL += addMoneyL;
        moreMoneyM += addMoneyM;
        moreMoneyS += addMoneyS;
    }


    /// <summary>
    /// 增加结算失误次数和金钱
    /// </summary>
    /// <param name="moneyL"></param>
    /// <param name="moneyM"></param>
    /// <param name="moneyS"></param>
    /// <param name="loseProportion">失去比例</param>
    public void AddAccountantFail(long moneyL, long moneyM, long moneyS,long loseMoneyL,long loseMoneyM,long loseMoneyS)
    {
        accountingTotalNumber += 1;
        accountingMoneyL += moneyL;
        accountingMoneyM += moneyM;
        accountingMoneyS += moneyS;

        accountingErrorNumber += 1;
        loseMoneyL += loseMoneyL;
        lostMoneyM += loseMoneyM;
        lostMoneyS += loseMoneyS;
    }

    /// <summary>
    /// 增加结算时间
    /// </summary>
    /// <param name="time"></param>
    public void AddAccountantTime(float time)
    {
        accountingTotalTime += time;
    }
}