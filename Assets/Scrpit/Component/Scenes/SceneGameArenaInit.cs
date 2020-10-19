using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.AI;
using System.Collections;
using System;

public class SceneGameArenaInit : BaseSceneInit, IBaseObserver
{
    //场景数据管理
    protected SceneArenaManager sceneArenaManager;
    //食物管理
    protected InnFoodManager innFoodManager;

    //弹幕游戏控制
    protected MiniGameBarrageHandler barrageHandler;
    //战斗游戏控制
    protected MiniGameCombatHandler combatHandler;
    //烹饪游戏控制
    protected MiniGameCookingHandler cookingHandler;
    //计算游戏处理
    protected MiniGameAccountHandler accountHandler;
    //辩论游戏处理
    protected MiniGameDebateHandler debateHandler;
   

    public override void Awake()
    {
        base.Awake();
        sceneArenaManager = Find<SceneArenaManager>(ImportantTypeEnum.SceneManager);
        innFoodManager = Find<InnFoodManager>(ImportantTypeEnum.FoodManager);
        barrageHandler = Find<MiniGameBarrageHandler>(ImportantTypeEnum.MiniGameHandler);
        combatHandler = Find<MiniGameCombatHandler>(ImportantTypeEnum.MiniGameHandler);
        cookingHandler = Find<MiniGameCookingHandler>(ImportantTypeEnum.MiniGameHandler);
        accountHandler = Find<MiniGameAccountHandler>(ImportantTypeEnum.MiniGameHandler);
        debateHandler = Find<MiniGameDebateHandler>(ImportantTypeEnum.MiniGameHandler);
    }

    public override void Start()
    {
        base.Start();
        InitSceneData();
        barrageHandler.AddObserver(this);
        combatHandler.AddObserver(this);
        cookingHandler.AddObserver(this);
        accountHandler.AddObserver(this);
        debateHandler.AddObserver(this);
    }

    public void InitSceneData()
    {
        //获取相关数据
        if (gameItemsManager != null)
            gameItemsManager.itemsInfoController.GetAllItemsInfo();
        //获取所有NPC
        if (npcInfoManager != null)
            npcInfoManager.npcInfoController.GetAllNpcInfo();
        if (storyInfoManager != null)
            storyInfoManager.storyInfoController.GetStoryInfoByScene(ScenesEnum.GameArenaScene);

        ArenaPrepareBean arenaPrepareData = GameCommonInfo.ArenaPrepareData;
        //测试数据 
        //arenaPrepareData = new ArenaPrepareBean(new MiniGameDebateBean());
        //arenaPrepareData.miniGameData.gameType = MiniGameEnum.Debate;
        //arenaPrepareData.miniGameData.InitData(gameItemsManager, npcInfoManager.GetCharacterDataById(100011), npcInfoManager.GetCharacterDataById(100021));
        //arenaPrepareData.miniGameData.winLife = 1;

        //arenaPrepareData.gameType = MiniGameEnum.Account;
        //arenaPrepareData.gameAccountData = new MiniGameAccountBean();
        //arenaPrepareData.gameAccountData.InitData(gameItemsManager, npcInfoManager.GetCharacterDataById(100001));
        //arenaPrepareData.gameAccountData.winMoneyS=10;
        //arenaPrepareData.gameAccountData.winMoneyM=1;
        //arenaPrepareData.gameAccountData.winMoneyL=0;

        //arenaPrepareData = new ArenaPrepareBean(new MiniGameCookingBean());
        //arenaPrepareData.miniGameData.gameType = MiniGameEnum.Cooking;
        //arenaPrepareData.miniGameData.gameReason = MiniGameReasonEnum.Improve;
        //arenaPrepareData.miniGameData.winScore = 60;
        //CharacterBean ourData = npcInfoManager.GetCharacterDataById(100011);
        //List<CharacterBean> listEnemyData = new List<CharacterBean>();
        //listEnemyData.Add(npcInfoManager.GetCharacterDataById(100021));
        //listEnemyData.Add(npcInfoManager.GetCharacterDataById(100031));
        //listEnemyData.Add(npcInfoManager.GetCharacterDataById(100041));
        //listEnemyData.Add(npcInfoManager.GetCharacterDataById(100051));
        //listEnemyData.Add(npcInfoManager.GetCharacterDataById(100061));
        //listEnemyData.Add(npcInfoManager.GetCharacterDataById(100071));
        //listEnemyData.Add(npcInfoManager.GetCharacterDataById(100081));
        //listEnemyData.Add(npcInfoManager.GetCharacterDataById(100091));
        //List<CharacterBean> listAuditerData = new List<CharacterBean>();
        //listAuditerData.Add(npcInfoManager.GetCharacterDataById(100021));
        //listAuditerData.Add(npcInfoManager.GetCharacterDataById(100031));
        //listAuditerData.Add(npcInfoManager.GetCharacterDataById(100041));
        //listAuditerData.Add(npcInfoManager.GetCharacterDataById(100051));
        //listAuditerData.Add(npcInfoManager.GetCharacterDataById(100061));
        //List<CharacterBean> listCompereData = new List<CharacterBean>();
        //listCompereData.Add(npcInfoManager.GetCharacterDataById(110051));
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
        //miniGameBarrage.InitData(gameItemsManager, npcInfoManager.GetCharacterDataById(100061));
        //arenaPrepareData = new ArenaPrepareBean(miniGameBarrage);

        //arenaPrepareData = new ArenaPrepareBean(new MiniGameCombatBean());
        //arenaPrepareData.miniGameData.winBringDownNumber = 3;
        //arenaPrepareData.miniGameData.winSurvivalNumber = 1;
        //List<CharacterBean> listOurData = new List<CharacterBean>();
        //listOurData.Add(npcInfoManager.GetCharacterDataById(100011));
        //listOurData.Add(npcInfoManager.GetCharacterDataById(100021));
        //listOurData.Add(npcInfoManager.GetCharacterDataById(100031));
        //List<CharacterBean> listEnemyData = new List<CharacterBean>();
        //listEnemyData.Add(npcInfoManager.GetCharacterDataById(100041));
        //listEnemyData.Add(npcInfoManager.GetCharacterDataById(100051));
        //listEnemyData.Add(npcInfoManager.GetCharacterDataById(100061));
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
        cookingHandler.miniGameBuilder.SetListCallBoard(listCallBoard);
        //设置游戏用评审桌子
        List<MiniGameCookingAuditTableCpt> listAuditTable = sceneArenaManager.GetArenaForCookingAuditTableBy2();
        cookingHandler.miniGameBuilder.SetListAuditTable(listAuditTable);
        //设置游戏用灶台
        List<MiniGameCookingStoveCpt> listStove = sceneArenaManager.GetArenaForCookingStoveBy2();
        cookingHandler.miniGameBuilder.SetListStove(listStove);
        cookingHandler.miniGameBuilder.SetListStove(listStove);
        //准备游戏
        cookingHandler.InitGame(gameCookingData);
    }

