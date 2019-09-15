using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

public class NpcAIWorkerForAccost : NpcAIWokerFoBaseCpt
{
    public enum AccostIntentEnum
    {
        Idle,//空闲
        GoToDoor,//前往门口
        Finding,//寻找中
        GoToCustomer,//走向客户
        Talking//交流中

    }

    //招募图标
    public GameObject accostPro;
    //谈话进度
    public GameObject talkPro;
    //拉人的检测范围
    public BoxCollider2D mAccostBox;

    //前往的目的地
    public Vector3 movePosition;
    public AccostIntentEnum accostIntent = AccostIntentEnum.Idle;
    public NpcAICustomerCpt npcAICustomer;//交流的顾客
    public override void Start()
    {
        base.Start();
        mAccostBox = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        switch (accostIntent)
        {
            case AccostIntentEnum.GoToDoor:
                //检测是否到达目的地
                if (npcAIWorker.characterMoveCpt.IsAutoMoveStop())
                {
                    SetIntent(AccostIntentEnum.Finding);
                }
                break;
            case AccostIntentEnum.GoToCustomer:
                if (npcAIWorker.characterMoveCpt.IsAutoMoveStop())
                {
                    SetIntent(AccostIntentEnum.Talking);
                }
                break;
        }
    }

    /// <summary>
    /// 开始招待
    /// </summary>
    public void StartAccost()
    {
        //如果在客栈内 则先去门口
        SetIntent(AccostIntentEnum.GoToDoor);
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
            case AccostIntentEnum.GoToDoor:
                SetIntentForGoToDoor();
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
        if (talkPro != null)
            talkPro.SetActive(false);
        if (mAccostBox != null)
            mAccostBox.enabled = false;
        npcAICustomer = null;
        npcAIWorker.SetIntent(NpcAIWorkerCpt.WorkerIntentEnum.Idle);
    }

    /// <summary>
    /// 前往门口
    /// </summary>
    public void SetIntentForGoToDoor()
    {
        if (accostPro != null)
            accostPro.SetActive(false);
        movePosition = npcAIWorker.innHandler.GetRandomEntrancePosition();
        npcAIWorker.characterMoveCpt.SetDestination(movePosition);
    }

    /// <summary>
    /// 意图-寻找顾客
    /// </summary>
    public void SetIntentForFinding()
    {
        //开启范围检测
        if (mAccostBox != null)
            mAccostBox.enabled = true;
        if (accostPro != null)
            accostPro.SetActive(true);
    }

    /// <summary>
    /// 意图-走向客户
    /// </summary>
    private void SetIntentForGoToCustomer()
    {
        if (accostPro != null)
            accostPro.SetActive(false);
        //设置客户等待
        npcAICustomer.SetIntent(NpcAICustomerCpt.CustomerIntentEnum.WaitAccost);
        //走向客户
        movePosition = Vector3.Lerp(transform.transform.position, npcAICustomer.transform.position, 0.9f);          
        npcAIWorker.characterMoveCpt.SetDestination(movePosition);
        //展示表情
        npcAIWorker.SetExpression(CharacterExpressionCpt.CharacterExpressionEnum.Surprise);
    }

    /// <summary>
    /// 意图-交流
    /// </summary>
    private void SetIntentForTalking()
    {
        if (accostPro != null)
            accostPro.SetActive(false);
        if (talkPro != null)
            talkPro.SetActive(true);
        StartCoroutine(StartTalking(5));
    }

    /// <summary>
    /// 范围检测
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            NpcAICustomerCpt npcAICustomer = collision.GetComponent<NpcAICustomerCpt>();
            if (this.npcAICustomer == null && npcAICustomer != null && npcAICustomer.customerIntent == NpcAICustomerCpt.CustomerIntentEnum.Walk)
            {
                //设置顾客
                this.npcAICustomer = npcAICustomer;
                //关闭检测
                if (mAccostBox != null)
                    mAccostBox.enabled = false;
                SetIntent(AccostIntentEnum.GoToCustomer);
            }
        }
    }

    public IEnumerator StartTalking(float talkTime)
    {
        yield return new WaitForSeconds(talkTime);
        int isSuccess = 1;
        isSuccess = UnityEngine.Random.Range(0, 2);
        if (isSuccess == 1)
        {
            npcAIWorker.SetExpression(CharacterExpressionCpt.CharacterExpressionEnum.Love);
            npcAICustomer.SetExpression(CharacterExpressionCpt.CharacterExpressionEnum.Love);
            npcAICustomer.SetIntent(NpcAICustomerCpt.CustomerIntentEnum.Want);
            //记录
            npcAIWorker.characterData.baseInfo.accostInfo.AddAccostSuccessNumber(1);
            //添加经验
            npcAIWorker.characterData.baseInfo.accostInfo.AddExp(1);
        }
        else
        {
            npcAIWorker.SetExpression(CharacterExpressionCpt.CharacterExpressionEnum.Shame);
            npcAICustomer.SetExpression(CharacterExpressionCpt.CharacterExpressionEnum.Mad);
            npcAICustomer.SetIntent(NpcAICustomerCpt.CustomerIntentEnum.Leave);

            //记录
            npcAIWorker.characterData.baseInfo.accostInfo.AddAccostFailNumber(1);
            //添加经验
            //npcAIWorker.characterData.baseInfo.accostInfo.AddExp(1);
        }
        SetIntent(AccostIntentEnum.Idle);
    }

}