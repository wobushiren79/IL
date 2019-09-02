using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SceneTownManager : BaseManager
{
    public enum TownBuildingEnum
    {
        None,
        Market,
    }

    //小镇出入口
    public List<Transform> listTownDoor;

    //市场
    public Transform marketOutDoor;
    public Transform marketInDoor;
    public Transform marketInside;

    /// <summary>
    /// 获取随机城镇门坐标
    /// </summary>
    /// <returns></returns>
    public Vector3 GetRandomTownDoorPosition()
    {
        if (CheckUtil.ListIsNull(listTownDoor))
        {
            return Vector3.zero;
        }
        Transform tfTownDoor = RandomUtil.GetRandomDataByList(listTownDoor);
        return GameUtil.GetTransformInsidePosition2D(tfTownDoor);
    }

    /// <summary>
    /// 获取建筑物门的位置
    /// </summary>
    /// <param name="townBuilding"></param>
    /// <param name="outDoorPosition"></param>
    /// <param name="inDoorPosition"></param>
    public void GetBuildingDoorPosition(TownBuildingEnum townBuilding, out Vector3 outDoorPosition, out Vector3 inDoorPosition)
    {
        outDoorPosition = Vector3.zero;
        inDoorPosition = Vector3.zero;
        switch (TownBuildingEnum.Market)
        {
            case TownBuildingEnum.Market:
                if (marketOutDoor == null || marketInDoor == null)
                    return;
                outDoorPosition = marketOutDoor.transform.position;
                inDoorPosition = marketInDoor.transform.position;
                break;
        }
    }

    /// <summary>
    /// 获取建筑物内部顶点坐标
    /// </summary>
    /// <returns></returns>
    public Vector3 GetRandomBuildingInsidePosition(TownBuildingEnum townBuilding)
    {
        if (marketInside == null)
            return Vector3.zero;
        Transform tfBuilding = null;
        switch (TownBuildingEnum.Market)
        {
            case TownBuildingEnum.Market:
                tfBuilding = marketInside;
                break;
        }
        if (tfBuilding == null)
            return Vector3.zero;
        List<Transform> tfList =CptUtil.GetAllCptInChildrenByContainName<Transform>(tfBuilding.gameObject,"Wall_");
        if (CheckUtil.ListIsNull(tfList))
            return Vector3.zero;
        Transform tfItem = RandomUtil.GetRandomDataByList(tfList);
        return  GameUtil.GetTransformInsidePosition2D(tfItem); ;
    }
}