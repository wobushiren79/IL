using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.AI;
using System.Collections.Generic;

public class NpcAICustomerCpt : BaseNpcAI
{
    public CustomerTypeEnum customerType;

    public enum CustomerNotifyEnum
    {
        StatusChange = 1,//状态改变
    }

    public enum CustomerIntentEnum
    {
        None=-1,
        Walk = 0,//路过
        Want = 1,//想要吃饭
        WaitSeat = 2,//等待座位
        GotoSeat = 3,//前往座位
        WaitFood = 4,//等待食物
        Eatting = 5,//吃
        GotoPay = 6,//去付钱
        WaitPay = 7,//等待付钱
        Pay = 8,//正在付钱
        Leave = 10,//离开
        WaitAccost = 11,//等待招待
        TalkWithAccost = 12,//和招待交流
    }

    public CustomerIntentEnum customerIntent = CustomerIntentEnum.Walk;//意图 顾客： 1路过 2思考 3进店 4找座位 5点菜 6吃 7结账 

    //例子效果容器
    public GameObject objEffectContainer;
    //表情控制
    public CharacterMoodCpt characterMoodCpt;

    //移动目标点
    public Vector3 movePosition;

    //根据客人点餐生成的点餐数据
    protected OrderForCustomer orderForCustomer;

    //等待座位的时间
    public float timeWaitSeat = 20;

    //吃饭动画
    public RuntimeAnimatorController eatIconAnim;
    //等待动画
    public RuntimeAnimatorController waitIconAnim;

    public override void Awake()
    {
        base.Awake();
        customerType = CustomerTypeEnum.Normal;
    }

    protected float timeIntentUpdate = 0;
    protected float timeIntentUpdateOffset = 0.2f;
    public void Update()
    {
        if (timeIntentUpdate > 0)
        {
            timeIntentUpdate -= Time.deltaTime;
            return;
        }
        timeIntentUpdate = timeIntentUpdateOffset;
        switch (customerIntent)
        {
            case CustomerIntentEnum.Walk:
            case CustomerIntentEnum.Leave:
                HandleForLeave();
                break;
            case CustomerIntentEnum.Want:
                if (characterMoveCpt.IsAutoMoveStop())
                {
                    //判断点是否关门
                    if (InnHandler.Instance.GetInnStatus() == InnHandler.InnStatusEnum.Open)
                        SetIntent(CustomerIntentEnum.WaitSeat);
                    else
                        SetIntent(CustomerIntentEnum.Leave);
                }
                break;
            case CustomerIntentEnum.GotoSeat:
                HandleForOrderFood();
                break;
            case CustomerIntentEnum.WaitFood:
                ChangeMood(-timeIntentUpdateOffset);
                break;
            case CustomerIntentEnum.GotoPay:
                if (characterMoveCpt.IsAutoMoveStop())
                {
                    SetIntent(CustomerIntentEnum.WaitPay);
                }
                break;
            case CustomerIntentEnum.WaitPay:
                ChangeMood(-timeIntentUpdateOffset);
                break;
        }
    }

    /// <summary>
    /// 获取顾客状态
    /// </summary>
    /// <param name="intentStr"></param>
    /// <returns></returns>
    public virtual CustomerIntentEnum GetCustomerStatus(out string intentStr)
    {
        intentStr = "???";
        switch (customerIntent)
        {
            case CustomerIntentEnum.Walk:
                intentStr = TextHandler.Instance.manager.GetTextById(151);
                break;
            case CustomerIntentEnum.Want:
                intentStr = TextHandler.Instance.manager.GetTextById(152);
                break;
            case CustomerIntentEnum.WaitSeat:
                intentStr = TextHandler.Instance.manager.GetTextById(153);
                break;
            case CustomerIntentEnum.GotoSeat:
                intentStr = TextHandler.Instance.manager.GetTextById(154);
                break;
            case CustomerIntentEnum.WaitFood:
                intentStr = TextHandler.Instance.manager.GetTextById(155);
                break;
            case CustomerIntentEnum.Eatting:
                intentStr = TextHandler.Instance.manager.GetTextById(156);
                break;
            case CustomerIntentEnum.GotoPay:
                intentStr = TextHandler.Instance.manager.GetTextById(157);
                break;
            case CustomerIntentEnum.WaitPay:
                intentStr = TextHandler.Instance.manager.GetTextById(158);
                break;
            case CustomerIntentEnum.Pay:
                intentStr = TextHandler.Instance.manager.GetTextById(159);
                break;
            case CustomerIntentEnum.Leave:
                intentStr = TextHandler.Instance.manager.GetTextById(160);
                break;
            case CustomerIntentEnum.WaitAccost:
                intentStr = TextHandler.Instance.manager.GetTextById(161);
                break;
        }
        return customerIntent;
    }

