using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.AI;
using System.Collections;

public class SceneGameArenaInit : BaseSceneInit, IBaseObserver
{
    //弹幕游戏控制
    public MiniGameBarrageHandler barrageHandler;
    //战斗游戏控制
    public MiniGameCombatHandler combatHandler;
    //地形控制
    public NavMeshSurface navMesh;

    private new void Start()
    {
        base.Start();
        InitSceneData();
        barrageHandler.AddObserver(this);
        combatHandler.AddObserver(this);
    }

    public void InitSceneData()
    {
        //获取相关数据
        if (gameItemsManager != null)
            gameItemsManager.itemsInfoController.GetAllItemsInfo();
        //获取所有NPC
        if (npcInfoManager != null)
            npcInfoManager.npcInfoController.GetAllNpcInfo();
        //测试数据
        ArenaPrepareBean arenaPrepareData = GameCommonInfo.ArenaPrepareData;
        arenaPrepareData = new ArenaPrepareBean();

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


        arenaPrepareData.gameType = MiniGameEnum.Combat;
        arenaPrepareData.gameCombatData = new MiniGameCombatBean();
        arenaPrepareData.gameCombatData.combatPosition = Vector3.zero;
        arenaPrepareData.gameCombatData.winBringDownNumber = 3;
        arenaPrepareData.gameCombatData.winSurvivalNumber = 3;
        List<CharacterBean> listOurData = new List<CharacterBean>();

        listOurData.Add(npcInfoManager.GetCharacterDataById(200001));
        listOurData.Add(npcInfoManager.GetCharacterDataById(200101));
        listOurData.Add(npcInfoManager.GetCharacterDataById(210001));
        List<CharacterBean> listEnemyData = new List<CharacterBean>();
        listEnemyData.Add(npcInfoManager.GetCharacterDataById(100001));
        listEnemyData.Add(npcInfoManager.GetCharacterDataById(100002));
        listEnemyData.Add(npcInfoManager.GetCharacterDataById(100003));
        arenaPrepareData.gameCombatData.InitData(gameItemsManager, listOurData, listEnemyData);
        arenaPrepareData.gameCombatData.AddRewardItem(100001, 1);
        arenaPrepareData.gameCombatData.AddRewardItem(100001, 2);
        arenaPrepareData.gameCombatData.AddRewardItem(200001, 3);
        arenaPrepareData.gameCombatData.AddRewardItem(1100006, 3);

        if (arenaPrepareData == null)
            return;
        switch (arenaPrepareData.gameType)
        {
            case MiniGameEnum.Cooking:
                break;
            case MiniGameEnum.Barrage:
                InitGameBarrage(arenaPrepareData.gameBarrageData);
                break;
            case MiniGameEnum.Combat:
                InitGameCombat(arenaPrepareData.gameCombatData);
                break;
        }

        //初始化地形
        StartCoroutine(BuildNavMesh());
    }

    /// <summary>
    /// 初始化弹幕游戏
    /// </summary>
    /// <param name="gameBarrageData"></param>
    public void InitGameBarrage(MiniGameBarrageBean gameBarrageData)
    {
        //添加竞技场发射台坐标
        gameBarrageData.listEjectorPosition = new List<Vector3>();
        if (gameBarrageData.gameLevel <= 2)
        {
            gameBarrageData.listEjectorPosition.Add(new Vector3(0, 0, 0));
        }
        else if (gameBarrageData.gameLevel > 2 && gameBarrageData.gameLevel <= 4)
        {
            gameBarrageData.listEjectorPosition.Add(new Vector3(2, 0, 0));
            gameBarrageData.listEjectorPosition.Add(new Vector3(-2, 0, 0));
        }
        else if (gameBarrageData.gameLevel > 4)
        {
            gameBarrageData.listEjectorPosition.Add(new Vector3(0, 1, 0));
            gameBarrageData.listEjectorPosition.Add(new Vector3(2, -1, 0));
            gameBarrageData.listEjectorPosition.Add(new Vector3(-2, -1, 0));
        }
        //弹幕处理初始化
        barrageHandler.InitGame(gameBarrageData);
    }

    /// <summary>
    /// 初始化战斗游戏
    /// </summary>
    /// <param name="gameCombatData"></param>
    public void InitGameCombat(MiniGameCombatBean gameCombatData)
    {
        combatHandler.InitGame(gameCombatData);
    }

    #region 通知回调
    public void ObserbableUpdate<T>(T observable, int type, params object[] obj) where T : Object
    {
        switch (type)
        {
            case (int)BaseMiniGameHandler<BaseMiniGameBuilder, MiniGameBaseBean>.MiniGameStatusEnum.Gameing:
                break;
            case (int)BaseMiniGameHandler<BaseMiniGameBuilder, MiniGameBaseBean>.MiniGameStatusEnum.GameEnd:
                break;
            case (int)BaseMiniGameHandler<BaseMiniGameBuilder, MiniGameBaseBean>.MiniGameStatusEnum.GameClose:
                SceneUtil.SceneChange(GameCommonInfo.ScenesChangeData.beforeScene);
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