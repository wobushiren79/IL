using UnityEngine;
using UnityEditor;

public class InteractiveTalkCpt : BaseInteractiveCpt
{
    private BaseNpcAI mNpcAI;
    public string interactiveContent;

    private EventHandler mEventHandler;

    private void Start()
    {
        mNpcAI = GetComponent<BaseNpcAI>();
        mEventHandler = FindObjectOfType<EventHandler>();
    }

    public override void InteractiveDetection()
    {
        if (Input.GetButtonDown(InputInfo.Interactive_E) && mEventHandler != null)
        {
            if (mEventHandler.GetEventStatus() == EventHandler.EventStatusEnum.EventEnd)
            {
                NpcInfoBean npcInfo = mNpcAI.characterData.npcInfoData;
                mEventHandler.EventTriggerForTalk(npcInfo.id, (NPCTypeEnum)npcInfo.npc_type);
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