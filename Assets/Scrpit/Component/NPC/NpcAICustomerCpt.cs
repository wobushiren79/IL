using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.AI;
using System.Collections.Generic;

public class NpcAICustomerCpt : BaseNpcAI
{
    public enum CustomerIntentEnum
    {
        Walk = 0,//路过
        Want = 1,//想要吃饭
        WaitSeat = 2,//等待座位
        GotoSeat = 3,//前往座位
        WaitFood = 4,//等待食物
        Eatting = 5,//吃
        GotoPay = 6,//去付钱
        WaitPay = 7,//等待付钱
        Pay = 8,//正在付钱
        Leave //离开
    }

    public CustomerIntentEnum customerIntent= CustomerIntentEnum.Walk;//意图 顾客： 1路过 2思考 3进店 4找座位 5点菜 6吃 7结账 

    //表情控制
    public CharacterMoodCpt characterMoodCpt;
    //喊叫控制
    public CharacterShoutCpt characterShoutCpt;

    //客栈处理
    public InnHandler innHandler;
    //客栈区域数据管理
    public SceneInnManager sceneInnManager;

    //移动目标点
    public Vector3 movePosition;

    //根据客人点餐生成的点餐数据
    public OrderForCustomer orderForCustomer;

    //等待座位的时间
    public float timeWaitSeat = 20;
    //评价数据
    public InnEvaluationBean innEvaluation = new InnEvaluationBean();

