using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class EventHandler : BaseSingleton<EventHandler>
{
    public GameDataManager gameDataManager;
    public BaseUIManager uiManager;
    public StoryInfoManager storyInfoManager;
    public StoryBuilder storyBuilder;
    public ControlHandler controlHandler;

    private bool mIsEventing = false;//事件是否进行中

    /// <summary>
    /// 调查事件触发
    /// </summary>
    /// <param name="markId"></param>
    public void EventTriggerForLook(long markId)
    {
        ChangeEventStatus(true);
        BaseUIComponent baseUIComponent = uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameText));
        ((UIGameText)baseUIComponent).SetData(TextEnum.Look, markId);
    }

    /// <summary>
    /// 对话时间触发
    /// </summary>
    /// <param name="markId"></param>
    public void EventTriggerForTalk(long markId)
    {
        ChangeEventStatus(true);
        BaseUIComponent baseUIComponent = uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameText));
        ((UIGameText)baseUIComponent).SetData(TextEnum.Talk, markId);
    }

    /// <summary>
    /// 剧情触发
    /// </summary>
    /// <param name="markId"></param>
    public void EventTriggerForStory(StoryInfoBean storyInfo)
    {
        ChangeEventStatus(true);
        uiManager.CloseAllUI();
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
    public bool EventTriggerForStory()
    {
        if (storyInfoManager == null)
            return false;
        StoryInfoBean storyInfo = storyInfoManager.CheckStory(gameDataManager.gameData);
        if (storyInfo != null)
        {
            EventTriggerForStory(storyInfo);
            return true;
        }
        else
            return false;
           
    }

    /// <summary>
    /// 改变事件状态
    /// </summary>
    /// <param name="isEvent"></param>
    public void ChangeEventStatus(bool isEvent)
    {
        mIsEventing = isEvent;
        if (controlHandler!=null)
            if (isEvent)
            {
                controlHandler.StartControl(ControlHandler.ControlEnum.Story);
            }
            else
            {
                controlHandler.StartControl(ControlHandler.ControlEnum.Normal);
            }  
    }

    /// <summary>
    /// 获取时间状态
    /// </summary>
    /// <returns></returns>
    public bool GetEventStatus()
    {
        return mIsEventing;
    }
}