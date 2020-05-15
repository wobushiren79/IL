using UnityEngine;
using UnityEditor;

public class UIGameComponent : BaseUIComponent
{
    //UI管理
    public UIGameManager uiGameManager;

    public override void Awake()
    {
        base.Awake();
        uiGameManager = (UIGameManager)uiManager;
    }

}