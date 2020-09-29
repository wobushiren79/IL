using UnityEngine;
using UnityEditor;

public class InteractiveTalkCpt : BaseInteractiveCpt
{
    public TextMesh tvContent;
    private BaseNpcAI mNpcAI;
    public string interactiveContent;

    private EventHandler mEventHandler;

    private void Start()
    {
        mNpcAI = GetComponent<BaseNpcAI>();
        mEventHandler = Find<EventHandler>( ImportantTypeEnum.EventHandler);
    }

    public override void InteractiveDetection(CharacterInteractiveCpt characterInt)
    {
        if (Input.GetButtonDown(InputInfo.Interactive_E) && mEventHandler != null)
        {
            if (mEventHandler.GetEventStatus() == EventHandler.EventStatusEnum.EventEnd)
            {
                //先改变人物面向
                if (characterInt.transform.position.x>transform.position.x)
                    mNpcAI.SetCharacterFace(2);
                else
                    mNpcAI.SetCharacterFace(1);
                //获取人物信息
                NpcInfoBean npcInfo = mNpcAI.characterData.npcInfoData;
                mEventHandler.EventTriggerForTalk(npcInfo,true);
                //如果角色有问题提示。则取消问号
                mNpcAI.CancelExpression();
            }
        }
    }

    public override void InteractiveEnd(CharacterInteractiveCpt characterInt)
    {
        characterInt.CloseInteractive();
    }

    public override void InteractiveStart(CharacterInteractiveCpt characterInt)
    {
        characterInt.ShowInteractive(mNpcAI.characterData.baseInfo.name);
    }
}