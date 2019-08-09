using UnityEngine;
using UnityEditor;

public class InteractiveLookCpt : BaseInteractiveCpt
{
    public string interactiveContent;

    public long markId;//交互ID

    public override void InteractiveDetection()
    {
        if (Input.GetButtonDown("Interactive_E"))
        {
            if (!EventHandler.Instance.isEventing)
                EventHandler.Instance.EventTriggerForLook(markId);
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