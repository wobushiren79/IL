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

    public CustomerIntentEnum intentType = CustomerIntentEnum.Walk;//意图 顾客： 1路过 2思考 3进店 4找座位 5点菜 6吃 7结账 

    //表情控制
    public CharacterMoodCpt characterMoodCpt;
    //喊叫控制
    public CharacterShoutCpt characterShoutCpt;
    //评价系统
    public InnEvaluationBean innEvaluation = new InnEvaluationBean();

    //客栈处理
    public InnHandler innHandler;
    //终点
    public Vector3 endPosition;
    public Vector3 doorPosition;
    //等到的位置
    public BuildTableCpt tableForEating;
    //做好的食物
    public FoodForCustomerCpt foodCpt;
    //支付的地方
    public BuildCounterCpt counterCpt;
    //等待支付时距离柜台的距离
    public float waitPayDistance;
    private void FixedUpdate()
    {
        switch (intentType)
        {
            case CustomerIntentEnum.Walk:
            case CustomerIntentEnum.Leave:
                if (Vector2.Distance(transform.position, endPosition) < 3)
                {
                    Destroy(gameObject);
                }
                break;
            case CustomerIntentEnum.Want:
                if (Vector2.Distance(transform.position, doorPosition) < 0.1f)
                {
                    if (innHandler.innStatus == InnHandler.InnStatusEnum.Open)
                    {
                        StopMove();
                        SetDestinationByIntent(CustomerIntentEnum.WaitSeat);
                    }
                    else
                    {
                        SetDestinationByIntent(CustomerIntentEnum.Leave);
                    }
                }
                break;
            case CustomerIntentEnum.GotoSeat:
                if (Vector2.Distance(transform.position, tableForEating.GetSeatPosition()) < 0.1f)
                {
                    SetDestinationByIntent(CustomerIntentEnum.WaitFood);
                    MenuInfoBean foodData = innHandler.OrderForFood(this, tableForEating);
                    if (foodData == null)
                    {
                        MoodGet(-100);
                        tableForEating.ClearTable();
                        SetDestinationByIntent(CustomerIntentEnum.Leave);
                    }
                    else
                    {
                        characterShoutCpt.Shout(foodData.name);
                    }  
                }
                break;
            case CustomerIntentEnum.GotoPay:
                if (Vector2.Distance(transform.position, counterCpt.GetPayPosition()) < waitPayDistance)
                {
                    characterMoveCpt.StopAutoMove();
                    SetDestinationByIntent(CustomerIntentEnum.WaitPay);
                    counterCpt.payQueue.Add(this);
                }
                break;
            case CustomerIntentEnum.WaitFood:
                MoodLose();
                if (innEvaluation.mood <= 0)
                {
                    tableForEating.ClearTable();
                }    
                break;
            case CustomerIntentEnum.WaitPay:
                MoodLose();
                if (innEvaluation.mood <= 0)
                {
                    innHandler.PayMoney(foodCpt,0.5f);
                }
                break;
        }
    }

    /// <summary>
    /// 通知不能完成食物
    /// </summary>
    public void SendForCanNotCook()
    {
        StopAllCoroutines();
        MoodGet(-100);
        tableForEating.ClearTable();
        SetDestinationByIntent(CustomerIntentEnum.Leave);
    }

    /// <summary>
    /// 心情递减
    /// </summary>
    public void MoodLose()
    {
        innEvaluation.mood -= Time.deltaTime;
        characterMoodCpt.SetMood(innEvaluation.mood);
        if (innEvaluation.mood <= 0)
        {
            SetDestinationByIntent(CustomerIntentEnum.Leave);
        }
    }

    public void MoodGet(float mood)
    {
        innEvaluation.mood += mood;
        characterMoodCpt.SetMood(innEvaluation.mood);
    }

    /// <summary>
    /// 设置餐桌
    /// </summary>
    /// <param name="buildTableCpt"></param>
    public void SetTable(BuildTableCpt buildTableCpt)
    {
        StopAllCoroutines();
        this.tableForEating = buildTableCpt;
        SetDestinationByIntent(CustomerIntentEnum.GotoSeat);
    }

    /// <summary>
    /// 设置终点
    /// </summary>
    /// <param name="endPosition"></param>
    public void SetEndPosition(Vector3 endPosition)
    {
        this.endPosition = endPosition;
    }

    /// <summary>
    /// 根据意图设置目的地
    /// </summary>
    /// 
    public void SetDestinationByIntent(CustomerIntentEnum intent, FoodForCustomerCpt foodCpt)
    {
        StopAllCoroutines();
        this.foodCpt = foodCpt;
        SetDestinationByIntent(intent);
    }

    public void SetDestinationByIntent(CustomerIntentEnum intent)
    {
        this.intentType = intent;
        switch (intentType)
        {
            case CustomerIntentEnum.Walk:
                characterMoveCpt.SetDestination(endPosition);
                break;
            case CustomerIntentEnum.Want:
                //移动到门口附近
                Vector3 door = RandomUtil.GetRandomDataByList(innHandler.GetEntrancePositionList());
                if (door == null || door == Vector3.zero)
                {
                    SetDestinationByIntent(CustomerIntentEnum.Leave);
                }
                else
                {
                    doorPosition = new Vector3(Random.Range(door.x - 0.5f, door.x + 0.5f),Random.Range(door.y - 0.7f, door.y - 0.2f));
                    characterMoveCpt.SetDestination(doorPosition);
                }
                break;
            case CustomerIntentEnum.WaitSeat:
                //加入排队队伍
                innHandler.cusomerQueue.Add(this);
                innHandler.cusomerList.Add(this);
                StartCoroutine(StartWaitSeat());
                break;
            case CustomerIntentEnum.GotoSeat:
                //判断路径是否有效
                if (CheckUtil.CheckPath(transform.position, tableForEating.GetSeatPosition()))
                {
                    characterMoodCpt.OpenMood();
                    characterMoodCpt.SetMood(innEvaluation.mood);
                    characterMoveCpt.SetDestination(tableForEating.GetSeatPosition());
                }
                else
                {
                    if (tableForEating != null)
                        tableForEating.tableState = BuildTableCpt.TableStateEnum.Idle;
                    SetDestinationByIntent(CustomerIntentEnum.Leave);
                }
                break;
            case CustomerIntentEnum.Eatting:
                MoodGet(20);
                StartCoroutine(StartEat());
                break;
            case CustomerIntentEnum.GotoPay:
                counterCpt = innHandler.GetCounter();
                if (counterCpt == null)
                {
                    SetDestinationByIntent(CustomerIntentEnum.Leave);
                }
                else
                {
                    characterMoveCpt.SetDestination(counterCpt.GetPayPosition());
                }

                break;
            case CustomerIntentEnum.Leave:
                if (innHandler.cusomerQueue.Contains(this))
                {
                    innHandler.cusomerQueue.Remove(this);
                }
                if (innHandler.cusomerList.Contains(this))
                {
                    innHandler.cusomerList.Remove(this);
                }
                innHandler.CanelOrder(this);
                characterMoveCpt.SetDestination(endPosition);
                break;
        }
    }

    /// <summary>
    /// 开始吃
    /// </summary>
    /// <returns></returns>
    public IEnumerator StartEat()
    {
        yield return new WaitForSeconds(5);
        waitPayDistance = Random.Range(0.1f, 0.5f);
        SetDestinationByIntent(CustomerIntentEnum.GotoPay);
        foodCpt.FinishFood();
        innHandler.clearQueue.Add(foodCpt);
    }

    /// <summary>
    /// 开始等待
    /// </summary>
    /// <returns></returns>
    public IEnumerator StartWaitSeat()
    {
        yield return new WaitForSeconds(20);
        SetDestinationByIntent(CustomerIntentEnum.Leave);
    }
}