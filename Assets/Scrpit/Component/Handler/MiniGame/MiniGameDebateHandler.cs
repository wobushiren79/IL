using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MiniGameDebateHandler : BaseMiniGameHandler<MiniGameDebateBuilder, MiniGameDebateBean>,
    UIMiniGameDebate.ICallBack
{
    public GameItemsManager gameItemsManager;

    public override void InitGame(MiniGameDebateBean miniGameData)
    {
        base.InitGame(miniGameData);
        //创建角色
        miniGameBuilder.CreateAllCharacter(miniGameData.listUserGameData, miniGameData.listEnemyGameData, miniGameData.debatePosition);
        //设置摄像机位置
        controlHandler.StartControl(ControlHandler.ControlEnum.MiniGameDebate);
        controlHandler.GetControl().SetCameraPosition(miniGameData.debatePosition);

        //打开倒计时UI
        OpenCountDownUI(miniGameData);
    }

    public override void StartGame()
    {
        base.StartGame();
        //打开UI
        UIMiniGameDebate uiMiniGameDebate = (UIMiniGameDebate)uiGameManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.MiniGameDebate));
        uiMiniGameDebate.SetCallBack(this);
        uiMiniGameDebate.SetData((MiniGameCharacterForDebateBean)miniGameData.listUserGameData[0], (MiniGameCharacterForDebateBean)miniGameData.listEnemyGameData[0]);
        uiMiniGameDebate.DrawCard();
    }

    public override void EndGame(bool isWinGame)
    {
        base.EndGame(isWinGame);
    }

    /// <summary>
    /// 检测是否游戏结束
    /// </summary>
    /// <param name="isWinGame"></param>
    /// <returns></returns>
    public bool CheckIsGameOver(out bool isWinGame)
    {
        isWinGame = false;
        NpcAIMiniGameDebateCpt userCharacter = miniGameBuilder.GetUserCharacter();
        NpcAIMiniGameDebateCpt enemyCharacter = miniGameBuilder.GetEnemyCharacter();
        if (userCharacter.characterMiniGameData.characterCurrentLife <= 0 || enemyCharacter.characterMiniGameData.characterCurrentLife <= 0)
        {
            if (userCharacter.characterMiniGameData.characterCurrentLife <= 0)
            {
                isWinGame = false;
            }
            else if (enemyCharacter.characterMiniGameData.characterCurrentLife <= 0)
            {
                isWinGame = true;
            }
            return true;
        }
        return false;
    }

    #region 倒计时UI回调
    public override void GamePreCountDownStart()
    {
        base.GamePreCountDownStart();
    }

    public override void GamePreCountDownEnd()
    {
        base.GamePreCountDownEnd();
        //开始游戏
        StartGame();
    }
    #endregion

    #region UI操作回调
    public void DamageForCharacter(int characterType)
    {
        NpcAIMiniGameDebateCpt aiAttackerCharacter = null;
        NpcAIMiniGameDebateCpt aiDamageCharacter = null;
        if (characterType == 1)
        {
            aiAttackerCharacter = miniGameBuilder.GetEnemyCharacter();
            aiDamageCharacter = miniGameBuilder.GetUserCharacter();
        }
        else if (characterType == 2)
        {
            aiAttackerCharacter = miniGameBuilder.GetUserCharacter();
            aiDamageCharacter = miniGameBuilder.GetEnemyCharacter();
        }
        if (aiAttackerCharacter != null && aiDamageCharacter != null)
        {
            aiAttackerCharacter.characterData.GetAttributes(gameItemsManager, out CharacterAttributesBean characterAttributes);
            int damageNumber = characterAttributes.charm * 5;
            aiDamageCharacter.UnderAttack(damageNumber);
        }
        UIMiniGameDebate uiMiniGameDebate = (UIMiniGameDebate)uiGameManager.GetOpenUI();
        uiMiniGameDebate.RefreshUI();

        if (CheckIsGameOver(out bool isWinGame))
        {
            EndGame(isWinGame);
        }
    }
    #endregion
}