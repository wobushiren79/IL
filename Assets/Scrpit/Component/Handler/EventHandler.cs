using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class EventHandler : BaseHandler, UIGameText.ICallBack
{
    public enum EventTypeEnum
    {
        Talk,//对话事件
        Look,//调查事件
        Story,//故事事件
    }

    public enum EventStatusEnum
    {
        EventIng,//事件进行中
        EventEnd,//事件结束
    }

    public enum NotifyEventTypeEnum
    {
        EventEnd,//事件结束
        TalkForAddFavorability,//对话增加高感
    }

    public GameDataManager gameDataManager;
    public BaseUIManager uiManager;
    public StoryInfoManager storyInfoManager;
    public StoryBuilder storyBuilder;
    public ControlHandler controlHandler;

    private EventStatusEnum mEventStatus = EventStatusEnum.EventEnd;
    private EventTypeEnum mEventType;

    private StoryInfoBean mStoryInfo;
    /// <summary>
    /// 调查事件触发
    /// </summary>
    /// <param name="markId"></param>
    public void EventTriggerForLook(long markId)
    {
        SetEventStatus(EventStatusEnum.EventIng);
        SetEventType(EventTypeEnum.Look);
        //控制模式修改
        if (controlHandler != null)
            controlHandler.StopControl();
        UIGameText uiGameText = (UIGameText)uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameText));
        uiGameText.SetCallBack(this);
        uiGameText.SetData(TextEnum.Look, markId);
    }

    /// <summary>
    /// 对话时间触发
    /// </summary>
    /// <param name="markId"></param>
    public void EventTriggerForTalk(long markId)
    {
        SetEventStatus(EventStatusEnum.EventIng);
        SetEventType(EventTypeEnum.Talk);
        //控制模式修改
        if (controlHandler != null)
            controlHandler.StopControl();
        UIGameText uiGameText = (UIGameText)uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameText));
        uiGameText.SetData(TextEnum.Talk, markId);
        uiGameText.SetCallBack(this);
    }

    /// <summary>
    /// 剧情触发
    /// </summary>
    /// <param name="markId"></param>
    public void EventTriggerForStory(StoryInfoBean storyInfo)
    {
        this.mStoryInfo = storyInfo;
        SetEventStatus(EventStatusEnum.EventIng);
        SetEventType(EventTypeEnum.Story);
        //控制模式修改
        if (controlHandler != null)
            controlHandler.StartControl(ControlHandler.ControlEnum.Story);
        uiManager.CloseAllUI();
        //设置文本的回调
        UIGameText uiGameText=(UIGameText)uiManager.GetUIByName(EnumUtil.GetEnumName(UIEnum.GameText));
        uiGameText.SetCallBack(this);
        storyBuilder.BuildStory(storyInfo);
    }

    /// <summary>
    /// 根据ID触发故事
    /// </summary>
    /// <param name="id"></param>
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
    public void SetEventStatus(EventStatusEnum eventStatus)
    {
        mEventStatus = eventStatus;
        if (eventStatus == EventStatusEnum.EventEnd)
        {
            if (controlHandler != null)
                //事件结束 操作回复
                //如果是故事模式 则恢复普通控制状态
                if (controlHandler.GetControl() == controlHandler.GetControl(ControlHandler.ControlEnum.Story))
                {
                    controlHandler.StartControl(ControlHandler.ControlEnum.Normal);
                }
                else
                {
                    controlHandler.RestoreControl();
                }
            //保存数据
            if (gameDataManager != null && mStoryInfo != null)
                gameDataManager.gameData.AddTraggeredEvent(mStoryInfo.id);
            //打开主界面UI
            uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameMain));
            //通知事件结束
            NotifyAllObserver((int)NotifyEventTypeEnum.EventEnd);
        }
    }

    /// <summary>
    /// 获取事件状态
    /// </summary>
    /// <returns></returns>
    public EventStatusEnum GetEventStatus()
    {
        return mEventStatus;
    }

    /// <summary>
    ///设置事件状态
    /// </summary>
    /// <param name="eventType"></param>
    public void SetEventType(EventTypeEnum eventType)
    {
        this.mEventType=eventType;
    }

    /// <summary>
    /// 获取时间类型
    /// </summary>
    /// <returns></returns>
    public EventTypeEnum GetEventType()
    {
        return mEventType;
    }

    #region 对话文本回调
    public void UITextEnd()
    {
        switch (mEventType)
        {
            //如果是对话或者调查事件 UI文本结束时间也就结束了
            case EventTypeEnum.Look:
            case EventTypeEnum.Talk:
                SetEventStatus(EventStatusEnum.EventEnd);
                break;
            case EventTypeEnum.Story:
                storyBuilder.NextStoryOrder();
                break;
        }

    }

    public void UITextAddFavorability(long characterId, int favorability)
    {
        NotifyAllObserver((int)NotifyEventTypeEnum.TalkForAddFavorability, characterId, favorability);
    }
    #endregion

}