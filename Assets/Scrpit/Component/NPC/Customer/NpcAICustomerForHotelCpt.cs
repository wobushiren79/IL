using UnityEngine;
using UnityEditor;
using System.Collections;
public class NpcAICustomerForHotelCpt : BaseNpcAI
{
    public enum CustomerHotelIntentEnum
    {
        Walk = 0,//路过
        GoToInn = 1,//前往客栈
        WaitAccost=2,
        GoToStairsForFirst=3, // 前往一楼楼梯
        GoToBed=4,
        GoToStairsForSecond=5,
        Sleep =6,
        GoToPay=7,
        WaitPay =8,
        Leave = 99,//离开
    }

    public CustomerHotelIntentEnum customerHotelIntent;
    //移动目标点
    public Vector3 movePosition;

    //客栈处理
    protected InnHandler innHandler;
    //客栈区域数据管理
    protected SceneInnManager sceneInnManager;

    public OrderForHotel orderForHotel;

    //等待动画
    public RuntimeAnimatorController waitIconAnim;

    public override  void Awake()
    {
        base.Awake();
        sceneInnManager = Find<SceneInnManager>(ImportantTypeEnum.SceneManager);
        innHandler = Find<InnHandler>(ImportantTypeEnum.InnHandler);
    }

    public void Update()
    {
        switch (customerHotelIntent)
        {
            case CustomerHotelIntentEnum.Walk:
            case CustomerHotelIntentEnum.Leave:
                HandleForLeave();
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
        }
    }

    public void SetIntent(CustomerHotelIntentEnum intent)
    {        
        //删除进度图标
        RemoveStatusIconByType(CharacterStatusIconEnum.Pro);
        if(this)
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
        }

    }

    /// <summary>
    /// 意图-散步
    /// </summary>
    public void IntentForWalk()
    {
        if (sceneInnManager == null)
            return;
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
        movePosition = sceneInnManager.GetRandomSceneExportPosition();
        characterMoveCpt.SetDestination(movePosition);
    }

    /// <summary>
    /// 意图-离开
    /// </summary>
    public void IntentForGoToInn()
    {
        movePosition = innHandler.GetRandomEntrancePosition();
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
        StartCoroutine(CoroutineForStartWaitAccost());
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
            innHandler.EndOrderForForce(orderForHotel);
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
        Direction2DEnum directionBed= orderForHotel.bed.GetDirection();
        SetCharacterSleep(directionBed);
        //开始睡觉
        StartCoroutine(CoroutineForStartSleep(orderForHotel.sleepTime));
    }


    /// <summary>
    /// 离开处理
    /// </summary>
    public virtual void HandleForLeave()
    {
        //到目标点就删除
        if (!characterMoveCpt.IsAutoMoveStop())
            return;
        Destroy(gameObject);
    }

    /// <summary>
    /// 前往客栈处理
    /// </summary>
    public virtual void HandleForGoToInn()
    {
        if (characterMoveCpt.IsAutoMoveStop())
        {
            //判断点是否关门
            if (innHandler.GetInnStatus() == InnHandler.InnStatusEnum.Open)
            {
                //首先判断还有没有床位
                BuildBedCpt buildBedCpt= innHandler.GetIdleBed();
                if (buildBedCpt != null)
                {
                    //如果没有关门分派一个接待
                    SetIntent(CustomerHotelIntentEnum.WaitAccost);
                    //创建订单
                    orderForHotel = innHandler.CreateOrderForHotel(this,buildBedCpt);
                    characterShoutCpt.Shout(GameCommonInfo.GetUITextById(13401));      
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
            SetIntent(CustomerHotelIntentEnum.Leave);
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
    /// 协程-开始等待接待
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoroutineForStartWaitAccost()
    {
        AddWaitIcon();
        yield return new WaitForSeconds(30);
        innHandler.EndOrderForForce(orderForHotel);
    }

    /// <summary>
    /// 协程-开始睡觉
    /// </summary>
    /// <param name="sleepTime"></param>
    /// <returns></returns>
    public IEnumerator CoroutineForStartSleep(int sleepTime)
    {
        Sprite spDaze = iconDataManager.GetIconSpriteByName("daze_1");
        string markId = SystemUtil.GetUUID(SystemUtil.UUIDTypeEnum.N);
        AddStatusIconForPro(spDaze, null, markId);
        yield return new WaitForSeconds(sleepTime * 60);
        RemoveStatusIconByType(CharacterStatusIconEnum.Pro);
        SetCharacterLive();
        SetIntent(CustomerHotelIntentEnum.GoToStairsForSecond);
    }

    /// <summary>
    /// 添加等待图标
    /// </summary>
    public void AddWaitIcon()
    {
        //添加等待图标
        string waitIconMarkId = SystemUtil.GetUUID(SystemUtil.UUIDTypeEnum.N);
        Sprite spWaitIcon = iconDataManager.GetIconSpriteByName("time_wait_1_0");
        AddStatusIconForPro(spWaitIcon, waitIconAnim, waitIconMarkId);
    }
}