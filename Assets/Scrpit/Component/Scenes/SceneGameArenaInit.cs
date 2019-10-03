using UnityEngine;
using UnityEditor;

public class SceneGameArenaInit : BaseSceneInit
{
    //弹幕游戏控制
    public ArenaGameBarrageHandler barrageHandler;

    private void Start()
    {
        InitSceneData();
    }

    public void InitSceneData()
    { 
        ArenaPrepareBean arenaPrepareData = GameCommonInfo.ArenaPrepareData;
        arenaPrepareData = new ArenaPrepareBean();
        arenaPrepareData.gameType = ArenaGameEnum.Barrage;
        arenaPrepareData.gamePlayerNumber = 1;
        arenaPrepareData.gameLevel = 1;
        arenaPrepareData.winSurvivalTime = 60;
        arenaPrepareData.winLife = 1;
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