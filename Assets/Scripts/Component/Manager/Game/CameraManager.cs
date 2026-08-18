using Unity.Cinemachine;


public partial class CameraManager 
{
    protected CinemachineVirtualCameraBase _camera2D;

    /// <summary>
    /// 2D主镜头
    /// 兼容两种组件：Cinemachine 3 的 CinemachineCamera，以及旧版（Cinemachine 2 遗留）的 CinemachineVirtualCamera
    /// </summary>
    public CinemachineVirtualCameraBase camera2D
    {
        get
        {
            if (_camera2D == null)
                _camera2D = FindWithTag<CinemachineVirtualCameraBase>(TagInfo.Tag_Camera2D);
            return _camera2D;
        }
    }

    /// <summary>
    /// 设置镜头远近
    /// </summary>
    /// <param name="orthographicSize"></param>
    public void SetCameraOrthographicSize(float orthographicSize)
    {
        CinemachineVirtualCameraBase vcam = camera2D;
        if (vcam == null)
            return;
#pragma warning disable 0618
        if (vcam is CinemachineCamera newCamera)
        {
            newCamera.Lens.OrthographicSize = orthographicSize;
        }
        else if (vcam is CinemachineVirtualCamera legacyCamera)
        {
            legacyCamera.m_Lens.OrthographicSize = orthographicSize;
        }
#pragma warning restore 0618
    }

    /// <summary>
    /// 获取镜头远近
    /// </summary>
    /// <returns></returns>
    public float GetCameraOrthographicSize()
    {
        CinemachineVirtualCameraBase vcam = camera2D;
        if (vcam == null)
            return 0;
#pragma warning disable 0618
        if (vcam is CinemachineCamera newCamera)
        {
            return newCamera.Lens.OrthographicSize;
        }
        else if (vcam is CinemachineVirtualCamera legacyCamera)
        {
            return legacyCamera.m_Lens.OrthographicSize;
        }
#pragma warning restore 0618
        return 0;
    }
}