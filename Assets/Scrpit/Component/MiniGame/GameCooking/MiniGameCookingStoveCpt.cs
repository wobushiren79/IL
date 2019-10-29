using UnityEngine;
using UnityEditor;

public class MiniGameCookingStoveCpt : BaseMonoBehaviour
{
    public GameObject objCookingPrePosition;
    public GameObject objCookingMakingPosition;
    public GameObject objCookingEndPosition;

    public GameObject objEffects;
    /// <summary>
    /// 获取料理准备阶段位置
    /// </summary>
    /// <returns></returns>
    public Vector3 GetCookingPrePosition()
    {
        return objCookingPrePosition.transform.position;
    }

    /// <summary>
    /// 获取料理制作阶段位置
    /// </summary>
    /// <returns></returns>
    public Vector3 GetCookingMakingPosition()
    {
        return objCookingMakingPosition.transform.position;
    }

    /// <summary>
    /// 获取料理结尾阶段位置
    /// </summary>
    /// <returns></returns>
    public Vector3 GetCookingEndPosition()
    {
        return objCookingEndPosition.transform.position;
    }

    /// <summary>
    /// 开启灶台
    /// </summary>
    public void OpenStove()
    {
        objEffects.SetActive(true);
    }

    /// <summary>
    /// 关闭灶台
    /// </summary>
    public void CloseStove()
    {
        objEffects.SetActive(false);
    }
}