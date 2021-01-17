using RotaryHeart.Lib;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : BaseUIHandler<UIHandler, UIManager>
{
    
    protected override void Awake()
    {
        sortingOrder = 1;
        base.Awake();
        ChangeUIRenderMode(RenderMode.ScreenSpaceCamera);
    }

    protected void Update()
    {
        if (canvas.worldCamera == null)
            canvas.worldCamera = Camera.main;
    }

}