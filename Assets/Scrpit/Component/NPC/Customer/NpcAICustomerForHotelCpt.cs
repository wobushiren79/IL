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
        Sleep=5,
        GoToPay=6,
        WaitPay =7,
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
                IntentForGoToStairsForFirst();
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
    public void IntentForGoToStairsForFirst()
    {
        SetCharacterMove(orderForHotel.layerFirstStairsPosition);
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