    /// <summary>
    /// 初始化弹幕游戏
    /// </summary>
    /// <param name="gameBarrageData"></param>
    public void InitGameBarrage(MiniGameBarrageBean gameBarrageData)
    {
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
        barrageHandler.InitGame(gameBarrageData);
    }

    /// <summary>
    /// 初始化战斗游戏
    /// </summary>
    /// <param name="gameCombatData"></param>
    public void InitGameCombat(MiniGameCombatBean gameCombatData)
    {
        //找到竞技场战斗的地点
        sceneArenaManager.GetArenaForCombatBy1(out Vector3 combatPosition);
        gameCombatData.miniGamePosition = combatPosition;
        //初始化游戏
        combatHandler.InitGame(gameCombatData);
    }

    /// <summary>
    /// 初始化算账游戏
    /// </summary>
    /// <param name="gameAccountData"></param>
    public void InitGameAccout(MiniGameAccountBean gameAccountData)
    {
        sceneArenaManager.GetArenaForAccountPlayerBy3(out Vector3 playerPosition);
        sceneArenaManager.GetArenaForAccountCameraBy3(out Vector3 cameraPosition);
        sceneArenaManager.GetArenaForAccountMoneyBy3(out Transform tfMoneyPosition);
        gameAccountData.playerPosition = playerPosition;
        gameAccountData.cameraPosition = cameraPosition;
        gameAccountData.tfMoneyPosition = tfMoneyPosition;
        //初始化游戏
        accountHandler.InitGame(gameAccountData);
    }

    /// <summary>
    /// 初始化辩论游戏
    /// </summary>
    /// <param name="gameDebateData"></param>
    public void InitGameDebate(MiniGameDebateBean gameDebateData)
    {
        //找到竞技场战斗的地点
        sceneArenaManager.GetArenaForCombatBy1(out Vector3 debatePosition);
        gameDebateData.miniGamePosition = debatePosition;

        //初始化游戏
        debateHandler.InitGame(gameDebateData);
    }

    #region 通知回调
    public void ObserbableUpdate<T>(T observable, int type, params object[] obj) where T : UnityEngine.Object
    {
        switch ((MiniGameStatusEnum)type)
        {
            case MiniGameStatusEnum.Gameing:
                break;
            case MiniGameStatusEnum.GameEnd:
                break;
            case MiniGameStatusEnum.GameClose:
                SceneUtil.SceneChange(GameCommonInfo.ScenesChangeData.beforeScene);
                //SceneUtil.SceneChange(ScenesEnum.GameArenaScene);
                break;
        }
    }
    #endregion

}