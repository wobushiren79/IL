﻿using UnityEngine;
using UnityEditor;

public class SceneMountainManager : SceneBaseManager
{
    //山顶出口
    public Transform exitDoor;

    //无尽之塔
    public Transform infiniteTowersOutDoor;
    public Transform infiniteTowersInDoor;
    public Transform infiniteTowersInside;
    public Transform infiniteTowersStairs;


    /// <summary>
    /// 获取出口位置
    /// </summary>
    /// <returns></returns>
    public Vector3 GetExitDoor()
    {
        return exitDoor.position;
    }

    /// <summary>
    /// 获取建筑物门的位置
    /// </summary>
    /// <param name="townBuilding"></param>
    /// <param name="outDoorPosition"></param>
    /// <param name="inDoorPosition"></param>
    public void GetBuildingDoorPosition(MountainBuildingEnum buildingEnum, out Vector2 outDoorPosition, out Vector2 inDoorPosition)
    {
        outDoorPosition = Vector2.zero;
        inDoorPosition = Vector2.zero;
        switch (buildingEnum)
        {
            case MountainBuildingEnum.InfiniteTowers:
                outDoorPosition = infiniteTowersOutDoor.transform.position;
                inDoorPosition = infiniteTowersInDoor.transform.position;
                break;
        }
    }

    /// <summary>
    /// 获取无尽之塔楼梯
    /// </summary>
    /// <returns></returns>
    public Vector3 GetInfiniteTowersStairs()
    {
        return infiniteTowersStairs.position;
    }
}