using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

[Serializable]
public class MiniGameBarrageBean : MiniGameBaseBean
{
    //发射间隔
    public float launchInterval;
    //发射器发射类型
    public BarrageEjectorCpt.LaunchTypeEnum[] launchTypes;
    //发射速度
    public float launchSpeed = 10;
    //发射台位置
    public List<Vector3> listEjectorPosition;
    //玩家数量
    public int playerNumber = 1;
    //当前时间
    public float currentTime;

    public MiniGameBarrageBean()
    {
        gameType = MiniGameEnum.Barrage;
    }

    public override void InitData(GameItemsManager gameItemsManager, List<CharacterBean> listUserData, List<CharacterBean> listEnemyData)
    {
        base.InitData(gameItemsManager, listUserData, listEnemyData);
        //初始化时间
        if (winSurvivalTime != 0)
        {
            currentTime = winSurvivalTime;
        }
    }
}