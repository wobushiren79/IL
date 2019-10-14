using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class MiniGameCombatHandler : BaseMiniGameHandler, UIMiniGameCountDown.ICallBack,UIMiniGameCombat.ICallBack
{
    //游戏数据
    public MiniGameCombatBean gameCombatData;
    //生成器
    public MiniGameCombatBuilder gameCombatBuilder;

    /// <summary>
    /// 初始化数据
    /// </summary>
    /// <param name="gameCombatData"></param>
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

    /// <summary>
    /// 开始游戏
    /// </summary>
    public void StartGame()
    {
      
    }

    #region 倒计时UI回调
    public override void GamePreCountDownStart()
    {
        base.GamePreCountDownStart();
    }

    public override void GamePreCountDownEnd()
    {
        base.GamePreCountDownEnd();
        //打开游戏UI
        UIMiniGameCombat uiMiniGameCombat = (UIMiniGameCombat)uiGameManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.MiniGameCombat));
        uiMiniGameCombat.SetCallBack(this);
        uiMiniGameCombat.SetData(gameCombatData);
        uiMiniGameCombat.StartRound();
        //开始游戏
        StartGame();
    }
    #endregion

    #region 游戏回调
    public void CharacterRoundCombat(MiniGameCharacterBean gameCharacterData)
    {
        //UIMiniGameCombat uiMiniGameCombat = (UIMiniGameCombat)uiGameManager.GetOpenUI();
        //uiMiniGameCombat.StartNewRoundForCharacter(gameCharacterData);
        //uiMiniGameCombat.StartRound();
    }
    #endregion

}