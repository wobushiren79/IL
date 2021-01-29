using Cinemachine;
using UnityEditor;
using UnityEngine;

public class GameCameraManager : BaseManager
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



}