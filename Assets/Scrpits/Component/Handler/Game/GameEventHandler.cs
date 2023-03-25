using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using static GameControlHandler;
using System;

public class GameEventHandler : BaseHandler<GameEventHandler, GameEventManager>, UIGameText.ICallBack
{
    public enum EventTypeEnum
    {
        Talk,//对话事件
        TalkForRascal,//捣乱对话
        Look,//调查事件
        Story,//故事事件
        StoryForMiniGameCooking//故事烹饪游戏
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
        TextSelectResult,//文本选择
    }

    private EventStatusEnum mEventStatus = EventStatusEnum.EventEnd;
    private EventTypeEnum mEventType;

    protected StoryInfoBean storyInfo;

    protected Action<NotifyEventTypeEnum, object[]> notifyForEvent;
    public override void Awake()
    {
        base.Awake();
        GameTimeHandler.Instance.RegisterNotifyForTime(NotifyForTime);

    }
    private void OnDestroy()
    {
        GameTimeHandler.Instance.UnRegisterNotifyForTime(NotifyForTime);
    }
    public void InitData()
    {
        mEventStatus = EventStatusEnum.EventEnd;
        //通知事件结束
        if (storyInfo == null)
            notifyForEvent?.Invoke(NotifyEventTypeEnum.EventEnd, new object[] { -1 });
        else
            notifyForEvent?.Invoke(NotifyEventTypeEnum.EventEnd, new object[] { storyInfo.id });
        //移除所有观察者
        notifyForEvent = null;
        //显示重要NPC
        if (NpcHandler.Instance.buildForImportant)
        {
            NpcHandler.Instance.buildForImportant.ShowNpc();
        }
        if (NpcHandler.Instance.builderForFamily)
        {
            NpcHandler.Instance.builderForFamily.ShowNpc();
        }
    }

    public void RegisterNotifyForEvent(Action<NotifyEventTypeEnum, object[]> notifyForEvent)
    {
        this.notifyForEvent += notifyForEvent;
    }

    public void UnRegisterNotifyForEvent(Action<NotifyEventTypeEnum, object[]> notifyForEvent)
    {
        this.notifyForEvent -= notifyForEvent;
    }

    public void UnRegisterAllNotifyForEvent()
    {
        this.notifyForEvent = null;

    }

