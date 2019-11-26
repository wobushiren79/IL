using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MiniGameDebateHandler : BaseMiniGameHandler<MiniGameDebateBuilder, MiniGameDebateBean>
{
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

    }

    #region 倒计时UI回调
    public override void GamePreCountDownStart()
    {
        base.GamePreCountDownStart();
    }

    public override void GamePreCountDownEnd()
    {
        base.GamePreCountDownEnd();
        //打开UI
        UIMiniGameDebate uiMiniGameDebate = (UIMiniGameDebate)uiGameManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.MiniGameDebate));
        uiMiniGameDebate.SetData((MiniGameCharacterForDebateBean)miniGameData.listUserGameData[0], (MiniGameCharacterForDebateBean)miniGameData.listEnemyGameData[0]);
        uiMiniGameDebate.DrawCard();
        //开始游戏
        StartGame();
    }
    #endregion
}