using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SceneInnManager : BaseManager
{

    //客栈区域出入口
    public List<Transform> listSceneExport;
    //城镇入口
    public Transform townEntranceLeft;
    public Transform townEntranceRight;

    public GameObject objEcologyContainer;

    /// <summary>
    /// 初始化场景
    /// </summary>
    public void InitScene(float innW, float innH)
    {
        //删除遮挡的生态物体
        if (objEcologyContainer != null)
        {
            for (int i = 0; i < objEcologyContainer.transform.childCount; i++)
            {
                Transform tfItem = objEcologyContainer.transform.GetChild(i);
                Vector3 itemPosition = tfItem.position;
                if (itemPosition.x > 0
                    && itemPosition.x < innW
                    && itemPosition.y > 0
                    && itemPosition.y < innH)
                {
                    GameObject.Destroy(tfItem.gameObject);
                }
            }
        }

    }

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