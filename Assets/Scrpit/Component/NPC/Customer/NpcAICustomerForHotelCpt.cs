using UnityEngine;
using UnityEditor;
using System.Collections;
public class NpcAICustomerForHotelCpt : BaseNpcAI
{
    public enum CustomerHotelIntentEnum
    {
        Walk = 0,//路过
        GoToInn = 1,//前往客栈
        WaitAccost = 2,
        GoToStairsForFirst = 3, // 前往一楼楼梯
        GoToBed = 4,
        GoToStairsForSecond = 5,
        Sleep = 6,
        GoToPay = 7,
        WaitPay = 8,
        Leave = 99,//离开
    }

    public enum CustomerHotelNotifyEnum
    {
        StatusChange = 1,//状态改变
    }

    /// <summary>
    /// 获取顾客状态
    /// </summary>
    /// <param name="intentStr"></param>
    /// <returns></returns>
    public virtual CustomerHotelIntentEnum GetCustomerHotelStatus(out string intentStr)
    {
        intentStr = "???";
        switch (customerHotelIntent)
        {
            case CustomerHotelIntentEnum.Walk:
                intentStr = TextHandler.Instance.manager.GetTextById(151);
                break;
            case CustomerHotelIntentEnum.GoToInn:
                intentStr = TextHandler.Instance.manager.GetTextById(152);
                break;
            case CustomerHotelIntentEnum.WaitAccost:
                intentStr = TextHandler.Instance.manager.GetTextById(161);
                break;
            case CustomerHotelIntentEnum.GoToStairsForFirst:
            case CustomerHotelIntentEnum.GoToBed:
                intentStr = TextHandler.Instance.manager.GetTextById(162);
                break;
            case CustomerHotelIntentEnum.Sleep:
                intentStr = TextHandler.Instance.manager.GetTextById(164);
                break;
            case CustomerHotelIntentEnum.GoToStairsForSecond:
                intentStr = TextHandler.Instance.manager.GetTextById(163);
                break;
            case CustomerHotelIntentEnum.GoToPay:
                intentStr = TextHandler.Instance.manager.GetTextById(157);
                break;
            case CustomerHotelIntentEnum.WaitPay:
                intentStr = TextHandler.Instance.manager.GetTextById(158);
                break;
            case CustomerHotelIntentEnum.Leave:
                intentStr = TextHandler.Instance.manager.GetTextById(160);
                break;
        }
        return customerHotelIntent;
    }

    public CustomerHotelIntentEnum customerHotelIntent;
    //移动目标点
    public Vector3 movePosition;
    //表情控制
    public CharacterMoodCpt characterMoodCpt;

    public OrderForHotel orderForHotel;

    //等待动画
    public RuntimeAnimatorController waitIconAnim;

    public void Update()
    {
        switch (customerHotelIntent)
        {
            case CustomerHotelIntentEnum.Walk:
            case CustomerHotelIntentEnum.Leave:
                HandleForLeave();
                break;
            case CustomerHotelIntentEnum.WaitAccost:
                ChangeMood(-Time.deltaTime);
                break;
            case CustomerHotelIntentEnum.GoToInn:
                HandleForGoToInn();
                break;
            case CustomerHotelIntentEnum.GoToStairsForFirst:
                HandleForGoToStairsForFirst();
                break;
            case CustomerHotelIntentEnum.GoToStairsForSecond:
                HandleForGoToStairsForSecond();
                break;
            case CustomerHotelIntentEnum.GoToBed:
                HandleForGoToBed();
                break;
            case CustomerHotelIntentEnum.GoToPay:
                HandleForGoToPay();
                break;
            case CustomerHotelIntentEnum.WaitPay:
                ChangeMood(-Time.deltaTime);
                break;
        }
    }

    public void SetIntent(CustomerHotelIntentEnum intent)
    {
        //删除进度图标
        RemoveStatusIconByType(CharacterStatusIconEnum.Pro);
        if (this)
            StopAllCoroutines();
        this.customerHotelIntent = intent;
        switch (intent)
        {
            case CustomerHotelIntentEnum.Walk:
                IntentForWalk();
                break;
            case CustomerHotelIntentEnum.Leave:
                IntentForLeave();
                break;
            case CustomerHotelIntentEnum.GoToInn:
                IntentForGoToInn();
                break;
            case CustomerHotelIntentEnum.WaitAccost:
                IntentForWaitAccost();
                break;
            case CustomerHotelIntentEnum.GoToStairsForFirst:
                IntentForGoToStairsFirst();
                break;
            case CustomerHotelIntentEnum.GoToBed:
                IntentForGoToBed();
                break;
            case CustomerHotelIntentEnum.GoToStairsForSecond:
                IntentForGoToStairsSecond();
                break;
            case CustomerHotelIntentEnum.Sleep:
                IntentForSleep();
                break;
            case CustomerHotelIntentEnum.GoToPay:
                IntentForGoToPay();
                break;
            case CustomerHotelIntentEnum.WaitPay:
                IntentForWaitPay();
                break;
        }
        NotifyAllObserver((int)CustomerHotelNotifyEnum.StatusChange, (int)intent);
    }

