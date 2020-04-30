using UnityEngine;
using UnityEditor;

public class SceneMountainManager 
{
    //山顶出口
    public Transform exitDoor;

    /// <summary>
    /// 获取出口位置
    /// </summary>
    /// <returns></returns>
    public Vector3 GetExitDoor()
    {
        return exitDoor.position;
    }
}