using UnityEngine;
using UnityEditor;
using System.Collections;
public class NpcAIPasserCpt : BaseNpcAI
{
    public enum PasserIntentEnum
    {
        LeaveTown,
        GoToBuilding,
        StayInBuilding,
        LeaveBuilding,
    }
    //城镇数据
    public SceneTownManager sceneTownManager;
    //移动目标点
    public Vector3 movePosition;
    //建筑相关点
    public Vector3 buildingOutDoorPosition;
    public Vector3 buildingInDoorPosition;
    //前往的建筑
    public SceneTownManager.TownBuildingEnum buildingToGo;

    //路人意图
    public PasserIntentEnum passerIntent = PasserIntentEnum.LeaveTown;


    private void FixedUpdate()
    {
        switch (passerIntent)
        {
            case PasserIntentEnum.LeaveTown:
                //检测是否到达目标点
                if (characterMoveCpt.IsAutoMoveStop())
                    //删除NPC
                    Destroy(gameObject);
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
                    characterMoveCpt.CloseNavMeshAgent();
                    //离开建筑内部
                    transform.position = buildingOutDoorPosition;
                    //离开建筑后开启自动寻路
                    characterMoveCpt.OpenNavMeshAgent();
                    SetIntent(PasserIntentEnum.LeaveTown);
                }
                break;
        }
    }

    /// <summary>
    /// 设置意图
    /// </summary>
    /// <param name="passerIntent"></param>
    /// <param name="movePosition"></param>
    public void SetIntent(PasserIntentEnum passerIntent, SceneTownManager.TownBuildingEnum buildingEnum)
    {
        this.passerIntent = passerIntent;

        switch (passerIntent)
        {
            case PasserIntentEnum.LeaveTown:
                IntentForLeaveTown();
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
        }
    }
    public void SetIntent(PasserIntentEnum passerIntent)
    {
        SetIntent(passerIntent, SceneTownManager.TownBuildingEnum.Guild);
    }


    /// <summary>
    /// 离开城镇
    /// </summary>
    public void IntentForLeaveTown()
    {
        if (sceneTownManager == null)
            return;
        //获取城镇出口位置
        movePosition = sceneTownManager.GetRandomTownDoorPosition();
        characterMoveCpt.SetDestination(movePosition);
    }

    /// <summary>
    /// 前往建筑物
    /// </summary>
    public void IntentForGoToBuilding(SceneTownManager.TownBuildingEnum buildingEnum)
    {
        if (sceneTownManager == null)
            return;
        this.buildingToGo = buildingEnum;
        //获取建筑的门
        sceneTownManager.GetBuildingDoorPosition(buildingEnum, out Vector3 outDoorPostion, out Vector3 inDoorPosition);
        buildingOutDoorPosition = outDoorPostion;
        buildingInDoorPosition = inDoorPosition;
        //前往门
        characterMoveCpt.SetDestination(buildingOutDoorPosition);
    }

    /// <summary>
    /// 在建筑物内逗留
    /// </summary>
    public void IntentForStayInBuilding(SceneTownManager.TownBuildingEnum buildingEnum)
    {
        //暂时关闭自动寻路（不关闭的话 无法跨范围移动NPC）
        characterMoveCpt.CloseNavMeshAgent();
        //进入建筑内部
        transform.position = buildingInDoorPosition;
        //进入建筑后开启自动寻路
        characterMoveCpt.OpenNavMeshAgent();
        //开始逛街
        movePosition = sceneTownManager.GetRandomBuildingInsidePosition(buildingEnum);
        characterMoveCpt.SetDestination(movePosition);
        //开始逛街
        StartCoroutine(StayTimeCountdown(buildingEnum));
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
    /// 逗留倒计时
    /// </summary>
    /// <param name="buildingEnum"></param>
    /// <returns></returns>
    public IEnumerator StayTimeCountdown(SceneTownManager.TownBuildingEnum buildingEnum)
    {
        for (int i = 0; i < 3; i++)
        {
            //重新设置地点
            movePosition = sceneTownManager.GetRandomBuildingInsidePosition(buildingEnum);
            characterMoveCpt.SetDestination(movePosition);
            yield return new WaitForSeconds(20);
        }
        SetIntent(PasserIntentEnum.LeaveBuilding);
    }
}