    /// <summary>
    /// 检测是否能触发事件
    /// </summary>
    /// <returns></returns>
    public bool CheckEventTrigger()
    {
        if (mEventStatus == EventStatusEnum.EventEnd)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 调查事件触发
    /// </summary>
    /// <param name="markId"></param>
    public bool EventTriggerForLook(long markId)
    {
        if (!CheckEventTrigger())
        {
            return false;
        }
        SetEventStatus(EventStatusEnum.EventIng);
        SetEventType(EventTypeEnum.Look);
        //暂停时间
        GameTimeHandler.Instance.SetTimeStop();
        //控制模式修改
        GameControlHandler.Instance.StopControl();

        UIGameText uiGameText = UIHandler.Instance.OpenUIAndCloseOther<UIGameText>();
        uiGameText.SetCallBack(this);
        uiGameText.SetData(TextEnum.Look, markId);
        return true;
    }

    /// <summary>
    /// 对话事件触发
    /// </summary>
    /// <param name="markId"></param>
    public bool EventTriggerForTalk(NpcInfoBean npcInfo, bool isStopTime)
    {
        if (!CheckEventTrigger())
        {
            return false;
        }
        SetEventStatus(EventStatusEnum.EventIng);
        SetEventType(EventTypeEnum.Talk);
        //暂停时间
        if (isStopTime)
            GameTimeHandler.Instance.SetTimeStop();
        //控制模式修改
        GameControlHandler.Instance.StopControl();

        UIGameText uiGameText = UIHandler.Instance.OpenUIAndCloseOther<UIGameText>();
        uiGameText.SetDataForTalk(npcInfo);
        uiGameText.SetCallBack(this);
        return true;
    }

    /// <summary>
    /// 对话事件触发
    /// </summary>
    /// <param name="markId"></param>
    public bool EventTriggerForTalk(long markId, bool isStopTime)
    {
        if (!CheckEventTrigger())
        {
            return false;
        }
        SetEventStatus(EventStatusEnum.EventIng);
        SetEventType(EventTypeEnum.Talk);
        //暂停时间
        if (isStopTime)
            GameTimeHandler.Instance.SetTimeStop();
        //控制模式修改
        GameControlHandler.Instance.StopControl();

        UIGameText uiGameText = UIHandler.Instance.OpenUIAndCloseOther<UIGameText>();
        uiGameText.SetData(TextEnum.Talk, markId);
        uiGameText.SetCallBack(this);
        return true;
    }

    /// <summary>
    /// 对话事件触发-捣乱者
    /// </summary>
    /// <param name="npcAIRascal"></param>
    /// <param name="markId"></param>
    /// <returns></returns>
    public bool EventTriggerForTalkByRascal(NpcAIRascalCpt npcAIRascal, long markId)
    {
        GameConfigBean gameConfig = GameDataHandler.Instance.manager.GetGameConfig();
        if (gameConfig.statusForEventCameraMove == 1)
        {
            //先还原层数
            ControlForWorkCpt controlForWork = GameControlHandler.Instance.manager.GetControl<ControlForWorkCpt>(ControlEnum.Work);
            if (controlForWork != null)
                controlForWork.SetLayer(1);
            //镜头跟随
            GameControlHandler.Instance.manager.GetControl().SetFollowPosition(npcAIRascal.transform.position);
        }
        float lastTimeScale = GameTimeHandler.Instance.GetTimeScale();
        bool isTrigger = EventTriggerForTalk(markId, false);
        if (gameConfig.statusForEventStopTimeScale == 0)
        {
            GameTimeHandler.Instance.SetTimeScale(lastTimeScale);
        }
        return isTrigger;
    }

    /// <summary>
    /// 对话事件触发-杂项
    /// </summary>
    /// <param name="npcAISundry"></param>
    /// <param name="markId"></param>
    /// <returns></returns>
    public bool EventTriggerForTalkBySundry(NpcAISundryCpt npcAISundry, long markId)
    {
        GameConfigBean gameConfig = GameDataHandler.Instance.manager.GetGameConfig();
        if (gameConfig.statusForEventCameraMove == 1)
        {
            //先还原层数
            ControlForWorkCpt controlForWork = GameControlHandler.Instance.manager.GetControl<ControlForWorkCpt>(GameControlHandler.ControlEnum.Work);
            if (controlForWork != null)
                controlForWork.SetLayer(1);
            //镜头跟随
            GameControlHandler.Instance.manager.GetControl().SetFollowPosition(npcAISundry.transform.position);
        }
        float lastTimeScale = GameTimeHandler.Instance.GetTimeScale();
        bool isTrigger = EventTriggerForTalk(markId, false);
        if (gameConfig.statusForEventStopTimeScale == 0)
        {
            GameTimeHandler.Instance.SetTimeScale(lastTimeScale);
        }
        return isTrigger;
    }

    /// <summary>
    /// 剧情触发
    /// </summary>
    /// <param name="markId"></param>
    public bool EventTriggerForStory(StoryInfoBean storyInfo)
    {
        if (!CheckEventTrigger())
        {
            return false;
        }
        this.storyInfo = storyInfo;
        SetEventStatus(EventStatusEnum.EventIng);
        SetEventType(EventTypeEnum.Story);
        //暂停时间
        GameTimeHandler.Instance.SetTimeStop();
        //控制模式修改
        BaseControl baseControl = GameControlHandler.Instance.StartControl<ControlForStoryCpt>(GameControlHandler.ControlEnum.Story);
        baseControl.transform.position = new Vector3(storyInfo.position_x, storyInfo.position_y);
        //隐藏重要NPC
        if (NpcHandler.Instance.buildForImportant != null)
        {
            NpcHandler.Instance.buildForImportant.HideNpc();
        }
        if (NpcHandler.Instance.builderForFamily != null)
        {
            NpcHandler.Instance.builderForFamily.HideNpc();
        }
        UIHandler.Instance.CloseAllUI();
        //设置文本的回调
        UIGameText uiGameText = UIHandler.Instance.GetUI<UIGameText>();
        uiGameText.SetCallBack(this);
        StoryInfoHandler.Instance.builderForStory.BuildStory(storyInfo);
        return true;
    }

    /// <summary>
    /// 根据ID触发故事
    /// </summary>
    /// <param name="id"></param>
    public bool EventTriggerForStory(long id)
    {
        if (!CheckEventTrigger())
        {
            return false;
        }

        StoryInfoBean storyInfo = StoryInfoHandler.Instance.manager.GetStoryInfoDataById(id);
        if (storyInfo != null)
            EventTriggerForStory(storyInfo);
        return true;
    }

    /// <summary>
    /// 检测故事 自动触发剧情
    /// </summary>
    public bool EventTriggerForStory(TownBuildingEnum positionType, int OutOrIn)
    {
        if (!CheckEventTrigger())
        {
            return false;
        }
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        StoryInfoBean storyInfo = StoryInfoHandler.Instance.manager.CheckStory(gameData, SceneUtil.GetCurrentScene(), positionType, OutOrIn);
        if (storyInfo != null)
        {
            EventTriggerForStory(storyInfo);
            return true;
        }
        else
            return false;
    }


    public bool EventTriggerForStory()
    {
        if (!CheckEventTrigger())
        {
            return false;
        }
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        StoryInfoBean storyInfo = StoryInfoHandler.Instance.manager.CheckStory(gameData, SceneUtil.GetCurrentScene());
        if (storyInfo != null)
        {
            EventTriggerForStory(storyInfo);
            return true;
        }
        else
            return false;
    }

    /// <summary>
    /// 烹饪游戏剧情触发
    /// </summary>
    /// <param name="gameCookingData"></param>
    /// <param name="id"></param>
    public bool EventTriggerForStoryCooking(MiniGameCookingBean gameCookingData, long id)
    {
        StoryInfoBean storyInfo = StoryInfoHandler.Instance.manager.GetStoryInfoDataById(id);
        if (storyInfo == null)
            return false;
        this.storyInfo = storyInfo;
        SetEventStatus(EventStatusEnum.EventIng);
        SetEventType(EventTypeEnum.StoryForMiniGameCooking);
        //控制模式修改
        BaseControl baseControl = GameControlHandler.Instance.StartControl<ControlForStoryCpt>(GameControlHandler.ControlEnum.Story);
        baseControl.transform.position = new Vector3(storyInfo.position_x, storyInfo.position_y);

        UIHandler.Instance.CloseAllUI();
        //设置文本的回调
        UIGameText uiGameText = UIHandler.Instance.GetUI<UIGameText>();
        uiGameText.SetCallBack(this);
        //设置文本的备用数据
        SortedList<string, string> listMarkData = GetMiniGameMarkStrData(gameCookingData);
        uiGameText.SetListMark(listMarkData);
        StoryInfoHandler.Instance.builderForStory.BuildStory(storyInfo);
        return true;
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

            //保存数据
            if (storyInfo != null)
            {
                GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
                gameData.AddTraggeredEvent(storyInfo.id);
            }
            if (mEventType != EventTypeEnum.StoryForMiniGameCooking)
            {
                //打开主界面UI
                UIHandler.Instance.OpenUIAndCloseOther<UIGameMain>();
                //恢复时间
                GameTimeHandler.Instance.SetTimeRestore();
            }

            //回复生成NPC
            //事件结束 操作回复
            if (mEventType == EventTypeEnum.Story)
            {
                NpcHandler.Instance.RestoreBuildNpc();
                GameControlHandler.Instance.StartControl<BaseControl>(ControlEnum.Normal);
            }
            else if (mEventType == EventTypeEnum.StoryForMiniGameCooking)
            {

            }
            else
            {
                GameControlHandler.Instance.RestoreControl();
            }

            //初始化数据
            InitData();

        }
        else if (eventStatus == EventStatusEnum.EventIng)
        {
            //暂停生成NPC
            if (mEventType == EventTypeEnum.Story)
                NpcHandler.Instance.StopBuildNpc();
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
        this.mEventType = eventType;
    }

    /// <summary>
    /// 获取时间类型
    /// </summary>
    /// <returns></returns>
    public EventTypeEnum GetEventType()
    {
        return mEventType;
    }

    /// <summary>
    /// 获取迷你游戏故事的备用文本数据
    /// </summary>
    /// <returns></returns>
    private SortedList<string, string> GetMiniGameMarkStrData(MiniGameBaseBean miniGameData)
    {
        SortedList<string, string> listData = new SortedList<string, string>();
        //为所有友方角色称呼 和 姓名
        string userCharacterList = "";
        foreach (MiniGameCharacterBean itemCharacter in miniGameData.listUserGameData)
        {
            userCharacterList += (itemCharacter.characterData.baseInfo.titleName + "" + itemCharacter.characterData.baseInfo.name) + " ";
        }
        listData.Add(GameSubstitutionInfo.MiniGame_UserNameList, userCharacterList);
        //为所有敌方角色称呼 和 姓名
        string enemyCharacterList = "";
        foreach (MiniGameCharacterBean itemCharacter in miniGameData.listEnemyGameData)
        {
            enemyCharacterList += (itemCharacter.characterData.baseInfo.titleName + "" + itemCharacter.characterData.baseInfo.name) + " ";
        }
        listData.Add(GameSubstitutionInfo.MiniGame_EnemyNameList, enemyCharacterList);

        if (miniGameData.gameType == MiniGameEnum.Cooking)
        {
            MiniGameCookingBean gameCookingData = (MiniGameCookingBean)miniGameData;
            //所有评审人员角色姓名
            string auditerCharaterList = "";
            foreach (MiniGameCharacterBean itemCharacter in gameCookingData.listAuditerGameData)
            {
                auditerCharaterList += (itemCharacter.characterData.baseInfo.titleName + "" + itemCharacter.characterData.baseInfo.name) + " ";
            }
            listData.Add(GameSubstitutionInfo.MiniGame_Cooking_AuditerNameList, auditerCharaterList);
            //料理的主题
            listData.Add(GameSubstitutionInfo.MiniGame_Cooking_Theme, gameCookingData.GetCookingTheme().name);
            //所有友方角色
            foreach (MiniGameCharacterBean itemCharacter in gameCookingData.listUserGameData)
            {
                MiniGameCharacterForCookingBean cookingCharacterData = (MiniGameCharacterForCookingBean)itemCharacter;
                if (cookingCharacterData.GetCookingMenuInfo() != null)
                    listData.Add(GameSubstitutionInfo.MiniGame_Cooking_UserFoodName, cookingCharacterData.GetCookingMenuInfo().name);
            }
        }
        return listData;
    }

    #region 对话文本回调
    public void UITextInitReady()
    {

    }

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
            case EventTypeEnum.StoryForMiniGameCooking:
                StoryInfoHandler.Instance.builderForStory.NextStoryOrder();
                break;
        }
    }

    public void UITextAddFavorability(long characterId, int favorability)
    {
        notifyForEvent?.Invoke(NotifyEventTypeEnum.TalkForAddFavorability, new object[] { characterId, favorability });
    }

    public void UITextSceneExpression(Dictionary<int, CharacterExpressionCpt.CharacterExpressionEnum> mapData)
    {
        foreach (var item in mapData)
        {
            int npcNum = item.Key;
            CharacterExpressionCpt.CharacterExpressionEnum expression = item.Value;
            GameObject objNpc = StoryInfoHandler.Instance.builderForStory.GetNpcByNpcNum(npcNum);
            NpcAIStoryCpt npcAI = objNpc.GetComponent<NpcAIStoryCpt>();
            npcAI.SetExpression(expression);
        }
    }

    public void UITextSelectResult(TextInfoBean textData, List<CharacterBean> listPickCharacterData)
    {
        if (!textData.pre_data_minigame.IsNull())
        {
            //小游戏初始化
            List<PreTypeForMiniGameBean> listPre = PreTypeForMiniGameEnumTools.GetListPreData(textData.pre_data_minigame);
            List<RewardTypeBean> listReward = RewardTypeEnumTools.GetListRewardData(textData.reward_data);
            MiniGameBaseBean miniGameData = PreTypeForMiniGameEnumTools.GetMiniGameData(null, textData.pre_data_minigame, listPickCharacterData);
            miniGameData.listReward = listReward;
            switch (miniGameData.gameType)
            {
                case MiniGameEnum.Combat:
                    MiniGameHandler.Instance.handlerForCombat.RegisterNotifyForMiniGameStatus(NotifyForMiniGameStatus);
                    MiniGameHandler.Instance.handlerForCombat.InitGame((MiniGameCombatBean)miniGameData);
                    break;
                case MiniGameEnum.Debate:
                    MiniGameHandler.Instance.handlerForDebate.RegisterNotifyForMiniGameStatus(NotifyForMiniGameStatus);
                    MiniGameHandler.Instance.handlerForDebate.InitGame((MiniGameDebateBean)miniGameData);
                    break;
            }
            //隐藏重要NPC
            if (NpcHandler.Instance.buildForImportant)
                NpcHandler.Instance.buildForImportant.HideNpc();
            if (NpcHandler.Instance.builderForFamily)
                NpcHandler.Instance.builderForFamily.HideNpc();
        }
        notifyForEvent?.Invoke(NotifyEventTypeEnum.TextSelectResult, new object[] { textData });
    }
    #endregion


    #region 回调处理
    public void NotifyForMiniGameStatus(MiniGameStatusEnum type, params object[] obj)
    {
        switch (type)
        {
            case MiniGameStatusEnum.Gameing:
                break;
            case MiniGameStatusEnum.GameEnd:
                break;
            case MiniGameStatusEnum.GameClose:
                MiniGameBaseBean miniGameData = (MiniGameBaseBean)obj[0];
                GameControlHandler.Instance.StartControl<BaseControl>(ControlEnum.Normal);
                SetEventStatus(EventStatusEnum.EventEnd);
                if (miniGameData.GetGameResult() == MiniGameResultEnum.Win)
                {
                    if (miniGameData.gameResultWinTalkMarkId != 0)
                        EventTriggerForTalk(miniGameData.gameResultWinTalkMarkId, true);
                }
                else
                {
                    if (miniGameData.gameResultLoseTalkMarkId != 0)
                        EventTriggerForTalk(miniGameData.gameResultLoseTalkMarkId, true);
                }
                break;
        }
    }
    public void NotifyForTime(GameTimeHandler.NotifyTypeEnum notifyType, float timeHour)
    {
        if (notifyType == GameTimeHandler.NotifyTypeEnum.NewDay)
        {
            InitData();
        }
        else if (notifyType == GameTimeHandler.NotifyTypeEnum.EndDay)
        {

        }
    }

    #endregion
}