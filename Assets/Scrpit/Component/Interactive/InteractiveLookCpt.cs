using UnityEngine;
using UnityEditor;

public class InteractiveLookCpt : BaseInteractiveCpt
{
    public string interactiveContent;

    public long markId;//交互ID

    private EventHandler mEventHandler;

    private void Start()
    {
        mEventHandler = FindObjectOfType<EventHandler>();
    }

    public override void InteractiveDetection()
    {
        if (Input.GetButtonDown(InputInfo.Interactive_E) && mEventHandler != null)
        {
            //如果没有事件
            if (mEventHandler.GetEventStatus() == EventHandler.EventStatusEnum.EventEnd)
                mEventHandler.EventTriggerForLook(markId);
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