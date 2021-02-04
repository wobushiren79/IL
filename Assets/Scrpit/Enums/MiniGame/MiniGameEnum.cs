﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public enum MiniGameEnum
{
    Cooking = 1,//烹饪游戏
    Barrage = 2,//弹幕游戏
    Account = 3,//算账游戏
    Debate = 4,//辩论游戏 （斗魅）
    Combat = 5,//战斗游戏

    Birth = 101,//生孩子啪啪啪小游戏
}

public class MiniGameEnumTools
{
    public static MiniGameEnum GetRandomMiniGameTypeForArena()
    {
        List<MiniGameEnum> list = new List<MiniGameEnum>(){ MiniGameEnum.Cooking, MiniGameEnum.Barrage, MiniGameEnum.Account, MiniGameEnum.Debate, MiniGameEnum.Combat };
        return RandomUtil.GetRandomDataByList(list);
    }

    /// <summary>
    /// 获取基础游戏数据
    /// </summary>
    /// <param name="miniGameType"></param>
    /// <returns></returns>
    public static MiniGameBaseBean GetMiniGameData(MiniGameEnum miniGameType)
    {
        MiniGameBaseBean miniGameData = null;
        switch (miniGameType)
        {
            case MiniGameEnum.Cooking:
                miniGameData = new MiniGameCookingBean();
                break;
            case MiniGameEnum.Barrage:
                miniGameData = new MiniGameBarrageBean();
                break;
            case MiniGameEnum.Account:
                miniGameData = new MiniGameAccountBean();
                break;
            case MiniGameEnum.Debate:
                miniGameData = new MiniGameDebateBean();
                break;
            case MiniGameEnum.Combat:
                miniGameData = new MiniGameCombatBean();
                break;
            case MiniGameEnum.Birth:
                miniGameData = new MiniGameBirthBean();
                break;
        }
        return miniGameData;
    }

    /// <summary>
    /// 通过游戏类型获取职业
    /// </summary>
    /// <param name="miniGameType"></param>
    /// <returns></returns>
    public static WorkerEnum GetWorkerTypeByMiniGameType(MiniGameEnum miniGameType)
    {
        switch (miniGameType)
        {
            case MiniGameEnum.Cooking:
                return WorkerEnum.Chef;
            case MiniGameEnum.Barrage:
                return WorkerEnum.Waiter;
            case MiniGameEnum.Account:
                return WorkerEnum.Accountant;
            case MiniGameEnum.Debate:
                return WorkerEnum.Accost;
            case MiniGameEnum.Combat:
                return WorkerEnum.Beater;
        }
        return WorkerEnum.Chef;
    }
}