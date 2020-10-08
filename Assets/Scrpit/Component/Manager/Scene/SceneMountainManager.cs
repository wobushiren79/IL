using UnityEngine;
using UnityEditor;

public class SceneMountainManager : BaseMonoBehaviour
{
    //山顶出口
    public Transform exitDoor;

    //无尽之塔
    public Transform infiniteTowersOutDoor;
    public Transform infiniteTowersInDoor;
    public Transform infiniteTowersInside;

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
}