using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SceneTownManager : BaseManager
{
    //小镇出入口
    public List<Transform> listTownDoor;
    //小镇
    public Transform townInside;
    //市场
    public Transform marketOutDoor;
    public Transform marketInDoor;
    public Transform marketInside;
    //人才市场
    public Transform recruitmentOutDoor;
    public Transform recruitmentInDoor;
    public Transform recruitmentInside;
    //杂货
    public Transform groceryOutDoor;
    public Transform groceryInDoor;
    public Transform groceryInside;
    //衣装
    public Transform dressOutDoor;
    public Transform dressInDoor;
    public Transform dressInside;
    //工匠
    public Transform carpenterOutDoor;
    public Transform carpenterInDoor;
    public Transform carpenterInside;
    //公会
    public Transform guildOutDoor;
    public Transform guildInDoor;
    public Transform guildInside;
    //竞技场
    public Transform arenaOutDoor;
    public Transform arenaInDoor;
    public Transform arenaInside;
    //银行
    public Transform bankOutDoor;
    public Transform bankInDoor;
    public Transform bankInside;

    //药房
    public Transform pharmacyOutDoor;
    public Transform pharmacyInDoor;
    public Transform pharmacyInside;
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
    /// 获取正门坐标
    /// </summary>
    /// <returns></returns>
    public Vector3 GetMainTownDoorPosition()
    {
        return listTownDoor[0].position;
    }

    /// <summary>
    /// 获取建筑物门的位置
    /// </summary>
    /// <param name="townBuilding"></param>
    /// <param name="outDoorPosition"></param>
    /// <param name="inDoorPosition"></param>
    public void GetBuildingDoorPosition(TownBuildingEnum townBuildingEnum, out Vector2 outDoorPosition, out Vector2 inDoorPosition)
    {
        outDoorPosition = Vector2.zero;
        inDoorPosition = Vector2.zero;
        switch (townBuildingEnum)
        {
            case TownBuildingEnum.Market:
                outDoorPosition = marketOutDoor.transform.position;
                inDoorPosition = marketInDoor.transform.position;
                break;
            case TownBuildingEnum.Recruitment:
                outDoorPosition = recruitmentOutDoor.transform.position;
                inDoorPosition = recruitmentInDoor.transform.position;
                break;
            case TownBuildingEnum.Grocery:
                outDoorPosition = groceryOutDoor.transform.position;
                inDoorPosition = groceryInDoor.transform.position;
                break;
            case TownBuildingEnum.Dress:
                outDoorPosition = dressOutDoor.transform.position;
                inDoorPosition = dressInDoor.transform.position;
                break;
            case TownBuildingEnum.Carpenter:
                outDoorPosition = carpenterOutDoor.transform.position;
                inDoorPosition = carpenterInDoor.transform.position;
                break;
            case TownBuildingEnum.Guild:
                outDoorPosition = guildOutDoor.transform.position;
                inDoorPosition = guildInDoor.transform.position;
                break;
            case TownBuildingEnum.Arena:
                outDoorPosition = arenaOutDoor.transform.position;
                inDoorPosition = arenaInDoor.transform.position;
                break;
            case TownBuildingEnum.Bank:
                outDoorPosition = bankOutDoor.transform.position;
                inDoorPosition = bankInDoor.transform.position;
                break;
            case TownBuildingEnum.Pharmacy:
                outDoorPosition =pharmacyOutDoor.transform.position;
                inDoorPosition = pharmacyInDoor.transform.position;
                break;
        }
    }

    /// <summary>
    /// 获取建筑物内部顶点坐标
    /// </summary>
    /// <returns></returns>
    public Vector3 GetRandomBuildingInsidePosition(TownBuildingEnum townBuildingEnum)
    {
        if (marketInside == null)
            return Vector3.zero;
        Transform tfBuilding = null;
        switch (townBuildingEnum)
        {
            case TownBuildingEnum.Town:
                tfBuilding = townInside;
                break;
            case TownBuildingEnum.Market:
                tfBuilding = marketInside;
                break;
            case TownBuildingEnum.Recruitment:
                tfBuilding = recruitmentInside;
                break;
            case TownBuildingEnum.Grocery:
                tfBuilding = groceryInside;
                break;
            case TownBuildingEnum.Dress:
                tfBuilding = dressInside;
                break;
            case TownBuildingEnum.Carpenter:
                tfBuilding = carpenterInside;
                break;
            case TownBuildingEnum.Guild:
                tfBuilding = guildInside;
                break;
            case TownBuildingEnum.Arena:
                tfBuilding = arenaInside;
                break;
            case TownBuildingEnum.Bank:
                tfBuilding = bankInside;
                break;
            case TownBuildingEnum.Pharmacy:
                tfBuilding = pharmacyInside;
                break;
        }
        if (tfBuilding == null)
            return Vector3.zero;
        List<Transform> tfList = CptUtil.GetAllCptInChildrenByContainName<Transform>(tfBuilding.gameObject, "Wall_");
        if (CheckUtil.ListIsNull(tfList))
            return Vector3.zero;
        Transform tfItem = RandomUtil.GetRandomDataByList(tfList);
        return GameUtil.GetTransformInsidePosition2D(tfItem); ;
    }
}