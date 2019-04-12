using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class UIGameMain : BaseUIComponent
{
    public Button btBuild;

    public void Start()
    {
        if (btBuild != null)
            btBuild.onClick.AddListener(OpenBuildUI);
    }

    public void OpenBuildUI()
    {
        uiManager.OpenUIAndCloseOtherByName("Build");
    }
}