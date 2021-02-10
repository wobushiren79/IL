using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.AI;
using System.Collections;
using System;

public class SceneGameArenaInit : BaseSceneInit
{
    public override void Start()
    {
        base.Start();
        InitSceneData();
        MiniGameHandler.Instance.handlerForBarrage.RegisterNotifyForMiniGameStatus(NotifyForMiniGameStatus);
        MiniGameHandler.Instance.handlerForCombat.RegisterNotifyForMiniGameStatus(NotifyForMiniGameStatus);
        MiniGameHandler.Instance.handlerForCooking.RegisterNotifyForMiniGameStatus(NotifyForMiniGameStatus);
        MiniGameHandler.Instance.handlerForAccount.RegisterNotifyForMiniGameStatus(NotifyForMiniGameStatus);
        MiniGameHandler.Instance.handlerForDebate.RegisterNotifyForMiniGameStatus(NotifyForMiniGameStatus);
    }

    private void OnDestroy()
    {
        MiniGameHandler.Instance.handlerForBarrage.UnRegisterNotifyForMiniGameStatus(NotifyForMiniGameStatus);
        MiniGameHandler.Instance.handlerForCombat.UnRegisterNotifyForMiniGameStatus(NotifyForMiniGameStatus);
        MiniGameHandler.Instance.handlerForCooking.UnRegisterNotifyForMiniGameStatus(NotifyForMiniGameStatus);
        MiniGameHandler.Instance.handlerForAccount.UnRegisterNotifyForMiniGameStatus(NotifyForMiniGameStatus);
        MiniGameHandler.Instance.handlerForDebate.UnRegisterNotifyForMiniGameStatus(NotifyForMiniGameStatus);
    }

