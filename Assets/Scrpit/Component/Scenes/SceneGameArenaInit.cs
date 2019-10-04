﻿using UnityEngine;
using UnityEditor;

public class SceneGameArenaInit : BaseSceneInit
{
    //弹幕游戏控制
    public ArenaGameBarrageHandler barrageHandler;

    private new void Start()
    {
        base.Start();
        InitSceneData();
    }

    public void InitSceneData()
    { 
        ArenaPrepareBean arenaPrepareData = GameCommonInfo.ArenaPrepareData;
        arenaPrepareData = new ArenaPrepareBean();
        arenaPrepareData.gameType = ArenaGameEnum.Barrage;
        arenaPrepareData.gameBarrageData = new MiniGameBarrageBean();
        arenaPrepareData.gameBarrageData.gameLevel = 1;
        arenaPrepareData.gameBarrageData.winSurvivalTime = 60;
        arenaPrepareData.gameBarrageData.winLife = 1;
        arenaPrepareData.gameBarrageData.InitData(gameItemsManager, gameDataManager.gameData.userCharacter);

        if (arenaPrepareData == null)
            return;
        switch (arenaPrepareData.gameType)
        {
            case ArenaGameEnum.Cooking:
                break;
            case ArenaGameEnum.Barrage:
                GameBarrage(arenaPrepareData);
                break;
        }
    }

    public void GameBarrage(ArenaPrepareBean arenaPrepareData)
    {
        barrageHandler.InitGame(arenaPrepareData);
    }
}