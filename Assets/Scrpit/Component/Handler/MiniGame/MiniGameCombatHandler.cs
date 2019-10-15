using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class MiniGameCombatHandler : BaseMiniGameHandler, UIMiniGameCountDown.ICallBack, UIMiniGameCombat.ICallBack
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

    /// <summary>
    /// 选中某一个角色
    /// </summary>
    /// <param name="gameCharacterData"></param>
    public NpcAIMiniGameCombatCpt SelectedCharacter(MiniGameCharacterBean gameCharacterData)
    {
        List<NpcAIMiniGameCombatCpt> listCharacter = gameCombatBuilder.GetAllCharacter();
        for (int i = 0; i < listCharacter.Count; i++)
        {
            NpcAIMiniGameCombatCpt itemNpc = listCharacter[i];
            if (itemNpc.characterMiniGameData == gameCharacterData)
            {
                itemNpc.SetSelected(true);
                return itemNpc;
            }
        }
        return null;
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
        //如果是敌方
        if (gameCharacterData.characterType == 0)
        {

        }
        //如果是友方
        else if (gameCharacterData.characterType == 1)
        {
            UIMiniGameCombat uiMiniGameCombat = (UIMiniGameCombat)uiGameManager.GetOpenUI();
            uiMiniGameCombat.OpenCommand();
        }
        //设置角色为选中状态
        NpcAIMiniGameCombatCpt npcCpt = SelectedCharacter(gameCharacterData);
        //聚焦回合角色
        ControlForMiniGameCombatCpt controlForMiniGameCombat = (ControlForMiniGameCombatCpt)controlHandler.GetControl();
        controlForMiniGameCombat.SetCameraPosition(npcCpt.transform.position);
        //UIMiniGameCombat uiMiniGameCombat = (UIMiniGameCombat)uiGameManager.GetOpenUI();
        //uiMiniGameCombat.StartNewRoundForCharacter(gameCharacterData);
        //uiMiniGameCombat.StartRound();
    }
    #endregion

}