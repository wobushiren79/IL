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

        if (arenaPrepareData == null)
            return;
        switch (arenaPrepareData.gameType)
        {
            case ArenaGameEnum.Cooking:
                break;
            case ArenaGameEnum.Barrage:
                barrageHandler.InitGame(arenaPrepareData);
                barrageHandler.StartGame();
                break;

        }
    }
}