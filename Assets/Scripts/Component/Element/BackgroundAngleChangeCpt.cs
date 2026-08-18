using UnityEngine;

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
        var camera2D = CameraHandler.Instance.manager.camera2D;
        if (camera2D == null)
        {
            return;
        }
        if (camera2D.transform.position.y >= minCameraY && camera2D.transform.position.y <= maxCameraY)
        {
            float lerp = (camera2D.transform.position.y - minCameraY) / (maxCameraY - minCameraY);
            transform.position = new Vector3(0, Mathf.Lerp(minUpAndDown, maxUpAndDown, lerp), 0);
        }

    }
}