    private void FixedUpdate()
    {
        switch (customerIntent)
        {
            case CustomerIntentEnum.Walk:
            case CustomerIntentEnum.Leave:
                // if ( Vector2.Distance(transform.position, endPosition) < 3)
                //到目标点就删除
                if (characterMoveCpt.IsAutoMoveStop())
                    Destroy(gameObject);
                break;
            case CustomerIntentEnum.Want:
                if (characterMoveCpt.IsAutoMoveStop())
                {
                    //判断点是否关门
                    if (innHandler.innStatus == InnHandler.InnStatusEnum.Open)
                        SetIntent(CustomerIntentEnum.WaitSeat);
                    else
                        SetIntent(CustomerIntentEnum.Leave);
                }
                break;
            case CustomerIntentEnum.GotoSeat:
                if (characterMoveCpt.IsAutoMoveStop())
                {
                    //修改朝向
                    SetCharacterFace(orderForCustomer.table.GetUserFace());
                    //点餐
                    innHandler.OrderForFood(orderForCustomer);
                    if (orderForCustomer.foodData == null)
                    {
                        //如果没有菜品出售 心情直接降100 
                        ChangeMood(-100);
                        //离开
                        SetIntent(CustomerIntentEnum.Leave);
                    }
                    else
                    {
                        //喊出需要的菜品
                        characterShoutCpt.Shout(orderForCustomer.foodData.name);
                        //设置等待食物
                        SetIntent(CustomerIntentEnum.WaitFood);
                    }
                }
                break;
            case CustomerIntentEnum.WaitFood:
                ChangeMood(-Time.deltaTime);
                break;
            case CustomerIntentEnum.GotoPay:
                if (characterMoveCpt.IsAutoMoveStop())
                {
                    SetIntent(CustomerIntentEnum.WaitPay);
                }
                break;
            case CustomerIntentEnum.WaitPay:
                ChangeMood(-Time.deltaTime);
                break;
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
    /// 意图-想要就餐
    /// </summary>
    public void IntentForWant()
    {
        //移动到门口附近
        Vector3 doorPosition = innHandler.GetRandomEntrancePosition();
        if (doorPosition == null || doorPosition == Vector3.zero)
            //如果找不到门则离开 散散步
            SetIntent(CustomerIntentEnum.Walk);
        else
            //前往门
            characterMoveCpt.SetDestination(doorPosition);
    }

    /// <summary>
    /// 意图-排队就餐
    /// </summary>
    public void IntentForWaitSeat()
    {
        //加入排队队伍
        innHandler.cusomerQueue.Add(this);
        StartCoroutine(StartWaitSeat());
    }

    /// <summary>
    /// 意图-前往餐桌
    /// </summary>
    /// <param name="buildTableCpt"></param>
    public void IntentForGoToSeat(OrderForCustomer orderForCustomer)
    {
        //停止等待
        StopAllCoroutines();
        this.orderForCustomer = orderForCustomer;
        //判断路径是否有效
        if (CheckUtil.CheckPath(transform.position, orderForCustomer.table.GetSeatPosition()))
        {
            //开启满意度
            characterMoodCpt.OpenMood();
            characterMoodCpt.SetMood(innEvaluation.mood);
            //前往桌子
            movePosition = orderForCustomer.table.GetSeatPosition();
            characterMoveCpt.SetDestination(movePosition);
        }
        else
        {
            SetIntent(CustomerIntentEnum.Leave);
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
        //好感+20
        ChangeMood(20);
        //设置桌子状态
        if (orderForCustomer.table != null)
            orderForCustomer.table.SetTableStatus(BuildTableCpt.TableStatusEnum.Eating);
        //开始吃
        StartCoroutine(StartEat());
    }

    /// <summary>
    /// 意图-去结账
    /// </summary>
    public void IntentForGotoPay()
    {
        orderForCustomer.counter = innHandler.GetCounter();
        //如果判断有无结算台
        if (orderForCustomer.counter == null)
        {
            SetIntent(CustomerIntentEnum.Leave);
        }
        else
        {
            movePosition = orderForCustomer.counter.GetPayPosition();
            characterMoveCpt.SetDestination(movePosition);
        }
    }

    /// <summary>
    /// 意图-等待结算
    /// </summary>
    public void IntentForWaitPay()
    {
        orderForCustomer.counter.payQueue.Add(orderForCustomer);
    }

    /// <summary>
    /// 意图-离开
    /// </summary>
    public void IntentForLeave()
    {
        //如果在排队则移除排队列表
        if (innHandler.cusomerQueue.Contains(this))
        {
            innHandler.cusomerQueue.Remove(this);
        }
        //如果在订单列表 则移除订单列表
        if (innHandler.orderList.Contains(orderForCustomer))
        {
            innHandler.orderList.Remove(orderForCustomer);
        }
        //随机获取一个退出点
        movePosition = sceneInnManager.GetRandomSceneExportPosition();
        characterMoveCpt.SetDestination(movePosition);
    }

    /// <summary>
    /// 通知不能完成食物
    /// </summary>
    public void SendForCanNotCook()
    {
        StopAllCoroutines();
        ChangeMood(-100);
    }

    /// <summary>
    /// 改变心情
    /// </summary>
    /// <param name="mood"></param>
    public void ChangeMood(float mood)
    {
        innEvaluation.mood += mood;
        characterMoodCpt.SetMood(innEvaluation.mood);
        if (innEvaluation.mood <= 0)
        {
            innHandler.EndOrderForForce(orderForCustomer);
            SetIntent(CustomerIntentEnum.Leave); 
        }
    }

    /// <summary>
    /// 开始吃计时
    /// </summary>
    /// <returns></returns>
    public IEnumerator StartEat()
    {
        yield return new WaitForSeconds(5);
        //吃完食物
        orderForCustomer.foodCpt.FinishFood(orderForCustomer.foodData);
        //设置桌子为待清理
        if (orderForCustomer.table != null)
            orderForCustomer.table.SetTableStatus(BuildTableCpt.TableStatusEnum.WaitClean);
        //清理列队增加
        innHandler.clearQueue.Add(orderForCustomer);
        //去结账
        SetIntent(CustomerIntentEnum.GotoPay);
    }

    /// <summary>
    /// 开始等待就餐计时
    /// </summary>
    /// <returns></returns>
    public IEnumerator StartWaitSeat()
    {
        yield return new WaitForSeconds(timeWaitSeat);
        SetIntent(CustomerIntentEnum.Leave);
    }
}