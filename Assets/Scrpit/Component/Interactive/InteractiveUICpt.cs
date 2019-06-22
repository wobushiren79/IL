using UnityEngine;
using UnityEditor;

public class InteractiveUICpt : BaseInteractiveCpt
{
    public string interactiveContent;
    public string uiName;

    //备注信息
    public string remarkData;
    public BaseUIManager uiManager;

    public override void InteractiveDetection()
    {
        if (Input.GetButtonDown("Interactive_E"))
        {
            uiManager.OpenUIAndCloseOtherByName(uiName);
            if (!CheckUtil.StringIsNull(remarkData))
            {
                BaseUIComponent baseUIComponent = uiManager.GetUIByName(uiName);
                baseUIComponent.SetRemarkData(remarkData);
            }
        }
    }

    public override void InteractiveEnd(CharacterInteractiveCpt characterInt)
    {
        characterInt.CloseInteractive();
    }

    public override void InteractiveStart(CharacterInteractiveCpt characterInt)
    {
        characterInt.ShowInteractive(interactiveContent);
    }
}