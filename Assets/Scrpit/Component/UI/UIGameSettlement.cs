using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class UIGameSettlement : BaseUIComponent
{
    public Button btSubmit;
    public Button btCancel;

    public InnHandler innHandler;
    public ControlHandler controlHandler;
    public GameTimeHandler gameTimeHandler;

    private void Start()
    {
        if (btSubmit != null)
            btSubmit.onClick.AddListener(OpenInn);
        if (btCancel != null)
            btCancel.onClick.AddListener(CloseInn);
    }

    public void OpenInn()
    {
        gameTimeHandler.dayStauts = GameTimeHandler.DayEnum.Work;
        uiManager.OpenUIAndCloseOtherByName("Main");
        innHandler.OpenInn();
        controlHandler.StartControl(ControlHandler.ControlEnum.Work);
    }

    public void CloseInn()
    {
        gameTimeHandler.dayStauts = GameTimeHandler.DayEnum.Rest;
        uiManager.OpenUIAndCloseOtherByName("Main");
        controlHandler.StartControl(ControlHandler.ControlEnum.Normal);
    }
}