    /// <summary>
    /// 获取订单信息
    /// </summary>
    /// <returns></returns>
    public OrderForCustomer GetOrderForCustomer()
    {
        return orderForCustomer;
    }

    /// <summary>
    /// 设置订单信息
    /// </summary>
    /// <param name="order"></param>
    public void SetOrderForCustomer(OrderForCustomer order)
    {
        orderForCustomer = order;
    }

    /// <summary>
    /// 离开处理
    /// </summary>
    public virtual void HandleForLeave()
    {
        //到目标点就删除
        if (!characterMoveCpt.IsAutoMoveStop())
            return;
        SetIntent(CustomerIntentEnum.None);
        gameObject.SetActive(false);
        gameObject.transform.position = new Vector3(0, -10000, 0);
        orderForCustomer = new OrderForCustomer(CustomerTypeEnum.Normal,this);
        timeWaitSeat = 20;
        NpcHandler.Instance.builderForCustomer.listHideNpc.Enqueue(gameObject);
    }

    /// <summary>
    /// 点餐
    /// </summary>
    public virtual void HandleForOrderFood()
    {
        if (!characterMoveCpt.IsAutoMoveStop())
            return;
        //首先调整修改朝向
        SetCharacterFace(orderForCustomer.table.GetUserFace());
        //点餐
        InnHandler.Instance.OrderForFood(orderForCustomer);
        if (orderForCustomer.foodData == null)
        {
            //如果没有菜品出售 心情直接降100 
            ChangeMood(-99999);
            characterShoutCpt.Shout(TextHandler.Instance.manager.GetTextById(13002));
        }
        else
        {
            //喊出需要的菜品
            characterShoutCpt.Shout(orderForCustomer.foodData.name);
            //设置等待食物
            SetIntent(CustomerIntentEnum.WaitFood);
        }
    }

    /// <summary>
    /// 根据意图设置目的地
    /// </summary>
    public void SetIntent(CustomerIntentEnum intent)
    {
        SetIntent(intent, null);
    }

    public void SetIntent(CustomerIntentEnum intent, OrderForCustomer orderForCustomer)
    {
        //删除进度图标
        RemoveStatusIconByType(CharacterStatusIconEnum.Pro);
        //停止所有进程
        if (this)
            StopAllCoroutines();
        this.customerIntent = intent;
        switch (customerIntent)
        {
            case CustomerIntentEnum.Walk:
                IntentForWalk();
                break;
            case CustomerIntentEnum.Want:
                IntentForWant();
                break;
            case CustomerIntentEnum.WaitSeat:
                IntentForWaitSeat();
                break;
            case CustomerIntentEnum.GotoSeat:
                IntentForGoToSeat(orderForCustomer);
                break;
            case CustomerIntentEnum.WaitFood:
                IntentForWaitFood();
                break;
            case CustomerIntentEnum.Eatting:
                IntentForEatting();
                break;
            case CustomerIntentEnum.GotoPay:
                IntentForGotoPay();
                break;
            case CustomerIntentEnum.WaitPay:
                IntentForWaitPay();
                break;
            case CustomerIntentEnum.Leave:
                IntentForLeave();
                break;
            case CustomerIntentEnum.WaitAccost:
                IntentForWaitAccost();
                break;
        }
        NotifyAllObserver((int)CustomerNotifyEnum.StatusChange, (int)intent);
    }

    /// <summary>
    /// 意图-散步
    /// </summary>
    public void IntentForWalk()
    {
        SceneInnManager sceneInnManager = GameScenesHandler.Instance.manager.GetSceneManager<SceneInnManager>();
        if (transform.position.x > 0)
        {
            //如果角色在右边生成 出口则设置为左边
            movePosition = sceneInnManager.GetRandomSceneExportPosition(1);
        }
        else
        {
            //如果角色在左边生成 出口则设置为右边
            movePosition = sceneInnManager.GetRandomSceneExportPosition(0);
        }
        characterMoveCpt.SetDestination(movePosition);
    }

