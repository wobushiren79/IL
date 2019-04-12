using UnityEngine;
using UnityEditor;
using Cinemachine;

public class BaseControl : BaseMonoBehaviour
{
    //镜头
    public CinemachineVirtualCamera camera2D;
    //镜头跟随对象
    public GameObject cameraFollowObj;

    /// <summary>
    /// 开始控制
    /// </summary>
    public virtual void StartControl()
    {
        gameObject.SetActive(true);
        camera2D.Follow = cameraFollowObj.transform;
    }

    /// <summary>
    /// 结束控制
    /// </summary>
    public virtual void EndControl()
    {
        gameObject.SetActive(false);
    }
}