    /// <summary>
    /// 意图-散步
    /// </summary>
    public void IntentForWalk()
    {
        SceneInnManager sceneInnManager = GameScenesHandler.Instance.manager.GetSceneManager<SceneInnManager>();
        if (transform.position.x > 0)
            //如果角色在右边生成 出口则设置为左边
            movePosition = sceneInnManager.GetRandomSceneExportPosition(1);
        else
            //如果角色在左边生成 出口则设置为右边
            movePosition = sceneInnManager.GetRandomSceneExportPosition(0);
        characterMoveCpt.SetDestination(movePosition);
    }

    /// <summary>
    /// 意图-离开
    /// </summary>
    public void IntentForLeave()
    {
        //随机获取一个退出点
        SceneInnManager sceneInnManager = GameScenesHandler.Instance.manager.GetSceneManager<SceneInnManager>();
        movePosition = sceneInnManager.GetRandomSceneExportPosition();
        characterMoveCpt.SetDestination(movePosition);
    }

    /// <summary>
    /// 意图-离开
    /// </summary>
    public void IntentForGoToInn()
    {
        movePosition = InnHandler.Instance.GetRandomEntrancePosition();
        //移动到门口附近
        if (movePosition == null || movePosition == Vector3.zero)
        {
            //如果找不到门则离开 散散步
            SetIntent(CustomerHotelIntentEnum.Leave);
        }
        else
            //前往门
            characterMoveCpt.SetDestination(movePosition);
    }

    /// <summary>
    /// 意图-等待接待
    /// </summary>
    public void IntentForWaitAccost()
    {
        //开启满意度
        characterMoodCpt.SetMood(orderForHotel.innEvaluation.GetPraise());
        AddWaitIcon();
    }

    /// <summary>
    /// 意图-等待接待
    /// </summary>
    public void IntentForGoToStairsFirst()
    {
        SetCharacterMove(orderForHotel.layerFirstStairsPosition);
    }

    /// <summary>
    /// 前往二楼楼梯
    /// </summary>
    public void IntentForGoToStairsSecond()
    {
        SetCharacterMove(orderForHotel.layerSecondStairsPosition);
    }

    /// <summary>
    /// 前往床
    /// </summary>
    public void IntentForGoToBed()
    {
        Vector3 sleepPosition = orderForHotel.bed.GetSleepPosition();
        if (!CheckUtil.CheckPath(sleepPosition, transform.position))
        {
            InnHandler.Instance.EndOrderForForce(orderForHotel, true);
            SetIntent(CustomerHotelIntentEnum.GoToStairsForSecond);
            return;
        }
        SetCharacterMove(sleepPosition);
    }

    /// <summary>
    /// 睡觉
    /// </summary>
    public void IntentForSleep()
    {
        //设置睡觉的朝向
        Direction2DEnum directionBed = orderForHotel.bed.GetDirection();
        SetCharacterSleep(directionBed);
        //开始睡觉
        orderForHotel.bed.SetBedStatus(BuildBedCpt.BedStatusEnum.Use);
        StartCoroutine(CoroutineForStartSleep(orderForHotel.sleepTime));
    }

    /// <summary>
    /// 前往支付
    /// </summary>
    public void IntentForGoToPay()
    {
        orderForHotel.counter = InnHandler.Instance.GetCounter(transform.position);
        //如果判断有无结算台
        if (orderForHotel.counter == null)
        {
            InnHandler.Instance.EndOrderForForce(orderForHotel, true);
        }
        else
        {
            movePosition = orderForHotel.counter.GetPayPosition();
            //if (!CheckUtil.CheckPath(transform.position, movePosition))
            //{
            //    InnHandler.Instance.EndOrderForForce(orderForHotel, true);
            //}
            //else
            //{
            //    characterMoveCpt.SetDestination(movePosition);
            //}
            characterMoveCpt.SetDestination(movePosition);
        }
    }


    /// <summary>
    /// 改变心情
    /// </summary>
    /// <param name="mood"></param>
    public virtual void ChangeMood(float mood)
    {
        if (orderForHotel == null)
            return;
        orderForHotel.innEvaluation.AddMood(mood);
        characterMoodCpt.SetMood(orderForHotel.innEvaluation.GetPraise());
        if (orderForHotel.innEvaluation.mood <= 0)
        {
            StopAllCoroutines();
            InnHandler.Instance.EndOrderForForce(orderForHotel, true);
        }
    }

    /// <summary>
    /// 等待支付
    /// </summary>
    public void IntentForWaitPay()
    {
        AddPayIcon();
        orderForHotel.counter.payQueue.Add(orderForHotel);
    }


