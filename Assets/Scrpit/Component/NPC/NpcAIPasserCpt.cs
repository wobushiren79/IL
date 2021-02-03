using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.AI;

public class NpcAIPasserCpt : BaseNpcAI
{
    public enum PasserIntentEnum
    {
        LeaveTown,//离开
        StayInTown,//镇上闲逛
        GoToBuilding,//前往建筑
        StayInBuilding,//留在建筑
        LeaveBuilding,//离开建筑
        GoToEvent,//前往看热闹
        LookOnEvent,//看热闹
    }

    //移动目标点
    public Vector2 movePosition;
    //备用目标点
    public Vector2 markPosition;
    //建筑相关点
    public Vector2 buildingOutDoorPosition;
    public Vector2 buildingInDoorPosition;
    //前往的建筑
    public TownBuildingEnum buildingToGo;
    //npc现在所在地
    public TownBuildingEnum npcLocation;
    //路人意图
    public PasserIntentEnum passerIntent = PasserIntentEnum.LeaveTown;

    private void FixedUpdate()
    {
        //检测是否有战斗
        CheckHasCombat();
        switch (passerIntent)
        {
            case PasserIntentEnum.LeaveTown:
                //检测是否到达目标点
                if (characterMoveCpt.IsAutoMoveStop())
                    //删除NPC
                    Destroy(gameObject);
                break;
            case PasserIntentEnum.StayInTown:
                if (characterMoveCpt.IsAutoMoveStop())
                    SetRandomBuildingToGo();
                break;
            case PasserIntentEnum.GoToBuilding:
                //检测是否到达目标点
                if (characterMoveCpt.IsAutoMoveStop())
                    //在建筑物内逗留
                    SetIntent(PasserIntentEnum.StayInBuilding);
                break;
            case PasserIntentEnum.StayInBuilding:
                break;
            case PasserIntentEnum.LeaveBuilding:
                if (characterMoveCpt.IsAutoMoveStop())
                {
                    //暂时关闭自动寻路（不关闭的话 无法跨范围移动NPC）
                    //characterMoveCpt.CloseNavMeshAgent();
                    //离开建筑内部
                    transform.position = buildingOutDoorPosition;
                    //设置当前所在地
                    SetLocation(TownBuildingEnum.Town);
                    //离开建筑后开启自动寻路
                   // characterMoveCpt.OpenNavMeshAgent();
                    //有一定概率去下一个地点
                    int isLeave = UnityEngine.Random.Range(0, 2);
                    if (isLeave == 1)
                    {
                        SetIntent(PasserIntentEnum.LeaveTown);
                    }
                    else
                    {
                        SetRandomBuildingToGo();
                    }
                }
                break;
            case PasserIntentEnum.GoToEvent:
                if (characterMoveCpt.IsAutoMoveStop())
                    SetIntent(PasserIntentEnum.LookOnEvent);
                break;
            case PasserIntentEnum.LookOnEvent:
                CheckEventEnd();
                break;
        }
    }

    /// <summary>
    ///  //随机获取一个要去的地方
    /// </summary>
    public void SetRandomBuildingToGo()
    {
        TownBuildingEnum buildingToGo = RandomUtil.GetRandomEnum<TownBuildingEnum>();
        if (buildingToGo == TownBuildingEnum.Town)
        {
            SetIntent(PasserIntentEnum.StayInTown);
        }
        else
        {
            SetIntent(PasserIntentEnum.GoToBuilding, buildingToGo);
        }
    }

