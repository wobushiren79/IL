using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MiniGameCookingHandler : BaseMiniGameHandler<MiniGameCookingBuilder, MiniGameCookingBean>, UIMiniGameCookingSelect.ICallBack, IBaseObserver
{
    //事件处理
    public EventHandler eventHandler;
    
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
        //因为剧情需要，先隐藏主持人
        miniGameBuilder.SetCompereCharacterActive(false);
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
        UIMiniGameCooking uiMiniGameCooking = (UIMiniGameCooking)uiGameManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.MiniGameCooking));

    }

    /// <summary>
    /// 开始故事 游戏开场
    /// </summary>
    public void StartStoryForGameOpen()
    {
        if (eventHandler != null)
        {
            eventHandler.AddObserver(this);
            eventHandler.EventTriggerForStoryCooking(miniGameData, miniGameData.storyGameOpenId);
        }
    }

    #region 通知回调
    public void ObserbableUpdate<T>(T observable, int type, params object[] obj) where T : Object
    {
        if (observable == eventHandler)
        {
            if (type == (int)EventHandler.NotifyEventTypeEnum.EventEnd)
            {
                StartGame();
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
}