﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class InnEntranceHandler : BaseMonoBehaviour
{
    //门列表
    public List<BuildDoorCpt> listDoorCpt;
    public List<BuildStairsCpt> listStairsCpt;

    //门容器
    public GameObject container;

    /// <summary>
    /// 找到所有门
    /// </summary>
    /// <returns></returns>
    public void InitEntranceList()
    {
        BuildDoorCpt[] doorArray = container.GetComponentsInChildren<BuildDoorCpt>();
        listDoorCpt = TypeConversionUtil.ArrayToList(doorArray);

        BuildStairsCpt[] stairsArray = container.GetComponentsInChildren<BuildStairsCpt>();
        listStairsCpt = TypeConversionUtil.ArrayToList(stairsArray);
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

    /// <summary>
    /// 通过备注ID获取楼梯位置
    /// </summary>
    /// <param name="remarkId"></param>
    /// <param name="layerFirstPosition"></param>
    /// <param name="layerSecondPosition"></param>
    public void GetStairsPosition(string remarkId,out Vector3 layerFirstPosition, out Vector3 layerSecondPosition)
    {
        layerFirstPosition = Vector3.zero;
        layerSecondPosition= Vector3.zero;
        if (CheckUtil.ListIsNull(listStairsCpt))
            return;
        for (int i=0;i< listStairsCpt.Count;i++)
        {
            BuildStairsCpt buildStairs = listStairsCpt[i];
            if (buildStairs.remarkId.Equals(remarkId))
            {
                if (buildStairs.layer ==1 )
                {
                    layerFirstPosition = buildStairs.GetStairsPosition();
                }
                else if (buildStairs.layer == 2)
                {
                    layerSecondPosition = buildStairs.GetStairsPosition();
                }
            }
        }
  
    }
}