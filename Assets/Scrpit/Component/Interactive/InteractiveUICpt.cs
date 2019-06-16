using UnityEngine;
using UnityEditor;

public class InteractiveUICpt : BaseInteractiveCpt
{
    public string interactiveContent;
    public string uiName;
    public BaseUIManager uiManager;

    public override void InteractiveDetection()
    {
        if (Input.GetButtonDown("Interactive_E"))
        {
            uiManager.OpenUIAndCloseOtherByName(uiName);
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