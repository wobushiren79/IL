using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class EventHandler : BaseSingleton<EventHandler>
{
    public BaseUIManager uiManager;
    public StoryInfoManager storyInfoManager;
    public StoryBuilder storyBuilder;

    public bool isEventing = false;//事件是否进行中

    /// <summary>
    /// 调查事件触发
    /// </summary>
    /// <param name="markId"></param>
    public void EventTriggerForLook(long markId)
    {
        isEventing = true;
        BaseUIComponent baseUIComponent = uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameText));
        ((UIGameText)baseUIComponent).SetData(TextEnum.Look, markId);
    }

    /// <summary>
    /// 对话时间触发
    /// </summary>
    /// <param name="markId"></param>
    public void EventTriggerForTalk(long markId)
    {
        isEventing = true;
        BaseUIComponent baseUIComponent = uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameText));
        ((UIGameText)baseUIComponent).SetData(TextEnum.Talk, markId);
    }

    /// <summary>
    /// 剧情触发
    /// </summary>
    /// <param name="markId"></param>
    public void EventTriggerForStory(StoryInfoBean storyInfo)
    {
        isEventing = true;
        storyBuilder.BuildStory(storyInfo);
    }

    public void EventTriggerForStory(long id)
    {
        if (storyInfoManager == null)
            return;
        StoryInfoBean storyInfo = storyInfoManager.GetStoryInfoDataById(id);
        if (storyInfo != null)
            EventTriggerForStory(storyInfo);
    }

    /// <summary>
    /// 检测故事 自动触发剧情
    /// </summary>
    public void CheckEventForStory()
    {
        if (storyInfoManager == null)
            return;
        StoryInfoBean storyInfo = storyInfoManager.CheckStory();
        if (storyInfo != null)
            EventTriggerForStory(storyInfo);
    }
}