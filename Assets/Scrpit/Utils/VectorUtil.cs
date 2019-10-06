﻿using UnityEngine;
using UnityEditor;

public class VectorUtil
{
    /// <summary>
    /// 获取两个点的夹角 vec2相对于vec1 的夹角
    /// </summary>
    /// <param name="vec1">坐标原点</param>
    /// <param name="vec2"></param>
    /// <returns></returns>
    public static float GetAngle(Vector3 vec1, Vector3 vec2)
    {
        //Vector3 v3 = Vector3.Cross(from_, to_);
        //if (v3.z > 0)
        //    return Vector3.Angle(from_, to_);
        //else
        //    return 360 - Vector3.Angle(from_, to_);
        Vector2 diference = vec2 - vec1;
        float sign = (vec2.y < vec1.y) ? -1.0f : 1.0f;
        return Vector2.Angle(Vector2.right, diference) * sign;
    }
}