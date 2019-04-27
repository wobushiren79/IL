using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class UIGameWorker : BaseUIComponent
{
    public Button btBack;

    private void Start()
    {
        if (btBack != null)
            btBack.onClick.AddListener(OpenMainUI);
    }

    public void OpenMainUI()
    {
        uiManager.OpenUIAndCloseOtherByName("Main");
    }
}