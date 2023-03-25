using Cinemachine;
using UnityEditor;
using UnityEngine;

public class CameraManager : BaseManager
{
    protected CinemachineVirtualCamera _camera2D;

    public CinemachineVirtualCamera camera2D
    {
        get
        {
            if (_camera2D == null)
                _camera2D = FindWithTag<CinemachineVirtualCamera>(TagInfo.Tag_Camera2D);
            return _camera2D;
        }
    }


    protected Camera _uiCamera;

    public Camera uiCamera
    {
        get
        {
            if (_uiCamera == null)
            {
                return Camera.main;
            }
            return Camera.main;
        }
    }
}