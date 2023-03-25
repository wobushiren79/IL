using UnityEngine;
using UnityEditor;
using Cinemachine;

public class BackgroundAngleChangeCpt : BaseMonoBehaviour
{
    //上下移动最小值
    public float minUpAndDown;
    //上下移动最大值
    public float maxUpAndDown;

    //镜头范围
    public float minCameraY;
    public float maxCameraY;

    private void Update()
    {
        if (minCameraY == maxCameraY)
        {
            return;
        }
       
        if (CameraHandler.Instance.manager.camera2D.transform.position.y >= minCameraY &&  CameraHandler.Instance.manager.camera2D.transform.position.y <= maxCameraY)
        {
            float lerp = (CameraHandler.Instance.manager.camera2D.transform.position.y - minCameraY) / (maxCameraY - minCameraY);
            transform.position = new Vector3(0, Mathf.Lerp(minUpAndDown, maxUpAndDown, lerp), 0);
        }

    }
}