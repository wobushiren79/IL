using UnityEngine;
using UnityEditor;
using Cinemachine;

public class BaseControl : BaseMonoBehaviour
{
    //镜头跟随对象
    public GameObject cameraFollowObj;

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
        GameCameraHandler.Instance.manager.camera2D.Follow = cameraFollowObj.transform;
    }

    /// <summary>
    /// 开始控制
    /// </summary>
    public virtual void StartControl()
    {
        gameObject.SetActive(true);
        this.enabled = true;
        if (cameraFollowObj != null)
            GameCameraHandler.Instance.manager.camera2D.Follow = cameraFollowObj.transform;
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
            GameCameraHandler.Instance.manager.camera2D.Follow = cameraFollowObj.transform;
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
        if (GameCameraHandler.Instance.manager.camera2D != null)
        {
            GameCameraHandler.Instance.manager.camera2D.transform.position = new Vector3(position.x, position.y, GameCameraHandler.Instance.manager.camera2D.transform.position.z);
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
        if (GameCameraHandler.Instance.manager.camera2D != null)
        {
            GameCameraHandler.Instance.manager.camera2D.m_Lens.OrthographicSize = orthographicSize;
        }

    }
    public void SetCameraOrthographicSize()
    {
        SetCameraOrthographicSize(7);
    }

    public float GetCameraOrthographicSize()
    {
        float size = 0;
        if (GameCameraHandler.Instance.manager.camera2D != null)
        {
            size = GameCameraHandler.Instance.manager.camera2D.m_Lens.OrthographicSize;
        }
        return size;
    }


    /// <summary>
    /// 鼠标移动处理
    /// </summary>
    public virtual void HandleForMouseMove(out float moveX, out float moveY)
    {
        moveX = 0;
        moveY = 0;

        if (GameCommonInfo.GameConfig.statusForMouseMove == 0)
        {
            return;
        }

        Vector3 mousePosition = Input.mousePosition;

        if (mousePosition.x <= 25 && mousePosition.x >=0)
        {
            moveX = -1f;
        }
        else if (mousePosition.x >= Screen.width - 25 && mousePosition.x <= Screen.width)
        {
            moveX = 1f;
        }

        if (mousePosition.y <= 25 && mousePosition.y >= 0)
        {
            moveY = -1f;
        }
        else if (mousePosition.y >= Screen.height - 25 && mousePosition.y <= Screen.height)
        {
            moveY = 1f;
        }
    }

    protected Vector3 oldMouseButtonMove = Vector3.zero;


    public virtual void HandleForMouseButtonMove(out float moveX, out float moveY)
    {
        moveX = 0;
        moveY = 0;
        if (Input.GetMouseButton(1))
        {
            Vector3 mousePosition = Input.mousePosition;
            if (oldMouseButtonMove!=Vector3.zero)
            {
                Vector3 moveNormalized =  (oldMouseButtonMove - mousePosition).normalized * 100 * Time.deltaTime;
                moveX = moveNormalized.x;
                moveY = moveNormalized.y;
            }
            oldMouseButtonMove = mousePosition; 
        }
        if (Input.GetMouseButtonUp(1))
        {
            oldMouseButtonMove = Vector3.zero;
        }
    }

    /// <summary>
    /// 缩放
    /// </summary>
    public void HandleForZoom()
    {
        if (Input.GetButton(InputInfo.Zoom_In))
        {
            ZoomCamera(-0.1f);
        }
        if (Input.GetButton(InputInfo.Zoom_Out))
        {
            ZoomCamera(+0.1f);
        }
        if (Input.GetAxis(InputInfo.Zoom_Mouse) > 0)
        {
            ZoomCamera(-0.2f);
        }
        if (Input.GetAxis(InputInfo.Zoom_Mouse) < 0)
        {
            ZoomCamera(+0.2f);
        }
    }

    public void ZoomCamera(float addZoom)
    {
        float size = GetCameraOrthographicSize();
        size += addZoom;
        if (size < 7)
        {
            size = 7;
        }
        else if (size > 12)
        {
            size = 12;
        }
        SetCameraOrthographicSize(size);
    }
}