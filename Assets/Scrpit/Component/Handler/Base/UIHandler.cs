using RotaryHeart.Lib;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : BaseHandler<UIHandler, UIManager>
{
    public Canvas canvas;
    public CanvasScaler canvasScaler;
    public GraphicRaycaster graphicRaycaster;

    protected override void Awake()
    {
        canvas = CptUtil.AddCpt<Canvas>(gameObject);
        canvasScaler = CptUtil.AddCpt<CanvasScaler>(gameObject);
        graphicRaycaster = CptUtil.AddCpt<GraphicRaycaster>(gameObject);
        ChangeUIRenderMode(RenderMode.ScreenSpaceCamera);
    }


    public void ChangeUIRenderMode(RenderMode renderMode)
    {
        canvas.renderMode = renderMode;
        switch (renderMode)
        {
            case RenderMode.ScreenSpaceOverlay:
                break;
            case RenderMode.ScreenSpaceCamera:
                canvas.planeDistance = 1;
                canvas.worldCamera = Camera.main;
                break;
            case RenderMode.WorldSpace:
                break;
        }
        canvas.sortingLayerName = "UI";
        canvas.sortingOrder = 1;
    }
}