    public void InitSceneData()
    {
        ArenaPrepareBean arenaPrepareData = GameCommonInfo.ArenaPrepareData;
        //测试数据 
        //arenaPrepareData = new ArenaPrepareBean(new MiniGameDebateBean());
        //arenaPrepareData.miniGameData.gameType = MiniGameEnum.Debate;
        //arenaPrepareData.miniGameData.InitData(gameItemsManager, NpcInfoHandler.Instance.manager.GetCharacterDataById(100011), NpcInfoHandler.Instance.manager.GetCharacterDataById(100021));
        //arenaPrepareData.miniGameData.winLife = 1;

        //arenaPrepareData.gameType = MiniGameEnum.Account;
        //arenaPrepareData.gameAccountData = new MiniGameAccountBean();
        //arenaPrepareData.gameAccountData.InitData(gameItemsManager, NpcInfoHandler.Instance.manager.GetCharacterDataById(100001));
        //arenaPrepareData.gameAccountData.winMoneyS=10;
        //arenaPrepareData.gameAccountData.winMoneyM=1;
        //arenaPrepareData.gameAccountData.winMoneyL=0;

        //arenaPrepareData = new ArenaPrepareBean(new MiniGameCookingBean());
        //arenaPrepareData.miniGameData.gameType = MiniGameEnum.Cooking;
        //arenaPrepareData.miniGameData.gameReason = MiniGameReasonEnum.Improve;
        //arenaPrepareData.miniGameData.winScore = 60;
        //CharacterBean ourData = NpcInfoHandler.Instance.manager.GetCharacterDataById(100011);
        //List<CharacterBean> listEnemyData = new List<CharacterBean>();
        //listEnemyData.Add(NpcInfoHandler.Instance.manager.GetCharacterDataById(100021));
        //listEnemyData.Add(NpcInfoHandler.Instance.manager.GetCharacterDataById(100031));
        //listEnemyData.Add(NpcInfoHandler.Instance.manager.GetCharacterDataById(100041));
        //listEnemyData.Add(NpcInfoHandler.Instance.manager.GetCharacterDataById(100051));
        //listEnemyData.Add(NpcInfoHandler.Instance.manager.GetCharacterDataById(100061));
        //listEnemyData.Add(NpcInfoHandler.Instance.manager.GetCharacterDataById(100071));
        //listEnemyData.Add(NpcInfoHandler.Instance.manager.GetCharacterDataById(100081));
        //listEnemyData.Add(NpcInfoHandler.Instance.manager.GetCharacterDataById(100091));
        //List<CharacterBean> listAuditerData = new List<CharacterBean>();
        //listAuditerData.Add(NpcInfoHandler.Instance.manager.GetCharacterDataById(100021));
        //listAuditerData.Add(NpcInfoHandler.Instance.manager.GetCharacterDataById(100031));
        //listAuditerData.Add(NpcInfoHandler.Instance.manager.GetCharacterDataById(100041));
        //listAuditerData.Add(NpcInfoHandler.Instance.manager.GetCharacterDataById(100051));
        //listAuditerData.Add(NpcInfoHandler.Instance.manager.GetCharacterDataById(100061));
        //List<CharacterBean> listCompereData = new List<CharacterBean>();
        //listCompereData.Add(NpcInfoHandler.Instance.manager.GetCharacterDataById(110051));
        //((MiniGameCookingBean)(arenaPrepareData.miniGameData)).storyGameStartId = 30000001;
        //((MiniGameCookingBean)(arenaPrepareData.miniGameData)).storyGameAuditId = 30000002;
        //((MiniGameCookingBean)(arenaPrepareData.miniGameData)).cookingThemeLevel = 1;
        //((MiniGameCookingBean)(arenaPrepareData.miniGameData)).InitData(gameItemsManager, ourData, listEnemyData, listAuditerData, listCompereData);

        //MiniGameBarrageBean miniGameBarrage = new MiniGameBarrageBean();
        //miniGameBarrage.launchInterval = 1;
        //miniGameBarrage.launchTypes = new MiniGameBarrageEjectorCpt.LaunchTypeEnum[] {
        //    MiniGameBarrageEjectorCpt.LaunchTypeEnum.Single,
        //    MiniGameBarrageEjectorCpt.LaunchTypeEnum.Double,
        //    MiniGameBarrageEjectorCpt.LaunchTypeEnum.Triple
        //};
        //miniGameBarrage.launchSpeed = 1;
        //miniGameBarrage.winSurvivalTime = 60;
        //miniGameBarrage.winLife = 1;
        //miniGameBarrage.InitData(gameItemsManager, NpcInfoHandler.Instance.manager.GetCharacterDataById(100061));
        //arenaPrepareData = new ArenaPrepareBean(miniGameBarrage);

        //arenaPrepareData = new ArenaPrepareBean(new MiniGameCombatBean());
        //arenaPrepareData.miniGameData.winBringDownNumber = 3;
        //arenaPrepareData.miniGameData.winSurvivalNumber = 1;
        //List<CharacterBean> listOurData = new List<CharacterBean>();
        //listOurData.Add(NpcInfoHandler.Instance.manager.GetCharacterDataById(100011));
        //listOurData.Add(NpcInfoHandler.Instance.manager.GetCharacterDataById(100021));
        //listOurData.Add(NpcInfoHandler.Instance.manager.GetCharacterDataById(100031));
        //List<CharacterBean> listEnemyData = new List<CharacterBean>();
        //listEnemyData.Add(NpcInfoHandler.Instance.manager.GetCharacterDataById(100041));
        //listEnemyData.Add(NpcInfoHandler.Instance.manager.GetCharacterDataById(100051));
        //listEnemyData.Add(NpcInfoHandler.Instance.manager.GetCharacterDataById(100061));
        //arenaPrepareData.miniGameData.InitData(gameItemsManager, listOurData, listEnemyData);

        if (arenaPrepareData == null || arenaPrepareData.miniGameData == null)
            return;
        switch (arenaPrepareData.miniGameData.gameType)
        {
            case MiniGameEnum.Cooking:
                InitGameCooking((MiniGameCookingBean)arenaPrepareData.miniGameData);
                break;
            case MiniGameEnum.Barrage:
                InitGameBarrage((MiniGameBarrageBean)arenaPrepareData.miniGameData);
                break;
            case MiniGameEnum.Combat:
                InitGameCombat((MiniGameCombatBean)arenaPrepareData.miniGameData);
                break;
            case MiniGameEnum.Account:
                InitGameAccout((MiniGameAccountBean)arenaPrepareData.miniGameData);
                break;
            case MiniGameEnum.Debate:
                InitGameDebate((MiniGameDebateBean)arenaPrepareData.miniGameData);
                break;
        }
        //初始化地形
        StartCoroutine(BuildNavMesh());
    }

