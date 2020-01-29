﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SceneInnManager : BaseManager
{

    //客栈区域出入口
    public List<Transform> listSceneExport;
    //城镇入口
    public Transform townEntranceLeft;
    public Transform townEntranceRight;

    /// <summary>
    /// 获取随机客栈区域出入口坐标
    /// </summary>
    /// <returns></returns>
    public Vector3 GetRandomSceneExportPosition()
    {
        if (CheckUtil.ListIsNull(listSceneExport))
        {
            return Vector3.zero;
        }
        Transform tfTownDoor = RandomUtil.GetRandomDataByList(listSceneExport);
        return GameUtil.GetTransformInsidePosition2D(tfTownDoor);
    }

    /// <summary>
    /// 获取随机客栈区域出入口坐标
    /// </summary>
    /// <param name="num">编号</param>
    /// <returns></returns>
    public Vector3 GetRandomSceneExportPosition(int num)
    {
        if (CheckUtil.ListIsNull(listSceneExport) || num > listSceneExport.Count - 1)
        {
            return Vector3.zero;
        }
        Transform tfTownDoor = listSceneExport[num];
        return GameUtil.GetTransformInsidePosition2D(tfTownDoor);
    }

    /// <summary>
    /// 获取城镇入口
    /// </summary>
    /// <returns></returns>
    public Vector2 GetTownEntranceLeft()
    {
        return townEntranceLeft.transform.position;
    }

    public Vector2 GetTownEntranceRight()
    {
        return townEntranceRight.transform.position;
    }
}