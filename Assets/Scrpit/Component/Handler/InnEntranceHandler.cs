using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class InnEntranceHandler : BaseMonoBehaviour
{
    //门列表
    public List<BuildDoorCpt> listDoorCpt;
    //门容器
    public GameObject doorContainer;

    /// <summary>
    /// 找到所有门
    /// </summary>
    /// <returns></returns>
    public List<BuildDoorCpt> InitDoorList()
    {
        if (listDoorCpt == null)
            return listDoorCpt;
        BuildDoorCpt[] tableArray = doorContainer.GetComponentsInChildren<BuildDoorCpt>();
        listDoorCpt = TypeConversionUtil.ArrayToList(tableArray);
        return listDoorCpt;
    }

    /// <summary>
    /// 获取所有入口地址
    /// </summary>
    /// <returns></returns>
    public List<BuildDoorCpt> GetEntranceList()
    {
        if (listDoorCpt == null)
            listDoorCpt = new List<BuildDoorCpt>();
        return listDoorCpt;
    }

    /// <summary>
    /// 获取随机一个入口附近的坐标
    /// </summary>
    /// <returns></returns>
    public Vector3 GetRandomEntrancePosition()
    {
        BuildDoorCpt buildDoor = RandomUtil.GetRandomDataByList(GetEntranceList());
        Transform tfEntrance = null;
        if (buildDoor != null && buildDoor.entranceObj != null)
            tfEntrance = buildDoor.entranceObj.transform;
        if (tfEntrance != null)
            return GameUtil.GetTransformInsidePosition2D(tfEntrance);
        else
            return Vector3.zero;
    }
}