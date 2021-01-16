using UnityEditor;
using UnityEngine;

public class RayUtil
{
 
    /// <summary>
    /// 屏幕点击射线检测
    /// </summary>
    /// <param name="isCollider"></param>
    /// <param name="hit"></param>
    public static void RayToScreenPoint(out bool isCollider,out RaycastHit hit)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        isCollider = Physics.Raycast(ray, out hit);
    }
}