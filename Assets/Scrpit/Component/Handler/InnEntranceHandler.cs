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
    public List<Vector3> GetEntrancePositionList()
    {
        if (listDoorCpt == null)
            listDoorCpt = new List<BuildDoorCpt>();
        List<Vector3> doorList = new List<Vector3>();
        for (int i = 0; i < listDoorCpt.Count; i++)
        {
            BuildDoorCpt doorCpt = listDoorCpt[i];
            if(doorCpt!=null)
            doorList.Add(doorCpt.GetEntrancePosition());
        }
        return doorList;
    }
}