    /// <summary>
    /// 离开处理
    /// </summary>
    public virtual void HandleForLeave()
    {
        //到目标点就删除
        if (!characterMoveCpt.IsAutoMoveStop())
            return;
        gameObject.SetActive(false);
        gameObject.transform.position = new Vector3(0, -10000, 0);
        NpcHandler.Instance.builderForCustomer.listCustomerForHotelHide.Enqueue(gameObject);
    }

    /// <summary>
    /// 前往客栈处理
    /// </summary>
    public virtual void HandleForGoToInn()
    {
        if (characterMoveCpt.IsAutoMoveStop())
        {
            //判断点是否关门
            if (InnHandler.Instance.GetInnStatus() == InnHandler.InnStatusEnum.Open)
            {
                //首先判断还有没有床位
                BuildBedCpt buildBedCpt = InnHandler.Instance.GetIdleBed();
                if (buildBedCpt != null)
                {
                    //如果没有关门分派一个接待
                    SetIntent(CustomerHotelIntentEnum.WaitAccost);
                    //创建订单
                    orderForHotel = InnHandler.Instance.CreateOrderForHotel(this, buildBedCpt);
                    //记录
                    InnHandler.Instance.RecordCustomer(orderForHotel);
                    characterShoutCpt.Shout(TextHandler.Instance.manager.GetTextById(13401));
                }
                else
                {
                    SetIntent(CustomerHotelIntentEnum.Leave);
                }
            }
            else
                SetIntent(CustomerHotelIntentEnum.Leave);
        }
    }

    /// <summary>
    /// 处理-到达楼梯
    /// </summary>
    public void HandleForGoToStairsForFirst()
    {
        if (characterMoveCpt.IsAutoMoveStop())
        {
            transform.position = orderForHotel.layerSecondStairsPosition;
            SetIntent(CustomerHotelIntentEnum.GoToBed);
        }
    }

    /// <summary>
    /// 处理-到达楼梯
    /// </summary>
    public void HandleForGoToStairsForSecond()
    {
        if (characterMoveCpt.IsAutoMoveStop())
        {
            transform.position = orderForHotel.layerFirstStairsPosition;
            if (orderForHotel.GetOrderStatus() == OrderHotelStatusEnum.Pay)
            {
                SetIntent(CustomerHotelIntentEnum.GoToPay);
            }
            else
            {
                SetIntent(CustomerHotelIntentEnum.Leave);
            }
        }
    }

    /// <summary>
    /// 处理-到达床
    /// </summary>
    public void HandleForGoToBed()
    {
        if (characterMoveCpt.IsAutoMoveStop())
        {
            SetIntent(CustomerHotelIntentEnum.Sleep);
        }
    }

    /// <summary>
    /// 前往支付
    /// </summary>
    public void HandleForGoToPay()
    {
        if (characterMoveCpt.IsAutoMoveStop())
        {
            SetIntent(CustomerHotelIntentEnum.WaitPay);
        }
    }


    /// <summary>
    /// 协程-开始睡觉
    /// </summary>
    /// <param name="sleepTime"></param>
    /// <returns></returns>
    public IEnumerator CoroutineForStartSleep(int sleepTime)
    {
        Sprite spDaze = IconDataHandler.Instance.manager.GetIconSpriteByName("daze_1");
        string markId = SystemUtil.GetUUID(SystemUtil.UUIDTypeEnum.N);
        AddStatusIconForPro(spDaze, null, markId);
        yield return new WaitForSeconds(sleepTime * 60);
        orderForHotel.SetOrderStatus(OrderHotelStatusEnum.Pay);
        orderForHotel.bed.SetBedStatus(BuildBedCpt.BedStatusEnum.WaitClean);
        RemoveStatusIconByType(CharacterStatusIconEnum.Pro);

        SetCharacterData(characterData);
        SetCharacterLive();

        SetIntent(CustomerHotelIntentEnum.GoToStairsForSecond);
        InnHandler.Instance.bedCleanQueue.Add(orderForHotel);
    }

    /// <summary>
    /// 添加等待图标
    /// </summary>
    public void AddWaitIcon()
    {
        //添加等待图标
        string waitIconMarkId = SystemUtil.GetUUID(SystemUtil.UUIDTypeEnum.N);
        Sprite spWaitIcon = IconDataHandler.Instance.manager.GetIconSpriteByName("worker_waiter_bed_pro_2");
        //Sprite spWaitIcon = IconDataHandler.Instance.manager.GetIconSpriteByName("time_wait_1_0");
        //AddStatusIconForPro(spWaitIcon, waitIconAnim, waitIconMarkId);
        AddStatusIconForPro(spWaitIcon, null, waitIconMarkId);
    }

    /// <summary>
    /// 添加支付图标
    /// </summary>
    public void AddPayIcon()
    {
        //添加等待图标
        string payIconMarkId = SystemUtil.GetUUID(SystemUtil.UUIDTypeEnum.N);
        Sprite spPayIcon = IconDataHandler.Instance.manager.GetIconSpriteByName("money_1");
        AddStatusIconForPro(spPayIcon, null, payIconMarkId);
    }
}