    /// <summary>
    /// 初始化烹饪游戏
    /// </summary>
    /// <param name="gameCookingData"></param>
    private void InitGameCooking(MiniGameCookingBean gameCookingData)
    {
        switch (gameCookingData.gameReason)
        {
            //如果是晋升 
            case MiniGameReasonEnum.Improve:
                break;
            default:
                break;
        }
        SceneArenaManager sceneArenaManager = GameScenesHandler.Instance.manager.GetSceneManager<SceneArenaManager>();
        //设置每个玩家的初始位置
        sceneArenaManager.GetArenaForCookingPlayerPositionBy2(out Vector3 playerStartPosition);
        //总共玩家数量为单数 则初始点为playerStartPosition，如果是双数则从playerStartPosition.x -0.5f开始
        float positionX = playerStartPosition.x + gameCookingData.listEnemyGameData.Count / 2f;
        float positionY = playerStartPosition.y;
        //随机站位
        int userRandomPositionNumber = UnityEngine.Random.Range(0, gameCookingData.listEnemyGameData.Count + 1);
        for (int i = 0; i < gameCookingData.listEnemyGameData.Count + 1; i++)
        {
            if (i == userRandomPositionNumber)
            {
                gameCookingData.userStartPosition = new Vector3(positionX, positionY);
            }
            else
            {
                gameCookingData.listEnemyStartPosition.Add(new Vector3(positionX, positionY));
            }
            positionX -= 1;
        }
        //设置评审员位置
        sceneArenaManager.GetArenaForCookingAuditorPositionBy2(out Vector3 auditorStartPosition);
        positionX = auditorStartPosition.x + ((gameCookingData.listAuditerGameData.Count - 1) / 2f);
        positionY = auditorStartPosition.y;
        for (int i = 0; i < gameCookingData.listAuditerGameData.Count; i++)
        {
            gameCookingData.listAuditerStartPosition.Add(new Vector3(positionX, positionY));
            positionX -= 1;
        }
        //设置主持人位置
        List<Vector3> listComperePosition = sceneArenaManager.GetArenaForCookingComperePositionBy2(gameCookingData.listCompereGameData.Count);
        gameCookingData.listCompereStartPosition = listComperePosition;
        //设置游戏用通告版
        List<MiniGameCookingCallBoardCpt> listCallBoard = sceneArenaManager.GetArenaForCookingCallBoardBy2();
        MiniGameHandler.Instance.handlerForCooking.miniGameBuilder.SetListCallBoard(listCallBoard);
        //设置游戏用评审桌子
        List<MiniGameCookingAuditTableCpt> listAuditTable = sceneArenaManager.GetArenaForCookingAuditTableBy2();
        MiniGameHandler.Instance.handlerForCooking.miniGameBuilder.SetListAuditTable(listAuditTable);
        //设置游戏用灶台
        List<MiniGameCookingStoveCpt> listStove = sceneArenaManager.GetArenaForCookingStoveBy2();
        MiniGameHandler.Instance.handlerForCooking.miniGameBuilder.SetListStove(listStove);
        MiniGameHandler.Instance.handlerForCooking.miniGameBuilder.SetListStove(listStove);
        //准备游戏
        MiniGameHandler.Instance.handlerForCooking.InitGame(gameCookingData);
    }