    /// <summary>
    /// 意图-想要就餐
    /// </summary>
    public virtual void IntentForWant()
    {
        movePosition = InnHandler.Instance.GetRandomEntrancePosition();
        //移动到门口附近
        if (movePosition == null || movePosition == Vector3.zero)
        {
            //如果找不到门则离开 散散步
            SetIntent(CustomerIntentEnum.Leave);
        }
        else
            //前往门
            characterMoveCpt.SetDestination(movePosition);
    }

    /// <summary>
    /// 意图-排队就餐
    /// </summary>
    public virtual void IntentForWaitSeat()
    {
        //加入排队队伍
        //添加一个订单
        OrderForCustomer orderForCustomer = InnHandler.Instance.CreateOrder(this);
        InnHandler.Instance.cusomerQueue.Add(orderForCustomer);
        StartCoroutine(CoroutineForStartWaitSeat());
    }

    /// <summary>
    /// 意图-前往餐桌
    /// </summary>
    /// <param name="buildTableCpt"></param>
    public void IntentForGoToSeat(OrderForCustomer orderForCustomer)
    {
        //停止等待
        if (gameObject == null)
        {
            return;
        }
        StopAllCoroutines();
        this.orderForCustomer = orderForCustomer;
        //判断路径是否有效
        if (CheckUtil.CheckPath(transform.position, orderForCustomer.table.GetSeatPosition()))
        {
            //开启满意度
            characterMoodCpt.SetMood(orderForCustomer.innEvaluation.GetPraise());
            //前往桌子
            movePosition = orderForCustomer.table.GetSeatPosition();
            characterMoveCpt.SetDestination(movePosition);
            //记录该顾客
            InnHandler.Instance.RecordCustomer(orderForCustomer);
        }
        else
        {
            SetMood(30);
            EndOrderAndLeave();
        }
    }

    /// <summary>
    /// 意图-等待食物
    /// </summary>
    public void IntentForWaitFood()
    {
        if (orderForCustomer.table != null)
            orderForCustomer.table.SetTableStatus(BuildTableCpt.TableStatusEnum.WaitFood);
    }

    /// <summary>
    /// 意图-吃饭中
    /// </summary>
    public void IntentForEatting()
    {
        //停止等待
        StopAllCoroutines();
        //好感+
        switch (orderForCustomer.foodLevel)
        {
            case -1:
                ChangeMood(-20);
                break;
            case 1:
                ChangeMood(20);
                break;
            case 2:
                ChangeMood(40);
                break;
        }
        //设置桌子状态
        if (orderForCustomer.table != null)
            orderForCustomer.table.SetTableStatus(BuildTableCpt.TableStatusEnum.Eating);
        //开始吃
        StartCoroutine(CoroutineForStartEat());
    }

    /// <summary>
    /// 意图-去结账
    /// </summary>
    public void IntentForGotoPay()
    {
        orderForCustomer.counter = InnHandler.Instance.GetCounter(transform.position);
        //如果判断有无结算台
        if (orderForCustomer.counter == null)
        {
            InnHandler.Instance.EndOrderForForce(orderForCustomer, true);
            SetIntent(CustomerIntentEnum.Leave);
        }
        else
        {
            movePosition = orderForCustomer.counter.GetPayPosition();
            //if (!CheckUtil.CheckPath(transform.position, movePosition))
            //{
            //    InnHandler.Instance.EndOrderForForce(orderForCustomer, true);
            //    SetIntent(CustomerIntentEnum.Leave);
            //}
            //else
            //{
            //    characterMoveCpt.SetDestination(movePosition);
            //}
            characterMoveCpt.SetDestination(movePosition);
        }
    }

    /// <summary>
    /// 意图-等待结算
    /// </summary>
    public void IntentForWaitPay()
    {
        AddPayIcon();
        orderForCustomer.counter.payQueue.Add(orderForCustomer);
    }

