using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Linq;

public class MiniGameCookingHandler : BaseMiniGameHandler<MiniGameCookingBuilder, MiniGameCookingBean>,
    UIMiniGameCookingSelect.ICallBack,
    UIMiniGameCooking.ICallBack,
    UIMiniGameCookingSettlement.ICallBack
{
    //事件处理
    protected UIMiniGameCooking uiMiniGameCooking;
    protected UIMiniGameCookingSelect uiMiniGameCookingSelect;
    protected UIMiniGameCookingSettlement uiMiniGameCookingSettlement;
    protected override void Awake()
    {
        builderName = "MiniGameCookingBuilder";
        base.Awake();
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
        //设置主题
        if (miniGameData.cookingThemeLevel != 0)
        {
            miniGameData.SetCookingThemeByLevel(miniGameData.cookingThemeLevel);
        }
        if (miniGameData.cookingThemeId != 0)
        {
            miniGameData.SetCookingThemeById(miniGameData.cookingThemeId);
        }
        //初始化摄像头位置
        InitCameraPosition();
        //设置通告板内容
        List<MiniGameCookingCallBoardCpt> listCallBoard = miniGameBuilder.GetListCallBoard();
        foreach (MiniGameCookingCallBoardCpt itemCpt in listCallBoard)
            itemCpt.SetCallBoardContent(miniGameData.GetCookingTheme().name);
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
            if (itemNpc.characterMiniGameData.GetCookingMenuInfo() == null && itemNpc.characterMiniGameData.characterType == 0)
            {
                MenuInfoBean menuInfo = InnFoodHandler.Instance.manager.GetRandomFoodDataByCookingTheme(miniGameData.GetCookingTheme());
                itemNpc.characterMiniGameData.SetCookingMenuInfo(menuInfo);
            }
            //给参赛选手分配灶台
            MiniGameCookingStoveCpt itemTable = listStove[i];
            itemNpc.SetStove(itemTable);
        }
        //打开倒计时UI
        OpenCountDownUI(miniGameData, false);
    }

    /// <summary>
    /// 设置摄像机位置
    /// </summary>
    public void InitCameraPosition()
    {
        BaseControl baseControl = GameControlHandler.Instance.StartControl<ControlForMiniGameCookingCpt>(GameControlHandler.ControlEnum.MiniGameCooking);
        baseControl.SetCameraFollowObj(baseControl.gameObject);
        SetCameraPosition(miniGameData.userStartPosition);
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
        BaseControl baseControl = GameControlHandler.Instance.StartControl<ControlForMiniGameCookingCpt>(GameControlHandler.ControlEnum.MiniGameCooking);
        baseControl.SetCameraFollowObj(miniGameBuilder.GetUserCharacter().gameObject);
    }

    /// <summary>
    /// 开始选择制作的食物
    /// </summary>
    public void StartSelectMenu()
    {
        //打开游戏UI
        uiMiniGameCookingSelect = UIHandler.Instance.OpenUIAndCloseOther<UIMiniGameCookingSelect>();
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
        miniGameBuilder.GetUserCharacter().characterData.GetAttributes( out CharacterAttributesBean attributes);
        gameTiming += (attributes.cook * 0.3f);
        //打开UI
        uiMiniGameCooking = UIHandler.Instance.OpenUIAndCloseOther<UIMiniGameCooking>();
        uiMiniGameCooking.SetData(miniGameData, gameTiming);
        uiMiniGameCooking.SetCallBack(this);
        uiMiniGameCooking.StartCookingPre();
        //角色就位
        NpcAIMiniGameCookingCpt npcAI = miniGameBuilder.GetUserCharacter();
        npcAI.characterMiniGameData.SetCookingMenuInfo(menuInfo);
        npcAI.SetIntent(NpcAIMiniGameCookingCpt.MiniGameCookingIntentEnum.CookingPre);
    }

    /// <summary>
    /// 开始制作料理阶段游戏
    /// </summary>
    public void StartMakingCooking()
    {
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
        UIHandler.Instance.CloseAllUI();
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
        GameEventHandler.Instance.RegisterNotifyForEvent(NotifyForEvent);
        GameEventHandler.Instance.EventTriggerForStoryCooking(miniGameData, miniGameData.storyGameStartId);
    }

    /// <summary>
    ///  开始故事 审核故事 
    /// </summary>
    public void StartStoryForGameAudit()
    {
        //因为剧情需要，先隐藏主持人
        miniGameBuilder.SetCompereCharacterActive(false);
        GameEventHandler.Instance.RegisterNotifyForEvent(NotifyForEvent);
        GameEventHandler.Instance.EventTriggerForStoryCooking(miniGameData, miniGameData.storyGameAuditId);
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
        AudioHandler.Instance.PlaySound(AudioSoundEnum.Correct);
        List<MiniGameCookingAuditTableCpt> listTable = miniGameBuilder.GetListAuditTable();
        foreach (MiniGameCookingAuditTableCpt itemTable in listTable)
        {
            FoodForCoverCpt foodCoverCpt = itemTable.GetFood();
            if (foodCoverCpt != null)
                foodCoverCpt.ShowFood();
        }
    }

    /// <summary>
    /// 隐藏所有参赛者手上的食物
    /// </summary>
    public void HideAllPlayFood()
    {
        List<NpcAIMiniGameCookingCpt> listAllPlayer = miniGameBuilder.GetCharacterByType(NpcAIMiniGameCookingCpt.MiniGameCookingNpcTypeEnum.Player);
        foreach (NpcAIMiniGameCookingCpt itemPlayer in listAllPlayer)
        {
            //隐藏自己手上的食物
            itemPlayer.foodForCover.gameObject.SetActive(false);
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

    public void NotifyForEvent(GameEventHandler.NotifyEventTypeEnum notifyEventType,params object[] obj)
    {
        if (notifyEventType == GameEventHandler.NotifyEventTypeEnum.EventEnd)
        {
            if (Convert.ToInt64(obj[0]) == miniGameData.storyGameStartId)
            {
                StartGame();
            }
            else if (Convert.ToInt64(obj[0]) == miniGameData.storyGameAuditId)
            {
                //显示主持人
                miniGameBuilder.SetCompereCharacterActive(true);
                //关闭评审员的分数
                CloseScoreForAudit();
                //结算分数
                List<NpcAIMiniGameCookingCpt> listPlayer = miniGameBuilder.GetCharacterByType(NpcAIMiniGameCookingCpt.MiniGameCookingNpcTypeEnum.Player);
                foreach (NpcAIMiniGameCookingCpt itemNpc in listPlayer)
                {
                    itemNpc.characterMiniGameData.InitScore();
                }
                //按分数排名
                listPlayer = listPlayer.OrderByDescending(item => item.characterMiniGameData.scoreForTotal).ToList();
                //打开结算UI
                uiMiniGameCookingSettlement = UIHandler.Instance.OpenUIAndCloseOther<UIMiniGameCookingSettlement>();
                uiMiniGameCookingSettlement.SetCallBack(this);
                uiMiniGameCookingSettlement.SetData(listPlayer);
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
        miniGameBuilder.GetUserCharacter().characterMiniGameData.SetCookingMenuInfo(menuInfo);
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

    #region UI结算回调
    public void UIMiniGameCookingSettlementClose()
    {   //打开游戏控制器
        BaseControl baseControl = GameControlHandler.Instance.StartControl<ControlForMiniGameCookingCpt>(GameControlHandler.ControlEnum.MiniGameCooking);
        baseControl.SetCameraFollowObj(miniGameBuilder.GetUserCharacter().gameObject);
        //如果是晋升则按照分数计算是否胜利
        if (miniGameData.gameReason == MiniGameReasonEnum.Improve)
        {
            MiniGameCharacterForCookingBean characterMiniGameData = (MiniGameCharacterForCookingBean)miniGameData.GetUserGameData();
            if (characterMiniGameData.scoreForTotal >= miniGameData.winScore)
            {
                EndGame(MiniGameResultEnum.Win, false);
            }
            else
            {
                EndGame(MiniGameResultEnum.Lose, false);
            }
        }
        //如果是其他 则按名次
        else
        {
            List<MiniGameCharacterBean> listCharacterGameData = miniGameData.GetPlayerGameData();
            //按分数排名
            listCharacterGameData = listCharacterGameData.OrderByDescending(item => ((MiniGameCharacterForCookingBean)item).scoreForTotal).ToList();
            for (int i = 0; i < miniGameData.winRank; i++)
            {
                MiniGameCharacterForCookingBean characterData = (MiniGameCharacterForCookingBean)listCharacterGameData[i];
                if (characterData.characterType == 1)
                {
                    EndGame(MiniGameResultEnum.Win, false);
                    return;
                }
            }
            EndGame(MiniGameResultEnum.Lose, false);
        }
    }
    #endregion
}