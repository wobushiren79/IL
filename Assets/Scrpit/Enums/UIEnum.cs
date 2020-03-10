using UnityEngine;
using UnityEditor;

public enum UIEnum
{
    MainStart = 1,
    MainContinue = 2,
    MainCreate = 3,
    MainMaker = 4,
    GameDate = 1001,
    GameMain = 1002,
    GameWorker = 1003,
    GameWorkerDetails = 1004,
    GameEquip = 1005,
    GameMenu = 1006,
    GameBackpack = 1007,
    GameBuild = 1008,
    GameSettle = 1009,
    GameAttendance = 1010,
    GameText = 1011,
    GameFavorability = 1012,//人物好感UI
    GameStatistics=1013,//统计UI

    TownMarket = 2001,//市场UI
    TownRecruitment = 2002,//招人UI
    TownGrocery = 2003,//杂货店UI
    TownDress = 2004,//服装店UI
    TownCarpenter = 2005,//工匠店UI
    TownGuildStore = 2006,//公会商店UI
    TownGuildAchievement = 2007,//公会成就UI
    TownGuildImprove = 2008,//公会晋升UI
    TownBank = 2009,//银行UI
    TownArena = 2010,//竞技场UI
    TownArenaStore = 2011,//竞技场商店UI
    TownPharmacy=2012,//药房

    MiniGameEnd = 3001,
    MiniGameCountDown = 3002,
    MiniGameBarrage = 3003,//弹幕游戏UI
    MiniGameCombat = 3004,//战斗游戏UI
    MiniGameCooking = 3005,//料理游戏UI
    MiniGameCookingSelect = 3006,//料理游戏 食物选择UI
    MiniGameCookingSettlement = 3007,//料理游戏 结算UI
    MiniGameAccount = 3008,//算账小游戏UI
    MiniGameDebate = 3009,//辩论小游戏UI
}