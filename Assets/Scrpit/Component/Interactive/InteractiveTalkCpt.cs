using UnityEngine;
using UnityEditor;
using static GameEventHandler;

public class InteractiveTalkCpt : BaseInteractiveCpt
{
    public TextMesh tvContent;
    private BaseNpcAI mNpcAI;
    public string interactiveContent;


    private void Start()
    {
        mNpcAI = GetComponent<BaseNpcAI>();
    }

    public override void InteractiveDetection(CharacterInteractiveCpt characterInt)
    {
        if (Input.GetButtonDown(InputInfo.Interactive_E))
        {
            if (GameEventHandler.Instance.GetEventStatus() == EventStatusEnum.EventEnd)
            {
                //先改变人物面向
                if (characterInt.transform.position.x>transform.position.x)
                    mNpcAI.SetCharacterFace(2);
                else
                    mNpcAI.SetCharacterFace(1);
                //获取人物信息
                NpcInfoBean npcInfo = mNpcAI.characterData.npcInfoData;
                GameEventHandler.Instance.EventTriggerForTalk(npcInfo,true);
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