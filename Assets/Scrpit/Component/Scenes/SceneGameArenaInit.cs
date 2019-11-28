using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.AI;
using System.Collections;
using System;

public class SceneGameArenaInit : BaseSceneInit, IBaseObserver
{
    //场景数据管理
    public SceneArenaManager sceneArenaManager;
    //食物管理
    public InnFoodManager innFoodManager;

    //弹幕游戏控制
    public MiniGameBarrageHandler barrageHandler;
    //战斗游戏控制
    public MiniGameCombatHandler combatHandler;
    //烹饪游戏控制
    public MiniGameCookingHandler cookingHandler;
    //计算游戏处理
    public MiniGameAccountHandler accountHandler;
    //辩论游戏处理
    public MiniGameDebateHandler debateHandler;

    //地形控制
    public NavMeshSurface navMesh;
    
    private new void Start()
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
            storyInfoManager.storyInfoController.GetStoryInfoByScene(3);

        //测试数据
        ArenaPrepareBean arenaPrepareData = GameCommonInfo.ArenaPrepareData;
        arenaPrepareData = new ArenaPrepareBean();

        arenaPrepareData.gameType = MiniGameEnum.Debate;
        arenaPrepareData.gameDebateData = new MiniGameDebateBean();
        arenaPrepareData.gameDebateData.InitData(gameItemsManager, npcInfoManager.GetCharacterDataById(100001), npcInfoManager.GetCharacterDataById(100002));
        arenaPrepareData.gameDebateData.winLife = 1;

        //arenaPrepareData.gameType = MiniGameEnum.Account;
        //arenaPrepareData.gameAccountData = new MiniGameAccountBean();
        //arenaPrepareData.gameAccountData.InitData(gameItemsManager, npcInfoManager.GetCharacterDataById(100001));
        //arenaPrepareData.gameAccountData.winMoneyS=10;
        //arenaPrepareData.gameAccountData.winMoneyM=1;
        //arenaPrepareData.gameAccountData.winMoneyL=0;


        //arenaPrepareData.gameType = MiniGameEnum.Cooking;
        //arenaPrepareData.gameCookingData = new MiniGameCookingBean();
        //arenaPrepareData.gameCookingData.gameReason = MiniGameReasonEnum.Improve;
        //arenaPrepareData.gameCookingData.winScore = 70;
        //List<CharacterBean> listOurData = new List<CharacterBean>();
        //listOurData.Add(npcInfoManager.GetCharacterDataById(200001));
        //List<CharacterBean> listEnemyData = new List<CharacterBean>();
        //listEnemyData.Add(npcInfoManager.GetCharacterDataById(100001));
        //listEnemyData.Add(npcInfoManager.GetCharacterDataById(100002));
        //listEnemyData.Add(npcInfoManager.GetCharacterDataById(100003));
        //listEnemyData.Add(npcInfoManager.GetCharacterDataById(100002));
        //listEnemyData.Add(npcInfoManager.GetCharacterDataById(100003));
        //listEnemyData.Add(npcInfoManager.GetCharacterDataById(100002));
        //listEnemyData.Add(npcInfoManager.GetCharacterDataById(100003));
        //listEnemyData.Add(npcInfoManager.GetCharacterDataById(100003));
        //listEnemyData.Add(npcInfoManager.GetCharacterDataById(100002));
        //listEnemyData.Add(npcInfoManager.GetCharacterDataById(100003));
        //List<CharacterBean> listAuditerData = new List<CharacterBean>();
        //listAuditerData.Add(npcInfoManager.GetCharacterDataById(100001));
        //listAuditerData.Add(npcInfoManager.GetCharacterDataById(100002));
        //listAuditerData.Add(npcInfoManager.GetCharacterDataById(100003));
        //listAuditerData.Add(npcInfoManager.GetCharacterDataById(100003));
        //listAuditerData.Add(npcInfoManager.GetCharacterDataById(100003));
        //List<CharacterBean> listCompereData = new List<CharacterBean>();
        //listCompereData.Add(npcInfoManager.GetCharacterDataById(110005));
        //listCompereData.Add(npcInfoManager.GetCharacterDataById(110006));
        //arenaPrepareData.gameCookingData.InitData(gameItemsManager, listOurData, listEnemyData, listAuditerData, listCompereData);

        //arenaPrepareData.gameType = MiniGameEnum.Barrage;
        //arenaPrepareData.gameBarrageData = new MiniGameBarrageBean();
        //arenaPrepareData.gameBarrageData.gameLevel = 1;
        //arenaPrepareData.gameBarrageData.launchInterval = 1;
        //arenaPrepareData.gameBarrageData.launchTypes = new BarrageEjectorCpt.LaunchTypeEnum[] {
        //    BarrageEjectorCpt.LaunchTypeEnum.Single,
        //    BarrageEjectorCpt.LaunchTypeEnum.Double,
        //    BarrageEjectorCpt.LaunchTypeEnum.Triple
        //};
        //arenaPrepareData.gameBarrageData.launchSpeed = 1;
        //arenaPrepareData.gameBarrageData.winSurvivalTime = 60;
        //arenaPrepareData.gameBarrageData.winLife = 1;
        //arenaPrepareData.gameBarrageData.InitData(gameItemsManager, gameDataManager.gameData.userCharacter);
        //arenaPrepareData.gameBarrageData.AddRewardItem(100001, 1);
        //arenaPrepareData.gameBarrageData.AddRewardItem(100001, 2);
        //arenaPrepareData.gameBarrageData.AddRewardItem(200001, 3);
        //arenaPrepareData.gameBarrageData.AddRewardItem(1100006, 3);


