using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class EventHandler : BaseHandler,
    UIGameText.ICallBack,
    IBaseObserver
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

    protected GameDataManager gameDataManager;
    protected GameItemsManager gameItemsManager;
    protected BaseUIManager uiManager;
    protected StoryInfoManager storyInfoManager;
    protected NpcInfoManager npcInfoManager;
    protected StoryBuilder storyBuilder;
    protected ControlHandler controlHandler;
    protected GameTimeHandler gameTimeHandler;
    protected NpcImportantBuilder npcImportantBuilder;

    protected MiniGameCombatHandler miniGameCombatHandler;
    protected MiniGameDebateHandler miniGameDebateHandler;

    private EventStatusEnum mEventStatus = EventStatusEnum.EventEnd;
    private EventTypeEnum mEventType;
    private Vector3 mEventPosition = Vector3.zero;

    private StoryInfoBean mStoryInfo;

    private void Awake()
    {
        gameDataManager = Find<GameDataManager>(ImportantTypeEnum.GameDataManager);
        gameItemsManager = Find<GameItemsManager>(ImportantTypeEnum.GameItemsManager);
        uiManager = Find<BaseUIManager>(ImportantTypeEnum.GameUI);
        storyInfoManager = Find<StoryInfoManager>(ImportantTypeEnum.StoryManager);
        npcInfoManager = Find<NpcInfoManager>(ImportantTypeEnum.NpcManager);
        storyBuilder = Find<StoryBuilder>(ImportantTypeEnum.StoryBuilder);
        controlHandler = Find<ControlHandler>(ImportantTypeEnum.ControlHandler);

        miniGameCombatHandler = Find<MiniGameCombatHandler>(ImportantTypeEnum.MiniGameHandler);
        miniGameDebateHandler = Find<MiniGameDebateHandler>(ImportantTypeEnum.MiniGameHandler);

        npcImportantBuilder = Find<NpcImportantBuilder>(ImportantTypeEnum.NpcBuilder);
        gameTimeHandler = Find<GameTimeHandler>(ImportantTypeEnum.TimeHandler);
        if (miniGameCombatHandler != null)
            miniGameCombatHandler.AddObserver(this);
        if (miniGameDebateHandler != null)
            miniGameDebateHandler.AddObserver(this);
        if (gameTimeHandler != null)
            gameTimeHandler.AddObserver(this);
    }

    public void InitData()
    {
        mEventStatus = EventStatusEnum.EventEnd;
        mEventPosition = Vector3.zero;
        //通知事件结束
        if (mStoryInfo == null)
            NotifyAllObserver((int)NotifyEventTypeEnum.EventEnd);
        else
            NotifyAllObserver((int)NotifyEventTypeEnum.EventEnd, mStoryInfo.id);
        //移除所有观察者
        RemoveAllObserver();
        //显示重要NPC
        if (npcImportantBuilder != null)
            npcImportantBuilder.ShowNpc();
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
        if (gameTimeHandler != null)
            gameTimeHandler.SetTimeStop();
        //控制模式修改
        if (controlHandler != null)
            controlHandler.StopControl();
        UIGameText uiGameText = (UIGameText)uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameText));
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
        if (gameTimeHandler != null && isStopTime)
            gameTimeHandler.SetTimeStop();
        //控制模式修改
        if (controlHandler != null)
            controlHandler.StopControl();
        UIGameText uiGameText = (UIGameText)uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameText));
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
        if (gameTimeHandler != null && isStopTime)
            gameTimeHandler.SetTimeStop();
        //控制模式修改
        if (controlHandler != null)
            controlHandler.StopControl();
        UIGameText uiGameText = (UIGameText)uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameText));
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
        if (controlHandler != null&&GameCommonInfo.GameConfig.statusForEventCameraMove == 1)
        {
            //先还原层数
            ControlForWorkCpt controlForWork =(ControlForWorkCpt) controlHandler.GetControl(ControlHandler.ControlEnum.Work);
            if (controlForWork != null)
                controlForWork.SetLayer(1);
            //镜头跟随
            controlHandler.GetControl().SetFollowPosition(npcAIRascal.transform.position);
        }
        return EventTriggerForTalk(markId, false);
    }

    /// <summary>
    /// 对话事件触发-杂项
    /// </summary>
    /// <param name="npcAISundry"></param>
    /// <param name="markId"></param>
    /// <returns></returns>
    public bool EventTriggerForTalkBySundry(NpcAISundryCpt npcAISundry, long markId)
    {
        if (controlHandler != null && GameCommonInfo.GameConfig.statusForEventCameraMove == 1)
        {
            //先还原层数
            ControlForWorkCpt controlForWork = (ControlForWorkCpt)controlHandler.GetControl(ControlHandler.ControlEnum.Work);
            if (controlForWork != null)
                controlForWork.SetLayer(1);
            //镜头跟随
            controlHandler.GetControl().SetFollowPosition(npcAISundry.transform.position);
        }
        return EventTriggerForTalk(markId, false);
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
        this.mStoryInfo = storyInfo;
        mEventPosition = new Vector3(storyInfo.position_x, storyInfo.position_y);
        SetEventStatus(EventStatusEnum.EventIng);
        SetEventType(EventTypeEnum.Story);
        //暂停时间
        if (gameTimeHandler != null)
            gameTimeHandler.SetTimeStop();
        //控制模式修改
        if (controlHandler != null)
        {
            BaseControl baseControl = controlHandler.StartControl(ControlHandler.ControlEnum.Story);
            baseControl.transform.position = new Vector3(storyInfo.position_x, storyInfo.position_y);
        }
        //隐藏重要NPC
        if (npcImportantBuilder != null)
            npcImportantBuilder.HideNpc();
        uiManager.CloseAllUI();
        //设置文本的回调
        UIGameText uiGameText = (UIGameText)uiManager.GetUIByName(EnumUtil.GetEnumName(UIEnum.GameText));
        uiGameText.SetCallBack(this);
        storyBuilder.BuildStory(storyInfo);
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
        if (storyInfoManager == null)
            return false;
        StoryInfoBean storyInfo = storyInfoManager.GetStoryInfoDataById(id);
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
        if (storyInfoManager == null)
            return false;
        StoryInfoBean storyInfo = storyInfoManager.CheckStory(gameDataManager.gameData, positionType, OutOrIn);
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
    /// 烹饪游戏剧情触发
    /// </summary>
    /// <param name="gameCookingData"></param>
    /// <param name="id"></param>
    public bool EventTriggerForStoryCooking(MiniGameCookingBean gameCookingData, long id)
    {
        if (storyInfoManager == null)
            return false;
        StoryInfoBean storyInfo = storyInfoManager.GetStoryInfoDataById(id);
        if (storyInfo == null)
            return false;
        this.mStoryInfo = storyInfo;
        SetEventStatus(EventStatusEnum.EventIng);
        SetEventType(EventTypeEnum.StoryForMiniGameCooking);
        //控制模式修改
        if (controlHandler != null)
        {
            BaseControl baseControl = controlHandler.StartControl(ControlHandler.ControlEnum.Story);
            baseControl.transform.position = new Vector3(storyInfo.position_x, storyInfo.position_y);
        }

        uiManager.CloseAllUI();
        //设置文本的回调
        UIGameText uiGameText = (UIGameText)uiManager.GetUIByName(EnumUtil.GetEnumName(UIEnum.GameText));
        uiGameText.SetCallBack(this);
        //设置文本的备用数据
        SortedList<string, string> listMarkData = GetMiniGameMarkStrData(gameCookingData);
        uiGameText.SetListMark(listMarkData);
        storyBuilder.BuildStory(storyInfo);
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
            //恢复时间
            if (gameTimeHandler != null)
                gameTimeHandler.SetTimeRestore();
            //初始化数据
            InitData();

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
                storyBuilder.NextStoryOrder();
                break;
        }
    }

    public void UITextAddFavorability(long characterId, int favorability)
    {
        NotifyAllObserver((int)NotifyEventTypeEnum.TalkForAddFavorability, characterId, favorability);
    }

    public void UITextSceneExpression(Dictionary<int, CharacterExpressionCpt.CharacterExpressionEnum> mapData)
    {
        foreach (var item in mapData)
        {
            int npcNum = item.Key;
            CharacterExpressionCpt.CharacterExpressionEnum expression = item.Value;
            GameObject objNpc = storyBuilder.GetNpcByNpcNum(npcNum);
            NpcAIStoryCpt npcAI = objNpc.GetComponent<NpcAIStoryCpt>();
            npcAI.SetExpression(expression);
        }
    }

    public void UITextSelectResult(TextInfoBean textData, List<CharacterBean> listPickCharacterData)
    {
        if (!CheckUtil.StringIsNull(textData.pre_data_minigame))
        {
            //小游戏初始化
            List<PreTypeForMiniGameBean> listPre = PreTypeForMiniGameEnumTools.GetListPreData(textData.pre_data_minigame);
            List<RewardTypeBean> listReward = RewardTypeEnumTools.GetListRewardData(textData.reward_data);
            MiniGameBaseBean miniGameData = PreTypeForMiniGameEnumTools.GetMiniGameData(null, textData.pre_data_minigame, listPickCharacterData, gameItemsManager, npcInfoManager);
            miniGameData.listReward = listReward;
            switch (miniGameData.gameType)
            {
                case MiniGameEnum.Combat:
                    miniGameCombatHandler.InitGame((MiniGameCombatBean)miniGameData);
                    break;
                case MiniGameEnum.Debate:
                    miniGameDebateHandler.InitGame((MiniGameDebateBean)miniGameData);
                    break;
            }
            mEventPosition = miniGameData.miniGamePosition;
            //隐藏重要NPC
            if (npcImportantBuilder != null)
                npcImportantBuilder.HideNpc();
        }
        NotifyAllObserver((int)NotifyEventTypeEnum.TextSelectResult, textData);
    }
    #endregion


    #region 回调处理
    public void ObserbableUpdate<T>(T observable, int type, params object[] obj) where T : Object
    {
        if (observable as MiniGameCombatHandler
            || observable as MiniGameDebateHandler)
        {
            switch (type)
            {
                case (int)BaseMiniGameHandler<BaseMiniGameBuilder, MiniGameBaseBean>.MiniGameStatusEnum.Gameing:
                    break;
                case (int)BaseMiniGameHandler<BaseMiniGameBuilder, MiniGameBaseBean>.MiniGameStatusEnum.GameEnd:
                    break;
                case (int)BaseMiniGameHandler<BaseMiniGameBuilder, MiniGameBaseBean>.MiniGameStatusEnum.GameClose:
                    MiniGameBaseBean miniGameData = (MiniGameBaseBean)obj[0];
                    controlHandler.StartControl(ControlHandler.ControlEnum.Normal);
                    SetEventStatus(EventStatusEnum.EventEnd);
                    if (miniGameData.gameResult == 0)
                    {
                        if (miniGameData.gameResultLoseTalkMarkId != 0)
                            EventTriggerForTalk(miniGameData.gameResultLoseTalkMarkId, true);
                    }
                    else
                    {
                        if (miniGameData.gameResultWinTalkMarkId != 0)
                            EventTriggerForTalk(miniGameData.gameResultWinTalkMarkId, true);
                    }
                    break;
            }
        }
        else if (observable == gameTimeHandler)
        {
            if (type == (int)GameTimeHandler.NotifyTypeEnum.NewDay)
            {
                InitData();
            }
            else if (type == (int)GameTimeHandler.NotifyTypeEnum.EndDay)
            {

            }
        }
    }

    #endregion
}