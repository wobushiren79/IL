using UnityEngine;
using UnityEditor;

public class InteractiveLookCpt : BaseInteractiveCpt
{
    public string interactiveContent;

    public long markId;//交互ID

    protected EventHandler eventHandler;

    private void Awake()
    {
        eventHandler = Find<EventHandler>(ImportantTypeEnum.EventHandler);
    }

    public override void InteractiveDetection(CharacterInteractiveCpt characterInt)
    {
        if (Input.GetButtonDown(InputInfo.Interactive_E) && eventHandler != null)
        {
            eventHandler.EventTriggerForLook(markId);
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