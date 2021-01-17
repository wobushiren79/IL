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
        Talking,//交流中

        GoToHotelCustomer,//走向住宿顾客

        GoToStairsForFirst,//前往一楼楼梯
        GoToStairsForSecond,//前往二楼楼梯
        GoToBed,

    }


    //招募图标
    public GameObject accostPro;
    //谈话进度
    public GameObject talkPro;
    //引路进度
    public GameObject guidePro;
    //拉人的检测范围
    protected CircleCollider2D mAccostBox;

    //前往的目的地
    public Vector3 movePosition;
    public AccostIntentEnum accostIntent = AccostIntentEnum.Idle;
    public NpcAICustomerCpt npcAICustomer;//交流的顾客

    public OrderForHotel orderForHotel;
    public override void Start()
    {
        base.Start();
        mAccostBox = GetComponent<CircleCollider2D>();
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
            case AccostIntentEnum.GoToHotelCustomer:
                HandleForGoToHotelCustomer();
                break;
            case AccostIntentEnum.GoToStairsForFirst:
                HandleForGoToStairsForFirst();
                break;
            case AccostIntentEnum.GoToStairsForSecond:
                HandleForGoToStairsForSecond();
                break;
            case AccostIntentEnum.GoToBed:
                HandleForGoToBed();
                break;
        }
    }

    /// <summary>
    /// 开始招待
    /// </summary>
    public void StartAccostSolicit()
    {
        //如果在客栈内 则先去门口
        SetIntent(AccostIntentEnum.GoToDoor);
    }

    /// <summary>
    /// 开始引路
    /// </summary>
    /// <param name="orderForHotel"></param>
    public void StartAccostGuide(OrderForHotel orderForHotel)
    {
        this.orderForHotel = orderForHotel;
        SetIntent(AccostIntentEnum.GoToHotelCustomer);
    }

    /// <summary>
    /// 设置意图
    /// </summary>
    /// <param name="accostIntent"></param>
    public void SetIntent(AccostIntentEnum accostIntent)
    {
        //暂停查询倒计时
        StopAllCoroutines();
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
            case AccostIntentEnum.GoToHotelCustomer:
                SetIntentForGoToHotelCustomer();
                break;
            case AccostIntentEnum.GoToStairsForFirst:
                SetIntentForGoToStairsForFirst();
                break;
            case AccostIntentEnum.GoToStairsForSecond:
                SetIntentForGoToStairsForSecond();
                break;
            case AccostIntentEnum.GoToBed:
                SetIntentForGoToBed();
                break;
        }
    }


    /// <summary>
    /// 意图-闲置
    /// </summary>
    public void SetIntentForIdle()
    {
        orderForHotel = null;
        if (accostPro != null)
            accostPro.SetActive(false);
        if (talkPro != null)
            talkPro.SetActive(false);
        if (guidePro != null)
            guidePro.SetActive(false);
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
        //回去最靠近的门的嘴表
        movePosition = npcAIWorker.innHandler.GetCloseRandomEntrancePosition(transform.position);
        npcAIWorker.characterMoveCpt.SetDestination(movePosition + new Vector3(0, -2.5f, 0));
    }

    /// <summary>
    /// 意图-寻找顾客
    /// </summary>
    public void SetIntentForFinding()
    {
        StartCoroutine(CoroutineForFind());
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
        StartCoroutine(CoroutineForStartTalking());
    }

    /// <summary>
    /// 意图-走向
    /// </summary>
    private void SetIntentForGoToHotelCustomer()
    {
        if (guidePro != null)
            guidePro.SetActive(true);
        if (orderForHotel == null)
            SetIntent(AccostIntentEnum.Idle);
        //走向客户
        movePosition = orderForHotel.customer.transform.position + new Vector3(0, 0.5f, 0);
        npcAIWorker.characterMoveCpt.SetDestination(movePosition);
    }

    /// <summary>
    /// 意图-前往二楼
    /// </summary>
    public void SetIntentForGoToStairsForFirst()
    {
        BuildStairsCpt buildStairs = npcAIWorker.innHandler.GetCloseStairs(transform.position);
        if (buildStairs == null)
        {
            npcAIWorker.SetShout(GameCommonInfo.GetUITextById(13402));
            orderForHotel.customer.ChangeMood(-99999);
            SetIntent(AccostIntentEnum.Idle);
            return;
        }
        npcAIWorker.SetShout(GameCommonInfo.GetUITextById(13404));
        npcAIWorker.innHandler.GetStairsByRemarkId(buildStairs.remarkId, out Vector3 layerFirstPosition, out Vector3 layerSecondPosition);
        orderForHotel.layerFirstStairsPosition = layerFirstPosition;
        orderForHotel.layerSecondStairsPosition = layerSecondPosition;
        npcAIWorker.SetCharacterMove(layerFirstPosition);

        //根据魅力增加好感
        int addMood= npcAIWorker.characterData.CalculationAccostAddMood();
        orderForHotel.customer.ChangeMood(addMood);
        //设置相同的移动速度
        orderForHotel.customer.characterMoveCpt.SetMoveSpeed(npcAIWorker.characterMoveCpt.moveSpeed);
        orderForHotel.customer.SetIntent( NpcAICustomerForHotelCpt.CustomerHotelIntentEnum.GoToStairsForFirst);
    }

    /// <summary>
    /// 前往二楼楼梯
    /// </summary>
    public void SetIntentForGoToStairsForSecond()
    {
        if (guidePro != null)
            guidePro.SetActive(false);
        npcAIWorker.SetCharacterMove(orderForHotel.layerSecondStairsPosition);
    }

    /// <summary>
    /// 前往床
    /// </summary>
    public void SetIntentForGoToBed()
    {
        Vector3 sleepPosition = orderForHotel.bed.GetSleepPosition();
        if (!CheckUtil.CheckPath(sleepPosition,transform.position))
        {
            //不能到达
            npcAIWorker.SetShout(GameCommonInfo.GetUITextById(13405));
            SetIntent(AccostIntentEnum.GoToStairsForSecond);
            return;
        }
        npcAIWorker.SetCharacterMove(sleepPosition);
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
            if (accostIntent == AccostIntentEnum.Finding
                && this.npcAICustomer == null
                && npcAICustomer != null
                && npcAICustomer.customerIntent == NpcAICustomerCpt.CustomerIntentEnum.Walk)
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

    /// <summary>
    /// 开始交谈
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoroutineForStartTalking()
    {
        //计算聊天时间
        float talkTime = npcAIWorker.characterData.CalculationAccostTalkTime();
        //设置状态
        npcAICustomer.SetIntent(NpcAICustomerCpt.CustomerIntentEnum.TalkWithAccost);
        yield return new WaitForSeconds(talkTime);
        //是否成功
        if (npcAIWorker.characterData.CalculationAccostRate())
        {
            //根据魅力增加好感
            int addMood = npcAIWorker.characterData.CalculationAccostAddMood();
            npcAICustomer.ChangeMood(addMood);

            npcAIWorker.SetExpression(CharacterExpressionCpt.CharacterExpressionEnum.Love);
            npcAICustomer.SetExpression(CharacterExpressionCpt.CharacterExpressionEnum.Love);
            npcAICustomer.SetIntent(NpcAICustomerCpt.CustomerIntentEnum.Want);
            //记录
            npcAIWorker.characterData.baseInfo.accostInfo.AddAccostSuccessNumber(1);
            //添加经验
            npcAIWorker.characterData.baseInfo.accostInfo.AddExp(1, out bool isLevelUp);
            if (isLevelUp)
            {
                ToastForLevelUp(WorkerEnum.Accost);
            }
        }
        else
        {
            npcAIWorker.SetExpression(CharacterExpressionCpt.CharacterExpressionEnum.Wordless);
            npcAICustomer.SetExpression(CharacterExpressionCpt.CharacterExpressionEnum.Mad);
            npcAICustomer.SetIntent(NpcAICustomerCpt.CustomerIntentEnum.Leave);

            //记录
            npcAIWorker.characterData.baseInfo.accostInfo.AddAccostFailNumber(1);
            //添加经验
            //npcAIWorker.characterData.baseInfo.accostInfo.AddExp(1);
        }
        SetIntent(AccostIntentEnum.Idle);
    }

    /// <summary>
    /// 处理 前往住宿顾客
    /// </summary>
    public void HandleForGoToHotelCustomer()
    {
        if (npcAIWorker.characterMoveCpt.IsAutoMoveStop())
        {
            if (orderForHotel.GetOrderStatus() != OrderHotelStatusEnum.End)
            {
                SetIntent(AccostIntentEnum.GoToStairsForFirst);
            }
            else
            {
                SetIntent(AccostIntentEnum.Idle);
            }
        }
    }

    /// <summary>
    /// 处理-到达楼梯
    /// </summary>
    public void HandleForGoToStairsForFirst()
    {
        if (npcAIWorker.characterMoveCpt.IsAutoMoveStop())
        {
            transform.position = orderForHotel.layerSecondStairsPosition;
            SetIntent(AccostIntentEnum.GoToBed);
        }
    }

    /// <summary>
    /// 处理-到达楼梯
    /// </summary>
    public void HandleForGoToStairsForSecond()
    {
        if (npcAIWorker.characterMoveCpt.IsAutoMoveStop())
        {
            transform.position = orderForHotel.layerFirstStairsPosition;
            SetIntent(AccostIntentEnum.Idle);
        }
    }

    /// <summary>
    /// 处理-到达床
    /// </summary>
    public void HandleForGoToBed()
    {
        if (npcAIWorker.characterMoveCpt.IsAutoMoveStop())
        {
            //记录数据
            npcAIWorker.characterData.baseInfo.accostInfo.AddGuideNumber(1);
            //增加经验
            npcAIWorker.characterData.baseInfo.accostInfo.AddExp(5, out bool isLevelUp);
            if (isLevelUp)
            {
                ToastForLevelUp(WorkerEnum.Accost);
            }
            SetIntent(AccostIntentEnum.GoToStairsForSecond);
        }
    }

    /// <summary>
    /// 协程搜索
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoroutineForFind()
    {
        yield return new WaitForSeconds(3);
        SetIntent(AccostIntentEnum.Idle);
    }

}