    /// <summary>
    /// 初始化弹幕游戏
    /// </summary>
    /// <param name="gameBarrageData"></param>
    public void InitGameBarrage(MiniGameBarrageBean gameBarrageData)
    {
        SceneArenaManager sceneArenaManager = GameScenesHandler.Instance.manager.GetSceneManager<SceneArenaManager>();
        //添加竞技场发射台坐标
        if (gameBarrageData.launchNumber <= 1)
        {
            gameBarrageData.listEjectorPosition = sceneArenaManager.GetArenaForBarrageEjectorBy1(1);
        }
        else if (gameBarrageData.launchNumber == 2)
        {
            gameBarrageData.listEjectorPosition = sceneArenaManager.GetArenaForBarrageEjectorBy1(2);
        }
        else if (gameBarrageData.launchNumber == 3)
        {
            gameBarrageData.listEjectorPosition = sceneArenaManager.GetArenaForBarrageEjectorBy1(3);
        }
        //添加用户起始位置
        sceneArenaManager.GetArenaForBarrageUserPositionBy1(out Vector3 userPosition);
        gameBarrageData.userStartPosition = userPosition;
        //弹幕处理初始化
        MiniGameHandler.Instance.handlerForBarrage.InitGame(gameBarrageData);
    }

    /// <summary>
    /// 初始化战斗游戏
    /// </summary>
    /// <param name="gameCombatData"></param>
    public void InitGameCombat(MiniGameCombatBean gameCombatData)
    {
        SceneArenaManager sceneArenaManager = GameScenesHandler.Instance.manager.GetSceneManager<SceneArenaManager>();
        //找到竞技场战斗的地点
        sceneArenaManager.GetArenaForCombatBy1(out Vector3 combatPosition);
        gameCombatData.miniGamePosition = combatPosition;
        //初始化游戏
        MiniGameHandler.Instance.handlerForCombat.InitGame(gameCombatData);
    }

    /// <summary>
    /// 初始化算账游戏
    /// </summary>
    /// <param name="gameAccountData"></param>
    public void InitGameAccout(MiniGameAccountBean gameAccountData)
    {
        SceneArenaManager sceneArenaManager = GameScenesHandler.Instance.manager.GetSceneManager<SceneArenaManager>();
        sceneArenaManager.GetArenaForAccountPlayerBy3(out Vector3 playerPosition);
        sceneArenaManager.GetArenaForAccountCameraBy3(out Vector3 cameraPosition);
        sceneArenaManager.GetArenaForAccountMoneyBy3(out Transform tfMoneyPosition);
        gameAccountData.playerPosition = playerPosition;
        gameAccountData.cameraPosition = cameraPosition;
        gameAccountData.tfMoneyPosition = tfMoneyPosition;
        //初始化游戏
        MiniGameHandler.Instance.handlerForAccount.InitGame(gameAccountData);
    }

    /// <summary>
    /// 初始化辩论游戏
    /// </summary>
    /// <param name="gameDebateData"></param>
    public void InitGameDebate(MiniGameDebateBean gameDebateData)
    {
        SceneArenaManager sceneArenaManager = GameScenesHandler.Instance.manager.GetSceneManager<SceneArenaManager>();
        //找到竞技场战斗的地点
        sceneArenaManager.GetArenaForCombatBy1(out Vector3 debatePosition);
        gameDebateData.miniGamePosition = debatePosition;

        //初始化游戏
        MiniGameHandler.Instance.handlerForDebate.InitGame(gameDebateData);
    }

    #region 通知回调
    public void NotifyForMiniGameStatus(MiniGameStatusEnum type, params object[] obj)
    {
        switch (type)
        {
            case MiniGameStatusEnum.Gameing:
                break;
            case MiniGameStatusEnum.GameEnd:
                break;
            case MiniGameStatusEnum.GameClose:
                GameScenesHandler.Instance.ChangeScene(GameCommonInfo.ScenesChangeData.beforeScene);
                //GameScenesHandler.Instance.ChangeScene(ScenesEnum.GameArenaScene);
                break;
        }
    }
    #endregion

}