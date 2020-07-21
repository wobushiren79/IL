using UnityEngine;
using UnityEditor;
using Cinemachine;

public class BaseControl : BaseMonoBehaviour
{
    //镜头
    protected CinemachineVirtualCamera camera2D;
    protected CinemachineConfiner cameraConfiner;
    //镜头跟随对象
    public GameObject cameraFollowObj;

    /// <summary>
    /// 设置摄像头
    /// </summary>
    /// <param name="camera"></param>
    public void SetCamera2D(CinemachineVirtualCamera camera2D)
    {
        this.camera2D = camera2D;
        cameraConfiner = this.camera2D.GetComponent<CinemachineConfiner>();
    }

    /// <summary>
    /// 设置镜头跟随
    /// </summary>
    /// <param name="objFollow"></param>
    public virtual void SetCameraFollowObj(GameObject objFollow)
    {
        if (objFollow != null)
        {
            cameraFollowObj = objFollow;
        }
        else
        {
            cameraFollowObj = gameObject;
        }
        camera2D.Follow = cameraFollowObj.transform;
    }

    /// <summary>
    /// 开始控制
    /// </summary>
    public virtual void StartControl()
    {
        gameObject.SetActive(true);
        this.enabled = true;
        if (cameraFollowObj != null)
            camera2D.Follow = cameraFollowObj.transform;
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
        if (cameraFollowObj != null)
            camera2D.Follow = cameraFollowObj.transform;
    }

    /// <summary>
    /// 结束控制
    /// </summary>
    public virtual void EndControl()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 设置摄像机跟随物体位置
    /// </summary>
    /// <param name="position"></param>
    public void SetFollowPosition(Vector3 position)
    {
        if (cameraFollowObj != null)
            cameraFollowObj.transform.position = position;
    }

    /// <summary>
    /// 设置摄像机位置
    /// </summary>
    /// <param name="position"></param>
    public void SetCameraPosition(Vector2 position)
    {
        if (camera2D != null)
        {
            camera2D.transform.position = new Vector3(position.x, position.y, camera2D.transform.position.z);
        }
;
    }

    /// <summary>
    /// 设置镜头远近
    /// </summary>
    /// <param name="orthographicSize"></param>
    ///
    public void SetCameraOrthographicSize(float orthographicSize)
    {
        if (camera2D != null)
        {
            camera2D.m_Lens.OrthographicSize = orthographicSize;
        }

    }
    public void SetCameraOrthographicSize()
    {
        SetCameraOrthographicSize(7);
    }


    /// <summary>
    /// 鼠标移动处理
    /// </summary>
    public virtual void HandleForMouseMove(out float moveX, out float moveY)
    {
        moveX = 0;
        moveY = 0;

        Vector3 mousePosition = Input.mousePosition;

        if (mousePosition.x <= 25)
        {
            moveX = -1f;
        }
        else if (mousePosition.x >= Screen.width - 25)
        {
            moveX = 1f;
        }

        if (mousePosition.y <= 25)
        {
            moveY = -1f;
        }
        else if (mousePosition.y >= Screen.height - 25)
        {
            moveY = 1f;
        }
    }
}