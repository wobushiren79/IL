using UnityEngine;
using UnityEditor;
using System.Collections;
using Pathfinding;

public class InnBaseHandler : BaseMonoBehaviour
{
    /// <summary>
    /// 判断路径是否有效
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="endPosition"></param>
    /// <returns></returns>
    public IEnumerator CheckPath(Vector3 startPosition, Vector3 endPosition)
    {
        ABPath path = ABPath.Construct(startPosition, endPosition);
        AstarPath.StartPath(path);
        yield return StartCoroutine(path.WaitForPath());
    }
}