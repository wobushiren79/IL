using UnityEngine;
using UnityEditor;

public class MiniGameCookingAuditTableCpt : BaseMonoBehaviour
{
    //座位位置
    public GameObject objSeatPosition;
    public GameObject objFoodPosition;

    /// <summary>
    /// 获取座位位置
    /// </summary>
    /// <returns></returns>
    public Vector3 GetSeatPosition()
    {
        return objSeatPosition.transform.position;
    }

    /// <summary>
    /// 获取食物位置
    /// </summary>
    /// <returns></returns>
    public Vector3 GetFoodPosition()
    {
        return objFoodPosition.transform.position;
    }
}