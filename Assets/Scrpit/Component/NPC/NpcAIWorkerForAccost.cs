using UnityEngine;
using UnityEditor;
using System;

public class NpcAIWorkerForAccost : NpcAIWokerFoBaseCpt
{
    public enum AccostIntentEnum
    {
        Idle,//空闲
        Finding,//寻找中
        GoToCustomer,//走向客户
        Talking//交流中

    }

    //招募图标
    public GameObject accostPro;

    public AccostIntentEnum accostIntent;

    /// <summary>
    /// 开始招待
    /// </summary>
    public void StartAccost()
    {
        SetIntent(AccostIntentEnum.Finding);
    }

    /// <summary>
    /// 设置意图
    /// </summary>
    /// <param name="accostIntent"></param>
    public void SetIntent(AccostIntentEnum accostIntent)
    {
        this.accostIntent = accostIntent;
        switch (accostIntent)
        {
            case AccostIntentEnum.Idle:
                SetIntentForIdle();
                break;
            case AccostIntentEnum.Finding:
                SetIntentForFinding();
                break;
            case AccostIntentEnum.GoToCustomer:
                SetIntentForGoToCustomer();
                break;
            case AccostIntentEnum.Talking:
                SetIntentForTalking();
                break;
        }
    }
    /// <summary>
    /// 意图-闲置
    /// </summary>
    public void SetIntentForIdle()
    {
        if (accostPro != null)
            accostPro.SetActive(false);
    }

    /// <summary>
    /// 意图-寻找顾客
    /// </summary>
    public void SetIntentForFinding()
    {
        if (accostPro != null)
            accostPro.SetActive(true);

    }

    /// <summary>
    /// 意图-走向客户
    /// </summary>
    private void SetIntentForGoToCustomer()
    {
    }
    
    /// <summary>
    /// 意图-交流
    /// </summary>
    private void SetIntentForTalking()
    {
    }

}