using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class UIGameStart : BaseUIComponent
{
    public Button btSubmit;
    public Button btCancel;

    public InnHandler innHandler;

    private void Start()
    {
        if (btSubmit != null)
            btSubmit.onClick.AddListener(OpenInn);
        if (btCancel != null)
            btCancel.onClick.AddListener(CloseInn);
    }

    public void OpenInn()
    {
        uiManager.OpenUIAndCloseOtherByName("Main");
        innHandler.OpenInn();
    }

    public void CloseInn()
    {
        uiManager.OpenUIAndCloseOtherByName("Main");
    }
}