    /// <summary>
    /// 检测是否有战斗可看
    /// </summary>
    /// <returns></returns>
    public bool CheckHasCombat()
    {
        if (passerIntent != PasserIntentEnum.GoToEvent
            && passerIntent != PasserIntentEnum.LookOnEvent
            && MiniGameHandler.Instance.handlerForCombat.GetMiniGameStatus() == MiniGameStatusEnum.Gameing
            && Vector2.Distance(transform.position, MiniGameHandler.Instance.handlerForCombat.GetMiniGamePosition()) <= 10)
        {
            SetIntent(PasserIntentEnum.GoToEvent, MiniGameHandler.Instance.handlerForCombat.GetMiniGamePosition());
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 检测战斗是否结束
    /// </summary>
    public bool CheckEventEnd()
    {
        if (MiniGameHandler.Instance.handlerForCombat.GetMiniGameStatus() != MiniGameStatusEnum.Gameing)
        {
            if (npcLocation == TownBuildingEnum.Town)
            {
                SetRandomBuildingToGo();
            }
            else
            {
                int randomIntent = UnityEngine.Random.Range(0, 2);
                //一半概率继续留在该建筑
                if (randomIntent == 0)
                {
                    //开始逛街
                    SceneTownManager sceneTownManager = GameScenesHandler.Instance.manager.GetSceneManager<SceneTownManager>();
                    movePosition = sceneTownManager.GetRandomBuildingInsidePosition(npcLocation);
                    characterMoveCpt.SetDestination(movePosition);
                    //开始逛街
                    StartCoroutine(CoroutineForStayTimeCountdown(npcLocation));
                }
                //一半概率离开该建筑
                else
                {
                    SetIntent(PasserIntentEnum.LeaveBuilding);
                }

            }

            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 设置所在地
    /// </summary>
    /// <param name="buildingType"></param>
    public void SetLocation(TownBuildingEnum buildingType)
    {
        npcLocation = buildingType;
    }

    /// <summary>
    /// 设置意图
    /// </summary>
    /// <param name="passerIntent"></param>
    /// <param name="movePosition"></param>
    public void SetIntent(PasserIntentEnum passerIntent, TownBuildingEnum buildingEnum, Vector3 eventPosition)
    {
        this.passerIntent = passerIntent;
        StopAllCoroutines();
        switch (passerIntent)
        {
            case PasserIntentEnum.LeaveTown:
                IntentForLeaveTown();
                break;
            case PasserIntentEnum.StayInTown:
                IntentForStayInTown();
                break;
            case PasserIntentEnum.GoToBuilding:
                IntentForGoToBuilding(buildingEnum);
                break;
            case PasserIntentEnum.StayInBuilding:
                IntentForStayInBuilding(buildingToGo);
                break;
            case PasserIntentEnum.LeaveBuilding:
                IntentForLeaveBuilding();
                break;
            case PasserIntentEnum.GoToEvent:
                IntentForGoToEvent(eventPosition);
                break;
            case PasserIntentEnum.LookOnEvent:
                IntentForLookOnEvent();
                break;
        }
    }



    public void SetIntent(PasserIntentEnum passerIntent)
    {
        SetIntent(passerIntent, TownBuildingEnum.Guild, Vector3.zero);
    }
    public void SetIntent(PasserIntentEnum passerIntent, Vector3 eventPosition)
    {
        SetIntent(passerIntent, TownBuildingEnum.Guild, eventPosition);
    }
    public void SetIntent(PasserIntentEnum passerIntent, TownBuildingEnum buildingEnum)
    {
        SetIntent(passerIntent, buildingEnum, Vector3.zero);
    }

    /// <summary>
    /// 镇上闲逛
    /// </summary>
    public void IntentForStayInTown()
    {
        //开始逛街
        SceneTownManager sceneTownManager = GameScenesHandler.Instance.manager.GetSceneManager<SceneTownManager>();
        movePosition = sceneTownManager.GetRandomBuildingInsidePosition(TownBuildingEnum.Town);
        characterMoveCpt.SetDestination(movePosition);
    }

    /// <summary>
    /// 离开城镇
    /// </summary>
    public void IntentForLeaveTown()
    {
        SceneTownManager sceneTownManager = GameScenesHandler.Instance.manager.GetSceneManager<SceneTownManager>();
        //获取城镇出口位置
        movePosition = sceneTownManager.GetRandomTownDoorPosition();
        characterMoveCpt.SetDestination(movePosition);
    }

    /// <summary>
    /// 前往建筑物
    /// </summary>
    public void IntentForGoToBuilding(TownBuildingEnum buildingEnum)
    {
        SceneTownManager sceneTownManager = GameScenesHandler.Instance.manager.GetSceneManager<SceneTownManager>();
        this.buildingToGo = buildingEnum;

        //获取建筑的门
        sceneTownManager.GetBuildingDoorPosition(buildingEnum, out Vector2 outDoorPostion, out Vector2 inDoorPosition);
        buildingOutDoorPosition = outDoorPostion;
        buildingInDoorPosition = inDoorPosition;
        //前往门
        characterMoveCpt.SetDestination(buildingOutDoorPosition);
    }

    /// <summary>
    /// 在建筑物内逗留
    /// </summary>
    public void IntentForStayInBuilding(TownBuildingEnum buildingEnum)
    {
        //暂时关闭自动寻路（不关闭的话 无法跨范围移动NPC）
       // characterMoveCpt.CloseNavMeshAgent();
        //进入建筑内部
        transform.position = buildingInDoorPosition;
        //设置当前所在地
        SetLocation(buildingEnum);
        //进入建筑后开启自动寻路
        // characterMoveCpt.OpenNavMeshAgent();
        //开始逛街
        SceneTownManager sceneTownManager = GameScenesHandler.Instance.manager.GetSceneManager<SceneTownManager>();
        movePosition = sceneTownManager.GetRandomBuildingInsidePosition(buildingEnum);
        characterMoveCpt.SetDestination(movePosition);
        //开始逛街
        StartCoroutine(CoroutineForStayTimeCountdown(buildingEnum));
    }

    /// <summary>
    /// 离开建筑物
    /// </summary>
    public void IntentForLeaveBuilding()
    {
        //前往门口
        characterMoveCpt.SetDestination(buildingInDoorPosition);
    }

    /// <summary>
    /// 前往看热闹
    /// </summary>
    /// <param name="eventPosition"></param>
    public void IntentForGoToEvent(Vector3 eventPosition)
    {
        this.markPosition = eventPosition;
        float angle = 0;
        float circler = Random.Range(5f, 6f);
        if (Vector2.Distance(transform.position, eventPosition) <= 5)
        {
            angle = Random.Range(0f, 360f);
        }
        else
        {
            float tempAngle = VectorUtil.GetAngle(eventPosition, transform.position);
            angle = Random.Range(tempAngle - 45f, tempAngle + 45f);
        }
        movePosition = VectorUtil.GetCirclePosition(angle, eventPosition, circler);
        characterMoveCpt.SetDestination(movePosition);
    }

    /// <summary>
    /// 看热闹
    /// </summary>
    public void IntentForLookOnEvent()
    {
        ///设置朝向
        if (markPosition.x < transform.position.x)
        {
            SetCharacterFace(1);
        }
        else
        {
            SetCharacterFace(2);
        }
        StartCoroutine(CoroutineForLookOnEvent());
    }

    /// <summary>
    /// 协程-逗留倒计时
    /// </summary>
    /// <param name="buildingEnum"></param>
    /// <returns></returns>
    public IEnumerator CoroutineForStayTimeCountdown(TownBuildingEnum buildingEnum)
    {
        SceneTownManager sceneTownManager = GameScenesHandler.Instance.manager.GetSceneManager<SceneTownManager>();
        for (int i = 0; i < 3; i++)
        {
            //重新设置地点
            movePosition = sceneTownManager.GetRandomBuildingInsidePosition(buildingEnum);
            characterMoveCpt.SetDestination(movePosition);
            yield return new WaitForSeconds(20);
        }
        SetIntent(PasserIntentEnum.LeaveBuilding);
    }

    /// <summary>
    /// 协程-看热闹
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoroutineForLookOnEvent()
    {
        while (PasserIntentEnum.LookOnEvent == passerIntent)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(10, 60));
            //int expression = UnityEngine.Random.Range(1, 7);
            //SetExpression((CharacterExpressionCpt.CharacterExpressionEnum)expression, 2);
            int shoutId = Random.Range(13301, 13306);
            SetShout(GameCommonInfo.GetUITextById(shoutId));
        }
    }
}