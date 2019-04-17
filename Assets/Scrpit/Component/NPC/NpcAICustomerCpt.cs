using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.AI;

public class NpcAICustomerCpt : BaseNpcAI
{
    public enum CustomerIntentEnum
    {
        Walk = 0,//路过
        Want = 1,//想要吃饭
        WaitSeat=2,//等待座位
        GotoSeat=3,//前往座位
        WaitFood=4,//等待食物
        Leave //离开
    }

    public CustomerIntentEnum intentType;//意图 顾客： 1路过 2思考 3进店 4找座位 5点菜 6吃 7结账 


    public CharacterShoutCpt characterShoutCpt;
    //客栈处理
    public InnHandler innHandler;
    //终点
    public Vector3 endPosition;
    public Vector3 doorPosition;
    //等到的位置
    public BuildTableCpt tableForEating;

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
                    StopMove();
                    intentType = CustomerIntentEnum.WaitSeat;
                    //加入排队队伍
                    innHandler.AddQueue(this);
                    //开始等待座位
                    StartCoroutine(StartWaitSeat());
                }
                break;
            case CustomerIntentEnum.GotoSeat:
                if (Vector2.Distance(transform.position, tableForEating.GetSeatPosition()) < 0.1f)
                {
                    SetDestinationByIntent(CustomerIntentEnum.WaitFood);
                    characterShoutCpt.Shout("麻婆豆腐");
                }
                break;
        }
    }

    /// <summary>
    /// 设置餐桌
    /// </summary>
    /// <param name="buildTableCpt"></param>
    public  void SetTable(BuildTableCpt buildTableCpt)
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
                doorPosition = new Vector3
                    (Random.Range(innHandler.doorPosition.x-1.5f, innHandler.doorPosition.x + 1.5f),
                    Random.Range(innHandler.doorPosition.y - 1.5f, innHandler.doorPosition.y + 1.5f));
                characterMoveCpt.SetDestination(doorPosition);
                break;
            case CustomerIntentEnum.GotoSeat:
                //判断路径是否有效
                NavMeshPath navpath = new NavMeshPath();
                NavMesh.CalculatePath(transform.position, tableForEating.GetSeatPosition(), -1, navpath);
                if (navpath.status == NavMeshPathStatus.PathPartial || navpath.status == NavMeshPathStatus.PathInvalid)
                {
                    SetDestinationByIntent(CustomerIntentEnum.Leave);
                }
                else
                    characterMoveCpt.SetDestination(tableForEating.GetSeatPosition());
              
                break;
            case CustomerIntentEnum.Leave:
                characterMoveCpt.SetDestination(endPosition);
                break;
        }
    }

    /// <summary>
    /// 开始等待座位
    /// </summary>
    /// <returns></returns>
    public IEnumerator StartWaitSeat()
    {
        yield return new WaitForSeconds(10);
        innHandler.RemoveQueue(this);
        SetDestinationByIntent(CustomerIntentEnum.Leave);
    }
}