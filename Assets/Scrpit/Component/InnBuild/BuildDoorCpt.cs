using UnityEngine;
using UnityEditor;

public class BuildDoorCpt : BaseBuildItemCpt
{
    public GameObject entranceObj;
    
    /// <summary>
    /// 获取入口位置
    /// </summary>
    /// <returns></returns>
    public Vector3 GetEntrancePosition()
    {
        return entranceObj.transform.position;
    }
}