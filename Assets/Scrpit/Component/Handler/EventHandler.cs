using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class EventHandler : BaseSingleton<EventHandler>,ITextInfoView
{
    private TextInfoController mTextInfoController;

    private void Awake()
    {
        mTextInfoController = new TextInfoController(this,this);
    }

    /// <summary>
    /// 调查事件触发
    /// </summary>
    /// <param name="markId"></param>
    public void EventTriggerForLook(long markId)
    {
        mTextInfoController.GetTextForLook(markId);
    }

    /// <summary>
    /// 对话时间触发
    /// </summary>
    /// <param name="markId"></param>
    public void EventTriggerForTalk(long markId)
    {
    }
    /// <summary>
    /// 剧情触发
    /// </summary>
    /// <param name="markId"></param>
    public void EventTriggerForStory(long markId)
    {

    }

    #region 文本获取回调
    public void GetTextInfoSuccess(List<TextInfoBean> listData)
    {

    }

    public void GetTextInfoFail()
    {

    }
    #endregion
}