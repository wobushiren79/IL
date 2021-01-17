using UnityEditor;
using UnityEngine;

public class PopupHandler : BaseUIHandler<PopupHandler, PopupManger>
{
    protected override void Awake()
    {
        sortingOrder = 4;
        base.Awake();
        ChangeUIRenderMode(RenderMode.ScreenSpaceCamera);
    }
}