    /// <summary>
    /// 意图-离开
    /// </summary>
    public virtual void IntentForLeave()
    {
        //随机获取一个退出点
        SceneInnManager sceneInnManager = GameScenesHandler.Instance.manager.GetSceneManager<SceneInnManager>();
        movePosition = sceneInnManager.GetRandomSceneExportPosition();
        characterMoveCpt.SetDestination(movePosition);
    }

    /// <summary>
    /// 意图-等待招待过来
    /// </summary>
    public virtual void IntentForWaitAccost()
    {
        SetExpression(CharacterExpressionCpt.CharacterExpressionEnum.Surprise);
        StopMove();
    }

    /// <summary>
    /// 通知不能完成食物
    /// </summary>
    public void SendForCanNotCook()
    {
        StopAllCoroutines();
        ChangeMood(-99999);
    }

    /// <summary>
    /// 改变心情
    /// </summary>
    /// <param name="mood"></param>
    public virtual void ChangeMood(float mood)
    {
        if (orderForCustomer == null)
            return;
        orderForCustomer.innEvaluation.AddMood(mood);
        characterMoodCpt.SetMood(orderForCustomer.innEvaluation.GetPraise());
        if (orderForCustomer.innEvaluation.mood <= 0)
        {
            //如果有订单。强制结束订单
            EndOrderAndLeave();
        }
    }

    /// <summary>
    /// 设置心情
    /// </summary>
    /// <param name="mood"></param>
    public virtual void SetMood(float mood)
    {
        if (orderForCustomer == null)
            return;
        orderForCustomer.innEvaluation.SetMood(mood);
        characterMoodCpt.SetMood(orderForCustomer.innEvaluation.GetPraise());
        if (orderForCustomer.innEvaluation.mood <= 0)
        {
            //如果有订单。强制结束订单
            EndOrderAndLeave();
        }
    }

    /// <summary>
    /// 结束订单并且离开
    /// </summary>
    public virtual void EndOrderAndLeave()
    {
        if (orderForCustomer != null)
        {
            StopAllCoroutines();
            InnHandler.Instance.EndOrderForForce(orderForCustomer, true);
            SetIntent(CustomerIntentEnum.Leave);
        }
    }

    /// <summary>
    /// 开始吃计时
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoroutineForStartEat()
    {
        //添加吃饭图标
        string eatIconMarkId = SystemUtil.GetUUID(SystemUtil.UUIDTypeEnum.N);
        Sprite spEatIcon = IconDataHandler.Instance.manager.GetIconSpriteByName("customer_eat_pro_0");
        AddStatusIconForPro(spEatIcon, eatIconAnim, eatIconMarkId);
        //播放吃饭动画
        characterMoveCpt.characterAnimtor.SetTrigger("Eat");
        // 播放音效
        AudioHandler.Instance.PlaySound(AudioSoundEnum.Eat);
        float eatTime = Random.Range(3f, 7f);
        yield return new WaitForSeconds(eatTime);
        if (isActiveAndEnabled)
        {
            //吃完食物
            orderForCustomer.foodCpt.FinishFood(orderForCustomer.foodData);
            //设置桌子为待清理
            if (orderForCustomer.table != null)
            {
                orderForCustomer.table.SetTableStatus(BuildTableCpt.TableStatusEnum.WaitClean);
                //清理列队增加
                InnHandler.Instance.cleanQueue.Add(orderForCustomer);
            }
            //去结账
            SetIntent(CustomerIntentEnum.GotoPay);
        }
    }

    /// <summary>
    /// 开始等待就餐计时
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerator CoroutineForStartWaitSeat()
    {
        AddWaitIcon();
        yield return new WaitForSeconds(timeWaitSeat);
        InnHandler.Instance.EndOrderForForce(orderForCustomer, false);
        SetIntent(CustomerIntentEnum.Leave);
    }

    /// <summary>
    /// 添加等待图标
    /// </summary>
    public void AddWaitIcon()
    {
        //添加等待图标
        string waitIconMarkId = SystemUtil.GetUUID(SystemUtil.UUIDTypeEnum.N);
        Sprite spWaitIcon = IconDataHandler.Instance.manager.GetIconSpriteByName("time_wait_1_0");
        AddStatusIconForPro(spWaitIcon, waitIconAnim, waitIconMarkId);
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