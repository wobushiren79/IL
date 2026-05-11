using UnityEditor;
using UnityEngine;

public class SceneCourtyardManager : SceneBaseManager
{
    public Transform courtyardEntrance;

    /// <summary>
    /// 获取后庭入口
    /// </summary>
    /// <returns></returns>
    public Vector2 GetCourtyardEntrance()
    {
        return courtyardEntrance.transform.position;
    }
}