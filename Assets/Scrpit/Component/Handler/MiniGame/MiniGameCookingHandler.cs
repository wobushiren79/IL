using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MiniGameCookingHandler : BaseMiniGameHandler<MiniGameCookingBuilder, MiniGameCookingBean>, IBaseObserver
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
            itemCpt.SetCallBoardContent("ma");
        //给评审人员分配桌子
        List<MiniGameCookingAuditTableCpt> listAuditTable = miniGameBuilder.GetListAuditTable();
        List<NpcAIMiniGameCookingCpt> listAuditNpcAI = miniGameBuilder.GetCharacterByType(NpcAIMiniGameCookingCpt.MiniGameCookingNpcTypeEnum.Auditer);
        for (int i = 0; i < listAuditNpcAI.Count; i++)
        {
            NpcAIMiniGameCookingCpt itemNpc = listAuditNpcAI[i];
            MiniGameCookingAuditTableCpt itemTable = listAuditTable[i];
            itemNpc.SetAuditTable(itemTable);
        }
        //给参赛选手分配灶台
        List<MiniGameCookingStoveCpt> listStove = miniGameBuilder.GetListStove();
        List<NpcAIMiniGameCookingCpt> listPlayerNpcAI = miniGameBuilder.GetCharacterByType(NpcAIMiniGameCookingCpt.MiniGameCookingNpcTypeEnum.Player);
        for (int i = 0; i < listPlayerNpcAI.Count; i++)
        {
            NpcAIMiniGameCookingCpt itemNpc = listPlayerNpcAI[i];
            MiniGameCookingStoveCpt itemTable = listStove[i];
            itemNpc.SetStove(itemTable);
        }
        //因为剧情需要，先隐藏主持人
        miniGameBuilder.SetCompereCharacterActive(false);
        //打开倒计时UI
        OpenCountDownUI(miniGameData,false);
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
        //打开游戏UI

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
        if (eventHandler != null)
        {
            eventHandler.AddObserver(this);
            eventHandler.EventTriggerForStoryCooking(miniGameData, miniGameData.storyGameOpenId);
        }
    }
    #endregion
}