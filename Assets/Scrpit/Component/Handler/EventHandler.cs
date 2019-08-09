using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class EventHandler : BaseSingleton<EventHandler>, ITextInfoView
{
    public BaseUIManager uiManager;

    private TextInfoController mTextInfoController;

    public bool isEventing = false;//事件是否进行中

    private void Awake()
    {
        mTextInfoController = new TextInfoController(this, this);
    }

    /// <summary>
    /// 调查事件触发
    /// </summary>
    /// <param name="markId"></param>
    public void EventTriggerForLook(long markId)
    {
        isEventing = true;
        mTextInfoController.GetTextForLook(markId);
    }

    /// <summary>
    /// 对话时间触发
    /// </summary>
    /// <param name="markId"></param>
    public void EventTriggerForTalk(long markId)
    {
        isEventing = true;
        mTextInfoController.GetTextForTalk(markId);
    }
    /// <summary>
    /// 剧情触发
    /// </summary>
    /// <param name="markId"></param>
    public void EventTriggerForStory(long markId)
    {
        isEventing = true;
    }

    #region 文本获取回调
    public void GetTextInfoForLookSuccess(List<TextInfoBean> listData)
    {
        BaseUIComponent baseUIComponent = uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameText));
        ((UIGameText)baseUIComponent).SetData(TextEnum.Look, listData);
    }

    public void GetTextInfoForTalkSuccess(List<TextInfoBean> listData)
    {
        BaseUIComponent baseUIComponent = uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameText));
        ((UIGameText)baseUIComponent).SetData(TextEnum.Talk, listData);
    }

    public void GetTextInfoForStorySuccess(List<TextInfoBean> listData)
    {

    }

    public void GetTextInfoSuccess(List<TextInfoBean> listData)
    {

    }

    public void GetTextInfoFail()
    {

    }
    #endregion
}