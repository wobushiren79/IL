using UnityEngine;
using UnityEditor;

public class MiniGameAccountHandler : BaseMiniGameHandler<MiniGameAccountBuilder, MiniGameAccountBean>,
    ControlForMiniGameAccountCpt.ICallBack
{
    public override void InitGame(MiniGameAccountBean miniGameData)
    {
        base.InitGame(miniGameData);
        //创建玩家
        miniGameBuilder.CreateUserCharacter(miniGameData.listUserGameData, miniGameData.playerPosition);
        //摄像头初始化
        ControlForMiniGameAccountCpt gameControl = (ControlForMiniGameAccountCpt)controlHandler.StartControl(ControlHandler.ControlEnum.MiniGameAccount);
        gameControl.SetCameraOrthographicSize(8);
        gameControl.SetCameraPosition(miniGameData.cameraPosition);
        gameControl.SetCallBack(this);
        //打开倒计时UI
        OpenCountDownUI(miniGameData);
    }

    public override void StartGame()
    {
        base.StartGame();
        //发射器开始旋转
        MiniGameAccountEjectorCpt ejectorCpt = miniGameBuilder.GetEjector();
        ejectorCpt.StartRotate();
        //打开游戏UI
        UIMiniGameAccount uiMiniGameAccount= (UIMiniGameAccount)uiGameManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.MiniGameAccount));
    }


    #region 倒计时回调
    public override void GamePreCountDownEnd()
    {
        base.GamePreCountDownEnd();
        StartGame();
    }

    #endregion

    #region 操作监听
    public void OnClickLaunch()
    {
        if (GetMiniGameStatus() == MiniGameStatusEnum.Gameing)
        {
            MiniGameAccountEjectorCpt ejectorCpt = miniGameBuilder.GetEjector();
            ejectorCpt.Launch();
        }
    }
    #endregion
}