using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class InteractiveLookCpt : BaseInteractiveCpt, TextInfoManager.ICallBack
{
    public long markId;//交互ID

    protected EventHandler eventHandler;
    protected TextInfoManager textInfoManager;

    private void Awake()
    {
        eventHandler = Find<EventHandler>(ImportantTypeEnum.EventHandler);
        textInfoManager = Find<TextInfoManager>(ImportantTypeEnum.TextManager);
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
        textInfoManager.SetCallBack(this);
        textInfoManager.GetTextById(TextEnum.Look, markId);
    }

    #region 数据回调
    public void SetTextInfoForLook(List<TextInfoBean> listData)
    {
        if (!CheckUtil.ListIsNull(listData))
            characterInt.ShowInteractive(listData[0].name);
    }

    public void SetTextInfoForStory(List<TextInfoBean> listData)
    {
    }

    public void SetTextInfoForTalkByFirstMeet(List<TextInfoBean> listData)
    {
    }

    public void SetTextInfoForTalkByMarkId(List<TextInfoBean> listData)
    {
    }

    public void SetTextInfoForTalkByUserId(List<TextInfoBean> listData)
    {
    }

    public void SetTextInfoForTalkByType(TextTalkTypeEnum textTalkType, List<TextInfoBean> listData)
    {
    }

    public void SetTextInfoForTalkOptions(List<TextInfoBean> listData)
    {
    }
    #endregion
}