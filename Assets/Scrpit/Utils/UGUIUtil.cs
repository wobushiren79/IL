using UnityEditor;
using UnityEngine;

public class UGUIUtil 
{
    /// <summary>
    /// 获取Icon在指定UIroot下的坐标
    /// </summary>
    /// <param name="tfRoot"></param>
    /// <param name="tfIcon"></param>
    public Vector3 GetUIRootPosForIcon(Transform tfRoot,Transform tfIcon)
    {
        return tfRoot.InverseTransformPoint(tfIcon.position);
    }
}