using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MiniGameCombatHandler : BaseMiniGameHandler, UIMiniGameCountDown.ICallBack
{
    //游戏数据
    public MiniGameCombatBean gameCombatData;
    //生成器
    public MiniGameCombatBuilder gameCombatBuilder;


    public void InitData(MiniGameCombatBean gameCombatData)
    {
        if (gameCombatData == null)
        {
            LogUtil.Log("战斗游戏数据为NULL，无法初始化战斗游戏");
            return;
        }
        this.gameCombatData = gameCombatData;
        //创建NPC
        gameCombatBuilder.CreateAllPlaer(gameCombatData.combatPosition, gameCombatData.listUserGameData, gameCombatData.listEnemyGameData);

        //设置摄像机位置
        controlHandler.StartControl(ControlHandler.ControlEnum.MiniGameCombat);
        controlHandler.GetControl().SetCameraPosition(gameCombatData.combatPosition);
        //打开倒计时UI
        OpenCountDownUI(gameCombatData);
    }

    #region 倒计时UI回调
    public override void GamePreCountDownStart()
    {
        base.GamePreCountDownStart();

    }

    public override void GamePreCountDownEnd()
    {
        base.GamePreCountDownEnd();

    }
    #endregion
}