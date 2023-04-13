using UnityEditor;
using UnityEngine;

public static class EventsInfo
{
    #region 小游戏
    //游戏开始倒计时
    public static readonly string MiniGame_GamePreCountDownStart = "MiniGameCombat_GamePreCountDownStart";
    //游戏倒计时结束
    public static readonly string MiniGame_GamePreCountDownEnd = "MiniGameCombat_GamePreCountDownEnd";
    //点击退出
    public static readonly string MiniGame_EventForOnClickClose = "MiniGame_EventForOnClickClose";


    #endregion

    #region 斗武
    //斗武角色回合
    public static readonly string MiniGameCombat_EventForCharacterRound = "MiniGameCombat_EventForCharacterRound";
    //选择命令结束
    public static readonly string MiniGameCombat_EventForCommandEnd = "MiniGameCombat_EventForCommandEnd";
    #endregion

    #region 斗厨
    //菜单选择
    public static readonly string MiniGameCooking_MenuSelect = "MiniGameCooking_MenuSelect";
    //阶段结算
    public static readonly string MiniGameCooking_CookingSettle = "MiniGameCooking_CookingSettle";
    //结算界面关闭
    public static readonly string MiniGameCooking_CookingSettlementClose = "MiniGameCooking_CookingSettlementClose";
    #endregion
}