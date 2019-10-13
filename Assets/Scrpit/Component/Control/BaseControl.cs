using UnityEngine;
using UnityEditor;
using Cinemachine;

public class BaseControl: BaseMonoBehaviour
{
    //镜头
    private CinemachineVirtualCamera mCamera2D;
    //镜头跟随对象
    public GameObject cameraFollowObj;

    /// <summary>
    /// 设置摄像头
    /// </summary>
    /// <param name="camera"></param>
    public void SetCamera2D(CinemachineVirtualCamera camera)
    {
        mCamera2D = camera;
    }

    /// <summary>
    /// 设置镜头跟随
    /// </summary>
    /// <param name="objFollow"></param>
    public virtual void SetCameraFollowObj(GameObject objFollow)
    {
        cameraFollowObj = objFollow;
        if (cameraFollowObj != null)
            mCamera2D.Follow = cameraFollowObj.transform;
    }

    /// <summary>
    /// 开始控制
    /// </summary>
    public virtual void StartControl()
    {
        gameObject.SetActive(true);
        this.enabled = true;
        if (cameraFollowObj != null)
            mCamera2D.Follow = cameraFollowObj.transform;
    }

    /// <summary>
    /// 暂停控制
    /// </summary>
    public virtual void StopControl()
    {
        this.enabled = false;
    }

    /// <summary>
    /// 恢复控制
    /// </summary>
    public virtual void RestoreControl()
    {
        this.enabled = true;
        mCamera2D.Follow = cameraFollowObj.transform;
    }

    /// <summary>
    /// 结束控制
    /// </summary>
    public virtual void EndControl()
    {
        gameObject.SetActive(false);
    }
    
    /// <summary>
    /// 设置摄像机位置
    /// </summary>
    /// <param name="position"></param>
    public void SetCameraPosition(Vector3 position)
    {
        if (cameraFollowObj != null)
            cameraFollowObj.transform.position = position;
    }
}