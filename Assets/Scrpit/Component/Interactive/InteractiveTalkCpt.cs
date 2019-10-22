using UnityEngine;
using UnityEditor;

public class InteractiveTalkCpt : BaseInteractiveCpt
{
    public string interactiveContent;

    public long[] markIds;//交互ID

    private EventHandler mEventHandler;

    private void Start()
    {
        mEventHandler = FindObjectOfType<EventHandler>();
    }

    public override void InteractiveDetection()
    {
        if (Input.GetButtonDown(InputInfo.Interactive_E) && mEventHandler != null)
        {
            if (mEventHandler.GetEventStatus() == EventHandler.EventStatusEnum.EventEnd && markIds != null && markIds.Length > 0)
                mEventHandler.EventTriggerForTalk(RandomUtil.GetRandomDataByArray(markIds));
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