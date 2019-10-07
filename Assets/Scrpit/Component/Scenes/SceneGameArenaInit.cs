using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SceneGameArenaInit : BaseSceneInit, IBaseObserver
{
    //弹幕游戏控制
    public MiniGameBarrageHandler barrageHandler;

    private new void Start()
    {
        base.Start();
        InitSceneData();
        barrageHandler.AddObserver(this);
    }

    public void InitSceneData()
    {        
        //获取相关数据
        if (gameItemsManager != null)
            gameItemsManager.itemsInfoController.GetAllItemsInfo();
        //测试数据
        ArenaPrepareBean arenaPrepareData = GameCommonInfo.ArenaPrepareData;
        arenaPrepareData = new ArenaPrepareBean();
        arenaPrepareData.gameType = MiniGameEnum.Barrage;
        arenaPrepareData.gameBarrageData = new MiniGameBarrageBean();
        arenaPrepareData.gameBarrageData.gameLevel = 1;
        arenaPrepareData.gameBarrageData.launchInterval = 5;
        arenaPrepareData.gameBarrageData.launchSpeed = 1;
        arenaPrepareData.gameBarrageData.winSurvivalTime = 60;
        arenaPrepareData.gameBarrageData.winLife = 1;
        List<CharacterBean> listEm = new List<CharacterBean>();
        listEm.Add(new CharacterBean());
        arenaPrepareData.gameBarrageData.InitData(gameItemsManager, gameDataManager.gameData.userCharacter, listEm);

        arenaPrepareData.gameBarrageData.AddRewardItem(100001, 1);
        arenaPrepareData.gameBarrageData.AddRewardItem(100001, 2);
        arenaPrepareData.gameBarrageData.AddRewardItem(200001, 3);
        arenaPrepareData.gameBarrageData.AddRewardItem(1100006, 3);

        if (arenaPrepareData == null)
            return;
        switch (arenaPrepareData.gameType)
        {
            case MiniGameEnum.Cooking:
                break;
            case MiniGameEnum.Barrage:
                InitGameBarrage(arenaPrepareData.gameBarrageData);
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
        else if ( gameBarrageData.gameLevel > 4)
        {
            gameBarrageData.listEjectorPosition.Add(new Vector3(0, 1, 0));
            gameBarrageData.listEjectorPosition.Add(new Vector3(2, -1, 0));
            gameBarrageData.listEjectorPosition.Add(new Vector3(-2, -1, 0));
        }
        //弹幕处理初始化
        barrageHandler.InitGame(gameBarrageData);
    }

    #region 通知回调
    public void ObserbableUpdate<T>(T observable, int type, params object[] obj) where T : Object
    {
        switch (type)
        {
            case (int)BaseMiniGameHandler.NotifyMiniGameEnum.GameStart:
                break;
            case (int)BaseMiniGameHandler.NotifyMiniGameEnum.GameEnd:
                break;
            case (int)BaseMiniGameHandler.NotifyMiniGameEnum.GameClose:
                SceneUtil.SceneChange(GameCommonInfo.ScenesChangeData.beforeScene);
                break;
        }
    }
    #endregion
}