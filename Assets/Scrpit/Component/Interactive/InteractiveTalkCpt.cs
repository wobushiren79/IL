using UnityEngine;
using UnityEditor;

public class InteractiveTalkCpt : BaseInteractiveCpt
{
    public string interactiveContent;

    public long[] markIds;//交互ID

    public override void InteractiveDetection()
    {
        if (Input.GetButtonDown("Interactive_E"))
        {
            if (!EventHandler.Instance.GetEventStatus() && markIds != null && markIds.Length > 0)
                EventHandler.Instance.EventTriggerForTalk(RandomUtil.GetRandomDataByArray(markIds));
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