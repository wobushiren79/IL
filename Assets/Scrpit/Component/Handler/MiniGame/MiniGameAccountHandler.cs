using UnityEngine;
using UnityEditor;

public class MiniGameAccountHandler : BaseMiniGameHandler<MiniGameAccountBuilder, MiniGameAccountBean>
{
    public override void InitGame(MiniGameAccountBean miniGameData)
    {
        base.InitGame(miniGameData);
        //创建玩家
        miniGameBuilder.CreateUserCharacter(miniGameData.listUserGameData, miniGameData.playerPosition);
        //摄像头初始化
        BaseControl baseControl = controlHandler.StartControl(ControlHandler.ControlEnum.MiniGameAccount);
        baseControl.SetCameraOrthographicSize(8);
        baseControl.SetCameraPosition(miniGameData.cameraPosition);
    }
}