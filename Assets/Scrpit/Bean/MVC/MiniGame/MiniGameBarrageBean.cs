using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

[Serializable]
public class MiniGameBarrageBean : MiniGameBaseBean
{
    //发射台个数
    public int launchNumber;
    //发射间隔
    public float launchInterval;
    //发射器发射类型
    public MiniGameBarrageEjectorCpt.LaunchTypeEnum[] launchTypes;
    //发射速度
    public float launchSpeed = 10;
    //子弹类型
    public  MiniGameBarrageBulletTypeEnum bulletType = MiniGameBarrageBulletTypeEnum.Stone;
    //用户起始位置
    public Vector2 userStartPosition;
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

    public override void InitForMiniGame(GameItemsManager gameItemsManager)
    {
        //初始化时间
        if (winSurvivalTime != 0)
        {
            currentTime = winSurvivalTime;
        }
    }
}