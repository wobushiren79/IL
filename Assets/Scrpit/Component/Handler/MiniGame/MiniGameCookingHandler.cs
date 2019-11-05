using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class MiniGameCookingHandler : BaseMiniGameHandler<MiniGameCookingBuilder, MiniGameCookingBean>,
    UIMiniGameCookingSelect.ICallBack,
    UIMiniGameCooking.ICallBack,
    IBaseObserver
{
    //事件处理
    public EventHandler eventHandler;
    public GameItemsManager gameItemsManager;

    private void Awake()
    {
        gameItemsManager = FindObjectOfType<GameItemsManager>();
    }

    /// <summary>
    /// 初始化游戏
    /// </summary>
    /// <param name="miniGameData"></param>
    public override void InitGame(MiniGameCookingBean miniGameData)
    {
        base.InitGame(miniGameData);
        miniGameBuilder.CreateAllCharacter(
            miniGameData.listUserGameData, miniGameData.userStartPosition,
            miniGameData.listEnemyGameData, miniGameData.listEnemyStartPosition,
            miniGameData.listAuditerGameData, miniGameData.listAuditerStartPosition,
            miniGameData.listCompereGameData, miniGameData.listCompereStartPosition);
        //设置通告板内容
        List<MiniGameCookingCallBoardCpt> listCallBoard = miniGameBuilder.GetListCallBoard();
        foreach (MiniGameCookingCallBoardCpt itemCpt in listCallBoard)
            itemCpt.SetCallBoardContent(miniGameData.cookingTheme.name);
        //给评审人员分配桌子
        List<MiniGameCookingAuditTableCpt> listAuditTable = miniGameBuilder.GetListAuditTable();
        List<NpcAIMiniGameCookingCpt> listAuditNpcAI = miniGameBuilder.GetCharacterByType(NpcAIMiniGameCookingCpt.MiniGameCookingNpcTypeEnum.Auditer);
        for (int i = 0; i < listAuditNpcAI.Count; i++)
        {
            NpcAIMiniGameCookingCpt itemNpc = listAuditNpcAI[i];
            MiniGameCookingAuditTableCpt itemTable = listAuditTable[i];
            itemNpc.SetAuditTable(itemTable);
        }
        //参赛选手相关设定
        List<MiniGameCookingStoveCpt> listStove = miniGameBuilder.GetListStove();
        List<NpcAIMiniGameCookingCpt> listPlayerNpcAI = miniGameBuilder.GetCharacterByType(NpcAIMiniGameCookingCpt.MiniGameCookingNpcTypeEnum.Player);
        for (int i = 0; i < listPlayerNpcAI.Count; i++)
        {
            //如果没有给定敌方角色的菜品 那就随机给参赛的敌方角色设置菜品
            NpcAIMiniGameCookingCpt itemNpc = listPlayerNpcAI[i];
            if (itemNpc.characterMiniGameData.cookingMenuInfo == null && itemNpc.characterMiniGameData.characterType == 0)
            {
                itemNpc.characterMiniGameData.cookingMenuInfo = uiGameManager.innFoodManager.GetRandomFoodDataByCookingTheme(miniGameData.cookingTheme);
            }
            //给参赛选手分配灶台
            MiniGameCookingStoveCpt itemTable = listStove[i];
            itemNpc.SetStove(itemTable);
        }
        //打开倒计时UI
        OpenCountDownUI(miniGameData, false);
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    public override void StartGame()
    {
        base.StartGame();
        //显示主持人
        miniGameBuilder.SetCompereCharacterActive(true);
        //评审找位置
        List<NpcAIMiniGameCookingCpt> listAuditNpcAI = miniGameBuilder.GetCharacterByType(NpcAIMiniGameCookingCpt.MiniGameCookingNpcTypeEnum.Auditer);
        for (int i = 0; i < listAuditNpcAI.Count; i++)
        {
            NpcAIMiniGameCookingCpt itemNpc = listAuditNpcAI[i];
            itemNpc.OpenAI();
            itemNpc.SetIntent(NpcAIMiniGameCookingCpt.MiniGameCookingIntentEnum.GoToAuditTable);
        }
        //选手找位置
        List<NpcAIMiniGameCookingCpt> listPlayerNpcAI = miniGameBuilder.GetCharacterByType(NpcAIMiniGameCookingCpt.MiniGameCookingNpcTypeEnum.Player);
        for (int i = 0; i < listPlayerNpcAI.Count; i++)
        {
            NpcAIMiniGameCookingCpt itemNpc = listPlayerNpcAI[i];
            itemNpc.OpenAI();
            itemNpc.SetIntent(NpcAIMiniGameCookingCpt.MiniGameCookingIntentEnum.GoToStove);
        }
        //打开游戏控制器
        if (controlHandler != null)
        {
            BaseControl baseControl = controlHandler.StartControl(ControlHandler.ControlEnum.MiniGameCooking);
            baseControl.SetCameraFollowObj(miniGameBuilder.GetUserCharacter().gameObject);
        }
    }

    /// <summary>
    /// 开始选择制作的食物
    /// </summary>
    public void StartSelectMenu()
    {
        //打开游戏UI
        UIMiniGameCookingSelect uiMiniGameCookingSelect = (UIMiniGameCookingSelect)uiGameManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.MiniGameCookingSelect));
        uiMiniGameCookingSelect.SetCallBack(this);
        uiMiniGameCookingSelect.SetData(miniGameData);
    }

    /// <summary>
    /// 开始准备阶段的料理游戏
    /// </summary>
    public void StartPreCooking(MenuInfoBean menuInfo)
    {
        //计算游戏时间
        float gameTiming = 10;
        miniGameBuilder.GetUserCharacter().characterData.GetAttributes(gameItemsManager, out CharacterAttributesBean attributes);
        gameTiming += (attributes.cook * 0.5f);
        //打开UI
        UIMiniGameCooking uiMiniGameCooking = (UIMiniGameCooking)uiGameManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.MiniGameCooking));
        uiMiniGameCooking.SetData(miniGameData, gameTiming);
        uiMiniGameCooking.SetCallBack(this);
        uiMiniGameCooking.StartCookingPre();
        //角色就位
        NpcAIMiniGameCookingCpt npcAI = miniGameBuilder.GetUserCharacter();
        npcAI.characterMiniGameData.cookingMenuInfo = menuInfo;
        npcAI.SetIntent(NpcAIMiniGameCookingCpt.MiniGameCookingIntentEnum.CookingPre);
    }

    /// <summary>
    /// 开始制作料理阶段游戏
    /// </summary>
    public void StartMakingCooking()
    {
        //打开UI
        UIMiniGameCooking uiMiniGameCooking = (UIMiniGameCooking)uiGameManager.GetOpenUI();
        uiMiniGameCooking.StartCookingMaking();
        //角色就位
        NpcAIMiniGameCookingCpt npcAI = miniGameBuilder.GetUserCharacter();
        npcAI.SetIntent(NpcAIMiniGameCookingCpt.MiniGameCookingIntentEnum.CookingMaking);
    }

    /// <summary>
    /// 开始摆盘料理阶段游戏
    /// </summary>
    public void StartEndCooking()
    {
        //打开UI
        UIMiniGameCooking uiMiniGameCooking = (UIMiniGameCooking)uiGameManager.GetOpenUI();
        uiMiniGameCooking.StartCookingEnd();
        //角色就位
        NpcAIMiniGameCookingCpt npcAI = miniGameBuilder.GetUserCharacter();
        npcAI.SetIntent(NpcAIMiniGameCookingCpt.MiniGameCookingIntentEnum.CookingEnd);
    }

    /// <summary>
    /// 开始审核
    /// </summary>
    public void StartAudit()
    {
        uiGameManager.CloseAllUI();
        List<NpcAIMiniGameCookingCpt> listPlayer = miniGameBuilder.GetCharacterByType(NpcAIMiniGameCookingCpt.MiniGameCookingNpcTypeEnum.Player);
        foreach (NpcAIMiniGameCookingCpt itemNpc in listPlayer)
        {
            itemNpc.SetIntent(NpcAIMiniGameCookingCpt.MiniGameCookingIntentEnum.GoToAudit);
        }
    }

    /// <summary>
    /// 开始故事 游戏开场
    /// </summary>
    public void StartStoryForGameOpen()
    {
        //因为剧情需要，先隐藏主持人
        miniGameBuilder.SetCompereCharacterActive(false);
        if (eventHandler != null)
        {
            eventHandler.AddObserver(this);
            eventHandler.EventTriggerForStoryCooking(miniGameData, miniGameData.storyGameOpenId);
        }
    }

    /// <summary>
    ///  开始故事 审核故事 
    /// </summary>
    public void StartStoryForGameAudit()
    {
        //因为剧情需要，先隐藏主持人
        miniGameBuilder.SetCompereCharacterActive(false);
        if (eventHandler != null)
        {
            eventHandler.AddObserver(this);
            eventHandler.EventTriggerForStoryCooking(miniGameData, miniGameData.storyGameAuditId);
        }
    }

    /// <summary>
    /// 呈上自己的作品
    /// </summary>
    public void PresentUserFoodForAudit()
    {
        PresentFoodForAudit(miniGameBuilder.GetUserCharacter());
    }

    /// <summary>
    /// 呈上某人的作品
    /// </summary>
    /// <param name="miniGameCharacterData"></param>
    public void PresentFoodForAudit(NpcAIMiniGameCookingCpt npc)
    {
        //获取所有评审
        List<NpcAIMiniGameCookingCpt> listAuditer = miniGameBuilder.GetCharacterByType(NpcAIMiniGameCookingCpt.MiniGameCookingNpcTypeEnum.Auditer);
        foreach (NpcAIMiniGameCookingCpt itemAuditer in listAuditer)
        {
            //复制食物给所有评审
            MiniGameCookingAuditTableCpt auditTableCpt = itemAuditer.auditTableCpt;
            GameObject objFoodForCover = Instantiate(auditTableCpt.objFoodPosition, npc.foodForCover.gameObject);
            objFoodForCover.transform.localPosition = Vector3.zero;
            objFoodForCover.transform.localScale = new Vector3(1, 1, 1);
            itemAuditer.foodForCover = objFoodForCover.GetComponent<FoodForCoverCpt>();
            itemAuditer.auditTargetNpc = npc;
        }
        //隐藏自己手上的食物
        npc.foodForCover.gameObject.SetActive(false);
    }

    /// <summary>
    /// 展示审核的食物
    /// </summary>
    public void ShowFoodForAudit()
    {
        List<MiniGameCookingAuditTableCpt> listTable = miniGameBuilder.GetListAuditTable();
        foreach (MiniGameCookingAuditTableCpt itemTable in listTable)
        {
            FoodForCoverCpt foodCoverCpt = itemTable.GetFood();
            if (foodCoverCpt != null)
                foodCoverCpt.ShowFood();
        }
    }
    
    /// <summary>
    /// 吃掉食物
    /// </summary>
    public void EatFoodForAudit()
    {
        //获取所有评审
        List<NpcAIMiniGameCookingCpt> listAuditer = miniGameBuilder.GetCharacterByType(NpcAIMiniGameCookingCpt.MiniGameCookingNpcTypeEnum.Auditer);
        foreach (NpcAIMiniGameCookingCpt itemAuditer in listAuditer)
        {
            itemAuditer.SetIntent(NpcAIMiniGameCookingCpt.MiniGameCookingIntentEnum.EatFood);
        }
    }

    /// <summary>
    /// 展示分数
    /// </summary>
    /// <param name="type">类型 1题 2色 3香 4味</param>
    public void ShowUserScoreForAudit(int type)
    {
        //获取所有评审
        List<NpcAIMiniGameCookingCpt> listAuditer = miniGameBuilder.GetCharacterByType(NpcAIMiniGameCookingCpt.MiniGameCookingNpcTypeEnum.Auditer);
        foreach (NpcAIMiniGameCookingCpt itemAuditer in listAuditer)
        {
            itemAuditer.SetIntent(NpcAIMiniGameCookingCpt.MiniGameCookingIntentEnum.AuditFood, type);
        }
    }

    /// <summary>
    /// 关闭分数展示
    /// </summary>
    public void CloseScoreForAudit()
    {
        List<NpcAIMiniGameCookingCpt> listAuditer = miniGameBuilder.GetCharacterByType(NpcAIMiniGameCookingCpt.MiniGameCookingNpcTypeEnum.Auditer);
        foreach (NpcAIMiniGameCookingCpt itemAuditer in listAuditer)
        {
            itemAuditer.CloseScore();
        }
    }

    #region 通知回调
    public void ObserbableUpdate<T>(T observable, int type, params object[] obj) where T : UnityEngine.Object
    {
        if (observable == eventHandler)
        {
            if (type == (int)EventHandler.NotifyEventTypeEnum.EventEnd)
            {
                if (Convert.ToInt64(obj[0]) == miniGameData.storyGameOpenId)
                {
                    StartGame();
                }
                else if (Convert.ToInt64(obj[0]) == miniGameData.storyGameAuditId)
                {
                    miniGameBuilder.SetCompereCharacterActive(true);
                    CloseScoreForAudit();
                }
            }
        }
    }
    #endregion

    #region 倒计时UI
    public override void GamePreCountDownEnd()
    {
        base.GamePreCountDownEnd();
        //触发开场事件
        StartStoryForGameOpen();
    }
    #endregion

    #region UI选择回调
    public void UIMiniGameCookingSelect(MenuInfoBean menuInfo)
    {
        //设置操作角色的料理
        miniGameBuilder.GetUserCharacter().characterMiniGameData.cookingMenuInfo = menuInfo;
        //开始准备烹饪的游戏
        StartPreCooking(menuInfo);
    }
    #endregion

    #region UI游戏回调
    public void UIMiniGameCookingSettle(UIMiniGameCooking.MiniGameCookingPhaseTypeEnum type, MiniGameCookingSettleBean settleData)
    {
        switch (type)
        {
            case UIMiniGameCooking.MiniGameCookingPhaseTypeEnum.Pre:
                miniGameBuilder.GetUserCharacter().characterMiniGameData.settleDataForPre = settleData;
                StartMakingCooking();
                break;
            case UIMiniGameCooking.MiniGameCookingPhaseTypeEnum.Making:
                miniGameBuilder.GetUserCharacter().characterMiniGameData.settleDataForMaking = settleData;
                StartEndCooking();
                break;
            case UIMiniGameCooking.MiniGameCookingPhaseTypeEnum.End:
                miniGameBuilder.GetUserCharacter().characterMiniGameData.settleDataForEnd = settleData;
                StartAudit();
                break;
        }
    }
    #endregion
}