        //arenaPrepareData.gameType = MiniGameEnum.Combat;
        //arenaPrepareData.gameCombatData = new MiniGameCombatBean();
        //arenaPrepareData.gameCombatData.winBringDownNumber = 3;
        //arenaPrepareData.gameCombatData.winSurvivalNumber = 3;
        //List<CharacterBean> listOurData = new List<CharacterBean>();
        //listOurData.Add(npcInfoManager.GetCharacterDataById(200001));
        //listOurData.Add(npcInfoManager.GetCharacterDataById(200101));
        //listOurData.Add(npcInfoManager.GetCharacterDataById(210001));
        //List<CharacterBean> listEnemyData = new List<CharacterBean>();
        //listEnemyData.Add(npcInfoManager.GetCharacterDataById(100001));
        //listEnemyData.Add(npcInfoManager.GetCharacterDataById(100002));
        //listEnemyData.Add(npcInfoManager.GetCharacterDataById(100003));
        //arenaPrepareData.gameCombatData.InitData(gameItemsManager, listOurData, listEnemyData);
        //arenaPrepareData.gameCombatData.AddRewardItem(100001, 1);
        //arenaPrepareData.gameCombatData.AddRewardItem(100001, 2);
        //arenaPrepareData.gameCombatData.AddRewardItem(200001, 3);
        //arenaPrepareData.gameCombatData.AddRewardItem(1100006, 3);

        if (arenaPrepareData == null)
            return;
        switch (arenaPrepareData.gameType)
        {
            case MiniGameEnum.Cooking:
                InitGameCooking(arenaPrepareData.gameCookingData);
                break;
            case MiniGameEnum.Barrage:
                InitGameBarrage(arenaPrepareData.gameBarrageData);
                break;
            case MiniGameEnum.Combat:
                InitGameCombat(arenaPrepareData.gameCombatData);
                break;
            case MiniGameEnum.Account:
                InitGameAccout(arenaPrepareData.gameAccountData);
                break;
            case MiniGameEnum.Debate:
                InitGameDebate(arenaPrepareData.gameDebateData);
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
        //如果没有料理主题 则随机获取一个
        if (gameCookingData.cookingTheme == null)
        {
            gameCookingData.cookingTheme = innFoodManager.GetRandomCookingTheme();
        }
        switch (gameCookingData.gameReason)
        {
            //如果是晋升 
            case MiniGameReasonEnum.Improve:
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
                List<MiniGameCookingCallBoardCpt> listCallBoard= sceneArenaManager.GetArenaForCookingCallBoardBy2();
                cookingHandler.miniGameBuilder.SetListCallBoard(listCallBoard);
                //设置游戏用评审桌子
                List < MiniGameCookingAuditTableCpt > listAuditTable = sceneArenaManager.GetArenaForCookingAuditTableBy2();
                cookingHandler.miniGameBuilder.SetListAuditTable(listAuditTable);
                //设置游戏用灶台
                List<MiniGameCookingStoveCpt> listStove = sceneArenaManager.GetArenaForCookingStoveBy2();
                cookingHandler.miniGameBuilder.SetListStove(listStove);
                //游戏开始动画
                gameCookingData.storyGameOpenId = 100001;
                gameCookingData.storyGameAuditId = 100002;
                //准备游戏
                cookingHandler.InitGame(gameCookingData);
                break;
        }
    }

    /// <summary>
    /// 初始化弹幕游戏
    /// </summary>
    /// <param name="gameBarrageData"></param>
    public void InitGameBarrage(MiniGameBarrageBean gameBarrageData)
    {
        //添加竞技场发射台坐标
        if (gameBarrageData.gameLevel <= 2)
        {
            gameBarrageData.listEjectorPosition = sceneArenaManager.GetArenaForBarrageEjectorBy1(1);
        }
        else if (gameBarrageData.gameLevel > 2 && gameBarrageData.gameLevel <= 4)
        {
            gameBarrageData.listEjectorPosition = sceneArenaManager.GetArenaForBarrageEjectorBy1(2);
        }
        else if (gameBarrageData.gameLevel > 4)
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
        gameCombatData.combatPosition = combatPosition;
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
        gameDebateData.debatePosition = debatePosition;

        //初始化游戏
        debateHandler.InitGame(gameDebateData);
    }

    #region 通知回调
    public void ObserbableUpdate<T>(T observable, int type, params object[] obj) where T : UnityEngine.Object
    {
        switch (type)
        {
            case (int)BaseMiniGameHandler<BaseMiniGameBuilder, MiniGameBaseBean>.MiniGameStatusEnum.Gameing:
                break;
            case (int)BaseMiniGameHandler<BaseMiniGameBuilder, MiniGameBaseBean>.MiniGameStatusEnum.GameEnd:
                break;
            case (int)BaseMiniGameHandler<BaseMiniGameBuilder, MiniGameBaseBean>.MiniGameStatusEnum.GameClose:
                SceneUtil.SceneChange(GameCommonInfo.ScenesChangeData.beforeScene);
                //SceneUtil.SceneChange(ScenesEnum.GameArenaScene);
                break;
        }
    }
    #endregion

    /// <summary>
    /// 生成地形
    /// </summary>
    /// <returns></returns>
    public IEnumerator BuildNavMesh()
    {
        yield return new WaitForEndOfFrame();
        navMesh.BuildNavMesh();
    }
}