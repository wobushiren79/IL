using Unity.Cinemachine;


public partial class CameraManager 
{
    protected CinemachineCamera _camera2D;

    public CinemachineCamera camera2D
    {
        get
        {
            if (_camera2D == null)
                _camera2D = FindWithTag<CinemachineCamera>(TagInfo.Tag_Camera2D);
            return _camera2D;
        }
    }

   
}