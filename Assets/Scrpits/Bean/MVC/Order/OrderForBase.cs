using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class OrderForBase
{
    //支付的地方
    public BuildCounterCpt counter;
    //评价数据
    public InnEvaluationBean innEvaluation = new InnEvaluationBean();

    public virtual bool  CheckOrder()
    {
        return true;
    }
}